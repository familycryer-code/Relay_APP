using SharedResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq;

namespace RelayControlLibrary
{
    public partial class ucDNPDIGITALGRIDData : UserControl
    {
        public ucDNPDIGITALGRIDData()
        {
            InitializeComponent();
            this.VisibleChanged += ucDNPDIGITALGRIDData_VisibleChanged;
            SetSize();
        }

        public ucDNPDIGITALGRIDData(Customers customer)
        {
            InitializeComponent();
            _customer = customer;
            this.Customer = customer; // keep existing behavior if setter does other setup
            ApplyDnpTabVisibilityPolicy();
        }

        public delegate void DNPPointChangedHandlder(object o, DNPPointEventArgs eA);
        public event DNPPointChangedHandlder PointChanged;
        private Customers _customer;

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
                    if (this.customer == Customers.ENMAX)
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
        // Toronto_Hydro DNP Map has Serial Number at its DNP Analog Input 29,
        // So, to get to the starting point of analog inputs : 886 - (29*6) ( since each analog input = 6 bytes ) = 712
        // Toronto_Hydro has 44 Analog Inputs and 28 Analog Outputs
        // so ( 44 * 6 ) points ( since each analog input = 6 bytes ) + ( 28 * 4 ) ( since each analog output = 4 bytes ) 
        // = 712 + 264 + 112 = 1088
        private byte[] dNPData = new byte[1088]; 
#else
        private byte[] dNPData = new byte[1196];//[1008];  //252 packet size * 4
        // index goes to 1120 at starting point of analog ouput reads. so, 19*4 bytes more after that
#endif
        private UInt32 relayMasterRevision = 260214;//140506;
        private Customers customer = Customers.TORONTO_HYDRO;


        private static int _packetLength = 98;

        #region Initialization

