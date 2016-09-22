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
    public partial class ucTimeControl : UserControl
    {
        public ucTimeControl()
        {
            InitializeComponent();
            this.SecondTimer.Interval = 1000;
            this.SecondTimer.Start();
            this.SecondTimer.Tick += SecondTimer_Tick;
        }

        public delegate void SendDataHandler(object o, SendEventArgs sEA);
        public event SendDataHandler SendData;

        public delegate void ErrorHandler(object o, ExceptionEventArgs eEA);
        public event ErrorHandler TimeControlError; 

        private Timer SecondTimer = new Timer();

        public DateTime RelayDateTimeUTC
        {
            set
            {
                this.CompareRelayTimeToRealTime(value);
                this.labelRelayTimeDisplay.Text = value.ToString();
            }
        }

        

        private void buttonRequestTime_Click(object sender, EventArgs e)
        {
            this.requestTime();
        }

        private void requestTime()
        {
            SendEventArgs sEA = new SendEventArgs(2);

            sEA.SendPacket[0] = 0x02;
            sEA.SendPacket[1] = 0x0D;

            if (SendData != null)
                SendData(this, sEA);
        }

        private void buttonSendTime_Click(object sender, EventArgs e)
        {
            this.sendTime();
        }

        private void sendTime()
        {
            SendEventArgs sEA = new SendEventArgs(6);
            long binaryDate = RelayModeFunctions.BinaryDate(DateTime.UtcNow);
            long temp;
            DateTime tempTime;

            sEA.SendPacket[0] = (byte)'j';
            sEA.SendPacket[1] = (byte)(binaryDate >> 24);
            sEA.SendPacket[2] = (byte)(binaryDate >> 16);
            sEA.SendPacket[3] = (byte)(binaryDate >> 8);
            sEA.SendPacket[4] = (byte)(binaryDate);
            sEA.SendPacket[5] = 0x0D;

            temp = binaryDate & 0x00000000FFFFFFFF;
            tempTime = RelayModeFunctions.DateFrom(temp);
            if (SendData != null)
                SendData(this, sEA);
        }

        private void SecondTimer_Tick(object sender, EventArgs e)
        {
            this.labelMachineTimeDisplay.Text = DateTime.UtcNow.ToString("HH:mm:ss");
        }

        private void updateRelayTimeLabel()
        {
            DateTime relayTime = new DateTime();
            try
            {
                relayTime = Convert.ToDateTime(this.labelRelayTimeDisplay.Text);
            }
            catch (Exception ex)
            {
                this.errorHandler(ex, "Error Converting Relay Time Label to Time");
                return;
            }

            try
            {
                this.labelRelayTimeDisplay.Text = relayTime.ToString();
            }
            catch (Exception ex)
            {
                this.errorHandler(ex, "Error Converting Relay Time to Text");
                return;
            }
        }

        bool CompareRelayTimeToRealTime(DateTime relayUTCTime)
        {
            bool returnBool = false;
            TimeSpan difference = relayUTCTime - DateTime.UtcNow;
            if(difference.Seconds > 0)
            {
                this.errorHandler(new Exception("Time Has Drifted by " + difference.Seconds.ToString() + " seconds"), "Time Drifted!");
            }
            return returnBool;
        }

        private void errorHandler(Exception ex, string title)
        {
            ExceptionEventArgs eEA = new ExceptionEventArgs(ex, title);
            if (TimeControlError != null)
                TimeControlError(this, eEA);
        }
    }
}
