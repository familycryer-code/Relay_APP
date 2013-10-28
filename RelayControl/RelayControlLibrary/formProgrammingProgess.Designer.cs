namespace RelayControlLibrary
{
    partial class formProgrammingProgess
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBarLoading = new System.Windows.Forms.ProgressBar();
            this.labelCurrentTask = new System.Windows.Forms.Label();
            this.checkBoxMasterCode = new System.Windows.Forms.CheckBox();
            this.checkBoxMasterData = new System.Windows.Forms.CheckBox();
            this.checkBoxRelayCode = new System.Windows.Forms.CheckBox();
            this.checkBoxRelayData = new System.Windows.Forms.CheckBox();
            this.checkBoxFPGA = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // progressBarLoading
            // 
            this.progressBarLoading.Location = new System.Drawing.Point(12, 101);
            this.progressBarLoading.Name = "progressBarLoading";
            this.progressBarLoading.Size = new System.Drawing.Size(431, 23);
            this.progressBarLoading.TabIndex = 0;
            // 
            // labelCurrentTask
            // 
            this.labelCurrentTask.AutoSize = true;
            this.labelCurrentTask.Location = new System.Drawing.Point(12, 85);
            this.labelCurrentTask.Name = "labelCurrentTask";
            this.labelCurrentTask.Size = new System.Drawing.Size(138, 13);
            this.labelCurrentTask.TabIndex = 1;
            this.labelCurrentTask.Text = "Loading Master Relay Code";
            // 
            // checkBoxMasterCode
            // 
            this.checkBoxMasterCode.AutoCheck = false;
            this.checkBoxMasterCode.AutoSize = true;
            this.checkBoxMasterCode.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxMasterCode.Location = new System.Drawing.Point(156, 12);
            this.checkBoxMasterCode.Name = "checkBoxMasterCode";
            this.checkBoxMasterCode.Size = new System.Drawing.Size(133, 17);
            this.checkBoxMasterCode.TabIndex = 2;
            this.checkBoxMasterCode.Text = "Master Code Complete";
            this.checkBoxMasterCode.UseVisualStyleBackColor = false;
            // 
            // checkBoxMasterData
            // 
            this.checkBoxMasterData.AutoCheck = false;
            this.checkBoxMasterData.AutoSize = true;
            this.checkBoxMasterData.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxMasterData.Location = new System.Drawing.Point(156, 35);
            this.checkBoxMasterData.Name = "checkBoxMasterData";
            this.checkBoxMasterData.Size = new System.Drawing.Size(131, 17);
            this.checkBoxMasterData.TabIndex = 3;
            this.checkBoxMasterData.Text = "Master Data Complete";
            this.checkBoxMasterData.UseVisualStyleBackColor = false;
            // 
            // checkBoxRelayCode
            // 
            this.checkBoxRelayCode.AutoCheck = false;
            this.checkBoxRelayCode.AutoSize = true;
            this.checkBoxRelayCode.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxRelayCode.Location = new System.Drawing.Point(12, 12);
            this.checkBoxRelayCode.Name = "checkBoxRelayCode";
            this.checkBoxRelayCode.Size = new System.Drawing.Size(128, 17);
            this.checkBoxRelayCode.TabIndex = 4;
            this.checkBoxRelayCode.Text = "Relay Code Complete";
            this.checkBoxRelayCode.UseVisualStyleBackColor = false;
            // 
            // checkBoxRelayData
            // 
            this.checkBoxRelayData.AutoCheck = false;
            this.checkBoxRelayData.AutoSize = true;
            this.checkBoxRelayData.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxRelayData.Location = new System.Drawing.Point(12, 35);
            this.checkBoxRelayData.Name = "checkBoxRelayData";
            this.checkBoxRelayData.Size = new System.Drawing.Size(126, 17);
            this.checkBoxRelayData.TabIndex = 5;
            this.checkBoxRelayData.Text = "Relay Data Complete";
            this.checkBoxRelayData.UseVisualStyleBackColor = false;
            // 
            // checkBoxFPGA
            // 
            this.checkBoxFPGA.AutoCheck = false;
            this.checkBoxFPGA.AutoSize = true;
            this.checkBoxFPGA.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxFPGA.Location = new System.Drawing.Point(295, 12);
            this.checkBoxFPGA.Name = "checkBoxFPGA";
            this.checkBoxFPGA.Size = new System.Drawing.Size(101, 17);
            this.checkBoxFPGA.TabIndex = 6;
            this.checkBoxFPGA.Text = "FPGA Complete";
            this.checkBoxFPGA.UseVisualStyleBackColor = false;
            // 
            // formProgrammingProgess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 136);
            this.Controls.Add(this.checkBoxFPGA);
            this.Controls.Add(this.checkBoxRelayData);
            this.Controls.Add(this.checkBoxRelayCode);
            this.Controls.Add(this.checkBoxMasterData);
            this.Controls.Add(this.checkBoxMasterCode);
            this.Controls.Add(this.labelCurrentTask);
            this.Controls.Add(this.progressBarLoading);
            this.Name = "formProgrammingProgess";
            this.Text = "Reprogramming Progress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarLoading;
        private System.Windows.Forms.Label labelCurrentTask;
        private System.Windows.Forms.CheckBox checkBoxMasterCode;
        private System.Windows.Forms.CheckBox checkBoxMasterData;
        private System.Windows.Forms.CheckBox checkBoxRelayCode;
        private System.Windows.Forms.CheckBox checkBoxRelayData;
        private System.Windows.Forms.CheckBox checkBoxFPGA;
    }
}