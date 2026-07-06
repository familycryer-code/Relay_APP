using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucCoverFlags : UserControl
    {
        public ucCoverFlags()
        {
            InitializeComponent();
            this.groupBoxRelayFlagPolarityCover.Visible = false;

        }

        public void setDNPCoverFlags(byte[] bytePacket)
        {
            this.setFlagPolarityDNPCover(bytePacket[9]);
        }

        public void setFlagPolarityDNPCover(byte p)
        {
            if ((p & 1) == 1)
                this.radioButtonFPACloseRelay.Checked = true;
            else
                this.radioButtonFPAOpenRelay.Checked = true;

            if ((p & 2) == 2)
                this.radioButtonFPBCloseRelay.Checked = true;
            else
                this.radioButtonFPBOpenRelay.Checked = true;

            if ((p & 4) == 4)
                this.radioButtonFPCCloseRelay.Checked = true;
            else
                this.radioButtonFPCOpenRelay.Checked = true;

            if ((p & 8) == 8)
                this.radioButtonFPDCloseRelay.Checked = true;
            else
                this.radioButtonFPDOpenRelay.Checked = true;

            if ((p & 16) == 16)
                this.radioButtonFPECloseRelay.Checked = true;
            else
                this.radioButtonFPEOpenRelay.Checked = true;

            if ((p & 32) == 32)
                this.radioButtonFPFCloseRelay.Checked = true;
            else
                this.radioButtonFPFOpenRelay.Checked = true;

            if ((p & 64) == 64)
                this.radioButtonFPGCloseRelay.Checked = true;
            else
                this.radioButtonFPGOpenRelay.Checked = true;

            if ((p & 128) == 128)
                this.radioButtonFPHCloseRelay.Checked = true;
            else
                this.radioButtonFPHOpenRelay.Checked = true;

        }

        public byte getDNPCoverFlagsByte()
        {
            byte DNPCoverFlags = 0;

            if (radioButtonFPACloseRelay.Checked == true)
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags | 1);
            else
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags & 254);

            if (radioButtonFPBCloseRelay.Checked == true)
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags | 2);
            else
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags & 253);

            if (radioButtonFPCCloseRelay.Checked == true)
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags | 4);
            else
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags & 251);

            if (radioButtonFPDCloseRelay.Checked == true)
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags | 8);
            else
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags & 247);

            if (radioButtonFPECloseRelay.Checked == true)
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags | 16);
            else
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags & 239);

            if (radioButtonFPFCloseRelay.Checked == true)
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags | 32);
            else
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags & 223);

            if (radioButtonFPGCloseRelay.Checked == true)
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags | 64);
            else
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags & 191);

            if (radioButtonFPHCloseRelay.Checked == true)
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags | 128);
            else
                DNPCoverFlags = Convert.ToByte(DNPCoverFlags & 127);

            return DNPCoverFlags;
        }
    }
}
