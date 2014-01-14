using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RelayDNPSecurity
{
    public class SecureSendEventArgs : EventArgs
    {
        public SecureSendEventArgs(uint dataLength)
        {
            this.Data = new byte[dataLength];
        }

        byte[] Data;
    }
}
