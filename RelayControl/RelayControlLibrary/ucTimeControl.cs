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
            this.SecondTimer.Interval = 5000;
            this.SecondTimer.Start();
            this.SecondTimer.Tick += SecondTimer_Tick;
            this.initializeTable();
        }

        public delegate void SendDataHandler(object o, SendEventArgs sEA);
        public event SendDataHandler SendData;

        public delegate void ErrorHandler(object o, ExceptionEventArgs eEA);
        public event ErrorHandler TimeControlError;

        public DateTime RelayDateTimeUTC
        {
            set
            {
                this.relayTime = value;
                this.CompareRelayTimeToRealTime(value);
                this.labelRelayTimeDisplay.Text = value.ToString("HH:mm:ss");
                this.updateTable();
            }
        }

        private Timer SecondTimer = new Timer();
        private DataTable timeTable = new DataTable();
        private DateTime relayTime;
        private TimeSpan difference;

        private void initializeTable()
        {
            timeTable.Columns.Add("TIME_NOW");
            timeTable.Columns.Add("RELAY_NOW");
            timeTable.Columns.Add("DIFF");
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
            this.labelMachineTimeDisplay.Text = DateTime.Now.ToString("HH:mm:ss");
            this.requestTime();
        }

        private void updateTable()
        {
            //this.timeTable.Rows.Add(DateTime.Now, this.relayTime, this.difference);
            Console.WriteLine(DateTime.Now + "\t" + this.relayTime + "\t" + this.difference);
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
                this.labelRelayTimeDisplay.Text = relayTime.ToString("HH:mm:ss");
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
            this.difference = relayUTCTime - DateTime.Now;
            this.labelTimeDiff.Text = difference.ToString();

            return returnBool;
        }

        private void errorHandler(Exception ex, string title)
        {
            ExceptionEventArgs eEA = new ExceptionEventArgs(ex, title);
            if (TimeControlError != null)
                TimeControlError(this, eEA);
        }

        private void buttonTable_Click(object sender, EventArgs e)
        {
            foreach(DataColumn dC in this.timeTable.Columns)
            {
                Console.Write(dC.ColumnName + "\t");
            }
            Console.WriteLine();

            foreach (DataRow dR in this.timeTable.Rows)
            {
                foreach(DataColumn dC in this.timeTable.Columns)
                {
                    Console.Write(dR[dC.ColumnName].ToString() + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}
