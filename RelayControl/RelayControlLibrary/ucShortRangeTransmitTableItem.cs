using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucShortRangeTransmitTableItem : UserControl
    {
        public ucShortRangeTransmitTableItem()
        {
            InitializeComponent();
            this.initializeToolTip();
        }

        public ucShortRangeTransmitTableItem(int slotNumber)
        {
            InitializeComponent();
            this.SlotNumber = slotNumber;
            this.initializeToolTip();
        }

        private void initializeToolTip()
        {
            this.toolTip.SetToolTip(this.textBoxID, "The ID of the remote RNC box");
            this.toolTip.SetToolTip(this.textBoxAvgStrength, "The average strength of the two channels for ID " + this.ID.ToString());
            this.toolTip.SetToolTip(this.textBoxChangeCount, "Number of times the ID has been replaced for this slot while the APP has been attached to the relay");
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
        public int AverageStregnth
        {
            get { return this.averageStrength; }
            set
            {
                this.averageStrength = value;
                if (this.iD == 65535)
                    this.textBoxAvgStrength.Text = "";
                else
                    this.textBoxAvgStrength.Text = value.ToString();
            }
        }
        public int ChangeCount
        {
            get { return this.changeCount; }
            set
            {
                this.changeCount = value;
                if (this.iD == 65535)
                    this.textBoxChangeCount.Text = "";
                else
                    this.textBoxChangeCount.Text = value.ToString();
            }
        }
        public int ID
        {
            get { return this.iD; }
            set
            {
                if (value == 65535)
                {
                    this.textBoxID.Text = "";
                    this.textBoxChangeCount.Text = "";
                    this.textBoxAvgStrength.Text = "";
                    this.changeCount = 0;
                    this.iD = value;
                    return;
                }

                // 65535 is the original ID number, both in the relay and in the APP
                if (this.iD != value)
                    this.ChangeCount++;

                this.iD = value;
                this.textBoxID.Text = value.ToString();
            }
        }
        public int TransmitStrength
        {
            get { return this.transmitStrength; }
            set
            {
                this.transmitStrength = value;
                if (this.averageStrength > this.transmitStrength && this.iD != 65535)
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

        private ToolTip toolTip = new ToolTip();
        private int slotNumber;
        private int averageStrength;
        private int iD = 65535;
        private int changeCount;
        private int transmitStrength = 255;
    }
}
