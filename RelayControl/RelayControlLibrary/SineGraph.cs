using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;

namespace RelayControlLibrary
{
    public partial class SineGraph : UserControl
    {
        public SineGraph()
        {
            InitializeComponent();
            this.myInitialization();
            this.ScrollEnabled = true;

        }


        public Int32 ClickedCycleNumber = -1;
        public ArrayList sineWavesToDraw = new ArrayList();
        public int[] currentIndex;
        public Int32 CTRatio = 320;
        public ProtectorVoltage ProtectorVoltage { get; set; } =
            ProtectorVoltages.GetVoltage();
        public string GraphName
        {
            get { return this.labelType.Text; }
            set
            {
                this.labelType.Text = value;
                this.labelType.ForeColor = RelayModeFunctions.GetPhaseColor(RelayModeFunctions.PhasorTypeFrom(value));
            }
        }
        public bool ScrollEnabled
        {
            get { return this.scrollEnabled; }
            set
            {
                this.scrollEnabled = value;
                if (value)
                {
                    this.hScrollBar1.Height = 12;
                }
                else
                {
                    this.hScrollBar1.Height = 0;
                }
                this.OnResize(new EventArgs());
            }
        }
        public PhasorTypes Type = PhasorTypes.VnA;

        private int minIndexToDraw = 1;
        private int maxIndexToDraw = 128;
        private int totalPoints = 129;
        private Size GraphSize;
        private bool scrollEnabled;
        private ToolTip tT = new ToolTip();
        private Rectangle graphBoundries;
        private PointF midwayTop;
        private PointF midwayBottom;
        private PointF midwayLeft;
        private PointF midwayRight;
        private Pen graphPen = new Pen(Color.Black, 1);

        private void myInitialization()
        {
            this.MouseWheel += new MouseEventHandler(SineGraph_MouseWheel);
            this.DoubleBuffered = true;
        }

        public void addNewSineWave(PhasorTypes pT)
        {
            SineWaveDefinition addedSineWave;
            this.Type = pT;
            this.sineWavesToDraw.Add(new SineWaveDefinition(pT, 129, new Pen(RelayModeFunctions.GetPhaseColor(pT), .1f)));
            addedSineWave = (SineWaveDefinition)this.sineWavesToDraw[this.sineWavesToDraw.Count - 1];

            if (this.Type == PhasorTypes.IA || this.Type == PhasorTypes.IB || this.Type == PhasorTypes.IC)
                addedSineWave.DilationMultiplier = -(float)(this.GraphSize.Height - 1) / 5f; //200F
            else
                addedSineWave.DilationMultiplier = -(float)(this.GraphSize.Height - 1) / 200f; //200F
            addedSineWave.TranslationValue = (float)(this.GraphSize.Height - 1) / 2f;

            this.indexMultiplier = addedSineWave.IndexMultiplier = (float)(this.GraphSize.Width - 1) / 128f;
            addedSineWave.Phase = pT;

            this.maxIndexToDraw = 129;
            this.totalPoints = 129;
            this.PointsToDraw = 129;
        }

        public void AddNewSineWave(PhasorTypes pT, int arraySize)
        {
            SineWaveDefinition addedSineWave;
            this.Type = pT;
            this.sineWavesToDraw.Add(new SineWaveDefinition(pT, arraySize + 1, new Pen(RelayModeFunctions.GetPhaseColor(pT), .1f)));
            addedSineWave = (SineWaveDefinition)this.sineWavesToDraw[this.sineWavesToDraw.Count - 1];

            addedSineWave.TranslationValue = (float)(this.GraphSize.Height - 1) / 2f;
            this.indexMultiplier = addedSineWave.IndexMultiplier = (float)(this.Size.Width - 1) / (float)arraySize;//128f;
            addedSineWave.Phase = pT;

            this.maxIndexToDraw = arraySize;
            this.totalPoints = arraySize;
            this.PointsToDraw = arraySize;

            if (pT == PhasorTypes.IA || pT == PhasorTypes.IB || pT == PhasorTypes.IC)
            {
                addedSineWave.MaxValueDivider = 2.12f;
                addedSineWave.DilationMultiplier = -(float)(this.GraphSize.Height - 1) / 5f; //200F
            }
            else
            {
                addedSineWave.DilationMultiplier = -(float)(this.GraphSize.Height - 1) / 200f; //200F
                addedSineWave.MaxValueDivider = 2.54f;
            }
        }

