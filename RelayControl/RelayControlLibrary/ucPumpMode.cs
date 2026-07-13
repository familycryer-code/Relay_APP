using SharedResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace RelayControlLibrary
{
    public partial class ucPumpMode : UserControl
    {
        public bool PumpProtectEnabled
        {
            get { return this.pumpProtectEnabled; }
            set
            {
                this.pumpProtectEnabled = value;
                if (value)
                {
                    this.labelPumpProtect.Visible = true;
                }
                else
                {
                    this.labelPumpProtect.Visible = false;
                }
            }
        }
        public ucPumpMode()
        {
            InitializeComponent();

            this.PumpReason = PumpReasons.NoPump;
            toolTip.SetToolTip(this.numericUpDownCycleLimit, "Number of Relay Cycles required to enter Pump State");
            toolTip.SetToolTip(this.numericUpDownMotorCycles, "Number of times the Reclose Motor calls for current before entering the Pump State");
            toolTip.SetToolTip(this.numericUpDownMotorTimeout, "Amount of time the Reclose Motor must call for current before entering the Pump State");
            toolTip.SetToolTip(this.numericUpDownProtectTime, "Amount of time Relay will inhibit the Automatic Reclose after a Pump State has been detected");
            toolTip.SetToolTip(this.numericUpDownPumpTime, "Maximum amount of time Relay Cycles has to meet its threshold before entering Pump State");
            toolTip.SetToolTip(this.checkBoxCycles, "Enables Relay Cycles Pump Protect Mode");
            toolTip.SetToolTip(this.checkBoxMotorCycles, "Enables Motor Cycles Pump Protect Mode");
            toolTip.SetToolTip(this.checkBoxMotorTime, "Enables Motor Timeout Pump Protect Mode");
            toolTip.SetToolTip(this.checkBoxNeverReclose, "Relay will inhibit Automatic Reclose until Pump State is cleared by the user");
            //toolTip.SetToolTip(this.buttonClearPumpProtect, "Clears any active Pump Protect state");
            toolTip.SetToolTip(this.labelPumpType, "Shows Pump Protect Reason");
            toolTip.SetToolTip(this.labelPumpTypeDisplay, "Shows Pump Protect Reason");
        }
        public delegate void SendEventHandler(object o, SendEventArgs sEA);
        public event SendEventHandler Send;
        public delegate void ExceptionHandler(object o, ExceptionEventArgs eEA);
        public event ExceptionHandler PumpControlException;
        public PumpReasons PumpReason
        {
            get { return this.pumpReason; }
            set
            {
                this.pumpReason = value;
                this.updatePumpReasonLable(value);
            }
        }
        public uint RelayRevisionNumber
        {
            get { return this.relayRevisionNumber; }
            set
            {
                this.relayRevisionNumber = value;
                //   if (this.relayRevisionNumber > 999999999) //TEST needs to be fixed when actually implemented
                //       this.displayAlarmOnly(true);
                         this.checkBoxAlarmOnly.Visible = true;
                //   else
                //       this.displayAlarmOnly(false);

                if (this.motorCycleValue != 5 && this.motorCycleValue != 0)
                    this.numericUpDownMotorCycles.Value = this.motorCycleValue;
                if (this.motorTimeoutValue != 10 && this.motorTimeoutValue != 0)
                    this.numericUpDownMotorTimeout.Value = this.motorTimeoutValue;
            }
        }
        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                this.customer = value;
                this.setCustomer();
            }
        }

        private uint relayRevisionNumber;
        private Customers customer;
        private SendEventArgs sEA = new SendEventArgs(8);
        private PumpDefinition PD = new PumpDefinition();
        private bool pumpProtectEnabled;
        private delegate void setAllCallBack(byte[] bytePacket);
        private PumpModeSaveObjectV2 saveObject = new PumpModeSaveObjectV2();
        private PumpReasons pumpReason;
        private ToolTip toolTip = new ToolTip();

        private void displayAlarmOnly(bool p)
        {
            this.checkBoxAlarmOnly.Visible = false;
        }

        private void setCustomer()
        {
            switch (this.customer)
            {
                default:
                case Customers.ENMAX:
                    this.numericUpDownProtectTime.Visible = true;
                    this.labelProtectTime.Visible = true;
                    this.labelProtectTimeUnits.Visible = true;
                    this.checkBoxNeverReclose.Visible = true;
                    break;
                case Customers.CONED:
                    this.numericUpDownProtectTime.Visible = true;
                    this.labelProtectTimeUnits.Visible = true;
                    this.labelProtectTime.Visible = true;
                    this.checkBoxNeverReclose.Visible = true;
                    break;
            }
        }

        public void buttonSend_Click(object sender, EventArgs e)
        {
            var choice = DialogResult.Cancel;

            if (sendAllF.SendAllFlag == false)
            {
                choice = DialogResult.OK; // MessageBox.Show("Sending Pump Mode Parameters as set in the APP to the Relay", "Send?", MessageBoxButtons.OKCancel);
            }
            if ((choice == DialogResult.OK) || (sendAllF.SendAllFlag == true))
            {
                Application.UseWaitCursor = true;  //keeps waitcursor even when the thread ends.
                Cursor.Current = Cursors.WaitCursor;
                screenD.screenDisable = true;
                try
                {
                    PD.RelayCycleEnabled = this.checkBoxCycles.Checked;
                    PD.MotorTimeoutEnabled = this.checkBoxMotorTime.Checked;
                    PD.MotorCycleEnabled = this.checkBoxMotorCycles.Checked;
                    PD.AlarmOnly = this.checkBoxAlarmOnly.Checked;

                    PD.Cycles = (byte)this.numericUpDownCycleLimit.Value;
                    PD.PumpTime = (Int16)this.numericUpDownPumpTime.Value;
                    PD.MotorCycles = (byte)this.numericUpDownMotorCycles.Value;
                    PD.MotorTimeout = (byte)(this.numericUpDownMotorTimeout.Value * 10);
                    if (this.checkBoxNeverReclose.Checked)
                    {
                        PD.PumpProtectTime = 0;
                    }
                    else
                    {
                        PD.PumpProtectTime = (Int16)this.numericUpDownProtectTime.Value;
                    }

                    this.sEA.SendPacket = this.bytePacketFor(PD);
                    this.sEA.WithAck = true;
                    OnSend(sEA);
                }
                catch
                {
                }
            }//((choice == DialogResult.OK) || (sendAllF.SendAllFlag == true))
            Thread.Sleep(1000);   // 1 second
        }

        public byte[] bytePacketFor(PumpDefinition pD)
        {
            //PUMPUPDATE~ byte[8];

            if (this.relayRevisionNumber >= 20100625)
            {
                byte[] returnArray = new byte[10];

                returnArray[0] = (byte)RelayModeFunctions._PumpModeOpCode;
                returnArray[1] = pD.EnableSendByte;
                returnArray[2] = pD.Cycles;
                returnArray[3] = pD.PumpTimeHigh;
                returnArray[4] = pD.PumpTimeLow;
                returnArray[5] = pD.PumpProtectTimeHigh;
                returnArray[6] = pD.PumpProtectTimeLow;
                returnArray[7] = pD.MotorCycles;
                returnArray[8] = pD.MotorTimeout;
                returnArray[9] = (byte)RelayModeFunctions.DC4;

                return returnArray;
            }
            else
            {
                byte[] returnArray = new byte[8];

                returnArray[0] = (byte)RelayModeFunctions._PumpModeOpCode;

                if (pD.RelayCycleEnabled)        //PUMPUPDATE~ 
                    returnArray[1] = 1;
                else
                    returnArray[1] = 0;

                returnArray[2] = pD.Cycles;
                returnArray[3] = pD.PumpTimeHigh;
                returnArray[4] = pD.PumpTimeLow;
                returnArray[5] = pD.PumpProtectTimeHigh;
                returnArray[6] = pD.PumpProtectTimeLow;
                returnArray[7] = (byte)RelayModeFunctions.DC4;

                return returnArray;
            }
        }

        private void OnSend(SendEventArgs sEA)
        {
            if (Send != null)
                Send(this, sEA);
        }

        public void SetAllValues(byte[] bytePacket)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    setAllCallBack sACB = new setAllCallBack(this.setAllValues);
                    this.Invoke(sACB, new object[] { bytePacket });
                }
                else
                {
                    this.setAllValues(bytePacket);
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSend_Click(this, new EventArgs());
            }
        }

        public void SetAllValues(PumpModeSavedStateV2 pMSS)
        {
            try
            {
                this.checkBoxCycles.Checked = pMSS.EnableRelayCycle;
                this.checkBoxMotorCycles.Checked = pMSS.EnableMotorCycle;
                this.checkBoxMotorTime.Checked = pMSS.EnableMotorTimeout;
                this.numericUpDownMotorTimeout.Value = pMSS.MotorTimeout;
                this.numericUpDownMotorCycles.Value = pMSS.MotorCycleLimit;
                this.checkBoxNeverReclose.Checked = pMSS.NeverReclose;
                this.numericUpDownCycleLimit.Value = pMSS.CycleLimit;
                this.numericUpDownProtectTime.Value = pMSS.PumpProtectTime;
                this.numericUpDownPumpTime.Value = pMSS.PumpTime;
            }
            catch
            {
                dataBackupPM.dataBackup_pumpModeDefaults = true;
                this.setDefaults();
                this.buttonSend_Click(this, new EventArgs());
            }
        }

        private void setAllValues(byte[] bytePacket)
        {
            Int16 temp = 0;
            decimal tempM = 0;

            try
            {
                if ((bytePacket[0] & 1) == 1)
                {
                    this.checkBoxCycles.Checked = true;
                }
                else
                {
                    this.checkBoxCycles.Checked = false;
                }
                if ((bytePacket[0] & 4) == 4)
                {
                    this.checkBoxMotorCycles.Checked = true;
                }
                else
                {
                    this.checkBoxMotorCycles.Checked = false;
                }
                if ((bytePacket[0] & 8) == 8)
                {
                    this.checkBoxMotorTime.Checked = true;
                }
                else
                {
                    this.checkBoxMotorTime.Checked = false;
                }
                if (this.relayRevisionNumber > 999999999)  //TEST needs to be fixed when added to relay
                {
                    if ((bytePacket[0] & 32) == 32)
                    {
                        this.checkBoxAlarmOnly.Checked = true;
                    }
                    else
                        this.checkBoxAlarmOnly.Checked = false;
                }
            }
            catch
            {
                dataBackupPM.dataBackup_pumpModeDefaults = true;
                this.errorHandler(new Exception("Problem with Pump Enable Value"));
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSend_Click(this, new EventArgs());
            }
            try
            {
                this.numericUpDownCycleLimit.Value = bytePacket[1];
            }
            catch
            {
                dataBackupPM.dataBackup_pumpModeDefaults = true;
                this.errorHandler(new Exception(bytePacket[1].ToString() + " is not a valid Cycle Limit value."));
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSend_Click(this, new EventArgs());
            }
            try
            {
                temp = bytePacket[3];
                temp <<= 8;
                temp += bytePacket[2];
                tempM = Math.Round((decimal)(temp / 4));
                this.numericUpDownPumpTime.Value = tempM;
            }
            catch
            {
                dataBackupPM.dataBackup_pumpModeDefaults = true;
                this.errorHandler(new Exception(tempM.ToString() + " is not a valid Pump Time value."));
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSend_Click(this, new EventArgs());
            }
            try
            {
                if (bytePacket[4] == 0)
                {
                    this.checkBoxNeverReclose.Checked = true;
                }
                else
                {
                    this.checkBoxNeverReclose.Checked = false;
                    this.numericUpDownProtectTime.Value = bytePacket[4];
                }

            }
            catch
            {
                dataBackupPM.dataBackup_pumpModeDefaults = true;
                this.errorHandler(new Exception(bytePacket[4].ToString() + " is not a valid Pump Protect Time value."));
                this.buttonRestoreDefaults_Click(this, new EventArgs());
                this.buttonSend_Click(this, new EventArgs());
            }

            bool needsUpdate = false;
            if (this.relayRevisionNumber >= 20100625)
            {
                //Motor Cycles
                try
                {
                    this.numericUpDownMotorCycles.Value = bytePacket[5];
                }
                catch
                {
                    if (bytePacket[5] == 0)
                    {
                        needsUpdate = true;
                        this.numericUpDownMotorCycles.Value = 5;
                    }
                    else
                    {
                        dataBackupPM.dataBackup_pumpModeDefaults = true;
                        this.errorHandler(new Exception(bytePacket[5].ToString() + " is not a valid Motor Cycle value."));
                        this.buttonRestoreDefaults_Click(this, new EventArgs());
                        this.buttonSend_Click(this, new EventArgs());
                    }
                }

                //Motor Timeout
                try
                {
                    this.numericUpDownMotorTimeout.Value = (int)(bytePacket[6] / 10);
                }
                catch
                {
                    if (bytePacket[6] == 0)
                    {
                        needsUpdate = true;
                        this.numericUpDownMotorTimeout.Value = 10;
                    }
                    else
                    {
                        dataBackupPM.dataBackup_pumpModeDefaults = true;
                        this.errorHandler(new Exception(bytePacket[6].ToString() + " is not a valid Motor Timeout value."));
                        this.buttonRestoreDefaults_Click(this, new EventArgs());
                        this.buttonSend_Click(this, new EventArgs());
                    }
                }

                if (needsUpdate)
                {
                    this.buttonSend_Click(this, new EventArgs());
                }
            }
            else
            {
                if (bytePacket[5] != 0)
                    this.motorCycleValue = bytePacket[5];
                if (bytePacket[6] != 0)
                    this.motorTimeoutValue = bytePacket[6] / 10;
            }
        }

        private int motorCycleValue = 5;
        private int motorTimeoutValue = 10;


        //private void buttonRestoreDefaults_Click(object sender, EventArgs e)
        public void buttonRestoreDefaults_Click(object sender, EventArgs e)
        {
            this.setDefaults();
        }

        private void setDefaults()
        {

            // numericUpDownCycleLimit.Value = Cycle Limit
            // numericUpDownPumpTime.Value = Pump Time
            // numericUpDownMotorTimeout.Value = Motor Timeout
            // numericUpDownMotorCycles.Value = Motor Cycles
            // numericUpDownProtectTime.Value = Protect Time

            // checkBoxCycles.Checked = Cycle Limit Enable
            // checkBoxMotorTime.Checked = Motor Timeout Enable
            // checkBoxMotorCycles.Checked = Motor Cycles Enable

            // checkBoxNeverReclose.Checked = Never Reclose

#if BGE
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 60;

            this.checkBoxCycles.Checked = true;
            this.checkBoxMotorTime.Checked = false;
            this.checkBoxMotorCycles.Checked = false;

            this.checkBoxNeverReclose.Checked = false;

#elif COMED
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = true;
            this.checkBoxMotorTime.Checked = true;
            this.checkBoxMotorCycles.Checked = true;

            this.checkBoxNeverReclose.Checked = false;

#elif CONED
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = false;
            this.checkBoxMotorTime.Checked = false;
            this.checkBoxMotorCycles.Checked = false;

            this.checkBoxNeverReclose.Checked = false;

#elif DOMINION
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 60;

            this.checkBoxCycles.Checked = true;
            this.checkBoxMotorTime.Checked = true;
            this.checkBoxMotorCycles.Checked = true;

            this.checkBoxNeverReclose.Checked = false;

#elif ENMAX
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = true;
            this.checkBoxMotorTime.Checked = true;
            this.checkBoxMotorCycles.Checked = true;

            this.checkBoxNeverReclose.Checked = false;

#elif EVERSOURCE
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = true;
            this.checkBoxMotorTime.Checked = true;
            this.checkBoxMotorCycles.Checked = true;

            this.checkBoxNeverReclose.Checked = false;

#elif LONDON_HYDRO
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = false;
            this.checkBoxMotorTime.Checked = false;
            this.checkBoxMotorCycles.Checked = false;

            this.checkBoxNeverReclose.Checked = false;
#elif ONCOR
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = false;
            this.checkBoxMotorTime.Checked = false;
            this.checkBoxMotorCycles.Checked = false;

            this.checkBoxNeverReclose.Checked = false;
#elif PSEG
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = false;
            this.checkBoxMotorTime.Checked = false;
            this.checkBoxMotorCycles.Checked = false;

            this.checkBoxNeverReclose.Checked = false;
#elif SCE
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = true;
            this.checkBoxMotorTime.Checked = true;
            this.checkBoxMotorCycles.Checked = true;

            this.checkBoxNeverReclose.Checked = false;
#elif SCL
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = true;
            this.checkBoxMotorTime.Checked = true;
            this.checkBoxMotorCycles.Checked = true;

            this.checkBoxNeverReclose.Checked = false;
#elif TAUNTON
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = true;
            this.checkBoxMotorTime.Checked = false;
            this.checkBoxMotorCycles.Checked = false;

            this.checkBoxNeverReclose.Checked = false;
#elif TORONTO_HYDRO
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = true;
            this.checkBoxMotorTime.Checked = true;
            this.checkBoxMotorCycles.Checked = true;

            this.checkBoxNeverReclose.Checked = false;
#else
            this.numericUpDownCycleLimit.Value = 3;
            this.numericUpDownPumpTime.Value = 30;
            this.numericUpDownMotorTimeout.Value = 10;
            this.numericUpDownMotorCycles.Value = 5;
            this.numericUpDownProtectTime.Value = 15;

            this.checkBoxCycles.Checked = true;
            this.checkBoxMotorTime.Checked = true;
            this.checkBoxMotorCycles.Checked = true;

            this.checkBoxNeverReclose.Checked = false;
#endif


        }

        private void errorHandler(Exception ex)
        {
            if (PumpControlException != null)
            {
                PumpControlException(this, new ExceptionEventArgs(ex, "Error in PumpMode Control"));
            }
            else
            {
                throw new Exception("No Exception Handler For Pump Control");
            }
        }

        private void checkBoxNeverReclose_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxNeverReclose.Checked)
            {
                this.numericUpDownProtectTime.Enabled = false;
            }
            else
            {
                this.numericUpDownProtectTime.Enabled = true;
            }
        }

        //private void buttonClearPumpProtect_Click(object sender, EventArgs e)
        public void sendClearPumpProtect()
        {
            SendEventArgs clearSEA = new SendEventArgs(10);
            clearSEA.SendPacket[0] = (byte)RelayModeFunctions._PumpModeOpCode;
            clearSEA.SendPacket[1] = 2;
            clearSEA.SendPacket[2] = 0;
            clearSEA.SendPacket[3] = 0;
            clearSEA.SendPacket[4] = 0;
            clearSEA.SendPacket[5] = 0;
            clearSEA.SendPacket[6] = 0;
            clearSEA.SendPacket[7] = 0;
            clearSEA.SendPacket[8] = 0;
            clearSEA.SendPacket[9] = (byte)RelayModeFunctions.DC4;

            this.Send(this, clearSEA);
        }
        /*
        private void buttonSaveState_Click(object sender, EventArgs e)
        {
            PumpModeSavedStateV2 pMSS = new PumpModeSavedStateV2();

            if(this.textBoxSaveStateName.Text == null || this.textBoxSaveStateName.Text == "")
            {
                this.errorHandler(new Exception("Must Supply a Name for Saved State"));
                return;
            }

            pMSS.Name = this.textBoxSaveStateName.Text;
            this.populatePumpModeState(pMSS);
            this.saveObject.AddSavedState(pMSS);

            this.writeSaveObjecToFile();
            
        }
        */
        private void populatePumpModeState(PumpModeSavedStateV2 pMSS)
        {
            pMSS.CycleLimit = (int)this.numericUpDownCycleLimit.Value;
            pMSS.NeverReclose = this.checkBoxNeverReclose.Checked;
            pMSS.PumpProtectTime = (int)this.numericUpDownProtectTime.Value;
            pMSS.PumpTime = (int)this.numericUpDownPumpTime.Value;
            pMSS.EnableMotorCycle = this.checkBoxMotorCycles.Checked;
            pMSS.EnableMotorTimeout = this.checkBoxMotorTime.Checked;
            pMSS.EnableRelayCycle = this.checkBoxCycles.Checked;
            pMSS.MotorCycleLimit = (byte)this.numericUpDownMotorCycles.Value;
            pMSS.MotorTimeout = (int)this.numericUpDownMotorTimeout.Value;
        }

        private const string _savePath = "PumpModeSavedStates.sav";

        private void writeSaveObjecToFile()
        {
            Stream stream = System.IO.File.Open(_savePath, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();

            bFormatter.Serialize(stream, this.saveObject);
            stream.Close();
        }

        public PumpModeSavedStateV2 GetSavedState()
        {
            PumpModeSavedStateV2 pMSS = new PumpModeSavedStateV2();
            pMSS = this.getAllValues();
            return pMSS;
        }

        private PumpModeSavedStateV2 getAllValues()
        {
            PumpModeSavedStateV2 pMSS = new PumpModeSavedStateV2();
            pMSS.CycleLimit = (int)this.numericUpDownCycleLimit.Value;
            pMSS.NeverReclose = this.checkBoxNeverReclose.Checked;
            pMSS.PumpProtectTime = (int)this.numericUpDownProtectTime.Value;
            pMSS.PumpTime = (int)this.numericUpDownPumpTime.Value;
            pMSS.EnableMotorCycle = this.checkBoxMotorCycles.Checked;
            pMSS.EnableMotorTimeout = this.checkBoxMotorTime.Checked;
            pMSS.EnableRelayCycle = this.checkBoxCycles.Checked;
            pMSS.MotorCycleLimit = (byte)this.numericUpDownMotorCycles.Value;
            pMSS.MotorTimeout = (int)this.numericUpDownMotorTimeout.Value;

            return pMSS;
        }

        private void updatePumpReasonLable(PumpReasons value)
        {
            switch (value)
            {
                case PumpReasons.NoPump:
                    this.labelPumpTypeDisplay.BackColor = Color.Transparent;
                    this.labelPumpTypeDisplay.Text = "No Problems"; //"No Pumping Problems";
                    pumpOK.pumpStatus = true;
                    break;
                case PumpReasons.MotorPump:
                    this.labelPumpTypeDisplay.BackColor = Color.Orange;
                    this.labelPumpTypeDisplay.Text = "Motor Cycles";
                    pumpOK.pumpStatus = false;
                    break;
                case PumpReasons.MotorTimeout:
                    this.labelPumpTypeDisplay.BackColor = Color.Orange;
                    this.labelPumpTypeDisplay.Text = "Motor Timeout";
                    pumpOK.pumpStatus = false;
                    break;
                case PumpReasons.RelayCallLimit:
                    this.labelPumpTypeDisplay.BackColor = Color.Orange;
                    this.labelPumpTypeDisplay.Text = "Cycle Limit";
                    pumpOK.pumpStatus = false;
                    break;
                default:
                    throw new Exception(value.ToString() + " is not a handled Pump Reason");
            }
        }
    }

    [Serializable()]

    public class PumpModeSavedState : ISerializable
    {
        public PumpModeSavedState()
        {
        }

        public string Name;
        public bool EnablePumpProtect;
        public bool NeverReclose;
        public int CycleLimit;
        public int PumpTime;
        public int PumpProtectTime;

        public PumpModeSavedState(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.EnablePumpProtect = (bool)info.GetValue("Enable Pump Protect", typeof(bool));
                this.NeverReclose = (bool)info.GetValue("Never Reclose", typeof(bool));
                this.CycleLimit = (int)info.GetValue("Cycle Limit", typeof(int));
                this.PumpTime = (int)info.GetValue("Pump Time", typeof(int));
                this.PumpProtectTime = (int)info.GetValue("Pump Protect Time", typeof(int));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Pump Mode Saved State v1", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", Name);
                info.AddValue("Enable Pump Protect", EnablePumpProtect);
                info.AddValue("Never Reclose", NeverReclose);
                info.AddValue("Cycle Limit", CycleLimit);
                info.AddValue("Pump Time", PumpTime);
                info.AddValue("Pump Protect Time", PumpProtectTime);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Getting Object Data in Pump Mode Saving", ex);
            }
        }

    }

    [Serializable()]

    public class PumpModeSaveObject : ISerializable
    {
        public PumpModeSaveObject()
        {
        }
        public List<PumpModeSavedState> SavedStates = new List<PumpModeSavedState>();

        public PumpModeSaveObject(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.SavedStates = (List<PumpModeSavedState>)info.GetValue("Saved States", typeof(List<PumpModeSavedState>));
            }
            catch
            {
                this.SavedStates = null;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Saved States", this.SavedStates);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Saving Data In Pump Mode Settings.", ex);
            }
        }

        public void AddSavedState(PumpModeSavedState pMSS)
        {
            int i = 0;

            for (; i < SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name == pMSS.Name || this.SavedStates[i].Name == null)
                {
                    this.SavedStates[i] = pMSS;
                    break;
                }
            }

            if (i == SavedStates.Count)
            {
                this.SavedStates.Add(pMSS);
            }

            this.SavedStates.Sort(delegate (PumpModeSavedState pMSS1, PumpModeSavedState pMSS2) { return pMSS1.Name.CompareTo(pMSS2.Name); });
        }

        private bool sameName(PumpModeSavedState pMSS, string s)
        {
            if (pMSS.Name == s)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveSavedState(PumpModeSavedState pMSS)
        {
            if (this.SavedStates == null)
                return;
            this.SavedStates.Remove(pMSS);
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

    public class PumpModeSavedStateV2 : ISerializable
    {
        public PumpModeSavedStateV2()
        {
        }

        public string Name;
        public bool NeverReclose;
        public int CycleLimit;
        public int PumpTime;
        public int PumpProtectTime;
        public int MotorTimeout;
        public byte MotorCycleLimit;
        public bool EnableRelayCycle;
        public bool EnableMotorTimeout;
        public bool EnableMotorCycle;

        public PumpModeSavedStateV2(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.MotorTimeout = (int)info.GetValue("Motor Timeout", typeof(int));
                this.NeverReclose = (bool)info.GetValue("Never Reclose", typeof(bool));
                this.CycleLimit = (int)info.GetValue("Cycle Limit", typeof(int));
                this.PumpTime = (int)info.GetValue("Pump Time", typeof(int));
                this.PumpProtectTime = (int)info.GetValue("Pump Protect Time", typeof(int));
                this.EnableRelayCycle = (bool)info.GetValue("Enable Relay Cycle", typeof(bool));
                this.EnableMotorTimeout = (bool)info.GetValue("Enable Motor Timeout", typeof(bool));
                this.EnableMotorCycle = (bool)info.GetValue("Enable Motor Cycle", typeof(bool));
                this.MotorCycleLimit = (byte)info.GetValue("Motor Cycle Limit", typeof(byte));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Pump Mode Saved State V2", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", Name);
                info.AddValue("Never Reclose", NeverReclose);
                info.AddValue("Cycle Limit", CycleLimit);
                info.AddValue("Pump Time", PumpTime);
                info.AddValue("Pump Protect Time", PumpProtectTime);
                info.AddValue("Enable Relay Cycle", this.EnableRelayCycle);
                info.AddValue("Enable Motor Timeout", this.EnableMotorTimeout);
                info.AddValue("Enable Motor Cycle", this.EnableMotorCycle);
                info.AddValue("Motor Timeout", this.MotorTimeout);
                info.AddValue("Motor Cycle Limit", this.MotorCycleLimit);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Getting Object Data in Pump Mode Saving", ex);
            }
        }
    }

    [Serializable()]

    public class PumpModeSaveObjectV2 : ISerializable
    {
        public PumpModeSaveObjectV2()
        {
        }

        public List<PumpModeSavedStateV2> SavedStates = new List<PumpModeSavedStateV2>();

        public PumpModeSaveObjectV2(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.SavedStates = (List<PumpModeSavedStateV2>)info.GetValue("Saved States", typeof(List<PumpModeSavedState>));
            }
            catch
            {
                this.SavedStates = null;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Saved States", this.SavedStates);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Saving Data In Pump Mode Settings.", ex);
            }
        }

        public void AddSavedState(PumpModeSavedStateV2 pMSS)
        {
            int i = 0;

            for (; i < SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name == pMSS.Name || this.SavedStates[i].Name == null)
                {
                    this.SavedStates[i] = pMSS;
                    break;
                }
            }

            if (i == SavedStates.Count)
            {
                this.SavedStates.Add(pMSS);
            }

            this.SavedStates.Sort(delegate (PumpModeSavedStateV2 pMSS1, PumpModeSavedStateV2 pMSS2) { return pMSS1.Name.CompareTo(pMSS2.Name); });
        }

        private bool sameName(PumpModeSavedStateV2 pMSS, string s)
        {
            if (pMSS.Name == s)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveSavedState(PumpModeSavedStateV2 pMSS)
        {
            if (this.SavedStates == null)
                return;
            this.SavedStates.Remove(pMSS);
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
