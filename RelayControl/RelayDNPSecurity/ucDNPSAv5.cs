using RelayControlLibrary;
using SharedResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayDNPSecurity
{
    public partial class ucDNPSAv5 : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5()
        {
            InitializeComponent();

            this.intializeComponentEvents();

#if !DEBUG
            this.buttonLoadDefaultAuthorityKey.Visible = false;
            this.buttonLoadDefaultUser.Visible = false;
#endif

            this.labelCurrentlyLoadedUsers.Location = new System.Drawing.Point(778, 619); 
            this.labelCurrentlyLoadedUsers.BringToFront();

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
        private bool authenticationEnabled = false;

        private static byte[] _defaultAuthorityKey = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06,
                                                       0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
        private static byte[] _defaultUserKey = { 0x49, 0xc8, 0x7d, 0x5d, 0x90, 0x21, 0x7a, 0xaf, 0xec, 0x80, 0x74, 0xeb, 0x71, 0x52, 0xfd, 0xb5 };
        private static string _defaultUserName = "Common";
        private static int _defaultUserRole = 2; // 2- Engineer
        private static int _defaultUserNumber = 1;
        private string oSName = "";
        private int remoteOSNameLength;

        private int serialNumber = 0;
        private bool showDNPSAV5Error = true;

        public bool ShowDNPSAV5Error
        {
            get { return this.showDNPSAV5Error; }
            set
            {
                this.showDNPSAV5Error = value;
            }
        }

        private void tab_subTabsDNPSAv5_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tab = tab_subTabsDNPSAv5.TabPages[e.Index];
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Background of the selected tab title 
            //using (var backBrush = new SolidBrush(isSelected ? Color.FromArgb(135, 206, 250) : SystemColors.Control)) // selected tab title has a blue background
            using (var backBrush = new SolidBrush(isSelected ? Color.FromArgb(255, 215, 0) : SystemColors.Control)) // selected tab title has a gold colored background
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            // Text color: blue for selected, gray for others (adjust as needed)
            var textColor = isSelected ? Color.Black : SystemColors.ControlText; // Color.Black is the color of the selected tab title
            using (var textBrush = new SolidBrush(textColor))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.DrawString(tab.Text, e.Font, textBrush, e.Bounds, format);
            }

            // Optional focus rectangle
            e.DrawFocusRectangle();

            if (this.tab_subTabsDNPSAv5.SelectedTab == this.tabPage2)
                this.labelCurrentlyLoadedUsers.Visible = true;
            if (this.tab_subTabsDNPSAv5.SelectedTab == this.tabPage1)
                this.labelCurrentlyLoadedUsers.Visible = false;
        }

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

        private void DNPSAv5_Send(object o, SendEventArgs sSEA)
        {
            this.onSend(sSEA);
            this.requestLoadedUsers();
            this.showDNPSAV5Error = true;
        }

        private void DNPSAv5_Error(object o, ExceptionEventArgs eEA)
        {
#if DEBUG
                this.onError(eEA.InnerException, eEA.Title);
#else
            this.showSAV5ErrorMessage();
            return;
#endif
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
#if DEBUG
                    this.onError(new Exception(((char)bytePacket[0]).ToString() + " is not a valid SAv5 SCI Command"), "Error in SAv5 Message");
#else
                    this.showSAV5ErrorMessage();
                    return;
#endif
                    break;
            }

        }

        //private void setOSName(byte[] bytePacket, int p)
        public void setOSName(byte[] bytePacket, int p)
        {
            string workingString = Encoding.ASCII.GetString(bytePacket, 1, 35);

            try
            {
                if (p == 1)
                {
                    this.remoteOSNameLength = bytePacket[1];
                    //TEST this will need to be cleaned up, name comes all in one packet now

                    if (this.remoteOSNameLength <= 70)
                    {
                        workingString = Encoding.ASCII.GetString(bytePacket, 2, this.remoteOSNameLength);
                        this.remoteOSNameLength = 0;
                    }
                    else
                    {
                        workingString = Encoding.ASCII.GetString(bytePacket, 2, 70);
                    }
                    this.oSName = workingString;
                    //this.ucDNPSAv5OSName1.OSName = this.oSName;
                    this.ucDNPSAv5OSName1.OSName = this.SerialNumber.ToString();
                    if (this.oSName.Contains("?") && this.ucDNPSAv5OSName1.RequestOSNameClicked)
                    {
                        this.ucDNPSAv5OSName1.RequestOSNameClicked = false;
                        this.showSAV5ErrorMessage();
                    }
                }
                else if (p == 2)
                {
                    if (this.remoteOSNameLength < 35)
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
#if DEBUG
                this.onError(new Exception("Error Setting OS/Relay Name: " + ex.ToString()), "Error in SAv5 Setting OS");
#else
                this.showSAV5ErrorMessage();
                return;
#endif
            }
        }

        private void showSAV5ErrorMessage()
        {
            if (showDNPSAV5Error)
            {
                showDNPSAV5Error = false;
                string dNPSAV5ErrorString = "Error in DNP SAV5 Security Settings. Check Security Settings";
                this.onError(new Exception(dNPSAV5ErrorString), "Error in DNP SAV5 Security Settings");
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
                    exceptionMessage = "Invalid Error Number.  Please Contact DIGITALGRID, INC. with this Number: " + bytePacket[1].ToString();
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
                    authenticationEnabled = ucDNPSAv5Settings1.AuthenticationEnabled;
                    if (authenticationEnabled)
                        exceptionMessage = _AuthSymKeyAlgorithmMismatch;
                    else
                        exceptionMessage = null;
                    break;
            }
            if (exceptionMessage != null)
            {
#if DEBUG
                this.onError(new Exception(exceptionMessage), "Error in SAv5 Handling Error Packet");
#else
                this.showSAV5ErrorMessage();
                return;
#endif
            }
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
#if DEBUG
                this.onError(new Exception("Error Populating User Numbers List: " + ex.Message, ex), "Error in SAv5 Showing user Numbers");
#else
                this.showSAV5ErrorMessage();
                return;
#endif
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
#if DEBUG
                this.onError(new Exception("Error populating User Numbers: " + ex.Message, ex), "Error in SAv5 Dsiplaying User Numbers");
#else
                this.showSAV5ErrorMessage();
                return;
#endif
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
                SendEventArgs sSEA = new SendEventArgs(98);
         //       MessageBox.Show("8 sending command D to master"); // Only for testing - to be removed
                sSEA.SendPacket[0] = ProjectConstants._DNPControlOpCode;
                sSEA.SendPacket[1] = (byte)'G';
                sSEA.SendPacket[sSEA.SendPacket.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
#if DEBUG
                this.onError(new Exception("Error Loading Loaded Users", ex),"Error in SAv5 Requesting User");
#else
                this.showSAV5ErrorMessage();
                return;
#endif
            }
        }

        private void buttonLoadDefaultUser_Click(object sender, EventArgs e)
        {
            this.ucDNPSAv5User1.SetUserName(_defaultUserName);
            this.ucDNPSAv5User1.SetUserKey(_defaultUserKey);
            this.ucDNPSAv5User1.SetUserRole(_defaultUserRole);
            this.ucDNPSAv5User1.SetUserNumber(_defaultUserNumber);
        }

        private void buttonLoadDefaultAuthorityKey_Click(object sender, EventArgs e)
        {
            this.ucDNPSAv5AuthoritySym1.SetAuthorityKey(_defaultAuthorityKey);
        }

    }
}
