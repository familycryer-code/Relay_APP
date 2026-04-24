using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SharedResources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            tabControlMemphisDNP.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlMemphisDNP.DrawItem += tabControlMemphisDNP_DrawItem;
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
        //private byte[] dNPData = new byte[1008]; //252 packet size * 4
#if ONCOR
        private byte[] dNPData = new byte[1043]; //per the new increased data size coming for Oncor master firmware : 1008 + 30 extra bytes + 4 bytes  added at end by analog output function
#elif CONED
        private byte[] dNPData = new byte[1165];//[1196]; //per the new increased data size coming for ConED master firmware : 
        // index goes to 1025 at starting point of analog ouput reads. so, 35*4 bytes more after that
#elif ENMAX
        // It is seen that in the incoming dNPData array ( from master uP ), the bytes corresponding to serial number ( 4 & 12 for SN3076 ) are placed at index 712 and 713.
        // Accordingly, since EnMax DNP Map has Serial Number at its DNP Analog Input 1 itself,
        // we take 712 as the starting point to read Analog Inputs
        // Enmax has 118 Analog Inputs and 38 analog Outputs
        // so ( 118 * 6 ) points ( since each analog input = 6 bytes ) + ( 38 * 4 ) ( since each analog output = 4 bytes ) 
        // = 712 + 708 + 152 = 1572
        private byte[] dNPData = new byte[1572]; 
#elif TORONTO_HYDRO
        // It is seen that in the incoming dNPData array ( from master uP ), the bytes corresponding to serial number ( 4 & 12 for SN3076 ) are placed at index 8886 and 887.
        // TorontoHydro DNP Map has Serial Number at its DNP Analog Input 29,
        // So, to get to the starting point of analog inputs : 886 - (29*6) ( since each analog input = 6 bytes ) = 712
        // TorontoHydro has 44 Analog Inputs and 28 Analog Outputs
        // so ( 44 * 6 ) points ( since each analog input = 6 bytes ) + ( 28 * 4 ) ( since each analog output = 4 bytes ) 
        // = 712 + 264 + 112 = 1088
        private byte[] dNPData = new byte[1088]; 
#else
        private byte[] dNPData = new byte[1196];//[1008];  //252 packet size * 4
        // index goes to 1120 at starting point of analog ouput reads. so, 19*4 bytes more after that
