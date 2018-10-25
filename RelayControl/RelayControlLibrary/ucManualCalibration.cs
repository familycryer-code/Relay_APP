using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SharedResources;

namespace RelayControlLibrary
{
    public partial class ucManualCalibration : UserControl
    {
        public ucManualCalibration()
        {
            try
            {
                InitializeComponent();
                myInit();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Calibration Control Initialization", ex);
            }
        }

        private Phases updatePhase;
        private PhaseTypes updatePhaseType;

        public delegate void SendHandler(object sender, SendEventArgs sEA);
        public event SendHandler Send;
        private SendEventArgs mySEA = new SendEventArgs(5);

        private void OnSend(object sender, SendEventArgs sEA)
        {
            if (Send != null)
            {
                Send(this, sEA);
            }
        }

        private void myInit()
        {
            try
            {
                this.comboBoxType.DataSource = Enum.GetNames(typeof(PhaseTypes));
                this.comboBoxPhase.DataSource = Enum.GetNames(typeof(Phases));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Getting Names", ex);
            }

        }

        private void buttonPlus_Click(object sender, EventArgs e)
        {
            mySEA.SendPacket = RelayModeFunctions.BytePacketFor(this.updatePhase, this.updatePhaseType, Convert.ToInt16(this.numericUpDownCalAmount.Value));

            OnSend(this, mySEA);
        }



        private void comboBoxPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            updatePhase = RelayModeFunctions.PhaseFrom(cb.SelectedItem.ToString());
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            updatePhaseType = RelayModeFunctions.PhaseTypesFrom(cb.SelectedValue.ToString());
        }

        private void buttonMinus_Click(object sender, EventArgs e)
        {
            mySEA.SendPacket = RelayModeFunctions.BytePacketFor(this.updatePhase, this.updatePhaseType, Convert.ToInt16(-this.numericUpDownCalAmount.Value));
            OnSend(this, mySEA);
        }
    }
}
