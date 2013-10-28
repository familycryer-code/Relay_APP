using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace RelayControlLibrary
{
    public partial class ucSafeService : UserControl
    {
        public ucSafeService()
        {
            InitializeComponent();
            this.setCTRatioValues(this.cTRatio);
            this.domainUpDownDataViews.Items.AddRange(dataViews);
            this.domainUpDownDataViews.SelectedIndex = 0;
            toolTip.SetToolTip(this.numericUpDownCurrentImbalance, "Ratio between the Negative Sequence and the Positive Sequence of the Currents");
            toolTip.SetToolTip(this.numericUpDownDelay, "Number of cycles to Delay after an Arc Flash has been detected before tripping");
            toolTip.SetToolTip(this.numericUpDownLowVoltage, "Positive Sequence of the Network Voltages");
            toolTip.SetToolTip(this.numericUpDownOverCurrent, "Current required in any phase (or ground) before looking at /rCurrent Imbalance, Low Voltage and Voltage Imbalance");
            toolTip.SetToolTip(this.numericUpDownVoltageImbalance, "Negative Sequence of the Network Voltages");
            toolTip.SetToolTip(this.comboBoxSSEnable, "Enable or Disable the mode");
            toolTip.SetToolTip(this.domainUpDownDataViews, "Selects the way the values are viewed in the GUI");
        }

        public bool LoadingNewCode
        {
            get { return this.loadingNewCode; }
            set
            {
                this.loadingNewCode = value;
                // Set To Disabled if Loading New Code
                if (value)
                    this.comboBoxSSEnable.SelectedIndex = 1; 
            }
        }

        private bool loadingNewCode = false;

        #region Send And Receive

        private void send()
        {
            SendEventArgs sEA = new SendEventArgs(22);
            uint tempInt;

            decimal tempValue;
            switch (this.domainUpDownDataViews.SelectedIndex)
            {
                case 0:
                default:
                    tempValue = this.numericUpDownOverCurrent.Value;
                    break;
                case 1:
                    tempValue = this.numericUpDownOverCurrent.Value / (decimal)this.CTRatio;
                    break;
                case 2:
                    tempValue = this.numericUpDownOverCurrent.Value * 20m;
                    break;
            }
            try
            {
                //low byte comes first
                sEA.SendPacket[0] = 0x0F;    
                if(this.comboBoxSSEnable.SelectedIndex == 0)
                    sEA.SendPacket[2] = 1;
                else
                    sEA.SendPacket[2] = 0;
                sEA.SendPacket[1] = 0;
                tempInt = RelayModeFunctions.ConvertTo6_10(tempValue);//this.numericUpDownOverCurrent.Value / (decimal)this.cTRatio);
                sEA.SendPacket[3] = (byte)(tempInt >> 8);
                sEA.SendPacket[4] = (byte)tempInt;
                tempInt = RelayModeFunctions.ConvertTo8_8(this.numericUpDownCurrentImbalance.Value);
                sEA.SendPacket[5] = (byte)(tempInt >> 8);
                sEA.SendPacket[6] = (byte)tempInt;
                tempInt = (uint)this.numericUpDownDelay.Value;
                sEA.SendPacket[7] = (byte)(tempInt >> 8);
                sEA.SendPacket[8] = (byte)tempInt;
                tempInt = RelayModeFunctions.ConvertTo8_8(this.numericUpDownLowVoltage.Value);
                sEA.SendPacket[9] = (byte)(tempInt >> 8);
                sEA.SendPacket[10] = (byte)tempInt;
                tempInt = RelayModeFunctions.ConvertTo8_8(this.numericUpDownVoltageImbalance.Value);
                sEA.SendPacket[11] = (byte)(tempInt >> 8);
                sEA.SendPacket[12] = (byte)tempInt;
                sEA.SendPacket[21] = 0x0D;
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Generating Safe Service Send Settings", ex);
                this.restoreDefaults();
            }

            try
            {
                if (this.Send != null)
                {
                    this.Send(this, sEA);
                }
                else
                    throw new Exception("Safe Service Send Event Not Handled");
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Sending Safe Service Settings", ex);
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            this.send();
        }

        public void SendAll()
        {
            this.send();
        }
        public delegate void SendHandler(object o, SendEventArgs sEA);
        public event SendHandler Send;

        private void buttonRequest_Click(object sender, EventArgs e)
        {
            SendEventArgs sEA = new SendEventArgs(3);
            try
            {
                if (this.Send != null)
                {
                    sEA.SendPacket[0] = 0x0E;
                    //sEA.SendPacket[1] = 0x55;
                    sEA.SendPacket[1] = 0x0D;
                    this.Send(this, sEA);
                }
                else
                    throw new Exception("Safe Service Send Event Not Handled");
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Requesting Safe Service Settings", ex);
            }
        }

        private delegate void setAllCallBack(byte[] bytePacket);
        public void SetAll(byte[] bytePacket)
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
                if (this.LoadingNewCode)
                    this.restoreDefaults();
                else
                    this.errorHandler("Error in SetAll", ex);
            }
        }
        private void setAll(byte[] bytePacket)
        {
            uint tempI;
            try
            {
                if((bytePacket[1] & 0x01) == 1)
                    this.comboBoxSSEnable.SelectedIndex = 0;
                else
                    this.comboBoxSSEnable.SelectedIndex = 1;

                tempI = bytePacket[2];
                tempI <<= 8;
                tempI += bytePacket[3];

                switch (this.domainUpDownDataViews.SelectedIndex)
                {
                    case 0:
                    default:
                        this.numericUpDownOverCurrent.Value = RelayModeFunctions.ConvertFrom6_10(tempI);
                        break;
                    case 1:
                        this.numericUpDownOverCurrent.Value = RelayModeFunctions.ConvertFrom6_10(tempI * (uint)this.CTRatio);
                        break;
                    case 2:
                        this.numericUpDownOverCurrent.Value = RelayModeFunctions.ConvertFrom6_10(tempI * 20);
                        break;
                }

                tempI = bytePacket[4];
                tempI <<= 8;
                tempI += bytePacket[5];

                this.numericUpDownCurrentImbalance.Value = RelayModeFunctions.ConvertFrom8_8(tempI);

                tempI = bytePacket[6];
                tempI <<= 8;
                tempI += bytePacket[7];

                this.numericUpDownDelay.Value = tempI;

                tempI = bytePacket[8];
                tempI <<= 8;
                tempI += bytePacket[9];

                this.numericUpDownLowVoltage.Value = RelayModeFunctions.ConvertFrom8_8(tempI);

                tempI = bytePacket[10];
                tempI <<= 8;
                tempI += bytePacket[11];

                this.numericUpDownVoltageImbalance.Value = RelayModeFunctions.ConvertFrom8_8(tempI);
            }
            catch (Exception ex)
            {
                if (this.LoadingNewCode)
                    this.restoreDefaults();
                else
                    this.errorHandler("Error in setAll", ex);
            }
        }

        #endregion

        #region Events

        public delegate void ErrorHandler(object o, ExceptionEventArgs eEA);
        public event ErrorHandler SafeServiceException;

        private void errorHandler(string p, Exception ex)
        {
            if(SafeServiceException != null)
                SafeServiceException(this, new ExceptionEventArgs(ex, p));
            else
                throw new Exception("Exceptions for SS Not Handled!!!!");
        }


        private int cTRatio = 320;
        public int CTRatio
        {
            get { return this.cTRatio; }
            set
            {
                this.setCTRatioValues(value);
            }
        }

        private void setCTRatioValues(int value)
        {
            decimal tempValue = this.numericUpDownOverCurrent.Value;
            
            tempValue /= this.cTRatio;
            tempValue *= value;
            this.cTRatio = value;

            // If it is set to "Protector"
            if (this.domainUpDownDataViews.SelectedIndex == 1)
            {
                this.numericUpDownOverCurrent.Value = 5;
                this.numericUpDownOverCurrent.Maximum = this.cTRatio * 5 * 6;
                this.numericUpDownOverCurrent.Value = tempValue;
            }
            // Else we just leave it alone
        }
        #endregion

        List<string> dataViews = new List<string>()
        {
            "Relay",
            "Protector",
            "Percent"
        };

        private ToolTip toolTip = new ToolTip();
        private int previousSelectedItem = 0;

        private void domainUpDownDataViews_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                decimal currentValue;

                switch (previousSelectedItem)
                {
                    case 0:
                    default:
                        currentValue = this.numericUpDownOverCurrent.Value;
                        break;
                    case 1:
                        currentValue = this.numericUpDownOverCurrent.Value / (decimal)this.CTRatio;
                        break;
                    case 2:
                        currentValue = this.numericUpDownOverCurrent.Value / 20m;
                        break;
                }

                this.numericUpDownOverCurrent.Value = 0;

                switch (this.domainUpDownDataViews.SelectedIndex)
                {
                    case 0:
                    default:
                        this.numericUpDownOverCurrent.Maximum = 10;
                        this.numericUpDownOverCurrent.Minimum = 0;
                        this.numericUpDownOverCurrent.DecimalPlaces = 1;
                        this.numericUpDownOverCurrent.Value = currentValue;
                        previousSelectedItem = 0;
                        break;
                    case 1:
                        // 2 * 5 * this.CTRatio should be the maxium setting in box.
                        this.numericUpDownOverCurrent.Maximum = 10 * this.CTRatio;
                        this.numericUpDownOverCurrent.Minimum = 0;
                        this.numericUpDownOverCurrent.DecimalPlaces = 0;
                        this.numericUpDownOverCurrent.Value = currentValue * (decimal)this.CTRatio;
                        previousSelectedItem = 1;
                        break;
                    case 2:
                        this.numericUpDownOverCurrent.Maximum = 200;
                        this.numericUpDownOverCurrent.Minimum = 0;
                        this.numericUpDownOverCurrent.DecimalPlaces = 2;
                        this.numericUpDownOverCurrent.Value = currentValue * 20m;
                        previousSelectedItem = 2;
                        break;
                }
            }
            catch (Exception ex)
            {
                ex = new Exception("Error Switching Views");
                this.errorHandler("Error Switching Views In Safe Service", ex);
            }
        }

        #region Saved States

        private SaveObject saveObject = new SaveObject();

        private void populateSafeServiceSavedData(SafeServiceSavedState sSSS)
        {
            try
            {
                // Not visible means safe service is not enabled.
                if (!this.Visible)
                    this.restoreDefaults();

                if (this.comboBoxSSEnable.SelectedIndex == 0)
                    sSSS.Enabled = true;
                else
                    sSSS.Enabled = false;

                sSSS.Delay = (int)this.numericUpDownDelay.Value;
                sSSS.LowVoltage = this.numericUpDownLowVoltage.Value;
                sSSS.VoltageImbalance = this.numericUpDownVoltageImbalance.Value;
                sSSS.CurrentImbalance = this.numericUpDownCurrentImbalance.Value;

                switch (this.domainUpDownDataViews.SelectedIndex)
                {
                    case 0:
                    default:
                        sSSS.OverCurrent = this.numericUpDownOverCurrent.Value;
                        break;
                    case 1:
                        // 2 * 5 * this.CTRatio should be the maxium setting in box.
                        sSSS.OverCurrent = this.numericUpDownOverCurrent.Value / (decimal)this.CTRatio;
                        break;
                    case 2:
                        sSSS.OverCurrent = this.numericUpDownOverCurrent.Value / 20m;
                        break;
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Setting Save Object in Safe Service Mode", ex);
            }
            
        }

        public void SetAllValues(SafeServiceSavedState sSSS)
        {
            try
            {
                if (sSSS.OverCurrent == 0)
                {
                    this.restoreDefaults();
                    return;
                }
                switch (this.domainUpDownDataViews.SelectedIndex)
                {
                    case 0:
                    default:
                        this.numericUpDownOverCurrent.Value = sSSS.OverCurrent;
                        break;
                    case 1:
                        this.numericUpDownOverCurrent.Value = sSSS.OverCurrent * (decimal)this.CTRatio;
                        break;
                    case 2:
                        this.numericUpDownOverCurrent.Value = sSSS.OverCurrent * 20m;
                        break;
                }
                this.numericUpDownCurrentImbalance.Value = sSSS.CurrentImbalance;
                this.numericUpDownDelay.Value = sSSS.Delay;
                this.numericUpDownLowVoltage.Value = sSSS.LowVoltage;
                this.numericUpDownVoltageImbalance.Value = sSSS.VoltageImbalance;
                if (sSSS.Enabled)
                    this.comboBoxSSEnable.SelectedIndex = 0;
                else
                    this.comboBoxSSEnable.SelectedIndex = 1;

            }
            catch (Exception ex)
            {
                this.errorHandler("Error Recalling Saved Values in Safe Service Mode", ex);
            }

        }

        public SafeServiceSavedState GetSavedState()
        {
            SafeServiceSavedState sSSS = new SafeServiceSavedState();
            this.populateSafeServiceSavedData(sSSS);
            return sSSS;
        }
        #endregion

        private void buttonRestoreDefaults_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you really want to restore default Safe Service Settings?", "Restore Safe Service Defaults", MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
                this.restoreDefaults();
        }

        private void restoreDefaults()
        {
            this.comboBoxSSEnable.SelectedIndex = 1; // 1 - Disable
            this.numericUpDownCurrentImbalance.Value = 0.8m;
            this.numericUpDownDelay.Value = 0;
            this.numericUpDownLowVoltage.Value = 95m;
            this.numericUpDownOverCurrent.Value = 10.0m;
            this.numericUpDownVoltageImbalance.Value = 10.0m;
            //if (this.LoadingNewCode)
            //{
            //    this.LoadingNewCode = false;
            //    this.SendAll();
            //}
        }

        public void SetDefaults()
        {
            this.restoreDefaults();
        }

        public void BreakSettings()
        {
            this.restoreDefaults();
            this.SendAll();
        }
    }

    [Serializable()]

    public class SafeServiceSavedState : ISerializable
    {
        public SafeServiceSavedState()
        {
            this.Enabled = false;
            this.OverCurrent = 10m;
            this.CurrentImbalance = 0.8m;
            this.Delay = 0;
            this.VoltageImbalance = 10m;
            this.LowVoltage = 95m;
        }

        public string Name;
        public bool Enabled;
        public decimal OverCurrent;
        public decimal CurrentImbalance;
        public decimal VoltageImbalance;
        public decimal LowVoltage;
        public int Delay;

        public SafeServiceSavedState(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.Enabled = (bool)info.GetValue("Enabled", typeof(bool));
                this.OverCurrent = (Decimal)info.GetValue("Over Current", typeof(decimal));
                this.CurrentImbalance = (Decimal)info.GetValue("Current Imbalance", typeof(decimal));
                this.VoltageImbalance = (Decimal)info.GetValue("Voltage Imbalance", typeof(decimal));
                this.LowVoltage = (Decimal)info.GetValue("Low Voltage", typeof(decimal));
                this.Delay = (int)info.GetValue("Delay", typeof(int));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Safe Service Saved State", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", Name);
                info.AddValue("Enabled", this.Enabled);
                info.AddValue("Over Current", this.OverCurrent);
                info.AddValue("Current Imbalance", this.CurrentImbalance);
                info.AddValue("Voltage Imbalance", this.VoltageImbalance);
                info.AddValue("Low Voltage", this.LowVoltage);
                info.AddValue("Delay", this.Delay);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Getting Object Data in Safe Service Saving", ex);
            }
        }
    }

    public class SafeServicceSaveObjectV4 : ISerializable
    {
        public SafeServicceSaveObjectV4()
        {
        }

        //public int NumberOfObjects;
        public List<SafeServiceSavedState> SavedStates = new List<SafeServiceSavedState>();

        public SafeServicceSaveObjectV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                //this.NumberOfObjects = (int)info.GetValue("Number Of Objects", typeof(int));
                this.SavedStates = (List<SafeServiceSavedState>)info.GetValue("Saved States", typeof(List<SafeServiceSavedState>));
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
                throw new Exception("Error Saving Data In Safe Service Settings.", ex);
            }
        }

        public void AddSavedState(SafeServiceSavedState tSS)
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
            this.SavedStates.Sort(delegate(SafeServiceSavedState tSS1, SafeServiceSavedState tSS2) { return tSS1.Name.CompareTo(tSS2.Name); });
        }

        private bool sameName(SafeServiceSavedState tSS, string s)
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

        public void RemoveSavedState(SafeServiceSavedState tSS)
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
