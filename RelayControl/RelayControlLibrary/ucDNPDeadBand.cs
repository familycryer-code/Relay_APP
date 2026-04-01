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


            if (min > this.numericUpDownValue.Maximum)
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
            this.multiplier = dBD.Mult;

            if (dBD.Minimum > this.numericUpDownValue.Maximum)
            {
                this.numericUpDownValue.Maximum = dBD.Maximum;
                this.numericUpDownValue.Value = dBD.Minimum;
                this.numericUpDownValue.Minimum = dBD.Minimum;
            }
            else
            {
               // MessageBox.Show(dBD.defVal + " default value");
                this.numericUpDownValue.Value = dBD.defVal;//dBD.Minimum;
                this.numericUpDownValue.Minimum = dBD.Minimum;
                this.numericUpDownValue.Maximum = dBD.Maximum;
            }

            this.numericUpDownValue.DecimalPlaces = (int)this.multiplier / 10;
            this.numericUpDownValue.Increment = 1 / this.multiplier;

            this.toolTip = new ToolTip();
            this.toolTip.SetToolTip(this.numericUpDownValue, dBD.ToolTip);
        }

        public decimal Value
        {
            set
            {
                // This line needs to be in here to make sure that it redraws the control 
                // when it is currently blank (the number has been deleted) and a new value
                // come in.
                numericUpDownValue.Text = " ";
                this.numericUpDownValue.Value = value / this.multiplier;
            }
            get
            {
                return Math.Round(this.numericUpDownValue.Value * this.multiplier);
            }
        }

        public new string Name
        {
            get { return this.labelName.Text; }
        }

        public string Label
        {
            get { return this.labelUnits.Text; }
        }

        private decimal multiplier = 1;

    }
}
