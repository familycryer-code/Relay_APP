using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucShortRangeFilterTableItem : UserControl
    {
        public ucShortRangeFilterTableItem()
        {
            InitializeComponent();
            this.initializeToolTip();
        }

        public ucShortRangeFilterTableItem(int slotNumber)
        {
            InitializeComponent();
            this.SlotNumber = slotNumber;
            this.initializeToolTip();
        }

        private void initializeToolTip()
        {
            this.toolTip.SetToolTip(this.textBoxID, "The ID of the remote RNC box");
            this.toolTip.SetToolTip(this.textBoxAge133, "The number of minutes since the last time ID " + this.ID.ToString() + " was heard on Channel 4");
            this.toolTip.SetToolTip(this.textBoxAge153, "The number of minutes since the last time ID " + this.ID.ToString() + " was heard on Channel 5");
            this.toolTip.SetToolTip(this.textBoxStrength133, "Strenth of ID " + this.ID.ToString() + " on Channel 4");
            this.toolTip.SetToolTip(this.textBoxStrength153, "Strenth of ID " + this.ID.ToString() + " on Channel 5");
        }

        public int ID
        {
            get { return this.iD; }
            set
            {
                if (value == 0)
                {
                    this.textBoxID.Text = "";
                    return;
                }

                this.iD = value;
                this.textBoxID.Text = value.ToString();
            }
        }
        public int SlotNumber
        {
            get { return this.slotNumber; }
            set
            {
                this.slotNumber = value;
                this.labelSlotNumber.Text = value.ToString();
            }
        }
        public int Strength133
        {
            get { return this.stregnth133; }
            set
            {
                this.stregnth133 = value;
                this.textBoxStrength133.Text = value.ToString();
            }
        }
        public int Strength153
        {
            get { return this.stregnth153; }
            set
            {
                this.stregnth153 = value;
                this.textBoxStrength153.Text = value.ToString();
            }
        }
        public int Age133
        {
            get { return this.age133; }
            set
            {
                this.age133 = value;
                this.textBoxAge133.Text = value.ToString();
            }
        }
        public int Age153
        {
            get { return this.age153; }
            set
            {
                this.age153 = value;
                this.textBoxAge153.Text = value.ToString();
            }
        }
        public int TransmitStrength
        {
            get { return this.transmitStrength; }
            set
            {
                this.transmitStrength = value;
                if (this.stregnth133 > this.transmitStrength || this.stregnth153 > this.transmitStrength)
                {
                    this.textBoxID.BackColor = Color.Green;
                    this.textBoxID.ForeColor = Color.White;
                }
                else
                {
                    this.textBoxID.ResetForeColor();
                    this.textBoxID.ResetBackColor();
                }
            }
        }

        private int iD;
        private int slotNumber;
        private int stregnth133;
        private int stregnth153;
        private int age133;
        private int age153;
        private int transmitStrength = 255;
        private ToolTip toolTip = new ToolTip();
    }
}
