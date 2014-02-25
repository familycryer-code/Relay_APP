using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RelayControlLibrary;

namespace RelayDNPSecurity
{
    public partial class ucDNPSAv5Settings : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5Settings()
        {
            InitializeComponent();
            this.initializeSecurityStatistics();
        }

        private List<DNPSAv5SecurityStatisticItem> statisticPoints = new List<DNPSAv5SecurityStatisticItem>();
        private static int _packetLength = 98;

        private void initializeSecurityStatistics()
        {
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Unexpected Messages", 3m));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Authorization Failures", 5m));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Authentication Failures", 5));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Reply Timeouts", 3));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Rekeys Due To Failure", 3));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Total Messages Sent", 100));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Total Messages Recevied", 100));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Critical Messages Received", 100));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Critical Messages Sent", 100));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Discarded Messages", 10));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Error Messages Sent", 2));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Error Messaged Received", 10));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Successful Authentications", 100));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Session Key Changes", 10));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Failed Session Key Changes", 5));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Update Key Changes", 1));
            this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Failed Update Key Changes", 1));

            Point workingPoint = new Point(5, 15);

            foreach(DNPSAv5SecurityStatisticItem sI in this.statisticPoints)
            {
                ucDNPSAv5SecurityStatisticThreshold workingStatistic = new ucDNPSAv5SecurityStatisticThreshold(sI.StatisticsName, sI.DefaultValue);
                workingStatistic.Location = workingPoint;
                this.groupBoxSecurityStats.Controls.Add(workingStatistic);

                workingPoint = new Point(workingPoint.X, workingPoint.Y + workingStatistic.Height);
                if(workingPoint.Y > this.groupBoxSecurityStats.Height - workingStatistic.Height)
                    workingPoint = new Point(workingPoint.X + workingStatistic.Width, 15);
            }
        }

        private void buttonSendSettings_Click(object sender, EventArgs e)
        {
            this.sendSettings();
        }

        private void sendSettings()
        {
            SecureSendEventArgs sSEA = new SecureSendEventArgs(_packetLength);
            UInt16 tempInt;
            byte tempByte;

            try
            {
                sSEA.Data[0] = (byte)RelayModeFunctions._DNPControlOpCode;
                sSEA.Data[1] = (byte)'S'; // For Settings
                sSEA.Data[2] = 0; //Spaced

                if (this.checkBoxAggressiveMode.Checked)
                    sSEA.Data[3] = 0x01;
                else
                    sSEA.Data[3] = 0x00;

                if(this.checkBoxSHA1.Checked)
                    sSEA.Data[3] |= 0x02;

                if (this.checkBoxAuthenticationEnabled.Checked)
                    sSEA.Data[3] |= 0x04;

                tempByte = (byte)this.comboBoxKeyChangeAlogrithm.SelectedIndex;
                tempByte <<= 3; //Takes up the next 3 bits

                sSEA.Data[3] |= tempByte;
                sSEA.Data[4] = 0;
                tempInt = (UInt16)(this.numericUpDownReplyTimeout.Value * 10);
                sSEA.Data[6] = (byte)(tempInt >> 8);
                sSEA.Data[5] = (byte)tempInt;

                tempInt = (UInt16)this.numericUpDownSessionKeyInterval.Value;
                sSEA.Data[8] = (byte)(tempInt >> 8);
                sSEA.Data[7] = (byte)tempInt;

                tempInt = (UInt16)this.numericUpDownSessionKeyChangeCount.Value;
                sSEA.Data[10] = (byte)(tempInt >> 8);
                sSEA.Data[9] = (byte)tempInt;

                tempInt = (UInt16)this.numericUpDownMaxSessionKeyCount.Value;
                sSEA.Data[11] = (byte)tempInt;

                sSEA.Data[12] = 0; //dummy spacer

                try
                {
                    int i = 13;
                    foreach (ucDNPSAv5SecurityStatisticThreshold sT in this.groupBoxSecurityStats.Controls)
                    {
                        byte[] tempBytes = sT.GetBytes();
                        sSEA.Data[i++] = tempBytes[0];
                        sSEA.Data[i++] = tempBytes[1];
                    }
                }
                catch (Exception ex)
                {
                    this.onError(new Exception("Error getting Security Statistics Values: " + ex.ToString()));
                    return;
                }
                sSEA.Data[sSEA.Data.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Sending SAv5 Settings: " + ex.ToString()));
            }
        }

        private void buttonRequestSettings_Click(object sender, EventArgs e)
        {
            this.requestSettings();
        }

        private void requestSettings()
        {
            SecureSendEventArgs sSEA = new SecureSendEventArgs(_packetLength);
            try
            {
                sSEA.Data[0] = (byte)RelayModeFunctions._DNPControlOpCode;
                sSEA.Data[1] = (byte)'s'; // For requesting settings

                sSEA.Data[sSEA.Data.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Requesting SAv5 Settings: " + ex.ToString()));
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            this.setDefaults();
        }

        private void setDefaults()
        {
            this.checkBoxAggressiveMode.Checked = true;
            this.checkBoxSHA1.Checked = false;
            this.checkBoxAuthenticationEnabled.Checked = true;

            this.comboBoxKeyChangeAlogrithm.SelectedIndex = 1;

            this.numericUpDownReplyTimeout.Value = 2.0m;
            this.numericUpDownSessionKeyInterval.Value = 900m;
            this.numericUpDownSessionKeyChangeCount.Value = 1000m;
            this.numericUpDownMaxSessionKeyCount.Value = 5m;

            foreach (ucDNPSAv5SecurityStatisticThreshold sT in this.groupBoxSecurityStats.Controls)
            {
                sT.SetDefault();
            }
        }

        internal void SetAll(byte[] bytePacket)
        {
            int i = 10; //TO DO
            decimal tempM;
            byte tempByte = 0;

            try
            {
                if ((bytePacket[1] & 0x01) == 0x01)
                    this.checkBoxAggressiveMode.Checked = true;
                else
                    this.checkBoxAggressiveMode.Checked = false;

                if ((bytePacket[1] & 0x02) == 0x02)
                    this.checkBoxSHA1.Checked = true;
                else
                    this.checkBoxSHA1.Checked = false;

                if ((bytePacket[1] & 0x04) == 0x04)
                    this.checkBoxAuthenticationEnabled.Checked = true;
                else
                    this.checkBoxAuthenticationEnabled.Checked = false;
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Setting DNP SAv5 bit Settings: " + ex.ToString()));
            }

            try
            {
                tempByte = (byte)(bytePacket[1] & 0x38);
                tempByte >>= 3;

                this.comboBoxKeyChangeAlogrithm.SelectedIndex = tempByte;
            }
            catch (Exception ex)
            {
                this.onError(new Exception(tempByte.ToString() + " not a valid index value for DNP SAv5 Key Change ALgorithm.  Threw error: " + ex.ToString()));
            }


            try
            {
                tempM = bytePacket[4];
                tempM *= 256;
                tempM += bytePacket[3];

                this.numericUpDownReplyTimeout.Value = tempM / 10m;
            }
            catch (Exception ex)
            {
                this.onError(new Exception("DNP SAv5 Error Setting Reply Timeout: " + ex.ToString()));
            }

            try
            {
                tempM = bytePacket[6];
                tempM *= 256;
                tempM += bytePacket[5];

                this.numericUpDownSessionKeyInterval.Value = tempM;
            }
            catch (Exception ex)
            {
                this.onError(new Exception("DNP SAv5 Error Setting Session Key Interval: " + ex.ToString()));
            }

            try
            {
                tempM = bytePacket[8];
                tempM *= 256;
                tempM += bytePacket[7];

                this.numericUpDownSessionKeyChangeCount.Value = tempM;
            }
            catch (Exception ex)
            {
                this.onError(new Exception("DNP SAv5 Error Setting Session Key Change Count: " + ex.ToString()));
            }

            try
            {
                this.numericUpDownMaxSessionKeyCount.Value = bytePacket[9];
            }
            catch (Exception ex)
            {
                this.onError(new Exception("DNP SAv5 Error Setting Max Session Key Change Count: " + ex.ToString()));
            }

            //Dummy byte 9 for now

            try
            {
                foreach (ucDNPSAv5SecurityStatisticThreshold sT in this.groupBoxSecurityStats.Controls)
                {
                    byte[] byteArray = new byte[2];
                    byteArray[1] = bytePacket[i++];
                    byteArray[0] = bytePacket[i++];
                    sT.SetBytes(byteArray);
                }
            }
            catch (Exception ex)
            {
                this.onError(new Exception("DNP SAv5 Error Setting Security Thresholds: " + ex.ToString()));
            }

        }
    }
}
