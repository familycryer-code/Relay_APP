using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace RelayControlLibrary
{
    [Serializable()]
    public class SineWaveDefinition
    {
        public SineWaveDefinition(PhasorTypes pT, int arraySize, Pen functionPen)
        {
            AdjustedArray = new PointF[arraySize];

            InputArray = new PointF[arraySize];
            ActualValues = new float[arraySize];

            float translatedZeroPoint = translateYPoint(0);

            for (int i = 0; i < arraySize - 1; ++i)
            {
                InputArray[i].X = i;
            }
            this.FunctionPen = functionPen;

            this.initializeSineGraph();
        }

        private void initializeSineGraph()
        {
            for (int i = 0; i < this.sineLUT.Length; i++)
            {
                double radians = (double)i / (double)this.sineLUT.Length * 2.0d * Math.PI;
                this.sineLUT[i] = (float)Math.Sin(radians);
            }
        }

        public bool Enabled = false;
        public PointF[] InputArray;
        public PointF[] AdjustedArray;
        public float[] ActualValues;
        public Pen FunctionPen;// = new Pen(Color.Red, 2);
        public float DilationMultiplier = 10000000f;
        public float TranslationValue = 10f;
        public float RMS;
        public float MaxValueDivider = 2.54f;
        private int currentIndex = 000;
        private float[] sineLUT = new float[128];

        private float indexMultipler;
        public float IndexMultiplier
        {
            get { return this.indexMultipler; }
            set
            {
                this.indexMultipler = value;
            }
        }

        public PhasorTypes Phase;

        private float translateYPoint(float YValue)
        {
            return this.translateYPoint(YValue, this.DilationMultiplier, this.TranslationValue);
        }

        private float translateYPoint(float YValue, float dilator, float translator)
        {
            return YValue * DilationMultiplier + TranslationValue;
        }

        private PointF[] translatedSineWave(PointF[] originalArray)
        {
            PointF[] returnArray = new PointF[originalArray.Length];

            for (int i = 0; i < originalArray.Length; ++i)
            {
                returnArray[i].Y = translateYPoint(originalArray[i].Y, DilationMultiplier, TranslationValue);
            }

            return returnArray;
        }

        public float tempUpdateYValue(PointF pointToUpdate)
        {
            float returnPoint = translateYPoint(pointToUpdate.Y, this.DilationMultiplier, this.TranslationValue);
            return returnPoint;
        }

        public void AddValue(float value)
        {
            if (currentIndex >= InputArray.Length - 1)
            {
                shiftValuesLeft();
                currentIndex = InputArray.Length - 1;
            }
            else
            {
                ++currentIndex;
            }
            InputArray[currentIndex].X = currentIndex * this.indexMultipler;
            InputArray[currentIndex].Y = value;

            AdjustedArray[currentIndex].X = currentIndex * this.indexMultipler;
            AdjustedArray[currentIndex].Y = this.translateYPoint(value, this.DilationMultiplier, this.TranslationValue);

        }

        public void AddValue(float value, int index)
        {
            try
            {
                if (currentIndex >= InputArray.Length - 1)
                {
                    return;
                }

                InputArray[index].Y = this.MaxValueDivider;

                AdjustedArray[index].X = index * this.indexMultipler;
                AdjustedArray[index].Y = this.translateYPoint(value, this.DilationMultiplier, this.TranslationValue);



                if (index == this.AdjustedArray.Length - 1)
                {
                    InputArray[InputArray.Length].Y = InputArray[0].Y;

                    AdjustedArray[InputArray.Length].X = (InputArray.Length) * this.indexMultipler;
                    AdjustedArray[InputArray.Length].Y = AdjustedArray[0].Y;
                }
                else //This part added to more cleanly handle lost data
                {
                    AdjustedArray[index + 1].X = AdjustedArray[index].X;
                    AdjustedArray[index + 1].Y = AdjustedArray[index].Y;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool dialationChanged = false;
        public void AddValueNew(float value, int index)
        {
            try
            {
                if (currentIndex >= InputArray.Length - 1)
                {
                    return;
                }
                ActualValues[index] = value;
                InputArray[index].Y = ActualValues[index] / this.MaxValueDivider;

                AdjustedArray[index].X = index * this.indexMultipler;
                AdjustedArray[index].Y = this.translateYPoint(InputArray[index].Y, this.DilationMultiplier, this.TranslationValue);

                //if the wave gets bigger than the graph

                if (AdjustedArray[index].Y < 0)
                {
                    this.DilationMultiplier /= 10f;

                    for (int i = 0; i <= index; ++i)
                    {
                        AdjustedArray[i].Y = this.translateYPoint(InputArray[i].Y, this.DilationMultiplier, this.TranslationValue); //InputArray[index].Y
                    }

                    this.dialationChanged = true;
                }


                if (index == this.AdjustedArray.Length - 1)
                {
                    InputArray[InputArray.Length].Y = InputArray[0].Y;

                    AdjustedArray[InputArray.Length].X = (InputArray.Length) * this.indexMultipler;
                    AdjustedArray[InputArray.Length].Y = AdjustedArray[0].Y;
                    if (this.dialationChanged)
                        this.DilationMultiplier *= 10;
                    this.dialationChanged = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void shiftValuesLeft()
        {
            for (int i = 1; i < InputArray.Length; ++i)
            {
                InputArray[i - 1].Y = InputArray[i].Y;
                AdjustedArray[i - 1].Y = AdjustedArray[i].Y;
            }
        }

        private void setPenColor(PhasorTypes pT)
        {
            this.FunctionPen.Color = RelayModeFunctions.GetPhaseColor(pT);
        }

        internal void ClearArray()
        {
            for (int i = 0; i < this.InputArray.Length; ++i)
            {
                this.InputArray[i].Y = 0;
            }
        }

        public float GetRMS()
        {
            float value = 0;
            for (int i = 0; i < 128; ++i)
            {
                value += this.ActualValues[i] * this.ActualValues[i];
            }

            value = value / 128;
            value = (float)Math.Sqrt(value);

            return value;
        }

        public float GetRMS(uint startingIndex)
        {
            float real = 0, imaginary = 0;

            for (uint i = startingIndex; i < 128 + startingIndex; ++i)
            {
                //real += this.ActualValues[i] * this.ActualValues[i];

                real += this.ActualValues[i] * this.sineLUT[i % 128];
                imaginary += this.ActualValues[i] * this.sineLUT[(i + 32) % 128];
            }

            real = real / 128f;
            imaginary = imaginary / 128f;

            real = real / 0.70710678118654752440084436210485f;
            imaginary = imaginary / 0.70710678118654752440084436210485f;

            real = (float)Math.Sqrt(Math.Pow(real, 2) + Math.Pow(imaginary, 2));//Math.Pow(real, 2) + Math.Pow(imaginary,2));
            //real = (float)Math.Sqrt(real / 128f);
            return real;
        }

        public void ClearAllValues()
        {
            for (int i = 0; i < AdjustedArray.Length; ++i)
            {
                AdjustedArray[i] = new PointF();
                InputArray[i] = new PointF();
            }
            this.DilationMultiplier = 10000000f;
        }

        public void UpdateAllValues()
        {
            for (int index = 0; index < this.ActualValues.Length - 1; ++index)
            {
                InputArray[index].Y = ActualValues[index] / this.MaxValueDivider;

                AdjustedArray[index].X = index * this.indexMultipler;
                AdjustedArray[index].Y = this.translateYPoint(InputArray[index].Y, this.DilationMultiplier, this.TranslationValue);

                //if the wave gets bigger than the graph

                if (AdjustedArray[index].Y < 0)
                {
                    this.DilationMultiplier /= 10f;

                    for (int i = 0; i <= index; ++i)
                    {
                        AdjustedArray[i].Y = this.translateYPoint(InputArray[i].Y, this.DilationMultiplier, this.TranslationValue);
                    }

                    this.dialationChanged = true;
                }

                if (index == this.AdjustedArray.Length - 1)
                {
                    InputArray[InputArray.Length].Y = InputArray[0].Y;

                    AdjustedArray[InputArray.Length].X = (InputArray.Length) * this.indexMultipler;
                    AdjustedArray[InputArray.Length].Y = AdjustedArray[0].Y;
                    if (this.dialationChanged)
                        this.DilationMultiplier *= 10;
                    this.dialationChanged = false;
                }
            }
        }

        public void Zero()
        {
            float average = 0;

            for (int i = 0; i < 128; ++i)
            {
                average += ActualValues[i];
            }

            average = average / 128f;

            for (int i = 0; i < ActualValues.Length; ++i)
            {
                ActualValues[i] -= average;
            }
            this.UpdateAllValues();
        }
    }
}
