using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SharedResources;

namespace RelayControlLibrary
{
    public partial class ucDNP : UserControl
    {
        private static int _ucDNPInstanceCounter = 0;
    
        public ucDNP()
        {
            InitializeComponent();
            this.numericUpDownFragmentSize.ValueChanged += new System.EventHandler(this.numericUpDownFragmentSize_ValueChanged);
            
        }
        public delegate void SendEventHandler(SendEventArgs sEA);
        public event SendEventHandler Send;
        public delegate void ExceptionHandler(object o, ExceptionEventArgs eEA);
        public event ExceptionHandler DNPControlException;
        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                if (this.customer != value)
                {
                    this.customer = value;
                    this.customerChanged = true;
                }
                this.setCustomer();
            }
        }
        private Customers customer = Customers.None;
        private List<ucDeadBandSettingsObject> deadBandVariables = new List<ucDeadBandSettingsObject>();

        private static int _packetLength = 98;

        #pragma warning disable CS0414 // field assigned but never used in Debug 
        private string dNPErrorMsg = "Please Verify all settings for DNP Tabs";
        #pragma warning restore CS0414 // field assigned but never used

        private bool customerChanged = false;
        #region Send Functions

        //private void buttonSendAllDNPSettings_Click(object sender, EventArgs e)
        public void buttonSendAllDNPSettings_Click(object sender, EventArgs e)
        {
#if DNP
            applyDNP.applyDNPSettings = true;

            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                this.SendAllDNPSettings();
            }
            finally
            {
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
            }