#endif
        private UInt32 relayMasterRevision = 260207;//140506;
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

        private void tabControlMemphisDNP_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tab = tabControlMemphisDNP.TabPages[e.Index];
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Background of the selected tab title 
            using (var backBrush = new SolidBrush(isSelected ? Color.FromArgb(255, 215, 0) : SystemColors.Control)) // selected tab title has a gold colored background
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            // Text color: blue for selected, gray for others (adjust as needed)
            var textColor = isSelected ? Color.Black : SystemColors.ControlText; // Color.Black is the color of the selected tab title
            using (var textBrush = new SolidBrush(textColor))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.DrawString(tab.Text, e.Font, textBrush, e.Bounds, format);
            }

            // Optional focus rectangle
            e.DrawFocusRectangle();
        }

        private void initializeBinaryInputs()
        {
            //     uint pointsToAdd = 46;
            uint pointsToAdd;
            this.binaryInputs.Clear();

            this.tabPageBinaryInputs.Controls.Clear();
            this.tabPageBinaryInputs2.Controls.Clear();
           
#if CONED
            this.binaryInputs.Add("Defaults Loaded");//0
            this.binaryInputs.Add("Network Protect Status/B Flag");
            this.binaryInputs.Add("Not Available");
            this.binaryInputs.Add("Not Available");
            this.binaryInputs.Add("WattVar");
            this.binaryInputs.Add("TimeDelay");
            this.binaryInputs.Add("Insensitve");
            this.binaryInputs.Add("Phase Angle Wrong");
            this.binaryInputs.Add("Differential Volts Too Low to Close");
            this.binaryInputs.Add("Digital Input 1");
            this.binaryInputs.Add("Digital Input 2");//10
            this.binaryInputs.Add("Digital Output 1");
            this.binaryInputs.Add("Digital Output 2");
            this.binaryInputs.Add("Blocked From Closing");
            this.binaryInputs.Add("Not Available");
            this.binaryInputs.Add("GE Relay");
            this.binaryInputs.Add("Calling For Float");
            this.binaryInputs.Add("Calling For Trip");
            this.binaryInputs.Add("Calling For Close");
            this.binaryInputs.Add("Breaker Status");
            this.binaryInputs.Add("Relax Close");//20
            this.binaryInputs.Add("Digital Input 3");
            this.binaryInputs.Add("Digital Input 4");
            this.binaryInputs.Add("Failure to Close");
            this.binaryInputs.Add("Failure to Trip");

            //Additional points per rev10 ConEd firmware
            this.binaryInputs.Add("Motor TimeOut");
            this.binaryInputs.Add("Motor Cycles");
            this.binaryInputs.Add("A-Flag");
            this.binaryInputs.Add("Circle Close");
            this.binaryInputs.Add("Trim curve");
            this.binaryInputs.Add("Relay Cycles");//30
            this.binaryInputs.Add("Trip on Power Down");
            this.binaryInputs.Add("Relay Algorithm");
            this.binaryInputs.Add("Override Blocked Open");
            this.binaryInputs.Add("Test Relay Ack");
            this.binaryInputs.Add("Sensitive");
            this.binaryInputs.Add("Never Reclose");
            this.binaryInputs.Add("Safe Service");
            this.binaryInputs.Add("Relay Failure");
            this.binaryInputs.Add("XP Detected");
            this.binaryInputs.Add("Phase ACB");//40
            this.binaryInputs.Add("Relay Phased OK");
            this.binaryInputs.Add("Insensitive Back Feed Detected");
            this.binaryInputs.Add("Command Lockout");
            this.binaryInputs.Add("Pump Protect Lockout");//44


#endif
#if SCE
            this.binaryInputs.Add("Time Delay Mode"); //0
            this.binaryInputs.Add("Insensitive Mode");//1
            this.binaryInputs.Add("Watt-Var Mode");//2
            this.binaryInputs.Add("AVG3");//3
            this.binaryInputs.Add("AND3");//4
            this.binaryInputs.Add("AVG Phase 1");//5
            this.binaryInputs.Add("AVG Phase 2");//6
            this.binaryInputs.Add("AVG Phase 3");//7
            this.binaryInputs.Add("GE Type Relay");//8
            this.binaryInputs.Add("Float");//9
            this.binaryInputs.Add("Calling for Trip");//10
            this.binaryInputs.Add("Calling for Close");//11
            this.binaryInputs.Add("Digital In 1");//
            this.binaryInputs.Add("Digital In 2");
            this.binaryInputs.Add("Digital In 3");
            this.binaryInputs.Add("Digital In 4");
            this.binaryInputs.Add("Defaults Loaded");
            this.binaryInputs.Add("Phase Angle Wrong");
            this.binaryInputs.Add("Differential Volts Too Low to close");
            this.binaryInputs.Add("Blocked from Closing");
            this.binaryInputs.Add("Digital Out 1 Status");
            this.binaryInputs.Add("Digital Out 2 Status");
#endif
#if PSEG
            this.binaryInputs.Add("Calling for Open"); //0
            this.binaryInputs.Add("Calling for Close");
            this.binaryInputs.Add("Float");
            this.binaryInputs.Add("Blocked from Closing");
            this.binaryInputs.Add("Relay Phasing OK");
            this.binaryInputs.Add("Pump Protect Lockout");
            this.binaryInputs.Add("Network Protector Status / B Flag");
            this.binaryInputs.Add("Defaults Loaded");
            this.binaryInputs.Add("Phase ACB");
            this.binaryInputs.Add("Insensetive Backfeed Detected");
            this.binaryInputs.Add("A Flag");
            this.binaryInputs.Add("Digital In 1");
            this.binaryInputs.Add("Digital In 2");
            this.binaryInputs.Add("SEC Physical Lockout");
            this.binaryInputs.Add("Relax CLose");
            this.binaryInputs.Add("Sensitive Trip");
            this.binaryInputs.Add("Insensitive Trip");//16
            this.binaryInputs.Add("Time Delay Trip");
            this.binaryInputs.Add("Watt Var Trip");
            this.binaryInputs.Add("Trip On Power Down");
            this.binaryInputs.Add("Trim Curve Enabled");
            this.binaryInputs.Add("Circle Close Enabled");//21
            this.binaryInputs.Add("Override Blocked Open");
            this.binaryInputs.Add("Relay Algorithm");
            this.binaryInputs.Add("Pump Mode Relay Cycles");
            this.binaryInputs.Add("Pump Mode Motor Cycles");
            this.binaryInputs.Add("Pump Mode Motor Timeout");
            this.binaryInputs.Add("Pump Mode Never Reclose");
            this.binaryInputs.Add("Safe Service Enabled");
            this.binaryInputs.Add("PLC Lockout");
            this.binaryInputs.Add("SEC Digital C");
            this.binaryInputs.Add("SEC Digital D");
            this.binaryInputs.Add("SEC Digital E");
            this.binaryInputs.Add("SEC Digital F");
            this.binaryInputs.Add("SEC Digital G");
            this.binaryInputs.Add("SEC Digital H");
            this.binaryInputs.Add("Q Bit");//36
#endif
#if ENMAX || DEBUG
            this.binaryInputs.Add("Calling for Trip"); //0
            this.binaryInputs.Add("Calling for Close");
            this.binaryInputs.Add("Float");
            this.binaryInputs.Add("Blocked Open");
            this.binaryInputs.Add("Relay Phasing OK");
            this.binaryInputs.Add("Pump Protect Lockout");
            this.binaryInputs.Add("Network Protector Status / B Flag");
            this.binaryInputs.Add("Defaults Loaded");
            this.binaryInputs.Add("Phase ACB");
            this.binaryInputs.Add("Insensetive Backfeed Detected");
            this.binaryInputs.Add("A Flag");
            this.binaryInputs.Add("Digital In 1");
            this.binaryInputs.Add("Digital In 2");
            this.binaryInputs.Add("Physical Lockout Enabled");
            this.binaryInputs.Add("Relax CLose");
            this.binaryInputs.Add("Sensitive Trip Enabled");
            this.binaryInputs.Add("InSensitive");
            this.binaryInputs.Add("Time Delay");
            this.binaryInputs.Add("Watt Var");
            this.binaryInputs.Add("Trip On Power Down");
            this.binaryInputs.Add("Trim Curve Enabled"); // 20
            this.binaryInputs.Add("Cirlce Close Enabled");
            this.binaryInputs.Add("Override Blocked Open");
            this.binaryInputs.Add("Relay Algorithm");
            this.binaryInputs.Add("Pump Mode Relay Cycles");
            this.binaryInputs.Add("Pump Mode Motor Cycles");
            this.binaryInputs.Add("Pump Mode Motor Timeout");
            this.binaryInputs.Add("Pump Mode Never Reclose");
            this.binaryInputs.Add("Safe Service Enabled");
            this.binaryInputs.Add("PLC Lockout"); // 29
            this.binaryInputs.Add("SEC Digital C");
            this.binaryInputs.Add("SEC Digital D");
            this.binaryInputs.Add("SEC Digital E");
            this.binaryInputs.Add("SEC Digital F");
            this.binaryInputs.Add("SEC Digital G");
            this.binaryInputs.Add("SEC Digital H");
            this.binaryInputs.Add("Q Bit"); //36
            this.binaryInputs.Add("DNP277 In");
            this.binaryInputs.Add("DNP Out Scaling");
            this.binaryInputs.Add("DNP347 In"); // 39
            this.binaryInputs.Add("DigitalIn 3");
            this.binaryInputs.Add("DigitalIn 4");
            this.binaryInputs.Add("DigitalOut 1");
            this.binaryInputs.Add("DigitalOut 2");
            this.binaryInputs.Add("AVG3");
            this.binaryInputs.Add("AVD3");
            this.binaryInputs.Add("AVG3_Phase1");
            this.binaryInputs.Add("AVG3_Phase2");
            this.binaryInputs.Add("AVG3_Phase3");
            this.binaryInputs.Add("GE Relay Enabled");
            this.binaryInputs.Add("PhaseAngle Incorrect To Close");
            this.binaryInputs.Add("DifferentialVolts ToLow ToClose");
            this.binaryInputs.Add("Test Relay Ack");
            this.binaryInputs.Add("Failure To Close");
            this.binaryInputs.Add("Failure To Trip");
            this.binaryInputs.Add("Relay Failure");
            this.binaryInputs.Add("Network Volts ToLowToClose");
            this.binaryInputs.Add("XPDetected"); // 57
#endif
//#if (ONCOR || DEBUG || TORONTO_HYDRO) && !ENMAX
#if (ONCOR || TORONTO_HYDRO) && !ENMAX
            this.binaryInputs.Add("Not Available");  //0
            this.binaryInputs.Add("Defaults Loaded"); //1
            this.binaryInputs.Add("Network Volts too Low to Close");
            this.binaryInputs.Add("Not Available");
            this.binaryInputs.Add("Not Available"); //4
            this.binaryInputs.Add("WattVar");
            this.binaryInputs.Add("TimeDelay");
            this.binaryInputs.Add("Insensitve"); //7
            this.binaryInputs.Add("Phase Angle Wrong");
            this.binaryInputs.Add("Differential Volts Too Low to Close"); //9
            this.binaryInputs.Add("Not Available");
            this.binaryInputs.Add("Network Protector Status / B Flag");
            this.binaryInputs.Add("Not Available");
            this.binaryInputs.Add("Test Relay Ack");
            this.binaryInputs.Add("Blocked From Closing");
            this.binaryInputs.Add("Not Available");
            this.binaryInputs.Add("GE Type Relay"); //16
            this.binaryInputs.Add("Calling for Float");
            this.binaryInputs.Add("Calling for Open");
            this.binaryInputs.Add("Calling for Close");
            this.binaryInputs.Add("Not Available");
            this.binaryInputs.Add("Relax Close Active");
            this.binaryInputs.Add("A Flag");
            this.binaryInputs.Add("Digital In 4");//23
            
#endif
            pointsToAdd = (uint)binaryInputs.Count;
            pointsToAdd += 12;

            uint i = 0;

            foreach (string s in this.binaryInputs)
            {
                ucDNPMemphisBinary workingBox = new ucDNPMemphisBinary();

                workingBox.PointNumber = i;
                workingBox.PointName = s;
                workingBox.EventEnableVisible = true;
                workingBox.PointChanged += dNPPoint_PointChanged;

#if !ENMAX
                if (i < 50)

                this.addBinaryBoxIn(workingBox, this.tabPageBinaryInputs);

                if (i == pointsToAdd)
                    break;
                i++;

#elif ENMAX
                // These numbers go according to the number of rows ( which is same as number of points displayed in each column ) that are set in the " addBinaryBoxIn() "
                // For ENMAX  since it is set to 15 rows. We can have a max of 2 columns worth of data displayed properly given the Tahoma 12 font and the form size etc
                // So we can have a max of 30 such Analog points ( 20 per column ) that can be displayed on one tab

                if (i <= 29)            // So 30 Analog Input points 0 to 29 will be displayed on the first BinaryInputs tab
                    this.addBinaryBoxIn(workingBox, this.tabPageBinaryInputs);
                else if (i <= 57)       // So 28 Analog Input points 30 to 57 will be displayed on the second BinaryInputs tab
                    this.addBinaryBoxIn(workingBox, this.tabPageBinaryInputs2);


                if (i == pointsToAdd)
                    break;
                i++;

#endif



            }

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

            //Additional points per rev10 ConEd firmware
            this.binaryOutputs.Add("Enabled Pump Mode Motor Cycles");
            this.binaryOutputs.Add("Trip On Power Down");
            this.binaryOutputs.Add("Override Blocked Close on Dead Network");
            this.binaryOutputs.Add("Relay Algorithm");
            this.binaryOutputs.Add("Not Used");
            this.binaryOutputs.Add("Command Test");
            this.binaryOutputs.Add("Not Used");
            this.binaryOutputs.Add("Safe Service Mode Enable");
            this.binaryOutputs.Add("Circle Close");
            this.binaryOutputs.Add("Trim Curve");
            this.binaryOutputs.Add("Enabled Pump Mode Relay Cycles");
            this.binaryOutputs.Add("Pump Lockout Never Reclose");
            this.binaryOutputs.Add("Clear Pump Protect Lockout");
            this.binaryOutputs.Add("Clear Cycle Counter");
            this.binaryOutputs.Add("Sensitive Trip");
            this.binaryOutputs.Add("Watt Var");
            this.binaryOutputs.Add("Time Delay");
            this.binaryOutputs.Add("Insenstive Trip");
            this.binaryOutputs.Add("Enable Pump Mode Motor Timeout");
            this.binaryOutputs.Add("Command Lockout");
#endif
#if SCE
            this.binaryOutputs.Add("Trip");
            this.binaryOutputs.Add("Block");
            this.binaryOutputs.Add("Unblock(Auto)");
            this.binaryOutputs.Add("Digital Out 1");
            this.binaryOutputs.Add("Digital Out 2");
            this.binaryOutputs.Add("Program Relay");
#endif
#if PSEG
            this.binaryOutputs.Add("Remote Trip");//0
            this.binaryOutputs.Add("Relax Close");
            this.binaryOutputs.Add("Block Open");
            this.binaryOutputs.Add("Sensitive Trip");
            this.binaryOutputs.Add("Insensitive Trip");
            this.binaryOutputs.Add("Time Delay");
            this.binaryOutputs.Add("Watt Var");
            this.binaryOutputs.Add("Trip On Power Down");
            this.binaryOutputs.Add("Trim Curve");
            this.binaryOutputs.Add("Circle Close");
            this.binaryOutputs.Add("Override Blocked Close on Dead Network");
            this.binaryOutputs.Add("Relay Algorithm");
            this.binaryOutputs.Add("Enabled Pump Mode Relay Cycles");
            this.binaryOutputs.Add("Enable Motor Cycles Pump Algorithm");
            this.binaryOutputs.Add("Enable Pump Mode Motor Timeout");
            this.binaryOutputs.Add("Pump Lockout Never Reclose");
            this.binaryOutputs.Add("Clear Pump Protect Lockout");
            this.binaryOutputs.Add("Clear Cycle Counter");
            this.binaryOutputs.Add("Safe Service Mode Enable");
            this.binaryOutputs.Add("Command Lockout");//19
#endif
#if ENMAX
            this.binaryOutputs.Add("Remote Trip");//0
            this.binaryOutputs.Add("Relax Close");
            this.binaryOutputs.Add("Block Open");
            this.binaryOutputs.Add("Sensitive Trip");
            this.binaryOutputs.Add("Insensitive Trip");
            this.binaryOutputs.Add("Time Delay");
            this.binaryOutputs.Add("Watt Var");
            this.binaryOutputs.Add("Trip On Power Down");
            this.binaryOutputs.Add("Trim Curve");
            this.binaryOutputs.Add("Circle Close");
            this.binaryOutputs.Add("Override Blocked Close on Dead Network"); // 10
            this.binaryOutputs.Add("Relay Algorithm");
            this.binaryOutputs.Add("Enabled Pump Mode Relay Cycles");
            this.binaryOutputs.Add("Enable Motor Cycles Pump Algorithm");
            this.binaryOutputs.Add("Enable Pump Mode Motor Timeout");
            this.binaryOutputs.Add("Pump Lockout Never Reclose");
            this.binaryOutputs.Add("Clear Pump Protect Lockout");
            this.binaryOutputs.Add("Clear Cycle Counter");
            this.binaryOutputs.Add("Safe Service Mode Enable"); // 18
            this.binaryOutputs.Add("DNP277 In");
            this.binaryOutputs.Add("DNPOut Scaling");
            this.binaryOutputs.Add("DNP347 In");
            this.binaryOutputs.Add("Command Lockout"); //22
            this.binaryOutputs.Add("Block And Trip");
            this.binaryOutputs.Add("Digital Out Cntl1");
            this.binaryOutputs.Add("Digital Out Cntl2");
            this.binaryOutputs.Add("Program Relay");
            this.binaryOutputs.Add("Command Test");
            this.binaryOutputs.Add("Clear Change");
            this.binaryOutputs.Add("Disable Relax Close");
            this.binaryOutputs.Add("Unblock Open");
#endif
            //#if (ONCOR || TORONTO_HYDRO) 
#if (TORONTO_HYDRO || DEBUG)
            this.binaryOutputs.Add("Remote Trip");//0
            this.binaryOutputs.Add("Block Open");
            this.binaryOutputs.Add("Not Available");
            this.binaryOutputs.Add("Not Available");
            this.binaryOutputs.Add("Command Test");
            this.binaryOutputs.Add("Not Available");
            this.binaryOutputs.Add("Not Available");
            this.binaryOutputs.Add("Relax Close");//7
           /* this.binaryOutputs.Add("Trim Curve");
            this.binaryOutputs.Add("Circle Close");
            this.binaryOutputs.Add("Override Blocked Close on Dead Network");
            this.binaryOutputs.Add("Relay Algorithm");
            this.binaryOutputs.Add("Enable Pump Mode Relay Cycles");
            this.binaryOutputs.Add("Enable Motor Cycles Pump Algorithm");
            this.binaryOutputs.Add("Enable Pump Mode Motor Timeout");
            this.binaryOutputs.Add("Pump Lockout Never Reclose");
            this.binaryOutputs.Add("Clear Pump Protect Lockout");
            this.binaryOutputs.Add("Clear Cycle Counter");
            this.binaryOutputs.Add("Safe Service Mode Enable");
            this.binaryOutputs.Add("Command Lockout");
            this.binaryOutputs.Add("Not Used");
            this.binaryOutputs.Add("Not Used");
            this.binaryOutputs.Add("Command Test");
           */
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
            /*   if (this.customer == Customers.Atlanta || this.customer == Customers.Oncor)
                   pointsToAdd = 69;
               else
                   pointsToAdd = 73;
            */
            this.analogInputs.Clear();
            this.tabPageAnalogInputs1.Controls.Clear();
            this.tabPageAnalogInputs2.Controls.Clear();
            this.tabPageAnalogInputs3.Controls.Clear();
#if CONED
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Volts", false));   //0
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Angle", false));   //1
            this.analogInputs.Add(new AnalogPointDefinition("Sensitive Trip", false));  //2
            this.analogInputs.Add(new AnalogPointDefinition("Time Delay", false));      //3
            this.analogInputs.Add(new AnalogPointDefinition("Instant Trip current", false));    //4
            this.analogInputs.Add(new AnalogPointDefinition("Insensitive Trip current", false));//5
            this.analogInputs.Add(new AnalogPointDefinition("CT Ratio", false));        //6
            this.analogInputs.Add(new AnalogPointDefinition("Phase Compensation angle", false)); //7
            this.analogInputs.Add(new AnalogPointDefinition("Extended Delay", false));  //8
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Time Delay", false));  //9
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
            this.analogInputs.Add(new AnalogPointDefinition("NWP Cycle Counter", false));//64

            //Additional points per rev10 ConEd firmware
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage (TV) Angle - Phase A ", false));//65
            this.analogInputs.Add(new AnalogPointDefinition("TV Angle - Phase B ", false));
            this.analogInputs.Add(new AnalogPointDefinition("TV Angle - Phase C ", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (NV) Angle - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("NV Angle - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("NV Angle - Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Average Differential Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Effective Current Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positvie Sequence Current Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Current Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase Angle A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase Angle B ", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase Angle C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Average Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Differential Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence  Differential Voltage Angle", false));//80
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Network Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Network Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase Compensation Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Angle Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Three Phase Power Factor", false));
            this.analogInputs.Add(new AnalogPointDefinition("Average Relay Differential Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Average", false));
            this.analogInputs.Add(new AnalogPointDefinition("Load (L) % A ", false));
            this.analogInputs.Add(new AnalogPointDefinition("L% B", false));
            this.analogInputs.Add(new AnalogPointDefinition("L% C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Effective Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Current", false));//95
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Differential Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Differential Voltage Negative Sequence", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage Total Harmonic Distortion (THD) Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage Total Harmonic Distortion (THD) Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage Total Harmonic Distortion (THD) Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Network Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Network Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog D", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog E", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog F", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog G", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog H", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog A1", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog A2", false));//111
#endif
#if SCE
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Volts", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Sensitive Trip", false));
            this.analogInputs.Add(new AnalogPointDefinition("Time Delay", false));
            this.analogInputs.Add(new AnalogPointDefinition("Instant Trip Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Insensitive Trip Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("CT Ratio", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase Compensation Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Extended Delay", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Time Delay", false));
            this.analogInputs.Add(new AnalogPointDefinition("Sensitive Trip Delay", false));//10
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 Network Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 Network Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 Network Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 Network Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 Network Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 Network Voltageo", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 Differential Voltage(real)", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 Differential Voltage(real)", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 Differential Voltage(real)y", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 Network Current Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 Network Current Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 Network Current Angle", false));//22
            this.analogInputs.Add(new AnalogPointDefinition("Analog 1", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 2", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 3", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 4", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Temperature", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Cycle Counter", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay SN", false));
            this.analogInputs.Add(new AnalogPointDefinition("Software Version", false));//30
            this.analogInputs.Add(new AnalogPointDefinition("Comm Software Version", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 KW", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 KW", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 KW", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 KVAR", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 KVAR", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 KVAR", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 KVA", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 KVA", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 KVA", false));//40
            this.analogInputs.Add(new AnalogPointDefinition("Total KW", false));
            this.analogInputs.Add(new AnalogPointDefinition("Total KVAR", false));
            this.analogInputs.Add(new AnalogPointDefinition("Total KVA", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 5", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 6", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 7", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 8", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 Differential Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 Differential Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 Differential Voltage Angle", false));//50
            this.analogInputs.Add(new AnalogPointDefinition("Phase 1 Differential Voltage (mag)", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 2 Differential Voltage (mag)", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase 3 Differential Voltage (mag)", false));
            this.analogInputs.Add(new AnalogPointDefinition("Trip Tilt Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Close Tilt Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("GE Zero Adjust", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage 1", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage 2", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage 3", false));//59
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Reclose Volts Settings", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Reclose Angle Settings", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Close Active Time", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relaxed Phasing Voltage Offset Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("NWP Cycle Counter", false));
#endif
#if PSEG
            this.analogInputs.Add(new AnalogPointDefinition("Serial Number", false));//0
            this.analogInputs.Add(new AnalogPointDefinition("Relay Version Number", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Transformer Vt A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Transformer Vt B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Transformer Vt C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage (TV) Angle - Phase A ", false));//5
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage (TV) Angle - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage (TV) Angle - Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) C", false));//10
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (NV) Angle - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (NV) Angle - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (NV) Angle - Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) B", false));//15
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd)  Angle A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd)  Angle B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd)  Angle C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Average Differential Voltage", false));//20
            this.analogInputs.Add(new AnalogPointDefinition("Average Differential Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Real Differential Voltage Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Real Differential Voltage Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Real Differential Voltage Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Average Relay Differential Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Phase Angle A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Phase Angle B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Phase Angle C", false));//31
            this.analogInputs.Add(new AnalogPointDefinition("Effective Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Effective Current Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positvie Sequence Current Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Current", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Current Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase C", false));//40
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase Angle A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase Angle B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase Angle C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Average", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Average Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Real Power Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Real Power Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Real Power Phase C", false));//48
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Differential Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Differential Voltage Negative Sequence", false));
            this.analogInputs.Add(new AnalogPointDefinition("Differential Sequence Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Network Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Network Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Network Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Network Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Vn Total Harmonic Distortion Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Vn Total Harmonic Distortion Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Vn Total Harmonic Distortion Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD Phase C", false));//62
            this.analogInputs.Add(new AnalogPointDefinition("NWP Internal/Relay Temperature", false));
            this.analogInputs.Add(new AnalogPointDefinition("NWP Cycle Count", false));
            this.analogInputs.Add(new AnalogPointDefinition("Aux Input 1", false));
            this.analogInputs.Add(new AnalogPointDefinition("Aux Input 2", false));
            this.analogInputs.Add(new AnalogPointDefinition("Aux Input 3", false));
            this.analogInputs.Add(new AnalogPointDefinition("Aux Input 4", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog D", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog E", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog F", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog G", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog H", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 1", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog 2", false));
            this.analogInputs.Add(new AnalogPointDefinition("Load (L) % A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Load (L) % B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Load (L) % C", false));//79
#endif

#if ENMAX
            // 118 Analog Input Points for DNP
             this.analogInputs.Add(new AnalogPointDefinition("Serial Number", false));//0
             this.analogInputs.Add(new AnalogPointDefinition("Relay Version Number", false));
             this.analogInputs.Add(new AnalogPointDefinition("Voltage Transformer Vt) A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Vt B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Vt C", false));
             this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage (TV) Angle - Phase A ", false));//5
             this.analogInputs.Add(new AnalogPointDefinition("TV Angle - Phase B", false));
             this.analogInputs.Add(new AnalogPointDefinition("TV Angle - Phase C", false));
             this.analogInputs.Add(new AnalogPointDefinition("Voltage Network (Vn)A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Vn B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Vn C", false));//10
             this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (NV) Angle - Phase A ", false));
             this.analogInputs.Add(new AnalogPointDefinition("NV Angle - Phase B", false));
             this.analogInputs.Add(new AnalogPointDefinition("NV Angle - Phase C", false));
             this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) A", false));
             this.analogInputs.Add(new AnalogPointDefinition("VdB", false));//15
             this.analogInputs.Add(new AnalogPointDefinition("VdC", false));
             this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd)  Angle A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd)  Angle B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd)  Angle C", false));
             this.analogInputs.Add(new AnalogPointDefinition("Average Differential Voltage", false));//20
             this.analogInputs.Add(new AnalogPointDefinition("Average Differential Voltage Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Real Differential Voltage Phase A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Real Differential Voltage Phase B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Real Differential Voltage Phase C", false));
             this.analogInputs.Add(new AnalogPointDefinition("Average Relay Differential Voltage", false));
             this.analogInputs.Add(new AnalogPointDefinition("Current Phase A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Current Phase B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Current Phase C", false));
             this.analogInputs.Add(new AnalogPointDefinition("Current Phase Angle A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Current Phase Angle B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Current Phase Angle C", false));//31
             this.analogInputs.Add(new AnalogPointDefinition("Effective Current", false));
             this.analogInputs.Add(new AnalogPointDefinition("Effective Current Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Current", false));
             this.analogInputs.Add(new AnalogPointDefinition("Positvie Sequence Current Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Current", false));
             this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Current Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase C", false));//40
             this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase Angle A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase Angle B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Phase Angle C", false));
             this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Average", false));
             this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Average Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Real Power Phase A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Real Power Phase B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Real Power Phase C", false));//48
             this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Differential Voltage", false));
             this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Voltage Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Negetive Sequence Differential Voltage", false));
             this.analogInputs.Add(new AnalogPointDefinition("Negetive Sequence Voltage Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Network Voltage", false)); //53
             this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Network Voltage Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Network Voltage", false));
             this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Network Voltage Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Vn Total Harmonic Distortion Phase A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Vn Total Harmonic Distortion Phase B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Vn Total Harmonic Distortion Phase C", false));
             this.analogInputs.Add(new AnalogPointDefinition("Current THD Phase A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Current THD Phase B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Current THD Phase C", false));//62
             this.analogInputs.Add(new AnalogPointDefinition("NWP Internal/Relay Temperature", false));
             this.analogInputs.Add(new AnalogPointDefinition("NWP Cycle Count", false));
             this.analogInputs.Add(new AnalogPointDefinition("Analog C", false));//65
             this.analogInputs.Add(new AnalogPointDefinition("Analog D", false));
             this.analogInputs.Add(new AnalogPointDefinition("Analog E", false));
             this.analogInputs.Add(new AnalogPointDefinition("Analog F", false));
             this.analogInputs.Add(new AnalogPointDefinition("Analog G", false));
             this.analogInputs.Add(new AnalogPointDefinition("Analog H", false));
             this.analogInputs.Add(new AnalogPointDefinition("Analog A1", false));
             this.analogInputs.Add(new AnalogPointDefinition("Analog A2", false));
             this.analogInputs.Add(new AnalogPointDefinition("Load Percentage A", false));
             this.analogInputs.Add(new AnalogPointDefinition("Load Percentage B", false));
             this.analogInputs.Add(new AnalogPointDefinition("Load Percentage C", false)); //75
             this.analogInputs.Add(new AnalogPointDefinition("Reclose Volts", false));
             this.analogInputs.Add(new AnalogPointDefinition("Extended Delay Setting", false));
             this.analogInputs.Add(new AnalogPointDefinition("Insensitive TripCurrent Setting", false));
             this.analogInputs.Add(new AnalogPointDefinition("Instant TripCurrent Setting", false));//79
             this.analogInputs.Add(new AnalogPointDefinition("Phase Compensation Setting", false));
             this.analogInputs.Add(new AnalogPointDefinition("Reclose Angle Setting", false)); //81
             this.analogInputs.Add(new AnalogPointDefinition("Sensitive TripCurrent Setting", false)); //82
             this.analogInputs.Add(new AnalogPointDefinition("Sensitive TripDelay Setting", false));//
             this.analogInputs.Add(new AnalogPointDefinition("Time Delay Setting", false));
             this.analogInputs.Add(new AnalogPointDefinition("CTRatio", false));
             this.analogInputs.Add(new AnalogPointDefinition("Phase Detection Volts", false));//
             this.analogInputs.Add(new AnalogPointDefinition("Close Tilt Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Phase A VAR", false));
             this.analogInputs.Add(new AnalogPointDefinition("Phase B VAR", false));
             this.analogInputs.Add(new AnalogPointDefinition("Phase C VAR", false)); //90
             this.analogInputs.Add(new AnalogPointDefinition("Total KVA", false));
             this.analogInputs.Add(new AnalogPointDefinition("Total KVAR", false));
             this.analogInputs.Add(new AnalogPointDefinition("Total KW", false));
             this.analogInputs.Add(new AnalogPointDefinition("Three Phase Power Factor", false));
             this.analogInputs.Add(new AnalogPointDefinition("Comm Software Version", false));  // 95
             this.analogInputs.Add(new AnalogPointDefinition("Reclose TimeDelay Setting", false));  
             this.analogInputs.Add(new AnalogPointDefinition("Relay Cycle Counter1", false)); 
             this.analogInputs.Add(new AnalogPointDefinition("Trip Tilt Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Reclose Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("AnalogIn_1", false));
             this.analogInputs.Add(new AnalogPointDefinition("AnalogIn_2", false)); // 101
             this.analogInputs.Add(new AnalogPointDefinition("AnalogIn_3", false));
             this.analogInputs.Add(new AnalogPointDefinition("AnalogIn_4", false));
             this.analogInputs.Add(new AnalogPointDefinition("AnalogIn_5", false));
             this.analogInputs.Add(new AnalogPointDefinition("AnalogIn_6", false));
             this.analogInputs.Add(new AnalogPointDefinition("AnalogIn_7", false));
             this.analogInputs.Add(new AnalogPointDefinition("AnalogIn_8", false)); //107
             this.analogInputs.Add(new AnalogPointDefinition("GE Zero Adj", false));
             this.analogInputs.Add(new AnalogPointDefinition("Relaxed Reclose Volts Settings", false));
             this.analogInputs.Add(new AnalogPointDefinition("Relaxed Reclose Angle Settings", false));
             this.analogInputs.Add(new AnalogPointDefinition("Relaxed Close Active Time", false));
             this.analogInputs.Add(new AnalogPointDefinition("Relaxed Phasing Volt Offset Settings", false));
             this.analogInputs.Add(new AnalogPointDefinition("Code Date", false));
             this.analogInputs.Add(new AnalogPointDefinition("WattVar Current", false));
             this.analogInputs.Add(new AnalogPointDefinition("WattVar Angle", false));
             this.analogInputs.Add(new AnalogPointDefinition("Vault Temperature", false));
             this.analogInputs.Add(new AnalogPointDefinition("Number Of MuxBoxes", false)); //117
#endif
#if (ONCOR || TORONTO_HYDRO || DEBUG)
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Volts Setting", false));//0
            this.analogInputs.Add(new AnalogPointDefinition("Phase Detection Angle Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Sensitive Trip Current Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Time Delay Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Instantaneous Trip Current SettingC", false));
            this.analogInputs.Add(new AnalogPointDefinition("Insensitive Trip Current Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("CT Ratio", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase Detection Offset Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Extended Time Delay Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Close Time Delay Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Sensitive Trip Time Delay Setting", false)); //10
            this.analogInputs.Add(new AnalogPointDefinition("Current (I) - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current (I) - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current (I) - Phase C", false)); //13
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) - Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) - Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Phase Angle - Phase A", false)); //20
            this.analogInputs.Add(new AnalogPointDefinition("Current Phase Angle - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current Phase Angle - Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog In 2 - Transformer Temperature", false));
            this.analogInputs.Add(new AnalogPointDefinition("Not Available", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog In 3 - Transformer Oil Level", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog In 4 - Transformer Pressure", false));
            this.analogInputs.Add(new AnalogPointDefinition("NWP Internal/Relay Temperature", false));
            this.analogInputs.Add(new AnalogPointDefinition("NWP Cycle Count ", false));
            this.analogInputs.Add(new AnalogPointDefinition("Serial Number", false));
            this.analogInputs.Add(new AnalogPointDefinition("Relay Version Number", false));
            this.analogInputs.Add(new AnalogPointDefinition("Com Software Version", false));//31
            this.analogInputs.Add(new AnalogPointDefinition("Real Power - Phase At", false));
            this.analogInputs.Add(new AnalogPointDefinition("Real Power - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Real Power - Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reactive Power - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reactive Power - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reactive Power - Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Apparent Power - Phase C", false));//40
            this.analogInputs.Add(new AnalogPointDefinition("Total KW", false));
            this.analogInputs.Add(new AnalogPointDefinition("Total KVAR", false));
            this.analogInputs.Add(new AnalogPointDefinition("Total KVA", false)); //43
            
         /*   this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Average Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Real Power - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Real Power - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Real Power - Phase C", false));//48
            this.analogInputs.Add(new AnalogPointDefinition("Reactive Power - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reactive Power - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reactive Power - Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Differential Voltage", false));//52
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Differential Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Network Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Network Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Network Voltage", false));
            this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Network Voltage Angle", false));
            this.analogInputs.Add(new AnalogPointDefinition("Vn Total Harmonic Distortion (THD) - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Vn Total Harmonic Distortion (THD) - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Vn Total Harmonic Distortion (THD) - Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Current THD - Phase C", false));//65
            this.analogInputs.Add(new AnalogPointDefinition("NWP Internal/Relay Temperature", false));
            this.analogInputs.Add(new AnalogPointDefinition("NWP Cycle Count", false));
            this.analogInputs.Add(new AnalogPointDefinition("TotalKVA", false));
            this.analogInputs.Add(new AnalogPointDefinition("TotalKVAR", false));
            this.analogInputs.Add(new AnalogPointDefinition("TotalKW", false));
            this.analogInputs.Add(new AnalogPointDefinition("3 Phase Power Factor", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog In 1", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog In 2 - Transformer Temperature", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog In 3 - Transformer Oil Level", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog In 4 - Transformer Pressure", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog In 5", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog In 6", false));
            this.analogInputs.Add(new AnalogPointDefinition("Analog In 7", false));//78
            this.analogInputs.Add(new AnalogPointDefinition("Load (L) % - Phase A", false));
            this.analogInputs.Add(new AnalogPointDefinition("Load (L) % - Phase B", false));
            this.analogInputs.Add(new AnalogPointDefinition("Load (L) % - Phase C", false));
            this.analogInputs.Add(new AnalogPointDefinition("CT Ratio Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Extended Delay Setting", false));//83
            this.analogInputs.Add(new AnalogPointDefinition("Insensitive Trip Current Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Phase Compensation Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Angle Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Volt", false));
            this.analogInputs.Add(new AnalogPointDefinition("Reclose Time Delay Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Sensitive Trip Current Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Sensitive Trip Delay Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Time Delay Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Instant Trip Current Setting", false));
            this.analogInputs.Add(new AnalogPointDefinition("Comm Software Version", false));//93
         */
#endif
            pointsToAdd = (uint)analogInputs.Count;
            pointsToAdd += 12;
            uint i = 0;

            foreach (AnalogPointDefinition aPD in this.analogInputs)
            {

                ucDNPDIGITALGRIDAnalogIn workingBox = new ucDNPDIGITALGRIDAnalogIn();

                workingBox.PointNumber = i;
                workingBox.PointName = aPD.Name;
                workingBox.Signed = aPD.Signed;
                workingBox.PointChanged += dNPPoint_PointChanged;

#if TORONTO_HYDRO
                if (i < 50)
                {
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs1);
                   
                }
           /*     else if (i <= 99) 
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs2);
                else if (i <= 111)
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs3);
           */
                if (i == pointsToAdd)
                    break;
                i++;
#elif CONED
                if (i <= 39) //(i <= 35)
                {
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs1);

                }
                else if (i <= 79) //(i <= 72) 
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs2);
                else if (i <= 111)
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs3);

                if (i == pointsToAdd)
                    break;
                i++;
