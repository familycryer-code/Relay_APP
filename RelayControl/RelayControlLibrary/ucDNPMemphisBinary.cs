using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucDNPMemphisBinary : UserControl
    {
        public ucDNPMemphisBinary()
        {
            InitializeComponent();
            this.checkBoxEventEnabled.Visible = false;
        }

        private uint pointNumber = 0;
        private string pointName = "";
        private bool eventEnabled = false;
        private bool eventEnableVisible = false;

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
            get { return this.eventEnabled; }
            set
            {
                this.eventEnabled = value;
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

        public bool CheckValue
        {
            get { return this.checkBoxPointName.Checked; }
            set
            {
                this.checkBoxPointName.Checked = value;
            }
        }

        private void checkBoxEventEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.eventEnabled = this.checkBoxEventEnabled.Checked;
        }
    }
}
