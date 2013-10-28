using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucDNPDeadBand : UserControl
    {
        public ucDNPDeadBand()
        {
            InitializeComponent();
        }

        public ucDNPDeadBand(string name, string units, decimal min, decimal max)
        {
            InitializeComponent();

            this.labelName.Text = name;
            this.labelUnits.Text = units;
                

            if(min > this.numericUpDownValue.Maximum)
            {
                this.numericUpDownValue.Maximum = max;
                this.numericUpDownValue.Value = min;
                this.numericUpDownValue.Minimum = min;
            }
            else
            {
                this.numericUpDownValue.Value = min;
                this.numericUpDownValue.Minimum = min;
                this.numericUpDownValue.Maximum = max;
            }
            
        }

        private System.Windows.Forms.ToolTip toolTip;

        public ucDNPDeadBand(ucDeadBandSettingsObject dBD)
        {
            InitializeComponent();
            this.labelName.Text = dBD.Name;
            this.labelUnits.Text = dBD.Units;

            if (dBD.Minimum > this.numericUpDownValue.Maximum)
            {
                this.numericUpDownValue.Maximum = dBD.Maximum;
                this.numericUpDownValue.Value = dBD.Minimum;
                this.numericUpDownValue.Minimum = dBD.Minimum;
            }
            else
            {
                this.numericUpDownValue.Value = dBD.Minimum;
                this.numericUpDownValue.Minimum = dBD.Minimum;
                this.numericUpDownValue.Maximum = dBD.Maximum;
            }

            this.toolTip = new ToolTip();
            this.toolTip.SetToolTip(this.numericUpDownValue, dBD.ToolTip);

            
        }
        
        public decimal Value
        {
            set { this.numericUpDownValue.Value = value; }
            get
            {
                return this.numericUpDownValue.Value;
            }
        }

        public string Name
        {
            get { return this.labelName.Text; }
            set{
                this.labelName.Text = value;
            }
        }

        public string Label
        {
            get { return this.labelUnits.Text; }
            set{
                this.labelUnits.Text = value;
            }
        }

    }
}
