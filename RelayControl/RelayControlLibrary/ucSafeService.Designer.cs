namespace RelayControlLibrary
{
    partial class ucSafeService
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
            this.groupBoxSafeService = new System.Windows.Forms.GroupBox();
            this.comboBox_DataViews = new System.Windows.Forms.ComboBox();
            this.labelCurrentlyEnabled = new System.Windows.Forms.Label();
            this.buttonRestoreDefaults = new System.Windows.Forms.Button();
            this.domainUpDownDataViews = new System.Windows.Forms.DomainUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownLowVoltage = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.labelVoltageImbalanceUnits = new System.Windows.Forms.Label();
            this.numericUpDownVoltageImbalance = new System.Windows.Forms.NumericUpDown();
            this.labelVoltageImbalance = new System.Windows.Forms.Label();
            this.labelDelayUnits = new System.Windows.Forms.Label();
            this.numericUpDownDelay = new System.Windows.Forms.NumericUpDown();
            this.labelDelay = new System.Windows.Forms.Label();
            this.numericUpDownCurrentImbalance = new System.Windows.Forms.NumericUpDown();
            this.labelCurrentImbalance = new System.Windows.Forms.Label();
            this.labelOverCurrentUnits = new System.Windows.Forms.Label();
            this.numericUpDownOverCurrent = new System.Windows.Forms.NumericUpDown();
            this.labelOverCurrent = new System.Windows.Forms.Label();
            this.comboBoxSSEnable = new System.Windows.Forms.ComboBox();
            this.labelSafeServiceEnable = new System.Windows.Forms.Label();
            this.buttonRequest = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.groupBoxSafeService.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLowVoltage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVoltageImbalance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCurrentImbalance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOverCurrent)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxSafeService
            // 
            this.groupBoxSafeService.Controls.Add(this.comboBox_DataViews);
            this.groupBoxSafeService.Controls.Add(this.labelCurrentlyEnabled);
            this.groupBoxSafeService.Controls.Add(this.buttonRestoreDefaults);
            this.groupBoxSafeService.Controls.Add(this.domainUpDownDataViews);
            this.groupBoxSafeService.Controls.Add(this.label1);
            this.groupBoxSafeService.Controls.Add(this.numericUpDownLowVoltage);
            this.groupBoxSafeService.Controls.Add(this.label2);
            this.groupBoxSafeService.Controls.Add(this.labelVoltageImbalanceUnits);
            this.groupBoxSafeService.Controls.Add(this.numericUpDownVoltageImbalance);
            this.groupBoxSafeService.Controls.Add(this.labelVoltageImbalance);
            this.groupBoxSafeService.Controls.Add(this.labelDelayUnits);
            this.groupBoxSafeService.Controls.Add(this.numericUpDownDelay);
            this.groupBoxSafeService.Controls.Add(this.labelDelay);
            this.groupBoxSafeService.Controls.Add(this.numericUpDownCurrentImbalance);
            this.groupBoxSafeService.Controls.Add(this.labelCurrentImbalance);
            this.groupBoxSafeService.Controls.Add(this.labelOverCurrentUnits);
            this.groupBoxSafeService.Controls.Add(this.numericUpDownOverCurrent);
            this.groupBoxSafeService.Controls.Add(this.labelOverCurrent);
            this.groupBoxSafeService.Controls.Add(this.comboBoxSSEnable);
            this.groupBoxSafeService.Controls.Add(this.labelSafeServiceEnable);
            this.groupBoxSafeService.Controls.Add(this.buttonRequest);
            this.groupBoxSafeService.Controls.Add(this.buttonSend);
            this.groupBoxSafeService.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSafeService.Location = new System.Drawing.Point(4, 4);
            this.groupBoxSafeService.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxSafeService.Name = "groupBoxSafeService";
            this.groupBoxSafeService.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxSafeService.Size = new System.Drawing.Size(331, 338);
            this.groupBoxSafeService.TabIndex = 0;
            this.groupBoxSafeService.TabStop = false;
            this.groupBoxSafeService.Text = "Safe Service Mode";
            // 
            // comboBox_DataViews
            // 
            this.comboBox_DataViews.FormattingEnabled = true;
            this.comboBox_DataViews.Items.AddRange(new object[] {
            "Relay",
            "Protector",
            "Percent"});
            this.comboBox_DataViews.Location = new System.Drawing.Point(153, 20);
            this.comboBox_DataViews.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_DataViews.Name = "comboBox_DataViews";
            this.comboBox_DataViews.Size = new System.Drawing.Size(84, 32);
            this.comboBox_DataViews.TabIndex = 22;
            this.comboBox_DataViews.SelectedIndexChanged += new System.EventHandler(this.comboBox_DataViews_SelectedItemChanged);
            // 
            // labelCurrentlyEnabled
            // 
            this.labelCurrentlyEnabled.AutoSize = true;
            this.labelCurrentlyEnabled.BackColor = System.Drawing.Color.White;
            this.labelCurrentlyEnabled.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCurrentlyEnabled.Location = new System.Drawing.Point(125, 62);
            this.labelCurrentlyEnabled.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCurrentlyEnabled.Name = "labelCurrentlyEnabled";
            this.labelCurrentlyEnabled.Size = new System.Drawing.Size(89, 26);
            this.labelCurrentlyEnabled.TabIndex = 21;
            this.labelCurrentlyEnabled.Text = "Disabled";
            // 
            // buttonRestoreDefaults
            // 
            this.buttonRestoreDefaults.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestoreDefaults.Location = new System.Drawing.Point(11, 265);
            this.buttonRestoreDefaults.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonRestoreDefaults.Name = "buttonRestoreDefaults";
            this.buttonRestoreDefaults.Size = new System.Drawing.Size(255, 28);
            this.buttonRestoreDefaults.TabIndex = 20;
            this.buttonRestoreDefaults.Text = "Restore Factory Defaults";
            this.buttonRestoreDefaults.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaults.Click += new System.EventHandler(this.buttonRestoreDefaults_Click);
            // 
            // domainUpDownDataViews
            // 
            this.domainUpDownDataViews.Enabled = false;
            this.domainUpDownDataViews.Location = new System.Drawing.Point(181, 47);
            this.domainUpDownDataViews.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.domainUpDownDataViews.Name = "domainUpDownDataViews";
            this.domainUpDownDataViews.Size = new System.Drawing.Size(104, 32);
            this.domainUpDownDataViews.TabIndex = 19;
            this.domainUpDownDataViews.Text = "Relay";
            this.domainUpDownDataViews.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(245, 188);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 24);
            this.label1.TabIndex = 18;
            this.label1.Text = "Volts";
            // 
            // numericUpDownLowVoltage
            // 
            this.numericUpDownLowVoltage.DecimalPlaces = 2;
            this.numericUpDownLowVoltage.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownLowVoltage.Location = new System.Drawing.Point(143, 186);
            this.numericUpDownLowVoltage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownLowVoltage.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDownLowVoltage.Minimum = new decimal(new int[] {
            65,
            0,
            0,
            0});
            this.numericUpDownLowVoltage.Name = "numericUpDownLowVoltage";
            this.numericUpDownLowVoltage.Size = new System.Drawing.Size(96, 32);
            this.numericUpDownLowVoltage.TabIndex = 17;
            this.numericUpDownLowVoltage.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 188);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 24);
            this.label2.TabIndex = 16;
            this.label2.Text = "Low Voltage:";
            // 
            // labelVoltageImbalanceUnits
            // 
            this.labelVoltageImbalanceUnits.AutoSize = true;
            this.labelVoltageImbalanceUnits.Location = new System.Drawing.Point(245, 233);
            this.labelVoltageImbalanceUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVoltageImbalanceUnits.Name = "labelVoltageImbalanceUnits";
            this.labelVoltageImbalanceUnits.Size = new System.Drawing.Size(54, 24);
            this.labelVoltageImbalanceUnits.TabIndex = 15;
            this.labelVoltageImbalanceUnits.Text = "Volts";
            // 
            // numericUpDownVoltageImbalance
            // 
            this.numericUpDownVoltageImbalance.DecimalPlaces = 2;
            this.numericUpDownVoltageImbalance.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownVoltageImbalance.Location = new System.Drawing.Point(143, 224);
            this.numericUpDownVoltageImbalance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownVoltageImbalance.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownVoltageImbalance.Name = "numericUpDownVoltageImbalance";
            this.numericUpDownVoltageImbalance.Size = new System.Drawing.Size(96, 32);
            this.numericUpDownVoltageImbalance.TabIndex = 14;
            // 
            // labelVoltageImbalance
            // 
            this.labelVoltageImbalance.AutoSize = true;
            this.labelVoltageImbalance.Location = new System.Drawing.Point(4, 226);
            this.labelVoltageImbalance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVoltageImbalance.Name = "labelVoltageImbalance";
            this.labelVoltageImbalance.Size = new System.Drawing.Size(184, 24);
            this.labelVoltageImbalance.TabIndex = 13;
            this.labelVoltageImbalance.Text = "Voltage Imbalance:";
            // 
            // labelDelayUnits
            // 
            this.labelDelayUnits.AutoSize = true;
            this.labelDelayUnits.Enabled = false;
            this.labelDelayUnits.Location = new System.Drawing.Point(4, 90);
            this.labelDelayUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDelayUnits.Name = "labelDelayUnits";
            this.labelDelayUnits.Size = new System.Drawing.Size(66, 24);
            this.labelDelayUnits.TabIndex = 12;
            this.labelDelayUnits.Text = "Cycles";
            this.labelDelayUnits.Visible = false;
            // 
            // numericUpDownDelay
            // 
            this.numericUpDownDelay.Enabled = false;
            this.numericUpDownDelay.Location = new System.Drawing.Point(8, 62);
            this.numericUpDownDelay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownDelay.Name = "numericUpDownDelay";
            this.numericUpDownDelay.Size = new System.Drawing.Size(52, 32);
            this.numericUpDownDelay.TabIndex = 11;
            this.numericUpDownDelay.Visible = false;
            // 
            // labelDelay
            // 
            this.labelDelay.AutoSize = true;
            this.labelDelay.Enabled = false;
            this.labelDelay.Location = new System.Drawing.Point(7, 49);
            this.labelDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDelay.Name = "labelDelay";
            this.labelDelay.Size = new System.Drawing.Size(68, 24);
            this.labelDelay.TabIndex = 10;
            this.labelDelay.Text = "Delay:";
            this.labelDelay.Visible = false;
            // 
            // numericUpDownCurrentImbalance
            // 
            this.numericUpDownCurrentImbalance.DecimalPlaces = 2;
            this.numericUpDownCurrentImbalance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownCurrentImbalance.Location = new System.Drawing.Point(143, 143);
            this.numericUpDownCurrentImbalance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownCurrentImbalance.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCurrentImbalance.Name = "numericUpDownCurrentImbalance";
            this.numericUpDownCurrentImbalance.Size = new System.Drawing.Size(96, 32);
            this.numericUpDownCurrentImbalance.TabIndex = 8;
            // 
            // labelCurrentImbalance
            // 
            this.labelCurrentImbalance.AutoSize = true;
            this.labelCurrentImbalance.Location = new System.Drawing.Point(8, 148);
            this.labelCurrentImbalance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCurrentImbalance.Name = "labelCurrentImbalance";
            this.labelCurrentImbalance.Size = new System.Drawing.Size(182, 24);
            this.labelCurrentImbalance.TabIndex = 7;
            this.labelCurrentImbalance.Text = "Current Imbalance:";
            // 
            // labelOverCurrentUnits
            // 
            this.labelOverCurrentUnits.AutoSize = true;
            this.labelOverCurrentUnits.Location = new System.Drawing.Point(245, 106);
            this.labelOverCurrentUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOverCurrentUnits.Name = "labelOverCurrentUnits";
            this.labelOverCurrentUnits.Size = new System.Drawing.Size(59, 24);
            this.labelOverCurrentUnits.TabIndex = 6;
            this.labelOverCurrentUnits.Text = "Amps";
            // 
            // numericUpDownOverCurrent
            // 
            this.numericUpDownOverCurrent.DecimalPlaces = 2;
            this.numericUpDownOverCurrent.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownOverCurrent.Location = new System.Drawing.Point(143, 103);
            this.numericUpDownOverCurrent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownOverCurrent.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownOverCurrent.Name = "numericUpDownOverCurrent";
            this.numericUpDownOverCurrent.Size = new System.Drawing.Size(96, 32);
            this.numericUpDownOverCurrent.TabIndex = 5;
            // 
            // labelOverCurrent
            // 
            this.labelOverCurrent.AutoSize = true;
            this.labelOverCurrent.Location = new System.Drawing.Point(33, 106);
            this.labelOverCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOverCurrent.Name = "labelOverCurrent";
            this.labelOverCurrent.Size = new System.Drawing.Size(131, 24);
            this.labelOverCurrent.TabIndex = 4;
            this.labelOverCurrent.Text = "Over Current:";
            // 
            // comboBoxSSEnable
            // 
            this.comboBoxSSEnable.FormattingEnabled = true;
            this.comboBoxSSEnable.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.comboBoxSSEnable.Location = new System.Drawing.Point(37, 20);
            this.comboBoxSSEnable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxSSEnable.Name = "comboBoxSSEnable";
            this.comboBoxSSEnable.Size = new System.Drawing.Size(96, 32);
            this.comboBoxSSEnable.TabIndex = 3;
            this.comboBoxSSEnable.Text = "Enable";
            // 
            // labelSafeServiceEnable
            // 
            this.labelSafeServiceEnable.AutoSize = true;
            this.labelSafeServiceEnable.Location = new System.Drawing.Point(64, 62);
            this.labelSafeServiceEnable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSafeServiceEnable.Name = "labelSafeServiceEnable";
            this.labelSafeServiceEnable.Size = new System.Drawing.Size(73, 24);
            this.labelSafeServiceEnable.TabIndex = 2;
            this.labelSafeServiceEnable.Text = "Status:";
            // 
            // buttonRequest
            // 
            this.buttonRequest.Location = new System.Drawing.Point(224, 0);
            this.buttonRequest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonRequest.Name = "buttonRequest";
            this.buttonRequest.Size = new System.Drawing.Size(75, 25);
            this.buttonRequest.TabIndex = 1;
            this.buttonRequest.Text = "Request";
            this.buttonRequest.UseVisualStyleBackColor = true;
            this.buttonRequest.Click += new System.EventHandler(this.buttonRequest_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(12, 288);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(227, 28);
            this.buttonSend.TabIndex = 0;
            this.buttonSend.Text = "Program Safe Service";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // ucSafeService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxSafeService);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ucSafeService";
            this.Size = new System.Drawing.Size(307, 310);
            this.groupBoxSafeService.ResumeLayout(false);
            this.groupBoxSafeService.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLowVoltage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVoltageImbalance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCurrentImbalance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOverCurrent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox groupBoxSafeService;
        private System.Windows.Forms.Button buttonRequest;
        public System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.ComboBox comboBoxSSEnable;
        private System.Windows.Forms.Label labelSafeServiceEnable;
        private System.Windows.Forms.Label labelOverCurrentUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownOverCurrent;
        private System.Windows.Forms.Label labelOverCurrent;
        private System.Windows.Forms.Label labelVoltageImbalanceUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownVoltageImbalance;
        private System.Windows.Forms.Label labelVoltageImbalance;
        private System.Windows.Forms.Label labelDelayUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownDelay;
        private System.Windows.Forms.Label labelDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownCurrentImbalance;
        private System.Windows.Forms.Label labelCurrentImbalance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownLowVoltage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DomainUpDown domainUpDownDataViews;
        private System.Windows.Forms.Button buttonRestoreDefaults;
        private System.Windows.Forms.Label labelCurrentlyEnabled;
        private System.Windows.Forms.ComboBox comboBox_DataViews;
    }
}
