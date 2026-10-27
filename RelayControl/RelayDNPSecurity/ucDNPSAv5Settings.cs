using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RelayControlLibrary;
using SharedResources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RelayDNPSecurity
{
    public partial class ucDNPSAv5Settings : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5Settings()
        {
            InitializeComponent();
            this.initializeSecurityStatistics();
            
            // Set Font of only the groupBoxMain Title in bold. keep rest of items in side in regular ( non bold ) font
            groupBoxMain.Font = new Font(groupBoxMain.Font, FontStyle.Bold);
            foreach (Control ctrl in groupBoxMain.Controls)
            {
                ctrl.Font = new Font(
                    groupBoxMain.Font.FontFamily,
                    groupBoxMain.Font.Size,
                    FontStyle.Regular
                );
            }

            // Set Font of only the groupBoxSecurityStats Title in bold. keep rest of items in side in regular ( non bold ) font
            groupBoxSecurityStats.Font = new Font(groupBoxSecurityStats.Font, FontStyle.Bold);
            foreach (Control ctrl in groupBoxSecurityStats.Controls)
            {
                ctrl.Font = new Font(
                    groupBoxSecurityStats.Font.FontFamily,
                    groupBoxSecurityStats.Font.Size,
                    FontStyle.Regular
                );
            }

        }

        public List<DNPSAv5SecurityStatisticItem> statisticPoints = new List<DNPSAv5SecurityStatisticItem>();
        private static int _packetLength = 98;
        private int _loadedKeyChangeAlgorithmIndex = -1;
        private int _loadedMacAlgorithmIndex = -1;
        private bool _loadedAuthenticationEnabled = false;
        private bool _loadedBaselineValid = false;


        public void initializeSecurityStatistics()
        {
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Unexpected Messages", 3m));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Authorization Failures", 5m));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Authentication Failures", 5));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Reply Timeouts", 3));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Rekeys Due To Failure", 3));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Total Messages Sent", 100));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Total Messages Recevied", 100));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Critical Messages Received", 100));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Critical Messages Sent", 100));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Discarded Messages", 10));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Error Messages Sent", 2));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Error Messaged Received", 10));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Successful Authentications", 100));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Session Key Changes", 10));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Failed Session Key Changes", 5));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Update Key Changes", 1));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Failed Update Key Changes", 1));
            //this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Special Statistic", 

            Point workingPoint = new Point(5, 15);
            
            foreach (DNPSAv5SecurityStatisticItem sI in this.statisticPoints)
            {
                ucDNPSAv5SecurityStatisticThreshold workingStatistic = new ucDNPSAv5SecurityStatisticThreshold(sI.StatisticsName, sI.DefaultValue);
                workingStatistic.Location = workingPoint;
                this.groupBoxSecurityStats.Controls.Add(workingStatistic);

                workingPoint = new Point(workingPoint.X, workingPoint.Y + workingStatistic.Height);
                //if (workingPoint.Y > this.groupBoxSecurityStats.Height - workingStatistic.Height)
                if (workingPoint.Y > 360 - workingStatistic.Height) //if (workingPoint.Y > 320 - workingStatistic.Height)
                    workingPoint = new Point(workingPoint.X + workingStatistic.Width, 15);
            }

            this.buttonDefault.Location = new System.Drawing.Point(25, 430);
            this.buttonSendSettings.Location = new System.Drawing.Point(200, 430);
            this.buttonRequestSettings.Location = new System.Drawing.Point(125, 470);

            this.comboBoxMACAlogrithm.Location = new System.Drawing.Point(160, 380);
            this.labelMACAlgorithm.Location = new System.Drawing.Point(65, 382);
            this.comboBoxKeyChangeAlogrithm.Location = new System.Drawing.Point(160, 335);
            this.labelKeyChangeAlgorithm.Location = new System.Drawing.Point(31, 337);
            this.numericUpDownMaxSessionKeyCount.Location = new System.Drawing.Point(160, 290);
            this.labelMaxSessionKeyCount.Location = new System.Drawing.Point(28, 292);
            this.numericUpDownSessionKeyChangeCount.Location = new System.Drawing.Point(160, 245);
            this.labelSessionKeyChangeCount.Location = new System.Drawing.Point(12, 246);
            this.numericUpDownSessionKeyInterval.Location = new System.Drawing.Point(160, 200);
            this.labelSessionKeyInterval.Location = new System.Drawing.Point(27, 202);
            this.numericUpDownReplyTimeout.Location = new System.Drawing.Point(160, 155);
            this.labelReplyTimeout.Location = new System.Drawing.Point(53, 157);

            this.checkBoxAuthenticationEnabled.Location = new System.Drawing.Point(19, 110);
            this.checkBoxSHA1.Location = new System.Drawing.Point(19, 65);
            this.checkBoxAggressiveMode.Location = new System.Drawing.Point(19, 20);

           
            this.panel_DNPSAv5set.Location = new System.Drawing.Point(337, 18);
            this.panel_DNPSAv5set.Size = new System.Drawing.Size(605, 492);

        }

        public bool AuthenticationEnabled
        {
            get
            {
                if (checkBoxAuthenticationEnabled.Checked)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    checkBoxAuthenticationEnabled.Checked = true;
                else
                    checkBoxAuthenticationEnabled.Checked = false;
            }
        }

        //private void buttonSendSettings_Click(object sender, EventArgs e)
        public void buttonSendSettings_Click(object sender, EventArgs e)
        {
            //=====================Display a throbber and disbale everything on the screen=====================
            SetBusyUi(true, "Sending SAv5 Settings");
            //this.enableAll(false);
            //========================================================================================================

            this.sendSettings();
        }

        //private void sendSettings()
        public void sendSettings()
        {
            SendEventArgs sSEA = new SendEventArgs(_packetLength);
            UInt16 tempInt;
            byte tempByte;
            sSEA.WithAck = true;

            var host = this.Parent as System.Windows.Forms.Control;
            while (host != null && !(host is ucDNPSAv5))
            {
                host = host.Parent;
            }

            var parentSAv5 = host as ucDNPSAv5;

            // Recovery path:
            // If relay readback baseline is invalid (all-00/all-FF/etc), allow one default apply
            // to reseed sane SAv5 settings.
            if (!_loadedBaselineValid)
            {
                this.setDefaults();
                this.checkBoxAuthenticationEnabled.Checked = false; // avoid key-validation block on first recovery write
            }

            bool authEnabled = this.checkBoxAuthenticationEnabled.Checked;

            // Only validate authority key when auth is actually being enabled
            if (authEnabled && parentSAv5 != null && this.RequiresAuthorityKeyValidationForApply())
            {
                if (!parentSAv5.ValidateAuthorityKeyForSelectedAlgorithm(out string validationError))
                {
                    this.onError(new Exception(validationError), "Error Sending SAv5 Settings");
                    SetBusyUi(false, "Sending SAv5 Settings");
                    return;
                }
            }

            try
            {
                sSEA.SendPacket[0] = ProjectConstants._DNPControlOpCode;
                sSEA.SendPacket[1] = (byte)'S'; // For Settings
                sSEA.SendPacket[2] = 0;         // Reserved

                if (this.checkBoxAggressiveMode.Checked)
                    sSEA.SendPacket[3] = 0x01;
                else
                    sSEA.SendPacket[3] = 0x00;

                if (this.checkBoxSHA1.Checked)
                    sSEA.SendPacket[3] |= 0x02;

                if (authEnabled)
                    sSEA.SendPacket[3] |= 0x04;

                // Guard invalid/unselected combo values
                int keyIdx = this.comboBoxKeyChangeAlogrithm.SelectedIndex;
                if (keyIdx < 0) keyIdx = 1; // sane recovery default
                if (keyIdx > 7) keyIdx = 7; // 3-bit field

                tempByte = (byte)keyIdx;
                tempByte <<= 3; // bits 3..5
                sSEA.SendPacket[3] |= tempByte;

                sSEA.SendPacket[4] = 0;

                tempInt = (UInt16)(this.numericUpDownReplyTimeout.Value * 10m);
                sSEA.SendPacket[6] = (byte)(tempInt >> 8);
                sSEA.SendPacket[5] = (byte)tempInt;

                tempInt = (UInt16)this.numericUpDownSessionKeyInterval.Value;
                sSEA.SendPacket[8] = (byte)(tempInt >> 8);
                sSEA.SendPacket[7] = (byte)tempInt;

                tempInt = (UInt16)this.numericUpDownSessionKeyChangeCount.Value;
                sSEA.SendPacket[10] = (byte)(tempInt >> 8);
                sSEA.SendPacket[9] = (byte)tempInt;

                tempInt = (UInt16)this.numericUpDownMaxSessionKeyCount.Value;
                sSEA.SendPacket[11] = (byte)tempInt;

                int macIdx = this.comboBoxMACAlogrithm.SelectedIndex;
                if (macIdx < 0) macIdx = 2; // sane recovery default
                if (macIdx > 255) macIdx = 255;
                sSEA.SendPacket[12] = (byte)macIdx;

                try
                {
                    int i = 13;
                    foreach (ucDNPSAv5SecurityStatisticThreshold sT in this.groupBoxSecurityStats.Controls)
                    {
                        byte[] tempBytes = sT.GetBytes();
                        sSEA.SendPacket[i++] = tempBytes[0];
                        sSEA.SendPacket[i++] = tempBytes[1];
                    }
                }
                catch (Exception ex)
                {
                    this.onError(new Exception("Error getting Security Statistics Values: " + ex), "Error Sending SAv5 Settings");
                    SetBusyUi(false, "Sending SAv5 Settings");
                    return;
                }

                sSEA.SendPacket[sSEA.SendPacket.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Sending SAv5 Settings: " + ex), "Error sending SAv5 Settings");
            }
        }

        //private void numericUpDownReplyTimeout_ValueChanged(object sender, EventArgs e)
        private void numericUpDownReplyTimeout_TextChanged(object sender, EventArgs e)
        {
            this.numericUpDownReplyTimeout.Value = numericUpDownReplyTimeout.Value;
        }

        //private void buttonRequestSettings_Click(object sender, EventArgs e)
        public void buttonRequestSettings_Click(object sender, EventArgs e)
        {
            //=====================Display throbber while parameters get requested from the master relay  =====================
            SetBusyUi(true, "Requesting SAv5 Settings");
            //this.enableAll(false);
            //========================================================================================================

            this.RequestSettings();
        }

        public void RequestSettings()
        {
            SendEventArgs sSEA = new SendEventArgs(_packetLength);
            try
            {
                sSEA.SendPacket[0] = ProjectConstants._DNPControlOpCode;
                sSEA.SendPacket[1] = (byte)'s'; // For requesting settings

                sSEA.SendPacket[sSEA.SendPacket.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Requesting SAv5 Settings: " + ex.ToString()), "Error Requesting SAv5 Settings");
            }
        }

        //private void buttonDefault_Click(object sender, EventArgs e)
        public void buttonDefault_Click(object sender, EventArgs e)
        {
            // Populate the screen only. Nothing is transmitted here.
            this.setDefaults();

            // Defaults are an operator-selected configuration,
            // not a verified readback from the relay.
            _loadedBaselineValid = false;

            // Allow the operator to explicitly Apply the defaults.
            this.buttonSendSettings.Enabled =
                this.comboBoxKeyChangeAlogrithm.SelectedIndex >= 0 &&
                this.comboBoxMACAlogrithm.SelectedIndex >= 0;
        }

        //private void setDefaults()
        public void setDefaults()
        {
            this.checkBoxAggressiveMode.Checked = true;
            this.checkBoxSHA1.Checked = false;

            this.checkBoxAuthenticationEnabled.Checked = false;

            this.comboBoxKeyChangeAlogrithm.SelectedIndex = 1;
            this.comboBoxMACAlogrithm.SelectedIndex = 2;

            this.numericUpDownReplyTimeout.Value = 2.0m;
            this.numericUpDownSessionKeyInterval.Value = 1800m;
            this.numericUpDownSessionKeyChangeCount.Value = 4000m;
            this.numericUpDownMaxSessionKeyCount.Value = 5m;

            foreach (ucDNPSAv5SecurityStatisticThreshold sT in this.groupBoxSecurityStats.Controls)
            {
                sT.SetDefault();
            }
        }
        private void SetBusyUi(bool busy, string statusText = null)
        {
            if (this.IsDisposed) return;

            if (this.InvokeRequired)
            {
                BeginInvoke((Action)(() => SetBusyUi(busy, statusText)));
                return;
            }

            var caller = new System.Diagnostics.StackTrace()
                .GetFrame(1)?.GetMethod()?.Name ?? "unknown";

            var msg = $"SetBusyUi busy={busy} statusText={statusText ?? "<null>"} caller={caller}";
            System.Diagnostics.Trace.WriteLine(msg);

            // if available in this control:
            // screenD.screenDisable = busy;

            Application.UseWaitCursor = busy;
            Cursor.Current = busy ? Cursors.WaitCursor : Cursors.Default;
            this.UseWaitCursor = busy;

            this.Refresh();
        }

        internal void SetAll(byte[] bytePacket)
        {
            _loadedBaselineValid = false;
            _loadedKeyChangeAlgorithmIndex = -1;
            _loadedMacAlgorithmIndex = -1;
            this.buttonSendSettings.Enabled = false;

            string packetDiagnostic = "Packet unavailable.";

            try
            {
                const int thresholdCount = 17;
                const int thresholdStart = 11;
                const int minimumPacketLength =
                    thresholdStart + (thresholdCount * 2); // 45

                if (bytePacket != null)
                {
                    int diagnosticLength =
                        Math.Min(bytePacket.Length, minimumPacketLength);

                    string settingsHex = diagnosticLength > 0
                        ? BitConverter.ToString(bytePacket, 0, diagnosticLength)
                        : "<empty>";

                    packetDiagnostic =
                        "PacketLength=" + bytePacket.Length +
                        ", SettingsHex=" + settingsHex;
                }

                if (bytePacket == null ||
                    bytePacket.Length < minimumPacketLength)
                {
                    throw new Exception(
                        "SAv5 settings packet is too short. Required at least " +
                        minimumPacketLength + " bytes.");
                }

                if (bytePacket[0] != (byte)'S')
                {
                    throw new Exception(
                        "Unexpected settings opcode: 0x" +
                        bytePacket[0].ToString("X2") + ".");
                }

                var thresholds = this.groupBoxSecurityStats.Controls
                    .OfType<ucDNPSAv5SecurityStatisticThreshold>()
                    .ToArray();

                if (thresholds.Length != thresholdCount)
                {
                    throw new Exception(
                        "Expected " + thresholdCount +
                        " security threshold controls; found " +
                        thresholds.Length + ".");
                }

                // Check the settings image only, excluding opcode and padding.
                bool allZero = true;
                bool allFF = true;

                for (int index = 1; index < minimumPacketLength; index++)
                {
                    if (bytePacket[index] != 0x00)
                        allZero = false;

                    if (bytePacket[index] != 0xFF)
                        allFF = false;
                }

                if (allZero || allFF)
                {
                    // Populate the screen only. Nothing is sent here.
                    // These defaults include Authentication Enabled = false.
                    this.setDefaults();

                    // Keep the baseline invalid until valid relay readback.
                    _loadedBaselineValid = false;
                    _loadedKeyChangeAlgorithmIndex = -1;
                    _loadedMacAlgorithmIndex = -1;

                    dataBackupSAV5.dataBackup_sav5Defaults = true;

                    this.buttonSendSettings.Enabled =
                        this.comboBoxKeyChangeAlogrithm.SelectedIndex >= 0 &&
                        this.comboBoxMACAlogrithm.SelectedIndex >= 0;

                    System.Diagnostics.Trace.WriteLine(
                        "[DNPSAv5] Blank settings readback (" +
                        (allZero ? "all 00" : "all FF") +
                        "). Defaults displayed locally; Apply available. " +
                        "No settings transmitted. " + packetDiagnostic);

                    return;
                }

                // Decode using the existing packet offsets.
                byte flags = bytePacket[1];
                int keyIndex = (flags & 0x38) >> 3;
                int macIndex = bytePacket[10];

                int rawReplyTimeout =
                    bytePacket[3] | (bytePacket[4] << 8);

                decimal replyTimeout = rawReplyTimeout / 10m;

                decimal sessionKeyInterval =
                    bytePacket[5] | (bytePacket[6] << 8);

                decimal sessionKeyChangeCount =
                    bytePacket[7] | (bytePacket[8] << 8);

                decimal maxSessionKeyCount = bytePacket[9];

                bool keyKnown =
                    keyIndex >= 0 &&
                    keyIndex < this.comboBoxKeyChangeAlogrithm.Items.Count;

                bool macKnown =
                    macIndex >= 0 &&
                    macIndex < this.comboBoxMACAlogrithm.Items.Count;

                packetDiagnostic +=
                    "; Flags=0x" + flags.ToString("X2") +
                    ", Reserved=0x" + bytePacket[2].ToString("X2") +
                    ", KeyIndex=" + keyIndex +
                    ", MACIndex=" + macIndex +
                    ", RawReplyTimeout=0x" + rawReplyTimeout.ToString("X4") +
                    ", ReplyTimeoutSeconds=" + replyTimeout +
                    ", SessionKeyInterval=" + sessionKeyInterval +
                    ", SessionKeyChangeCount=" + sessionKeyChangeCount +
                    ", MaxSessionKeyCount=" + maxSessionKeyCount;

                var problems = new List<string>();

                Action<NumericUpDown, decimal, string> validateRange =
                    (control, value, fieldName) =>
                    {
                        if (value < control.Minimum ||
                            value > control.Maximum)
                        {
                            problems.Add(
                                fieldName + " from relay out of range: " +
                                value + ". Allowed " +
                                control.Minimum + ".." + control.Maximum);
                        }
                    };

                validateRange(
                    this.numericUpDownReplyTimeout,
                    replyTimeout,
                    "Reply Timeout");

                validateRange(
                    this.numericUpDownSessionKeyInterval,
                    sessionKeyInterval,
                    "Session Key Interval");

                validateRange(
                    this.numericUpDownSessionKeyChangeCount,
                    sessionKeyChangeCount,
                    "Session Key Change Count");

                validateRange(
                    this.numericUpDownMaxSessionKeyCount,
                    maxSessionKeyCount,
                    "Max Session Key Count");

                if (!keyKnown)
                {
                    problems.Add(
                        "Unsupported Key Change Algorithm index: " +
                        keyIndex + ". Dropdown has " +
                        this.comboBoxKeyChangeAlogrithm.Items.Count +
                        " choices.");
                }

                if (!macKnown)
                {
                    problems.Add(
                        "Unsupported MAC Algorithm index: " +
                        macIndex + ". Dropdown has " +
                        this.comboBoxMACAlogrithm.Items.Count +
                        " choices.");
                }

                if (problems.Count > 0)
                    throw new Exception(string.Join("; ", problems));

                this.checkBoxAggressiveMode.Checked =
                    (flags & 0x01) != 0;

                this.checkBoxSHA1.Checked =
                    (flags & 0x02) != 0;

                this.checkBoxAuthenticationEnabled.Checked =
                    (flags & 0x04) != 0;

                this.comboBoxKeyChangeAlogrithm.SelectedIndex = keyIndex;
                this.comboBoxMACAlogrithm.SelectedIndex = macIndex;

                this.numericUpDownReplyTimeout.Value = replyTimeout;
                this.numericUpDownSessionKeyInterval.Value = sessionKeyInterval;
                this.numericUpDownSessionKeyChangeCount.Value =
                    sessionKeyChangeCount;
                this.numericUpDownMaxSessionKeyCount.Value = maxSessionKeyCount;

                int offset = thresholdStart;

                foreach (var threshold in thresholds)
                {
                    threshold.SetBytes(new byte[]
                    {
                bytePacket[offset],
                bytePacket[offset + 1]
                    });

                    offset += 2;
                }

                // Establish the baseline only after the complete load succeeds.
                _loadedKeyChangeAlgorithmIndex = keyIndex;
                _loadedMacAlgorithmIndex = macIndex;
                _loadedAuthenticationEnabled =
                    this.checkBoxAuthenticationEnabled.Checked;

                _loadedBaselineValid = true;
                this.buttonSendSettings.Enabled = true;

                System.Diagnostics.Trace.WriteLine(
                    "[DNPSAv5] Settings readback loaded. " + packetDiagnostic);
            }
            catch (Exception ex)
            {
                _loadedBaselineValid = false;
                this.buttonSendSettings.Enabled = false;

                this.comboBoxKeyChangeAlogrithm.SelectedIndex = -1;
                this.comboBoxMACAlogrithm.SelectedIndex = -1;

                dataBackupSAV5.dataBackup_sav5Defaults = true;

                this.onError(
                    new Exception(
                        "SAv5 settings readback rejected; Apply disabled. " +
                        ex.Message + " | " + packetDiagnostic,
                        ex),
                    "Error Setting SAv5 Settings");
            }
            finally
            {
                SetBusyUi(false);
            }
        }
        private int CoerceIndex(System.Windows.Forms.ComboBox cb, int idx)
        {
            if (cb == null || idx < 0 || idx >= cb.Items.Count)
                return -1;

            return idx;
        }

        private decimal ClampToNumericRange(NumericUpDown n, decimal value)
        {
            if (value < n.Minimum) return n.Minimum;
            if (value > n.Maximum) return n.Maximum;
            return value;
        }
        private bool TrySetSelectedIndex(System.Windows.Forms.ComboBox cb, int idx, string fieldName)
        {
            int coerced = CoerceIndex(cb, idx);
            if (coerced < 0)
            {
                this.onError(new Exception($"Invalid {fieldName} index from relay: {idx}"), $"Error Setting {fieldName}");
                return false;
            }

            cb.SelectedIndex = coerced;
            return true;
        }

        public int GetKeyChangeAlgorithmIndexForDebug()
        {
            return this.comboBoxKeyChangeAlogrithm?.SelectedIndex ?? -1;
        }

        public int GetMacAlgorithmIndexForDebug()
        {
            return this.comboBoxMACAlogrithm?.SelectedIndex ?? -1;
        }

        public bool RequiresAuthorityKeyValidationForApply()
        {
            if (!_loadedBaselineValid) return true; // safe fallback

            bool keyAlgChanged = this.comboBoxKeyChangeAlogrithm.SelectedIndex != _loadedKeyChangeAlgorithmIndex;
            bool macChanged = this.comboBoxMACAlogrithm.SelectedIndex != _loadedMacAlgorithmIndex;
            bool authChanged = this.checkBoxAuthenticationEnabled.Checked != _loadedAuthenticationEnabled;

            return keyAlgChanged || macChanged || authChanged;
        }
    }
}
