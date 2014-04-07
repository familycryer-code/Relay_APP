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

            #if !DEBUG
            this.buttonLoadDefaultUser.Visible = false;
            #endif
        }

        public int SerialNumber
        {
            get { return this.serialNumber; }
            set
            {
                this.serialNumber = value;
                this.ucDNPSAv5OSName1.SerialNumber = value;
            }
        }

        private List<byte> userNumbers = new List<byte>();

        private static string _TooManyUsers = "User limit reached, please delete user before preceeding";
        private static string _UserDoesNotExist = "User Number Does Not Exist in Relay";
        private static string _InvalidPublicOSKey = "Invalid Public Key.  Has it been generated?";
        private static string _AuthSymKeyAlgorithmMismatch = "Authority Key Length does not match Key Change Algorithm expected length.  Please Check and Resend";


        private static byte[] _defaultUserKey = { 0x49, 0xc8, 0x7d, 0x5d, 0x90, 0x21, 0x7a, 0xaf, 0xec, 0x80, 0x74, 0xeb, 0x71, 0x52, 0xfd, 0xb5 };
        private static string _defaultUserName = "Common";
        private static int _defaultUserRole = 1;
        private static int _defaultUserNumber = 1;
        private string oSName = "";
        private int remoteOSNameLength;

        private int serialNumber = 0;

        public void RequestAllData()
        {
            this.requestLoadedUsers();
            this.ucDNPSAv5OSName1.RequestName();
            this.ucDNPSAv5Settings1.RequestSettings();
        }

        private void intializeComponentEvents()
        {
            this.ucDNPSAv5User1.Send += DNPSAv5_Send;
            this.ucDNPSAv5User1.Error += DNPSAv5_Error;
            this.ucDNPSAv5AuthoritySym1.Send += DNPSAv5_Send;
            this.ucDNPSAv5AuthoritySym1.Error += DNPSAv5_Error;
            this.ucDNPSAv5OSName1.Send += DNPSAv5_Send;
            this.ucDNPSAv5OSName1.Error += DNPSAv5_Error;
            this.ucDNPSAv5Settings1.Send += DNPSAv5_Send;
            this.ucDNPSAv5Settings1.Error += DNPSAv5_Error;
        }

        private void DNPSAv5_Send(object o, SecureSendEventArgs sSEA)
        {
            this.onSend(sSEA);
            this.requestLoadedUsers();
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
                case 'O': // OS Name FIrst Packet
                    this.setOSName(bytePacket, 1);
                    break;
                case 'o': // OS Name Second Packet
                    this.setOSName(bytePacket, 2);
                    break;
                case 'S': // Settings
                    this.setSettings(bytePacket);
                    break;
                default:
                    this.onError(new Exception(((char)bytePacket[0]).ToString() + " is not a valid SAv5 SCI Command"));
                    break;
            }
        }

        private void setOSName(byte[] bytePacket, int p)
        {
            string workingString = Encoding.ASCII.GetString(bytePacket, 1, 35);

            try
            {
                if (p == 1)
                {
                    this.remoteOSNameLength = bytePacket[1];
                    if (this.remoteOSNameLength < 35)
                    {
                        workingString = Encoding.ASCII.GetString(bytePacket, 2, this.remoteOSNameLength);
                        this.remoteOSNameLength = 0;
                    }
                    else
                    {
                        workingString = Encoding.ASCII.GetString(bytePacket, 2, 35);
                        this.remoteOSNameLength -= 35;
                    }
                    this.oSName = workingString;
                    this.ucDNPSAv5OSName1.OSName = this.oSName;
                }
                else if (p == 2)
                {
                    if(this.remoteOSNameLength < 35)
                        workingString = Encoding.ASCII.GetString(bytePacket, 1, this.remoteOSNameLength);
                    else
                        workingString = Encoding.ASCII.GetString(bytePacket, 1, 35);
                    this.oSName += workingString;
                    this.ucDNPSAv5OSName1.OSName = this.oSName;
                }
                else
                    throw new Exception(p.ToString() + " is a bad number for setOSName()");
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Setting OS/Relay Name: " + ex.ToString()));
            }
        }

        private void setSettings(byte[] bytePacket)
        {
            this.ucDNPSAv5Settings1.SetAll(bytePacket);
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
                case 4:
                    exceptionMessage = _AuthSymKeyAlgorithmMismatch;
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
            this.requestLoadedUsers();
        }

        private void requestLoadedUsers()
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

        private void buttonLoadDefaultUser_Click(object sender, EventArgs e)
        {
            this.ucDNPSAv5User1.SetUserName(_defaultUserName);
            this.ucDNPSAv5User1.SetUserKey(_defaultUserKey);
            this.ucDNPSAv5User1.SetUserRole(_defaultUserRole);
            this.ucDNPSAv5User1.SetUserNumber(_defaultUserNumber);
        }

    }
}
