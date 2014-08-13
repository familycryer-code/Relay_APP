using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucDNP : UserControl
    {
        public ucDNP()
        {
            InitializeComponent();
        }
        public delegate void SendEventHandler(SendEventArgs sEA);
        public event SendEventHandler Send;
        public delegate void ExceptionHandler(Exception ex);
        public event ExceptionHandler DNPControlException;
        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                this.customer = value;
                this.setCustomer();
            }
        }
        private Customers customer;
        private List<ucDeadBandSettingsObject> deadBandVariables = new List<ucDeadBandSettingsObject>();

#if ATLANTA
        private static int _packetLength = 42;
#else
        private static int _packetLength = 98;
#endif

        #region Send Functions

        private void buttonSendAllDNPSettings_Click(object sender, EventArgs e)
        {
            try
            {
                SendEventArgs sEA = new SendEventArgs(_packetLength);
                byte tempByte = 0;
                UInt32 tempInt32;

                sEA.SendPacket[0] = (byte)RelayModeFunctions._DNPControlOpCode;
                sEA.SendPacket[1] = (byte)'a';        //For set all

                //Setting the command bits 0 - 6
                if((string)this.comboBoxLinkLayerConfirm.SelectedItem == "Always")
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
                
                if(this.Customer == Customers.Memphis)
                {
                    tempByte &= 0x1F; //Clear the Memphis Stage Bits
                    tempByte |= (byte)((int)this.numericUpDownMemphisStage.Value << 5); //Set them
                }

                sEA.SendPacket[3] = tempByte;

                // Baud Rate bottom 3 bits of next byte
                tempByte = 0;

                tempByte |= (byte)this.comboBoxBaudRate.SelectedIndex;

                sEA.SendPacket[4] = tempByte;
                
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

                //Event Trigger Ranges

                sEA.SendPacket[sEA.SendPacket.Length - 1] = 0x0D;
                
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
                SendEventArgs sEA = new SendEventArgs(_packetLength);
                uint index = 2;

                sEA.SendPacket[0] = (byte)RelayModeFunctions._DNPControlOpCode;
                sEA.SendPacket[1] = (byte)'d';        //For set deadband limits

                foreach(Control c in this.groupBoxDigitalGridDNPDeadBand.Controls)
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

                    if(!failed)
                    {
                        sEA.SendPacket[index] = (byte)uDDB.Value;
                        index++;
                        if(index >= 41)
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

        private void buttonSendMemphis_Click(object sender, EventArgs e)
        {
            try
            {
                SendEventArgs sEA = new SendEventArgs(_packetLength);

                sEA.SendPacket[0] = (byte)RelayModeFunctions._DNPControlOpCode;
                sEA.SendPacket[1] = (byte)'d';        //For set deadband limits

                //Event Trigger Ranges
                if(this.Customer == Customers.Memphis)
                {
                    UInt16 temp;
                    temp = (UInt16)this.numericUpDownTriggerRangeVoltage.Value;

                    sEA.SendPacket[3] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[4] = (byte)(temp & 0xFF);

                    temp = (UInt16)(this.numericUpDownTriggerRangeTHD.Value * 10);
                    sEA.SendPacket[5] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[6] = (byte)(temp & 0xFF);

                    temp = (UInt16)this.numericUpDownTriggerRangeCurrent.Value;
                    sEA.SendPacket[7] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[8] = (byte)(temp & 0xFF);

                    temp = (UInt16)this.numericUpDownTriggerRangeTemperature.Value;
                    sEA.SendPacket[9] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[10] = (byte)(temp & 0xFF);

                    temp = (UInt16)this.numericUpDownOdometer.Value;
                    sEA.SendPacket[11] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[12] = (byte)(temp & 0xFF);

                    temp = (UInt16)(this.numericUpDownDifferentialVoltsDB.Value * 10);
                    sEA.SendPacket[13] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[14] = (byte)(temp & 0xFF);

                    temp = (UInt16)(this.numericUpDownDifferentialVoltsRealDB.Value * 10);
                    sEA.SendPacket[15] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[16] = (byte)(temp & 0xFF);

                    temp = (UInt16)(this.numericUpDownCurrentAngleDB.Value * 10);
                    sEA.SendPacket[17] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[18] = (byte)(temp & 0xFF);

                    temp = (UInt16)this.numericUpDownPhaseKWDB.Value;
                    sEA.SendPacket[19] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[20] = (byte)(temp & 0xFF);

                    temp = (UInt16)this.numericUpDownPhaseKVARDB.Value;
                    sEA.SendPacket[21] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[22] = (byte)(temp & 0xFF);

                    temp = (UInt16)this.numericUpDownPhaseKVADB.Value;
                    sEA.SendPacket[23] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[24] = (byte)(temp & 0xFF);

                    temp = (UInt16)this.numericUpDownTotalKWDB.Value;
                    sEA.SendPacket[25] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[26] = (byte)(temp & 0xFF);

                    temp = (UInt16)this.numericUpDownTotalKVAVARDB.Value;
                    sEA.SendPacket[27] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[28] = (byte)(temp & 0xFF);

                    temp = (UInt16)(this.numericUpDownAnalog1DeadBand.Value * 100);
                    sEA.SendPacket[29] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[30] = (byte)(temp & 0xFF);

                    temp = (UInt16)(this.numericUpDownAnalog2DeadBand.Value * 100);
                    sEA.SendPacket[31] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[32] = (byte)(temp & 0xFF);

                    temp = (UInt16)(this.numericUpDownAnalog3DeadBand.Value * 100);
                    sEA.SendPacket[33] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[34] = (byte)(temp & 0xFF);

                    temp = (UInt16)(this.numericUpDownAnalog4DeadBand.Value * 100);
                    sEA.SendPacket[35] = (byte)(temp & 0xFF);
                    temp >>= 8;
                    sEA.SendPacket[36] = (byte)(temp & 0xFF);

                }
                else
                {
                    this.errorHandler(new Exception("No Event Ranges Defined For This Customer"));
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
                DNPControlException(ex);
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
                if(this.InvokeRequired)
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
            try
            {
                temp = (byte)(bytePacket[0] & 3);
                switch(temp)
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
                this.errorHandler(new Exception("Error Setting Link Layer Confirm", ex));
                return;
            }

            try
            {
                //Self Address
                temp = (byte)(bytePacket[0] & 4);
                if(temp == 4)
                    this.comboBoxSelfAddress.SelectedItem = "Enable";
                else
                    this.comboBoxSelfAddress.SelectedItem = "Disable";
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error Setting Self Address", ex));
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
                this.errorHandler(new Exception("Error Setting Unsolicited Allowed", ex));
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
                this.errorHandler(new Exception("Error Setting Resistor Termination", ex));
                return;
            }

            try
            {
                if(this.Customer == Customers.Memphis)
                {
                    temp = (byte)(bytePacket[0] & 0xE0);
                    temp >>= 5;
                    this.numericUpDownMemphisStage.Value = temp;
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error Setting Memphis Stage", ex));
                return;
            }

            try
            {
                temp = (byte)(bytePacket[1] & 0x07);
                this.comboBoxBaudRate.SelectedIndex = temp;
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error Setting Baud Rate", ex));
                return;
            }

            //bytes 2 & 3???

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
                this.errorHandler(new Exception("Error Setting MSB unsoltimeout", ex));
                return;
            }
            
            //8 9 = Fragment Size
            try
            {
                UInt16 tempInt = bytePacket[9];
                tempInt <<= 8;
                tempInt += bytePacket[8];

                this.numericUpDownFragmentSize.Value = tempInt;
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error Setting Fragment Size", ex));
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
                this.errorHandler(new Exception("Error Setting Destination Address", ex));
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
                this.errorHandler(new Exception("Error Setting Source Address", ex));
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
                this.errorHandler(new Exception("Error Setting Unsolicited Max Retries", ex));
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
                this.errorHandler(new Exception("Error Setting Max Events", ex));
                return;
            }

            try
            {
                if(this.Customer == Customers.Memphis)
                {
                    //starting at 37
                    this.numericUpDownTriggerRangeVoltage.Value = bytePacket[38] + bytePacket[39] * 256;
                    this.numericUpDownTriggerRangeTHD.Value = (decimal)(bytePacket[40] + bytePacket[41] * 256) / 10m;
                    this.numericUpDownTriggerRangeCurrent.Value = bytePacket[42] + bytePacket[43] * 256;
                    this.numericUpDownTriggerRangeTemperature.Value = bytePacket[44] + bytePacket[45] * 256;
                    this.numericUpDownOdometer.Value = bytePacket[46] + bytePacket[47] * 256;
                    this.numericUpDownDifferentialVoltsDB.Value = (decimal)(bytePacket[48] + bytePacket[49] * 256) / 10m;
                    this.numericUpDownDifferentialVoltsRealDB.Value = (decimal)(bytePacket[50] + bytePacket[51] * 256) / 10m;
                    this.numericUpDownCurrentAngleDB.Value = (decimal)(bytePacket[52] + bytePacket[53] * 256) / 10m;
                    this.numericUpDownPhaseKWDB.Value = bytePacket[54] + bytePacket[55] * 256;
                    this.numericUpDownPhaseKVARDB.Value = bytePacket[56] + bytePacket[57] * 256;
                    this.numericUpDownPhaseKVADB.Value = bytePacket[58] + bytePacket[59] * 256;
                    this.numericUpDownTotalKWDB.Value = bytePacket[60] + bytePacket[61] * 256;
                    this.numericUpDownTotalKVAVARDB.Value = bytePacket[62] + bytePacket[63] * 256;
                    this.numericUpDownAnalog1DeadBand.Value = (decimal)(bytePacket[64] + bytePacket[65] * 256) / 100m;
                    this.numericUpDownAnalog2DeadBand.Value = (decimal)(bytePacket[66] + bytePacket[67] * 256) / 100m;
                    this.numericUpDownAnalog3DeadBand.Value = (decimal)(bytePacket[68] + bytePacket[69] * 256) / 100m;
                    this.numericUpDownAnalog4DeadBand.Value = (decimal)(bytePacket[70] + bytePacket[71] * 256) / 100m;
                    
                }
                else
                {
                    ucDNPDeadBand uDDB = new ucDNPDeadBand();
                    uint index = 30;

                    foreach(Control C in this.groupBoxDigitalGridDNPDeadBand.Controls)
                    {
                        bool failed = false;

                        try{ uDDB = (ucDNPDeadBand)C;   }
                        catch { failed = true; }
                        
                        if(!failed)
                        {
                            uDDB.Value = bytePacket[index];
                            index++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error Setting Trigger Ranges", ex));
            }
        }
        #endregion

        #region Customer Handlers
        private void setCustomer()
        {
            if(this.Customer == Customers.Memphis)
                this.makeMemphis();
            else
                this.makeDefault();
        }

        private void makeDefault()
        {
            this.numericUpDownMemphisStage.Visible = false;
            this.labelMemphisStage.Visible = false;

            if(this.Customer != Customers.DigitalGridDNP || this.deadBandVariables.Count == 0)
            {
                this.deadBandVariables.Clear();
                this.groupBoxDigitalGridDNPDeadBand.Controls.Clear();

                this.groupBoxMemphisDeadBand.Visible = false;
                
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Voltage", "V", 0.0m, 255m, "Applies to all Network and Transformer Voltages"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Voltage Angle", "Degrees", 0, 180));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Apparent Diff Voltage", ".1 V", 0, 255, "Applies to all three Differential Voltages in 0.1 Volt steps"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Diff Voltage Angle", "Degrees", 0, 180, "Applies to all three Differential Voltages in 0.1 Volt steps"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Apparent Diff Voltage", ".1 V", 0, 255));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Diff Angle", "Degrees", 0, 180));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Real Diff Voltage", ".1 V", 0, 255, "Applies to all three Differential Voltages in 0.1 Volt steps"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Real Diff Voltage", ".1 V", 0, 255, "In 0.1 Volt steps"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Current", "* 10 Amps", 0m, 255m, "Applies to all three Phase Currents"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Current Angle", "Degrees", 0, 180, "Applies to all three Phase Currents"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Effective Current", "* 10 Amps", 0m, 255m));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Effective Current Angle", "Degrees", 0, 180));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Pos Seq Current", "* 10 Amps", 0m, 255m));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Pos Seq Current Angle", "Degrees", 0, 180));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Neg Seq Current", "* 10 Amps", 0m, 255m));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Neg Seq Current Angle", "Degrees", 0, 180));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Apparent Power", "kVA", 0m, 255m, "Applies to all three Apparent Powers"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Apparent Power Angle", "Degrees", 0, 180, "Applies to all three Apparent Powers"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Apparent Power", "kVA", 0m, 255m));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Apparent Power Angle", "Degrees", 0, 180));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Real Power", "kVA", 0m, 255m, "Applies to all three Real Powers"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Avg Real Power", "kVA", 0m, 255m));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Diff Pos Seq Voltage", "* .1 V", 0m, 255m));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Diff Pos Seq Voltage Angle", "Degrees", 0, 180));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Diff Neg Seq Voltage", "* .1 V", 0m, 255m));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Diff Neg Seq Voltage Angle", "Degrees", 0, 180));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Votlage Pos Seq", "V", 0m, 255m, "Applies to both Network and Transformer Sets"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Voltage Pos Seq Angle", "Degrees", 0, 180m, "Applies to both Network and Transformer Sets"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Voltage Neg Seq", "V", 0m, 255m, "Applies to both Network and Transformer Sets"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Voltage Neg Seq Angle", "Degrees", 0, 180, "Applies to both Network and Transformer Sets"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Voltage THD", "* .1 %", 0m, 255m, "Applies to all three Voltage THDs"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Current THD", "* .1 %", 0m, 255m, "Applies to all three Current THDs"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Temperature", "Degrees C", 0m, 255m));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Relay Odometer", "Cycles", 0m, 255m));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog 1", "* .1 Volts", 0m, 50m, "Voltage input from 0-5 volts in 0.1 V steps"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog 2", "* .1 Volts", 0m, 50m, "Voltage input from 0-5 volts in 0.1 V steps"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog 3", "* .1 Volts", 0m, 50m, "Voltage input from 0-5 volts in 0.1 V steps"));
                this.deadBandVariables.Add(new ucDeadBandSettingsObject("Analog 4", "* .1 Volts", 0m, 50m, "Voltage input from 0-5 volts in 0.1 V steps"));
                
                Point location = new Point();
                ucDNPDeadBand workingDDB = new ucDNPDeadBand();

                location.Y = this.groupBoxDNPSettings.Location.Y;
                location.X = this.groupBoxDNPSettings.Location.X + this.groupBoxDNPSettings.Width + 2;

                this.groupBoxDigitalGridDNPDeadBand.Location = location;
                this.groupBoxDigitalGridDNPDeadBand.Height = 0;

                location = new Point(2,15);//Now make location the starting spot of the first control

                foreach(ucDeadBandSettingsObject dBD in this.deadBandVariables)
                {
                    workingDDB = new ucDNPDeadBand(dBD);
                    workingDDB.Location = location;
                    this.groupBoxDigitalGridDNPDeadBand.Controls.Add(workingDDB);

                    if(this.deadBandVariables.IndexOf(dBD) >= (this.deadBandVariables.Count / 2) - 1 && location.X == 2) //the 2 is for the first column so we only do this once.
                    {
                        location = new Point(location.X + workingDDB.Width, 15);
                    }
                    else
                    {
                        location = new Point(location.X, location.Y + workingDDB.Height + 1);
                    }
                }

                if(workingDDB != null)
                    location = new Point(location.X, location.Y - workingDDB.Height);

                this.groupBoxDigitalGridDNPDeadBand.Size = new Size(location.X + workingDDB.Width + 2, location.Y + workingDDB.Height + 2);
                this.groupBoxDigitalGridDNPDeadBand.Show();

                this.buttonSendDeadBand.Location = new Point(this.groupBoxDigitalGridDNPDeadBand.Location.X, this.groupBoxDigitalGridDNPDeadBand.Location.Y + this.groupBoxDigitalGridDNPDeadBand.Height + 5);
            }
            this.buttonDefaults.Text = "Restore Defaults";
        }

        private void makeMemphis()
        {
            this.numericUpDownMemphisStage.Visible = true;
            this.labelMemphisStage.Visible = true;

            this.groupBoxMemphisDeadBand.Location = this.groupBoxDigitalGridDNPDeadBand.Location;
            this.groupBoxDigitalGridDNPDeadBand.Hide();
            this.groupBoxMemphisDeadBand.Show();

            this.buttonDefaults.Text = "Restore Memphis Defaults";
        }

        private void buttonDefaults_Click(object sender, EventArgs e)
        {
            if(this.customer == Customers.Memphis)
                this.setMemphisDefaults();
            else
                this.setDefaultDefaults();
        }



        private void setDefaultDefaults()
        {
            this.numericUpDownDestinationAddress.Value = 3;
            this.numericUpDownFragmentSize.Value = 1024;
            this.numericUpDownMaxEvents.Value = 20;
            this.numericUpDownSourceAddress.Value = 4;
            this.numericUpDownUnsolRetries.Value = 5;
            this.numericUpDownUnsolTimeout.Value = 10000;
            this.comboBoxLinkLayerConfirm.SelectedIndex = 0;
            this.comboBoxSelfAddress.SelectedIndex = 1;
            this.comboBoxTerminationResistor.SelectedIndex = 1;
            this.comboBoxUnsolResponse.SelectedIndex = 1;
            this.comboBoxBaudRate.SelectedIndex = 5;
        }

        private void setMemphisDefaults()
        {
            this.numericUpDownDestinationAddress.Value = 3;
            this.numericUpDownFragmentSize.Value = 1024;
            this.numericUpDownMaxEvents.Value = 20;
            this.numericUpDownMemphisStage.Value = 1;
            this.numericUpDownSourceAddress.Value = 4;
            //this.numericUpDownTriggerRangeAnalog.Value = 10;
            this.numericUpDownTriggerRangeCurrent.Value = 10;
            this.numericUpDownTriggerRangeTemperature.Value = 10;
            this.numericUpDownTriggerRangeTHD.Value = 20;
            this.numericUpDownTriggerRangeVoltage.Value = 10;
            this.numericUpDownUnsolRetries.Value = 5;
            this.numericUpDownUnsolTimeout.Value = 10000;
            this.comboBoxLinkLayerConfirm.SelectedIndex = 0;
            this.comboBoxSelfAddress.SelectedIndex = 1;
            this.comboBoxTerminationResistor.SelectedIndex = 1;
            this.comboBoxUnsolResponse.SelectedIndex = 1;
            this.comboBoxBaudRate.SelectedIndex = 3;

            this.numericUpDownAnalog1DeadBand.Value = 0.0m;
            this.numericUpDownAnalog2DeadBand.Value = 0.0m;
            this.numericUpDownAnalog3DeadBand.Value = 0.0m;
            this.numericUpDownAnalog4DeadBand.Value = 0.0m;
            this.numericUpDownCurrentAngleDB.Value = 0.0m;
            this.numericUpDownDifferentialVoltsDB.Value = 0.0m;
            this.numericUpDownDifferentialVoltsRealDB.Value = 0.0m;
            this.numericUpDownPhaseKVADB.Value = 0.0m;
            this.numericUpDownPhaseKVARDB.Value = 0.0m;
            this.numericUpDownPhaseKWDB.Value = 0.0m;
            this.numericUpDownTotalKVAVARDB.Value = 0.0m;
            this.numericUpDownTotalKWDB.Value = 0.0m;
            this.numericUpDownTriggerRangeCurrent.Value = 0.0m;
            this.numericUpDownTriggerRangeTemperature.Value = 0.0m;
            this.numericUpDownTriggerRangeTHD.Value = 0.0m;
            this.numericUpDownTriggerRangeVoltage.Value = 0.0m;
            this.numericUpDownOdometer.Value = 0.0m;
        }

        #endregion
    }

    public class ucDeadBandSettingsObject
    {
        public ucDeadBandSettingsObject()
        {
        }

        public ucDeadBandSettingsObject(string name, string units, decimal min, decimal max)
        {
            this.Name = name + ":";
            this.Units = units + "*";
            this.Maximum = max;
            this.Minimum = min;
        }
        public ucDeadBandSettingsObject(string name, string units, decimal min, decimal max, string toolTip)
        {
            this.Name = name + ":";
            this.Units = units + "*";
            this.Maximum = max;
            this.Minimum = min;
            this.ToolTip = toolTip;
        }

        public string ToolTip;
        public string Name;
        public string Units;
        public decimal Minimum;
        public decimal Maximum;

    }
}
