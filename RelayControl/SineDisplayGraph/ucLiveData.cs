using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RelayControlLibrary;
using System.Threading;
using SharedResources;

namespace SineDisplayGraph
{
    public partial class ucLiveData : UserControl
    {
        public ucLiveData()
        {
            InitializeComponent();
            this.ucLiveData_Resize(this, new EventArgs());
            this.initializeComponents();
        }

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
                this.sineGraphVtA.Visible = false;
                this.sineGraphVtB.Visible = false;
                this.sineGraphVtC.Visible = false;

                this.sineGraphVnA.Location = this.sineGraphVtA.Location;
                this.sineGraphVnB.Location = this.sineGraphVtB.Location;
                this.sineGraphVnC.Location = this.sineGraphVtC.Location;
            }
            else
            {
                this.sineGraphVtA.Visible = true;
                this.sineGraphVtB.Visible = true;
                this.sineGraphVtC.Visible = true;

                this.ucLiveData_Resize(this, new EventArgs());
            }

        }
        public UInt16 ID = 0;
        public CalibrationConstant CalConstantVtA;
        public CalibrationConstant CalConstantVtB;
        public CalibrationConstant CalConstantVtC;
        public CalibrationConstant CalConstantVnA;
        public CalibrationConstant CalConstantVnB;
        public CalibrationConstant CalConstantVnC;
        public CalibrationConstant CalConstantIA;
        public CalibrationConstant CalConstantIB;
        public CalibrationConstant CalConstantIC;
        public Int32 CTRatio
        {
            get { return this.cTRatio; }
            set
            {
                this.cTRatio = value;
                this.cTRatioChanged();
            }
        }
        public ProtectorVoltage ProtectorVoltage
        {
            get { return protectorVoltage; }
            set
            {
                protectorVoltage = value;
                this.setProtectorValue();
            }
        }

        private ProtectorVoltage protectorVoltage =
            ProtectorVoltages.GetVoltage();
        private Int32 cTRatio = 320;

        private void cTRatioChanged()
        {
            this.sineGraphIA.CTRatio = this.cTRatio;
            this.sineGraphIB.CTRatio = this.cTRatio;
            this.sineGraphIC.CTRatio = this.cTRatio;
        }



        private const int _arraySize = 8192;

        private void ucLiveData_Resize(object sender, EventArgs e)
        {
            int componentWidth = (this.Size.Width - 3) / 2;
            int componentHeight = (this.Size.Height - 15) / 6;
            int verticalSpacing = componentHeight + 2;
            int horizontalSpacing = componentWidth + 2;
            Size componentSize = new Size(componentWidth, componentHeight);

            this.sineGraphVtA.Size = componentSize;
            this.sineGraphVtB.Size = componentSize;
            this.sineGraphVtC.Size = componentSize;
            this.sineGraphVnA.Size = componentSize;
            this.sineGraphVnB.Size = componentSize;
            this.sineGraphVnC.Size = componentSize;
            this.sineGraphIA.Size = componentSize;
            this.sineGraphIB.Size = componentSize;
            this.sineGraphIC.Size = componentSize;
            this.frequencyGraphA.Size = componentSize;
            this.frequencyGraphB.Size = componentSize;
            this.frequencyGraphC.Size = componentSize;

            this.sineGraphVtA.Location = new Point(0, 0);
            this.sineGraphVnA.Location = new Point(horizontalSpacing, 0);
            this.sineGraphIA.Location = new Point(0, verticalSpacing);
            this.frequencyGraphA.Location = new Point(horizontalSpacing, verticalSpacing);

            this.sineGraphVtB.Location = new Point(0, verticalSpacing * 2);
            this.sineGraphVnB.Location = new Point(horizontalSpacing, verticalSpacing * 2);
            this.sineGraphIB.Location = new Point(0, verticalSpacing * 3);
            this.frequencyGraphB.Location = new Point(horizontalSpacing, verticalSpacing * 3);

            this.sineGraphVtC.Location = new Point(0, verticalSpacing * 4);
            this.sineGraphVnC.Location = new Point(horizontalSpacing, verticalSpacing * 4);
            this.sineGraphIC.Location = new Point(0, verticalSpacing * 5);
            this.frequencyGraphC.Location = new Point(horizontalSpacing, verticalSpacing * 5);
        }

        private void initializeComponents()
        {
            this.sineGraphVtA.BackColor = Color.White;
            this.sineGraphVtA.ScrollEnabled = true;
            this.sineGraphVtA.AddNewSineWave(PhasorTypes.VtA, _arraySize);
            this.sineGraphVtA.GraphName = "VtA";
            this.sineGraphVtA.ShowEventLine = false;
            this.sineGraphVtA.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVtA.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVtA.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphClicked);
            this.sineGraphVtA.DoubleClick += new EventHandler(sineGraph_DoubleClick);
            this.sineGraphVtA.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphVtB.BackColor = Color.White;
            this.sineGraphVtB.ScrollEnabled = true;
            this.sineGraphVtB.AddNewSineWave(PhasorTypes.VtB, _arraySize);
            this.sineGraphVtB.GraphName = "VtB";
            this.sineGraphVtB.ShowEventLine = false;
            this.sineGraphVtB.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVtB.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVtB.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphClicked);
            this.sineGraphVtB.DoubleClick += new EventHandler(sineGraph_DoubleClick);
            this.sineGraphVtB.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphVtC.BackColor = Color.White;
            this.sineGraphVtC.ScrollEnabled = true;
            this.sineGraphVtC.AddNewSineWave(PhasorTypes.VtC, _arraySize);
            this.sineGraphVtC.GraphName = "VtC";
            this.sineGraphVtC.ShowEventLine = false;
            this.sineGraphVtC.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVtC.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVtC.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphClicked);
            this.sineGraphVtC.DoubleClick += new EventHandler(sineGraph_DoubleClick);
            this.sineGraphVtC.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphVnA.BackColor = Color.White;
            this.sineGraphVnA.ScrollEnabled = true;
            this.sineGraphVnA.AddNewSineWave(PhasorTypes.VnA, _arraySize);
            this.sineGraphVnA.GraphName = "VnA";
            this.sineGraphVnA.ShowEventLine = false;
            this.sineGraphVnA.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVnA.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVnA.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphClicked);
            this.sineGraphVnA.DoubleClick += new EventHandler(sineGraph_DoubleClick);
            this.sineGraphVnA.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphVnB.BackColor = Color.White;
            this.sineGraphVnB.ScrollEnabled = true;
            this.sineGraphVnB.AddNewSineWave(PhasorTypes.VnB, _arraySize);
            this.sineGraphVnB.GraphName = "VnB";
            this.sineGraphVnB.ShowEventLine = false;
            this.sineGraphVnB.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVnB.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVnB.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphClicked);
            this.sineGraphVnB.DoubleClick += new EventHandler(sineGraph_DoubleClick);
            this.sineGraphVnB.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphVnC.BackColor = Color.White;
            this.sineGraphVnC.ScrollEnabled = true;
            this.sineGraphVnC.AddNewSineWave(PhasorTypes.VnC, _arraySize);
            this.sineGraphVnC.GraphName = "VnC";
            this.sineGraphVnC.ShowEventLine = false;
            this.sineGraphVnC.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphVnC.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphVnC.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphClicked);
            this.sineGraphVnC.DoubleClick += new EventHandler(sineGraph_DoubleClick);
            this.sineGraphVnC.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphIA.BackColor = Color.White;
            this.sineGraphIA.ScrollEnabled = true;
            this.sineGraphIA.AddNewSineWave(PhasorTypes.IA, _arraySize);
            this.sineGraphIA.GraphName = "IA";
            this.sineGraphIA.ShowEventLine = false;
            this.sineGraphIA.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphIA.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphIA.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphClicked);
            this.sineGraphIA.DoubleClick += new EventHandler(sineGraph_DoubleClick);
            this.sineGraphIA.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphIB.BackColor = Color.White;
            this.sineGraphIB.ScrollEnabled = true;
            this.sineGraphIB.AddNewSineWave(PhasorTypes.IB, _arraySize);
            this.sineGraphIB.GraphName = "IB";
            this.sineGraphIB.ShowEventLine = false;
            this.sineGraphIB.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphIB.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphIB.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphClicked);
            this.sineGraphIB.DoubleClick += new EventHandler(sineGraph_DoubleClick);
            this.sineGraphIB.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);

            this.sineGraphIC.BackColor = Color.White;
            this.sineGraphIC.ScrollEnabled = true;
            this.sineGraphIC.AddNewSineWave(PhasorTypes.IC, _arraySize);
            this.sineGraphIC.GraphName = "IC";
            this.sineGraphIC.ShowEventLine = false;
            this.sineGraphIC.MouseWheeledEvent += new SineGraph.MouseWheeledHandler(sineGraph_MouseWheeledEvent);
            this.sineGraphIC.ScrollEvent += new SineGraph.ScrollEventHandler(sineGraph_ScrollEvent);
            this.sineGraphIC.GraphLeftClicked += new SineGraph.GraphLeftClickedHandler(graphClicked);
            this.sineGraphIC.DoubleClick += new EventHandler(sineGraph_DoubleClick);
            this.sineGraphIC.GraphRightClicked += new SineGraph.GraphRightClickedHandler(graphRightClicked);
        }

        #region Setting Data

        delegate void SetAllCallBack(byte[] bytePacket);

        /// <summary>
        /// Handles Individual Incoming Packets
        /// </summary>
        /// <param name="bytePacket">Single Live Event Data Packet</param>
        public void PacketHandler(byte[] bytePacket)
        {
            if (this.InvokeRequired)
            {
                SetAllCallBack sACB = new SetAllCallBack(this.packetHandler);

                Invoke(sACB, new object[] { bytePacket });
            }
            else
            {
                this.packetHandler(bytePacket);
            }

        }

        private int savedIndexOffset = 0;
        /// <summary>
        /// Actually Handling of Packet
        /// </summary>
        /// <param name="bytePacket">Single Live Event Data Packet</param>
        private void packetHandler(byte[] bytePacket)
        {
            PhasorTypes phasor;
            SineGraph workingGraph;
            SineWaveDefinition workingSineWave;
            CalibrationConstant workingCalConstant;
            PacketHandledEventArgs pHEA = new PacketHandledEventArgs();

            int indexOffset;

            try
            {
                phasor = RelayModeFunctions.PhasorTypeFrom((char)bytePacket[1], (char)bytePacket[2]);

                switch (phasor)  //Select proper graph and CalConstant
                {
                    case PhasorTypes.IA:
                        workingGraph = this.sineGraphIA;
                        workingCalConstant = this.CalConstantIA;
                        break;
                    case PhasorTypes.IB:
                        workingGraph = this.sineGraphIB;
                        workingCalConstant = this.CalConstantIB;
                        break;
                    case PhasorTypes.IC:
                        workingGraph = this.sineGraphIC;
                        workingCalConstant = this.CalConstantIC;
                        break;
                    case PhasorTypes.VtA:
                        workingGraph = this.sineGraphVtA;
                        workingCalConstant = this.CalConstantVtA;
                        break;
                    case PhasorTypes.VtB:
                        workingGraph = this.sineGraphVtB;
                        workingCalConstant = this.CalConstantVtB;
                        break;
                    case PhasorTypes.VtC:
                        workingGraph = this.sineGraphVtC;
                        workingCalConstant = this.CalConstantVtC;
                        break;
                    case PhasorTypes.VnA:
                        workingGraph = this.sineGraphVnA;
                        workingCalConstant = this.CalConstantVnA;
                        break;
                    case PhasorTypes.VnB:
                        workingGraph = this.sineGraphVnB;
                        workingCalConstant = this.CalConstantVnB;
                        break;
                    case PhasorTypes.VnC:
                        workingGraph = this.sineGraphVnC;
                        workingCalConstant = this.CalConstantVnC;
                        break;
                    default:
                        throw new Exception(bytePacket[1].ToString() + " & " + bytePacket[2].ToString() + " are bad phasor numbers");
                }

                workingSineWave = (SineWaveDefinition)workingGraph.sineWavesToDraw[0];
                workingSineWave.Enabled = true;
                indexOffset = this.getIndexOffset(bytePacket[3], bytePacket[4]);

                if (indexOffset == 0 && phasor == PhasorTypes.VnA)
                {
                    this.savedIndexOffset = indexOffset;
                }
                else if (indexOffset - 64 != this.savedIndexOffset && indexOffset != this.savedIndexOffset)
                {
                    throw new Exception();
                }
                else
                {
                    this.savedIndexOffset = indexOffset;
                }
                //Slightly offset because data starts later and i += 2 because 2 bytes used per calculation
                for (int i = 5; i < 132; i += 2)
                {
                    float temp;

                    if (phasor == PhasorTypes.IA || phasor == PhasorTypes.IB || phasor == PhasorTypes.IC)
                        temp = this.dACReadingI(bytePacket[i], bytePacket[i + 1], workingCalConstant);
                    else
                        temp = this.dACReadingV(bytePacket[i], bytePacket[i + 1], workingCalConstant);

                    workingSineWave.AddValueNew(temp, indexOffset + (i - 5) / 2);
                }

                pHEA.Successful = true;
                if (indexOffset == 8128)
                {
                    workingGraph.Invalidate();
                    this.savedIndexOffset = 0;
                    if (phasor == PhasorTypes.IA || phasor == PhasorTypes.IB || phasor == PhasorTypes.IC)
                    {
                        workingSineWave.Zero();
                    }

                    if (phasor == PhasorTypes.IC)
                    {
                        this.Done();
                        this.Saveable = true;
                    }
                    else
                    {
                        this.packetHandled(pHEA);
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
                pHEA.Successful = false;
                this.packetHandled(pHEA);
            }
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
            cCEA.EventNumber = 9999;//this.EventNumber;  Make 9999 live data

            if (PopulatePhasorGraph != null)
                PopulatePhasorGraph(this, cCEA);
        }

        public delegate void DownloadCompleteHandler();
        public event DownloadCompleteHandler DownloadComplete;

        private void Done()
        {
            if (DownloadComplete != null)
                this.DownloadComplete();
        }

        public delegate void PacketHandledHandler(object sender, PacketHandledEventArgs e);
        public event PacketHandledHandler PacketHandled;

        private void packetHandled(PacketHandledEventArgs e)
        {
            if (PacketHandled != null)
                PacketHandled(this, e);
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

        private int getIndexOffset(byte cycleNumber, byte half)
        {
            int returnInt;

            if (cycleNumber >= 65)
                throw new Exception(cycleNumber.ToString() + " is a bad cycle number.");

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
                throw new Exception(half.ToString() + " is a bad half cycle character");
            }
        }

        #endregion

        public delegate void ErrorHandler(object o, ExceptionEventArgs eEA);
        public event ErrorHandler Error;

        private void errorHandler(Exception ex, string p)
        {
            if (Error == null)
                throw new Exception("No Handler for Live Data Errors");

            Error(this, new ExceptionEventArgs(ex, p));
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

        private void sineGraph_MouseWheeledEvent(object sender, MouseEventArgs e)
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

        private void sineGraph_ScrollEvent(object sender, ScrollEventArgs e)
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

        public bool Saveable = false;
        private void graphClicked(object sender, EventArgs e)
        {
            SineGraph sGraph = (SineGraph)sender;
            if (!this.Saveable)
            {
                this.errorHandler(new Exception("Need all data to do an FFT"), "Please Get All Data");
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

        private void setFrequencyGraph(FrequencyGraph fG, SineGraph sG)
        {
            SineWaveDefinition sWD = (SineWaveDefinition)sG.sineWavesToDraw[0];
            float[] tempFloat = new float[128 * 8];
            int startIndex = sG.ClickedCycleNumber * 128;

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
            fG.PhasorType = sG.Type;
            fG.SineWave = tempFloat;

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

                switch (pT)
                {
                    case PhasorTypes.IA:
                        sWD = (SineWaveDefinition)this.sineGraphIA.sineWavesToDraw[0];
                        break;
                    case PhasorTypes.IB:
                        sWD = (SineWaveDefinition)this.sineGraphIB.sineWavesToDraw[0];
                        break;
                    case PhasorTypes.IC:
                        sWD = (SineWaveDefinition)this.sineGraphIC.sineWavesToDraw[0];
                        break;
                    case PhasorTypes.VnA:
                        sWD = (SineWaveDefinition)this.sineGraphVnA.sineWavesToDraw[0];
                        break;
                    case PhasorTypes.VnB:
                        sWD = (SineWaveDefinition)this.sineGraphVnB.sineWavesToDraw[0];
                        break;
                    case PhasorTypes.VnC:
                        sWD = (SineWaveDefinition)this.sineGraphVnC.sineWavesToDraw[0];
                        break;
                    case PhasorTypes.VtA:
                        sWD = (SineWaveDefinition)this.sineGraphVtA.sineWavesToDraw[0];
                        break;
                    case PhasorTypes.VtB:
                        sWD = (SineWaveDefinition)this.sineGraphVtB.sineWavesToDraw[0];
                        break;
                    case PhasorTypes.VtC:
                        sWD = (SineWaveDefinition)this.sineGraphVtC.sineWavesToDraw[0];
                        break;
                    default:
                        throw new Exception("Bad Phasor Type");
                }

                //sWD.ClearAllValues();

                sWD.Enabled = true;

                for (int i = 0; i < f.Length - 1; ++i)
                {
                    sWD.AddValueNew(f[i], i);
                }
            }
            catch //(Exception ex)
            {
                this.errorHandler(new Exception("Error Setting Sine Wave"), "Error Setting Sine Wave Live Data");
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

        private void sineGraph_DoubleClick(object sender, EventArgs e)
        {
            SineGraph sG = (SineGraph)sender;

            if (sFP != null)
            {
                sFP.BringToFront();
                return;
            }

            sFP = new SineFrequencyPopup((SineWaveDefinition)sG.sineWavesToDraw[0], this.CTRatio);
            sFP.Disposed += new EventHandler(sFP_Disposed);
            sFP.Show();
        }

        void sFP_Disposed(object sender, EventArgs e)
        {
            if (this.sFP != null)
                this.sFP = null;
        }

        private void setProtectorValue()
        {
            this.sineGraphIA.ProtectorVoltage = protectorVoltage;
            this.sineGraphIB.ProtectorVoltage = protectorVoltage;
            this.sineGraphIC.ProtectorVoltage = protectorVoltage;

            this.sineGraphVnA.ProtectorVoltage = protectorVoltage;
            this.sineGraphVnB.ProtectorVoltage = protectorVoltage;
            this.sineGraphVnC.ProtectorVoltage = protectorVoltage;
            this.sineGraphVtA.ProtectorVoltage = protectorVoltage;
            this.sineGraphVtB.ProtectorVoltage = protectorVoltage;
            this.sineGraphVtC.ProtectorVoltage = protectorVoltage;

            this.frequencyGraphA.ProtectorVoltage = protectorVoltage;
            this.frequencyGraphB.ProtectorVoltage = protectorVoltage;
            this.frequencyGraphC.ProtectorVoltage = protectorVoltage;
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
            cCEA.EventNumber = 9999;

            if (PopulatePhasorGraph != null)
                PopulatePhasorGraph(this, cCEA);
        }
    }
}
