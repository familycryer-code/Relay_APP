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
    public partial class ucOSAsymKeyGen : ucDNPSAv5SuperClass
    {
        public ucOSAsymKeyGen()
        {
            InitializeComponent();
            this.initializeKeyValueControl();
        }

        private ucKeyValuesInputControl keyBox;
        private static string _keyName = "Relay (Outstation) Public Key";

        #region Initialization

        void initializeKeyValueControl()
        {
            this.keyBox = new ucKeyValuesInputControl(32, _keyName);

            Point tempPoint = new Point(40, 15);

            this.keyBox.Location = tempPoint;

            this.groupBoxMain.Controls.Add(this.keyBox);
        }

        #endregion
    }
}
