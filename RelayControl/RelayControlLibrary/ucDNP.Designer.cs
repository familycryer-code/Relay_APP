namespace RelayControlLibrary
{
    partial class ucDNP
    //public class ucDNP
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
       // private System.ComponentModel.IContainer components = null;
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
        //private void InitializeComponent()
        public void InitializeComponent()
        {
            this.labelLinkLayerConfirm = new System.Windows.Forms.Label();
            this.comboBoxLinkLayerConfirm = new System.Windows.Forms.ComboBox();
            this.comboBoxSelfAddress = new System.Windows.Forms.ComboBox();
            this.labelSelfAddress = new System.Windows.Forms.Label();
            this.comboBoxUnsolResponse = new System.Windows.Forms.ComboBox();
            this.labelUnsolResponse = new System.Windows.Forms.Label();
            this.labelUnsolTimeout = new System.Windows.Forms.Label();
            this.numericUpDownUnsolTimeout = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownFragmentSize = new System.Windows.Forms.NumericUpDown();
            this.labelFragmentSize = new System.Windows.Forms.Label();
            this.numericUpDownDestinationAddress = new System.Windows.Forms.NumericUpDown();
            this.labelDestinationAddress = new System.Windows.Forms.Label();
            this.numericUpDownSourceAddress = new System.Windows.Forms.NumericUpDown();
            this.labelSourceAddress = new System.Windows.Forms.Label();
            this.numericUpDownMaxEvents = new System.Windows.Forms.NumericUpDown();
            this.labelMaxEvents = new System.Windows.Forms.Label();
            this.numericUpDownUnsolRetries = new System.Windows.Forms.NumericUpDown();
            this.labelUnsolRetries = new System.Windows.Forms.Label();
            this.labelTerminationResistor = new System.Windows.Forms.Label();
            this.comboBoxTerminationResistor = new System.Windows.Forms.ComboBox();
            this.labelMemphisStage = new System.Windows.Forms.Label();
            this.numericUpDownMemphisStage = new System.Windows.Forms.NumericUpDown();
            this.labelTHDTriggerRange = new System.Windows.Forms.Label();
            this.labelVoltageTriggerRange = new System.Windows.Forms.Label();
            this.numericUpDownTriggerRangeTHD = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTriggerRangeVoltage = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTriggerRangeCurrent = new System.Windows.Forms.NumericUpDown();
            this.labelTriggerRangeCurrent = new System.Windows.Forms.Label();
            this.labelTriggerRangeVoltageUnits = new System.Windows.Forms.Label();
            this.labelTriggerRangeTHDUnits = new System.Windows.Forms.Label();
            this.labelTriggerRangeCurrentUnits = new System.Windows.Forms.Label();
            this.buttonRQDNPSettings = new System.Windows.Forms.Button();
            this.buttonSendAllDNPSettings = new System.Windows.Forms.Button();
            this.labelZeroDisables = new System.Windows.Forms.Label();
            this.labelTriggerRangeTemperatureUnits = new System.Windows.Forms.Label();
            this.numericUpDownTriggerRangeTemperature = new System.Windows.Forms.NumericUpDown();
            this.labelTriggerRangeTemperature = new System.Windows.Forms.Label();
            this.buttonDefaults = new System.Windows.Forms.Button();
            this.groupBoxMemphisDeadBand = new System.Windows.Forms.GroupBox();
            this.buttonSendMemphis = new System.Windows.Forms.Button();
            this.labelAnalog4DeadBand = new System.Windows.Forms.Label();
            this.numericUpDownAnalog4DeadBand = new System.Windows.Forms.NumericUpDown();
            this.labelAnalog4DeadBandUnits = new System.Windows.Forms.Label();
            this.labelAnalog3DeadBandUnits = new System.Windows.Forms.Label();
            this.labelTotalKWDB = new System.Windows.Forms.Label();
            this.numericUpDownAnalog3DeadBand = new System.Windows.Forms.NumericUpDown();
            this.labelTotalKVAVARDB = new System.Windows.Forms.Label();
            this.labelAnalog3DeadBand = new System.Windows.Forms.Label();
            this.labelAnalog1DeadBand = new System.Windows.Forms.Label();
            this.numericUpDownTotalKWDB = new System.Windows.Forms.NumericUpDown();
            this.labelAnalog2DeadBandUnits = new System.Windows.Forms.Label();
            this.numericUpDownTotalKVAVARDB = new System.Windows.Forms.NumericUpDown();
            this.labelAnalog1DeadBandUnits = new System.Windows.Forms.Label();
            this.numericUpDownAnalog1DeadBand = new System.Windows.Forms.NumericUpDown();
            this.labelTotalKVAVARDBUnits = new System.Windows.Forms.Label();
            this.labelAnalog2DeadBand = new System.Windows.Forms.Label();
            this.labelTotalKWDBUnits = new System.Windows.Forms.Label();
            this.numericUpDownAnalog2DeadBand = new System.Windows.Forms.NumericUpDown();
            this.labelPhaseKVADBUNits = new System.Windows.Forms.Label();
            this.numericUpDownPhaseKVADB = new System.Windows.Forms.NumericUpDown();
            this.labelPhaseKVADB = new System.Windows.Forms.Label();
            this.labelPhaseKVARDBUnits = new System.Windows.Forms.Label();
            this.labelPhaseKVARDB = new System.Windows.Forms.Label();
            this.numericUpDownPhaseKVARDB = new System.Windows.Forms.NumericUpDown();
            this.labelPhaseKWDBUnits = new System.Windows.Forms.Label();
            this.labelOdometer = new System.Windows.Forms.Label();
            this.numericUpDownPhaseKWDB = new System.Windows.Forms.NumericUpDown();
            this.labelDifferentialVoltsDB = new System.Windows.Forms.Label();
            this.labelPhaseKWDB = new System.Windows.Forms.Label();
            this.labelDifferentialVoltsRealDB = new System.Windows.Forms.Label();
            this.numericUpDownOdometer = new System.Windows.Forms.NumericUpDown();
            this.labelCurrentAngleDBUnits = new System.Windows.Forms.Label();
            this.numericUpDownDifferentialVoltsDB = new System.Windows.Forms.NumericUpDown();
            this.labelDifferentialVoltsRealDBUnits = new System.Windows.Forms.Label();
            this.numericUpDownDifferentialVoltsRealDB = new System.Windows.Forms.NumericUpDown();
            this.labelDifferentialVoltsDBUnits = new System.Windows.Forms.Label();
            this.labelCurrentAngleDB = new System.Windows.Forms.Label();
            this.labelOdomoterUnits = new System.Windows.Forms.Label();
            this.numericUpDownCurrentAngleDB = new System.Windows.Forms.NumericUpDown();
            this.groupBoxDNPSettings = new System.Windows.Forms.GroupBox();
            this.comboBoxDNPBaudRate = new System.Windows.Forms.ComboBox();
            this.labelBaudRate = new System.Windows.Forms.Label();
            this.groupBoxDIGITALGRIDDNPDeadBand = new System.Windows.Forms.GroupBox();
            this.buttonSendDIGITALGRIDDeadBand = new System.Windows.Forms.Button();
            this.label29 = new System.Windows.Forms.Label();
            this.buttonSendDeadBand = new System.Windows.Forms.Button();
            this.labelSAv5AggressiveMode = new System.Windows.Forms.Label();
            this.comboBoxSAv5AggressiveMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numericUpDownSAv5UserNumber = new System.Windows.Forms.NumericUpDown();
            this.labelSAv5UserNumber = new System.Windows.Forms.Label();
            this.labelSAv5UserKey = new System.Windows.Forms.Label();
            this.textBoxSAv5UserUpdateKey = new System.Windows.Forms.TextBox();
            this.labelDNPtext1 = new System.Windows.Forms.Label();
            this.labelDNPStatusInidcation = new System.Windows.Forms.Label();
            this.groupBoxDNPStatus = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUnsolTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFragmentSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDestinationAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSourceAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxEvents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUnsolRetries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMemphisStage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTriggerRangeTHD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTriggerRangeVoltage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTriggerRangeCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTriggerRangeTemperature)).BeginInit();
            this.groupBoxMemphisDeadBand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnalog4DeadBand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnalog3DeadBand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTotalKWDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTotalKVAVARDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnalog1DeadBand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnalog2DeadBand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPhaseKVADB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPhaseKVARDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPhaseKWDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOdometer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDifferentialVoltsDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDifferentialVoltsRealDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCurrentAngleDB)).BeginInit();
            this.groupBoxDNPSettings.SuspendLayout();
            this.groupBoxDIGITALGRIDDNPDeadBand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSAv5UserNumber)).BeginInit();
            this.groupBoxDNPStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelLinkLayerConfirm
            // 
            this.labelLinkLayerConfirm.AutoSize = true;
            this.labelLinkLayerConfirm.Location = new System.Drawing.Point(6, 16);
            this.labelLinkLayerConfirm.Name = "labelLinkLayerConfirm";
            this.labelLinkLayerConfirm.Size = new System.Drawing.Size(97, 13);
            this.labelLinkLayerConfirm.TabIndex = 0;
            this.labelLinkLayerConfirm.Text = "Link Layer Confirm:";
            // 
            // comboBoxLinkLayerConfirm
            // 
            this.comboBoxLinkLayerConfirm.FormattingEnabled = true;
            this.comboBoxLinkLayerConfirm.Items.AddRange(new object[] {
            "Never",
            "Sometimes",
            "Always"});
            this.comboBoxLinkLayerConfirm.Location = new System.Drawing.Point(143, 13);
            this.comboBoxLinkLayerConfirm.Name = "comboBoxLinkLayerConfirm";
            this.comboBoxLinkLayerConfirm.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLinkLayerConfirm.TabIndex = 1;
            this.comboBoxLinkLayerConfirm.Text = "Never";
            // 
            // comboBoxSelfAddress
            // 
            this.comboBoxSelfAddress.FormattingEnabled = true;
            this.comboBoxSelfAddress.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.comboBoxSelfAddress.Location = new System.Drawing.Point(143, 46);
            this.comboBoxSelfAddress.Name = "comboBoxSelfAddress";
            this.comboBoxSelfAddress.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSelfAddress.TabIndex = 4;
            this.comboBoxSelfAddress.Text = "Disable";
            // 
            // labelSelfAddress
            // 
            this.labelSelfAddress.AutoSize = true;
            this.labelSelfAddress.Location = new System.Drawing.Point(6, 49);
            this.labelSelfAddress.Name = "labelSelfAddress";
            this.labelSelfAddress.Size = new System.Drawing.Size(69, 13);
            this.labelSelfAddress.TabIndex = 3;
            this.labelSelfAddress.Text = "Self Address:";
            // 
            // comboBoxUnsolResponse
            // 
            this.comboBoxUnsolResponse.FormattingEnabled = true;
            this.comboBoxUnsolResponse.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.comboBoxUnsolResponse.Location = new System.Drawing.Point(143, 79);
            this.comboBoxUnsolResponse.Name = "comboBoxUnsolResponse";
            this.comboBoxUnsolResponse.Size = new System.Drawing.Size(121, 21);
            this.comboBoxUnsolResponse.TabIndex = 7;
            this.comboBoxUnsolResponse.Text = "Disable";
            // 
            // labelUnsolResponse
            // 
            this.labelUnsolResponse.AutoSize = true;
            this.labelUnsolResponse.Location = new System.Drawing.Point(6, 82);
            this.labelUnsolResponse.Name = "labelUnsolResponse";
            this.labelUnsolResponse.Size = new System.Drawing.Size(113, 13);
            this.labelUnsolResponse.TabIndex = 6;
            this.labelUnsolResponse.Text = "Unsolicited Response:";
            // 
            // labelUnsolTimeout
            // 
            this.labelUnsolTimeout.AutoSize = true;
            this.labelUnsolTimeout.Location = new System.Drawing.Point(6, 113);
            this.labelUnsolTimeout.Name = "labelUnsolTimeout";
            this.labelUnsolTimeout.Size = new System.Drawing.Size(125, 13);
            this.labelUnsolTimeout.TabIndex = 9;
            this.labelUnsolTimeout.Text = "Unsolicited Timeout (ms):";
            // 
            // numericUpDownUnsolTimeout
            // 
            this.numericUpDownUnsolTimeout.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownUnsolTimeout.Location = new System.Drawing.Point(144, 109);
            this.numericUpDownUnsolTimeout.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownUnsolTimeout.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownUnsolTimeout.Name = "numericUpDownUnsolTimeout";
            this.numericUpDownUnsolTimeout.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownUnsolTimeout.TabIndex = 10;
            this.numericUpDownUnsolTimeout.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numericUpDownFragmentSize
            // 
            this.numericUpDownFragmentSize.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownFragmentSize.Location = new System.Drawing.Point(144, 140);
            this.numericUpDownFragmentSize.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDownFragmentSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownFragmentSize.Name = "numericUpDownFragmentSize";
            this.numericUpDownFragmentSize.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownFragmentSize.TabIndex = 13;
            this.numericUpDownFragmentSize.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // labelFragmentSize
            // 
            this.labelFragmentSize.AutoSize = true;
            this.labelFragmentSize.Location = new System.Drawing.Point(6, 144);
            this.labelFragmentSize.Name = "labelFragmentSize";
            this.labelFragmentSize.Size = new System.Drawing.Size(74, 13);
            this.labelFragmentSize.TabIndex = 12;
            this.labelFragmentSize.Text = "Fragment Size";
            // 
            // numericUpDownDestinationAddress
            // 
            this.numericUpDownDestinationAddress.Location = new System.Drawing.Point(144, 200);
            this.numericUpDownDestinationAddress.Maximum = new decimal(new int[] {
            65519,
            0,
            0,
            0});
            this.numericUpDownDestinationAddress.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDestinationAddress.Name = "numericUpDownDestinationAddress";
            this.numericUpDownDestinationAddress.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownDestinationAddress.TabIndex = 19;
            this.numericUpDownDestinationAddress.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // labelDestinationAddress
            // 
            this.labelDestinationAddress.AutoSize = true;
            this.labelDestinationAddress.Location = new System.Drawing.Point(6, 204);
            this.labelDestinationAddress.Name = "labelDestinationAddress";
            this.labelDestinationAddress.Size = new System.Drawing.Size(106, 13);
            this.labelDestinationAddress.TabIndex = 18;
            this.labelDestinationAddress.Text = "Destination  (master):";
            // 
            // numericUpDownSourceAddress
            // 
            this.numericUpDownSourceAddress.Location = new System.Drawing.Point(144, 169);
            this.numericUpDownSourceAddress.Maximum = new decimal(new int[] {
            65519,
            0,
            0,
            0});
            this.numericUpDownSourceAddress.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSourceAddress.Name = "numericUpDownSourceAddress";
            this.numericUpDownSourceAddress.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownSourceAddress.TabIndex = 16;
            this.numericUpDownSourceAddress.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // labelSourceAddress
            // 
            this.labelSourceAddress.AutoSize = true;
            this.labelSourceAddress.Location = new System.Drawing.Point(6, 173);
            this.labelSourceAddress.Name = "labelSourceAddress";
            this.labelSourceAddress.Size = new System.Drawing.Size(116, 13);
            this.labelSourceAddress.TabIndex = 15;
            this.labelSourceAddress.Text = "Source Address (relay):";
            // 
            // numericUpDownMaxEvents
            // 
            this.numericUpDownMaxEvents.Location = new System.Drawing.Point(144, 229);
            this.numericUpDownMaxEvents.Maximum = new decimal(new int[] {
            125,
            0,
            0,
            0});
            this.numericUpDownMaxEvents.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMaxEvents.Name = "numericUpDownMaxEvents";
            this.numericUpDownMaxEvents.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMaxEvents.TabIndex = 22;
            this.numericUpDownMaxEvents.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelMaxEvents
            // 
            this.labelMaxEvents.AutoSize = true;
            this.labelMaxEvents.Location = new System.Drawing.Point(6, 233);
            this.labelMaxEvents.Name = "labelMaxEvents";
            this.labelMaxEvents.Size = new System.Drawing.Size(123, 13);
            this.labelMaxEvents.TabIndex = 21;
            this.labelMaxEvents.Text = "Max Events (all classes):";
            // 
            // numericUpDownUnsolRetries
            // 
            this.numericUpDownUnsolRetries.Location = new System.Drawing.Point(144, 256);
            this.numericUpDownUnsolRetries.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownUnsolRetries.Name = "numericUpDownUnsolRetries";
            this.numericUpDownUnsolRetries.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownUnsolRetries.TabIndex = 25;
            this.numericUpDownUnsolRetries.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelUnsolRetries
            // 
            this.labelUnsolRetries.AutoSize = true;
            this.labelUnsolRetries.Location = new System.Drawing.Point(6, 260);
            this.labelUnsolRetries.Name = "labelUnsolRetries";
            this.labelUnsolRetries.Size = new System.Drawing.Size(128, 13);
            this.labelUnsolRetries.TabIndex = 24;
            this.labelUnsolRetries.Text = "Unsol Retires (0 = infinte):";
            // 
            // labelTerminationResistor
            // 
            this.labelTerminationResistor.AutoSize = true;
            this.labelTerminationResistor.Location = new System.Drawing.Point(6, 290);
            this.labelTerminationResistor.Name = "labelTerminationResistor";
            this.labelTerminationResistor.Size = new System.Drawing.Size(103, 13);
            this.labelTerminationResistor.TabIndex = 27;
            this.labelTerminationResistor.Text = "Termination Resistor";
            // 
            // comboBoxTerminationResistor
            // 
            this.comboBoxTerminationResistor.FormattingEnabled = true;
            this.comboBoxTerminationResistor.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.comboBoxTerminationResistor.Location = new System.Drawing.Point(143, 286);
            this.comboBoxTerminationResistor.Name = "comboBoxTerminationResistor";
            this.comboBoxTerminationResistor.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTerminationResistor.TabIndex = 30;
            this.comboBoxTerminationResistor.Text = "Disable";
            // 
            // labelMemphisStage
            // 
            this.labelMemphisStage.AutoSize = true;
            this.labelMemphisStage.Location = new System.Drawing.Point(5, 345);
            this.labelMemphisStage.Name = "labelMemphisStage";
            this.labelMemphisStage.Size = new System.Drawing.Size(83, 13);
            this.labelMemphisStage.TabIndex = 33;
            this.labelMemphisStage.Text = "Memphis Stage:";
            // 
            // numericUpDownMemphisStage
            // 
            this.numericUpDownMemphisStage.Location = new System.Drawing.Point(142, 343);
            this.numericUpDownMemphisStage.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownMemphisStage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMemphisStage.Name = "numericUpDownMemphisStage";
            this.numericUpDownMemphisStage.Size = new System.Drawing.Size(121, 20);
            this.numericUpDownMemphisStage.TabIndex = 1;
            this.numericUpDownMemphisStage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelTHDTriggerRange
            // 
            this.labelTHDTriggerRange.AutoSize = true;
            this.labelTHDTriggerRange.Location = new System.Drawing.Point(6, 50);
            this.labelTHDTriggerRange.Name = "labelTHDTriggerRange";
            this.labelTHDTriggerRange.Size = new System.Drawing.Size(51, 13);
            this.labelTHDTriggerRange.TabIndex = 37;
            this.labelTHDTriggerRange.Text = "THD DB:";
            // 
            // labelVoltageTriggerRange
            // 
            this.labelVoltageTriggerRange.AutoSize = true;
            this.labelVoltageTriggerRange.Location = new System.Drawing.Point(6, 21);
            this.labelVoltageTriggerRange.Name = "labelVoltageTriggerRange";
            this.labelVoltageTriggerRange.Size = new System.Drawing.Size(64, 13);
            this.labelVoltageTriggerRange.TabIndex = 35;
            this.labelVoltageTriggerRange.Text = "Voltage DB:";
            // 
            // numericUpDownTriggerRangeTHD
            // 
            this.numericUpDownTriggerRangeTHD.DecimalPlaces = 1;
            this.numericUpDownTriggerRangeTHD.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownTriggerRangeTHD.Location = new System.Drawing.Point(98, 48);
            this.numericUpDownTriggerRangeTHD.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            65536});
            this.numericUpDownTriggerRangeTHD.Name = "numericUpDownTriggerRangeTHD";
            this.numericUpDownTriggerRangeTHD.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownTriggerRangeTHD.TabIndex = 3;
            this.numericUpDownTriggerRangeTHD.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            // 
            // numericUpDownTriggerRangeVoltage
            // 
            this.numericUpDownTriggerRangeVoltage.Location = new System.Drawing.Point(98, 19);
            this.numericUpDownTriggerRangeVoltage.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownTriggerRangeVoltage.Name = "numericUpDownTriggerRangeVoltage";
            this.numericUpDownTriggerRangeVoltage.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownTriggerRangeVoltage.TabIndex = 2;
            this.numericUpDownTriggerRangeVoltage.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // numericUpDownTriggerRangeCurrent
            // 
            this.numericUpDownTriggerRangeCurrent.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownTriggerRangeCurrent.Location = new System.Drawing.Point(98, 77);
            this.numericUpDownTriggerRangeCurrent.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownTriggerRangeCurrent.Name = "numericUpDownTriggerRangeCurrent";
            this.numericUpDownTriggerRangeCurrent.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownTriggerRangeCurrent.TabIndex = 5;
            this.numericUpDownTriggerRangeCurrent.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // labelTriggerRangeCurrent
            // 
            this.labelTriggerRangeCurrent.AutoSize = true;
            this.labelTriggerRangeCurrent.Location = new System.Drawing.Point(6, 79);
            this.labelTriggerRangeCurrent.Name = "labelTriggerRangeCurrent";
            this.labelTriggerRangeCurrent.Size = new System.Drawing.Size(62, 13);
            this.labelTriggerRangeCurrent.TabIndex = 49;
            this.labelTriggerRangeCurrent.Text = "Current DB:";
            // 
            // labelTriggerRangeVoltageUnits
            // 
            this.labelTriggerRangeVoltageUnits.AutoSize = true;
            this.labelTriggerRangeVoltageUnits.Location = new System.Drawing.Point(223, 21);
            this.labelTriggerRangeVoltageUnits.Name = "labelTriggerRangeVoltageUnits";
            this.labelTriggerRangeVoltageUnits.Size = new System.Drawing.Size(37, 13);
            this.labelTriggerRangeVoltageUnits.TabIndex = 51;
            this.labelTriggerRangeVoltageUnits.Text = "Volts *";
            // 
            // labelTriggerRangeTHDUnits
            // 
            this.labelTriggerRangeTHDUnits.AutoSize = true;
            this.labelTriggerRangeTHDUnits.Location = new System.Drawing.Point(223, 50);
            this.labelTriggerRangeTHDUnits.Name = "labelTriggerRangeTHDUnits";
            this.labelTriggerRangeTHDUnits.Size = new System.Drawing.Size(22, 13);
            this.labelTriggerRangeTHDUnits.TabIndex = 52;
            this.labelTriggerRangeTHDUnits.Text = "% *";
            // 
            // labelTriggerRangeCurrentUnits
            // 
            this.labelTriggerRangeCurrentUnits.AutoSize = true;
            this.labelTriggerRangeCurrentUnits.Location = new System.Drawing.Point(223, 79);
            this.labelTriggerRangeCurrentUnits.Name = "labelTriggerRangeCurrentUnits";
            this.labelTriggerRangeCurrentUnits.Size = new System.Drawing.Size(40, 13);
            this.labelTriggerRangeCurrentUnits.TabIndex = 54;
            this.labelTriggerRangeCurrentUnits.Text = "Amps *";
            // 
            // buttonRQDNPSettings
            // 
            // this.buttonRQDNPSettings.Location = new System.Drawing.Point(0, 526);
            this.buttonRQDNPSettings.Location = new System.Drawing.Point(60, 500);
            this.buttonRQDNPSettings.Name = "buttonRQDNPSettings";
            this.buttonRQDNPSettings.Size = new System.Drawing.Size(150, 23);
            this.buttonRQDNPSettings.TabIndex = 31;
            this.buttonRQDNPSettings.Text = "Request DNP Settings";
            this.buttonRQDNPSettings.UseVisualStyleBackColor = true;
            this.buttonRQDNPSettings.Click += new System.EventHandler(this.buttonRQDNPSettings_Click);
            // 
            // buttonSendAllDNPSettings
            // 
            this.buttonSendAllDNPSettings.Location = new System.Drawing.Point(114, 369);
            this.buttonSendAllDNPSettings.Name = "buttonSendAllDNPSettings";
            this.buttonSendAllDNPSettings.Size = new System.Drawing.Size(150, 23);
            this.buttonSendAllDNPSettings.TabIndex = 32;
            this.buttonSendAllDNPSettings.Text = "Send All DNP Settings";
            this.buttonSendAllDNPSettings.UseVisualStyleBackColor = true;
            this.buttonSendAllDNPSettings.Click += new System.EventHandler(this.buttonSendAllDNPSettings_Click);
            // 
            // labelZeroDisables
            // 
            this.labelZeroDisables.AutoSize = true;
            this.labelZeroDisables.Location = new System.Drawing.Point(441, 196);
            this.labelZeroDisables.Name = "labelZeroDisables";
            this.labelZeroDisables.Size = new System.Drawing.Size(122, 13);
            this.labelZeroDisables.TabIndex = 55;
            this.labelZeroDisables.Text = "*Zero Will Disable Event";
            // 
            // labelTriggerRangeTemperatureUnits
            // 
            this.labelTriggerRangeTemperatureUnits.AutoSize = true;
            this.labelTriggerRangeTemperatureUnits.Location = new System.Drawing.Point(223, 108);
            this.labelTriggerRangeTemperatureUnits.Name = "labelTriggerRangeTemperatureUnits";
            this.labelTriggerRangeTemperatureUnits.Size = new System.Drawing.Size(61, 13);
            this.labelTriggerRangeTemperatureUnits.TabIndex = 58;
            this.labelTriggerRangeTemperatureUnits.Text = "Degrees C*";
            // 
            // numericUpDownTriggerRangeTemperature
            // 
            this.numericUpDownTriggerRangeTemperature.DecimalPlaces = 1;
            this.numericUpDownTriggerRangeTemperature.Location = new System.Drawing.Point(98, 106);
            this.numericUpDownTriggerRangeTemperature.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            65536});
            this.numericUpDownTriggerRangeTemperature.Name = "numericUpDownTriggerRangeTemperature";
            this.numericUpDownTriggerRangeTemperature.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownTriggerRangeTemperature.TabIndex = 6;
            this.numericUpDownTriggerRangeTemperature.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // labelTriggerRangeTemperature
            // 
            this.labelTriggerRangeTemperature.AutoSize = true;
            this.labelTriggerRangeTemperature.Location = new System.Drawing.Point(6, 108);
            this.labelTriggerRangeTemperature.Name = "labelTriggerRangeTemperature";
            this.labelTriggerRangeTemperature.Size = new System.Drawing.Size(88, 13);
            this.labelTriggerRangeTemperature.TabIndex = 56;
            this.labelTriggerRangeTemperature.Text = "Temperature DB:";
            // 
            // buttonDefaults
            // 
            //this.buttonDefaults.Location = new System.Drawing.Point(0, 497);
            this.buttonDefaults.Location = new System.Drawing.Point(60, 448);
            this.buttonDefaults.Name = "buttonDefaults";
            this.buttonDefaults.Size = new System.Drawing.Size(150, 23);
            this.buttonDefaults.TabIndex = 59;
            this.buttonDefaults.Text = "Memphis Defaults";
            this.buttonDefaults.UseVisualStyleBackColor = true;
            this.buttonDefaults.Click += new System.EventHandler(this.buttonDefaults_Click);
            // 
            // groupBoxMemphisDeadBand
            // 
            this.groupBoxMemphisDeadBand.Controls.Add(this.buttonSendMemphis);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelAnalog4DeadBand);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownAnalog4DeadBand);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelAnalog4DeadBandUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelAnalog3DeadBandUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelTotalKWDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownAnalog3DeadBand);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelTotalKVAVARDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelAnalog3DeadBand);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelAnalog1DeadBand);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownTotalKWDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelAnalog2DeadBandUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownTotalKVAVARDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelAnalog1DeadBandUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownAnalog1DeadBand);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelTotalKVAVARDBUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelAnalog2DeadBand);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelTotalKWDBUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownAnalog2DeadBand);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelPhaseKVADBUNits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownPhaseKVADB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelPhaseKVADB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelPhaseKVARDBUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelPhaseKVARDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownPhaseKVARDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelPhaseKWDBUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelOdometer);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownPhaseKWDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelDifferentialVoltsDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelPhaseKWDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelDifferentialVoltsRealDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownOdometer);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelCurrentAngleDBUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownDifferentialVoltsDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelDifferentialVoltsRealDBUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownDifferentialVoltsRealDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelDifferentialVoltsDBUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelCurrentAngleDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelOdomoterUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownCurrentAngleDB);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelTriggerRangeTemperatureUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelZeroDisables);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelVoltageTriggerRange);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownTriggerRangeTemperature);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelTHDTriggerRange);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelTriggerRangeTemperature);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownTriggerRangeVoltage);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelTriggerRangeCurrentUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownTriggerRangeTHD);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelTriggerRangeTHDUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelTriggerRangeCurrent);
            this.groupBoxMemphisDeadBand.Controls.Add(this.labelTriggerRangeVoltageUnits);
            this.groupBoxMemphisDeadBand.Controls.Add(this.numericUpDownTriggerRangeCurrent);
            this.groupBoxMemphisDeadBand.Location = new System.Drawing.Point(398, 112);
            this.groupBoxMemphisDeadBand.Name = "groupBoxMemphisDeadBand";
            this.groupBoxMemphisDeadBand.Size = new System.Drawing.Size(606, 398);
            this.groupBoxMemphisDeadBand.TabIndex = 60;
            this.groupBoxMemphisDeadBand.TabStop = false;
            this.groupBoxMemphisDeadBand.Text = "Memphis Dead Band (DB) Variables";
            // 
            // buttonSendMemphis
            // 
            this.buttonSendMemphis.Location = new System.Drawing.Point(407, 321);
            this.buttonSendMemphis.Name = "buttonSendMemphis";
            this.buttonSendMemphis.Size = new System.Drawing.Size(136, 23);
            this.buttonSendMemphis.TabIndex = 143;
            this.buttonSendMemphis.Text = "Send Memphis Variables";
            this.buttonSendMemphis.UseVisualStyleBackColor = true;
            this.buttonSendMemphis.Click += new System.EventHandler(this.buttonSendMemphis_Click);
            // 
            // labelAnalog4DeadBand
            // 
            this.labelAnalog4DeadBand.AutoSize = true;
            this.labelAnalog4DeadBand.Location = new System.Drawing.Point(297, 168);
            this.labelAnalog4DeadBand.Name = "labelAnalog4DeadBand";
            this.labelAnalog4DeadBand.Size = new System.Drawing.Size(70, 13);
            this.labelAnalog4DeadBand.TabIndex = 125;
            this.labelAnalog4DeadBand.Text = "Analog 4 DB:";
            // 
            // numericUpDownAnalog4DeadBand
            // 
            this.numericUpDownAnalog4DeadBand.DecimalPlaces = 1;
            this.numericUpDownAnalog4DeadBand.Location = new System.Drawing.Point(407, 166);
            this.numericUpDownAnalog4DeadBand.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            65536});
            this.numericUpDownAnalog4DeadBand.Name = "numericUpDownAnalog4DeadBand";
            this.numericUpDownAnalog4DeadBand.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownAnalog4DeadBand.TabIndex = 19;
            this.numericUpDownAnalog4DeadBand.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // labelAnalog4DeadBandUnits
            // 
            this.labelAnalog4DeadBandUnits.AutoSize = true;
            this.labelAnalog4DeadBandUnits.Location = new System.Drawing.Point(532, 168);
            this.labelAnalog4DeadBandUnits.Name = "labelAnalog4DeadBandUnits";
            this.labelAnalog4DeadBandUnits.Size = new System.Drawing.Size(37, 13);
            this.labelAnalog4DeadBandUnits.TabIndex = 133;
            this.labelAnalog4DeadBandUnits.Text = "Volts *";
            // 
            // labelAnalog3DeadBandUnits
            // 
            this.labelAnalog3DeadBandUnits.AutoSize = true;
            this.labelAnalog3DeadBandUnits.Location = new System.Drawing.Point(532, 139);
            this.labelAnalog3DeadBandUnits.Name = "labelAnalog3DeadBandUnits";
            this.labelAnalog3DeadBandUnits.Size = new System.Drawing.Size(37, 13);
            this.labelAnalog3DeadBandUnits.TabIndex = 124;
            this.labelAnalog3DeadBandUnits.Text = "Volts *";
            // 
            // labelTotalKWDB
            // 
            this.labelTotalKWDB.AutoSize = true;
            this.labelTotalKWDB.Location = new System.Drawing.Point(297, 21);
            this.labelTotalKWDB.Name = "labelTotalKWDB";
            this.labelTotalKWDB.Size = new System.Drawing.Size(73, 13);
            this.labelTotalKWDB.TabIndex = 110;
            this.labelTotalKWDB.Text = "Total KW DB:";
            // 
            // numericUpDownAnalog3DeadBand
            // 
            this.numericUpDownAnalog3DeadBand.DecimalPlaces = 1;
            this.numericUpDownAnalog3DeadBand.Location = new System.Drawing.Point(407, 137);
            this.numericUpDownAnalog3DeadBand.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            65536});
            this.numericUpDownAnalog3DeadBand.Name = "numericUpDownAnalog3DeadBand";
            this.numericUpDownAnalog3DeadBand.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownAnalog3DeadBand.TabIndex = 18;
            this.numericUpDownAnalog3DeadBand.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // labelTotalKVAVARDB
            // 
            this.labelTotalKVAVARDB.AutoSize = true;
            this.labelTotalKVAVARDB.Location = new System.Drawing.Point(297, 50);
            this.labelTotalKVAVARDB.Name = "labelTotalKVAVARDB";
            this.labelTotalKVAVARDB.Size = new System.Drawing.Size(108, 13);
            this.labelTotalKVAVARDB.TabIndex = 111;
            this.labelTotalKVAVARDB.Text = "Total kVAR/kVA DB:";
            // 
            // labelAnalog3DeadBand
            // 
            this.labelAnalog3DeadBand.AutoSize = true;
            this.labelAnalog3DeadBand.Location = new System.Drawing.Point(297, 139);
            this.labelAnalog3DeadBand.Name = "labelAnalog3DeadBand";
            this.labelAnalog3DeadBand.Size = new System.Drawing.Size(67, 13);
            this.labelAnalog3DeadBand.TabIndex = 122;
            this.labelAnalog3DeadBand.Text = "Analog 3 DB";
            // 
            // labelAnalog1DeadBand
            // 
            this.labelAnalog1DeadBand.AutoSize = true;
            this.labelAnalog1DeadBand.Location = new System.Drawing.Point(297, 80);
            this.labelAnalog1DeadBand.Name = "labelAnalog1DeadBand";
            this.labelAnalog1DeadBand.Size = new System.Drawing.Size(61, 13);
            this.labelAnalog1DeadBand.TabIndex = 112;
            this.labelAnalog1DeadBand.Text = "Analog DB:";
            // 
            // numericUpDownTotalKWDB
            // 
            this.numericUpDownTotalKWDB.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownTotalKWDB.Location = new System.Drawing.Point(407, 19);
            this.numericUpDownTotalKWDB.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownTotalKWDB.Name = "numericUpDownTotalKWDB";
            this.numericUpDownTotalKWDB.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownTotalKWDB.TabIndex = 14;
            this.numericUpDownTotalKWDB.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // labelAnalog2DeadBandUnits
            // 
            this.labelAnalog2DeadBandUnits.AutoSize = true;
            this.labelAnalog2DeadBandUnits.Location = new System.Drawing.Point(532, 110);
            this.labelAnalog2DeadBandUnits.Name = "labelAnalog2DeadBandUnits";
            this.labelAnalog2DeadBandUnits.Size = new System.Drawing.Size(37, 13);
            this.labelAnalog2DeadBandUnits.TabIndex = 121;
            this.labelAnalog2DeadBandUnits.Text = "Volts *";
            // 
            // numericUpDownTotalKVAVARDB
            // 
            this.numericUpDownTotalKVAVARDB.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownTotalKVAVARDB.Location = new System.Drawing.Point(407, 48);
            this.numericUpDownTotalKVAVARDB.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownTotalKVAVARDB.Name = "numericUpDownTotalKVAVARDB";
            this.numericUpDownTotalKVAVARDB.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownTotalKVAVARDB.TabIndex = 15;
            this.numericUpDownTotalKVAVARDB.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // labelAnalog1DeadBandUnits
            // 
            this.labelAnalog1DeadBandUnits.AutoSize = true;
            this.labelAnalog1DeadBandUnits.Location = new System.Drawing.Point(532, 80);
            this.labelAnalog1DeadBandUnits.Name = "labelAnalog1DeadBandUnits";
            this.labelAnalog1DeadBandUnits.Size = new System.Drawing.Size(37, 13);
            this.labelAnalog1DeadBandUnits.TabIndex = 120;
            this.labelAnalog1DeadBandUnits.Text = "Volts *";
            // 
            // numericUpDownAnalog1DeadBand
            // 
            this.numericUpDownAnalog1DeadBand.DecimalPlaces = 1;
            this.numericUpDownAnalog1DeadBand.Location = new System.Drawing.Point(407, 78);
            this.numericUpDownAnalog1DeadBand.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            65536});
            this.numericUpDownAnalog1DeadBand.Name = "numericUpDownAnalog1DeadBand";
            this.numericUpDownAnalog1DeadBand.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownAnalog1DeadBand.TabIndex = 16;
            this.numericUpDownAnalog1DeadBand.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // labelTotalKVAVARDBUnits
            // 
            this.labelTotalKVAVARDBUnits.AutoSize = true;
            this.labelTotalKVAVARDBUnits.Location = new System.Drawing.Point(532, 50);
            this.labelTotalKVAVARDBUnits.Name = "labelTotalKVAVARDBUnits";
            this.labelTotalKVAVARDBUnits.Size = new System.Drawing.Size(67, 13);
            this.labelTotalKVAVARDBUnits.TabIndex = 119;
            this.labelTotalKVAVARDBUnits.Text = "kVA/kVAR *";
            // 
            // labelAnalog2DeadBand
            // 
            this.labelAnalog2DeadBand.AutoSize = true;
            this.labelAnalog2DeadBand.Location = new System.Drawing.Point(297, 110);
            this.labelAnalog2DeadBand.Name = "labelAnalog2DeadBand";
            this.labelAnalog2DeadBand.Size = new System.Drawing.Size(70, 13);
            this.labelAnalog2DeadBand.TabIndex = 116;
            this.labelAnalog2DeadBand.Text = "Analog 2 DB:";
            // 
            // labelTotalKWDBUnits
            // 
            this.labelTotalKWDBUnits.AutoSize = true;
            this.labelTotalKWDBUnits.Location = new System.Drawing.Point(532, 21);
            this.labelTotalKWDBUnits.Name = "labelTotalKWDBUnits";
            this.labelTotalKWDBUnits.Size = new System.Drawing.Size(31, 13);
            this.labelTotalKWDBUnits.TabIndex = 118;
            this.labelTotalKWDBUnits.Text = "kW *";
            // 
            // numericUpDownAnalog2DeadBand
            // 
            this.numericUpDownAnalog2DeadBand.DecimalPlaces = 1;
            this.numericUpDownAnalog2DeadBand.Location = new System.Drawing.Point(407, 108);
            this.numericUpDownAnalog2DeadBand.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            65536});
            this.numericUpDownAnalog2DeadBand.Name = "numericUpDownAnalog2DeadBand";
            this.numericUpDownAnalog2DeadBand.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownAnalog2DeadBand.TabIndex = 17;
            this.numericUpDownAnalog2DeadBand.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // labelPhaseKVADBUNits
            // 
            this.labelPhaseKVADBUNits.AutoSize = true;
            this.labelPhaseKVADBUNits.Location = new System.Drawing.Point(223, 321);
            this.labelPhaseKVADBUNits.Name = "labelPhaseKVADBUNits";
            this.labelPhaseKVADBUNits.Size = new System.Drawing.Size(34, 13);
            this.labelPhaseKVADBUNits.TabIndex = 109;
            this.labelPhaseKVADBUNits.Text = "kVA *";
            // 
            // numericUpDownPhaseKVADB
            // 
            this.numericUpDownPhaseKVADB.Location = new System.Drawing.Point(98, 319);
            this.numericUpDownPhaseKVADB.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownPhaseKVADB.Name = "numericUpDownPhaseKVADB";
            this.numericUpDownPhaseKVADB.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownPhaseKVADB.TabIndex = 13;
            this.numericUpDownPhaseKVADB.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // labelPhaseKVADB
            // 
            this.labelPhaseKVADB.AutoSize = true;
            this.labelPhaseKVADB.Location = new System.Drawing.Point(6, 321);
            this.labelPhaseKVADB.Name = "labelPhaseKVADB";
            this.labelPhaseKVADB.Size = new System.Drawing.Size(81, 13);
            this.labelPhaseKVADB.TabIndex = 107;
            this.labelPhaseKVADB.Text = "Phase kVA DB:";
            // 
            // labelPhaseKVARDBUnits
            // 
            this.labelPhaseKVARDBUnits.AutoSize = true;
            this.labelPhaseKVARDBUnits.Location = new System.Drawing.Point(223, 292);
            this.labelPhaseKVARDBUnits.Name = "labelPhaseKVARDBUnits";
            this.labelPhaseKVARDBUnits.Size = new System.Drawing.Size(42, 13);
            this.labelPhaseKVARDBUnits.TabIndex = 106;
            this.labelPhaseKVARDBUnits.Text = "kVAR *";
            // 
            // labelPhaseKVARDB
            // 
            this.labelPhaseKVARDB.AutoSize = true;
            this.labelPhaseKVARDB.Location = new System.Drawing.Point(6, 292);
            this.labelPhaseKVARDB.Name = "labelPhaseKVARDB";
            this.labelPhaseKVARDB.Size = new System.Drawing.Size(89, 13);
            this.labelPhaseKVARDB.TabIndex = 104;
            this.labelPhaseKVARDB.Text = "Phase kVAR DB:";
            // 
            // numericUpDownPhaseKVARDB
            // 
            this.numericUpDownPhaseKVARDB.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownPhaseKVARDB.Location = new System.Drawing.Point(98, 290);
            this.numericUpDownPhaseKVARDB.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownPhaseKVARDB.Name = "numericUpDownPhaseKVARDB";
            this.numericUpDownPhaseKVARDB.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownPhaseKVARDB.TabIndex = 12;
            this.numericUpDownPhaseKVARDB.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // labelPhaseKWDBUnits
            // 
            this.labelPhaseKWDBUnits.AutoSize = true;
            this.labelPhaseKWDBUnits.Location = new System.Drawing.Point(223, 262);
            this.labelPhaseKWDBUnits.Name = "labelPhaseKWDBUnits";
            this.labelPhaseKWDBUnits.Size = new System.Drawing.Size(31, 13);
            this.labelPhaseKWDBUnits.TabIndex = 73;
            this.labelPhaseKWDBUnits.Text = "kW *";
            // 
            // labelOdometer
            // 
            this.labelOdometer.AutoSize = true;
            this.labelOdometer.Location = new System.Drawing.Point(6, 138);
            this.labelOdometer.Name = "labelOdometer";
            this.labelOdometer.Size = new System.Drawing.Size(74, 13);
            this.labelOdometer.TabIndex = 59;
            this.labelOdometer.Text = "Odometer DB:";
            // 
            // numericUpDownPhaseKWDB
            // 
            this.numericUpDownPhaseKWDB.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownPhaseKWDB.Location = new System.Drawing.Point(98, 260);
            this.numericUpDownPhaseKWDB.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownPhaseKWDB.Name = "numericUpDownPhaseKWDB";
            this.numericUpDownPhaseKWDB.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownPhaseKWDB.TabIndex = 11;
            this.numericUpDownPhaseKWDB.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // labelDifferentialVoltsDB
            // 
            this.labelDifferentialVoltsDB.AutoSize = true;
            this.labelDifferentialVoltsDB.Location = new System.Drawing.Point(6, 167);
            this.labelDifferentialVoltsDB.Name = "labelDifferentialVoltsDB";
            this.labelDifferentialVoltsDB.Size = new System.Drawing.Size(83, 13);
            this.labelDifferentialVoltsDB.TabIndex = 60;
            this.labelDifferentialVoltsDB.Text = "Diff Voltage DB:";
            // 
            // labelPhaseKWDB
            // 
            this.labelPhaseKWDB.AutoSize = true;
            this.labelPhaseKWDB.Location = new System.Drawing.Point(6, 262);
            this.labelPhaseKWDB.Name = "labelPhaseKWDB";
            this.labelPhaseKWDB.Size = new System.Drawing.Size(78, 13);
            this.labelPhaseKWDB.TabIndex = 71;
            this.labelPhaseKWDB.Text = "Phase kW DB:";
            // 
            // labelDifferentialVoltsRealDB
            // 
            this.labelDifferentialVoltsRealDB.AutoSize = true;
            this.labelDifferentialVoltsRealDB.Location = new System.Drawing.Point(6, 198);
            this.labelDifferentialVoltsRealDB.Name = "labelDifferentialVoltsRealDB";
            this.labelDifferentialVoltsRealDB.Size = new System.Drawing.Size(90, 13);
            this.labelDifferentialVoltsRealDB.TabIndex = 61;
            this.labelDifferentialVoltsRealDB.Text = "Real Diff Volt DB:";
            // 
            // numericUpDownOdometer
            // 
            this.numericUpDownOdometer.Location = new System.Drawing.Point(98, 136);
            this.numericUpDownOdometer.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownOdometer.Name = "numericUpDownOdometer";
            this.numericUpDownOdometer.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownOdometer.TabIndex = 7;
            this.numericUpDownOdometer.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // labelCurrentAngleDBUnits
            // 
            this.labelCurrentAngleDBUnits.AutoSize = true;
            this.labelCurrentAngleDBUnits.Location = new System.Drawing.Point(223, 229);
            this.labelCurrentAngleDBUnits.Name = "labelCurrentAngleDBUnits";
            this.labelCurrentAngleDBUnits.Size = new System.Drawing.Size(51, 13);
            this.labelCurrentAngleDBUnits.TabIndex = 70;
            this.labelCurrentAngleDBUnits.Text = "Degrees*";
            // 
            // numericUpDownDifferentialVoltsDB
            // 
            this.numericUpDownDifferentialVoltsDB.DecimalPlaces = 1;
            this.numericUpDownDifferentialVoltsDB.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownDifferentialVoltsDB.Location = new System.Drawing.Point(98, 165);
            this.numericUpDownDifferentialVoltsDB.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            65536});
            this.numericUpDownDifferentialVoltsDB.Name = "numericUpDownDifferentialVoltsDB";
            this.numericUpDownDifferentialVoltsDB.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownDifferentialVoltsDB.TabIndex = 8;
            this.numericUpDownDifferentialVoltsDB.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            // 
            // labelDifferentialVoltsRealDBUnits
            // 
            this.labelDifferentialVoltsRealDBUnits.AutoSize = true;
            this.labelDifferentialVoltsRealDBUnits.Location = new System.Drawing.Point(223, 198);
            this.labelDifferentialVoltsRealDBUnits.Name = "labelDifferentialVoltsRealDBUnits";
            this.labelDifferentialVoltsRealDBUnits.Size = new System.Drawing.Size(37, 13);
            this.labelDifferentialVoltsRealDBUnits.TabIndex = 69;
            this.labelDifferentialVoltsRealDBUnits.Text = "Volts *";
            // 
            // numericUpDownDifferentialVoltsRealDB
            // 
            this.numericUpDownDifferentialVoltsRealDB.DecimalPlaces = 1;
            this.numericUpDownDifferentialVoltsRealDB.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownDifferentialVoltsRealDB.Location = new System.Drawing.Point(98, 196);
            this.numericUpDownDifferentialVoltsRealDB.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            65536});
            this.numericUpDownDifferentialVoltsRealDB.Name = "numericUpDownDifferentialVoltsRealDB";
            this.numericUpDownDifferentialVoltsRealDB.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownDifferentialVoltsRealDB.TabIndex = 9;
            this.numericUpDownDifferentialVoltsRealDB.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            // 
            // labelDifferentialVoltsDBUnits
            // 
            this.labelDifferentialVoltsDBUnits.AutoSize = true;
            this.labelDifferentialVoltsDBUnits.Location = new System.Drawing.Point(223, 167);
            this.labelDifferentialVoltsDBUnits.Name = "labelDifferentialVoltsDBUnits";
            this.labelDifferentialVoltsDBUnits.Size = new System.Drawing.Size(37, 13);
            this.labelDifferentialVoltsDBUnits.TabIndex = 68;
            this.labelDifferentialVoltsDBUnits.Text = "Volts *";
            // 
            // labelCurrentAngleDB
            // 
            this.labelCurrentAngleDB.AutoSize = true;
            this.labelCurrentAngleDB.Location = new System.Drawing.Point(6, 229);
            this.labelCurrentAngleDB.Name = "labelCurrentAngleDB";
            this.labelCurrentAngleDB.Size = new System.Drawing.Size(92, 13);
            this.labelCurrentAngleDB.TabIndex = 65;
            this.labelCurrentAngleDB.Text = "Current Angle DB:";
            // 
            // labelOdomoterUnits
            // 
            this.labelOdomoterUnits.AutoSize = true;
            this.labelOdomoterUnits.Location = new System.Drawing.Point(223, 138);
            this.labelOdomoterUnits.Name = "labelOdomoterUnits";
            this.labelOdomoterUnits.Size = new System.Drawing.Size(45, 13);
            this.labelOdomoterUnits.TabIndex = 67;
            this.labelOdomoterUnits.Text = "Cycles *";
            // 
            // numericUpDownCurrentAngleDB
            // 
            this.numericUpDownCurrentAngleDB.DecimalPlaces = 1;
            this.numericUpDownCurrentAngleDB.Location = new System.Drawing.Point(98, 227);
            this.numericUpDownCurrentAngleDB.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            65536});
            this.numericUpDownCurrentAngleDB.Name = "numericUpDownCurrentAngleDB";
            this.numericUpDownCurrentAngleDB.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownCurrentAngleDB.TabIndex = 10;
            this.numericUpDownCurrentAngleDB.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // groupBoxDNPSettings
            // 
            this.groupBoxDNPSettings.Controls.Add(this.comboBoxDNPBaudRate);
            this.groupBoxDNPSettings.Controls.Add(this.labelBaudRate);
            this.groupBoxDNPSettings.Controls.Add(this.labelLinkLayerConfirm);
            this.groupBoxDNPSettings.Controls.Add(this.comboBoxLinkLayerConfirm);
            this.groupBoxDNPSettings.Controls.Add(this.labelSelfAddress);
            this.groupBoxDNPSettings.Controls.Add(this.buttonSendAllDNPSettings);
            this.groupBoxDNPSettings.Controls.Add(this.comboBoxSelfAddress);
            this.groupBoxDNPSettings.Controls.Add(this.labelUnsolResponse);
            this.groupBoxDNPSettings.Controls.Add(this.comboBoxUnsolResponse);
            this.groupBoxDNPSettings.Controls.Add(this.comboBoxTerminationResistor);
            this.groupBoxDNPSettings.Controls.Add(this.labelUnsolTimeout);
            this.groupBoxDNPSettings.Controls.Add(this.labelTerminationResistor);
            this.groupBoxDNPSettings.Controls.Add(this.numericUpDownUnsolTimeout);
            this.groupBoxDNPSettings.Controls.Add(this.numericUpDownUnsolRetries);
            this.groupBoxDNPSettings.Controls.Add(this.labelFragmentSize);
            this.groupBoxDNPSettings.Controls.Add(this.labelUnsolRetries);
            this.groupBoxDNPSettings.Controls.Add(this.numericUpDownFragmentSize);
            this.groupBoxDNPSettings.Controls.Add(this.numericUpDownMaxEvents);
            this.groupBoxDNPSettings.Controls.Add(this.labelSourceAddress);
            this.groupBoxDNPSettings.Controls.Add(this.labelMaxEvents);
            this.groupBoxDNPSettings.Controls.Add(this.numericUpDownSourceAddress);
            this.groupBoxDNPSettings.Controls.Add(this.numericUpDownDestinationAddress);
            this.groupBoxDNPSettings.Controls.Add(this.labelDestinationAddress);
            this.groupBoxDNPSettings.Controls.Add(this.labelMemphisStage);
            this.groupBoxDNPSettings.Controls.Add(this.numericUpDownMemphisStage);
            this.groupBoxDNPSettings.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDNPSettings.Name = "groupBoxDNPSettings";
            this.groupBoxDNPSettings.Size = new System.Drawing.Size(269, 403);
            this.groupBoxDNPSettings.TabIndex = 61;
            this.groupBoxDNPSettings.TabStop = false;
            this.groupBoxDNPSettings.Text = "DNP Settings";
            // 
            // comboBoxDNPBaudRate
            // 
            this.comboBoxDNPBaudRate.FormattingEnabled = true;
            this.comboBoxDNPBaudRate.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "28800",
            "38400"});
            this.comboBoxDNPBaudRate.Location = new System.Drawing.Point(142, 316);
            this.comboBoxDNPBaudRate.Name = "comboBoxDNPBaudRate";
            this.comboBoxDNPBaudRate.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDNPBaudRate.TabIndex = 35;
            this.comboBoxDNPBaudRate.Text = "19200";
            // 
            // labelBaudRate
            // 
            this.labelBaudRate.AutoSize = true;
            this.labelBaudRate.Location = new System.Drawing.Point(5, 320);
            this.labelBaudRate.Name = "labelBaudRate";
            this.labelBaudRate.Size = new System.Drawing.Size(58, 13);
            this.labelBaudRate.TabIndex = 34;
            this.labelBaudRate.Text = "Baud Rate";
            // 
            // groupBoxDIGITALGRIDDNPDeadBand
            // 
            this.groupBoxDIGITALGRIDDNPDeadBand.Controls.Add(this.buttonSendDIGITALGRIDDeadBand);
            this.groupBoxDIGITALGRIDDNPDeadBand.Controls.Add(this.label29);
            this.groupBoxDIGITALGRIDDNPDeadBand.Location = new System.Drawing.Point(278, 0);
            this.groupBoxDIGITALGRIDDNPDeadBand.Name = "groupBoxDIGITALGRIDDNPDeadBand";
            this.groupBoxDIGITALGRIDDNPDeadBand.Size = new System.Drawing.Size(606, 590);
            this.groupBoxDIGITALGRIDDNPDeadBand.TabIndex = 144;
            this.groupBoxDIGITALGRIDDNPDeadBand.TabStop = false;
            this.groupBoxDIGITALGRIDDNPDeadBand.Text = "DNP Dead Band (DB) Variables";
            // 
            // buttonSendDIGITALGRIDDeadBand
            // 
            this.buttonSendDIGITALGRIDDeadBand.Location = new System.Drawing.Point(448, 553);
            this.buttonSendDIGITALGRIDDeadBand.Name = "buttonSendDIGITALGRIDDeadBand";
            this.buttonSendDIGITALGRIDDeadBand.Size = new System.Drawing.Size(136, 23);
            this.buttonSendDIGITALGRIDDeadBand.TabIndex = 143;
            this.buttonSendDIGITALGRIDDeadBand.Text = "Send DNP DeadBands";
            this.buttonSendDIGITALGRIDDeadBand.UseVisualStyleBackColor = true;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(6, 563);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(122, 13);
            this.label29.TabIndex = 55;
            this.label29.Text = "*Zero Will Disable Event";
            // 
            // buttonSendDeadBand
            // 
            this.buttonSendDeadBand.Location = new System.Drawing.Point(911, 553);
            this.buttonSendDeadBand.Name = "buttonSendDeadBand";
            this.buttonSendDeadBand.Size = new System.Drawing.Size(101, 23);
            this.buttonSendDeadBand.TabIndex = 144;
            this.buttonSendDeadBand.Text = "Send Dead Band";
            this.buttonSendDeadBand.UseVisualStyleBackColor = true;
            this.buttonSendDeadBand.Click += new System.EventHandler(this.buttonSendDeadBand_Click);
            // 
            // labelSAv5AggressiveMode
            // 
            this.labelSAv5AggressiveMode.AutoSize = true;
            this.labelSAv5AggressiveMode.Location = new System.Drawing.Point(9, 409);
            this.labelSAv5AggressiveMode.Name = "labelSAv5AggressiveMode";
            this.labelSAv5AggressiveMode.Size = new System.Drawing.Size(92, 13);
            this.labelSAv5AggressiveMode.TabIndex = 34;
            this.labelSAv5AggressiveMode.Text = "Aggressive Mode:";
            this.labelSAv5AggressiveMode.Visible = false;
            // 
            // comboBoxSAv5AggressiveMode
            // 
            this.comboBoxSAv5AggressiveMode.FormattingEnabled = true;
            this.comboBoxSAv5AggressiveMode.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.comboBoxSAv5AggressiveMode.Location = new System.Drawing.Point(146, 406);
            this.comboBoxSAv5AggressiveMode.Name = "comboBoxSAv5AggressiveMode";
            this.comboBoxSAv5AggressiveMode.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSAv5AggressiveMode.TabIndex = 35;
            this.comboBoxSAv5AggressiveMode.Text = "Disable";
            this.comboBoxSAv5AggressiveMode.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(395, 288);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 145;
            this.label2.Text = "Aggressive Mode:";
            this.label2.Visible = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.comboBox1.Location = new System.Drawing.Point(532, 285);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 146;
            this.comboBox1.Text = "Disable";
            this.comboBox1.Visible = false;
            // 
            // numericUpDownSAv5UserNumber
            // 
            this.numericUpDownSAv5UserNumber.Location = new System.Drawing.Point(147, 429);
            this.numericUpDownSAv5UserNumber.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownSAv5UserNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSAv5UserNumber.Name = "numericUpDownSAv5UserNumber";
            this.numericUpDownSAv5UserNumber.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownSAv5UserNumber.TabIndex = 35;
            this.numericUpDownSAv5UserNumber.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownSAv5UserNumber.Visible = false;
            // 
            // labelSAv5UserNumber
            // 
            this.labelSAv5UserNumber.AutoSize = true;
            this.labelSAv5UserNumber.Location = new System.Drawing.Point(9, 433);
            this.labelSAv5UserNumber.Name = "labelSAv5UserNumber";
            this.labelSAv5UserNumber.Size = new System.Drawing.Size(72, 13);
            this.labelSAv5UserNumber.TabIndex = 34;
            this.labelSAv5UserNumber.Text = "User Number:";
            this.labelSAv5UserNumber.Visible = false;
            // 
            // labelSAv5UserKey
            // 
            this.labelSAv5UserKey.AutoSize = true;
            this.labelSAv5UserKey.Location = new System.Drawing.Point(11, 458);
            this.labelSAv5UserKey.Name = "labelSAv5UserKey";
            this.labelSAv5UserKey.Size = new System.Drawing.Size(91, 13);
            this.labelSAv5UserKey.TabIndex = 147;
            this.labelSAv5UserKey.Text = "User Update Key:";
            this.labelSAv5UserKey.Visible = false;
            // 
            // textBoxSAv5UserUpdateKey
            // 
            this.textBoxSAv5UserUpdateKey.Location = new System.Drawing.Point(146, 458);
            this.textBoxSAv5UserUpdateKey.Name = "textBoxSAv5UserUpdateKey";
            this.textBoxSAv5UserUpdateKey.Size = new System.Drawing.Size(121, 20);
            this.textBoxSAv5UserUpdateKey.TabIndex = 148;
            this.textBoxSAv5UserUpdateKey.Visible = false;
            // 
            // labelDNPtext1
            // 
            this.labelDNPtext1.AutoSize = true;
            this.labelDNPtext1.Location = new System.Drawing.Point(6, 24);
            this.labelDNPtext1.Name = "labelDNPtext1";
            this.labelDNPtext1.Size = new System.Drawing.Size(37, 13);
            this.labelDNPtext1.TabIndex = 149;
            this.labelDNPtext1.Text = "Status";
            // 
            // labelDNPStatusInidcation
            // 
            this.labelDNPStatusInidcation.AutoSize = true;
            this.labelDNPStatusInidcation.BackColor = System.Drawing.SystemColors.ControlDark;
            this.labelDNPStatusInidcation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDNPStatusInidcation.Location = new System.Drawing.Point(47, 21);
            this.labelDNPStatusInidcation.Name = "labelDNPStatusInidcation";
            this.labelDNPStatusInidcation.Size = new System.Drawing.Size(63, 16);
            this.labelDNPStatusInidcation.TabIndex = 150;
            this.labelDNPStatusInidcation.Text = "Unknown";
            // 
            // groupBoxDNPStatus
            // 
            this.groupBoxDNPStatus.Controls.Add(this.labelDNPtext1);
            this.groupBoxDNPStatus.Controls.Add(this.labelDNPStatusInidcation);
            this.groupBoxDNPStatus.Location = new System.Drawing.Point(155, 497);
            this.groupBoxDNPStatus.Name = "groupBoxDNPStatus";
            this.groupBoxDNPStatus.Size = new System.Drawing.Size(116, 52);
            this.groupBoxDNPStatus.TabIndex = 151;
            this.groupBoxDNPStatus.TabStop = false;
            this.groupBoxDNPStatus.Text = "DNP Status";
            this.groupBoxDNPStatus.Visible = false;
            // 
            // ucDNP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMemphisDeadBand);
            this.Controls.Add(this.groupBoxDNPStatus);
            this.Controls.Add(this.textBoxSAv5UserUpdateKey);
            this.Controls.Add(this.labelSAv5UserKey);
            this.Controls.Add(this.numericUpDownSAv5UserNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelSAv5UserNumber);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.labelSAv5AggressiveMode);
            this.Controls.Add(this.comboBoxSAv5AggressiveMode);
            this.Controls.Add(this.buttonSendDeadBand);
            this.Controls.Add(this.groupBoxDIGITALGRIDDNPDeadBand);
            this.Controls.Add(this.groupBoxDNPSettings);
            this.Controls.Add(this.buttonDefaults);
            this.Controls.Add(this.buttonRQDNPSettings);
            this.Name = "ucDNP";
            this.Size = new System.Drawing.Size(1048, 590);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUnsolTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFragmentSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDestinationAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSourceAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxEvents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUnsolRetries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMemphisStage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTriggerRangeTHD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTriggerRangeVoltage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTriggerRangeCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTriggerRangeTemperature)).EndInit();
            this.groupBoxMemphisDeadBand.ResumeLayout(false);
            this.groupBoxMemphisDeadBand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnalog4DeadBand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnalog3DeadBand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTotalKWDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTotalKVAVARDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnalog1DeadBand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnalog2DeadBand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPhaseKVADB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPhaseKVARDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPhaseKWDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOdometer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDifferentialVoltsDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDifferentialVoltsRealDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCurrentAngleDB)).EndInit();
            this.groupBoxDNPSettings.ResumeLayout(false);
            this.groupBoxDNPSettings.PerformLayout();
            this.groupBoxDIGITALGRIDDNPDeadBand.ResumeLayout(false);
            this.groupBoxDIGITALGRIDDNPDeadBand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSAv5UserNumber)).EndInit();
            this.groupBoxDNPStatus.ResumeLayout(false);
            this.groupBoxDNPStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLinkLayerConfirm;
        private System.Windows.Forms.ComboBox comboBoxLinkLayerConfirm;
        private System.Windows.Forms.ComboBox comboBoxSelfAddress;
        private System.Windows.Forms.Label labelSelfAddress;
        private System.Windows.Forms.ComboBox comboBoxUnsolResponse;
        private System.Windows.Forms.Label labelUnsolResponse;
        private System.Windows.Forms.Label labelUnsolTimeout;
        private System.Windows.Forms.NumericUpDown numericUpDownUnsolTimeout;
        private System.Windows.Forms.NumericUpDown numericUpDownFragmentSize;
        private System.Windows.Forms.Label labelFragmentSize;
        private System.Windows.Forms.NumericUpDown numericUpDownDestinationAddress;
        private System.Windows.Forms.Label labelDestinationAddress;
        private System.Windows.Forms.NumericUpDown numericUpDownSourceAddress;
        private System.Windows.Forms.Label labelSourceAddress;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxEvents;
        private System.Windows.Forms.Label labelMaxEvents;
        private System.Windows.Forms.NumericUpDown numericUpDownUnsolRetries;
        private System.Windows.Forms.Label labelUnsolRetries;
        private System.Windows.Forms.Label labelTerminationResistor;
        private System.Windows.Forms.ComboBox comboBoxTerminationResistor;
        private System.Windows.Forms.Label labelMemphisStage;
        private System.Windows.Forms.NumericUpDown numericUpDownMemphisStage;
        private System.Windows.Forms.Label labelTHDTriggerRange;
        private System.Windows.Forms.Label labelVoltageTriggerRange;
        private System.Windows.Forms.NumericUpDown numericUpDownTriggerRangeTHD;
        private System.Windows.Forms.NumericUpDown numericUpDownTriggerRangeVoltage;
        private System.Windows.Forms.NumericUpDown numericUpDownTriggerRangeCurrent;
        private System.Windows.Forms.Label labelTriggerRangeCurrent;
        private System.Windows.Forms.Label labelTriggerRangeVoltageUnits;
        private System.Windows.Forms.Label labelTriggerRangeTHDUnits;
        private System.Windows.Forms.Label labelTriggerRangeCurrentUnits;
        public System.Windows.Forms.Button buttonRQDNPSettings;
        public System.Windows.Forms.Button buttonSendAllDNPSettings;
        private System.Windows.Forms.Label labelZeroDisables;
        private System.Windows.Forms.Label labelTriggerRangeTemperatureUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownTriggerRangeTemperature;
        private System.Windows.Forms.Label labelTriggerRangeTemperature;
        private System.Windows.Forms.Button buttonDefaults;
        private System.Windows.Forms.GroupBox groupBoxMemphisDeadBand;
        private System.Windows.Forms.GroupBox groupBoxDNPSettings;
        private System.Windows.Forms.Label labelPhaseKWDBUnits;
        private System.Windows.Forms.Label labelOdometer;
        private System.Windows.Forms.NumericUpDown numericUpDownPhaseKWDB;
        private System.Windows.Forms.Label labelDifferentialVoltsDB;
        private System.Windows.Forms.Label labelPhaseKWDB;
        private System.Windows.Forms.Label labelDifferentialVoltsRealDB;
        private System.Windows.Forms.NumericUpDown numericUpDownOdometer;
        private System.Windows.Forms.Label labelCurrentAngleDBUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownDifferentialVoltsDB;
        private System.Windows.Forms.Label labelDifferentialVoltsRealDBUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownDifferentialVoltsRealDB;
        private System.Windows.Forms.Label labelDifferentialVoltsDBUnits;
        private System.Windows.Forms.Label labelCurrentAngleDB;
        private System.Windows.Forms.Label labelOdomoterUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownCurrentAngleDB;
        private System.Windows.Forms.Label labelPhaseKVADBUNits;
        private System.Windows.Forms.NumericUpDown numericUpDownPhaseKVADB;
        private System.Windows.Forms.Label labelPhaseKVADB;
        private System.Windows.Forms.Label labelPhaseKVARDBUnits;
        private System.Windows.Forms.Label labelPhaseKVARDB;
        private System.Windows.Forms.NumericUpDown numericUpDownPhaseKVARDB;
        private System.Windows.Forms.Label labelAnalog4DeadBand;
        private System.Windows.Forms.NumericUpDown numericUpDownAnalog4DeadBand;
        private System.Windows.Forms.Label labelAnalog4DeadBandUnits;
        private System.Windows.Forms.Label labelAnalog3DeadBandUnits;
        private System.Windows.Forms.Label labelTotalKWDB;
        private System.Windows.Forms.NumericUpDown numericUpDownAnalog3DeadBand;
        private System.Windows.Forms.Label labelTotalKVAVARDB;
        private System.Windows.Forms.Label labelAnalog3DeadBand;
        private System.Windows.Forms.Label labelAnalog1DeadBand;
        private System.Windows.Forms.NumericUpDown numericUpDownTotalKWDB;
        private System.Windows.Forms.Label labelAnalog2DeadBandUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownTotalKVAVARDB;
        private System.Windows.Forms.Label labelAnalog1DeadBandUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownAnalog1DeadBand;
        private System.Windows.Forms.Label labelTotalKVAVARDBUnits;
        private System.Windows.Forms.Label labelAnalog2DeadBand;
        private System.Windows.Forms.Label labelTotalKWDBUnits;
        private System.Windows.Forms.NumericUpDown numericUpDownAnalog2DeadBand;
        private System.Windows.Forms.Button buttonSendMemphis;
        private System.Windows.Forms.GroupBox groupBoxDIGITALGRIDDNPDeadBand;
        private System.Windows.Forms.Button buttonSendDIGITALGRIDDeadBand;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button buttonSendDeadBand;
        private System.Windows.Forms.Label labelSAv5AggressiveMode;
        private System.Windows.Forms.ComboBox comboBoxSAv5AggressiveMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown numericUpDownSAv5UserNumber;
        private System.Windows.Forms.Label labelSAv5UserNumber;
        private System.Windows.Forms.Label labelSAv5UserKey;
        private System.Windows.Forms.TextBox textBoxSAv5UserUpdateKey;
        private System.Windows.Forms.ComboBox comboBoxDNPBaudRate;
        private System.Windows.Forms.Label labelBaudRate;
        public System.Windows.Forms.Label labelDNPtext1;
        public System.Windows.Forms.Label labelDNPStatusInidcation;
        public System.Windows.Forms.GroupBox groupBoxDNPStatus;
    }
}
