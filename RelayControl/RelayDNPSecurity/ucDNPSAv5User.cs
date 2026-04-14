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
    public partial class ucDNPSAv5User : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5User()
        {
            InitializeComponent();
            this.initializeKeyValueControl();

        }

        private static int _userNameLimit = 40;
        private ucKeyValuesInputControl keyBox;
        private static string _keyName = "Symmetrical User Update Key - 16/32 key bytes";
        private static int _packetLength = 98;

        #region Initialization

        void initializeKeyValueControl()
        {
            this.keyBox = new ucKeyValuesInputControl(32, _keyName);

            Point tempPoint = new Point(this.textBoxUserNumber.Location.X, this.textBoxUserNumber.Location.Y);

           // tempPoint.X += 5 + this.textBoxUserNumber.Width;
            tempPoint.X = 115;
            tempPoint.Y -= 5;

            this.keyBox.Location = tempPoint;

            this.groupBoxUserControl.Controls.Add(this.keyBox);

            this.label1.Location = new System.Drawing.Point(170, 117); //(29, 96);
            this.textBoxUserName.Location = new System.Drawing.Point(250, 115); //(98, 93);

            this.labelUserNumber.Location = new System.Drawing.Point(9, 32); //(9, 22);
            this.textBoxUserNumber.Location = new System.Drawing.Point(67, 30); //(67, 19);
            this.labelUserRole.Location = new System.Drawing.Point(9, 72); //(9, 50);
            this.comboBoxUserRole.Location = new System.Drawing.Point(67, 70); //(67, 45);
        }

        #endregion

        #region Validation

        private bool validUserName()
        {
            //TODO - might not need to do anything
            return true;
        }

        private void textBoxUserName_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxUserName.Text.Length > _userNameLimit)
                this.textBoxUserName.Text = this.textBoxUserName.Text.Substring(0, _userNameLimit);
        }

        #endregion

        #region Send Control

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            try
            {

                this.sendUserName();
                // Send User Key must go second, it is where it is flashed
                this.sendUserKey();
            }
            catch (Exception ex)
            {
                this.onError(ex, "Error Adding SAv5 User");
            }
        }

        private void sendUserName()
        {
            if (!this.validUserName())
            {
                this.onError(new Exception("Bad User Name"), "Bad SAv5 User Name");
            }

            SendEventArgs sSEA = new SendEventArgs(_userNameLimit + 3);
            sSEA.WithAck = true;

            sSEA.SendPacket = getUserNamePacket();

            this.onSend(sSEA);
        }

        private byte[] getUserNamePacket()
        {
            byte[] returnArray = new byte[98];
            try
            {
                returnArray[0] = ProjectConstants._DNPControlOpCode;
                returnArray[1] = (byte)'N'; //For Name
                returnArray[2] = this.getUserNumer();
                returnArray[3] = this.getUserRole();
                returnArray[4] = (byte)this.textBoxUserName.Text.Length;
                byte[] tempArray = Encoding.ASCII.GetBytes(this.textBoxUserName.Text.ToString());
                Array.Copy(tempArray, 0, returnArray, 5, tempArray.Length);
                returnArray[returnArray.Length - 1] = 0x0D;
            }
            catch (Exception ex)
            {
                this.onError(ex, "Error getting SAv5 User Name Packet");
            }

            return returnArray;
        }

        private byte getUserRole()
        {
            try
            {
                return Convert.ToByte(this.comboBoxUserRole.SelectedIndex);
            }
            catch
            {
                throw new Exception("Bad User Role Value");
            }
        }

        private byte getUserNumer()
        {
            try
            {
                return Convert.ToByte(this.textBoxUserNumber.Text);
            }
            catch
            {
                throw new Exception("Bad User Number Value");
            }
        }

        private void sendUserKey()
        {
            if (!this.keyBox.KeyDataValid())
            {
                this.onError(new Exception("Bad Key Data"), "Bad SAv5 Key Data");
            }

            SendEventArgs sSEA = new SendEventArgs(_packetLength);
            sSEA.WithAck = true;

            sSEA.SendPacket = this.getUserUpdateKeyPacket();

            this.onSend(sSEA);
        }

        private byte[] getUserUpdateKeyPacket()
        {
            byte[] returnArray = new byte[98];
            byte[] tempArray = null;
            try
            {
                returnArray[0] = ProjectConstants._DNPControlOpCode;
                returnArray[1] = (byte)'K'; //For Name
                returnArray[2] = this.getUserNumer();
                returnArray[3] = this.getUserRole();
                try
                {
                    tempArray = this.keyBox.GetKey();
                    returnArray[4] = (byte)tempArray.Length;
                    Array.Copy(tempArray, 0, returnArray, 5, tempArray.Length);
                }
                catch (Exception ex)
                {
                    this.onError(ex, "Error getting SAv5 User Update Key Packet");
                }
                returnArray[returnArray.Length - 1] = 0x0D;
            }
            catch (Exception ex)
            {
                this.onError(ex, "Error formulating SAv5 User Update Key packet");
            }

            return returnArray;
        }
        #endregion

        private void buttonDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Delete User", "Are you sure you want to delete the user?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (dr == DialogResult.Yes)
                {
                    this.deleteUser();
                }
            }
            catch (Exception ex)
            {
                this.onError(new Exception("Error Deleting User: " + ex.Message, ex), "Error Deleting SAv5 User");
            }
        }

        private void deleteUser()
        {
            byte temp = 0;

            try
            {
                temp = Convert.ToByte(this.textBoxUserNumber.Text);
            }
            catch
            {
                throw new Exception("Need a valid User Number");
            }

            try
            {
                SendEventArgs sSEA = new SendEventArgs(98);
                sSEA.WithAck = true;

                sSEA.SendPacket[0] = (byte)ProjectConstants._DNPControlOpCode;
                sSEA.SendPacket[1] = (byte)'D'; // Delete User
                sSEA.SendPacket[2] = temp;
                sSEA.SendPacket[sSEA.SendPacket.Length - 1] = 0x0D;

                this.onSend(sSEA);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Sending Delete User Packet: " + ex.Message);
            }
        }

        public void SetUserNumber(int userNumber)
        {
            this.textBoxUserNumber.Text = userNumber.ToString();
        }
        public void SetUserName(string name)
        {
            this.textBoxUserName.Text = name;
        }

        public void SetUserKey(byte[] keyData)
        {
            this.keyBox.SetKey(keyData);
        }

        public void SetUserRole(int roleNumber)
        {
            this.comboBoxUserRole.SelectedIndex = roleNumber;
        }

    }
}
