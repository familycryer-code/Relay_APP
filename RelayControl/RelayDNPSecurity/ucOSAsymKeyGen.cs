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
    public partial class ucOSAsymKeyGen : ucDNPSAv5SuperClass
    {
        public ucOSAsymKeyGen()
        {
            InitializeComponent();
            this.initializeKeyValueControl();
        }

        private ucKeyValuesInputControl privateKeyBox;
        private ucKeyValuesInputControl publicKeyBox;
        private static string _privateKeyName = "Relay (Outstation) Private Key";
        private static string _publicKeyName = "Relay (Outstation) Private Key";

        #region Initialization

        void initializeKeyValueControl()
        {
            this.privateKeyBox = new ucKeyValuesInputControl(32, _privateKeyName);
            this.publicKeyBox = new ucKeyValuesInputControl(32, _publicKeyName);

            Point tempPoint = new Point(40, 15);
            this.privateKeyBox.Location = tempPoint;

            tempPoint = new Point(this.privateKeyBox.Location.X, this.privateKeyBox.Location.Y + this.privateKeyBox.Height + 5);
            this.publicKeyBox.Location = tempPoint;

            this.groupBoxMain.Controls.Add(this.privateKeyBox);
            this.groupBoxMain.Controls.Add(this.publicKeyBox);
        }

        #endregion

        private void buttonGetKeyPair_Click(object sender, EventArgs e)
        {            

        }

        private void buttonGenerateKey_Click(object sender, EventArgs e)
        {
            try
            {
                SecureSendEventArgs sSEA = new SecureSendEventArgs();
                sSEA.Data[0] = (byte)RelayModeFunctions._DNPControlOpCode;
                sSEA.Data[1] = (byte)'g'; // Generate Key Data
                sSEA.Data[2] = 1; // 1 for 128, 2 for 256 BYTES
                sSEA.Data[sSEA.Data.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Sending Generate Key Packet: " + ex.Message);
            }
        }

        private void buttonSendKeyPair_Click(object sender, EventArgs e)
        {

        }
    }
}
