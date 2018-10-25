using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RelayControlLibrary;
using System.Windows.Forms;

namespace SineDisplayGraph
{
    public class PhasorDefinition
    {
        public DateTime RevisionNumber = new DateTime();

        public PhasorDefinition(PhasorTypes pT)
        {
            this.Type = pT;
            this.Enabled = true;
        }
        public PhasorTypes Type
        {
            get { return this.type; }
            set
            {
                this.type = value;
                this.phasorPen.Width = 4;
                switch (value)
                {
                    case PhasorTypes.IA:
                        this.phasorPen.Color = Color.Pink;
                        break;
                    case PhasorTypes.IB:
                        this.phasorPen.Color = Color.LightGreen;
                        break;
                    case PhasorTypes.IC:
                        this.phasorPen.Color = Color.LightBlue;
                        break;
                    case PhasorTypes.IN:
                        this.phasorPen.Color = Color.DarkRed;
                        break;
                    case PhasorTypes.IP:
                        this.phasorPen.Color = Color.DarkSeaGreen;
                        break;
                    case PhasorTypes.PA:
                        this.phasorPen.Color = Color.DarkRed;
                        break;
                    case PhasorTypes.PB:
                        this.phasorPen.Color = Color.DarkGreen;
                        break;
                    case PhasorTypes.PC:
                        this.phasorPen.Color = Color.DarkBlue;
                        break;
                    case PhasorTypes.PT:
                        this.phasorPen.Color = Color.DarkOrange;
                        break;
                    case PhasorTypes.VnA:
                        this.phasorPen.Color = Color.DarkRed;
                        break;
                    case PhasorTypes.VnB:
                        this.phasorPen.Color = Color.DarkGreen;
                        break;
                    case PhasorTypes.VnC:
                        this.phasorPen.Color = Color.Navy;
                        break;
                    case PhasorTypes.VtA:
                        this.phasorPen.Color = Color.Red;
                        break;
                    case PhasorTypes.VtB:
                        this.phasorPen.Color = Color.Green;
                        break;
                    case PhasorTypes.VtC:
                        this.phasorPen.Color = Color.Blue;
                        break;
                    case PhasorTypes.VtN:
                    case PhasorTypes.VnN:
                        this.phasorPen.Color = Color.Black;
                        break;
                    case PhasorTypes.VtP:
                    case PhasorTypes.VnP:
                        this.phasorPen.Color = Color.SteelBlue;
                        break;
                    case PhasorTypes.Ieff:
                        this.phasorPen.Color = Color.Purple;
                        break;
                    case PhasorTypes.VdN:
                        this.phasorPen.Color = Color.Pink;
                        break;
                    case PhasorTypes.VdP:
                        this.phasorPen.Color = Color.OrangeRed;
                        break;
                    case PhasorTypes.VdA:
                        this.phasorPen.Color = Color.Red;
                        break;
                    case PhasorTypes.VdB:
                        this.phasorPen.Color = Color.Green;
                        break;
                    case PhasorTypes.VdC:
                        this.phasorPen.Color = Color.Blue;
                        break;
                    default:
                        this.phasorPen.Color = Color.Purple;
                        break;
                }
            }
        }
        public PointF EndPoint
        {
            get { return this.endPoint; }
            set
            {
                this.endPoint = value;
                this.translatedEndPoint = this.translatePoint(value);
            }
        }
        public PointF TranslatedEndPoint
        {
            get { return translatedEndPoint; }
        }
        public PointF BeginPoint
        {
            get { return this.beginPoint; }
            set
            {
                this.beginPoint = value;
                this.translatedBeginPoint = this.translatePoint(value);
            }
        }
        public bool Enabled = false;
        public PointF TranslatedBeginPoint
        {
            get { return this.translatedBeginPoint; }
        }
        public Pen phasorPen = new Pen(Color.Red);
        public float YDilationMultiplier
        {
            get { return this.yDilationMultiplier; }
            set
            {
                this.yDilationMultiplier = value;
                this.translatePoints();
            }
        }
        public float XDilationMultiplier
        {
            get { return this.xDilationMultiplier; }
            set
            {
                this.xDilationMultiplier = value;
                this.translatePoints();
            }
        }
        public float YTranslationValue
        {
            get { return yTranslationValue; }
            set
            {
                this.yTranslationValue = value;
                this.translatePoints();
            }
        }
        public float XTranslationValue
        {
            get { return this.xTranslationValue; }
            set
            {
                this.xTranslationValue = value;
                this.translatePoints();
            }
        }
        public float RealValue
        {
            get { return this.realValue; }
            set
            {
                this.realValue = value;
                if (float.IsNaN(this.realValue) || float.IsNaN(this.imaginaryValue))
                    return;
                this.setRMS();
            }
        }
        public float ImaginaryValue
        {
            get { return this.imaginaryValue; }
            set
            {
                this.imaginaryValue = value;
                if (float.IsNaN(this.realValue) || float.IsNaN(this.imaginaryValue))
                    return;
                this.setRMS();
            }
        }
        public float RMSValue;
        public float MaxValue;

        private PhasorTypes type;
        private PointF endPoint;
        private PointF translatedEndPoint;
        private PointF beginPoint;
        private PointF translatedBeginPoint;
        private float yDilationMultiplier;
        private float xDilationMultiplier;
        private float yTranslationValue;
        private float xTranslationValue;
        private float realValue;
        private float imaginaryValue;

        public PhasorDefinition()
        {
        }

        private void setRMS()
        {
            this.RMSValue = (float)Math.Sqrt((double)(this.realValue * this.realValue + this.imaginaryValue * this.imaginaryValue));
        }

        private void translatePoints()
        {
            this.translatedBeginPoint = this.translatePoint(beginPoint);
            this.translatedEndPoint = this.translatePoint(endPoint);
        }
        private PointF translatePoint(PointF point)
        {
            PointF returnPoint = new PointF();

            returnPoint.X = this.translateXPoint(point.X);
            returnPoint.Y = this.translateYValue(point.Y);

            return returnPoint;
        }

        private float translateXPoint(float XValue)
        {
            return this.translateValue(XValue, this.xDilationMultiplier, this.xTranslationValue);
        }

        private float translateYValue(float YValue)
        {
            return this.translateValue(YValue, this.yDilationMultiplier, this.yTranslationValue);
        }

        private float translateValue(float value, float dilator, float translator)
        {
            return value * dilator + translator;
        }

        public TextBox RMSBox;
        public TextBox AngleBox;
        public TextBox RealBox;

        public void DialateEndPoint(float f)
        {
            this.endPoint.X *= f;
            this.endPoint.Y *= f;
            this.translatedEndPoint = this.translatePoint(this.endPoint);
        }

        public void SetEndPoint()
        {
            float tempReal, tempImaginary;

            tempReal = (this.RealValue / this.MaxValue) * 100f;
            tempImaginary = (this.ImaginaryValue / this.MaxValue) * 100f;

            this.EndPoint = new PointF(tempReal, tempImaginary);
        }
    }
}
