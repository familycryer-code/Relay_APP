using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Exocortex.DSP;

namespace RelayControlLibrary
{
    public partial class ucFrequencyAnalysis : UserControl
    {
        public ucFrequencyAnalysis()
        {
            InitializeComponent();
            generateLUT();
            this.frequencyGraph1.Size = this.Size;
            this.frequencyGraph1.NumberOfHarmonics = 64;
        }

        private int samplesPerCycle = 128;
        public int SamplesPerCycle
        {
            get { return this.samplesPerCycle; }
            set
            {
                this.samplesPerCycle = value;
                
                this.generateLUT();
            }
        }

        private float[] sineLUT = new float[128];

        private void generateLUT()
        {
            double angle, radians;
            this.sineLUT = new float[this.samplesPerCycle];

            for (int i = 0; i < this.samplesPerCycle; ++i)
            {
                angle = ((double)i/(double)this.samplesPerCycle) * 360;
                radians = angle * 0.0174532925;

                this.sineLUT[i] = (float)Math.Sin(radians);
            }
        }
        
        private float[] values;
        public float[] Values
        {
            get { return this.values; }
            set 
            { 
                this.values = value;
                if(values != null)
                    this.FFT();
            }
        }
        
        private void FFT()
        {
            float[] tempArray = new float[128];
            
            ComplexF[] complexArray = new ComplexF[256];

            this.frequencyGraph1.Amplitudes.Clear();
            this.frequencyGraph1.SineWave = this.values;

            this.frequencyGraph1.Invalidate();
            
        }

        private void frequencyGraph1_Resize(object sender, EventArgs e)
        {
            this.frequencyGraph1.Size = this.Size;
        }
    }
}
