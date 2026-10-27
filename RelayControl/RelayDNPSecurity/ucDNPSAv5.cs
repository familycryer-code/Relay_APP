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
        private bool _initialRequestDone = false;
        

        public ucDNPSAv5()
        {
            InitializeComponent();

            this.intializeComponentEvents();
            this.VisibleChanged += ucDNPSAv5_VisibleChanged;

            this.Enter += ucDNPSAv5_Enter;
            this.Leave += ucDNPSAv5_Leave;

            this.labelCurrentlyLoadedUsers.Location = new System.Drawing.Point(778, 619);
            this.labelCurrentlyLoadedUsers.BringToFront();
        }

        private void ucDNPSAv5_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && !_initialRequestDone)
            {
                _initialRequestDone = true;
                this.RequestAllData();
            }
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
        private DateTime _lastSAv5AutoInitUtc = DateTime.MinValue;
        private DateTime _lastFullRequestUtc = DateTime.MinValue;
        private DateTime _lastSAv5TabLeaveUtc = DateTime.MinValue;

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

        private void ucDNPSAv5_Enter(object sender, EventArgs e)
        {
            InitializeSAv5TabDefaultsOnEnter();
        }

        private void ucDNPSAv5_Leave(object sender, EventArgs e)
        {
            _lastSAv5TabLeaveUtc = DateTime.UtcNow;
        }

        private void InitializeSAv5TabDefaultsOnEnter()
        {
            try
            {
                // Startup already did initial full request; don't auto-refresh immediately on first enter.
                if (_initialRequestDone && _lastSAv5AutoInitUtc == DateTime.MinValue)
                    return;

                // skip if full request just happened
                if ((DateTime.UtcNow - _lastFullRequestUtc).TotalSeconds < 8)
                    return;

                // only refresh if user was away from tab for 30+ seconds
                if (_lastSAv5AutoInitUtc != DateTime.MinValue &&
                    (DateTime.UtcNow - _lastSAv5TabLeaveUtc).TotalSeconds < 30)
                    return;

                _lastSAv5AutoInitUtc = DateTime.UtcNow;

                BeginBusy("SAv5TabEnterInit");
                this.ucDNPSAv5Settings1.RequestSettings();
                this.ucDNPSAv5OSName1.RequestName();
                this.requestLoadedUsers();
            }
            catch (Exception ex)
            {
                Log($"InitializeSAv5TabDefaultsOnEnter exception: {ex.Message}");
                EndBusy("SAv5TabEnterInit exception");
            }
        }

        public void RequestAllData()
        {
            _lastFullRequestUtc = DateTime.UtcNow;
            BeginBusy("RequestAllData");
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
            BeginBusy("DNPSAv5_Send");
            this.onSend(sSEA);
            this.requestLoadedUsers();
            this.showDNPSAV5Error = true;
        }

        
        private void DNPSAv5_Error(object o, ExceptionEventArgs eEA)
        {
            Log($"DNPSAv5_Error: title='{eEA?.Title}', ex='{eEA?.InnerException?.Message}'");
            EndBusy("DNPSAv5_Error");
            this.showSAV5ErrorMessage();
            return;
        }

        // replace Message(...) with this hardened/logged version
        // REPLACE Message(...)
        public void Message(byte[] bytePacket)
        {
            if (bytePacket == null || bytePacket.Length < 1)
            {
                Log($"Message: invalid packet len={bytePacket?.Length ?? 0}");
                EndBusy("Message invalid packet");
                this.showSAV5ErrorMessage();
                return;
            }

            char op = (char)bytePacket[0];
            Log($"Message: opcode='{op}', len={bytePacket.Length}, pendingOps={_pendingOps}");

            switch (op)
            {
                case 'E': // Error
                    this.handleErrorPacket(bytePacket);
                    EndBusy("Message E");
                    break;
                case 'U': // Users
                    this.ShowLoadedUserNumbers(bytePacket);
                    EndBusy("Message U");
                    break;
                case 'O': // OS Name First Packet
                    this.setOSName(bytePacket, 1);
                    EndBusy("Message O");
                    break;
                case 'o': // OS Name Second Packet
                    this.setOSName(bytePacket, 2);
                    EndBusy("Message o");
                    break;
                case 'S': // Settings
                    this.setSettings(bytePacket);
                    EndBusy("Message S");
                    break;
                default:
                    Log($"Message: unknown opcode='{op}'");
                    EndBusy("Message unknown opcode");
                    this.showSAV5ErrorMessage();
                    return;
            }
        }


        public void setOSName(byte[] bytePacket, int p)
        {
            try
            {
                if (bytePacket == null)
                    throw new Exception("setOSName received null packet");

                if (p == 1)
                {
                    if (bytePacket.Length < 2)
                        throw new Exception($"setOSName p1 short packet len={bytePacket.Length}");

                    this.remoteOSNameLength = bytePacket[1];

                    int available = Math.Max(0, bytePacket.Length - 2);
                    int copyLen = Math.Min(this.remoteOSNameLength, available);

                    string part1 = copyLen > 0 ? Encoding.ASCII.GetString(bytePacket, 2, copyLen) : string.Empty;
                    this.oSName = part1.TrimEnd('\0');

                    Log($"setOSName p1: remoteOSNameLength={this.remoteOSNameLength}, available={available}, copyLen={copyLen}, part1Len={this.oSName.Length}");

                    this.ucDNPSAv5OSName1.OSName = this.oSName;
                    Log($"OSName RECV p1 ascii='{this.oSName}', len={this.oSName.Length}, remoteLen={this.remoteOSNameLength}");
                }
                else if (p == 2)
                {
                    int remaining = Math.Max(0, this.remoteOSNameLength - this.oSName.Length);
                    int available = Math.Max(0, bytePacket.Length - 1);
                    int copyLen = Math.Min(remaining, available);

                    string part2 = copyLen > 0 ? Encoding.ASCII.GetString(bytePacket, 1, copyLen) : string.Empty;
                    part2 = part2.TrimEnd('\0');

                    this.oSName = (this.oSName + part2).TrimEnd('\0');

                    Log($"setOSName p2: remaining={remaining}, available={available}, copyLen={copyLen}, totalLen={this.oSName.Length}");

                    this.ucDNPSAv5OSName1.OSName = this.oSName;
                }
                else
                {
                    throw new Exception(p.ToString() + " is a bad number for setOSName()");
                }

                if (this.oSName.Contains("?") && this.ucDNPSAv5OSName1.RequestOSNameClicked)
                {
                    this.ucDNPSAv5OSName1.RequestOSNameClicked = false;
                    this.showSAV5ErrorMessage();
                }
            }
            catch (Exception ex)
            {
                Log($"setOSName exception: {ex.Message}");
                EndBusy("setOSName exception");
                this.showSAV5ErrorMessage();
            }
        }



        private void showSAV5ErrorMessage()
        {
            EndBusy("showSAV5ErrorMessage");
            MessageBox.Show("SAv5 Error", "SAv5 Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void showSAV5ErrorMessage(object sender, ErrorEventArgs eEA)
        {
            EndBusy("showSAV5ErrorMessage");
            MessageBox.Show("SAv5 Error", "SAv5 Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void setSettings(byte[] bytePacket)
        {
            this.ucDNPSAv5Settings1.SetAll(bytePacket);
        }

        // update handleErrorPacket(...)
        // REPLACE handleErrorPacket(...)
        private void handleErrorPacket(byte[] bytePacket)
        {
            if (bytePacket == null || bytePacket.Length < 2)
            {
                Log($"handleErrorPacket: invalid packet len={bytePacket?.Length ?? 0}");
                this.showSAV5ErrorMessage();
                return;
            }

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
                    exceptionMessage =
                        _AuthSymKeyAlgorithmMismatch +
                        $" (AuthEnabled={authenticationEnabled}, KeyAlgIdx={ucDNPSAv5Settings1.GetKeyChangeAlgorithmIndexForDebug()}, MACAlgIdx={ucDNPSAv5Settings1.GetMacAlgorithmIndexForDebug()})";
                    break;
            }

            if (exceptionMessage != null)
            {
                Log($"handleErrorPacket: code={bytePacket[1]}, message='{exceptionMessage}'");
                this.showSAV5ErrorMessage();
                return;
            }

            Log($"handleErrorPacket: code={bytePacket[1]}, message='<none>'");
        }

        // update ShowLoadedUserNumbers(...)
        // REPLACE ShowLoadedUserNumbers(...)
        private void ShowLoadedUserNumbers(byte[] bytePacket)
        {
            try
            {
                if (bytePacket == null || bytePacket.Length < 2)
                    throw new Exception($"ShowLoadedUserNumbers short packet len={bytePacket?.Length ?? 0}");

                this.userNumbers = new List<byte>();
                int i = 1;
                bool terminatorFound = false;

                while (i < bytePacket.Length)
                {
                    if (bytePacket[i] == 0)
                    {
                        terminatorFound = true;
                        break;
                    }

                    this.userNumbers.Add(bytePacket[i]);
                    i++;
                }

                Log($"ShowLoadedUserNumbers: len={bytePacket.Length}, users={this.userNumbers.Count}, terminatorFound={terminatorFound}");

                if (!terminatorFound)
                    throw new Exception("ShowLoadedUserNumbers terminator not found");

                this.displayUserNumbers(this.userNumbers);
            }
            catch (Exception ex)
            {
                Log($"ShowLoadedUserNumbers exception: {ex.Message}");
                EndBusy("ShowLoadedUserNumbers exception");
                this.showSAV5ErrorMessage();
                return;
            }
        }

        // fix null-check order in displayUserNumbers(...)
        // REPLACE displayUserNumbers(...)
        private void displayUserNumbers(List<byte> list)
        {
            try
            {
                this.labelCurrentlyLoadedUsers.Text = "";

                if (list == null || list.Count == 0)
                {
                    this.labelCurrentlyLoadedUsers.Text = "No Users Loaded";
                }
                else
                {
                    foreach (byte b in list)
                    {
                        this.labelCurrentlyLoadedUsers.Text += b.ToString() + ", ";
                    }

                    this.labelCurrentlyLoadedUsers.Text =
                        this.labelCurrentlyLoadedUsers.Text.Substring(0, this.labelCurrentlyLoadedUsers.Text.Length - 2);
                }
            }
            catch (Exception ex)
            {
                Log($"displayUserNumbers exception: {ex.Message}");
                EndBusy("displayUserNumbers exception");
                this.showSAV5ErrorMessage();
                return;
            }
        }

        // remove debug popup
        // REPLACE buttonGetLoadedUsers_Click(...)
        private void buttonGetLoadedUsers_Click(object sender, EventArgs e)
        {
            BeginBusy("buttonGetLoadedUsers_Click");
            this.requestLoadedUsers();
        }

        private void requestLoadedUsers()
        {
            try
            {
                SendEventArgs sSEA = new SendEventArgs(98);
                sSEA.SendPacket[0] = ProjectConstants._DNPControlOpCode;
                sSEA.SendPacket[1] = (byte)'G';
                sSEA.SendPacket[sSEA.SendPacket.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception)

            {

                this.showSAV5ErrorMessage();
                return;
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

        public event Action<string> LogMessage;

        private void Log(string msg)
        {
            try { LogMessage?.Invoke("[DNPSAv5] " + msg); } catch { }
        }

        private int _pendingOps = 0;

        private void BeginBusy(string reason)
        {
            _pendingOps++;
            Log($"BeginBusy reason='{reason}', pendingOps={_pendingOps}");
            this.UseWaitCursor = true;
            Application.UseWaitCursor = true;
        }

        private void EndBusy(string reason)
        {
            if (_pendingOps > 0) _pendingOps--;
            Log($"EndBusy reason='{reason}', pendingOps={_pendingOps}");

            if (_pendingOps == 0)
            {
                this.UseWaitCursor = false;
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
            }
        }

        public bool ValidateAuthorityKeyForSelectedAlgorithm(out string validationError)
        {
            validationError = null;

            int keyAlg = this.ucDNPSAv5Settings1.GetKeyChangeAlgorithmIndexForDebug();
            int keyLen = this.ucDNPSAv5AuthoritySym1.GetCurrentKeyLengthForDebug();

            if (keyAlg < 0)
            {
                validationError = "Key Change Algorithm is not selected.";
                return false;
            }

            if (keyLen <= 0)
            {
                validationError = "Authority Symmetrical Key is empty or invalid.";
                return false;
            }

            // Firmware rule from dnp_CheckForKeyChangeAlgorithmMismatch():
            // method 0 => 16 bytes
            // method 1 or 2 => 32 bytes
            if (keyAlg == 0 && keyLen != 16)
            {
                validationError = $"Key Change Algorithm {keyAlg} requires a 16-byte Authority Key, but current key length is {keyLen}.";
                return false;
            }

            if ((keyAlg == 1 || keyAlg == 2) && keyLen != 32)
            {
                validationError = $"Key Change Algorithm {keyAlg} requires a 32-byte Authority Key, but current key length is {keyLen}.";
                return false;
            }

            // Unknown future algorithms - fail safe
            if (keyAlg > 2)
            {
                validationError = $"Unsupported Key Change Algorithm index {keyAlg}.";
                return false;
            }

            return true;
        }

    }
}
