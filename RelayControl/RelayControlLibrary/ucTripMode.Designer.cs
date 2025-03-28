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
            this.SuspendLayout();
            // 
            // listBoxTripModes
            // 
            this.listBoxTripModes.FormattingEnabled = true;
            this.listBoxTripModes.Items.AddRange(new object[] {
            "Sensitive",
            "Insensitive",
            "Time Delay",
            "Watt-Var"});
            this.listBoxTripModes.Location = new System.Drawing.Point(5, 51);
            this.listBoxTripModes.Name = "listBoxTripModes";
            this.listBoxTripModes.Size = new System.Drawing.Size(75, 69);
            this.listBoxTripModes.TabIndex = 2;
            this.listBoxTripModes.SelectedIndexChanged += new System.EventHandler(this.listBoxTripModes_SelectedIndexChanged);
            // 
            // buttonSendTripData
            // 
            this.buttonSendTripData.Location = new System.Drawing.Point(195, 226);
            this.buttonSendTripData.Name = "buttonSendTripData";
            this.buttonSendTripData.Size = new System.Drawing.Size(75, 23);
            this.buttonSendTripData.TabIndex = 1;
            this.buttonSendTripData.Text = "Send";
            this.buttonSendTripData.UseVisualStyleBackColor = true;
            this.buttonSendTripData.Click += new System.EventHandler(this.buttonSendTripMode_Click);
            // 
            // numericUpDownSensitiveTimeDelay
            // 
            this.numericUpDownSensitiveTimeDelay.Location = new System.Drawing.Point(197, 49);
            this.numericUpDownSensitiveTimeDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownSensitiveTimeDelay.Name = "numericUpDownSensitiveTimeDelay";
            this.numericUpDownSensitiveTimeDelay.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownSensitiveTimeDelay.TabIndex = 3;
            this.numericUpDownSensitiveTimeDelay.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // numericUpDownTimeDelay
            // 
            this.numericUpDownTimeDelay.Location = new System.Drawing.Point(197, 71);
            this.numericUpDownTimeDelay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownTimeDelay.Name = "numericUpDownTimeDelay";
            this.numericUpDownTimeDelay.Size = new System.Drawing.Size(54, 20);
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
            this.labelSTD.Location = new System.Drawing.Point(87, 51);
            this.labelSTD.Name = "labelSTD";
            this.labelSTD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelSTD.Size = new System.Drawing.Size(109, 13);
            this.labelSTD.TabIndex = 14;
            this.labelSTD.Text = "Sensitive Time Delay:";
            // 
            // labelTD
            // 
            this.labelTD.AutoSize = true;
            this.labelTD.Location = new System.Drawing.Point(133, 73);
            this.labelTD.Name = "labelTD";
            this.labelTD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelTD.Size = new System.Drawing.Size(63, 13);
            this.labelTD.TabIndex = 15;
            this.labelTD.Text = "Time Delay:";
            // 
            // labelETD
            // 
            this.labelETD.AutoSize = true;
            this.labelETD.Location = new System.Drawing.Point(85, 51);
            this.labelETD.Name = "labelETD";
            this.labelETD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelETD.Size = new System.Drawing.Size(111, 13);
            this.labelETD.TabIndex = 17;
            this.labelETD.Text = "Extended Time Delay:";
            // 
            // numericUpDownExtendedTimeDelay
            // 
            this.numericUpDownExtendedTimeDelay.Location = new System.Drawing.Point(197, 49);
            this.numericUpDownExtendedTimeDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownExtendedTimeDelay.Name = "numericUpDownExtendedTimeDelay";
            this.numericUpDownExtendedTimeDelay.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownExtendedTimeDelay.TabIndex = 4;
            // 
            // labelSTDunit
            // 
            this.labelSTDunit.AutoSize = true;
            this.labelSTDunit.Location = new System.Drawing.Point(252, 51);
            this.labelSTDunit.Name = "labelSTDunit";
            this.labelSTDunit.Size = new System.Drawing.Size(38, 13);
            this.labelSTDunit.TabIndex = 19;
            this.labelSTDunit.Text = "Cycles";
            // 
            // labelTDunit
            // 
            this.labelTDunit.AutoSize = true;
            this.labelTDunit.Location = new System.Drawing.Point(251, 73);
            this.labelTDunit.Name = "labelTDunit";
            this.labelTDunit.Size = new System.Drawing.Size(12, 13);
            this.labelTDunit.TabIndex = 20;
            this.labelTDunit.Text = "s";
            // 
            // labelETDunit
            // 
            this.labelETDunit.AutoSize = true;
            this.labelETDunit.Location = new System.Drawing.Point(251, 51);
            this.labelETDunit.Name = "labelETDunit";
            this.labelETDunit.Size = new System.Drawing.Size(12, 13);
            this.labelETDunit.TabIndex = 21;
            this.labelETDunit.Text = "s";
            // 
            // numericUpDownAngle
            // 
            this.numericUpDownAngle.Location = new System.Drawing.Point(197, 116);
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
            this.numericUpDownAngle.Size = new System.Drawing.Size(54, 20);
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
            this.numericUpDownSensTrip.Location = new System.Drawing.Point(197, 94);
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
            this.numericUpDownSensTrip.Size = new System.Drawing.Size(54, 20);
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
            this.numericUpDownInsensTrip.Location = new System.Drawing.Point(197, 138);
            this.numericUpDownInsensTrip.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownInsensTrip.Name = "numericUpDownInsensTrip";
            this.numericUpDownInsensTrip.Size = new System.Drawing.Size(54, 20);
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
            this.labelInsensTrip.Location = new System.Drawing.Point(96, 141);
            this.labelInsensTrip.Name = "labelInsensTrip";
            this.labelInsensTrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelInsensTrip.Size = new System.Drawing.Size(100, 13);
            this.labelInsensTrip.TabIndex = 27;
            this.labelInsensTrip.Text = "Insensitive Trip (IT):";
            // 
            // labelAngle
            // 
            this.labelAngle.AutoSize = true;
            this.labelAngle.Location = new System.Drawing.Point(142, 118);
            this.labelAngle.Name = "labelAngle";
            this.labelAngle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelAngle.Size = new System.Drawing.Size(54, 13);
            this.labelAngle.TabIndex = 26;
            this.labelAngle.Text = "Tilt Angle:";
            // 
            // labelSensTrip
            // 
            this.labelSensTrip.AutoSize = true;
            this.labelSensTrip.Location = new System.Drawing.Point(122, 96);
            this.labelSensTrip.Name = "labelSensTrip";
            this.labelSensTrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelSensTrip.Size = new System.Drawing.Size(74, 13);
            this.labelSensTrip.TabIndex = 25;
            this.labelSensTrip.Text = "Sensitive Trip:";
            // 
            // labelInsensTripUnit
            // 
            this.labelInsensTripUnit.AutoSize = true;
            this.labelInsensTripUnit.Location = new System.Drawing.Point(257, 141);
            this.labelInsensTripUnit.Name = "labelInsensTripUnit";
            this.labelInsensTripUnit.Size = new System.Drawing.Size(14, 13);
            this.labelInsensTripUnit.TabIndex = 30;
            this.labelInsensTripUnit.Text = "A";
            // 
            // labelAngleUnit
            // 
            this.labelAngleUnit.AutoSize = true;
            this.labelAngleUnit.Location = new System.Drawing.Point(257, 118);
            this.labelAngleUnit.Name = "labelAngleUnit";
            this.labelAngleUnit.Size = new System.Drawing.Size(47, 13);
            this.labelAngleUnit.TabIndex = 29;
            this.labelAngleUnit.Text = "Degrees";
            // 
            // labelSensTripUnit
            // 
            this.labelSensTripUnit.AutoSize = true;
            this.labelSensTripUnit.Location = new System.Drawing.Point(257, 96);
            this.labelSensTripUnit.Name = "labelSensTripUnit";
            this.labelSensTripUnit.Size = new System.Drawing.Size(22, 13);
            this.labelSensTripUnit.TabIndex = 28;
            this.labelSensTripUnit.Text = "mA";
            // 
            // domainUpDownType
            // 
            this.domainUpDownType.Items.Add("Relay");
            this.domainUpDownType.Items.Add("Percent");
            this.domainUpDownType.Items.Add("Protector");
            this.domainUpDownType.Location = new System.Drawing.Point(173, 21);
            this.domainUpDownType.Name = "domainUpDownType";
            this.domainUpDownType.ReadOnly = true;
            this.domainUpDownType.Size = new System.Drawing.Size(120, 20);
            this.domainUpDownType.TabIndex = 31;
            this.domainUpDownType.Text = "Relay";
            this.domainUpDownType.SelectedItemChanged += new System.EventHandler(this.domainUpDownType_SelectedItemChanged);
            // 
            // labelWVCurrentUnit
            // 
            this.labelWVCurrentUnit.AutoSize = true;
            this.labelWVCurrentUnit.Location = new System.Drawing.Point(257, 163);
            this.labelWVCurrentUnit.Name = "labelWVCurrentUnit";
            this.labelWVCurrentUnit.Size = new System.Drawing.Size(14, 13);
            this.labelWVCurrentUnit.TabIndex = 36;
            this.labelWVCurrentUnit.Text = "A";
            // 
            // labelWVCurrent
            // 
            this.labelWVCurrent.AutoSize = true;
            this.labelWVCurrent.Location = new System.Drawing.Point(131, 163);
            this.labelWVCurrent.Name = "labelWVCurrent";
            this.labelWVCurrent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelWVCurrent.Size = new System.Drawing.Size(65, 13);
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
            this.numericUpDownWVCurrent.Location = new System.Drawing.Point(197, 160);
            this.numericUpDownWVCurrent.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownWVCurrent.Name = "numericUpDownWVCurrent";
            this.numericUpDownWVCurrent.Size = new System.Drawing.Size(54, 20);
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
            this.labelWVAngleUnit.Location = new System.Drawing.Point(257, 183);
            this.labelWVAngleUnit.Name = "labelWVAngleUnit";
            this.labelWVAngleUnit.Size = new System.Drawing.Size(47, 13);
            this.labelWVAngleUnit.TabIndex = 39;
            this.labelWVAngleUnit.Text = "Degrees";
            // 
            // labelWVAngle
            // 
            this.labelWVAngle.AutoSize = true;
            this.labelWVAngle.Location = new System.Drawing.Point(138, 183);
            this.labelWVAngle.Name = "labelWVAngle";
            this.labelWVAngle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelWVAngle.Size = new System.Drawing.Size(58, 13);
            this.labelWVAngle.TabIndex = 38;
            this.labelWVAngle.Text = "WV Angle:";
            // 
            // numericUpDownWVAngle
            // 
            this.numericUpDownWVAngle.Location = new System.Drawing.Point(197, 181);
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
            this.numericUpDownWVAngle.Size = new System.Drawing.Size(54, 20);
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
            this.labelInstantCurrent.Location = new System.Drawing.Point(63, 139);
            this.labelInstantCurrent.Name = "labelInstantCurrent";
            this.labelInstantCurrent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelInstantCurrent.Size = new System.Drawing.Size(133, 13);
            this.labelInstantCurrent.TabIndex = 40;
            this.labelInstantCurrent.Text = "Instantaneous Current (IC):";
            // 
            // buttonRestoreDefaults
            // 
            this.buttonRestoreDefaults.Location = new System.Drawing.Point(15, 19);
            this.buttonRestoreDefaults.Name = "buttonRestoreDefaults";
            this.buttonRestoreDefaults.Size = new System.Drawing.Size(124, 23);
            this.buttonRestoreDefaults.TabIndex = 41;
            this.buttonRestoreDefaults.Text = "Restore Defaults";
            this.buttonRestoreDefaults.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaults.Click += new System.EventHandler(this.buttonRestoreDefaults_Click);
            // 
            // labelGullWingUnits
            // 
            this.labelGullWingUnits.AutoSize = true;
            this.labelGullWingUnits.Location = new System.Drawing.Point(257, 205);
            this.labelGullWingUnits.Name = "labelGullWingUnits";
            this.labelGullWingUnits.Size = new System.Drawing.Size(47, 13);
            this.labelGullWingUnits.TabIndex = 44;
            this.labelGullWingUnits.Text = "Degrees";
            // 
            // labelGullWingAngle
            // 
            this.labelGullWingAngle.AutoSize = true;
            this.labelGullWingAngle.Location = new System.Drawing.Point(136, 205);
            this.labelGullWingAngle.Name = "labelGullWingAngle";
            this.labelGullWingAngle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelGullWingAngle.Size = new System.Drawing.Size(60, 13);
            this.labelGullWingAngle.TabIndex = 43;
            this.labelGullWingAngle.Text = "Trim Angle:";
            // 
            // numericUpDownGullWingAngle
            // 
            this.numericUpDownGullWingAngle.Location = new System.Drawing.Point(197, 203);
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
            this.numericUpDownGullWingAngle.Size = new System.Drawing.Size(54, 20);
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
            this.checkBoxEnableGullWing.Location = new System.Drawing.Point(5, 183);
            this.checkBoxEnableGullWing.Name = "checkBoxEnableGullWing";
            this.checkBoxEnableGullWing.Size = new System.Drawing.Size(82, 17);
            this.checkBoxEnableGullWing.TabIndex = 45;
            this.checkBoxEnableGullWing.Text = "Enable Trim";
            this.checkBoxEnableGullWing.UseVisualStyleBackColor = true;
            this.checkBoxEnableGullWing.CheckedChanged += new System.EventHandler(this.checkBoxEnableGullWing_CheckedChanged);
            // 
            // checkBoxTripOnPowerDown
            // 
            this.checkBoxTripOnPowerDown.AutoSize = true;
            this.checkBoxTripOnPowerDown.Location = new System.Drawing.Point(5, 206);
            this.checkBoxTripOnPowerDown.Name = "checkBoxTripOnPowerDown";
            this.checkBoxTripOnPowerDown.Size = new System.Drawing.Size(125, 17);
            this.checkBoxTripOnPowerDown.TabIndex = 54;
            this.checkBoxTripOnPowerDown.Text = "Trip On Power Down";
            this.checkBoxTripOnPowerDown.UseVisualStyleBackColor = true;
            // 
            // domainUpDownTripStyle
            // 
            this.domainUpDownTripStyle.BackColor = System.Drawing.SystemColors.Window;
            this.domainUpDownTripStyle.Items.Add("Hold Trip");
            this.domainUpDownTripStyle.Items.Add("Pulse Trip");
            this.domainUpDownTripStyle.Items.Add("Single Attempt");
            this.domainUpDownTripStyle.Items.Add("Short Trip");
            this.domainUpDownTripStyle.Location = new System.Drawing.Point(62, 229);
            this.domainUpDownTripStyle.Name = "domainUpDownTripStyle";
            this.domainUpDownTripStyle.ReadOnly = true;
            this.domainUpDownTripStyle.Size = new System.Drawing.Size(102, 20);
            this.domainUpDownTripStyle.TabIndex = 53;
            this.domainUpDownTripStyle.Text = "Hold Trip";
            // 
            // labelTripStyle
            // 
            this.labelTripStyle.AutoSize = true;
            this.labelTripStyle.Location = new System.Drawing.Point(2, 231);
            this.labelTripStyle.Name = "labelTripStyle";
            this.labelTripStyle.Size = new System.Drawing.Size(54, 13);
            this.labelTripStyle.TabIndex = 52;
            this.labelTripStyle.Text = "Trip Style:";
            // 
            // groupBoxTripModeSettings
            // 
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
            this.groupBoxTripModeSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.groupBoxTripModeSettings.Location = new System.Drawing.Point(3, 3);
            this.groupBoxTripModeSettings.Name = "groupBoxTripModeSettings";
            this.groupBoxTripModeSettings.Size = new System.Drawing.Size(307, 254);
            this.groupBoxTripModeSettings.TabIndex = 47;
            this.groupBoxTripModeSettings.TabStop = false;
            this.groupBoxTripModeSettings.Text = "Trip Mode Settings:";
            // 
            // ucTripMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.groupBoxTripModeSettings);
            this.Name = "ucTripMode";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(313, 293);
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
            this.ResumeLayout(false);

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
        private System.Windows.Forms.GroupBox groupBoxTripModeSettings;
    }
}
