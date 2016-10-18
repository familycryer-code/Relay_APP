using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class formProgrammingProgess : Form
    {
        public formProgrammingProgess()
        {
            InitializeComponent();
            this.progressBarLoading.Maximum = 500;
        }

        public bool MasterCodeComplete
        {
            get { return this.checkBoxMasterCode.Checked; }
            set
            {
                this.checkBoxMasterCode.Checked = value;
            }
        }
        public bool MasterDataComplete
        {
            get { return this.checkBoxMasterData.Checked; }
            set
            {
                this.checkBoxMasterData.Checked = value;
            }
        }
        public bool RelayCodeComplete
        {
            get { return this.checkBoxRelayCode.Checked; }
            set
            {
                this.checkBoxRelayCode.Checked = value;
            }
        }
        public bool RelayDataComplete
        {
            get { return this.checkBoxRelayData.Checked; }
            set
            {
                this.checkBoxRelayData.Checked = value;
            }

        }
        public bool FPGAComplete
        {
            get { return this.checkBoxFPGA.Checked; }
            set
            {
                this.checkBoxFPGA.Checked = value;
            }
        }
        public bool TransmitterPresent
        {
            get { return this.checkBoxFPGA.Visible; }
            set
            {
                this.checkBoxFPGA.Visible = value;
            }
        }

        public bool MasterBootComplete
        {
            get { return this.checkBoxMasterBootCodeComplete.Checked; }
            set
            {
                this.checkBoxMasterBootCodeComplete.Checked = value;
            }
        }

        public int Maximum
        {
            get { return this.progressBarLoading.Maximum; }
            set
            {
                this.progressBarLoading.Maximum = value;
                this.progressBarLoading.Value = 0;
            }
        }
        public int ProgressValue
        {
            get { return this.progressBarLoading.Value; }
            set
            {
                this.Show();
                if (value > this.Maximum)
                    this.Maximum = value;

                this.progressBarLoading.Value = value;
            }
        }

        public string CurrentTask
        {
            get { return this.labelCurrentTask.Text; }
            set
            {
                this.Show();
                this.labelCurrentTask.Text = value;
            }
        }

        public void ClearAllChecks()
        {
            this.MasterCodeComplete = false;
            this.MasterDataComplete = false;
            this.RelayCodeComplete = false;
            this.RelayDataComplete = false;
            this.FPGAComplete = false;
        }
    }
}
