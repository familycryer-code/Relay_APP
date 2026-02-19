using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.IO;
using SharedResources;
using System.Threading;
using System.Reflection;
using GraphicsServer.GSNet.Charting;
using System.Linq;

namespace RelayControlLibrary
{

    public partial class ucTripMode : UserControl
    {
        public ucTripMode()
        {
            InitializeComponent();
            this.listBoxTripModes.SelectedIndex = 0;
            //this.domainUpDownType.SelectedIndex = 0;
            this.comboBox_TripType.SelectedIndex = 0;
#if NU
            this.checkBoxEnableGullWing.Checked = true;
            this.showGullWing(true);
#else
            this.checkBoxEnableGullWing.Checked = false;
            this.showGullWing(false);
#endif

            //this.domainUpDownTripStyle.SelectedItem = "Hold Trip";
            //this.domainUpDownTripStyle.SelectedItem = "Hold Trip (Troubleshooting Only)";
            this.comboBox_TripStyle.SelectedItem = "Hold Trip (Troubleshooting Only)";
            //this.domainUpDownTripStyle.Hide();
            this.comboBox_TripStyle.Hide();
            this.labelTripStyle.Hide();
            this.initializeToolTip();

            this.restoreDefaults();

#if NUCREW
            this.numericUpDownAngle.Enabled = false;
            this.numericUpDownGullWingAngle.Enabled = false;
            this.numericUpDownSensTrip.Enabled = true;
            this.numericUpDownTimeDelay.Enabled = false;
            this.checkBoxEnableGullWing.Enabled = false;
#endif

#if CONED && !Debug
            this.Customer = Customers.ConEdison;
#else
            this.Customer = Customers.NonConEd;
#endif

        }
        public enum eDisplayType
        {
            Relay,
            Percent,
            Protector
        }

        private bool sequenceRelay = false;
        public bool SequenceRelay
        {
            get { return this.sequenceRelay; }
            set
            {
                this.sequenceRelay = value;
                this.sequenceStyleWattVar();
            }
        }

        private void initializeToolTip()
        {
            this.toolTip.SetToolTip(this.numericUpDownExtendedTimeDelay, "This Time Delay extends the Sensitive Time Delay\r\nin the Sensitive Trip Region");
            this.toolTip.SetToolTip(this.numericUpDownInsensTrip, "Amount of Current needed, on at least one phase, in Insensitive or Time Delay Trip to enter the Sensitive Trip Region.");
            this.toolTip.SetToolTip(this.numericUpDownSensitiveTimeDelay, "Time Delay for the Sensitive Trip Region");
            this.toolTip.SetToolTip(this.numericUpDownSensTrip, "Reverse Current needed at 180 degrees to enter Sensitive Trip Region");
            this.toolTip.SetToolTip(this.numericUpDownTimeDelay, "Time Delay for Time Delay region on Time Delay trip");
            this.toolTip.SetToolTip(this.numericUpDownWVAngle, "Number of degrees to rotate the Sensitive Trip Curve when Watt-Var Current has been exceeded");
            this.toolTip.SetToolTip(this.numericUpDownWVCurrent, "Amount of Current needed to trigger Watt-Var tripping characteristics");
            this.toolTip.SetToolTip(this.listBoxTripModes, "Select trip algorithm");
            //this.toolTip.SetToolTip(this.domainUpDownTripStyle, "Determines what relay does after the 3 trip pulses and the Trip Condition still exists");
            this.toolTip.SetToolTip(this.comboBox_TripStyle, "Determines what relay does after the 3 trip pulses and the Trip Condition still exists");
            //this.toolTip.SetToolTip(this.domainUpDownType, "Determines how the values are viewed in the GUI for the Trip Settings");
            this.toolTip.SetToolTip(this.comboBox_TripType, "Determines how the values are viewed in the GUI for the Trip Settings");
            this.toolTip.SetToolTip(this.checkBoxEnableGullWing, "Enables the Trim Curve");
            this.toolTip.SetToolTip(this.checkBoxTripOnPowerDown, "Relay will attempt to Trip as it is losing power");

        }

        private void setWattVarToolTip()
        {
            this.setSensitiveToolTip();
        }

        private void setTimeDelayToolTip()
        {
            this.toolTip.SetToolTip(this.numericUpDownSensTrip, "Reverse Current needed at 180 degrees to enter Time Delay Region");
        }

        private void setSensitiveToolTip()
        {
            this.toolTip.SetToolTip(this.numericUpDownSensTrip, "Reverse Current needed at 180 degrees to enter Sensitive Trip Region");
        }

        private void setInsensitiveToolTip()
        {
            this.toolTip.SetToolTip(this.numericUpDownSensTrip, "Reverse Current needed at 180 degrees to enter Insensitive Trip Region");
        }


