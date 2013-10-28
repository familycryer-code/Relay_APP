using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class CustomYesNoDialog : Form
    {
        public CustomYesNoDialog(string title, string question, string buttonYes, string buttonNo)
        {
            InitializeComponent();
            this.Text = title;
            this.labelRelayType.Text = question;
            this.buttonYes.Text = buttonYes;
            this.buttonNo.Text = buttonNo;
            this.buttonYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
        }

    }

    public class GEWHFormEventArgs : EventArgs
    {
        public GEWHFormEventArgs(bool gERelay)
        {
            this.GERelay = gERelay;
        }

        public bool GERelay = false;
    }
}
