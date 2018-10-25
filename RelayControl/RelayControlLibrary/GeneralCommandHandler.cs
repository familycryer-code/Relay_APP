using System;
using System.Collections.Generic;
using System.Text;

namespace RelayControlLibrary
{
    public static class GeneralCommandHandler
    {
        public static void HandleCommand(byte[] packet)
        {
            uint tempCommand = packet[0];
            tempCommand <<= 8;
            tempCommand += packet[1];

            switch (tempCommand)
            {
                // case 0
                case 0:
                    break;
                case 1: //I2C Variable

                    break;
            }
        }
    }
}
