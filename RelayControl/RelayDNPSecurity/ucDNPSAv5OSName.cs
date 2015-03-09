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
    public partial class ucDNPSAv5OSName : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5OSName()
        {
            InitializeComponent();
        }

        public int SerialNumber = 0;
        public string OSName
        {
            get { return this.textBoxOSName.Text; }
            set
            {
                this.textBoxOSName.Text = value;
            }
        }

        private static int _maxOSNameLength = 70; // Make sure this matches relay
        private static string _OSDefaultName = "DigitalGrid Inc, DNP Relay Serial Number: ";

        private static int _packetLength = 98;

        private void textBoxOSName_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (this.textBoxOSName.Text.Length > _maxOSNameLength)
            {
                this.textBoxOSName.Text = this.textBoxOSName.Text.Substring(0, _maxOSNameLength);
                this.textBoxOSName.Select(_maxOSNameLength, 0);
            }
        }

        private void buttonSendName_Click(object sender, EventArgs e)
        {
            SecureSendEventArgs sSEA = new SecureSendEventArgs(_packetLength);
            try
            {
                int i = 3;

                sSEA.Data[0] = (byte)RelayModeFunctions._DNPControlOpCode;
                sSEA.Data[1] = (byte)'O'; // For OS Name
                sSEA.Data[2] = (byte)this.textBoxOSName.Text.Length;
                
                foreach (char c in this.textBoxOSName.Text)
                {
                    sSEA.Data[i] = (byte)c;
                    i++;
                }

                sSEA.Data[sSEA.Data.Length - 1] = 0x0D;
                
                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Sending OS/Relay Name: " + ex.ToString()));
            }
        }

        private void buttonGenerateName_Click(object sender, EventArgs e)
        {
            this.textBoxOSName.Text = _OSDefaultName + this.SerialNumber.ToString();
        }

        private void buttonRequestName_Click(object sender, EventArgs e)
        {
            this.RequestName();
        }

        public void RequestName()
        {
            try
            {
                SecureSendEventArgs sSEA = new SecureSendEventArgs(_packetLength);

                sSEA.Data[0] = (byte)RelayModeFunctions._DNPControlOpCode;
                sSEA.Data[1] = (byte)'o'; // Get OS Name
                sSEA.Data[sSEA.Data.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Requesting OS/Relay Name: " + ex.ToString()));
            }
        }
    }
}
