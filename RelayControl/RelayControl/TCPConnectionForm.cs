using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace RelayControl
{
    public partial class TCPConnectionForm : Form
    {
        public TCPConnectionForm()
        {
            InitializeComponent();
        }

        public TCPConnectionForm(IPAddress iPAddress, int Port)
        {
            InitializeComponent();
            maskedTextBoxIPAddress.Text = iPAddress.ToString();
            numericUpDownPort.Value = Port;
        }

        public IPAddress IPAddress;
        public int Port;

        private void button1_Click(object sender, EventArgs e)
        {
            if (IPAddress.TryParse(maskedTextBoxIPAddress.Text, out IPAddress))
            {
                Port = (int)numericUpDownPort.Value;
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid IP Address");
            }
        }
    }
}
