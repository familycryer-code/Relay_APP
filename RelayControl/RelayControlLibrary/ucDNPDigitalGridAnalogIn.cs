using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucDNPDigitalGridAnalogIn : UserControl
    {
        public ucDNPDigitalGridAnalogIn()
        {
            InitializeComponent();
        }

        private uint pointNumber = 0;
        private string pointName = "";
        private bool signed = false;
        
        public bool PointEnabled
        {
            get { return this.Enabled; }
            set
            {
                this.Enabled = value;
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
                this.labelPointName.Text = value;
            }
        }

        public bool Signed
        {
            get { return this.signed; }
            set
            {
                this.signed = value;
            }
        }
        public uint PointValue
        {
            set
            {
                if(this.signed)
                {
                    try
                    {
                        Int16 temp = (Int16)value;
                        this.textBoxPointValue.Text = temp.ToString();
                    }
                    catch(Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                }
                else
                    this.textBoxPointValue.Text = value.ToString();
            }
        }
    }
}
