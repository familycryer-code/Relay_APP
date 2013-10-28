using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using RelayControlLibrary;

namespace SineDisplayGraph
{
    public partial class PhasorGraph : UserControl
    {
        public PhasorGraph()
        {
            InitializeComponent();
            this.OnResize(new EventArgs());

            XAxisValues[0] = "10";
            XAxisValues[1] = "20";
            XAxisValues[2] = "30";
            XAxisValues[3] = "40";

            YAxisValues[0] = "1";
            YAxisValues[1] = "2";
            YAxisValues[2] = "3";
            YAxisValues[3] = "4";
        }

        public Size GraphSize;
        private RectangleF graphBoundries;
        private PointF midwayTop;
        private PointF midwayBottom;
        private PointF midwayLeft;
        private PointF midwayRight;
        public string[] XAxisValues = new string[4];
        public string[] YAxisValues = new string[4];
        private PointF[] xAxisPoints = new PointF[4];
        private PointF[] yAxisPoints = new PointF[4];
        private float hashMarkWidth = 6;
        private Font hashFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
        private SolidBrush hashBrush = new SolidBrush(Color.Black);

        private Pen graphPen = new Pen(Color.Black, 1);

        public ArrayList phasorsToDraw = new ArrayList();


        public void AddPhasor(PhasorDefinition pD)
        {
            this.phasorsToDraw.Add(pD);
            pD.YDilationMultiplier = -(float)(this.Size.Height - 1) / 200f; //200F
            pD.YTranslationValue = (float)(this.Size.Height - 1) / 2f;
            pD.XDilationMultiplier = (float)(this.Size.Width - 1) / 200f;
            pD.XTranslationValue = (float)(this.Size.Width - 1) / 2f;
        }

        public void ClearPhasors()
        {
            this.phasorsToDraw.Clear();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.drawGraph(e);
            Graphics onPaintGraphics = e.Graphics;

            if (phasorsToDraw != null)
            {
                foreach (PhasorDefinition pD in phasorsToDraw)
                {
                    try
                    {
                        if(pD.Enabled)
                            onPaintGraphics.DrawLine(pD.phasorPen, pD.TranslatedBeginPoint, pD.TranslatedEndPoint);
                    }
                    catch
                    {
                        pD.EndPoint = new PointF(0f, 0f);
                        onPaintGraphics.DrawLine(pD.phasorPen, pD.TranslatedBeginPoint, pD.TranslatedEndPoint);
                    }
                }
            }
        }

