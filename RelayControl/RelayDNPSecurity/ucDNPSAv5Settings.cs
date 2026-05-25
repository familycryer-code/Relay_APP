using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RelayControlLibrary;
using SharedResources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RelayDNPSecurity
{
    public partial class ucDNPSAv5Settings : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5Settings()
        {
            InitializeComponent();
            this.initializeSecurityStatistics();
                      
            // Set Font of only the groupBoxMain Title in bold. keep rest of items in side in regular ( non bold ) font
            groupBoxMain.Font = new Font(groupBoxMain.Font, FontStyle.Bold);
            foreach (Control ctrl in groupBoxMain.Controls)
            {
                ctrl.Font = new Font(
                    groupBoxMain.Font.FontFamily,
                    groupBoxMain.Font.Size,
                    FontStyle.Regular
                );
            }

            // Set Font of only the groupBoxSecurityStats Title in bold. keep rest of items in side in regular ( non bold ) font
            groupBoxSecurityStats.Font = new Font(groupBoxSecurityStats.Font, FontStyle.Bold);
            foreach (Control ctrl in groupBoxSecurityStats.Controls)
            {
                ctrl.Font = new Font(
                    groupBoxSecurityStats.Font.FontFamily,
                    groupBoxSecurityStats.Font.Size,
                    FontStyle.Regular
                );
            }
        }

        public List<DNPSAv5SecurityStatisticItem> statisticPoints = new List<DNPSAv5SecurityStatisticItem>();
        private static int _packetLength = 98;

        public void initializeSecurityStatistics()
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
            //this.statisticPoints.Add(new DNPSAv5SecurityStatisticItem("Special Statistic", 

            Point workingPoint = new Point(5, 15);
            
            foreach (DNPSAv5SecurityStatisticItem sI in this.statisticPoints)
            {
                ucDNPSAv5SecurityStatisticThreshold workingStatistic = new ucDNPSAv5SecurityStatisticThreshold(sI.StatisticsName, sI.DefaultValue);
                workingStatistic.Location = workingPoint;
                this.groupBoxSecurityStats.Controls.Add(workingStatistic);

                workingPoint = new Point(workingPoint.X, workingPoint.Y + workingStatistic.Height);
                //if (workingPoint.Y > this.groupBoxSecurityStats.Height - workingStatistic.Height)
                if (workingPoint.Y > 360 - workingStatistic.Height) //if (workingPoint.Y > 320 - workingStatistic.Height)
                    workingPoint = new Point(workingPoint.X + workingStatistic.Width, 15);
            }

            this.buttonDefault.Location = new System.Drawing.Point(25, 430);
            this.buttonSendSettings.Location = new System.Drawing.Point(200, 430);
            this.buttonRequestSettings.Location = new System.Drawing.Point(125, 470);

            this.comboBoxMACAlogrithm.Location = new System.Drawing.Point(160, 380);
            this.labelMACAlgorithm.Location = new System.Drawing.Point(65, 382);
            this.comboBoxKeyChangeAlogrithm.Location = new System.Drawing.Point(160, 335);
            this.labelKeyChangeAlgorithm.Location = new System.Drawing.Point(31, 337);
            this.numericUpDownMaxSessionKeyCount.Location = new System.Drawing.Point(160, 290);
            this.labelMaxSessionKeyCount.Location = new System.Drawing.Point(28, 292);
            this.numericUpDownSessionKeyChangeCount.Location = new System.Drawing.Point(160, 245);
            this.labelSessionKeyChangeCount.Location = new System.Drawing.Point(12, 246);
            this.numericUpDownSessionKeyInterval.Location = new System.Drawing.Point(160, 200);
            this.labelSessionKeyInterval.Location = new System.Drawing.Point(27, 202);
            this.numericUpDownReplyTimeout.Location = new System.Drawing.Point(160, 155);
            this.labelReplyTimeout.Location = new System.Drawing.Point(53, 157);

            this.checkBoxAuthenticationEnabled.Location = new System.Drawing.Point(19, 110);
            this.checkBoxSHA1.Location = new System.Drawing.Point(19, 65);
            this.checkBoxAggressiveMode.Location = new System.Drawing.Point(19, 20);

            // Set Font of only the groupBoxSecurityStats Title in bold. keep rest of items in side in regular ( non bold ) font
            /*groupBoxSecurityStats.Font = new Font(groupBoxSecurityStats.Font, FontStyle.Bold);
            foreach (Control ctrl in groupBoxSecurityStats.Controls)
            {
                ctrl.Font = new Font(
                    groupBoxSecurityStats.Font.FontFamily,
                    groupBoxSecurityStats.Font.Size,
                    FontStyle.Regular
                );
            }*/

            this.panel_DNPSAv5set.Location = new System.Drawing.Point(337, 18);
            this.panel_DNPSAv5set.Size = new System.Drawing.Size(605, 492);

        }

        public bool AuthenticationEnabled
        {
            get
            {
                if (checkBoxAuthenticationEnabled.Checked)
                    return true;
                else
                    return false;
            }
            set
            {
                if (AuthenticationEnabled)
                    checkBoxAuthenticationEnabled.Checked = true;
                else
                    checkBoxAuthenticationEnabled.Checked = false;
            }
        }

        //private void buttonSendSettings_Click(object sender, EventArgs e)
        public void buttonSendSettings_Click(object sender, EventArgs e)
        {
            this.sendSettings();
        }

        //private void sendSettings()
        public void sendSettings()
        {
            SendEventArgs sSEA = new SendEventArgs(_packetLength);
            UInt16 tempInt;
            byte tempByte;
            sSEA.WithAck = true;

            try
            {
                sSEA.SendPacket[0] = ProjectConstants._DNPControlOpCode;
                sSEA.SendPacket[1] = (byte)'S'; // For Settings
                sSEA.SendPacket[2] = 0; //Spaced

                if (this.checkBoxAggressiveMode.Checked)
                    sSEA.SendPacket[3] = 0x01;
                else
                    sSEA.SendPacket[3] = 0x00;

                if (this.checkBoxSHA1.Checked)
                    sSEA.SendPacket[3] |= 0x02;

                if (this.checkBoxAuthenticationEnabled.Checked)
                    sSEA.SendPacket[3] |= 0x04;

                tempByte = (byte)this.comboBoxKeyChangeAlogrithm.SelectedIndex;
                tempByte <<= 3; //Takes up the next 3 bits

                sSEA.SendPacket[3] |= tempByte;
                sSEA.SendPacket[4] = 0;
                tempInt = (UInt16)(this.numericUpDownReplyTimeout.Value * 10);
                sSEA.SendPacket[6] = (byte)(tempInt >> 8);
                sSEA.SendPacket[5] = (byte)tempInt;

                tempInt = (UInt16)this.numericUpDownSessionKeyInterval.Value;
                sSEA.SendPacket[8] = (byte)(tempInt >> 8);
                sSEA.SendPacket[7] = (byte)tempInt;

                tempInt = (UInt16)this.numericUpDownSessionKeyChangeCount.Value;
                sSEA.SendPacket[10] = (byte)(tempInt >> 8);
                sSEA.SendPacket[9] = (byte)tempInt;

                tempInt = (UInt16)this.numericUpDownMaxSessionKeyCount.Value;
                sSEA.SendPacket[11] = (byte)tempInt;

                tempInt = (UInt16)this.comboBoxMACAlogrithm.SelectedIndex;
                sSEA.SendPacket[12] = (byte)tempInt;

                try
                {
                    int i = 13;
                    foreach (ucDNPSAv5SecurityStatisticThreshold sT in this.groupBoxSecurityStats.Controls)
                    {
                        byte[] tempBytes = sT.GetBytes();
                        sSEA.SendPacket[i++] = tempBytes[0];
                        sSEA.SendPacket[i++] = tempBytes[1];
                    }
                }
                catch (Exception ex)
                {
                    this.onError(new Exception("Error getting Security Statistics Values: " + ex.ToString()), "Error Sending SAv5 Settings");
                    return;
                }
                sSEA.SendPacket[sSEA.SendPacket.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Sending SAv5 Settings: " + ex.ToString()), "Error sending SAv5 Settings");
            }
        }

        //private void numericUpDownReplyTimeout_ValueChanged(object sender, EventArgs e)
        private void numericUpDownReplyTimeout_TextChanged(object sender, EventArgs e)
        {
            this.numericUpDownReplyTimeout.Value = numericUpDownReplyTimeout.Value;
        }

        //private void buttonRequestSettings_Click(object sender, EventArgs e)
        public void buttonRequestSettings_Click(object sender, EventArgs e)
        {
            this.RequestSettings();
        }

        public void RequestSettings()
        {
            SendEventArgs sSEA = new SendEventArgs(_packetLength);
            try
            {
                sSEA.SendPacket[0] = ProjectConstants._DNPControlOpCode;
                sSEA.SendPacket[1] = (byte)'s'; // For requesting settings

                sSEA.SendPacket[sSEA.SendPacket.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Requesting SAv5 Settings: " + ex.ToString()), "Error Requesting SAv5 Settings");
            }
        }

        //private void buttonDefault_Click(object sender, EventArgs e)
        public void buttonDefault_Click(object sender, EventArgs e)
        {
            this.setDefaults();
        }

        //private void setDefaults()
        public void setDefaults()
        {
            this.checkBoxAggressiveMode.Checked = true;
            this.checkBoxSHA1.Checked = false;

            this.checkBoxAuthenticationEnabled.Checked = false;

            this.comboBoxKeyChangeAlogrithm.SelectedIndex = 1;
            this.comboBoxMACAlogrithm.SelectedIndex = 2;

            this.numericUpDownReplyTimeout.Value = 2.0m;
            this.numericUpDownSessionKeyInterval.Value = 1800m;
            this.numericUpDownSessionKeyChangeCount.Value = 4000m;
            this.numericUpDownMaxSessionKeyCount.Value = 5m;

            foreach (ucDNPSAv5SecurityStatisticThreshold sT in this.groupBoxSecurityStats.Controls)
            {
                sT.SetDefault();
            }
        }

        internal void SetAll(byte[] bytePacket)
        {
            int i = 11; //TO DO
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
                dataBackupSAV5.dataBackup_sav5Defaults = true;
                this.onError(new Exception("Error Setting DNP SAv5 bit Settings: " + ex.ToString()), "Error Setting DNP SAv5 bit");
            }

            try
            {
                tempByte = (byte)(bytePacket[1] & 0x38);
                tempByte >>= 3;

                this.comboBoxKeyChangeAlogrithm.SelectedIndex = tempByte;
            }
            catch (Exception ex)
            {
                dataBackupSAV5.dataBackup_sav5Defaults = true;
                this.onError(new Exception(tempByte.ToString() + " not a valid index value for DNP SAv5 Key Change ALgorithm.  Threw error: " + ex.ToString()), "Error Setting KeyChange Algorithm");
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
                dataBackupSAV5.dataBackup_sav5Defaults = true;
                this.onError(new Exception("DNP SAv5 Error Setting Reply Timeout: " + ex.ToString()), "Error Changing SAv5 Timeout");
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
                dataBackupSAV5.dataBackup_sav5Defaults = true;
                this.onError(new Exception("DNP SAv5 Error Setting Session Key Interval: " + ex.ToString()), "Error Setting SAv5 Session Key");
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
                dataBackupSAV5.dataBackup_sav5Defaults = true;
                this.onError(new Exception("DNP SAv5 Error Setting Session Key Change Count: " + ex.ToString()), "Error Setting SAv5 Session Key Change Count");
            }

            try
            {
                this.numericUpDownMaxSessionKeyCount.Value = bytePacket[9];
            }
            catch (Exception ex)
            {
                dataBackupSAV5.dataBackup_sav5Defaults = true;
                this.onError(new Exception("DNP SAv5 Error Setting Max Session Key Change Count: " + ex.ToString()), "Error Setting SAv5 Max Session Key Change");
            }

            try
            {
                this.comboBoxMACAlogrithm.SelectedIndex = bytePacket[10];
            }
            catch (Exception ex)
            {
                dataBackupSAV5.dataBackup_sav5Defaults = true;
                this.onError(new Exception("DNP SAv5 Error Setting MAC Algorithm: " + ex.ToString()), "Error Setting SAv5 MAC Algorithm");
            }

            try
            {
                foreach (ucDNPSAv5SecurityStatisticThreshold sT in this.groupBoxSecurityStats.Controls)
                {
                    byte[] byteArray = new byte[2];
                    byteArray[0] = bytePacket[i++];
                    byteArray[1] = bytePacket[i++];
                    sT.SetBytes(byteArray);
                }
            }
            catch (Exception ex)
            {
                dataBackupSAV5.dataBackup_sav5Defaults = true;
                this.onError(new Exception("DNP SAv5 Error Setting Security Thresholds: " + ex.ToString()), "Error Setting SAv5 Security Thresholds");
            }

        }
    }
}
