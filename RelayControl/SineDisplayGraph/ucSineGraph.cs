using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RelayControlLibrary;

namespace SineDisplayGraph
{
    public partial class ucSineGraph : UserControl
    {
        public ucSineGraph()
        {
            InitializeComponent();
            this.sineGraph1.ScrollEnabled = false;
            this.sineGraph1.ShowEventLine = false;
            this.myInitialize();
            /*
            this.AddNewSineWave(PhasorTypes.VnA, this.sineGraph1);
            this.AddNewSineWave(PhasorTypes.VnB, this.sineGraph1);
            this.AddNewSineWave(PhasorTypes.VnC, this.sineGraph1);
            this.AddNewSineWave(PhasorTypes.IA, this.sineGraph1);
            this.AddNewSineWave(PhasorTypes.IB, this.sineGraph1);
            this.AddNewSineWave(PhasorTypes.IC, this.sineGraph1);

            this.checkBoxPhAI.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.IA);
            this.checkBoxPhBI.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.IB);
            this.checkBoxPhCI.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.IC);

            this.checkBoxPhAVn.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.VnA);
            this.checkBoxPhBVn.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.VnB);
            this.checkBoxPhCVn.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.VnC);
            */
            this.checkBoxPhBI.Checked = true;
            this.checkBoxPhBVn.Checked = true;

        }

