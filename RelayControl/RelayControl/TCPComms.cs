using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SharedResources;
using System.Threading;

namespace RelayControl
{
    class TCPComms
    {
        public TCPComms(IPAddress iPAddress, int port)
        {
            IPAddress = iPAddress;
            Port = port;

            connect();
        }


        public delegate void DataReceivedHandler(object o, TCPCommsEventArgs tCPCEA);
        public event DataReceivedHandler DataReceived;

        public delegate void ExceptionHandler(object o, ExceptionEventArgs eEA);
        public event ExceptionHandler TCPCommsException;

        public bool IsConnected { get => client.Connected; }
        private TcpClient client;
        private NetworkStream stream;
        public int Port;
        public IPAddress IPAddress;
        private byte[] readBuffer = new byte[1000];


        public void SendPacket(byte[] bytePacket)
        {
            if (connect())
            {
                stream.Write(bytePacket, 0, bytePacket.Length);
            }
        }

        private bool connect()
        {
            if (client != null && client.Connected)
                return true;

            client = new TcpClient();
            client.Connect(IPAddress, Port);

            if (!client.Connected)
            {
                // Failed to connect
                exceptionHandler(new ExceptionEventArgs(new Exception(String.Format("Failed to Connect to {0}:{1}", IPAddress, Port)), "TCP/IP Connect fail"));
                return false;
            }

            return true;
        }
        private void exceptionHandler(ExceptionEventArgs eEA)
        {
            if (TCPCommsException != null)
                TCPCommsException(this, eEA);
            else
                Console.Write("No Subscription for TCPCommsException");
        }

        private void dataReceveid(TCPCommsEventArgs tCPCEA)
        {
            if (DataReceived != null)
                DataReceived(this, tCPCEA);
            else
                Console.Write("No Subscription for TCP DataReceived event");
        }

        private void readPort()
        {
            while (client.Connected)
            {
                using (NetworkStream stream = client.GetStream())
                {
                    Int32 readBytes = stream.Read(readBuffer, 0, readBuffer.Length);
                    if (readBytes > 0)
                    {
                        byte[] receivedBytes = new byte[readBytes];
                        Array.Copy(readBuffer, receivedBytes, receivedBytes.Length);
                        dataReceveid(new TCPCommsEventArgs(receivedBytes));
                    }

                    Thread.Sleep(10);
                }
            }
        }
    }
    class TCPCommsEventArgs : EventArgs
    {
        public TCPCommsEventArgs(byte[] packet)
        {
            IncomingData = packet;
        }

        public byte[] IncomingData;
        public bool Acknowledged = false;
    }
}
