using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class uc8CheckBoxFlags : UserControl
    {
        public uc8CheckBoxFlags()
        {
            InitializeComponent();
        }

        public List<string> Names
        {
            get { return this.names; }
            set
            {
                this.names = value;

                if (this.names != null)
                {
                    this.checkBox1.Text = value[0];
                    this.checkBox2.Text = value[1];
                    this.checkBox3.Text = value[2];
                    this.checkBox4.Text = value[3];
                    this.checkBox5.Text = value[4];
                    this.checkBox6.Text = value[5];
                    this.checkBox7.Text = value[6];
                    this.checkBox8.Text = value[7];
                }
            }
        }

        private List<string> names;

        public void SetValues(byte b)
        {
            if ((b & 1) == 1)
                this.checkBox1.Checked = true;
            else
                this.checkBox1.Checked = false;

            if ((b & 2) == 2)
                this.checkBox2.Checked = true;
            else
                this.checkBox2.Checked = false;

            if ((b & 4) == 4)
                this.checkBox3.Checked = true;
            else
                this.checkBox3.Checked = false;

            if ((b & 8) == 8)
                this.checkBox4.Checked = true;
            else
                this.checkBox4.Checked = false;

            if ((b & 16) == 16)
                this.checkBox5.Checked = true;
            else
                this.checkBox5.Checked = false;

            if ((b & 32) == 32)
                this.checkBox6.Checked = true;
            else
                this.checkBox6.Checked = false;

            if ((b & 64) == 64)
                this.checkBox7.Checked = true;
            else
                this.checkBox7.Checked = false;

            if ((b & 128) == 128)
                this.checkBox8.Checked = true;
            else
                this.checkBox8.Checked = false;
        }

        private void uc4CheckBoxFlags_Load(object sender, EventArgs e)
        {

        }
    }
}
