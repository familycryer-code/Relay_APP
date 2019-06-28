using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RelayControlLibrary;

namespace RelayControlLibrary
{
    public partial class SineFrequencyPopup : Form
    {
        public SineFrequencyPopup(SineWaveDefinition sWD, int cTRatio, ProtectorVoltage voltage)
        {
            InitializeComponent();
            this.voltage = voltage;
            this.myInitialization(sWD, cTRatio);
        }

        private int cTRatio = 320;
        private ProtectorVoltage voltage;

        private void myInitialization(SineWaveDefinition sWD, int cTRatio)
        {
            this.sineGraph1.BackColor = Color.White;
            sineGraph1.ProtectorVoltage = voltage;
            this.cTRatio = cTRatio;
            this.Text = sWD.Phase.ToString();
            this.SineFrequencyPopup_Resize(this, new EventArgs());
            this.sineGraph1.AddNewSineWave(sWD.Phase, sWD.InputArray.Length);

            SineWaveDefinition outputSWD = (SineWaveDefinition)this.sineGraph1.sineWavesToDraw[0];

            this.sineGraph1.CTRatio = this.cTRatio;
            for (int i = 0; i < sWD.AdjustedArray.Length - 1; ++i)
            {
                outputSWD.AddValueNew(sWD.ActualValues[i], i);
            }
            outputSWD.Enabled = true;
            this.sineGraph1.Invalidate();
        }

        private void SineFrequencyPopup_Resize(object sender, EventArgs e)
        {
            System.Drawing.Size componentSize = new Size(this.ClientSize.Width - 6, (this.ClientSize.Height - this.buttonClose.Height - 12) / 2);

            this.sineGraph1.Size = componentSize;
            this.sineGraph1.Location = new Point(3, 3);

            this.frequencyGraph1.Size = componentSize;
            this.frequencyGraph1.Location = new Point(3, componentSize.Height + 6);

            this.buttonClose.Location = new Point(this.ClientSize.Width - this.buttonClose.Width - 3, this.ClientSize.Height - this.buttonClose.Height - 3);

            this.frequencyGraph1.Invalidate();
            this.sineGraph1.Invalidate();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sineGraph1_MouseClick(object sender, MouseEventArgs e)
        {
            this.setFrequencyGraph(this.frequencyGraph1, this.sineGraph1);
        }

        private void setFrequencyGraph(FrequencyGraph fG, SineGraph sG)
        {
            SineWaveDefinition sWD = (SineWaveDefinition)sG.sineWavesToDraw[0];
            float[] tempFloat = new float[128 * 8];//float[sWD.InputArray.Length];
            int startIndex = 128;

            for (int i = 0; i < tempFloat.Length; ++i, ++startIndex)
            {
                if (RelayModeFunctions.IsCurrent(sWD.Phase))
                    tempFloat[i] = sWD.ActualValues[startIndex] * this.cTRatio;
                else
                    tempFloat[i] = sWD.ActualValues[startIndex];
            }

            fG.ClickedCycleNumber = sG.ClickedCycleNumber;
            fG.PhasorType = sG.Type;
            fG.SineWave = tempFloat;
        }
    }
}