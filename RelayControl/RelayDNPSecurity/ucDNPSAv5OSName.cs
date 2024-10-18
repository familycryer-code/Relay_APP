using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharedResources;

namespace RelayDNPSecurity
{
    public partial class ucDNPSAv5OSName : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5OSName()
        {
            InitializeComponent();
        }
        public event ExceptionHandler DNPOSNameException;
        public int SerialNumber = 0;
        public string OSName
        {
            get { return this.textBoxOSName.Text; }
            set
            {
                // this.textBoxOSName.Text = value;
                this.textBoxOSName.Text = _OSDefaultName + value;
            }
        }

        private static int _maxOSNameLength = 70; // Make sure this matches relay
        private static string _OSDefaultName = "DIGITALGRID, INC. DNP Relay Serial Number: ";

        private static int _packetLength = 98;

        private bool requestOSNameClicked = false;

        public bool RequestOSNameClicked
        {
            get { return this.requestOSNameClicked; }
            set
            {
                this.requestOSNameClicked = value;
            }
        }

        public void textBoxOSName_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxOSName.Text.Length > _maxOSNameLength)
            {
                this.textBoxOSName.Text = this.textBoxOSName.Text.Substring(0, _maxOSNameLength);
                this.textBoxOSName.Select(_maxOSNameLength, 0);
            }
        }

        //   private void buttonSendName_Click(object sender, EventArgs e)
        public void buttonSendName_Click(object sender, EventArgs e)
        {
            sendOSName();
        }

        public void sendOSName()
        {
            // this.buttonGenerateName.BackColor = Color.Blue;
            SendEventArgs sSEA = new SendEventArgs(_packetLength);
            sSEA.WithAck = true;
            try
            {
                int i = 3;

                sSEA.SendPacket[0] = ProjectConstants._DNPControlOpCode;
                sSEA.SendPacket[1] = (byte)'O'; // For OS Name
                sSEA.SendPacket[2] = (byte)this.textBoxOSName.Text.Length;

                foreach (char c in this.textBoxOSName.Text)
                {
                    sSEA.SendPacket[i] = (byte)c;
                    i++;
                }

                sSEA.SendPacket[sSEA.SendPacket.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Sending OS/Relay Name: " + ex.ToString()), "Error Sending Name");
            }
        }

        public void buttonGenerateName_Click(object sender, EventArgs e)
        {
            newOSname();
        }

        public void newOSname()
        {
            try
            {
                this.textBoxOSName.Text = _OSDefaultName + this.SerialNumber.ToString();
                //this.textBoxOSName.Text = _OSDefaultName + this.OSName;
            }
            catch (Exception ex)
            {
                this.errorHandler(ex);
            }
        }

        private void buttonRequestName_Click(object sender, EventArgs e)
        {
            this.requestOSNameClicked = true;
            this.RequestName();
        }

        public void RequestName()
        {
            try
            {
                SendEventArgs sSEA = new SendEventArgs(_packetLength);

                sSEA.SendPacket[0] = ProjectConstants._DNPControlOpCode;
                sSEA.SendPacket[1] = (byte)'o'; // Get OS Name
                sSEA.SendPacket[sSEA.SendPacket.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Requesting OS/Relay Name: " + ex.ToString()), "Error Requesting Name");
            }
        }

        private void errorHandler(Exception ex)
        {
            if (DNPOSNameException != null)
            {
                DNPOSNameException(this, new ExceptionEventArgs(ex, "Error in DNPSAv5 Outstation Name"));
            }
            else
            {
                throw new Exception("No Exception Handler For DNPSAv5 OS Name");
            }
        }

    }
}
