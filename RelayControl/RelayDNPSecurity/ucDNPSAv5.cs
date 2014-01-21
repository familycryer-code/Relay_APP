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
    public partial class ucDNPSAv5 : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5()
        {
            InitializeComponent();

            this.ucDNPSAv5User1.Send += User_Send;
            this.ucDNPSAv5User1.Error += DNP_Error;
        }

        private void User_Send(object o, SecureSendEventArgs sSEA)
        {
            this.onSend(sSEA);
        }

        private void DNP_Error(object o, Exception ex)
        {
            this.onError(ex);
        }

        public void Message(byte[] bytePacket)
        {
            switch ((char)bytePacket[0])
            {
                case 'U': //Users
                    this.ShowLoadedUserNumbers(bytePacket);
                    break;
            }
        }

        private void ShowLoadedUserNumbers(byte[] bytePacket)
        {
            try
            {
            }
            catch
            {
            }
        }

        private void buttonGetLoadedUsers_Click(object sender, EventArgs e)
        {
            try
            {
                SecureSendEventArgs sSEA = new SecureSendEventArgs();

                sSEA.Data[0] = (byte)RelayModeFunctions._DNPControlOpCode;
                sSEA.Data[1] = (byte)'G';
                sSEA.Data[sSEA.Data.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Loadinged Loaded Users", ex));
            }
        }
    }
}
