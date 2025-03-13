using GraphicsServer.GSNet.Charting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class SendAll_Message_PopUp : Form
    {
        public int cntSendAllTick = 0;
        public SendAll_Message_PopUp()
        {
            InitializeComponent();
           // timer_SendAll.Start();
            //timer_SendAll.Enabled = true;
        }
        
        private void SendAll_Message_PopUp_Load(object sender, EventArgs e)
        {
            progressBar1__SendAll.Step = 1;
            progressBar1__SendAll.Style = ProgressBarStyle.Marquee;//ProgressBarStyle.Marquee;
            progressBar1__SendAll.Visible = true;
            progressBar1__SendAll.Value = 0;

            /* for (int i=0;i<=99; i++)
             { 
                progressBar1__SendAll.Value = i;
                Percent.Text = progressBar1__SendAll.Value.ToString() + "%";
                System.Threading.Thread.Sleep(100);
           } */
            
            
        /*private void timer_SendAll_Tick(object sender, EventArgs e)
        {
            progressBar1__SendAll.Increment(1);
            
            if (progressBar1__SendAll.Value < 100)
            {
                //progressBar1__SendAll.Value += 1;
                progressBar1__SendAll.Increment(1);
                Percent.Text = progressBar1__SendAll.Value.ToString() + "%";
            }
            else 
            {
                timer_SendAll.Stop();
                label1.Text = "Relay Parameter Update Completed !";
            }
           */

        }


    }
}
