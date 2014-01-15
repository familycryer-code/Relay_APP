using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RelayDNPSecurity
{
    public class SecureSendEventArgs : EventArgs
    {
        public SecureSendEventArgs(int dataLength)
        {
            this.Data = new byte[dataLength];
        }

        public byte[] Data;
    }
}