        private void initializeComponents()
        {
            this.initializeBinaryInputs();
            this.initializeBinaryOutputs();
            this.initializeAnalogInputs();
            this.initializeAnalogOutputs();

            // Never allow silent blank screen
            bool empty =
                this.tabPageBinaryInputs.Controls.Count == 0 &&
                this.tabPageBinaryInputs2.Controls.Count == 0 &&
                this.tabPageAnalogInputs1.Controls.Count == 0 &&
                this.tabPageAnalogInputs2.Controls.Count == 0 &&
                this.tabPageAnalogInputs3.Controls.Count == 0 &&
                this.tabPageAnalogOutputs.Controls.Count == 0;

            if (empty)
            {
                this.tabPageBinaryInputs.Controls.Add(new Label
                {
                    AutoSize = true,
                    Location = new Point(20, 20),
                    Text = "No DNP point map initialized for current customer."
                });
            }

            this.tabControlMemphisDNP_SelectedIndexChanged_1(this, EventArgs.Empty);

#if DEBUG
    System.Diagnostics.Debug.WriteLine(
        $"[DNPData] BI1={this.tabPageBinaryInputs.Controls.Count}, " +
        $"BI2={this.tabPageBinaryInputs2.Controls.Count}, " +
        $"AI1={this.tabPageAnalogInputs1.Controls.Count}, " +
        $"AI2={this.tabPageAnalogInputs2.Controls.Count}, " +
        $"AI3={this.tabPageAnalogInputs3.Controls.Count}, " +
        $"AO={this.tabPageAnalogOutputs.Controls.Count}, " +
        $"Tabs={this.tabControlMemphisDNP.TabPages.Count}, " +
        $"Customer={this.customer}");
#endif
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
            this.binaryInputs.Clear();

            this.tabPageBinaryInputs.Controls.Clear();
            this.tabPageBinaryInputs2.Controls.Clear();

            // -----------------------------
            // Runtime customer point map
            // -----------------------------
            if (this.customer == Customers.CONED)
            {
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
            }
            else if (this.customer == Customers.SCE)
            {
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
                this.binaryInputs.Add("Digital In 1");
                this.binaryInputs.Add("Digital In 2");
                this.binaryInputs.Add("Digital In 3");
                this.binaryInputs.Add("Digital In 4");
                this.binaryInputs.Add("Defaults Loaded");
                this.binaryInputs.Add("Phase Angle Wrong");
                this.binaryInputs.Add("Differential Volts Too Low to close");
                this.binaryInputs.Add("Blocked from Closing");
                this.binaryInputs.Add("Digital Out 1 Status");
                this.binaryInputs.Add("Digital Out 2 Status");
            }
            else if (this.customer == Customers.PSEG)
            {
                // keep your existing PSEG list exactly as-is
            }
            else if (this.customer == Customers.ENMAX)
            {
                // keep your existing ENMAX list exactly as-is
            }
            else if (this.customer == Customers.TORONTO_HYDRO)
            {
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
            }

            uint i = 0;
            foreach (string s in this.binaryInputs)
            {
                var workingBox = new ucDNPMemphisBinary
                {
                    PointNumber = i,
                    PointName = s,
                    EventEnableVisible = (this.customer == Customers.TORONTO_HYDRO)
                };
                workingBox.PointChanged += dNPPoint_PointChanged;

                // Runtime layout
                if (this.customer == Customers.ENMAX || this.customer == Customers.PSEG)
                {
                    if (i <= 29)
                        this.addBinaryBoxIn(workingBox, this.tabPageBinaryInputs);
                    else if (i <= 57)
                        this.addBinaryBoxIn(workingBox, this.tabPageBinaryInputs2);
                }
                else
                {
                    if (i < 50)
                        this.addBinaryBoxIn(workingBox, this.tabPageBinaryInputs);
                }

                i++;
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
          
            this.binaryOutputs.Clear();
            this.tabPageBinaryOuputs.Controls.Clear();

            // Runtime customer point map (no compile-time #if)
            if (this.customer == Customers.CONED)
            {
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
            }
            else if (this.customer == Customers.SCE)
            {
                this.binaryOutputs.Add("Trip");
                this.binaryOutputs.Add("Block");
                this.binaryOutputs.Add("Unblock(Auto)");
                this.binaryOutputs.Add("Digital Out 1");
                this.binaryOutputs.Add("Digital Out 2");
                this.binaryOutputs.Add("Program Relay");
            }
            else if (this.customer == Customers.PSEG)
            {
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
            }
            else if (this.customer == Customers.ENMAX)
            {
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
                this.binaryOutputs.Add("Unblock Open");//30
            }
            else if (this.customer == Customers.ONCOR || this.customer == Customers.TORONTO_HYDRO)
            {
                this.binaryOutputs.Add("Remote Trip");//0
                this.binaryOutputs.Add("Block Open");
                this.binaryOutputs.Add("Not Available");
                this.binaryOutputs.Add("Not Available");
                this.binaryOutputs.Add("Command Test");
                this.binaryOutputs.Add("Not Available");
                this.binaryOutputs.Add("Not Available");
                this.binaryOutputs.Add("Relax Close");//7
            }

            uint i = 0;
            foreach (string s in this.binaryOutputs)
            {
                ucDNPMemphisBinary workingBox = new ucDNPMemphisBinary();
                workingBox.PointNumber = i;
                workingBox.PointName = s;
                workingBox.EventEnableVisible = false;
                workingBox.PointChanged += dNPPoint_PointChanged;

                this.addBinaryBox(workingBox, this.tabPageBinaryOuputs);
                i++;
            }
        }

        private void initializeAnalogInputs()
        {
            this.analogInputs.Clear();
            this.tabPageAnalogInputs1.Controls.Clear();
            this.tabPageAnalogInputs2.Controls.Clear();
            this.tabPageAnalogInputs3.Controls.Clear();

            // Runtime customer point map
            if (this.customer == Customers.CONED)
            {
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
            }
            else if (this.customer == Customers.SCE)
            {
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
            }
            else if (this.customer == Customers.PSEG)
            {
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
            }
            else if (this.customer == Customers.ENMAX)
            {
                // 122 Analog Input Points for DNP
                this.analogInputs.Add(new AnalogPointDefinition("Serial Number", false));//0
                this.analogInputs.Add(new AnalogPointDefinition("Relay Version Number", false));
                this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage (Vt) - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage (Vt) - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage (Vt) - Phase C", false));
                this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage (Vt) Angle - Phase A", false));//5
                this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage (Vt) Angle - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Transformer Voltage (Vt) Angle - Phase C", false));
                this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) - Phase C", false));//10
                this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) Angle - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) Angle - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Network Voltage (Vn) Angle - Phase C", false));
                this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) - Phase B", false));//15
                this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) - Phase C", false));
                this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) Angle - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) Angle - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Voltage Differential (Vd) Angle - Phase C", false));
                this.analogInputs.Add(new AnalogPointDefinition("Average Differential Voltage", false));   //20
                this.analogInputs.Add(new AnalogPointDefinition("Average Differential Voltage Angle", false));
                this.analogInputs.Add(new AnalogPointDefinition("Real Voltage Differential (Vd) - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Real Voltage Differential (Vd) - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Real Voltage Differential (Vd) - Phase C", false));
                this.analogInputs.Add(new AnalogPointDefinition("Average Real Differential Voltage", false));
                this.analogInputs.Add(new AnalogPointDefinition("Current (I) - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Current (I) - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Current (I) - Phase C", false));
                this.analogInputs.Add(new AnalogPointDefinition("Current (I) Angle - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Current (I) Angle - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Current (I) Angle - Phase C", false));//31
                this.analogInputs.Add(new AnalogPointDefinition("Effective Current", false));
                this.analogInputs.Add(new AnalogPointDefinition("Effective Current Angle", false));
                this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Current", false));
                this.analogInputs.Add(new AnalogPointDefinition("Positvie Sequence Current Angle", false));
                this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Current", false));
                this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Current Angle", false));
                this.analogInputs.Add(new AnalogPointDefinition("Apparent Power - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Apparent Power - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Apparent Power - Phase C", false));//40
                this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Angle - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Angle - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Angle - Phase C", false));
                this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Average", false));
                this.analogInputs.Add(new AnalogPointDefinition("Apparent Power Average Angle", false));
                this.analogInputs.Add(new AnalogPointDefinition("Real Power - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Real Power - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Real Power - Phase C", false));//48
                this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Differential Voltage", false)); //49
                this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Differential Voltage Angle", false));
                this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Differential Voltage ", false));
                this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Differential Voltage Angle", false));
                this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Network Voltage", false)); //53
                this.analogInputs.Add(new AnalogPointDefinition("Positive Sequence Network Voltage Angle", false));
                this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Network Voltage", false));
                this.analogInputs.Add(new AnalogPointDefinition("Negative Sequence Network Voltage Angle", false));
                this.analogInputs.Add(new AnalogPointDefinition("Network Voltage Total Harmonic Distortion (THD) - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Network Voltage Total Harmonic Distortion (THD) - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Network Voltage Total Harmonic Distortion (THD) - Phase C", false));
                this.analogInputs.Add(new AnalogPointDefinition("Current Total Harmonic Distortion (THD) - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Current Total Harmonic Distortion (THD) - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Current Total Harmonic Distortion (THD) - Phase C", false));//62
                this.analogInputs.Add(new AnalogPointDefinition("NWP Internal/Relay Temperature", false));
                this.analogInputs.Add(new AnalogPointDefinition("NWP Cycle Count", false));
                this.analogInputs.Add(new AnalogPointDefinition("Aux Input 1", false));//65
                this.analogInputs.Add(new AnalogPointDefinition("Aux Input 2", false));
                this.analogInputs.Add(new AnalogPointDefinition("Aux Input 3", false));
                this.analogInputs.Add(new AnalogPointDefinition("Aux Input 4", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Analog H", false));
                this.analogInputs.Add(new AnalogPointDefinition("Analog 1", false)); //75
                this.analogInputs.Add(new AnalogPointDefinition("Analog 2", false));
                this.analogInputs.Add(new AnalogPointDefinition("Load (L) % - Phase A", false));
                this.analogInputs.Add(new AnalogPointDefinition("Load (L) % - Phase B", false));
                this.analogInputs.Add(new AnalogPointDefinition("Load (L) % - Phase C", false));//79
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false)); // 80
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false)); //90
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false)); //100
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false)); // 110
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false));
                this.analogInputs.Add(new AnalogPointDefinition("Not Applicable", false)); //120
                this.analogInputs.Add(new AnalogPointDefinition("Number of RNCs Reporting", false)); // 121    
            }
            else if (this.customer == Customers.ONCOR || this.customer == Customers.TORONTO_HYDRO)
            {
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
                this.analogInputs.Add(new AnalogPointDefinition("NWP Cycle Count ", false));
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
            }

            uint i = 0;
            foreach (AnalogPointDefinition aPD in this.analogInputs)
            {
                ucDNPDIGITALGRIDAnalogIn workingBox = new ucDNPDIGITALGRIDAnalogIn();
                workingBox.PointNumber = i;
                workingBox.PointName = aPD.Name;
                workingBox.Signed = aPD.Signed;
                workingBox.PointChanged += dNPPoint_PointChanged;

                // Runtime layout (replaces #if/#elif layout)
                if (this.customer == Customers.TORONTO_HYDRO)
                {
                    if (i < 50) this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs1);
                }
                else if (this.customer == Customers.CONED)
                {
                    if (i <= 39) this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs1);
                    else if (i <= 79) this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs2);
                    else if (i <= 111) this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs3);
                }
                else if (this.customer == Customers.PSEG)
                {
                    if (i <= 29) this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs1);
                    else if (i <= 59) this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs2);
                    else if (i <= 79) this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs3);
                }
                else // ENMAX / ONCOR / SCE default
                {
                    if (i <= 41) this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs1);
                    else if (i <= 83) this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs2);
                    else if (i <= 122) this.addAnalogBoxIn(workingBox, this.tabPageAnalogInputs3);
                }

                i++;
            }
        }

        private void initializeAnalogOutputs()
        {
            this.analogOutputs.Clear();
            this.tabPageAnalogOutputs.Controls.Clear();

            // Runtime customer point map (same lists, no compile-time #if)
            if (this.customer == Customers.CONED)
            {
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
            }
            else if (this.customer == Customers.SCE)
            {
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
            }
            else if (this.customer == Customers.PSEG)
            {
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
            }
            else if (this.customer == Customers.ONCOR)
            {
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
            }
            else if (this.customer == Customers.ENMAX)
            {
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
            }

            uint i = 0;
            foreach (AnalogPointDefinition aPD in this.analogOutputs)
            {
                ucDNPDIGITALGRIDAnalogOut workingBox = new ucDNPDIGITALGRIDAnalogOut();
                workingBox.PointNumber = i;
                workingBox.PointName = aPD.Name;
                workingBox.Signed = aPD.Signed;

                this.addAnalogBoxOut(workingBox, this.tabPageAnalogOutputs);
                i++;
            }
        }

        private void SetSize()
        {
            this.tabControlMemphisDNP.Dock = DockStyle.Fill;
            this.tabControlMemphisDNP.Location = new Point(0, 0);
            this.tabControlMemphisDNP.Size = this.ClientSize;
            this.tabControlMemphisDNP.Visible = true;
            this.tabControlMemphisDNP.BringToFront();
        }

        //Adds a binary box to the selected page
        private void addBinaryBoxIn(ucDNPMemphisBinary box, TabPage tB)
        {
            int rowsPerColumn;

            if (this.customer == Customers.TORONTO_HYDRO)
                rowsPerColumn = 13;
            else if (this.customer == Customers.CONED)
                rowsPerColumn = 23;
            else if (this.customer == Customers.PSEG)
                rowsPerColumn = 19;
            else if (this.customer == Customers.ENMAX ||
                     this.customer == Customers.ONCOR ||
                     this.customer == Customers.SCE ||
                     this.customer == Customers.DOMINION ||
                     this.customer == Customers.BGE)
                rowsPerColumn = 15;
            else
                rowsPerColumn = 25; // default

            int y = (tB.Controls.Count % rowsPerColumn) * box.Height + 5;
            int x = box.Width * (tB.Controls.Count / rowsPerColumn) + 1;

            box.Location = new Point(x, y);
            tB.Controls.Add(box);
        }

        private void addBinaryBox(ucDNPMemphisBinary box, TabPage tB)
        {
            int y = tB.Controls.Count % 20 * 22 + 1;
            int x = (tB.Controls.Count >= 20) ? (tB.Width / 2) : 1;

            box.Location = new Point(x, y);
            tB.Controls.Add(box);
        }

        //Adds an analog box to the selected page
        private void addAnalogBoxIn(ucDNPDIGITALGRIDAnalogIn box, TabPage tB)
        {
            int rowsPerColumn;

            if (this.customer == Customers.TORONTO_HYDRO)
                rowsPerColumn = 22;
            else if (this.customer == Customers.CONED)
                rowsPerColumn = 20;
            else if (this.customer == Customers.PSEG)
                rowsPerColumn = 15;
            else if (this.customer == Customers.ENMAX ||
                     this.customer == Customers.ONCOR ||
                     this.customer == Customers.SCE ||
                     this.customer == Customers.DOMINION ||
                     this.customer == Customers.BGE)
                rowsPerColumn = 21;
            else
                rowsPerColumn = 25; // default (e.g., EVERSOURCE)

            int y = (tB.Controls.Count % rowsPerColumn) * box.Height + 5;
            int x = box.Width * (tB.Controls.Count / rowsPerColumn) + 1;

            box.Location = new Point(x, y);
            tB.Controls.Add(box);
        }

        private void addAnalogBoxOut(ucDNPMemphisAnalog box, TabPage tB)
        {
            int rowsPerColumn;
            int rowHeight;
            int columnWidth;

            if (this.customer == Customers.ENMAX)
            {
                // 20 rows/column for ENMAX
                rowsPerColumn = 20;
                rowHeight = box.Height;
                columnWidth = box.Width;
            }
            else
            {
                // legacy default layout
                rowsPerColumn = 25;
                rowHeight = 20;
                columnWidth = 347;
            }

            int y = (tB.Controls.Count % rowsPerColumn) * rowHeight + 5;
            int x = columnWidth * (tB.Controls.Count / rowsPerColumn) + 1;

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
       //     MessageBox.Show("4 sending command D to master"); // Only for testing - to be removed
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
            bool onBinaryInputs = this.tabControlMemphisDNP.SelectedTab == this.tabPageBinaryInputs;

            this.buttonSendBinaryEventEnables.Visible = onBinaryInputs;
            this.buttonEnableAllBinaryEvents.Visible = onBinaryInputs;
            this.buttonDisableAllBinaryEvents.Visible = onBinaryInputs;
        }

        private void ApplyDnpTabVisibilityPolicy()
        {
            bool isENMAX = (_customer == Customers.ENMAX);
            bool isCONED = (_customer == Customers.CONED);

            SetTabVisible(this.tabPageBinaryInputs, true);
            SetTabVisible(this.tabPageBinaryInputs2, isENMAX);
            SetTabVisible(this.tabPageBinaryOuputs, true);

            SetTabVisible(this.tabPageAnalogInputs1, true);
            SetTabVisible(this.tabPageAnalogInputs2, isCONED || isENMAX);
            SetTabVisible(this.tabPageAnalogInputs3, isCONED || isENMAX);

            // Force OFF for this instance
            SetTabVisible(this.tabPageAnalogOutputs, false);

            if (this.tabControlMemphisDNP.TabPages.Count > 0 &&
                this.tabControlMemphisDNP.SelectedIndex < 0)
            {
                this.tabControlMemphisDNP.SelectedIndex = 0;
            }
        }

        private void SetTabVisible(TabPage page, bool visible)
        {
            bool contains = this.tabControlMemphisDNP.TabPages.Contains(page);

            if (visible && !contains)
                this.tabControlMemphisDNP.TabPages.Add(page);
            else if (!visible && contains)
                this.tabControlMemphisDNP.TabPages.Remove(page);
        }

        #endregion

        #region Incoming Data Handling

        public int setBinaryInputs(byte[] bytePacket)
        {
            //int i = this.tabPageBinaryInputs.Controls.Count;
            int j = 0;
            foreach (Control C in this.tabPageBinaryInputs.Controls)
            {
                ucDNPMemphisBinary uDMB = new ucDNPMemphisBinary();
                
                uDMB = (ucDNPMemphisBinary)C;
                uDMB.CheckValue = this.convertDataByteToBool(bytePacket[j]);
                j += 1;
                
            }
#if ENMAX
            j = 30;
            foreach (Control C in this.tabPageBinaryInputs2.Controls)
            {
                ucDNPMemphisBinary uDMB = new ucDNPMemphisBinary();

                uDMB = (ucDNPMemphisBinary)C;
                uDMB.CheckValue = this.convertDataByteToBool(bytePacket[j]);
                j += 1;
            }
#endif
            return 0;
        }

        public int setBinaryOutputs(byte[] bytePacket)
        {
            int j = 0;
            foreach (Control C in this.tabPageBinaryOuputs.Controls)
            {
                ucDNPMemphisBinary uDMB = new ucDNPMemphisBinary();

                uDMB = (ucDNPMemphisBinary)C;
                uDMB.CheckValue = this.convertDataByteToBool(bytePacket[j]);
                j += 1;

            }

            return 0;

        }

        public int setAnalogInputs(byte[] bytePacket, int aiCommand)
        {
            int j = 0;

            int maxTab2Command13 = 21; 
            int current = 0;
            int maxTab3Command14 = 37;
            int curr = 0;

            if (aiCommand == 13) // upto analog input number 41
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
                        uDDGA.PointValue = this.convertDataBytesToAnalogIn(bytePacket, j);
                        j += 4;
                    }
                }
            }
            if (aiCommand == 13) // upto analog input number 62
            {
                // AI number 43 is starting point for analog input tab 2. so, to start reading from corresponding point in the 
                // incomming string message considering 4 bytes data per point
                // 4 * 42 = 168
                j = 168; 
                foreach (Control C in this.tabPageAnalogInputs2.Controls)
                {

                    if(current >= maxTab2Command13)
                        break;

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
                        uDDGA.PointValue = this.convertDataBytesToAnalogIn(bytePacket, j);
                        j += 4;
                    }

                    current++;
                }

            }
            if (aiCommand == 14) // upto analog input number 83
            {
                // AI number 63 is starting point for dnp message command 0x14 
                // this is control number 22 on tab 2
                j = 0;
                //foreach (Control C in this.tabPageAnalogInputs2.Controls)
                foreach (Control C in this.tabPageAnalogInputs2.Controls.Cast<Control>().Skip(21))
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
                        uDDGA.PointValue = this.convertDataBytesToAnalogIn(bytePacket, j);
                        j += 4;
                    }
                }

            }
            if (aiCommand == 14) // upto analog input number 121
            {
                // AI number 84 is starting point for analog input tab#3 with dnp message command 0x14 
                // incomming string message considering 4 bytes data per point
                // 4 * 21 = 84
                j = 84;
                foreach (Control C in this.tabPageAnalogInputs3.Controls)
                {

                    if (curr >= maxTab3Command14)
                        break;

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
                        uDDGA.PointValue = this.convertDataBytesToAnalogIn(bytePacket, j);
                        j += 4;
                    }

                    current++;
                }
                
            }
            return 0;

        }

        public int setAnalogOutputs(byte[] bytePacket)
        {
            int j = 0;
            foreach (ucDNPMemphisAnalog uDMA in this.tabPageAnalogOutputs.Controls)
            {
                uDMA.PointValue = this.convertDataBytesToAnalog(bytePacket, j);
                j += 4;

            }

            return 0;

        }       

        //private uint convertDataBytesToAnalog(byte[] bytePacket, int i)
        private int convertDataBytesToAnalogIn(byte[] bytePacket, int i)
        {
            Int16 temp;
            temp = bytePacket[i + 1];
            temp <<= 8;
            temp += bytePacket[i];

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
            TabPage selected = this.tabControlMemphisDNP.SelectedTab;

            // ----- Helper local actions -----
            void RemoveAnalogButtonsFrom(TabPage page)
            {
                if (page.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    page.Controls.Remove(this.buttonSendAnalogEnables);
                    page.Controls.Remove(this.buttonEnableAllAnalogEvents);
                    page.Controls.Remove(this.buttonDisableAllAnalogEvents);
                }
            }

            void EnsureAnalogButtonsOn(TabPage page)
            {
                if (!page.Controls.Contains(this.buttonSendAnalogEnables))
                {
                    page.Controls.Add(this.buttonSendAnalogEnables);
                    page.Controls.Add(this.buttonEnableAllAnalogEvents);
                    page.Controls.Add(this.buttonDisableAllAnalogEvents);
                }
            }

            void SetAnalogButtonLocations()
            {
                this.buttonDisableAllAnalogEvents.Location = new System.Drawing.Point(170, 720);
                this.buttonEnableAllAnalogEvents.Location = new System.Drawing.Point(470, 720);
                this.buttonSendAnalogEnables.Location = new System.Drawing.Point(770, 720);
            }

            void SetAnalogButtonsVisibleEnabled(bool enabledVisible)
            {
                this.buttonDisableAllAnalogEvents.Enabled = enabledVisible;
                this.buttonDisableAllAnalogEvents.Visible = enabledVisible;
                this.buttonEnableAllAnalogEvents.Enabled = enabledVisible;
                this.buttonEnableAllAnalogEvents.Visible = enabledVisible;
                this.buttonSendAnalogEnables.Enabled = enabledVisible;
                this.buttonSendAnalogEnables.Visible = enabledVisible;
            }

            void RemoveBinaryButtonsFrom(TabPage page)
            {
                if (page.Controls.Contains(this.buttonSendBinaryEventEnables))
                {
                    page.Controls.Remove(this.buttonSendBinaryEventEnables);
                    page.Controls.Remove(this.buttonEnableAllBinaryEvents);
                    page.Controls.Remove(this.buttonDisableAllBinaryEvents);
                }
            }

            void EnsureBinaryButtonsOn(TabPage page)
            {
                if (!page.Controls.Contains(this.buttonSendBinaryEventEnables))
                {
                    page.Controls.Add(this.buttonSendBinaryEventEnables);
                    page.Controls.Add(this.buttonEnableAllBinaryEvents);
                    page.Controls.Add(this.buttonDisableAllBinaryEvents);
                }
            }

            void SetBinaryButtonsThStyle()
            {
                this.buttonDisableAllBinaryEvents.Location = new System.Drawing.Point(170, 720);
                this.buttonDisableAllBinaryEvents.Visible = true;

                this.buttonEnableAllBinaryEvents.Location = new System.Drawing.Point(470, 720);
                this.buttonEnableAllBinaryEvents.Visible = true;

                this.buttonSendBinaryEventEnables.Location = new System.Drawing.Point(770, 720);
                this.buttonSendBinaryEventEnables.Visible = true;
            }

            void SetBinaryButtonsConedPsegStyle()
            {
                this.buttonDisableAllBinaryEvents.Location = new System.Drawing.Point(670, 723);
                this.buttonDisableAllBinaryEvents.Size = new System.Drawing.Size(192, 32);
                this.buttonDisableAllBinaryEvents.Visible = true;

                this.buttonEnableAllBinaryEvents.Location = new System.Drawing.Point(870, 723);
                this.buttonEnableAllBinaryEvents.Size = new System.Drawing.Size(190, 32);
                this.buttonEnableAllBinaryEvents.Visible = true;

                this.buttonSendBinaryEventEnables.Location = new System.Drawing.Point(1285, 722);
                this.buttonSendBinaryEventEnables.Size = new System.Drawing.Size(220, 32);
                this.buttonSendBinaryEventEnables.Visible = true;
            }

            void SetBinaryButtonsEnmaxStyleHidden()
            {
                this.buttonDisableAllBinaryEvents.Location = new System.Drawing.Point(170, 720);
                this.buttonEnableAllBinaryEvents.Location = new System.Drawing.Point(470, 720);
                this.buttonSendBinaryEventEnables.Location = new System.Drawing.Point(770, 720);

                // DNP Event Enable buttons not displayed for Enmax
                this.buttonDisableAllBinaryEvents.Enabled = false;
                this.buttonDisableAllBinaryEvents.Visible = false;
                this.buttonEnableAllBinaryEvents.Enabled = false;
                this.buttonEnableAllBinaryEvents.Visible = false;
                this.buttonSendBinaryEventEnables.Enabled = false;
                this.buttonSendBinaryEventEnables.Visible = false;
            }

            // ----- Analog tab handling -----
            if (selected == this.tabPageAnalogInputs1 ||
                selected == this.tabPageAnalogInputs2 ||
                selected == this.tabPageAnalogInputs3)
            {
                // remove from all three first
                RemoveAnalogButtonsFrom(this.tabPageAnalogInputs1);
                RemoveAnalogButtonsFrom(this.tabPageAnalogInputs2);
                RemoveAnalogButtonsFrom(this.tabPageAnalogInputs3);

                // add to selected analog tab
                EnsureAnalogButtonsOn(selected);
                SetAnalogButtonLocations();

                // runtime equivalent of #if !TORONTO_HYDRO
                bool analogEventsEnabled = (this.customer == Customers.TORONTO_HYDRO);
                SetAnalogButtonsVisibleEnabled(analogEventsEnabled);
                return;
            }

            // ----- Binary Inputs tab handling -----
            if (selected == this.tabPageBinaryInputs)
            {
                EnsureBinaryButtonsOn(this.tabPageBinaryInputs);

                // runtime equivalent of old #if branches
                if (this.customer == Customers.TORONTO_HYDRO)
                {
                    SetBinaryButtonsThStyle();
                    this.buttonDisableAllBinaryEvents.Enabled = true;
                    this.buttonEnableAllBinaryEvents.Enabled = true;
                    this.buttonSendBinaryEventEnables.Enabled = true;
                }
                else if (this.customer == Customers.CONED || this.customer == Customers.PSEG)
                {
                    SetBinaryButtonsConedPsegStyle();
                    this.buttonDisableAllBinaryEvents.Enabled = true;
                    this.buttonEnableAllBinaryEvents.Enabled = true;
                    this.buttonSendBinaryEventEnables.Enabled = true;
                }
                else if (this.customer == Customers.ENMAX)
                {
                    RemoveBinaryButtonsFrom(this.tabPageBinaryInputs2); // mirrors old ENMAX swap behavior
                    EnsureBinaryButtonsOn(this.tabPageBinaryInputs);
                    SetBinaryButtonsEnmaxStyleHidden();
                }
                else
                {
                    // default: hide buttons for other customers
                    this.buttonDisableAllBinaryEvents.Enabled = false;
                    this.buttonDisableAllBinaryEvents.Visible = false;
                    this.buttonEnableAllBinaryEvents.Enabled = false;
                    this.buttonEnableAllBinaryEvents.Visible = false;
                    this.buttonSendBinaryEventEnables.Enabled = false;
                    this.buttonSendBinaryEventEnables.Visible = false;
                }

                return;
            }

            // ----- Binary Inputs 2 tab handling -----
            if (selected == this.tabPageBinaryInputs2)
            {
                // old behavior only had logic here for ENMAX
                if (this.customer == Customers.ENMAX)
                {
                    RemoveBinaryButtonsFrom(this.tabPageBinaryInputs);
                    EnsureBinaryButtonsOn(this.tabPageBinaryInputs2);
                    SetBinaryButtonsEnmaxStyleHidden();
                }

                return;
            }
        }

        private void buttonSendBinaryEventEnables_Click(object sender, EventArgs e)
        {
            uint i = 0;
            uint packetByteNumber = 2; //starts at 2 after OpCode and SubCode
            byte tempByte = 0;

            SendEventArgs sEA = new SendEventArgs(_packetLength);
     //       MessageBox.Show("5 sending command D to master"); // Only for testing - to be removed
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
       //     MessageBox.Show("6 sending command D to master"); // Only for testing - to be removed
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
        private void ucDNPDIGITALGRIDData_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.initializeComponents();

                // Re-apply runtime tab policy every time view becomes visible
                ApplyDnpTabVisibilityPolicy();

                // Hard-stop: no Analog Outputs in this instance
                this.tabControlMemphisDNP.TabPages.Remove(this.tabPageAnalogOutputs);

                SetSize();

                this.tabControlMemphisDNP.Visible = true;
                this.tabControlMemphisDNP.Enabled = true;
                this.tabControlMemphisDNP.BringToFront();

                if (this.tabControlMemphisDNP.TabPages.Count > 0)
                    this.tabControlMemphisDNP.SelectedIndex = 0;

                this.tabControlMemphisDNP_SelectedIndexChanged_1(this, EventArgs.Empty);

                System.Diagnostics.Debug.WriteLine(
                    $"[VisibleChanged] HasAnalogOut={this.tabControlMemphisDNP.TabPages.Contains(this.tabPageAnalogOutputs)}, " +
                    $"TabCount={this.tabControlMemphisDNP.TabPages.Count}");

                this.PerformLayout();
                this.Refresh();
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
