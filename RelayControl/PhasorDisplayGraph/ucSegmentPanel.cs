using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace PhasorDisplayGraph
{
    public partial class ucSegmentPanel : UserControl
    {
        public ucSegmentPanel()
        {
            InitializeComponent();
        }

        public ucSegmentPanel(string segmentName)
        {
            this.SegmentName = segmentName;
        }

        public string SegmentName
        {
            get { return this.checkBoxCurveName.Text; }
            set
            {
                this.checkBoxCurveName.Text = value;
            }
        }

        public bool CheckVisible
        {
            get { return this.checkBoxCurveName.Visible; }
            set { this.checkBoxCurveName.Visible = value; }
        }

        public delegate void SegmentValueChangedHandler(SegmentEventArgs sEA);
        public event SegmentValueChangedHandler SegmentValueChanged;

        private void valueChangedEvent(object sender, EventArgs e)
        {
            double angle = 90 - (double)this.numericUpDownAngle.Value;
            float slope = (float)Math.Tan(angle * Math.PI / 180d);
            SegmentEventArgs mySEA = new SegmentEventArgs(slope, (float)this.numericUpDownOffset.Value, (float)angle, true);
            
            onEvent(mySEA);
        }

        private void onEvent(SegmentEventArgs sEA)
        {
            if (SegmentValueChanged != null)
                SegmentValueChanged(sEA);
        }

        private void checkBoxCurveName_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.checkBoxCurveName.Checked)
            {
                this.numericUpDownAngle.Enabled = false;
                this.numericUpDownOffset.Enabled = false;
                this.numericUpDownOffset.Value = 0m;
                this.numericUpDownAngle.Value = 0m;
            }
            else
            {
                this.numericUpDownAngle.Enabled = true;
                this.numericUpDownOffset.Enabled = true;
            }

            SegmentEventArgs mySEA = new SegmentEventArgs(0f, 0f, 0f, this.checkBoxCurveName.Checked);
            onEvent(mySEA);
        }
    }

    public class SegmentEventArgs : EventArgs
    {
        public SegmentEventArgs(float slope, float offset, float angle, bool enabled)
        {
            this.Slope = slope;
            this.Offset = offset;
            this.Enabled = enabled;
            this.Angle = angle;
        }

        public float Slope;
        public float Offset;
        public float Angle;
        public bool Enabled;
    }
}
