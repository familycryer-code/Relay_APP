using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using SharedResources;

namespace RelayControlLibrary
{
    public partial class ucShortRange : UserControl
    {
        public ucShortRange()
        {
            InitializeComponent();

            this.generalInitialize();
#if DEBUG
            this.debugInitialize();
#else
            this.releaseInitialize();
#endif
        }

        private List<ucShortRangeFilterTableItem> filterTableItems = new List<ucShortRangeFilterTableItem>(16);
        private List<ucShortRangeTransmitTableItem> transmitTableItems = new List<ucShortRangeTransmitTableItem>(10);
        private const int _transmitThreshold = 30;

        private void generalInitialize()
        {
            Point location = new Point(1, 40);

            if (this.filterTableItems.Count != 0)
                return;

            for (int i = 0; i < 16; i++)
            {
                // Add one with each slot number
                this.filterTableItems.Add(new ucShortRangeFilterTableItem(i + 1));
            }

            foreach (ucShortRangeFilterTableItem item in this.filterTableItems)
            {
                item.Location = location;
                this.Controls.Add(item);
                item.Show();
                location.Y += 27;
            }

            location = new Point(this.labelTransmitTableSlot.Location.X + 1, 40);

            for (int i = 0; i < 10; i++)
            {
                this.transmitTableItems.Add(new ucShortRangeTransmitTableItem(i + 1));
            }

            foreach (ucShortRangeTransmitTableItem item in this.transmitTableItems)
            {
                item.Location = location;
                this.Controls.Add(item);
                item.Show();
                location.Y += 27;
            }
        }

        private void debugInitialize()
        {

        }

        private void releaseInitialize()
        {
            this.groupBoxNoise.Hide();
            this.groupBoxSignal.Hide();
        }

        #region Communications

        private delegate void setAllCallBack(byte[] bytePacket);
        public delegate void SendPacketDelegate(object sender, SendEventArgs e);
        public event SendPacketDelegate Send;

        private bool resetThreshold = false;
        public void ResetThreshold()
        {

            this.resetThreshold = true;
            this.requestMonitoringData();
            return;
        }

        private void resetThresholdToDefault()
        {
            Int32 currentThreshold = 0;
            try
            {
                currentThreshold = Convert.ToInt32(this.textBoxTransmitAboveSS.Text);
            }
            catch
            {
                // If the textbox is empty, it means we have to request the data
                this.requestMonitoringData();
                this.resetThreshold = true;
            }

            try
            {
                UInt16 tempCount = 0;

                if (currentThreshold != _transmitThreshold)
                {
                    Int32 temp = currentThreshold - _transmitThreshold;

                    if (temp < -10)
                    {
                        currentThreshold += 10;
                        this.transmitThresholdChange_Click(this.buttonPlus10, new EventArgs());
                    }
                    else if (temp <= -3)
                    {
                        currentThreshold += 3;
                        this.transmitThresholdChange_Click(this.buttonPlus3, new EventArgs());
                    }
                    else if (temp <= 3)
                    {
                        currentThreshold -= 3;
                        this.transmitThresholdChange_Click(this.buttonMinus3, new EventArgs());
                    }
                    else
                    {
                        currentThreshold -= 10;
                        this.transmitThresholdChange_Click(this.buttonMinus10, new EventArgs());
                    }

                    tempCount++;
                    if (tempCount >= 20)
                        throw new Exception("Too Many Iterations For Threshold Fix");

                    this.requestMonitoringData();
                }
                else
                    this.resetThreshold = false;

            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }
        }

