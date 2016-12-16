using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public class RelayStatusCodeConverter
    {
        public RelayStatusCodeConverter()
        {
            this.status.Add(new RelayReceiverStatus(0, Color.Green, Color.White, "OP", "Open"));
            this.status.Add(new RelayReceiverStatus(1, Color.Red, Color.White, "CL", "Close"));
            this.status.Add(new RelayReceiverStatus(2, Color.YellowGreen, Color.Black, "FB", "Float Block"));
            this.status.Add(new RelayReceiverStatus(3, Color.Yellow, Color.Black, "FL", "Float"));
            this.status.Add(new RelayReceiverStatus(4, Color.DarkGreen, Color.White, "BF", "Back Feed"));
            this.status.Add(new RelayReceiverStatus(5, Color.Orange, Color.Black, "BO", "Blocked Open"));
            this.status.Add(new RelayReceiverStatus(6, Color.DarkRed, Color.White, "FC", "Failed To Close"));
            this.status.Add(new RelayReceiverStatus(7, Color.Purple, Color.White, "NR", "Bad State - Not Responding"));
            this.status.Add(new RelayReceiverStatus(8, Color.LightGreen, Color.Black, "IB", "Insensitive Backfeed"));
            this.status.Add(new RelayReceiverStatus(9, Color.Pink, Color.Black, "RC", "Relaxed Close"));
            this.status.Add(new RelayReceiverStatus(10, Color.OrangeRed, Color.White, "PA", "Pump Alaram - Lockout"));
            this.status.Add(new RelayReceiverStatus(11, Color.LightBlue, Color.Black, "SL", "Save Service Lockout"));
#if chicago && !DEBUG
            this.status.Add(new RelayReceiverStatus(12, Color.DarkKhaki, Color.Black, "Cross Phase Detected", "Error"));
#else
            this.status.Add(new RelayReceiverStatus(12, Color.DarkKhaki, Color.Black, "XP", "Error"));
#endif
            this.status.Add(new RelayReceiverStatus(13, Color.White, Color.Black, "ER", "Error"));
            this.status.Add(new RelayReceiverStatus(14, Color.White, Color.Black, "ER", "Error"));
            this.status.Add(new RelayReceiverStatus(15, Color.White, Color.Black, "ER", "Error"));
        }

        private List<RelayReceiverStatus> status = new List<RelayReceiverStatus>(16);
        private int statusCode;
        private ToolTip toolTip = new ToolTip();

        public Color CurrentColor;
        public Color CurrentForeColor;
        public string CurrentStatus;
        public string CurrentDescription;

        public int IncomingStatusCode
        {
            get { return this.statusCode; }
            set
            {
                try
                {
                    this.statusCode = value;

                    this.CurrentStatus = status[value].Code;
                    this.CurrentColor = status[value].Color;
                    this.CurrentDescription = status[value].Description;
                    this.CurrentForeColor = status[value].ForeColor;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error In Relay Receiver Status Label", ex);
                }
            }
        }
    }

    class RelayReceiverStatus
    {
        public RelayReceiverStatus(int codeNumber, Color color, Color foreColor, string code, string description)
        {
            this.CodeNumber = codeNumber;
            this.Color = color;
            this.ForeColor = foreColor;
            this.Code = code;
            this.Description = description;
        }

        public int CodeNumber;
        public Color Color;
        public Color ForeColor;
        public string Code;
        public string Description;
    }
}
