using System.Windows.Forms;

namespace RelayDNPSecurity
{
    partial class ucDNPSAv5Settings
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;
        public System.ComponentModel.IContainer components = null;
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
        public void InitializeComponent()
        {
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.panel_DNPSAv5set = new System.Windows.Forms.Panel();
            this.comboBoxMACAlogrithm = new System.Windows.Forms.ComboBox();
            this.labelMACAlgorithm = new System.Windows.Forms.Label();
            this.comboBoxKeyChangeAlogrithm = new System.Windows.Forms.ComboBox();
            this.labelKeyChangeAlgorithm = new System.Windows.Forms.Label();
            this.checkBoxAuthenticationEnabled = new System.Windows.Forms.CheckBox();
            this.numericUpDownMaxSessionKeyCount = new System.Windows.Forms.NumericUpDown();
            this.labelMaxSessionKeyCount = new System.Windows.Forms.Label();
            this.numericUpDownSessionKeyChangeCount = new System.Windows.Forms.NumericUpDown();
            this.labelSessionKeyChangeCount = new System.Windows.Forms.Label();
            this.numericUpDownSessionKeyInterval = new System.Windows.Forms.NumericUpDown();
            this.labelSessionKeyInterval = new System.Windows.Forms.Label();
            this.groupBoxSecurityStats = new System.Windows.Forms.GroupBox();
            this.numericUpDownReplyTimeout = new System.Windows.Forms.NumericUpDown();
            this.labelReplyTimeout = new System.Windows.Forms.Label();
            this.checkBoxSHA1 = new System.Windows.Forms.CheckBox();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.buttonRequestSettings = new System.Windows.Forms.Button();
            this.buttonSendSettings = new System.Windows.Forms.Button();
            this.checkBoxAggressiveMode = new System.Windows.Forms.CheckBox();
            this.groupBoxMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSessionKeyCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSessionKeyChangeCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSessionKeyInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReplyTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.comboBoxMACAlogrithm);
            this.groupBoxMain.Controls.Add(this.labelMACAlgorithm);
            this.groupBoxMain.Controls.Add(this.comboBoxKeyChangeAlogrithm);
            this.groupBoxMain.Controls.Add(this.labelKeyChangeAlgorithm);
            this.groupBoxMain.Controls.Add(this.checkBoxAuthenticationEnabled);
            this.groupBoxMain.Controls.Add(this.numericUpDownMaxSessionKeyCount);
            this.groupBoxMain.Controls.Add(this.labelMaxSessionKeyCount);
            this.groupBoxMain.Controls.Add(this.numericUpDownSessionKeyChangeCount);
            this.groupBoxMain.Controls.Add(this.labelSessionKeyChangeCount);
            this.groupBoxMain.Controls.Add(this.numericUpDownSessionKeyInterval);
            this.groupBoxMain.Controls.Add(this.labelSessionKeyInterval);
            this.groupBoxMain.Controls.Add(this.groupBoxSecurityStats);
            this.groupBoxMain.Controls.Add(this.numericUpDownReplyTimeout);
            this.groupBoxMain.Controls.Add(this.labelReplyTimeout);
            this.groupBoxMain.Controls.Add(this.checkBoxSHA1);
            this.groupBoxMain.Controls.Add(this.buttonDefault);
            this.groupBoxMain.Controls.Add(this.buttonRequestSettings);
            this.groupBoxMain.Controls.Add(this.buttonSendSettings);
            this.groupBoxMain.Controls.Add(this.checkBoxAggressiveMode);
            this.groupBoxMain.Controls.Add(this.panel_DNPSAv5set);
            this.groupBoxMain.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMain.Location = new System.Drawing.Point(4, 4);
            this.groupBoxMain.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxMain.Size = new System.Drawing.Size(1297, 985);
            this.groupBoxMain.TabIndex = 0;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "SAv5 Settings";
            // 
            // panel_DNPSAv5set
            // 
            this.panel_DNPSAv5set.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_DNPSAv5set.Location = new System.Drawing.Point(405, 37);
            this.panel_DNPSAv5set.Name = "panel_DNPSAv5set";
            this.panel_DNPSAv5set.Size = new System.Drawing.Size(45, 38);
            this.panel_DNPSAv5set.TabIndex = 18;
            // 
            // comboBoxMACAlogrithm
            // 
            this.comboBoxMACAlogrithm.FormattingEnabled = true;
            this.comboBoxMACAlogrithm.Items.AddRange(new object[] {
            "SHA1 10 OCTET",
            "SHA256 8 OCTET",
            "SHA256 16 OCTET",
            "SHA1 8 OCTET",
            "AESGMAC 12 OCTET"});
            this.comboBoxMACAlogrithm.Location = new System.Drawing.Point(213, 251);
            this.comboBoxMACAlogrithm.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxMACAlogrithm.Name = "comboBoxMACAlogrithm";
            this.comboBoxMACAlogrithm.Size = new System.Drawing.Size(197, 32);
            this.comboBoxMACAlogrithm.TabIndex = 16;
            // 
            // labelMACAlgorithm
            // 
            this.labelMACAlgorithm.AutoSize = true;
            this.labelMACAlgorithm.Location = new System.Drawing.Point(25, 255);
            this.labelMACAlgorithm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMACAlgorithm.Name = "labelMACAlgorithm";
            this.labelMACAlgorithm.Size = new System.Drawing.Size(148, 24);
            this.labelMACAlgorithm.TabIndex = 17;
            this.labelMACAlgorithm.Text = "MAC Algorithm:";
            // 
            // comboBoxKeyChangeAlogrithm
            // 
            this.comboBoxKeyChangeAlogrithm.FormattingEnabled = true;
            this.comboBoxKeyChangeAlogrithm.Items.AddRange(new object[] {
            "AES-128/SHA1-HMAC",
            "AES-256/SHA256-HMAC",
            "AES-256/AES_GMAC",
            "REMOTE UPDATE DISABLE"});
            this.comboBoxKeyChangeAlogrithm.Location = new System.Drawing.Point(213, 215);
            this.comboBoxKeyChangeAlogrithm.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxKeyChangeAlogrithm.Name = "comboBoxKeyChangeAlogrithm";
            this.comboBoxKeyChangeAlogrithm.Size = new System.Drawing.Size(197, 32);
            this.comboBoxKeyChangeAlogrithm.TabIndex = 7;
            // 
            // labelKeyChangeAlgorithm
            // 
            this.labelKeyChangeAlgorithm.AutoSize = true;
            this.labelKeyChangeAlgorithm.Location = new System.Drawing.Point(25, 219);
            this.labelKeyChangeAlgorithm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelKeyChangeAlgorithm.Name = "labelKeyChangeAlgorithm";
            this.labelKeyChangeAlgorithm.Size = new System.Drawing.Size(215, 24);
            this.labelKeyChangeAlgorithm.TabIndex = 15;
            this.labelKeyChangeAlgorithm.Text = "Key Change Algorithm:";
            // 
            // checkBoxAuthenticationEnabled
            // 
            this.checkBoxAuthenticationEnabled.AutoSize = true;
            this.checkBoxAuthenticationEnabled.Location = new System.Drawing.Point(25, 71);
            this.checkBoxAuthenticationEnabled.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAuthenticationEnabled.Name = "checkBoxAuthenticationEnabled";
            this.checkBoxAuthenticationEnabled.Size = new System.Drawing.Size(238, 28);
            this.checkBoxAuthenticationEnabled.TabIndex = 2;
            this.checkBoxAuthenticationEnabled.Text = "Authentication Enabled";
            this.checkBoxAuthenticationEnabled.UseVisualStyleBackColor = true;
            // 
            // numericUpDownMaxSessionKeyCount
            // 
            this.numericUpDownMaxSessionKeyCount.Location = new System.Drawing.Point(213, 186);
            this.numericUpDownMaxSessionKeyCount.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownMaxSessionKeyCount.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMaxSessionKeyCount.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownMaxSessionKeyCount.Name = "numericUpDownMaxSessionKeyCount";
            this.numericUpDownMaxSessionKeyCount.Size = new System.Drawing.Size(68, 32);
            this.numericUpDownMaxSessionKeyCount.TabIndex = 6;
            this.numericUpDownMaxSessionKeyCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownMaxSessionKeyCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelMaxSessionKeyCount
            // 
            this.labelMaxSessionKeyCount.AutoSize = true;
            this.labelMaxSessionKeyCount.Location = new System.Drawing.Point(24, 188);
            this.labelMaxSessionKeyCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMaxSessionKeyCount.Name = "labelMaxSessionKeyCount";
            this.labelMaxSessionKeyCount.Size = new System.Drawing.Size(223, 24);
            this.labelMaxSessionKeyCount.TabIndex = 12;
            this.labelMaxSessionKeyCount.Text = "Max Session Key Count:";
            // 
            // numericUpDownSessionKeyChangeCount
            // 
            this.numericUpDownSessionKeyChangeCount.Location = new System.Drawing.Point(213, 158);
            this.numericUpDownSessionKeyChangeCount.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownSessionKeyChangeCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownSessionKeyChangeCount.Name = "numericUpDownSessionKeyChangeCount";
            this.numericUpDownSessionKeyChangeCount.Size = new System.Drawing.Size(68, 32);
            this.numericUpDownSessionKeyChangeCount.TabIndex = 5;
            this.numericUpDownSessionKeyChangeCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownSessionKeyChangeCount.Value = new decimal(new int[] {
            4025,
            0,
            0,
            0});
            // 
            // labelSessionKeyChangeCount
            // 
            this.labelSessionKeyChangeCount.AutoSize = true;
            this.labelSessionKeyChangeCount.Location = new System.Drawing.Point(24, 160);
            this.labelSessionKeyChangeCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSessionKeyChangeCount.Name = "labelSessionKeyChangeCount";
            this.labelSessionKeyChangeCount.Size = new System.Drawing.Size(254, 24);
            this.labelSessionKeyChangeCount.TabIndex = 10;
            this.labelSessionKeyChangeCount.Text = "Session Key Change Count:";
            // 
            // numericUpDownSessionKeyInterval
            // 
            this.numericUpDownSessionKeyInterval.Location = new System.Drawing.Point(213, 129);
            this.numericUpDownSessionKeyInterval.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownSessionKeyInterval.Maximum = new decimal(new int[] {
            7200,
            0,
            0,
            0});
            this.numericUpDownSessionKeyInterval.Name = "numericUpDownSessionKeyInterval";
            this.numericUpDownSessionKeyInterval.Size = new System.Drawing.Size(68, 32);
            this.numericUpDownSessionKeyInterval.TabIndex = 4;
            this.numericUpDownSessionKeyInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownSessionKeyInterval.Value = new decimal(new int[] {
            1800,
            0,
            0,
            0});
            // 
            // labelSessionKeyInterval
            // 
            this.labelSessionKeyInterval.AutoSize = true;
            this.labelSessionKeyInterval.Location = new System.Drawing.Point(24, 132);
            this.labelSessionKeyInterval.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSessionKeyInterval.Name = "labelSessionKeyInterval";
            this.labelSessionKeyInterval.Size = new System.Drawing.Size(229, 24);
            this.labelSessionKeyInterval.TabIndex = 8;
            this.labelSessionKeyInterval.Text = "Session Key Interval (s):";
            // 
            // groupBoxSecurityStats
            // 
            this.groupBoxSecurityStats.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSecurityStats.Location = new System.Drawing.Point(457, 23);
            this.groupBoxSecurityStats.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxSecurityStats.Name = "groupBoxSecurityStats";
            this.groupBoxSecurityStats.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxSecurityStats.Size = new System.Drawing.Size(793, 985);
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
            this.numericUpDownReplyTimeout.Location = new System.Drawing.Point(213, 101);
            this.numericUpDownReplyTimeout.Margin = new System.Windows.Forms.Padding(4);
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
            this.numericUpDownReplyTimeout.Size = new System.Drawing.Size(68, 32);
            this.numericUpDownReplyTimeout.TabIndex = 3;
            this.numericUpDownReplyTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownReplyTimeout.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // labelReplyTimeout
            // 
            this.labelReplyTimeout.AutoSize = true;
            this.labelReplyTimeout.Location = new System.Drawing.Point(24, 103);
            this.labelReplyTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelReplyTimeout.Name = "labelReplyTimeout";
            this.labelReplyTimeout.Size = new System.Drawing.Size(177, 24);
            this.labelReplyTimeout.TabIndex = 5;
            this.labelReplyTimeout.Text = "Reply Timeout (s):";
            // 
            // checkBoxSHA1
            // 
            this.checkBoxSHA1.AutoSize = true;
            this.checkBoxSHA1.Location = new System.Drawing.Point(25, 47);
            this.checkBoxSHA1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxSHA1.Name = "checkBoxSHA1";
            this.checkBoxSHA1.Size = new System.Drawing.Size(164, 28);
            this.checkBoxSHA1.TabIndex = 1;
            this.checkBoxSHA1.Text = "SHA-1 Enabled";
            this.checkBoxSHA1.UseVisualStyleBackColor = true;
            // 
            // buttonDefault
            // 
            this.buttonDefault.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDefault.Location = new System.Drawing.Point(113, 306);
            this.buttonDefault.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(188, 28);
            this.buttonDefault.TabIndex = 3;
            this.buttonDefault.Text = "Restore Defaults";
            this.buttonDefault.UseMnemonic = false;
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // buttonRequestSettings
            // 
            this.buttonRequestSettings.Location = new System.Drawing.Point(113, 378);
            this.buttonRequestSettings.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRequestSettings.Name = "buttonRequestSettings";
            this.buttonRequestSettings.Size = new System.Drawing.Size(143, 28);
            this.buttonRequestSettings.TabIndex = 2;
            this.buttonRequestSettings.Text = "Request Settings";
            this.buttonRequestSettings.UseVisualStyleBackColor = true;
            this.buttonRequestSettings.Click += new System.EventHandler(this.buttonRequestSettings_Click);
            // 
            // buttonSendSettings
            // 
            this.buttonSendSettings.Location = new System.Drawing.Point(113, 342);
            this.buttonSendSettings.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSendSettings.Name = "buttonSendSettings";
            this.buttonSendSettings.Size = new System.Drawing.Size(137, 28);
            this.buttonSendSettings.TabIndex = 1;
            this.buttonSendSettings.Text = "Apply";
            this.buttonSendSettings.UseVisualStyleBackColor = true;
            this.buttonSendSettings.Click += new System.EventHandler(this.buttonSendSettings_Click);
            // 
            // checkBoxAggressiveMode
            // 
            this.checkBoxAggressiveMode.AutoSize = true;
            this.checkBoxAggressiveMode.Checked = true;
            this.checkBoxAggressiveMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAggressiveMode.Location = new System.Drawing.Point(25, 23);
            this.checkBoxAggressiveMode.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAggressiveMode.Name = "checkBoxAggressiveMode";
            this.checkBoxAggressiveMode.Size = new System.Drawing.Size(259, 28);
            this.checkBoxAggressiveMode.TabIndex = 0;
            this.checkBoxAggressiveMode.Text = "Aggressive Mode Enabled";
            this.checkBoxAggressiveMode.UseVisualStyleBackColor = true;
            // 
            // ucDNPSAv5Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMain);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucDNPSAv5Settings";
            this.Size = new System.Drawing.Size(1305, 480);
            this.groupBoxMain.ResumeLayout(false);
            this.groupBoxMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSessionKeyCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSessionKeyChangeCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSessionKeyInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReplyTimeout)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox groupBoxMain;
        public System.Windows.Forms.Button buttonDefault;
        public System.Windows.Forms.Button buttonRequestSettings;
        public System.Windows.Forms.Button buttonSendSettings;
        public System.Windows.Forms.CheckBox checkBoxAggressiveMode;
        public System.Windows.Forms.CheckBox checkBoxSHA1;
        public System.Windows.Forms.NumericUpDown numericUpDownReplyTimeout;
        public System.Windows.Forms.Label labelReplyTimeout;
        public System.Windows.Forms.GroupBox groupBoxSecurityStats;
        public System.Windows.Forms.NumericUpDown numericUpDownSessionKeyInterval;
        public System.Windows.Forms.Label labelSessionKeyInterval;
        public System.Windows.Forms.NumericUpDown numericUpDownSessionKeyChangeCount;
        public System.Windows.Forms.Label labelSessionKeyChangeCount;
        public System.Windows.Forms.NumericUpDown numericUpDownMaxSessionKeyCount;
        public System.Windows.Forms.Label labelMaxSessionKeyCount;
        public System.Windows.Forms.CheckBox checkBoxAuthenticationEnabled;
        public System.Windows.Forms.ComboBox comboBoxKeyChangeAlogrithm;
        public System.Windows.Forms.Label labelKeyChangeAlgorithm;
        public System.Windows.Forms.ComboBox comboBoxMACAlogrithm;
        public System.Windows.Forms.Label labelMACAlgorithm;
        private Panel panel_DNPSAv5set;
    }
}
