using System;
using System.Collections.Generic;
using System.Text;

namespace RelayControlLibrary
{
    public class SineGraphEventArgs : EventArgs
    {
        public SineGraphEventArgs()
        {
        }

        public int ClickedCycleNumber = 0;
    }

    public class CompleteCycleEventArgs : EventArgs
    {
        public CompleteCycleEventArgs(int size)
        {
            this.VtA = new float[size];
            this.VtB = new float[size];
            this.VtC = new float[size];
            this.VnA = new float[size];
            this.VnB = new float[size];
            this.VnC = new float[size];
            this.IA = new float[size];
            this.IB = new float[size];
            this.IC = new float[size];
        }
        
        public float[] VtA;
        public float[] VtB;
        public float[] VtC;
        public float[] VnA;
        public float[] VnB;
        public float[] VnC;
        public float[] IA;
        public float[] IB;
        public float[] IC;
        public int CycleNumber;
        public uint EventNumber;
    }

    public class CycleInfoRequestEventArgs : EventArgs
    {
        public CycleInfoRequestEventArgs(int cycleNumber, uint eventNumber)
        {
            this.CycleNumber = cycleNumber;
            this.EventNumber = eventNumber;
        }

        public int CycleNumber;
        public uint EventNumber;
    }

    public class ExceptionEventArgs: EventArgs
    {
        public ExceptionEventArgs(Exception ex, string title)
        {
            this.Title = title;
            this.InnerException = ex;
        }

        public Exception InnerException;
        public string Title;
    }
}
