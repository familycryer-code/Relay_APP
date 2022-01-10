using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RelayControlLibrary;
using Exocortex.DSP;
using System.Runtime.Serialization;
using System.Collections;
using SharedResources;

namespace SineDisplayGraph
{
    public partial class ucEventGraph : UserControl
    {
        private Customers customer = Customers.NonConEd;
        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                if (this.customer != value)
                {
                    this.customer = value;
                    this.setCustomer();
                }

            }
        }

        private void setCustomer()
        {
            if (this.gEEnabled)
            {
#if !DEBUG
                this.sineGraphVtA.Visible = false;
                this.sineGraphVtB.Visible = false;
                this.sineGraphVtC.Visible = false;
                this.sineGraphVnA.Location = this.sineGraphVtA.Location;
                this.sineGraphVnB.Location = this.sineGraphVtB.Location;
                this.sineGraphVnC.Location = this.sineGraphVtC.Location;
#endif
            }
            else
            {
                this.sineGraphVtA.Visible = true;
                this.sineGraphVtB.Visible = true;
                this.sineGraphVtC.Visible = true;
                ucEventGraph_SizeChanged(this, new EventArgs());
            }
        }

        public uint RelayRevisionNumber = 0;
        public Int32 CTRatio
        {
            get { return this.cTRatio; }
            set
            {
                this.cTRatio = value;
                this.sineGraphIA.CTRatio = value;
                this.sineGraphIB.CTRatio = value;
                this.sineGraphIC.CTRatio = value;
            }
        }
        public uint EventNumber
        {
            get { return this.eventNumber; }
            set
            {
                this.eventNumber = value;
            }
        }
        public EventTypes Type
        {
            get { return this.type; }
            set
            {
                this.type = value;
                this.setType(value);
            }
        }
        public DateTime BaseTime = new DateTime(2009, 8, 1);
        public DateTime EndTime = new DateTime(2145, 9, 7, 6, 28, 15);
        public DateTime EventTime
        {
            get { return this.eventTime; }
            set
            {
                this.eventTime = value;
                if (!EventTime.Equals(EndTime))
                {
                    this.showAllEventLines(true);
                }
                else
                {
                    this.showAllEventLines(false);
                }

            }
        }
        public uint DelayToBFlag
        {
            get
            {
                return this.delayToBFlag;
            }
            set
            {
                this.delayToBFlag = value;
                //this.setEventLabel();
            }
        }
        public uint DelayToFloat
        {
            get
            {
                return this.delayToFloat;
            }
            set
            {
                this.delayToFloat = value;
                //this.setEventLabel();
            }
        }
        public bool Saveable = false;
        public UInt16 RelayID;
        public CalibrationConstants CalConstants = new CalibrationConstants();
        public delegate void ExceptionHandler(object o, ExceptionEventArgs eEA);
        public event ExceptionHandler EventGraphException;
        public delegate void DownloadCompleteHandler();
        public event DownloadCompleteHandler DownloadComplete;
        public delegate void PacketHandledHandler(object sender, PacketHandledEventArgs e);
        public event PacketHandledHandler PacketHandled;
        public ProtectorVoltage ProtectorVoltage
        {
            get { return this.protectorVoltage; }
            set
            {
                if (value != this.protectorVoltage)
                {
                    this.protectorVoltage = value;
                    this.setProtectorVoltage();
                }
            }
        }

        const int _arraySize = 2048;
        private uint eventNumber;
        private EventTypes type;
        private bool EventVisible = false;
        private DateTime eventTime;
        private delegate void setAllCallBack(byte[] bytePacket);
        private Int32 cTRatio;
        private ProtectorVoltage protectorVoltage =
            ProtectorVoltages.GetVoltage();
        private uint delayToBFlag;
        private uint delayToFloat;

        public ucEventGraph()
        {
            InitializeComponent();
            this.ucEventGraph_SizeChanged(this, new EventArgs());
            this.myInitialize();
        }

        public ucEventGraph(PhasorTypes pT)
        {
            InitializeComponent();

            this.myInitialize();
        }

        public void SetLabel()
        {
            this.setEventLabel();
        }

        private void myInitialize()
        {
            this.sineGraphVtA.BackColor = Color.White;
            this.sineGraphVtA.ScrollEnabled = true;
            this.sineGraphVtA.AddNewSineWave(PhasorTypes.VtA, _arraySize);
            this.sineGraphVtA.GraphName = "VtA";
            this.sineGraphVtA.ShowEventLine = true;
            this.sineGraphVtA.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVtA.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVtA.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphLeftClicked);
            this.sineGraphVtA.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphVtB.BackColor = Color.White;
            this.sineGraphVtB.ScrollEnabled = true;
            this.sineGraphVtB.AddNewSineWave(PhasorTypes.VtB, _arraySize);
            this.sineGraphVtB.GraphName = "VtB";
            this.sineGraphVtB.ShowEventLine = true;
            this.sineGraphVtB.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVtB.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVtB.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphLeftClicked);
            this.sineGraphVtB.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphVtC.BackColor = Color.White;
            this.sineGraphVtC.ScrollEnabled = true;
            this.sineGraphVtC.AddNewSineWave(PhasorTypes.VtC, _arraySize);
            this.sineGraphVtC.GraphName = "VtC";
            this.sineGraphVtC.ShowEventLine = true;
            this.sineGraphVtC.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVtC.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVtC.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphLeftClicked);
            this.sineGraphVtC.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphVnA.BackColor = Color.White;
            this.sineGraphVnA.ScrollEnabled = true;
            this.sineGraphVnA.AddNewSineWave(PhasorTypes.VnA, _arraySize);
            this.sineGraphVnA.GraphName = "VnA";
            this.sineGraphVnA.ShowEventLine = true;
            this.sineGraphVnA.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVnA.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVnA.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphLeftClicked);
            this.sineGraphVnA.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphVnB.BackColor = Color.White;
            this.sineGraphVnB.ScrollEnabled = true;
            this.sineGraphVnB.AddNewSineWave(PhasorTypes.VnB, _arraySize);
            this.sineGraphVnB.GraphName = "VnB";
            this.sineGraphVnB.ShowEventLine = true;
            this.sineGraphVnB.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVnB.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVnB.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphLeftClicked);
            this.sineGraphVnB.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphVnC.BackColor = Color.White;
            this.sineGraphVnC.ScrollEnabled = true;
            this.sineGraphVnC.AddNewSineWave(PhasorTypes.VnC, _arraySize);
            this.sineGraphVnC.GraphName = "VnC";
            this.sineGraphVnC.ShowEventLine = true;
            this.sineGraphVnC.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVnC.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVnC.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphLeftClicked);
            this.sineGraphVnC.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphIA.BackColor = Color.White;
            this.sineGraphIA.ScrollEnabled = true;
            this.sineGraphIA.AddNewSineWave(PhasorTypes.IA, _arraySize);
            this.sineGraphIA.GraphName = "IA";
            this.sineGraphIA.ShowEventLine = true;
            this.sineGraphIA.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphIA.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphIA.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphLeftClicked);
            this.sineGraphIA.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphIB.BackColor = Color.White;
            this.sineGraphIB.ScrollEnabled = true;
            this.sineGraphIB.AddNewSineWave(PhasorTypes.IB, _arraySize);
            this.sineGraphIB.GraphName = "IB";
            this.sineGraphIB.ShowEventLine = true;
            this.sineGraphIB.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphIB.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphIB.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphLeftClicked);
            this.sineGraphIB.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphIC.BackColor = Color.White;
            this.sineGraphIC.ScrollEnabled = true;
            this.sineGraphIC.AddNewSineWave(PhasorTypes.IC, _arraySize);
            this.sineGraphIC.GraphName = "IC";
            this.sineGraphIC.ShowEventLine = true;
            this.sineGraphIC.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphIC.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphIC.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphLeftClicked);
            this.sineGraphIC.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.frequencyGraphA.NumberOfHarmonics = 32;
            this.frequencyGraphB.NumberOfHarmonics = 32;
            this.frequencyGraphC.NumberOfHarmonics = 32;
        }

        void sineGraph_MouseWheeledEvent(object sender, MouseEventArgs e)
        {
            this.sineGraphVtA.ExternalMouseWheel(sender, e);
            this.sineGraphVtB.ExternalMouseWheel(sender, e);
            this.sineGraphVtC.ExternalMouseWheel(sender, e);

            this.sineGraphVnA.ExternalMouseWheel(sender, e);
            this.sineGraphVnB.ExternalMouseWheel(sender, e);
            this.sineGraphVnC.ExternalMouseWheel(sender, e);

            this.sineGraphIA.ExternalMouseWheel(sender, e);
            this.sineGraphIB.ExternalMouseWheel(sender, e);
            this.sineGraphIC.ExternalMouseWheel(sender, e);
        }

        void sineGraph_ScrollEvent(object sender, ScrollEventArgs e)
        {
            this.sineGraphVtA.ExternalScroll(sender, e);
            this.sineGraphVtB.ExternalScroll(sender, e);
            this.sineGraphVtC.ExternalScroll(sender, e);

            this.sineGraphVnA.ExternalScroll(sender, e);
            this.sineGraphVnB.ExternalScroll(sender, e);
            this.sineGraphVnC.ExternalScroll(sender, e);

            this.sineGraphIA.ExternalScroll(sender, e);
            this.sineGraphIB.ExternalScroll(sender, e);
            this.sineGraphIC.ExternalScroll(sender, e);
        }

        private void showAllEventLines(bool b)
        {
            if (!b)
                this.Type = EventTypes.NoEvent;

            this.EventVisible = b;
            this.sineGraphIA.ShowEventLine = b;
            this.sineGraphIB.ShowEventLine = b;
            this.sineGraphIC.ShowEventLine = b;

            this.sineGraphVnA.ShowEventLine = b;
            this.sineGraphVnB.ShowEventLine = b;
            this.sineGraphVnC.ShowEventLine = b;

            this.sineGraphVtA.ShowEventLine = b;
            this.sineGraphVtB.ShowEventLine = b;
            this.sineGraphVtC.ShowEventLine = b;

            this.setEventLabel();
        }

        private void setEventLabel()
        {
            string outputText = "";
            if (this.EventVisible)
            {
                this.labelEventLabel.Text = "Relay ID: " + this.RelayID.ToString() + " - " + this.Type.ToString() + " " + this.eventTime.ToString();
                this.centerLabel(this.labelEventLabel);

                if (this.RelayRevisionNumber >= 20111123 && this.type == EventTypes.Trip && this.Customer != Customers.Memphis) //for when the trip delay times were put in
                {
                    outputText = "Cycles To Protector Open Flag: ";

                    if (this.delayToBFlag == (UInt16)0xFFFF || this.delayToBFlag == (UInt16)0xFFFE)
                    {
                        outputText += "Time Out    -    ";
                    }
                    else
                    {
                        outputText += this.delayToBFlag.ToString() + "    -    ";
                    }

                    outputText += " Cycles To Float Condition: ";

                    if (this.delayToFloat == (UInt16)0xFFFF || this.delayToFloat == (UInt16)0xFFFE)
                    {
                        outputText += "Time Out";
                    }
                    else
                    {
                        outputText += this.delayToFloat.ToString();
                    }

                    this.labelEventLabel2.Text = outputText;
                    this.centerLabel(this.labelEventLabel);
                    this.centerLabel(this.labelEventLabel2);
                }
                else
                    this.labelEventLabel2.Text = "";
            }
            else
            {
                this.labelEventLabel.Text = "No Event";
                this.labelEventLabel2.Text = "";
                this.centerLabel(this.labelEventLabel);
            }
        }

        private void centerLabel(Label label)
        {
            int halfLabelWidth = label.Width / 2;
            int halfControlWidth = this.Width / 2;

            Point labelLocation = label.Location;
            labelLocation.X = halfControlWidth - halfLabelWidth;
            label.Location = labelLocation;
        }

        #region Resizing

        private void ucEventGraph_SizeChanged(object sender, EventArgs e)
        {
            int spaceForGraph = ((this.Height - 50) / 6);
            int heightOfGraph = spaceForGraph - 2;
            int widthOfGraph = (this.Width - 4) / 2;
            int xOfRightGraph = widthOfGraph + 4;
            int xOfLeftGraph = 2;

            this.sineGraphVtA.Height = heightOfGraph;
            this.sineGraphVtB.Height = heightOfGraph;
            this.sineGraphVtC.Height = heightOfGraph;
            this.sineGraphVnA.Height = heightOfGraph;
            this.sineGraphVnB.Height = heightOfGraph;
            this.sineGraphVnC.Height = heightOfGraph;
            this.sineGraphIA.Height = heightOfGraph;
            this.sineGraphIB.Height = heightOfGraph;
            this.sineGraphIC.Height = heightOfGraph;
            this.frequencyGraphA.Height = heightOfGraph;
            this.frequencyGraphB.Height = heightOfGraph;
            this.frequencyGraphC.Height = heightOfGraph;

            this.sineGraphVtA.Location = new Point(xOfLeftGraph, 50);
            this.sineGraphVnA.Location = new Point(xOfRightGraph, 50);
            this.sineGraphIA.Location = new Point(xOfLeftGraph, spaceForGraph + 50);
            this.frequencyGraphA.Location = new Point(xOfRightGraph, spaceForGraph + 50);

            this.sineGraphVtB.Location = new Point(xOfLeftGraph, spaceForGraph * 2 + 50);
            this.sineGraphVnB.Location = new Point(xOfRightGraph, spaceForGraph * 2 + 50);
            this.sineGraphIB.Location = new Point(xOfLeftGraph, spaceForGraph * 3 + 50);
            this.frequencyGraphB.Location = new Point(xOfRightGraph, spaceForGraph * 3 + 50);

            this.sineGraphVtC.Location = new Point(xOfLeftGraph, spaceForGraph * 4 + 50);
            this.sineGraphVnC.Location = new Point(xOfRightGraph, spaceForGraph * 4 + 50);
            this.sineGraphIC.Location = new Point(xOfLeftGraph, spaceForGraph * 5 + 50);
            this.frequencyGraphC.Location = new Point(xOfRightGraph, spaceForGraph * 5 + 50);

            this.sineGraphVtA.Width = widthOfGraph;
            this.sineGraphVtB.Width = widthOfGraph;
            this.sineGraphVtC.Width = widthOfGraph;
            this.sineGraphVnA.Width = widthOfGraph;
            this.sineGraphVnB.Width = widthOfGraph;
            this.sineGraphVnC.Width = widthOfGraph;
            this.sineGraphIA.Width = widthOfGraph;
            this.sineGraphIB.Width = widthOfGraph;
            this.sineGraphIC.Width = widthOfGraph;
            this.frequencyGraphA.Width = widthOfGraph;
            this.frequencyGraphB.Width = widthOfGraph;
            this.frequencyGraphC.Width = widthOfGraph;
        }

        #endregion

        #region Event

        private void setType(EventTypes value)
        {
            switch (value)
            {
                case EventTypes.Close:
                case EventTypes.Trip:
                case EventTypes.Float:
                    this.Saveable = false;
                    break;
                case EventTypes.NoEvent:
                    this.Saveable = true;
                    this.EventVisible = false;
                    break;
            }
            this.setEventLabel();
        }

        public delegate void ValuesForPhasorGraph(object sender, CompleteCycleEventArgs cCEA);
        public event ValuesForPhasorGraph PopulatePhasorGraph;

        private void graphRightClicked(object sender, SineGraphEventArgs sGEA)
        {
            CompleteCycleEventArgs cCEA = new CompleteCycleEventArgs(128);

            cCEA.VtA = this.sineGraphVtA.GetSingleCycle(sGEA.ClickedCycleNumber);
            cCEA.VtB = this.sineGraphVtB.GetSingleCycle(sGEA.ClickedCycleNumber);
            cCEA.VtC = this.sineGraphVtC.GetSingleCycle(sGEA.ClickedCycleNumber);
            cCEA.VnA = this.sineGraphVnA.GetSingleCycle(sGEA.ClickedCycleNumber);
            cCEA.VnB = this.sineGraphVnB.GetSingleCycle(sGEA.ClickedCycleNumber);
            cCEA.VnC = this.sineGraphVnC.GetSingleCycle(sGEA.ClickedCycleNumber);
            cCEA.IA = this.sineGraphIA.GetSingleCycle(sGEA.ClickedCycleNumber);
            cCEA.IB = this.sineGraphIB.GetSingleCycle(sGEA.ClickedCycleNumber);
            cCEA.IC = this.sineGraphIC.GetSingleCycle(sGEA.ClickedCycleNumber);
            cCEA.CycleNumber = sGEA.ClickedCycleNumber;
            cCEA.EventNumber = this.EventNumber;

            if (PopulatePhasorGraph != null)
                PopulatePhasorGraph(this, cCEA);
        }

        private void graphLeftClicked(object sender, EventArgs e)
        {
            SineGraph sGraph = (SineGraph)sender;
            if (!this.Saveable)
            {
                this.error(new Exception("Need all data to do an FFT"));
                return;
            }

            switch (sGraph.Name)
            {
                case "sineGraphVtA":
                    this.setFrequencyGraph(this.frequencyGraphA, this.sineGraphVtA);
                    break;
                case "sineGraphVnA":
                    this.setFrequencyGraph(this.frequencyGraphA, this.sineGraphVnA);
                    break;
                case "sineGraphIA":
                    this.setFrequencyGraph(this.frequencyGraphA, this.sineGraphIA);
                    break;
                case "sineGraphVtB":
                    this.setFrequencyGraph(this.frequencyGraphB, this.sineGraphVtB);
                    break;
                case "sineGraphVnB":
                    this.setFrequencyGraph(this.frequencyGraphB, this.sineGraphVnB);
                    break;
                case "sineGraphIB":
                    this.setFrequencyGraph(this.frequencyGraphB, this.sineGraphIB);
                    break;
                case "sineGraphVtC":
                    this.setFrequencyGraph(this.frequencyGraphC, this.sineGraphVtC);
                    break;
                case "sineGraphVnC":
                    this.setFrequencyGraph(this.frequencyGraphC, this.sineGraphVnC);
                    break;
                case "sineGraphIC":
                    this.setFrequencyGraph(this.frequencyGraphC, this.sineGraphIC);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Data Handling

        public void SetAll(byte[] bytePacket)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    setAllCallBack sACB = new setAllCallBack(this.SetAll);
                    Invoke(sACB, new object[] { bytePacket });
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
            this.setSineGraph(bytePacket);
        }

        private void setSineGraph(byte[] bytePacket)
        {
            SineGraph sineGraph;
            PacketHandledEventArgs pHEA = new PacketHandledEventArgs();

            try
            {
                if (!this.EventVisible)
                    return;
                PhasorTypes phasor = RelayModeFunctions.PhasorTypeFrom((char)bytePacket[1], (char)bytePacket[2]);
                SineWaveDefinition sWD;
                CalibrationConstant cC;
                int indexOffset;

                sineGraph = this.getSineGraph(phasor);
                cC = this.getCalConstant(phasor);

                sWD = (SineWaveDefinition)sineGraph.sineWavesToDraw[0];
                sWD.Enabled = true;
                indexOffset = this.getIndexOffset(bytePacket[3], bytePacket[4]);

                for (int i = 5; i < 132; i += 2)
                {
                    float temp;

                    if (phasor == PhasorTypes.IA || phasor == PhasorTypes.IB || phasor == PhasorTypes.IC)
                        temp = this.dACReadingI(bytePacket[i], bytePacket[i + 1], cC);
                    else
                        temp = this.dACReadingV(bytePacket[i], bytePacket[i + 1], cC);

                    sWD.AddValueNew(temp, indexOffset + (i - 5) / 2);
                }

                pHEA.Successful = true;

                if (indexOffset == 1984)
                {
                    sineGraph.Invalidate();
                    if (phasor == PhasorTypes.IA || phasor == PhasorTypes.IB || phasor == PhasorTypes.IC)
                    {
                        sWD.Zero();
                    }

                    if (phasor == PhasorTypes.IC)
                    {
                        this.Done();
                        this.Saveable = true;
                    }
                    else
                    {
                        packetHandled(pHEA);
                    }
                }
                else
                {
                    this.packetHandled(pHEA);
                    this.Saveable = false;
                }
            }
            catch
            {
                this.packetHandled(new PacketHandledEventArgs(false));
            }
        }

        private EventTypes eventType(byte p)
        {
            switch ((char)p)
            {
                case 'O':
                    return EventTypes.Trip;
                case 'C':
                    return EventTypes.Close;
                case 'I':
                    return EventTypes.InInsensitiveRegion;
                default:
                    throw new Exception(Convert.ToChar(p).ToString() + " is an unrecognized character for eventType");
            }
        }

        private Int16 convertRawDACToInt16(byte mSB, byte lSB)
        {
            Int16 returnInt;

            returnInt = (Int16)(mSB << 8);
            returnInt += (Int16)lSB;

            if (returnInt > 0x0800)
                returnInt = (Int16)(-returnInt);

            return returnInt;
        }

        private SineGraph getSineGraph(PhasorTypes phasor)
        {
            switch (phasor)
            {
                case PhasorTypes.IA:
                    return this.sineGraphIA;
                case PhasorTypes.IB:
                    return this.sineGraphIB;
                case PhasorTypes.IC:
                    return this.sineGraphIC;
                case PhasorTypes.VnA:
                    return this.sineGraphVnA;
                case PhasorTypes.VnB:
                    return this.sineGraphVnB;
                case PhasorTypes.VnC:
                    return this.sineGraphVnC;
                case PhasorTypes.VtA:
                    return this.sineGraphVtA;
                case PhasorTypes.VtB:
                    return this.sineGraphVtB;
                case PhasorTypes.VtC:
                    return this.sineGraphVtC;
                default:
                    throw new Exception(phasor.ToString() + " is not a valid phasor type");
            }
        }

        private CalibrationConstant getCalConstant(PhasorTypes phasor)
        {
            switch (phasor)
            {
                case PhasorTypes.IA:
                    return this.CalConstants.IA;
                case PhasorTypes.IB:
                    return this.CalConstants.IB;
                case PhasorTypes.IC:
                    return this.CalConstants.IC;
                case PhasorTypes.VnA:
                    return this.CalConstants.VnA;
                case PhasorTypes.VnB:
                    return this.CalConstants.VnB;
                case PhasorTypes.VnC:
                    return this.CalConstants.VnC;
                case PhasorTypes.VtA:
                    return this.CalConstants.VtA;
                case PhasorTypes.VtB:
                    return this.CalConstants.VtB;
                case PhasorTypes.VtC:
                    return this.CalConstants.VtC;
                default:
                    throw new Exception(phasor.ToString() + " is not a valid phasor type");
            }
        }

        private int getIndexOffset(byte cycleNumber, byte half)
        {
            int returnInt;

            returnInt = (cycleNumber - 1) * 128;

            if (half == 1)
            {
                return returnInt;
            }
            else if (half == 0)
            {
                return returnInt + 64;
            }
            else
            {
                throw new Exception("Bad Half Cycle Character");
            }
        }

        private float dACReadingV(byte p, byte p_2, CalibrationConstant workingCalConstant)
        {
            Int16 tempInt;
            float returnFloat;

            tempInt = p_2;  //Create 16 bit number from two bytes
            tempInt <<= 8;
            tempInt += p;

            tempInt >>= 3; //Shift over three because that is format of information from DAC

            returnFloat = (float)tempInt * workingCalConstant.RealValue;

            returnFloat = returnFloat / 1024f;

            return returnFloat;
        }

        private float dACReadingI(byte p, byte p_2, CalibrationConstant workingCalConstant)
        {
            Int16 tempInt;
            float returnFloat;

            tempInt = p_2;  //Create 16 bit number from two bytes
            tempInt <<= 8;
            tempInt += p;

            tempInt >>= 3; //Shift over three because that is format of information from DAC

            returnFloat = (float)tempInt * workingCalConstant.RealValue;

            returnFloat = returnFloat / 4096;

            return returnFloat;
        }

        private void Done()
        {
            if (DownloadComplete != null)
                this.DownloadComplete();
        }

        private void packetHandled(PacketHandledEventArgs e)
        {
            if (PacketHandled != null)
                PacketHandled(this, e);
        }


        #endregion

        #region Exception Control

        private void error(Exception ex)
        {
            if (this.EventGraphException != null)
            {
                this.EventGraphException(this, new ExceptionEventArgs(ex, "Error in Event Graph Control"));
            }
            else
            {
                throw new Exception("Event Graph Exceptions are not being handled");
            }
        }
        #endregion

        private void setFrequencyGraph(FrequencyGraph fG, SineGraph sG)
        {
            SineWaveDefinition sWD = (SineWaveDefinition)sG.sineWavesToDraw[0];
            float[] tempFloat = new float[128 << 3];//float[sWD.InputArray.Length];
            int startIndex = sG.ClickedCycleNumber * 128;
            fG.PhasorType = sG.Type;

            sWD.Zero();

            for (int i = 0; i < 128; ++i, ++startIndex)
            {
                if (RelayModeFunctions.IsCurrent(sWD.Phase))
                    tempFloat[i] = sWD.ActualValues[startIndex] * this.CTRatio;
                else
                    tempFloat[i] = sWD.ActualValues[startIndex];
            }
            for (int i = 128; i < tempFloat.Length; ++i)
            {
                tempFloat[i] = 0;
            }

            fG.ClickedCycleNumber = sG.ClickedCycleNumber;
            fG.maxValue = 100;
            fG.SineWave = tempFloat;
        }

        public void ClearAllGraphs()
        {
            this.sineGraphIA.ClearData();
            this.sineGraphIA.AddNewSineWave(PhasorTypes.IA, _arraySize);
            this.sineGraphIA.Invalidate();

            this.sineGraphIB.ClearData();
            this.sineGraphIB.AddNewSineWave(PhasorTypes.IB, _arraySize);
            this.sineGraphIB.Invalidate();

            this.sineGraphIC.ClearData();
            this.sineGraphIC.AddNewSineWave(PhasorTypes.IC, _arraySize);
            this.sineGraphIC.Invalidate();

            this.sineGraphVtA.ClearData();
            this.sineGraphVtA.AddNewSineWave(PhasorTypes.VtA, _arraySize);
            this.sineGraphVtA.Invalidate();

            this.sineGraphVtB.ClearData();
            this.sineGraphVtB.AddNewSineWave(PhasorTypes.VtB, _arraySize);
            this.sineGraphVtB.Invalidate();

            this.sineGraphVtC.ClearData();
            this.sineGraphVtC.AddNewSineWave(PhasorTypes.VtC, _arraySize);
            this.sineGraphVtC.Invalidate();

            this.sineGraphVnA.ClearData();
            this.sineGraphVnA.AddNewSineWave(PhasorTypes.VnA, _arraySize);
            this.sineGraphVnA.Invalidate();

            this.sineGraphVnB.ClearData();
            this.sineGraphVnB.AddNewSineWave(PhasorTypes.VnB, _arraySize);
            this.sineGraphVnB.Invalidate();

            this.sineGraphVnC.ClearData();
            this.sineGraphVnC.AddNewSineWave(PhasorTypes.VnC, _arraySize);
            this.sineGraphVnC.Invalidate();

            this.frequencyGraphA.ClearAllValues();
            this.frequencyGraphB.ClearAllValues();
            this.frequencyGraphC.ClearAllValues();
        }

        public float[] GetSineWave(PhasorTypes pT)
        {
            SineWaveDefinition sWD;

            switch (pT)
            {
                case PhasorTypes.IA:
                    sWD = (SineWaveDefinition)this.sineGraphIA.sineWavesToDraw[0];
                    return sWD.ActualValues;
                case PhasorTypes.IB:
                    sWD = (SineWaveDefinition)this.sineGraphIB.sineWavesToDraw[0];
                    return sWD.ActualValues;
                case PhasorTypes.IC:
                    sWD = (SineWaveDefinition)this.sineGraphIC.sineWavesToDraw[0];
                    return sWD.ActualValues;
                case PhasorTypes.VnA:
                    sWD = (SineWaveDefinition)this.sineGraphVnA.sineWavesToDraw[0];
                    return sWD.ActualValues;
                case PhasorTypes.VnB:
                    sWD = (SineWaveDefinition)this.sineGraphVnB.sineWavesToDraw[0];
                    return sWD.ActualValues;
                case PhasorTypes.VnC:
                    sWD = (SineWaveDefinition)this.sineGraphVnC.sineWavesToDraw[0];
                    return sWD.ActualValues;
                case PhasorTypes.VtA:
                    sWD = (SineWaveDefinition)this.sineGraphVtA.sineWavesToDraw[0];
                    return sWD.ActualValues;
                case PhasorTypes.VtB:
                    sWD = (SineWaveDefinition)this.sineGraphVtB.sineWavesToDraw[0];
                    return sWD.ActualValues;
                case PhasorTypes.VtC:
                    sWD = (SineWaveDefinition)this.sineGraphVtC.sineWavesToDraw[0];
                    return sWD.ActualValues;
                default:
                    throw new Exception("Bad Sine Wave Request In " + this.ToString());
            }
        }

        public void SetSineWave(PhasorTypes pT, float[] f)
        {
            try
            {
                SineWaveDefinition sWD;
                SineGraph sG;

                switch (pT)
                {
                    case PhasorTypes.IA:
                        sG = this.sineGraphIA;
                        break;
                    case PhasorTypes.IB:
                        sG = this.sineGraphIB;
                        break;
                    case PhasorTypes.IC:
                        sG = this.sineGraphIC;
                        break;
                    case PhasorTypes.VnA:
                        sG = this.sineGraphVnA;
                        break;
                    case PhasorTypes.VnB:
                        sG = this.sineGraphVnB;
                        break;
                    case PhasorTypes.VnC:
                        sG = this.sineGraphVnC;
                        break;
                    case PhasorTypes.VtA:
                        sG = this.sineGraphVtA;
                        break;
                    case PhasorTypes.VtB:
                        sG = this.sineGraphVtB;
                        break;
                    case PhasorTypes.VtC:
                        sG = this.sineGraphVtC;
                        break;
                    default:
                        throw new Exception("Bad Phasor Type");
                }

                sWD = (SineWaveDefinition)sG.sineWavesToDraw[0];
                sWD.Enabled = true;

                for (int i = 0; i < f.Length - 1; ++i)
                {
                    sWD.AddValueNew(f[i], i);
                }
                sG.Invalidate();
            }
            catch (Exception ex)
            {
                this.error(new Exception("Error Setting Sine Wave", ex));
            }
        }

        private SineFrequencyPopup sFP;
        private bool gEEnabled;

        public bool GEEnabled
        {
            get { return this.gEEnabled; }
            set
            {
                this.gEEnabled = value;
                this.setCustomer();
            }
        }

        private void sineGraph_DoubleClick(object sender, MouseEventArgs e)
        {
            SineGraph sG = (SineGraph)sender;

            if (sFP != null)
            {
                sFP.BringToFront();
                return;
            }

            sFP = new SineFrequencyPopup((SineWaveDefinition)sG.sineWavesToDraw[0], this.CTRatio, protectorVoltage);
            sFP.Disposed += new EventHandler(sFP_Disposed);
            sFP.Show();
        }

        void sFP_Disposed(object sender, EventArgs e)
        {
            if (this.sFP != null)
                this.sFP = null;
        }

        private void setProtectorVoltage()
        {
            this.sineGraphVnA.ProtectorVoltage = this.protectorVoltage;
            this.sineGraphVnB.ProtectorVoltage = this.protectorVoltage;
            this.sineGraphVnC.ProtectorVoltage = this.protectorVoltage;
            this.sineGraphVtA.ProtectorVoltage = this.protectorVoltage;
            this.sineGraphVtB.ProtectorVoltage = this.protectorVoltage;
            this.sineGraphVtC.ProtectorVoltage = this.protectorVoltage;
            this.frequencyGraphA.ProtectorVoltage = this.protectorVoltage;
            this.frequencyGraphB.ProtectorVoltage = this.protectorVoltage;
            this.frequencyGraphC.ProtectorVoltage = this.protectorVoltage;
        }

        public void GetCycleInfo(CycleInfoRequestEventArgs cIREA)
        {
            CompleteCycleEventArgs cCEA = new CompleteCycleEventArgs(128);

            cCEA.VtA = this.sineGraphVtA.GetSingleCycle(cIREA.CycleNumber);
            cCEA.VtB = this.sineGraphVtB.GetSingleCycle(cIREA.CycleNumber);
            cCEA.VtC = this.sineGraphVtC.GetSingleCycle(cIREA.CycleNumber);
            cCEA.VnA = this.sineGraphVnA.GetSingleCycle(cIREA.CycleNumber);
            cCEA.VnB = this.sineGraphVnB.GetSingleCycle(cIREA.CycleNumber);
            cCEA.VnC = this.sineGraphVnC.GetSingleCycle(cIREA.CycleNumber);
            cCEA.IA = this.sineGraphIA.GetSingleCycle(cIREA.CycleNumber);
            cCEA.IB = this.sineGraphIB.GetSingleCycle(cIREA.CycleNumber);
            cCEA.IC = this.sineGraphIC.GetSingleCycle(cIREA.CycleNumber);
            cCEA.CycleNumber = cIREA.CycleNumber;
            cCEA.EventNumber = this.EventNumber;

            if (PopulatePhasorGraph != null)
                PopulatePhasorGraph(this, cCEA);
        }
    }
}
