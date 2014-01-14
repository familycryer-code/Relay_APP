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
    public partial class ucDNPSAv5 : UserControl
    {
        public ucDNPSAv5()
        {
            InitializeComponent();

            testKeyValues = new ucKeyValuesInputControl(16, "hello world");
            testKeyValues.Visible = true;
            this.testKeyValues.Location = new Point(1, 1);
            this.Controls.Add(this.testKeyValues);
        }

        private ucKeyValuesInputControl testKeyValues;
    }
}
