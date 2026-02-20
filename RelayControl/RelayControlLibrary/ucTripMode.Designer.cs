namespace RelayControlLibrary
{
    partial class ucTripMode
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
            this.listBoxTripModes = new System.Windows.Forms.ListBox();
            this.buttonSendTripData = new System.Windows.Forms.Button();
            this.numericUpDownSensitiveTimeDelay = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTimeDelay = new System.Windows.Forms.NumericUpDown();
            this.labelSTD = new System.Windows.Forms.Label();
            this.labelTD = new System.Windows.Forms.Label();
            this.labelETD = new System.Windows.Forms.Label();
            this.numericUpDownExtendedTimeDelay = new System.Windows.Forms.NumericUpDown();
            this.labelSTDunit = new System.Windows.Forms.Label();
            this.labelTDunit = new System.Windows.Forms.Label();
            this.labelETDunit = new System.Windows.Forms.Label();
            this.numericUpDownAngle = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSensTrip = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownInsensTrip = new System.Windows.Forms.NumericUpDown();
            this.labelInsensTrip = new System.Windows.Forms.Label();
            this.labelAngle = new System.Windows.Forms.Label();
            this.labelSensTrip = new System.Windows.Forms.Label();
            this.labelInsensTripUnit = new System.Windows.Forms.Label();
            this.labelAngleUnit = new System.Windows.Forms.Label();
            this.labelSensTripUnit = new System.Windows.Forms.Label();
            this.domainUpDownType = new System.Windows.Forms.DomainUpDown();
            this.labelWVCurrentUnit = new System.Windows.Forms.Label();
            this.labelWVCurrent = new System.Windows.Forms.Label();
            this.numericUpDownWVCurrent = new System.Windows.Forms.NumericUpDown();
            this.labelWVAngleUnit = new System.Windows.Forms.Label();
            this.labelWVAngle = new System.Windows.Forms.Label();
            this.numericUpDownWVAngle = new System.Windows.Forms.NumericUpDown();
            this.labelInstantCurrent = new System.Windows.Forms.Label();
            this.buttonRestoreDefaults = new System.Windows.Forms.Button();
            this.labelGullWingUnits = new System.Windows.Forms.Label();
            this.labelGullWingAngle = new System.Windows.Forms.Label();
            this.numericUpDownGullWingAngle = new System.Windows.Forms.NumericUpDown();
            this.checkBoxEnableGullWing = new System.Windows.Forms.CheckBox();
            this.checkBoxTripOnPowerDown = new System.Windows.Forms.CheckBox();
            this.domainUpDownTripStyle = new System.Windows.Forms.DomainUpDown();
            this.labelTripStyle = new System.Windows.Forms.Label();
            this.groupBoxTripModeSettings = new System.Windows.Forms.GroupBox();
            this.comboBox_TripType = new System.Windows.Forms.ComboBox();
            this.comboBox_TripStyle = new System.Windows.Forms.ComboBox();
            this.lblUnitInCur_kVARdir = new System.Windows.Forms.Label();
            this.lblUnitInCur_kWdir = new System.Windows.Forms.Label();
            this.lblUnitGreenMagY = new System.Windows.Forms.Label();
            this.lblUnitGreenMagX = new System.Windows.Forms.Label();
            this.lblUnitGreenDelay = new System.Windows.Forms.Label();
            this.numericUpDown_InCurrkW = new System.Windows.Forms.NumericUpDown();
            this.lbl_InstCurrent_kWdirection = new System.Windows.Forms.Label();
            this.lblGreenDelay = new System.Windows.Forms.Label();
            this.numericUpDown_GreenDelay = new System.Windows.Forms.NumericUpDown();
            this.lblGreenMagX = new System.Windows.Forms.Label();
            this.numericUpDown_GreenMagX = new System.Windows.Forms.NumericUpDown();
            this.lblGreenMagY = new System.Windows.Forms.Label();
            this.numericUpDown_GreenMagY = new System.Windows.Forms.NumericUpDown();
            this.lbl_InstCurrent_kVARdirection = new System.Windows.Forms.Label();
            this.numericUpDown_InCurrkVAR = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSensitiveTimeDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExtendedTimeDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSensTrip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInsensTrip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWVCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWVAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGullWingAngle)).BeginInit();
            this.groupBoxTripModeSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_InCurrkW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenMagX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenMagY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_InCurrkVAR)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxTripModes
            // 
            this.listBoxTripModes.FormattingEnabled = true;
            this.listBoxTripModes.ItemHeight = 16;
            this.listBoxTripModes.Items.AddRange(new object[] {
            "Sensitive",
            "Time Delay",
            "Insensitive",
            "Watt-Var",
            "Adaptive"});
            this.listBoxTripModes.Location = new System.Drawing.Point(7, 63);
            this.listBoxTripModes.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxTripModes.Name = "listBoxTripModes";
            this.listBoxTripModes.Size = new System.Drawing.Size(99, 84);
            this.listBoxTripModes.TabIndex = 2;
            this.listBoxTripModes.SelectedIndexChanged += new System.EventHandler(this.listBoxTripModes_SelectedIndexChanged);
            // 
            // buttonSendTripData
            // 
            this.buttonSendTripData.Location = new System.Drawing.Point(260, 278);
            this.buttonSendTripData.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSendTripData.Name = "buttonSendTripData";
            this.buttonSendTripData.Size = new System.Drawing.Size(100, 28);
            this.buttonSendTripData.TabIndex = 1;
            this.buttonSendTripData.Text = "Program Trip";
            this.buttonSendTripData.UseVisualStyleBackColor = true;
            this.buttonSendTripData.Click += new System.EventHandler(this.buttonSendTripMode_Click);
            // 
            // numericUpDownSensitiveTimeDelay
            // 
            this.numericUpDownSensitiveTimeDelay.Location = new System.Drawing.Point(263, 60);
            this.numericUpDownSensitiveTimeDelay.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownSensitiveTimeDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownSensitiveTimeDelay.Name = "numericUpDownSensitiveTimeDelay";
            this.numericUpDownSensitiveTimeDelay.Size = new System.Drawing.Size(72, 22);
            this.numericUpDownSensitiveTimeDelay.TabIndex = 3;
            this.numericUpDownSensitiveTimeDelay.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // numericUpDownTimeDelay
            // 
            this.numericUpDownTimeDelay.Location = new System.Drawing.Point(263, 87);
            this.numericUpDownTimeDelay.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownTimeDelay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownTimeDelay.Name = "numericUpDownTimeDelay";
            this.numericUpDownTimeDelay.Size = new System.Drawing.Size(72, 22);
            this.numericUpDownTimeDelay.TabIndex = 5;
            this.numericUpDownTimeDelay.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // labelSTD
            // 
            this.labelSTD.AutoSize = true;
            this.labelSTD.Location = new System.Drawing.Point(116, 63);
            this.labelSTD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSTD.Name = "labelSTD";
            this.labelSTD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelSTD.Size = new System.Drawing.Size(138, 16);
            this.labelSTD.TabIndex = 14;
            this.labelSTD.Text = "Sensitive Time Delay:";
            // 
            // labelTD
            // 
            this.labelTD.AutoSize = true;
            this.labelTD.Location = new System.Drawing.Point(177, 90);
            this.labelTD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTD.Name = "labelTD";
            this.labelTD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelTD.Size = new System.Drawing.Size(80, 16);
            this.labelTD.TabIndex = 15;
            this.labelTD.Text = "Time Delay:";
            // 
            // labelETD
            // 
            this.labelETD.AutoSize = true;
            this.labelETD.Location = new System.Drawing.Point(113, 63);
            this.labelETD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelETD.Name = "labelETD";
            this.labelETD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelETD.Size = new System.Drawing.Size(140, 16);
            this.labelETD.TabIndex = 17;
            this.labelETD.Text = "Extended Time Delay:";
            // 
            // numericUpDownExtendedTimeDelay
            // 
            this.numericUpDownExtendedTimeDelay.Location = new System.Drawing.Point(263, 60);
            this.numericUpDownExtendedTimeDelay.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownExtendedTimeDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownExtendedTimeDelay.Name = "numericUpDownExtendedTimeDelay";
            this.numericUpDownExtendedTimeDelay.Size = new System.Drawing.Size(72, 22);
            this.numericUpDownExtendedTimeDelay.TabIndex = 4;
            // 
            // labelSTDunit
            // 
            this.labelSTDunit.AutoSize = true;
            this.labelSTDunit.Location = new System.Drawing.Point(342, 63);
            this.labelSTDunit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSTDunit.Name = "labelSTDunit";
            this.labelSTDunit.Size = new System.Drawing.Size(48, 16);
            this.labelSTDunit.TabIndex = 19;
            this.labelSTDunit.Text = "Cycles";
            // 
            // labelTDunit
            // 
            this.labelTDunit.AutoSize = true;
            this.labelTDunit.Location = new System.Drawing.Point(353, 90);
            this.labelTDunit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTDunit.Name = "labelTDunit";
            this.labelTDunit.Size = new System.Drawing.Size(14, 16);
            this.labelTDunit.TabIndex = 20;
            this.labelTDunit.Text = "s";
            // 
            // labelETDunit
            // 
            this.labelETDunit.AutoSize = true;
            this.labelETDunit.Location = new System.Drawing.Point(353, 63);
            this.labelETDunit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelETDunit.Name = "labelETDunit";
            this.labelETDunit.Size = new System.Drawing.Size(14, 16);
            this.labelETDunit.TabIndex = 21;
            this.labelETDunit.Text = "s";
            // 
            // numericUpDownAngle
            // 
            this.numericUpDownAngle.Location = new System.Drawing.Point(263, 143);
            this.numericUpDownAngle.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownAngle.Maximum = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.numericUpDownAngle.Minimum = new decimal(new int[] {
            85,
            0,
            0,
            0});
            this.numericUpDownAngle.Name = "numericUpDownAngle";
            this.numericUpDownAngle.Size = new System.Drawing.Size(72, 22);
            this.numericUpDownAngle.TabIndex = 7;
            this.numericUpDownAngle.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // numericUpDownSensTrip
            // 
            this.numericUpDownSensTrip.DecimalPlaces = 1;
            this.numericUpDownSensTrip.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownSensTrip.Location = new System.Drawing.Point(263, 116);
            this.numericUpDownSensTrip.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownSensTrip.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownSensTrip.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownSensTrip.Name = "numericUpDownSensTrip";
            this.numericUpDownSensTrip.Size = new System.Drawing.Size(72, 22);
            this.numericUpDownSensTrip.TabIndex = 6;
            this.numericUpDownSensTrip.Value = new decimal(new int[] {
            75,
            0,
            0,
            65536});
            // 
            // numericUpDownInsensTrip
            // 
            this.numericUpDownInsensTrip.DecimalPlaces = 1;
            this.numericUpDownInsensTrip.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownInsensTrip.Location = new System.Drawing.Point(263, 170);
            this.numericUpDownInsensTrip.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownInsensTrip.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownInsensTrip.Name = "numericUpDownInsensTrip";
            this.numericUpDownInsensTrip.Size = new System.Drawing.Size(72, 22);
            this.numericUpDownInsensTrip.TabIndex = 8;
            this.numericUpDownInsensTrip.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            // 
            // labelInsensTrip
            // 
            this.labelInsensTrip.AutoSize = true;
            this.labelInsensTrip.Location = new System.Drawing.Point(128, 174);
            this.labelInsensTrip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInsensTrip.Name = "labelInsensTrip";
            this.labelInsensTrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelInsensTrip.Size = new System.Drawing.Size(123, 16);
            this.labelInsensTrip.TabIndex = 27;
            this.labelInsensTrip.Text = "Insensitive Trip (IT):";
            // 
            // labelAngle
            // 
            this.labelAngle.AutoSize = true;
            this.labelAngle.Location = new System.Drawing.Point(189, 145);
            this.labelAngle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAngle.Name = "labelAngle";
            this.labelAngle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelAngle.Size = new System.Drawing.Size(66, 16);
            this.labelAngle.TabIndex = 26;
            this.labelAngle.Text = "Tilt Angle:";
            // 
            // labelSensTrip
            // 
            this.labelSensTrip.AutoSize = true;
            this.labelSensTrip.Location = new System.Drawing.Point(163, 118);
            this.labelSensTrip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSensTrip.Name = "labelSensTrip";
            this.labelSensTrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelSensTrip.Size = new System.Drawing.Size(92, 16);
            this.labelSensTrip.TabIndex = 25;
            this.labelSensTrip.Text = "Sensitive Trip:";
            // 
            // labelInsensTripUnit
            // 
            this.labelInsensTripUnit.AutoSize = true;
            this.labelInsensTripUnit.Location = new System.Drawing.Point(349, 174);
            this.labelInsensTripUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInsensTripUnit.Name = "labelInsensTripUnit";
            this.labelInsensTripUnit.Size = new System.Drawing.Size(16, 16);
            this.labelInsensTripUnit.TabIndex = 30;
            this.labelInsensTripUnit.Text = "A";
            // 
            // labelAngleUnit
            // 
            this.labelAngleUnit.AutoSize = true;
            this.labelAngleUnit.Location = new System.Drawing.Point(349, 145);
            this.labelAngleUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAngleUnit.Name = "labelAngleUnit";
            this.labelAngleUnit.Size = new System.Drawing.Size(60, 16);
            this.labelAngleUnit.TabIndex = 29;
            this.labelAngleUnit.Text = "Degrees";
            // 
            // labelSensTripUnit
            // 
            this.labelSensTripUnit.AutoSize = true;
            this.labelSensTripUnit.Location = new System.Drawing.Point(349, 118);
            this.labelSensTripUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSensTripUnit.Name = "labelSensTripUnit";
            this.labelSensTripUnit.Size = new System.Drawing.Size(27, 16);
            this.labelSensTripUnit.TabIndex = 28;
            this.labelSensTripUnit.Text = "mA";
            // 
            // domainUpDownType
            // 
            this.domainUpDownType.Enabled = false;
            this.domainUpDownType.Items.Add("Relay");
            this.domainUpDownType.Items.Add("Percent");
            this.domainUpDownType.Items.Add("Protector");
            this.domainUpDownType.Location = new System.Drawing.Point(69, 187);
            this.domainUpDownType.Margin = new System.Windows.Forms.Padding(4);
            this.domainUpDownType.Name = "domainUpDownType";
            this.domainUpDownType.ReadOnly = true;
            this.domainUpDownType.Size = new System.Drawing.Size(104, 22);
            this.domainUpDownType.TabIndex = 31;
            this.domainUpDownType.Text = "Relay";
            this.domainUpDownType.Visible = false;
            // 
            // labelWVCurrentUnit
            // 
            this.labelWVCurrentUnit.AutoSize = true;
            this.labelWVCurrentUnit.Location = new System.Drawing.Point(349, 201);
            this.labelWVCurrentUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWVCurrentUnit.Name = "labelWVCurrentUnit";
            this.labelWVCurrentUnit.Size = new System.Drawing.Size(16, 16);
            this.labelWVCurrentUnit.TabIndex = 36;
            this.labelWVCurrentUnit.Text = "A";
            // 
            // labelWVCurrent
            // 
            this.labelWVCurrent.AutoSize = true;
            this.labelWVCurrent.Location = new System.Drawing.Point(175, 201);
            this.labelWVCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWVCurrent.Name = "labelWVCurrent";
            this.labelWVCurrent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelWVCurrent.Size = new System.Drawing.Size(77, 16);
            this.labelWVCurrent.TabIndex = 35;
            this.labelWVCurrent.Text = "WV Current:";
            // 
            // numericUpDownWVCurrent
            // 
            this.numericUpDownWVCurrent.DecimalPlaces = 1;
            this.numericUpDownWVCurrent.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownWVCurrent.Location = new System.Drawing.Point(263, 197);
            this.numericUpDownWVCurrent.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownWVCurrent.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownWVCurrent.Name = "numericUpDownWVCurrent";
            this.numericUpDownWVCurrent.Size = new System.Drawing.Size(72, 22);
            this.numericUpDownWVCurrent.TabIndex = 34;
            this.numericUpDownWVCurrent.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            // 
            // labelWVAngleUnit
            // 
            this.labelWVAngleUnit.AutoSize = true;
            this.labelWVAngleUnit.Location = new System.Drawing.Point(349, 225);
            this.labelWVAngleUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWVAngleUnit.Name = "labelWVAngleUnit";
            this.labelWVAngleUnit.Size = new System.Drawing.Size(60, 16);
            this.labelWVAngleUnit.TabIndex = 39;
            this.labelWVAngleUnit.Text = "Degrees";
            // 
            // labelWVAngle
            // 
            this.labelWVAngle.AutoSize = true;
            this.labelWVAngle.Location = new System.Drawing.Point(184, 225);
            this.labelWVAngle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWVAngle.Name = "labelWVAngle";
            this.labelWVAngle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelWVAngle.Size = new System.Drawing.Size(70, 16);
            this.labelWVAngle.TabIndex = 38;
            this.labelWVAngle.Text = "WV Angle:";
            // 
            // numericUpDownWVAngle
            // 
            this.numericUpDownWVAngle.Location = new System.Drawing.Point(263, 223);
            this.numericUpDownWVAngle.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownWVAngle.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDownWVAngle.Minimum = new decimal(new int[] {
            80,
            0,
            0,
            -2147483648});
            this.numericUpDownWVAngle.Name = "numericUpDownWVAngle";
            this.numericUpDownWVAngle.Size = new System.Drawing.Size(72, 22);
            this.numericUpDownWVAngle.TabIndex = 37;
            this.numericUpDownWVAngle.Value = new decimal(new int[] {
            60,
            0,
            0,
            -2147483648});
            // 
            // labelInstantCurrent
            // 
            this.labelInstantCurrent.AutoSize = true;
            this.labelInstantCurrent.Location = new System.Drawing.Point(84, 171);
            this.labelInstantCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInstantCurrent.Name = "labelInstantCurrent";
            this.labelInstantCurrent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelInstantCurrent.Size = new System.Drawing.Size(161, 16);
            this.labelInstantCurrent.TabIndex = 40;
            this.labelInstantCurrent.Text = "Instantaneous Current (IC):";
            // 
            // buttonRestoreDefaults
            // 
            this.buttonRestoreDefaults.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestoreDefaults.Location = new System.Drawing.Point(40, 277);
            this.buttonRestoreDefaults.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRestoreDefaults.Name = "buttonRestoreDefaults";
            this.buttonRestoreDefaults.Size = new System.Drawing.Size(167, 28);
            this.buttonRestoreDefaults.TabIndex = 41;
            this.buttonRestoreDefaults.Text = "Restore Factory Defaults";
            this.buttonRestoreDefaults.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaults.Click += new System.EventHandler(this.buttonRestoreDefaults_Click);
            // 
            // labelGullWingUnits
            // 
            this.labelGullWingUnits.AutoSize = true;
            this.labelGullWingUnits.Location = new System.Drawing.Point(349, 252);
            this.labelGullWingUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGullWingUnits.Name = "labelGullWingUnits";
            this.labelGullWingUnits.Size = new System.Drawing.Size(60, 16);
            this.labelGullWingUnits.TabIndex = 44;
            this.labelGullWingUnits.Text = "Degrees";
            // 
            // labelGullWingAngle
            // 
            this.labelGullWingAngle.AutoSize = true;
            this.labelGullWingAngle.Location = new System.Drawing.Point(181, 252);
            this.labelGullWingAngle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGullWingAngle.Name = "labelGullWingAngle";
            this.labelGullWingAngle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelGullWingAngle.Size = new System.Drawing.Size(75, 16);
            this.labelGullWingAngle.TabIndex = 43;
            this.labelGullWingAngle.Text = "Trim Angle:";
            // 
            // numericUpDownGullWingAngle
            // 
            this.numericUpDownGullWingAngle.Location = new System.Drawing.Point(263, 250);
            this.numericUpDownGullWingAngle.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownGullWingAngle.Maximum = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.numericUpDownGullWingAngle.Minimum = new decimal(new int[] {
            85,
            0,
            0,
            0});
            this.numericUpDownGullWingAngle.Name = "numericUpDownGullWingAngle";
            this.numericUpDownGullWingAngle.Size = new System.Drawing.Size(72, 22);
            this.numericUpDownGullWingAngle.TabIndex = 42;
            this.numericUpDownGullWingAngle.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // checkBoxEnableGullWing
            // 
            this.checkBoxEnableGullWing.AutoSize = true;
            this.checkBoxEnableGullWing.Location = new System.Drawing.Point(7, 225);
            this.checkBoxEnableGullWing.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxEnableGullWing.Name = "checkBoxEnableGullWing";
            this.checkBoxEnableGullWing.Size = new System.Drawing.Size(99, 20);
            this.checkBoxEnableGullWing.TabIndex = 45;
            this.checkBoxEnableGullWing.Text = "Enable Trim";
            this.checkBoxEnableGullWing.UseVisualStyleBackColor = true;
            this.checkBoxEnableGullWing.CheckedChanged += new System.EventHandler(this.checkBoxEnableGullWing_CheckedChanged);
            // 
            // checkBoxTripOnPowerDown
            // 
            this.checkBoxTripOnPowerDown.AutoSize = true;
            this.checkBoxTripOnPowerDown.Location = new System.Drawing.Point(7, 254);
            this.checkBoxTripOnPowerDown.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxTripOnPowerDown.Name = "checkBoxTripOnPowerDown";
            this.checkBoxTripOnPowerDown.Size = new System.Drawing.Size(148, 20);
            this.checkBoxTripOnPowerDown.TabIndex = 54;
            this.checkBoxTripOnPowerDown.Text = "Trip On Power Down";
            this.checkBoxTripOnPowerDown.UseVisualStyleBackColor = true;
            // 
            // domainUpDownTripStyle
            // 
            this.domainUpDownTripStyle.BackColor = System.Drawing.SystemColors.Window;
            this.domainUpDownTripStyle.Enabled = false;
            this.domainUpDownTripStyle.Items.Add("Hold Trip (Troubleshooting Only)");
            this.domainUpDownTripStyle.Items.Add("Continuous Pulse");
            this.domainUpDownTripStyle.Items.Add("3 Pulse, then off");
            this.domainUpDownTripStyle.Items.Add("Short Trip");
            this.domainUpDownTripStyle.Location = new System.Drawing.Point(55, 155);
            this.domainUpDownTripStyle.Margin = new System.Windows.Forms.Padding(4);
            this.domainUpDownTripStyle.Name = "domainUpDownTripStyle";
            this.domainUpDownTripStyle.ReadOnly = true;
            this.domainUpDownTripStyle.Size = new System.Drawing.Size(200, 22);
            this.domainUpDownTripStyle.TabIndex = 53;
            this.domainUpDownTripStyle.Text = "Hold Trip (Troubleshooting Only)";
            this.domainUpDownTripStyle.Visible = false;
            // 
            // labelTripStyle
            // 
            this.labelTripStyle.AutoSize = true;
            this.labelTripStyle.Location = new System.Drawing.Point(8, 28);
            this.labelTripStyle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTripStyle.Name = "labelTripStyle";
            this.labelTripStyle.Size = new System.Drawing.Size(67, 16);
            this.labelTripStyle.TabIndex = 52;
            this.labelTripStyle.Text = "Trip Style:";
            // 
            // groupBoxTripModeSettings
            // 
            this.groupBoxTripModeSettings.Controls.Add(this.comboBox_TripType);
            this.groupBoxTripModeSettings.Controls.Add(this.comboBox_TripStyle);
            this.groupBoxTripModeSettings.Controls.Add(this.lblUnitInCur_kVARdir);
            this.groupBoxTripModeSettings.Controls.Add(this.lblUnitInCur_kWdir);
            this.groupBoxTripModeSettings.Controls.Add(this.lblUnitGreenMagY);
            this.groupBoxTripModeSettings.Controls.Add(this.lblUnitGreenMagX);
            this.groupBoxTripModeSettings.Controls.Add(this.lblUnitGreenDelay);
            this.groupBoxTripModeSettings.Controls.Add(this.numericUpDown_InCurrkW);
            this.groupBoxTripModeSettings.Controls.Add(this.lbl_InstCurrent_kWdirection);
            this.groupBoxTripModeSettings.Controls.Add(this.checkBoxTripOnPowerDown);
            this.groupBoxTripModeSettings.Controls.Add(this.buttonRestoreDefaults);
            this.groupBoxTripModeSettings.Controls.Add(this.domainUpDownTripStyle);
            this.groupBoxTripModeSettings.Controls.Add(this.labelSensTripUnit);
            this.groupBoxTripModeSettings.Controls.Add(this.labelTripStyle);
            this.groupBoxTripModeSettings.Controls.Add(this.labelAngleUnit);
            this.groupBoxTripModeSettings.Controls.Add(this.buttonSendTripData);
            this.groupBoxTripModeSettings.Controls.Add(this.labelInsensTrip);
            this.groupBoxTripModeSettings.Controls.Add(this.checkBoxEnableGullWing);
            this.groupBoxTripModeSettings.Controls.Add(this.labelInsensTripUnit);
            this.groupBoxTripModeSettings.Controls.Add(this.listBoxTripModes);
            this.groupBoxTripModeSettings.Controls.Add(this.labelAngle);
            this.groupBoxTripModeSettings.Controls.Add(this.labelGullWingUnits);
            this.groupBoxTripModeSettings.Controls.Add(this.domainUpDownType);
            this.groupBoxTripModeSettings.Controls.Add(this.numericUpDownSensitiveTimeDelay);
            this.groupBoxTripModeSettings.Controls.Add(this.labelSensTrip);
            this.groupBoxTripModeSettings.Controls.Add(this.labelGullWingAngle);
            this.groupBoxTripModeSettings.Controls.Add(this.numericUpDownInsensTrip);
            this.groupBoxTripModeSettings.Controls.Add(this.numericUpDownTimeDelay);
            this.groupBoxTripModeSettings.Controls.Add(this.numericUpDownSensTrip);
            this.groupBoxTripModeSettings.Controls.Add(this.numericUpDownGullWingAngle);
            this.groupBoxTripModeSettings.Controls.Add(this.numericUpDownWVCurrent);
            this.groupBoxTripModeSettings.Controls.Add(this.labelSTD);
            this.groupBoxTripModeSettings.Controls.Add(this.numericUpDownAngle);
            this.groupBoxTripModeSettings.Controls.Add(this.labelWVCurrent);
            this.groupBoxTripModeSettings.Controls.Add(this.labelTD);
            this.groupBoxTripModeSettings.Controls.Add(this.labelETDunit);
            this.groupBoxTripModeSettings.Controls.Add(this.labelInstantCurrent);
            this.groupBoxTripModeSettings.Controls.Add(this.labelWVCurrentUnit);
            this.groupBoxTripModeSettings.Controls.Add(this.numericUpDownExtendedTimeDelay);
            this.groupBoxTripModeSettings.Controls.Add(this.labelTDunit);
            this.groupBoxTripModeSettings.Controls.Add(this.labelWVAngleUnit);
            this.groupBoxTripModeSettings.Controls.Add(this.numericUpDownWVAngle);
            this.groupBoxTripModeSettings.Controls.Add(this.labelETD);
            this.groupBoxTripModeSettings.Controls.Add(this.labelSTDunit);
            this.groupBoxTripModeSettings.Controls.Add(this.labelWVAngle);
            this.groupBoxTripModeSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxTripModeSettings.Location = new System.Drawing.Point(4, 4);
            this.groupBoxTripModeSettings.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxTripModeSettings.Name = "groupBoxTripModeSettings";
            this.groupBoxTripModeSettings.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxTripModeSettings.Size = new System.Drawing.Size(409, 313);
            this.groupBoxTripModeSettings.TabIndex = 47;
            this.groupBoxTripModeSettings.TabStop = false;
            this.groupBoxTripModeSettings.Text = "Trip Mode Settings:";
            // 
            // comboBox_TripType
            // 
            this.comboBox_TripType.FormattingEnabled = true;
            this.comboBox_TripType.Items.AddRange(new object[] {
            "Relay",
            "Percent",
            "Protector"});
            this.comboBox_TripType.Location = new System.Drawing.Point(293, 25);
            this.comboBox_TripType.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox_TripType.Name = "comboBox_TripType";
            this.comboBox_TripType.Size = new System.Drawing.Size(99, 24);
            this.comboBox_TripType.TabIndex = 69;
            this.comboBox_TripType.SelectedIndexChanged += new System.EventHandler(this.comboBox_TripType_SelectedItemChanged);
            // 
            // comboBox_TripStyle
            // 
            this.comboBox_TripStyle.FormattingEnabled = true;
            this.comboBox_TripStyle.Items.AddRange(new object[] {
            "Hold Trip (Troubleshooting Only)",
            "Continuous Pulse",
            "3 Pulse, then off",
            "Short Trip"});
            this.comboBox_TripStyle.Location = new System.Drawing.Point(80, 25);
            this.comboBox_TripStyle.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox_TripStyle.Name = "comboBox_TripStyle";
            this.comboBox_TripStyle.Size = new System.Drawing.Size(204, 24);
            this.comboBox_TripStyle.TabIndex = 68;
            this.comboBox_TripStyle.SelectedIndexChanged += new System.EventHandler(this.comboBox_TripStyle_SelectedItemChanged);
            // 
            // lblUnitInCur_kVARdir
            // 
            this.lblUnitInCur_kVARdir.AutoSize = true;
            this.lblUnitInCur_kVARdir.Location = new System.Drawing.Point(36, 201);
            this.lblUnitInCur_kVARdir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUnitInCur_kVARdir.Name = "lblUnitInCur_kVARdir";
            this.lblUnitInCur_kVARdir.Size = new System.Drawing.Size(19, 16);
            this.lblUnitInCur_kVARdir.TabIndex = 67;
            this.lblUnitInCur_kVARdir.Text = "%";
            // 
            // lblUnitInCur_kWdir
            // 
            this.lblUnitInCur_kWdir.AutoSize = true;
            this.lblUnitInCur_kWdir.Location = new System.Drawing.Point(8, 199);
            this.lblUnitInCur_kWdir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUnitInCur_kWdir.Name = "lblUnitInCur_kWdir";
            this.lblUnitInCur_kWdir.Size = new System.Drawing.Size(19, 16);
            this.lblUnitInCur_kWdir.TabIndex = 66;
            this.lblUnitInCur_kWdir.Text = "%";
            // 
            // lblUnitGreenMagY
            // 
            this.lblUnitGreenMagY.AutoSize = true;
            this.lblUnitGreenMagY.Location = new System.Drawing.Point(8, 183);
            this.lblUnitGreenMagY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUnitGreenMagY.Name = "lblUnitGreenMagY";
            this.lblUnitGreenMagY.Size = new System.Drawing.Size(19, 16);
            this.lblUnitGreenMagY.TabIndex = 65;
            this.lblUnitGreenMagY.Text = "%";
            // 
            // lblUnitGreenMagX
            // 
            this.lblUnitGreenMagX.AutoSize = true;
            this.lblUnitGreenMagX.Location = new System.Drawing.Point(16, 167);
            this.lblUnitGreenMagX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUnitGreenMagX.Name = "lblUnitGreenMagX";
            this.lblUnitGreenMagX.Size = new System.Drawing.Size(19, 16);
            this.lblUnitGreenMagX.TabIndex = 64;
            this.lblUnitGreenMagX.Text = "%";
            // 
            // lblUnitGreenDelay
            // 
            this.lblUnitGreenDelay.AutoSize = true;
            this.lblUnitGreenDelay.Location = new System.Drawing.Point(16, 151);
            this.lblUnitGreenDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUnitGreenDelay.Name = "lblUnitGreenDelay";
            this.lblUnitGreenDelay.Size = new System.Drawing.Size(27, 16);
            this.lblUnitGreenDelay.TabIndex = 63;
            this.lblUnitGreenDelay.Text = "mS";
            // 
            // numericUpDown_InCurrkW
            // 
            this.numericUpDown_InCurrkW.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown_InCurrkW.Location = new System.Drawing.Point(313, -2);
            this.numericUpDown_InCurrkW.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown_InCurrkW.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numericUpDown_InCurrkW.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown_InCurrkW.Name = "numericUpDown_InCurrkW";
            this.numericUpDown_InCurrkW.Size = new System.Drawing.Size(71, 22);
            this.numericUpDown_InCurrkW.TabIndex = 62;
            this.numericUpDown_InCurrkW.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // lbl_InstCurrent_kWdirection
            // 
            this.lbl_InstCurrent_kWdirection.AutoSize = true;
            this.lbl_InstCurrent_kWdirection.Location = new System.Drawing.Point(147, 0);
            this.lbl_InstCurrent_kWdirection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_InstCurrent_kWdirection.Name = "lbl_InstCurrent_kWdirection";
            this.lbl_InstCurrent_kWdirection.Size = new System.Drawing.Size(217, 16);
            this.lbl_InstCurrent_kWdirection.TabIndex = 61;
            this.lbl_InstCurrent_kWdirection.Text = "Instantaneous Current kW Direction:";
            // 
            // lblGreenDelay
            // 
            this.lblGreenDelay.AutoSize = true;
            this.lblGreenDelay.Location = new System.Drawing.Point(4, 320);
            this.lblGreenDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGreenDelay.Name = "lblGreenDelay";
            this.lblGreenDelay.Size = new System.Drawing.Size(103, 16);
            this.lblGreenDelay.TabIndex = 55;
            this.lblGreenDelay.Text = "Adaptive Delay:";
            // 
            // numericUpDown_GreenDelay
            // 
            this.numericUpDown_GreenDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_GreenDelay.Location = new System.Drawing.Point(107, 320);
            this.numericUpDown_GreenDelay.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown_GreenDelay.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown_GreenDelay.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_GreenDelay.Name = "numericUpDown_GreenDelay";
            this.numericUpDown_GreenDelay.Size = new System.Drawing.Size(71, 22);
            this.numericUpDown_GreenDelay.TabIndex = 56;
            this.numericUpDown_GreenDelay.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // lblGreenMagX
            // 
            this.lblGreenMagX.AutoSize = true;
            this.lblGreenMagX.Location = new System.Drawing.Point(193, 314);
            this.lblGreenMagX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGreenMagX.Name = "lblGreenMagX";
            this.lblGreenMagX.Size = new System.Drawing.Size(141, 16);
            this.lblGreenMagX.TabIndex = 57;
            this.lblGreenMagX.Text = "Adaptive Magnitude X:";
            // 
            // numericUpDown_GreenMagX
            // 
            this.numericUpDown_GreenMagX.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown_GreenMagX.Location = new System.Drawing.Point(337, 311);
            this.numericUpDown_GreenMagX.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown_GreenMagX.Maximum = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.numericUpDown_GreenMagX.Name = "numericUpDown_GreenMagX";
            this.numericUpDown_GreenMagX.Size = new System.Drawing.Size(71, 22);
            this.numericUpDown_GreenMagX.TabIndex = 58;
            this.numericUpDown_GreenMagX.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // lblGreenMagY
            // 
            this.lblGreenMagY.AutoSize = true;
            this.lblGreenMagY.Location = new System.Drawing.Point(7, 336);
            this.lblGreenMagY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGreenMagY.Name = "lblGreenMagY";
            this.lblGreenMagY.Size = new System.Drawing.Size(142, 16);
            this.lblGreenMagY.TabIndex = 59;
            this.lblGreenMagY.Text = "Adaptive Magnitude Y:";
            // 
            // numericUpDown_GreenMagY
            // 
            this.numericUpDown_GreenMagY.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown_GreenMagY.Location = new System.Drawing.Point(145, 332);
            this.numericUpDown_GreenMagY.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown_GreenMagY.Maximum = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.numericUpDown_GreenMagY.Name = "numericUpDown_GreenMagY";
            this.numericUpDown_GreenMagY.Size = new System.Drawing.Size(71, 22);
            this.numericUpDown_GreenMagY.TabIndex = 60;
            this.numericUpDown_GreenMagY.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // lbl_InstCurrent_kVARdirection
            // 
            this.lbl_InstCurrent_kVARdirection.AutoSize = true;
            this.lbl_InstCurrent_kVARdirection.Location = new System.Drawing.Point(167, 341);
            this.lbl_InstCurrent_kVARdirection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_InstCurrent_kVARdirection.Name = "lbl_InstCurrent_kVARdirection";
            this.lbl_InstCurrent_kVARdirection.Size = new System.Drawing.Size(232, 16);
            this.lbl_InstCurrent_kVARdirection.TabIndex = 63;
            this.lbl_InstCurrent_kVARdirection.Text = "Instantaneous Current kVAR Direction:";
            // 
            // numericUpDown_InCurrkVAR
            // 
            this.numericUpDown_InCurrkVAR.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown_InCurrkVAR.Location = new System.Drawing.Point(235, 334);
            this.numericUpDown_InCurrkVAR.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown_InCurrkVAR.Maximum = new decimal(new int[] {
            2880,
            0,
            0,
            0});
            this.numericUpDown_InCurrkVAR.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown_InCurrkVAR.Name = "numericUpDown_InCurrkVAR";
            this.numericUpDown_InCurrkVAR.Size = new System.Drawing.Size(71, 22);
            this.numericUpDown_InCurrkVAR.TabIndex = 64;
            this.numericUpDown_InCurrkVAR.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // ucTripMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.numericUpDown_InCurrkVAR);
            this.Controls.Add(this.lbl_InstCurrent_kVARdirection);
            this.Controls.Add(this.numericUpDown_GreenMagY);
            this.Controls.Add(this.lblGreenMagY);
            this.Controls.Add(this.numericUpDown_GreenMagX);
            this.Controls.Add(this.lblGreenMagX);
            this.Controls.Add(this.numericUpDown_GreenDelay);
            this.Controls.Add(this.lblGreenDelay);
            this.Controls.Add(this.groupBoxTripModeSettings);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucTripMode";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(417, 377);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSensitiveTimeDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExtendedTimeDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSensTrip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInsensTrip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWVCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWVAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGullWingAngle)).EndInit();
            this.groupBoxTripModeSettings.ResumeLayout(false);
            this.groupBoxTripModeSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_InCurrkW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenMagX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenMagY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_InCurrkVAR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxTripModes;
        private System.Windows.Forms.Button buttonSendTripData;
        private System.Windows.Forms.NumericUpDown numericUpDownSensitiveTimeDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeDelay;
        private System.Windows.Forms.Label labelSTD;
        private System.Windows.Forms.Label labelTD;
        private System.Windows.Forms.Label labelETD;
        private System.Windows.Forms.NumericUpDown numericUpDownExtendedTimeDelay;
        private System.Windows.Forms.Label labelSTDunit;
        private System.Windows.Forms.Label labelTDunit;
        private System.Windows.Forms.Label labelETDunit;
        private System.Windows.Forms.NumericUpDown numericUpDownAngle;
        private System.Windows.Forms.NumericUpDown numericUpDownSensTrip;
        private System.Windows.Forms.NumericUpDown numericUpDownInsensTrip;
        private System.Windows.Forms.Label labelInsensTrip;
        private System.Windows.Forms.Label labelAngle;
        private System.Windows.Forms.Label labelSensTrip;
        private System.Windows.Forms.Label labelInsensTripUnit;
        private System.Windows.Forms.Label labelAngleUnit;
        private System.Windows.Forms.Label labelSensTripUnit;
        private System.Windows.Forms.DomainUpDown domainUpDownType;
        private System.Windows.Forms.Label labelWVCurrentUnit;
        private System.Windows.Forms.Label labelWVCurrent;
        private System.Windows.Forms.NumericUpDown numericUpDownWVCurrent;
        private System.Windows.Forms.Label labelWVAngleUnit;
        private System.Windows.Forms.Label labelWVAngle;
        private System.Windows.Forms.NumericUpDown numericUpDownWVAngle;
        private System.Windows.Forms.Label labelInstantCurrent;
        private System.Windows.Forms.Button buttonRestoreDefaults;
        private System.Windows.Forms.Label labelGullWingUnits;
        private System.Windows.Forms.Label labelGullWingAngle;
        private System.Windows.Forms.NumericUpDown numericUpDownGullWingAngle;
        private System.Windows.Forms.CheckBox checkBoxEnableGullWing;
        private System.Windows.Forms.Label labelTripStyle;
        private System.Windows.Forms.DomainUpDown domainUpDownTripStyle;
        private System.Windows.Forms.CheckBox checkBoxTripOnPowerDown;
        public System.Windows.Forms.GroupBox groupBoxTripModeSettings;
        private System.Windows.Forms.Label lblGreenDelay;
        private System.Windows.Forms.NumericUpDown numericUpDown_GreenDelay;
        private System.Windows.Forms.Label lblGreenMagX;
        private System.Windows.Forms.NumericUpDown numericUpDown_GreenMagX;
        private System.Windows.Forms.Label lblGreenMagY;
        private System.Windows.Forms.NumericUpDown numericUpDown_GreenMagY;
        private System.Windows.Forms.NumericUpDown numericUpDown_InCurrkW;
        private System.Windows.Forms.Label lbl_InstCurrent_kWdirection;
        private System.Windows.Forms.Label lbl_InstCurrent_kVARdirection;
        private System.Windows.Forms.NumericUpDown numericUpDown_InCurrkVAR;
        private System.Windows.Forms.Label lblUnitGreenDelay;
        private System.Windows.Forms.Label lblUnitGreenMagX;
        private System.Windows.Forms.Label lblUnitGreenMagY;
        private System.Windows.Forms.Label lblUnitInCur_kWdir;
        private System.Windows.Forms.Label lblUnitInCur_kVARdir;
        private System.Windows.Forms.ComboBox comboBox_TripStyle;
        private System.Windows.Forms.ComboBox comboBox_TripType;
    }
}
