using GraphicsServer.GSNet.Charting;
using SharedResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucCloseMode : UserControl
    {
        public ucCloseMode()
        {
            try
            {
                InitializeComponent();
                this.myInitialize();
                this.lbl_CLoseMode_Title.Location = new System.Drawing.Point(2, 2);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Close Mode Initialization", ex);
            }
#if DEBUG
#else
            //this.numericUpDownTimeDelay.Visible = false;
            //this.labelTD.Visible = false;
            //this.labelTDUnit.Visible = false;
            //this.labelRelaxClose.Visible = false;
#endif
#if CONED
            this.Customer = Customers.CONED;
#else
            this.Customer = Customers.ENMAX;
#endif
        }

        private readonly int _singleCommandRelaxCloseUpdate = 20190627;
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

        private ProtectorVoltage protectorVoltage =
            ProtectorVoltages.GetVoltage();
        public ProtectorVoltage ProtectorVoltage
        {
            get { return protectorVoltage; }
            set
            {
                if (protectorVoltage != value)
                {
                    setProtectorVoltage(value);
                }
            }
        }

        private readonly decimal _v125RecloseMaximum = 10.0m;
        private readonly decimal _v125RecloseMinimum = 0.0m;
        private readonly decimal _v125RecloseIncrement = 0.1m;
        private readonly decimal _v125RecloseValue = 1.5m;

        private readonly decimal _v125PDMaximum = 0.4m;
        private readonly decimal _v125PDMinimum = 0.0m;
        private readonly decimal _v125PDIncrement = 0.1m;
        private readonly decimal _v125PDValue = 0.0m;

        private void setProtectorVoltage(ProtectorVoltage value)
        {
            try
            {
                // The protectVoltage values has not been updated yet, so it can be used to scale things down
                var phaseVoltage = numericUpDownPDV.Value / protectorVoltage.Scaling;
                var recloseVoltage = numericUpDownRecloseVolts.Value / protectorVoltage.Scaling;

                protectorVoltage = value;
                numericUpDownRecloseVolts.Maximum = _v125RecloseMaximum * protectorVoltage.Scaling;
                numericUpDownRecloseVolts.Minimum = _v125RecloseMinimum * protectorVoltage.Scaling;
                numericUpDownRecloseVolts.Increment = _v125RecloseIncrement * protectorVoltage.Scaling;

                numericUpDownPDV.Maximum = _v125PDMaximum * protectorVoltage.Scaling;
                numericUpDownPDV.Minimum = _v125PDMinimum * protectorVoltage.Scaling;
                numericUpDownPDV.Increment = _v125PDIncrement * protectorVoltage.Scaling;

                numericUpDownPDV.Value = phaseVoltage * protectorVoltage.Scaling;
                numericUpDownRecloseVolts.Value = recloseVoltage * protectorVoltage.Scaling;
            }
            catch
            {
                MessageBox.Show("Error setting Close Mode Protector voltages");

                numericUpDownRecloseVolts.Maximum = _v125RecloseMaximum;
                numericUpDownRecloseVolts.Minimum = _v125RecloseMinimum;
                numericUpDownRecloseVolts.Increment = _v125RecloseIncrement;
                numericUpDownRecloseVolts.Value = _v125RecloseValue;

                numericUpDownPDV.Maximum = _v125PDMaximum;
                numericUpDownPDV.Minimum = _v125PDMinimum;
                numericUpDownPDV.Increment = _v125PDIncrement;
                numericUpDownPDV.Value = _v125PDValue;

                protectorVoltage = ProtectorVoltages.GetVoltage();
            }
        }


        private bool relaxClose = false;        ///added so that we could use the bit from the relay to see if relax close is active
        public bool RelaxClose
        {
            get { return this.relaxClose; }
            set
            {
                this.relaxClose = value;
                if (this.relayRevisionNumber > 20110907)
                {
                    if (!value)
                    {
                        if (this.Mode == CloseModes.CircleAndRelax || this.Mode == CloseModes.CircleClose)
                            this.Mode = CloseModes.CircleClose;
                        else
                            this.Mode = CloseModes.Normal;
                    }
                    else
                    {
                        if (this.Mode == CloseModes.Normal)
                            this.Mode = CloseModes.RelaxClose;
                        else if (this.Mode == CloseModes.CircleClose)
                            this.Mode = CloseModes.CircleAndRelax;
                    }
                }
            }
        }
        private const byte _packetSize = 7;

        private uint relayRevisionNumber = 0;
        public uint RelayRevisionNumber
        {
            get { return this.relayRevisionNumber; }
            set
            {
                this.relayRevisionNumber = value;
               /* if (value < 20110907)
                    this.panelBlockedOpenOverride.Visible = false;
                else
                    if (this.customer != Customers.ConEdison)
                    this.panelBlockedOpenOverride.Visible = true;
               */
            }
        }
        public delegate void SendHandler(object sender, SendEventArgs sEA);
        public event SendHandler Send;
        private SendEventArgs mySEA;

        public CloseModeDefinition CloseModeDef = new CloseModeDefinition(CloseModes.Normal);
        public CloseCurveDefinition CloseCurve;

        public bool SendTimedOut = false;

        private void myInitialize()
        {
            try
            {
                toolTip.SetToolTip(this.numericUpDownCloseTiltAngle, "Angle of Master Reclose Line");
                toolTip.SetToolTip(this.numericUpDownPDA, "Angle of the Phasing Reclose Line");
                toolTip.SetToolTip(this.numericUpDownPDV, "Offset Voltage of the Phasing Reclose Line");
                toolTip.SetToolTip(this.numericUpDownRecloseVolts, "Offset Voltage of the Master Reclose Line\n\rOr Differential Voltage for Circle Close");
                toolTip.SetToolTip(this.numericUpDownTimeDelay, "Number of Cycles the Close Condition must exist before Close Operation is initiated");
                toolTip.SetToolTip(this.buttonRelaxClose, "Temporarily Sets Reclose Voltage to 0.1 V");
                toolTip.SetToolTip(this.checkBoxCircleClose, "Enables Circle Close Algorithm");
               // toolTip.SetToolTip(this.panelBlockedOpenOverride, "Determines how Relay Treats Blocked Open command on a Dead Network");
               // toolTip.SetToolTip(this.radioButtonNeverOverride, "Determines how Relay Treats Blocked Open command on a Dead Network");
                //toolTip.SetToolTip(this.radioButtonOverrideBlockedOpen, "Determines how Relay Treats Blocked Open command on a Dead Network");
                toolTip.SetToolTip(this.checkBox1, "Enables Block OverRide");
                CloseModeDef.TimeDelay = 0;
                this.CloseModeDef.CloseMode = CloseModes.Normal;
                //this.radioButtonNeverOverride.Checked = true;
                this.CloseCurve = new CloseCurveDefinition();
                this.checkBox1.Checked = false; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error Setting Default Values", ex);
            }
        }


        private void setCustomer()
        {
            switch (this.customer)
            {
                default:
                case Customers.ENMAX:
                    this.setNonConEd();
                    break;
                case Customers.CONED:
                    this.setConEd();
                    break;
            }
        }

        private void setConEd()
        {
            this.checkBoxCircleClose.Visible = true;// false;
            //this.panelBlockedOpenOverride.Visible = false;
            this.buttonRelaxClose.Visible = false;
            this.numericUpDownPDV.Value = 0.4m;
            this.numericUpDownPDV.Maximum = 0.4m;
            this.numericUpDownPDV.Minimum = 0.4m;
        }

        private void setNonConEd()
        {
            this.checkBoxCircleClose.Visible = true;
            //this.panelBlockedOpenOverride.Visible = false;// true;
            this.buttonRelaxClose.Visible = true;
            this.numericUpDownPDV.Maximum = 0.4m * (decimal)protectorVoltage.Scaling;
            this.numericUpDownPDV.Minimum = 0.0m;
        }

        private void buttonSendCloseMode_Click(object sender, EventArgs e)
        {
            // writes to 6 bytes MClose_byte 1 to MClose_byte6 in master uP
            // these 6 bytes correspond to the byte packet refering to APP contents as seen on line 461-468 in RelayModeFunctions.cs
            try
            {
                mySEA = new SendEventArgs(_packetSize);
                mySEA.WithAck = false;
                mySEA.RequestAll = false;
                CloseModeDef.TimeDelay = (int)this.numericUpDownTimeDelay.Value;
                mySEA.SendPacket = RelayModeFunctions.BytePacketFor(CloseModeDef);
                /*
                 if (dataBackupR.dataBackup_fromRelay == true)
                {
                    string lineRead;
                    StreamReader sr = new StreamReader("C:\\DGI Systems\\Relay\\Saved Data\\test_fileRead.txt");
                    
                    mySEA.SendPacket[0] = 77;  // 'M'
                    mySEA.SendPacket[1] = 67;  // 'C'
                    for (int cnt = 2; cnt <= 6; cnt++)
                    {
                        lineRead = sr.ReadLine(); //Read the next line
                        if((cnt%2) == 0)//even numbered
                            mySEA.SendPacket[cnt] = Convert.ToByte(lineRead);
                        else
                            mySEA.SendPacket[cnt-2] = Convert.ToByte(lineRead);
                    }
                    mySEA.SendPacket[7] = 0x0D;
                }
                */
                OnSend(this, mySEA);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Sending Close Mode", ex);
            }
        }

        private void OnSend(object sender, SendEventArgs sEA)
        {
            if (Send != null && !this.SendTimedOut)
            {
                Send(this, sEA);
            }
        }

        private void TimeDelayVisibility(bool value)
        {
            this.labelTDUnit.Visible = value;
            this.numericUpDownTimeDelay.Visible = value;
            this.labelTD.Visible = value;
        }

        private void RecloseVoltsVisibility(bool value)
        {
            this.labelReclose.Visible = value;
            this.numericUpDownRecloseVolts.Visible = value;
            this.labelRecloseUnit.Visible = value;
        }

        private void PhaseDetectionAngleVisibility(bool value)
        {
            this.labelPhaseDetectOffsetVolts.Visible = value;
            this.numericUpDownPDA.Visible = value;
            this.labelPDAUnit.Visible = value;
        }

        private void PhaseDetectionOffsetVisibility(bool value)
        {
            this.labelPhaseDetectOffsetVolts.Visible = value;
            this.numericUpDownPDV.Visible = value;
            this.labelPDVUnit.Visible = value;
        }

        private void TiltAngleVisibility(bool value)
        {
            this.labelTiltAngle.Visible = value;
            this.numericUpDownCloseTiltAngle.Visible = value;
            this.labelTiltAngleUnit.Visible = value;
        }

        public void buttonSendCloseData_Click(object sender, EventArgs e)
        {
            if (this.checkBoxCircleClose.Checked)
                this.Mode = CloseModes.CircleClose;
            else
                this.Mode = CloseModes.Normal;
            if (sendAllF.SendAllFlag == false)
            {
                var choice = DialogResult.OK;// MessageBox.Show("Sending Close Mode Parameters as set in the APP to the Relay", "Send?", MessageBoxButtons.OKCancel);
                if (choice == DialogResult.OK)
                {
                    this.sendCloseData();
                }
            }
            else
            {
                this.sendCloseData();
            }
            //this.sendCloseData();
        }

        private void buttonRelaxClose_Click(object sender, EventArgs e)
        {
            relaxCloseC.RelaxCloseClick = true;
            this.sendRelaxClose();
        }

        //private void sendCloseData()
        public void sendCloseData()
        {
            if (relaxCloseC.RelaxCloseClick == false)
            {
                Application.UseWaitCursor = true; //keeps waitcursor even when the thread ends.
                Cursor.Current = Cursors.WaitCursor;
                screenD.screenDisable = true;
                this.SendTimedOut = false;
            }
            relaxCloseC.RelaxCloseClick = false;
            buttonSendCloseMode_Click(this, new EventArgs());  // Sends 6 bytes of MClose params with command 'M' + 'C'

            // If sending relax, just send the command and no curves
            if ((mode != CloseModes.CircleAndRelax && mode != CloseModes.RelaxClose) ||
                relayRevisionNumber < _singleCommandRelaxCloseUpdate)
            {
                mySEA = new SendEventArgs(_packetSize);
                this.setVerticalLine();
                this.setHorizontalLine();

                mySEA.SendPacket = this.CloseCurve.BytePacket();  // Sends 8 bytes of C params with command 'C'
                
                //if(chkBox_EnablePermClose.Checked == true)
                //this.SendPermissiveData();

                mySEA.WithAck = true;
                mySEA.RequestAll = true;
                this.OnSend(this, mySEA);
            }
            Thread.Sleep(1000);   //1 second delay
           
        }
        /*
        public void SendPermissiveData()
        {
            decimal tempVoltage = GetFixed_12FracBits(numericnumericUpDown_PermClVoltage.Value);
            byte[] packet = new byte[7]; //permissivePacketSize
            
            packet[0] = (byte)'}';
            if(this.chkBox_EnablePermClose.Checked == true)
                packet[1] = 1;
            else 
                packet[1] = 0;
            packet[2] = (byte)numericUpDown_FloatTime.Value;
            packet[3] = (byte)numericUpDown_PermClActTime.Value;
            packet[4] = (byte)((int)tempVoltage & 0xFF);           // LOW byte//(byte)(((int)tempVoltage >> 8) & 0x00FF);
            packet[5] = (byte)(((int)tempVoltage >> 8) & 0xFF);    // HIGH byte//(byte)((int)tempVoltage & 0x00FF);
            packet[6] = 0x0D;
            this.OnSend(this, new SendEventArgs(7) { SendPacket = packet });
        }
        */
        public decimal GetFixed_12FracBits(decimal value)
        {
            Int16 temp;
            temp = (Int16)(value / Constants.TwelveFracBits);
            return (decimal)temp;
        }

        public void sendRelaxClose()
        {
            if (this.checkBoxCircleClose.Checked)
                this.Mode = CloseModes.CircleAndRelax;
            else
                this.Mode = CloseModes.RelaxClose;

            this.sendCloseData();
        }

        private decimal savedRecloseValue = 1.5m;

        private void setVerticalLine()
        {
            try
            {
                if (this.Mode == CloseModes.RelaxClose || this.Mode == CloseModes.CircleAndRelax)
                {
                    this.CloseCurve.RecloseVolts = 0.0m;
                    this.numericUpDownRecloseVolts.Value = this.savedRecloseValue;
                    this.CloseCurve.TiltAngle = 95;
                }
                else
                {
                    this.CloseCurve.RecloseVolts = this.numericUpDownRecloseVolts.Value / protectorVoltage.Scaling;
                    this.CloseCurve.TiltAngle = this.numericUpDownCloseTiltAngle.Value;
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }
        }

        private void setHorizontalLine()
        {
            try
            {
                if (this.Mode == CloseModes.RelaxClose || this.Mode == CloseModes.CircleAndRelax)
                {
                    this.CloseCurve.PhasingOffset = 0;
                    this.CloseCurve.PhaseDetectAngle = -10;
                }
                else
                {
                    this.CloseCurve.PhasingOffset = this.numericUpDownPDV.Value / protectorVoltage.Scaling;
                    this.CloseCurve.PhaseDetectAngle = this.numericUpDownPDA.Value;
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }

        }

        public delegate void ExceptionHandler(object o, ExceptionEventArgs eEA);
        public event ExceptionHandler CloseControlException;

        private void errorHandler(Exception ex)
        {
            if (CloseControlException != null)
            {
                CloseControlException(this, new ExceptionEventArgs(ex, "Error in CloseControl"));
            }
            else
            {
                throw new Exception("No Exception Handler For Close Control");
            }
        }

        private delegate void setAllCallBack(byte[] bytePacket);

        public void SetAllValues(byte[] bytePacket)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    setAllCallBack sACB = new setAllCallBack(this.setAll);
                    this.Invoke(sACB, new object[] { bytePacket });
                }
                else
                {
                    this.setAll(bytePacket);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setAll(byte[] bytePacket)
        {
            decimal tempM = 0, tempM2 = 0;
            Int16 temp;
            UInt16 uTemp;
            
            try
            {
                if ((char)bytePacket[10] == 'r' || (char)bytePacket[10] == 'R')
                {
                    this.Mode = CloseModes.RelaxClose;
                }
                else if ((char)bytePacket[10] == 's')
                {
                    this.Mode = CloseModes.CircleAndRelax;
                }
                else if ((char)bytePacket[10] == 'C')
                {
                    this.Mode = CloseModes.CircleClose;
                }
                else
                {
                    this.Mode = CloseModes.Normal;
                }
            }
            catch
            {
                dataBackupCM.dataBackup_closeModeDefaults = true;
                this.errorHandler(new Exception("'" + Convert.ToChar(bytePacket[10]).ToString() + "' is not a valid Close Type Character."));
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSendCloseData_Click(this, new EventArgs());
            }

            try
            {
                uTemp = bytePacket[1];
                uTemp <<= 8;
                uTemp += bytePacket[0];
                tempM = (decimal)uTemp * Constants.TwelveFracBits;

                tempM2 = Math.Round(tempM, 1);

                if (!this.relaxClose && this.Mode != CloseModes.CircleAndRelax && this.Mode != CloseModes.RelaxClose)
                {
                    this.savedRecloseValue = tempM2;
                    this.numericUpDownRecloseVolts.Value = tempM2;
                }

            }
            catch
            {
                dataBackupCM.dataBackup_closeModeDefaults = true;
                this.errorHandler(new Exception(tempM2.ToString() + " is not a valid Reclose/Circle Close Voltage Value"));
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSendCloseData_Click(this, new EventArgs());
            }
            try
            {
                //Tilt Angle Bytes - Vertical
                temp = bytePacket[3];
                temp <<= 8;
                temp += bytePacket[2];

                if (temp == 0)
                {
                    tempM = 90m;
                }
                else
                {
                    tempM = (decimal)temp * Constants.EightFracBits;
                    tempM = (decimal)Math.Atan((double)tempM);
                    tempM = (decimal)RelayModeFunctions.RadiansToDegrees((double)tempM);
                }

                if (tempM < 0)
                {
                    tempM += 180;
                }
                tempM2 = Math.Round(tempM);

                this.numericUpDownCloseTiltAngle.Value = tempM2;

            }
            catch
            {
                dataBackupCM.dataBackup_closeModeDefaults = true;
                this.errorHandler(new Exception(tempM2.ToString() + " is not a valid Tilt Angle."));
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSendCloseData_Click(this, new EventArgs());
            }
            try
            {
              //Phasing Voltage Bytes - Horizontal
                uTemp = bytePacket[5];
                uTemp <<= 8;
                uTemp += bytePacket[4];
                tempM = (decimal)uTemp * Constants.TwelveFracBits;

                if (!this.relaxClose && this.Mode != CloseModes.CircleAndRelax && this.Mode != CloseModes.RelaxClose)
                {
                    tempM2 = Math.Round(tempM, 1);
                    this.numericUpDownPDV.Value = tempM2;
                }
            }
            catch
            {
                dataBackupCM.dataBackup_closeModeDefaults = true;
                this.errorHandler(new Exception(tempM2.ToString() + " is not a valid Phasing Voltage."));
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSendCloseData_Click(this, new EventArgs());
            }
            try
            {
               //Phase Detect Angle Bytes - Horizontal
                temp = bytePacket[7];
                temp <<= 8;
                temp += bytePacket[6];

                tempM = (decimal)temp * Constants.TwelveFracBits;
                tempM = (decimal)Math.Atan((double)tempM);
                tempM = (decimal)RelayModeFunctions.RadiansToDegrees((double)tempM);

                if (!this.relaxClose && this.Mode != CloseModes.CircleAndRelax && this.Mode != CloseModes.RelaxClose)
                {
                    tempM2 = Math.Round(tempM);
                    this.numericUpDownPDA.Value = tempM2;
                }
            }
            catch
            {
                dataBackupCM.dataBackup_closeModeDefaults = true;
                this.errorHandler(new Exception(tempM2.ToString() + " is not a valid Phase Detect Angle."));
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSendCloseData_Click(this, new EventArgs());
            }
            uTemp = 0;
            try
            {
              //Time Delay Value

                uTemp = bytePacket[9];
                uTemp <<= 8;
                uTemp += bytePacket[8];

                this.numericUpDownTimeDelay.Value = uTemp;

            }
            catch
            {
                dataBackupCM.dataBackup_closeModeDefaults = true;
                this.errorHandler(new Exception(uTemp.ToString() + " is not a valid Time Delay."));
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSendCloseData_Click(this, new EventArgs());
            }

            try
            {
                uTemp = bytePacket[12];
                uTemp <<= 8;
                uTemp += bytePacket[11];

                if (uTemp == 0)
                {
                   // this.radioButtonNeverOverride.Checked = true;
                    this.checkBox1.Checked = false;
                }
                else
                {
                    //this.radioButtonOverrideBlockedOpen.Checked = true;
                    this.checkBox1.Checked = true;
                }
            }
            catch
            {
                dataBackupCM.dataBackup_closeModeDefaults = true;
                this.errorHandler(new Exception(uTemp.ToString() + " is not a valid Close Control Word."));
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSendCloseData_Click(this, new EventArgs());
            }

            try
            {
                this.numericUpDownPDV.Value = numericUpDownPDV.Value * (decimal)protectorVoltage.Scaling;
                this.numericUpDownRecloseVolts.Value = numericUpDownRecloseVolts.Value * (decimal)protectorVoltage.Scaling;
            }
            catch
            {
                dataBackupCM.dataBackup_closeModeDefaults = true;
                MessageBox.Show("Error setting close mode 277");
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSendCloseData_Click(this, new EventArgs());
            }
        }

        private CloseModes mode = CloseModes.None;
        public CloseModes Mode
        {
            get { return this.mode; }
            set
            {
                if (this.mode != value)
                {
                    this.mode = value;
                    this.CloseModeDef.CloseMode = value;
                    switch (value)
                    {
                        case CloseModes.CircleClose:
                            this.checkBoxCircleClose.Checked = true;
                            this.buttonRelaxClose.BackColor = Color.Transparent;
                            break;
                        case CloseModes.RelaxClose:
                            this.checkBoxCircleClose.Checked = false;
                            this.buttonRelaxClose.BackColor = Color.Yellow; //Color.Orange;
                            break;
                        case CloseModes.Normal:
                        default:
                            this.checkBoxCircleClose.Checked = false;
                            this.buttonRelaxClose.BackColor = Color.Transparent;
                            break;
                        case CloseModes.CircleAndRelax:
                            this.checkBoxCircleClose.Checked = true;
                            this.buttonRelaxClose.BackColor = Color.Yellow; //Color.Orange;
                            break;
                    }
                }
            }
        }
        private void checkBoxCircleClose_CheckedChanged(object sender, EventArgs e)
        {
            this.handleCheckChange();
        }

        private void handleCheckChange()
        {
            if (this.checkBoxCircleClose.Checked)
            {
                if (this.Mode == CloseModes.Normal || this.Mode == CloseModes.CircleClose)
                {
                    this.Mode = CloseModes.CircleClose;
                }
                else
                    this.Mode = CloseModes.CircleAndRelax;

                this.labelReclose.Visible = false;
                this.labelCircleCloseVolts.Visible = true;
            }
            else
            {
                if (this.Mode == CloseModes.Normal || this.Mode == CloseModes.CircleClose)
                    this.Mode = CloseModes.Normal;
                else
                    this.Mode = CloseModes.RelaxClose;

                this.labelReclose.Visible = true;
                this.labelCircleCloseVolts.Visible = false;

                this.labelReclose.Location = new Point(120, 117);  

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
             if (this.checkBox1.Checked)
                this.CloseModeDef.OverrideBlockedClose = true;
             else
                this.CloseModeDef.OverrideBlockedClose = false;
        }

        //private void buttonRestoreDefaults_Click(object sender, EventArgs e)
        public void buttonRestoreDefaults_Click(object sender, EventArgs e)
        {
            if ((this.Customer == Customers.CONED) || (this.Customer == Customers.ENMAX)) 
            {
                this.checkBoxCircleClose.Checked = false;
                this.numericUpDownCloseTiltAngle.Value = 95;
                this.numericUpDownPDA.Value = -5;
#if ENMAX
                this.checkBoxCircleClose.Checked = true;
                this.numericUpDownPDV.Value = 0.0m;
#elif EVERSOURCE
                this.checkBoxCircleClose.Checked = false;
                this.CloseModeDef.CloseMode = CloseModes.Normal;
                this.numericUpDownPDV.Value = 0.0m;
#else
                this.numericUpDownPDV.Value = 0.4m;
#endif
                this.numericUpDownRecloseVolts.Value = 1.5m;
                this.numericUpDownTimeDelay.Value = 6;
                this.checkBox1.Checked = false;
#if !EVERSOURCE
                this.checkBoxCircleClose.Checked = true;
                this.CloseModeDef.CloseMode = CloseModes.CircleClose;
#endif
                this.CloseModeDef.TimeDelay = 6;
            }
            else
            {

#if ENMAX
                this.numericUpDownTimeDelay.Value = 6;
                this.numericUpDownRecloseVolts.Value = 1.5m;
                this.numericUpDownPDA.Value = -5;
                this.numericUpDownCloseTiltAngle.Value = 95;
                this.numericUpDownPDV.Value = 0.0m;
                this.checkBoxCircleClose.Checked = true;
                //this.radioButtonNeverOverride.Checked = true;
                this.CloseModeDef.CloseMode = CloseModes.CircleClose;
                this.CloseModeDef.TimeDelay = 6;
#elif PSEG
                this.numericUpDownTimeDelay.Value = 6;
                this.numericUpDownRecloseVolts.Value = 1.4m;
                this.numericUpDownPDA.Value = -5;
                this.numericUpDownCloseTiltAngle.Value = 95;
                this.numericUpDownPDV.Value = 0.4m;
                this.checkBoxCircleClose.Checked = false;
                //this.radioButtonNeverOverride.Checked = true;
                this.CloseModeDef.CloseMode = CloseModes.Normal;
                this.CloseModeDef.TimeDelay = 6;
#elif LONDON_HYDRO
                this.numericUpDownTimeDelay.Value = 6;
                this.numericUpDownRecloseVolts.Value = 1.2m;
                this.numericUpDownPDA.Value = -5;
                this.numericUpDownCloseTiltAngle.Value = 95;
                this.numericUpDownPDV.Value = 0.0m;
                this.checkBoxCircleClose.Checked = false;
                //this.radioButtonNeverOverride.Checked = true;
                this.CloseModeDef.CloseMode = CloseModes.Normal;
                this.CloseModeDef.TimeDelay = 6;
#elif TAUNTON
                this.numericUpDownTimeDelay.Value = CloseModeDef.TimeDelay = 5;
                this.numericUpDownRecloseVolts.Value = 1.4m;
                this.numericUpDownPDA.Value = -6;
                this.numericUpDownCloseTiltAngle.Value = 95;
                this.numericUpDownPDV.Value = 0.3m;
                this.checkBoxCircleClose.Checked = false;
                //this.radioButtonNeverOverride.Checked = true;
                this.CloseModeDef.CloseMode = CloseModes.Normal;
#else // SEATTLE, DOMINION, CHICAGO, BGE
                this.numericUpDownTimeDelay.Value = 6;
                this.numericUpDownRecloseVolts.Value = 1.5m;
                this.numericUpDownPDA.Value = -5;
                this.numericUpDownCloseTiltAngle.Value = 95;
                this.checkBoxCircleClose.Checked = false;
                //this.radioButtonNeverOverride.Checked = true;
                this.CloseModeDef.CloseMode = CloseModes.Normal;
                this.CloseModeDef.TimeDelay = 6;
                this.checkBox1.Checked = false;
                this.numericUpDownPDV.Value = 0.4m;

#endif

            }
            this.setVerticalLine();
            this.setHorizontalLine();

            try
            {
                this.numericUpDownPDV.Value = numericUpDownPDV.Value * (decimal)protectorVoltage.Scaling;
                this.numericUpDownRecloseVolts.Value = numericUpDownRecloseVolts.Value * (decimal)protectorVoltage.Scaling;
            }
            catch
            {
                MessageBox.Show("Error setting close mode to 277 values");
            }
        }

        private void populateSaveModeCloseData(CloseModeSaveStateV4 cMSS)
        {
            cMSS.PhaseDetectionAngle = (int)this.numericUpDownPDA.Value;
            cMSS.CircleCloseEnabled = this.checkBoxCircleClose.Checked;
            cMSS.CloseDelay = (int)this.numericUpDownTimeDelay.Value;
            cMSS.PhaseDetectionOffset = this.numericUpDownPDV.Value;
            cMSS.RecloseVolts = this.numericUpDownRecloseVolts.Value;
            cMSS.TiltAngle = (int)this.numericUpDownCloseTiltAngle.Value;
            // cMSS.BlockedOverride = this.radioButtonOverrideBlockedOpen.Checked;
            cMSS.BlockedOverride = this.checkBox1.Checked;
        }

        public void SetAllValues(CloseModeSaveStateV4 cMSS)
        {
            try
            {
                this.checkBoxCircleClose.Checked = cMSS.CircleCloseEnabled;
                this.numericUpDownTimeDelay.Value = cMSS.CloseDelay;
                this.numericUpDownRecloseVolts.Value = cMSS.RecloseVolts;
                this.numericUpDownPDA.Value = cMSS.PhaseDetectionAngle;
                this.numericUpDownPDV.Value = cMSS.PhaseDetectionOffset;
                this.numericUpDownCloseTiltAngle.Value = cMSS.TiltAngle;
                /*if (cMSS.BlockedOverride)
                   this.radioButtonOverrideBlockedOpen.Checked = true;
                else
                    this.radioButtonNeverOverride.Checked = true;
                */
                this.checkBox1.Checked = cMSS.BlockedOverride;
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error In Setting Values From Saved State in Close Control", ex));
            }
        }

        public CloseModeSaveStateV4 GetSavedState()
        {
            CloseModeSaveStateV4 cMSS = new CloseModeSaveStateV4();

            this.populateSaveModeCloseData(cMSS);

            return cMSS;
        }

        private void radioOverride_CheckedChanged(object sender, EventArgs e)
        {
           // if (this.radioButtonNeverOverride.Checked)
            //    this.CloseModeDef.OverrideBlockedClose = false;
           // else
            //    this.CloseModeDef.OverrideBlockedClose = true;
        }

    }

    [Serializable()]

    public class CloseModeSaveState : ISerializable
    {
        public CloseModeSaveState()
        {
        }

        public string Name;
        public bool CircleCloseEnabled;
        public int CloseDelay;
        public decimal RecloseVolts;
        public int PhaseDetectionAngle;
        public decimal PhaseDetectionOffset;
        public int TiltAngle;

        public CloseModeSaveState(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.CircleCloseEnabled = (bool)info.GetValue("Circle Close Enabled", typeof(bool));
                this.CloseDelay = (int)info.GetValue("Close Delay", typeof(int));
                this.RecloseVolts = (decimal)info.GetValue("Reclose Volts", typeof(decimal));
                this.PhaseDetectionAngle = (int)info.GetValue("Phase Detection Angle", typeof(int));
                this.PhaseDetectionOffset = (decimal)info.GetValue("Phase Detection Offset", typeof(decimal));
                this.TiltAngle = (int)info.GetValue("Tilt Angle", typeof(int));
            }
            catch (Exception ex)
            {
                throw new Exception("Error In Close Mode Save State Contructor.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", this.Name);
                info.AddValue("Circle Close Enabled", this.CircleCloseEnabled);
                info.AddValue("Close Delay", this.CloseDelay);
                info.AddValue("Reclose Volts", this.RecloseVolts);
                info.AddValue("Phase Detection Offset", this.PhaseDetectionOffset);
                info.AddValue("Phase Detection Angle", this.PhaseDetectionAngle);
                info.AddValue("Tilt Angle", this.TiltAngle);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Close Mode GetObjectData", ex);
            }
        }
    }

    [Serializable()]

    public class CloseModeSaveStateV4 : ISerializable
    {
        public CloseModeSaveStateV4()
        {
        }

        public string Name;
        public bool CircleCloseEnabled;
        public int CloseDelay;
        public decimal RecloseVolts;
        public int PhaseDetectionAngle;
        public decimal PhaseDetectionOffset;
        public int TiltAngle;
        public bool BlockedOverride;

        public CloseModeSaveStateV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.CircleCloseEnabled = (bool)info.GetValue("Circle Close Enabled", typeof(bool));
                this.CloseDelay = (int)info.GetValue("Close Delay", typeof(int));
                this.RecloseVolts = (decimal)info.GetValue("Reclose Volts", typeof(decimal));
                this.PhaseDetectionAngle = (int)info.GetValue("Phase Detection Angle", typeof(int));
                this.PhaseDetectionOffset = (decimal)info.GetValue("Phase Detection Offset", typeof(decimal));
                this.TiltAngle = (int)info.GetValue("Tilt Angle", typeof(int));
                this.BlockedOverride = info.GetBoolean("Blocked Override");
            }
            catch (Exception ex)
            {
                throw new Exception("Error In Close Mode Save State Contructor.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", this.Name);
                info.AddValue("Circle Close Enabled", this.CircleCloseEnabled);
                info.AddValue("Close Delay", this.CloseDelay);
                info.AddValue("Reclose Volts", this.RecloseVolts);
                info.AddValue("Phase Detection Offset", this.PhaseDetectionOffset);
                info.AddValue("Phase Detection Angle", this.PhaseDetectionAngle);
                info.AddValue("Tilt Angle", this.TiltAngle);
                info.AddValue("Blocked Override", this.BlockedOverride);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Close Mode GetObjectData", ex);
            }
        }
    }
}