#elif PSEG
                // These numbers go according to the number of rows ( which is same as number of points displayed in each column ) that are set in the " addAnalogBoxIn() "
                // For PSE&G  since it is set to 15 rows. We can have a max of 2 columns worth of data displayed properly given the Tahoma 12 font and the form size etc
                // So we can have a max of 30 such Analog points ( 15 per column ) that can be displayed on one tab
                if (i <= 29)            // So 30 Analog Input points 0 to 29 will be displayed on the first AnalogInputs tab
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs1);
                else if (i <= 59)       // So 30 Analog Input points 30 to 59 will be displayed on the first AnalogInputs tab
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs2);
                else if (i <= 79)       // So 19 Analog Input points 60 to 79 will be displayed on the first AnalogInputs tab
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs3);

                if (i == pointsToAdd)
                    break;
                i++;
#elif ENMAX
                // These numbers go according to the number of rows ( which is same as number of points displayed in each column ) that are set in the " addAnalogBoxIn() "
                // For ENMAX  since it is set to 20 rows. We can have a max of 2 columns worth of data displayed properly given the Tahoma 12 font and the form size etc
                // So we can have a max of 40 such Analog points ( 20 per column ) that can be displayed on one tab

                if (i <= 39)            // So 40 Analog Input points 0 to 39 will be displayed on the first AnalogInputs tab
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs1);
                else if (i <= 79)       // So 40 Analog Input points 40 to 79 will be displayed on the second AnalogInputs tab
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs2);
                else if (i <= 117)      // So 38 Analog Input points 80 to 118 will be displayed on the third AnalogInputs tab
                    this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs3);

                if (i == pointsToAdd)
                    break;
                i++;

