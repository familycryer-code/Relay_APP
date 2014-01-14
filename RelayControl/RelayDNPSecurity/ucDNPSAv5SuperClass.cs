using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayDNPSecurity
{
    public partial class ucDNPSAv5SuperClass : UserControl
    {
        public ucDNPSAv5SuperClass()
        {
            InitializeComponent();
        }

        public delegate void SendHandler(object o, SecureSendEventArgs sSEA);

        public event SendHandler Send;

        private void onSend(SecureSendEventArgs sSEA)
        {
            if (this.Send != null)
                this.Send(this, sSEA);
        }
    }
}
