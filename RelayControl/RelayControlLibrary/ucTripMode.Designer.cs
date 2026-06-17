using System.Windows.Forms;
using System.Drawing;

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
            this.groupBoxTripModeSettings = new RelayControlLibrary.BorderlessGroupBox();
            this.comboBox_TripStyle = new System.Windows.Forms.ComboBox();
            this.lblUnitInCur_kVARdir = new System.Windows.Forms.Label();
            this.lblUnitInCur_kWdir = new System.Windows.Forms.Label();
            this.lblUnitGreenMagY = new System.Windows.Forms.Label();
            this.lblUnitGreenMagX = new System.Windows.Forms.Label();
            this.lblUnitGreenDelay = new System.Windows.Forms.Label();
            this.numericUpDown_InCurrkW = new System.Windows.Forms.NumericUpDown();
            this.lbl_InstCurrent_kWdirection = new System.Windows.Forms.Label();
            this.panel_TMsettings = new System.Windows.Forms.Panel();
            this.lbl_TripMode_Title = new System.Windows.Forms.Label();
            this.comboBox_TripType = new System.Windows.Forms.ComboBox();
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
            this.panel_TMsettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenMagX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenMagY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_InCurrkVAR)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxTripModes
            // 
            this.listBoxTripModes.FormattingEnabled = true;
            this.listBoxTripModes.ItemHeight = 19;
            this.listBoxTripModes.Items.AddRange(new object[] {
            "Sensitive",
            "Time Delay",
            "Insensitive",
            "Watt-Var"});
            this.listBoxTripModes.Location = new System.Drawing.Point(5, 51);
            this.listBoxTripModes.Name = "listBoxTripModes";
            this.listBoxTripModes.Size = new System.Drawing.Size(80, 80);
            this.listBoxTripModes.TabIndex = 2;
            this.listBoxTripModes.SelectedIndexChanged += new System.EventHandler(this.listBoxTripModes_SelectedIndexChanged);
            // 
            // buttonSendTripData
            // 
            this.buttonSendTripData.Location = new System.Drawing.Point(155, 226);
            this.buttonSendTripData.Name = "buttonSendTripData";
            this.buttonSendTripData.Size = new System.Drawing.Size(70, 23);
            this.buttonSendTripData.TabIndex = 1;
            this.buttonSendTripData.Text = "Apply";
            this.buttonSendTripData.UseVisualStyleBackColor = true;
            this.buttonSendTripData.Click += new System.EventHandler(this.buttonSendTripMode_Click);
            // 
            // numericUpDownSensitiveTimeDelay
            // 
            this.numericUpDownSensitiveTimeDelay.Location = new System.Drawing.Point(233, 50);
            this.numericUpDownSensitiveTimeDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownSensitiveTimeDelay.Name = "numericUpDownSensitiveTimeDelay";
            this.numericUpDownSensitiveTimeDelay.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownSensitiveTimeDelay.TabIndex = 3;
            this.numericUpDownSensitiveTimeDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownSensitiveTimeDelay.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // numericUpDownTimeDelay
            // 
            this.numericUpDownTimeDelay.Location = new System.Drawing.Point(233, 73);
            this.numericUpDownTimeDelay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownTimeDelay.Name = "numericUpDownTimeDelay";
            this.numericUpDownTimeDelay.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownTimeDelay.TabIndex = 5;
            this.numericUpDownTimeDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownTimeDelay.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // labelSTD
            // 
            this.labelSTD.AutoSize = true;
            this.labelSTD.Location = new System.Drawing.Point(106, 51);
            this.labelSTD.Name = "labelSTD";
            this.labelSTD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelSTD.Size = new System.Drawing.Size(162, 19);
            this.labelSTD.TabIndex = 14;
            this.labelSTD.Text = "Sensitive Time Delay:";
            // 
            // labelTD
            // 
            this.labelTD.AutoSize = true;
            this.labelTD.Location = new System.Drawing.Point(155, 75);
            this.labelTD.Name = "labelTD";
            this.labelTD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelTD.Size = new System.Drawing.Size(95, 19);
            this.labelTD.TabIndex = 15;
            this.labelTD.Text = "Time Delay:";
            // 
            // labelETD
            // 
            this.labelETD.AutoSize = true;
            this.labelETD.Location = new System.Drawing.Point(102, 51);
            this.labelETD.Name = "labelETD";
            this.labelETD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelETD.Size = new System.Drawing.Size(165, 19);
            this.labelETD.TabIndex = 17;
            this.labelETD.Text = "Extended Time Delay:";
            // 
            // numericUpDownExtendedTimeDelay
            // 
            this.numericUpDownExtendedTimeDelay.Location = new System.Drawing.Point(233, 50);
            this.numericUpDownExtendedTimeDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownExtendedTimeDelay.Name = "numericUpDownExtendedTimeDelay";
            this.numericUpDownExtendedTimeDelay.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownExtendedTimeDelay.TabIndex = 4;
            this.numericUpDownExtendedTimeDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelSTDunit
            // 
            this.labelSTDunit.AutoSize = true;
            this.labelSTDunit.Location = new System.Drawing.Point(293, 51);
            this.labelSTDunit.Name = "labelSTDunit";
            this.labelSTDunit.Size = new System.Drawing.Size(53, 19);
            this.labelSTDunit.TabIndex = 19;
            this.labelSTDunit.Text = "Cycles";
            // 
            // labelTDunit
            // 
            this.labelTDunit.AutoSize = true;
            this.labelTDunit.Location = new System.Drawing.Point(293, 75);
            this.labelTDunit.Name = "labelTDunit";
            this.labelTDunit.Size = new System.Drawing.Size(67, 19);
            this.labelTDunit.TabIndex = 20;
            this.labelTDunit.Text = "Seconds";
            // 
            // labelETDunit
            // 
            this.labelETDunit.AutoSize = true;
            this.labelETDunit.Location = new System.Drawing.Point(293, 51);
            this.labelETDunit.Name = "labelETDunit";
            this.labelETDunit.Size = new System.Drawing.Size(67, 19);
            this.labelETDunit.TabIndex = 21;
            this.labelETDunit.Text = "Seconds";
            // 
            // numericUpDownAngle
            // 
            this.numericUpDownAngle.Location = new System.Drawing.Point(233, 120);
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
            this.numericUpDownAngle.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownAngle.TabIndex = 7;
            this.numericUpDownAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            this.numericUpDownSensTrip.Location = new System.Drawing.Point(233, 97);
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
            this.numericUpDownSensTrip.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownSensTrip.TabIndex = 6;
            this.numericUpDownSensTrip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            this.numericUpDownInsensTrip.Location = new System.Drawing.Point(233, 144);
            this.numericUpDownInsensTrip.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownInsensTrip.Name = "numericUpDownInsensTrip";
            this.numericUpDownInsensTrip.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownInsensTrip.TabIndex = 8;
            this.numericUpDownInsensTrip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownInsensTrip.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            // 
            // labelInsensTrip
            // 
            this.labelInsensTrip.AutoSize = true;
            this.labelInsensTrip.Location = new System.Drawing.Point(107, 149);
            this.labelInsensTrip.Name = "labelInsensTrip";
            this.labelInsensTrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelInsensTrip.Size = new System.Drawing.Size(157, 19);
            this.labelInsensTrip.TabIndex = 27;
            this.labelInsensTrip.Text = "Insensitive Trip (IT):";
            // 
            // labelAngle
            // 
            this.labelAngle.AutoSize = true;
            this.labelAngle.Location = new System.Drawing.Point(163, 122);
            this.labelAngle.Name = "labelAngle";
            this.labelAngle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelAngle.Size = new System.Drawing.Size(84, 19);
            this.labelAngle.TabIndex = 26;
            this.labelAngle.Text = "Tilt Angle:";
            // 
            // labelSensTrip
            // 
            this.labelSensTrip.AutoSize = true;
            this.labelSensTrip.Location = new System.Drawing.Point(143, 98);
            this.labelSensTrip.Name = "labelSensTrip";
            this.labelSensTrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelSensTrip.Size = new System.Drawing.Size(111, 19);
            this.labelSensTrip.TabIndex = 25;
            this.labelSensTrip.Text = "Sensitive Trip:";
            // 
            // labelInsensTripUnit
            // 
            this.labelInsensTripUnit.AutoSize = true;
            this.labelInsensTripUnit.Location = new System.Drawing.Point(293, 144);
            this.labelInsensTripUnit.Name = "labelInsensTripUnit";
            this.labelInsensTripUnit.Size = new System.Drawing.Size(50, 19);
            this.labelInsensTripUnit.TabIndex = 30;
            this.labelInsensTripUnit.Text = "Amps";
            // 
            // labelAngleUnit
            // 
            this.labelAngleUnit.AutoSize = true;
            this.labelAngleUnit.Location = new System.Drawing.Point(293, 123);
            this.labelAngleUnit.Name = "labelAngleUnit";
            this.labelAngleUnit.Size = new System.Drawing.Size(66, 19);
            this.labelAngleUnit.TabIndex = 29;
            this.labelAngleUnit.Text = "Degrees";
            // 
            // labelSensTripUnit
            // 
            this.labelSensTripUnit.AutoSize = true;
            this.labelSensTripUnit.Location = new System.Drawing.Point(293, 98);
            this.labelSensTripUnit.Name = "labelSensTripUnit";
            this.labelSensTripUnit.Size = new System.Drawing.Size(34, 19);
            this.labelSensTripUnit.TabIndex = 28;
            this.labelSensTripUnit.Text = "mA";
            // 
            // domainUpDownType
            // 
            this.domainUpDownType.BackColor = System.Drawing.SystemColors.Window;
            this.domainUpDownType.Items.Add("Relay");
            this.domainUpDownType.Items.Add("Percent");
            this.domainUpDownType.Items.Add("Protector");
            this.domainUpDownType.Location = new System.Drawing.Point(233, 21);
            this.domainUpDownType.Name = "domainUpDownType";
            this.domainUpDownType.ReadOnly = true;
            this.domainUpDownType.Size = new System.Drawing.Size(78, 27);
            this.domainUpDownType.TabIndex = 31;
            this.domainUpDownType.Text = "Relay";
            this.domainUpDownType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.domainUpDownType.SelectedItemChanged += new System.EventHandler(this.domainUpDownType_SelectedItemChanged);
            // 
            // labelWVCurrentUnit
            // 
            this.labelWVCurrentUnit.AutoSize = true;
            this.labelWVCurrentUnit.Location = new System.Drawing.Point(293, 169);
            this.labelWVCurrentUnit.Name = "labelWVCurrentUnit";
            this.labelWVCurrentUnit.Size = new System.Drawing.Size(50, 19);
            this.labelWVCurrentUnit.TabIndex = 36;
            this.labelWVCurrentUnit.Text = "Amps";
            // 
            // labelWVCurrent
            // 
            this.labelWVCurrent.AutoSize = true;
            this.labelWVCurrent.Location = new System.Drawing.Point(125, 169);
            this.labelWVCurrent.Name = "labelWVCurrent";
            this.labelWVCurrent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelWVCurrent.Size = new System.Drawing.Size(135, 19);
            this.labelWVCurrent.TabIndex = 35;
            this.labelWVCurrent.Text = "Watt-Var Current:";
            // 
            // numericUpDownWVCurrent
            // 
            this.numericUpDownWVCurrent.DecimalPlaces = 1;
            this.numericUpDownWVCurrent.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownWVCurrent.Location = new System.Drawing.Point(233, 167);
            this.numericUpDownWVCurrent.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownWVCurrent.Name = "numericUpDownWVCurrent";
            this.numericUpDownWVCurrent.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownWVCurrent.TabIndex = 34;
            this.numericUpDownWVCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownWVCurrent.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            // 
            // labelWVAngleUnit
            // 
            this.labelWVAngleUnit.AutoSize = true;
            this.labelWVAngleUnit.Location = new System.Drawing.Point(293, 191);
            this.labelWVAngleUnit.Name = "labelWVAngleUnit";
            this.labelWVAngleUnit.Size = new System.Drawing.Size(66, 19);
            this.labelWVAngleUnit.TabIndex = 39;
            this.labelWVAngleUnit.Text = "Degrees";
            // 
            // labelWVAngle
            // 
            this.labelWVAngle.AutoSize = true;
            this.labelWVAngle.Location = new System.Drawing.Point(134, 195);
            this.labelWVAngle.Name = "labelWVAngle";
            this.labelWVAngle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelWVAngle.Size = new System.Drawing.Size(123, 19);
            this.labelWVAngle.TabIndex = 38;
            this.labelWVAngle.Text = "Watt-Var Angle:";
            // 
            // numericUpDownWVAngle
            // 
            this.numericUpDownWVAngle.Location = new System.Drawing.Point(233, 191);
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
            this.numericUpDownWVAngle.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownWVAngle.TabIndex = 37;
            this.numericUpDownWVAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownWVAngle.Value = new decimal(new int[] {
            60,
            0,
            0,
            -2147483648});
            // 
            // labelInstantCurrent
            // 
            this.labelInstantCurrent.AutoSize = true;
            this.labelInstantCurrent.Location = new System.Drawing.Point(72, 144);
            this.labelInstantCurrent.Name = "labelInstantCurrent";
            this.labelInstantCurrent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelInstantCurrent.Size = new System.Drawing.Size(205, 19);
            this.labelInstantCurrent.TabIndex = 40;
            this.labelInstantCurrent.Text = "Instantaneous Current (IC):";
            // 
            // buttonRestoreDefaults
            // 
            this.buttonRestoreDefaults.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestoreDefaults.Location = new System.Drawing.Point(20, 232);
            this.buttonRestoreDefaults.Name = "buttonRestoreDefaults";
            this.buttonRestoreDefaults.Size = new System.Drawing.Size(100, 23);
            this.buttonRestoreDefaults.TabIndex = 41;
            this.buttonRestoreDefaults.Text = "Restore Defaults";
            this.buttonRestoreDefaults.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaults.Click += new System.EventHandler(this.buttonRestoreDefaults_Click);
            // 
            // labelGullWingUnits
            // 
            this.labelGullWingUnits.AutoSize = true;
            this.labelGullWingUnits.Location = new System.Drawing.Point(293, 218);
            this.labelGullWingUnits.Name = "labelGullWingUnits";
            this.labelGullWingUnits.Size = new System.Drawing.Size(66, 19);
            this.labelGullWingUnits.TabIndex = 44;
            this.labelGullWingUnits.Text = "Degrees";
            // 
            // labelGullWingAngle
            // 
            this.labelGullWingAngle.AutoSize = true;
            this.labelGullWingAngle.Location = new System.Drawing.Point(154, 218);
            this.labelGullWingAngle.Name = "labelGullWingAngle";
            this.labelGullWingAngle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelGullWingAngle.Size = new System.Drawing.Size(95, 19);
            this.labelGullWingAngle.TabIndex = 43;
            this.labelGullWingAngle.Text = "Trim Angle:";
            // 
            // numericUpDownGullWingAngle
            // 
            this.numericUpDownGullWingAngle.Location = new System.Drawing.Point(233, 215);
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
            this.numericUpDownGullWingAngle.Size = new System.Drawing.Size(54, 27);
            this.numericUpDownGullWingAngle.TabIndex = 42;
            this.numericUpDownGullWingAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            this.checkBoxEnableGullWing.Size = new System.Drawing.Size(114, 23);
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
            this.checkBoxTripOnPowerDown.Size = new System.Drawing.Size(178, 23);
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
            this.domainUpDownTripStyle.Location = new System.Drawing.Point(41, 126);
            this.domainUpDownTripStyle.Name = "domainUpDownTripStyle";
            this.domainUpDownTripStyle.ReadOnly = true;
            this.domainUpDownTripStyle.Size = new System.Drawing.Size(150, 27);
            this.domainUpDownTripStyle.TabIndex = 53;
            this.domainUpDownTripStyle.Text = "Hold Trip (Troubleshooting Only)";
            this.domainUpDownTripStyle.Visible = false;
            // 
            // labelTripStyle
            // 
            this.labelTripStyle.AutoSize = true;
            this.labelTripStyle.Location = new System.Drawing.Point(49, 25);
            this.labelTripStyle.Name = "labelTripStyle";
            this.labelTripStyle.Size = new System.Drawing.Size(83, 19);
            this.labelTripStyle.TabIndex = 52;
            this.labelTripStyle.Text = "Trip Style:";
            // 
            // groupBoxTripModeSettings
            // 
            this.groupBoxTripModeSettings.BackColor = System.Drawing.SystemColors.Control;
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
            this.groupBoxTripModeSettings.Controls.Add(this.panel_TMsettings);
            this.groupBoxTripModeSettings.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxTripModeSettings.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBoxTripModeSettings.Location = new System.Drawing.Point(3, 3);
            this.groupBoxTripModeSettings.Name = "groupBoxTripModeSettings";
            this.groupBoxTripModeSettings.Size = new System.Drawing.Size(356, 294);
            this.groupBoxTripModeSettings.TabIndex = 47;
            this.groupBoxTripModeSettings.TabStop = false;
            this.groupBoxTripModeSettings.Text = "Trip Mode";
            // 
            // comboBox_TripStyle
            // 
            this.comboBox_TripStyle.FormattingEnabled = true;
            this.comboBox_TripStyle.Items.AddRange(new object[] {
            "Hold Trip",
            "Pulse Trip",
            "Single Attempt",
            "Short Trip"});
            this.comboBox_TripStyle.Location = new System.Drawing.Point(117, 23);
            this.comboBox_TripStyle.Name = "comboBox_TripStyle";
            this.comboBox_TripStyle.Size = new System.Drawing.Size(101, 27);
            this.comboBox_TripStyle.TabIndex = 68;
            this.comboBox_TripStyle.SelectedIndexChanged += new System.EventHandler(this.comboBox_TripStyle_SelectedItemChanged);
            // 
            // lblUnitInCur_kVARdir
            // 
            this.lblUnitInCur_kVARdir.AutoSize = true;
            this.lblUnitInCur_kVARdir.Location = new System.Drawing.Point(27, 163);
            this.lblUnitInCur_kVARdir.Name = "lblUnitInCur_kVARdir";
            this.lblUnitInCur_kVARdir.Size = new System.Drawing.Size(25, 19);
            this.lblUnitInCur_kVARdir.TabIndex = 67;
            this.lblUnitInCur_kVARdir.Text = "%";
            // 
            // lblUnitInCur_kWdir
            // 
            this.lblUnitInCur_kWdir.AutoSize = true;
            this.lblUnitInCur_kWdir.Location = new System.Drawing.Point(6, 162);
            this.lblUnitInCur_kWdir.Name = "lblUnitInCur_kWdir";
            this.lblUnitInCur_kWdir.Size = new System.Drawing.Size(25, 19);
            this.lblUnitInCur_kWdir.TabIndex = 66;
            this.lblUnitInCur_kWdir.Text = "%";
            // 
            // lblUnitGreenMagY
            // 
            this.lblUnitGreenMagY.AutoSize = true;
            this.lblUnitGreenMagY.Location = new System.Drawing.Point(6, 149);
            this.lblUnitGreenMagY.Name = "lblUnitGreenMagY";
            this.lblUnitGreenMagY.Size = new System.Drawing.Size(25, 19);
            this.lblUnitGreenMagY.TabIndex = 65;
            this.lblUnitGreenMagY.Text = "%";
            // 
            // lblUnitGreenMagX
            // 
            this.lblUnitGreenMagX.AutoSize = true;
            this.lblUnitGreenMagX.Location = new System.Drawing.Point(12, 136);
            this.lblUnitGreenMagX.Name = "lblUnitGreenMagX";
            this.lblUnitGreenMagX.Size = new System.Drawing.Size(25, 19);
            this.lblUnitGreenMagX.TabIndex = 64;
            this.lblUnitGreenMagX.Text = "%";
            // 
            // lblUnitGreenDelay
            // 
            this.lblUnitGreenDelay.AutoSize = true;
            this.lblUnitGreenDelay.Location = new System.Drawing.Point(12, 123);
            this.lblUnitGreenDelay.Name = "lblUnitGreenDelay";
            this.lblUnitGreenDelay.Size = new System.Drawing.Size(32, 19);
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
            this.numericUpDown_InCurrkW.Location = new System.Drawing.Point(235, -2);
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
            this.numericUpDown_InCurrkW.Size = new System.Drawing.Size(53, 27);
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
            this.lbl_InstCurrent_kWdirection.Location = new System.Drawing.Point(110, 0);
            this.lbl_InstCurrent_kWdirection.Name = "lbl_InstCurrent_kWdirection";
            this.lbl_InstCurrent_kWdirection.Size = new System.Drawing.Size(267, 19);
            this.lbl_InstCurrent_kWdirection.TabIndex = 61;
            this.lbl_InstCurrent_kWdirection.Text = "Instantaneous Current kW Direction:";
            // 
            // panel_TMsettings
            // 
            this.panel_TMsettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_TMsettings.Controls.Add(this.lbl_TripMode_Title);
            this.panel_TMsettings.Controls.Add(this.comboBox_TripType);
            this.panel_TMsettings.Controls.Add(this.domainUpDownType);
            this.panel_TMsettings.Location = new System.Drawing.Point(0, 0);
            this.panel_TMsettings.Name = "panel_TMsettings";
            this.panel_TMsettings.Size = new System.Drawing.Size(356, 293);
            this.panel_TMsettings.TabIndex = 70;
            // 
            // lbl_TripMode_Title
            // 
            this.lbl_TripMode_Title.AutoSize = true;
            this.lbl_TripMode_Title.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TripMode_Title.Location = new System.Drawing.Point(77, 80);
            this.lbl_TripMode_Title.Name = "lbl_TripMode_Title";
            this.lbl_TripMode_Title.Size = new System.Drawing.Size(91, 19);
            this.lbl_TripMode_Title.TabIndex = 70;
            this.lbl_TripMode_Title.Text = "Trip Mode";
            // 
            // comboBox_TripType
            // 
            this.comboBox_TripType.Enabled = false;
            this.comboBox_TripType.FormattingEnabled = true;
            this.comboBox_TripType.Items.AddRange(new object[] {
            "Relay",
            "Percent",
            "Protector"});
            this.comboBox_TripType.Location = new System.Drawing.Point(5, -1);
            this.comboBox_TripType.Name = "comboBox_TripType";
            this.comboBox_TripType.Size = new System.Drawing.Size(75, 27);
            this.comboBox_TripType.TabIndex = 69;
            this.comboBox_TripType.Visible = false;
            // 
            // lblGreenDelay
            // 
            this.lblGreenDelay.AutoSize = true;
            this.lblGreenDelay.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreenDelay.Location = new System.Drawing.Point(3, 260);
            this.lblGreenDelay.Name = "lblGreenDelay";
            this.lblGreenDelay.Size = new System.Drawing.Size(121, 19);
            this.lblGreenDelay.TabIndex = 55;
            this.lblGreenDelay.Text = "Adaptive Delay:";
            // 
            // numericUpDown_GreenDelay
            // 
            this.numericUpDown_GreenDelay.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_GreenDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_GreenDelay.Location = new System.Drawing.Point(80, 260);
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
            this.numericUpDown_GreenDelay.Size = new System.Drawing.Size(53, 27);
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
            this.lblGreenMagX.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreenMagX.Location = new System.Drawing.Point(145, 255);
            this.lblGreenMagX.Name = "lblGreenMagX";
            this.lblGreenMagX.Size = new System.Drawing.Size(169, 19);
            this.lblGreenMagX.TabIndex = 57;
            this.lblGreenMagX.Text = "Adaptive Magnitude X:";
            // 
            // numericUpDown_GreenMagX
            // 
            this.numericUpDown_GreenMagX.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_GreenMagX.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown_GreenMagX.Location = new System.Drawing.Point(253, 253);
            this.numericUpDown_GreenMagX.Maximum = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.numericUpDown_GreenMagX.Name = "numericUpDown_GreenMagX";
            this.numericUpDown_GreenMagX.Size = new System.Drawing.Size(53, 27);
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
            this.lblGreenMagY.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreenMagY.Location = new System.Drawing.Point(5, 273);
            this.lblGreenMagY.Name = "lblGreenMagY";
            this.lblGreenMagY.Size = new System.Drawing.Size(170, 19);
            this.lblGreenMagY.TabIndex = 59;
            this.lblGreenMagY.Text = "Adaptive Magnitude Y:";
            // 
            // numericUpDown_GreenMagY
            // 
            this.numericUpDown_GreenMagY.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_GreenMagY.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown_GreenMagY.Location = new System.Drawing.Point(109, 270);
            this.numericUpDown_GreenMagY.Maximum = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.numericUpDown_GreenMagY.Name = "numericUpDown_GreenMagY";
            this.numericUpDown_GreenMagY.Size = new System.Drawing.Size(53, 27);
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
            this.lbl_InstCurrent_kVARdirection.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_InstCurrent_kVARdirection.Location = new System.Drawing.Point(125, 277);
            this.lbl_InstCurrent_kVARdirection.Name = "lbl_InstCurrent_kVARdirection";
            this.lbl_InstCurrent_kVARdirection.Size = new System.Drawing.Size(284, 19);
            this.lbl_InstCurrent_kVARdirection.TabIndex = 63;
            this.lbl_InstCurrent_kVARdirection.Text = "Instantaneous Current kVAR Direction:";
            // 
            // numericUpDown_InCurrkVAR
            // 
            this.numericUpDown_InCurrkVAR.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_InCurrkVAR.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown_InCurrkVAR.Location = new System.Drawing.Point(176, 271);
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
            this.numericUpDown_InCurrkVAR.Size = new System.Drawing.Size(53, 27);
            this.numericUpDown_InCurrkVAR.TabIndex = 64;
            this.numericUpDown_InCurrkVAR.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // ucTripMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
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
            this.Name = "ucTripMode";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(412, 303);
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
            this.panel_TMsettings.ResumeLayout(false);
            this.panel_TMsettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenMagX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GreenMagY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_InCurrkVAR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        private System.Windows.Forms.ListBox listBoxTripModes;
        public System.Windows.Forms.Button buttonSendTripData;
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
        public System.Windows.Forms.Button buttonRestoreDefaults;
        private System.Windows.Forms.Label labelGullWingUnits;
        private System.Windows.Forms.Label labelGullWingAngle;
        private System.Windows.Forms.NumericUpDown numericUpDownGullWingAngle;
        public System.Windows.Forms.CheckBox checkBoxEnableGullWing;
        private System.Windows.Forms.Label labelTripStyle;
        private System.Windows.Forms.DomainUpDown domainUpDownTripStyle;
        public System.Windows.Forms.CheckBox checkBoxTripOnPowerDown;
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
        public System.Windows.Forms.ComboBox comboBox_TripStyle;
        public System.Windows.Forms.ComboBox comboBox_TripType;
        private Panel panel_TMsettings;
        private Label lbl_TripMode_Title;
        public BorderlessGroupBox groupBoxTripModeSettings;
    }
}
