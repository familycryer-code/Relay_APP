using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace PhasorDisplayGraph
{
    public partial class PhasorGraph : UserControl
    {
        private Point origin;
        private Matrix originTranslate = new Matrix();

        private Pen graphPen = new Pen(Color.Black, 1);
        private Rectangle componentRectangle;  //Defines the edge of the rectangle in which the code exists.
        public ArrayList DrawObjects;

        private Point midwayHeightLeft;
        private Point midwayHeightRight;
        private Point midwayWidthTop;
        private Point midwayWidthBottom;

        public PhasorGraph()
        {
            InitializeComponent();
            
            this.defineOrigin();
            //define the cross in the box
            this.defineCross();
            this.defineEllipse();

            this.BackColor = Color.White;

        }


        #region Drawing Functions

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics myGraphics = e.Graphics;


            this.drawGraph(e);
            //myGraphics.Transform = this.originTranslate;

            /*
            for (int i = 0; i < DrawObjects.Count; i++)
            {
                
            }
            */
            //myGraphics.DrawArc(this.graphPen, -5, -5f, 10f, 10f, 0, 360);
            //myGraphics.DrawArc(new Pen(Color.Red), -50, -50f, 100f, 100f, 85, 5);
            //myGraphics.DrawArc(new Pen(Color.Cyan), -40, -50, 100f, 100f, 95, 5);
            //myGraphics.DrawLine(this.graphPen, new PointF(0, 0), new PointF(8.7, 100));
            //myGraphics.DrawLine(this.graphPen, new PointF(10, 0), new PointF(5, 100));
            //myGraphics.
            /*
            foreach (PhasorClass phasor in this.PhasorsToDraw)
            {
                myGraphics.DrawLine(phasor.PhasorPen, phasor.TranslatedPhasorPoint, this.origin);
            }
            */
        }



        private void drawGraph(PaintEventArgs e)
        {
            Graphics graphGraphics = e.Graphics;


            graphGraphics.DrawEllipse(this.graphPen, this.componentRectangle);
            
            graphGraphics.DrawLine(this.graphPen, midwayHeightLeft, midwayHeightRight);
            graphGraphics.DrawLine(this.graphPen, midwayWidthTop, midwayWidthBottom);
        }
        
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.defineOrigin();
            this.defineCross();
            this.defineEllipse();

        }

        #endregion

        #region Graph Defining Functions

        private void defineOrigin()
        {
            this.origin = new Point(this.Size.Width / 2, this.Size.Height / 2);

            this.originTranslate.Reset();                                       //set to identity matrix
            this.originTranslate.Translate(this.origin.X, this.origin.Y);       //translate to the origin.
            this.originTranslate.Scale(1, -1);                                  //make so positive y goes up
            this.originTranslate.Scale(((float)this.Size.Width / 200f),  ((float)this.Size.Height / 200f));
                                                                                //Scale it so 100 = max
        }

        private void defineCross()
        {
            midwayHeightLeft = new Point(this.componentRectangle.X, this.componentRectangle.Y + (this.Size.Height / 2));
            midwayHeightRight = new Point(this.componentRectangle.X + this.Size.Width, this.componentRectangle.Y + (this.Size.Height / 2));
            midwayWidthTop = new Point(this.componentRectangle.X + (this.Size.Width / 2), this.componentRectangle.Y);
            midwayWidthBottom = new Point(this.componentRectangle.X + (this.Size.Width / 2), this.componentRectangle.Y + this.Size.Height);
        }

        private void defineEllipse()
        {
            componentRectangle = new Rectangle(0, 0, this.Size.Width - 1, this.Size.Height - 1);
        }

        #endregion

    }
}