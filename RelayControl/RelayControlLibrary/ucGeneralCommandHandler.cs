using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucGeneralCommandHandler : UserControl
    {
        public ucGeneralCommandHandler()
        {
            InitializeComponent();
            this.initializeOutgoingCommandList();
            this.initializeIncomingCommandList();
            this.testInterruptValues = new ucGeneralCommandTestValues(16, new Size(this.Width - 10, this.Height - this.labelIncomingCommandName.Location.Y - this.labelIncomingCommandName.Height - 10));
        }

        public delegate void SendDelegate(object o, SendEventArgs sEA);
        public event SendDelegate Send;

        private List<OutgoingCommand> OutgoingCommands = new List<OutgoingCommand>();
        private List<IncommingCommand> IncomingCommands = new List<IncommingCommand>();

        private ucGeneralCommandTestValues testInterruptValues;

        private void initializeOutgoingCommandList()
        {
            this.OutgoingCommands.Add(new OutgoingCommand(0, "Handshake", new byte[0]));
            this.OutgoingCommands.Add(new OutgoingCommand(1, "Reset Calibration Constants", new byte[0]));
            this.OutgoingCommands.Add(new OutgoingCommand(2, "I2C Test Command 1", new byte[0]));
            this.OutgoingCommands.Add(new OutgoingCommand(3, "I2C Test Command 2", new byte[0]));
            this.OutgoingCommands.Add(new OutgoingCommand(4, "Charge Clock Battery", new byte[0]));
            this.OutgoingCommands.Add(new OutgoingCommand(5, "Don't Charge Battery", new byte[0]));
            this.OutgoingCommands.Add(new OutgoingCommand(6, "Get Clock Data", new byte[0]));
            this.OutgoingCommands.Add(new OutgoingCommand(7, "Set Backup Clock", new byte[4], new OutgoingCommandFunctionDelegate(this.GetTime)));
            this.OutgoingCommands.Add(new OutgoingCommand(8, "Get Math Time", new byte[0]));

            foreach (OutgoingCommand oC in this.OutgoingCommands)
            {
                this.comboBoxOutgoingCommands.Items.Add(oC.Name);
            }

            this.comboBoxOutgoingCommands.SelectedIndex = 0;
        }

        private void initializeIncomingCommandList()
        {
            this.IncomingCommands.Add(new IncommingCommand(0, "Hand Shake", new IncommingCommandFunctionDelegate(this.incommingCommandError)));
            this.IncomingCommands.Add(new IncommingCommand(1, "Request Clock Time", new IncommingCommandFunctionDelegate(this.incommingCommandClockTime)));
            this.IncomingCommands.Add(new IncommingCommand(2, "Test Interrupt Times", new IncommingCommandFunctionDelegate(this.handleTestInterruptTimes)));
            this.IncomingCommands.Add(new IncommingCommand(3, "Math Time", new IncommingCommandFunctionDelegate(this.incomingCommandSingleData)));
        }

        private void incommingCommandError(byte[] b)
        {
        }

        private void incommingCommandClockTime(byte[] b)
        {
            UInt32 temp = 0;

            for (int i = 2; i < 6; i++)
            {
                temp <<= 8;
                temp += b[i];
            }

            EventBaseTime eBT = new EventBaseTime();

            eBT.BinaryTime = temp;
            this.textBoxReturnValue.Text = eBT.SystemTime.ToString();
        }

        private void handleTestInterruptTimes(byte[] b)
        {

            if (!this.groupBoxGeneralCommand.Controls.Contains(this.testInterruptValues))
            {
                this.testInterruptValues.Location = new Point(this.labelIncomingCommandName.Location.X, this.labelIncomingCommandName.Location.Y + this.labelIncomingCommandName.Height + 5);
                this.testInterruptValues.Size = new Size(this.Width - 10, this.Height - this.labelIncomingCommandName.Location.Y - this.labelIncomingCommandName.Height - 10);
                this.groupBoxGeneralCommand.Controls.Add(this.testInterruptValues);
            }

            byte[] c = new byte[b.Length-2];

            System.Buffer.BlockCopy(b, 2, c, 0, c.Length);

            this.testInterruptValues.SetValues(c);
 
        }

        private void incomingCommandSingleData(byte[] b)
        {
            uint temp;
            temp = b[2];
            temp <<= 8;
            temp += b[3];

            this.labelIncomingCommandName.Text = "Math Time";
            this.textBoxReturnValue.Text = temp.ToString();
        }

        private byte[] GetTime()
        {
            byte[] returnByte = new byte[4];
            EventBaseTime eBT = new EventBaseTime();
            UInt32 time;

            eBT.SystemTime = DateTime.Now;
            time = eBT.BinaryTime;

            for (int i = 0; i < 4; ++i)
            {
                returnByte[i] = (byte)time;
                time >>= 8;
            }
            return returnByte;
        }

        public void HandleCommand(byte[] packet)
        {
            IncommingCommand workingCommand = null;
            int tempCommand = packet[0];
            tempCommand <<= 8;
            tempCommand += packet[1];

            foreach (IncommingCommand iC in this.IncomingCommands)
            {
                if (iC.Key == tempCommand)
                {
                    workingCommand = iC;
                    break;
                }
            }

            if (workingCommand != null)
            {
                this.labelIncomingCommandName.Text = workingCommand.Name;
                workingCommand.PassData(packet);
            }
        }

        private void onSend(object sender, SendEventArgs sEA)
        {
            if(Send != null)
                this.Send(sender, sEA);
        }

        private void buttonSendOnce_Click(object sender, EventArgs e)
        {
            this.SendCommand();
        }

        private Timer repeatTimer = new Timer();

        private void buttonRepeatedSend_Click(object sender, EventArgs e)
        {
            if (!this.repeatTimer.Enabled)
            {
                
                this.repeatTimer.Tick += repeatTimer_Tick;
                this.repeatTimer.Interval = 100;
                this.repeatTimer.Start();
            }
            else
            {
                this.repeatTimer.Stop();
                this.repeatTimer.Tick -= repeatTimer_Tick;
            }
        }

        void repeatTimer_Tick(object sender, EventArgs e)
        {
            this.SendCommand();
        }

        private void SendCommand()
        {
            SendEventArgs sEA = new SendEventArgs(82);
            OutgoingCommand workingCommand = null;

            foreach (OutgoingCommand oC in this.OutgoingCommands)
            {
                if ((string)this.comboBoxOutgoingCommands.SelectedItem == oC.Name)
                {
                    workingCommand = oC;
                    break;
                }
            }

            try
            {
                if (workingCommand != null)
                {
                    if (workingCommand.GetData != null)
                    {
                        workingCommand.AdditionalData = workingCommand.GetData();
                    }

                    sEA.SendPacket[0] = 0x17;
                    sEA.SendPacket[1] = 0;
                    sEA.SendPacket[2] = Convert.ToByte(workingCommand.Key);
                    for (int i = 0; i < workingCommand.AdditionalData.Length; ++i)
                    {
                        sEA.SendPacket[3 + i] = workingCommand.AdditionalData[workingCommand.AdditionalData.Length-i-1];
                    }
                    sEA.SendPacket[81] = 0x0D;

                    if (this.Send != null)
                        this.Send(this, sEA);
                }
            }
            catch
            {
            }
        }

        #region Drawing

        private void labelIncomingCommandName_Resize(object sender, EventArgs e)
        {
            this.resizeIncomingCommandTextBox();
        }

        private void resizeIncomingCommandTextBox()
        {
            Point tempPoint = this.labelIncomingCommandName.Location;

            tempPoint.X += this.labelIncomingCommandName.Width + 5;

            this.textBoxReturnValue.Location = tempPoint;

            int newWidth = this.groupBoxGeneralCommand.Width - this.labelIncomingCommandName.Width - 5 - (this.textBoxReturnValue.Location.X - this.groupBoxGeneralCommand.Location.X);

            this.textBoxReturnValue.Width = newWidth;
        }
        #endregion


    }

    public delegate void IncommingCommandFunctionDelegate(byte[] b);
    public delegate byte[] OutgoingCommandFunctionDelegate();

    public class IncommingCommand
    {
        public IncommingCommand(int key, string name, IncommingCommandFunctionDelegate cFD)
        {
            this.Key = key;
            this.Name = name;
            this.PassData = cFD;
        }

        public int Key = 0;
        public string Name = "";
        public IncommingCommandFunctionDelegate PassData;
    }

    public class OutgoingCommand
    {
        public OutgoingCommand(int key, string name, byte[] additionalData)
        {
            this.Key = key;
            this.Name = name;
            this.AdditionalData = additionalData;
            this.GetData = null;
        }

        public OutgoingCommand(int key, string name, byte[] additionalData, OutgoingCommandFunctionDelegate oCFD)
        {
            this.Key = key;
            this.Name = name;
            this.AdditionalData = additionalData;
            this.GetData = oCFD;
        }

        public int Key = 0;
        public string Name = "";
        public byte[] AdditionalData;
        public OutgoingCommandFunctionDelegate GetData;
    }

}
