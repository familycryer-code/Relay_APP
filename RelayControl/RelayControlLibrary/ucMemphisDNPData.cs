using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SharedResources;

namespace RelayControlLibrary
{
    public partial class ucMemphisDNPData : UserControl
    {
        public ucMemphisDNPData()
        {
            InitializeComponent();
            SetSize();
            this.initializeStage();

        }

        private List<string> binaryInputs = new List<string>();
        private List<string> binaryOutputs = new List<string>();
        private List<AnalogPointDefinition> analogInputs = new List<AnalogPointDefinition>();
        private List<string> analogOutputs = new List<string>();
        private uint memphisStage = 5;

        private static int _packetLength = 98;

        public uint MemphisStage
        {
            get { return this.memphisStage; }
            set
            {
                if (value > 0 && value < 6)
                {
                    if (value != this.memphisStage)
                    {
                        this.memphisStage = value;
                        this.initializeStage();
                    }
                }
                else
                {
                    throw new Exception("Memphis Stage Out Of Range");
                }
            }
        }

        #region Initialization

        private void initializeStage()
        {
            this.initializeBinaryInputs();
            this.initializeBinaryOutputs();
            this.initializeAnalogInputs();
            this.initializeAnalogOutputs();
        }

        private void initializeBinaryInputs()
        {
            uint pointsToAdd;

            switch (this.memphisStage)
            {
                case 1:
                case 2:
                    pointsToAdd = 15;
                    break;
                case 3:
                case 4:
                    pointsToAdd = 31;
                    break;
                case 5:
                default:
                    pointsToAdd = 35;
                    break;
            }

            this.binaryInputs.Clear();

            this.tabPageBinaryInputs.Controls.Clear();

            this.binaryInputs.Add("Relay Failure");
            this.binaryInputs.Add("Calling For Open");
            this.binaryInputs.Add("Calling For Close");
            this.binaryInputs.Add("Calling For Float");
            this.binaryInputs.Add("Blocked From Closing");
            this.binaryInputs.Add("Relaxed Close/Protected Close");
            this.binaryInputs.Add("Network Volts Too Low To Close");
            this.binaryInputs.Add("Differential Volts Too Low To Close");
            this.binaryInputs.Add("Phase Angle Incorrect To Close");
            this.binaryInputs.Add("Anti-Pump Lockout");
            this.binaryInputs.Add("Breaker Status");
            this.binaryInputs.Add("Digital Input 1: NWP Position");
            this.binaryInputs.Add("Digital Input 2: NWP Enclosure Pressure");
            this.binaryInputs.Add("Digital Input 3: Water in NWP");
            this.binaryInputs.Add("Digital Input 4: Water in Vault");
            this.binaryInputs.Add("Dummy");
            this.binaryInputs.Add("Remote Close Enabled");
            this.binaryInputs.Add("Remote Open Enabled");
            this.binaryInputs.Add("Remote Block Enabled");
            this.binaryInputs.Add("Logic Mode");
            this.binaryInputs.Add("CH/GE");
            this.binaryInputs.Add("Frequency");
            this.binaryInputs.Add("Straight Line Master/Circle");
            this.binaryInputs.Add("Time Delay");
            this.binaryInputs.Add("Time Delay Infinite/Insensitive");
            this.binaryInputs.Add("Watt Var Trip");
            this.binaryInputs.Add("Pump Protect");
            this.binaryInputs.Add("Mode - Standard/Sensitive");
            this.binaryInputs.Add("Mode - Insensitive");
            this.binaryInputs.Add("Voltage Constant Enabled");
            this.binaryInputs.Add("Defaults Loaded");
            this.binaryInputs.Add("Digital Out 1");
            this.binaryInputs.Add("Digital Out 2");
            this.binaryInputs.Add("Digital Out 3");
            this.binaryInputs.Add("Digital Out 4");

            uint i = 1;

            foreach (string s in this.binaryInputs)
            {
                ucDNPMemphisBinary workingBox = new ucDNPMemphisBinary();

                workingBox.PointNumber = i;
                workingBox.PointName = s;
#if TORONTO_HYDRO
                workingBox.EventEnableVisible = true;
#endif
                this.addBinaryBox(workingBox, this.tabPageBinaryInputs);

                if (i == pointsToAdd)
                    break;
                i++;
            }
        }

