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

            this.intializeComponentEvents();
        }
        private List<byte> userNumbers = new List<byte>();

        private static string _TooManyUsers = "User limit reached, please delete user before preceeding";
        private static string _UserDoesNotExist = "User Number Does Not Exist in Relay";
        private static string _InvalidPublicOSKey = "Invalid Public Key.  Has it been generated?";

        private void intializeComponentEvents()
        {
            this.ucDNPSAv5User1.Send += DNPSAv5_Send;
            this.ucDNPSAv5User1.Error += DNPSAv5_Error;
        }

        private void DNPSAv5_Send(object o, SecureSendEventArgs sSEA)
        {
            this.onSend(sSEA);
        }

        private void DNPSAv5_Error(object o, Exception ex)
        {
            this.onError(ex);
        }

        public void Message(byte[] bytePacket)
        {
            switch ((char)bytePacket[0])
            {
                case 'E': // Error
                    this.handleErrorPacket(bytePacket);
                    break;
                case 'U': // Users
                    this.ShowLoadedUserNumbers(bytePacket);
                    break;
                case 'O': // OS Public Key
                    break;//this.
            }
        }

        private void handleErrorPacket(byte[] bytePacket)
        {
            string exceptionMessage;

            switch (bytePacket[1])
            {
                case 0:
                default:
                    exceptionMessage = "Invalid Error Number.  Please Contact Digital Grid with this Number: " + bytePacket[1].ToString();
                    break;
                case 1:
                    exceptionMessage = _TooManyUsers;
                    break;
                case 2:
                    exceptionMessage = _UserDoesNotExist;
                    break;
                case 3:
                    exceptionMessage = _InvalidPublicOSKey;
                    break;
            }
            this.onError(new Exception(exceptionMessage));
        }

        private void ShowLoadedUserNumbers(byte[] bytePacket)
        {
            int i = 1;

            try
            {
                this.userNumbers = new List<byte>();

                while (bytePacket[i] != 0)
                {
                    this.userNumbers.Add(bytePacket[i]);
                    i++;
                }

                this.displayUserNumbers(this.userNumbers);
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Populating User Numbers List: " + ex.Message, ex));
            }
        }

        private void displayUserNumbers(List<byte> list)
        {

            try
            {
                this.labelCurrentlyLoadedUsers.Text = "";

                if (list.Count == 0 || list == null)
                {
                    this.labelCurrentlyLoadedUsers.Text = "No Users Loaded";
                }
                else
                {
                    foreach (byte b in list)
                    {
                        this.labelCurrentlyLoadedUsers.Text += b.ToString() + ", ";
                    }
                    // Remove the last ", "
                    this.labelCurrentlyLoadedUsers.Text = this.labelCurrentlyLoadedUsers.Text.Substring(0, this.labelCurrentlyLoadedUsers.Text.Length - 2);
                }
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error populating User Numbers: " + ex.Message, ex));
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
