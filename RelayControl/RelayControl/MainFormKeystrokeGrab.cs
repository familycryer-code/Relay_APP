using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RelayControlLibrary;
using System.Threading;

namespace RelayControl
{
    public partial class MainControl : Form
    {
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.Control)
            {
                if (e.KeyCode == Keys.S)
                {
                    this.ForceUpdateSerialNumber();
                }
                else if (e.KeyCode == Keys.D)
                {
                    this.EnableDNP();
                }
                else if (e.KeyCode == Keys.T)
                {
                    this.ChangeRelayType();
                }
                else if (e.KeyCode == Keys.R)
                {
                    this.ResetTransmitterSettings();
                }
                else if (e.KeyCode == Keys.E)
                {
                    this.EnableTransmitterSettings();
                }
                else if (e.KeyCode == Keys.B)
                {
                    CheckBootCode();
                }
            }
        }

        private void ChangeRelayType()
        {
            CustomYesNoDialog cYN = new CustomYesNoDialog("GE or Westinghouse", "Is this a GE or Westinghouse style relay?", "GE", "WH");
            DialogResult dR = cYN.ShowDialog();

            if (dR == System.Windows.Forms.DialogResult.Yes)
                this.ucTransmitter1.GERelay = true;
            else if (dR == System.Windows.Forms.DialogResult.No)
                this.ucTransmitter1.GERelay = false;
            else
                return;

            this.ucTransmitter1.SendTransmitterSettings();
        }

        private void EnableDNP()
        {
            DialogResult dR = MessageBox.Show("Do you want to enable DNP?\r\n(No to Disable)", "DNP Enable", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

            this.setDNPShortucutCheck();

            if (dR == System.Windows.Forms.DialogResult.Yes)
                this.ucTransmitter1.DNPEnabled = true;
            else if (dR == System.Windows.Forms.DialogResult.No)
                this.ucTransmitter1.DNPEnabled = false;
            else
                return;

            this.ucTransmitter1.SendTransmitterSettings();
            Thread.Sleep(100);
            this.requestTransmitterSettings(); // read back actual relay state

            if (applyTX.applyTxSettings && applyDNP.applyDNPSettings)//if (uplinkC.uplinkCount == 2)
                this.ucTransmitter1.DNPCommLabelStatus = true;
            else
                this.ucTransmitter1.DNPCommLabelStatus = false;

        }

        private formForceUpdateSerialNumber tempForm;

        private void ForceUpdateSerialNumber()
        {
            tempForm = new formForceUpdateSerialNumber();

            tempForm.UpdateSerialNumber += updateSerialNumber;
            tempForm.ShowDialog();
        }

        private void updateSerialNumber(object e, UpdateSerialNumberEventArgs uSNEA)
        {
            this.ucTransmitter1.SerialNumber = (int)uSNEA.SerialNumber;
            this.ucTransmitter1.SendTransmitterSettings();
            this.ucTransmitter1.buttonSendAllDNPSettings_Click(this, new EventArgs());
            if (tempForm != null)
            {
                tempForm.UpdateSerialNumber -= updateSerialNumber;
                this.tempForm.Close();
                this.tempForm = null;
            }

        }

        private void ResetTransmitterSettings()
        {
            DialogResult dR = MessageBox.Show("Do you want to reset Transmitter Settings?\r\n", "Reset Transmitter Settings", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

            if (dR == System.Windows.Forms.DialogResult.Yes)
            {
                this.ucTransmitter1.SetDefaults();
                this.ucTransmitter1.SendTransmitterSettings();
            }
        }

        private void EnableTransmitterSettings()
        {
            DialogResult dR = MessageBox.Show("Do you want to enable the Transmitter?\r\n(No to Disable)", "Transmitter Enable", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

            if (dR == System.Windows.Forms.DialogResult.Yes)
                this.ucTransmitter1.EnableTransmitter();
            else if (dR == System.Windows.Forms.DialogResult.No)
                this.ucTransmitter1.DisableTransmitter();
            else
                return;

            this.ucTransmitter1.SendTransmitterSettings();
        }
        private void CheckBootCode()
        {
            if (!ucRelayProgramming1.ProgramBootCodeInProgress)
            {
                this.quietMode = true;
                this.pQMonitoringEnabled = false;
                this.allEnabled = false;
                this.pauseMonitoring = true;
                this.ucRelayProgramming1.startReloadingJustBoot();
                this.ProgramState = ProgramStates.Running;
            }

        }
    }
}