        private void initializeBinaryOutputs()
        {
            uint pointsToAdd;

            switch (this.memphisStage)
            {
                case 1:
                case 2:
                case 3:
                    pointsToAdd = 3;
                    break;
                case 4:
                    pointsToAdd = 5;
                    break;
                case 5:
                default:
                    pointsToAdd = 9;
                    break;
            }

            this.binaryOutputs.Clear();
            this.tabPageBinaryOuputs.Controls.Clear();

            this.binaryOutputs.Add("Call For Trip");
            this.binaryOutputs.Add("Block From Closing");
            this.binaryOutputs.Add("Relaxed Close");
            this.binaryOutputs.Add("Get Event Log");
            this.binaryOutputs.Add("Capture Waveform");
            this.binaryOutputs.Add("Digital Output 1");
            this.binaryOutputs.Add("Digital Output 2");
            this.binaryOutputs.Add("Digital Output 3");
            this.binaryOutputs.Add("Digital Output 4");

            uint i = 1;

            foreach (string s in this.binaryOutputs)
            {
                ucDNPMemphisBinary workingBox = new ucDNPMemphisBinary();

                workingBox.PointNumber = i;
                workingBox.PointName = s;
                workingBox.EventEnableVisible = false;

                this.addBinaryBox(workingBox, this.tabPageBinaryOuputs);

                if (i == pointsToAdd)
                    break;
                i++;
            }
        }

