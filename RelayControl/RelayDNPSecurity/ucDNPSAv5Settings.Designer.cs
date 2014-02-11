namespace RelayDNPSecurity
{
    partial class ucDNPSAv5Settings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.groupBoxSecurityStats = new System.Windows.Forms.GroupBox();
            this.numericUpDownReplyTimeout = new System.Windows.Forms.NumericUpDown();
            this.labelReplyTimeout = new System.Windows.Forms.Label();
            this.checkBoxSHA1 = new System.Windows.Forms.CheckBox();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.buttonRequestSettings = new System.Windows.Forms.Button();
            this.buttonSendSettings = new System.Windows.Forms.Button();
            this.checkBoxAggressiveMode = new System.Windows.Forms.CheckBox();
            this.groupBoxMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReplyTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.groupBoxSecurityStats);
            this.groupBoxMain.Controls.Add(this.numericUpDownReplyTimeout);
            this.groupBoxMain.Controls.Add(this.labelReplyTimeout);
            this.groupBoxMain.Controls.Add(this.checkBoxSHA1);
            this.groupBoxMain.Controls.Add(this.buttonDefault);
            this.groupBoxMain.Controls.Add(this.buttonRequestSettings);
            this.groupBoxMain.Controls.Add(this.buttonSendSettings);
            this.groupBoxMain.Controls.Add(this.checkBoxAggressiveMode);
            this.groupBoxMain.Location = new System.Drawing.Point(3, 3);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Size = new System.Drawing.Size(917, 290);
            this.groupBoxMain.TabIndex = 0;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "SAv5 Settings";
            // 
            // groupBoxSecurityStats
            // 
            this.groupBoxSecurityStats.Location = new System.Drawing.Point(343, 19);
            this.groupBoxSecurityStats.Name = "groupBoxSecurityStats";
            this.groupBoxSecurityStats.Size = new System.Drawing.Size(568, 264);
            this.groupBoxSecurityStats.TabIndex = 7;
            this.groupBoxSecurityStats.TabStop = false;
            this.groupBoxSecurityStats.Text = "Security Statistics Thresholds";
            // 
            // numericUpDownReplyTimeout
            // 
            this.numericUpDownReplyTimeout.DecimalPlaces = 1;
            this.numericUpDownReplyTimeout.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownReplyTimeout.Location = new System.Drawing.Point(119, 60);
            this.numericUpDownReplyTimeout.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDownReplyTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownReplyTimeout.Name = "numericUpDownReplyTimeout";
            this.numericUpDownReplyTimeout.Size = new System.Drawing.Size(66, 20);
            this.numericUpDownReplyTimeout.TabIndex = 6;
            this.numericUpDownReplyTimeout.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // labelReplyTimeout
            // 
            this.labelReplyTimeout.AutoSize = true;
            this.labelReplyTimeout.Location = new System.Drawing.Point(18, 62);
            this.labelReplyTimeout.Name = "labelReplyTimeout";
            this.labelReplyTimeout.Size = new System.Drawing.Size(95, 13);
            this.labelReplyTimeout.TabIndex = 5;
            this.labelReplyTimeout.Text = "Reply Timeout (s) :";
            // 
            // checkBoxSHA1
            // 
            this.checkBoxSHA1.AutoSize = true;
            this.checkBoxSHA1.Location = new System.Drawing.Point(19, 42);
            this.checkBoxSHA1.Name = "checkBoxSHA1";
            this.checkBoxSHA1.Size = new System.Drawing.Size(99, 17);
            this.checkBoxSHA1.TabIndex = 4;
            this.checkBoxSHA1.Text = "SHA-1 Enabled";
            this.checkBoxSHA1.UseVisualStyleBackColor = true;
            // 
            // buttonDefault
            // 
            this.buttonDefault.Location = new System.Drawing.Point(119, 260);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(107, 23);
            this.buttonDefault.TabIndex = 3;
            this.buttonDefault.Text = "Set Defaults";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // buttonRequestSettings
            // 
            this.buttonRequestSettings.Location = new System.Drawing.Point(6, 260);
            this.buttonRequestSettings.Name = "buttonRequestSettings";
            this.buttonRequestSettings.Size = new System.Drawing.Size(107, 23);
            this.buttonRequestSettings.TabIndex = 2;
            this.buttonRequestSettings.Text = "Request Settings";
            this.buttonRequestSettings.UseVisualStyleBackColor = true;
            this.buttonRequestSettings.Click += new System.EventHandler(this.buttonRequestSettings_Click);
            // 
            // buttonSendSettings
            // 
            this.buttonSendSettings.Location = new System.Drawing.Point(232, 260);
            this.buttonSendSettings.Name = "buttonSendSettings";
            this.buttonSendSettings.Size = new System.Drawing.Size(103, 23);
            this.buttonSendSettings.TabIndex = 1;
            this.buttonSendSettings.Text = "Send Settings";
            this.buttonSendSettings.UseVisualStyleBackColor = true;
            this.buttonSendSettings.Click += new System.EventHandler(this.buttonSendSettings_Click);
            // 
            // checkBoxAggressiveMode
            // 
            this.checkBoxAggressiveMode.AutoSize = true;
            this.checkBoxAggressiveMode.Location = new System.Drawing.Point(19, 19);
            this.checkBoxAggressiveMode.Name = "checkBoxAggressiveMode";
            this.checkBoxAggressiveMode.Size = new System.Drawing.Size(150, 17);
            this.checkBoxAggressiveMode.TabIndex = 0;
            this.checkBoxAggressiveMode.Text = "Aggressive Mode Enabled";
            this.checkBoxAggressiveMode.UseVisualStyleBackColor = true;
            // 
            // ucDNPSAv5Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMain);
            this.Name = "ucDNPSAv5Settings";
            this.Size = new System.Drawing.Size(979, 298);
            this.groupBoxMain.ResumeLayout(false);
            this.groupBoxMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReplyTimeout)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.Button buttonRequestSettings;
        private System.Windows.Forms.Button buttonSendSettings;
        private System.Windows.Forms.CheckBox checkBoxAggressiveMode;
        private System.Windows.Forms.CheckBox checkBoxSHA1;
        private System.Windows.Forms.NumericUpDown numericUpDownReplyTimeout;
        private System.Windows.Forms.Label labelReplyTimeout;
        private System.Windows.Forms.GroupBox groupBoxSecurityStats;
    }
}
