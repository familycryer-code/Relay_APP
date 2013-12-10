using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class formForceUpdateSerialNumber : Form
    {
        public formForceUpdateSerialNumber()
        {
            InitializeComponent();
        }

        public delegate void UpdateSerialNumberHandler(object o, UpdateSerialNumberEventArgs uSNEA);

        public event UpdateSerialNumberHandler UpdateSerialNumber;

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            UpdateSerialNumberEventArgs uSNEA;
            uint temp;

            try
            {
                temp = Convert.ToUInt16(this.textBoxSerialNumber.Text);
            }
            catch
            {
                MessageBox.Show("Bad Serial Number");
                return;
            }

            try
            {
                if (UpdateSerialNumber != null)
                {
                    uSNEA = new UpdateSerialNumberEventArgs(temp);
                    UpdateSerialNumber(this, uSNEA);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error Setting Serial Number");
            }
        }
    }

    public class UpdateSerialNumberEventArgs : EventArgs
    {
        public UpdateSerialNumberEventArgs(uint serialNumber)
        {
            this.SerialNumber = serialNumber;
        }

        public uint SerialNumber;
    }
}