        private void myInitialize()
        {
            this.checkBoxPhBVn.Checked = true;
            this.checkBoxPhBI.Checked = true;
            this.AddNewSineWave(PhasorTypes.VnA, this.sineGraph1);
            this.AddNewSineWave(PhasorTypes.VnB, this.sineGraph1);
            this.AddNewSineWave(PhasorTypes.VnC, this.sineGraph1);
            this.AddNewSineWave(PhasorTypes.IA, this.sineGraph1);
            this.AddNewSineWave(PhasorTypes.IB, this.sineGraph1);
            this.AddNewSineWave(PhasorTypes.IC, this.sineGraph1);

            this.checkBoxPhAI.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.IA);
            this.checkBoxPhBI.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.IB);
            this.checkBoxPhCI.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.IC);

            this.checkBoxPhAVn.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.VnA);
            this.checkBoxPhBVn.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.VnB);
            this.checkBoxPhCVn.ForeColor = RelayModeFunctions.GetPhaseColor(PhasorTypes.VnC);
             
        }
        private void checkCheckBoxes()
        {
            if (this.checkBoxPhAI.Checked)
            {
                this.checkBox_CheckedChanged1(this.checkBoxPhAI, new EventArgs());
            }
            if (this.checkBoxPhBI.Checked)
            {
                this.checkBox_CheckedChanged1(this.checkBoxPhBI, new EventArgs());
            }
            if (this.checkBoxPhCI.Checked)
            {
                this.checkBox_CheckedChanged1(this.checkBoxPhCI, new EventArgs());
            }
            if (this.checkBoxPhAVn.Checked)
            {
                this.checkBox_CheckedChanged1(this.checkBoxPhAVn, new EventArgs());
            }
            if (this.checkBoxPhBVn.Checked)
            {
                this.checkBox_CheckedChanged1(this.checkBoxPhBVn, new EventArgs());
            }
            if (this.checkBoxPhCVn.Checked)
            {
                this.checkBox_CheckedChanged1(this.checkBoxPhCVn, new EventArgs());
            }
        }

        public float MaxVValue = 254f;
        public float MaxIValue = 15f;

        public void AddNewSineWave(PhasorTypes pT, SineGraph sG)
        {
            sG.addNewSineWave(pT);
        }

        public void AddValue(float value, PhasorTypes pT)
        {
            foreach (SineWaveDefinition sWD in this.sineGraph1.sineWavesToDraw)
            {
                if (sWD.Phase == pT)
                {
                    sWD.AddValue(value);
                    if(pT == PhasorTypes.IC)
                        this.sineGraph1.Invalidate();
                }
            }
        }

        public void AddValueV(float value, PhasorTypes pT, int index)
        {
            float tempValue;
            if(value < 0)
                tempValue = 100 * (value / this.MaxVValue); 
            else
                tempValue = 100 * (value / this.MaxVValue);
            foreach (SineWaveDefinition sWD in this.sineGraph1.sineWavesToDraw)
            {
                if (sWD.Phase == pT)
                {
                    //sWD.AddValueNew(tempVal, index);
                    sWD.AddValueNew(value, index);
                    if (pT == PhasorTypes.IC)
                        this.sineGraph1.Invalidate();
                }
            }
        }

        public void AddValueI(float value, PhasorTypes pT, int index)
        {
            if (value < 0)
                value = 100 * (value / this.MaxIValue);
            else
                value = 100 * (value / this.MaxIValue);
            foreach (SineWaveDefinition sWD in this.sineGraph1.sineWavesToDraw)
            {
                if (sWD.Phase == pT)
                {
                    sWD.AddValue(value, index);
                    if (pT == PhasorTypes.IC)
                    {
                        this.sineGraph1.Invalidate();
                    }
                }
            }
        }

        private void checkBox_CheckedChanged1(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            ucSineGraphEventArgs ucSEV = new ucSineGraphEventArgs();
            ucSEV.Phase = this.getPhaseType(cb.Name);
            ucSEV.Monitor = cb.Checked;

            SineWaveDefinition sWD = (SineWaveDefinition)this.sineGraph1.GetWave(ucSEV.Phase);
            if(sWD != null)
            {
                sWD.Enabled = cb.Checked;
            }
            this.sineGraph1.Invalidate();
        }
        
        private PhasorTypes getPhaseType(string p)
        {
            switch (p)
            {
                case "checkBoxPhAVt":
                    return PhasorTypes.VtA;                    
                case "checkBoxPhAVn":
                    return PhasorTypes.VnA;                    
                case "checkBoxPhAI":
                    return PhasorTypes.IA;                    
                case "checkBoxPhBVt":
                    return PhasorTypes.VtB;                    
                case "checkBoxPhBVn":
                    return PhasorTypes.VnB;                    
                case "checkBoxPhBI":
                    return PhasorTypes.IB;                    
                case "checkBoxPhCVt":
                    return PhasorTypes.VtC;                    
                case "checkBoxPhCVn":
                    return PhasorTypes.VnC;                    
                case "checkBoxPhCI":
                    return PhasorTypes.IC;
                case "checkBoxPhAVd":
                    return PhasorTypes.VdA;
                case "checkBoxPhBVd":
                    return PhasorTypes.VdB;
                case "checkBoxPhCVd":
                    return PhasorTypes.VdC;
                default:
                    return PhasorTypes.Ieff;
                    
            }
        }

        private void buttonClearData_Click(object sender, EventArgs e)
        {
            this.sineGraph1.ClearData();
        }

        private void checkBox10xCurrent_CheckedChanged(object sender, EventArgs e)
        {
            if(this.checkBox10xCurrent.Checked)
            {
                this.MaxIValue = 1.5f;
                this.sineGraph1.ScaleCurrentValues(10f);
                this.labelCurrentMaxValue.Text = "-1.5 A";
            }
            else
            {
                this.MaxIValue = 15f;
                this.sineGraph1.ScaleCurrentValues(.1f);
                this.labelCurrentMaxValue.Text = "-15 A";
            }
        }

        public float[] GetWaveArray(PhasorTypes pT, int arraySize)
        {
            SineWaveDefinition workingSWD = new SineWaveDefinition(PhasorTypes.IA, 128, new Pen(Color.Red, 1));
            float[] returnArray = new float[arraySize];

            foreach (SineWaveDefinition sWD in this.sineGraph1.sineWavesToDraw)
            {
                if(sWD.Phase == pT)
                {
                    workingSWD = sWD;
                }
            }

            for (int i = 0; i < arraySize; ++i)
            {
                returnArray[i] = workingSWD.ActualValues[i];
            }
            return returnArray;
        }

        public void ClearAllValues()
        {
            this.sineGraph1.sineWavesToDraw.Clear();
            this.myInitialize();
            this.checkCheckBoxes();
        }
    }

    public class ucSineGraphEventArgs : EventArgs
    {
        public ucSineGraphEventArgs()
        {
        }

        public PhasorTypes Phase;
        public bool Monitor;
    }
}
