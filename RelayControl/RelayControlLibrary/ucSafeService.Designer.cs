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
            this.lbl_CurrImbalance_unit = new System.Windows.Forms.Label();
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
            this.panel_SMsettings = new System.Windows.Forms.Panel();
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
            this.groupBoxSafeService.Controls.Add(this.lbl_CurrImbalance_unit);
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
            this.groupBoxSafeService.Location = new System.Drawing.Point(3, 3);
            this.groupBoxSafeService.Name = "groupBoxSafeService";
            this.groupBoxSafeService.Size = new System.Drawing.Size(255, 292);
            this.groupBoxSafeService.TabIndex = 0;
            this.groupBoxSafeService.TabStop = false;
            this.groupBoxSafeService.Text = "Safe Service Mode";
            // 
            // lbl_CurrImbalance_unit
            // 
            this.lbl_CurrImbalance_unit.AutoSize = true;
            this.lbl_CurrImbalance_unit.Location = new System.Drawing.Point(194, 120);
            this.lbl_CurrImbalance_unit.Name = "lbl_CurrImbalance_unit";
            this.lbl_CurrImbalance_unit.Size = new System.Drawing.Size(50, 19);
            this.lbl_CurrImbalance_unit.TabIndex = 23;
            this.lbl_CurrImbalance_unit.Text = "Amps";
            // 
            // comboBox_DataViews
            // 
            this.comboBox_DataViews.FormattingEnabled = true;
            this.comboBox_DataViews.Items.AddRange(new object[] {
            "Relay",
            "Protector",
            "Percent"});
            this.comboBox_DataViews.Location = new System.Drawing.Point(115, 20);
            this.comboBox_DataViews.Name = "comboBox_DataViews";
            this.comboBox_DataViews.Size = new System.Drawing.Size(64, 27);
            this.comboBox_DataViews.TabIndex = 22;
            this.comboBox_DataViews.SelectedIndexChanged += new System.EventHandler(this.comboBox_DataViews_SelectedItemChanged);
            // 
            // labelCurrentlyEnabled
            // 
            this.labelCurrentlyEnabled.AutoSize = true;
            this.labelCurrentlyEnabled.BackColor = System.Drawing.Color.White;
            this.labelCurrentlyEnabled.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCurrentlyEnabled.Location = new System.Drawing.Point(136, 52);
            this.labelCurrentlyEnabled.Name = "labelCurrentlyEnabled";
            this.labelCurrentlyEnabled.Size = new System.Drawing.Size(71, 21);
            this.labelCurrentlyEnabled.TabIndex = 21;
            this.labelCurrentlyEnabled.Text = "Disabled";
            // 
            // buttonRestoreDefaults
            // 
            this.buttonRestoreDefaults.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestoreDefaults.Location = new System.Drawing.Point(30, 255);
            this.buttonRestoreDefaults.Name = "buttonRestoreDefaults";
            this.buttonRestoreDefaults.Size = new System.Drawing.Size(100, 23);
            this.buttonRestoreDefaults.TabIndex = 20;
            this.buttonRestoreDefaults.Text = "Restore Defaults";
            this.buttonRestoreDefaults.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaults.Click += new System.EventHandler(this.buttonRestoreDefaults_Click);
            // 
            // domainUpDownDataViews
            // 
            this.domainUpDownDataViews.Enabled = false;
            this.domainUpDownDataViews.Location = new System.Drawing.Point(136, 38);
            this.domainUpDownDataViews.Name = "domainUpDownDataViews";
            this.domainUpDownDataViews.Size = new System.Drawing.Size(78, 27);
            this.domainUpDownDataViews.TabIndex = 19;
            this.domainUpDownDataViews.Text = "Relay";
            this.domainUpDownDataViews.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 19);
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
            this.numericUpDownLowVoltage.Location = new System.Drawing.Point(136, 149);
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
            this.numericUpDownLowVoltage.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownLowVoltage.TabIndex = 17;
            this.numericUpDownLowVoltage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownLowVoltage.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 19);
            this.label2.TabIndex = 16;
            this.label2.Text = "Low Voltage:";
            // 
            // labelVoltageImbalanceUnits
            // 
            this.labelVoltageImbalanceUnits.AutoSize = true;
            this.labelVoltageImbalanceUnits.Location = new System.Drawing.Point(194, 189);
            this.labelVoltageImbalanceUnits.Name = "labelVoltageImbalanceUnits";
            this.labelVoltageImbalanceUnits.Size = new System.Drawing.Size(44, 19);
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
            this.numericUpDownVoltageImbalance.Location = new System.Drawing.Point(136, 182);
            this.numericUpDownVoltageImbalance.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownVoltageImbalance.Name = "numericUpDownVoltageImbalance";
            this.numericUpDownVoltageImbalance.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownVoltageImbalance.TabIndex = 14;
            this.numericUpDownVoltageImbalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelVoltageImbalance
            // 
            this.labelVoltageImbalance.AutoSize = true;
            this.labelVoltageImbalance.Location = new System.Drawing.Point(22, 184);
            this.labelVoltageImbalance.Name = "labelVoltageImbalance";
            this.labelVoltageImbalance.Size = new System.Drawing.Size(146, 19);
            this.labelVoltageImbalance.TabIndex = 13;
            this.labelVoltageImbalance.Text = "Voltage Imbalance:";
            // 
            // labelDelayUnits
            // 
            this.labelDelayUnits.AutoSize = true;
            this.labelDelayUnits.Enabled = false;
            this.labelDelayUnits.Location = new System.Drawing.Point(3, 73);
            this.labelDelayUnits.Name = "labelDelayUnits";
            this.labelDelayUnits.Size = new System.Drawing.Size(53, 19);
            this.labelDelayUnits.TabIndex = 12;
            this.labelDelayUnits.Text = "Cycles";
            this.labelDelayUnits.Visible = false;
            // 
            // numericUpDownDelay
            // 
            this.numericUpDownDelay.Enabled = false;
            this.numericUpDownDelay.Location = new System.Drawing.Point(6, 50);
            this.numericUpDownDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownDelay.Name = "numericUpDownDelay";
            this.numericUpDownDelay.Size = new System.Drawing.Size(39, 27);
            this.numericUpDownDelay.TabIndex = 11;
            this.numericUpDownDelay.Visible = false;
            // 
            // labelDelay
            // 
            this.labelDelay.AutoSize = true;
            this.labelDelay.Enabled = false;
            this.labelDelay.Location = new System.Drawing.Point(5, 40);
            this.labelDelay.Name = "labelDelay";
            this.labelDelay.Size = new System.Drawing.Size(54, 19);
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
            this.numericUpDownCurrentImbalance.Location = new System.Drawing.Point(136, 117);
            this.numericUpDownCurrentImbalance.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCurrentImbalance.Name = "numericUpDownCurrentImbalance";
            this.numericUpDownCurrentImbalance.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownCurrentImbalance.TabIndex = 8;
            this.numericUpDownCurrentImbalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelCurrentImbalance
            // 
            this.labelCurrentImbalance.AutoSize = true;
            this.labelCurrentImbalance.Location = new System.Drawing.Point(21, 120);
            this.labelCurrentImbalance.Name = "labelCurrentImbalance";
            this.labelCurrentImbalance.Size = new System.Drawing.Size(146, 19);
            this.labelCurrentImbalance.TabIndex = 7;
            this.labelCurrentImbalance.Text = "Current Imbalance:";
            // 
            // labelOverCurrentUnits
            // 
            this.labelOverCurrentUnits.AutoSize = true;
            this.labelOverCurrentUnits.Location = new System.Drawing.Point(194, 86);
            this.labelOverCurrentUnits.Name = "labelOverCurrentUnits";
            this.labelOverCurrentUnits.Size = new System.Drawing.Size(50, 19);
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
            this.numericUpDownOverCurrent.Location = new System.Drawing.Point(136, 84);
            this.numericUpDownOverCurrent.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownOverCurrent.Name = "numericUpDownOverCurrent";
            this.numericUpDownOverCurrent.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownOverCurrent.TabIndex = 5;
            this.numericUpDownOverCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelOverCurrent
            // 
            this.labelOverCurrent.AutoSize = true;
            this.labelOverCurrent.Location = new System.Drawing.Point(50, 86);
            this.labelOverCurrent.Name = "labelOverCurrent";
            this.labelOverCurrent.Size = new System.Drawing.Size(107, 19);
            this.labelOverCurrent.TabIndex = 4;
            this.labelOverCurrent.Text = "Over Current:";
            // 
            // comboBoxSSEnable
            // 
            this.comboBoxSSEnable.FormattingEnabled = true;
            this.comboBoxSSEnable.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.comboBoxSSEnable.Location = new System.Drawing.Point(28, 20);
            this.comboBoxSSEnable.Name = "comboBoxSSEnable";
            this.comboBoxSSEnable.Size = new System.Drawing.Size(69, 27);
            this.comboBoxSSEnable.TabIndex = 3;
            this.comboBoxSSEnable.Text = "Enable";
            // 
            // labelSafeServiceEnable
            // 
            this.labelSafeServiceEnable.AutoSize = true;
            this.labelSafeServiceEnable.Location = new System.Drawing.Point(86, 51);
            this.labelSafeServiceEnable.Name = "labelSafeServiceEnable";
            this.labelSafeServiceEnable.Size = new System.Drawing.Size(58, 19);
            this.labelSafeServiceEnable.TabIndex = 2;
            this.labelSafeServiceEnable.Text = "Status:";
            // 
            // buttonRequest
            // 
            this.buttonRequest.Location = new System.Drawing.Point(168, 0);
            this.buttonRequest.Name = "buttonRequest";
            this.buttonRequest.Size = new System.Drawing.Size(56, 20);
            this.buttonRequest.TabIndex = 1;
            this.buttonRequest.Text = "Request";
            this.buttonRequest.UseVisualStyleBackColor = true;
            this.buttonRequest.Click += new System.EventHandler(this.buttonRequest_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(145, 255);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(70, 23);
            this.buttonSend.TabIndex = 0;
            this.buttonSend.Text = "Apply";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // panel_SMsettings
            // 
            this.panel_SMsettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_SMsettings.Location = new System.Drawing.Point(1, 0);
            this.panel_SMsettings.Name = "panel_SMsettings";
            this.panel_SMsettings.Size = new System.Drawing.Size(260, 296); //(227, 249);
            this.panel_SMsettings.TabIndex = 25;
            // 
            // ucSafeService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxSafeService);
            this.Controls.Add(this.panel_SMsettings);
            this.Name = "ucSafeService";
            this.Size = new System.Drawing.Size(230, 252);
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
        public System.Windows.Forms.ComboBox comboBox_DataViews;
        private System.Windows.Forms.Label lbl_CurrImbalance_unit;
        private System.Windows.Forms.Panel panel_SMsettings;
    }
}
