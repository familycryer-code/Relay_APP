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
    public partial class ucDNPSAv5AuthoritySym : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5AuthoritySym()
        {
            InitializeComponent();
            this.initializeKeyValueControl();
        }

        private ucKeyValuesInputControl keyBox;
        private static string _keyName = "Authority Symmetrical Key";

        #region Initialization

        void initializeKeyValueControl()
        {
            this.keyBox = new ucKeyValuesInputControl(32, _keyName);

            Point tempPoint = new Point(10, 20);

            this.keyBox.Location = tempPoint;

            this.groupBoxMain.Controls.Add(this.keyBox);
        }

        #endregion

        private void buttonSendKey_Click(object sender, EventArgs e)
        {
            if (this.keyBox.KeyDataValid())
            {
                this.sendKeyData();
            }
        }

        private void sendKeyData()
        {
            SecureSendEventArgs sSEA = new SecureSendEventArgs(98);
            byte[] tempArray = null;
            try
            {
                sSEA.Data[0] = (byte)RelayModeFunctions._DNPControlOpCode;
                sSEA.Data[1] = (byte)'A'; // For Authority Key
                try
                {
                    tempArray = this.keyBox.GetKey();
                    sSEA.Data[2] = (byte)tempArray.Length;
                    Array.Copy(tempArray, 0, sSEA.Data, 3, tempArray.Length);
                }
                catch (Exception ex)
                {
                    this.onError(ex);
                }
                sSEA.Data[sSEA.Data.Length - 1] = 0x0D;
            }
            catch (Exception ex)
            {
                this.onError(ex);
            }

            this.onSend(sSEA);
        }
    }
}