        private void initializeAnalogInputs()
        {
            uint pointsToAdd;

            switch (this.memphisStage)
            {
                case 1:
                    pointsToAdd = 30;
                    break;
                case 2:
                    pointsToAdd = 45;
                    break;
                case 3:
                    pointsToAdd = 81;
                    break;
                case 4:
                    pointsToAdd = 84;
                    break;
                case 5:
                default:
                    pointsToAdd = 100;
                    break;
            }

            this.analogInputs.Clear();
            this.tabPageAnalogInputs1.Controls.Clear();
            this.tabPageAnalogInputs2.Controls.Clear();
            
            this.analogInputs.Add(new AnalogPointDefinition("Device DNP Address", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Serial Number High", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Serial Number Low", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Odometer", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Manufacturer", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase A Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase B Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase C Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase A Network Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase B Network Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase C Network Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase A Transformer Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase B Transformer Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase C Transformer Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase A Differential Voltage Magnitude", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase B Differential Voltage Magnitude", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase C Differential Voltage Magnitude", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase A Differential Voltage w/ PF", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase B Differential Voltage w/ PF", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase C Differential Voltage w/ PF", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase A Current Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase B Current Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase C Current Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase A Voltage THD", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase B Voltage THD", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase C Voltage THD", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase A Current THD", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase B Current THD", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase C Current THD", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Temperature", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase A - kW", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase B - kW", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase C - kW", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase A - kVAR", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase B - kVAR", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase C - kVAR", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase A - kVA", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase B - kVA", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase C - kVA", false));
            this.analogInputs.Add(new AnalogPointDefinition("Total kW", true));
            this.analogInputs.Add(new AnalogPointDefinition("Total kVAR", true));
            this.analogInputs.Add(new AnalogPointDefinition("Total kVA", false));
            this.analogInputs.Add(new AnalogPointDefinition("Software Version", false));
            this.analogInputs.Add(new AnalogPointDefinition("System Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("CT Ratio", false));
            this.analogInputs.Add(new AnalogPointDefinition("Left Hand Master Line", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reverse Trip", false));
            this.analogInputs.Add(new AnalogPointDefinition("Time Delay", false));
            this.analogInputs.Add(new AnalogPointDefinition("Overcurrent/Instant Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Angle", true));
            this.analogInputs.Add(new AnalogPointDefinition("Sensitive Trip Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Insensitive Trip Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Extended Delay", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Time Delay", false));
            this.analogInputs.Add(new AnalogPointDefinition("Sensitive Trip Delay", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage Lower Limit to Close", false));
            this.analogInputs.Add(new AnalogPointDefinition("Pump Relay Time Limit", false));
            this.analogInputs.Add(new AnalogPointDefinition("Watt Var Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Watt Var Angle", true));
            this.analogInputs.Add(new AnalogPointDefinition("Phase Compensation - GE Only", true));
            this.analogInputs.Add(new AnalogPointDefinition("Network Volts Too Low To Close", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phasing Voltage Off-Set", false));
            this.analogInputs.Add(new AnalogPointDefinition("Trip Pulses", false));
            this.analogInputs.Add(new AnalogPointDefinition("Close Tilt Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Trip Tilt Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Reclose Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Reclose Angle", true));
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Active Time", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Phasing Offset", false));
            this.analogInputs.Add(new AnalogPointDefinition("Close Penalty", false));
            this.analogInputs.Add(new AnalogPointDefinition("Upper Limit", false));
            this.analogInputs.Add(new AnalogPointDefinition("Lower Limit", false));
            this.analogInputs.Add(new AnalogPointDefinition("Pump Protection - Breaker Cycles", false));
            this.analogInputs.Add(new AnalogPointDefinition("Pump Protection - Pump Time", false));
            this.analogInputs.Add(new AnalogPointDefinition("Pump Protection - Reset Time", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay De-Energized Action", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Failure Position", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Algorithm", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase Rotation Sensitivity", false));
            this.analogInputs.Add(new AnalogPointDefinition("Anti-Pump Logic", false));
            this.analogInputs.Add(new AnalogPointDefinition("Number Events in Log", false));
            this.analogInputs.Add(new AnalogPointDefinition("Wayform Capture Sample Rate", false));
            this.analogInputs.Add(new AnalogPointDefinition("Number of Captured Cycles", false));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 1", true));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 2", true));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 3", true));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 4", true));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 5", true));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 6", true));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 7", true));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 8", true));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Settings Spare 1", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Settings Spare 2", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Settings Spare 3", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Settings Spare 4", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Settings Spare 5", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Settings Spare 6", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Settings Spare 7", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Settings Spare 8", false));

            uint i = 1;

            foreach (AnalogPointDefinition aPD in this.analogInputs)
            {

                ucDNPMemphisAnalog workingBox = new ucDNPMemphisAnalog();

                workingBox.PointNumber = i;
                workingBox.PointName = aPD.Name;
                workingBox.Signed = aPD.Signed;

                if (i <= 50)
                    this.addAnalogBox(workingBox, this.tabPageAnalogInputs1);
                else
                    this.addAnalogBox(workingBox, this.tabPageAnalogInputs2);

                if (i == pointsToAdd)
                    break;
                i++;
            }
        }

        private void initializeAnalogOutputs()
        {
            uint pointsToAdd;

            switch (this.memphisStage)
            {
                case 1:
                case 2:
                case 3:
                    pointsToAdd = 0;
                    break;
                case 4:
                case 5:
                default:
                    pointsToAdd = 2;
                    break;
            }

            this.analogOutputs.Clear();
            this.tabPageAnalogOutputs.Controls.Clear();

            this.analogOutputs.Add("Get Waveform Data / Pop Log Waveform");
            this.analogOutputs.Add("Get Event Log");

            uint i = 1;
            if (pointsToAdd != 0)
            {
                foreach (string s in this.analogOutputs)
                {

                    ucDNPMemphisAnalog workingBox = new ucDNPMemphisAnalog();

                    workingBox.PointNumber = i;
                    workingBox.PointName = s;

                    this.addAnalogBox(workingBox, this.tabPageAnalogOutputs);

                    if (i == pointsToAdd)
                        break;
                    i++;
                }
            }
        }

        private void SetSize()
        {
            this.tabControlMemphisDNP.Size = this.Size;
        }

        private void tabControlMemphisDNP_Resize(object sender, EventArgs e)
        {
            this.SetSize();
        }

        //Adds a binary box to the selected page
        private void addBinaryBox(ucDNPMemphisBinary box, TabPage tB)
        {
            int y = tB.Controls.Count % 20 * 22 + 1; //22 is the height of the control - %20 because 20 per row
            int x;

            if (tB.Controls.Count >= 20)
                x = this.tabPageBinaryInputs.Width / 2;
            else
                x = 1;
            box.Location = new Point(x, y);
            tB.Controls.Add(box);
        }

        //Adds a analog box to the selected page
        private void addAnalogBox(ucDNPMemphisAnalog box, TabPage tB)
        {
            int y = tB.Controls.Count % 25 * 20 + 5; //22 is the height of the control - %20 because 20 per row
            int x = 347 * (tB.Controls.Count / 25) + 1;


            /*

            if (tB.Controls.Count >= 25)
                x = 347; //width is 347
            else
                x = 1;
             */
            box.Location = new Point(x, y);
            tB.Controls.Add(box);
        }
