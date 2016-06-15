using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucDNPMemphisBinary : ucDNPPointSuperClass
    {
        public ucDNPMemphisBinary()
        {
            InitializeComponent();
            this.checkBoxEventEnabled.Visible = false;
        }

        public uint PointNumber
        {
            get { return this.pointNumber; }
            set
            {
                this.pointNumber = value;
                this.labelPointNumber.Text = value.ToString();
            }
        }
        public string PointName
        {
            get { return this.pointName; }
            set
            {
                this.pointName = value;
                this.checkBoxPointName.Text = value;
            }
        }
        public bool EventEnabled
        {
            get { return this.checkBoxEventEnabled.Checked; }
            set
            {
                this.checkBoxEventEnabled.Checked = value;
            }
        }
        public bool EventEnableVisible
        {
            get { return this.eventEnableVisible; }
            set
            {
                this.eventEnableVisible = value;
                this.checkBoxEventEnabled.Visible = value;
                this.labelEventEnable.Visible = value; 
            }
        }
        public bool PointEnabled
        {
            get { return this.Enabled; }
            set {
                this.Enabled = value;
            }
        }


        public bool CheckValue
        {
            get { return this.checkBoxPointName.Checked; }
            set
            {
                this.checkBoxPointName.Checked = value;
            }
        }
    }
}
