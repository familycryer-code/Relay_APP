using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucDNPDigitalGridData : UserControl
    {
        public ucDNPDigitalGridData()
        {
            InitializeComponent();
            SetSize();
            this.initializeComponents();
        }

        public ucDNPDigitalGridData(Customers customer)
        {
            InitializeComponent();
            this.customer = customer;
            SetSize();
            this.initializeComponents();

        }

        public uint RelayMasterRevision
        {
            set
            {
                if (value != this.relayMasterRevision)
                {
                    this.relayMasterRevision = value;
                    this.initializeComponents();
                }
            }
        }
        public Customers Customer
        {
            set
            {
                if (value != this.customer)
                {
                    this.customer = value;
                    if (this.customer == Customers.NonConEd)
                        return;
                    SetSize();
                    this.initializeComponents();
                }
            }
        }

        private List<string> binaryInputs = new List<string>();
        private List<string> binaryOutputs = new List<string>();
        private List<AnalogPointDefinition> analogInputs = new List<AnalogPointDefinition>();
        private List<AnalogPointDefinition> analogOutputs = new List<AnalogPointDefinition>();
        private byte[] dNPData = new byte[1008]; //252 packet size * 4
        private uint relayMasterRevision = 140506;
        private Customers customer = Customers.DigitalGridDNP;

#if ATLANTA
        private static int _packetLength = 42;
#else
        private static int _packetLength = 98;
#endif
        #region Initialization

        private void initializeComponents()
        {
            this.initializeBinaryInputs();
            this.initializeBinaryOutputs();
            this.initializeAnalogInputs();
            this.initializeAnalogOutputs();

            this.tabControlMemphisDNP_SelectedIndexChanged_1(this, new EventArgs());
        }

        private void initializeBinaryInputs()
        {
            uint pointsToAdd = 14;

            this.binaryInputs.Clear();

            this.tabPageBinaryInputs.Controls.Clear();

            this.binaryInputs.Add("Calling For Open");
            this.binaryInputs.Add("Calling For Close");
            this.binaryInputs.Add("Calling For Float");
            this.binaryInputs.Add("Blocked From Closing");
            this.binaryInputs.Add("Relay Phasing OK");
            this.binaryInputs.Add("Pump Protect Lockout");
            this.binaryInputs.Add("Network Protect Status/B Flag");
            this.binaryInputs.Add("Defaults Loaded");
            this.binaryInputs.Add("Phased ACB");
            this.binaryInputs.Add("Insensitive Backfeed Detected");
            if (this.customer == Customers.DigitalGridDNP || this.customer == Customers.Atlanta)
            {
                this.binaryInputs.Add("Digital Input 1");
                this.binaryInputs.Add("Digital Input 2");
                this.binaryInputs.Add("Digital Input 3");
                this.binaryInputs.Add("Digital Input 4");

                pointsToAdd = 14;
            }
            else if (this.customer == Customers.DNPwithPLC)
            {
                this.binaryInputs.Add("A Flag");
                this.binaryInputs.Add("Digital Input 1");
                this.binaryInputs.Add("Digital Input 2");
                this.binaryInputs.Add("Digital Input 3");

                pointsToAdd = 14;
            }
            else
            {
                pointsToAdd = 14;
            }

            uint i = 0;

            foreach (string s in this.binaryInputs)
            {
                ucDNPMemphisBinary workingBox = new ucDNPMemphisBinary();

                workingBox.PointNumber = i;
                workingBox.PointName = s;
                workingBox.EventEnableVisible = true;

                this.addBinaryBox(workingBox, this.tabPageBinaryInputs);

                if (i == pointsToAdd)
                    break;
                i++;
            }

            int j = this.tabPageBinaryInputs.Controls.Count;
        }

        private void initializeBinaryOutputs()
        {
            uint pointsToAdd;
            
            if (this.relayMasterRevision < 140107 || this.customer != Customers.Atlanta)
                pointsToAdd = 20;
            else
                pointsToAdd = 22;

            this.binaryOutputs.Clear();

            this.tabPageBinaryOuputs.Controls.Clear();

            this.binaryOutputs.Add("Call For Trip");
            this.binaryOutputs.Add("Relax Close");
            this.binaryOutputs.Add("Block From Closing");
            this.binaryOutputs.Add("Sensitive Trip");
            this.binaryOutputs.Add("Insensitive Trip");
            this.binaryOutputs.Add("Time Delay Trip");
            this.binaryOutputs.Add("Watt-Var Trip");
            this.binaryOutputs.Add("Trip On Power Down");
            this.binaryOutputs.Add("Trim Curve");
            this.binaryOutputs.Add("Circle Close");
            this.binaryOutputs.Add("Override Block on Dead Network");
            this.binaryOutputs.Add("Sequence(1)/Power(0) Relay");
            this.binaryOutputs.Add("Pump Mode Relay Cycles");
            this.binaryOutputs.Add("Pump Mode Motor Cycles");
            this.binaryOutputs.Add("Pump Mode Motor Timeout");
            this.binaryOutputs.Add("Pump Lockout Never Reclose");
            this.binaryOutputs.Add("Clear Pump Protect Lockout");
            this.binaryOutputs.Add("Clear Cycle Counter");
            this.binaryOutputs.Add("Digital Out 1");
            this.binaryOutputs.Add("Digital Out 2");
            this.binaryOutputs.Add("Block And Trip Relay");
            this.binaryOutputs.Add("SafeService Enable");

            uint i = 0;

            foreach (string s in this.binaryOutputs)
            {
                ucDNPMemphisBinary workingBox = new ucDNPMemphisBinary();

                workingBox.PointNumber = i;
                workingBox.PointName = s;
                workingBox.EventEnableVisible = false;

                this.addBinaryBox(workingBox, this.tabPageBinaryOuputs);

                i++;

                if (i == pointsToAdd)
                    break;
            }
        }

        private void initializeAnalogInputs()
        {
            uint pointsToAdd;
            if (this.customer == Customers.Atlanta)
                pointsToAdd = 69;
            else
                pointsToAdd = 73;

            this.analogInputs.Clear();
            this.tabPageAnalogInputs1.Controls.Clear();
            this.tabPageAnalogInputs2.Controls.Clear();

            this.analogInputs.Add(new AnalogPointDefinition("Relay Serial Number", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Version Number", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage - A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage - B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage - C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage Angle - A", true));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage Angle - B", true));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage Angle - C", true));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage - A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage - B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage - C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage Angle - A", true));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage Angle - B", true));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage Angle - C", true));
            this.analogInputs.Add(new AnalogPointDefinition("Differential Voltage Magnitude - A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Differential Voltage Magnitude - B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Differential Voltage Magnitude - C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Differential Voltage Angle - A", true));
            this.analogInputs.Add(new AnalogPointDefinition("Differential Voltage Angle - B", true));
            this.analogInputs.Add(new AnalogPointDefinition("Differential Voltage Angle - C", true));
            this.analogInputs.Add(new AnalogPointDefinition("Differential Voltage Magnitude - Avg", false));
            this.analogInputs.Add(new AnalogPointDefinition("Differential Voltage Angle - Avg", true));
            this.analogInputs.Add(new AnalogPointDefinition("Real Differential Voltage - A", true));
            this.analogInputs.Add(new AnalogPointDefinition("Real Differential Voltage - B", true));
            this.analogInputs.Add(new AnalogPointDefinition("Real Differential Voltage - C", true));
            this.analogInputs.Add(new AnalogPointDefinition("Real Differential Voltage - Avg", true));
            this.analogInputs.Add(new AnalogPointDefinition("Current - A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current - B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current - C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Voltage Angle - A", true));
            this.analogInputs.Add(new AnalogPointDefinition("Current Voltage Angle - B", true));
            this.analogInputs.Add(new AnalogPointDefinition("Current Voltage Angle - C", true));
            this.analogInputs.Add(new AnalogPointDefinition("Current - Effective", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Angle - Effective", true));
            this.analogInputs.Add(new AnalogPointDefinition("Current - Positive Sequence", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Angle - Positive Sequence", true));
            this.analogInputs.Add(new AnalogPointDefinition("Current - Negative Sequence", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Angle - Negative Sequence", true));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power - A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power - B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power - C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Angle - A", true));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Angle - B", true));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Angle - C", true));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power - Avg", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Angle - Avg", true));
            this.analogInputs.Add(new AnalogPointDefinition("Real Power - A", true));
            this.analogInputs.Add(new AnalogPointDefinition("Real Power - B", true));
            this.analogInputs.Add(new AnalogPointDefinition("Real Power - C", true));
            this.analogInputs.Add(new AnalogPointDefinition("Diff Volt - Pos Seq", false));
            this.analogInputs.Add(new AnalogPointDefinition("Diff Volt Angle - Pos Seq", true));
            this.analogInputs.Add(new AnalogPointDefinition("Diff Voltage - Neg Seq", false));
            this.analogInputs.Add(new AnalogPointDefinition("Diff Voltage Angle - Neg Seq", true));
            this.analogInputs.Add(new AnalogPointDefinition("Network Volts - Pos Seq", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Volts Angle - Pos Seq", true));
            this.analogInputs.Add(new AnalogPointDefinition("Network Volts - Neg Seq", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Volts Angle - Neg Seq", true));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage THD - A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage THD - B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage THD - C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD - A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD - B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD - C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Temperature", true));
            this.analogInputs.Add(new AnalogPointDefinition("Breaker Cycles", false));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 1", true));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 2", true));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 3", true));
            this.analogInputs.Add(new AnalogPointDefinition("Auxiliary Input 4", true));

            if (this.customer == Customers.DNPwithPLC)
            {
                this.analogInputs.Add(new AnalogPointDefinition("SEC - Analog 1", true));
                this.analogInputs.Add(new AnalogPointDefinition("SEC - Analog 2", true));
                this.analogInputs.Add(new AnalogPointDefinition("SEC - Analog 3", true));
                this.analogInputs.Add(new AnalogPointDefinition("SEC - Analog 4", true));
                this.analogInputs.Add(new AnalogPointDefinition("SEC - Analog 5", true));
                this.analogInputs.Add(new AnalogPointDefinition("SEC - Analog 6", true));
                this.analogInputs.Add(new AnalogPointDefinition("SEC - Analog 7", true));
                this.analogInputs.Add(new AnalogPointDefinition("SEC - Analog 8", true));
                this.analogInputs.Add(new AnalogPointDefinition("SEC - Q Bit", true));

                pointsToAdd += 9;
            }

            uint i = 0;

            foreach (AnalogPointDefinition aPD in this.analogInputs)
            {

                ucDNPDigitalGridAnalogIn workingBox = new ucDNPDigitalGridAnalogIn();

                workingBox.PointNumber = i;
                workingBox.PointName = aPD.Name;
                workingBox.Signed = aPD.Signed;

                if (i < 50)
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs1);
                else
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs2);

                if (i == pointsToAdd)
                    break;
                i++;
            }
        }

        private void initializeAnalogOutputs()
        {
            uint pointsToAdd = 28;

            this.analogOutputs.Clear();
            this.tabPageAnalogOutputs.Controls.Clear();

            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Sensitive Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Sensitive Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Insensitive Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Instantaneous Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Time Delay Trip Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Time Delay Extended Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Watt-Var Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Watt-Var Angle", true));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Trip Style", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Trim Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Mode - Close Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Mode - Reclose/Circle Close Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Mode - Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Mode - Phasing Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Mode - Phasing Angle", true));
            this.analogOutputs.Add(new AnalogPointDefinition("CT Ratio", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phasing Mode", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode - Relay Cycle Limit", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode - Time Limit", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode - Motor Cycles", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode - Motor Timeout", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode - Pump Lockout Time", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service - Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service - OverCurrent", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service - Current Imbalance", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service - Low Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service - Voltage Imbalance", false));

            uint i = 0;
            if (pointsToAdd != 0)
            {
                foreach (AnalogPointDefinition aPD in this.analogOutputs)
                {

                    ucDNPMemphisAnalog workingBox = new ucDNPMemphisAnalog();

                    workingBox.PointNumber = i;
                    workingBox.PointName = aPD.Name;
                    workingBox.Signed = aPD.Signed;

                    this.addAnalogBoxOut(workingBox, this.tabPageAnalogOutputs);

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
        private void addAnalogBoxIn(ucDNPDigitalGridAnalogIn box, TabPage tB)
        {
            int y = tB.Controls.Count % 25 * box.Height + 5; //22 is the height of the control - %20 because 20 per row
            int x = box.Width * (tB.Controls.Count / 25) + 1;

            box.Location = new Point(x, y);
            tB.Controls.Add(box);
        }

        private void addAnalogBoxOut(ucDNPMemphisAnalog box, TabPage tB)
        {
            int y = tB.Controls.Count % 25 * 20 + 5; //22 is the height of the control - %20 because 20 per row
            int x = 347 * (tB.Controls.Count / 25) + 1;

            box.Location = new Point(x, y);
            tB.Controls.Add(box);
        }
        #endregion

        #region Send Region

        public delegate void DigitalGridSendEventHandler(object o, SendEventArgs mEA);
        public event DigitalGridSendEventHandler Send;

        private void buttonSendBinaryInputEventEnables_Click(object sender, EventArgs e)
        {
            uint i = 1;
            uint packetByteNumber = 2; //starts at 2 after OpCode and SubCode
            byte tempByte = 0;

            SendEventArgs sEA = new SendEventArgs(_packetLength);

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

        #endregion

        #region Incoming Data Handling

        public void SetAll(byte[] bytePacket, int p)
        {
            for(int packetIndex = 0, dataIndex = 252 * (p-1); packetIndex < 252; packetIndex++, dataIndex++)
            {
                this.dNPData[dataIndex] = bytePacket[packetIndex];
            }
            
            if(p == 4)
                this.setAllDNPData();
        }

        private void setAllDNPData()
        {
            int savedIndex = 0;

            savedIndex = this.setBinaryInputs(this.dNPData, savedIndex);
            savedIndex = this.setBinaryOutputs(this.dNPData, savedIndex);
            savedIndex = this.setAnalogInputs(this.dNPData, savedIndex);
            this.setAnalogOutputs(this.dNPData, savedIndex);
        }

        private int setBinaryInputs(byte[] bytePacket, int index)
        {
            int i = this.tabPageBinaryInputs.Controls.Count;
            foreach (Control C in this.tabPageBinaryInputs.Controls)
            {
                ucDNPMemphisBinary uDMB = new ucDNPMemphisBinary();
                bool failed = false;

                try
                {
                    uDMB = (ucDNPMemphisBinary)C;
                }
                catch
                {
                    failed = true;
                }

                if(!failed)
                {
                    uDMB.CheckValue = this.convertDataByteToBool(bytePacket[index]);
                    if ((bytePacket[index + 2] & 0x02) == 0x02)
                        uDMB.EventEnabled = true;
                    else
                        uDMB.EventEnabled = false;
                    index += 4;
                }
            }

            return index;
        }

        private int setBinaryOutputs(byte[] bytePacket, int index)
        {
            foreach (ucDNPMemphisBinary uDMB in this.tabPageBinaryOuputs.Controls)
            {
                uDMB.CheckValue = this.convertDataByteToBool(bytePacket[index]);
                index += 4;
            }

            return index;
        }

        private int setAnalogInputs(byte[] bytePacket, int index)
        {
            foreach (Control C in this.tabPageAnalogInputs1.Controls)
            {
                ucDNPDigitalGridAnalogIn uDDGA = new ucDNPDigitalGridAnalogIn();
                bool failed = false;

                try
                {
                    uDDGA = (ucDNPDigitalGridAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }

                if(!failed)
                {
                    uDDGA.EventEnabled = this.convertAnalogControlByteToBool(bytePacket[index + 4]);
                    uDDGA.PointValue = this.convertDataBytesToAnalog(bytePacket, index);
                    index += 6;
                }
            }
            foreach (Control C in this.tabPageAnalogInputs2.Controls)
            {
                ucDNPDigitalGridAnalogIn uDDGA = new ucDNPDigitalGridAnalogIn();
                bool failed = false;

                try
                {
                    uDDGA = (ucDNPDigitalGridAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }

                if(!failed)
                {
                    uDDGA.EventEnabled = this.convertAnalogControlByteToBool(bytePacket[index + 4]);
                    uDDGA.PointValue = this.convertDataBytesToAnalog(bytePacket, index);
                    index += 6;
                }
            }
            return index;
        }

        private void setAnalogOutputs(byte[] bytePacket, int index)
        {
            foreach (ucDNPMemphisAnalog uDMA in this.tabPageAnalogOutputs.Controls)
            {
                try
                {
                    uDMA.PointValue = this.convertDataBytesToAnalog(bytePacket, index);
                    index += 4;
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

        private bool convertAnalogControlByteToBool(byte p)
        {
            if ((p & 0x02) == 2)
                return true;
            else
                return false;
        }

        private bool convertDataByteToBool(byte b)
        {
            if ((b & 0x01) == 1)
                return true;
            else
                return false;
        }
        #endregion

        private void tabControlMemphisDNP_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if(this.tabControlMemphisDNP.SelectedTab == this.tabPageAnalogInputs1)
            {
                if(this.tabPageAnalogInputs2.Controls.Contains(this.buttonSendAnalogEnables))
                    this.tabPageAnalogInputs2.Controls.Remove(this.buttonSendAnalogEnables);
                if(!this.tabPageAnalogInputs1.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs1.Controls.Add(this.buttonSendAnalogEnables);
                }
                this.buttonSendAnalogEnables.Location = new Point(this.tabPageAnalogInputs1.Width - this.buttonSendAnalogEnables.Width - 2, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonSendAnalogEnables.Visible = true;
            }
            else if (this.tabControlMemphisDNP.SelectedTab == this.tabPageAnalogInputs2)
            {
                if(this.tabPageAnalogInputs1.Controls.Contains(this.buttonSendAnalogEnables))
                    this.tabPageAnalogInputs1.Controls.Remove(this.buttonSendAnalogEnables);
                if(!this.tabPageAnalogInputs2.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs2.Controls.Add(this.buttonSendAnalogEnables);
                }
                this.buttonSendAnalogEnables.Location = new Point(this.tabPageAnalogInputs2.Width - this.buttonSendAnalogEnables.Width - 2, this.tabPageAnalogInputs2.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonSendAnalogEnables.Visible = true;
            }
            else if (this.tabControlMemphisDNP.SelectedTab == this.tabPageBinaryInputs)
            {
                if(!this.tabPageBinaryInputs.Controls.Contains(this.buttonSendBinaryEventEnables))
                {
                    this.tabPageBinaryInputs.Controls.Add(this.buttonSendBinaryEventEnables);
                }
                this.buttonSendBinaryEventEnables.Location = new Point(this.tabPageBinaryInputs.Width - this.buttonSendBinaryEventEnables.Width - 2, this.tabPageBinaryInputs.Height - this.buttonSendBinaryEventEnables.Height - 2);
                this.buttonSendBinaryEventEnables.Visible = true;
            }
        }

        private void buttonSendBinaryEventEnables_Click(object sender, EventArgs e)
        {
            uint i = 0;
            uint packetByteNumber = 2; //starts at 2 after OpCode and SubCode
            byte tempByte = 0;

            SendEventArgs sEA = new SendEventArgs(_packetLength);

            sEA.SendPacket[0] = (byte)RelayModeFunctions._DNPControlOpCode;
            sEA.SendPacket[1] = (byte)'e';        //For set binary events subcode

            foreach (Control C in this.tabPageBinaryInputs.Controls)
            {
                ucDNPMemphisBinary uDMB = new ucDNPMemphisBinary();
                bool failed = false;
                try
                {
                    uDMB = (ucDNPMemphisBinary)C;
                }
                catch
                {
                    failed = true;
                }

                if(!failed)
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
            }
            if (i % 8 != 0)
            {
                sEA.SendPacket[packetByteNumber] = tempByte;
            }

            sEA.SendPacket[sEA.SendPacket.Length - 1] = 0x0D;
            if (this.Send != null)
                this.Send(this, sEA);
        }

        private void buttonSendAnalogEnables_Click(object sender, EventArgs e)
        {
            uint i = 0;
            uint packetByteNumber = 2; //starts at 2 after OpCode and SubCode
            byte tempByte = 0;

            SendEventArgs sEA = new SendEventArgs(_packetLength);

            sEA.SendPacket[0] = (byte)RelayModeFunctions._DNPControlOpCode;
            sEA.SendPacket[1] = (byte)'E';        //For set analog events subcode

            foreach (Control C in this.tabPageAnalogInputs1.Controls)
            {
                ucDNPDigitalGridAnalogIn uDDGA = new ucDNPDigitalGridAnalogIn();
                bool failed = false;

                try
                {
                    uDDGA = (ucDNPDigitalGridAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }
                if(!failed)
                {
                    if (i % 8 == 0)
                    {
                        tempByte = 0; //First value of byte, so clear tempByte
                        if (uDDGA.EventEnabled)
                            tempByte = 1;
                    }
                    else if (i % 8 == 7) //last value of group
                    {
                        if (uDDGA.EventEnabled)
                            tempByte += (byte)0x80;

                        sEA.SendPacket[packetByteNumber] = tempByte;

                        packetByteNumber++; //next byte goes into next byte of packet
                    }
                    else
                    {
                        if (uDDGA.EventEnabled)
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
            }
            foreach(Control C in this.tabPageAnalogInputs2.Controls)
            {
                ucDNPDigitalGridAnalogIn uDDGA = new ucDNPDigitalGridAnalogIn();
                bool failed = false;

                try
                {
                    uDDGA = (ucDNPDigitalGridAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }
                if(!failed)
                {
                    if (i % 8 == 0)
                    {
                        tempByte = 0; //First value of byte, so clear tempByte
                        if (uDDGA.EventEnabled)
                            tempByte = 1;
                    }
                    else if (i % 8 == 7) //last value of group
                    {
                        if (uDDGA.EventEnabled)
                            tempByte += (byte)0x80;

                        sEA.SendPacket[packetByteNumber] = tempByte;

                        packetByteNumber++; //next byte goes into next byte of packet
                    }
                    else
                    {
                        if (uDDGA.EventEnabled)
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
            }

            sEA.SendPacket[sEA.SendPacket.Length-1] = 0x0D;
            if (this.Send != null)
                this.Send(this, sEA);
        }
    }
}