#endregion

        #region Input Data

        public void SetAll(byte[] bytePacket, int p)
        {
            switch (p)
            {
                case 1:
                   // this.setBinaryInputs(bytePacket, 4); //first data is dummy, so start with 4?
                   // this.setBinaryOutputs(bytePacket, 148); //145 is where binary outputs starts, each BINARY point has 4 points
                    this.setAnalogInputs(bytePacket, 1, 190); // is where it starts, but skip one due to "dummy"
                    break;
                case 2:
                    this.setAnalogInputs(bytePacket, 2, 4); //4 for proper offset
                    break;
                case 3:
                    this.setAnalogInputs(bytePacket, 3, 4);
                    break;
                case 4:
                    this.setAnalogInputs(bytePacket, 4, 4);
                    this.setAnalogOutputs(bytePacket, 44);
                    break;
                default:
                    throw new Exception("Bad Packet Number for DNP Data");
            }
        }
        /*
       private void setBinaryInputs(byte[] bytePacket, int index)
       {

           foreach (ucDNPMemphisBinary uDMB in this.tabPageBinaryInputs.Controls)
           {
               uDMB.CheckValue = this.convertDataByteToBool(bytePacket[index]);
               if ((bytePacket[index + 2] & 0x02) == 0x02)
                   uDMB.EventEnabled = true;
               else
                   uDMB.EventEnabled = false;
               index += 4;
           }
       }

       private void setBinaryOutputs(byte[] bytePacket, int index)
       {
           foreach (ucDNPMemphisBinary uDMB in this.tabPageBinaryOuputs.Controls)
           {
               uDMB.CheckValue = this.convertDataByteToBool(bytePacket[index]);
               index += 4;
           }
       }
       */
        private bool convertDataByteToBool(byte b)
        {
            if (b == 1)
                return true;
            else
                return false;
        }

        private ucDNPMemphisAnalog savedAnalogControl;
        private TabPage workingTabPage;

        private void setAnalogInputs(byte[] bytePacket, int packetNumber, int index)
        {
            bool endOfTabControlsReached = true;

            if (packetNumber == 1)           //first packet so initialize everything
            {
                this.workingTabPage = this.tabPageAnalogInputs1;
                this.savedAnalogControl = null;
            }

            foreach (ucDNPMemphisAnalog uDMA in this.workingTabPage.Controls)
            {
                if (this.savedAnalogControl == null || this.savedAnalogControl == uDMA)  //null means we are passed the saved control
                {
                    this.savedAnalogControl = null;

                    try
                    {
                        uDMA.PointValue = this.convertDataBytesToAnalog(bytePacket, index);
                        index += 6;
                    }
                    catch
                    {
                        this.savedAnalogControl = uDMA;
                        endOfTabControlsReached = false;
                        break;
                    }
                }
            }

            if (endOfTabControlsReached) //the final control of that page was reached so there are still points in the packet
            {
                if (this.workingTabPage == this.tabPageAnalogInputs1)
                    this.workingTabPage = this.tabPageAnalogInputs2;
                else if (this.workingTabPage == this.tabPageAnalogInputs2)
                    this.workingTabPage = this.tabPageAnalogOutputs;
                else
                    return; //no more points

                this.savedAnalogControl = null;

                foreach (ucDNPMemphisAnalog uDMA in this.workingTabPage.Controls)  //this one should ALWAYS run out of index points
                {
                    try
                    {
                        uDMA.PointValue = this.convertDataBytesToAnalog(bytePacket, index);
                        index += 6;
                    }
                    catch
                    {
                        this.savedAnalogControl = uDMA;
                        break;
                    }
                }
            }
        }

        private void setAnalogOutputs(byte[] bytePacket, int i)
        {
            foreach (ucDNPMemphisAnalog uDMA in this.tabPageAnalogOutputs.Controls)
            {
                try
                {
                    uDMA.PointValue = this.convertDataBytesToAnalog(bytePacket, i);
                }
                catch
                {
                }
            }

        }

        private uint convertDataBytesToAnalog(byte[] bytePacket, int i)
        {
            UInt16 temp;
            temp = bytePacket[i + 1];
            temp <<= 8;
            temp += bytePacket[i];

            return temp;
        }

        #endregion

        public delegate void MemphisSendEventHandler(object o, SendEventArgs mEA);
        public event MemphisSendEventHandler Send;

        private void buttonSendBinaryInputEventEnables_Click(object sender, EventArgs e)
        {
            uint i = 1;
            uint packetByteNumber = 2; //starts at 2 after OpCode and SubCode
            byte tempByte = 0;

            SendEventArgs sEA = new SendEventArgs(_packetLength);
       //     MessageBox.Show("7 sending command D to master"); // Only for testing - to be removed
            sEA.SendPacket[0] = (byte)RelayModeFunctions._DNPControlOpCode;
            sEA.SendPacket[1] = (byte)'e';        //For set binary events subcode

            foreach (ucDNPMemphisBinary uDMB in this.tabPageBinaryInputs.Controls)
            {
                if (i % 8 == 0)
                {
                    tempByte = 0; //First value of byte, so clear tempByte
                    if (uDMB.EventEnabled)
                        tempByte = 1;
                }
                else if (i % 8 == 7) //last value of group
                {
                    if (uDMB.EventEnabled)
                        tempByte += (byte)0x80;

                    sEA.SendPacket[packetByteNumber] = tempByte;

                    packetByteNumber++; //next byte goes into next byte of packet
                }
                else
                {
                    if (uDMB.EventEnabled)
                    {
                        byte j = 0;
                        byte value = 1;
                        for (j = 0; j < i % 8; ++j)
                        {
                            value <<= 1;
                        }

                        tempByte += value;
                    }
                }

                i++;
            }
            if (i % 8 != 0)
            {
                sEA.SendPacket[packetByteNumber] = tempByte;
            }

            sEA.SendPacket[sEA.SendPacket.Length - 1] = 0x0D;
            if (this.Send != null)
                this.Send(this, sEA);
        }

        private void tabControlMemphisDNP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControlMemphisDNP.SelectedTab == this.tabPageBinaryInputs)
                this.buttonSendBinaryEventEnables.Visible = true;
            else
                this.buttonSendBinaryEventEnables.Visible = false;
        }

    }

    public class AnalogPointDefinition
    {
        public AnalogPointDefinition(string name, bool signed)
        {
            this.Name = name;
            this.Signed = signed;
        }
        public string Name;
        public bool Signed;
    }
}