        private ToolTip toolTip = new ToolTip();
        private Customers customer = Customers.None;

        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                this.customer = value;
                this.setCustomer();
            }
        }

        private void setCustomer()
        {
            switch (this.customer)
            {
                case Customers.None:
                case Customers.NonConEd:
                case Customers.Memphis:
                case Customers.NonConEdGE:
                case Customers.DIGITALGRIDDNP:
                case Customers.DIGITALGRID:
                case Customers.DNPwithPLC:
                case Customers.SMUD:
                case Customers.Atlanta:
                case Customers.Oncor:
                case Customers.LondonH:
                case Customers.SCE:
                case Customers.TorontoHydro:
                    this.makeNonConEd();
                    break;
                case Customers.ConEdison:
                    this.makeConEd();
                    break;
                default:
                    this.errorHandler(new Exception("Bad Customer Setting In Trip Mode Control"));
                    break;
            }
        }
        /*
        string[] conEdTripModes = new string[] {
            "Sensitive",
            "Insensitive",
            "Time Delay",
            "Adaptive"};

        string[] nonConEdTripModes = new string[] {
            "Sensitive",
            "Insensitive",
            "Time Delay",
            "Watt-Var",
            "Adaptive"};
       */
        /* string[] conEdTripModes = new string[] {
             "Sensitive",
             "Insensitive",
             "Time Delay"};

         string[] nonConEdTripModes = new string[] {
             "Sensitive",
             "Insensitive",
             "Time Delay",
             "Watt-Var"};
        */

        string[] conEdTripModes = new string[] {
            "Sensitive",
            "Time Delay",
            "Insensitive",
            "Adaptive"};

        string[] nonConEdTripModes = new string[] {
            "Sensitive",
            "Time Delay",
            "Insensitive",
            "Watt-Var",
            "Adaptive"};

        private void makeConEd()
        {
            int savedSelectedIndex = this.listBoxTripModes.SelectedIndex;

            this.listBoxTripModes.Items.Clear();
            this.listBoxTripModes.Items.AddRange(this.conEdTripModes);
            try
            {
                this.listBoxTripModes.SelectedIndex = savedSelectedIndex;
            }
            catch
            {
                this.listBoxTripModes.SelectedIndex = 0;
            }
            //this.domainUpDownTripStyle.Visible = false;
            this.comboBox_TripStyle.Visible = false;
            this.labelTripStyle.Visible = false;
            this.labelGullWingAngle.Visible = false;
            this.checkBoxEnableGullWing.Visible = false;
            this.labelGullWingUnits.Visible = false;
            this.numericUpDownGullWingAngle.Visible = false;
            this.checkBoxTripOnPowerDown.Visible = false;

            //this.domainUpDownType.Visible = false;
            this.comboBox_TripType.Visible = false;
        }

        private void makeNonConEd()
        {
            int savedSelectedIndex = this.listBoxTripModes.SelectedIndex;
            this.listBoxTripModes.Items.Clear();
            this.listBoxTripModes.Items.AddRange(this.nonConEdTripModes);
            try
            {
                this.listBoxTripModes.SelectedIndex = savedSelectedIndex;
            }
            catch
            {
                this.listBoxTripModes.SelectedIndex = 0;
            }
            // this.domainUpDownTripStyle.Visible = true;
            this.comboBox_TripStyle.Visible = true;
            this.labelTripStyle.Visible = true;
            this.checkBoxEnableGullWing.Visible = true;
            this.checkBoxTripOnPowerDown.Visible = true;
            //this.domainUpDownType.Visible = true;
            this.comboBox_TripType.Visible = true;
        }

        private void hideSensitiveTimeDelay()
        {
            this.numericUpDownSensitiveTimeDelay.Visible = false;
            this.labelSTD.Visible = false;
            this.labelSTDunit.Visible = false;
        }


        private eDisplayType displayType = eDisplayType.Relay;

        private const byte _packetSize = 7;

        public delegate void SendEventHandler(object o, SendEventArgs sEA);
        public event SendEventHandler Send;
        private SendEventArgs mySEA = new SendEventArgs(_packetSize);

        public delegate void TripModeEventHandler(TripModeChangeEventArgs tMCEA);
        public event TripModeEventHandler TripModeChanged;
        private TripModeChangeEventArgs myTMCEA = new TripModeChangeEventArgs();

        public TripModeDefinition TripModeDef = new TripModeDefinition(TripModes.Sensitive);
        public TripCurveDefinition TripCurve1 = new TripCurveDefinition(TripCurveTypes.OffsetAngle);
        public TripCurveDefinition TripCurveTimeDelay = new TripCurveDefinition(TripCurveTypes.Magnitude);
        public TripCurveDefinition TripCurveInsensTripMag = new TripCurveDefinition(TripCurveTypes.Magnitude);
        public TripCurveDefinition TripCurveWV = new TripCurveDefinition(TripCurveTypes.OffsetAngle);
        public TripCurveDefinition TripCurveGW = new TripCurveDefinition(TripCurveTypes.OffsetAngle);

        public TripCurveDefinition AdaptiveMag_X = new TripCurveDefinition(TripCurveTypes.OffsetAngle);
        public TripCurveDefinition AdaptiveMag_Y = new TripCurveDefinition(TripCurveTypes.OffsetAngle);
        public TripCurveDefinition AdaptiveKVA_Curve = new TripCurveDefinition(TripCurveTypes.Magnitude);
        public bool SendTimedOut = false;

        private uint versionNumber = 0;
        public uint VersionNumber
        {
            get { return this.versionNumber; }
            set
            {
                this.versionNumber = value;

                if (this.versionNumber >= 110609 && this.Customer != Customers.ConEdison)
                {
                    this.labelTripStyle.Visible = true;
                    // this.domainUpDownTripStyle.Visible = true;
                    this.comboBox_TripStyle.Visible = true;
                    this.checkBoxTripOnPowerDown.Visible = true;
                }
                else
                {
                    this.labelTripStyle.Hide();
                    // this.domainUpDownTripStyle.Hide();
                    this.comboBox_TripStyle.Hide();
                    this.checkBoxTripOnPowerDown.Hide();
                }
            }
        }

        private bool sensitiveTimeEnabled = true;
        private Int32 cTRatio = 320;
        public Int32 CTRatio
        {
            get { return this.cTRatio; }
            set
            {
                this.setProtectorValues(value);
                this.cTRatio = value;
            }
        }


        private bool sending = false;
        public void buttonSendTripMode_Click(object sender, EventArgs e)
        {
            var choice = DialogResult.Cancel;
            decimal temp_AT = 0;
            byte[] adaptiveTrip_package = new byte[12];

            if (sending)
                return;
            if (sendAllF.SendAllFlag == false)
            {
                choice = DialogResult.OK;// MessageBox.Show("Sending Trip Mode Parameters as set in the APP to the Relay", "Send?", MessageBoxButtons.OKCancel);
            }
            if ((choice == DialogResult.OK) || (sendAllF.SendAllFlag == true))
            {
                //if (this.listBoxTripModes.SelectedIndex != 4) // For Trip Modes other than Adaptive trip mode
                //{
                Application.UseWaitCursor = true; //keeps waitcursor even when the thread ends.
                Cursor.Current = Cursors.WaitCursor;
                screenD.screenDisable = true;
                this.SendTimedOut = false;
                sending = true;
                mySEA.WithAck = true;
                mySEA.RequestAll = false;

                this.TripModeDef.Mode = RelayModeFunctions.TripModeFrom(this.listBoxTripModes.Text);
                TripModeDef.SensitiveTimeDelay = (int)this.numericUpDownSensitiveTimeDelay.Value;
                TripModeDef.ExtendedDelay = (int)this.numericUpDownExtendedTimeDelay.Value;
                if (this.TripModeDef.Mode == TripModes.TimeDelay || this.TripModeDef.Mode == TripModes.WattVar)
                    TripModeDef.TimeDelay = (int)this.numericUpDownTimeDelay.Value;
                else
                    TripModeDef.TimeDelay = 0;

                if (this.TripModeDef.Mode == TripModes.RemoteTrip)
                {
                    mySEA.SendPacket = RelayModeFunctions.BytePacketFor(TripModeDef);
                    OnSend(mySEA);
                    return;
                }

                TripCurve1.CurveNumber = 0;
                TripCurve1.CurveType = TripCurveTypes.OffsetAngle;

                TripCurveGW.CurveNumber = 1;

                if (this.checkBoxEnableGullWing.Checked)
                {

                    TripCurveGW.CurveType = TripCurveTypes.OffsetAngle;
                    TripCurveGW.CodomainMaximum = Constants.MaxFixedPointValue;
                    TripCurveGW.CodomainMinimum = 0;
                    TripCurve1.CodomainMaximum = 0;
                    TripCurve1.CodomainMinimum = Constants.MinFixedPointValue;
                }
                else
                {
                    TripCurveGW.CurveType = TripCurveTypes.NoCurve;
                    TripCurveGW.CodomainMaximum = Constants.MaxFixedPointValue;
                    TripCurveGW.CodomainMinimum = 0;
                    TripCurve1.CodomainMaximum = Constants.MaxFixedPointValue;
                    TripCurve1.CodomainMinimum = Constants.MinFixedPointValue;
                }

                if (this.displayType == eDisplayType.Relay)
                {
                    TripCurve1.Offset = this.numericUpDownSensTrip.Value;
                    TripCurveGW.Offset = this.numericUpDownSensTrip.Value;
                    TripCurveWV.Offset = this.numericUpDownSensTrip.Value;
                }
                else if (this.displayType == eDisplayType.Percent)
                {
                    TripCurve1.Offset = this.numericUpDownSensTrip.Value * 50m;
                    TripCurveGW.Offset = this.numericUpDownSensTrip.Value * 50m;
                    TripCurveWV.Offset = this.numericUpDownSensTrip.Value * 50m;
                }
                else
                {
                    TripCurve1.Offset = this.numericUpDownSensTrip.Value * 1000m / CTRatio;
                    TripCurveGW.Offset = this.numericUpDownSensTrip.Value * 1000m / CTRatio;
                    TripCurveWV.Offset = this.numericUpDownSensTrip.Value * 1000m / CTRatio;
                }

                TripCurve1.Tilt = this.numericUpDownAngle.Value;
                TripCurveGW.Tilt = this.numericUpDownGullWingAngle.Value;

                mySEA.SendPacket = RelayModeFunctions.BytePacketFor(TripCurve1, 0); // To be saved in master uP as T0_byte
                Thread.Sleep(1000);   // 1 second delay
                OnSend(mySEA);

                mySEA.SendPacket = RelayModeFunctions.BytePacketFor(TripCurveGW, 1);
                /* if (dataBackupR.dataBackup_fromRelay == true)
                    {
                        // writes to 12 bytes T1_byte1 to T1_byte12 in master uP
                        // these 12 bytes correspond to the byte packet refering to APP contents as seen on line 381-394 in RelayModeFunctions.cs
                        string lineRead;
                        StreamReader sr = new StreamReader("C:\\DGI Systems\\Relay\\Saved Data\\test_fileRead.txt");
                        int c = 1;
                        while (c <= 32)
                        {// skip through first 32 data bytes
                            lineRead = sr.ReadLine(); //Read the next line
                            c++;
                        }

                        mySEA.SendPacket[0] = 84;  // 'T'
                        //mySEA.SendPacket[1] = 49;   // '1'
                        for (int cnt = 1; cnt <= 12; cnt++)
                        {
                            lineRead = sr.ReadLine(); //Read the next line
                            if ((cnt % 2) != 0)//odd 
                                mySEA.SendPacket[cnt] = Convert.ToByte(lineRead);
                            else
                                mySEA.SendPacket[cnt - 2] = Convert.ToByte(lineRead);


                        }
                        mySEA.SendPacket[13] = 0x0D;

                        dataBackupR.dataBackup_fromRelay = false;
                        sr.Close();

                    }
                */
                this.OnSend(mySEA);

                decimal tempDecimal;

                if (this.displayType == eDisplayType.Relay)
                    tempDecimal = this.numericUpDownInsensTrip.Value;
                else if (this.displayType == eDisplayType.Percent)
                    tempDecimal = this.numericUpDownInsensTrip.Value * .050m;
                else
                    tempDecimal = this.numericUpDownInsensTrip.Value / CTRatio;

                if (this.numericUpDownTimeDelay.Visible)
                {
                    this.instantaneousCurrent = tempDecimal;
                }
                else
                {
                    this.insensitiveCurrent = tempDecimal;
                }

                TripCurveTimeDelay.CurveNumber = 3;
                TripCurveTimeDelay.CurveType = TripCurveTypes.Magnitude;
                TripCurveTimeDelay.CodomainMaximum = Constants.MaxFixedPointValue;
                TripCurveTimeDelay.CodomainMinimum = Constants.MinFixedPointValue;
                TripCurveTimeDelay.Offset = 0;
                TripCurveTimeDelay.Tilt = 90;
                TripCurveTimeDelay.Magnitude = this.instantaneousCurrent;

                if (!this.numericUpDownTimeDelay.Visible)
                {
                    TripCurveTimeDelay.CurveType = TripCurveTypes.NoCurve;

                }
                else
                {
                    TripCurveTimeDelay.CurveType = TripCurveTypes.Magnitude;
                }
                mySEA.SendPacket = RelayModeFunctions.BytePacketFor(TripCurveTimeDelay, 3);
                OnSend(mySEA);

                this.TripCurveInsensTripMag.CurveNumber = 2;
                this.TripCurveInsensTripMag.CurveType = TripCurveTypes.Magnitude;
                this.TripCurveInsensTripMag.CodomainMaximum = Constants.MaxFixedPointValue;
                this.TripCurveInsensTripMag.CodomainMinimum = Constants.MinFixedPointValue;
                this.TripCurveInsensTripMag.Offset = 0;
                this.TripCurveInsensTripMag.Tilt = 90;
                this.TripCurveInsensTripMag.Magnitude = this.insensitiveCurrent;

                if (!this.labelInsensTrip.Visible)
                {
                    this.TripCurveInsensTripMag.CurveType = TripCurveTypes.NoCurve;
                }
                else
                {
                    this.TripCurveInsensTripMag.CurveType = TripCurveTypes.Magnitude;
                }
                mySEA.SendPacket = RelayModeFunctions.BytePacketFor(this.TripCurveInsensTripMag, 2);
                OnSend(mySEA);


                TripCurveWV.CurveNumber = 4;
                TripCurveWV.CurveType = TripCurveTypes.WattVar;
                //Offset Set Above
                TripCurveWV.CodomainMaximum = Constants.MaxFixedPointValue;
                TripCurveWV.CodomainMinimum = Constants.MinFixedPointValue;
                TripCurveWV.Tilt = this.numericUpDownAngle.Value + this.numericUpDownWVAngle.Value;

                if (this.displayType == eDisplayType.Relay)
                    TripCurveWV.Magnitude = this.numericUpDownWVCurrent.Value;
                else if (this.displayType == eDisplayType.Percent)
                    TripCurveWV.Magnitude = this.numericUpDownWVCurrent.Value * .050m;
                else
                    TripCurveWV.Magnitude = this.numericUpDownWVCurrent.Value / CTRatio;

                mySEA.SendPacket = RelayModeFunctions.BytePacketFor(TripCurveWV, 4);

                if (this.numericUpDownWVCurrent.Visible)
                    this.TripCurveWV.CurveType = TripCurveTypes.WattVar;
                else
                    this.TripCurveWV.CurveType = TripCurveTypes.NoCurve;

                OnSend(mySEA);

                adaptiveTrip_package[0] = (byte)'{'; //Adaptive trip command
                adaptiveTrip_package[1] = (byte)((int)this.numericUpDown_GreenDelay.Value >> 8);                   // high byte of GreenDelay
                adaptiveTrip_package[2] = (byte)(0x00FF & (int)this.numericUpDown_GreenDelay.Value);               // low byte of GreenDelay
                decimal tempKW, tempkVA, temp_AdaptiveMag_X, temp_AdaptiveMag_Y = 0;
                if (this.displayType == eDisplayType.Relay)
                {
                    temp_AdaptiveMag_X = (this.numericUpDown_GreenMagX.Value);
                    temp_AdaptiveMag_Y = (this.numericUpDown_GreenMagY.Value);
                    tempKW = (-this.numericUpDown_InCurrkW.Value);
                    tempkVA = (this.numericUpDown_InCurrkVAR.Value);
                }

                else if (this.displayType == eDisplayType.Percent)
                {
                    temp_AdaptiveMag_X = (this.numericUpDown_GreenMagX.Value * 50m);
                    temp_AdaptiveMag_Y = (this.numericUpDown_GreenMagY.Value * 50m);
                    tempKW = (-this.numericUpDown_InCurrkW.Value * .050m);
                    tempkVA = (this.numericUpDown_InCurrkVAR.Value * .050m);
                }

                else
                {
                    temp_AdaptiveMag_X = (this.numericUpDown_GreenMagX.Value / CTRatio);
                    temp_AdaptiveMag_Y = (this.numericUpDown_GreenMagY.Value / CTRatio);
                    tempKW = (-this.numericUpDown_InCurrkW.Value / CTRatio);
                    tempkVA = (this.numericUpDown_InCurrkVAR.Value / CTRatio);
                }

                temp_AdaptiveMag_X = GetFixed_16FracBits(temp_AdaptiveMag_X);
                temp_AdaptiveMag_Y = GetFixed_16FracBits(temp_AdaptiveMag_Y);
                tempKW = GetFixed_12FracBits(tempKW);
                tempkVA = GetFixed_12FracBits(tempkVA);


                adaptiveTrip_package[3] = (byte)((int)temp_AdaptiveMag_X >> 8);             // high byte of Green Magnitude X
                adaptiveTrip_package[4] = (byte)(0x00FF & (int)temp_AdaptiveMag_X);         // low byte of Green Magnitude X
                adaptiveTrip_package[5] = (byte)((int)temp_AdaptiveMag_Y >> 8);             // high byte of Green Magnitude X
                adaptiveTrip_package[6] = (byte)(0x00FF & (int)temp_AdaptiveMag_Y);
                adaptiveTrip_package[7] = (byte)((int)tempKW >> 8);                         // high byte of Instantenous Current KW direction
                adaptiveTrip_package[8] = (byte)(0x00FF & (int)tempKW);                     // low byte of Instantenous Current KW direction
                adaptiveTrip_package[9] = (byte)((int)tempkVA >> 8);                        // high byte of Instantenous Current kVAR direction
                adaptiveTrip_package[10] = (byte)(0x00FF & (int)tempkVA);                   // low byte of Instantenous Current kVAR direction
                adaptiveTrip_package[11] = (byte)0x0D;

                mySEA.SendPacket = adaptiveTrip_package; // To be saved in master uP as in place of Green Delay parameter storage
                //Thread.Sleep(1000);   // 1 second delay
                OnSend(mySEA);

                mySEA.SendPacket = RelayModeFunctions.BytePacketFor(TripModeDef);
                if (dataBackupR.dataBackup_fromRelay == true)
                {
                    // writes to 6 bytes Mtrip_byte1 to Mtrip_byte6 in master uP
                    // these 6 bytes correspond to the byte packet refering to APP contents as seen on line 342-349 in RelayModeFunctions.cs
                    string lineRead;
                    StreamReader sr = new StreamReader("C:\\DGI Systems\\Relay\\Saved Data\\test_fileRead.txt");
                    int c = 1;
                    while (c <= 14)
                    {// skip through first 14 data bytes
                        lineRead = sr.ReadLine(); //Read the next line
                        c++;
                    }

                    mySEA.SendPacket[0] = 77;  // 'M'
                    mySEA.SendPacket[1] = 84;  // 'T'
                    for (int cnt = 2; cnt <= 6; cnt++)
                    {
                        lineRead = sr.ReadLine(); //Read the next line
                        if ((cnt % 2) == 0)//odd 
                            mySEA.SendPacket[cnt] = Convert.ToByte(lineRead);
                        else
                            mySEA.SendPacket[cnt + 1] = Convert.ToByte(lineRead);
                    }
                    mySEA.SendPacket[7] = 0x0D;

                    //dataBackupR.dataBackup_fromRelay = false;
                    sr.Close();

                }

                OnSend(mySEA);

                //New Trip Parameters
                mySEA.WithAck = true;
                mySEA.RequestAll = true;
                mySEA.SendPacket[0] = (byte)'M';
                mySEA.SendPacket[1] = (byte)'S';

                //mySEA.SendPacket[2] = 0;
                //if ((string)this.domainUpDownTripStyle.SelectedItem == "Hold Trip")
                //if ((string)this.domainUpDownTripStyle.SelectedItem == "Hold Trip (Troubleshooting Only)")
                if ((string)this.comboBox_TripStyle.SelectedItem == "Hold Trip (Troubleshooting Only)")
                    mySEA.SendPacket[2] = 0;
                //else if ((string)this.domainUpDownTripStyle.SelectedItem == "Pulse Trip")
                //else if ((string)this.domainUpDownTripStyle.SelectedItem == "Continuous Pulse")
                else if ((string)this.comboBox_TripStyle.SelectedItem == "Continuous Pulse")
                    mySEA.SendPacket[2] = 1;
                //else if ((string)this.domainUpDownTripStyle.SelectedItem == "Single Attempt")
                //else if ((string)this.domainUpDownTripStyle.SelectedItem == "3 Pulse, then off")
                else if ((string)this.comboBox_TripStyle.SelectedItem == "3 Pulse, then off")
                    mySEA.SendPacket[2] = 2;
                //else if ((string)this.domainUpDownTripStyle.SelectedItem == "Short Trip")
                else if ((string)this.comboBox_TripStyle.SelectedItem == "Short Trip")
                    mySEA.SendPacket[2] = 3;
                else
                    //throw new Exception(this.domainUpDownTripStyle.SelectedItem.ToString());
                    throw new Exception(this.comboBox_TripStyle.SelectedItem.ToString());

                if (this.checkBoxTripOnPowerDown.Checked)        //Reversed to be backward compatible in the relay
                    mySEA.SendPacket[2] = (byte)(mySEA.SendPacket[2] & (byte)0xFB);
                else
                    mySEA.SendPacket[2] = (byte)(mySEA.SendPacket[2] | 0x04);

                mySEA.SendPacket[3] = mySEA.SendPacket[4] = mySEA.SendPacket[5] = mySEA.SendPacket[6] = 0;
                mySEA.SendPacket[7] = 0x0D;
                if (this.VersionNumber >= 110609)
                    OnSend(mySEA);

                sending = false;

            }//((choice == DialogResult.OK) || (sendAllF.SendAllFlag == true))
            Thread.Sleep(1500);   // 1 second
        }

        private void OnSend(SendEventArgs sEA)
        {
            if (Send != null && !this.SendTimedOut)
                Send(this, sEA);
        }

        public decimal GetFixed_16FracBits(decimal value)
        {
            Int32 temp;
            temp = (Int32)Math.Round((value / 1000m) / Constants.SixteenFracBits);
            return (decimal)temp;
        }

        public decimal GetFixed_12FracBits(decimal value)
        {
            //Int16 temp;
            //temp = (Int16)(value / Constants.TwelveFracBits);
            Int32 temp;
            temp = (Int32)(value / Constants.TwelveFracBits);
            return (decimal)temp;
        }

        private void listBoxTripModes_SelectedIndexChanged(object sender, EventArgs e)
        {
            TripModes tripMode = RelayModeFunctions.TripModeFrom(this.listBoxTripModes.Text);

            this.lblGreenDelay.Visible = false;
            this.numericUpDown_GreenDelay.Visible = false;
            this.lblUnitGreenDelay.Visible = false;
            this.lblGreenMagX.Visible = false;
            this.numericUpDown_GreenMagX.Visible = false;
            this.lblUnitGreenMagX.Visible = false;
            this.lblGreenMagY.Visible = false;
            this.numericUpDown_GreenMagY.Visible = false;
            this.lblUnitGreenMagY.Visible = false;
            this.lbl_InstCurrent_kWdirection.Visible = false;
            this.numericUpDown_InCurrkW.Visible = false;
            this.lblUnitInCur_kWdir.Visible = false;
            this.lbl_InstCurrent_kVARdirection.Visible = false;
            this.numericUpDown_InCurrkVAR.Visible = false;
            this.lblUnitInCur_kVARdir.Visible = false;
            this.checkBoxEnableGullWing.Visible = true;

            myTMCEA.TripMode = tripMode;
            switch (tripMode)
            {
                case TripModes.Sensitive:
                    this.numericUpDownInsensTrip.Visible = false;
                    this.labelInsensTripUnit.Visible = false;
                    this.labelInstantCurrent.Visible = false;
                    this.labelInsensTrip.Visible = false;
                    this.sensitiveTimeVisible(true);
                    this.SensitiveVisible(true);
                    this.TimeDelayVisible(false);
                    this.TimeDelayInstantCurrentLabelVisible(false);
                    this.InsensitiveLabelVisible(false);
                    this.ExtendedTDVisible(false);
                    this.WattVarVisible(false);
                    this.setSensitiveToolTip();
                    break;
                case TripModes.Insensitive:
                    try
                    {
                        this.numericUpDownInsensTrip.Value = this.convertDisplay(this.insensitiveCurrent);
                    }
                    catch
                    {
                        this.numericUpDownInsensTrip.Value = 2.5m;
                    }
                    this.numericUpDownInsensTrip.Visible = true;
                    this.labelInsensTripUnit.Visible = true;
                    this.labelInstantCurrent.Visible = false;
                    this.labelInsensTrip.Visible = true;
                    this.sensitiveTimeVisible(false);
                    this.SensitiveVisible(true);
                    this.TimeDelayVisible(false);
                    this.TimeDelayInstantCurrentLabelVisible(false);
                    this.InsensitiveLabelVisible(true);
                    this.ExtendedTDVisible(true);
                    this.WattVarVisible(false);
                    this.setInsensitiveToolTip();
                    break;
                case TripModes.TimeDelay:
                    try
                    {
                        this.numericUpDownInsensTrip.Value = this.convertDisplay(this.instantaneousCurrent);
                    }
                    catch
                    {
                        this.numericUpDownInsensTrip.Value = 2.5m;
                    }
                    this.numericUpDownInsensTrip.Visible = true;
                    this.labelInsensTripUnit.Visible = true;
                    this.labelInstantCurrent.Visible = true;
                    this.labelInsensTrip.Visible = false;
                    this.sensitiveTimeVisible(false);
                    this.SensitiveVisible(true);
                    this.ExtendedTDVisible(true);
                    this.WattVarVisible(false);
                    this.TimeDelayVisible(true);
                    this.TimeDelayInstantCurrentLabelVisible(true);
                    this.InsensitiveLabelVisible(false);
                    this.setTimeDelayToolTip();
                    break;
                case TripModes.RemoteTrip:
                    this.sensitiveTimeVisible(false);
                    this.SensitiveVisible(false);
                    this.TimeDelayVisible(false);
                    this.ExtendedTDVisible(false);
                    this.WattVarVisible(false);
                    this.TimeDelayInstantCurrentLabelVisible(false);
                    this.InsensitiveLabelVisible(false);
                    break;
                case TripModes.WattVar:
                    this.numericUpDownInsensTrip.Visible = true;
                    this.labelInsensTripUnit.Visible = true;
                    this.labelInstantCurrent.Visible = true;
                    this.labelInsensTrip.Visible = false;
                    this.sensitiveTimeVisible(true);
                    this.SensitiveVisible(true);
                    this.TimeDelayVisible(true);
                    this.TimeDelayInstantCurrentLabelVisible(true);
                    this.InsensitiveLabelVisible(false);
                    this.ExtendedTDVisible(false);
                    this.WattVarVisible(true);
                    this.setWattVarToolTip();
                    break;
                case TripModes.Adaptive:
                    this.numericUpDownInsensTrip.Visible = false;
                    this.labelInsensTripUnit.Visible = false;
                    this.labelInstantCurrent.Visible = false;
                    this.labelInsensTrip.Visible = false;
                    this.checkBoxEnableGullWing.Visible = false;
                    this.sensitiveTimeVisible(false);
                    this.SensitiveVisible(false);
                    this.TimeDelayVisible(false);
                    this.TimeDelayInstantCurrentLabelVisible(false);
                    this.InsensitiveLabelVisible(false);
                    this.ExtendedTDVisible(false);
                    this.WattVarVisible(false);
                    this.Display_adaptiveTrip_Settings();
                    break;

            }

            this.modeChanged();
        }

        private void Display_adaptiveTrip_Settings()
        {
            this.lblGreenDelay.Visible = true;
            this.numericUpDown_GreenDelay.Visible = true;
            this.lblGreenDelay.Location = new System.Drawing.Point(140, 74); //(150, 74);
            this.numericUpDown_GreenDelay.Location = new System.Drawing.Point(235, 72);
            this.lblUnitGreenDelay.Visible = true;
            this.lblUnitGreenDelay.Location = new System.Drawing.Point(300, 74);
            this.numericUpDown_GreenDelay.Enabled = true;// false;


            this.lblGreenMagX.Visible = true;
            this.numericUpDown_GreenMagX.Visible = true;
            this.lblGreenMagX.Location = new System.Drawing.Point(100, 104); //Point(110, 104);
            this.numericUpDown_GreenMagX.Location = new System.Drawing.Point(235, 102);
            this.lblUnitGreenMagX.Visible = true; 
            this.lblUnitGreenMagX.Location = new System.Drawing.Point(300, 104);
            this.numericUpDown_GreenMagX.Enabled = true;//false;

            this.lblGreenMagY.Visible = true;
            this.numericUpDown_GreenMagY.Visible = true;
            this.lblGreenMagY.Location = new System.Drawing.Point(100, 134); //(110, 134);
            this.numericUpDown_GreenMagY.Location = new System.Drawing.Point(235, 132);
            this.lblUnitGreenMagY.Visible = true;
            this.lblUnitGreenMagY.Location = new System.Drawing.Point(300, 134);
            this.numericUpDown_GreenMagY.Enabled = true;//false;

            this.lbl_InstCurrent_kWdirection.Visible = true;
            this.numericUpDown_InCurrkW.Visible = true;
            this.lbl_InstCurrent_kWdirection.Location = new System.Drawing.Point(45, 164); //(40, 164);
            this.numericUpDown_InCurrkW.Location = new System.Drawing.Point(232, 162);
            this.lblUnitInCur_kWdir.Visible = true;
            this.lblUnitInCur_kWdir.Location = new System.Drawing.Point(300, 166);
            this.numericUpDown_InCurrkW.Enabled = true;//false;

            this.lbl_InstCurrent_kVARdirection.Visible = true;
            this.numericUpDown_InCurrkVAR.Visible = true;
            this.lbl_InstCurrent_kVARdirection.Location = new System.Drawing.Point(15, 194); //(10, 194);
            this.numericUpDown_InCurrkVAR.Location = new System.Drawing.Point(235, 192);
            this.lblUnitInCur_kVARdir.Visible = true;
            this.lblUnitInCur_kVARdir.Location = new System.Drawing.Point(300, 196);
            this.numericUpDown_InCurrkVAR.Enabled = true;//false;
        }


        /// <summary>
        /// Converts a decimal on Relay Setting into the appropriate display value
        /// </summary>
        /// <param name="p">Relay Setting</param>
        /// <returns>Proper Display Value</returns>
        private decimal convertDisplay(decimal p)
        {
            switch (this.displayType)
            {
                case eDisplayType.Percent:
                    return p * 20m;
                case eDisplayType.Protector:
                    return p * this.CTRatio;
                case eDisplayType.Relay:
                default:
                    return p;

            }
        }

        private void sensitiveTimeVisible(bool value)
        {
            this.numericUpDownSensitiveTimeDelay.Visible = value;
            this.labelSTD.Visible = value;
            this.labelSTDunit.Visible = value;
            if (!this.sensitiveTimeEnabled)
            {
                this.hideSensitiveTimeDelay();
            }

        }

        private void SensitiveVisible(bool value)
        {
            this.numericUpDownSensTrip.Visible = value;
            this.labelSensTrip.Visible = value;
            this.labelSensTripUnit.Visible = value;

            this.numericUpDownAngle.Visible = value;
            this.labelAngle.Visible = value;
            this.labelAngleUnit.Visible = value;

        }

        private void TimeDelayVisible(bool value)
        {
            this.numericUpDownTimeDelay.Visible = value;
            this.labelTD.Visible = value;
            this.labelTDunit.Visible = value;
        }

        private void TimeDelayInstantCurrentLabelVisible(bool value)
        {
            this.labelInstantCurrent.Visible = value;
        }

        private void InsensitiveLabelVisible(bool value)
        {

        }

        private void ExtendedTDVisible(bool value)
        {
            this.numericUpDownExtendedTimeDelay.Visible = value;
            this.labelETD.Visible = value;
            this.labelETDunit.Visible = value;
        }

        private void WattVarVisible(bool value)
        {
            // This one version has Time Delay removed for Watt-Var, due to confusion with Seattle
            if (this.versionNumber == 150304)
            {
                this.numericUpDownTimeDelay.Visible = false;
                this.labelTDunit.Visible = false;
                this.labelTD.Visible = false;
            }

            this.numericUpDownWVAngle.Visible = value;
            this.labelWVAngle.Visible = value;
            this.labelWVAngleUnit.Visible = value;

            this.numericUpDownWVCurrent.Visible = value;
            this.labelWVCurrent.Visible = value;
            this.labelWVCurrentUnit.Visible = value;


        }

        private void sequenceStyleWattVar()
        {

            if (this.sequenceRelay)
            {
                this.toolTip.SetToolTip(this.numericUpDownWVAngle, "Number of degrees to rotate the Sensitive Trip Curve when V2N exceeds 0.06%");
                this.numericUpDownWVCurrent.Enabled = false;
            }
            else
            {
                this.toolTip.SetToolTip(this.numericUpDownWVAngle, "Number of degrees to rotate the Sensitive Trip Curve when Watt-Var Current has been exceeded");
                this.numericUpDownWVCurrent.Enabled = true;
            }
        }

        private void modeChanged()
        {
            if (TripModeChanged != null)
            {
                TripModeChanged(myTMCEA);
            }
        }

        public void SetAllValue(byte[] packet)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    invokeSetAllCallBack iSCB = new invokeSetAllCallBack(this.setAll);
                    this.Invoke(iSCB, new object[] { packet });
                }
                else
                {
                    this.setAll(packet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private decimal insensitiveCurrent = 2.5m;
        private decimal instantaneousCurrent = 2.5m;
        private delegate void invokeSetAllCallBack(byte[] packet);

        private void setAll(byte[] bytePacket)
        {
            TripModes tempTM = 0;
            Int32 temp = 0;
            decimal tempD = 0, tripAngle = 0, conversionFactor = 1m;

            //switch (this.domainUpDownType.SelectedIndex)
            switch (this.comboBox_TripType.SelectedIndex)
            {
                case 0:
                    conversionFactor = 1m;
                    break;
                case 1:
                    conversionFactor = Math.Round(1000m / 50m, 3);
                    break;
                case 2:
                    conversionFactor = (decimal)this.CTRatio;// / 1000m;
                    break;
                default:
                    conversionFactor = 1m;
                    break;

            }

            tempTM = RelayModeFunctions.TripModeFrom((char)bytePacket[0]);

            try
            {
                /*switch (tempTM)
                {
                    case TripModes.Insensitive:
                        this.listBoxTripModes.SelectedIndex = 1;
                        break;
                    case TripModes.TimeDelay:
                        this.listBoxTripModes.SelectedIndex = 2;
                        break;
                    case TripModes.WattVar:
                        this.listBoxTripModes.SelectedIndex = 3;
                        break;
                    case TripModes.Adaptive:
                        this.listBoxTripModes.SelectedIndex = 4;
                        break;
                    case TripModes.Sensitive:
                    case TripModes.RemoteTrip:
                    default:
                        this.listBoxTripModes.SelectedIndex = 0;
                        break;

                }*/

                switch (tempTM)
                {
                    case TripModes.Insensitive:
                        this.listBoxTripModes.SelectedIndex = 2;
                        tripI.tripInsensitive = true;
                        break;
                    case TripModes.TimeDelay:
                        this.listBoxTripModes.SelectedIndex = 1;
                        tripI.tripInsensitive = false;
                        break;
                    case TripModes.WattVar:
                        this.listBoxTripModes.SelectedIndex = 3;
                        tripI.tripInsensitive = false;
                        break;
                    case TripModes.Adaptive:
                        this.listBoxTripModes.SelectedIndex = 4;
                        tripI.tripInsensitive = false;
                        break;
                    case TripModes.Sensitive:
                    case TripModes.RemoteTrip:
                    default:
                        this.listBoxTripModes.SelectedIndex = 0;
                        tripI.tripInsensitive = false;
                        break;

                }

            }
            catch
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
                this.errorHandler(new Exception("'" + Convert.ToChar(bytePacket[0]).ToString() + " is not a valid Trip Mode Character."));
            }
            //TimeDelay

            try
            {
                temp = bytePacket[2];//13
                temp <<= 8;
                temp += bytePacket[1];//12


                this.numericUpDownTimeDelay.Value = temp;
            }
            catch
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
                this.errorHandler(new Exception(temp.ToString() + " is not a valid Time Delay Value."));
            }
            try
            {
                //Extended Delay
                temp = bytePacket[4];
                this.numericUpDownExtendedTimeDelay.Value = temp;

            }
            catch
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
                this.errorHandler(new Exception(temp.ToString() + " is not a valid Extended Time Delay Value."));
            }
            try
            {
                //Sensitive Delay
                temp = bytePacket[3];

                this.numericUpDownSensitiveTimeDelay.Value = temp;
            }
            catch
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
                this.errorHandler(new Exception(temp.ToString() + " is not a valid Sensitive Time Delay Value."));
                throw new Exception("Bad Trip Delay Value");
            }

            try
            {
                //bytePacket[5] = 113;// 20;
                //bytePacket[6] = 253;// 254;
                //Sensitive Trip Setting
                temp = bytePacket[12];
                temp <<= 8;
                temp += bytePacket[11];
                temp <<= 8;
                temp += bytePacket[6];
                temp <<= 8;
                temp += bytePacket[5];


                tempD = ((decimal)temp * Constants.SixteenFracBits);
                tempD = Math.Round(tempD, 4);
                tempD *= conversionFactor;
                if (this.displayType == eDisplayType.Relay)
                {
                    tempD *= -1000m;
                    tempD = Math.Round(tempD, 1);
                }
                else if (this.displayType == eDisplayType.Percent)
                {
                    tempD *= -1m;
                    tempD = Math.Round(tempD, 3);
                }
                else
                {
                    tempD *= -1m;
                    tempD = Math.Round(tempD, 3);
                }
                this.numericUpDownSensTrip.Value = tempD;

            }
            catch
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
                this.errorHandler(new Exception(tempD.ToString() + " is not a valid Sensitive Trip Value."));
            }

            try
            {
                //Sensitive Angle
                temp = bytePacket[8];
                temp <<= 8;
                temp += bytePacket[7];

                if (temp == 0)
                {
                    tempD = 90;
                }
                else
                {
                    temp <<= 16;
                    temp >>= 16;            //this is to make it negative if it is.
                    tempD = (decimal)temp * Constants.EightFracBits;
                    tempD = (decimal)Math.Atan((double)tempD);
                    tempD = (decimal)RelayModeFunctions.RadiansToDegrees((double)tempD);
                }

                tempD = Math.Round(tempD);
                if (tempD > 0)
                {
                    this.numericUpDownAngle.Value = tempD;
                }
                else
                {
                    this.numericUpDownAngle.Value = 180m + tempD;
                }

                tripAngle = tempD;

            }
            catch
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
                this.errorHandler(new Exception(tempD.ToString() + " is not a valid Sensitive Trip Angle."));
            }


            try
            {
                //Instantaneous Current (IC), Magnitude of 4 trip curve
                temp = bytePacket[10];
                temp <<= 8;
                temp += bytePacket[9];

                tempD = (decimal)temp * Constants.TenFracBits;
                tempD = this.instantaneousCurrent = Math.Round(tempD, 1);
                tempD *= conversionFactor;


                if (tempTM == TripModes.TimeDelay)
                    this.numericUpDownInsensTrip.Value = tempD;


            }
            catch
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
                this.errorHandler(new Exception(tempD.ToString() + " is not a valid Instantaneous Current Value."));
            }



            try
            {
                //Insensitive Trip (IT), Magnitude of 3 trip curve
                temp = bytePacket[18];
                temp <<= 8;
                temp += bytePacket[17];

                tempD = (decimal)temp * Constants.TenFracBits;
                this.insensitiveCurrent = Math.Round(tempD, 1);
                tempD = this.insensitiveCurrent;
                tempD *= conversionFactor;


                if (tempTM == TripModes.Insensitive)
                    this.numericUpDownInsensTrip.Value = tempD;
            }
            catch
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
                this.errorHandler(new Exception(tempD.ToString() + " is not a valid Insensitive Trip Value."));
            }

            try
            {
                temp = bytePacket[14];
                temp <<= 8;
                temp += bytePacket[13];

                tempD = (decimal)temp * Constants.TenFracBits;
                tempD = Math.Round(tempD, 1);
                tempD *= conversionFactor;

                this.numericUpDownWVCurrent.Value = tempD;
            }
            catch
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
                this.errorHandler(new Exception(tempD.ToString() + " is not a valid Watt Varr Value."));
            }

            try
            {
                //Watt Var Angle
                temp = bytePacket[16];
                temp <<= 8;
                temp += bytePacket[15];

                if (temp == 0)
                {
                    tempD = 90;
                }
                else
                {
                    temp <<= 16;
                    temp >>= 16;    //converts 16 bit negative number to 32 bit negative
                    tempD = ((decimal)temp * Constants.EightFracBits);
                    tempD = (decimal)Math.Atan((double)tempD);
                    tempD = (decimal)RelayModeFunctions.RadiansToDegrees((double)tempD);

                    if (tempD < 0)
                        tempD = 180 + tempD;
                }
                tempD = tempD - tripAngle;

                if (tempD > 90)
                    tempD -= 180m;

                this.numericUpDownWVAngle.Value = Math.Round(tempD);
            }
            catch
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
                this.errorHandler(new Exception(Math.Round(tempD).ToString() + " is not a valid Watt Varr Angle."));
            }

            try
            {
                if (bytePacket[19] == (byte)'O')
                {
                    this.checkBoxEnableGullWing.Checked = true;
                }
                else
                {
                    this.checkBoxEnableGullWing.Checked = false;
                }

                temp = bytePacket[21];
                temp <<= 8;
                temp += bytePacket[20];

                if (temp == 0)
                {
                    tempD = 90;
                }
                else
                {
                    temp <<= 16;
                    temp >>= 16;            //this is to make it negative if it is.
                    tempD = (decimal)temp * Constants.EightFracBits;
                    tempD = (decimal)Math.Atan((double)tempD);
                    tempD = (decimal)RelayModeFunctions.RadiansToDegrees((double)tempD);
                }

                tempD = Math.Round(tempD);
                if (tempD > 0)
                {
                    this.numericUpDownGullWingAngle.Value = tempD;
                }
                else
                {
                    this.numericUpDownGullWingAngle.Value = 180m + tempD;
                }
            }
            catch
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
            }

            try
            {
                if (this.versionNumber >= 110609)
                {
                    //Set Trip Style Drop down
                    if ((bytePacket[22] & 0x03) == 1)
                    {
                        //this.domainUpDownTripStyle.SelectedItem = "Pulse Trip";
                        //this.domainUpDownTripStyle.SelectedItem = "Continuous Pulse";
                        this.comboBox_TripStyle.SelectedItem = "Continuous Pulse";
                    }
                    else if ((bytePacket[22] & 0x03) == 2)
                    {
                        //this.domainUpDownTripStyle.SelectedItem = "Single Attempt";
                        //this.domainUpDownTripStyle.SelectedItem = "3 Pulse, then off";
                        this.comboBox_TripStyle.SelectedItem = "3 Pulse, then off";
                    }
                    else if ((bytePacket[22] & 0x03) == 3)
                    {
                        //  this.domainUpDownTripStyle.SelectedIndex = 3;
                        //this.domainUpDownTripStyle.SelectedItem = "Short Trip";
                        this.comboBox_TripStyle.SelectedItem = "Short Trip";
                    }
                    else
                    {
                        //this.domainUpDownTripStyle.SelectedItem = "Hold Trip";
                        //this.domainUpDownTripStyle.SelectedItem = "Hold Trip (Troubleshooting Only)";
                        this.comboBox_TripStyle.SelectedItem = "Hold Trip (Troubleshooting Only)";
                    }

                    //Set Power Down Trip Checkbox - reversed for backwards compatibility
                    if ((bytePacket[22] & 0x04) == 4)
                    {
                        this.checkBoxTripOnPowerDown.Checked = false;
                    }
                    else
                    {
                        this.checkBoxTripOnPowerDown.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                dataBackupTM.dataBackup_tripModeDefaults = true;
                Exception except = new Exception("Error Setting Trip Style, Trip Mode", ex);
                this.errorHandler(except);
            }

        }

        //private void domainUpDownType_SelectedItemChanged(object sender, EventArgs e)
        private void comboBox_TripType_SelectedItemChanged(object sender, EventArgs e)
        {
            //DomainUpDown dUP = (DomainUpDown)sender;

            //switch (dUP.SelectedIndex)
            switch (this.comboBox_TripType.SelectedIndex)
            {
                case 0:
                    this.makeRelayType();
                    break;
                case 1:
                    this.makePercentType();
                    break;
                case 2:
                    this.makeProtectorType();
                    break;
                default:
                    this.makeRelayType();
                    break;
            }
        }

        //private void domainUpDownTripStyle_SelectedItemChanged(object sender, EventArgs e)
        private void comboBox_TripStyle_SelectedItemChanged(object sender, EventArgs e)
        {
            // DomainUpDown dUP = (DomainUpDown)sender;

            if (tripModeM.tripMode_message == true)
            {
                //switch (dUP.SelectedIndex)
                switch (this.comboBox_TripStyle.SelectedIndex)
                {
                    case 0:
                        //this.toolTip.SetToolTip(this.domainUpDownTripStyle, "Maintains the trip contact in the closed state as long as the trip condition exists");
                        this.toolTip.SetToolTip(this.comboBox_TripStyle, "Maintains the trip contact in the closed state as long as the trip condition exists");
                        break;
                    case 1:
                        //this.toolTip.SetToolTip(this.domainUpDownTripStyle, "Continuously pulses the trip contact on and off at one-second intervals for the duration of the trip conditions");
                        this.toolTip.SetToolTip(this.comboBox_TripStyle, "Continuously pulses the trip contact on and off at one-second intervals for the duration of the trip conditions");
                        break;
                    case 2:
                        //this.toolTip.SetToolTip(this.domainUpDownTripStyle, "Pulses the trip contact three times, then deactivates the contact and flashes the trip LED until the trip condition clears");
                        this.toolTip.SetToolTip(this.comboBox_TripStyle, "Pulses the trip contact three times, then deactivates the contact and flashes the trip LED until the trip condition clears");
                        break;
                    case 3:
                        //this.toolTip.SetToolTip(this.domainUpDownTripStyle, "Pulses the trip contact three times. If the trip still exists afterward, the contact opens. However, if a Close or Float condition is detected prior to the trip sequence being completed, the relay aborts the sequence and transitions immediately to the appropriate state");
                        this.toolTip.SetToolTip(this.comboBox_TripStyle, "Pulses the trip contact three times. If the trip still exists afterward, the contact opens. However, if a Close or Float condition is detected prior to the trip sequence being completed, the relay aborts the sequence and transitions immediately to the appropriate state");
                        break;
                    default:
                        //this.toolTip.SetToolTip(this.domainUpDownTripStyle, "Pulses the trip contact three times, then deactivates the contact and flashes the trip LED until the trip condition clears");
                        this.toolTip.SetToolTip(this.comboBox_TripStyle, "Pulses the trip contact three times, then deactivates the contact and flashes the trip LED until the trip condition clears");
                        break;
                }
            }

        }



        private void makeProtectorType()
        {
            decimal temp, temp2, temp3, temp4;

            this.labelInsensTripUnit.Text = "A";
            this.labelSensTripUnit.Text = "A";
            this.labelWVCurrentUnit.Text = "A";

            temp = this.numericUpDownSensTrip.Value;

            this.numericUpDownSensTrip.Minimum = .0001m * this.CTRatio;
            this.numericUpDownSensTrip.Maximum = 5m * this.CTRatio;
            this.numericUpDownSensTrip.Increment = .0001m * this.CTRatio;

            if (this.displayType == eDisplayType.Relay)
            {
                this.numericUpDownSensTrip.Value = Math.Round(temp * this.CTRatio, 3);
            }
            else if (this.displayType == eDisplayType.Percent)
            {
                this.numericUpDownSensTrip.Value = Math.Round(temp * .05m * CTRatio, 3);
            }

            temp = this.numericUpDownInsensTrip.Value;

            this.numericUpDownInsensTrip.Minimum = .1m * this.CTRatio;
            this.numericUpDownInsensTrip.Maximum = 15m * this.CTRatio;
            this.numericUpDownInsensTrip.Increment = .1m * this.CTRatio;

            if (this.displayType == eDisplayType.Relay)
            {
                this.numericUpDownInsensTrip.Value = Math.Round(temp * this.CTRatio, 1);
            }
            else if (this.displayType == eDisplayType.Percent)
            {
                this.numericUpDownInsensTrip.Value = Math.Round(temp * .05m * CTRatio, 1);
            }

            temp = this.numericUpDownWVCurrent.Value;

            this.numericUpDownWVCurrent.Minimum = .1m * this.CTRatio;
            this.numericUpDownWVCurrent.Maximum = 15m * this.CTRatio;
            this.numericUpDownWVCurrent.Increment = .1m * this.CTRatio;

            if (this.displayType == eDisplayType.Relay)
            {
                this.numericUpDownWVCurrent.Value = Math.Round(temp * this.CTRatio);
            }
            else if (this.displayType == eDisplayType.Percent)
            {
                this.numericUpDownWVCurrent.Value = Math.Round(temp * .050m * CTRatio, 1);
            }

            temp = this.numericUpDown_GreenMagX.Value;
            temp2 = this.numericUpDown_GreenMagY.Value;
            temp3 = this.numericUpDown_InCurrkW.Value;
            temp4 = this.numericUpDown_InCurrkVAR.Value;

            this.numericUpDown_GreenMagX.Minimum = this.numericUpDown_GreenMagY.Minimum = .0001m * this.CTRatio;
            this.numericUpDown_GreenMagX.Maximum = this.numericUpDown_GreenMagY.Maximum = 15m * this.CTRatio;
            this.numericUpDown_GreenMagX.Increment = this.numericUpDown_GreenMagY.Increment = .0001m * this.CTRatio;
            this.lblUnitGreenMagX.Text = this.lblUnitGreenMagY.Text = "A";

            this.numericUpDown_InCurrkW.Minimum = this.numericUpDown_InCurrkW.Minimum = .1m * this.CTRatio;
            this.numericUpDown_InCurrkW.Maximum = this.numericUpDown_InCurrkVAR.Maximum = 15m * this.CTRatio;
            this.numericUpDown_InCurrkW.Increment = this.numericUpDown_InCurrkVAR.Increment = .1m * this.CTRatio;
            this.lblUnitInCur_kWdir.Text = this.lblUnitInCur_kVARdir.Text = "A";
            if (this.displayType == eDisplayType.Relay)
            {
                this.numericUpDown_GreenMagX.Value = Math.Round(temp * this.CTRatio, 3);
                this.numericUpDown_GreenMagY.Value = Math.Round(temp2 * this.CTRatio, 3);
                this.numericUpDown_InCurrkW.Value = Math.Round(temp3 * this.CTRatio, 1);
                this.numericUpDown_InCurrkVAR.Value = Math.Round(temp4 * this.CTRatio, 1);
            }
            else if (this.displayType == eDisplayType.Percent)
            {
                this.numericUpDown_GreenMagX.Value = Math.Round(temp * .05m * CTRatio, 3);
                this.numericUpDown_GreenMagY.Value = Math.Round(temp2 * .05m * CTRatio, 3);
                this.numericUpDown_InCurrkW.Value = Math.Round(temp3 * .05m * CTRatio, 1);
                this.numericUpDown_InCurrkVAR.Value = Math.Round(temp4 * .05m * CTRatio, 1);
            }

            displayType = eDisplayType.Protector;
        }

        private void setProtectorValues(Int32 value)
        {
            decimal temp, temp2;
            if (this.displayType == eDisplayType.Protector)
            {
                temp = this.numericUpDownSensTrip.Value;
                temp = temp / this.CTRatio;

                this.numericUpDownSensTrip.Increment = (decimal)value * .0001m;
                this.numericUpDownSensTrip.Maximum = (decimal)value * 5m;
                this.numericUpDownSensTrip.Minimum = (decimal)value * .0001m;

                this.numericUpDownSensTrip.Value = value * temp;

                temp = this.numericUpDownInsensTrip.Value;
                temp = temp / this.CTRatio;

                this.numericUpDownInsensTrip.Increment = (decimal)value * .1m;
                this.numericUpDownInsensTrip.Maximum = (decimal)value * 15m;
                this.numericUpDownInsensTrip.Minimum = (decimal)value * .1m;

                this.numericUpDownInsensTrip.Value = value * temp;

                temp = this.numericUpDownWVCurrent.Value;
                temp = temp / this.CTRatio;

                this.numericUpDownWVCurrent.Increment = (decimal)value * .1m;
                this.numericUpDownWVCurrent.Maximum = (decimal)value * 15m;
                this.numericUpDownWVCurrent.Minimum = (decimal)value * .1m;

                this.numericUpDownWVCurrent.Value = value * temp;

                temp = this.numericUpDown_GreenMagX.Value / this.CTRatio;
                temp2 = this.numericUpDown_GreenMagY.Value / this.CTRatio;
                this.numericUpDown_GreenMagX.Minimum = this.numericUpDown_GreenMagY.Minimum = (decimal)value * .0001m;
                this.numericUpDown_GreenMagX.Maximum = this.numericUpDown_GreenMagY.Maximum = (decimal)value * 15m;
                this.numericUpDown_GreenMagX.Increment = this.numericUpDown_GreenMagY.Increment = (decimal)value * .0001m;
                this.numericUpDown_GreenMagX.Value = temp * value;
                this.numericUpDown_GreenMagY.Value = temp2 * value;

                temp = this.numericUpDown_InCurrkW.Value / this.CTRatio;
                temp2 = this.numericUpDown_InCurrkVAR.Value / this.CTRatio;
                this.numericUpDown_InCurrkW.Minimum = this.numericUpDown_InCurrkVAR.Minimum = (decimal)value * .1m;
                this.numericUpDown_InCurrkW.Maximum = this.numericUpDown_InCurrkVAR.Maximum = (decimal)value * 15m;
                this.numericUpDown_InCurrkW.Increment = this.numericUpDown_InCurrkVAR.Increment = (decimal)value * .1m;
                this.numericUpDown_InCurrkW.Value = temp * value;
                this.numericUpDown_InCurrkVAR.Value = temp2 * value;
            }
            value = value * 5;
        }

        private void makePercentType()
        {
            decimal temp, temp2, temp3, temp4;

            this.labelInsensTripUnit.Text = "%";
            this.labelSensTripUnit.Text = "%";
            this.labelWVCurrentUnit.Text = "%";

            temp = this.numericUpDownSensTrip.Value;

            this.numericUpDownSensTrip.Minimum = .002m;
            this.numericUpDownSensTrip.Maximum = 100;
            this.numericUpDownSensTrip.Increment = .002m;
            this.numericUpDownSensTrip.DecimalPlaces = 3;
            if (this.displayType == eDisplayType.Relay)
            {
                this.numericUpDownSensTrip.Value = Math.Round(temp / 50m, 3);
            }
            else if (this.displayType == eDisplayType.Protector)
            {
                this.numericUpDownSensTrip.Value = Math.Round(temp / CTRatio / .050m, 3);
            }

            temp = this.numericUpDownInsensTrip.Value;

            this.numericUpDownInsensTrip.Minimum = 2;
            this.numericUpDownInsensTrip.Maximum = 300;
            this.numericUpDownInsensTrip.Increment = 2;

            if (this.displayType == eDisplayType.Relay)
            {
                this.numericUpDownInsensTrip.Value = Math.Round(temp * 1000m / 50m, 3);
            }
            else if (this.displayType == eDisplayType.Protector)
            {
                this.numericUpDownInsensTrip.Value = Math.Round(temp / CTRatio / .050m, 3);
            }

            temp = this.numericUpDownWVCurrent.Value;

            this.numericUpDownWVCurrent.Minimum = 2;
            this.numericUpDownWVCurrent.Maximum = 100000;
            this.numericUpDownWVCurrent.Increment = 2;

            if (this.displayType == eDisplayType.Relay)
            {
                this.numericUpDownWVCurrent.Value = Math.Round(temp * 1000m / 50m, 3);
            }
            else if (this.displayType == eDisplayType.Protector)
            {
                this.numericUpDownWVCurrent.Value = Math.Round(temp / CTRatio / .050m, 3);
            }

            temp = this.numericUpDown_GreenMagX.Value;
            temp2 = this.numericUpDown_GreenMagY.Value;
            temp3 = this.numericUpDown_InCurrkW.Value;
            temp4 = this.numericUpDown_InCurrkVAR.Value;

            this.numericUpDown_GreenMagX.Minimum = this.numericUpDown_GreenMagY.Minimum =
                this.numericUpDown_InCurrkW.Minimum = this.numericUpDown_InCurrkW.Minimum = 1;
            this.numericUpDown_GreenMagX.Maximum = this.numericUpDown_GreenMagY.Maximum =
                this.numericUpDown_InCurrkW.Maximum = this.numericUpDown_InCurrkVAR.Maximum = 100;
            this.lblUnitGreenMagX.Text = this.lblUnitGreenMagY.Text =
                this.lblUnitInCur_kWdir.Text = this.lblUnitInCur_kVARdir.Text = "%";
            this.numericUpDown_GreenMagX.Increment = this.numericUpDown_GreenMagY.Increment = .002m;
            this.numericUpDown_InCurrkW.Increment = this.numericUpDown_InCurrkVAR.Increment = 1;

            if (this.displayType == eDisplayType.Relay)
            {
                this.numericUpDown_GreenMagX.Value = Math.Round(temp / 50m, 3);
                this.numericUpDown_GreenMagY.Value = Math.Round(temp2 / 50m, 3); ;
                this.numericUpDown_InCurrkW.Value = Math.Round(temp3 * 1000m / 50m, 3);
                this.numericUpDown_InCurrkVAR.Value = Math.Round(temp4 * 1000m / 50m, 3);
            }
            else if (this.displayType == eDisplayType.Protector)
            {
                this.numericUpDown_GreenMagX.Value = Math.Round(temp / CTRatio / .050m, 3);
                this.numericUpDown_GreenMagY.Value = Math.Round(temp2 / CTRatio / .050m, 3);
                this.numericUpDown_InCurrkW.Value = Math.Round(temp3 / CTRatio / .050m, 3);
                this.numericUpDown_InCurrkVAR.Value = Math.Round(temp4 / CTRatio / .050m, 3);
            }

            displayType = eDisplayType.Percent;
        }

        private void makeRelayType()
        {
            decimal temp, temp2, temp3, temp4;

            this.labelInsensTripUnit.Text = "A";
            this.labelSensTripUnit.Text = "mA";
            this.labelWVCurrentUnit.Text = "A";

            temp = this.numericUpDownSensTrip.Value;

            this.numericUpDownSensTrip.Minimum = .1m;
            this.numericUpDownSensTrip.Maximum = 5000;
            this.numericUpDownSensTrip.Increment = .1m;
            this.numericUpDownSensTrip.DecimalPlaces = 1;

            if (this.displayType == eDisplayType.Percent)
            {
                this.numericUpDownSensTrip.Value = Math.Round(temp * 50m, 1);
            }
            else if (this.displayType == eDisplayType.Protector)
            {
                try
                {
                    this.numericUpDownSensTrip.Value = Math.Round(temp * 1000m / CTRatio, 1);
                }
                catch { }
            }

            temp = this.numericUpDownInsensTrip.Value;

            this.numericUpDownInsensTrip.Minimum = .1m;
            this.numericUpDownInsensTrip.Maximum = 15;
            this.numericUpDownInsensTrip.Increment = .1m;

            if (this.displayType == eDisplayType.Percent)
            {
                this.numericUpDownInsensTrip.Value = Math.Round(temp / 1000m * 50m, 1);
            }
            else if (this.displayType == eDisplayType.Protector)
            {
                this.numericUpDownInsensTrip.Value = Math.Round(temp / CTRatio, 1);
            }

            temp = this.numericUpDownWVCurrent.Value;

            this.numericUpDownWVCurrent.Minimum = .1m;
            this.numericUpDownWVCurrent.Maximum = 15;
            this.numericUpDownWVCurrent.Increment = .1m;

            if (this.displayType == eDisplayType.Percent)
            {
                this.numericUpDownWVCurrent.Value = Math.Round(temp / 1000m * 50m, 1);
            }
            else if (this.displayType == eDisplayType.Protector)
            {
                this.numericUpDownWVCurrent.Value = Math.Round(temp / CTRatio, 1);
            }

            temp = this.numericUpDown_GreenMagX.Value;
            temp2 = this.numericUpDown_GreenMagY.Value;
            temp3 = this.numericUpDown_InCurrkW.Value;
            temp4 = this.numericUpDown_InCurrkVAR.Value;

            this.numericUpDown_GreenMagX.Minimum = this.numericUpDown_GreenMagY.Minimum = 0;// 1;
            this.numericUpDown_GreenMagX.Maximum = this.numericUpDown_GreenMagY.Maximum = 160;// 1000;
            this.numericUpDown_GreenMagX.Increment = this.numericUpDown_GreenMagY.Increment = 16;// 10;
            this.lblUnitGreenMagX.Text = this.lblUnitGreenMagY.Text = "A";// "mA";

            this.numericUpDown_InCurrkW.Minimum = this.numericUpDown_InCurrkW.Minimum = 16;// 0.5m;
            this.numericUpDown_InCurrkW.Maximum = this.numericUpDown_InCurrkVAR.Maximum = 2880;// 10;
            this.numericUpDown_InCurrkW.Increment = this.numericUpDown_InCurrkVAR.Increment = 16;// 0.1m;
            this.lblUnitInCur_kWdir.Text = this.lblUnitInCur_kVARdir.Text = "A";
            if (this.displayType == eDisplayType.Percent)
            {
                this.numericUpDown_GreenMagX.Value = Math.Round(temp * 50m, 1);
                this.numericUpDown_GreenMagY.Value = Math.Round(temp2 * 50m, 1);
                this.numericUpDown_InCurrkW.Value = Math.Round(temp3 / 1000m * 50m, 2);
                this.numericUpDown_InCurrkVAR.Value = Math.Round(temp4 / 1000m * 50m, 2);
            }
            else if (this.displayType == eDisplayType.Protector)
            {
                this.numericUpDown_GreenMagX.Value = Math.Round(temp * 1000m / CTRatio, 1);
                this.numericUpDown_GreenMagY.Value = Math.Round(temp2 * 1000m / CTRatio, 1);
                this.numericUpDown_InCurrkW.Value = Math.Round(temp3 / CTRatio, 2);
                this.numericUpDown_InCurrkVAR.Value = Math.Round(temp4 / CTRatio, 2);
            }
            displayType = eDisplayType.Relay;
        }

        //private void buttonRestoreDefaults_Click(object sender, EventArgs e)
        public void buttonRestoreDefaults_Click(object sender, EventArgs e)
        {
            this.restoreDefaults();
        }

        private void restoreDefaults()
        {
            /*   this.setTypeIndependentDefaults();

                switch (this.displayType)
                {
                    case eDisplayType.Percent:
                        this.setPercentageTypeDefaults();
                        break;
                    case eDisplayType.Protector:
                        this.setProtectorTypeDefaults();
                        break;
                    case eDisplayType.Relay:
                    default:
                        this.setRelayTypeDefaults();
                        break;
                }
             */

            //this.domainUpDownType.SelectedIndex = 0;
            this.comboBox_TripType.SelectedIndex = 0;
            this.setRelayTypeDefaults();
            this.makeRelayType();

        }

        private void setTypeIndependentDefaults()
        {
            insensitiveCurrent = 2.5m;
            instantaneousCurrent = 2.5m;
            this.listBoxTripModes.SelectedIndex = 0;
            this.numericUpDownSensitiveTimeDelay.Value = 6;
            this.numericUpDownExtendedTimeDelay.Value = 0;
            this.numericUpDownTimeDelay.Value = 0;
            this.numericUpDownWVAngle.Value = -60;
            //  this.numericUpDownAngle.Value = 90;
            // Making this the case for all defaults, I want them to 
            // actively set it if they are going to use it.
            checkBoxTripOnPowerDown.Checked = false;

#if ENMAX || PSEG
            checkBoxTripOnPowerDown.Checked = true;
#endif


            // Trip Style 
            // 0 - Hold, 1 - Pulse, 2 - Single
#if NU || BOSTON
            this.checkBoxEnableGullWing.Checked = true;
            this.gullWingEnabled = true;
            this.numericUpDownTimeDelay.Value = 0;
            this.numericUpDownAngle.Value = 95;
            this.numericUpDownGullWingAngle.Value = 85;
            //this.domainUpDownTripStyle.SelectedIndex = 0;
            this.comboBox_TripStyle.SelectedIndex = 0;
            //domainUpDownType.SelectedIndex = 0;
            this.comboBox_TripType.SelectedIndex = 0;
#elif DOMINION || BGE
            this.checkBoxEnableGullWing.Checked = false;
            this.gullWingEnabled = false;
            this.numericUpDownTimeDelay.Value = 0;
            this.numericUpDownAngle.Value = 90;
            this.numericUpDownGullWingAngle.Value = 90;
            //this.domainUpDownTripStyle.SelectedIndex = 3;
            this.comboBox_TripStyle.SelectedIndex = 3;
#elif CHICAGO || MADISON || LONDONH
            this.checkBoxEnableGullWing.Checked = false;
            this.gullWingEnabled = false;
            this.numericUpDownTimeDelay.Value = 0;
            this.numericUpDownAngle.Value = 90;
            this.numericUpDownGullWingAngle.Value = 90;
            //this.domainUpDownTripStyle.SelectedIndex = 0;
            this.comboBox_TripStyle.SelectedIndex = 0;
            checkBoxTripOnPowerDown.Checked = true;
#elif TAUNTON
            listBoxTripModes.SelectedIndex = 3;
            checkBoxEnableGullWing.Checked = false;
            gullWingEnabled = false;
            numericUpDownTimeDelay.Value = 0;
            numericUpDownAngle.Value = 90;
            numericUpDownGullWingAngle.Value = 90;
            //domainUpDownTripStyle.SelectedIndex = 0;
            this.comboBox_TripStyle.SelectedIndex = 0;
#elif SEATTLE
            this.checkBoxEnableGullWing.Checked = false;
            checkBoxTripOnPowerDown.Checked = true;
            this.gullWingEnabled = false;
            this.numericUpDownAngle.Value = 90;
            this.numericUpDownGullWingAngle.Value = 90;
            //this.domainUpDownTripStyle.SelectedIndex = 0;
            this.comboBox_TripStyle.SelectedIndex = 0;
#elif PSEG
            this.checkBoxEnableGullWing.Checked = false;
            this.gullWingEnabled = false;
            this.numericUpDownAngle.Value = 90;
            this.numericUpDownGullWingAngle.Value = 90;
            //this.domainUpDownTripStyle.SelectedIndex = 1;
            this.comboBox_TripStyle.SelectedIndex = 1;
#endif
        }

        private void setRelayTypeDefaults()
        {
#if NU || DOMINION || CHICAGO || ENMAX || BOSTON || ONCOR || TORONTO_HYDRO
            insensitiveCurrent = 2.5m;
            instantaneousCurrent = 2.5m;
            this.listBoxTripModes.SelectedIndex = 0;    // Sensitive
            this.numericUpDownSensitiveTimeDelay.Value = 6;
            this.numericUpDownInsensTrip.Value = 2.5m;
            this.numericUpDownSensTrip.Value = 10.0m;
#if (TORONTO_HYDRO || ONCOR)//H Board and DNP customers
            this.numericUpDownSensTrip.Value = 7.5m;// 10.0m;
#endif
            this.numericUpDownWVCurrent.Value = 2.5m;

            this.checkBoxEnableGullWing.Checked = false;
            this.checkBoxTripOnPowerDown.Checked = false;
#if TORONTO_HYDRO
            this.checkBoxTripOnPowerDown.Checked = true;
#endif
            this.gullWingEnabled = false;
            this.numericUpDownTimeDelay.Value = 0;
            this.numericUpDownAngle.Value = 90;
            this.numericUpDownGullWingAngle.Value = 90;
            //this.domainUpDownTripStyle.SelectedIndex = 3;
            this.comboBox_TripStyle.SelectedIndex = 3;
#if ONCOR
            //this.domainUpDownTripStyle.SelectedIndex = 0; // Hold Trip (Troubleshooting Only)
            this.comboBox_TripStyle.SelectedIndex = 0;
#endif
#if TORONTO_HYDRO
            //this.domainUpDownTripStyle.SelectedIndex = 0; 
            this.comboBox_TripStyle.SelectedIndex = 0;
#endif
            this.numericUpDownExtendedTimeDelay.Value = 0;
            this.numericUpDownInsensTrip.Value = 2.5m;
            this.numericUpDownWVAngle.Value = -60;
#elif SEATTLE || ATLANTA || CONED || PSEG //|| ONCOR
            this.numericUpDownSensitiveTimeDelay.Value = 6;
            this.numericUpDownInsensTrip.Value = 2.5m;
            this.numericUpDownSensTrip.Value = 7.5m;
            this.numericUpDownWVCurrent.Value = 2.5m;
            this.numericUpDownAngle.Value = 90;
            this.listBoxTripModes.SelectedIndex = 0;

            this.numericUpDown_GreenDelay.Value = 250;
            this.numericUpDown_GreenMagX.Value = 150;
            this.numericUpDown_GreenMagY.Value = 150;
            this.numericUpDown_InCurrkW.Value = 128;// 1.25m;
            this.numericUpDown_InCurrkVAR.Value = 128;// 2.5m;    
            this.checkBoxTripOnPowerDown.Checked = true;
            //this.domainUpDownTripStyle.SelectedItem = "Single Attempt";
            //this.domainUpDownTripStyle.SelectedItem = "3 Pulse, then off";
            this.comboBox_TripStyle.SelectedItem = "3 Pulse, then off";
            this.checkBoxEnableGullWing.Checked = false;
            this.numericUpDownExtendedTimeDelay.Value = 0;
            this.numericUpDownTimeDelay.Value = 150;
            this.numericUpDownWVAngle.Value = -60;

#elif LONDONH
            this.numericUpDownInsensTrip.Value = 2.5m;
            this.numericUpDownSensTrip.Value = 9.3m;
            this.numericUpDownWVCurrent.Value = 2.5m;
#elif TAUNTON
            this.numericUpDownInsensTrip.Value = 2.5m;
            this.numericUpDownSensTrip.Value = 7.5m;
            this.numericUpDownWVCurrent.Value = 2.5m;
#elif DIGITALGRID
            this.numericUpDownInsensTrip.Value = 2.5m;
            this.numericUpDownSensTrip.Value = 7.5m;
            this.numericUpDownWVCurrent.Value = 2.5m;
            this.numericUpDownAngle.Value = 90;
            
#else
            this.numericUpDownInsensTrip.Value = 2.5m;
            this.numericUpDownSensTrip.Value = 7.5m;//10m;
            this.numericUpDownWVCurrent.Value = 2.5m;
            this.numericUpDownAngle.Value = 90;
#endif
        }

        private void setPercentageTypeDefaults()
        {
#if NU
            this.numericUpDownInsensTrip.Value = 50m;
            this.numericUpDownSensTrip.Value = .2m;
            this.numericUpDownWVCurrent.Value = 50m;
#elif SEATTLE || DOMINION || CHICAGO || ENMAX || PSEG || TAUNTON || BGE
            this.numericUpDownInsensTrip.Value = 50m;
            this.numericUpDownSensTrip.Value = .15m;
            this.numericUpDownWVCurrent.Value = 50m;
#elif LONDONH
            this.numericUpDownInsensTrip.Value = 50m;
            this.numericUpDownSensTrip.Value = .186m;
            this.numericUpDownWVCurrent.Value = 50m;
#elif DIGITALGRID
            this.numericUpDownInsensTrip.Value = 2.5m;
            this.numericUpDownSensTrip.Value = 7.5m;
            this.numericUpDownWVCurrent.Value = 2.5m;
            this.numericUpDownAngle.Value = 90;

#endif
        }

        private void setProtectorTypeDefaults()
        {
#if NU
            this.numericUpDownInsensTrip.Value = (decimal)this.CTRatio * 2.5m;
            this.numericUpDownSensTrip.Value = .0100m * (decimal)this.CTRatio;
            this.numericUpDownWVCurrent.Value = (decimal)this.CTRatio * 2.5m;
#elif SEATTLE || DOMINION || CHICAGO || ENMAX || PSEG || TAUNTON || BGE
            this.numericUpDownInsensTrip.Value = (decimal)this.CTRatio * 2.5m;
            this.numericUpDownSensTrip.Value = .0075m * (decimal)this.CTRatio;
            this.numericUpDownWVCurrent.Value = (decimal)this.CTRatio * 2.5m;
#elif LONDONH
            this.numericUpDownInsensTrip.Value = (decimal)this.CTRatio * 2.5m;
            this.numericUpDownSensTrip.Value = .0093m * (decimal)this.CTRatio;
            this.numericUpDownWVCurrent.Value = (decimal)this.CTRatio * 2.5m;
#elif DIGITALGRID
            this.numericUpDownInsensTrip.Value = 2.5m;
            this.numericUpDownSensTrip.Value = 7.5m;
            this.numericUpDownWVCurrent.Value = 2.5m;
            this.numericUpDownAngle.Value = 90;

#endif
        }

        public delegate void ExceptionHandler(object o, ExceptionEventArgs eEA);

        public event ExceptionHandler TripControlException;

        private void errorHandler(Exception ex)
        {
            if (TripControlException != null)
            {
                TripControlException(this, new ExceptionEventArgs(ex, "Error in Trip Control"));
            }
            else
            {
                throw new Exception("No Exception Handler For Trip Control");
            }
        }

        private bool gullWingEnabled = false;
        private ToolTip gullWingToolTip = new ToolTip();
        private ToolTip tiltAngleToolTip = new ToolTip();

        private void checkBoxEnableGullWing_CheckedChanged(object sender, EventArgs e)
        {
            this.showGullWing(this.checkBoxEnableGullWing.Checked);
            if (this.checkBoxEnableGullWing.Checked)
            {
                this.gullWingToolTip.SetToolTip(this.numericUpDownGullWingAngle, "Applies to Quadrants I and II");
                this.tiltAngleToolTip.SetToolTip(this.numericUpDownAngle, "Applies to Quadrans III and IV");
                this.tiltAngleToolTip.Active = true;
            }
            else
            {
                this.tiltAngleToolTip.SetToolTip(this.numericUpDownAngle, "Reverse Current Sensitive Trip Line Angle");
            }
        }

        private void showGullWing(bool p)
        {
            if (this.Customer != Customers.ConEdison)
            {
                this.gullWingEnabled = p;
                this.labelGullWingAngle.Visible = p;
                this.labelGullWingUnits.Visible = p;
                this.numericUpDownGullWingAngle.Visible = p;
            }
        }

        #region Saved States

        private SaveObject saveObject = new SaveObject();


        private void populateTripModeSavedData(TripModeSavedStateV4 tSS)
        {
            decimal sensConversionFactor, insensConversionFactor;
            //switch (this.domainUpDownType.SelectedIndex)
            switch (this.comboBox_TripType.SelectedIndex)
            {
                case 0:
                    sensConversionFactor = 1m;
                    insensConversionFactor = 1m;
                    break;
                case 1:
                    sensConversionFactor = Math.Round(50m, 3);
                    insensConversionFactor = Math.Round(50m / 1000m, 3);
                    break;
                case 2:
                    sensConversionFactor = 1000m / (decimal)this.CTRatio;
                    insensConversionFactor = 1m / (decimal)this.CTRatio;
                    break;
                default:
                    sensConversionFactor = 1;
                    insensConversionFactor = 1;
                    break;
            }
            tSS.ExtendedTimeDelay = (int)this.numericUpDownExtendedTimeDelay.Value;
            tSS.GullWingAngle = (int)this.numericUpDownGullWingAngle.Value;
            tSS.GullWingEnabled = this.checkBoxEnableGullWing.Checked;
            tSS.InsensitiveCurrent = this.numericUpDownInsensTrip.Value * insensConversionFactor;
            tSS.SensitiveTrip = this.numericUpDownSensTrip.Value * sensConversionFactor;
            tSS.SensitiveTripDelay = (int)this.numericUpDownSensitiveTimeDelay.Value;
            tSS.TiltAngle = (int)this.numericUpDownAngle.Value;
            tSS.TimeDelay = (int)this.numericUpDownTimeDelay.Value;
            tSS.TripMode = RelayModeFunctions.TripModeFrom((string)this.listBoxTripModes.SelectedItem);
            tSS.WattVarAngle = (int)this.numericUpDownWVAngle.Value;
            tSS.WattVarCurrent = this.numericUpDownWVCurrent.Value * insensConversionFactor;
            //tSS.TripStyle = (int)this.domainUpDownTripStyle.SelectedIndex;
            tSS.TripStyle = (int)this.comboBox_TripStyle.SelectedIndex;
            tSS.TripOnPowerDown = this.checkBoxTripOnPowerDown.Checked;
        }

        public void SetAllValues(TripModeSavedStateV4 lTSS)
        {
            decimal sensConversionFactor, insensConversionFactor;

            //switch (this.domainUpDownType.SelectedIndex)
            switch (this.comboBox_TripType.SelectedIndex)
            {
                case 0:
                    sensConversionFactor = insensConversionFactor = 1m;
                    break;
                case 1:
                    sensConversionFactor = Math.Round(1m / 50m, 3);
                    insensConversionFactor = Math.Round(1000m / 50m, 3);
                    break;
                case 2:
                    sensConversionFactor = (decimal)this.CTRatio / 1000m;
                    insensConversionFactor = (decimal)this.CTRatio;
                    break;
                default:
                    sensConversionFactor = insensConversionFactor = 1m;
                    break;

            }
            try
            {
                this.listBoxTripModes.SelectedItem = RelayModeFunctions.StringRepresentationOf(lTSS.TripMode);
                this.numericUpDownAngle.Value = lTSS.TiltAngle;
                this.numericUpDownExtendedTimeDelay.Value = lTSS.ExtendedTimeDelay;
                this.numericUpDownGullWingAngle.Value = lTSS.GullWingAngle;
                this.numericUpDownInsensTrip.Value = lTSS.InsensitiveCurrent * insensConversionFactor;
                this.numericUpDownSensitiveTimeDelay.Value = lTSS.SensitiveTripDelay;
                this.numericUpDownSensTrip.Value = lTSS.SensitiveTrip * sensConversionFactor;
                this.numericUpDownTimeDelay.Value = lTSS.TimeDelay;
                this.numericUpDownWVAngle.Value = lTSS.WattVarAngle;
                this.numericUpDownWVCurrent.Value = lTSS.WattVarCurrent * insensConversionFactor;
                this.checkBoxEnableGullWing.Checked = lTSS.GullWingEnabled;
                //this.domainUpDownTripStyle.SelectedIndex = lTSS.TripStyle;
                this.comboBox_TripStyle.SelectedIndex = lTSS.TripStyle;
                this.checkBoxTripOnPowerDown.Checked = lTSS.TripOnPowerDown;
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }

        }

        public TripModeSavedStateV4 GetSavedState()
        {
            TripModeSavedStateV4 tSS = new TripModeSavedStateV4();
            this.populateTripModeSavedData(tSS);
            return tSS;
        }

        #endregion

    }

    [Serializable()]

    public class TripModeSavedState : ISerializable
    {
        public TripModeSavedState()
        {
        }

        public string Name;
        public TripModes TripMode;
        public int SensitiveTripDelay;
        public int ExtendedTimeDelay;
        public int TimeDelay;
        public decimal SensitiveTrip;
        public int TiltAngle;
        public decimal InsensitiveCurrent;
        public decimal WattVarCurrent;
        public int WattVarAngle;
        public int GullWingAngle;
        public bool GullWingEnabled;

        public TripModeSavedState(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripMode = (TripModes)info.GetValue("Trip Mode", typeof(TripModes));
                this.SensitiveTripDelay = (int)info.GetValue("Sensitive Trip Delay", typeof(int));
                this.ExtendedTimeDelay = (int)info.GetValue("Extended Time Delay", typeof(int));
                this.TimeDelay = (int)info.GetValue("Time Delay", typeof(int));
                this.SensitiveTrip = (Decimal)info.GetValue("Sensitive Trip", typeof(decimal));
                this.TiltAngle = (int)info.GetValue("Tilt Angle", typeof(int));
                this.InsensitiveCurrent = (Decimal)info.GetValue("Insensitive Current", typeof(decimal));
                this.WattVarAngle = (int)info.GetValue("Watt Var Angle", typeof(int));
                this.WattVarCurrent = (Decimal)info.GetValue("Watt Var Current", typeof(decimal));
                this.GullWingAngle = (int)info.GetValue("Gull Wing Angle", typeof(int));
                this.GullWingEnabled = (bool)info.GetValue("Gull Wing Enabled", typeof(bool));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Trip Mode Saved State", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", Name);
                info.AddValue("Trip Mode", TripMode);
                info.AddValue("Sensitive Trip Delay", SensitiveTripDelay);
                info.AddValue("Extended Time Delay", ExtendedTimeDelay);
                info.AddValue("Time Delay", TimeDelay);
                info.AddValue("Sensitive Trip", SensitiveTrip);
                info.AddValue("Tilt Angle", TiltAngle);
                info.AddValue("Insensitive Current", InsensitiveCurrent);
                info.AddValue("Watt Var Current", WattVarCurrent);
                info.AddValue("Watt Var Angle", WattVarAngle);
                info.AddValue("Gull Wing Angle", GullWingAngle);
                info.AddValue("Gull Wing Enabled", GullWingEnabled);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Getting Object Data in Trip Mode Saving", ex);
            }
        }


    }

    [Serializable()]

    public class TripModeSavedStateV4 : ISerializable
    {
        public TripModeSavedStateV4()
        {
        }

        public string Name;
        public TripModes TripMode;
        public int SensitiveTripDelay;
        public int ExtendedTimeDelay;
        public int TimeDelay;
        public decimal SensitiveTrip;
        public int TiltAngle;
        public decimal InsensitiveCurrent;
        public decimal WattVarCurrent;
        public int WattVarAngle;
        public int GullWingAngle;
        public bool GullWingEnabled;
        public int TripStyle;
        public bool TripOnPowerDown = true;

        public TripModeSavedStateV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripMode = (TripModes)info.GetValue("Trip Mode", typeof(TripModes));
                this.SensitiveTripDelay = (int)info.GetValue("Sensitive Trip Delay", typeof(int));
                this.ExtendedTimeDelay = (int)info.GetValue("Extended Time Delay", typeof(int));
                this.TimeDelay = (int)info.GetValue("Time Delay", typeof(int));
                this.SensitiveTrip = (Decimal)info.GetValue("Sensitive Trip", typeof(decimal));
                this.TiltAngle = (int)info.GetValue("Tilt Angle", typeof(int));
                this.InsensitiveCurrent = (Decimal)info.GetValue("Insensitive Current", typeof(decimal));
                this.WattVarAngle = (int)info.GetValue("Watt Var Angle", typeof(int));
                this.WattVarCurrent = (Decimal)info.GetValue("Watt Var Current", typeof(decimal));
                this.GullWingAngle = (int)info.GetValue("Gull Wing Angle", typeof(int));
                this.GullWingEnabled = (bool)info.GetValue("Gull Wing Enabled", typeof(bool));
                this.TripStyle = (int)info.GetValue("Trip Style", typeof(int));
                this.TripOnPowerDown = (bool)info.GetValue("Trip On Power Down", typeof(bool));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Trip Mode Saved State", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", Name);
                info.AddValue("Trip Mode", TripMode);
                info.AddValue("Sensitive Trip Delay", SensitiveTripDelay);
                info.AddValue("Extended Time Delay", ExtendedTimeDelay);
                info.AddValue("Time Delay", TimeDelay);
                info.AddValue("Sensitive Trip", SensitiveTrip);
                info.AddValue("Tilt Angle", TiltAngle);
                info.AddValue("Insensitive Current", InsensitiveCurrent);
                info.AddValue("Watt Var Current", WattVarCurrent);
                info.AddValue("Watt Var Angle", WattVarAngle);
                info.AddValue("Gull Wing Angle", GullWingAngle);
                info.AddValue("Gull Wing Enabled", GullWingEnabled);
                info.AddValue("Trip Style", TripStyle);
                info.AddValue("Trip On Power Down", TripOnPowerDown);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Getting Object Data in Trip Mode Saving", ex);
            }
        }
    }

    [Serializable()]

    public class SaveObject : ISerializable
    {
        public SaveObject()
        {
        }

        //public int NumberOfObjects;
        public List<TripModeSavedStateV4> SavedStates = new List<TripModeSavedStateV4>();

        public SaveObject(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                //this.NumberOfObjects = (int)info.GetValue("Number Of Objects", typeof(int));
                this.SavedStates = (List<TripModeSavedStateV4>)info.GetValue("Saved States", typeof(List<TripModeSavedStateV4>));
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
                throw new Exception("Error Saving Data In Trip Mode Settings.", ex);
            }
        }

        public void AddSavedState(TripModeSavedStateV4 tSS)
        {
            int i = 0;

            for (; i < SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name == tSS.Name || this.SavedStates[i].Name == null)
                {
                    this.SavedStates[i] = tSS;
                    break;
                }
            }

            if (i == SavedStates.Count)
            {
                this.SavedStates.Add(tSS);
            }

            //Sort list alphabetically
            this.SavedStates.Sort(delegate (TripModeSavedStateV4 tSS1, TripModeSavedStateV4 tSS2) { return tSS1.Name.CompareTo(tSS2.Name); });
        }

        private bool sameName(TripModeSavedState tSS, string s)
        {
            if (tSS.Name == s)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveSavedState(TripModeSavedStateV4 tSS)
        {
            if (this.SavedStates == null)
                return;
            this.SavedStates.Remove(tSS);
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

    [Serializable()]

    public class SaveObjectV4 : ISerializable
    {
        public SaveObjectV4()
        {
        }

        //public int NumberOfObjects;
        public List<TripModeSavedState> SavedStates = new List<TripModeSavedState>();

        public SaveObjectV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                //this.NumberOfObjects = (int)info.GetValue("Number Of Objects", typeof(int));
                this.SavedStates = (List<TripModeSavedState>)info.GetValue("Saved States", typeof(List<TripModeSavedState>));
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
                throw new Exception("Error Saving Data In Trip Mode Settings.", ex);
            }
        }

        public void AddSavedState(TripModeSavedState tSS)
        {
            int i = 0;

            for (; i < SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name == tSS.Name || this.SavedStates[i].Name == null)
                {
                    this.SavedStates[i] = tSS;
                    break;
                }
            }

            if (i == SavedStates.Count)
            {
                this.SavedStates.Add(tSS);
            }

            //Sort list alphabetically
            this.SavedStates.Sort(delegate (TripModeSavedState tSS1, TripModeSavedState tSS2) { return tSS1.Name.CompareTo(tSS2.Name); });
        }

        private bool sameName(TripModeSavedState tSS, string s)
        {
            if (tSS.Name == s)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveSavedState(TripModeSavedState tSS)
        {
            if (this.SavedStates == null)
                return;
            this.SavedStates.Remove(tSS);
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



}
