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
    public partial class ucDNPPointSuperClass : UserControl
    {
        public ucDNPPointSuperClass()
        {
            InitializeComponent();
            this.labelEventEnable.Visible = false;
            this.checkBoxEventEnabled.Visible = false;
        }

        public delegate void PointChangedHandler(object o, DNPPointEventArgs dPEA);
        public event PointChangedHandler PointChanged;

        protected uint pointNumber = 0;
        protected string pointName = "";
        protected bool signed = false;
        protected bool eventEnableVisible = false;

        protected void checkBoxEventEnabled_Click(object sender, EventArgs e)
        {
            if (PointChanged != null)
            {
                DNPPointEventArgs dPEA = new DNPPointEventArgs(this.checkBoxEventEnabled.Checked);
                this.PointChanged(this, dPEA);
            }
        }
    }
}
