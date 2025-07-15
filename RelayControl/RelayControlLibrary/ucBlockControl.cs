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
    public partial class ucBlockControl : ucSuperClass
    {
        public ucBlockControl()
        {
            InitializeComponent();
        }

        public bool RelayBlocked
        {
            get { return relayBlocked; }
            set
            {
                setBlockedState(value);
            }
        }

        private bool relayBlocked;
        private ToolTip toolTip = new ToolTip();

        private void setBlockedState(bool blocked)
        {
            tsBlockOpen.Checked = relayBlocked = blocked;
            if (blocked)
            {
                toolTip.SetToolTip(tsBlockOpen, "Enable automatic Reclose Function in Relay");
                labelBlockedState.Text = "BLOCKED OPEN";
                labelBlockedState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labelBlockedState.BackColor = Color.Yellow;
            }
            else
            {
                toolTip.SetToolTip(tsBlockOpen, "Inhibit automatic Reclose Function in Relay");
                labelBlockedState.Text = "Unblocked";
                labelBlockedState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labelBlockedState.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        public void SendBlockState(bool blocked)
        {
            if (blocked)
                sendBlockCommand();
            else
                sendUnblockCommand();
        }

        private void sendBlockCommand()
        {
            SendEventArgs sendCommand = new SendEventArgs(8);

            sendCommand.SendPacket[0] = (byte)'M'; // Mode
            sendCommand.SendPacket[1] = (byte)'B'; // Blocked sate
            sendCommand.SendPacket[2] = (byte)'B'; // Block Relay
            sendCommand.SendPacket[3] = Constants.DummyData;
            sendCommand.SendPacket[4] = Constants.DummyData;
            sendCommand.SendPacket[5] = Constants.DummyData;
            sendCommand.SendPacket[6] = Constants.DummyData;
            sendCommand.SendPacket[7] = 0x0D;

            sendCommand.WithAck = true;
            OnSend(this, sendCommand);
        }

        private void sendUnblockCommand()
        {
            SendEventArgs sendCommand = new SendEventArgs(8);

            sendCommand.SendPacket[0] = (byte)'M'; // Mode
            sendCommand.SendPacket[1] = (byte)'B'; // Blocked sate
            sendCommand.SendPacket[2] = (byte)'U'; // Unblock Relay
            sendCommand.SendPacket[3] = Constants.DummyData;
            sendCommand.SendPacket[4] = Constants.DummyData;
            sendCommand.SendPacket[5] = Constants.DummyData;
            sendCommand.SendPacket[6] = Constants.DummyData;
            sendCommand.SendPacket[7] = 0x0D;

            sendCommand.WithAck = true;
            OnSend(this, sendCommand);
        }

        private void tsBlockOpen_CheckedChanged(object sender, EventArgs e)
        {
            if (tsBlockOpen.Focused)
                SendBlockState(tsBlockOpen.Checked);
        }
    }
}
