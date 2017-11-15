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
    public partial class ucRemoteCommandBlock : ucSuperClass
    {
        public ucRemoteCommandBlock()
        {
            InitializeComponent();
            tsCommandBlock.CheckedColor = Color.Red;
            toolTip.SetToolTip(tsCommandBlock, "Enables or disables PLC Communications");
        }

        private static string _commandsAllowedString = "Control Allowed";
        private static string _commandsDisallowedString = "Control Inhibited";
        private ToolTip toolTip = new ToolTip();

        public bool CommandsBlocked
        {
            get { return commandsBlocked; }
            set
            {
                setBlockedState(value);
            }
        }

        private bool commandsBlocked = false;

        private void setBlockedState(bool blocked)
        {
            tsCommandBlock.Checked = commandsBlocked = blocked;

            if (blocked)
            {
                labelRemoteCommandState.Text = _commandsDisallowedString;
            }
            else
            {
                labelRemoteCommandState.Text = _commandsAllowedString;
            }
        }

        private void buttonSendCommand_Click(object sender, EventArgs e)
        {
            sendCommand();
        }

        private void sendCommand()
        {
            if (commandsBlocked)
                sendUnblockCommand();
            else
                sendBlockCommand();
        }

        private void sendUnblockCommand()
        {
            SendEventArgs sendCommand = new SendEventArgs(8);

            sendCommand.SendPacket[0] = (byte)'M'; // Mode
            sendCommand.SendPacket[1] = (byte)'B'; // Blocked sate
            sendCommand.SendPacket[2] = (byte)'S'; // Remote Commands unblock
            sendCommand.SendPacket[3] = Constants.DummyData;
            sendCommand.SendPacket[4] = Constants.DummyData;
            sendCommand.SendPacket[5] = Constants.DummyData;
            sendCommand.SendPacket[6] = Constants.DummyData;
            sendCommand.SendPacket[7] = 0x0D;

            sendCommand.WithAck = true;
            OnSend(this, sendCommand);
        }

        private void sendBlockCommand()
        {
            SendEventArgs sendCommand = new SendEventArgs(8);

            sendCommand.SendPacket[0] = (byte)'M'; // Mode
            sendCommand.SendPacket[1] = (byte)'B'; // Blocked sate
            sendCommand.SendPacket[2] = (byte)'R'; // Remote Commands unblock
            sendCommand.SendPacket[3] = Constants.DummyData;
            sendCommand.SendPacket[4] = Constants.DummyData;
            sendCommand.SendPacket[5] = Constants.DummyData;
            sendCommand.SendPacket[6] = Constants.DummyData;
            sendCommand.SendPacket[7] = 0x0D;

            sendCommand.WithAck = true;
            OnSend(this, sendCommand);
        }

        private void ucToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            commandsBlocked = !tsCommandBlock.Checked;
            if(tsCommandBlock.Focused)
                sendCommand();
        }
    }
}
