using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharedResources;

namespace RelayControlLibrary
{
    public partial class ucSuperClass : UserControl
    {
        public ucSuperClass()
        {
            InitializeComponent();
        }

        public delegate void SendEventHandler(object o, SendEventArgs sEA);
        public event SendEventHandler Send;

        public delegate void ExceptionHandler(object o, ExceptionEventArgs eEA);
        public event ExceptionHandler Error;

        protected virtual void OnSend(object o, SendEventArgs sEA)
        {
            if (Send != null)
            {
                Send(o, sEA);
            }
        }

        protected virtual void OnError(object o, ExceptionEventArgs eEA)
        {
            if (Error != null)
            {
                Error(o, eEA);
            }
        }
    }
}
