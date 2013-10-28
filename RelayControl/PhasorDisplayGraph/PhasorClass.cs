using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PhasorDisplayGraph
{
    public class PhasorClass
    {
        public PhasorClass(Pen phasorPen, Point endPoint, Size graphSize, string phasorName)
        {
            this.PhasorPen = phasorPen;
            this.GraphSize = graphSize;
            this.ActualPhasorPoint = endPoint;
            this.PhasorName = phasorName;
        }

        public string PhasorName;

        private Size graphSize;
        public Size GraphSize
        {
            get
            {
                return this.graphSize;
            }
            set
            {
                this.graphSize = value;
                this.fullRange = this.graphSize.Width / 2;
            }
        }

        public Pen PhasorPen;

        private Point actualPhasorPoint;
        public Point ActualPhasorPoint
        {
            get
            {
                return this.actualPhasorPoint;
            }
            set
            {
                this.actualPhasorPoint = value;
                this.TranslatedPhasorPoint.X = (int)this.translateX(this.actualPhasorPoint.X);
                this.TranslatedPhasorPoint.Y = (int)this.translateY(this.actualPhasorPoint.Y);
            }
        }

        public Point TranslatedPhasorPoint;

        private double fullRange;

        private double translateY(double y)
        {
            //double test = ((-y / 100d) * ((double)this.graphSize.Height / 2d) + this.graphSize.Height / 2);
            if (y < 100 && y > -100)
                return (-y/100d * (this.graphSize.Height / 2d) + (double)this.graphSize.Height / 2d);
            else if (y > 100)
                return -this.graphSize.Height / 2;
            else
                return this.graphSize.Height / 2;

        }

        private double translateX(double x)
        {
            double test = (x / 100d * ((double)this.GraphSize.Width / 2d)) + (double)this.graphSize.Width / 2d;
            if (x < 100 && x > -100)
                return (x / 100 * ((double)this.GraphSize.Width / 2d)) + (double)this.graphSize.Width / 2d;
            else if (x > 100)
                return this.graphSize.Width / 2;
            else
                return -this.graphSize.Width / 2;
        }

        //forces a translation of the phasor point.
        public void TranslatePhasorPoint()
        {
            this.TranslatedPhasorPoint.X = (int)this.translateX(this.actualPhasorPoint.X);
            this.TranslatedPhasorPoint.Y = (int)this.translateY(this.actualPhasorPoint.Y);
        }
    }

    public class Segment
    {
        public Segment(SizeF graphSize)
        {
            xTranslation = graphSize.Width / 2f;
            yTranslation = -graphSize.Height / 2f;
        }

        private float xTranslation;
        private float yTranslation;

        PointF InputBeginPoint
        {
            set
            {
                this.BeginPoint = TranslatePoint(value);
            }
        }
        PointF InputEndPointer
        {
            set
            {
                this.EndPoint = TranslatePoint(value);
            }
        }

        PointF BeginPoint;
        PointF EndPoint;

        private PointF TranslatePoint(PointF point)
        {
            point.X += xTranslation;
            point.Y += yTranslation;

            return point;
        }
    }
}
