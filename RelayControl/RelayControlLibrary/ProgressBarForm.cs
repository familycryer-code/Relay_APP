using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ProgressBarForm : Form
    {
        public ProgressBarForm()
        {
            InitializeComponent();
            this.Text = "Downloading";
            this.labelDownloadingText.Text = "Downloading";
        }

        public ProgressBarForm(string title, string downloadingText, int halfSecondCounts, bool timeoutacceptable)
        {
            InitializeComponent();
            this.Text = title;
            this.labelDownloadingText.Text = downloadingText;
            this.HalfSecondCounts = halfSecondCounts;
            if(timeoutacceptable)
                this.buttonCancel.Visible = false;

            this.timeOutAcceptable = timeoutacceptable;

            this.startProgressBar();
        }

        public ProgressBarForm(string downloadingText, string title, int halfSecondCounts)
        {
            InitializeComponent();
            this.Text = title;
            this.labelDownloadingText.Text = downloadingText;
            this.HalfSecondCounts = halfSecondCounts;
            this.startProgressBar();
        }

        private bool timeOutAcceptable = false;
        public int HalfSecondCounts = 100;

        private void startProgressBar()
        {
            this.progressBarMain.Maximum = this.HalfSecondCounts;
            this.timerCountDown.Enabled = true;
        }

        private void timerCountDown_Tick(object sender, EventArgs e)
        {
            this.progressBarMain.Value++;
            if(progressBarMain.Value >= progressBarMain.Maximum)
            {
                this.timerCountDown.Enabled = false;
                if(!this.timeOutAcceptable)
                    this.done(ProgressFormCompleteStates.TimeOut, "Timed Out");
                else
                    this.done(ProgressFormCompleteStates.AcceptableTimeOut, "Done");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.timerCountDown.Enabled = false;
            this.done(ProgressFormCompleteStates.Cancelled, this.Text + " Canceled By User");
        }

        public delegate void ProgressBarEvent(ProgressFormCompleteStates b, string s);  //bool is whether or not it finished successfully
        public event ProgressBarEvent Done;

        private void done(ProgressFormCompleteStates b, string s)
        {
            if (Done != null)
                this.Done(b, s);
        }

        private void ProgressBarForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.timerCountDown.Enabled = false;
            if(!this.timeOutAcceptable)
                this.done(ProgressFormCompleteStates.Failure, "Form Closed \r\n No Gaurantee that All Data was Received.");
        }
    }

    public enum ProgressFormCompleteStates
    {
        Success,
        Cancelled,
        Failure,
        TimeOut,
        AcceptableTimeOut //for use when it times out, but it is supposed to.
    }
}