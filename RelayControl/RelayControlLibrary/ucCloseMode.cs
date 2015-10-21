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
#if ConEd && !Debug
            this.Customer = Customers.ConEdison;
#else
            this.Customer = Customers.NonConEd;
#endif
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

        private decimal conversion277 = 2.216m;
        private bool voltage277Changed = false;

        public bool voltage277State = false;

        public bool Voltage277State
        {
            get { return this.voltage277State; }
            set
            {
                if (this.voltage277State != value)
                {
                    this.voltage277State = value;
                    this.setVoltage277State();
                }
            }
        }

        private decimal storeRecloseVoltageIncrement = 0;
        private decimal storeRecloseVoltage = 0;
        private decimal storeRecloseMaximum = 0;
        private decimal storePDVoltageIncrement = 0;
        private decimal storeRecloseMinimum = 0;
        private decimal storePDVoltage = 0;
        private decimal storePDMaximum = 0;
        private decimal storePDMinimum = 0;

        private void setVoltage277State()
        {
            try
            {
                if (voltage277State == true) //Increase voltage values
                {
                    this.storeRecloseVoltageIncrement = this.numericUpDownRecloseVolts.Increment;
                    this.storeRecloseVoltage = this.numericUpDownRecloseVolts.Value;
                    this.storeRecloseMaximum = this.numericUpDownRecloseVolts.Maximum;
                    this.storeRecloseMinimum = this.numericUpDownRecloseVolts.Minimum;
                    this.storePDVoltageIncrement = this.numericUpDownPDV.Increment;
                    this.storePDVoltage = this.numericUpDownPDV.Value;
                    this.storePDMaximum = this.numericUpDownPDV.Maximum;
                    this.storePDMinimum = this.numericUpDownPDV.Minimum;

                    //Maximum must be increased before value
                    this.numericUpDownRecloseVolts.Maximum = numericUpDownRecloseVolts.Maximum * conversion277;
                    this.numericUpDownRecloseVolts.Value = numericUpDownRecloseVolts.Value * conversion277;
                    this.numericUpDownRecloseVolts.Increment = this.numericUpDownRecloseVolts.Increment * conversion277;
                    this.numericUpDownRecloseVolts.Minimum = this.numericUpDownRecloseVolts.Minimum * conversion277;

                    this.numericUpDownPDV.Maximum = numericUpDownPDV.Maximum * conversion277;
                    this.numericUpDownPDV.Value = numericUpDownPDV.Value * conversion277;
                    this.numericUpDownPDV.Increment = this.numericUpDownPDV.Increment * conversion277;
                    this.numericUpDownPDV.Minimum = this.numericUpDownPDV.Minimum * conversion277;
                }
                else //decrease voltage values
                {
                    //order matters must decrease minimum and voltage values before maximum value
                    this.numericUpDownRecloseVolts.Minimum = this.storeRecloseMinimum;
                    if (this.numericUpDownRecloseVolts.Value != 0)
                        this.numericUpDownRecloseVolts.Value = numericUpDownRecloseVolts.Value / conversion277;
                    this.numericUpDownRecloseVolts.Maximum = this.storeRecloseMaximum;
                    this.numericUpDownRecloseVolts.Increment = this.storeRecloseVoltageIncrement;

                    this.numericUpDownPDV.Minimum = this.storePDMinimum;
                    if (this.numericUpDownPDV.Value != 0)
                        this.numericUpDownPDV.Value = numericUpDownPDV.Value / conversion277;
                    this.numericUpDownPDV.Maximum = this.storePDMaximum;
                    this.numericUpDownPDV.Increment = this.storePDVoltageIncrement;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting Close Mode 277 voltages");

                this.numericUpDownRecloseVolts.Increment = this.storeRecloseVoltageIncrement;
                this.numericUpDownRecloseVolts.Minimum = this.storeRecloseMinimum;
                this.numericUpDownRecloseVolts.Value = this.storeRecloseVoltage;
                this.numericUpDownRecloseVolts.Maximum = this.storeRecloseMaximum;
                this.numericUpDownPDV.Increment = this.storePDVoltageIncrement;
                this.numericUpDownPDV.Minimum = this.storePDMinimum;
                this.numericUpDownPDV.Value = this.storePDVoltage;
                this.numericUpDownPDV.Maximum = this.storePDMaximum;
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
                    if(!value)
                    {
                        if(this.Mode == CloseModes.CircleAndRelax || this.Mode == CloseModes.CircleClose)
                            this.Mode = CloseModes.CircleClose;
                        else
                            this.Mode = CloseModes.Normal;
                    }
                    else
                    {
                        if(this.Mode == CloseModes.Normal)
                            this.Mode = CloseModes.RelaxClose;
                        else if(this.Mode == CloseModes.CircleClose)
                            this.Mode = CloseModes.CircleAndRelax;
                    }
                    /*
                    if(value)
                        this.buttonRelaxClose.BackColor = Color.Orange;
                    else
                        this.buttonRelaxClose.BackColor = Color.Transparent;
                     */
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
                if(value < 20110907)
                    this.panelBlockedOpenOverride.Visible = false;
                else
                    if(this.customer != Customers.ConEdison)
                        this.panelBlockedOpenOverride.Visible = true;
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
                toolTip.SetToolTip(this.panelBlockedOpenOverride, "Determines how Relay Treats Blocked Open command on a Dead Network");
                toolTip.SetToolTip(this.radioButtonNeverOverride, "Determines how Relay Treats Blocked Open command on a Dead Network");
                toolTip.SetToolTip(this.radioButtonOverrideBlockedOpen, "Determines how Relay Treats Blocked Open command on a Dead Network");
                CloseModeDef.TimeDelay = 0;
                this.CloseModeDef.CloseMode = CloseModes.Normal;
                this.radioButtonNeverOverride.Checked = true;
                this.CloseCurve = new CloseCurveDefinition();
            }
            catch (Exception ex)
            {
                throw new Exception("Error Setting Default Values", ex);
            }
        }


        private void setCustomer()
        {
            switch(this.customer)
            {
                default:
                case Customers.NonConEd:
                    this.setNonConEd();
                    break;
                case Customers.ConEdison:
                    this.setConEd();
                    break;
            }
        }

        private void setConEd()
        {
            this.checkBoxCircleClose.Visible = false;
            this.panelBlockedOpenOverride.Visible = false;
            this.buttonRelaxClose.Visible = false;
            this.numericUpDownPDV.Value = 0.4m;
            this.numericUpDownPDV.Maximum = 0.4m;
            this.numericUpDownPDV.Minimum = 0.4m;
        }

        private void setNonConEd()
        {
            this.checkBoxCircleClose.Visible = true;
            this.panelBlockedOpenOverride.Visible = true;
            this.buttonRelaxClose.Visible = true;
            this.numericUpDownPDV.Maximum = 0.4m;
            this.numericUpDownPDV.Minimum = 0.0m;
        }

        private void buttonSendCloseMode_Click(object sender, EventArgs e)
        {
            try
            {
                mySEA = new SendEventArgs(_packetSize);
                CloseModeDef.TimeDelay = (int)this.numericUpDownTimeDelay.Value;
                mySEA.SendPacket = RelayModeFunctions.BytePacketFor(CloseModeDef);
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
            if(this.checkBoxCircleClose.Checked)
                this.Mode = CloseModes.CircleClose;
            else
                this.Mode = CloseModes.Normal;

            this.sendCloseData();
        }

        private void buttonRelaxClose_Click(object sender, EventArgs e)
        {
            this.sendRelaxClose();
        }

        private void sendCloseData()
        {
            this.SendTimedOut = false;

            buttonSendCloseMode_Click(this, new EventArgs());

            mySEA = new SendEventArgs(_packetSize);
            this.setVerticalLine();
            this.setHorizontalLine();

            mySEA.SendPacket = this.CloseCurve.BytePacket();

            this.OnSend(this, mySEA);
        }

        private void sendRelaxClose()
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
                if(this.Mode == CloseModes.RelaxClose || this.Mode == CloseModes.CircleAndRelax)
                {
                    this.CloseCurve.RecloseVolts = 0.0m;
                    this.numericUpDownRecloseVolts.Value = this.savedRecloseValue;
                }
                else
                {
                    if (voltage277State == true) //adjust for 277
                        this.CloseCurve.RecloseVolts = this.numericUpDownRecloseVolts.Value / conversion277;
                    else
                        this.CloseCurve.RecloseVolts = this.numericUpDownRecloseVolts.Value;
                }

                this.CloseCurve.TiltAngle = this.numericUpDownCloseTiltAngle.Value;
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
                if (voltage277State == true) //adjust for 277
                    this.CloseCurve.PhasingOffset = this.numericUpDownPDV.Value / (decimal)conversion277;
                else
                    this.CloseCurve.PhasingOffset = this.numericUpDownPDV.Value;

                this.CloseCurve.PhaseDetectAngle = this.numericUpDownPDA.Value;
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }

        }

        public delegate void ExceptionHandler(Exception ex);

        public event ExceptionHandler CloseControlException;

        private void errorHandler(Exception ex)
        {
            if(CloseControlException != null)
            {
                CloseControlException(ex);
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
                if(this.InvokeRequired)
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
                this.errorHandler(new Exception("'" + Convert.ToChar(bytePacket[10]).ToString() + "' is not a valid Close Type Character."));
            }

            try
            {

                uTemp = bytePacket[1];
                uTemp <<= 8;
                uTemp += bytePacket[0];
                tempM = (decimal)uTemp * Constants.TwelveFracBits;
                
                tempM2 = Math.Round(tempM, 1);

                if(!this.relaxClose && this.Mode != CloseModes.CircleAndRelax && this.Mode != CloseModes.RelaxClose)
                {
                    this.savedRecloseValue = tempM2;
                    this.numericUpDownRecloseVolts.Value = tempM2;
                }
                
            }
            catch
            {
                this.errorHandler(new Exception(tempM2.ToString() + " is not a valid Reclose/Circle Close Voltage Value"));
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
                this.errorHandler(new Exception(tempM2.ToString() + " is not a valid Tilt Angle."));
            }
            try
            {
                //Phasing Voltage Bytes - Horizontal
                uTemp = bytePacket[5];
                uTemp <<= 8;
                uTemp += bytePacket[4];
                tempM = (decimal)uTemp * Constants.TwelveFracBits;
                
                tempM2 = Math.Round(tempM, 1);
                this.numericUpDownPDV.Value = tempM2;
            }
            catch
            {
                this.errorHandler(new Exception(tempM2.ToString() + " is not a valid Phasing Voltage."));
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

                tempM2 = Math.Round(tempM);
                this.numericUpDownPDA.Value = tempM2;
            }
            catch
            {
                this.errorHandler(new Exception(tempM2.ToString() + " is not a valid Phase Detect Angle."));
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
                this.errorHandler(new Exception(uTemp.ToString() + " is not a valid Time Delay."));
            }

            try
            {
                uTemp = bytePacket[12];
                uTemp <<= 8;
                uTemp += bytePacket[11];

                if(uTemp == 0)
                {
                    this.radioButtonNeverOverride.Checked = true;
                }
                else
                {
                    this.radioButtonOverrideBlockedOpen.Checked = true;
                }
            }
            catch
            {
                this.errorHandler(new Exception(uTemp.ToString() + " is not a valid Close Control Word."));
            }

            try //set 277 value Close Mode
            {
                if (voltage277State == true)
                {
                    this.numericUpDownPDV.Value = numericUpDownPDV.Value * (decimal)conversion277;
                    this.numericUpDownRecloseVolts.Value = numericUpDownRecloseVolts.Value * (decimal)conversion277;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error setting close mode 277");
            }
        }

        private CloseModes mode = CloseModes.None;
        public CloseModes Mode
        {
            get { return this.mode; }
            set
            {
                if(this.mode != value)
                {
                    this.mode = value;
                    this.CloseModeDef.CloseMode = value;
                    switch(value)
                    {
                        case CloseModes.CircleClose:
                            this.checkBoxCircleClose.Checked = true;
                            this.buttonRelaxClose.BackColor = Color.Transparent;
                            break;
                        case CloseModes.RelaxClose:
                            this.checkBoxCircleClose.Checked = false;
                            this.buttonRelaxClose.BackColor = Color.Orange;
                            break;
                        case CloseModes.Normal:
                        default:
                            this.checkBoxCircleClose.Checked = false;
                            this.buttonRelaxClose.BackColor = Color.Transparent;
                            break;
                        case CloseModes.CircleAndRelax:
                            this.checkBoxCircleClose.Checked = true;
                            this.buttonRelaxClose.BackColor = Color.Orange;
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
                if(this.Mode == CloseModes.Normal || this.Mode == CloseModes.CircleClose)   
                    this.Mode = CloseModes.CircleClose;
                else
                    this.Mode = CloseModes.CircleAndRelax;

                this.labelReclose.Visible = false;
                this.labelCircleCloseVolts.Visible = true;
            }
            else
            {
                if(this.Mode == CloseModes.Normal || this.Mode == CloseModes.CircleClose)
                    this.Mode = CloseModes.Normal;
                else
                    this.Mode = CloseModes.RelaxClose;

                this.labelReclose.Visible = true;
                this.labelCircleCloseVolts.Visible = false;
            }
        }

        private void buttonRestoreDefaults_Click(object sender, EventArgs e)
        {
            if (this.Customer == Customers.ConEdison)
            {
                this.checkBoxCircleClose.Checked = false;
                this.numericUpDownCloseTiltAngle.Value = 95;
                this.numericUpDownPDA.Value = -5;
                this.numericUpDownPDV.Value = 0.4m;
                this.numericUpDownRecloseVolts.Value = 1.4m;
                this.numericUpDownTimeDelay.Value = 6;

                this.CloseModeDef.CloseMode = CloseModes.Normal;
                this.CloseModeDef.TimeDelay = 6;
            }
            else
            {
#if NU
                this.numericUpDownTimeDelay.Value = 6;
                this.numericUpDownRecloseVolts.Value = 1.5m;
                this.numericUpDownPDA.Value = -5;
                this.numericUpDownCloseTiltAngle.Value = 95;
                this.numericUpDownPDV.Value = 0.0m;
                this.checkBoxCircleClose.Checked = false;
                this.radioButtonNeverOverride.Checked = true;
                this.CloseModeDef.CloseMode = CloseModes.Normal;
                this.CloseModeDef.TimeDelay = 6;
#else // SEATTLE, DOMINION
                this.numericUpDownTimeDelay.Value = 6;
                this.numericUpDownRecloseVolts.Value = 1.5m;
                this.numericUpDownPDA.Value = -5;
                this.numericUpDownCloseTiltAngle.Value = 95;
                this.numericUpDownPDV.Value = 0.0m;
                this.checkBoxCircleClose.Checked = false;
                this.radioButtonNeverOverride.Checked = true;
                this.CloseModeDef.CloseMode = CloseModes.Normal;
                this.CloseModeDef.TimeDelay = 6;
#endif

            }
            this.setVerticalLine();
            this.setHorizontalLine();

            try //set 277 value Close Mode
            {
                if (voltage277State == true)
                {
                    this.numericUpDownPDV.Value = numericUpDownPDV.Value * (decimal)conversion277;
                    this.numericUpDownRecloseVolts.Value = numericUpDownRecloseVolts.Value * (decimal)conversion277;
                }
            }
            catch (Exception ex)
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
            cMSS.BlockedOverride = this.radioButtonOverrideBlockedOpen.Checked;
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
                if(cMSS.BlockedOverride)
                    this.radioButtonOverrideBlockedOpen.Checked = true;
                else
                    this.radioButtonNeverOverride.Checked = true;

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
            if(this.radioButtonNeverOverride.Checked)
                this.CloseModeDef.OverrideBlockedClose = false;
            else
                this.CloseModeDef.OverrideBlockedClose = true;
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
