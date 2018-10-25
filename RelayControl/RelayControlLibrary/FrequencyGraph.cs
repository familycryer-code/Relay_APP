using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Exocortex.DSP;
using RelayControlLibrary;

namespace RelayControlLibrary
{
    public partial class FrequencyGraph : UserControl
    {

        public PhasorTypes PhasorType = new PhasorTypes();
        public int ClickedCycleNumber = 0;
        public float maxValue = 100f;
        public bool ShowOdds = false;
        public ArrayList Amplitudes = new ArrayList(128);
        public bool NoScale = false;
        public int NumberOfHarmonics
        {
            get { return this.numberOfHarmonics; }
            set
            {
                this.numberOfHarmonics = value;
                this.Amplitudes.Capacity = this.numberOfHarmonics;
                this.setXAxisValues();
            }
        }
        public float[] SineWave
        {
            set
            {
                this.sineWave = value;
                if (this.sineWave != null)
                    this.FFT(this.sineWave);
            }
        }
        public bool Protector277
        {
            get { return this.protector277; }
            set
            {
                this.protector277 = value;
            }
        }

        private bool protector277 = false;
        private int numberOfHarmonics = 32;
        private float conversionFactor;
        private Pen graphPen = new Pen(Color.Black, 1);
        private ArrayList xAxisValues = new ArrayList(128);
        private float[] sineWave;
        private ToolTip tT = new ToolTip();
        private float savedDistance;
        private int savedIndex;
        private float totalHarmonicDistortion;


        public FrequencyGraph()
        {
            InitializeComponent();
            this.setXAxisValues();
            this.setGraphDrawingValues();
            this.setConversionFactor();
            this.DoubleBuffered = true;
        }

        #region Drawing

        private void FrequencyGraph_Resize(object sender, EventArgs e)
        {
            this.setGraphDrawingValues();
            this.setXAxisValues();
            this.setConversionFactor();
        }

        private void setXAxisValues()
        {
            this.xAxisValues = new ArrayList(this.numberOfHarmonics);

            float temp = this.numberOfHarmonics + 1;

            temp = (this.Size.Width - 2) / temp;

            for (int i = 0; i < this.xAxisValues.Capacity; ++i)
            {
                this.xAxisValues.Add((float)(i + 1) * temp);
            }
        }

