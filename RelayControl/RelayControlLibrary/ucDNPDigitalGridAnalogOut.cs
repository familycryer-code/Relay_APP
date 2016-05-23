using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucDNPDigitalGridAnalogOut : ucDNPMemphisAnalog
    {
        public ucDNPDigitalGridAnalogOut()
        {
            InitializeComponent();
            this.labelEventEnable.Visible = false;
        }
    }
}
