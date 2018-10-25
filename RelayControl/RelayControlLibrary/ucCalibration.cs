using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using SharedResources;

namespace RelayControlLibrary
{
    public partial class ucCalibration : UserControl
    {
        public ucCalibration()
        {
            InitializeComponent();
        }

        public CalibrationConstant[] CalibrationConstants = new CalibrationConstant[15];
        private Customers customer = Customers.DigitalGridDNP;
        //private bool gEVersion = false;

        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                this.customer = value;
                //if (this.customer == Customers.NonConEdGE || this.customer == Customers.Memphis)
                //    this.gEVersion = true;
                //else
                //    this.gEVersion = false;
            }
        }


        private void buttonRestConstants_Click(object sender, EventArgs e)
        {
            DialogResult dr = new YesNoMessageBoxResized("Do You Really Want To Reset Calibration Constants?", "Reset Constants?").ShowDialog();

            if (dr == DialogResult.Yes)
            {
                this.resetConstants();
            }
        }

        private void resetConstants()
        {
            SendEventArgs sEA = new SendEventArgs(82);

            sEA.SendPacket[0] = 0x17; //general command
            sEA.SendPacket[1] = 0;      //LSB and then MSB, so command is 1 or 0x0001
            sEA.SendPacket[2] = 1;
            sEA.SendPacket[81] = 0x0D;

            this.Send(this, sEA);
        }

        public void PopulateTextBox()
        {
            this.textBoxDisplayCalConstants.Text = "";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[0].RealValue.ToString() + "\r\n";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[1].RealValue.ToString() + "\r\n";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[2].RealValue.ToString() + "\r\n";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[3].RealValue.ToString() + "\r\n";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[4].RealValue.ToString() + "\r\n";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[5].RealValue.ToString() + "\r\n";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[9].RealValue.ToString() + "\r\n";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[10].RealValue.ToString() + "\r\n";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[11].RealValue.ToString() + "\r\n";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[12].RealValue.ToString() + "\r\n";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[13].RealValue.ToString() + "\r\n";
            this.textBoxDisplayCalConstants.Text += this.CalibrationConstants[14].RealValue.ToString();

        }

        private bool resetSingleConstant(Phases p, PhaseTypes pT, Int32 currentValue, Int32 defaultValue)
        {
            SendEventArgs sEA = new SendEventArgs(5);

            Int32 difference = defaultValue - currentValue;

            if (difference == 0)
                return true;
            if (difference > Int16.MaxValue)
                sEA.SendPacket = RelayModeFunctions.BytePacketFor(p, pT, Int16.MaxValue);
            else if (difference < Int16.MinValue)
                sEA.SendPacket = RelayModeFunctions.BytePacketFor(p, pT, Int16.MinValue);
            else
                sEA.SendPacket = RelayModeFunctions.BytePacketFor(p, pT, (Int16)difference);

            this.OnSend(this, sEA);

            return false;
        }

        public delegate void SendHandler(object sender, SendEventArgs sEA);
        public event SendHandler Send;

        private void OnSend(object sender, SendEventArgs sEA)
        {
            if (Send != null)
            {
                Send(this, sEA);
            }
            else
            {
                this.errorHandler(new Exception("OnSend Not Set For Calibration Module"), "In OnSend");
            }
        }

        public delegate void ExceptionHandler(object o, ExceptionEventArgs eEA);

        public event ExceptionHandler CalibrationException;

        private void errorHandler(Exception ex, string title)
        {
            if (CalibrationException != null)
            {
                CalibrationException(this, new ExceptionEventArgs(ex, title));
            }
            else
            {
                throw new Exception("No Exception Handler For Trip Control");
            }
        }



        private void requestCalibrationConstants()
        {
            SendEventArgs sEA = new SendEventArgs(2);

            sEA.SendPacket[0] = 0x6C;
            sEA.SendPacket[1] = 0x0D;

            this.Send(this, sEA);
        }

        private void buttonReqCalConstants_Click(object sender, EventArgs e)
        {
            this.requestCalibrationConstants();
        }

        private void buttonCalHigh_Click(object sender, EventArgs e)
        {
            SendEventArgs sEA = new SendEventArgs(3);
            //DialogResult dR = this.messageHandler("Calibration", "This may take a few moments to complete.\r\nCalibrate Unit?", MessageBoxButtons.YesNo);

            //if (dR == DialogResult.Yes)
            {
                sEA.SendPacket[0] = (byte)'m';
                sEA.SendPacket[1] = 0x55;
                sEA.SendPacket[2] = 0x0D;

                this.Send(this, sEA);
                //this.downloadingDialogCountDown("Calibrating ", "Calibration", 60);
            }
        }

        private void buttonStartCal_Click(object sender, EventArgs e)
        {
            SendEventArgs sEA = new SendEventArgs(3);

            //DialogResult dR = this.messageHandler("Calibration", "This may take a few moments to complete.\r\nCalibrate Unit?", MessageBoxButtons.YesNo);

            //if (dR == DialogResult.Yes)
            {
                sEA.SendPacket[0] = (byte)'c';
                sEA.SendPacket[1] = 0x55;
                sEA.SendPacket[2] = 0x0D;

                this.Send(this, sEA);

                //this.downloadingDialogCountDown("Calibrating ", "Calibration", 60);
            }
        }

        private void sendSaveCalibration()
        {
            byte[] packet = new byte[3];
            SendEventArgs sEA = new SendEventArgs(3);

            sEA.SendPacket[0] = (byte)'a';
            sEA.SendPacket[1] = 0x55;
            sEA.SendPacket[2] = 0x0D;

            this.Send(this, sEA);
        }

        private void buttonSaveCalibration_Click(object sender, EventArgs e)
        {
            this.sendSaveCalibration();
        }

    }
}
