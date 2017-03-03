using System;

namespace SharedResources
{
    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception ex, string title)
        {
            this.Title = title;
            this.InnerException = ex;
        }

        public Exception InnerException;
        public string Title;
    }

    public class SendEventArgs : EventArgs
    {
        public SendEventArgs(int arraySize)
        {
            this.SendPacket = new byte[arraySize];
        }

        public byte[] SendPacket;
        public bool WithAck = false;
        public bool RequestAll = true;
    }
}