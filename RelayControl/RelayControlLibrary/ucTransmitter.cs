using NLog;
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

          //  this.textBoxTransmitterOutputPower.Text = powerP.pwrPer.ToString();//"abcd";
          //  this.textBoxTransmitterOutputPower.Enabled = false;
            this.DNPCoverFlags = ((byte)(0));
#if DEBUG
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

#if DOMINION || MADISON || BGE
            this.panelFlasgStatusWB.Visible = false;// true;
            this.labelTransFlagStatus.Visible = false;// true;
#elif CONED
            this.panelFlasgStatusWB.Visible = false;
            this.labelTransFlagStatus.Visible = false;
            this.button_FastFire.Enabled = true;
            this.button_FastFire.Visible = true;
            this.button_FastMode.Enabled = true;
            this.button_FastMode.Visible = true;
            this.labelTransFlagStatus.Visible = false;
#elif !DEBUG
            this.panelFlasgStatusWB.Visible = false;
            this.labelTransFlagStatus.Visible = false;
#endif

#if !DEBUG
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

            this.button_FastFire.Location = new System.Drawing.Point(1017, 400); //(780,390);
            this.button_FastFire.Size = new System.Drawing.Size(184, 75);
            this.button_FastMode.Location = new System.Drawing.Point(1017, 490); //(780, 470);
            this.button_FastMode.Size = new System.Drawing.Size(184, 75);
            this.button_FastMode.Text = "Fast Mode Disabled";

            this.checkBoxDNPEnable.Location = new System.Drawing.Point(1017, 585); //(780, 580); 

            this.panelGeneralSettings.Size = new System.Drawing.Size(370, 600); 
            this.panelFlagSettings.Size = new System.Drawing.Size(250, 600);
            this.grpBox_TXcommands.Location = new System.Drawing.Point(1000, 10); //(760,10);
            this.grpBox_TXcommands.Size = new System.Drawing.Size(220, 610);

            this.panelFlagSettings.Size = new System.Drawing.Size(221, 600);
            this.panelFlagSettings.Location = new System.Drawing.Point(600, 18); //(470, 18);

            this.panelFlagSettingH.Location = new System.Drawing.Point(42, 550);
            this.panelFlagSettingH.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPHClose.Location = new System.Drawing.Point(3, 2); //(85, 2);
            this.label12.Location = new System.Drawing.Point(19, 553);

            this.panelFlagSettingG.Location = new System.Drawing.Point(42, 480);
            this.panelFlagSettingG.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPGClose.Location = new System.Drawing.Point(3, 2); //(85, 2);
            this.label11.Location = new System.Drawing.Point(19, 483);

            this.panelFlagSettingF.Location = new System.Drawing.Point(42, 410);
            this.panelFlagSettingF.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPFClose.Location = new System.Drawing.Point(3, 2); //(85, 2);
            this.label10.Location = new System.Drawing.Point(19, 413);

            this.panelFlagSettingE.Location = new System.Drawing.Point(42, 340);
            this.panelFlagSettingE.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPEClose.Location = new System.Drawing.Point(3, 2); //(85, 2);
            this.label9.Location = new System.Drawing.Point(19, 343);

            this.panelFlagSettingD.Location = new System.Drawing.Point(42, 270);
            this.panelFlagSettingD.Size = new System.Drawing.Size(178, 30);
            this.radioButtonFPDClose.Location = new System.Drawing.Point(3, 2); //(85, 2);
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
                    this.dNPEnabled = value;
                    this.checkBoxDNPEnable.Checked = value;
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

        private int serialNumber = 0;
        private bool waterBugNoTransmitter = false;
        private SendEventArgs RQSEA = new SendEventArgs(3);
        private SendEventArgs TXSEA = new SendEventArgs(31);
        private int packetLength = 30;

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
                if ((bA[28] & 0x04) == 0x04)
                {
                    this.DNPEnabled = true;
                }
                else
                {
                    this.DNPEnabled = false;
                }
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
#if DEBUG
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
                this.checkBoxDNPEnable.Checked = true;
                this.buttonTX_Click(this, new EventArgs());
            }

        }

        private void showWaterbugNoTransmitter(bool value)
        {
#if !DEBUG
            if (value)
            {
                this.panelGeneralSettings.Hide();
                this.panelMessageFreqSettings.Hide();
                this.panelOtherAlarmSettings.Hide();
                this.panelFreqPanel.Hide();
                this.panel2.Hide();
                this.labelGeneralSettings.Hide();
                this.labelOtherAlarmSettings.Hide();
                this.buttonRestoreDefaults.Hide();
            }
            else
            {
                this.panelGeneralSettings.Show();

                //#if (CHICAGO || ENMAX || SEATTLE || BOSTON || NU || MADISON || LONDONH || TAUNTON || BGE) && !DEBUG
#if (CHICAGO || SEATTLE || BOSTON || NU || MADISON || LONDONH || TAUNTON || BGE) && !DEBUG
                this.panelOtherAlarmSettings.Hide();
                this.labelOtherAlarmSettings.Hide();
                this.panelSmartExternalCable.Hide();
                labelSmartExternalCable.Hide();

                this.panelFlasgStatusWB.Location = new Point(260, 300);
                this.labelTransFlagStatus.Location = new Point(267, 294);

                this.buttonTX.Location = new Point(460, 6); //(260, 6);
                this.buttonRQ.Location = new Point(260, 93);
                this.buttonForceConfigMessage.Location = new Point(260, 180); //13, 250
                this.buttonRestoreDefaults.Location = new Point(260, 235);

                this.buttonRQ.Size = new Size(110, 74);
                this.buttonForceConfigMessage.Size = new Size(110, 44);
                this.buttonRestoreDefaults.Size = new Size(110, 44);

                this.panelSmartExternalCable.Location = new Point(7, 250);
                this.panelSmartExternalCable.Size = new Size(242, 39);

                this.labelSmartExternalCable.Location = new Point(16, 243);
                this.checkBoxSmartExternalCableEnable.Location = new Point(63, 12);

#else
                //this.panelOtherAlarmSettings.Show();
                //this.labelOtherAlarmSettings.Show();
                this.panelAlarmSettings.Hide();
                this.labelAlarmSettings.Hide();

                this.buttonTX.Location = new Point(1019, 40); //(1190, 40); //(780, 40);
                this.buttonRQ.Location = new Point(1019, 130); //(1200, 130); //(780, 130); 
                this.buttonForceConfigMessage.Location = new Point(1019, 220); //(1200, 220); //(780, 220);  
                this.buttonRestoreDefaults.Location = new Point(1019, 310); //(1200, 310); //(780, 310); 

                this.buttonRQ.Size = new Size(184, 75);
                this.buttonForceConfigMessage.Size = new Size(184, 75);
                this.buttonRestoreDefaults.Size = new Size(184, 75);
                // this.panelFlagSettings.Size = new Size(242, 217);
#endif
                this.panelFreqPanel.Show();
                this.panel2.Show();
                this.labelGeneralSettings.Show();
                this.labelGeneralSettings.BringToFront();
                this.labelOtherAlarmSettings.BringToFront();
                this.buttonRestoreDefaults.Show();
            }

#endif
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
            byte[] packet = new byte[3];

            packet[0] = (byte)'X';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.RQSEA.SendPacket = packet;

            OnSend(RQSEA);
        }

        private void buttonForceConfigMessage_Click(object sender, EventArgs e)
        {
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
                dR = new YesNoMessageBoxResized("DNP Uplink", "Have you installed the 'DNP Uplink Kit'?", "Yes", "No").ShowDialog();

                if (dR == DialogResult.Yes)
                {
                    dnpUplinkK.dnpEnabledWithKit = true;
                    //this.ucDNP2.buttonSendAllDNPSettings_Click(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Please ensure the 'DNP Uplink Kit' is installed before activating the 'DNP Uplink' feature. Activating this feature without the required kit will disable communication with the Relay Control and monitoring Application", "Kit Required");
                    dnpUplinkK.dnpEnabledWithKit = false;
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
            this.SendTransmitterSettings();
        }

        public void SendTransmitterSettings()
        {
            string errorMessage = "";
            try
            {
                errorMessage = "Bad ID value";
                this.tempID = Convert.ToUInt16(this.textBoxID.Text);
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
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength | (byte)0x04);
                else
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength & (byte)0xFB);

                if (this.checkBoxTransmitterEnable.Checked)
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength | (byte)0x08);
                else
                    this.TXSettings.Type1MessageLength = (byte)(this.TXSettings.Type1MessageLength & (byte)0xF7);

                //External Data/ Data from Waterbury
                errorMessage = "Waterbug Error";
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
#if DEBUG
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
            this.TXSettings.FlagPolarity.G = Convert.ToBoolean(dNPCoverFlags & 62);
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
                cB.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Bold);
            }
            else
            {
                cB.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Strikeout);
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
                if (this.Customer == Customers.Memphis)
                    this.setMemphisDefaults();
                else if (this.GERelay)
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

        private void setMemphisDefaults()
        {
            this.textBoxID.Text = "1023";
            this.textBoxTXCTRatio.Text = "120";

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

            this.checkBoxSmartExternalCableEnable.Checked = false;
            this.checkBoxWBAn1.Checked = false;
            this.checkBoxWBAn2.Checked = false;
            this.checkBoxWBC.Checked = false;
            this.checkBoxWBD.Checked = false;
            this.checkBoxWBE.Checked = false;
            this.checkBoxWBF.Checked = false;
            this.checkBoxWBG.Checked = false;
            this.checkBoxWBH.Checked = false;

            this.enableWaterbury(false);

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

            this.checkBoxSmartExternalCableEnable.Checked = false;
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
#if PSEG || NU || CONED
            this.CTRatio = 320;
#else
            this.CTRatio = 600;
#endif

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

            //this.enableWaterbury(true);

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

        private void enableWaterbury(bool p) // SEC
        {
            //#if (CHICAGO || ENMAX || DOMINION || SEATTLE || BOSTON || NU || MADISON || PSEG || LONDONH || TAUNTON || BGE) && !DEBUG
#if (CHICAGO || ENMAX || DOMINION || SEATTLE || BOSTON || NU || MADISON || PSEG || LONDONH || TAUNTON || BGE || ONCOR) && !DEBUG
            this.panelWaterburyMain.Visible = false;
            this.labelSmartExternalCableMain.Visible = false;
            this.panelAlarmSettings.Visible = false;
            this.labelAlarmSettings.Visible = false;
#else
            this.panelWaterburyMain.Visible = p;
            this.labelSmartExternalCableMain.Visible = p;
            this.panelAlarmSettings.Visible = true;
            this.labelAlarmSettings.Visible = true;
#endif
            this.labelSmartExternalCableMain.BringToFront();

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

                if (temp > 1023)
                {
                    this.textBoxID.Text = "1023";
                    throw new Exception();
                }
            }
            catch
            {
                this.errorHandler(new Exception("ID value must be between 1 and 1023"));
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
                if (value)
                {
                    this.DNPEnabled = this.checkBoxDNPEnable.Checked = true;
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
    }
}