        #region Drawing

        private float indexMultiplier;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics onPaintGraphics = e.Graphics;
            float temp = 1;
            int shadingMinIndex = -1;

            if (this.ClickedCycleNumber >= 0)
            {
                shadingMinIndex = this.ClickedCycleNumber << 7;
            }

            if (this.ScrollEnabled)
            {
                Matrix originalTransform = e.Graphics.Transform;
                this.drawGraph(e);
                Matrix zoomMatrix = new Matrix();


                temp = (float)this.totalPoints / (float)this.pointsToDraw;

                zoomMatrix.Scale(temp, 1f);
                zoomMatrix.Translate(-(float)this.minIndexToDraw + 1f, 0);


                e.Graphics.Transform = zoomMatrix;
            }
            else
            {
                this.drawGraph(e);
            }
            try
            {
                if (sineWavesToDraw != null)
                {
                    foreach (SineWaveDefinition s in sineWavesToDraw)
                    {
                        if (s.Enabled)
                        {
                            for (int i = 1; i < s.AdjustedArray.Length; ++i)
                            {
                                if (!s.AdjustedArray[i].IsEmpty)
                                {
                                    onPaintGraphics.DrawLine(s.FunctionPen, s.AdjustedArray[i - 1], s.AdjustedArray[i]);

                                    if (i == s.AdjustedArray.Length >> 1)
                                    {
                                        if (this.ShowEventLine)
                                        {
                                            onPaintGraphics.DrawLine(new Pen(Color.DarkRed, 1), new PointF(s.AdjustedArray[i].X, 0), new PointF(s.AdjustedArray[i].X, this.GraphSize.Height));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void drawGraph(PaintEventArgs e)
        {
            Graphics graphGraphics = e.Graphics;

            //draw the boundries of the graph
            graphGraphics.DrawRectangle(graphPen, this.graphBoundries);
            if (!this.ShowEventLine)
                graphGraphics.DrawLine(graphPen, this.midwayBottom, this.midwayTop);
            graphGraphics.DrawLine(graphPen, this.midwayRight, this.midwayLeft);

        }

        protected override void OnResize(EventArgs e)
        {
            this.GraphSize = new Size(this.Size.Width - 1, this.Size.Height - 1 - this.hScrollBar1.Height);

            float midwayHeight = (this.Height - this.hScrollBar1.Height) / 2 - 1;
            float midwayWidth = this.Width / 2;

            Point upperLeftCorner = new Point(0, 0);

            this.midwayBottom = new PointF(midwayWidth, this.GraphSize.Height);
            this.midwayTop = new PointF(midwayWidth, 0);
            this.midwayLeft = new PointF(0, midwayHeight);
            this.midwayRight = new PointF(this.GraphSize.Width, midwayHeight);

            this.graphBoundries = new Rectangle(upperLeftCorner, this.GraphSize);

            //Resizing scroll bar
            this.hScrollBar1.Width = this.GraphSize.Width;
            this.hScrollBar1.Location = new Point(0, this.Size.Height - this.hScrollBar1.Height);

            //Fix all the dialation multipliers.
            if (sineWavesToDraw != null)
            {
                foreach (SineWaveDefinition s in sineWavesToDraw)
                {
                    if (this.Type == PhasorTypes.IA || this.Type == PhasorTypes.IB || this.Type == PhasorTypes.IC)
                        s.DilationMultiplier = (float)(this.GraphSize.Height - 1) / 5f;
                    else
                        s.DilationMultiplier = (float)(this.GraphSize.Height - 1) / 200f;
                    s.TranslationValue = (float)(this.GraphSize.Height - 1) / 2f;
                    s.IndexMultiplier = (float)(this.Size.Width) / (float)(this.maxIndexToDraw - 1);
                    s.UpdateAllValues();
                }
            }
            base.OnResize(e);
        }

        #endregion

        public SineWaveDefinition GetWave(PhasorTypes phasorTypes)
        {
            foreach (SineWaveDefinition s in this.sineWavesToDraw)
            {
                if (s.Phase == phasorTypes)
                {
                    return s;
                }
            }
            return null;
        }

        public void ClearData()
        {
            this.sineWavesToDraw = new ArrayList();
        }

        public void ScaleCurrentValues(float p)
        {
            try
            {
                SineWaveDefinition sWD;
                for (int i = 0; i < 128; ++i)
                {
                    for (int j = 0; j < this.sineWavesToDraw.Count; ++j)
                    {
                        sWD = (SineWaveDefinition)this.sineWavesToDraw[j];
                        if (sWD.Phase == PhasorTypes.IA || sWD.Phase == PhasorTypes.IB || sWD.Phase == PhasorTypes.IC)
                        {
                            sWD.AddValue(sWD.InputArray[i].Y * p, i);
                        }
                    }
                }
                this.Invalidate();
            }
            catch
            {
                throw new Exception("Error Scaling Current Values");
            }
        }

        /// <summary>
        /// Returns the request cycle of data
        /// </summary>
        /// <param name="cycleNumber">cycle number to retrieve</param>
        /// <returns>Actual Float Values of wave</returns>
        public float[] GetSingleCycle(int cycleNumber)
        {
            SineWaveDefinition workingSineWave;
            float[] returnArray = new float[128];
            try
            {
                workingSineWave = (SineWaveDefinition)this.sineWavesToDraw[0];
            }
            catch
            {
                MessageBox.Show("No Sine Wave Yet");
                return null;
            }

            if (cycleNumber < 0 || cycleNumber * 128 >= workingSineWave.ActualValues.Length - 128)
            {
                MessageBox.Show("Bad Cycle Number");
                return null;
            }

            for (int i = 0; i < 128; ++i)
            {
                returnArray[i] = workingSineWave.ActualValues[cycleNumber * 128 + i];
            }

            return returnArray;
        }

        #region Zoom Control
        private int pointsToDraw;
        public int PointsToDraw
        {
            get { return this.pointsToDraw; }
            set
            {
                double maximumMinIndex;

                this.pointsToDraw = value;
                maximumMinIndex = (double)this.Width - ((double)this.PointsToDraw / (double)this.totalPoints * (double)this.Width);

                if (this.minIndexToDraw > maximumMinIndex)
                {
                    this.minIndexToDraw = (int)maximumMinIndex;
                }

            }
        }

        void SineGraph_MouseWheel(object sender, MouseEventArgs e)
        {
            this.ExternalMouseWheel(new object(), e);
            this.MouseWheeled(sender, e);
        }

        public void ExternalMouseWheel(object sender, MouseEventArgs e)
        {
            if (sender == this)   //Put in to stop it from happening twice
                return;

            int temp = e.Delta;
            if (!this.ScrollEnabled)
                return;

            if (e.Delta > 0)
            {
                this.zoomIn();
            }
            else
            {
                this.zoomOut();
            }
            this.Invalidate();

            this.hScrollBar1.LargeChange = (int)((double)this.PointsToDraw / (double)this.totalPoints * 100d);
        }

        public delegate void MouseWheeledHandler(object sender, MouseEventArgs e);
        public event MouseWheeledHandler MouseWheeledEvent;

        private void MouseWheeled(object sender, MouseEventArgs e)
        {
            if (MouseWheeledEvent != null)
                this.MouseWheeledEvent(sender, e);
        }

        private void zoomIn()
        {
            if (this.PointsToDraw <= 1000)
            {
                this.PointsToDraw = 500;
            }
            else
            {
                this.PointsToDraw -= 500;
            }

        }

        private void zoomOut()
        {
            if (this.PointsToDraw >= this.totalPoints - 500)
            {
                this.PointsToDraw = this.totalPoints;
            }
            else
            {
                this.PointsToDraw += 500;
            }
        }

        #endregion


        #region Event Control

        public bool ShowEventLine = true;

        #endregion


        #region Mouse & Scroll Event Handlers

        private void SineGraph_MouseLeave(object sender, EventArgs e)
        {
            this.tT.Hide(this);
        }

        public delegate void GraphLeftClickedHandler(object sender, EventArgs e);
        public event GraphLeftClickedHandler GraphLeftClicked;

        public delegate void GraphRightClickedHandler(object sender, SineGraphEventArgs sGEA);
        public event GraphRightClickedHandler GraphRightClicked;

        private void leftClicked()      //To send the data to the 
        {
            if (GraphLeftClicked != null)
                this.GraphLeftClicked(this, new EventArgs());
        }


        private void rightClicked(int cycleClicked)
        {
            SineGraphEventArgs sGEA = new SineGraphEventArgs();
            sGEA.ClickedCycleNumber = cycleClicked;

            if (GraphRightClicked != null)
                this.GraphRightClicked(this, sGEA);
        }

        private void SineGraph_MouseClick(object sender, MouseEventArgs e)
        {
            float pointsPerX;
            int closestX;

            pointsPerX = (float)this.PointsToDraw / (float)this.Width;

            closestX = (int)(e.X * pointsPerX);
            closestX += (int)(((float)this.minIndexToDraw / (float)this.Width) * (float)this.totalPoints);

            this.ClickedCycleNumber = closestX / 128;

            if (e.Button == MouseButtons.Left)
            {
                this.leftClicked();
            }
            else if (e.Button == MouseButtons.Right)
            {
                this.rightClicked(this.ClickedCycleNumber);
            }
        }

        private void SineGraph_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 0xBB) //=
            {
                MouseEventArgs mE = new MouseEventArgs(MouseButtons.Middle, 0, 0, 0, 120);
                this.ExternalMouseWheel(new object(), mE);
                this.MouseWheeled(sender, mE);
            }
            else if (e.KeyValue == 0xBD) //-
            {
                MouseEventArgs mE = new MouseEventArgs(MouseButtons.Middle, 0, 0, 0, -120);
                this.ExternalMouseWheel(new object(), mE);
                this.MouseWheeled(sender, mE);
            }
        }

        private void SineGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.tT.Active)
            {
                SineWaveDefinition sWD = (SineWaveDefinition)this.sineWavesToDraw[0];

                float pointsPerX;
                int closestX;

                pointsPerX = (float)this.PointsToDraw / (float)this.Width;

                closestX = (int)(e.X * pointsPerX);
                closestX += (int)(((float)this.minIndexToDraw / (float)this.Width) * (float)this.totalPoints);

                closestX /= 128;
                closestX *= 128;
                float value = sWD.GetRMS((uint)closestX);

                if (!RelayModeFunctions.IsCurrent(this.Type))
                {
                    this.tT.Show(Math.Round(value * (float)ProtectorVoltage.Scaling, 1).ToString() + " Volts RMS", this, 10, 10);
                }
                else
                    this.tT.Show(Math.Round(value * this.CTRatio, 1).ToString() + " Amps RMS", this, 10, 10);
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.ExternalScroll(new object(), e);
            this.Scrolled(sender, e);
        }

        public delegate void ScrollEventHandler(object sender, ScrollEventArgs e);
        public event ScrollEventHandler ScrollEvent;

        public void ExternalScroll(object sender, ScrollEventArgs e)
        {
            if (sender == this)
                return;

            double percentage;

            this.hScrollBar1.Value = e.NewValue;

            if (this.hScrollBar1.LargeChange == this.hScrollBar1.Maximum || this.hScrollBar1.Value == 0)
                percentage = 0d;
            else if (this.hScrollBar1.Value > this.hScrollBar1.Maximum - this.hScrollBar1.LargeChange)
                percentage = (double)(this.hScrollBar1.Maximum - this.hScrollBar1.LargeChange - 1) / (double)(this.hScrollBar1.Maximum);
            else
                percentage = (double)(this.hScrollBar1.Value - 1) / (double)(this.hScrollBar1.Maximum);

            this.minIndexToDraw = (int)Math.Round(percentage * (double)this.Size.Width + 1d);

            this.Invalidate();
        }

        private void Scrolled(object sender, ScrollEventArgs e)
        {
            if (ScrollEvent != null)
                this.ScrollEvent(sender, e);
        }

        #endregion


    }
}