        private void setConversionFactor()
        {
            this.conversionFactor = this.Size.Height / this.maxValue;
            foreach (floatWrapper f in this.Amplitudes)
            {
                f.ConversionFactor = this.conversionFactor;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics onPaintGraphics = e.Graphics;

            this.drawBorder(onPaintGraphics);
            if (this.Amplitudes.Count == 0)
                return;

            for (int i = 0; i < this.xAxisValues.Count; ++i)
            {
                PointF p1, p2;
                floatWrapper fW;
                try
                {
                    fW = (floatWrapper)this.Amplitudes[i];
                }
                catch
                {
                    return;
                }

                p1 = new PointF((float)this.xAxisValues[i], this.Size.Height);
                p2 = new PointF((float)this.xAxisValues[i], this.Size.Height - (fW.GraphingValue));// / this.maxValue));
                try
                {
                    onPaintGraphics.DrawLine(new Pen(Color.Red, 2), p1, p2);
                }
                catch
                {

                }
            }
        }

        private PointF topLeft = new PointF();
        private PointF topRight = new PointF();
        private PointF bottomLeft = new PointF();
        private PointF bottomRight = new PointF();

        private void setGraphDrawingValues()
        {
            this.topLeft = new PointF(0, 0);
            this.topRight = new PointF(this.Size.Width - 1, 0);
            this.bottomLeft = new PointF(0, this.Size.Height - 1);
            this.bottomRight = new PointF(this.Size.Width - 1, this.Size.Height - 1);
        }

        private void drawBorder(Graphics oPG)
        {

            oPG.DrawLine(this.graphPen, this.topLeft, this.topRight);
            oPG.DrawLine(this.graphPen, this.topRight, this.bottomRight);
            oPG.DrawLine(this.graphPen, this.bottomRight, this.bottomLeft);
            oPG.DrawLine(this.graphPen, this.bottomLeft, this.topLeft);
        }

        #endregion

        private void FFT(float[] floatArray)
        {
            float[] harmonicsArray = new float[numberOfHarmonics << 4];

            Exocortex.DSP.Fourier.RFFT(floatArray, harmonicsArray.Length, FourierDirection.Forward);
            this.Amplitudes.Clear();

            for (int i = 0; i < harmonicsArray.Length - 1; ++i)
            {
                harmonicsArray[i] = (float)Math.Sqrt(((floatArray[(i << 1) + 2] / 128f) * (floatArray[(i << 1) + 2] / 128f)) + ((floatArray[(i << 1) + 3] / 128f) * (floatArray[(i << 1) + 3] / 128f)));
                harmonicsArray[i] /= 2f;
                harmonicsArray[i] /= (float)Math.Sqrt(2d);
                //For when it is only 128 samples in the whole thing
                harmonicsArray[i] *= 8;
            }

            for (int i = 0; i < numberOfHarmonics; ++i)
            {
                this.UpdateValue(harmonicsArray[(i * 8) + 7], i);

            }

            this.SetTHD(floatArray);
            this.ScaleValues();
            this.Invalidate();

        }

        public void SetTHD(float[] floatArray)
        {

            try
            {
                float sumOfSquares = 0;
                floatWrapper fW;
                int i = 1;

                if (this.Amplitudes.Count < 2)
                {
                    this.totalHarmonicDistortion = 0;
                    return;
                }
                for (; i < this.Amplitudes.Count >> 1; ++i)
                {
                    fW = (floatWrapper)this.Amplitudes[i];
                    fW.Value *= (float)Math.Sqrt(2d);

                    sumOfSquares += fW.Value * fW.Value;
                }
                fW = (floatWrapper)this.Amplitudes[0];
                fW.Value *= (float)Math.Sqrt(2d);

                this.totalHarmonicDistortion = (float)Math.Sqrt(sumOfSquares / (fW.Value * fW.Value)) * 100f;

            }
            catch (Exception ex)
            {
                throw new Exception("Error Setting THD", ex);
            }
        }

        public void UpdateValue(float value, int index)
        {
            floatWrapper fW = new floatWrapper();

            try
            {
                if (index >= this.Amplitudes.Count)
                {
                    this.Amplitudes.Add(fW);
                    fW.ConversionFactor = this.conversionFactor;
                }
                else
                {
                    fW = (floatWrapper)this.Amplitudes[index];
                }
                fW.Value = value;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Updating Values in Frequency Graph", ex);
            }
        }

        private void ScaleValues()
        {
            float highestValue = 0;
            floatWrapper fW;

            for (int i = 0; i < this.Amplitudes.Count; ++i)
            {
                fW = (floatWrapper)this.Amplitudes[i];
                if (fW.Value > highestValue)
                    highestValue = fW.Value;
            }

            if (highestValue > this.maxValue || highestValue < this.maxValue / 2f)
            {
                this.maxValue = highestValue * 1.25f;
            }

            foreach (floatWrapper fWrapper in this.Amplitudes)
            {
                this.conversionFactor = fWrapper.ConversionFactor = this.Size.Height / this.maxValue;

            }
        }

        private void FrequencyGraph_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                float distance = 10000, tempDistance;
                int closestIndex = 0;

                for (int i = 0; i < this.xAxisValues.Count; ++i)
                {

                    tempDistance = Math.Abs((float)this.xAxisValues[i] - e.X);
                    if (tempDistance < distance)
                    {
                        distance = tempDistance;
                        closestIndex = i;
                    }
                }

                this.savedDistance = distance;
                this.savedIndex = closestIndex;
                floatWrapper fW = (floatWrapper)this.Amplitudes[closestIndex];

                string harmonic;

                switch (++closestIndex)
                {
                    case 1:
                        harmonic = "Fundamental";
                        break;
                    case 2:
                        harmonic = "2nd";
                        break;
                    case 3:
                        harmonic = "3rd";
                        break;
                    case 4:
                        harmonic = "4th";
                        break;
                    case 5:
                        harmonic = "5th";
                        break;
                    case 6:
                        harmonic = "6th";
                        break;
                    case 7:
                        harmonic = "7th";
                        break;
                    case 8:
                        harmonic = "8th";
                        break;
                    case 9:
                        harmonic = "9th";
                        break;
                    case 10:
                        harmonic = "10th";
                        break;
                    case 11:
                        harmonic = "11th";
                        break;
                    case 12:
                        harmonic = "12th";
                        break;
                    case 13:
                        harmonic = "13th";
                        break;
                    case 14:
                        harmonic = "14th";
                        break;
                    case 15:
                        harmonic = "15th";
                        break;
                    case 16:
                        harmonic = "16th";
                        break;
                    case 17:
                        harmonic = "17th";
                        break;
                    case 18:
                        harmonic = "18th";
                        break;
                    case 19:
                        harmonic = "19th";
                        break;
                    case 20:
                        harmonic = "20th";
                        break;
                    case 21:
                        harmonic = "21st";
                        break;
                    case 22:
                        harmonic = "22nd";
                        break;
                    case 23:
                        harmonic = "23rd";
                        break;
                    case 24:
                        harmonic = "24th";
                        break;
                    case 25:
                        harmonic = "25th";
                        break;
                    case 26:
                        harmonic = "26th";
                        break;
                    case 27:
                        harmonic = "27th";
                        break;
                    case 28:
                        harmonic = "28th";
                        break;
                    case 29:
                        harmonic = "29th";
                        break;
                    case 30:
                        harmonic = "30th";
                        break;
                    case 31:
                        harmonic = "31st";
                        break;
                    case 32:
                        harmonic = "32nd";
                        break;
                    default:
                        harmonic = "Error " + closestIndex.ToString();
                        break;
                }

                float temp = fW.Value;

                if (!this.NoScale)
                {
                    temp = fW.Value / (float)Math.Sqrt(2d);

                }

                if (this.protector277 && !RelayModeFunctions.IsCurrent(this.PhasorType))
                {
                    temp = temp * Constants.Protector277Convert;
                }

                tT.Show(harmonic + " - " + Math.Round(temp, 2).ToString() + " - Cycle " + this.ClickedCycleNumber.ToString() + " - " + this.PhasorType.ToString() + " - " + Math.Round(this.totalHarmonicDistortion, 2).ToString() + "% THD", this, 10, 10);
            }
            catch
            {
            }

        }

        private void FrequencyGraph_MouseLeave(object sender, EventArgs e)
        {
            this.tT.Hide(this);
        }

        public void ClearAllValues()
        {
            floatWrapper fW;

            for (int i = 0; i < this.Amplitudes.Count; ++i)
            {
                fW = (floatWrapper)this.Amplitudes[i];
                fW.Value = 0;
            }
            this.Invalidate();
        }
    }

    public class floatWrapper
    {
        public floatWrapper()
        {
        }

        public floatWrapper(float conversionFactor)
        {
            this.ConversionFactor = conversionFactor;
        }

        public floatWrapper(float conversionFactor, float value)
        {
            this.conversionFactor = conversionFactor;
            this.Value = value;
        }

        private float conversionFactor = 1;
        public float ConversionFactor
        {
            get { return this.conversionFactor; }
            set
            {
                this.conversionFactor = value;

                this.GraphingValue = this.conversionFactor * this.Value;
            }
        }

        private float value;
        public float Value
        {
            get { return this.value; }
            set
            {
                this.value = value;


                this.GraphingValue = value * this.conversionFactor;
            }
        }

        public float GraphingValue;
    }
}
