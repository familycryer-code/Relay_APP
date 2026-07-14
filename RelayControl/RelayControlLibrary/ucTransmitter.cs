using NLog;
using RelayControlLibrary;
using SharedResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static RelayControlLibrary.ucTransmitterMonitoring;

namespace RelayControlLibrary
{
    public partial class ucTransmitter : UserControl
    {
        public ucTransmitter()
        {
            InitializeComponent();
            this.textBoxTXCTRatio.Text = "120";

            this.DNPCoverFlags = ((byte)(0));
            this.numericUpDownFragmentSize.ValueChanged += new System.EventHandler(this.numericUpDownFragmentSize_ValueChanged);

#if DEBUG || ENGINEERING
            this.textBoxSerialNumber.Enabled = true;
           // this.textBoxTransmitterOutputPower.Enabled = true;
            this.numericUpDownCurrentThresholdLow.Visible = true;
            this.labelCurrentThresholdLow.Visible = true;
            this.labelOperatingMode.Visible = true;
            this.textBoxOperatingMode.Visible = true;
            this.checkBoxWBC.Enabled = true;
            this.checkBoxWBD.Enabled = true;
            this.checkBoxWBE.Enabled = true;
            this.checkBoxWBF.Enabled = true;
            this.checkBoxWBG.Enabled = true;
            this.checkBoxWBH.Enabled = true;
            this.panelFlasgStatusWB.Location = new Point(730, 256);
            this.labelTransFlagStatus.Location = new Point(740, 250);
            labelGEWHDisplay.Visible = true;
#else
            this.textBoxSerialNumber.Enabled = false;
            this.numericUpDownCurrentThresholdLow.Visible = false;
            this.labelCurrentThresholdLow.Visible = false;
            this.labelOperatingMode.Visible = false;
            this.textBoxOperatingMode.Visible = false;
            this.panelMessageFreqSettings.Visible = false;
            this.labelMessageFrequencySettings.Visible = false;
            this.customerVersion = true;
            this.checkBoxDNPEnable.Visible = true;// false;
            this.dNPEnabled = this.checkBoxDNPEnable.Visible;
            this.checkBoxTransmitterEnable.Visible = false;
            this.panelMessageFreqSettings.Visible = false;
            this.labelLEDSpeed.Visible = false;
            this.numericUpDownLEDSpeed.Visible = false;
            this.button_FastFire.Enabled = false;
            this.button_FastFire.Visible = false;
            this.button_FastMode.Enabled = false;
            this.button_FastMode.Visible = false;

#endif

            this.panelFlasgStatusWB.Visible = false;
            this.labelTransFlagStatus.Visible = false;


#if !(DEBUG || ENGINEERING)
            checkBoxExtendedPLCMessage.Visible = false;
#endif

            this.comboBoxAnalog1OU.SelectedIndex = 0;
            this.comboBoxAnalog2OU.SelectedIndex = 0;

            this.checkBoxWBC.Checked = true;
            this.checkBoxWBD.Checked = true;
            this.checkBoxWBE.Checked = true;
            this.checkBoxWBF.Checked = true;
            this.checkBoxWBG.Checked = true;
            this.checkBoxWBH.Checked = true;

            this.panelGeneralSettings.Size = new System.Drawing.Size(370, 600);
            this.panelFlagSettings.Size = new System.Drawing.Size(250, 600);
            this.grpBox_TXcommands.Location = new System.Drawing.Point(715, 19);
            this.grpBox_TXcommands.Size = new System.Drawing.Size(220, 594);
            this.panel_TXco.Location = new System.Drawing.Point(713, 18);
            this.panel_TXco.Size = new System.Drawing.Size(226, 599);

            this.panelFlagSettings.Size = new System.Drawing.Size(221, 600);
            this.panelFlagSettings.Location = new System.Drawing.Point(440, 18);

            this.panelFlagSettingH.Location = new System.Drawing.Point(42, 550);
            this.panelFlagSettingH.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPHClose.Location = new System.Drawing.Point(3, 2);
            this.label12.Location = new System.Drawing.Point(19, 553);

            this.panelFlagSettingG.Location = new System.Drawing.Point(42, 480);
            this.panelFlagSettingG.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPGClose.Location = new System.Drawing.Point(3, 2);
            this.label11.Location = new System.Drawing.Point(19, 483);

            this.panelFlagSettingF.Location = new System.Drawing.Point(42, 410);
            this.panelFlagSettingF.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPFClose.Location = new System.Drawing.Point(3, 2);
            this.label10.Location = new System.Drawing.Point(19, 413);

            this.panelFlagSettingE.Location = new System.Drawing.Point(42, 340);
            this.panelFlagSettingE.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPEClose.Location = new System.Drawing.Point(3, 2);
            this.label9.Location = new System.Drawing.Point(19, 343);

            this.panelFlagSettingD.Location = new System.Drawing.Point(42, 270);
            this.panelFlagSettingD.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPDClose.Location = new System.Drawing.Point(3, 2);
            this.label8.Location = new System.Drawing.Point(19, 273);

            this.panelFlagSettingC.Location = new System.Drawing.Point(42, 200);
            this.panelFlagSettingC.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPCClose.Location = new System.Drawing.Point(3, 2); //(85, 2);
            this.label6.Location = new System.Drawing.Point(19, 203);

            this.panelFlagSettingB.Location = new System.Drawing.Point(42, 130);
            this.panelFlagSettingB.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPBClose.Location = new System.Drawing.Point(3, 2); //(85, 2);
            this.label5.Location = new System.Drawing.Point(19, 133);

            this.panelFlagSettingA.Location = new System.Drawing.Point(42, 60);
            this.panelFlagSettingA.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPAClose.Location = new System.Drawing.Point(3, 2); //(85, 2);
            this.labelFlagSettingA.Location = new System.Drawing.Point(19, 63);

            this.panelGeneralSettings.Size = new System.Drawing.Size(370, 600);

            this.panelFreqPanel.Location = new System.Drawing.Point(170, 461);
            this.panelFreqPanel.Size = new System.Drawing.Size(200, 210);
            this.label2.Location = new System.Drawing.Point(15, 505);
            this.checkBoxRed.Size = new System.Drawing.Size(160, 30);
            this.checkBoxBlue.Size = new System.Drawing.Size(160, 30);
            this.checkBoxGreen.Size = new System.Drawing.Size(160, 30);
            this.checkBoxYellow.Size = new System.Drawing.Size(160, 30);
            this.panelGeneralSettings.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelFreqPanel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxYellow.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxGreen.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxBlue.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxRed.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.labelTXCTRatio.Location = new System.Drawing.Point(49, 330);
            this.textBoxTXCTRatio.Location = new System.Drawing.Point(170, 330);
            this.btn_CTratioCal.Location = new System.Drawing.Point(245, 320);

            this.labelTXSN.Location = new System.Drawing.Point(49, 200);
            this.textBoxSerialNumber.Location = new System.Drawing.Point(170, 200);
            this.label1.Location = new System.Drawing.Point(33, 80);
            this.textBoxID.Location = new System.Drawing.Point(170, 78);

            //this.checkBoxDNPEnable.Location = new System.Drawing.Point(750, 515);
            //this.checkBoxDNPEnable.Size = new System.Drawing.Size(175, 100);
            //this.checkBoxDNPEnable.Text = "DNP Uplink Feature.       (A DIGITALGRID DNP Uplink kit is required for wireless/fiber locations)";
           // this.lbl_UplinkEn.Location = new System.Drawing.Point(738, 500);

            // DNP Comm settings groupBox ==============================================
            this.grpBx_DNPSettings.Location = new System.Drawing.Point(987, 19);
            this.grpBx_DNPSettings.Size = new System.Drawing.Size(285, 594);
            this.panel_dnpComSet.Location = new System.Drawing.Point(985, 17);
            this.panel_dnpComSet.Size = new System.Drawing.Size(289, 599);
            // DNP Comm settings groupBox ==============================================

#if !DNP
            this.grpBx_DNPSettings.Enabled = false;
            this.grpBx_DNPSettings.Visible = false;
            this.panel_dnpComSet.Enabled = false;
            this.panel_dnpComSet.Visible = false;
            this.lbl_UplinkEn.Enabled = false;
            this.lbl_UplinkEn.Visible = false;
            this.checkBoxDNPEnable.Enabled = false;
            this.checkBoxDNPEnable.Visible = false;
#endif

            NormalizeTxCommandButtonsLayout();
            NormalizeDnpUplinkPlacement();
            NormalizeTxButtonTextAlignment();

            // Default caption/state
            this.button_FastMode.Text = "Fast Mode Disabled";
            this.button_FastMode.BackColor = Color.Transparent;
            this.button_FastFire.Text = "Fast Fire";

            if (this.Customer == Customers.CONED)
            {
                this.button_FastFire.Visible = true;
                this.button_FastFire.Enabled = true;
                this.button_FastFire.Size = new Size(184, 75);
                this.button_FastFire.Location = new Point(18, 385);

                this.button_FastMode.Visible = true;
                this.button_FastMode.Enabled = true;
                this.button_FastMode.Size = new Size(184, 75);
                this.button_FastMode.Location = new Point(18, 510);

                this.buttonForceConfigMessage.Visible = false;
                this.buttonForceConfigMessage.Enabled = false;

                this.button_FastFire.BringToFront();
                this.button_FastMode.BringToFront();
            }
            else
            {
                this.button_FastFire.Location = new Point(1017, 400);
                this.button_FastFire.Size = new Size(184, 75);

                this.button_FastMode.Location = new Point(1017, 490);
                this.button_FastMode.Size = new Size(184, 75);

                this.buttonForceConfigMessage.Visible = true;
                this.buttonForceConfigMessage.Enabled = true;
                this.buttonForceConfigMessage.Text = "Send Configuration Message";
            }

            this.Load += ucTransmitter_Load;

        }
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private Customers customer;
        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                this.customer = value;
            }
        }



        private uint cTRatio = 320;
        public uint CTRatio
        {
            get { return this.cTRatio; }
            set
            {

                if (this.cTRatio != value && CTChanged != null)
                {
                    this.cTRatio = value;
                    CTChanged(this, new EventArgs());
                }
                else
                    this.cTRatio = value;

                this.TXSettings.CTRatio = (ushort)value;
            }
        }
        private bool dNPEnabled = false;
        public bool DNPEnabled
        {
            get { return this.dNPEnabled; }
            set
            {
                if (!this.forceDNPEnable)
                {
                    this.dNPEnabled = value;                  // <- keep state
                    //this.checkBoxDNPEnable.Checked = value;   // <- update UI
                                                              // do NOT set dnpUplinkK here
                }
            }
        }
        public delegate void CTChangedHandler(object sender, EventArgs e);
        public event CTChangedHandler CTChanged;


        public delegate void SendEventHandler(SendEventArgs sEA);
        public event SendEventHandler Send;
        public TransmitterSettings TXSettings = new TransmitterSettings();
        public bool WaterBugNoTransmitter
        {
            get { return this.waterBugNoTransmitter; }
            set
            {
                this.waterBugNoTransmitter = value;
                this.showWaterbugNoTransmitter(value);
            }
        }

        private uint masterRevisionNumber = 0;
        public uint MasterRevisionNumber
        {
            get { return this.masterRevisionNumber; }
            set { this.masterRevisionNumber = value; }
        }

        private int serialNumber = 0;
        private bool waterBugNoTransmitter = false;
        private SendEventArgs RQSEA = new SendEventArgs(3);
        private SendEventArgs TXSEA = new SendEventArgs(31);
        private int packetLength = 30;
