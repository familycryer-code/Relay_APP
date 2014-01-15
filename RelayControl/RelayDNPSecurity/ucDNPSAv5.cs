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
    public partial class ucDNPSAv5 : ucDNPSAv5SuperClass
    {
        public ucDNPSAv5()
        {
            InitializeComponent();

            testKeyValues = new ucKeyValuesInputControl(64, "hello world");
            testKeyValues.Visible = true;
            this.testKeyValues.Location = new Point(10, 10);
            this.Controls.Add(this.testKeyValues);
            
        }

        private ucKeyValuesInputControl testKeyValues;

        private void buttonTestKey_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] testArray = this.testKeyValues.GetKey();

                StringBuilder s = new StringBuilder(testArray.Length * 2);

                foreach (byte b in testArray)
                {
                    s.AppendFormat("{0:x2}", b);
                }
                this.onError(new Exception(s.ToString()));
            }
            catch (Exception ex)
            {
                this.onError(ex);
            }
        }
    }
}
