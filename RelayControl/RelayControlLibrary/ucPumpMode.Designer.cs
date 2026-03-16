namespace RelayControlLibrary
{
    partial class ucPumpMode
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
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownCycleLimit = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSend = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownPumpTime = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonRestoreDefaults = new System.Windows.Forms.Button();
            this.checkBoxNeverReclose = new System.Windows.Forms.CheckBox();
            this.groupBoxRelayCycles = new System.Windows.Forms.GroupBox();
            this.checkBoxCycles = new System.Windows.Forms.CheckBox();
            this.groupBoxMotorTimeout = new System.Windows.Forms.GroupBox();
            this.labelMotorTimeout = new System.Windows.Forms.Label();
            this.numericUpDownMotorTimeout = new System.Windows.Forms.NumericUpDown();
            this.labelMotorTimeoutUnits = new System.Windows.Forms.Label();
            this.checkBoxMotorTime = new System.Windows.Forms.CheckBox();
            this.labelPumpTypeDisplay = new System.Windows.Forms.Label();
            this.labelPumpType = new System.Windows.Forms.Label();
            this.labelEnable = new System.Windows.Forms.Label();
            this.labelPumpProtect = new System.Windows.Forms.Label();
            this.labelProtectTimeUnits = new System.Windows.Forms.Label();
            this.numericUpDownProtectTime = new System.Windows.Forms.NumericUpDown();
            this.labelProtectTime = new System.Windows.Forms.Label();
            this.groupBoxMotorCycles = new System.Windows.Forms.GroupBox();
            this.labelMotorCycles = new System.Windows.Forms.Label();
            this.numericUpDownMotorCycles = new System.Windows.Forms.NumericUpDown();
            this.labelCyclesUnits = new System.Windows.Forms.Label();
            this.checkBoxMotorCycles = new System.Windows.Forms.CheckBox();
            this.groupBoxPumpMode = new System.Windows.Forms.GroupBox();
            this.checkBoxAlarmOnly = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCycleLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPumpTime)).BeginInit();
            this.groupBoxRelayCycles.SuspendLayout();
            this.groupBoxMotorTimeout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMotorTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownProtectTime)).BeginInit();
            this.groupBoxMotorCycles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMotorCycles)).BeginInit();
            this.groupBoxPumpMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(23, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cycle Limit:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numericUpDownCycleLimit
            // 
            this.numericUpDownCycleLimit.Location = new System.Drawing.Point(119, 14);
            this.numericUpDownCycleLimit.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownCycleLimit.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownCycleLimit.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownCycleLimit.Name = "numericUpDownCycleLimit";
            this.numericUpDownCycleLimit.Size = new System.Drawing.Size(87, 32);
            this.numericUpDownCycleLimit.TabIndex = 2;
            this.numericUpDownCycleLimit.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(213, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cycles";
            // 
            // buttonSend
            // 
            this.buttonSend.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSend.Location = new System.Drawing.Point(35, 293);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(186, 28);
            this.buttonSend.TabIndex = 13;
            this.buttonSend.Text = "Program Pump";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(213, 48);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "Seconds";
            // 
            // numericUpDownPumpTime
            // 
            this.numericUpDownPumpTime.Location = new System.Drawing.Point(119, 46);
            this.numericUpDownPumpTime.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownPumpTime.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownPumpTime.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownPumpTime.Name = "numericUpDownPumpTime";
            this.numericUpDownPumpTime.Size = new System.Drawing.Size(87, 32);
            this.numericUpDownPumpTime.TabIndex = 4;
            this.numericUpDownPumpTime.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(23, 48);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 22);
            this.label4.TabIndex = 4;
            this.label4.Text = "Pump Time:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonRestoreDefaults
            // 
            this.buttonRestoreDefaults.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestoreDefaults.Location = new System.Drawing.Point(34, 257);
            this.buttonRestoreDefaults.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRestoreDefaults.Name = "buttonRestoreDefaults";
            this.buttonRestoreDefaults.Size = new System.Drawing.Size(267, 28);
            this.buttonRestoreDefaults.TabIndex = 1;
            this.buttonRestoreDefaults.Text = "Restore Factory Defaults";
            this.buttonRestoreDefaults.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaults.Click += new System.EventHandler(this.buttonRestoreDefaults_Click);
            // 
            // checkBoxNeverReclose
            // 
            this.checkBoxNeverReclose.AutoSize = true;
            this.checkBoxNeverReclose.Location = new System.Drawing.Point(24, 238);
            this.checkBoxNeverReclose.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxNeverReclose.Name = "checkBoxNeverReclose";
            this.checkBoxNeverReclose.Size = new System.Drawing.Size(158, 28);
            this.checkBoxNeverReclose.TabIndex = 10;
            this.checkBoxNeverReclose.Text = "Never Reclose";
            this.checkBoxNeverReclose.UseVisualStyleBackColor = true;
            this.checkBoxNeverReclose.CheckedChanged += new System.EventHandler(this.checkBoxNeverReclose_CheckedChanged);
            // 
            // groupBoxRelayCycles
            // 
            this.groupBoxRelayCycles.Controls.Add(this.label1);
            this.groupBoxRelayCycles.Controls.Add(this.label3);
            this.groupBoxRelayCycles.Controls.Add(this.numericUpDownPumpTime);
            this.groupBoxRelayCycles.Controls.Add(this.label4);
            this.groupBoxRelayCycles.Controls.Add(this.checkBoxCycles);
            this.groupBoxRelayCycles.Controls.Add(this.label2);
            this.groupBoxRelayCycles.Controls.Add(this.numericUpDownCycleLimit);
            this.groupBoxRelayCycles.Location = new System.Drawing.Point(8, 39);
            this.groupBoxRelayCycles.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxRelayCycles.Name = "groupBoxRelayCycles";
            this.groupBoxRelayCycles.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxRelayCycles.Size = new System.Drawing.Size(321, 75);
            this.groupBoxRelayCycles.TabIndex = 30;
            this.groupBoxRelayCycles.TabStop = false;
            // 
            // checkBoxCycles
            // 
            this.checkBoxCycles.AutoSize = true;
            this.checkBoxCycles.Location = new System.Drawing.Point(287, 17);
            this.checkBoxCycles.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxCycles.Name = "checkBoxCycles";
            this.checkBoxCycles.Size = new System.Drawing.Size(18, 17);
            this.checkBoxCycles.TabIndex = 26;
            this.checkBoxCycles.UseVisualStyleBackColor = true;
            // 
            // groupBoxMotorTimeout
            // 
            this.groupBoxMotorTimeout.Controls.Add(this.labelMotorTimeout);
            this.groupBoxMotorTimeout.Controls.Add(this.numericUpDownMotorTimeout);
            this.groupBoxMotorTimeout.Controls.Add(this.labelMotorTimeoutUnits);
            this.groupBoxMotorTimeout.Controls.Add(this.checkBoxMotorTime);
            this.groupBoxMotorTimeout.Location = new System.Drawing.Point(8, 108);
            this.groupBoxMotorTimeout.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxMotorTimeout.Name = "groupBoxMotorTimeout";
            this.groupBoxMotorTimeout.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxMotorTimeout.Size = new System.Drawing.Size(323, 43);
            this.groupBoxMotorTimeout.TabIndex = 31;
            this.groupBoxMotorTimeout.TabStop = false;
            // 
            // labelMotorTimeout
            // 
            this.labelMotorTimeout.AutoSize = true;
            this.labelMotorTimeout.Location = new System.Drawing.Point(12, 18);
            this.labelMotorTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMotorTimeout.Name = "labelMotorTimeout";
            this.labelMotorTimeout.Size = new System.Drawing.Size(148, 24);
            this.labelMotorTimeout.TabIndex = 22;
            this.labelMotorTimeout.Text = "Motor Timeout:";
            // 
            // numericUpDownMotorTimeout
            // 
            this.numericUpDownMotorTimeout.Location = new System.Drawing.Point(119, 14);
            this.numericUpDownMotorTimeout.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownMotorTimeout.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDownMotorTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMotorTimeout.Name = "numericUpDownMotorTimeout";
            this.numericUpDownMotorTimeout.Size = new System.Drawing.Size(87, 32);
            this.numericUpDownMotorTimeout.TabIndex = 5;
            this.numericUpDownMotorTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labelMotorTimeoutUnits
            // 
            this.labelMotorTimeoutUnits.AutoSize = true;
            this.labelMotorTimeoutUnits.Location = new System.Drawing.Point(213, 16);
            this.labelMotorTimeoutUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMotorTimeoutUnits.Name = "labelMotorTimeoutUnits";
            this.labelMotorTimeoutUnits.Size = new System.Drawing.Size(83, 24);
            this.labelMotorTimeoutUnits.TabIndex = 24;
            this.labelMotorTimeoutUnits.Text = "Seconds";
            // 
            // checkBoxMotorTime
            // 
            this.checkBoxMotorTime.AutoSize = true;
            this.checkBoxMotorTime.Location = new System.Drawing.Point(287, 17);
            this.checkBoxMotorTime.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxMotorTime.Name = "checkBoxMotorTime";
            this.checkBoxMotorTime.Size = new System.Drawing.Size(18, 17);
            this.checkBoxMotorTime.TabIndex = 6;
            this.checkBoxMotorTime.UseVisualStyleBackColor = true;
            // 
            // labelPumpTypeDisplay
            // 
            this.labelPumpTypeDisplay.AutoSize = true;
            this.labelPumpTypeDisplay.Location = new System.Drawing.Point(109, 20);
            this.labelPumpTypeDisplay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPumpTypeDisplay.Name = "labelPumpTypeDisplay";
            this.labelPumpTypeDisplay.Size = new System.Drawing.Size(90, 24);
            this.labelPumpTypeDisplay.TabIndex = 29;
            this.labelPumpTypeDisplay.Text = "No Pump";
            // 
            // labelPumpType
            // 
            this.labelPumpType.AutoSize = true;
            this.labelPumpType.Location = new System.Drawing.Point(23, 20);
            this.labelPumpType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPumpType.Name = "labelPumpType";
            this.labelPumpType.Size = new System.Drawing.Size(129, 24);
            this.labelPumpType.TabIndex = 28;
            this.labelPumpType.Text = "Pump Status:";
            // 
            // labelEnable
            // 
            this.labelEnable.AutoSize = true;
            this.labelEnable.Location = new System.Drawing.Point(271, 20);
            this.labelEnable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEnable.Name = "labelEnable";
            this.labelEnable.Size = new System.Drawing.Size(70, 24);
            this.labelEnable.TabIndex = 25;
            this.labelEnable.Text = "Enable";
            // 
            // labelPumpProtect
            // 
            this.labelPumpProtect.AutoSize = true;
            this.labelPumpProtect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.labelPumpProtect.Location = new System.Drawing.Point(234, 222);
            this.labelPumpProtect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPumpProtect.Name = "labelPumpProtect";
            this.labelPumpProtect.Size = new System.Drawing.Size(129, 24);
            this.labelPumpProtect.TabIndex = 18;
            this.labelPumpProtect.Text = "Pump Protect";
            this.labelPumpProtect.Visible = false;
            // 
            // labelProtectTimeUnits
            // 
            this.labelProtectTimeUnits.AutoSize = true;
            this.labelProtectTimeUnits.Location = new System.Drawing.Point(223, 204);
            this.labelProtectTimeUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProtectTimeUnits.Name = "labelProtectTimeUnits";
            this.labelProtectTimeUnits.Size = new System.Drawing.Size(79, 24);
            this.labelProtectTimeUnits.TabIndex = 9;
            this.labelProtectTimeUnits.Text = "Minutes";
            // 
            // numericUpDownProtectTime
            // 
            this.numericUpDownProtectTime.Location = new System.Drawing.Point(128, 202);
            this.numericUpDownProtectTime.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownProtectTime.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDownProtectTime.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownProtectTime.Name = "numericUpDownProtectTime";
            this.numericUpDownProtectTime.Size = new System.Drawing.Size(87, 32);
            this.numericUpDownProtectTime.TabIndex = 9;
            this.numericUpDownProtectTime.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // labelProtectTime
            // 
            this.labelProtectTime.Location = new System.Drawing.Point(20, 204);
            this.labelProtectTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProtectTime.Name = "labelProtectTime";
            this.labelProtectTime.Size = new System.Drawing.Size(100, 22);
            this.labelProtectTime.TabIndex = 7;
            this.labelProtectTime.Text = "Protect Time:";
            this.labelProtectTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBoxMotorCycles
            // 
            this.groupBoxMotorCycles.Controls.Add(this.labelMotorCycles);
            this.groupBoxMotorCycles.Controls.Add(this.numericUpDownMotorCycles);
            this.groupBoxMotorCycles.Controls.Add(this.labelCyclesUnits);
            this.groupBoxMotorCycles.Controls.Add(this.checkBoxMotorCycles);
            this.groupBoxMotorCycles.Location = new System.Drawing.Point(8, 146);
            this.groupBoxMotorCycles.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxMotorCycles.Name = "groupBoxMotorCycles";
            this.groupBoxMotorCycles.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxMotorCycles.Size = new System.Drawing.Size(323, 46);
            this.groupBoxMotorCycles.TabIndex = 32;
            this.groupBoxMotorCycles.TabStop = false;
            // 
            // labelMotorCycles
            // 
            this.labelMotorCycles.AutoSize = true;
            this.labelMotorCycles.Location = new System.Drawing.Point(24, 15);
            this.labelMotorCycles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMotorCycles.Name = "labelMotorCycles";
            this.labelMotorCycles.Size = new System.Drawing.Size(130, 24);
            this.labelMotorCycles.TabIndex = 19;
            this.labelMotorCycles.Text = "Motor Cycles:";
            // 
            // numericUpDownMotorCycles
            // 
            this.numericUpDownMotorCycles.Location = new System.Drawing.Point(119, 12);
            this.numericUpDownMotorCycles.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownMotorCycles.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMotorCycles.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownMotorCycles.Name = "numericUpDownMotorCycles";
            this.numericUpDownMotorCycles.Size = new System.Drawing.Size(87, 32);
            this.numericUpDownMotorCycles.TabIndex = 7;
            this.numericUpDownMotorCycles.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelCyclesUnits
            // 
            this.labelCyclesUnits.AutoSize = true;
            this.labelCyclesUnits.Location = new System.Drawing.Point(213, 15);
            this.labelCyclesUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCyclesUnits.Name = "labelCyclesUnits";
            this.labelCyclesUnits.Size = new System.Drawing.Size(66, 24);
            this.labelCyclesUnits.TabIndex = 21;
            this.labelCyclesUnits.Text = "Cycles";
            // 
            // checkBoxMotorCycles
            // 
            this.checkBoxMotorCycles.AutoSize = true;
            this.checkBoxMotorCycles.Location = new System.Drawing.Point(287, 14);
            this.checkBoxMotorCycles.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxMotorCycles.Name = "checkBoxMotorCycles";
            this.checkBoxMotorCycles.Size = new System.Drawing.Size(18, 17);
            this.checkBoxMotorCycles.TabIndex = 8;
            this.checkBoxMotorCycles.UseVisualStyleBackColor = true;
            // 
            // groupBoxPumpMode
            // 
            this.groupBoxPumpMode.Controls.Add(this.checkBoxAlarmOnly);
            this.groupBoxPumpMode.Controls.Add(this.buttonRestoreDefaults);
            this.groupBoxPumpMode.Controls.Add(this.groupBoxRelayCycles);
            this.groupBoxPumpMode.Controls.Add(this.groupBoxMotorTimeout);
            this.groupBoxPumpMode.Controls.Add(this.groupBoxMotorCycles);
            this.groupBoxPumpMode.Controls.Add(this.labelProtectTime);
            this.groupBoxPumpMode.Controls.Add(this.labelPumpTypeDisplay);
            this.groupBoxPumpMode.Controls.Add(this.numericUpDownProtectTime);
            this.groupBoxPumpMode.Controls.Add(this.labelPumpType);
            this.groupBoxPumpMode.Controls.Add(this.labelProtectTimeUnits);
            this.groupBoxPumpMode.Controls.Add(this.labelEnable);
            this.groupBoxPumpMode.Controls.Add(this.labelPumpProtect);
            this.groupBoxPumpMode.Controls.Add(this.checkBoxNeverReclose);
            this.groupBoxPumpMode.Controls.Add(this.buttonSend);
            this.groupBoxPumpMode.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxPumpMode.Location = new System.Drawing.Point(4, 4);
            this.groupBoxPumpMode.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxPumpMode.Name = "groupBoxPumpMode";
            this.groupBoxPumpMode.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxPumpMode.Size = new System.Drawing.Size(333, 360);
            this.groupBoxPumpMode.TabIndex = 15;
            this.groupBoxPumpMode.TabStop = false;
            this.groupBoxPumpMode.Text = "Pump Protect Mode";
            // 
            // checkBoxAlarmOnly
            // 
            this.checkBoxAlarmOnly.AutoSize = true;
            this.checkBoxAlarmOnly.Location = new System.Drawing.Point(224, 238);
            this.checkBoxAlarmOnly.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAlarmOnly.Name = "checkBoxAlarmOnly";
            this.checkBoxAlarmOnly.Size = new System.Drawing.Size(130, 28);
            this.checkBoxAlarmOnly.TabIndex = 33;
            this.checkBoxAlarmOnly.Text = "Alarm Only";
            this.checkBoxAlarmOnly.UseVisualStyleBackColor = true;
            this.checkBoxAlarmOnly.Visible = false;
            // 
            // ucPumpMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxPumpMode);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucPumpMode";
            this.Size = new System.Drawing.Size(341, 329);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCycleLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPumpTime)).EndInit();
            this.groupBoxRelayCycles.ResumeLayout(false);
            this.groupBoxRelayCycles.PerformLayout();
            this.groupBoxMotorTimeout.ResumeLayout(false);
            this.groupBoxMotorTimeout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMotorTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownProtectTime)).EndInit();
            this.groupBoxMotorCycles.ResumeLayout(false);
            this.groupBoxMotorCycles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMotorCycles)).EndInit();
            this.groupBoxPumpMode.ResumeLayout(false);
            this.groupBoxPumpMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownCycleLimit;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownPumpTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonRestoreDefaults;
        private System.Windows.Forms.CheckBox checkBoxNeverReclose;
        private System.Windows.Forms.Label labelPumpProtect;
        private System.Windows.Forms.Label labelMotorCycles;
        private System.Windows.Forms.Label labelMotorTimeoutUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownMotorTimeout;
        private System.Windows.Forms.Label labelMotorTimeout;
        private System.Windows.Forms.Label labelCyclesUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownMotorCycles;
        private System.Windows.Forms.Label labelProtectTimeUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownProtectTime;
        private System.Windows.Forms.Label labelProtectTime;
        private System.Windows.Forms.CheckBox checkBoxMotorTime;
        private System.Windows.Forms.CheckBox checkBoxCycles;
        private System.Windows.Forms.Label labelEnable;
        private System.Windows.Forms.CheckBox checkBoxMotorCycles;
        private System.Windows.Forms.Label labelPumpType;
        private System.Windows.Forms.Label labelPumpTypeDisplay;
        private System.Windows.Forms.GroupBox groupBoxRelayCycles;
        private System.Windows.Forms.GroupBox groupBoxMotorTimeout;
        private System.Windows.Forms.GroupBox groupBoxMotorCycles;
        public System.Windows.Forms.GroupBox groupBoxPumpMode;
        private System.Windows.Forms.CheckBox checkBoxAlarmOnly;
    }
}
