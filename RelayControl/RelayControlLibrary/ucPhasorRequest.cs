using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharedResources;

namespace RelayControlLibrary
{
    public partial class ucPhasorRequest : ucSuperClass
    {
        public ucPhasorRequest()
        {
            InitializeComponent();
            populateComboBoxes();
        }

        public void SetPhasorValues(byte type, byte phase, long real, long imaginary, long rms)
        {
            textBoxIncType.Text = Convert.ToChar(type).ToString();
            textBoxIncPhase.Text = Convert.ToChar(phase).ToString();

            if (textBoxIncType.Text == "I")
            {
                textBoxIncReal.Text = ((decimal)real * Constants.SixteenFracBits).ToString("0.00");
                textBoxIncImaginary.Text = ((decimal)imaginary * Constants.SixteenFracBits).ToString("0.00");
                textBoxIncRMS.Text = ((decimal)rms * Constants.SixteenFracBits).ToString("0.00");
            }
            else
            {
                textBoxIncReal.Text = ((decimal)real * Constants.TwelveFracBits).ToString("0.00");
                textBoxIncImaginary.Text = ((decimal)imaginary * Constants.TwelveFracBits).ToString("0.00");
                textBoxIncRMS.Text = ((decimal)rms * Constants.TwelveFracBits).ToString("0.00");
            }
        }

        private void populateComboBoxes()
        {
            comboBoxPhase.DataSource = Enum.GetNames(typeof(Phases));
            comboBoxType.DataSource = Enum.GetNames(typeof(PhaseTypes));
        }
        private void buttonRequest_Click(object sender, EventArgs e)
        {
            var phase = (Phases)Enum.Parse(typeof(Phases), comboBoxPhase.SelectedIndex.ToString());
            var type = (PhaseTypes)Enum.Parse(typeof(PhaseTypes), comboBoxType.SelectedIndex.ToString());

            requestSinglePhasor(phase, type);

        }
        private void requestSinglePhasor(Phases phase, PhaseTypes type)
        {
            SendEventArgs sEA = new SendEventArgs(8);

            sEA.SendPacket[0] = 0x03;
            sEA.SendPacket[1] = RelayModeFunctions.ByteRepresentationOf(phase);
            sEA.SendPacket[2] = RelayModeFunctions.ByteRepresentationOf(type);
            sEA.SendPacket[7] = 0x0D;

            OnSend(this, sEA);
        }
    }
}
