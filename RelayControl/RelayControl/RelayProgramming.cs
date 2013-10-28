using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace RelayControl
{
    partial class MainControl
    {
        private void buttonRPUploadMaster_Click(object sender, EventArgs e)
        {
            SaveFileDialog masterUploadFile = new SaveFileDialog();

            masterUploadFile.Title = "Master File";
            masterUploadFile.InitialDirectory = @"C:\DGI Systems\Relay\Saved Data";
        }

        private void buttonRPUploadRelay_Click(object sender, EventArgs e)
        {

        }

        private void buttonRPUploadFPGA_Click(object sender, EventArgs e)
        {

        }
    }
}
