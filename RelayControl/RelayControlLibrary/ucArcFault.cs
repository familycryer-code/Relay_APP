using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucArcFault : UserControl
    {
        public ucArcFault()
        {
            InitializeComponent();
        }

        public void SetAll(byte[] bytePacket)
        {
            Int16 tempI;
            uint temp;

            tempI = bytePacket[0];
            tempI += (Int16)(bytePacket[1] * 256);

            this.textBoxADCOffsetShort.Text = tempI.ToString();

            tempI = bytePacket[2];
            tempI += (Int16)(bytePacket[3] * 256);

            this.textBoxBDCOffsetShort.Text = tempI.ToString();

            tempI = bytePacket[4];
            tempI += (Int16)(bytePacket[5] * 256);

            this.textBoxCDCOffsetShort.Text = tempI.ToString();

            temp = bytePacket[6];
            temp += (uint)bytePacket[7] * 256;

            this.textBoxANoiseShort.Text = temp.ToString();

            temp = bytePacket[8];
            temp += (uint)bytePacket[9] * 256;

            this.textBoxBNoiseShort.Text = temp.ToString();

            temp = bytePacket[10];
            temp += (uint)bytePacket[11] * 256;

            this.textBoxCNoiseShort.Text = temp.ToString();

            tempI = bytePacket[12];
            tempI += (Int16)(bytePacket[13] * 256);

            this.textBoxADCOffsetLong.Text = tempI.ToString();

            tempI = bytePacket[14];
            tempI += (Int16)(bytePacket[15] * 256);

            this.textBoxBDCOffsetLong.Text = tempI.ToString();

            tempI = bytePacket[16];
            tempI += (Int16)(bytePacket[17] * 256);

            this.textBoxCDCOffsetLong.Text = tempI.ToString();

            temp = bytePacket[18];
            temp += (uint)bytePacket[19] * 256;

            this.textBoxAHarmShort.Text = temp.ToString();

            temp = bytePacket[20];
            temp += (uint)bytePacket[21] * 256;

            this.textBoxBHarmShort.Text = temp.ToString();

            temp = bytePacket[22];
            temp += (uint)bytePacket[23] * 256;

            this.textBoxCHarmShort.Text = temp.ToString();

            temp = bytePacket[24];
            temp += (uint)bytePacket[25] * 256;

            this.textBoxAAmpsLong.Text = temp.ToString();

            temp = bytePacket[26];
            temp += (uint)bytePacket[27] * 256;

            this.textBoxBAmpsLong.Text = temp.ToString();

            temp = bytePacket[28];
            temp += (uint)bytePacket[29] * 256;

            this.textBoxCAmpsLong.Text = temp.ToString();

            temp = bytePacket[30];
            temp += (uint)bytePacket[31] * 256;

            this.textBoxAAmpsShort.Text = temp.ToString();

            temp = bytePacket[32];
            temp += (uint)bytePacket[33] * 256;

            this.textBoxBAmpsShort.Text = temp.ToString();

            temp = bytePacket[34];
            temp += (uint)bytePacket[35] * 256;

            this.textBoxCAmpsShort.Text = temp.ToString();

            temp = bytePacket[36];
            temp += (uint)bytePacket[37] * 256;

            this.textBoxReceiverNoise.Text = temp.ToString();

            temp = bytePacket[38];
            temp += (uint)bytePacket[39] * 256;

            this.textBoxReceiverHarm.Text = temp.ToString();

            if (bytePacket.Length > 40)
            {
                tempI = bytePacket[40];
                tempI += (Int16)(bytePacket[41] * 256);

                this.textBoxDCOffset.Text = Convert.ToInt16(tempI).ToString();
            }
            else
                this.textBoxDCOffset.Text = "";
        }
    }
}
