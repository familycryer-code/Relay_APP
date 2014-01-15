using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayDNPSecurity
{
    public partial class ucDNPSAv5User : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5User()
        {
            InitializeComponent();
            this.initializeKeyValueControl();
            
        }

        private static int _userNameLimit = 100;
        private ucKeyValuesInputControl keyBox;
        private static string _keyName = "Symmetrical User Update Key - 16/32 key bytes";

        #region Initialization

        void initializeKeyValueControl()
        {
            this.keyBox = new ucKeyValuesInputControl(32, _keyName);
            
            Point tempPoint = new Point(this.textBoxUserNumber.Location.X, this.textBoxUserNumber.Location.Y);

            tempPoint.X += 5 + this.textBoxUserNumber.Width;
            tempPoint.Y -= 5;

            this.keyBox.Location = tempPoint;

            this.Controls.Add(this.keyBox);
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
            this.sendUserName();
        }

        private void sendUserName()
        {
            if (!this.validUserName())
            {
                this.onError(new Exception("Bad User Name"));
            }

            SecureSendEventArgs sSEA = new SecureSendEventArgs(_userNameLimit + 3);

            sSEA.Data = getUserNamePacket();
        }

        private byte[] getUserNamePacket()
        {
            byte[] returnArray = new byte[this.textBoxUserName.Text.Length + 3];

            returnArray[0] = 0x00; //TODO Command Letter
            System.Buffer.BlockCopy(this.textBoxUserName.Text.ToCharArray(), 0, returnArray, 2, this.textBoxUserName.Text.Length);
            returnArray[returnArray.Length - 1] = 0x0D;

            return returnArray;
        }

        #endregion
    }
}
