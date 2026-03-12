namespace RelayControlLibrary
{
    partial class ucCloseMode
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
            this.numericUpDownTimeDelay = new System.Windows.Forms.NumericUpDown();
            this.labelTD = new System.Windows.Forms.Label();
            this.numericUpDownRecloseVolts = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPDA = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPDV = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCloseTiltAngle = new System.Windows.Forms.NumericUpDown();
            this.labelReclose = new System.Windows.Forms.Label();
            this.labelPDA = new System.Windows.Forms.Label();
            this.labelPhaseDetectOffsetVolts = new System.Windows.Forms.Label();
            this.labelTiltAngle = new System.Windows.Forms.Label();
            this.labelTDUnit = new System.Windows.Forms.Label();
            this.labelRecloseUnit = new System.Windows.Forms.Label();
            this.labelPDAUnit = new System.Windows.Forms.Label();
            this.labelPDVUnit = new System.Windows.Forms.Label();
            this.labelTiltAngleUnit = new System.Windows.Forms.Label();
            this.buttonSendCloseData = new System.Windows.Forms.Button();
            this.checkBoxCircleClose = new System.Windows.Forms.CheckBox();
            this.buttonRestoreDefaults = new System.Windows.Forms.Button();
            this.labelCircleCloseVolts = new System.Windows.Forms.Label();
            this.buttonRelaxClose = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBoxCloseMode = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecloseVolts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPDA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPDV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCloseTiltAngle)).BeginInit();
            this.groupBoxCloseMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDownTimeDelay
            // 
            this.numericUpDownTimeDelay.Location = new System.Drawing.Point(157, 37);
            this.numericUpDownTimeDelay.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numericUpDownTimeDelay.Name = "numericUpDownTimeDelay";
            this.numericUpDownTimeDelay.Size = new System.Drawing.Size(64, 23);
            this.numericUpDownTimeDelay.TabIndex = 13;
            this.numericUpDownTimeDelay.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // labelTD
            // 
            this.labelTD.AutoSize = true;
            this.labelTD.Location = new System.Drawing.Point(59, 39);
            this.labelTD.Name = "labelTD";
            this.labelTD.Size = new System.Drawing.Size(111, 16);
            this.labelTD.TabIndex = 14;
            this.labelTD.Text = "Close Time Delay:";
            // 
            // numericUpDownRecloseVolts
            // 
            this.numericUpDownRecloseVolts.DecimalPlaces = 1;
            this.numericUpDownRecloseVolts.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownRecloseVolts.Location = new System.Drawing.Point(157, 61);
            this.numericUpDownRecloseVolts.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownRecloseVolts.Name = "numericUpDownRecloseVolts";
            this.numericUpDownRecloseVolts.Size = new System.Drawing.Size(64, 23);
            this.numericUpDownRecloseVolts.TabIndex = 15;
            this.numericUpDownRecloseVolts.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // numericUpDownPDA
            // 
            this.numericUpDownPDA.Location = new System.Drawing.Point(157, 85);
            this.numericUpDownPDA.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownPDA.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            -2147483648});
            this.numericUpDownPDA.Name = "numericUpDownPDA";
            this.numericUpDownPDA.Size = new System.Drawing.Size(64, 23);
            this.numericUpDownPDA.TabIndex = 16;
            this.numericUpDownPDA.Value = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            // 
            // numericUpDownPDV
            // 
            this.numericUpDownPDV.DecimalPlaces = 1;
            this.numericUpDownPDV.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownPDV.Location = new System.Drawing.Point(157, 109);
            this.numericUpDownPDV.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.numericUpDownPDV.Name = "numericUpDownPDV";
            this.numericUpDownPDV.Size = new System.Drawing.Size(64, 23);
            this.numericUpDownPDV.TabIndex = 17;
            this.numericUpDownPDV.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            // 
            // numericUpDownCloseTiltAngle
            // 
            this.numericUpDownCloseTiltAngle.Location = new System.Drawing.Point(156, 13);
            this.numericUpDownCloseTiltAngle.Maximum = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.numericUpDownCloseTiltAngle.Minimum = new decimal(new int[] {
            85,
            0,
            0,
            0});
            this.numericUpDownCloseTiltAngle.Name = "numericUpDownCloseTiltAngle";
            this.numericUpDownCloseTiltAngle.Size = new System.Drawing.Size(64, 23);
            this.numericUpDownCloseTiltAngle.TabIndex = 18;
            this.numericUpDownCloseTiltAngle.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            // 
            // labelReclose
            // 
            this.labelReclose.AutoSize = true;
            this.labelReclose.Location = new System.Drawing.Point(71, 63);
            this.labelReclose.Name = "labelReclose";
            this.labelReclose.Size = new System.Drawing.Size(88, 16);
            this.labelReclose.TabIndex = 19;
            this.labelReclose.Text = "Reclose Volts:";
            // 
            // labelPDA
            // 
            this.labelPDA.AutoSize = true;
            this.labelPDA.Location = new System.Drawing.Point(32, 87);
            this.labelPDA.Name = "labelPDA";
            this.labelPDA.Size = new System.Drawing.Size(139, 16);
            this.labelPDA.TabIndex = 20;
            this.labelPDA.Text = "Phase Detection Angle:";
            // 
            // labelPhaseDetectOffsetVolts
            // 
            this.labelPhaseDetectOffsetVolts.AutoSize = true;
            this.labelPhaseDetectOffsetVolts.Location = new System.Drawing.Point(31, 111);
            this.labelPhaseDetectOffsetVolts.Name = "labelPhaseDetectOffsetVolts";
            this.labelPhaseDetectOffsetVolts.Size = new System.Drawing.Size(141, 16);
            this.labelPhaseDetectOffsetVolts.TabIndex = 21;
            this.labelPhaseDetectOffsetVolts.Text = "Phase Detection Offset:";
            // 
            // labelTiltAngle
            // 
            this.labelTiltAngle.AutoSize = true;
            this.labelTiltAngle.Location = new System.Drawing.Point(94, 16);
            this.labelTiltAngle.Name = "labelTiltAngle";
            this.labelTiltAngle.Size = new System.Drawing.Size(66, 16);
            this.labelTiltAngle.TabIndex = 22;
            this.labelTiltAngle.Text = "Tilt Angle:";
            // 
            // labelTDUnit
            // 
            this.labelTDUnit.AutoSize = true;
            this.labelTDUnit.Location = new System.Drawing.Point(227, 41);
            this.labelTDUnit.Name = "labelTDUnit";
            this.labelTDUnit.Size = new System.Drawing.Size(41, 16);
            this.labelTDUnit.TabIndex = 23;
            this.labelTDUnit.Text = "cycles";
            // 
            // labelRecloseUnit
            // 
            this.labelRecloseUnit.AutoSize = true;
            this.labelRecloseUnit.Location = new System.Drawing.Point(227, 65);
            this.labelRecloseUnit.Name = "labelRecloseUnit";
            this.labelRecloseUnit.Size = new System.Drawing.Size(15, 16);
            this.labelRecloseUnit.TabIndex = 24;
            this.labelRecloseUnit.Text = "V";
            // 
            // labelPDAUnit
            // 
            this.labelPDAUnit.AutoSize = true;
            this.labelPDAUnit.Location = new System.Drawing.Point(227, 89);
            this.labelPDAUnit.Name = "labelPDAUnit";
            this.labelPDAUnit.Size = new System.Drawing.Size(54, 16);
            this.labelPDAUnit.TabIndex = 25;
            this.labelPDAUnit.Text = "Degrees";
            // 
            // labelPDVUnit
            // 
            this.labelPDVUnit.AutoSize = true;
            this.labelPDVUnit.Location = new System.Drawing.Point(227, 113);
            this.labelPDVUnit.Name = "labelPDVUnit";
            this.labelPDVUnit.Size = new System.Drawing.Size(15, 16);
            this.labelPDVUnit.TabIndex = 26;
            this.labelPDVUnit.Text = "V";
            // 
            // labelTiltAngleUnit
            // 
            this.labelTiltAngleUnit.AutoSize = true;
            this.labelTiltAngleUnit.Location = new System.Drawing.Point(227, 20);
            this.labelTiltAngleUnit.Name = "labelTiltAngleUnit";
            this.labelTiltAngleUnit.Size = new System.Drawing.Size(54, 16);
            this.labelTiltAngleUnit.TabIndex = 27;
            this.labelTiltAngleUnit.Text = "Degrees";
            // 
            // buttonSendCloseData
            // 
            this.buttonSendCloseData.Location = new System.Drawing.Point(180, 209);
            this.buttonSendCloseData.Name = "buttonSendCloseData";
            this.buttonSendCloseData.Size = new System.Drawing.Size(84, 23);
            this.buttonSendCloseData.TabIndex = 28;
            this.buttonSendCloseData.Text = "Program Close";
            this.buttonSendCloseData.UseVisualStyleBackColor = true;
            this.buttonSendCloseData.Click += new System.EventHandler(this.buttonSendCloseData_Click);
            // 
            // checkBoxCircleClose
            // 
            this.checkBoxCircleClose.AutoSize = true;
            this.checkBoxCircleClose.Location = new System.Drawing.Point(6, 183);
            this.checkBoxCircleClose.Name = "checkBoxCircleClose";
            this.checkBoxCircleClose.Size = new System.Drawing.Size(93, 20);
            this.checkBoxCircleClose.TabIndex = 29;
            this.checkBoxCircleClose.Text = "Circle Close";
            this.checkBoxCircleClose.UseVisualStyleBackColor = true;
            this.checkBoxCircleClose.CheckedChanged += new System.EventHandler(this.checkBoxCircleClose_CheckedChanged);
            // 
            // buttonRestoreDefaults
            // 
            this.buttonRestoreDefaults.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestoreDefaults.Location = new System.Drawing.Point(3, 209);
            this.buttonRestoreDefaults.Name = "buttonRestoreDefaults";
            this.buttonRestoreDefaults.Size = new System.Drawing.Size(171, 23);
            this.buttonRestoreDefaults.TabIndex = 30;
            this.buttonRestoreDefaults.Text = "Restore Factory Defaults";
            this.buttonRestoreDefaults.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaults.Click += new System.EventHandler(this.buttonRestoreDefaults_Click);
            // 
            // labelCircleCloseVolts
            // 
            this.labelCircleCloseVolts.AutoSize = true;
            this.labelCircleCloseVolts.Location = new System.Drawing.Point(57, 63);
            this.labelCircleCloseVolts.Name = "labelCircleCloseVolts";
            this.labelCircleCloseVolts.Size = new System.Drawing.Size(111, 16);
            this.labelCircleCloseVolts.TabIndex = 31;
            this.labelCircleCloseVolts.Text = "Circle Close Volts:";
            this.labelCircleCloseVolts.Visible = false;
            // 
            // buttonRelaxClose
            // 
            this.buttonRelaxClose.Location = new System.Drawing.Point(184, 236);
            this.buttonRelaxClose.Name = "buttonRelaxClose";
            this.buttonRelaxClose.Size = new System.Drawing.Size(92, 23);
            this.buttonRelaxClose.TabIndex = 39;
            this.buttonRelaxClose.Text = "Relax Close";
            this.buttonRelaxClose.UseVisualStyleBackColor = true;
            this.buttonRelaxClose.Click += new System.EventHandler(this.buttonRelaxClose_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 160);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(260, 20);
            this.checkBox1.TabIndex = 40;
            this.checkBox1.Text = "Override Blocked Open On Dead Network";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBoxCloseMode
            // 
            this.groupBoxCloseMode.Controls.Add(this.checkBox1);
            this.groupBoxCloseMode.Controls.Add(this.buttonRestoreDefaults);
            this.groupBoxCloseMode.Controls.Add(this.labelTiltAngle);
            this.groupBoxCloseMode.Controls.Add(this.checkBoxCircleClose);
            this.groupBoxCloseMode.Controls.Add(this.labelPhaseDetectOffsetVolts);
            this.groupBoxCloseMode.Controls.Add(this.labelCircleCloseVolts);
            this.groupBoxCloseMode.Controls.Add(this.labelTDUnit);
            this.groupBoxCloseMode.Controls.Add(this.numericUpDownTimeDelay);
            this.groupBoxCloseMode.Controls.Add(this.labelPDA);
            this.groupBoxCloseMode.Controls.Add(this.labelRecloseUnit);
            this.groupBoxCloseMode.Controls.Add(this.labelTD);
            this.groupBoxCloseMode.Controls.Add(this.labelReclose);
            this.groupBoxCloseMode.Controls.Add(this.numericUpDownRecloseVolts);
            this.groupBoxCloseMode.Controls.Add(this.labelPDAUnit);
            this.groupBoxCloseMode.Controls.Add(this.buttonSendCloseData);
            this.groupBoxCloseMode.Controls.Add(this.numericUpDownCloseTiltAngle);
            this.groupBoxCloseMode.Controls.Add(this.numericUpDownPDA);
            this.groupBoxCloseMode.Controls.Add(this.labelPDVUnit);
            this.groupBoxCloseMode.Controls.Add(this.labelTiltAngleUnit);
            this.groupBoxCloseMode.Controls.Add(this.numericUpDownPDV);
            this.groupBoxCloseMode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCloseMode.Location = new System.Drawing.Point(3, 3);
            this.groupBoxCloseMode.Name = "groupBoxCloseMode";
            this.groupBoxCloseMode.Size = new System.Drawing.Size(277, 260);
            this.groupBoxCloseMode.TabIndex = 33;
            this.groupBoxCloseMode.TabStop = false;
            this.groupBoxCloseMode.Text = "Close Mode Settings";
            // 
            // ucCloseMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCloseMode);
            this.Name = "ucCloseMode";
            this.Size = new System.Drawing.Size(283, 250);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecloseVolts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPDA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPDV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCloseTiltAngle)).EndInit();
            this.groupBoxCloseMode.ResumeLayout(false);
            this.groupBoxCloseMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownTimeDelay;
        private System.Windows.Forms.Label labelTD;
        private System.Windows.Forms.NumericUpDown numericUpDownRecloseVolts;
        private System.Windows.Forms.NumericUpDown numericUpDownPDA;
        private System.Windows.Forms.NumericUpDown numericUpDownPDV;
        private System.Windows.Forms.NumericUpDown numericUpDownCloseTiltAngle;
        private System.Windows.Forms.Label labelReclose;
        private System.Windows.Forms.Label labelPDA;
        private System.Windows.Forms.Label labelPhaseDetectOffsetVolts;
        private System.Windows.Forms.Label labelTiltAngle;
        private System.Windows.Forms.Label labelTDUnit;
        private System.Windows.Forms.Label labelRecloseUnit;
        private System.Windows.Forms.Label labelPDAUnit;
        private System.Windows.Forms.Label labelPDVUnit;
        private System.Windows.Forms.Label labelTiltAngleUnit;
        private System.Windows.Forms.Button buttonSendCloseData;
        private System.Windows.Forms.CheckBox checkBoxCircleClose;
        private System.Windows.Forms.Button buttonRestoreDefaults;
        private System.Windows.Forms.Label labelCircleCloseVolts;
        //private System.Windows.Forms.RadioButton radioButtonNeverOverride;
        //private System.Windows.Forms.RadioButton radioButtonOverrideBlockedOpen;
        private System.Windows.Forms.Button buttonRelaxClose;
        public System.Windows.Forms.GroupBox groupBoxCloseMode;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