#endif
        }

        private decimal SnapFragmentSize(decimal value)
        {
            decimal[] allowed = { 256, 512, 1024, 2048, 4096 };

            decimal closest = allowed[0];
            decimal smallestDiff = Math.Abs(value - allowed[0]);

            for (int i = 1; i < allowed.Length; i++)
            {
                decimal diff = Math.Abs(value - allowed[i]);
                if (diff < smallestDiff)
                {
                    smallestDiff = diff;
                    closest = allowed[i];
                }
            }

            return closest;
        }

        private void numericUpDownFragmentSize_ValueChanged(object sender, EventArgs e)
        {
            decimal snappedValue = SnapFragmentSize(this.numericUpDownFragmentSize.Value);

            if (this.numericUpDownFragmentSize.Value != snappedValue)
            {
                this.numericUpDownFragmentSize.Value = snappedValue;
            }
        }
        public void SendAllDNPSettings()
        {
            // MessageBox.Show("send dnp settings to master processor"); // Only for testing - to be removed
            try
            {
                SendEventArgs sEA = new SendEventArgs(_packetLength);
                byte tempByte = 0;
                UInt32 tempInt32;
                //    MessageBox.Show("sending command D + a to uP for all dnp settings"); // Only for testing - to be removed
                sEA.SendPacket[0] = (byte)RelayModeFunctions._DNPControlOpCode; //"D"
                sEA.SendPacket[1] = (byte)'a';        //For set all

                //Setting the command bits 0 - 6
                if ((string)this.comboBoxLinkLayerConfirm.SelectedItem == "Always")
                    tempByte = 2;
                else if ((string)this.comboBoxLinkLayerConfirm.SelectedItem == "Sometimes")
                    tempByte = 1;
                else if ((string)this.comboBoxLinkLayerConfirm.SelectedItem == "Never")
                    tempByte = 0;
                else
                    throw new Exception("Error Getting Value For Link Layer: " + this.comboBoxLinkLayerConfirm.SelectedItem.ToString());

                if ((string)this.comboBoxSelfAddress.SelectedItem == "Enable")
                    tempByte |= 4;
                else if ((string)this.comboBoxSelfAddress.SelectedItem != "Disable")
                    throw new Exception("Error Getting Value For Self Address: " + this.comboBoxSelfAddress.SelectedItem.ToString());

                if ((string)this.comboBoxUnsolResponse.SelectedItem == "Enable")
                    tempByte |= 8;
                else if ((string)this.comboBoxUnsolResponse.SelectedItem != "Disable")
                    throw new Exception("Error Getting Value For Unsolicited Response: " + this.comboBoxUnsolResponse.SelectedItem.ToString());

                if ((string)this.comboBoxTerminationResistor.SelectedItem == "Enable")
                    tempByte |= 16;
                else if ((string)this.comboBoxTerminationResistor.SelectedItem != "Disable")
                    throw new Exception("Error Getting Value For Termination Resistor: " + this.comboBoxTerminationResistor.SelectedItem.ToString());


                sEA.SendPacket[3] = tempByte;

                //Unsolicited Timeout
                tempInt32 = (UInt32)this.numericUpDownUnsolTimeout.Value;

                sEA.SendPacket[7] = (byte)tempInt32;
                sEA.SendPacket[8] = (byte)(tempInt32 >> 8);
                sEA.SendPacket[5] = (byte)(tempInt32 >> 16);
                sEA.SendPacket[6] = (byte)(tempInt32 >> 24);

                //Fragment Size
                tempInt32 = (UInt32)this.numericUpDownFragmentSize.Value;
                sEA.SendPacket[9] = (byte)tempInt32;
                sEA.SendPacket[10] = (byte)(tempInt32 >> 8);

                //Destination Address
                tempInt32 = (UInt32)this.numericUpDownDestinationAddress.Value;
                sEA.SendPacket[11] = (byte)tempInt32;
                sEA.SendPacket[12] = (byte)(tempInt32 >> 8);

                //Source Address
                tempInt32 = (UInt32)this.numericUpDownSourceAddress.Value;
                sEA.SendPacket[13] = (byte)tempInt32;
                sEA.SendPacket[14] = (byte)(tempInt32 >> 8);

                //Unsolicited Max Retries
                tempInt32 = (UInt32)this.numericUpDownUnsolRetries.Value;
                sEA.SendPacket[15] = (byte)tempInt32;
                sEA.SendPacket[16] = (byte)(tempInt32 >> 8);

                //Max Events
                tempByte = (byte)this.numericUpDownMaxEvents.Value;
                sEA.SendPacket[17] = tempByte;

                // Baude Rate
                sEA.SendPacket[18] = (byte)this.comboBoxDNPBaudRate.SelectedIndex;

                sEA.SendPacket[sEA.SendPacket.Length - 1] = 0x0D;

                //uplinkC.uplinkCount += 1;

                this.Send(sEA);
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }
        }

        private void buttonSendDeadBand_Click(object sender, EventArgs e)
        {

            try
            {
                SendEventArgs sEA = new SendEventArgs(_packetLength); //98
                int index = 2;
            //    MessageBox.Show("2 sending command D to master"); // Only for testing - to be removed
                sEA.SendPacket[0] = (byte)RelayModeFunctions._DNPControlOpCode; // "D"
                sEA.SendPacket[1] = (byte)'d';        //For set deadband limits

                foreach (Control c in this.groupBoxDIGITALGRIDDNPDeadBand.Controls)
                {
                    bool failed = false;
                    ucDNPDeadBand uDDB = new ucDNPDeadBand();
                    try
                    {
                        uDDB = (ucDNPDeadBand)c;
                    }
                    catch //if it is not a ucDeadBand box
                    {
                        failed = true;
                    }

                    if (!failed)
                    {
                        sEA.SendPacket[index] = (byte)uDDB.Value;
                       // sEA.SendPacket[index] = (byte)this.deadBandVariables[index-2].Maximum; // only for testing -3/30/2026- to be removed
                        index++;
                        if (index >= 90)
                            throw new Exception("Too many DeadBand Variables for single packet");
                    }
                }

                sEA.SendPacket[sEA.SendPacket.Length - 1] = 0x0D;

                
                this.Send(sEA);
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }
        }

        private void buttonRQDNPSettings_Click(object sender, EventArgs e)
        {
            //=====================Display throbber while parameters get requested from the master relay  =====================
            Application.UseWaitCursor = true; //keeps waitcursor even when the thread ends.
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor; //Normal mode of setting waitcursor
            //this.enableAll(false);
            //========================================================================================================

            try
            {
                SendEventArgs sEA = new SendEventArgs(3);

                sEA.SendPacket[0] = (byte)RelayModeFunctions._DNPDataRequestOpCode;
                sEA.SendPacket[1] = (byte)'U';
                sEA.SendPacket[2] = 0x0D;

                this.Send(sEA);
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }
        }

        private bool TryExtractDnpSettingsPayload(byte[] packet, out byte[] payload, out string reason)
        {
            payload = null;
            reason = null;

            if (packet == null || packet.Length == 0)
            {
                reason = "null/empty packet";
                return false;
            }

            bool hasTerminator = packet.Length >= 3 &&
                                 (packet[packet.Length - 1] == 0x0D || packet[packet.Length - 1] == 0x0A);

            if (hasTerminator && packet[0] == (byte)'D')
            {
                int declaredLen = packet[1];
                int availablePayload = packet.Length - 3; // cmd + len + terminator removed

                if (declaredLen <= 0 || declaredLen > availablePayload)
                {
                    reason = $"bad declared length={declaredLen}, available={availablePayload}";
                    return false;
                }

                payload = new byte[declaredLen];
                Buffer.BlockCopy(packet, 2, payload, 0, declaredLen);
            }
            else
            {
                // raw payload fallback
                payload = packet;
            }

            if (payload.Length < 18)
            {
                reason = $"payload too short ({payload.Length})";
                payload = null;
                return false;
            }

            byte linkLayer = (byte)(payload[0] & 0x03);
            if (linkLayer > 2)
            {
                reason = $"invalid link-layer bits ({linkLayer})";
                payload = null;
                return false;
            }

            return true;
        }

        public bool ShouldShowDnpTabs()
        {
            return this.Customer == Customers.TORONTO_HYDRO
                || this.Customer == Customers.ONCOR
                || this.Customer == Customers.ENMAX
                || this.Customer == Customers.PSEG
                || this.Customer == Customers.EVERSOURCE
                || this.Customer == Customers.CONED
                || this.Customer == Customers.SCE;
        }

        private void OnSend(SendEventArgs sEA)
        {
            if (Send != null)
                Send(sEA);
        }


        #endregion

        #region Error Handling
        private void errorHandler(Exception ex)
        {
            if (DNPControlException != null)
            {
                DNPControlException(this, new ExceptionEventArgs(ex, "Error in DNP Setting Control"));
            }
            else
            {
                throw new Exception("No Exception Handler For DNP Control");
            }
        }
        #endregion

        #region Setting Values
        private delegate void setAllCB(byte[] bytePacket);

        public void SetAll(byte[] bytePacket)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    setAllCB sACB = new setAllCB(this.setAll);
                    this.Invoke(sACB, bytePacket);
                }
                else
                {
                    this.setAll(bytePacket);
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }
        }

        private void setAll(byte[] bytePacket)
        {
            byte temp;
            //LSByte comes first
            //bytes 0 and 1 for control word
            //bits 0 and 1 are for link layer

            if (!TryExtractDnpSettingsPayload(bytePacket, out var payload, out var reason))
            {
                System.Diagnostics.Debug.WriteLine($"[DNP] ucDNP ignored packet: {reason}");
                return;
            }

            bytePacket = payload;

            try
            {
                temp = (byte)(bytePacket[0] & 3);
                switch (temp)
                {
                    case 0:
                        this.comboBoxLinkLayerConfirm.SelectedItem = "Never";
                        break;
                    case 1:
                        this.comboBoxLinkLayerConfirm.SelectedItem = "Sometimes";
                        break;
                    case 2:
                        this.comboBoxLinkLayerConfirm.SelectedItem = "Always";
                        break;
                    default:
                        throw new Exception("Bad Value For Link Layer");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong DNP Setting from the master relay for Link Layer Confirm.");
                this.errorHandler(new Exception(dNPErrorMsg, ex));
                dataBackupDNP.dataBackup_dnpDefaults = true;
                return;
            }

            try
            {
                //Self Address
                temp = (byte)(bytePacket[0] & 4);
                if (temp == 4)
                    this.comboBoxSelfAddress.SelectedItem = "Enable";
                else
                    this.comboBoxSelfAddress.SelectedItem = "Disable";
            }
            catch (Exception ex)
            {

                MessageBox.Show("DNP Address error.");
                this.errorHandler(new Exception(dNPErrorMsg, ex));
                dataBackupDNP.dataBackup_dnpDefaults = true;
                return;
            }

            try
            {
                //Unsolallowed
                temp = (byte)(bytePacket[0] & 8);
                if (temp == 8)
                    this.comboBoxUnsolResponse.SelectedItem = "Enable";
                else
                    this.comboBoxUnsolResponse.SelectedItem = "Disable";
            }
            catch (Exception ex)
            {

                MessageBox.Show("DNP Unsolicited error.");
                this.errorHandler(new Exception(dNPErrorMsg, ex));
                dataBackupDNP.dataBackup_dnpDefaults = true;
                return;
            }

            try
            {
                //Termination Resistor
                temp = (byte)(bytePacket[0] & 16);
                if (temp == 16)
                    this.comboBoxTerminationResistor.SelectedItem = "Enable";
                else
                    this.comboBoxTerminationResistor.SelectedItem = "Disable";
            }
            catch (Exception ex)
            {

                MessageBox.Show("DNP Resistor error.");
                this.errorHandler(new Exception(dNPErrorMsg, ex));
                dataBackupDNP.dataBackup_dnpDefaults = true;
                return;
            }

            try
            {
                this.comboBoxDNPBaudRate.SelectedIndex = bytePacket[17];
            }
            catch (Exception ex)
            {

                MessageBox.Show("DNP Baud rate error.");
                this.errorHandler(new Exception(dNPErrorMsg, ex));
                dataBackupDNP.dataBackup_dnpDefaults = true;
                return;
            }

            try
            {
                //4&5&6&7 7 = MSB unsoltimeout
                UInt32 tempInt = bytePacket[7];
                tempInt <<= 8;
                tempInt += bytePacket[6];
                tempInt <<= 8;
                tempInt += bytePacket[5];
                tempInt <<= 8;
                tempInt += bytePacket[4];

                this.numericUpDownUnsolTimeout.Value = tempInt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong DNP Setting from the master relay for Unsolicited TimeOut.");
                this.errorHandler(new Exception(dNPErrorMsg, ex));
                dataBackupDNP.dataBackup_dnpDefaults = true;
                return;
            }

            try
            {
                UInt16 tempInt = bytePacket[9];
                tempInt <<= 8;
                tempInt += bytePacket[8];

                this.numericUpDownFragmentSize.Value = SnapFragmentSize(tempInt);
            }
            catch (Exception ex)
            {

                MessageBox.Show("DNP Fragment Size error.");
                this.errorHandler(new Exception(dNPErrorMsg, ex));
                dataBackupDNP.dataBackup_dnpDefaults = true;
                return;
            }

            //10 11 Destinaton addy
            try
            {
                UInt16 tempInt = bytePacket[11];
                tempInt <<= 8;
                tempInt += bytePacket[10];

                this.numericUpDownDestinationAddress.Value = tempInt;
            }
            catch (Exception ex)
            {

                MessageBox.Show("DNP Destination Address error.");
                this.errorHandler(new Exception(dNPErrorMsg, ex));
                dataBackupDNP.dataBackup_dnpDefaults = true;
                return;
            }
            //12 13 Source Addy
            try
            {
                UInt16 tempInt = bytePacket[13];
                tempInt <<= 8;
                tempInt += bytePacket[12];

                this.numericUpDownSourceAddress.Value = tempInt;
            }
            catch (Exception ex)
            {

                MessageBox.Show("DNP Source Address error.");
                this.errorHandler(new Exception(dNPErrorMsg, ex));
                dataBackupDNP.dataBackup_dnpDefaults = true;
                return;
            }
            //14 15 unsol max retries
            try
            {
                UInt16 tempInt = bytePacket[15];
                tempInt <<= 8;
                tempInt += bytePacket[14];

                this.numericUpDownUnsolRetries.Value = tempInt;
            }
            catch (Exception ex)
            {

                MessageBox.Show("DNP Unsolicited Retry error.");
                this.errorHandler(new Exception(dNPErrorMsg, ex));
                dataBackupDNP.dataBackup_dnpDefaults = true;
                return;
            }

            try
            {
                //byte 16 is Retries
                UInt16 tempInt2 = bytePacket[16];

                this.numericUpDownMaxEvents.Value = tempInt2;
            }
            catch (Exception ex)
            {

                MessageBox.Show("DNP error.");
                this.errorHandler(new Exception(dNPErrorMsg, ex));
                dataBackupDNP.dataBackup_dnpDefaults = true;
                return;
            }

            try
            {
                    ucDNPDeadBand uDDB = new ucDNPDeadBand();
                    uint index = 30;

                    foreach (Control C in this.groupBoxDIGITALGRIDDNPDeadBand.Controls)
                    {
                        bool failed = false;

                        try { uDDB = (ucDNPDeadBand)C; }
                        catch { failed = true; }

                        if (!failed)
                        {
                            uDDB.Value = bytePacket[index];
                            index++;
                        }
                    }
               
                //=====================Remove throbber and enable everything disaplayed on the screen=====================
                Application.UseWaitCursor = false;
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                //this.enableAll(true);
                //========================================================================================================

            }
            catch
            {
                dataBackupDNP.dataBackup_dnpDefaults = true;
                this.restoreDefaultsDeadBandVariables();
            }
        }
        #endregion

        #region Customer Handlers
        private void setCustomer()
        {
            this.numericUpDownMaxEvents.Value = 120;
            this.makeDefault();
            this.customerChanged = false;
        }

        private void restoreDefaultsDeadBandVariables()
        {
            foreach (Control c in this.groupBoxDIGITALGRIDDNPDeadBand.Controls)
            {
                bool failed = false;
                ucDNPDeadBand uDDB = new ucDNPDeadBand();

                try
                {
                    uDDB = (ucDNPDeadBand)c;
                }
                catch //if it is not a ucDeadBand box
                {
                    failed = true;
                }

                if (!failed)
                {
                    uDDB.Value = 0;
                }
            }

            this.buttonSendDeadBand_Click(this, new EventArgs());
        }
        private bool IsUplinkCustomer()
        {
            return this.Customer == Customers.ONCOR
                || this.Customer == Customers.CONED
                || this.Customer == Customers.EVERSOURCE
                || this.Customer == Customers.PSEG
                || this.Customer == Customers.ENMAX
                || this.Customer == Customers.SCE;
        }
        private bool IsDnpDbCustomer()
        {
            return this.Customer == Customers.TORONTO_HYDRO
                || this.Customer == Customers.ONCOR
                || this.Customer == Customers.CONED
                || this.Customer == Customers.EVERSOURCE
                || this.Customer == Customers.PSEG
                || this.Customer == Customers.ENMAX
                || this.Customer == Customers.SCE;
        }

        private void makeDefault()
        {
            if (this.customerChanged)
            {
                this.deadBandVariables.Clear();
            }

            if (!this.customerChanged)
                return;

            bool hasDeadBand = IsDnpDbCustomer();

            this.buttonDefaults.Text = "Restore Defaults";

            // ------------------------------------------------------------------------------------
            // Customers WITHOUT deadband: hide deadband UI and center DNP settings panel
            // ------------------------------------------------------------------------------------
            if (!hasDeadBand)
            {
                this.groupBoxMemphisDeadBand.Visible = false;

                this.groupBoxDIGITALGRIDDNPDeadBand.Enabled = false;
                this.groupBoxDIGITALGRIDDNPDeadBand.Visible = false;
                this.groupBoxDIGITALGRIDDNPDeadBand.Controls.Clear();

                this.buttonSendDeadBand.Enabled = false;
                this.buttonSendDeadBand.Visible = false;

                this.groupBoxDNPSettings.Location = new System.Drawing.Point(530, 40);
                this.buttonSendAllDNPSettings.Location = new System.Drawing.Point(40, 425);
                this.buttonRQDNPSettings.Location = new System.Drawing.Point(570, 565);
                this.buttonDefaults.Location = new System.Drawing.Point(570, 515);
                this.groupBoxDNPStatus.Location = new System.Drawing.Point(605, 620);
                this.panel_DNPsettings.Location = new System.Drawing.Point(527, 35);
                this.panel_DNPsettings.Size = new System.Drawing.Size(339, 690);

                this.customerChanged = false;
                return;
            }

            // ------------------------------------------------------------------------------------
            // TORONTO_HYDRO deadband path
            // ------------------------------------------------------------------------------------
            this.deadBandVariables.Clear();
            this.groupBoxDIGITALGRIDDNPDeadBand.Controls.Clear();
            this.groupBoxMemphisDeadBand.Visible = false;

            // Reset TH layout baseline (prevents overlap from prior non-deadband positioning)
            this.groupBoxDNPSettings.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDNPSettings.Size = new System.Drawing.Size(320, 590);

            this.panel_DNPsettings.Location = new System.Drawing.Point(0, 0);
            this.panel_DNPsettings.Size = new System.Drawing.Size(335, 820);

            this.buttonSendAllDNPSettings.Location = new System.Drawing.Point(40, 425);
            this.buttonDefaults.Location = new System.Drawing.Point(40, 475);
            this.buttonRQDNPSettings.Location = new System.Drawing.Point(40, 525);
            this.groupBoxDNPStatus.Location = new System.Drawing.Point(40, 600);

            // 54 new rev10 firmware - ucDeadBandSettingsObject includes default value parameter
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Voltage", "Volts", 0.0m, 255m, 1, "Applies to all Network and Transformer Voltages", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Voltage Angle", "Degrees", 0, 180, 1, "Applies to all Network and Transformer Voltages", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Apparent Diff Voltage", "1Volts", 0, 25.5m, 10, "Applies to all three Differential Voltages in 0.1 Volt steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Apparent Diff Volt Angle", "Degrees", 0, 180, 1, "Applies to all three Differential Voltages in 0.1 Volt steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Apparent Diff Voltage", "1Volts", 0, 25.5m, 10, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Apparent Diff Volt Angle", "Degrees", 0, 180, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Real Diff Voltage", "1Volts", 0, 25.5m, 10, "Applies to all three Differential Voltages in 0.1 Volt steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Real Diff Voltage", "1Volts", 0, 25.5m, 1, "In 0.1 Volt steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Current", "Amps", 0m, 255m, 1, "Applies to all three Phase Currents", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Current Angle", "Degrees", 0, 180, 1, "Applies to all three Phase Currents", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Effective Current", "Amps", 0m, 255m, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Effective Current Angle", "Degrees", 0, 180, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Pos Seq Current", "Amps", 0m, 255m, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Pos Seq Current Angle", "Degrees", 0, 180, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Neg Seq Current", "Amps", 0m, 255m, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Neg Seq Current Angle", "Degrees", 0, 180, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Apparent Power", "kVA", 0.0m, 255m, 1, "Applies to all three Apparent Powers", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Apparent Power Angle", "Degrees", 0, 180, 1, "Applies to all three Apparent Powers", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Apparent Power", "kVA", 0.0m, 255m, 1, "Avg Apparent Power", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Apparent Power Angle", "Degrees", 0, 180, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Real Power", "kVA", 0.0m, 255m, 1, "Applies to all three Real Powers", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Real Power", "kVA", 0.0m, 255m, 1, "Avg Real Power", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("PhaseKVAR", "kVAR", 0m, 25.5m, 10, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("PhaseKVAR", "kVAR", 0m, 25.5m, 10, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("DiffVoltPositiveSequence", "Volts", 0m, 25.5m, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("DiffVoltPositiveSequenceAngle", "Degrees", 0m, 180m, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("DiffVoltNegativeSequence", "Volts", 0m, 25.5m, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("DiffVoltNegativeSequenceAngle", "Degrees", 0m, 180m, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("VoltPositiveSequence", "Volts", 0m, 255m, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("VoltPositiveSequenceAngle", "Degrees", 0m, 180, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("VoltNegativeSequence", "Volts", 0m, 255m, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("VoltNegativeSequenceAngle", "Degrees", 0m, 180, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("VoltageTHD", "Volts", 0m, 25.5m, 1, "Applies to all three Voltage THDs", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("CurrentTHD", "Volts", 0m, 25.5m, 1, "Applies to all three Current THDs", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Temperature", "Degrees C", 0.0m, 255m, 1, "Applies to Temperature", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Odometer", "Units", 0m, 1.0m, 1m, "", 0));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog1", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog2", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog3", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog4", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog5", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog6", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog7", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog8", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("AnalogA1", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("AnalogA2", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("AnalogC", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("AnalogD", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("AnalogE", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("AnalogF", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("AnalogG", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("AnalogH", "Volts", 0m, 5.0m, 10m, "Voltage input from 0-5 volts in 0.1 V steps", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("LoadPercentage", "%", 0m, 5.0m, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("TotalKW", "kWatts", 0m, 25.5m, 1, "", 1));
            this.deadBandVariables.Add(new ucDeadBandSettingsObject("TotalKVAR", "kVAR", 0m, 25.5m, 1, "Used for both TotalKVAR and TotalKVA", 1));

            Point location = new Point();
            ucDNPDeadBand workingDDB = new ucDNPDeadBand();

            location.Y = this.groupBoxDNPSettings.Location.Y;
            location.X = this.groupBoxDNPSettings.Location.X + this.groupBoxDNPSettings.Width + 2;

            bool hideLeftDnpSettings = IsUplinkCustomer(); // ONCOR/CONED/EVERSOURCE/PSEG/ENMAX/SCE only

            this.groupBoxDNPSettings.Visible = !hideLeftDnpSettings;
            this.groupBoxDNPStatus.Visible = !hideLeftDnpSettings;
            this.buttonSendAllDNPSettings.Visible = !hideLeftDnpSettings;
            this.buttonSendAllDNPSettings.Enabled = !hideLeftDnpSettings;

            // hide leftover left-side buttons for uplink customers
            this.buttonDefaults.Visible = !hideLeftDnpSettings;
            this.buttonRQDNPSettings.Visible = !hideLeftDnpSettings;
            this.buttonDefaults.Enabled = !hideLeftDnpSettings;
            this.buttonRQDNPSettings.Enabled = !hideLeftDnpSettings;

            this.groupBoxDIGITALGRIDDNPDeadBand.Location = hideLeftDnpSettings
                ? new System.Drawing.Point(8, 6)     // move left for uplink customers
                : new System.Drawing.Point(430, 6);  // keep TH/current layout

            location = new Point(2, 15); // starting spot

            foreach (ucDeadBandSettingsObject dBD in this.deadBandVariables)
            {
                workingDDB = new ucDNPDeadBand(dBD);
                workingDDB.Location = location;
                this.groupBoxDIGITALGRIDDNPDeadBand.Controls.Add(workingDDB);

                if (this.deadBandVariables.IndexOf(dBD) >= (this.deadBandVariables.Count / 2) - 1 && location.X == 2)
                {
                    location = new Point(location.X + workingDDB.Width - 100, 15);
                }
                else
                {
                    location = new Point(location.X, location.Y + (workingDDB.Height - 10) + 1);
                }
            }

            if (workingDDB != null)
                location = new Point(location.X, location.Y - workingDDB.Height);

            this.groupBoxDIGITALGRIDDNPDeadBand.Size = new System.Drawing.Size(1200, 900);
            this.groupBoxDIGITALGRIDDNPDeadBand.Enabled = true;
            this.groupBoxDIGITALGRIDDNPDeadBand.Visible = true;
            this.groupBoxDIGITALGRIDDNPDeadBand.Show();

            this.buttonSendDeadBand.Location = new System.Drawing.Point(1235, 670);
            this.buttonSendDeadBand.Size = new System.Drawing.Size(100, 80);
            this.buttonSendDeadBand.Enabled = true;
            this.buttonSendDeadBand.Visible = true;

            this.customerChanged = false;

            this.groupBoxDIGITALGRIDDNPDeadBand.BringToFront();
            this.buttonSendDeadBand.BringToFront();
        }

        //private void buttonDefaults_Click(object sender, EventArgs e)
        public void buttonDefaults_Click(object sender, EventArgs e)
        {
            this.setDefaultDefaults();
        }



        private void setDefaultDefaults()
        {
            this.numericUpDownDestinationAddress.Value = 3;
            this.numericUpDownFragmentSize.Value = 1024;
            this.numericUpDownMaxEvents.Value = 120;
            this.numericUpDownSourceAddress.Value = 4;
            this.numericUpDownUnsolRetries.Value = 5;
            this.numericUpDownUnsolTimeout.Value = 1000;
            this.comboBoxLinkLayerConfirm.SelectedIndex = 0;
            this.comboBoxSelfAddress.SelectedIndex = 1;
            this.comboBoxTerminationResistor.SelectedIndex = 1;
            this.comboBoxUnsolResponse.SelectedIndex = 1;

            bool is9600Customer =
                    this.Customer == Customers.ENMAX ||
                    this.Customer == Customers.CONED ||
                    this.Customer == Customers.ONCOR ||
                    this.Customer == Customers.SCE ||
                    this.Customer == Customers.TORONTO_HYDRO;

            this.comboBoxDNPBaudRate.SelectedIndex = is9600Customer ? 3 : 4;
        }

        #endregion

        private bool dNPLabelStatus = false;
        public bool DNPLabelStatus
        {
            get { return this.dNPLabelStatus; }
            set
            {
                this.dNPLabelStatus = value;
                this.setDNPLabelStatus();
            }
        }

        public void SetDnpBaudIndex(int index)
        {
            if (index < 0 || index >= this.comboBoxDNPBaudRate.Items.Count)
                return;

            this.comboBoxDNPBaudRate.SelectedIndex = index;
        }

        void setDNPLabelStatus()
        {
          //  this.labelDNPStatusInidcation.Visible = false;
            // This label is no more displayed since it caused confusion regarding the DNP activation Vs being installated in the relay firmware

                if (dNPLabelStatus == true)
              {
                  this.labelDNPStatusInidcation.Text = "Enabled";
                  this.labelDNPStatusInidcation.BackColor = Color.SkyBlue;
              }
              else
              {
                  this.labelDNPStatusInidcation.Text = "Disabled";
                  this.labelDNPStatusInidcation.BackColor = Color.LightSalmon;
              }
            

        }

        private void populateSaveModeDNPData(DNPSaveStateV4 DNPSS)
        {
            DNPSS.LinkLayerConfirm = (string)comboBoxLinkLayerConfirm.Text;


            if (comboBoxSelfAddress.SelectedIndex == 0)
                DNPSS.SelfAddress = true;
            else
                DNPSS.SelfAddress = false;


            if (comboBoxUnsolResponse.SelectedIndex == 0)
                DNPSS.UnsolResponse = true;
            else
                DNPSS.UnsolResponse = false;



            DNPSS.UnsolTimeout = (int)numericUpDownUnsolTimeout.Value;
            DNPSS.FragmentSize = (int)numericUpDownFragmentSize.Value;
            DNPSS.SourceAddress = (int)numericUpDownSourceAddress.Value;
            DNPSS.DestinationAddress = (int)numericUpDownDestinationAddress.Value;
            DNPSS.MaxEvents = (int)numericUpDownMaxEvents.Value;
            DNPSS.UnsolRetries = (int)numericUpDownUnsolRetries.Value;

            if (comboBoxTerminationResistor.SelectedIndex == 0)
                DNPSS.TerminationResistor = true;
            else
                DNPSS.TerminationResistor = false;

            DNPSS.DNPBaudRate = Convert.ToInt32(comboBoxDNPBaudRate.Text);

            ucDNPDeadBand uDDB = new ucDNPDeadBand();

            foreach (Control c in this.groupBoxDIGITALGRIDDNPDeadBand.Controls)
            {
                try
                {
                    if (c is ucDNPDeadBand)
                    {
                        uDDB = (ucDNPDeadBand)c;
                        DNPSS.deadBandControlSaveddecimal.Add(uDDB.Value);
                    }
                }
                catch (Exception ex)
                {
                    this.errorHandler(new Exception("Error In Saving Setting From DeadBand Values in DNP", ex));
                }
            }
        }

        public void SetAllValues(DNPSaveStateV4 DNPSS)
        {
            try
            {
                if (DNPSS.LinkLayerConfirm == "Never")
                    comboBoxLinkLayerConfirm.SelectedIndex = 0;
                else if (DNPSS.LinkLayerConfirm == "Sometimes")
                    comboBoxLinkLayerConfirm.SelectedIndex = 1;
                else if (DNPSS.LinkLayerConfirm == "Always")
                    comboBoxLinkLayerConfirm.SelectedIndex = 2;

                if (DNPSS.SelfAddress == true)
                    comboBoxSelfAddress.SelectedIndex = 0;
                else
                    comboBoxSelfAddress.SelectedIndex = 1;


                if (DNPSS.UnsolResponse == true)
                    comboBoxUnsolResponse.SelectedIndex = 0;
                else
                    comboBoxUnsolResponse.SelectedIndex = 1;

                numericUpDownUnsolTimeout.Value = DNPSS.UnsolTimeout;
                numericUpDownFragmentSize.Value = DNPSS.FragmentSize;
                numericUpDownSourceAddress.Value = DNPSS.SourceAddress;
                numericUpDownDestinationAddress.Value = DNPSS.DestinationAddress;
                numericUpDownMaxEvents.Value = DNPSS.MaxEvents;
                numericUpDownUnsolRetries.Value = DNPSS.UnsolRetries;

                if (DNPSS.TerminationResistor == true)
                    comboBoxTerminationResistor.SelectedIndex = 0;
                else
                    comboBoxTerminationResistor.SelectedIndex = 1;

                for (int i = 0; i <= comboBoxDNPBaudRate.Items.Count - 1; i++)
                {
                    comboBoxDNPBaudRate.SelectedIndex = i;
                    if (comboBoxDNPBaudRate.Text == Convert.ToString(DNPSS.DNPBaudRate))
                    {
                        break;
                    }
                    else
                    {
                        comboBoxDNPBaudRate.SelectedIndex++;
                    }
                }

                ucDNPDeadBand uDDB = new ucDNPDeadBand(); //start populating dead band saved data

                int indexDNPDEAD = 0;

                if (DNPSS.deadBandControlSaveddecimal.Count != 0)
                {
                    foreach (Control c in groupBoxDIGITALGRIDDNPDeadBand.Controls)
                    {
                        try
                        {
                            if (c is ucDNPDeadBand)
                            {
                                uDDB = (ucDNPDeadBand)c;
                                uDDB.Value = DNPSS.deadBandControlSaveddecimal[indexDNPDEAD];
                                indexDNPDEAD++;
                            }
                        }
                        catch (Exception ex) //if it is not a ucDeadBand box
                        {
                            this.errorHandler(new Exception("Error In Setting Values From DeadBand Save in DNP", ex));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error In Setting Values From Saved State in DNP", ex));
                this.errorHandler(new Exception("No Event Ranges Defined For This Customer"));
            }
        }

        public DNPSaveStateV4 GetSavedState()
        {
            DNPSaveStateV4 DNPSS = new DNPSaveStateV4();

            this.populateSaveModeDNPData(DNPSS);

            return DNPSS;
        }
    }

    [Serializable()]
    public class ucDeadBandSettingsObject
    {
        public ucDeadBandSettingsObject()
        {
        }

        public ucDeadBandSettingsObject(string name, string units, decimal min, decimal max)
        {
            this.Name = name + ":";
            this.Units = units;// + "*";
            this.Maximum = max;
            this.Minimum = min;
        }
        public ucDeadBandSettingsObject(string name, string units, decimal min, decimal max, string toolTip)
        {
            this.Name = name + ":";
            this.Units = units;// + "*";
            this.Maximum = max;
            this.Minimum = min;
            this.ToolTip = toolTip;
        }
        //public ucDeadBandSettingsObject(string name, string units, decimal min, decimal max, decimal mult, string toolTip)
        public ucDeadBandSettingsObject(string name, string units, decimal min, decimal max, decimal mult, string toolTip, decimal currDefault)
        {
            this.Name = name + ":";
            this.Units = units;// + "*";
            this.Maximum = max;
            this.Minimum = min;
            this.ToolTip = toolTip;
            this.Mult = mult;
            this.defVal = currDefault;
        }

        public string ToolTip;
        public string Name;
        public string Units;
        public decimal Minimum;
        public decimal Maximum;
        public decimal Mult = 1;
        public decimal defVal;
    }


    #region Saved States


    [Serializable()]

    public class DNPSaveStateV4 : ISerializable
    {
        public DNPSaveStateV4()
        {
            this.LinkLayerConfirm = "Never";
            this.SelfAddress = false;
            this.UnsolResponse = false;
            this.UnsolTimeout = 1000;
            this.FragmentSize = 1024;
            this.SourceAddress = 4;
            this.DestinationAddress = 3;
            this.MaxEvents = 120;
            this.UnsolRetries = 5;
            this.TerminationResistor = false;
            this.DNPBaudRate = 9600;
        }

        public string Name;
        public string LinkLayerConfirm;
        public bool SelfAddress;
        public bool UnsolResponse;
        public int UnsolTimeout;
        public int FragmentSize;
        public int SourceAddress;
        public int DestinationAddress;
        public int MaxEvents;
        public int UnsolRetries;
        public bool TerminationResistor;
        public int DNPBaudRate;

        public List<decimal> deadBandControlSaveddecimal = new List<decimal>();

        public List<string> SavedSettingsAvailable = new List<string>();

        public DNPSaveStateV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                foreach (SerializationEntry entry in info)
                {
                    SavedSettingsAvailable.Add(entry.Name);
                }

                this.Name = (string)info.GetValue("Name", typeof(string)); //fix
                this.LinkLayerConfirm = (string)info.GetValue("Link Layer Confirm", typeof(string));
                this.SelfAddress = (bool)info.GetValue("Self Address", typeof(bool));
                this.UnsolResponse = (bool)info.GetValue("Unsolicited Response", typeof(bool));
                this.UnsolTimeout = (int)info.GetValue("Unsolicited Timeout", typeof(int));
                this.FragmentSize = (int)info.GetValue("Fragment Size", typeof(int));
                this.SourceAddress = (int)info.GetValue("Source Address", typeof(int));
                this.DestinationAddress = (int)info.GetValue("Destination Address", typeof(int));
                this.MaxEvents = (int)info.GetValue("Max Events", typeof(int));
                this.UnsolRetries = (int)info.GetValue("Unsolicited Retries", typeof(int));
                this.TerminationResistor = (bool)info.GetValue("Termination Resistor", typeof(bool));
                this.DNPBaudRate = (int)info.GetValue("DNP BaudRate", typeof(int));

                if (SavedSettingsAvailable.Contains("DNP Deadband Values"))
                    this.deadBandControlSaveddecimal = (List<decimal>)info.GetValue("DNP Deadband Values", typeof(List<decimal>));
            }
            catch (Exception ex)
            {
                throw new Exception("Error In DNP Save State Contructor.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", this.Name);
                info.AddValue("Link Layer Confirm", this.LinkLayerConfirm);
                info.AddValue("Self Address", this.SelfAddress);
                info.AddValue("Unsolicited Response", this.UnsolResponse);
                info.AddValue("Unsolicited Timeout", this.UnsolTimeout);
                info.AddValue("Fragment Size", this.FragmentSize);
                info.AddValue("Source Address", this.SourceAddress);
                info.AddValue("Destination Address", this.DestinationAddress);
                info.AddValue("Unsolicited Retries", this.UnsolRetries);
                info.AddValue("Max Events", this.MaxEvents);
                info.AddValue("Termination Resistor", this.TerminationResistor);
                info.AddValue("DNP BaudRate", this.DNPBaudRate);
                info.AddValue("DNP Deadband Values", this.deadBandControlSaveddecimal);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in DNP Mode GetObjectData", ex);
            }
        }
        private SaveObject saveObject = new SaveObject();
    }



    [Serializable()]

    public class DNPSaveObjectV4 : ISerializable
    {
        public DNPSaveObjectV4()
        {
        }

        //public int NumberOfObjects;
        public List<DNPSaveStateV4> SavedStates = new List<DNPSaveStateV4>();

        public DNPSaveObjectV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                //this.NumberOfObjects = (int)info.GetValue("Number Of Objects", typeof(int));
                this.SavedStates = (List<DNPSaveStateV4>)info.GetValue("Saved States", typeof(List<DNPSaveStateV4>));
            }
            catch //(Exception ex)
            {
                this.SavedStates = null;
                //throw new Exception("Error in deserializing of Save Object in Trip Mode Settings.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Saved States", this.SavedStates);
                //info.AddValue("Number Of Objects", this.NumberOfObjects);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Saving Data In DNP Settings.", ex);
            }
        }

        public void AddSavedState(DNPSaveStateV4 dSS)
        {
            int i = 0;

            for (; i < SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name == dSS.Name || this.SavedStates[i].Name == null)
                {
                    this.SavedStates[i] = dSS;
                    break;
                }
            }

            if (i == SavedStates.Count)
            {
                this.SavedStates.Add(dSS);
            }

            //Sort list alphabetically
            this.SavedStates.Sort(delegate (DNPSaveStateV4 dSS1, DNPSaveStateV4 dSS2) { return dSS1.Name.CompareTo(dSS2.Name); });
        }

        private bool sameName(DNPSaveStateV4 dSS, string s)
        {
            if (dSS.Name == s)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveSavedState(DNPSaveStateV4 dSS)
        {
            if (this.SavedStates == null)
                return;
            this.SavedStates.Remove(dSS);
        }

        public void RemoveSavedState(string name)
        {
            if (this.SavedStates == null)
                return;

            for (int i = 0; i < this.SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name.Equals(name))
                {
                    this.SavedStates.Remove(this.SavedStates[i]);
                    break;
                }
            }
        }

    }

    #endregion
}