#pragma warning disable CS0414 // field assigned but its value is never used
        private string dNPErrorMsg = "Please Verify all settings for DNP Tabs";
#pragma warning restore CS0414

        public int SerialNumber
        {
            get { return this.serialNumber; }
            set
            {
                this.serialNumber = value;
                this.textBoxSerialNumber.Text = value.ToString();
            }
        }

        private void CTCalc_Click(object sender, EventArgs e)
        {
            Form frmCT = new RelayControlLibrary.CTRatioCaculator2();
            frmCT.TopMost = true;
            frmCT.StartPosition = FormStartPosition.CenterScreen;
            DialogResult dlg = frmCT.ShowDialog(this);

            if (dlg == DialogResult.OK)
            {
                if (Convert.ToInt32(RelayControlLibrary.Calc2Data.Calc2DataInstance.get_CTCalc2Value()) >= 80 &&
                    Convert.ToInt32(RelayControlLibrary.Calc2Data.Calc2DataInstance.get_CTCalc2Value()) <= 335)
                {
                    this.textBoxTXCTRatio.Text = RelayControlLibrary.Calc2Data.Calc2DataInstance.get_CTCalc2Value();
                }
                else
                {
                    RelayControlLibrary.Calc2Data.Calc2DataInstance.set_CTRatioValueStatus("Value Must be between 80 and 335!");
                }
            }

            frmCT.Dispose();
            frmCT.TopMost = false;
        }

        public int PacketLength
        {
            get { return this.packetLength; }
            set
            {
                this.packetLength = value;
                TXSettings.PacketLength = value;
            }
        }

        private uint relayMasterRevision = 0;
        public uint RelayMasterRevision
        {
            get { return this.relayMasterRevision; }
            set { this.relayMasterRevision = value; }
        }

        private delegate void setAllCallBack(byte[] bA);
        private bool customerVersion = false;
        private bool badType1MessagePeriod = false;

        public void SetAllValues(byte[] bA)
        {
            if (this.InvokeRequired)
            {
                setAllCallBack sACB = new setAllCallBack(this.setAllValues);
                this.Invoke(sACB, new object[] { bA });
            }
            else
            {
                this.setAllValues(bA);
            }
        }

        void setAllValues(byte[] bA)
        {
            string errorString = "";
            this.version2Settings();

            try
            {
                UInt16 uTemp;
                //this.textBoxTransmitterOutputPower.Text = powerP.pwrPer.ToString();

                //Set ID number
                uTemp = bA[1];
                uTemp <<= 8;
                uTemp += bA[0];
                this.TXSettings.ID = uTemp;
                this.textBoxID.Text = this.TXSettings.ID.ToString();

                //string path = @"C:\DGI Systems\Relay\Saved Data\RelayData.txt";
                //TextWriter tw = new StreamWriter(path, true);
                //tw.WriteLine("ID"+uTemp);
                //tw.Close();

                //Set Serial Number
                uTemp = bA[3];
                uTemp <<= 8;
                uTemp += bA[2];
                this.TXSettings.SerialNumber = uTemp;
#if CONED
                //CONED asked to display serial numbers in range of 900001 to 965535
                //Just so they can distinguish DGI relays
                /* if (uTemp >= 1 || uTemp < 65535)
                     this.textBoxSerialNumber.Text = (900000 + uTemp).ToString();
                 else
                     this.textBoxSerialNumber.Text = "903076"; // default serial number for CONED
                */
                if (uTemp >= 1 || uTemp < 65535)
                    //this.textBoxSerialNumber.Text = this.TXSettings.SerialNumber.ToString();
                    this.textBoxSerialNumber.Text = (900000 + this.TXSettings.SerialNumber).ToString();
                else
                    this.textBoxSerialNumber.Text = "3076"; // default serial number
#else
                if (uTemp >= 1 || uTemp < 65535)
                    this.textBoxSerialNumber.Text = this.TXSettings.SerialNumber.ToString();
                else
                    this.textBoxSerialNumber.Text = "3076"; // default serial number
#endif
                //set Relay CT Ratio
                uTemp = bA[7];
                uTemp <<= 8;
                uTemp += bA[6];

                //set TX Ratio
                uTemp = bA[5];
                uTemp <<= 8;
                uTemp += bA[4];
                this.TXSettings.TXCTRatio = uTemp;

                this.textBoxTXCTRatio.Text = this.TXSettings.TXCTRatio.ToString();
                //tw.WriteLine("CT" + this.TXSettings.TXCTRatio.ToString());
                //tw.Close();

                //Set the frequency
                this.SetFrequency(RelayModeFunctions.FrequencyFrom(bA[8]));

                //Set the Flag Polarity
                this.TXSettings.FlagPolarity.ByteValue = bA[9];
                this.setFlagPolarity(bA[9]);
                statusNew.flagFromRelay = true;
                flagP.transmitterFlagPolarity = bA[9];
                //this.ucTransmitterMonitoring2.setFlagPolarity(bA[9]);
                flagS.flagSettings = bA[9];

                //Enable Flag Alarms
                this.TXSettings.EnableFlagAlarms = bA[10];
                this.setEnableFlagAlarms(bA[10]);

                //Enable Other Alarms
                this.TXSettings.EnableOtherAlarms = bA[11];
                this.setEnableOtherAlarms(bA[11]);

                //Set Current Thresholds
                this.numericUpDownCurrentThresholdHigh.Value = this.TXSettings.CurrentThresholdHigh = bA[12];
                this.numericUpDownCurrentThresholdLow.Value = this.TXSettings.CurrentThresholdLow = bA[13];

                //Set Voltage Thresholds
                this.numericUpDownVoltageThresholdHigh.Value = this.TXSettings.VoltageThresholdHigh = bA[14];
                this.numericUpDownVoltageThresholdLow.Value = this.TXSettings.VoltageThresholdLow = bA[15];

                //Set Analog Thresholds
                this.numericUpDownAnalog1Threshold.Value = this.TXSettings.A1Threshold = (sbyte)bA[16];
                this.numericUpDownAnalog2Threshold.Value = this.TXSettings.A2Threshold = (sbyte)bA[17];

                //Analog Sense Over/Under
                this.TXSettings.AnalogAlarmSense = bA[18];
                this.setAnalogAlarmSenseValues(bA[18]);

                //Type 1 Message Period
                this.TXSettings.MessagePeriod = bA[19];
                this.setType1MessagePeriod(bA[19]);
                //Mux Period
                this.TXSettings.MuxPeriod = bA[20];
                this.setMuxPeriod(bA[20]);

                //Type 2 Message Period
                this.setType2MessagePeriod(bA[21]);

                //Config Message Period
                this.TXSettings.ConfigMessagePeriod = bA[22];
                this.setConfigMessagePeriod(bA[22]);

                //Alarm Burst Count
                this.TXSettings.AlarmBurstCount = bA[23];

                //Alarm Spacing
                this.TXSettings.AlarmSpacing = bA[24];

                //Other MEssage Burst Count
                this.TXSettings.OtherMessageBurstCount = bA[25];

                //Other Message Spacing
                this.TXSettings.OtherMessageBurstInterval = bA[26];

                //Temp Calibration
                if (this.packetLength == 30)
                    this.TXSettings.TemperatureCalibration = (sbyte)bA[27];
                else
                {
                    checkBoxExtendedPLCMessage.Checked =
                        this.TXSettings.ExtendedPLCMessage = (bA[27] & 0x01) == 1 ? true : false;
                }
                //Zero Crossing Phasing

                //Type 1 Message Length
                this.TXSettings.Type1MessageLength = bA[28];
                if ((bA[28] & 0x80) == 0x80)
                {
                    this.checkBoxSmartExternalCableEnable.Checked = true;
                    this.enableWaterbury(true);
                }
                else
                {
                    this.enableWaterbury(false);
                    this.checkBoxSmartExternalCableEnable.Checked = false;
                }

                bool txUplinkBit = (bA[28] & 0x04) == 0x04;
                bool isRev10Plus = this.MasterRevisionNumber >= Constants.Rev10Master;
                bool isToronto = this.Customer == Customers.TORONTO_HYDRO;

                // Rev10+: uplink bit controls uplink checkbox/state (except Toronto special handling)
                if (isRev10Plus && !isToronto)
                {
                    this.DNPEnabled = txUplinkBit;
                }
                else
                {
                    // Rev9/older (and Toronto exception path): do NOT auto-enable uplink from TX bit
                    this.DNPEnabled = false;
                }

                // Comm label should reflect relay-reported DNP state
                this.DNPCommLabelStatus = this.DNPEnabled;

                if ((bA[28] & 0x08) == 0x08)
                {
                    this.checkBoxTransmitterEnable.Checked = true;
                }
                else
                {
                    if (this.fPGARevisionValid)
                        this.checkBoxTransmitterEnable.Checked = true;
                    else
                        this.checkBoxTransmitterEnable.Checked = false;
                }

                if ((bA[28] & 0x10) == 0x10)
                {
                    labelGEWHDisplay.Text = "GE";
                    GeWhF.GeWh = true;
                }
                else
                {
                    labelGEWHDisplay.Text = "WH";
                    GeWhF.GeWh = false;
                }

                //Waterbury Harness Data
                this.TXSettings.DataFromWaterBug = bA[29];
                this.setWaterburyEnables(bA[29]);

                this.setButtonEnable(true, this.buttonTX);

                if (this.badType1MessagePeriod)
                {
                    this.TXSettings.MessagePeriod = 2;
                    this.buttonTX_Click(this, new EventArgs());
                }

                if (this.packetLength == 30)
                {
                    this.labelLEDSpeed.Visible = false;
                    this.numericUpDownLEDSpeed.Visible = false;
                    return;
                }
#if DEBUG || ENGINEERING
                this.labelLEDSpeed.Visible = true;
                this.numericUpDownLEDSpeed.Visible = true;
#endif
                this.numericUpDownLEDSpeed.Value = bA[30];

            }
            catch (Exception ex)
            {
                dataBackupTX.dataBackup_txDefaults = true;
                this.errorHandler(new Exception(ex.Message + " " + errorString));
            }
        }
        /// <summary>
        /// Called To Force DNP To be Enabled if it is a memphis style relay
        /// </summary>
        private void checkForDNPEnabled()
        {
            if (!this.checkBoxDNPEnable.Checked)
            {
                this.buttonTX_Click(this, new EventArgs());
            }

        }

        private void showWaterbugNoTransmitter(bool value)
        {
            // Deprecated: no-op (legacy layout overrides removed)
        }

        private void version2Settings()
        {
        }

        private void setAnalogAlarmSenseValues(byte p)
        {
            if ((p & 0x01) == 0x01)
                this.comboBoxAnalog1OU.SelectedIndex = 0;
            else
                this.comboBoxAnalog1OU.SelectedIndex = 1;

            if ((p & 0x02) == 0x02)
                this.comboBoxAnalog2OU.SelectedIndex = 0;
            else
                this.comboBoxAnalog2OU.SelectedIndex = 1;
        }

        private void setType1MessagePeriod(byte p)
        {
            if (p == 0)
                this.radioButton10S.Checked = true;
            else if (p == 1)
                this.radioButton180S.Checked = true;
            else
                this.radioButton60S.Checked = true;

            if (this.customerVersion && (p == 0 || p == 1))
            {
                this.badType1MessagePeriod = true;
                this.radioButton60S.Checked = true;
            }
            else
            {
                this.badType1MessagePeriod = false;
            }
        }

        private void setConfigMessagePeriod(byte p)
        {
            if (p == 0xFF)
            {
                this.checkBoxConfigOff.Checked = true;
                this.textBoxConfigMessageTime.Enabled = false;
            }
            else
            {
                this.checkBoxConfigOff.Checked = false;
                this.textBoxConfigMessageTime.Enabled = true;
                this.textBoxConfigMessageTime.Text = p.ToString();
            }

        }

        private void setType2MessagePeriod(byte p)
        {
            if (p == 0xFF)
            {
                this.checkBoxType2Off.Checked = true;
                this.textBoxType2MessageTime.Enabled = false;
            }
            else
            {
                this.checkBoxType2Off.Checked = false;
                this.textBoxType2MessageTime.Enabled = true;
                this.textBoxType2MessageTime.Text = p.ToString();
            }
        }

        private void setMuxPeriod(byte p)
        {
            if (p == 0xFF)
            {
                this.textBoxMuxBoxMessageTime.Enabled = false;
                this.checkBoxMUXBOXOff.Checked = true;
            }
            else
            {
                this.textBoxMuxBoxMessageTime.Enabled = true;
                this.checkBoxMUXBOXOff.Checked = false;
                this.textBoxMuxBoxMessageTime.Text = p.ToString();
            }
        }

        private void setEnableOtherAlarms(byte p)
        {
            if ((p & 4) == 4)
                this.checkBoxCurrent.Checked = true;
            else
                this.checkBoxCurrent.Checked = false;

            if ((p & 8) == 8)
                this.checkBoxUnderVolt.Checked = true;
            else
                this.checkBoxUnderVolt.Checked = false;

            if ((p & 16) == 16)
                this.checkBoxAnalog1.Checked = true;
            else
                this.checkBoxAnalog1.Checked = false;

            if ((p & 32) == 32)
                this.checkBoxAnalog2.Checked = true;
            else
                this.checkBoxAnalog2.Checked = false;

            if ((p & 64) == 64)
                this.checkBoxPump.Checked = true;
            else
                this.checkBoxPump.Checked = false;

            if ((p & 128) == 128)
                this.checkBoxOverVolt.Checked = true;
            else
                this.checkBoxOverVolt.Checked = false;
        }



        private void setWaterburyEnables(byte p)
        {

            if ((p & 1) == 1)
                this.checkBoxWBC.Checked = true;
            else
                this.checkBoxWBC.Checked = false;

            if ((p & 2) == 2)
                this.checkBoxWBD.Checked = true;
            else
                this.checkBoxWBD.Checked = false;

            if ((p & 4) == 4)
                this.checkBoxWBE.Checked = true;
            else
                this.checkBoxWBE.Checked = false;

            if ((p & 8) == 8)
                this.checkBoxWBF.Checked = true;
            else
                this.checkBoxWBF.Checked = false;

            if ((p & 16) == 16)
                this.checkBoxWBG.Checked = true;
            else
                this.checkBoxWBG.Checked = false;

            if ((p & 32) == 32)
                this.checkBoxWBH.Checked = true;
            else
                this.checkBoxWBH.Checked = false;

            if ((p & 64) == 64)
                this.checkBoxWBAn1.Checked = true;
            else
                this.checkBoxWBAn1.Checked = false;

            if ((p & 128) == 128)
                this.checkBoxWBAn2.Checked = true;
            else
                this.checkBoxWBAn2.Checked = false;

        }

        private void setEnableFlagAlarms(byte p)
        {
            if ((p & 1) == 1)
                this.checkBoxFAA.Checked = true;
            else
                this.checkBoxFAA.Checked = false;

            if ((p & 2) == 2)
                this.checkBoxFAB.Checked = true;
            else
                this.checkBoxFAB.Checked = false;

            if ((p & 4) == 4)
                this.checkBoxFAC.Checked = true;
            else
                this.checkBoxFAC.Checked = false;

            if ((p & 8) == 8)
                this.checkBoxFAD.Checked = true;
            else
                this.checkBoxFAD.Checked = false;

            if ((p & 16) == 16)
                this.checkBoxFAE.Checked = true;
            else
                this.checkBoxFAE.Checked = false;

            if ((p & 32) == 32)
                this.checkBoxFAF.Checked = true;
            else
                this.checkBoxFAF.Checked = false;

            if ((p & 64) == 64)
                this.checkBoxFAG.Checked = true;
            else
                this.checkBoxFAG.Checked = false;

            if ((p & 128) == 128)
                this.checkBoxFAH.Checked = true;
            else
                this.checkBoxFAH.Checked = false;
        }

        public void setMonitoringData(byte[] bytePacket)
        {
            // this.textBoxTransmitterOutputPower.Text = powerP.pwrPer.ToString();
            SetMonitoringData(bytePacket);
        }

        private void SetMonitoringData(byte[] bytePacket)
        {
            if (relayHBD.relayWithHBD == false)
            {// For master uP with SEC

                if ((bytePacket[6] & 1) == 1)//Transmitter Flags A is LSB
                {
                    this.checkBoxFlagStatusA.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusA.Checked = false;
                }

                if ((bytePacket[6] & 2) == 2)
                {
                    this.checkBoxFlagStatusB.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusB.Checked = false;
                }
                if ((bytePacket[6] & 4) == 4)
                {
                    this.checkBoxFlagStatusC.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusC.Checked = false;
                }
                if ((bytePacket[6] & 8) == 8)
                {
                    this.checkBoxFlagStatusD.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusD.Checked = false;
                }
                if ((bytePacket[6] & 16) == 16)
                {
                    this.checkBoxFlagStatusE.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusE.Checked = false;
                }
                if ((bytePacket[6] & 32) == 32)
                {
                    this.checkBoxFlagStatusF.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusF.Checked = false;
                }
                if ((bytePacket[6] & 64) == 64)
                {
                    this.checkBoxFlagStatusG.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusG.Checked = false;
                }
                if ((bytePacket[6] & 128) == 128)
                {
                    this.checkBoxFlagStatusH.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusH.Checked = false;
                }
            }
            else if (relayHBD.relayWithHBD == true)
            {// For master uP with HBoard
#if CONED
                if ((bytePacket[6] & 2) == 2) // Digital Input 1
                {
                    this.checkBoxFlagStatusA.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusA.Checked = false;
                }

                if ((bytePacket[6] & 8) == 8) // Digital Input 2
                {
                    this.checkBoxFlagStatusB.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusB.Checked = false;
                }
#elif TORONTO_HYDRO
                if ((bytePacket[6] & 2) == 2) // Digital Input 1
                {
                    this.checkBoxFlagStatusB.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusB.Checked = false;
                }

                if ((bytePacket[6] & 8) == 8) // Digital Input 2
                {
                    this.checkBoxFlagStatusA.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusA.Checked = false;
                }
#endif
                if ((bytePacket[6] & 1) == 1) // Digital Input 3
                {
                    this.checkBoxFlagStatusC.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusC.Checked = false;
                }

                if ((bytePacket[6] & 16) == 16) // Digital Input 4
                {
                    this.checkBoxFlagStatusD.Checked = true;
                }
                else
                {
                    this.checkBoxFlagStatusD.Checked = false;
                }
            }
        }

        private void setCTRatioBox(UInt16 p, DomainUpDown dUP, TextBox tB)
        {
            switch (p)
            {
                case 160:
                    this.setDomainIndex(6, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 240:
                    this.setDomainIndex(5, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 320:
                    this.setDomainIndex(4, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 400:
                    this.setDomainIndex(3, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 500:
                    this.setDomainIndex(2, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 600:
                    this.setDomainIndex(1, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 700:
                    this.setDomainIndex(0, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                default:
                    this.setDomainIndex(7, dUP);
                    this.setTextBoxEnable(true, tB);
                    break;
            }
        }

        private void setCTRatioBox(Int16 p, DomainUpDown dUP, TextBox tB)
        {
            switch (p)
            {
                case 160:
                    this.setDomainIndex(6, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 240:
                    this.setDomainIndex(5, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 320:
                    this.setDomainIndex(4, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 400:
                    this.setDomainIndex(3, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 500:
                    this.setDomainIndex(2, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 600:
                    this.setDomainIndex(1, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                case 700:
                    this.setDomainIndex(0, dUP);
                    this.setTextBoxEnable(false, tB);
                    break;
                default:
                    this.setDomainIndex(7, dUP);
                    this.setTextBoxEnable(true, tB);
                    break;
            }
        }

        public void SetID(Int16 i)
        {
            this.setTextBox(i.ToString(), this.textBoxID);
            this.buttonTX.Enabled = true;
        }

        public void SetFrequency(Frequencies f)
        {
            switch (f)
            {
                case Frequencies.Blue:
                    this.setCheckBox(true, this.checkBoxBlue);
                    this.setCheckBox(false, this.checkBoxGreen);
                    this.setCheckBox(false, this.checkBoxRed);
                    this.setCheckBox(false, this.checkBoxYellow);
                    break;
                case Frequencies.Green:
                    this.setCheckBox(false, this.checkBoxBlue);
                    this.setCheckBox(true, this.checkBoxGreen);
                    this.setCheckBox(false, this.checkBoxRed);
                    this.setCheckBox(false, this.checkBoxYellow);
                    break;
                case Frequencies.Red:
                    this.setCheckBox(false, this.checkBoxBlue);
                    this.setCheckBox(false, this.checkBoxGreen);
                    this.setCheckBox(true, this.checkBoxRed);
                    this.setCheckBox(false, this.checkBoxYellow);
                    break;
                case Frequencies.Yellow:
                    this.setCheckBox(false, this.checkBoxBlue);
                    this.setCheckBox(false, this.checkBoxGreen);
                    this.setCheckBox(false, this.checkBoxRed);
                    this.setCheckBox(true, this.checkBoxYellow);
                    break;
            }
        }

        private void OnSend(SendEventArgs sEA)
        {
            if (Send != null)
                Send(sEA);
        }

        public void buttonRQ_Click(object sender, EventArgs e)
        {
            //=====================Display throbber while parameters get requested from the master relay  =====================
            Application.UseWaitCursor = true; //keeps waitcursor even when the thread ends.
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor; //Normal mode of setting waitcursor
            //this.enableAll(false);
            //========================================================================================================

            byte[] packet = new byte[3];

            packet[0] = (byte)'X';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.RQSEA.SendPacket = packet;

            OnSend(RQSEA);
        }

        private void buttonForceConfigMessage_Click(object sender, EventArgs e)
        {
            //=====================Display throbber while parameters get requested from the master relay  =====================
            Application.UseWaitCursor = true; //keeps waitcursor even when the thread ends.
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor; //Normal mode of setting waitcursor
            //this.enableAll(false);
            //========================================================================================================

            byte[] packet = new byte[3];

            packet[0] = 0x66;
            packet[1] = 0x01;
            packet[2] = 0x0D;

            this.RQSEA.SendPacket = packet;

            OnSend(RQSEA);
        }

        private void dnpUplink_Click(object sender, EventArgs e)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            DialogResult dR;

            if (this.checkBoxDNPEnable.Checked == true)
            {
                //dR = new YesNoMessageBoxResized("DNP Uplink", "Have you installed the 'DNP Uplink Kit'?", "Yes", "No").ShowDialog();
                dR = new YesNoMessageBoxResized("Enable", " Enable DNP Uplink Feature ? ", "Yes", "No").ShowDialog();
                if (dR == DialogResult.Yes)
                {
                    dnpUplinkK.dnpEnabledWithKit = true;
                    MessageBox.Show("Please click on Apply button in Transmission Commands followed by apply button in DNP settings to actually Enable the DNP status");
                }
                else
                {
                    MessageBox.Show("Please ensure the 'DNP Uplink Kit' is installed before activating the 'DNP Uplink' feature. Activating this feature without the required kit will disable communication with the Relay Control and Monitoring Application", "Kit Required");
                    dnpUplinkK.dnpEnabledWithKit = false;
                    checkBoxDNPEnable.Checked = false;
                }
            }
            else
            {
                dnpUplinkK.dnpEnabledWithKit = false;
            }
        }

        //private void buttonTX_Click(object sender, EventArgs e)
        public void buttonTX_Click(object sender, EventArgs e)
        {
            if (dnpUplinkK.dnpEnabledWithKit == true)
            {
                applyTX.applyTxSettings = true;
            }
            //=====================Display throbber while parameters get sent to the master relay  =====================
            Application.UseWaitCursor = true; //keeps waitcursor even when the thread ends.
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor; //Normal mode of setting waitcursor
            //this.enableAll(false);
            //========================================================================================================
            this.SendTransmitterSettings();
        }

        public void SendTransmitterSettings()
        {
            string errorMessage = "";
            try
            {
                errorMessage = "Bad ID value";
                this.tempID = Convert.ToUInt16(this.textBoxID.Text);

#if CONED
                if (this.tempID < 1 || this.tempID > 2046)
                {
                    throw new Exception("Transmission ID must be between 1 and 2046");
                }
#else
                if (this.tempID < 1 || this.tempID > 1023)
                {
                    throw new Exception("Transmission ID must be between 1 and 1023");
                }
#endif
                this.TXSettings.ID = this.tempID;

                errorMessage = "Bad Serial Number";
                //this.tempID = Convert.ToUInt16(this.textBoxSerialNumber.Text);
#if CONED
                //this.tempID = Convert.ToUInt16(this.SerialNumber);     //this.tempID = this.TXSettings.SerialNumber;
           //     int iSN = Convert.ToUInt16(this.textBoxSerialNumber.Text);
           if((Convert.ToUInt32(this.textBoxSerialNumber.Text) < 65535))
                    this.tempID = Convert.ToUInt16(this.textBoxSerialNumber.Text);
                else if ((Convert.ToUInt32(this.textBoxSerialNumber.Text) > 900000))
                            this.tempID = (ushort)(Convert.ToUInt32(this.textBoxSerialNumber.Text) - 900000);
           
#else
                this.tempID = Convert.ToUInt16(this.textBoxSerialNumber.Text);
#endif

                this.TXSettings.SerialNumber = this.tempID;

                this.textBoxSerialNumber.Text = this.TXSettings.SerialNumber.ToString();
                this.labelErrorLabel.Text = errorMessage;

                errorMessage = "Bad TX CT value";
                this.tempID = Convert.ToUInt16(this.textBoxTXCTRatio.Text);
                this.TXSettings.TXCTRatio = this.tempID;


                //Frequency
                if (this.checkBoxRed.Checked)
                    this.TXSettings.Frequency = Frequencies.Red;
                else if (this.checkBoxBlue.Checked)
                    this.TXSettings.Frequency = Frequencies.Blue;
                else if (this.checkBoxGreen.Checked)
                    this.TXSettings.Frequency = Frequencies.Green;
                else if (this.checkBoxYellow.Checked)
                    this.TXSettings.Frequency = Frequencies.Yellow;

                //Flag Polarity

                errorMessage = "Error Setting Flag Polarities";

                this.TXSettings.FlagPolarity.A = radioButtonFPAClose.Checked;
                this.TXSettings.FlagPolarity.B = radioButtonFPBClose.Checked;
                this.TXSettings.FlagPolarity.C = radioButtonFPCClose.Checked;
                this.TXSettings.FlagPolarity.D = radioButtonFPDClose.Checked;
                this.TXSettings.FlagPolarity.E = radioButtonFPEClose.Checked;
                this.TXSettings.FlagPolarity.F = radioButtonFPFClose.Checked;
                this.TXSettings.FlagPolarity.G = radioButtonFPGClose.Checked;
                this.TXSettings.FlagPolarity.H = radioButtonFPHClose.Checked;
                this.TXSettings.SetFlagPolartityByte();


                //enable Flag Alarms
                errorMessage = "Bad Error Flag Alarm Settings";
                byte tempByte = 0;

                if (this.checkBoxFAA.Checked)
                    tempByte += 1;
                if (this.checkBoxFAB.Checked)
                    tempByte += 2;
                if (this.checkBoxFAC.Checked)
                    tempByte += 4;
                if (this.checkBoxFAD.Checked)
                    tempByte += 8;
                if (this.checkBoxFAE.Checked)
                    tempByte += 16;
                if (this.checkBoxFAF.Checked)
                    tempByte += 32;
                if (this.checkBoxFAG.Checked)
                    tempByte += 64;
                if (this.checkBoxFAH.Checked)
                    tempByte += 128;

                this.TXSettings.EnableFlagAlarms = tempByte;
                //Enable Other Alarms
                errorMessage = "Bad Enable Other Alarms Setting";
                tempByte = 0;

                if (this.checkBoxCurrent.Checked)
                    tempByte += 4;
                if (this.checkBoxUnderVolt.Checked)
                    tempByte += 8;
                if (this.checkBoxAnalog1.Checked)
                    tempByte += 16;
                if (this.checkBoxAnalog2.Checked)
                    tempByte += 32;
                if (this.checkBoxPump.Checked)
                    tempByte += 64;
                if (this.checkBoxOverVolt.Checked)
                    tempByte += 128;

                this.TXSettings.EnableOtherAlarms = tempByte;

                //Thresholds
                errorMessage = "Error Setting Thresholds";

                if (this.numericUpDownCurrentThresholdHigh.Value <= this.numericUpDownCurrentThresholdLow.Value)
                {
                    errorMessage = "Current Thresholds Bad";
                    throw new Exception("Current Threshold Low must be lower than Current Threshold High");
                }

                if (this.numericUpDownVoltageThresholdHigh.Value <= this.numericUpDownVoltageThresholdLow.Value)
                {
                    errorMessage = "Bad Voltage Thresholds";
                    throw new Exception("Voltage Threshold Low must be lower than Voltage Threshold High");
                }
                this.TXSettings.VoltageThresholdHigh = (byte)this.numericUpDownVoltageThresholdHigh.Value;
                this.TXSettings.VoltageThresholdLow = (byte)this.numericUpDownVoltageThresholdLow.Value;
                this.TXSettings.CurrentThresholdHigh = (byte)this.numericUpDownCurrentThresholdHigh.Value;
                this.TXSettings.CurrentThresholdLow = (byte)this.numericUpDownCurrentThresholdLow.Value;
                this.TXSettings.A1Threshold = (sbyte)this.numericUpDownAnalog1Threshold.Value;
                this.TXSettings.A2Threshold = (sbyte)this.numericUpDownAnalog2Threshold.Value;

                //Analog Alarm Sense
                errorMessage = "Bad Analog Alarm Sense";
                tempByte = 0;

                if (this.comboBoxAnalog1OU.SelectedIndex == 0)
                    tempByte += 1;
                if (this.comboBoxAnalog2OU.SelectedIndex == 0)
                    tempByte += 2;

                this.TXSettings.AnalogAlarmSense = tempByte;
                //Type 1 Message Period

                errorMessage = "Bad Type 1 Message Period";
                if (this.radioButton10S.Checked)
                {
                    this.TXSettings.MessagePeriod = 0;
                }
                else if (this.radioButton180S.Checked)
                {
                    this.TXSettings.MessagePeriod = 1;
                }
                else
                {
                    this.TXSettings.MessagePeriod = 2;
                }

                //Mux Period
                errorMessage = "Bad Mux Period";
                try
                {
                    if (this.checkBoxMUXBOXOff.Checked)
                        tempByte = 0xFF;
                    else
                        tempByte = Convert.ToByte(this.textBoxMuxBoxMessageTime.Text);
                }
                catch
                {
                    throw new Exception("Illegal Mux Box Period Value");
                }
                this.TXSettings.MuxPeriod = tempByte;

                //Type 2 Message Period
                errorMessage = "Type 2 Message Period";
                if (this.checkBoxType2Off.Checked)
                    this.TXSettings.Type2MessagePeriod = 0xFF;
                else
                {
                    try
                    {
                        tempByte = Convert.ToByte(this.textBoxType2MessageTime.Text);
                    }
                    catch
                    {
                        throw new Exception("Illegal Type 2 Interval Value");
                    }
                    this.TXSettings.Type2MessagePeriod = tempByte;
                }

                //Config Message period
                errorMessage = "Config Message Period Error";
                if (this.checkBoxConfigOff.Checked)
                    this.TXSettings.ConfigMessagePeriod = 0xFF;
                else
                {
                    try
                    {
                        tempByte = Convert.ToByte(this.textBoxConfigMessageTime.Text);
                    }
                    catch
                    {
                        throw new Exception("Illegal Config Message Value");
                    }
                    this.TXSettings.ConfigMessagePeriod = tempByte;
                }

                //Waterbury
                if (this.checkBoxSmartExternalCableEnable.Checked)
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength | (byte)0x80);
                else
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength & (byte)0x7F);

                if (GERelay)
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength | (byte)0x10);
                else
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength & (byte)0xEF);

                //if (this.checkBoxDNPEnable.Checked)
                if (dnpUplinkK.dnpEnabledWithKit == true) // check box checked AND DNP Uplink kit is also present
                {
                    applyDNP.applyDNPSettings = false; //uplinkC.uplinkCount += 1;
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength | (byte)0x04);
                }
                else
                {
                    applyTX.applyTxSettings = false; //uplinkC.uplinkCount = 0;
                    applyDNP.applyDNPSettings = false;
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength & (byte)0xFB);
                }

                if (this.checkBoxTransmitterEnable.Checked)
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength | (byte)0x08);
                else
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength & (byte)0xF7);

                //External Data/ Data from Waterbury
                errorMessage = "SEC Error";
                tempByte = 0;

                if (this.checkBoxWBC.Checked)
                    tempByte += 1;
                if (this.checkBoxWBD.Checked)
                    tempByte += 2;
                if (this.checkBoxWBE.Checked)
                    tempByte += 4;
                if (this.checkBoxWBF.Checked)
                    tempByte += 8;
                if (this.checkBoxWBG.Checked)
                    tempByte += 16;
                if (this.checkBoxWBH.Checked)
                    tempByte += 32;
                if (this.checkBoxWBAn1.Checked)
                    tempByte += 64;
                if (this.checkBoxWBAn2.Checked)
                    tempByte += 128;
                this.TXSettings.DataFromWaterBug = tempByte;

                if (this.packetLength > 30)
                {
                    this.TXSettings.LEDSpeed = (byte)this.numericUpDownLEDSpeed.Value;
                }
#if (DEBUG || ENGINEERING)

                TXSettings.ExtendedPLCMessage = checkBoxExtendedPLCMessage.Checked;
#else
                TXSettings.ExtendedPLCMessage = true;
#endif
            }
            catch (Exception ex)
            {
                this.labelErrorLabel.Text = errorMessage;
                this.errorHandler(ex);
                this.buttonRQ_Click(this, new EventArgs());
                return;
            }

            this.labelErrorLabel.Text = "";
            this.TXSEA.SendPacket = this.TXSettings.SendPacket;

            OnSend(this.TXSEA);
        }

        public void setFlagPolarity(byte p)
        {
            if ((p & 1) == 1)
                this.radioButtonFPAClose.Checked = true;
            else
                this.radioButtonFPAOpen.Checked = true;

            if ((p & 2) == 2)
                this.radioButtonFPBClose.Checked = true;
            else
                this.radioButtonFPBOpen.Checked = true;

            if ((p & 4) == 4)
                this.radioButtonFPCClose.Checked = true;
            else
                this.radioButtonFPCOpen.Checked = true;

            if ((p & 8) == 8)
                this.radioButtonFPDClose.Checked = true;
            else
                this.radioButtonFPDOpen.Checked = true;

            if ((p & 16) == 16)
                this.radioButtonFPEClose.Checked = true;
            else
                this.radioButtonFPEOpen.Checked = true;

            if ((p & 32) == 32)
                this.radioButtonFPFClose.Checked = true;
            else
                this.radioButtonFPFOpen.Checked = true;

            if ((p & 64) == 64)
                this.radioButtonFPGClose.Checked = true;
            else
                this.radioButtonFPGOpen.Checked = true;

            if ((p & 128) == 128)
                this.radioButtonFPHClose.Checked = true;
            else
                this.radioButtonFPHOpen.Checked = true;

        }

        public byte dNPCoverFlags = 0;
        public byte DNPCoverFlags
        {
            get { return this.dNPCoverFlags; }
            set
            {
                this.dNPCoverFlags = value;
                this.setFlagPolarity(dNPCoverFlags);

            }
        }

        public void setPolarityFromRelaySettings()
        {
            this.TXSettings.FlagPolarity.A = Convert.ToBoolean(dNPCoverFlags & 1);
            this.TXSettings.FlagPolarity.B = Convert.ToBoolean(dNPCoverFlags & 2);
            this.TXSettings.FlagPolarity.C = Convert.ToBoolean(dNPCoverFlags & 4);
            this.TXSettings.FlagPolarity.D = Convert.ToBoolean(dNPCoverFlags & 8);
            this.TXSettings.FlagPolarity.E = Convert.ToBoolean(dNPCoverFlags & 16);
            this.TXSettings.FlagPolarity.F = Convert.ToBoolean(dNPCoverFlags & 32);
            this.TXSettings.FlagPolarity.G = Convert.ToBoolean(dNPCoverFlags & 64);
            this.TXSettings.FlagPolarity.H = Convert.ToBoolean(dNPCoverFlags & 128);
            this.TXSettings.SetFlagPolartityByte();

            this.radioButtonFPAClose.Checked = this.TXSettings.FlagPolarity.A;
            this.radioButtonFPBClose.Checked = this.TXSettings.FlagPolarity.B;
            this.radioButtonFPCClose.Checked = this.TXSettings.FlagPolarity.C;
            this.radioButtonFPDClose.Checked = this.TXSettings.FlagPolarity.D;
            this.radioButtonFPEClose.Checked = this.TXSettings.FlagPolarity.E;
            this.radioButtonFPFClose.Checked = this.TXSettings.FlagPolarity.F;
            this.radioButtonFPGClose.Checked = this.TXSettings.FlagPolarity.G;
            this.radioButtonFPHClose.Checked = this.TXSettings.FlagPolarity.H;

        }

        private void updateCheckBox(CheckBox cB, bool b)
        {
            if (b)
            {
                //cB.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Bold);
            }
            else
            {
                // cB.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Strikeout);
                cB.Checked = false;
            }
        }

        private void checkBoxRed_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cB = (CheckBox)sender;

            if (cB.Checked)
            {
                this.TXSettings.Frequency = Frequencies.Red;
                this.updateCheckBox(this.checkBoxRed, true);
                this.updateCheckBox(this.checkBoxBlue, false);
                this.updateCheckBox(this.checkBoxGreen, false);
                this.updateCheckBox(this.checkBoxYellow, false);
            }
        }

        private void checkBoxBlue_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cB = (CheckBox)sender;

            if (cB.Checked)
            {
                this.TXSettings.Frequency = Frequencies.Blue;
                this.updateCheckBox(this.checkBoxRed, false);
                this.updateCheckBox(this.checkBoxBlue, true);
                this.updateCheckBox(this.checkBoxGreen, false);
                this.updateCheckBox(this.checkBoxYellow, false);
            }

        }

        private void checkBoxGreen_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cB = (CheckBox)sender;

            if (cB.Checked)
            {
                this.TXSettings.Frequency = Frequencies.Green;

                this.updateCheckBox(this.checkBoxRed, false);
                this.updateCheckBox(this.checkBoxBlue, false);
                this.updateCheckBox(this.checkBoxGreen, true);
                this.updateCheckBox(this.checkBoxYellow, false);
            }
        }

        private void checkBoxYellow_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cB = (CheckBox)sender;

            if (cB.Checked)
            {
                this.TXSettings.Frequency = Frequencies.Yellow;
                this.updateCheckBox(this.checkBoxRed, false);
                this.updateCheckBox(this.checkBoxBlue, false);
                this.updateCheckBox(this.checkBoxGreen, false);
                this.updateCheckBox(this.checkBoxYellow, true);
            }
        }

        private UInt16 tempID;

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.tempID = Convert.ToUInt16(this.textBoxID.Text);
            }
            catch
            {
                this.buttonRQ_Click(this, new EventArgs());
            }
        }

        private delegate void setTextBoxCallBack(string s, TextBox tB);

        private void setTextBox(string s, TextBox tB)
        {
            if (tB.InvokeRequired)
            {
                setTextBoxCallBack sTB = new setTextBoxCallBack(setTextBox);
                this.Invoke(sTB, new object[] { s, tB });
            }
            else
            {
                tB.Text = s;
            }
        }

        private delegate void setCheckBoxCallBack(bool b, CheckBox tB);

        private void setCheckBox(bool b, CheckBox cB)
        {
            if (cB.InvokeRequired)
            {
                setCheckBoxCallBack sTB = new setCheckBoxCallBack(setCheckBox);
                this.Invoke(sTB, new object[] { b, cB });
            }
            else
            {
                cB.Checked = b;
            }
        }

        private delegate void setDomainIndexCallBack(int i, DomainUpDown dUP);

        private void setDomainIndex(int i, DomainUpDown dUP)
        {
            if (dUP.InvokeRequired)
            {
                setDomainIndexCallBack sTB = new setDomainIndexCallBack(setDomainIndex);
                this.Invoke(sTB, new object[] { i, dUP });
            }
            else
            {
                dUP.SelectedIndex = i;
            }
        }

        private delegate void setButtonEnableCallBack(bool b, Button bU);

        private void setButtonEnable(bool b, Button bU)
        {
            if (bU.InvokeRequired)
            {
                setButtonEnableCallBack sTB = new setButtonEnableCallBack(setButtonEnable);
                this.Invoke(sTB, new object[] { b, bU });
            }
            else
            {
                bU.Enabled = b;
            }
        }

        private delegate void setTextBoxEnableCallBack(bool b, TextBox tB);

        private void setTextBoxEnable(bool b, TextBox tB)
        {
            if (tB.InvokeRequired)
            {
                setTextBoxEnableCallBack sTB = new setTextBoxEnableCallBack(setTextBoxEnable);
                this.Invoke(sTB, new object[] { b, tB });
            }
            else
            {
                tB.Enabled = b;
            }
        }

        private void domainUpDownTXCTRatio_SelectedItemChanged(object sender, EventArgs e)
        {
            DomainUpDown dUD = (DomainUpDown)sender;
            UInt16 ratio = 160;

            switch (dUD.SelectedIndex)
            {
                case 0:
                    ratio = 160;
                    this.textBoxTXCTRatio.Enabled = false;
                    this.textBoxTXCTRatio.Text = ratio.ToString();
                    break;
                case 1:
                    ratio = 240;
                    this.textBoxTXCTRatio.Enabled = false;
                    this.textBoxTXCTRatio.Text = ratio.ToString();
                    break;
                case 2:
                    ratio = 320;
                    this.textBoxTXCTRatio.Enabled = false;
                    this.textBoxTXCTRatio.Text = ratio.ToString();
                    break;
                case 3:
                    ratio = 400;
                    this.textBoxTXCTRatio.Enabled = false;
                    this.textBoxTXCTRatio.Text = ratio.ToString();
                    break;
                case 4:
                    ratio = 500;
                    this.textBoxTXCTRatio.Enabled = false;
                    this.textBoxTXCTRatio.Text = ratio.ToString();
                    break;
                case 5:
                    ratio = 600;
                    this.textBoxTXCTRatio.Enabled = false;
                    this.textBoxTXCTRatio.Text = ratio.ToString();
                    break;
                case 6:
                    ratio = 700;
                    this.textBoxTXCTRatio.Enabled = false;
                    this.textBoxTXCTRatio.Text = ratio.ToString();
                    break;
                case 7:
                    try
                    {
                        ratio = Convert.ToUInt16(this.textBoxTXCTRatio.Text);
                    }
                    catch
                    {
                        this.labelErrorLabel.Text = "Bad CT Value";
                        this.buttonRQ_Click(this, new EventArgs());
                    }
                    this.textBoxTXCTRatio.Enabled = true;
                    this.textBoxTXCTRatio.Text = ratio.ToString();
                    break;
                default:
                    ratio = 160;
                    break;
            }
            this.TXSettings.TXCTRatio = ratio;
            this.labelErrorLabel.Text = "";
        }

        //private void buttonRestoreDefaults_Click(object sender, EventArgs e)
        public void buttonRestoreDefaults_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to restore default settings?", "Restore Defaults", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                if (this.GERelay)
                    this.setGEDefaults();
                else
                    this.SetDefaults();

                this.buttonTX.Enabled = true;
            }
        }

        private void setGEDefaults()
        {
            this.textBoxID.Text = "1023";
            this.textBoxTXCTRatio.Text = "120";
            this.CTRatio = 600;

            this.checkBoxBlue.Checked = true;
            this.checkBoxGreen.Checked = false;
            this.checkBoxRed.Checked = false;
            this.checkBoxYellow.Checked = false;

            this.radioButtonFPAClose.Checked = true;
            this.radioButtonFPBClose.Checked = true;
            this.radioButtonFPCClose.Checked = true;
            this.radioButtonFPDClose.Checked = true;
            this.radioButtonFPEClose.Checked = true;
            this.radioButtonFPFClose.Checked = true;
            this.radioButtonFPGClose.Checked = true;
            this.radioButtonFPHClose.Checked = true;

            this.checkBoxFAA.Checked = false;
            this.checkBoxFAB.Checked = false;
            this.checkBoxFAC.Checked = false;
            this.checkBoxFAD.Checked = false;
            this.checkBoxFAE.Checked = false;
            this.checkBoxFAF.Checked = false;
            this.checkBoxFAG.Checked = false;
            this.checkBoxFAH.Checked = false;

            this.checkBoxCurrent.Checked = false;
            this.checkBoxAnalog1.Checked = false;
            this.checkBoxAnalog2.Checked = false;
            this.checkBoxOverVolt.Checked = false;
            this.checkBoxPump.Checked = false;
            this.checkBoxType2Off.Checked = true;
            this.checkBoxUnderVolt.Checked = false;

            this.checkBoxSmartExternalCableEnable.Checked = true;
            this.checkBoxWBAn1.Checked = true;
            this.checkBoxWBAn2.Checked = true;
            this.checkBoxWBC.Checked = true;
            this.checkBoxWBD.Checked = true;
            this.checkBoxWBE.Checked = true;
            this.checkBoxWBF.Checked = true;
            this.checkBoxWBG.Checked = true;
            this.checkBoxWBH.Checked = true;

            //  this.enableWaterbury(true);

            this.numericUpDownAnalog1Threshold.Value = 100;
            this.numericUpDownAnalog2Threshold.Value = 100;
            this.numericUpDownCurrentThresholdHigh.Value = 100;
            this.numericUpDownCurrentThresholdLow.Value = 75;
            this.numericUpDownVoltageThresholdHigh.Value = 135;
            this.numericUpDownVoltageThresholdLow.Value = 110;
            this.radioButton60S.Checked = true;

            this.textBoxMuxBoxMessageTime.Text = "15";
            this.textBoxType2MessageTime.Text = "23";
            this.textBoxConfigMessageTime.Text = "23";

            this.TXSettings.AlarmBurstCount = 4;
            this.TXSettings.AlarmSpacing = 20;
            this.TXSettings.OtherMessageBurstCount = 4;
            this.TXSettings.OtherMessageBurstInterval = 30;
            this.TXSettings.TemperatureCalibration = 100;

            this.checkBoxSmartExternalCableEnable.Checked = true;
            this.TXSettings.Type1MessageLength = 0x82;

            this.TXSettings.DataFromWaterBug = 00;

            this.buttonTX.Enabled = true;

            this.DNPEnabled = this.checkBoxDNPEnable.Checked;

            this.checkBoxMUXBOXOff.Checked = true;

            this.numericUpDownLEDSpeed.Value = 20;
        }

        public void EnableTransmitter()
        {
            checkBoxTransmitterEnable.Checked = true;
        }

        public void DisableTransmitter()
        {
            checkBoxTransmitterEnable.Checked = false;
        }

        public void SetDefaults()
        {
            this.textBoxID.Text = "1023";
            this.textBoxTXCTRatio.Text = "120";
#if PSEG || CONED
            this.CTRatio = 320;
#else
            this.CTRatio = 600;
#endif

            this.checkBoxBlue.Checked = true;
            this.checkBoxGreen.Checked = false;
            this.checkBoxRed.Checked = false;
            this.checkBoxYellow.Checked = false;
            this.radioButtonFPAClose.Checked = true;

#if TORONTO_HYDRO
            this.radioButtonFPBClose.Checked = false;
            this.radioButtonFPBOpen.Checked = true;
#else
            this.radioButtonFPBClose.Checked = true;
#endif

            this.radioButtonFPCClose.Checked = true;
            this.radioButtonFPDClose.Checked = true;
            this.radioButtonFPEClose.Checked = true;
            this.radioButtonFPFClose.Checked = true;
            this.radioButtonFPGClose.Checked = true;
            this.radioButtonFPHClose.Checked = true;

            this.checkBoxFAA.Checked = false;
            this.checkBoxFAB.Checked = false;
            this.checkBoxFAC.Checked = false;
            this.checkBoxFAD.Checked = false;
            this.checkBoxFAE.Checked = false;
            this.checkBoxFAF.Checked = false;
            this.checkBoxFAG.Checked = false;
            this.checkBoxFAH.Checked = false;

            this.checkBoxCurrent.Checked = false;
            this.checkBoxAnalog1.Checked = false;
            this.checkBoxAnalog2.Checked = false;
            this.checkBoxOverVolt.Checked = false;
            this.checkBoxPump.Checked = false;
            this.checkBoxType2Off.Checked = true;
            this.checkBoxUnderVolt.Checked = false;

            this.checkBoxSmartExternalCableEnable.Checked = true;
            this.checkBoxWBAn1.Checked = true;
            this.checkBoxWBAn2.Checked = true;
            this.checkBoxWBC.Checked = true;
            this.checkBoxWBD.Checked = true;
            this.checkBoxWBE.Checked = true;
            this.checkBoxWBF.Checked = true;
            this.checkBoxWBG.Checked = true;
            this.checkBoxWBH.Checked = true;

            this.numericUpDownAnalog1Threshold.Value = 100;
            this.numericUpDownAnalog2Threshold.Value = 100;
            this.numericUpDownCurrentThresholdHigh.Value = 100;
            this.numericUpDownCurrentThresholdLow.Value = 75;
            this.numericUpDownVoltageThresholdHigh.Value = 135;
            this.numericUpDownVoltageThresholdLow.Value = 110;
            this.radioButton60S.Checked = true;

            this.textBoxMuxBoxMessageTime.Text = "15";
            this.textBoxType2MessageTime.Text = "23";
            this.textBoxConfigMessageTime.Text = "23";

            this.TXSettings.AlarmBurstCount = 4;
            this.TXSettings.AlarmSpacing = 20;
            this.TXSettings.OtherMessageBurstCount = 4;
            this.TXSettings.OtherMessageBurstInterval = 30;
            this.TXSettings.TemperatureCalibration = 100;

            this.checkBoxSmartExternalCableEnable.Checked = true;
            this.TXSettings.Type1MessageLength = 0x82;

            this.TXSettings.DataFromWaterBug = 00;

            this.buttonTX.Enabled = true;

            this.DNPEnabled = false;

            this.checkBoxMUXBOXOff.Checked = true;

            this.numericUpDownLEDSpeed.Value = 20;
        }

        public void SetDefaults(int tempI)
        {
            this.TXSettings.SerialNumber = (UInt16)tempI;
            this.textBoxSerialNumber.Text = tempI.ToString();
            this.SetDefaults();
        }


        #region Error Handling

        public delegate void ExceptionHandler(object o, ExceptionEventArgs eEA);
        public event ExceptionHandler TransmitterException;

        private void errorHandler(Exception ex)
        {
            if (TransmitterException != null)
                TransmitterException(this, new ExceptionEventArgs(ex, "Error in Transmitter Control"));
            else
                throw new Exception("No Exception Handler in Main for Transmitter Unit");
        }

        #endregion

        private void checkBoxType2Off_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxType2Off.Checked)
            {
                this.textBoxType2MessageTime.Enabled = false;
            }
            else
            {
                this.textBoxType2MessageTime.Enabled = true;
            }
        }

        private void checkBoxConfigOff_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxConfigOff.Checked)
            {
                this.textBoxConfigMessageTime.Enabled = false;
            }
            else
            {
                this.textBoxConfigMessageTime.Enabled = true;
            }

        }

        public void EnableAll()
        {
            this.buttonRestoreDefaults.Enabled = true;
            this.buttonTX.Enabled = true;
        }

        private PasswordForm pF;
        private bool passwordValidated = false;

        void pF_PasswordValidated(bool b)
        {
            if (b)  //password was accepted.
            {
                if (this.checkBoxSmartExternalCableEnable.Checked)
                {
                    this.enableWaterbury(true);
                }
                else
                {
                    this.enableWaterbury(false);
                }
            }
            else //password was rejected
            {
                if (this.checkBoxSmartExternalCableEnable.Checked)
                {
                    this.checkBoxSmartExternalCableEnable.Checked = false;
                }
                else
                {
                    this.checkBoxSmartExternalCableEnable.Checked = true;
                }
            }
            this.passwordValidated = true;
            this.pF.Close();
        }

        private void enableWaterbury(bool p)
        {
            // Keep UI stable: no customer-specific compile-time layout overrides
            this.panelWaterburyMain.Visible = p;
            this.labelSmartExternalCableMain.Visible = p;
            this.panelAlarmSettings.Visible = true;
            this.labelAlarmSettings.Visible = true;

            this.labelSmartExternalCableMain.BringToFront();

            // Keep original positioning behavior tied only to p
            if (p)
            {
                this.panelAlarmSettings.Location = new Point(342, this.panelAlarmSettings.Location.Y);
                this.labelAlarmSettings.Location = new Point(348, this.labelAlarmSettings.Location.Y);
                this.panelMessageFreqSettings.Location = new Point(491, this.panelMessageFreqSettings.Location.Y);
                this.labelMessageFrequencySettings.Location = new Point(497, this.labelMessageFrequencySettings.Location.Y);
            }
            else
            {
                this.panelAlarmSettings.Location = new Point(193, this.panelAlarmSettings.Location.Y);
                this.labelAlarmSettings.Location = new Point(199, this.labelAlarmSettings.Location.Y);
                this.panelMessageFreqSettings.Location = new Point(342, this.panelMessageFreqSettings.Location.Y);
                this.labelMessageFrequencySettings.Location = new Point(348, this.labelMessageFrequencySettings.Location.Y);
            }

            this.checkBoxWBAn1.Checked = p;
            this.checkBoxWBAn2.Checked = p;
            this.checkBoxWBC.Checked = p;
            this.checkBoxWBD.Checked = p;
            this.checkBoxWBE.Checked = p;
            this.checkBoxWBF.Checked = p;
            this.checkBoxWBG.Checked = p;
            this.checkBoxWBH.Checked = p;
        }

        private void checkBoxWaterburyHarnessEnable_Click(object sender, EventArgs e)
        {
            pF = new PasswordForm();

            this.passwordValidated = false;

            pF.PasswordValidated += new PasswordForm.ReturnValueHandler(pF_PasswordValidated);
            pF.FormClosed += new FormClosedEventHandler(pF_FormClosed);
            pF.Left = 200;
            pF.ControlBox = false;
            pF.ShowDialog();
        }

        void pF_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!this.passwordValidated)
            {
                this.pF_PasswordValidated(false);
            }
        }

        private void checkBoxMUXBOXOff_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxMuxBoxMessageTime.Enabled = !this.checkBoxMUXBOXOff.Checked;
        }

        public void EnableControl()
        {
            this.buttonTX.Enabled = true;
        }

        private void textBoxID_Leave(object sender, EventArgs e)
        {
            UInt64 temp;

            try
            {
                temp = Convert.ToUInt64(this.textBoxID.Text);
            }
            catch
            {
                this.errorHandler(new Exception("Bad ID Character"));
                return;
            }

            try
            {
                if (temp < 1)
                {
                    this.textBoxID.Text = "1";
                    throw new Exception();
                }

#if CONED
                if (temp > 2046)
                {
                    this.textBoxID.Text = "2046";
                    throw new Exception();
                }
#else
        if (temp > 1023)
        {
            this.textBoxID.Text = "1023";
            throw new Exception();
        }
#endif
            }
            catch
            {
#if CONED
                this.errorHandler(new Exception("ID value must be between 1 and 2046"));
#else
        this.errorHandler(new Exception("ID value must be between 1 and 1023"));
#endif
            }
        }



        private bool fPGARevisionValid = true;
        public bool FPGARevisionValid
        {
            get { return this.fPGARevisionValid; }
            set
            {
                this.fPGARevisionValid = value;
                if (value)
                    this.checkBoxTransmitterEnable.Checked = true;
                else
                    this.checkBoxTransmitterEnable.Checked = false;
            }
        }


        public bool GERelay
        {
            get => gERelay;
            set
            {
                gERelay = value;
            }
        }
        private bool gERelay = false;

        private bool forceDNPEnable = false;
        public bool ForceDNPEnable
        {
            get { return this.forceDNPEnable; }
            set
            {
                this.forceDNPEnable = value;

                // Do not force UI checked state here.
                // Only relay readback should set checkbox truth.
                if (value)
                {
                    this.buttonTX_Click(this, new EventArgs()); // send request/apply path
                }
            }
        }

        public bool CheckDNPEnable
        {
            get { return this.checkBoxDNPEnable.Checked; }
        }

        private void button_FastMode_Click(object sender, EventArgs e)
        {
            if (button_FastMode.Text == "Fast Mode Enabled")
            {
                button_FastMode.Text = "Fast Mode Disabled";
                button_FastMode.BackColor = Color.Transparent;
            }
            else if (button_FastMode.Text == "Fast Mode Disabled")
            {
                button_FastMode.Text = "Fast Mode Enabled";
                button_FastMode.BackColor = Color.Yellow;

                //set the 10 second config and send it to the relay
                this.radioButton10S.Checked = true;
                SendTransmitterSettings();

                // The sequence in which these timers are enabledhere matters - to follow up the next timer time out
                // so do not change this sequence
                this.timer_FireFastConfig.Enabled = true; // 3 minutes
                this.timer_FastMode.Enabled = true;       // 10 minutes
            }

        }

        private void timer_FastMode_Tick(object sender, EventArgs e)
        {
            // 10 minute timer for fast mode is finished
            // stop the 3 minute fast fire timer
            // get this fast mode button background color and text to default
            this.timer_FastMode.Enabled = false;
            this.timer_FireFastConfig.Enabled = false;
            this.button_FastMode.BackColor = Color.Transparent;
            this.button_FastMode.Text = "Fast Mode Disabled";
            MessageBox.Show("Disabling Fast Mode. 10 minute Time Out");
            MessageBox.Show("No more firing of fast config every 3 minutes");

            //After 10 minutes getting the config back to 60 second in the relay
            this.radioButton60S.Checked = true;
            SendTransmitterSettings();
        }

        private void timer_FireFastConfig_Tick(object sender, EventArgs e)
        {
            this.timer_FireFastConfig.Enabled = false;
            MessageBox.Show("3 minute Time Out. Another Fire of Fast config");
            this.radioButton10S.Checked = true;
            buttonForceConfigMessage_Click(this, new EventArgs());
            this.timer_FireFastConfig.Enabled = true; // restart the 3 minute timer
        }

        private void button_FastFire_Click(object sender, EventArgs e)
        {
            this.radioButton10S.Checked = true;
            SendTransmitterSettings();
            this.buttonForceConfigMessage_Click(this, new EventArgs());
        }

        private void btn_CTratioCal_Click(object sender, EventArgs e)
        {
            //  CTRatioCaculator CTCalculator = new CTRatioCaculator();
            //   CTCalculator.ShowDialog(this);
            clickbuttonCT.CTratioButton = true;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn_DNPsettings_defaults_Click(object sender, EventArgs e)
        {
            this.setDefaultDNPsettings();
        }

        private void SetDnpBaudByValue(int baud)
        {
            if (this.comboBox_DNPBaudRate == null) return;
            if (this.comboBox_DNPBaudRate.Items == null || this.comboBox_DNPBaudRate.Items.Count == 0) return;

            string target = baud.ToString();
            int idx = this.comboBox_DNPBaudRate.FindStringExact(target);

            if (idx < 0) idx = 0;
            if (idx >= this.comboBox_DNPBaudRate.Items.Count) idx = this.comboBox_DNPBaudRate.Items.Count - 1;

            this.comboBox_DNPBaudRate.SelectedIndex = idx;
        }

        private void setDefaultDNPsettings()
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

            SetDnpBaudByValue(is9600Customer ? 9600 : 19200);
        }

        public void buttonSendAllDNPSettings_Click(object sender, EventArgs e)
        {
#if DNP
            if (this.checkBoxDNPEnable.Checked && dnpUplinkK.dnpEnabledWithKit && applyTX.applyTxSettings)
            {
                applyDNP.applyDNPSettings = true;
            }
            else
            {

                MessageBox.Show(
                    "Please enable the DNP Uplink feature and click Apply in Transmission Commands before applying DNP settings.",
                    "DNP Uplink Required");
                applyDNP.applyDNPSettings = false;
                return;
            }

            //=====================Display throbber while parameters get requested from the master relay  =====================
            Application.UseWaitCursor = true; //keeps waitcursor even when the thread ends.
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor; //Normal mode of setting waitcursor
                                                                      //this.enableAll(false);
                                                                      //========================================================================================================

            this.SendAllDNPSettings();
#endif
        }
        private static int _DNPpacketLength = 98;
        private void SendAllDNPSettings()
        {
            // MessageBox.Show("send dnp settings to master processor"); // Only for testing - to be removed
            try
            {
                SendEventArgs sEA = new SendEventArgs(_DNPpacketLength);
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
                sEA.SendPacket[18] = (byte)this.comboBox_DNPBaudRate.SelectedIndex;

                sEA.SendPacket[sEA.SendPacket.Length - 1] = 0x0D;

                //uplinkC.uplinkCount += 1;

                this.Send(sEA);
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }
        }

        private void buttonRQDNPSettings_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("read dnp settings from master processor"); // Only for testing - to be removed
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

        private delegate void setAllCB(byte[] bytePacket);

        public void SetAll(byte[] bytePacket)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    setAllCB sACB = new setAllCB(this.setDNPsettings);
                    this.Invoke(sACB, bytePacket);
                }
                else
                {
                    this.setDNPsettings(bytePacket);
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }
        }

        private bool dNPCommStatus = false;
        public bool DNPCommLabelStatus
        {
            get { return this.dNPCommStatus; }
            set
            {
                this.dNPCommStatus = value;
                this.setDNPCommunicationStatus();
            }
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

        private void setDNPCommunicationStatus()
        {
            if (dNPCommStatus)
            {
                this.lbl_DNPCommStatus.Text = "Enabled";
                this.lbl_DNPCommStatus.BackColor = Color.SkyBlue;
            }
            else
            {
                this.lbl_DNPCommStatus.Text = "Disabled";
                this.lbl_DNPCommStatus.BackColor = Color.LightSalmon;
            }
        }

        private void setDNPsettings(byte[] bytePacket)
        {
#if DNP
            byte temp;

            try
            {
                temp = (byte)(bytePacket[0] & 0x03);
                switch (temp)
                {
                    case 0: this.comboBoxLinkLayerConfirm.SelectedItem = "Never"; break;
                    case 1: this.comboBoxLinkLayerConfirm.SelectedItem = "Sometimes"; break;
                    case 2: this.comboBoxLinkLayerConfirm.SelectedItem = "Always"; break;
                    default: throw new Exception("Bad Value For Link Layer");
                }
            }
            catch (Exception ex) { HandleDnpParseFailure("Error Setting Link Layer Confirm", ex); return; }

            try
            {
                temp = (byte)(bytePacket[0] & 0x04);
                this.comboBoxSelfAddress.SelectedItem = (temp == 0x04) ? "Enable" : "Disable";
            }
            catch (Exception ex) { HandleDnpParseFailure("Error Setting Self Address", ex); return; }

            try
            {
                temp = (byte)(bytePacket[0] & 0x08);
                this.comboBoxUnsolResponse.SelectedItem = (temp == 0x08) ? "Enable" : "Disable";
            }
            catch (Exception ex) { HandleDnpParseFailure("Error Setting Unsolicited Allowed", ex); return; }

            try
            {
                temp = (byte)(bytePacket[0] & 0x10);
                this.comboBoxTerminationResistor.SelectedItem = (temp == 0x10) ? "Enable" : "Disable";
            }
            catch (Exception ex) { HandleDnpParseFailure("Error Setting Resistor Termination", ex); return; }

            try
            {
                UInt32 tempInt = bytePacket[7];
                tempInt <<= 8; tempInt += bytePacket[6];
                tempInt <<= 8; tempInt += bytePacket[5];
                tempInt <<= 8; tempInt += bytePacket[4];
                this.numericUpDownUnsolTimeout.Value = tempInt;
            }
            catch (Exception ex) { HandleDnpParseFailure("Error Setting MSB unsoltimeout", ex); return; }

            try
            {
                UInt16 tempInt = bytePacket[9];
                tempInt <<= 8; tempInt += bytePacket[8];
                this.numericUpDownFragmentSize.Value = SnapFragmentSize(tempInt);
            }
            catch (Exception ex) { HandleDnpParseFailure("Error Setting Fragment Size", ex); return; }

            try
            {
                UInt16 tempInt = bytePacket[11];
                tempInt <<= 8; tempInt += bytePacket[10];
                this.numericUpDownDestinationAddress.Value = tempInt;
            }
            catch (Exception ex) { HandleDnpParseFailure("Error Setting Destination Address", ex); return; }

            try
            {
                UInt16 tempInt = bytePacket[13];
                tempInt <<= 8; tempInt += bytePacket[12];
                this.numericUpDownSourceAddress.Value = tempInt;
            }
            catch (Exception ex) { HandleDnpParseFailure("Error Setting Source Address", ex); return; }

            try
            {
                UInt16 tempInt = bytePacket[15];
                tempInt <<= 8; tempInt += bytePacket[14];
                this.numericUpDownUnsolRetries.Value = tempInt;
            }
            catch (Exception ex) { HandleDnpParseFailure("Error Setting Unsolicited Max Retries", ex); return; }

            try
            {
                this.numericUpDownMaxEvents.Value = bytePacket[16];
            }
            catch (Exception ex) { HandleDnpParseFailure("Error Setting Max Events", ex); return; }

            try
            {
                SetDnpBaudByIndexSafe(bytePacket[17]);
            }
            catch (Exception ex) { HandleDnpParseFailure("Error Setting Baud Rate", ex); return; }

            bool isRev10Plus = this.MasterRevisionNumber >= Constants.Rev10Master;

            // DNP bits in command byte
            bool selfAddrEnabled = (bytePacket[0] & 0x04) == 0x04;
            bool unsolEnabled = (bytePacket[0] & 0x08) == 0x08;

            // Comm label should reflect relay-reported DNP state
            //this.DNPCommLabelStatus = this.DNPEnabled;
#endif
        }

        private void SetDnpBaudByIndexSafe(int index)
        {
            if (this.comboBox_DNPBaudRate == null) return;
            int count = this.comboBox_DNPBaudRate.Items?.Count ?? 0;
            if (count <= 0) return;

            if (index < 0) index = 0;
            if (index >= count) index = count - 1;

            this.comboBox_DNPBaudRate.SelectedIndex = index;
        }

        private void NormalizeTxCommandButtonsLayout()
        {
            const int TX_BUTTON_X = 37; // was 18; bump right until it looks centered
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            const int TX_UPLINK_X = 35;
#pragma warning restore CS0219 // Variable is assigned but its value is never used


            // Ensure TX command buttons are inside the TX commands group
            if (this.buttonRestoreDefaults.Parent != this.grpBox_TXcommands)
                this.grpBox_TXcommands.Controls.Add(this.buttonRestoreDefaults);

            if (this.buttonTX.Parent != this.grpBox_TXcommands)
                this.grpBox_TXcommands.Controls.Add(this.buttonTX);

            if (this.buttonRQ.Parent != this.grpBox_TXcommands)
                this.grpBox_TXcommands.Controls.Add(this.buttonRQ);

            if (this.buttonForceConfigMessage.Parent != this.grpBox_TXcommands)
                this.grpBox_TXcommands.Controls.Add(this.buttonForceConfigMessage);


            // ConEd override: hide config-message button, show fast controls
            if (this.Customer == Customers.CONED)
            {
                // Ensure fast buttons are in TX commands group
                if (this.button_FastFire.Parent != this.grpBox_TXcommands)
                    this.grpBox_TXcommands.Controls.Add(this.button_FastFire);

                if (this.button_FastMode.Parent != this.grpBox_TXcommands)
                    this.grpBox_TXcommands.Controls.Add(this.button_FastMode);

                this.grpBox_TXcommands.Size = new Size(220, 594);

                int x = TX_BUTTON_X, w = 184, h = 62, gap = 16, y = 25;

                this.buttonRestoreDefaults.Location = new Point(x, y);
                this.buttonRestoreDefaults.Size = new Size(w, h); y += h + gap;

                this.buttonTX.Location = new Point(x, y);
                this.buttonTX.Size = new Size(w, h); y += h + gap;

                this.buttonRQ.Location = new Point(x, y);
                this.buttonRQ.Size = new Size(w, h); y += h + gap;

                this.button_FastFire.Location = new Point(x, y);
                this.button_FastFire.Size = new Size(w, h);
                this.button_FastFire.Visible = true;
                this.button_FastFire.Enabled = true;
                this.button_FastFire.Text = "Fast Fire";
                y += h + gap;

                this.button_FastMode.Location = new Point(x, y);
                this.button_FastMode.Size = new Size(w, h);
                this.button_FastMode.Visible = true;
                this.button_FastMode.Enabled = true;
                this.button_FastMode.Text = "Fast Mode Disabled";
                this.button_FastMode.BackColor = Color.Transparent;
                y += h + gap;

                this.buttonForceConfigMessage.Visible = false;
                this.buttonForceConfigMessage.Enabled = false;

                // Uplink block starts below buttons
                this.lbl_UplinkEn.Location = new Point(TX_UPLINK_X, y + 4);
                this.checkBoxDNPEnable.Location = new Point(TX_UPLINK_X, y + 56);

                this.buttonRestoreDefaults.BringToFront();
                this.buttonTX.BringToFront();
                this.buttonRQ.BringToFront();
                this.button_FastFire.BringToFront();
                this.button_FastMode.BringToFront();

                return;
            }



#if DNP
            // TX commands group sizing
            this.grpBox_TXcommands.Size = new Size(220, 594);

            // Uniform button sizes
            this.buttonRestoreDefaults.Size = new Size(184, 75);
            this.buttonTX.Size = new Size(184, 75);
            this.buttonRQ.Size = new Size(184, 75);
            this.buttonForceConfigMessage.Size = new Size(184, 75);

            // Fixed legacy placement
            this.buttonRestoreDefaults.Location = new Point(TX_BUTTON_X, 25);
            this.buttonTX.Location = new Point(TX_BUTTON_X, 145);
            this.buttonRQ.Location = new Point(TX_BUTTON_X, 265);
            this.buttonForceConfigMessage.Location = new Point(TX_BUTTON_X, 385);
#else
    // Original/non-DNP sizing
    this.grpBox_TXcommands.Size = new Size(220, 594);

    this.buttonRestoreDefaults.Size = new Size(184, 75);
    this.buttonTX.Size = new Size(184, 75);
    this.buttonRQ.Size = new Size(184, 75);
    this.buttonForceConfigMessage.Size = new Size(184, 75);

    this.buttonRestoreDefaults.Location = new Point(TX_BUTTON_X, 60);
    this.buttonTX.Location = new Point(TX_BUTTON_X, 210);
    this.buttonRQ.Location = new Point(TX_BUTTON_X, 360);
    this.buttonForceConfigMessage.Location = new Point(TX_BUTTON_X, 510);
#endif

            this.buttonRestoreDefaults.BringToFront();
            this.buttonTX.BringToFront();
            this.buttonRQ.BringToFront();
            this.buttonForceConfigMessage.BringToFront();
        }
        private void NormalizeDnpUplinkPlacement()
        {
#if DNP
            const int TX_UPLINK_X = 30;
            const int TX_UPLINK_Y = 496;

            if (this.lbl_UplinkEn.Parent != this.grpBox_TXcommands)
                this.grpBox_TXcommands.Controls.Add(this.lbl_UplinkEn);

            if (this.checkBoxDNPEnable.Parent != this.grpBox_TXcommands)
                this.grpBox_TXcommands.Controls.Add(this.checkBoxDNPEnable);

            this.checkBoxDNPEnable.AutoSize = true;
            this.checkBoxDNPEnable.Text = "";
            this.checkBoxDNPEnable.Location = new Point(TX_UPLINK_X, TX_UPLINK_Y + 5);
            this.checkBoxDNPEnable.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.checkBoxDNPEnable.Visible = true;

            this.lbl_UplinkEn.Visible = true;
            this.lbl_UplinkEn.AutoSize = false;
            this.lbl_UplinkEn.Location = new Point(TX_UPLINK_X + 18, TX_UPLINK_Y);
            this.lbl_UplinkEn.Size = new Size(185, 120);
            this.lbl_UplinkEn.Text =
                "Enable\r\n" +
                "DNP Uplink Feature.\r\n" +
                "(DIGITALGRID DNP\r\n" +
                "Uplink kit required for\r\n" +
                "wireless/fiber locations)";
            this.lbl_UplinkEn.TextAlign = ContentAlignment.TopLeft;

            this.checkBoxDNPEnable.BringToFront();
            this.lbl_UplinkEn.BringToFront();

            this.checkBoxDNPEnable.Invalidate();
            this.lbl_UplinkEn.Invalidate();
            this.grpBox_TXcommands.PerformLayout();
            this.grpBox_TXcommands.Refresh();
#endif
        }
        private void ucTransmitter_Load(object sender, EventArgs e)
        {
            // Run again after all parent/container layout passes
            NormalizeTxCommandButtonsLayout();
            NormalizeDnpUplinkPlacement();
            NormalizeTxButtonTextAlignment();
        }
        private void HandleDnpParseFailure(string context, Exception ex)
        {
            MessageBox.Show(
                context + Environment.NewLine + ex.Message,
                "DNP Parse Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        private void NormalizeTxButtonTextAlignment()
        {
            this.buttonRestoreDefaults.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonTX.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonRQ.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonForceConfigMessage.TextAlign = ContentAlignment.MiddleCenter;
            this.button_FastFire.TextAlign = ContentAlignment.MiddleCenter;
            this.button_FastMode.TextAlign = ContentAlignment.MiddleCenter;
        }
    }

}