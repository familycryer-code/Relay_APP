using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PhasorDisplayGraph
{
    public partial class PanelPhasorControl : UserControl
    {
        public delegate void SendButtonPressedHandler(object sender, PanelPhasorEventArgs e);
        public event SendButtonPressedHandler buttonSendClick;
    
        public PanelPhasorControl()
        {
            InitializeComponent();
            /*
            this.Name = name;
            this.labelName.Text = name;
            */
        }

        public PanelPhasorControl(string name)
        {
            InitializeComponent();
            this.Name = name;
            this.labelName.Text = name;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            PanelPhasorEventArgs localEventArgs = new PanelPhasorEventArgs();
            localEventArgs.PhasorPoint.X = Convert.ToInt32(this.textBoxX.Text);
            localEventArgs.PhasorPoint.Y = Convert.ToInt32(this.textBoxY.Text);
            buttonSendClick(this, localEventArgs);
        }

    }
}
