using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RelayControlLibrary;

namespace RelayControl
{
    public partial class ucForceCustomerSwitch : UserControl
    {
        public ucForceCustomerSwitch()
        {
            InitializeComponent();
            
            this.comboBoxCustomers.DataSource = Enum.GetValues(typeof(Customers));
        }

        public Customers Customer;

        public delegate void CustomerSwitchHanlder(object sender, CustomerSwitchEventArgs cSEA);
        public event CustomerSwitchHanlder CustomerSwitch;

        private void buttonForceSwitch_Click(object sender, EventArgs e)
        {
            Customers selectedEnum = (Customers)this.comboBoxCustomers.SelectedIndex;

            CustomerSwitchEventArgs cSEA = new CustomerSwitchEventArgs(selectedEnum);

            if(CustomerSwitch != null)
                CustomerSwitch(this, cSEA);
        }
    }

    public class CustomerSwitchEventArgs : EventArgs
    {
        public CustomerSwitchEventArgs(Customers customer)
        {
            this.Customer = customer;
        }

        public Customers Customer;
    }
}
