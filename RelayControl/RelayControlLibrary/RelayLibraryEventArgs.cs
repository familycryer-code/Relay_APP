using System;
using System.Collections.Generic;
using System.Text;

namespace RelayControlLibrary
{
    public class SendEventArgs : EventArgs
    {
        public SendEventArgs(int arraySize)
        {
            this.SendPacket = new byte[arraySize];
        }

        public byte[] SendPacket;

    }

    public class TripModeChangeEventArgs : EventArgs
    {
        public TripModeChangeEventArgs()
        {
        }

        public TripModes TripMode;
        public byte TimeDelay;
        public int SpecialTimeDelay;
    }
}
