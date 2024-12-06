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
    public partial class ucDNPDIGITALGRIDData : UserControl
    {
        public ucDNPDIGITALGRIDData()
        {
            InitializeComponent();
            SetSize();
            this.initializeComponents();
        }
        static readonly int _600V_ADDED = 190611;

        public ucDNPDIGITALGRIDData(Customers customer)
        {
            InitializeComponent();
            this.customer = customer;
            SetSize();
            this.initializeComponents();

        }

        public delegate void DNPPointChangedHandlder(object o, DNPPointEventArgs eA);
        public event DNPPointChangedHandlder PointChanged;

        public UInt32 RelayMasterRevision
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
        private UInt32 relayMasterRevision = 140506;
        private Customers customer = Customers.DIGITALGRIDDNP;


        private static int _packetLength = 98;

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

#if CONED
            this.binaryInputs.Add("Defaults Loaded");
            this.binaryInputs.Add("Network Protect Status/B Flag");
            this.binaryInputs.Add("Pump Protect Lockout");
            this.binaryInputs.Add("Not Available");
            this.binaryInputs.Add("WattVar");
            this.binaryInputs.Add("TimeDelay");
            this.binaryInputs.Add("Insensitve");
            this.binaryInputs.Add("Phase Angle Wrong");
            this.binaryInputs.Add("Differential Volts Too Low to Close");
            this.binaryInputs.Add("Digital Input 1");
            this.binaryInputs.Add("Digital Input 2");
            this.binaryInputs.Add("Digital Output 1");
            this.binaryInputs.Add("Digital Output 2");
            this.binaryInputs.Add("Blocked From Closing");
            this.binaryInputs.Add("Not Available");
            this.binaryInputs.Add("GE Relay");
            this.binaryInputs.Add("Calling For Float");
            this.binaryInputs.Add("Calling For Trip");
            this.binaryInputs.Add("Calling For Close");
            this.binaryInputs.Add("Breaker Status");
            this.binaryInputs.Add("Relax Close");
            this.binaryInputs.Add("Digital Input 3");
            this.binaryInputs.Add("Digital Input 4");
            this.binaryInputs.Add("Failure to Close");
            this.binaryInputs.Add("Failure to Trip");
#endif

            pointsToAdd = (uint)binaryInputs.Count;
            
            

            uint i = 0;

            foreach (string s in this.binaryInputs)
            {
                ucDNPMemphisBinary workingBox = new ucDNPMemphisBinary();

                workingBox.PointNumber = i;
                workingBox.PointName = s;
                workingBox.EventEnableVisible = true;
                workingBox.PointChanged += dNPPoint_PointChanged;

                this.addBinaryBox(workingBox, this.tabPageBinaryInputs);

                if (i == pointsToAdd)
                    break;
                i++;
            }

            int j = this.tabPageBinaryInputs.Controls.Count;
        }

        private void dNPPoint_PointChanged(object o, DNPPointEventArgs dPEA)
        {
            if (this.PointChanged != null)
            {
                this.PointChanged(this, dPEA);
            }
        }

        private void initializeBinaryOutputs()
        {
            uint pointsToAdd;

            if ((this.relayMasterRevision < 140107) || (this.customer != Customers.Atlanta && this.customer != Customers.Oncor))
                pointsToAdd = 20;
            else
                pointsToAdd = 22;

            this.binaryOutputs.Clear();

            this.tabPageBinaryOuputs.Controls.Clear();
#if CONED
            this.binaryOutputs.Add("Call For Trip");
            this.binaryOutputs.Add("Block From Closing");
            this.binaryOutputs.Add("Unblock ( Auto )");
            this.binaryOutputs.Add("Digital Out 1");
            this.binaryOutputs.Add("Digital Out 2");
            this.binaryOutputs.Add("Program Relay");
            this.binaryOutputs.Add("Clear Changes");
            this.binaryOutputs.Add("Enable Relaxed Close");
            this.binaryOutputs.Add("Disable Relaxed Close");
#endif
            uint i = 0;

            pointsToAdd = (uint)binaryOutputs.Count;
            foreach (string s in this.binaryOutputs)
            {
                ucDNPMemphisBinary workingBox = new ucDNPMemphisBinary();

                workingBox.PointNumber = i;
                workingBox.PointName = s;
                workingBox.EventEnableVisible = false;
                workingBox.PointChanged += dNPPoint_PointChanged;

                this.addBinaryBox(workingBox, this.tabPageBinaryOuputs);

                i++;

                if (i == pointsToAdd)
                    break;
            }
        }

        private void initializeAnalogInputs()
        {
            uint pointsToAdd;
            if (this.customer == Customers.Atlanta || this.customer == Customers.Oncor)
                pointsToAdd = 69;
            else
                pointsToAdd = 73;

            this.analogInputs.Clear();
            this.tabPageAnalogInputs1.Controls.Clear();
            this.tabPageAnalogInputs2.Controls.Clear();
#if CONED
            this.analogInputs.Add(new AnalogPointDefinition("ReClose Volts", false));   //0
            this.analogInputs.Add(new AnalogPointDefinition("ReClose Angle", false));   //1
            this.analogInputs.Add(new AnalogPointDefinition("Sensitive Trip", false));  //2
            this.analogInputs.Add(new AnalogPointDefinition("Time Delay", false));      //3
            this.analogInputs.Add(new AnalogPointDefinition("Instant Trip current", false));    //4
            this.analogInputs.Add(new AnalogPointDefinition("Insensitive Trip current", false));//5
            this.analogInputs.Add(new AnalogPointDefinition("CT Ratio", false));        //6
            this.analogInputs.Add(new AnalogPointDefinition("Phase Compensation angle", false)); //7
            this.analogInputs.Add(new AnalogPointDefinition("Extended Delay", false));  //8
            this.analogInputs.Add(new AnalogPointDefinition("ReClose Time Delay", false));  //9
            this.analogInputs.Add(new AnalogPointDefinition("Sensitive Trip Delay", false));//10
            this.analogInputs.Add(new AnalogPointDefinition("Phase1 Network Current", false));  //11
            this.analogInputs.Add(new AnalogPointDefinition("Phase2 Network Current", false));  //12
            this.analogInputs.Add(new AnalogPointDefinition("Phase3 Network Current", false));  //13
            this.analogInputs.Add(new AnalogPointDefinition("Phase1 Network Voltage", false));  //14
            this.analogInputs.Add(new AnalogPointDefinition("Phase2 Network Voltage", false));  //15
            this.analogInputs.Add(new AnalogPointDefinition("Phase3 Network Voltage", false));  //16
            this.analogInputs.Add(new AnalogPointDefinition("Phase1 Differential Voltage", false));  //17
            this.analogInputs.Add(new AnalogPointDefinition("Phase2 Differential Voltage", false));  //18
            this.analogInputs.Add(new AnalogPointDefinition("Phase3 Differential Voltage", false));  //19
            this.analogInputs.Add(new AnalogPointDefinition("Phase1 Network Current Angle", false)); //20
            this.analogInputs.Add(new AnalogPointDefinition("Phase2 Network Current Angle", false)); //21
            this.analogInputs.Add(new AnalogPointDefinition("Phase3 Network Current Angle", false)); //22
            this.analogInputs.Add(new AnalogPointDefinition("Analog 1", false));    //23
            this.analogInputs.Add(new AnalogPointDefinition("Analog 2", false));    //24
            this.analogInputs.Add(new AnalogPointDefinition("Analog 3", false));    //25
            this.analogInputs.Add(new AnalogPointDefinition("Analog 4", false));    //26
            this.analogInputs.Add(new AnalogPointDefinition("Relay Temperature", false));//27
            this.analogInputs.Add(new AnalogPointDefinition("Relay Cycle Counter", false));//28
            this.analogInputs.Add(new AnalogPointDefinition("Relay SN", false));//29
            this.analogInputs.Add(new AnalogPointDefinition("Software Version", false));//30
            this.analogInputs.Add(new AnalogPointDefinition("Comm Software Version", false));//31
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 KW", false));  //32
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 KW", false));  //33
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 KW", false));  //34
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 KVAR", false));//35
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 KVAR", false));//36
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 KVAR", false));//37
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 KVA", false));//38
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 KVA", false));//39
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 KVA", false));//40
            this.analogInputs.Add(new AnalogPointDefinition("Total KW", false));//41
            this.analogInputs.Add(new AnalogPointDefinition("Total KVAR", false));//42
            this.analogInputs.Add(new AnalogPointDefinition("Total KVA", false));//43
            this.analogInputs.Add(new AnalogPointDefinition("Analog 5", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 6", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 7", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 8", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 Differential Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 Differential Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 Differential Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 Differential Voltage (mag)", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 Differential Voltage (mag)", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 Differential Voltage (mag)", false));
            this.analogInputs.Add(new AnalogPointDefinition("Trip Tilt Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Close Tilt Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("GE Zero Adjust", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage 1", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage 2", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage 3", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Reclose Volts Settings", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Reclose Angle Settings", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Close Active Time", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Phasing Voltage offset setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("NWP Cycle Counter", false));
#endif
            uint i = 0;

            foreach (AnalogPointDefinition aPD in this.analogInputs)
            {

                ucDNPDIGITALGRIDAnalogIn workingBox = new ucDNPDIGITALGRIDAnalogIn();

                workingBox.PointNumber = i;
                workingBox.PointName = aPD.Name;
                workingBox.Signed = aPD.Signed;
                workingBox.PointChanged += dNPPoint_PointChanged;

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
#if CONED
            this.analogOutputs.Add(new AnalogPointDefinition("ReClose Volt Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("ReClose Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Sensitive Trip Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Instant Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Insensitive Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("CT Ratio", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phase Compensation", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Extended Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("ReClose Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Sensetive Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("GE Zero Adj", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Reclose Volts Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Reclose Angle Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Phasing Voltage Offset Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Close Active Time", false));
            this.analogOutputs.Add(new AnalogPointDefinition("NWP Cycle Counter", false));
#endif
            
            uint i = 0;
            if (pointsToAdd != 0)
            {
                foreach (AnalogPointDefinition aPD in this.analogOutputs)
                {

                    ucDNPDIGITALGRIDAnalogOut workingBox = new ucDNPDIGITALGRIDAnalogOut();

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
        private void addAnalogBoxIn(ucDNPDIGITALGRIDAnalogIn box, TabPage tB)
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

        public delegate void DIGITALGRIDSendEventHandler(object o, SendEventArgs mEA);
        public event DIGITALGRIDSendEventHandler Send;

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
            {
                this.buttonSendBinaryEventEnables.Visible = true;
                this.buttonEnableAllBinaryEvents.Visible = true;
                this.buttonDisableAllBinaryEvents.Visible = true;
            }
            else
            {
                this.buttonSendBinaryEventEnables.Visible = false;
                this.buttonEnableAllBinaryEvents.Visible = false;
                this.buttonDisableAllBinaryEvents.Visible = false;
            }
        }

        #endregion

        #region Incoming Data Handling

        public void SetAll(byte[] bytePacket, int p)
        {
            for (int packetIndex = 0, dataIndex = 252 * (p - 1); packetIndex < 252; packetIndex++, dataIndex++)
            {
                this.dNPData[dataIndex] = bytePacket[packetIndex];
            }

            if (p == 4)
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

                if (!failed)
                {
                    uDMB.CheckValue = this.convertDataByteToBool(bytePacket[index]);
                    if ((bytePacket[index + 2] & 0x02) == 0x02)
                        uDMB.EventEnabled = true;
                    else
                        uDMB.EventEnabled = false;

                    if ((bytePacket[index + 2] & 0x01) == 0x01)
                        uDMB.PointEnabled = true;
                    else
                        uDMB.PointEnabled = false;


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

                if ((bytePacket[index + 2] & 0x01) == 0x01)
                    uDMB.PointEnabled = true;
                else
                    uDMB.PointEnabled = false;

                index += 4;
            }

            return index;
        }

        private int setAnalogInputs(byte[] bytePacket, int index)
        {
            foreach (Control C in this.tabPageAnalogInputs1.Controls)
            {
                ucDNPDIGITALGRIDAnalogIn uDDGA = new ucDNPDIGITALGRIDAnalogIn();
                bool failed = false;

                try
                {
                    uDDGA = (ucDNPDIGITALGRIDAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }

                if (!failed)
                {
                    uDDGA.EventEnabled = this.convertAnalogControlByteToBool(bytePacket[index + 4]);
                    if ((bytePacket[index + 4] & 0x01) == 0x01)
                        uDDGA.PointEnabled = true;
                    else
                        uDDGA.PointEnabled = false;
                    uDDGA.PointValue = this.convertDataBytesToAnalog(bytePacket, index);
                    index += 6;
                }
            }
            foreach (Control C in this.tabPageAnalogInputs2.Controls)
            {
                ucDNPDIGITALGRIDAnalogIn uDDGA = new ucDNPDIGITALGRIDAnalogIn();
                bool failed = false;

                try
                {
                    uDDGA = (ucDNPDIGITALGRIDAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }

                if (!failed)
                {
                    uDDGA.EventEnabled = this.convertAnalogControlByteToBool(bytePacket[index + 4]);
                    if ((bytePacket[index + 4] & 0x01) == 0x01)
                        uDDGA.PointEnabled = true;
                    else
                        uDDGA.PointEnabled = false;
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
            if (this.tabControlMemphisDNP.SelectedTab == this.tabPageAnalogInputs1)
            {
                if (this.tabPageAnalogInputs2.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs2.Controls.Remove(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs2.Controls.Remove(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs2.Controls.Remove(this.buttonDisableAllAnalogEvents);
                }
                if (!this.tabPageAnalogInputs1.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs1.Controls.Add(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs1.Controls.Add(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs1.Controls.Add(this.buttonDisableAllAnalogEvents);
                }
                this.buttonSendAnalogEnables.Location = new Point(this.tabPageAnalogInputs1.Width - this.buttonSendAnalogEnables.Width - 2, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonSendAnalogEnables.Visible = true;
                this.buttonEnableAllAnalogEvents.Location = new Point(this.tabPageAnalogInputs1.Width - this.buttonEnableAllAnalogEvents.Width - this.buttonSendAnalogEnables.Width - 4, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonEnableAllAnalogEvents.Visible = true;
                this.buttonDisableAllAnalogEvents.Location = new Point(this.tabPageAnalogInputs1.Width - this.buttonDisableAllAnalogEvents.Width * 3 + 23, this.tabPageAnalogInputs1.Height - this.buttonEnableAllAnalogEvents.Height - 2);
                this.buttonDisableAllAnalogEvents.Visible = true;
            }
            else if (this.tabControlMemphisDNP.SelectedTab == this.tabPageAnalogInputs2)
            {
                if (this.tabPageAnalogInputs1.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs1.Controls.Remove(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs1.Controls.Remove(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs1.Controls.Remove(this.buttonDisableAllAnalogEvents);
                }
                if (!this.tabPageAnalogInputs2.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs2.Controls.Add(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs2.Controls.Add(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs2.Controls.Add(this.buttonDisableAllAnalogEvents);
                }
                this.buttonSendAnalogEnables.Location = new Point(this.tabPageAnalogInputs2.Width - this.buttonSendAnalogEnables.Width - 2, this.tabPageAnalogInputs2.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonSendAnalogEnables.Visible = true;
                this.buttonEnableAllAnalogEvents.Location = new Point(this.tabPageAnalogInputs1.Width - this.buttonEnableAllAnalogEvents.Width - this.buttonSendAnalogEnables.Width - 4, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonEnableAllAnalogEvents.Visible = true;
                this.buttonDisableAllAnalogEvents.Location = new Point(this.tabPageAnalogInputs1.Width - this.buttonDisableAllAnalogEvents.Width * 3 + 23, this.tabPageAnalogInputs1.Height - this.buttonEnableAllAnalogEvents.Height - 2);
                this.buttonDisableAllAnalogEvents.Visible = true;
            }
            else if (this.tabControlMemphisDNP.SelectedTab == this.tabPageBinaryInputs)
            {
                if (!this.tabPageBinaryInputs.Controls.Contains(this.buttonSendBinaryEventEnables))
                {
                    this.tabPageBinaryInputs.Controls.Add(this.buttonSendBinaryEventEnables);
                    this.tabPageBinaryInputs.Controls.Add(this.buttonEnableAllBinaryEvents);
                    this.tabPageBinaryInputs.Controls.Add(this.buttonDisableAllBinaryEvents);
                }
                this.buttonSendBinaryEventEnables.Location = new Point(this.tabPageBinaryInputs.Width - this.buttonSendBinaryEventEnables.Width - 2, this.tabPageBinaryInputs.Height - this.buttonSendBinaryEventEnables.Height - 2);
                this.buttonSendBinaryEventEnables.Visible = true;
                this.buttonEnableAllBinaryEvents.Location = new Point(this.tabPageBinaryInputs.Width - this.buttonSendBinaryEventEnables.Width - this.buttonEnableAllBinaryEvents.Width - 4, this.tabPageBinaryInputs.Height - this.buttonSendBinaryEventEnables.Height - 2);
                this.buttonEnableAllBinaryEvents.Visible = true;
                this.buttonDisableAllBinaryEvents.Location = new Point(this.tabPageBinaryInputs.Width - this.buttonEnableAllBinaryEvents.Width * 3 - 23, this.tabPageBinaryInputs.Height - this.buttonEnableAllBinaryEvents.Height - 2);
                this.buttonDisableAllBinaryEvents.Visible = true;
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

                if (!failed)
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
                ucDNPDIGITALGRIDAnalogIn uDDGA = new ucDNPDIGITALGRIDAnalogIn();
                bool failed = false;

                try
                {
                    uDDGA = (ucDNPDIGITALGRIDAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }
                if (!failed)
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
            foreach (Control C in this.tabPageAnalogInputs2.Controls)
            {
                ucDNPDIGITALGRIDAnalogIn uDDGA = new ucDNPDIGITALGRIDAnalogIn();
                bool failed = false;

                try
                {
                    uDDGA = (ucDNPDIGITALGRIDAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }
                if (!failed)
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

            sEA.SendPacket[sEA.SendPacket.Length - 1] = 0x0D;
            if (this.Send != null)
                this.Send(this, sEA);
        }

        private void buttonEnableAllBinaryEvents_Click(object sender, EventArgs e)
        {
            if (PointChanged != null)
            {
                this.PointChanged(this, new DNPPointEventArgs(true));
            }
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

                if (!failed)
                {
                    uDMB.EventEnabled = true;
                }
            }
        }

        private void buttonEnableAllAnalogEvents_Click(object sender, EventArgs e)
        {
            ucDNPDIGITALGRIDAnalogIn uDDGA = new ucDNPDIGITALGRIDAnalogIn();
            bool failed = false;

            if (PointChanged != null)
            {
                this.PointChanged(this, new DNPPointEventArgs(true));
            }
            foreach (Control C in this.tabPageAnalogInputs2.Controls)
            {
                try
                {
                    uDDGA = (ucDNPDIGITALGRIDAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }
                if (!failed)
                {
                    uDDGA.EventEnabled = true;
                }
            }
            foreach (Control C in this.tabPageAnalogInputs1.Controls)
            {
                try
                {
                    uDDGA = (ucDNPDIGITALGRIDAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }
                if (!failed)
                {
                    uDDGA.EventEnabled = true;
                }
            }
        }

        private void buttonDisableAllBinaryEvents_Click(object sender, EventArgs e)
        {
            if (PointChanged != null)
            {
                this.PointChanged(this, new DNPPointEventArgs(false));
            }
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

                if (!failed)
                {
                    uDMB.EventEnabled = false;
                }
            }
        }

        private void buttonDisableAllAnalogEvents_Click(object sender, EventArgs e)
        {
            ucDNPDIGITALGRIDAnalogIn uDDGA = new ucDNPDIGITALGRIDAnalogIn();
            bool failed = false;

            if (PointChanged != null)
            {
                this.PointChanged(this, new DNPPointEventArgs(false));
            }
            foreach (Control C in this.tabPageAnalogInputs2.Controls)
            {
                try
                {
                    uDDGA = (ucDNPDIGITALGRIDAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }
                if (!failed)
                {
                    uDDGA.EventEnabled = false;
                }
            }
            foreach (Control C in this.tabPageAnalogInputs1.Controls)
            {
                try
                {
                    uDDGA = (ucDNPDIGITALGRIDAnalogIn)C;
                }
                catch
                {
                    failed = true;
                }
                if (!failed)
                {
                    uDDGA.EventEnabled = false;
                }
            }
        }
    }

    public class DNPPointEventArgs : EventArgs
    {
        public DNPPointEventArgs(bool eventState)
        {
            this.EventState = eventState;
        }

        public bool EventState = false;
    }
}
