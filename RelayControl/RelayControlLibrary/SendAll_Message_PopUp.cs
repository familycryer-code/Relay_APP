using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class SendAll_Message_PopUp : Form
    {
        TextBox txtBox;
        public SendAll_Message_PopUp()
        {
            InitializeComponent();
        }

        private void SendAll_Message_PopUp_Load(object sender, EventArgs e)
        {
            txtBox = new TextBox();
            txtBox.Location = new Point(20, 10);
            txtBox.Size = new System.Drawing.Size(790, 60);
            txtBox.Enabled = false;
            txtBox.Visible = true;
            txtBox.BringToFront();
            Controls.Add(txtBox);
            txtBox.Text = "Please have patience. The relay is updating its critical parameters !";
        }
    }
}
