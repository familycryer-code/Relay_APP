using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TransmitterLibrary
{
  public partial class About : Form
  {
    public About(string AppName)
    {
      InitializeComponent();
      // Text
			this.Text = "About " + AppName; 
			// lblProgram
			this.lblProgram.Text = AppName;
    }

    private void About_Load(object sender, EventArgs e)
    {
			this.FormBorderStyle = FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.StartPosition = FormStartPosition.CenterScreen;

			// Version
      this.lblVersion.Text = TransmitterLib.GetDONSoftVersion();
			// Law
			this.lblLaw.Text = "Warning: The computer program is protected by copyright law. Unauthorized" + "\n";
			this.lblLaw.Text += "reproduction or distribution of this program, or any portion of it, may result" + "\n";
			this.lblLaw.Text += "in severe civil and criminal penalties, and will be prosecuted to the maximum" + "\n";
			this.lblLaw.Text += "extent possible under law.";
			// Copy right
			this.lblCopyright.Text += "Copyright © Digitalgrid Inc. 2002~2009.";

		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
  }
}