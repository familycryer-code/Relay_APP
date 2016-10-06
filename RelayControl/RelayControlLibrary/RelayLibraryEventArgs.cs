using System;
using System.Collections.Generic;
using System.Text;

namespace RelayControlLibrary
{
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
