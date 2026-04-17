using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayDNPSecurity
{
    public partial class ucDNPSAv5SecurityStatisticThreshold : UserControl
    {
        public ucDNPSAv5SecurityStatisticThreshold()
        {
            InitializeComponent();
            this.numericUpDownValue.Location = new System.Drawing.Point(150, 4);
        }

        public ucDNPSAv5SecurityStatisticThreshold(string name, decimal defaultValue)
        {
            InitializeComponent();

            this.labelName.Text = name;
            this.numericUpDownValue.Value = defaultValue;
            this.defaultValue = defaultValue;
        }

        private decimal defaultValue;

        public byte[] GetBytes()
        {
            byte[] returnBytes = new byte[2];

            UInt16 tempInt = (UInt16)this.numericUpDownValue.Value;

            returnBytes[0] = (byte)tempInt;
            returnBytes[1] = (byte)(tempInt >> 8);

            return returnBytes;
        }

        public void SetBytes(byte[] bytePacket)
        {
            UInt16 tempInt = bytePacket[1];
            tempInt <<= 8;
            tempInt += bytePacket[0];

            this.numericUpDownValue.Value = tempInt;
        }

        public void SetDefault()
        {
            this.numericUpDownValue.Value = this.defaultValue;
        }
    }
}
