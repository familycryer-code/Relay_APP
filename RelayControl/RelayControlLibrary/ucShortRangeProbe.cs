using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucShortRangeProbe : UserControl
    {
        public ucShortRangeProbe()
        {
            InitializeComponent();
#if DEBUG
            this.debugVersionInitialize();
#else
            this.releaseVersionInitialize();
#endif
        }

        public ucShortRangeProbe(int iD)
        {
            InitializeComponent();
#if DEBUG
            this.debugVersionInitialize();
#else
            this.releaseVersionInitialize();
#endif
            this.IDNumber = iD;
        }

        /*
        public int NumberOfPoints
        {
            get { return this.numberOfPoints; }
            set
            {
                this.numberOfPoints = value;
                this.numericUpDownNumberOfPoints.Value = value;
            }
        }
        */
        public int IDNumber
        {
            get { return this.iDNumber; }
            set
            {
                this.iDNumber = value;
                this.numericUpDownIDNumber.Value = value;
            }
        }
        public int Signal133KHz
        {
            get { return this.signal133Khz; }
            set
            {
                this.signal133Khz = value;
                this.textBoxSignal133KHz.Text = value.ToString();
            }
        }
        public int Age133KHz
        {
            get { return this.age133Khz; }
            set
            {
                this.age133Khz = value;
                this.textBoxAge133KHz.Text = value.ToString();
            }
        }
        public int Signal153KHz
        {
            get { return this.signal153Khz; }
            set
            {
                this.signal153Khz = value;
                this.textBoxSignal153KHz.Text = value.ToString();
            }
        }
        public int Age153KHz
        {
            get { return this.age153Khz; }
            set
            {
                this.age153Khz = value;
                this.textBoxAge153KHz.Text = value.ToString();
            }
        }


        //private int numberOfPoints = 0;
        private int iDNumber = 0;
        private int signal133Khz = 0;
        private int age133Khz = 0;
        private int signal153Khz = 0;
        private int age153Khz = 0;

        #region Initialize Functions

        private void debugVersionInitialize()
        {
            
        }

        private void releaseVersionInitialize()
        {
            //this.Size = new Size(this.numericUpDownNumberOfPoints.Location.X + this.numericUpDownNumberOfPoints.Width + 3, this.Size.Height);
        }

        #endregion

        #region Validate Numbers

        public delegate void ValuesChangedDelegate(object sender, EventArgs e);

        /*
        private void numericUpDownIDNumber_Leave(object sender, EventArgs e)
        {
            this.validateNumbers();
        }
        */
        private void numericUpDownNumberOfPoints_Leave(object sender, EventArgs e)
        {
            //this.validateNumbers();
        }
        
        /*
        private void validateNumbers()
        {
            this.IDNumber = (int)this.numericUpDownIDNumber.Value;
            this.NumberOfPoints = (int)this.numericUpDownNumberOfPoints.Value;

            if(ValuesChanged != null)
            {
                ValuesChanged(this, new EventArgs());
            }
        }
        */
        #endregion

    }
}