#endif
            }
        }

        private void initializeAnalogOutputs()
        {
            uint pointsToAdd = 37;// 35;// 28;

            this.analogOutputs.Clear();
            this.tabPageAnalogOutputs.Controls.Clear();
#if CONED
            this.analogOutputs.Add(new AnalogPointDefinition("Reclose Volt Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Reclose Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Sensitive Trip Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Instant Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode - Insensitive Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("CT Ratio", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phase Compensation", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Extended Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Reclose Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Sensetive Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("GE Zero Adj", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Reclose Volts Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Reclose Angle Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Phasing Voltage Offset Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Close Active Time", false));
            this.analogOutputs.Add(new AnalogPointDefinition("NWP Cycle Counter", false));

            //Additional points per rev10 ConEd firmware
            this.analogOutputs.Add(new AnalogPointDefinition("Close Mode Phasing Detection Offset", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Mode Phasing Detection Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phasing Mode", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Lockout time", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Motor Cycles", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Motor Timeout", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Relay Cycle Limit", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Relay Cycle Time", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Mode Current Imbalance", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Mode Trip Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Mode Low Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Mode Overcurrent", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Mode Voltage Imbalance", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Mode Trim Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Style", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Watt-Var Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Watt-Var Current", false));
#endif
#if SCE
            this.analogOutputs.Add(new AnalogPointDefinition("Reclose Volts Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Reclose Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Sensitive Trip Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Instant Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Insensitive Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Watt Var Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Time Delay Trip Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Watt Var Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Time Delay Trip Extended Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("CT Ratio", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Sensitive Trip Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phase Compensation", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Unsolicited Fire Time", false));
#endif
#if PSEG
            this.analogOutputs.Add(new AnalogPointDefinition("Sensitive Time Delay", false));//0
            this.analogOutputs.Add(new AnalogPointDefinition("Sensitive Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Insensitive Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Instantaneous Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Time Delay Trip Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Extended Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Watt-Var Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Watt-Var Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Style", false));//9
            this.analogOutputs.Add(new AnalogPointDefinition("Trim Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Reclose Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Reclose Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phasing Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phasing Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("CT Ratio", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phasing Mode", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relay Cycle Limit", false));//18
            this.analogOutputs.Add(new AnalogPointDefinition("Cycle Time Limit", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Motor Cycles", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Motor Timeout", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Lockout time", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Trip Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Overcurrent", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Current Imbalance", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Low Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Voltage Imbalance", false));//27

#endif
#if (ONCOR || TORONTO_HYDRO)
            this.analogOutputs.Add(new AnalogPointDefinition("Sensitive Time Delay", false));//0
            this.analogOutputs.Add(new AnalogPointDefinition("Sensitive Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Insensitive Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Instantaneous Trip Current", false));//4
            this.analogOutputs.Add(new AnalogPointDefinition("Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Extended Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Watt-Var Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Watt-Var Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Style", false));//9
            this.analogOutputs.Add(new AnalogPointDefinition("Trim Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Reclose Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phasing Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phasing Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("CT Ratio", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phasing Mode", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Cycle Limit", false));//18
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Pump Time", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Motor Cycles", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Motor Timeout", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Protect time", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Overcurrent", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Current Imbalance", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Low Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Voltage Imbalance", false));//27
#endif

#if ENMAX
            this.analogOutputs.Add(new AnalogPointDefinition("Sensitive Time Delay", false));//0
            this.analogOutputs.Add(new AnalogPointDefinition("Sensitive Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Insensitive Trip Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Instantaneous Trip Current", false));//4
            this.analogOutputs.Add(new AnalogPointDefinition("Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Extended Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Watt-Var Current", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Watt-Var Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Trip Style", false));//9
            this.analogOutputs.Add(new AnalogPointDefinition("Trim Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Time Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Reclose Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Close Tilt Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phasing Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phasing Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("CT Ratio", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phasing Mode", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Cycle Limit", false));//18
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Pump Time", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Motor Cycles", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Motor Timeout", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Pump Mode Protect time", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Delay", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Overcurrent", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Current Imbalance", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Low Voltage", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Safe Service Voltage Imbalance", false));//27
            this.analogOutputs.Add(new AnalogPointDefinition("Reclose Angle", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Close", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Phase Compensation", false));
            this.analogOutputs.Add(new AnalogPointDefinition("GE Zero Adj", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Reclose Volts Settings", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Reclose Angle Settingse", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Phasing Voltage Offset Setting", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Relaxed Close Active Timer", false));
            this.analogOutputs.Add(new AnalogPointDefinition("Unsolicited Fire Time", false));
            this.analogOutputs.Add(new AnalogPointDefinition("NWP Cycle Counter", false)); //37
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
        private void addBinaryBoxIn(ucDNPMemphisBinary box, TabPage tB)
        {
#if DEBUG
            int y = tB.Controls.Count % 25 * box.Height + 5; //22 is the height of the control - %20 because 20 per row
             int x = box.Width * (tB.Controls.Count / 25) + 1;
#elif TORONTO_HYDRO
            //13 Rows of Binary Input Points per column
            int y = tB.Controls.Count % 13 * box.Height + 5;
            int x = box.Width * (tB.Controls.Count / 13) + 1;
#elif CONED
            //23 Rows of Binary Input Points per column
            int y = tB.Controls.Count % 23 * box.Height + 5;
            int x = box.Width * (tB.Controls.Count / 23) + 1;
#elif PSEG
            //19 Rows of Binary Input Points per column
            int y = tB.Controls.Count % 19 * box.Height + 5;
            int x = box.Width * (tB.Controls.Count / 19) + 1;
#elif ENMAX
            //15 Rows of Binary Input Points per column
            int y = tB.Controls.Count % 15 * box.Height + 5;
            int x = box.Width * (tB.Controls.Count / 15) + 1;
#endif
            box.Location = new Point(x, y);
            tB.Controls.Add(box);
        }

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

        //Adds an analog box to the selected page
        private void addAnalogBoxIn(ucDNPDIGITALGRIDAnalogIn box, TabPage tB)
        {
#if DEBUG
            int y = tB.Controls.Count % 25 * box.Height + 5; //22 is the height of the control - %20 because 20 per row
            int x = box.Width * (tB.Controls.Count / 25) + 1;
#elif TORONTO_HYDRO
            //22 Rows of Analog Inputs Points per column
            int y = tB.Controls.Count % 22 * box.Height + 5; 
            int x = box.Width * (tB.Controls.Count / 22) + 1;
#elif CONED
            //20 Rows of Analog Inputs Points per column
            int y = tB.Controls.Count % 20 * box.Height + 5;
            int x = box.Width * (tB.Controls.Count / 20) + 1;
#elif PSEG
            //15 Rows of Analog Inputs Points per column
            int y = tB.Controls.Count % 15 * box.Height + 5;
            int x = box.Width * (tB.Controls.Count / 15) + 1;
#elif ENMAX
            //20 Rows of Analog Inputs Points per column ( 40 points per tab )
            int y = tB.Controls.Count % 20 * box.Height + 5;
            int x = box.Width * (tB.Controls.Count / 20) + 1;
#endif
            box.Location = new Point(x, y);
            tB.Controls.Add(box);
        }

        private void addAnalogBoxOut(ucDNPMemphisAnalog box, TabPage tB)
        {

#if DEBUG
            //13 Rows of Analog Output Points per column
            int y = tB.Controls.Count % 13 * 20 + 5;
            int x = 347 * (tB.Controls.Count / 13) + 1;

#elif ENMAX
            //20 Rows of Analog Outputs Points per column ( 40 points per tab )
            int y = tB.Controls.Count % 20 * box.Height + 5;
            int x = box.Width * (tB.Controls.Count / 20) + 1;
#else
            int y = tB.Controls.Count % 25 * 20 + 5; //22 is the height of the control - %20 because 20 per row
            int x = 347 * (tB.Controls.Count / 25) + 1;
#endif
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
#if ONCOR
// starting point for setBinaryOutputs considering 48 bytes of BinaryInputs ( per ver10 Oncor firmware )
           savedIndex = 192; 
#elif CONED
            // starting point for setBinaryOutputs considering 45 BinaryInputs points ( per ver10 ConEd firmware - including the 10 newly added ones )
            // 4 bytes per binary input point : 44 * 4 = 176
            savedIndex = 176 + 4;// 224;
#elif (TORONTO_HYDRO || DEBUG)
            // starting point for setBinaryOutputs considering 24 BinaryInputs 
             savedIndex = 96; //(24 * 4) 
            //savedIndex = 232;
#elif (ENMAX)
            // starting point for setBinaryOutputs considering 58 BinaryInputs 
            savedIndex = 228; // 58 Inputs * 4 ( 4 Bytes per binary input )
#endif
            savedIndex = this.setBinaryOutputs(this.dNPData, savedIndex);
#if ONCOR
// starting point for setAnalogInputs considering 48 bytes of BinaryInputs & 26 bytes of BinaryOutputs( per ver10 Oncor firmware )
// 48 * 4(binary input incremented by 4 for each reading)  + 26 * 4(binary output incremented by 4 for each reading) = 296
            savedIndex = 296; 
#elif CONED
            // starting point for setAnalogInputs considering 45 points of BinaryInputs & 29 points of BinaryOutputs( per ver10 ConEd firmware )
            // 44 * 4(binary input incremented by 4 for each reading) + 4 + 28 * 4(binary output incremented by 4 for each reading) = 292
            savedIndex = 344;// 292 + 4; // starting point for setAnalogInputs considering 45 bytes of BinaryInputs & 29 bytes of BinaryOutputs( per ver10 ConEd firmware )
#elif (TORONTO_HYDRO || DEBUG)
            // It is seen that in the incoming dNPData array ( from master uP ), the bytes corresponding to serial number ( 4 & 12 for SN3076 ) are placed at index 8886 and 887.
            // TorontoHydro DNP Map has Serial Number at its DNP Analog Input 29,
            // So, to get to the starting point of analog inputs : 886 - (29*6) ( since each analog input = 6 bytes ) = 712
            savedIndex = 712;
#elif ENMAX
            // It is seen that in the dNPData array, the bytes corresponding to serial number ( 4 & 12 for SN3076 ) are placed at index 712 and 713.
            // Accordingly, since for EnMax has Serial Number at its DNP Analog Input 1 itself,
            // we take 712 as the starting point to read Analog Inputs
            savedIndex = 712;
#endif
            savedIndex = this.setAnalogInputs(this.dNPData, savedIndex);
#if ONCOR
// starting point for setAnalogOutputs considering 44 bytes of BinaryInputs & 28 bytes of BinaryOutputs & 111 bytes of AnalogInputs( per ver10 ConEd firmware )
// 48*4(binary input incremented by 4 for each reading)   + 26*4(binary output incremented by 4 for each reading) + 105*6(analog input incremented by 6 for each reading)= 926         
            savedIndex = 926; 
#elif CONED
            // starting point for setAnalogInputs considering 45 points of BinaryInputs & 29 points of BinaryOutputs( per ver10 ConEd firmware ) & 112 points of Analog Inputs
            // 44 * 4(binary input incremented by 4 for each reading)  + 4 + 28 * 4(binary output incremented by 4 for each reading) + 4 + 111*6(analog input incremented by 6 for each reading)= 962
            savedIndex = 1025;// 962 + 6;
#elif (TORONTO_HYDRO || DEBUG)
            // savedIndex = 396;
            //128 + ( 44 * 6) = 392 + 4 = 396
#endif
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
                    { 
                        uDMB.PointEnabled = false;
                        //MessageBox.Show("Incorrect Binary Input data coming from Master uP");
                    }


                    index += 4;
                }
            }
#if ENMAX
            foreach (Control C in this.tabPageBinaryInputs2.Controls)
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
#endif

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


                    //uDDGA.PointValue = -100;// this.convertDataBytesToAnalog(bytePacket, index);
                    uDDGA.PointValue = this.convertDataBytesToAnalogIn(bytePacket, index);
                    index += 6;
                }
            }
#if !TORONTO_HYDRO
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

                    /*if (index == 758)
                    {
                       uDDGA.PointValue = this.convertDataBytesToAnalogIn(bytePacket, index); 
                    }
                    else
                        uDDGA.PointValue = -150;*/
                    uDDGA.PointValue = this.convertDataBytesToAnalogIn(bytePacket, index);
                    index += 6;
                }
            }
            foreach (Control C in this.tabPageAnalogInputs3.Controls)
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
                    //uDDGA.PointValue = -100;// this.convertDataBytesToAnalog(bytePacket, index);
                    uDDGA.PointValue = this.convertDataBytesToAnalogIn(bytePacket, index);
                    index += 6;
                }
            }
#endif
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

        //private uint convertDataBytesToAnalog(byte[] bytePacket, int i)
        private int convertDataBytesToAnalogIn(byte[] bytePacket, int i)
        {
            Int16 temp;
            temp = bytePacket[i + 1];
            temp <<= 8;
            temp += bytePacket[i];

            //int twosComplement = ~temp + 1;

           // temp = temp--;
           // int twosComplement = ~temp;

            return temp;
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
                if (this.tabPageAnalogInputs3.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs3.Controls.Remove(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs3.Controls.Remove(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs3.Controls.Remove(this.buttonDisableAllAnalogEvents);
                }
                if (!this.tabPageAnalogInputs1.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs1.Controls.Add(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs1.Controls.Add(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs1.Controls.Add(this.buttonDisableAllAnalogEvents);
                }
                this.buttonDisableAllAnalogEvents.Location = new System.Drawing.Point(170, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonDisableAllAnalogEvents.Width * 3 + 23, this.tabPageAnalogInputs1.Height - this.buttonEnableAllAnalogEvents.Height - 2);
                this.buttonDisableAllAnalogEvents.Visible = true;
                this.buttonEnableAllAnalogEvents.Location = new System.Drawing.Point(470, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonEnableAllAnalogEvents.Width - this.buttonSendAnalogEnables.Width - 4, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonEnableAllAnalogEvents.Visible = true;
                this.buttonSendAnalogEnables.Location = new System.Drawing.Point(770, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonSendAnalogEnables.Width - 2, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonSendAnalogEnables.Visible = true;
            }
            else if (this.tabControlMemphisDNP.SelectedTab == this.tabPageAnalogInputs2)
            {
                if (this.tabPageAnalogInputs1.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs1.Controls.Remove(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs1.Controls.Remove(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs1.Controls.Remove(this.buttonDisableAllAnalogEvents);
                }
                if (this.tabPageAnalogInputs3.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs3.Controls.Remove(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs3.Controls.Remove(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs3.Controls.Remove(this.buttonDisableAllAnalogEvents);
                }
                if (!this.tabPageAnalogInputs2.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs2.Controls.Add(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs2.Controls.Add(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs2.Controls.Add(this.buttonDisableAllAnalogEvents);
                }
                /*this.buttonDisableAllAnalogEvents.Location = new Point(this.tabPageAnalogInputs2.Width - this.buttonDisableAllAnalogEvents.Width * 3 + 23, this.tabPageAnalogInputs2.Height - this.buttonEnableAllAnalogEvents.Height - 2);
                this.buttonDisableAllAnalogEvents.Visible = true;
                this.buttonEnableAllAnalogEvents.Location = new Point(this.tabPageAnalogInputs2.Width - this.buttonEnableAllAnalogEvents.Width - this.buttonSendAnalogEnables.Width - 4, this.tabPageAnalogInputs2.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonEnableAllAnalogEvents.Visible = true;
                this.buttonSendAnalogEnables.Location = new Point(this.tabPageAnalogInputs2.Width - this.buttonSendAnalogEnables.Width - 2, this.tabPageAnalogInputs2.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonSendAnalogEnables.Visible = true;
                */
                this.buttonDisableAllAnalogEvents.Location = new System.Drawing.Point(170, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonDisableAllAnalogEvents.Width * 3 + 23, this.tabPageAnalogInputs1.Height - this.buttonEnableAllAnalogEvents.Height - 2);
                this.buttonDisableAllAnalogEvents.Visible = true;
                this.buttonEnableAllAnalogEvents.Location = new System.Drawing.Point(470, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonEnableAllAnalogEvents.Width - this.buttonSendAnalogEnables.Width - 4, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonEnableAllAnalogEvents.Visible = true;
                this.buttonSendAnalogEnables.Location = new System.Drawing.Point(770, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonSendAnalogEnables.Width - 2, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonSendAnalogEnables.Visible = true;
            }
            else if (this.tabControlMemphisDNP.SelectedTab == this.tabPageAnalogInputs3)
            {
                if (this.tabPageAnalogInputs1.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs1.Controls.Remove(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs1.Controls.Remove(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs1.Controls.Remove(this.buttonDisableAllAnalogEvents);
                }
                if (this.tabPageAnalogInputs2.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs2.Controls.Remove(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs2.Controls.Remove(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs2.Controls.Remove(this.buttonDisableAllAnalogEvents);
                }
                if (!this.tabPageAnalogInputs3.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    this.tabPageAnalogInputs3.Controls.Add(this.buttonSendAnalogEnables);
                    this.tabPageAnalogInputs3.Controls.Add(this.buttonEnableAllAnalogEvents);
                    this.tabPageAnalogInputs3.Controls.Add(this.buttonDisableAllAnalogEvents);
                }
                /*this.buttonDisableAllAnalogEvents.Location = new Point(this.tabPageAnalogInputs3.Width - this.buttonDisableAllAnalogEvents.Width * 3 + 23, this.tabPageAnalogInputs3.Height - this.buttonEnableAllAnalogEvents.Height - 2);
                this.buttonDisableAllAnalogEvents.Visible = true;
                this.buttonEnableAllAnalogEvents.Location = new Point(this.tabPageAnalogInputs3.Width - this.buttonEnableAllAnalogEvents.Width - this.buttonSendAnalogEnables.Width - 4, this.tabPageAnalogInputs3.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonEnableAllAnalogEvents.Visible = true;
                this.buttonSendAnalogEnables.Location = new Point(this.tabPageAnalogInputs3.Width - this.buttonSendAnalogEnables.Width - 2, this.tabPageAnalogInputs3.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonSendAnalogEnables.Visible = true;
                */
                this.buttonDisableAllAnalogEvents.Location = new System.Drawing.Point(170, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonDisableAllAnalogEvents.Width * 3 + 23, this.tabPageAnalogInputs1.Height - this.buttonEnableAllAnalogEvents.Height - 2);
                this.buttonDisableAllAnalogEvents.Visible = true;
                this.buttonEnableAllAnalogEvents.Location = new System.Drawing.Point(470, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonEnableAllAnalogEvents.Width - this.buttonSendAnalogEnables.Width - 4, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonEnableAllAnalogEvents.Visible = true;
                this.buttonSendAnalogEnables.Location = new System.Drawing.Point(770, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonSendAnalogEnables.Width - 2, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonSendAnalogEnables.Visible = true;
            }
            else if (this.tabControlMemphisDNP.SelectedTab == this.tabPageBinaryInputs)
            {
                if (!this.tabPageBinaryInputs.Controls.Contains(this.buttonSendBinaryEventEnables))
                {
                    this.tabPageBinaryInputs.Controls.Add(this.buttonSendBinaryEventEnables);
                    this.tabPageBinaryInputs.Controls.Add(this.buttonEnableAllBinaryEvents);
                    this.tabPageBinaryInputs.Controls.Add(this.buttonDisableAllBinaryEvents);
                }
#if TORONTO_HYDRO//!CONED
                //this.buttonDisableAllBinaryEvents.Location = new Point(this.tabPageBinaryInputs.Width - this.buttonEnableAllBinaryEvents.Width * 3 - 23, this.tabPageBinaryInputs.Height - this.buttonEnableAllBinaryEvents.Height - 2);
                this.buttonDisableAllBinaryEvents.Location = new System.Drawing.Point(170, 720);  
                this.buttonDisableAllBinaryEvents.Visible = true;
                //this.buttonEnableAllBinaryEvents.Location = new Point(this.tabPageBinaryInputs.Width - this.buttonSendBinaryEventEnables.Width - this.buttonEnableAllBinaryEvents.Width - 4, this.tabPageBinaryInputs.Height - this.buttonSendBinaryEventEnables.Height - 2);
                this.buttonEnableAllBinaryEvents.Location = new System.Drawing.Point(470, 720);  
                this.buttonEnableAllBinaryEvents.Visible = true;
                //this.buttonSendBinaryEventEnables.Location = new Point(this.tabPageBinaryInputs.Width - this.buttonSendBinaryEventEnables.Width - 2, this.tabPageBinaryInputs.Height - this.buttonSendBinaryEventEnables.Height - 2);
                this.buttonSendBinaryEventEnables.Location = new System.Drawing.Point(770, 720); 
                this.buttonSendBinaryEventEnables.Visible = true;
#elif CONED || PSEG
                this.buttonDisableAllBinaryEvents.Location = new System.Drawing.Point(670, 723);
                this.buttonDisableAllBinaryEvents.Size = new System.Drawing.Size(192, 32);
                this.buttonDisableAllBinaryEvents.Visible = true;

                this.buttonEnableAllBinaryEvents.Location = new System.Drawing.Point(870, 723);
                this.buttonEnableAllBinaryEvents.Size = new System.Drawing.Size(190, 32);
                this.buttonEnableAllBinaryEvents.Visible = true;

                this.buttonSendBinaryEventEnables.Location = new System.Drawing.Point(1285, 722);
                this.buttonSendBinaryEventEnables.Size = new System.Drawing.Size(220, 32);
                this.buttonSendBinaryEventEnables.Visible = true;
#elif ENMAX
                if (this.tabPageBinaryInputs2.Controls.Contains(this.buttonSendBinaryEventEnables))
                {
                    this.tabPageBinaryInputs.Controls.Remove(this.buttonSendBinaryEventEnables);
                    this.tabPageBinaryInputs.Controls.Remove(this.buttonEnableAllBinaryEvents);
                    this.tabPageBinaryInputs.Controls.Remove(this.buttonDisableAllBinaryEvents);
                }
                if (!this.tabPageBinaryInputs.Controls.Contains(this.buttonSendBinaryEventEnables))
                {
                    this.tabPageBinaryInputs.Controls.Add(this.buttonSendBinaryEventEnables);
                    this.tabPageBinaryInputs.Controls.Add(this.buttonEnableAllBinaryEvents);
                    this.tabPageBinaryInputs.Controls.Add(this.buttonDisableAllBinaryEvents);
                }
                this.buttonDisableAllBinaryEvents.Location = new System.Drawing.Point(170, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonDisableAllAnalogEvents.Width * 3 + 23, this.tabPageAnalogInputs1.Height - this.buttonEnableAllAnalogEvents.Height - 2);
                this.buttonDisableAllBinaryEvents.Visible = true;
                this.buttonEnableAllBinaryEvents.Location = new System.Drawing.Point(470, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonEnableAllAnalogEvents.Width - this.buttonSendAnalogEnables.Width - 4, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonEnableAllBinaryEvents.Visible = true;
                this.buttonSendBinaryEventEnables.Location = new System.Drawing.Point(770, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonSendAnalogEnables.Width - 2, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonSendBinaryEventEnables.Visible = true;
#endif

#if (DIGITALGRID && (ONCOR || CONED || TORONTO_HYDRO))
                /* this.buttonDisableAllBinaryEvents.Enabled = false;
                 this.buttonDisableAllBinaryEvents.Visible = false;
                 this.buttonEnableAllBinaryEvents.Enabled = false;
                 this.buttonEnableAllBinaryEvents.Visible = false;
                 this.buttonSendBinaryEventEnables.Enabled = false;
                 this.buttonSendBinaryEventEnables.Visible = false;
                */
#endif
            }
            else if (this.tabControlMemphisDNP.SelectedTab == this.tabPageBinaryInputs2)
            {
#if ENMAX
                if (this.tabPageBinaryInputs.Controls.Contains(this.buttonSendBinaryEventEnables))
                {
                    this.tabPageBinaryInputs.Controls.Remove(this.buttonSendBinaryEventEnables);
                    this.tabPageBinaryInputs.Controls.Remove(this.buttonEnableAllBinaryEvents);
                    this.tabPageBinaryInputs.Controls.Remove(this.buttonDisableAllBinaryEvents);
                }
                if (!this.tabPageBinaryInputs2.Controls.Contains(this.buttonSendBinaryEventEnables))
                {
                    this.tabPageBinaryInputs2.Controls.Add(this.buttonSendBinaryEventEnables);
                    this.tabPageBinaryInputs2.Controls.Add(this.buttonEnableAllBinaryEvents);
                    this.tabPageBinaryInputs2.Controls.Add(this.buttonDisableAllBinaryEvents);
                }
                this.buttonDisableAllBinaryEvents.Location = new System.Drawing.Point(170, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonDisableAllAnalogEvents.Width * 3 + 23, this.tabPageAnalogInputs1.Height - this.buttonEnableAllAnalogEvents.Height - 2);
                this.buttonDisableAllBinaryEvents.Visible = true;
                this.buttonEnableAllBinaryEvents.Location = new System.Drawing.Point(470, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonEnableAllAnalogEvents.Width - this.buttonSendAnalogEnables.Width - 4, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonEnableAllBinaryEvents.Visible = true;
                this.buttonSendBinaryEventEnables.Location = new System.Drawing.Point(770, 720); //new Point(this.tabPageAnalogInputs1.Width - this.buttonSendAnalogEnables.Width - 2, this.tabPageAnalogInputs1.Height - this.buttonSendAnalogEnables.Height - 2);
                this.buttonSendBinaryEventEnables.Visible = true;
#endif
            }

        }

        private void buttonSendBinaryEventEnables_Click(object sender, EventArgs e)
        {
            uint i = 0;
            uint packetByteNumber = 2; //starts at 2 after OpCode and SubCode
            byte tempByte = 0;

            SendEventArgs sEA = new SendEventArgs(_packetLength);

            sEA.SendPacket[0] = (byte)RelayModeFunctions._DNPControlOpCode; // "D"
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
            
                if (i % 8 != 0)
                {
                    sEA.SendPacket[packetByteNumber] = tempByte;
                }
            }
#if ENMAX
            foreach (Control C in this.tabPageBinaryInputs2.Controls)
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

                if (i % 8 != 0)
                {
                    sEA.SendPacket[packetByteNumber] = tempByte;
                }
            }
#endif
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

            sEA.SendPacket[0] = (byte)RelayModeFunctions._DNPControlOpCode; // "D"
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
                        //MessageBox.Show(tempByte + " TEMPBYTE : ");// Only for testing - to be removed
                        if (uDDGA.EventEnabled)
                            tempByte += (byte)0x80;

                        //MessageBox.Show(tempByte + " tempByte at : " + packetByteNumber + " packetByteNumber and " + i + " i");// Only for testing - to be removed
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
                            //MessageBox.Show(tempByte + " : tempByte with value : " + value + " i j and i%8 : " + i + j + i % 8);// Only for testing - to be removed
                        }
                    }

                    i++;
                }
#if TORONTO_HYDRO
                if (i == 43)
                {  
                       // MessageBox.Show(tempByte + " TEMPBYTE @ 43 : ");// Only for testing - to be removed
                    if (uDDGA.EventEnabled)
                        tempByte += (byte)0x80;

                    //MessageBox.Show(tempByte + " tempByte at : " + packetByteNumber + " packetByteNumber and " + i + " i");// Only for testing - to be removed
                    sEA.SendPacket[packetByteNumber] = tempByte;
                    
                    if (uDDGA.EventEnabled)
                    {
                        byte j = 0;
                        byte value = 1;
                        for (j = 0; j < i % 8; ++j)
                        {
                            value <<= 1;
                        }

                        tempByte += value;
                     //   MessageBox.Show(tempByte + " : tempByte with value : " + value + " i j and i%8 : " + i + j + i % 8);// Only for testing - to be removed
                    }

                   // MessageBox.Show(tempByte + " tempByte at : " + packetByteNumber + " packetByteNumber and " + i + " i");// Only for testing - to be removed
                    sEA.SendPacket[packetByteNumber] = tempByte;

                    packetByteNumber++; //next byte goes into next byte of packet
                }
#endif
            }
#if CONED || ENMAX
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
            foreach (Control C in this.tabPageAnalogInputs3.Controls)
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
#endif
            sEA.SendPacket[sEA.SendPacket.Length - 1] = 0x0D;

           // for (int tempX = 0; tempX <= 97; tempX++)
            //    MessageBox.Show(sEA.SendPacket[tempX].ToString() + " : sEA.SendPacket[] at " + tempX);// Only for testing - to be removed

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
#if ENMAX
            foreach (Control C in this.tabPageBinaryInputs2.Controls)
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
#endif
        }

        private void buttonEnableAllAnalogEvents_Click(object sender, EventArgs e)
        {
            ucDNPDIGITALGRIDAnalogIn uDDGA = new ucDNPDIGITALGRIDAnalogIn();
            bool failed = false;

            if (PointChanged != null)
            {
                this.PointChanged(this, new DNPPointEventArgs(true));
            }
            foreach (Control C in this.tabPageAnalogInputs3.Controls)
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
#if ENMAX
            foreach (Control C in this.tabPageBinaryInputs2.Controls)
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
#endif
        }

        private void buttonDisableAllAnalogEvents_Click(object sender, EventArgs e)
        {
            ucDNPDIGITALGRIDAnalogIn uDDGA = new ucDNPDIGITALGRIDAnalogIn();
            bool failed = false;

            if (PointChanged != null)
            {
                this.PointChanged(this, new DNPPointEventArgs(false));
            }
            foreach (Control C in this.tabPageAnalogInputs3.Controls)
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
