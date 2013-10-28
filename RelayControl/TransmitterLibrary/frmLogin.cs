using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TransmitterLibrary
{
  public partial class frmLogin : Form
  {
    private string myPassword;

    public frmLogin() // Default constructor using default password "ncs007"
    {
      InitializeComponent();
      this.myPassword = "ncs007";
    }
    public frmLogin(string Password) // Customized Login using different password.
    {
      InitializeComponent();
      this.myPassword = Password;
    }

    private void btnApply_Click(object sender, System.EventArgs e)
    {
      if (this.txtPassword.Text == this.myPassword)
      {
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
      else
      {
        MessageBox.Show("Sorry, that's not a valid password!" + "\n" +
          "Please try again.", "Invalid Password",
          MessageBoxButtons.OK, MessageBoxIcon.Error);
        this.txtPassword.Clear();
        this.txtPassword.Focus();
      }

    }

    private void btnCancel_Click(object sender, System.EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void frmLogin_Load(object sender, System.EventArgs e)
    {
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.AcceptButton = this.btnApply;
      this.CancelButton = this.btnCancel;

      //
      // this.txtPassword
      //
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.AcceptsReturn = true;
    }

  }
}