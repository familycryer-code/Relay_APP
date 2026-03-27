using RelayControlLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SineDisplayGraph
{
    public partial class ucPhasorGraph : UserControl
    {
        private bool gEEnabled = false;
        private bool GEEnabled
        {
            get { return this.gEEnabled; }
            set
            {
                this.gEEnabled = value;
                this.makeGE(value);
            }
        }

        public ucPhasorGraph()
        {

            InitializeComponent();
            this.myInitialize();
            this.makeNonConEd();

        }
        public int CTRatio = 320;
        public ProtectorVoltage ProtectorVoltage =
            ProtectorVoltages.GetVoltage();

        public uint RevisionNumber
        {
            get { return this.revisionNumber; }
            set
            {
                this.revisionNumber = value;
                if (this.revisionNumber < 20100617)
                {
                    //this.labelRealValue.Visible = false;
                    this.labelVdAReal.Visible = false;
                    this.labelVdBReal.Visible = false;
                    this.labelVdCReal.Visible = false;
                    //this.labelVdTReal.Visible = false;
                    this.textBoxVdAReal.Visible = false;
                    this.textBoxVdBReal.Visible = false;
                    this.textBoxVdCReal.Visible = false;
                    //this.textBoxVdTReal.Visible = false;
                }
                else
                {
                    this.labelVdAReal.Visible = true;
                    this.labelVdBReal.Visible = true;
                    this.labelVdCReal.Visible = true;
                    this.textBoxVdAReal.Visible = true;
                    this.textBoxVdBReal.Visible = true;
                    this.textBoxVdCReal.Visible = true;
                }
            }
        }

        private uint revisionNumber;
        private PhasorDefinition[] trippedPhasors = new PhasorDefinition[6];
        private PhasorDefinition[] closePhasors = new PhasorDefinition[6];
        private PhasorDefinition[] powerPhasors = new PhasorDefinition[4];
        private PhasorDefinition[] ieffPhasors = new PhasorDefinition[1];
        private PhasorDefinition[] differentialVoltages = new PhasorDefinition[4];
        private PhasorDefinition[] sequenceTrippedPhasors = new PhasorDefinition[2];
        private PhasorDefinition[] sequenceClosePhasors = new PhasorDefinition[4];

        private PhasorDefinition[] allPhasors;

        private ArrayList graph1Labels = new ArrayList();
        private ArrayList graph2Labels = new ArrayList();

        private string[] VoltageDisplayValues = new string[4];
        private string[] NoValues = new string[4] { "", "", "", "" };
        private string[] PowerDisplayValues = new string[4];
        private string[] CurrentDisplayValues = new string[4];
        private string[] CurrentSequenceDisplayValues = new string[4];
        private string[] IeffDisplayValues = new string[4];
        private string[] VdDisplayValues = new string[4];
        private string[] VdSeqDisplayValues = new string[4];
        private string[] VoltageSequenceValues = new string[4];
        private readonly int _cycleCount = 128;
        private readonly float _amplitude = 125f * (float)Math.Sqrt(2); // amplitude = sqrt(2) * rms

        private void myInitialize()
        {
            Array temp = Enum.GetValues(typeof(PhasorTypes));
            int count = 0;

            // Relocate the voltage, current, power  labels, text and boxes as per new design for PQ Monitor tab and Tahoma font
            this.labelRealValue.Location = new System.Drawing.Point(1420, 16);

            this.textBoxIEffReal.Location = new System.Drawing.Point(1400, 583);
            this.labelIEffRealUnits.Location = new System.Drawing.Point(1470, 586);

            this.textBoxICReal.Location = new System.Drawing.Point(1400, 553);
            this.labelICRealUnits.Location = new System.Drawing.Point(1470, 556); 

            this.textBoxIBReal.Location = new System.Drawing.Point(1400, 523);
            this.labelIBRealUnits.Location = new System.Drawing.Point(1470, 526);

            this.textBoxIAReal.Location = new System.Drawing.Point(1400, 493);
            this.labelIARealUnits.Location = new System.Drawing.Point(1470, 496);

            this.textBoxVdAReal.Location = new System.Drawing.Point(1400, 193);
            this.labelVdAReal.Location = new System.Drawing.Point(1470, 197);

            this.textBoxVdBReal.Location = new System.Drawing.Point(1400, 223);
            this.labelVdBReal.Location = new System.Drawing.Point(1470, 227);

            this.textBoxVdCReal.Location = new System.Drawing.Point(1400, 253);
            this.labelVdCReal.Location = new System.Drawing.Point(1470, 257);

            this.textBoxVdTReal.Location = new System.Drawing.Point(1400, 283);
            this.labelVdTReal.Location = new System.Drawing.Point(1470, 286);

            generateRefWav();
            foreach (PhasorTypes pT in temp)
            {
                ++count;
            }

            allPhasors = new PhasorDefinition[count];
            count = 0;

            foreach (PhasorTypes pT in temp)
            {
                allPhasors[count] = new PhasorDefinition(pT);
                switch (allPhasors[count].Type)
                {
                    //
                    case PhasorTypes.VtA:
                        trippedPhasors[0] = allPhasors[count];
                        this.labelVtAColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVtARMS;
                        allPhasors[count].AngleBox = this.textBoxVtAAngle;
                        VtA.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VtB:
                        trippedPhasors[1] = allPhasors[count];
                        this.labelVtBColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVtBRMS;
                        allPhasors[count].AngleBox = this.textBoxVtBAngle;
                        VtB.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VtC:
                        trippedPhasors[2] = allPhasors[count];
                        this.labelVtCColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVtCRMS;
                        allPhasors[count].AngleBox = this.textBoxVtCAngle;
                        VtC.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VnA:
                        trippedPhasors[3] = allPhasors[count];
                        closePhasors[0] = allPhasors[count];
                        this.labelVnAColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVnARMS;
                        allPhasors[count].AngleBox = this.textBoxVnAAngle;
                        VnA.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VnB:
                        trippedPhasors[4] = allPhasors[count];
                        closePhasors[1] = allPhasors[count];
                        this.labelVnBColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVnBRMS;
                        allPhasors[count].AngleBox = this.textBoxVnBAngle;
                        VnB.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VnC:
                        trippedPhasors[5] = allPhasors[count];
                        closePhasors[2] = allPhasors[count];
                        this.labelVnCColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVnCRMS;
                        allPhasors[count].AngleBox = this.textBoxVnCAngle;
                        VnC.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VdA:
                        differentialVoltages[0] = allPhasors[count];
                        this.labelVdAColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVdARMS;
                        allPhasors[count].RealBox = this.textBoxVdAReal;
                        allPhasors[count].AngleBox = this.textBoxVdAAngle;
                        VdA.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VdB:
                        differentialVoltages[1] = allPhasors[count];
                        this.labelVdBColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVdBRMS;
                        allPhasors[count].RealBox = this.textBoxVdBReal;
                        allPhasors[count].AngleBox = this.textBoxVdBAngle;
                        VdB.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VdC:
                        differentialVoltages[2] = allPhasors[count];
                        this.labelVdCColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVdCRMS;
                        allPhasors[count].RealBox = this.textBoxVdCReal;
                        allPhasors[count].AngleBox = this.textBoxVdCAngle;
                        VdC.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VdT:
                        differentialVoltages[3] = allPhasors[count];
                        this.labelVdTColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVdTRMS;
                        allPhasors[count].RealBox = this.textBoxVdTReal;
                        allPhasors[count].AngleBox = this.textBoxVdTAngle;
                        VdAvg.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VdN:
                        sequenceTrippedPhasors[0] = allPhasors[count];
                        this.labelVdNColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVdNRMS;
                        allPhasors[count].AngleBox = this.textBoxVdNAngle;
                        VdN.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VdP:
                        sequenceTrippedPhasors[1] = allPhasors[count];
                        this.labelVdPColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVdPRMS;
                        allPhasors[count].AngleBox = this.textBoxVdPAngle;
                        VdP.PD = allPhasors[count];
                        break;
                    case PhasorTypes.IA:
                        closePhasors[3] = allPhasors[count];
                        this.labelIAColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxIARMS;
                        allPhasors[count].RealBox = this.textBoxIAReal;
                        allPhasors[count].AngleBox = this.textBoxIAAngle;
                        IA.PD = allPhasors[count];
                        break;
                    case PhasorTypes.IB:
                        closePhasors[4] = allPhasors[count];
                        this.labelIBColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxIBRMS;
                        allPhasors[count].RealBox = this.textBoxIBReal;
                        allPhasors[count].AngleBox = this.textBoxIBAngle;
                        IB.PD = allPhasors[count];
                        break;
                    case PhasorTypes.IC:
                        closePhasors[5] = allPhasors[count];
                        this.labelICColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxICRMS;
                        allPhasors[count].RealBox = this.textBoxICReal;
                        allPhasors[count].AngleBox = this.textBoxICAngle;
                        IC.PD = allPhasors[count];
                        break;
                    case PhasorTypes.IN:
                        sequenceClosePhasors[2] = allPhasors[count];
                        this.labelINColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxINRMS;
                        allPhasors[count].AngleBox = this.textBoxINAngle;
                        IN.PD = allPhasors[count];
                        break;
                    case PhasorTypes.IP:
                        sequenceClosePhasors[3] = allPhasors[count];
                        this.labelIPColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxIPRMS;
                        allPhasors[count].AngleBox = this.textBoxIPAngle;
                        IP.PD = allPhasors[count];
                        break;
                    case PhasorTypes.Ieff:
                        ieffPhasors[0] = allPhasors[count];
                        this.labelIeffColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxIEffRMS;
                        allPhasors[count].RealBox = this.textBoxIEffReal;
                        allPhasors[count].AngleBox = this.textBoxIEffAngle;
                        Ieff.PD = allPhasors[count];
                        break;
                    case PhasorTypes.PA:
                        powerPhasors[0] = allPhasors[count];
                        this.labelPAColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxPARMS;
                        allPhasors[count].AngleBox = this.textBoxPAAngle;
                        PA.PD = allPhasors[count];
                        break;
                    case PhasorTypes.PB:
                        powerPhasors[1] = allPhasors[count];
                        this.labelPBColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxPBRMS;
                        allPhasors[count].AngleBox = this.textBoxPBAngle;
                        PB.PD = allPhasors[count];
                        break;
                    case PhasorTypes.PC:
                        powerPhasors[2] = allPhasors[count];
                        this.labelPCColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxPCRMS;
                        allPhasors[count].AngleBox = this.textBoxPCAngle;
                        PC.PD = allPhasors[count];
                        break;
                    case PhasorTypes.PT:
                        powerPhasors[3] = allPhasors[count];
                        this.labelPTColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxPTRMS;
                        allPhasors[count].AngleBox = this.textBoxPTAngle;
                        PAvg.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VtP:
                        //sequenceClosePhasors[0] = allPhasors[count];
                        //this.labelVtPColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVtPRMS;
                        allPhasors[count].AngleBox = this.textBoxVtPAngle;
                        VtP.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VtN:
                        //sequenceClosePhasors[1] = allPhasors[count];
                        //this.labelVtNColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVtNRMS;
                        allPhasors[count].AngleBox = this.textBoxVtNAngle;
                        VtN.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VnP:
                        sequenceClosePhasors[0] = allPhasors[count];
                        this.labelVnPColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVnPRMS;
                        allPhasors[count].AngleBox = this.textBoxVnPAngle;
                        VnP.PD = allPhasors[count];
                        break;
                    case PhasorTypes.VnN:
                        sequenceClosePhasors[1] = allPhasors[count];
                        this.labelVnNColor.BackColor = allPhasors[count].phasorPen.Color;
                        allPhasors[count].RMSBox = this.textBoxVnNRMS;
                        allPhasors[count].AngleBox = this.textBoxVnNAngle;
                        VnN.PD = allPhasors[count];
                        break;
                    case PhasorTypes.None:
                    default:
                        break;
                }
                count++;
            }

            trippedPhasors[0].EndPoint = new PointF(1, 2);
            string[] str = Enum.GetNames(typeof(RawPhasorGroups));
           /* foreach (string s in str)
            {
                this.listBoxMode.Items.Add(s);
            }
            */
            this.listBoxMode.SelectedIndex = 0;
            this.textBoxCTRatio.Visible = true;
        }

        private void makeGE(bool b)
        {
#if DEBUG
            b = false; //We want to see all phasors debugging
#endif
            this.labelVtA.Visible = !b;
            this.labelVtAColor.Visible = !b;
            this.labelVtAUnits.Visible = !b;
            this.textBoxVtAAngle.Visible = !b;
            this.textBoxVtARMS.Visible = !b;
            this.labelVtAClosed.Visible = !b;

            this.labelVtB.Visible = !b;
            this.labelVtBColor.Visible = !b;
            this.labelVtBUnits.Visible = !b;
            this.textBoxVtBAngle.Visible = !b;
            this.textBoxVtBRMS.Visible = !b;
            this.labelVtBClosed.Visible = !b;

            this.labelVtC.Visible = !b;
            this.labelVtCColor.Visible = !b;
            this.labelVtCUnits.Visible = !b;
            this.textBoxVtCAngle.Visible = !b;
            this.textBoxVtCRMS.Visible = !b;
            this.labelVtCClosed.Visible = !b;

            this.labelVtNStupid.Visible = !b;
            this.labelVtNUnits.Visible = !b;
            this.textBoxVtNAngle.Visible = !b;
            this.textBoxVtNRMS.Visible = !b;

            this.labelVtP.Visible = !b;
            this.labelVtPUnits.Visible = !b;
            this.textBoxVtPAngle.Visible = !b;
            //this.labelVtPStupid.Visible = !b;
            this.labelVtPUnits.Visible = !b;
            this.textBoxVtPRMS.Visible = !b;

            //Transformer Phasors A, B C
            if (this.trippedPhasors[0] != null)
            {
                this.trippedPhasors[0].Enabled = !b;
                this.trippedPhasors[1].Enabled = !b;
                this.trippedPhasors[2].Enabled = !b;
            }

        }

        private void makeConEd()
        {
            this.labelIP.Visible = false;
            this.textBoxIPAngle.Visible = false;
            this.textBoxIPRMS.Visible = false;
            this.labelIPUnits.Visible = false;

            this.labelIN.Visible = false;
            this.textBoxINAngle.Visible = false;
            this.textBoxINRMS.Visible = false;
            this.labelINUnits.Visible = false;

            this.labelVdP.Visible = false;
            this.textBoxVdPAngle.Visible = false;
            this.textBoxVdPRMS.Visible = false;
            this.labelVdPUnits.Visible = false;

            this.labelVdN.Visible = false;
            this.textBoxVdNAngle.Visible = false;
            this.textBoxVdNRMS.Visible = false;
            this.labelVdNUnits.Visible = false;

            this.labelVtP.Visible = false;
            this.textBoxVtPAngle.Visible = false;
            this.textBoxVtPRMS.Visible = false;
            this.labelVtPUnits.Visible = false;


            this.labelVtNStupid.Visible = false;
            this.textBoxVtNAngle.Visible = false;
            this.textBoxVtNRMS.Visible = false;
            this.labelVtNUnits.Visible = false;

            this.labelVnP.Visible = false;
            this.textBoxVnPAngle.Visible = false;
            this.textBoxVnPRMS.Visible = false;
            this.labelVnPUnits.Visible = false;

            this.labelVnN.Visible = false;
            this.textBoxVnNAngle.Visible = false;
            this.textBoxVnNRMS.Visible = false;
            this.labelVnNUnits.Visible = false;

            this.groupBoxTHD.Visible = false;

            this.listBoxSequencePower.Items.Clear();
            this.listBoxSequencePower.Items.Add("Power Phasors");
            this.listBoxSequencePower.Items.Add("Effective Current");
            this.listBoxSequencePower.Items.Add("Differential Voltages");
            this.listBoxSequencePower.SelectedIndex = 0;
        }

        private void makeNonConEd()
        {
            this.labelIP.Visible = true;
            this.textBoxIPAngle.Visible = true;
            this.textBoxIPRMS.Visible = true;
            this.labelIPUnits.Visible = true;

            this.labelIN.Visible = true;
            this.textBoxINAngle.Visible = true;
            this.textBoxINRMS.Visible = true;
            this.labelINUnits.Visible = true;

            this.labelVdP.Visible = true;
            this.textBoxVdPAngle.Visible = true;
            this.textBoxVdPRMS.Visible = true;
            this.labelVdPUnits.Visible = true;

            this.labelVdN.Visible = true;
            this.textBoxVdNAngle.Visible = true;
            this.textBoxVdNRMS.Visible = true;
            this.labelVdNUnits.Visible = true;

            this.labelVtP.Visible = true;
            this.textBoxVtPAngle.Visible = true;
            this.textBoxVtPRMS.Visible = true;
            this.labelVtPUnits.Visible = true;

            this.labelVtNStupid.Visible = true;
            this.textBoxVtNAngle.Visible = true;
            this.textBoxVtNRMS.Visible = true;
            this.labelVtNUnits.Visible = true;

            this.labelVnP.Visible = true;
            this.textBoxVnPAngle.Visible = true;
            this.textBoxVnPRMS.Visible = true;
            this.labelVnPUnits.Visible = true;

            this.labelVnN.Visible = true;
            this.textBoxVnNAngle.Visible = true;
            this.textBoxVnNRMS.Visible = true;
            this.labelVnNUnits.Visible = true;

            this.groupBoxTHD.Visible = true;

            this.listBoxSequencePower.Items.Clear();
            this.listBoxSequencePower.Items.Add("Power Phasors");
            this.listBoxSequencePower.Items.Add("Sequence Phasors");
            this.listBoxSequencePower.Items.Add("Effective Current");
            this.listBoxSequencePower.Items.Add("Differential Voltages");
            this.listBoxSequencePower.Items.Add("Diff Sequence Voltages");
            this.listBoxSequencePower.SelectedIndex = 0;
        }


        private double vnAAngle, vnBAngle, vnCAngle;

        public void ValuesForUpdate(PhasorTypes phasorType, long realValue, long imaginaryValue, int cTRatio, long rMS)
        {
            PointF point;
            PhasorDefinition workingPD;
            double angle;
            Matrix matrix = new Matrix();

            workingPD = new PhasorDefinition();
            foreach (PhasorDefinition pD in allPhasors)
            {
                if (pD.Type == phasorType)
                {
                    workingPD = pD;
                    break;
                }
            }

            switch (phasorType)
            {
                case PhasorTypes.VtA:
                case PhasorTypes.VtB:
                case PhasorTypes.VtC:
                case PhasorTypes.VnA:
                case PhasorTypes.VnB:
                case PhasorTypes.VnC:
                    workingPD.RealValue = this.convertRMSV(realValue);
                    workingPD.ImaginaryValue = this.convertRMSV(imaginaryValue);

                    angle = (double)(workingPD.ImaginaryValue / workingPD.RealValue);
                    angle = RelayControlLibrary.RelayModeFunctions.RadiansToDegrees(Math.Atan(angle));

                    if (workingPD.RealValue < 0)
                    {
                        if (workingPD.ImaginaryValue > 0)
                            angle = 180 + angle;
                        else
                            angle = -180 + angle;
                    }

                    if (phasorType == PhasorTypes.VnA)
                    {
                        this.vnAAngle = angle;
                    }
                    if (phasorType == PhasorTypes.VnB)
                    {
                        this.vnBAngle = angle;
                    }
                    if (phasorType == PhasorTypes.VnC)
                    {
                        this.vnCAngle = angle;
                    }

                    if (workingPD.RMSValue < (float)(ProtectorVoltage.Scaling * 1.5m) && this.CTRatio != 1)
                    {
                        workingPD.Enabled = false;
                        this.setText("0.0", workingPD.AngleBox);
                        this.setText("0.0", workingPD.RMSBox);
                    }
                    else
                    {
                        workingPD.Enabled = true;
                        this.setText(String.Format("{0:0.0}", angle), workingPD.AngleBox);
                        this.setText(String.Format("{0:0.0}", workingPD.RMSValue), workingPD.RMSBox);
                    }


                    if (phasorType == PhasorTypes.VnC)
                    {
                        this.scaleMeasuredVoltages();
                    }
                    break;
                case PhasorTypes.VdA:
                case PhasorTypes.VdB:
                case PhasorTypes.VdC:
                case PhasorTypes.VdT:

                    workingPD.RealValue = this.convertRMSV(realValue);
                    workingPD.ImaginaryValue = this.convertRMSV(imaginaryValue);

                    angle = (double)(workingPD.ImaginaryValue / workingPD.RealValue);
                    angle = RelayControlLibrary.RelayModeFunctions.RadiansToDegrees(Math.Atan(angle));
                    if (workingPD.RealValue < 0)
                    {
                        if (workingPD.ImaginaryValue > 0)
                            angle = 180 + angle;
                        else
                            angle = -180 + angle;
                    }

                    if (this.CTRatio != 1 &&
                        (workingPD.RMSValue < 0.2 * (float)ProtectorVoltage.Scaling &&
                         workingPD.RMSValue > -0.2 * (float)ProtectorVoltage.Scaling)
                        )
                    {
                        workingPD.Enabled = false;

                        this.setText("0.0", workingPD.AngleBox);

                        this.setText(String.Format("{0:0.0}", workingPD.RMSValue), workingPD.RMSBox);
                        this.setText("0.0", workingPD.RealBox);
                    }
                    else
                    {
                        workingPD.Enabled = true;
                        this.setText(String.Format("{0:0.0}", angle), workingPD.AngleBox);
                        this.setText(String.Format("{0:0.0}", workingPD.RMSValue), workingPD.RMSBox);

                        double realValueDisplay;

                        //Using RMS because I stuck the real value of the phasor in the RMS because I never use it

                        if (phasorType != PhasorTypes.VdT)
                        {
                            realValueDisplay = convertRMSV(rMS);
                            this.setText(String.Format("{0:0.0}", Math.Floor(realValueDisplay * 10d) / 10d), workingPD.RealBox);

                            if (realValueDisplay > 0)
                                this.setText(String.Format("{0:0.0}", Math.Floor(realValueDisplay * 10d) / 10d), workingPD.RealBox);
                            else
                                this.setText(String.Format("{0:0.0}", Math.Ceiling(realValueDisplay * 10d) / 10d), workingPD.RealBox);

                        }
                        else
                        {
                            this.setText(String.Format("{0:0.0}", Math.Floor(workingPD.RealValue * 10d) / 10d), workingPD.RealBox);
                        }

                    }

                    if (phasorType == PhasorTypes.VdT)
                    {
                        this.scaleCalculatedVoltages();
                    }
                    break;
                case PhasorTypes.VtN:
                case PhasorTypes.VtP:
                case PhasorTypes.VnP:
                case PhasorTypes.VnN:

                    workingPD.RealValue = this.convertRMSV(realValue);
                    workingPD.ImaginaryValue = this.convertRMSV(imaginaryValue);

                    angle = (double)(workingPD.ImaginaryValue / workingPD.RealValue);
                    angle = RelayControlLibrary.RelayModeFunctions.RadiansToDegrees(Math.Atan(angle));
                    if (workingPD.RealValue < 0)
                    {
                        if (workingPD.ImaginaryValue > 0)
                            angle = 180 + angle;
                        else
                            angle = -180 + angle;
                    }

                    if (this.CTRatio != 1 && (workingPD.RMSValue < (1.5 * (float)ProtectorVoltage.Scaling)))
                    {
                        workingPD.Enabled = false;
                        this.setText("0.0", workingPD.AngleBox);
                        this.setText("0.0", workingPD.RMSBox);
                    }
                    else
                    {
                        workingPD.Enabled = true;
                        this.setText(String.Format("{0:0.0}", angle), workingPD.AngleBox);
                        this.setText(String.Format("{0:0.0}", workingPD.RMSValue), workingPD.RMSBox);
                    }

                    if (phasorType == PhasorTypes.VnN)
                    {
                        this.scaleMeasuredSequenceVoltages();

                        if (panelClosed.Visible)
                            this.phasorGraph1.YAxisValues = this.CurrentDisplayValues;
                        else
                            this.phasorGraph1.YAxisValues = this.VoltageDisplayValues;

                        if (this.panelPower.Visible)
                        {
                            this.phasorGraph2.XAxisValues = this.NoValues;
                            this.phasorGraph2.YAxisValues = this.PowerDisplayValues;
                        }
                        else if (this.panelIeff.Visible)
                        {
                            this.phasorGraph2.XAxisValues = this.NoValues;
                            this.phasorGraph2.YAxisValues = this.IeffDisplayValues;
                        }
                        else if (this.panelDifferentialVoltage.Visible)
                        {
                            this.phasorGraph2.XAxisValues = this.NoValues;
                            this.phasorGraph2.YAxisValues = this.VdDisplayValues;
                        }
                        else if (this.panelDifferentialSquence.Visible)
                        {
                            this.phasorGraph2.XAxisValues = this.NoValues;
                            this.phasorGraph2.YAxisValues = this.VdSeqDisplayValues;
                        }
                        else if (this.panelClosedSequence.Visible)
                        {
                            this.phasorGraph2.XAxisValues = this.VoltageSequenceValues;
                            this.phasorGraph2.YAxisValues = this.CurrentSequenceDisplayValues;
                        }

                        this.phasorGraph1.Invalidate();
                        this.phasorGraph2.Invalidate();

                    }
                    break;
                case PhasorTypes.VdN:
                case PhasorTypes.VdP:
                    workingPD.RealValue = this.convertRMSV(realValue);
                    workingPD.ImaginaryValue = this.convertRMSV(imaginaryValue);

                    angle = (double)(workingPD.ImaginaryValue / workingPD.RealValue);
                    angle = RelayControlLibrary.RelayModeFunctions.RadiansToDegrees(Math.Atan(angle));
                    if (workingPD.RealValue < 0)
                    {
                        if (workingPD.ImaginaryValue > 0)
                            angle = 180 + angle;
                        else
                            angle = -180 + angle;
                    }

                    if (this.CTRatio != 1 && workingPD.RMSValue < 0.2 * (float)ProtectorVoltage.Scaling)
                    {
                        workingPD.Enabled = false;
                        this.setText("0.0", workingPD.AngleBox);
                        this.setText("0.0", workingPD.RMSBox);
                    }
                    else
                    {
                        workingPD.Enabled = true;
                        this.setText(String.Format("{0:0.0}", angle), workingPD.AngleBox);
                        this.setText(String.Format("{0:0.0}", workingPD.RMSValue), workingPD.RMSBox);
                    }
                    if (phasorType == PhasorTypes.VdP)
                    {
                        this.scaleDifferentialSequenceVoltages();
                    }
                    break;
                case PhasorTypes.IA:
                case PhasorTypes.IB:
                case PhasorTypes.IC:
                    workingPD.RealValue = this.convertRMSI(realValue) * this.CTRatio;
                    workingPD.ImaginaryValue = this.convertRMSI(imaginaryValue) * this.CTRatio;

                    angle = (double)(workingPD.ImaginaryValue / workingPD.RealValue);
                    angle = RelayControlLibrary.RelayModeFunctions.RadiansToDegrees(Math.Atan(angle));
                    if (workingPD.RealValue < 0)
                    {
                        if (workingPD.ImaginaryValue > 0)
                            angle = 180 + angle;
                        else
                            angle = -180 + angle;
                    }

                    if (this.CTRatio != 1 && (workingPD.RMSValue / (float)this.CTRatio < .001f))
                    {
                        workingPD.Enabled = false;
                        this.setText("0.0", workingPD.AngleBox);
                        this.setText("0.0", workingPD.RMSBox);
                        this.setText("0.0", workingPD.RealBox);
                    }
                    else
                    {
                        PhasorDefinition voltagePD;

                        switch (workingPD.Type)
                        {
                            case PhasorTypes.IA:
                            default:
                                voltagePD = this.allPhasors[3];
                                break;
                            case PhasorTypes.IB:
                                voltagePD = this.allPhasors[4];
                                break;
                            case PhasorTypes.IC:
                                voltagePD = this.allPhasors[5];
                                break;
                        }
                        workingPD.Enabled = true;
                        this.setText(String.Format("{0:0.0}", angle), workingPD.AngleBox);

                        if (CTRatio == 1)
                        {
                            this.setText(String.Format("{0:0.0000}", workingPD.RMSValue), workingPD.RMSBox);
                            this.setText(String.Format("{0:0.000}", this.getInPhaseValue(workingPD, voltagePD)), workingPD.RealBox);
                        }
                        else
                        {
                            this.setText(String.Format("{0:0.0}", workingPD.RMSValue), workingPD.RMSBox);
                            this.setText(String.Format("{0:0.0}", this.getInPhaseValue(workingPD, voltagePD)), workingPD.RealBox);
                        }


                    }

                    if (phasorType == PhasorTypes.IC)
                    {
                        this.scaleCurrents();
                    }
                    break;
                case PhasorTypes.IN:
                case PhasorTypes.IP:
                    workingPD.RealValue = this.convertRMSI(realValue) * this.CTRatio;
                    workingPD.ImaginaryValue = this.convertRMSI(imaginaryValue) * this.CTRatio;

                    angle = (double)(workingPD.ImaginaryValue / workingPD.RealValue);
                    angle = RelayControlLibrary.RelayModeFunctions.RadiansToDegrees(Math.Atan(angle));
                    if (workingPD.RealValue < 0)
                    {
                        if (workingPD.ImaginaryValue > 0)
                            angle = 180 + angle;
                        else
                            angle = -180 + angle;
                    }

                    if (workingPD.RMSValue / (float)this.CTRatio < .001f)//.0171f)
                    {
                        workingPD.Enabled = false;
                        this.setText("0.0", workingPD.AngleBox);
                        this.setText("0.0", workingPD.RMSBox);
                    }
                    else
                    {
                        workingPD.Enabled = true;
                        this.setText(String.Format("{0:0.0}", angle), workingPD.AngleBox);

                        if (CTRatio == 1)
                            this.setText(String.Format("{0:0.0000}", workingPD.RMSValue), workingPD.RMSBox);
                        else
                            this.setText(String.Format("{0:0.0}", workingPD.RMSValue), workingPD.RMSBox);
                    }

                    if (phasorType == PhasorTypes.IP)
                    {
                        this.scaleSequenceCurrents();
                    }
                    break;
                case PhasorTypes.Ieff:
                    workingPD.RealValue = this.convertRMSI(realValue) * this.CTRatio;
                    workingPD.ImaginaryValue = this.convertRMSI(imaginaryValue) * this.CTRatio;

                    angle = (double)(workingPD.ImaginaryValue / workingPD.RealValue);
                    angle = RelayControlLibrary.RelayModeFunctions.RadiansToDegrees(Math.Atan(angle));
                    if (workingPD.RealValue < 0)
                    {
                        if (workingPD.ImaginaryValue > 0)
                            angle = 180 + angle;
                        else
                            angle = -180 + angle;
                    }

                    if (workingPD.RMSValue / (float)this.CTRatio < .001f)//.0171f)
                    {
                        workingPD.Enabled = false;
                        this.setText("0.0", workingPD.AngleBox);
                        this.setText("0.0", workingPD.RMSBox);
                        this.setText("0.0", workingPD.RealBox);
                    }
                    else
                    {
                        workingPD.Enabled = true;
                        this.setText(String.Format("{0:0.0}", angle), workingPD.AngleBox);

                        if (CTRatio == 1)
                        {
                            this.setText(String.Format("{0:0.0000}", workingPD.RMSValue), workingPD.RMSBox);
                            this.setText(String.Format("{0:0.000}", workingPD.RealValue), workingPD.RealBox);
                        }
                        else
                        {
                            this.setText(String.Format("{0:0.0}", workingPD.RMSValue), workingPD.RMSBox);
                            this.setText(String.Format("{0:0.0}", workingPD.RealValue), workingPD.RealBox);
                        }
                    }

                    this.scaleIEff();

                    break;
                case PhasorTypes.PA:
                case PhasorTypes.PB:
                case PhasorTypes.PC:
                case PhasorTypes.PT:
                    workingPD.RealValue = this.convertRMSV(realValue) * this.CTRatio;// * (float)ProtectorVoltage.Scaling;              //V is the same as Power (12 frac bits)
                    workingPD.ImaginaryValue = this.convertRMSV(imaginaryValue) * this.CTRatio;// * (float)ProtectorVoltage.Scaling;
                    workingPD.RMSValue = this.convertRMSV(rMS);
                    angle = (double)(workingPD.ImaginaryValue / workingPD.RealValue);
                    angle = RelayControlLibrary.RelayModeFunctions.RadiansToDegrees(Math.Atan(angle));
                    if (workingPD.RealValue < 0)
                    {
                        if (workingPD.ImaginaryValue > 0)
                            angle = 180 + angle;
                        else
                            angle = -180 + angle;
                    }


                    if (workingPD.RMSValue / (float)ProtectorVoltage.Scaling / (float)this.CTRatio < .012f)//2.052f)
                    {
                        workingPD.Enabled = false;
                        this.setText("0.0", workingPD.AngleBox);
                         this.setText("0.0", workingPD.RMSBox);                        
                    }
                    else
                    {
                        workingPD.Enabled = true;
                        this.setText(String.Format("{0:0.0}", angle), workingPD.AngleBox);
                       // this.setText(String.Format("{0:0.0}", workingPD.RMSValue), workingPD.RMSBox);
                        this.setText(String.Format("{0:0.0}", (workingPD.RMSValue / 1000)), workingPD.RMSBox); // " Divided by 1000 to display the power in kiloWatts"
                    }

                    this.scalePowerPhasors();
                    break;
                case PhasorTypes.None:
                default:
                    point = new PointF(0, 0);
                    break;
            }
        }

        private float getInPhaseValue(PhasorDefinition workingPD, PhasorDefinition voltagePD)
        {
            try
            {
                double voltageAngle = Convert.ToDouble(voltagePD.AngleBox.Text);
                double workingValue = Convert.ToDouble(workingPD.AngleBox.Text);

                workingValue = voltageAngle - workingValue;

                workingValue = workingPD.RMSValue * Math.Cos(RelayModeFunctions.DegreesToRadians(workingValue));

                return (float)workingValue;
            }
            catch
            {
                return 0f;
            }
        }

        public void UpdateTHDValue(PhasorTypes phasorType, double value)
        {
            TextBox tB;
            PhasorDefinition associatedCurrentPD;
            PhasorDefinition associatedVoltagePD;

            switch (phasorType)
            {
                case PhasorTypes.IA:
                    associatedCurrentPD = this.allPhasors[6];
                    associatedVoltagePD = null;
                    tB = this.textBoxIATHD;
                    break;
                case PhasorTypes.IB:
                    associatedCurrentPD = this.allPhasors[7];
                    associatedVoltagePD = null;
                    tB = this.textBoxIBTHD;
                    break;
                case PhasorTypes.IC:
                    associatedCurrentPD = this.allPhasors[8];
                    associatedVoltagePD = null;
                    tB = this.textBoxICTHD;
                    break;
                case PhasorTypes.VnA:
                    associatedCurrentPD = null;
                    associatedVoltagePD = this.allPhasors[3];
                    tB = this.textBoxVnATHD;
                    break;
                case PhasorTypes.VnB:
                    associatedCurrentPD = null;
                    associatedVoltagePD = this.allPhasors[4];
                    tB = this.textBoxVnBTHD;
                    break;
                case PhasorTypes.VnC:
                    associatedCurrentPD = null;
                    associatedVoltagePD = this.allPhasors[5];
                    tB = this.textBoxVnCTHD;
                    break;
                default:
                    throw new Exception("Bad Phasor Type for THD Monitor Value");
            }

            if (associatedCurrentPD != null)
            {
                if (associatedCurrentPD.RMSValue / (float)this.CTRatio > .25f)
                {
                    tB.Text = value.ToString("0.00");
                }
                else
                {
                    tB.Text = "";
                }
            }
            else
            {
                if (associatedVoltagePD.RMSValue > 60.0f)
                    tB.Text = value.ToString("0.00");
                else
                    tB.Text = "";
            }
        }

        private void scaleMeasuredVoltages()
        {
            float maxVoltageRMS = 0;

            foreach (PhasorDefinition pD in trippedPhasors)
            {
                if (pD.RMSValue > maxVoltageRMS)                 //find which phasor has highest RMS value
                {
                    maxVoltageRMS = pD.RMSValue;
                }
            }

            maxVoltageRMS = this.setVoltageLabelsandGetMaxGraphValue(maxVoltageRMS, this.VoltageDisplayValues); //highest value that will display on graph

            foreach (PhasorDefinition pD in trippedPhasors)
            {
                pD.MaxValue = maxVoltageRMS;
                pD.SetEndPoint();
            }
        }

        private float setVoltageLabelsandGetMaxGraphValue(float maxValue, string[] displayValues)
        {
            double hiddenValue, highValue, midHighValue, midLowValue, lowValue;

            if (maxValue > 50)
            {
                maxValue /= 10; //to round to nearest 10;

                highValue = Math.Ceiling((double)maxValue);
                hiddenValue = highValue * 1.25d;

                hiddenValue *= 10d;

                highValue = Math.Round(hiddenValue * .8d);
                midHighValue = Math.Round(hiddenValue * .6d);
                midLowValue = Math.Round(hiddenValue * .4d);
                lowValue = Math.Round(hiddenValue * .2d);

                displayValues[0] = lowValue.ToString();
                displayValues[1] = midLowValue.ToString();
                displayValues[2] = midHighValue.ToString();
                displayValues[3] = highValue.ToString();
            }
            else if (maxValue > 10)
            {
                highValue = Math.Ceiling((double)maxValue);
                hiddenValue = highValue * 1.25d;

                highValue = Math.Round(hiddenValue * .8d);
                midHighValue = Math.Round(hiddenValue * .6d);
                midLowValue = Math.Round(hiddenValue * .4d);
                lowValue = Math.Round(hiddenValue * .2d);

                displayValues[0] = lowValue.ToString();
                displayValues[1] = midLowValue.ToString();
                displayValues[2] = midHighValue.ToString();
                displayValues[3] = highValue.ToString();
            }
            else
            {
                maxValue *= 10;

                highValue = Math.Ceiling((double)maxValue);
                hiddenValue = highValue * 1.25d;

                highValue = Math.Round(hiddenValue * .8d) / 10d;
                midHighValue = Math.Round(hiddenValue * .6d) / 10d;
                midLowValue = Math.Round(hiddenValue * .4d) / 10d;
                lowValue = Math.Round(hiddenValue * .2d) / 10d;

                hiddenValue /= 10d;

                displayValues[0] = lowValue.ToString("0.0");
                displayValues[1] = midLowValue.ToString("0.0");
                displayValues[2] = midHighValue.ToString("0.0");
                displayValues[3] = highValue.ToString("0.0");
            }

            return (float)hiddenValue;
        }

        private void scaleCalculatedVoltages()
        {
            float maxVoltageRMS = 0;

            foreach (PhasorDefinition pD in differentialVoltages)
            {
                if (pD.RMSValue > maxVoltageRMS)                 //find which phasor has highest RMS value
                {
                    maxVoltageRMS = pD.RMSValue;
                }
            }

            maxVoltageRMS = this.setVoltageLabelsandGetMaxGraphValue(maxVoltageRMS, this.VdDisplayValues); //highest value that will display on graph

            foreach (PhasorDefinition pD in differentialVoltages)
            {
                pD.MaxValue = maxVoltageRMS;
                pD.SetEndPoint();
            }
        }


        private void scaleMeasuredSequenceVoltages()
        {
            float maxVoltageRMS = 0;

            foreach (PhasorDefinition pD in sequenceClosePhasors)
            {
                if ((pD.Type == PhasorTypes.VnP || pD.Type == PhasorTypes.VnN) && pD.RMSValue > maxVoltageRMS)                 //find which phasor has highest RMS value
                {
                    maxVoltageRMS = pD.RMSValue;
                }
            }

            maxVoltageRMS = this.setVoltageLabelsandGetMaxGraphValue(maxVoltageRMS, this.VoltageSequenceValues); //highest value that will display on graph

            foreach (PhasorDefinition pD in sequenceClosePhasors)
            {
                if (pD.Type == PhasorTypes.VnP || pD.Type == PhasorTypes.VnN)
                {
                    pD.MaxValue = maxVoltageRMS;
                    pD.SetEndPoint();
                }
            }
        }

        private void scaleDifferentialSequenceVoltages()
        {
            float maxVoltageRMS = 0;

            foreach (PhasorDefinition pD in sequenceTrippedPhasors)
            {
                if (pD.RMSValue > maxVoltageRMS)                 //find which phasor has highest RMS value
                {
                    maxVoltageRMS = pD.RMSValue;
                }
            }

            maxVoltageRMS = this.setVoltageLabelsandGetMaxGraphValue(maxVoltageRMS, this.VdSeqDisplayValues); //highest value that will display on graph

            foreach (PhasorDefinition pD in sequenceTrippedPhasors)
            {
                pD.MaxValue = maxVoltageRMS;
                pD.SetEndPoint();
            }
        }

        private void scaleCurrents()
        {
            float maxCurrentRMS = 0;

            foreach (PhasorDefinition pD in closePhasors)
            {
                if ((pD.Type == PhasorTypes.IA || pD.Type == PhasorTypes.IB || pD.Type == PhasorTypes.IC) && pD.RMSValue > maxCurrentRMS)                 //find which phasor has highest RMS value
                {
                    maxCurrentRMS = pD.RMSValue;
                }
            }

            maxCurrentRMS = this.setCurrentLabelsAndGetMaxGraphValue(maxCurrentRMS, this.CurrentDisplayValues); //highest value that will display on graph

            foreach (PhasorDefinition pD in closePhasors)
            {
                if (pD.Type == PhasorTypes.IA || pD.Type == PhasorTypes.IB || pD.Type == PhasorTypes.IC)
                {
                    pD.MaxValue = maxCurrentRMS;
                    pD.SetEndPoint();
                }
            }
        }

        private float setCurrentLabelsAndGetMaxGraphValue(float maxValue, string[] displayValues)
        {
            double hiddenValue, highValue, midHighValue, midLowValue, lowValue;

            if (maxValue > 1000)
            {
                maxValue /= 100; //to round to nearest 10;

                highValue = Math.Ceiling((double)maxValue);
                hiddenValue = highValue * 1.25d;

                highValue = Math.Round(hiddenValue * .8d) / 10d;
                midHighValue = Math.Round(hiddenValue * .6d) / 10d;
                midLowValue = Math.Round(hiddenValue * .4d) / 10d;
                lowValue = Math.Round(hiddenValue * .2d) / 10d;

                displayValues[0] = lowValue.ToString("0.0") + "k";
                displayValues[1] = midLowValue.ToString("0.0") + "k";
                displayValues[2] = midHighValue.ToString("0.0") + "k";
                displayValues[3] = highValue.ToString("0.0") + "k";

                hiddenValue *= 100;
            }
            else if (maxValue > 50)
            {
                maxValue /= 10; //to round to nearest 10;

                highValue = Math.Ceiling((double)maxValue);
                hiddenValue = highValue * 1.25d;

                hiddenValue *= 10d;

                highValue = Math.Round(hiddenValue * .8d);
                midHighValue = Math.Round(hiddenValue * .6d);
                midLowValue = Math.Round(hiddenValue * .4d);
                lowValue = Math.Round(hiddenValue * .2d);

                displayValues[0] = lowValue.ToString();
                displayValues[1] = midLowValue.ToString();
                displayValues[2] = midHighValue.ToString();
                displayValues[3] = highValue.ToString();
            }
            else if (maxValue > 10)
            {
                highValue = Math.Ceiling((double)maxValue);
                hiddenValue = highValue * 1.25d;

                highValue = Math.Round(hiddenValue * .8d);
                midHighValue = Math.Round(hiddenValue * .6d);
                midLowValue = Math.Round(hiddenValue * .4d);
                lowValue = Math.Round(hiddenValue * .2d);

                displayValues[0] = lowValue.ToString();
                displayValues[1] = midLowValue.ToString();
                displayValues[2] = midHighValue.ToString();
                displayValues[3] = highValue.ToString();
            }
            else if (maxValue > 1)
            {
                maxValue *= 10;

                highValue = Math.Ceiling((double)maxValue);
                hiddenValue = highValue * 1.25d;

                highValue = Math.Round(hiddenValue * .8d) / 10d;
                midHighValue = Math.Round(hiddenValue * .6d) / 10d;
                midLowValue = Math.Round(hiddenValue * .4d) / 10d;
                lowValue = Math.Round(hiddenValue * .2d) / 10d;

                hiddenValue /= 10d;

                displayValues[0] = lowValue.ToString("0.0");
                displayValues[1] = midLowValue.ToString("0.0");
                displayValues[2] = midHighValue.ToString("0.0");
                displayValues[3] = highValue.ToString("0.0");
            }
            else
            {
                maxValue *= 1000;

                highValue = Math.Ceiling((double)maxValue);
                hiddenValue = highValue * 1.25d;

                highValue = Math.Round(hiddenValue * .8d) / 1000d;
                midHighValue = Math.Round(hiddenValue * .6d) / 1000d;
                midLowValue = Math.Round(hiddenValue * .4d) / 1000d;
                lowValue = Math.Round(hiddenValue * .2d) / 1000d;

                hiddenValue /= 1000d;

                displayValues[0] = lowValue.ToString("0.000");
                displayValues[1] = midLowValue.ToString("0.000");
                displayValues[2] = midHighValue.ToString("0.000");
                displayValues[3] = highValue.ToString("0.000");
            }

            return (float)hiddenValue;
        }

        private void scaleSequenceCurrents()
        {
            float maxCurrentRMS = 0;

            foreach (PhasorDefinition pD in sequenceClosePhasors)
            {
                if ((pD.Type == PhasorTypes.IN || pD.Type == PhasorTypes.IP) && pD.RMSValue > maxCurrentRMS)                 //find which phasor has highest RMS value
                {
                    maxCurrentRMS = pD.RMSValue;
                }
            }

            maxCurrentRMS = this.setCurrentLabelsAndGetMaxGraphValue(maxCurrentRMS, this.CurrentSequenceDisplayValues); //highest value that will display on graph

            foreach (PhasorDefinition pD in sequenceClosePhasors)
            {
                if (pD.Type == PhasorTypes.IN || pD.Type == PhasorTypes.IP)
                {
                    pD.MaxValue = maxCurrentRMS;
                    pD.SetEndPoint();
                }
            }
        }


        private void scaleIEff()
        {
            float maxCurrentRMS = 0;

            maxCurrentRMS = ieffPhasors[0].RMSValue;

            maxCurrentRMS = this.setCurrentLabelsAndGetMaxGraphValue(maxCurrentRMS, this.IeffDisplayValues); //highest value that will display on graph

            ieffPhasors[0].MaxValue = maxCurrentRMS;
            ieffPhasors[0].SetEndPoint();
        }


        private void scalePowerPhasors()
        {
            float maxCurrentRMS = 0;

            foreach (PhasorDefinition pD in powerPhasors)
            {
                if (pD.RMSValue > maxCurrentRMS)                 //find which phasor has highest RMS value
                {
                    maxCurrentRMS = pD.RMSValue;
                }
            }

            maxCurrentRMS = this.setCurrentLabelsAndGetMaxGraphValue(maxCurrentRMS, this.PowerDisplayValues); //highest value that will display on graph

            foreach (PhasorDefinition pD in powerPhasors)
            {
                pD.MaxValue = maxCurrentRMS;
                pD.SetEndPoint();
            }

        }

        private float convertRMSV(long rMS)
        {
            float returnFloat;

            returnFloat = (float)rMS;                     //convert to a float
            returnFloat = returnFloat * (float)Constants.TwelveFracBits;

            returnFloat *= (float)ProtectorVoltage.Scaling;
            return returnFloat;
        }

        private float convertRMSI(long rMS)
        {
            float returnFloat;

            returnFloat = (float)rMS;                     //convert to a float
            returnFloat = returnFloat * (float)Constants.SixteenFracBits;

            return returnFloat;
        }

        private float convertRMSI(long realValue, long imaginaryValue)
        {
            float realFloat, imaginaryFloat;

            realFloat = (float)realValue * (float)Constants.SixteenFracBits;
            imaginaryFloat = (float)imaginaryValue * (float)Constants.SixteenFracBits;

            realFloat = (realFloat * realFloat) + (imaginaryFloat * imaginaryFloat);
            realFloat = (float)Math.Sqrt(realFloat);

            return realFloat;
        }

        private float RMS(long f, long g)
        {
            double tempF, tempG;

            if (f >= 0x80000000)
            {
                f = 0x80000000 - f;  //get the difference between the two values
                f = 0x80000000 + f;  //Add the difference to 0x8000 to get the converted value
                f = 0 - f;       //make it negative.
            }
            if (g >= 0x80000000)
            {
                g = 0x80000000 - g;  //get the difference between the two values
                g = 0x80000000 + g;  //Add the difference to 0x8000 to get the converted value
                g = 0 - g;       //make it negative.
            }
            tempF = (double)f;
            tempG = (double)g;

            tempF = tempF * (double)Constants.TwelveFracBits;
            tempG = tempG * (double)Constants.TwelveFracBits;

            tempF = Math.Pow(tempF, 2) + Math.Pow(tempG, 2);
            tempF = Math.Sqrt(tempF);

            return (float)tempF;
        }

        private float currentRMS(long f, long g)
        {
            double tempF, tempG;

            if (f >= 0x80000000)
            {
                f = 0x80000000 - f;  //get the difference between the two values
                f = 0x80000000 + f;  //Add the difference to 0x8000 to get the converted value
                f = 0 - f;       //make it negative.
            }
            if (g >= 0x80000000)
            {
                g = 0x80000000 - g;  //get the difference between the two values
                g = 0x80000000 + g;  //Add the difference to 0x8000 to get the converted value
                g = 0 - g;       //make it negative.
            }
            tempF = (double)f;
            tempG = (double)g;

            tempF = tempF * (double)Constants.SixteenFracBits;
            tempG = tempG * (double)Constants.SixteenFracBits;

            tempF = Math.Pow(tempF, 2) + Math.Pow(tempG, 2);
            tempF = Math.Sqrt(tempF);

            return (float)tempF;
        }

        private delegate void setCheckBoxCheckedCallBack(bool b, CheckBox cB);

        private void setCheckedValue(bool b, CheckBox cB)
        {
            if (cB.InvokeRequired)
            {
                setCheckBoxCheckedCallBack callBack = new setCheckBoxCheckedCallBack(setCheckedValue);
                this.Invoke(callBack, new object[] { b, cB });
            }
            else
            {
                cB.Checked = b;
            }
        }
        private delegate void setLabelTextCallBack(string s, Label l);

        private void setText(string s, Label l)
        {
            if (l.InvokeRequired)
            {
                setLabelTextCallBack b = new setLabelTextCallBack(setText);
                this.Invoke(b, new object[] { s, l });
            }
            else
            {
                l.Text = s;
            }
        }
        private delegate void setTextBoxTextCallBack(string s, TextBox tB);

        private void setText(string s, TextBox tB)
        {
            if (tB.InvokeRequired)
            {
                setTextBoxTextCallBack b = new setTextBoxTextCallBack(setText);
                this.Invoke(b, new object[] { s, tB });
            }
            else
            {
                tB.Text = s;
            }
        }

        private int getPhaseNumber(string s)
        {
            switch (s)
            {
                case "Phase A":
                    return 0;
                case "Phase B":
                    return 3;
                case "Phase C":
                    return 6;
                case "Combined":
                    return 9;
                default:
                    return 0;
            }
        }

        private void listBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;

            if (lb.SelectedItem.ToString().Equals(RawPhasorGroups.Tripped.ToString()))
                this.switchToTripped();
            if (lb.SelectedItem.ToString().Equals(RawPhasorGroups.Closed.ToString()))
                this.switchToClosed();
        }

        private void listBoxSequencePower_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            switch (lb.SelectedItem.ToString())
            {
                case "Power Phasors":
                default:
                    this.switchToPower();
                    break;
                case "Sequence Phasors":
                    this.switchToClosedSequence();
                    break;
                case "Effective Current":
                    this.switchToIeff();
                    break;
                case "Differential Voltages":
                    this.switchToDifferentialVoltage();
                    break;
                case "Diff Sequence Voltages":
                    this.switchToDifferentialSequence();
                    break;
            }
        }

        private void switchToClosed()
        {
            this.phasorGraph1.XAxisValues = this.VoltageDisplayValues;

            this.switchTo(this.closePhasors, this.phasorGraph1);
            this.panelClosed.Show();
            this.panelTripped.Hide();

            if (!this.RealTimeMonitoring)
            {
                this.updateAxisValues();
                this.phasorGraph1.Invalidate();
            }
        }

        private void updateAxisValues()
        {
            if (panelClosed.Visible)
                this.phasorGraph1.YAxisValues = this.CurrentDisplayValues;
            else
                this.phasorGraph1.YAxisValues = this.VoltageDisplayValues;

            if (this.panelPower.Visible)
            {
                this.phasorGraph2.XAxisValues = this.NoValues;
                this.phasorGraph2.YAxisValues = this.PowerDisplayValues;
            }
            else if (this.panelIeff.Visible)
            {
                this.phasorGraph2.XAxisValues = this.NoValues;
                this.phasorGraph2.YAxisValues = this.IeffDisplayValues;
            }
            else if (this.panelDifferentialVoltage.Visible)
            {
                this.phasorGraph2.XAxisValues = this.NoValues;
                this.phasorGraph2.YAxisValues = this.VdDisplayValues;
            }
            else if (this.panelDifferentialSquence.Visible)
            {
                this.phasorGraph2.XAxisValues = this.NoValues;
                this.phasorGraph2.YAxisValues = this.VdSeqDisplayValues;
            }
            else if (this.panelClosedSequence.Visible)
            {
                this.phasorGraph2.XAxisValues = this.VoltageSequenceValues;
                this.phasorGraph2.YAxisValues = this.CurrentSequenceDisplayValues;
            }
        }

        private void switchToTripped()
        {
            this.phasorGraph1.XAxisValues = this.VoltageDisplayValues;
            this.phasorGraph1.YAxisValues = this.VoltageDisplayValues;
            this.switchTo(this.trippedPhasors, this.phasorGraph1);
            this.panelClosed.Hide();
            this.panelTripped.Show();
            if (!this.RealTimeMonitoring)
            {
                this.updateAxisValues();
                this.phasorGraph1.Invalidate();
            }
        }

        private void switchToIeff()
        {
            this.phasorGraph2.XAxisValues = this.IeffDisplayValues;
            this.phasorGraph2.YAxisValues = this.IeffDisplayValues;
            this.switchTo(this.ieffPhasors, this.phasorGraph2);
            this.panelIeff.Show();
            this.panelDifferentialVoltage.Hide();
            this.panelDifferentialSquence.Hide();
            this.panelClosedSequence.Hide();
            this.panelPower.Hide();

            if (!this.RealTimeMonitoring)
            {
                this.updateAxisValues();
                this.phasorGraph2.Invalidate();
            }
        }

        private void switchToPower()
        {
            this.phasorGraph2.XAxisValues = this.PowerDisplayValues;
            this.phasorGraph2.YAxisValues = this.PowerDisplayValues;

            this.switchTo(this.powerPhasors, this.phasorGraph2);
            this.panelIeff.Hide();
            this.panelDifferentialVoltage.Hide();
            this.panelDifferentialSquence.Hide();
            this.panelClosedSequence.Hide();
            this.panelPower.Show();

            if (!this.RealTimeMonitoring)
            {
                this.updateAxisValues();
                this.phasorGraph2.Invalidate();
            }
        }

        private void switchToClosedSequence()
        {
            this.phasorGraph2.XAxisValues = this.CurrentSequenceDisplayValues;
            this.phasorGraph2.YAxisValues = this.CurrentSequenceDisplayValues;
            this.switchTo(this.sequenceClosePhasors, this.phasorGraph2);
            this.panelIeff.Hide();
            this.panelDifferentialVoltage.Hide();
            this.panelDifferentialSquence.Hide();
            this.panelClosedSequence.Show();
            this.panelPower.Hide();

            if (!this.RealTimeMonitoring)
            {
                this.updateAxisValues();
                this.phasorGraph2.Invalidate();
            }
        }

        private void switchToDifferentialSequence()
        {
            this.phasorGraph2.XAxisValues = this.VdSeqDisplayValues;
            this.phasorGraph2.YAxisValues = this.VdSeqDisplayValues;
            this.switchTo(this.sequenceTrippedPhasors, this.phasorGraph2);
            this.panelIeff.Hide();
            this.panelDifferentialVoltage.Hide();
            this.panelDifferentialSquence.Show();
            this.panelClosedSequence.Hide();
            this.panelPower.Hide();

            if (!this.RealTimeMonitoring)
            {
                this.updateAxisValues();
                this.phasorGraph2.Invalidate();
            }
        }

        private void switchToDifferentialVoltage()
        {
            this.phasorGraph2.XAxisValues = this.VdDisplayValues;
            this.phasorGraph2.YAxisValues = this.VdDisplayValues;
            this.switchTo(this.differentialVoltages, this.phasorGraph2);
            this.panelIeff.Hide();
            this.panelDifferentialVoltage.Show();
            this.panelDifferentialSquence.Hide();
            this.panelClosedSequence.Hide();
            this.panelPower.Hide();

            if (!this.RealTimeMonitoring)
            {
                this.updateAxisValues();
                this.phasorGraph2.Invalidate();
            }
        }

        private void switchTo(PhasorDefinition[] phasorArray, PhasorGraph pg)
        {
            pg.phasorsToDraw.Clear();

            foreach (PhasorDefinition pD in phasorArray)
            {
                pg.AddPhasor(pD);
            }

        }

        public void ClearAllLabels()
        {
            this.setText("", this.textBoxIAAngle);
            this.setText("", this.textBoxIARMS);
            this.setText("", this.textBoxIBAngle);
            this.setText("", this.textBoxIBRMS);
            this.setText("", this.textBoxICAngle);
            this.setText("", this.textBoxICRMS);
            this.setText("", this.textBoxIEffAngle);
            this.setText("", this.textBoxIEffRMS);
            this.setText("", this.textBoxINAngle);
            this.setText("", this.textBoxINRMS);
            this.setText("", this.textBoxIPAngle);
            this.setText("", this.textBoxIPRMS);
            this.setText("", this.textBoxPAAngle);
            this.setText("", this.textBoxPARMS);
            this.setText("", this.textBoxPBAngle);
            this.setText("", this.textBoxPBRMS);
            this.setText("", this.textBoxPCAngle);
            this.setText("", this.textBoxPCRMS);
            this.setText("", this.textBoxPTAngle);
            this.setText("", this.textBoxPTRMS);
            this.setText("", this.textBoxVdAAngle);
            this.setText("", this.textBoxVdARMS);
            this.setText("", this.textBoxVdBAngle);
            this.setText("", this.textBoxVdBRMS);
            this.setText("", this.textBoxVdCAngle);
            this.setText("", this.textBoxVdCRMS);
            this.setText("", this.textBoxVdNAngle);
            this.setText("", this.textBoxVdNRMS);
            this.setText("", this.textBoxVdPAngle);
            this.setText("", this.textBoxVdPRMS);
            this.setText("", this.textBoxVdTAngle);
            this.setText("", this.textBoxVdTRMS);
            this.setText("", this.textBoxVnAAngle);
            this.setText("", this.textBoxVnARMS);
            this.setText("", this.textBoxVnBAngle);
            this.setText("", this.textBoxVnBRMS);
            this.setText("", this.textBoxVnCAngle);
            this.setText("", this.textBoxVnCRMS);
            this.setText("", this.textBoxVnNAngle);
            this.setText("", this.textBoxVnNRMS);
            this.setText("", this.textBoxVnPAngle);
            this.setText("", this.textBoxVnPRMS);
            this.setText("", this.textBoxVtAAngle);
            this.setText("", this.textBoxVtARMS);
            this.setText("", this.textBoxVtBAngle);
            this.setText("", this.textBoxVtBRMS);
            this.setText("", this.textBoxVtCAngle);
            this.setText("", this.textBoxVtCRMS);
            this.setText("", this.textBoxVtNAngle);
            this.setText("", this.textBoxVtNRMS);
            this.setText("", this.textBoxVtPAngle);
            this.setText("", this.textBoxVtPRMS);
            this.setText("", this.textBoxIAReal);
            this.setText("", this.textBoxIBReal);
            this.setText("", this.textBoxICReal);
            this.setText("", this.textBoxVdAReal);
            this.setText("", this.textBoxVdBReal);
            this.setText("", this.textBoxVdCReal);
            this.setText("", this.textBoxVdBReal);
            this.setText("", this.textBoxIEffReal);
        }

        private void textBoxPAAngle_TextChanged(object sender, EventArgs e)
        {
            this.setPF(this.textBoxPAAngle, this.textBoxPFA);
        }

        private void textBoxPBAngle_TextChanged(object sender, EventArgs e)
        {
            this.setPF(this.textBoxPBAngle, this.textBoxPFB);
        }

        private void textBoxPCAngle_TextChanged(object sender, EventArgs e)
        {
            this.setPF(this.textBoxPCAngle, this.textBoxPFC);
        }

        private void textBoxPTAngle_TextChanged(object sender, EventArgs e)
        {
            this.setPF(this.textBoxPTAngle, this.textBoxPFT);
        }

        private void setPF(TextBox Angle, TextBox PF)
        {
            double temp;

            if (Angle.Text != "")
            {
                temp = RelayControlLibrary.RelayModeFunctions.DegreesToRadians(Convert.ToDouble(Angle.Text));
                temp = Math.Cos(temp);
                PF.Text = String.Format("{0:0.0000}", temp);
            }
            else
            {
                PF.Text = "";
            }
        }

        #region Manual Phasor Calculation Section

        public Phasors VtA = new Phasors();
        public Phasors VtB = new Phasors();
        public Phasors VtC = new Phasors();
        public Phasors VnA = new Phasors();
        public Phasors VnB = new Phasors();
        public Phasors VnC = new Phasors();
        public Phasors IA = new Phasors();
        public Phasors IB = new Phasors();
        public Phasors IC = new Phasors();
        public Phasors VdA = new Phasors();
        public Phasors VdB = new Phasors();
        public Phasors VdC = new Phasors();
        public Phasors VdAvg = new Phasors();
        public Phasors Ieff = new Phasors();
        public Phasors IN = new Phasors();
        public Phasors IP = new Phasors();
        public Phasors PA = new Phasors();
        public Phasors PB = new Phasors();
        public Phasors PC = new Phasors();
        public Phasors PAvg = new Phasors();
        public Phasors VtN = new Phasors();
        public Phasors VtP = new Phasors();
        public Phasors VdN = new Phasors();
        public Phasors VdP = new Phasors();
        public Phasors VnN = new Phasors();
        public Phasors VnP = new Phasors();

        private bool realTimeMonitoring = false;
        public bool RealTimeMonitoring
        {
            get { return this.realTimeMonitoring; }
            set
            {
                this.realTimeMonitoring = value;

                if (value)
                    this.enableEventNavigation(false);
            }
        }

        private void enableEventNavigation(bool b)
        {
            int tempInt = this.CTRatio * 5;

            this.textBoxCTRatio.Text = tempInt.ToString();
            this.textBoxViewedCycleNumber.Visible = b;
            this.labelViewedCycleNumber.Visible = b;
            this.buttonDownCycle.Visible = b;
            this.buttonUpCycle.Visible = b;
            this.textBoxCTRatio.Visible = b;
            this.labelCTRatio.Visible = b;
            this.labelCTRatioOver5.Visible = b;
            this.checkBoxGERelay.Visible = b;
            this.checkBoxABC.Visible = b;
            this.checkBoxBFlag.Visible = b;
        }

        private float[] referenceWave;

        private void generateRefWav()
        {
            referenceWave = new float[_cycleCount];

            for (int i = 0; i < _cycleCount; i++)
            {
                var rads = 2 * (float)Math.PI * (float)i / (float)_cycleCount;
                referenceWave[i] = _amplitude * (float)Math.Sin(rads);
            }
        }

        public float[] generateReferenceWave(CompleteCycleEventArgs sEA)
        {

            return referenceWave;
        }

        public void UpdateValuesFromWaves(CompleteCycleEventArgs sEA)
        {
            this.enableEventNavigation(true);
            this.RealTimeMonitoring = false;
            this.textBoxViewedCycleNumber.Text = sEA.CycleNumber.ToString();
            this.cycleNumber = sEA.CycleNumber;
            this.workingEventNumber = sEA.EventNumber;
            //Measured voltages - transformer and network
            VnA.CalculatePhasorFromWaves(referenceWave, sEA.VnA);
            VnB.CalculatePhasorFromWaves(referenceWave, sEA.VnB);
            VnC.CalculatePhasorFromWaves(referenceWave, sEA.VnC);
            if (!this.gEEnabled)
            {
                VtA.CalculatePhasorFromWaves(referenceWave, sEA.VtA);
                VtB.CalculatePhasorFromWaves(referenceWave, sEA.VtB);
                VtC.CalculatePhasorFromWaves(referenceWave, sEA.VtC);
            }

            //Currents
            IA.CalculatePhasorFromWaves(referenceWave, sEA.IA);
            IB.CalculatePhasorFromWaves(referenceWave, sEA.IB);
            IC.CalculatePhasorFromWaves(referenceWave, sEA.IC);

            // choose the phase to rotate by, in case VnA is dead
            Phasors rotationPhasor = getRotationPhasor();
            this.rotatePhasors(-rotationPhasor.Degrees);


            //differential voltage section
            this.calculateDifferentialAndTransformerVoltages(sEA);
            this.scaleCalculatedVoltages();

            if (this.gEEnabled)
            {
                this.determineGEState();
            }

            //Sequence section
            this.calculateAllSequenceVectors();

            //Power
            this.calculatePowerPhasors();


            //ieff - needs to be done AFTER power.
            this.calculateEffectiveCurrentPhasor();

            // scaling
            this.scaleDifferentialSequenceVoltages();
            this.scaleSequenceCurrents();
            this.scaleMeasuredSequenceVoltages();
            this.scalePowerPhasors();
            this.scaleMeasuredVoltages();
            this.scaleCurrents();
            this.scaleIEff();

            this.ClearAllLabels();

            this.setLabelsFromStoredPhasors();

            if (panelClosed.Visible)
                this.phasorGraph1.YAxisValues = this.CurrentDisplayValues;
            else
                this.phasorGraph1.YAxisValues = this.VoltageDisplayValues;

            if (this.panelPower.Visible)
            {
                this.phasorGraph2.XAxisValues = this.NoValues;
                this.phasorGraph2.YAxisValues = this.PowerDisplayValues;
            }
            else if (this.panelIeff.Visible)
            {
                this.phasorGraph2.XAxisValues = this.NoValues;
                this.phasorGraph2.YAxisValues = this.IeffDisplayValues;
            }
            else if (this.panelDifferentialVoltage.Visible)
            {
                this.phasorGraph2.XAxisValues = this.NoValues;
                this.phasorGraph2.YAxisValues = this.VdDisplayValues;
            }
            else if (this.panelDifferentialSquence.Visible)
            {
                this.phasorGraph2.XAxisValues = this.NoValues;
                this.phasorGraph2.YAxisValues = this.VdSeqDisplayValues;
            }
            else if (this.panelClosedSequence.Visible)
            {
                this.phasorGraph2.XAxisValues = this.VoltageSequenceValues;
                this.phasorGraph2.YAxisValues = this.CurrentSequenceDisplayValues;
            }

            this.phasorGraph1.Invalidate();
            this.phasorGraph2.Invalidate();
        }

        private readonly float _phasorCuttoff = 20.0f;
        private Phasors getRotationPhasor()
        {
            Phasors rotationPhasor = VnA;
            if (VnA.RMS >= _phasorCuttoff)
            {
                rotationPhasor = (Phasors)VnA.Clone();
            }
            else if (VnB.RMS >= _phasorCuttoff)
            {
                rotationPhasor = (Phasors)VnB.Clone();
                rotationPhasor.Degrees += checkBoxABC.Checked ? 120 : -120;
            }
            else if (VnC.RMS >= _phasorCuttoff)
            {
                rotationPhasor = (Phasors)VnC.Clone();
                rotationPhasor.Degrees += checkBoxABC.Checked ? -120 : 120;
            }
            else if (VtA.RMS >= _phasorCuttoff)
            {
                rotationPhasor = (Phasors)VtA.Clone();
            }
            else if (VtB.RMS >= _phasorCuttoff)
            {
                rotationPhasor = (Phasors)VtB.Clone();
                rotationPhasor.Degrees += checkBoxABC.Checked ? 120 : -120;
            }
            else if (VtC.RMS >= _phasorCuttoff)
            {
                rotationPhasor = (Phasors)VtC.Clone();
                rotationPhasor.Degrees += checkBoxABC.Checked ? -120 : 120;
            }
            return rotationPhasor;
        }

        private void rotatePhasors(float p)
        {
            VnA.Degrees += p;
            VnB.Degrees += p;
            VnC.Degrees += p;
            VtA.Degrees += p;
            VtB.Degrees += p;
            VtC.Degrees += p;
            IA.Degrees += p;
            IB.Degrees += p;
            IC.Degrees += p;
        }

        private void determineGEState()
        {
            if (this.checkBoxBFlag.Checked)
            {
                this.gERelayOpened();
            }
            else
            {
                this.gERelayClosed();
            }
            /*
            if( !(VnA.RMS < 7.5f && VnB.RMS < 7.5f && VnC.RMS < 7.5f) &&
                (IA.RMS > .1f || IB.RMS > .1f || IC.RMS > .1f)
              )
            {
                this.gERelayClosed();
            }
            else if(    (VnA.RMS < 7.5f && VnB.RMS < 7.5f && VnC.RMS < 7.5f) ||
                        (VtA.RMS < 7.5f && VtA.RMS < 7.5f && VtC.RMS < 7.5f)
                    )
            {
                this.gERelayOpened();
            }
            */
        }

        private void gERelayOpened()
        {
            IA.Real = 0f;
            IB.Real = 0f;
            IC.Real = 0f;

            IA.Imaginary = 0f;
            IB.Imaginary = 0f;
            IC.Imaginary = 0f;
        }

        private void gERelayClosed()
        {
            // Left this part out intentionally as I don't want to deal with trying to figure out the state of the relay locally

            VtA.Real = VnA.Real;
            VtB.Real = VnB.Real;
            VtC.Real = VnC.Real;

            VtA.Imaginary = VnA.Imaginary;
            VtB.Imaginary = VnB.Imaginary;
            VtC.Imaginary = VnC.Imaginary;

            VdA.Real = 0f;
            VdB.Real = 0f;
            VdC.Real = 0f;
            VdAvg.Real = 0f;

            VdA.Imaginary = 0f;
            VdB.Imaginary = 0f;
            VdC.Imaginary = 0f;
            VdAvg.Imaginary = 0f;
        }

        private void calculateEffectiveCurrentPhasor()
        {

            this.Ieff.RMS = this.PAvg.RMS / 125f;

            this.Ieff.Real = this.Ieff.RMS * this.PAvg.Real;
            this.Ieff.Real = this.Ieff.Real / this.PAvg.RMS;

            this.Ieff.Imaginary = this.Ieff.RMS * this.PAvg.Imaginary;
            this.Ieff.Imaginary = this.Ieff.Imaginary / this.PAvg.RMS;
        }

        private void calculatePowerPhasors()
        {
            Phasors tempPhasor = new Phasors();
            this.PA.RMS = this.VnA.RMS * this.IA.RMS;
            this.PA.Degrees = this.IA.Degrees;

            this.PB.RMS = this.VnB.RMS * this.IB.RMS;
            this.PB.Degrees = this.IB.Degrees - this.VnB.Degrees;

            this.PC.RMS = this.VnC.RMS * this.IC.RMS;
            this.PC.Degrees = this.IC.Degrees - this.VnC.Degrees;

            this.PAvg.RMS = (this.PA.RMS + this.PB.RMS + this.PC.RMS) / 3f;
            tempPhasor.Real = this.PA.Real + this.PB.Real + this.PC.Real;
            tempPhasor.Imaginary = this.PA.Imaginary + this.PB.Imaginary + this.PC.Imaginary;
            this.PAvg.Degrees = tempPhasor.Degrees;
        }

        private readonly float GEConversionFactor = 93.75f;
        private float getRotationAngle()
        {
            float retVal = 0;
            if (CTRatio == 1)
                retVal = 0;
            else if (CTRatio <= 400)
                retVal = 5;
            else if (CTRatio < 600)
                retVal = 10;
            else
                retVal = 15;

            return retVal;
        }

        private void convertDiff(ref Phasors diff, Phasors current)
        {
            diff.Real = current.Real * GEConversionFactor;
            diff.Imaginary = current.Imaginary * GEConversionFactor;
        }

        private void calculateGEDifferentialVoltages(CompleteCycleEventArgs sEA)
        {
            Phasors temp = (Phasors)IA.Clone();
            temp.Degrees += getRotationAngle();
            convertDiff(ref VdA, temp);

            temp = (Phasors)IB.Clone();
            temp.Degrees += getRotationAngle();
            convertDiff(ref VdB, temp);

            temp = (Phasors)IC.Clone();
            temp.Degrees += getRotationAngle();
            convertDiff(ref VdC, temp);

            VtA.Real = VdA.Real + VnA.Real;
            VtA.Imaginary = VdA.Imaginary + VnA.Imaginary;

            VtB.Real = VdB.Real + VnB.Real;
            VtB.Imaginary = VdB.Imaginary + VnB.Imaginary;

            VtC.Real = VdC.Real + VnC.Real;
            VtC.Imaginary = VdC.Imaginary + VnC.Imaginary;
        }

        private void calculateDifferentialAndTransformerVoltages(CompleteCycleEventArgs sEA)
        {
            Phasors tempdA = new Phasors();
            Phasors tempdB = new Phasors();
            Phasors tempdC = new Phasors();

            if (!this.gEEnabled)
            {
                this.calculateDifferentialVoltage(VtA, VnA, VdA);
                this.calculateDifferentialVoltage(VtB, VnB, VdB);
                this.calculateDifferentialVoltage(VtC, VnC, VdC);
            }
            else
            {
                this.calculateGEDifferentialVoltages(sEA);
            }

            tempdA.RMS = VdA.RMS;
            tempdB.RMS = VdB.RMS;
            tempdC.RMS = VdC.RMS;

            // Voltages come in as absolute differential voltages
            // So to get the Total/Average differential voltage we have to rotate them
            // so they become relative to their respective voltage.
            // Here we choose which voltage to use as the relative voltage.
            // A is always at 0, so we can just choose it.
            tempdA.Degrees = VdA.Degrees;
            if (VnB.RMS > VtB.RMS)
            {
                tempdB.Degrees = VdB.Degrees - VnB.Degrees;
            }
            else
            {
                tempdB.Degrees = VdB.Degrees - VtB.Degrees;
            }

            if (VnC.RMS > VtC.RMS)
            {
                tempdC.Degrees = VdC.Degrees - VnC.Degrees;
            }
            else
            {
                tempdC.Degrees = VdC.Degrees - VtC.Degrees;
            }

            VdAvg.Real = (tempdA.Real + tempdB.Real + tempdC.Real) / 3f;
            VdAvg.Imaginary = (tempdA.Imaginary + tempdB.Imaginary + tempdC.Imaginary) / 3f;
        }

        private void calculateAllSequenceVectors()
        {
            if (this.checkBoxABC.Checked)
            {
                VtN = SequenceMath.NegativeSequence(VtN, VtA, VtB, VtC);
                VtP = SequenceMath.PositiveSequence(VtP, VtA, VtB, VtC);
                VnN = SequenceMath.NegativeSequence(VnN, VnA, VnB, VnC);
                VnP = SequenceMath.PositiveSequence(VnP, VnA, VnB, VnC);
                IN = SequenceMath.NegativeSequence(IN, IA, IB, IC);
                IP = SequenceMath.PositiveSequence(IP, IA, IB, IC);
                VdN = SequenceMath.NegativeSequence(VdN, VdA, VdB, VdC);
                VdP = SequenceMath.PositiveSequence(VdP, VdA, VdB, VdC);
            }
            else
            {
                VtN = SequenceMath.NegativeSequence(VtN, VtA, VtC, VtB);
                VtP = SequenceMath.PositiveSequence(VtP, VtA, VtC, VtB);
                VnN = SequenceMath.NegativeSequence(VnN, VnA, VnC, VnB);
                VnP = SequenceMath.PositiveSequence(VnP, VnA, VnC, VnB);
                IN = SequenceMath.NegativeSequence(IN, IA, IC, IB);
                IP = SequenceMath.PositiveSequence(IP, IA, IC, IB);
                VdN = SequenceMath.NegativeSequence(VdN, VdA, VdC, VdB);
                VdP = SequenceMath.PositiveSequence(VdP, VdA, VdC, VdB);
            }

        }

        private void calculateDifferentialVoltage(Phasors transformer, Phasors network, Phasors differential)
        {
            differential.Real = transformer.Real - network.Real;
            differential.Imaginary = transformer.Imaginary - network.Imaginary;
        }

        private void setLabelsFromStoredPhasors()
        {
            this.setLabelsFromSinglePhasor(VnA);
            this.setLabelsFromSinglePhasor(VnB);
            this.setLabelsFromSinglePhasor(VnC);
            this.setLabelsFromSinglePhasor(VtA);
            this.setLabelsFromSinglePhasor(VtB);
            this.setLabelsFromSinglePhasor(VtC);
            this.setLabelsFromSinglePhasor(IA);
            this.setLabelsFromSinglePhasor(IB);
            this.setLabelsFromSinglePhasor(IC);
            this.setLabelsFromSinglePhasor(VdA);
            this.setLabelsFromSinglePhasor(VdB);
            this.setLabelsFromSinglePhasor(VdC);
            this.setLabelsFromSinglePhasor(VdAvg);
            this.setLabelsFromSinglePhasor(VtP);
            this.setLabelsFromSinglePhasor(VtN);
            this.setLabelsFromSinglePhasor(VnP);
            this.setLabelsFromSinglePhasor(VnN);
            this.setLabelsFromSinglePhasor(VdP);
            this.setLabelsFromSinglePhasor(VdN);
            this.setLabelsFromSinglePhasor(IP);
            this.setLabelsFromSinglePhasor(IN);
            this.setLabelsFromSinglePhasor(PA);
            this.setLabelsFromSinglePhasor(PB);
            this.setLabelsFromSinglePhasor(PC);
            this.setLabelsFromSinglePhasor(PAvg);
            this.setLabelsFromSinglePhasor(Ieff);
        }

        private void setLabelsFromSinglePhasor(Phasors workingPhasor)
        {
            Phasors tempPhasor = new Phasors();

            if (RelayModeFunctions.IsCurrent(workingPhasor.PD.Type))
            {
                tempPhasor.Real = workingPhasor.Real * this.CTRatio;
                tempPhasor.Imaginary = workingPhasor.Imaginary * this.CTRatio;

                if (
                    (workingPhasor.RMS > .03f && !this.gEEnabled) ||
                    (workingPhasor.RMS != 0f && this.gEEnabled) ||
                    this.CTRatio == 1
                  )
                {
                    workingPhasor.PD.Enabled = true;
                    if (this.CTRatio != 1)
                    {
                        workingPhasor.PD.RMSBox.Text = tempPhasor.RMS.ToString("0.0");
                        if (workingPhasor.PD.RealBox != null)
                            workingPhasor.PD.RealBox.Text = tempPhasor.Real.ToString("0.0");
                    }
                    else
                    {
                        workingPhasor.PD.RMSBox.Text = tempPhasor.RMS.ToString("0.0000");
                        if (workingPhasor.PD.RealBox != null)
                            workingPhasor.PD.RealBox.Text = tempPhasor.Real.ToString("0.0000");
                    }
                    workingPhasor.PD.AngleBox.Text = tempPhasor.Degrees.ToString("0.0");
                }
                else
                {
                    workingPhasor.PD.Enabled = false;
                    workingPhasor.PD.RMSBox.Text = "";
                    workingPhasor.PD.AngleBox.Text = "";
                    if (workingPhasor.PD.RealBox != null)
                        workingPhasor.PD.RealBox.Text = "";
                }
            }
            else if (RelayModeFunctions.IsPower(workingPhasor.PD.Type))
            {
                tempPhasor.Real = workingPhasor.Real * this.CTRatio / 1000f;
                tempPhasor.Imaginary = workingPhasor.Imaginary * this.CTRatio / 1000f;

                if ((workingPhasor.RMS > .6f && !this.gEEnabled) ||
                    (workingPhasor.RMS != 0f && this.gEEnabled) ||
                    this.CTRatio == 1)
                {
                    workingPhasor.PD.Enabled = true;
                    workingPhasor.PD.RMSBox.Text = tempPhasor.RMS.ToString("0.0");
                    workingPhasor.PD.AngleBox.Text = tempPhasor.Degrees.ToString("0.0");
                    if (workingPhasor.PD.RealBox != null)
                        workingPhasor.PD.RealBox.Text = tempPhasor.Real.ToString("0.0");
                }
                else
                {
                    workingPhasor.PD.Enabled = false;
                    workingPhasor.PD.RMSBox.Text = "";
                    workingPhasor.PD.AngleBox.Text = "";
                    if (workingPhasor.PD.RealBox != null)
                        workingPhasor.PD.RealBox.Text = "";
                }
            }
            else
            {
                if ((workingPhasor.RMS > .2f && !this.gEEnabled) ||
                    (workingPhasor.RMS != 0f && this.gEEnabled) ||
                    this.CTRatio == 1)
                {
                    workingPhasor.PD.Enabled = true;
                    workingPhasor.PD.RMSBox.Text = workingPhasor.RMS.ToString("0.0");
                    workingPhasor.PD.AngleBox.Text = workingPhasor.Degrees.ToString("0.0");
                    if (workingPhasor.PD.RealBox != null)
                        workingPhasor.PD.RealBox.Text = workingPhasor.Real.ToString("0.0");
                }
                else
                {
                    workingPhasor.PD.Enabled = false;
                    workingPhasor.PD.RMSBox.Text = "";
                    workingPhasor.PD.AngleBox.Text = "";
                    if (workingPhasor.PD.RealBox != null)
                        workingPhasor.PD.RealBox.Text = "";
                }
            }
        }

        private int cycleNumber = 0;

        private void buttonUpCycle_Click(object sender, EventArgs e)
        {
            if (this.workingEventNumber != 9999)
            {
                if (cycleNumber != 15)
                    this.incrementCycleNumber();
            }
            else //live data
            {
                if (cycleNumber != 63)
                    this.incrementCycleNumber();
            }

        }

        private void buttonDownCycle_Click(object sender, EventArgs e)
        {
            if (this.cycleNumber != 0)
            {
                this.decrementCycleNumber();
            }
        }

        private void incrementCycleNumber()
        {
            this.cycleNumber++;
            this.getNewCycle(this.cycleNumber);
        }

        private void decrementCycleNumber()
        {
            this.cycleNumber--;
            this.getNewCycle(this.cycleNumber);
        }

        public delegate void RequestNewCycleHandler(object sender, CycleInfoRequestEventArgs cIREA);
        public event RequestNewCycleHandler RequestNewCycle;

        private uint workingEventNumber = 0;

        private void checkBoxGERelay_CheckedChanged(object sender, EventArgs e)
        {
            this.GEEnabled = ((CheckBox)sender).Checked;
        }

        private void ucPhasorGraph_Load(object sender, EventArgs e)
        {

        }

        private void textBoxVdPAngle_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelVdPUnits_Click(object sender, EventArgs e)
        {

        }

        private void textBoxVdPRMS_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelVdP_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxVnATHD_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelIATHD_Click(object sender, EventArgs e)
        {

        }

        private void textBoxIATHD_TextChanged(object sender, EventArgs e)
        {

        }

        private void getNewCycle(int p)
        {
            CycleInfoRequestEventArgs cIREA = new CycleInfoRequestEventArgs(p, this.workingEventNumber);

            if (this.RequestNewCycle != null)
                this.RequestNewCycle(this, cIREA);
        }

        private void textBoxCTRatio_ValueChanged(object sender, EventArgs e)
        {
            int tempCTRatio = this.CTRatio * 5;
            try
            {
                this.CTRatio = Convert.ToInt16(this.textBoxCTRatio.Text) / 5;
            }
            catch
            {
                this.textBoxCTRatio.Text = tempCTRatio.ToString();
            }
            this.setLabelsFromStoredPhasors();
        }

        #endregion


    }

    public class Phasors : ICloneable
    {
        public Phasors()
        {
        }

        public Phasors(float degrees, float rMS)
        {
            this.RMS = rMS;
            this.Degrees = degrees;
        }

        public Phasors(PhasorDefinition pd)
        {
            this.PD = pd;
        }

        public float Real
        {
            get { return this.real; }
            set
            {
                if (this.PD != null)                 //check to make sure if assigned before doing this
                    this.PD.RealValue = value;
                this.real = value;
                if (float.IsNaN(this.real) || float.IsNaN(this.imaginary))
                    return;
                this.setDegrees();
                this.setRMS();
            }
        }
        public float Imaginary
        {
            get { return this.imaginary; }
            set
            {
                if (this.PD != null)                //check to make sure it is assigned before doing this
                    this.PD.ImaginaryValue = value;
                this.imaginary = value;
                if (this.real == float.NaN || this.imaginary == float.NaN)
                    return;
                this.setDegrees();
                this.setRMS();
            }
        }
        public float RMS = 0f;
        public float Degrees
        {
            get { return this.degrees; }
            set
            {
                value %= 360;

                if (value > 180)
                    this.degrees = value - 360;
                else if (value < -180)
                    this.degrees = value + 360;
                else
                    this.degrees = value;

                this.calculateRealImaginary();
            }
        }

        private float degrees;
        private float real;
        private float imaginary;

        public PhasorDefinition PD;
        public void CalculatePhasorFromWaves(float[] referenceWave, float[] actualWave)
        {
            float tempRefRMS = 0;

            this.real = 0;
            this.imaginary = 0;
            this.RMS = 0;
            for (int i = 0; i < referenceWave.Length; ++i)
            {
                int i_cos = (i + referenceWave.Length / 4) % 128;     //get 90 degrees off for imaginar value

                //calculate the real value
                float temp = referenceWave[i] * actualWave[i];
                this.real += temp;

                //calculate the imaginary value
                temp = referenceWave[i_cos] * actualWave[i];
                this.imaginary += temp;

                temp = referenceWave[i] * referenceWave[i];
                tempRefRMS += temp;

                temp = actualWave[i] * actualWave[i];
                this.RMS += temp;
            }


            this.real /= (float)referenceWave.Length;
            this.imaginary /= (float)referenceWave.Length;
            this.RMS = (float)Math.Sqrt((double)(this.RMS / (float)referenceWave.Length));
            tempRefRMS = (float)Math.Sqrt((double)(tempRefRMS / (float)referenceWave.Length));

            this.real = this.real / tempRefRMS;
            this.imaginary = this.imaginary / tempRefRMS;

            this.Real = this.real;
            this.Imaginary = this.imaginary;
        }

        private void setRMS()
        {
            this.RMS = (float)Math.Sqrt(Math.Pow((double)this.Real, 2d) + Math.Pow((double)this.Imaginary, 2d));
        }

        private void setDegrees()
        {
            if (this.real != 0f)
                this.degrees = (float)RelayModeFunctions.RadiansToDegrees(Math.Atan((double)(this.imaginary / this.real)));
            else
                this.degrees = 90;

            if (this.real < 0)
            {
                if (this.imaginary < 0)
                    this.degrees = -180f + this.degrees;
                else
                    this.degrees = 180f + this.degrees;
            }
        }

        private void calculateRealImaginary()
        {
            double radians = (double)this.Degrees * 0.0174532925d;

            this.real = this.RMS * (float)Math.Cos(radians);
            this.imaginary = this.RMS * (float)Math.Sin(radians);
            if (this.PD != null)
            {
                this.PD.RealValue = this.real;
                this.PD.ImaginaryValue = this.imaginary;
            }

        }

        public void AdjustForCTRatio(int cTRatio)
        {
            if (this.PD != null)                                     //check to see if type is checkable, if not, just assume I didn't screw up
                if (!RelayModeFunctions.IsCurrent(this.PD.Type))     //if it isn't a current, it is not adjusted.  May need to change for Power
                    return;

            this.Real *= (float)cTRatio;
            this.Imaginary *= (float)cTRatio;
        }

        public object Clone()
        {
            return new Phasors() { PD = PD, Real = Real, Imaginary = Imaginary };
        }
    }

    public static class SequenceMath
    {
        private static Phasors AVect = new Phasors(120f, 1);
        private static Phasors AVect2 = new Phasors(-120f, 1);

        public static Phasors NegativeSequence(Phasors workingPhasor, Phasors A, Phasors B, Phasors C)
        {
            Phasors EA = new Phasors();
            Phasors EB = new Phasors();
            Phasors EC = new Phasors();

            EA = A;
            EB = phasorsMult(B, AVect2);
            EC = phasorsMult(C, AVect);

            workingPhasor.Real = (EA.Real + EB.Real + EC.Real) / 3f;
            workingPhasor.Imaginary = (EA.Imaginary + EB.Imaginary + EC.Imaginary) / 3f;

            return workingPhasor;
        }

        public static Phasors PositiveSequence(Phasors workingPhasor, Phasors A, Phasors B, Phasors C)
        {
            Phasors EA = new Phasors();
            Phasors EB = new Phasors();
            Phasors EC = new Phasors();

            EA = A;
            EB = phasorsMult(B, AVect);
            EC = phasorsMult(C, AVect2);

            workingPhasor.Real = (EA.Real + EB.Real + EC.Real) / 3f;
            workingPhasor.Imaginary = (EA.Imaginary + EB.Imaginary + EC.Imaginary) / 3f;

            return workingPhasor;
        }

        public static Phasors ZeroSequence(Phasors workingPhasor, Phasors A, Phasors B, Phasors C)
        {
            workingPhasor.Real = (A.Real + B.Real + C.Real) / 3f;
            workingPhasor.Imaginary = (A.Imaginary + B.Imaginary + C.Imaginary) / 3f;

            return workingPhasor;
        }

        private static Phasors phasorsMult(Phasors A, Phasors B)
        {
            float tempA, tempB;
            Phasors returnPhasor = new Phasors();

            tempA = A.Real * B.Real;
            tempB = A.Imaginary * B.Imaginary;

            returnPhasor.Real = tempA - tempB;

            tempA = A.Real * B.Imaginary;
            tempB = A.Imaginary * B.Real;

            returnPhasor.Imaginary = tempA + tempB;

            return returnPhasor;
        }
    }

    public class ucPhasorGraphEventArgs : EventArgs
    {
        public ucPhasorGraphEventArgs()
        {
        }

        public int PhaseNumber;
        public bool Monitor;
    }



}