        private void drawGraph(PaintEventArgs e)
        {
            Graphics graphGraphics = e.Graphics;

            graphGraphics.FillEllipse(new SolidBrush(Color.White), this.graphBoundries);
            graphGraphics.DrawLine(graphPen, this.midwayBottom, this.midwayTop);
            graphGraphics.DrawLine(graphPen, this.midwayRight, this.midwayLeft);
            graphGraphics.DrawEllipse(graphPen, this.graphBoundries);
           
            graphGraphics.DrawLine(graphPen, this.yAxisPoints[0], new PointF(this.yAxisPoints[0].X + this.hashMarkWidth, this.yAxisPoints[0].Y));
            graphGraphics.DrawLine(graphPen, this.yAxisPoints[1], new PointF(this.yAxisPoints[1].X + this.hashMarkWidth, this.yAxisPoints[1].Y));
            graphGraphics.DrawLine(graphPen, this.yAxisPoints[2], new PointF(this.yAxisPoints[2].X + this.hashMarkWidth, this.yAxisPoints[2].Y));
            graphGraphics.DrawLine(graphPen, this.yAxisPoints[3], new PointF(this.yAxisPoints[3].X + this.hashMarkWidth, this.yAxisPoints[3].Y));
            graphGraphics.DrawString(YAxisValues[0], this.hashFont, this.hashBrush, new PointF(this.yAxisPoints[0].X + this.hashMarkWidth, this.yAxisPoints[0].Y - this.hashFont.Height / 2));
            graphGraphics.DrawString(YAxisValues[1], this.hashFont, this.hashBrush, new PointF(this.yAxisPoints[1].X + this.hashMarkWidth, this.yAxisPoints[1].Y - this.hashFont.Height / 2));
            graphGraphics.DrawString(YAxisValues[2], this.hashFont, this.hashBrush, new PointF(this.yAxisPoints[2].X + this.hashMarkWidth, this.yAxisPoints[2].Y - this.hashFont.Height / 2));
            graphGraphics.DrawString(YAxisValues[3], this.hashFont, this.hashBrush, new PointF(this.yAxisPoints[3].X + this.hashMarkWidth, this.yAxisPoints[3].Y - this.hashFont.Height / 2));

            graphGraphics.DrawLine(graphPen, this.xAxisPoints[0], new PointF(this.xAxisPoints[0].X, this.xAxisPoints[0].Y + this.hashMarkWidth));
            graphGraphics.DrawLine(graphPen, this.xAxisPoints[1], new PointF(this.xAxisPoints[1].X, this.xAxisPoints[1].Y + this.hashMarkWidth));
            graphGraphics.DrawLine(graphPen, this.xAxisPoints[2], new PointF(this.xAxisPoints[2].X, this.xAxisPoints[2].Y + this.hashMarkWidth));
            graphGraphics.DrawLine(graphPen, this.xAxisPoints[3], new PointF(this.xAxisPoints[3].X, this.xAxisPoints[3].Y + this.hashMarkWidth));
            graphGraphics.DrawString(XAxisValues[0], this.hashFont, this.hashBrush, new PointF(this.xAxisPoints[0].X - this.hashFont.Height / 2, this.xAxisPoints[0].Y + this.hashMarkWidth));
            graphGraphics.DrawString(XAxisValues[1], this.hashFont, this.hashBrush, new PointF(this.xAxisPoints[1].X - this.hashFont.Height / 2, this.xAxisPoints[1].Y + this.hashMarkWidth));
            graphGraphics.DrawString(XAxisValues[2], this.hashFont, this.hashBrush, new PointF(this.xAxisPoints[2].X - this.hashFont.Height / 2, this.xAxisPoints[2].Y + this.hashMarkWidth));
            graphGraphics.DrawString(XAxisValues[3], this.hashFont, this.hashBrush, new PointF(this.xAxisPoints[3].X - this.hashFont.Height / 2, this.xAxisPoints[3].Y + this.hashMarkWidth));

        }

        protected override void OnResize(EventArgs e)
        {
            this.GraphSize = new Size(this.Size.Width - 1, this.Size.Height - 1);

            float midwayHeight = this.Height / 2;
            float midwayWidth = this.Width / 2;
            Point upperLeftCorner = new Point(0, 0);

            this.midwayBottom = new PointF(midwayWidth, this.GraphSize.Height);
            this.midwayTop = new PointF(midwayWidth, 0);
            this.midwayLeft = new PointF(0, midwayHeight);
            this.midwayRight = new PointF(this.GraphSize.Width, midwayHeight);

            this.graphBoundries = new RectangleF(upperLeftCorner, this.GraphSize);

            yAxisPoints[0].X = midwayWidth - this.hashMarkWidth / 2;
            yAxisPoints[1].X = midwayWidth - this.hashMarkWidth / 2;
            yAxisPoints[2].X = midwayWidth - this.hashMarkWidth / 2;
            yAxisPoints[3].X = midwayWidth - this.hashMarkWidth / 2;

            yAxisPoints[0].Y = midwayHeight + .2f * midwayHeight;
            yAxisPoints[1].Y = midwayHeight + .4f * midwayHeight;   
            yAxisPoints[2].Y = midwayHeight + .6f * midwayHeight;
            yAxisPoints[3].Y = midwayHeight + .8f * midwayHeight;

            xAxisPoints[0].Y = midwayHeight - this.hashMarkWidth / 2;
            xAxisPoints[1].Y = midwayHeight - this.hashMarkWidth / 2;
            xAxisPoints[2].Y = midwayHeight - this.hashMarkWidth / 2;
            xAxisPoints[3].Y = midwayHeight - this.hashMarkWidth / 2;

            xAxisPoints[0].X = midwayWidth + .2f * midwayWidth;
            xAxisPoints[1].X = midwayWidth + .4f * midwayWidth;
            xAxisPoints[2].X = midwayWidth + .6f * midwayWidth;
            xAxisPoints[3].X = midwayWidth + .8f * midwayWidth;

            base.OnResize(e);
        }
    }
}