        public void SetAll(byte[] bytePacket)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    setAllCallBack sACB = new setAllCallBack(this.setAll);
                    this.Invoke(sACB, new object[] { bytePacket });
                }
                else
                {
                    this.setAll(bytePacket);
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }
        }

        private void setAll(byte[] bytePacket)
        {
            // 107 is Signal Strengths
            if (bytePacket.Length == 107)
            {
                this.setSignalStrengths(bytePacket);
            }
            else //Settings
            {
                this.setTransmitValues(bytePacket);
            }
        }

        private void setTransmitValues(byte[] bytePacket)
        {
            try
            {
                for (int i = 0; i < this.transmitTableItems.Count; i++)
                {
                    this.transmitTableItems[i].ID = bytePacket[i * 3] + bytePacket[i * 3 + 1] * 256;
                    this.transmitTableItems[i].AverageStregnth = bytePacket[i * 3 + 2];
                    this.transmitTableItems[i].TransmitStrength = bytePacket[30];
                }
                this.textBoxTransmitAboveSS.Text = bytePacket[30].ToString();
                if (this.resetThreshold)
                    this.resetThresholdToDefault();
                foreach (ucShortRangeFilterTableItem item in this.filterTableItems)
                {
                    item.TransmitStrength = bytePacket[30];
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error Setting Transmit Values because: " + ex.ToString()));
            }
        }

        private void setSignalStrengths(byte[] bytePacket)
        {
            try
            {
                this.textBoxNoise47KHz.Text = bytePacket[0].ToString();
                this.textBoxNoise52KHz.Text = bytePacket[1].ToString();
                this.textBoxNoise58KHz.Text = bytePacket[2].ToString();
                this.textBoxNoise133KHz.Text = bytePacket[3].ToString();
                this.textBoxNoise153KHz.Text = bytePacket[4].ToString();

                this.textBoxSignal47KHz.Text = bytePacket[5].ToString();
                this.textBoxSignal52KHz.Text = bytePacket[6].ToString();
                this.textBoxSignal58KHz.Text = bytePacket[7].ToString();

                this.textBoxAge47.Text = bytePacket[40].ToString();
                this.textBoxAge52.Text = bytePacket[41].ToString();
                this.textBoxAge58.Text = bytePacket[42].ToString();

                // IDs start at byte 75 and go to 106
                for (int i = 0; i < 16; i++)
                {
                    int tempID = (int)bytePacket[i * 2 + 75] + ((int)bytePacket[i * 2 + 76] * 256);

                    this.filterTableItems[i].ID = tempID;
                    this.filterTableItems[i].Strength133 = bytePacket[i * 2 + 8];
                    this.filterTableItems[i].Strength153 = bytePacket[i * 2 + 9];
                    this.filterTableItems[i].Age133 = bytePacket[i * 2 + 43];
                    this.filterTableItems[i].Age153 = bytePacket[i * 2 + 44];
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error Setting Signal Strengths because: " + ex.ToString()));
            }
        }

        private int lastBadIDValue = 0;
        private void setAllMonitoringValues(byte[] bytePacket)
        {
            ucShortRangeProbe workingProbe = new ucShortRangeProbe();
            int tempIDNumber;
            bool badIDFound = false;

            if (bytePacket.Length == 59)
            {
                //clearAllMuxBoxes();
                this.tempMuxBoxes.Clear();

                for (int i = 43; i < 56; i += 2)
                {
                    tempIDNumber = bytePacket[i + 1];
                    tempIDNumber <<= 8;
                    tempIDNumber += bytePacket[i];
                    try
                    {
                        if (tempIDNumber != 65535)
                            this.tempMuxBoxes.Add(new ucShortRangeProbe(tempIDNumber));
                    }
                    catch
                    {
                        badIDFound = true;

                        if (this.lastBadIDValue != tempIDNumber)
                            this.addLineToMessageHandler(tempIDNumber.ToString() + " is a bad ID Number, ID 666 added as filler");

                        this.lastBadIDValue = tempIDNumber;

#if DEBUG
                        this.tempMuxBoxes.Add(new ucShortRangeProbe(666));
#endif

                    }
                }
                if (!badIDFound)
                {
                    this.lastBadIDValue = 65535;
                }

                if (this.tempMuxBoxes.Count > this.MuxBoxes.Count) //if there are more in the temp value, add them
                {
                    for (int i = 0; i < this.tempMuxBoxes.Count - this.MuxBoxes.Count; ++i)
                    {
                        this.addMuxBox();
                    }
                }
                else if (this.tempMuxBoxes.Count < this.MuxBoxes.Count)       //if there are fewer
                {
                    for (int i = 0; i < this.MuxBoxes.Count - this.tempMuxBoxes.Count; ++i)
                    {
                        this.removeMuxBox();
                    }
                }
                foreach (ucShortRangeProbe uSRP in this.MuxBoxes)
                {
                    if (this.tempMuxBoxes == null || this.tempMuxBoxes.Count == 0)
                    {
                        this.addLineToMessageHandler("No IDs");
                        return;
                    }

                    workingProbe = (ucShortRangeProbe)this.tempMuxBoxes[0];

                    if (workingProbe.IDNumber != uSRP.IDNumber)
                    {
                        this.addLineToMessageHandler("ID: " + workingProbe.IDNumber.ToString() + " replaced " + uSRP.IDNumber);
                    }

                    this.tempMuxBoxes.RemoveAt(0);

                    uSRP.IDNumber = workingProbe.IDNumber;
                }
            }
            try
            {
                this.textBoxNoise47KHz.Text = bytePacket[0].ToString();
                this.textBoxNoise52KHz.Text = bytePacket[1].ToString();
                this.textBoxNoise58KHz.Text = bytePacket[2].ToString();
                this.textBoxNoise133KHz.Text = bytePacket[3].ToString();
                this.textBoxNoise153KHz.Text = bytePacket[4].ToString();
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error Setting Noise Levels", ex));
            }

            try
            {
                this.textBoxSignal47KHz.Text = bytePacket[5].ToString();
                this.textBoxSignal52KHz.Text = bytePacket[6].ToString();
                this.textBoxSignal58KHz.Text = bytePacket[7].ToString();

                this.textBoxAge47.Text = bytePacket[24].ToString();
                this.textBoxAge52.Text = bytePacket[25].ToString();
                this.textBoxAge58.Text = bytePacket[26].ToString();
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error Setting Signal and Age Values", ex));
            }

            try
            {
                int i = 8; //point to the first 
                foreach (ucShortRangeProbe uSRP in this.MuxBoxes)
                {
                    uSRP.Signal133KHz = bytePacket[i];
                    uSRP.Signal153KHz = bytePacket[i + 1];
                    uSRP.Age133KHz = bytePacket[i + 19];
                    uSRP.Age153KHz = bytePacket[i + 20];

                    i += 2;
                }
            }
            catch (Exception ex)
            {
                this.errorHandler(new Exception("Error Setting MuxBox Monitoring Values", ex));
            }
        }

        string[] recentErrors = new string[14];

        private void addLineToMessageHandler(string p)
        {
            string displayString = "";

            if (p == recentErrors[recentErrors.Length - 1])          //don't repeat an error
                return;

            for (int i = 0; i < recentErrors.Length - 1; i++)
            {
                recentErrors[i] = recentErrors[i + 1];
            }
            this.recentErrors[this.recentErrors.Length - 1] = p;

            foreach (string s in this.recentErrors)
            {
                displayString += s + "\r\n";
            }

            //this.labelErrorLabel.Text = displayString;
        }

        private void internalMessageHandler(string p)
        {
#if DEBUG
            //this.labelErrorLabel.Text = p;
#endif
        }

        private ArrayList tempMuxBoxes = new ArrayList();

        private bool probeListChanged(int[] tempProbeNumbers)
        {
            bool listChanged = false;

            this.tempMuxBoxes.Clear();

            for (int i = 0; i < 8; i++)
            {
                bool iDExists = false;
                if (tempProbeNumbers[i] < 0 || tempProbeNumbers[i] > 1023)    //bad or no ID value
                {
                    if (tempProbeNumbers[i] != 65535)                     //65535 is just the ID number for NO mux box
                    {
                        this.errorHandler(new Exception(tempProbeNumbers[i].ToString() + " is a bad Mux Box ID value"));
                    }
                }
                else
                {
                    foreach (ucShortRangeProbe uSRP in this.MuxBoxes)            //check if the ID pulled from the packet exists in the current Mux Boxes
                    {
                        if (uSRP.IDNumber == tempProbeNumbers[i])                //if it does, change the bool and add it to the tempMuxBoses
                        {
                            iDExists = true;
                            this.tempMuxBoxes.Add(uSRP);
                        }
                    }
                    if (!iDExists)                                               //if the ID does not exist
                    {
                        listChanged = true;                                     //indicate that the list has changed
                        ucShortRangeProbe workingProbe = new ucShortRangeProbe();
                        workingProbe.IDNumber = tempProbeNumbers[i];
                        this.tempMuxBoxes.Add(workingProbe);           //and add the new probe to the temp list, which will be used later to create a new list
                    }
                }
            }
            if (this.tempMuxBoxes.Count > this.MuxBoxes.Count)
                listChanged = true;
            return listChanged;
        }

        public bool relayFound_forRNCMonitoring = false;

        private void requestMonitoringData()
        {
            SendEventArgs sEA = new SendEventArgs(2);

            sEA.SendPacket[0] = (byte)'N';
            sEA.SendPacket[1] = (byte)0x0D;

            this.onSend(sEA);

            sEA.SendPacket[0] = (byte)'K';

            this.onSend(sEA);
        }

        private void buttonRequest_Click(object sender, EventArgs e)
        {
            SendEventArgs sEA = new SendEventArgs(2);

            sEA.SendPacket[0] = (byte)'K';
            sEA.SendPacket[1] = (byte)0x0D;

            this.onSend(sEA);
        }

        private void onSend(SendEventArgs sEA)
        {
            if (sEA.SendPacket.Length > 3)
                sEA.WithAck = true;
            else
                sEA.WithAck = false;

            if (Send != null)
                Send(this, sEA);
        }



        #endregion

        #region Error Handling
        public delegate void ErrorHandlerDelegate(object o, ExceptionEventArgs eEA);
        public event ErrorHandlerDelegate ErrorHandler;

        private void errorHandler(Exception ex)
        {
            if (ErrorHandler != null)
                ErrorHandler(this, new ExceptionEventArgs(ex, "Error in ShortRange Control"));
            else
                throw new Exception("No Exception Handler For Secondary Monitor Control");
        }

        #endregion

        #region Mux Box functions

        private ArrayList MuxBoxes = new ArrayList();


        private ucShortRangeProbe addMuxBox()
        {

            {
                ucShortRangeProbe workingSRP = new ucShortRangeProbe();

                this.Controls.Add(workingSRP);
                this.MuxBoxes.Add(workingSRP);

                int index = this.MuxBoxes.IndexOf(workingSRP);

                Point p = new Point(workingSRP.Location.X, workingSRP.Location.Y + (index * workingSRP.Size.Height));       //place it vertically

                workingSRP.Location = p;


                return workingSRP;
            }
        }
        private bool checkForDuplicateIDs()
        {
            ucShortRangeProbe uSRP1, uSRP2;

            if (this.MuxBoxes.Count < 2)
                return false;
            for (int i = 0; i < this.MuxBoxes.Count - 1; ++i)
            {
                uSRP1 = (ucShortRangeProbe)this.MuxBoxes[i];
                for (int j = i + 1; j <= this.MuxBoxes.Count - 1; ++j)
                {
                    uSRP2 = (ucShortRangeProbe)this.MuxBoxes[j];

                    if (uSRP2.IDNumber == uSRP1.IDNumber)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void clearAllMuxBoxes()
        {
            if (this.MuxBoxes.Count == 0)
                return;

            while (this.MuxBoxes.Count != 0)
            {
                this.removeMuxBox();
            }
        }
        private void removeMuxBox()
        {
            if (this.MuxBoxes.Count == 0)
                return;
            else
            {
                ucShortRangeProbe workingSRP;
                workingSRP = (ucShortRangeProbe)this.MuxBoxes[this.MuxBoxes.Count - 1];                            //get the last one

                this.MuxBoxes.Remove(workingSRP);
                this.Controls.Remove(workingSRP);

                workingSRP = null;
            }
        }
        #endregion

        #region Monitoring Control
        public void StopMonitoring()
        {
            this.timerMonitor.Enabled = false;
            this.buttonMonitor.Text = "Start Monitoring";
        }

        private void buttonMonitor_Click(object sender, EventArgs e)
        {
            if (this.timerMonitor.Enabled)
            {
                this.timerMonitor.Enabled = false;
                this.buttonMonitor.Text = "Start Monitoring";
            }
            else if (!this.timerMonitor.Enabled && this.relayFound_forRNCMonitoring == true)
            {
                this.timerMonitor.Enabled = true;
                this.buttonMonitor.Text = "Stop Monitoring";
            }
            else if (!this.timerMonitor.Enabled && this.relayFound_forRNCMonitoring == false)
            {
                string text = "Relay not found. Please check for its Power and then start the Monitoring "; // Only for testing - to be removed
                MessageBox.Show(text);// Only for testing - to be removed
                this.timerMonitor.Enabled = false;
                this.buttonMonitor.Text = "Start Monitoring";
            }
        }

        private void timerMonitor_Tick(object sender, EventArgs e)
        {
            if (this.relayFound_forRNCMonitoring == true)
            {
                this.requestMonitoringData();
            }
            else
            {
                this.timerMonitor.Enabled = false;
                this.buttonMonitor.Text = "Start Monitoring";
                this.relayFound_forRNCMonitoring = false;
                string text = "Relay not found. Please check for its Power and then start the Monitoring "; // Only for testing - to be removed
                MessageBox.Show(text);// Only for testing - to be removed
                
            }
        }

        public void DisableMonitoring()
        {
            this.timerMonitor.Enabled = false;
            this.buttonMonitor.Text = "Start Monitoring";
        }

        #endregion

        private void transmitThresholdChange_Click(object sender, EventArgs e)
        {
            Button workingButton = (Button)sender;
            SendEventArgs sEA = new SendEventArgs(3);

            sEA.SendPacket[0] = 0x77; // 'w'
            sEA.SendPacket[2] = 0x0D;

            if (workingButton == this.buttonMinus10)
                sEA.SendPacket[1] = 0x39;
            else if (workingButton == this.buttonMinus3)
                sEA.SendPacket[1] = 0x38;
            else if (workingButton == this.buttonPlus3)
                sEA.SendPacket[1] = 0x37;
            else // plus 10
                sEA.SendPacket[1] = 0x36;

            this.onSend(sEA);
        }

        private void buttonClearChangeCount_Click(object sender, EventArgs e)
        {
            foreach (ucShortRangeTransmitTableItem item in this.transmitTableItems)
            {
                item.ChangeCount = 0;
            }
        }


    }
}