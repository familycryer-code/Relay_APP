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
    public partial class ucDNPSAv5AuthoritySym : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5AuthoritySym()
        {
            InitializeComponent();
            this.panel_dnpAuthKey.Location = new System.Drawing.Point(7, 6);
            this.panel_dnpAuthKey.Size = new System.Drawing.Size(852, 105);
            this.initializeKeyValueControl();
        }

        private ucKeyValuesInputControl keyBox;
        private static string _keyName = "Authority Symmetrical Key";
        private static int _packetLength = 98;

        #region Initialization

        void initializeKeyValueControl()
        {
            this.keyBox = new ucKeyValuesInputControl(32, _keyName);

            Point tempPoint = new Point(5, 5);

            this.keyBox.Location = tempPoint;

            this.Controls.Add(this.keyBox);
            this.buttonSendKey.Location = new System.Drawing.Point(900, 40); //new Point(this.keyBox.Location.X + this.keyBox.Width + 5, this.keyBox.Location.Y + 10);
            this.keyBox.BringToFront();
        }

        #endregion

        public void SetAuthorityKey(byte[] keyData)
        {
            this.keyBox.SetKey(keyData);
        }

        private void buttonSendKey_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.keyBox.KeyDataValid())
                {
                    this.sendKeyData();
                }
            }
            catch (Exception ex)
            {
                this.onError(ex, "Error Sending KeyData");
            }
        }

        private void sendKeyData()
        {
            SendEventArgs sSEA = new SendEventArgs(_packetLength);
            byte[] tempArray = null;
            sSEA.WithAck = true;
            try
            {
          //      MessageBox.Show("9 sending command D to master"); // Only for testing - to be removed
                sSEA.SendPacket[0] = ProjectConstants._DNPControlOpCode;
                sSEA.SendPacket[1] = (byte)'A'; // For Authority Key
                try
                {
                    tempArray = this.keyBox.GetKey();
                    sSEA.SendPacket[2] = (byte)tempArray.Length;
                    Array.Copy(tempArray, 0, sSEA.SendPacket, 3, tempArray.Length);
                }
                catch (Exception ex)
                {
                    this.onError(ex, "Error copying SAv5 Key Data");
                }
                sSEA.SendPacket[sSEA.SendPacket.Length - 1] = 0x0D;
            }
            catch (Exception ex)
            {
                this.onError(ex, "Error Sending SAv5 Key Data");
                return;
            }

            this.onSend(sSEA);
        }
    }
}
