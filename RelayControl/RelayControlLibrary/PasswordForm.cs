using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class PasswordForm : Form
    {
        public PasswordForm()
        {
            InitializeComponent();
        }

        const string _PasswordValue = "Stewie007*";

        private void buttonOkay_Click(object sender, EventArgs e)
        {
            if(this.textBoxPassword.Text == _PasswordValue)
                this.sendValidation(true);
            else
                this.sendValidation(false);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.sendValidation(false);
        }

        public delegate void ReturnValueHandler(bool b);
        public event ReturnValueHandler PasswordValidated;

        private void sendValidation(bool b)
        {
            if(this.PasswordValidated != null)
            {
                this.PasswordValidated(b);
            }
            else
                throw new Exception("No Handler in Transmitter Settings for Password");
        }
    }
}