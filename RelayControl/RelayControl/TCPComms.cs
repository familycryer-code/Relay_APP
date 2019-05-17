using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SharedResources;
using System.Threading;
using System.Diagnostics;

namespace RelayControl
{
    class TCPComms
    {
        public TCPComms(IPAddress iPAddress, int port)
        {
            IPAddress = iPAddress;
            Port = port;
            dataPoll.Elapsed += DataPoll_Elapsed;
        }

        // Connect timeout in mS
        private readonly int _connectTimeout = 1000;

        public delegate void DataReceivedHandler(object o, TCPCommsEventArgs tCPCEA);
        public event DataReceivedHandler DataReceived;

        public delegate void ExceptionHandler(object o, ExceptionEventArgs eEA);
        public event ExceptionHandler TCPCommsException;

        public bool IsConnected { get => checkConecction(); }

        private TcpClient client;
        private NetworkStream stream;
        private bool connectionSuccessful = false;
        public int Port;
        public IPAddress IPAddress;
        private byte[] readBuffer = new byte[1000];
        System.Timers.Timer dataPoll = new System.Timers.Timer(1);

        public bool Connect()
        {
            return connect();
        }

        public void SendPacket(byte[] bytePacket)
        {
            Debug.WriteLine(String.Format("Sending Packet: {0}", BitConverter.ToString(bytePacket)));
            if (checkConecction())
            {
                stream.Write(bytePacket, 0, bytePacket.Length);
            }
            else
            {
                attemptReconnect();
            }
        }

        private void attemptReconnect()
        {
            if (client != null)
            {
                client.Close();
                client = null;
            }
            connect();
        }

        private bool checkConecction()
        {
            if (client != null && client.Client != null)
                return client.Connected;
            else
            {
                Debug.WriteLine("Checking Connection Failed");
                return false;
            }
        }


        private bool connect()
        {
            if (client != null)
                return false;
            Debug.WriteLine("TCP Connecting");
            client = new TcpClient();
            var result = client.BeginConnect(IPAddress, Port, portConnected, connectionSuccessful);
            var connected = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(_connectTimeout));

            if (!connected)
            {
                // Failed to connect
                exceptionHandler(new ExceptionEventArgs(new Exception(String.Format("Failed to Connect to {0}:{1}", IPAddress, Port)), "TCP/IP Connect fail"));
                client.Close();
                client = null;
                return false;
            }

            // Successfully Connected
            client.EndConnect(result);
            return true;
        }

        private void portConnected(IAsyncResult ar)
        {
            if (client == null || client.Client == null || !client.Connected)
                return;
            Debug.WriteLine("TCP Port Connected");
            stream = client.GetStream();
            dataPoll.Start();
        }

        private void DataPoll_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            dataPoll.Stop();
            Debug.WriteLine("Beginning Read");
            stream.BeginRead(readBuffer, 0, readBuffer.Length, readDone, null);
        }

        private void exceptionHandler(ExceptionEventArgs eEA)
        {
            if (TCPCommsException != null)
                TCPCommsException(this, eEA);
            else
                Debug.WriteLine("No Subscription for TCPCommsException");
        }

        private void dataReceveid(TCPCommsEventArgs tCPCEA)
        {
            if (DataReceived != null)
                DataReceived(this, tCPCEA);
            else
                Debug.WriteLine("No Subscription for TCP DataReceived event");
        }

        private void readDone(IAsyncResult ar)
        {
            try
            {
                Debug.WriteLine("Reading Done");
                int length = stream.EndRead(ar);
                if (length > 0)
                {
                    byte[] receivedBytes = new byte[length];
                    Array.Copy(readBuffer, receivedBytes, receivedBytes.Length);
                    dataReceveid(new TCPCommsEventArgs(receivedBytes));
                }
                dataPoll.Start();
            }
            catch (Exception ex)
            {
                dataPoll.Stop();
                if (client != null)
                    client.Close();
                client = null;
                exceptionHandler(new ExceptionEventArgs(ex, "TCP Connection Error"));
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
