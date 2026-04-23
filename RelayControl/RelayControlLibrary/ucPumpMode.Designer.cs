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
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonRestoreDefaults = new System.Windows.Forms.Button();
            this.checkBoxNeverReclose = new System.Windows.Forms.CheckBox();
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
            this.labelMotorCycles = new System.Windows.Forms.Label();
            this.numericUpDownMotorCycles = new System.Windows.Forms.NumericUpDown();
            this.labelCyclesUnits = new System.Windows.Forms.Label();
            this.checkBoxMotorCycles = new System.Windows.Forms.CheckBox();
            this.groupBoxPumpMode = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownPumpTime = new System.Windows.Forms.NumericUpDown();
            this.checkBoxAlarmOnly = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxCycles = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownCycleLimit = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMotorTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownProtectTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMotorCycles)).BeginInit();
            this.groupBoxPumpMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPumpTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCycleLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSend
            // 
            this.buttonSend.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSend.Location = new System.Drawing.Point(145, 255);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(70, 23);
            this.buttonSend.TabIndex = 13;
            this.buttonSend.Text = "Apply";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // buttonRestoreDefaults
            // 
            this.buttonRestoreDefaults.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestoreDefaults.Location = new System.Drawing.Point(30, 255);
            this.buttonRestoreDefaults.Name = "buttonRestoreDefaults";
            this.buttonRestoreDefaults.Size = new System.Drawing.Size(100, 23);
            this.buttonRestoreDefaults.TabIndex = 1;
            this.buttonRestoreDefaults.Text = "Restore Defaults";
            this.buttonRestoreDefaults.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaults.Click += new System.EventHandler(this.buttonRestoreDefaults_Click);
            // 
            // checkBoxNeverReclose
            // 
            this.checkBoxNeverReclose.AutoSize = true;
            this.checkBoxNeverReclose.Location = new System.Drawing.Point(4, 209);
            this.checkBoxNeverReclose.Name = "checkBoxNeverReclose";
            this.checkBoxNeverReclose.Size = new System.Drawing.Size(127, 23);
            this.checkBoxNeverReclose.TabIndex = 10;
            this.checkBoxNeverReclose.Text = "Never Reclose";
            this.checkBoxNeverReclose.UseVisualStyleBackColor = true;
            this.checkBoxNeverReclose.CheckedChanged += new System.EventHandler(this.checkBoxNeverReclose_CheckedChanged);
            // 
            // labelMotorTimeout
            // 
            this.labelMotorTimeout.AutoSize = true;
            this.labelMotorTimeout.Location = new System.Drawing.Point(25, 112);
            this.labelMotorTimeout.Name = "labelMotorTimeout";
            this.labelMotorTimeout.Size = new System.Drawing.Size(120, 19);
            this.labelMotorTimeout.TabIndex = 22;
            this.labelMotorTimeout.Text = "Motor Timeout:";
            // 
            // numericUpDownMotorTimeout
            // 
            this.numericUpDownMotorTimeout.Location = new System.Drawing.Point(121, 110);
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
            this.numericUpDownMotorTimeout.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownMotorTimeout.TabIndex = 5;
            this.numericUpDownMotorTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownMotorTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labelMotorTimeoutUnits
            // 
            this.labelMotorTimeoutUnits.AutoSize = true;
            this.labelMotorTimeoutUnits.Location = new System.Drawing.Point(176, 112);
            this.labelMotorTimeoutUnits.Name = "labelMotorTimeoutUnits";
            this.labelMotorTimeoutUnits.Size = new System.Drawing.Size(67, 19);
            this.labelMotorTimeoutUnits.TabIndex = 24;
            this.labelMotorTimeoutUnits.Text = "Seconds";
            // 
            // checkBoxMotorTime
            // 
            this.checkBoxMotorTime.AutoSize = true;
            this.checkBoxMotorTime.Location = new System.Drawing.Point(239, 115);
            this.checkBoxMotorTime.Name = "checkBoxMotorTime";
            this.checkBoxMotorTime.Size = new System.Drawing.Size(15, 14);
            this.checkBoxMotorTime.TabIndex = 6;
            this.checkBoxMotorTime.UseVisualStyleBackColor = true;
            // 
            // labelPumpTypeDisplay
            // 
            this.labelPumpTypeDisplay.AutoSize = true;
            this.labelPumpTypeDisplay.Location = new System.Drawing.Point(75, 23);
            this.labelPumpTypeDisplay.Name = "labelPumpTypeDisplay";
            this.labelPumpTypeDisplay.Size = new System.Drawing.Size(75, 19);
            this.labelPumpTypeDisplay.TabIndex = 29;
            this.labelPumpTypeDisplay.Text = "No Pump";
            // 
            // labelPumpType
            // 
            this.labelPumpType.AutoSize = true;
            this.labelPumpType.Location = new System.Drawing.Point(0, 23);
            this.labelPumpType.Name = "labelPumpType";
            this.labelPumpType.Size = new System.Drawing.Size(104, 19);
            this.labelPumpType.TabIndex = 28;
            this.labelPumpType.Text = "Pump Status:";
            // 
            // labelEnable
            // 
            this.labelEnable.AutoSize = true;
            this.labelEnable.Location = new System.Drawing.Point(210, 23);
            this.labelEnable.Name = "labelEnable";
            this.labelEnable.Size = new System.Drawing.Size(56, 19);
            this.labelEnable.TabIndex = 25;
            this.labelEnable.Text = "Enable";
            // 
            // labelPumpProtect
            // 
            this.labelPumpProtect.AutoSize = true;
            this.labelPumpProtect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.labelPumpProtect.Location = new System.Drawing.Point(5, 198);
            this.labelPumpProtect.Name = "labelPumpProtect";
            this.labelPumpProtect.Size = new System.Drawing.Size(104, 19);
            this.labelPumpProtect.TabIndex = 18;
            this.labelPumpProtect.Text = "Pump Protect";
            this.labelPumpProtect.Visible = false;
            // 
            // labelProtectTimeUnits
            // 
            this.labelProtectTimeUnits.AutoSize = true;
            this.labelProtectTimeUnits.Location = new System.Drawing.Point(176, 179);
            this.labelProtectTimeUnits.Name = "labelProtectTimeUnits";
            this.labelProtectTimeUnits.Size = new System.Drawing.Size(63, 19);
            this.labelProtectTimeUnits.TabIndex = 9;
            this.labelProtectTimeUnits.Text = "Minutes";
            // 
            // numericUpDownProtectTime
            // 
            this.numericUpDownProtectTime.Location = new System.Drawing.Point(121, 176);
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
            this.numericUpDownProtectTime.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownProtectTime.TabIndex = 9;
            this.numericUpDownProtectTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownProtectTime.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // labelProtectTime
            // 
            this.labelProtectTime.Location = new System.Drawing.Point(18, 180);
            this.labelProtectTime.Name = "labelProtectTime";
            this.labelProtectTime.Size = new System.Drawing.Size(97, 18);
            this.labelProtectTime.TabIndex = 7;
            this.labelProtectTime.Text = "Protect Time:";
            this.labelProtectTime.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // labelMotorCycles
            // 
            this.labelMotorCycles.AutoSize = true;
            this.labelMotorCycles.Location = new System.Drawing.Point(35, 145);
            this.labelMotorCycles.Name = "labelMotorCycles";
            this.labelMotorCycles.Size = new System.Drawing.Size(105, 19);
            this.labelMotorCycles.TabIndex = 19;
            this.labelMotorCycles.Text = "Motor Cycles:";
            // 
            // numericUpDownMotorCycles
            // 
            this.numericUpDownMotorCycles.Location = new System.Drawing.Point(121, 143);
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
            this.numericUpDownMotorCycles.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownMotorCycles.TabIndex = 7;
            this.numericUpDownMotorCycles.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownMotorCycles.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelCyclesUnits
            // 
            this.labelCyclesUnits.AutoSize = true;
            this.labelCyclesUnits.Location = new System.Drawing.Point(176, 151);
            this.labelCyclesUnits.Name = "labelCyclesUnits";
            this.labelCyclesUnits.Size = new System.Drawing.Size(53, 19);
            this.labelCyclesUnits.TabIndex = 21;
            this.labelCyclesUnits.Text = "Cycles";
            // 
            // checkBoxMotorCycles
            // 
            this.checkBoxMotorCycles.AutoSize = true;
            this.checkBoxMotorCycles.Location = new System.Drawing.Point(239, 154);
            this.checkBoxMotorCycles.Name = "checkBoxMotorCycles";
            this.checkBoxMotorCycles.Size = new System.Drawing.Size(15, 14);
            this.checkBoxMotorCycles.TabIndex = 8;
            this.checkBoxMotorCycles.UseVisualStyleBackColor = true;
            // 
            // groupBoxPumpMode
            // 
            this.groupBoxPumpMode.Controls.Add(this.checkBoxMotorCycles);
            this.groupBoxPumpMode.Controls.Add(this.labelCyclesUnits);
            this.groupBoxPumpMode.Controls.Add(this.numericUpDownMotorCycles);
            this.groupBoxPumpMode.Controls.Add(this.labelMotorCycles);
            this.groupBoxPumpMode.Controls.Add(this.checkBoxMotorTime);
            this.groupBoxPumpMode.Controls.Add(this.labelMotorTimeoutUnits);
            this.groupBoxPumpMode.Controls.Add(this.numericUpDownMotorTimeout);
            this.groupBoxPumpMode.Controls.Add(this.labelMotorTimeout);
            this.groupBoxPumpMode.Controls.Add(this.label3);
            this.groupBoxPumpMode.Controls.Add(this.label1);
            this.groupBoxPumpMode.Controls.Add(this.numericUpDownPumpTime);
            this.groupBoxPumpMode.Controls.Add(this.checkBoxAlarmOnly);
            this.groupBoxPumpMode.Controls.Add(this.label4);
            this.groupBoxPumpMode.Controls.Add(this.buttonRestoreDefaults);
            this.groupBoxPumpMode.Controls.Add(this.checkBoxCycles);
            this.groupBoxPumpMode.Controls.Add(this.label2);
            this.groupBoxPumpMode.Controls.Add(this.numericUpDownCycleLimit);
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
            this.groupBoxPumpMode.Location = new System.Drawing.Point(3, 3);
            this.groupBoxPumpMode.Name = "groupBoxPumpMode";
            this.groupBoxPumpMode.Size = new System.Drawing.Size(253, 292);
            this.groupBoxPumpMode.TabIndex = 15;
            this.groupBoxPumpMode.TabStop = false;
            this.groupBoxPumpMode.Text = "Pump Protect Mode";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "Seconds";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(46, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cycle Limit:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numericUpDownPumpTime
            // 
            this.numericUpDownPumpTime.Location = new System.Drawing.Point(121, 78);
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
            this.numericUpDownPumpTime.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownPumpTime.TabIndex = 4;
            this.numericUpDownPumpTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownPumpTime.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // checkBoxAlarmOnly
            // 
            this.checkBoxAlarmOnly.AutoSize = true;
            this.checkBoxAlarmOnly.Location = new System.Drawing.Point(144, 209);
            this.checkBoxAlarmOnly.Name = "checkBoxAlarmOnly";
            this.checkBoxAlarmOnly.Size = new System.Drawing.Size(109, 23);
            this.checkBoxAlarmOnly.TabIndex = 33;
            this.checkBoxAlarmOnly.Text = "Alarm Only";
            this.checkBoxAlarmOnly.UseVisualStyleBackColor = true;
            this.checkBoxAlarmOnly.Visible = false;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(46, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "Pump Time:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // checkBoxCycles
            // 
            this.checkBoxCycles.AutoSize = true;
            this.checkBoxCycles.Location = new System.Drawing.Point(238, 53);
            this.checkBoxCycles.Name = "checkBoxCycles";
            this.checkBoxCycles.Size = new System.Drawing.Size(15, 14);
            this.checkBoxCycles.TabIndex = 26;
            this.checkBoxCycles.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cycles";
            // 
            // numericUpDownCycleLimit
            // 
            this.numericUpDownCycleLimit.Location = new System.Drawing.Point(121, 45);
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
            this.numericUpDownCycleLimit.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownCycleLimit.TabIndex = 2;
            this.numericUpDownCycleLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownCycleLimit.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // ucPumpMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxPumpMode);
            this.Name = "ucPumpMode";
            this.Size = new System.Drawing.Size(256, 267);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMotorTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownProtectTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMotorCycles)).EndInit();
            this.groupBoxPumpMode.ResumeLayout(false);
            this.groupBoxPumpMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPumpTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCycleLimit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Button buttonSend;
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
        private System.Windows.Forms.Label labelEnable;
        private System.Windows.Forms.CheckBox checkBoxMotorCycles;
        public System.Windows.Forms.Label labelPumpType;
        public System.Windows.Forms.Label labelPumpTypeDisplay;
        public System.Windows.Forms.GroupBox groupBoxPumpMode;
        private System.Windows.Forms.CheckBox checkBoxAlarmOnly;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownPumpTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxCycles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownCycleLimit;
    }
}
