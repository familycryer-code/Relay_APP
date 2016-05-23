namespace RelayControl
{
    partial class MainControl
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label labelTemperature;
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findRelayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reprogramRelayFileSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAction = new System.Windows.Forms.ToolStripMenuItem();
            this.eventActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadEventFromRelayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadEventSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liveDataActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.requestLiveDataToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLiveDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadLiveDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acknowledgeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cTRatioCalculatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableAutoloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerSCITimeOut = new System.Windows.Forms.Timer(this.components);
            this.timerCheckPortTime = new System.Windows.Forms.Timer(this.components);
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelRelayDisconnected = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelReceiverStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerTimeOutCountdown = new System.Windows.Forms.Timer(this.components);
            this.timerRegisterPolling = new System.Windows.Forms.Timer(this.components);
            this.tabPageTransmitterMonitoring = new System.Windows.Forms.TabPage();
            this.ucTransmitterMonitoring1 = new RelayControlLibrary.ucTransmitterMonitoring();
            this.labelRelayDisconnected2 = new System.Windows.Forms.Label();
            this.tabPageEngineering = new System.Windows.Forms.TabPage();
            this.ucGeneralCommandHandler1 = new RelayControlLibrary.ucGeneralCommandHandler();
            this.groupBoxTimeConvert = new System.Windows.Forms.GroupBox();
            this.textBoxTimeOutput = new System.Windows.Forms.TextBox();
            this.textBoxTimeInput = new System.Windows.Forms.TextBox();
            this.buttonTimeConvert = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonToggleQuietMode = new System.Windows.Forms.Button();
            this.buttonFPGAProcVersion = new System.Windows.Forms.Button();
            this.buttonCauseEvent = new System.Windows.Forms.Button();
            this.buttonSendTime = new System.Windows.Forms.Button();
            this.buttonUpdateDisplay = new System.Windows.Forms.Button();
            this.buttonRSTRelay = new System.Windows.Forms.Button();
            this.buttonResetMaster = new System.Windows.Forms.Button();
            this.buttonRQRelayProcVersion = new System.Windows.Forms.Button();
            this.buttonForceI = new System.Windows.Forms.Button();
            this.buttonRequestRelayRegisters = new System.Windows.Forms.Button();
            this.ucRelayProgramming1 = new RelayControlLibrary.ucRelayProgramming();
            this.uc8CheckBoxFlagsGEControl2 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.uc8CheckBoxFlagsGEControl1 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.ucCSVConverterCSVFile1 = new RelayControlLibrary.ucCSVConverterCSVFile();
            this.ucCalibration2 = new RelayControlLibrary.ucCalibration();
            this.uc8CheckBoxFlagsCommFlags2 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.uc8CheckBoxFlagsCommFlags1 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.uc8CheckBoxFlagsRelayStatus2 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.uc8CheckBoxFlagsRelayStatus1 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.uc8CheckBoxFlagsRelayFlags2 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.uc8CheckBoxFlagsRelayFlags1 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.ucCalibration1 = new RelayControlLibrary.ucManualCalibration();
            this.ucForceCustomerSwitch1 = new RelayControl.ucForceCustomerSwitch();
            this.tabPageEvents = new System.Windows.Forms.TabPage();
            this.buttonClearEvents = new System.Windows.Forms.Button();
            this.buttonRQEventData = new System.Windows.Forms.Button();
            this.panelEventSelect = new System.Windows.Forms.Panel();
            this.radioButtonEvent7 = new System.Windows.Forms.RadioButton();
            this.radioButtonEvent6 = new System.Windows.Forms.RadioButton();
            this.radioButtonEvent5 = new System.Windows.Forms.RadioButton();
            this.radioButtonEvent4 = new System.Windows.Forms.RadioButton();
            this.radioButtonEvent3 = new System.Windows.Forms.RadioButton();
            this.radioButtonEvent2 = new System.Windows.Forms.RadioButton();
            this.radioButtonEvent1 = new System.Windows.Forms.RadioButton();
            this.radioButtonEvent0 = new System.Windows.Forms.RadioButton();
            this.ucEventGraph7 = new SineDisplayGraph.ucEventGraph();
            this.ucEventGraph6 = new SineDisplayGraph.ucEventGraph();
            this.ucEventGraph5 = new SineDisplayGraph.ucEventGraph();
            this.ucEventGraph4 = new SineDisplayGraph.ucEventGraph();
            this.ucEventGraph3 = new SineDisplayGraph.ucEventGraph();
            this.ucEventGraph2 = new SineDisplayGraph.ucEventGraph();
            this.ucEventGraph1 = new SineDisplayGraph.ucEventGraph();
            this.ucEventGraph0 = new SineDisplayGraph.ucEventGraph();
            this.tabPageFlightRecorder = new System.Windows.Forms.TabPage();
            this.labelLiveDataTriggerTime = new System.Windows.Forms.Label();
            this.ucLiveData1 = new SineDisplayGraph.ucLiveData();
            this.tabPageTransmitter = new System.Windows.Forms.TabPage();
            this.labelRelayDisconnected3 = new System.Windows.Forms.Label();
            this.ucTransmitter1 = new RelayControlLibrary.ucTransmitter();
            this.tabPageMonitor = new System.Windows.Forms.TabPage();
            this.checkBox277ProtectorPQ = new System.Windows.Forms.CheckBox();
            this.labelSNPQMonitor = new System.Windows.Forms.Label();
            this.textBoxRelaySNControlPQ = new System.Windows.Forms.TextBox();
            this.textBoxCTRatioPQMonitor = new System.Windows.Forms.TextBox();
            this.checkBoxInTripRegion = new System.Windows.Forms.CheckBox();
            this.textBoxTemperatureMonitoringPage = new System.Windows.Forms.TextBox();
            this.labelTemperatureMonitoringPage = new System.Windows.Forms.Label();
            this.buttonUpdateCTRatio = new System.Windows.Forms.Button();
            this.labelRelayTrippedOrClose = new System.Windows.Forms.Label();
            this.buttonToggleMonitor = new System.Windows.Forms.Button();
            this.labelCtRatioMonitor = new System.Windows.Forms.Label();
            this.ucPhasorGraph1 = new SineDisplayGraph.ucPhasorGraph();
            this.tabPageControl = new System.Windows.Forms.TabPage();
            this.groupBoxLRLockoutMain = new System.Windows.Forms.GroupBox();
            this.textBoxLRLockoutStatusMain = new System.Windows.Forms.TextBox();
            this.labelLRLockoutMain = new System.Windows.Forms.Label();
            this.groupBoxLowVoltThres = new System.Windows.Forms.GroupBox();
            this.buttonRequestLowVotlageThres = new System.Windows.Forms.Button();
            this.numericUpDownLowVoltageThres = new System.Windows.Forms.NumericUpDown();
            this.buttonSendLowVoltageThres = new System.Windows.Forms.Button();
            this.groupBoxRelayStatus = new System.Windows.Forms.GroupBox();
            this.labelNWPStatus = new System.Windows.Forms.Label();
            this.checkBoxTripFlag = new System.Windows.Forms.CheckBox();
            this.checkBoxPhasingOkayFlag = new System.Windows.Forms.CheckBox();
            this.checkBoxPumping = new System.Windows.Forms.CheckBox();
            this.textBoxTemperature = new System.Windows.Forms.TextBox();
            this.checkBoxDefaultsUsed = new System.Windows.Forms.CheckBox();
            this.checkBoxBlockedOpenFlag = new System.Windows.Forms.CheckBox();
            this.textBoxTripCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxBFlag = new System.Windows.Forms.CheckBox();
            this.checkBoxTrippingFlag = new System.Windows.Forms.CheckBox();
            this.checkBoxFloatFlag = new System.Windows.Forms.CheckBox();
            this.groupBoxPhasingAndType = new System.Windows.Forms.GroupBox();
            this.labelConEdPowerRelay = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.labelProtectorType = new System.Windows.Forms.Label();
            this.domainUpDownPhasings = new System.Windows.Forms.DomainUpDown();
            this.labelGEWH = new System.Windows.Forms.Label();
            this.domainUpDownRelayType = new System.Windows.Forms.DomainUpDown();
            this.buttonTypePhasingRestoreDefaults = new System.Windows.Forms.Button();
            this.buttonRelayType = new System.Windows.Forms.Button();
            this.groupBoxNetworkCTRatio = new System.Windows.Forms.GroupBox();
            this.labelOver5 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.buttonSendCTRatio = new System.Windows.Forms.Button();
            this.textBoxCTRatio = new System.Windows.Forms.TextBox();
            this.domainUpDownCTRatioM = new System.Windows.Forms.DomainUpDown();
            this.groupBoxRelayFlags = new System.Windows.Forms.GroupBox();
            this.labelQuietMode = new System.Windows.Forms.Label();
            this.checkBoxOffsetOkay = new System.Windows.Forms.CheckBox();
            this.checkBoxCalibrating = new System.Windows.Forms.CheckBox();
            this.checkBoxMathOverTime = new System.Windows.Forms.CheckBox();
            this.checkBoxBlockedCloseFlag = new System.Windows.Forms.CheckBox();
            this.checkBoxMathError = new System.Windows.Forms.CheckBox();
            this.checkBoxInInsensRegion = new System.Windows.Forms.CheckBox();
            this.checkBoxMonitorPhasors = new System.Windows.Forms.CheckBox();
            this.checkBoxFlag2 = new System.Windows.Forms.CheckBox();
            this.checkBoxSequence = new System.Windows.Forms.CheckBox();
            this.checkBoxACB = new System.Windows.Forms.CheckBox();
            this.checkBoxFlag1 = new System.Windows.Forms.CheckBox();
            this.checkBoxPowerSaveFlag = new System.Windows.Forms.CheckBox();
            this.ucSafeService1 = new RelayControlLibrary.ucSafeService();
            this.panelOtherRelayControls = new System.Windows.Forms.Panel();
            this.buttonClearCycleCount = new System.Windows.Forms.Button();
            this.buttonBlockAndTrip = new System.Windows.Forms.Button();
            this.buttonBlockedStateOpen = new System.Windows.Forms.Button();
            this.buttonRequestRelayParamaters = new System.Windows.Forms.Button();
            this.labelRelaySNControl = new System.Windows.Forms.Label();
            this.buttonUnblockOpen = new System.Windows.Forms.Button();
            this.textBoxRelaySNControl = new System.Windows.Forms.TextBox();
            this.labelRevision = new System.Windows.Forms.Label();
            this.labelFPGARevision = new System.Windows.Forms.Label();
            this.labelRelayRevision = new System.Windows.Forms.Label();
            this.buttonResetBothProc = new System.Windows.Forms.Button();
            this.labelRelayStateControlPage = new System.Windows.Forms.Label();
            this.buttonSendAll = new System.Windows.Forms.Button();
            this.labelBlockedOpenState = new System.Windows.Forms.Label();
            this.textBoxSaveStateName = new System.Windows.Forms.TextBox();
            this.buttonTripRelay = new System.Windows.Forms.Button();
            this.labelRelayDisconnected = new System.Windows.Forms.Label();
            this.comboBoxSavedStates = new System.Windows.Forms.ComboBox();
            this.buttonSaveSetting = new System.Windows.Forms.Button();
            this.buttonDeleteSetting = new System.Windows.Forms.Button();
            this.checkBox277Protector = new System.Windows.Forms.CheckBox();
            this.ucTripMode2 = new RelayControlLibrary.ucTripMode();
            this.ucCloseMode1 = new RelayControlLibrary.ucCloseMode();
            this.ucPumpMode1 = new RelayControlLibrary.ucPumpMode();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageDNP = new System.Windows.Forms.TabPage();
            this.buttonResetRelay2 = new System.Windows.Forms.Button();
            this.ucDNP1 = new RelayControlLibrary.ucDNP();
            this.tabPageArcFault = new System.Windows.Forms.TabPage();
            this.buttonArcFaultStartMonitoring = new System.Windows.Forms.Button();
            this.ucArcFault1 = new RelayControlLibrary.ucArcFault();
            this.tabPageShortRange = new System.Windows.Forms.TabPage();
            this.ucShortRange1 = new RelayControlLibrary.ucShortRange();
            this.tabPageDNPData = new System.Windows.Forms.TabPage();
            this.buttonRequestDNPData = new System.Windows.Forms.Button();
            this.tabPageDNPSecureAuth = new System.Windows.Forms.TabPage();
            this.ucDNPSAv51 = new RelayDNPSecurity.ucDNPSAv5();
            this.timerResponseTimeOut = new System.Windows.Forms.Timer(this.components);
            this.timerScreenCapDelay = new System.Windows.Forms.Timer(this.components);
            this.timerFindRelayTimeout = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new RelayControl.MyPort(this.components);
            labelTemperature = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.tabPageTransmitterMonitoring.SuspendLayout();
            this.tabPageEngineering.SuspendLayout();
            this.groupBoxTimeConvert.SuspendLayout();
            this.tabPageEvents.SuspendLayout();
            this.panelEventSelect.SuspendLayout();
            this.tabPageFlightRecorder.SuspendLayout();
            this.tabPageTransmitter.SuspendLayout();
            this.tabPageMonitor.SuspendLayout();
            this.tabPageControl.SuspendLayout();
            this.groupBoxLRLockoutMain.SuspendLayout();
            this.groupBoxLowVoltThres.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLowVoltageThres)).BeginInit();
            this.groupBoxRelayStatus.SuspendLayout();
            this.groupBoxPhasingAndType.SuspendLayout();
            this.groupBoxNetworkCTRatio.SuspendLayout();
            this.groupBoxRelayFlags.SuspendLayout();
            this.panelOtherRelayControls.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageDNP.SuspendLayout();
            this.tabPageArcFault.SuspendLayout();
            this.tabPageShortRange.SuspendLayout();
            this.tabPageDNPData.SuspendLayout();
            this.tabPageDNPSecureAuth.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTemperature
            // 
            labelTemperature.AutoSize = true;
            labelTemperature.Location = new System.Drawing.Point(62, 220);
            labelTemperature.Name = "labelTemperature";
            labelTemperature.Size = new System.Drawing.Size(53, 13);
            labelTemperature.TabIndex = 50;
            labelTemperature.Text = "Temp (C):";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OptionsToolStripMenuItem,
            this.toolStripMenuItemAction,
            this.acknowledgeToolStripMenuItem1,
            this.sToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.loadConfigurationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(999, 24);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // OptionsToolStripMenuItem
            // 
            this.OptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cOMPortToolStripMenuItem,
            this.findRelayToolStripMenuItem,
            this.enableAllToolStripMenuItem,
            this.reprogramRelayFileSelectToolStripMenuItem});
            this.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
            this.OptionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.OptionsToolStripMenuItem.Text = "Options";
            this.OptionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
            // 
            // cOMPortToolStripMenuItem
            // 
            this.cOMPortToolStripMenuItem.Name = "cOMPortToolStripMenuItem";
            this.cOMPortToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.cOMPortToolStripMenuItem.Text = "COM Port";
            // 
            // findRelayToolStripMenuItem
            // 
            this.findRelayToolStripMenuItem.Name = "findRelayToolStripMenuItem";
            this.findRelayToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.findRelayToolStripMenuItem.Text = "Find Relay";
            this.findRelayToolStripMenuItem.Click += new System.EventHandler(this.findRelayToolStripMenuItem_Click);
            // 
            // enableAllToolStripMenuItem
            // 
            this.enableAllToolStripMenuItem.Name = "enableAllToolStripMenuItem";
            this.enableAllToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.enableAllToolStripMenuItem.Text = "Enable All";
            this.enableAllToolStripMenuItem.Click += new System.EventHandler(this.enableAllToolStripMenuItem_Click);
            // 
            // reprogramRelayFileSelectToolStripMenuItem
            // 
            this.reprogramRelayFileSelectToolStripMenuItem.Name = "reprogramRelayFileSelectToolStripMenuItem";
            this.reprogramRelayFileSelectToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.reprogramRelayFileSelectToolStripMenuItem.Text = "Reprogram Relay File Select";
            this.reprogramRelayFileSelectToolStripMenuItem.Click += new System.EventHandler(this.reprogramRelayFileSelectToolStripMenuItem_Click);
            // 
            // toolStripMenuItemAction
            // 
            this.toolStripMenuItemAction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eventActionsToolStripMenuItem,
            this.liveDataActionsToolStripMenuItem});
            this.toolStripMenuItemAction.Name = "toolStripMenuItemAction";
            this.toolStripMenuItemAction.Size = new System.Drawing.Size(59, 20);
            this.toolStripMenuItemAction.Text = "Actions";
            // 
            // eventActionsToolStripMenuItem
            // 
            this.eventActionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadEventFromRelayToolStripMenuItem,
            this.saveEventsToolStripMenuItem,
            this.loadEventSetToolStripMenuItem,
            this.clearEventsToolStripMenuItem});
            this.eventActionsToolStripMenuItem.Name = "eventActionsToolStripMenuItem";
            this.eventActionsToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.eventActionsToolStripMenuItem.Text = "Event Actions";
            // 
            // downloadEventFromRelayToolStripMenuItem
            // 
            this.downloadEventFromRelayToolStripMenuItem.Name = "downloadEventFromRelayToolStripMenuItem";
            this.downloadEventFromRelayToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.downloadEventFromRelayToolStripMenuItem.Text = "Download Event From Relay";
            this.downloadEventFromRelayToolStripMenuItem.Click += new System.EventHandler(this.downloadEventFromRelayToolStripMenuItem_Click);
            // 
            // saveEventsToolStripMenuItem
            // 
            this.saveEventsToolStripMenuItem.Name = "saveEventsToolStripMenuItem";
            this.saveEventsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.saveEventsToolStripMenuItem.Text = "Save Event Set to File";
            this.saveEventsToolStripMenuItem.Click += new System.EventHandler(this.saveEventsToolStripMenuItem_Click);
            // 
            // loadEventSetToolStripMenuItem
            // 
            this.loadEventSetToolStripMenuItem.Name = "loadEventSetToolStripMenuItem";
            this.loadEventSetToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.loadEventSetToolStripMenuItem.Text = "Load Event Set from File";
            this.loadEventSetToolStripMenuItem.Click += new System.EventHandler(this.loadEventSetToolStripMenuItem_Click);
            // 
            // clearEventsToolStripMenuItem
            // 
            this.clearEventsToolStripMenuItem.Name = "clearEventsToolStripMenuItem";
            this.clearEventsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.clearEventsToolStripMenuItem.Text = "Clear Events";
            this.clearEventsToolStripMenuItem.Click += new System.EventHandler(this.clearEventsToolStripMenuItem_Click);
            // 
            // liveDataActionsToolStripMenuItem
            // 
            this.liveDataActionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.requestLiveDataToolStripMenuItem1,
            this.saveLiveDataToolStripMenuItem,
            this.loadLiveDataToolStripMenuItem});
            this.liveDataActionsToolStripMenuItem.Name = "liveDataActionsToolStripMenuItem";
            this.liveDataActionsToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.liveDataActionsToolStripMenuItem.Text = "Live Data Actions";
            // 
            // requestLiveDataToolStripMenuItem1
            // 
            this.requestLiveDataToolStripMenuItem1.Name = "requestLiveDataToolStripMenuItem1";
            this.requestLiveDataToolStripMenuItem1.Size = new System.Drawing.Size(239, 22);
            this.requestLiveDataToolStripMenuItem1.Text = "Download Live Data from Relay";
            this.requestLiveDataToolStripMenuItem1.Click += new System.EventHandler(this.requestLiveDataToolStripMenuItem1_Click);
            // 
            // saveLiveDataToolStripMenuItem
            // 
            this.saveLiveDataToolStripMenuItem.Name = "saveLiveDataToolStripMenuItem";
            this.saveLiveDataToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.saveLiveDataToolStripMenuItem.Text = "Save Live Data to File";
            this.saveLiveDataToolStripMenuItem.Click += new System.EventHandler(this.saveLiveDataToolStripMenuItem_Click);
            // 
            // loadLiveDataToolStripMenuItem
            // 
            this.loadLiveDataToolStripMenuItem.Name = "loadLiveDataToolStripMenuItem";
            this.loadLiveDataToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.loadLiveDataToolStripMenuItem.Text = "Load Live Data from File";
            this.loadLiveDataToolStripMenuItem.Click += new System.EventHandler(this.loadLiveDataToolStripMenuItem_Click);
            // 
            // acknowledgeToolStripMenuItem1
            // 
            this.acknowledgeToolStripMenuItem1.Name = "acknowledgeToolStripMenuItem1";
            this.acknowledgeToolStripMenuItem1.Size = new System.Drawing.Size(91, 20);
            this.acknowledgeToolStripMenuItem1.Text = "Acknowledge";
            this.acknowledgeToolStripMenuItem1.Click += new System.EventHandler(this.acknowledgeToolStripMenuItem1_Click);
            // 
            // sToolStripMenuItem
            // 
            this.sToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.printScreenToolStripMenuItem});
            this.sToolStripMenuItem.Name = "sToolStripMenuItem";
            this.sToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.sToolStripMenuItem.Text = "Screen Capture";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveToolStripMenuItem.Text = "Save Screen";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.mnuFileSaveScreen_Click);
            // 
            // printScreenToolStripMenuItem
            // 
            this.printScreenToolStripMenuItem.Name = "printScreenToolStripMenuItem";
            this.printScreenToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.printScreenToolStripMenuItem.Text = "Print Screen";
            this.printScreenToolStripMenuItem.Click += new System.EventHandler(this.mnuFilePrintScreen_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cTRatioCalculatorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // cTRatioCalculatorToolStripMenuItem
            // 
            this.cTRatioCalculatorToolStripMenuItem.Name = "cTRatioCalculatorToolStripMenuItem";
            this.cTRatioCalculatorToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.cTRatioCalculatorToolStripMenuItem.Text = "CT Ratio Calculator";
            this.cTRatioCalculatorToolStripMenuItem.Click += new System.EventHandler(this.cTRatioCalculatorToolStripMenuItem_Click);
            // 
            // loadConfigurationToolStripMenuItem
            // 
            this.loadConfigurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableAutoloadToolStripMenuItem});
            this.loadConfigurationToolStripMenuItem.Name = "loadConfigurationToolStripMenuItem";
            this.loadConfigurationToolStripMenuItem.Size = new System.Drawing.Size(122, 20);
            this.loadConfigurationToolStripMenuItem.Text = "Load Configuration";
            // 
            // enableAutoloadToolStripMenuItem
            // 
            this.enableAutoloadToolStripMenuItem.Checked = true;
            this.enableAutoloadToolStripMenuItem.CheckOnClick = true;
            this.enableAutoloadToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableAutoloadToolStripMenuItem.Name = "enableAutoloadToolStripMenuItem";
            this.enableAutoloadToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.enableAutoloadToolStripMenuItem.Text = "Enable Autoload";
            this.enableAutoloadToolStripMenuItem.Click += new System.EventHandler(this.enableAutoloadToolStripMenuItem_Click);
            // 
            // timerSCITimeOut
            // 
            this.timerSCITimeOut.Interval = 500;
            this.timerSCITimeOut.Tick += new System.EventHandler(this.timerSCITimeOut_Tick);
            // 
            // timerCheckPortTime
            // 
            this.timerCheckPortTime.Interval = 500;
            this.timerCheckPortTime.Tick += new System.EventHandler(this.timerCheckPortTime_Tick);
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMain,
            this.toolStripStatusLabelRelayDisconnected,
            this.toolStripStatusLabelReceiverStatus});
            this.statusStripMain.Location = new System.Drawing.Point(0, 677);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(999, 22);
            this.statusStripMain.TabIndex = 37;
            // 
            // toolStripStatusLabelMain
            // 
            this.toolStripStatusLabelMain.Name = "toolStripStatusLabelMain";
            this.toolStripStatusLabelMain.Size = new System.Drawing.Size(108, 17);
            this.toolStripStatusLabelMain.Text = "Checking For Relay";
            // 
            // toolStripStatusLabelRelayDisconnected
            // 
            this.toolStripStatusLabelRelayDisconnected.BackColor = System.Drawing.Color.Red;
            this.toolStripStatusLabelRelayDisconnected.Name = "toolStripStatusLabelRelayDisconnected";
            this.toolStripStatusLabelRelayDisconnected.RightToLeftAutoMirrorImage = true;
            this.toolStripStatusLabelRelayDisconnected.Size = new System.Drawing.Size(110, 17);
            this.toolStripStatusLabelRelayDisconnected.Text = "Relay Disconnected";
            // 
            // toolStripStatusLabelReceiverStatus
            // 
            this.toolStripStatusLabelReceiverStatus.Name = "toolStripStatusLabelReceiverStatus";
            this.toolStripStatusLabelReceiverStatus.Size = new System.Drawing.Size(23, 17);
            this.toolStripStatusLabelReceiverStatus.Text = "NR";
            this.toolStripStatusLabelReceiverStatus.Visible = false;
            // 
            // timerTimeOutCountdown
            // 
            this.timerTimeOutCountdown.Interval = 500;
            this.timerTimeOutCountdown.Tick += new System.EventHandler(this.timerCalibrationTimer_Tick);
            // 
            // timerRegisterPolling
            // 
            this.timerRegisterPolling.Interval = 1500;
            this.timerRegisterPolling.Tick += new System.EventHandler(this.timerRegisterPolling_Tick);
            // 
            // tabPageTransmitterMonitoring
            // 
            this.tabPageTransmitterMonitoring.BackColor = System.Drawing.Color.Transparent;
            this.tabPageTransmitterMonitoring.Controls.Add(this.ucTransmitterMonitoring1);
            this.tabPageTransmitterMonitoring.Controls.Add(this.labelRelayDisconnected2);
            this.tabPageTransmitterMonitoring.Location = new System.Drawing.Point(4, 22);
            this.tabPageTransmitterMonitoring.Name = "tabPageTransmitterMonitoring";
            this.tabPageTransmitterMonitoring.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTransmitterMonitoring.Size = new System.Drawing.Size(991, 624);
            this.tabPageTransmitterMonitoring.TabIndex = 8;
            this.tabPageTransmitterMonitoring.Text = "Transmitter Monitoring";
            this.tabPageTransmitterMonitoring.UseVisualStyleBackColor = true;
            // 
            // ucTransmitterMonitoring1
            // 
            this.ucTransmitterMonitoring1.CTMult = "";
            this.ucTransmitterMonitoring1.CTRatio = 320;
            this.ucTransmitterMonitoring1.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucTransmitterMonitoring1.Frequency = RelayControlLibrary.Frequencies.Red;
            this.ucTransmitterMonitoring1.GEEnabled = false;
            this.ucTransmitterMonitoring1.Location = new System.Drawing.Point(4, 0);
            this.ucTransmitterMonitoring1.Name = "ucTransmitterMonitoring1";
            this.ucTransmitterMonitoring1.Protector277 = false;
            this.ucTransmitterMonitoring1.Size = new System.Drawing.Size(981, 575);
            this.ucTransmitterMonitoring1.TabIndex = 86;
            this.ucTransmitterMonitoring1.TimeElapsedHours = "";
            this.ucTransmitterMonitoring1.TimeElapsedMinutes = "";
            this.ucTransmitterMonitoring1.TimeElapsedSeconds = "";
            this.ucTransmitterMonitoring1.TransmitterID = "";
            this.ucTransmitterMonitoring1.TransmitterMonitoring = false;
            this.ucTransmitterMonitoring1.TransmitterSN = "";
            // 
            // labelRelayDisconnected2
            // 
            this.labelRelayDisconnected2.AutoSize = true;
            this.labelRelayDisconnected2.BackColor = System.Drawing.Color.Red;
            this.labelRelayDisconnected2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRelayDisconnected2.Location = new System.Drawing.Point(421, 601);
            this.labelRelayDisconnected2.Name = "labelRelayDisconnected2";
            this.labelRelayDisconnected2.Size = new System.Drawing.Size(151, 20);
            this.labelRelayDisconnected2.TabIndex = 85;
            this.labelRelayDisconnected2.Text = "Relay Disconnected";
            this.labelRelayDisconnected2.Visible = false;
            // 
            // tabPageEngineering
            // 
            this.tabPageEngineering.Controls.Add(this.ucGeneralCommandHandler1);
            this.tabPageEngineering.Controls.Add(this.groupBoxTimeConvert);
            this.tabPageEngineering.Controls.Add(this.buttonToggleQuietMode);
            this.tabPageEngineering.Controls.Add(this.buttonFPGAProcVersion);
            this.tabPageEngineering.Controls.Add(this.buttonCauseEvent);
            this.tabPageEngineering.Controls.Add(this.buttonSendTime);
            this.tabPageEngineering.Controls.Add(this.buttonUpdateDisplay);
            this.tabPageEngineering.Controls.Add(this.buttonRSTRelay);
            this.tabPageEngineering.Controls.Add(this.buttonResetMaster);
            this.tabPageEngineering.Controls.Add(this.buttonRQRelayProcVersion);
            this.tabPageEngineering.Controls.Add(this.buttonForceI);
            this.tabPageEngineering.Controls.Add(this.buttonRequestRelayRegisters);
            this.tabPageEngineering.Controls.Add(this.ucRelayProgramming1);
            this.tabPageEngineering.Controls.Add(this.uc8CheckBoxFlagsGEControl2);
            this.tabPageEngineering.Controls.Add(this.uc8CheckBoxFlagsGEControl1);
            this.tabPageEngineering.Controls.Add(this.ucCSVConverterCSVFile1);
            this.tabPageEngineering.Controls.Add(this.ucCalibration2);
            this.tabPageEngineering.Controls.Add(this.uc8CheckBoxFlagsCommFlags2);
            this.tabPageEngineering.Controls.Add(this.uc8CheckBoxFlagsCommFlags1);
            this.tabPageEngineering.Controls.Add(this.uc8CheckBoxFlagsRelayStatus2);
            this.tabPageEngineering.Controls.Add(this.uc8CheckBoxFlagsRelayStatus1);
            this.tabPageEngineering.Controls.Add(this.uc8CheckBoxFlagsRelayFlags2);
            this.tabPageEngineering.Controls.Add(this.uc8CheckBoxFlagsRelayFlags1);
            this.tabPageEngineering.Controls.Add(this.ucCalibration1);
            this.tabPageEngineering.Controls.Add(this.ucForceCustomerSwitch1);
            this.tabPageEngineering.Location = new System.Drawing.Point(4, 22);
            this.tabPageEngineering.Name = "tabPageEngineering";
            this.tabPageEngineering.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEngineering.Size = new System.Drawing.Size(991, 624);
            this.tabPageEngineering.TabIndex = 7;
            this.tabPageEngineering.Text = "Engineering";
            this.tabPageEngineering.UseVisualStyleBackColor = true;
            // 
            // ucGeneralCommandHandler1
            // 
            this.ucGeneralCommandHandler1.Location = new System.Drawing.Point(557, 197);
            this.ucGeneralCommandHandler1.Name = "ucGeneralCommandHandler1";
            this.ucGeneralCommandHandler1.Size = new System.Drawing.Size(426, 221);
            this.ucGeneralCommandHandler1.TabIndex = 117;
            // 
            // groupBoxTimeConvert
            // 
            this.groupBoxTimeConvert.Controls.Add(this.textBoxTimeOutput);
            this.groupBoxTimeConvert.Controls.Add(this.textBoxTimeInput);
            this.groupBoxTimeConvert.Controls.Add(this.buttonTimeConvert);
            this.groupBoxTimeConvert.Controls.Add(this.label24);
            this.groupBoxTimeConvert.Controls.Add(this.label1);
            this.groupBoxTimeConvert.Location = new System.Drawing.Point(775, 98);
            this.groupBoxTimeConvert.Name = "groupBoxTimeConvert";
            this.groupBoxTimeConvert.Size = new System.Drawing.Size(220, 100);
            this.groupBoxTimeConvert.TabIndex = 98;
            this.groupBoxTimeConvert.TabStop = false;
            this.groupBoxTimeConvert.Text = "Time Convert";
            // 
            // textBoxTimeOutput
            // 
            this.textBoxTimeOutput.Location = new System.Drawing.Point(78, 47);
            this.textBoxTimeOutput.Name = "textBoxTimeOutput";
            this.textBoxTimeOutput.ReadOnly = true;
            this.textBoxTimeOutput.Size = new System.Drawing.Size(136, 20);
            this.textBoxTimeOutput.TabIndex = 4;
            // 
            // textBoxTimeInput
            // 
            this.textBoxTimeInput.Location = new System.Drawing.Point(78, 21);
            this.textBoxTimeInput.Name = "textBoxTimeInput";
            this.textBoxTimeInput.Size = new System.Drawing.Size(136, 20);
            this.textBoxTimeInput.TabIndex = 3;
            // 
            // buttonTimeConvert
            // 
            this.buttonTimeConvert.Location = new System.Drawing.Point(15, 68);
            this.buttonTimeConvert.Name = "buttonTimeConvert";
            this.buttonTimeConvert.Size = new System.Drawing.Size(91, 23);
            this.buttonTimeConvert.TabIndex = 2;
            this.buttonTimeConvert.Text = "Convert Time";
            this.buttonTimeConvert.UseVisualStyleBackColor = true;
            this.buttonTimeConvert.Click += new System.EventHandler(this.buttonTimeConvert_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(4, 50);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(68, 13);
            this.label24.TabIndex = 1;
            this.label24.Text = "Time Output:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Time Input:";
            // 
            // buttonToggleQuietMode
            // 
            this.buttonToggleQuietMode.Location = new System.Drawing.Point(473, 69);
            this.buttonToggleQuietMode.Name = "buttonToggleQuietMode";
            this.buttonToggleQuietMode.Size = new System.Drawing.Size(129, 23);
            this.buttonToggleQuietMode.TabIndex = 95;
            this.buttonToggleQuietMode.Text = "Quiet Mode";
            this.buttonToggleQuietMode.UseVisualStyleBackColor = true;
            this.buttonToggleQuietMode.Click += new System.EventHandler(this.buttonQuietMode_Click);
            // 
            // buttonFPGAProcVersion
            // 
            this.buttonFPGAProcVersion.Location = new System.Drawing.Point(17, 220);
            this.buttonFPGAProcVersion.Name = "buttonFPGAProcVersion";
            this.buttonFPGAProcVersion.Size = new System.Drawing.Size(162, 23);
            this.buttonFPGAProcVersion.TabIndex = 91;
            this.buttonFPGAProcVersion.Text = "Request FPGA Proc Version";
            this.buttonFPGAProcVersion.UseVisualStyleBackColor = true;
            this.buttonFPGAProcVersion.Click += new System.EventHandler(this.buttonFPGAProcVersion_Click);
            // 
            // buttonCauseEvent
            // 
            this.buttonCauseEvent.Location = new System.Drawing.Point(6, 61);
            this.buttonCauseEvent.Name = "buttonCauseEvent";
            this.buttonCauseEvent.Size = new System.Drawing.Size(103, 23);
            this.buttonCauseEvent.TabIndex = 85;
            this.buttonCauseEvent.Text = "Cause Event";
            this.buttonCauseEvent.UseVisualStyleBackColor = true;
            this.buttonCauseEvent.Click += new System.EventHandler(this.buttonCauseEvent_Click);
            // 
            // buttonSendTime
            // 
            this.buttonSendTime.Location = new System.Drawing.Point(8, 133);
            this.buttonSendTime.Name = "buttonSendTime";
            this.buttonSendTime.Size = new System.Drawing.Size(75, 23);
            this.buttonSendTime.TabIndex = 84;
            this.buttonSendTime.Text = "Send Time";
            this.buttonSendTime.UseVisualStyleBackColor = true;
            this.buttonSendTime.Click += new System.EventHandler(this.buttonSendTime_Click);
            // 
            // buttonUpdateDisplay
            // 
            this.buttonUpdateDisplay.Location = new System.Drawing.Point(89, 104);
            this.buttonUpdateDisplay.Name = "buttonUpdateDisplay";
            this.buttonUpdateDisplay.Size = new System.Drawing.Size(88, 23);
            this.buttonUpdateDisplay.TabIndex = 81;
            this.buttonUpdateDisplay.Text = "Update Display";
            this.buttonUpdateDisplay.UseVisualStyleBackColor = true;
            this.buttonUpdateDisplay.Click += new System.EventHandler(this.buttonUpdateDisplay_Click);
            // 
            // buttonRSTRelay
            // 
            this.buttonRSTRelay.Location = new System.Drawing.Point(6, 3);
            this.buttonRSTRelay.Name = "buttonRSTRelay";
            this.buttonRSTRelay.Size = new System.Drawing.Size(105, 23);
            this.buttonRSTRelay.TabIndex = 76;
            this.buttonRSTRelay.Text = "RST Relay Proc";
            this.buttonRSTRelay.UseVisualStyleBackColor = true;
            this.buttonRSTRelay.Click += new System.EventHandler(this.buttonRSTRelay_Click);
            // 
            // buttonResetMaster
            // 
            this.buttonResetMaster.Location = new System.Drawing.Point(6, 32);
            this.buttonResetMaster.Name = "buttonResetMaster";
            this.buttonResetMaster.Size = new System.Drawing.Size(105, 23);
            this.buttonResetMaster.TabIndex = 74;
            this.buttonResetMaster.Text = "RST Master Proc";
            this.buttonResetMaster.UseVisualStyleBackColor = true;
            this.buttonResetMaster.Click += new System.EventHandler(this.buttonResetMaster_Click);
            // 
            // buttonRQRelayProcVersion
            // 
            this.buttonRQRelayProcVersion.Location = new System.Drawing.Point(17, 191);
            this.buttonRQRelayProcVersion.Name = "buttonRQRelayProcVersion";
            this.buttonRQRelayProcVersion.Size = new System.Drawing.Size(162, 23);
            this.buttonRQRelayProcVersion.TabIndex = 71;
            this.buttonRQRelayProcVersion.Text = "Request Relay Proc Version";
            this.buttonRQRelayProcVersion.UseVisualStyleBackColor = true;
            this.buttonRQRelayProcVersion.Click += new System.EventHandler(this.buttonRQRelayProcVersion_Click);
            // 
            // buttonForceI
            // 
            this.buttonForceI.Location = new System.Drawing.Point(8, 104);
            this.buttonForceI.Name = "buttonForceI";
            this.buttonForceI.Size = new System.Drawing.Size(75, 23);
            this.buttonForceI.TabIndex = 69;
            this.buttonForceI.Text = "Force I";
            this.buttonForceI.UseVisualStyleBackColor = true;
            this.buttonForceI.Click += new System.EventHandler(this.buttonForceI_Click);
            // 
            // buttonRequestRelayRegisters
            // 
            this.buttonRequestRelayRegisters.Location = new System.Drawing.Point(17, 162);
            this.buttonRequestRelayRegisters.Name = "buttonRequestRelayRegisters";
            this.buttonRequestRelayRegisters.Size = new System.Drawing.Size(145, 23);
            this.buttonRequestRelayRegisters.TabIndex = 68;
            this.buttonRequestRelayRegisters.Text = "Request Relay Registers";
            this.buttonRequestRelayRegisters.UseVisualStyleBackColor = true;
            this.buttonRequestRelayRegisters.Click += new System.EventHandler(this.buttonRequestRelayRegisters_Click);
            // 
            // ucRelayProgramming1
            // 
            this.ucRelayProgramming1.Customer = RelayControlLibrary.Customers.None;
            this.ucRelayProgramming1.DNPRelay = false;
            this.ucRelayProgramming1.FPGARevisionNumber = ((uint)(0u));
            this.ucRelayProgramming1.GEEnabled = false;
            this.ucRelayProgramming1.Location = new System.Drawing.Point(525, 413);
            this.ucRelayProgramming1.MasterRevisionNumber = ((uint)(0u));
            this.ucRelayProgramming1.Name = "ucRelayProgramming1";
            this.ucRelayProgramming1.RelayRevisionNumber = ((uint)(0u));
            this.ucRelayProgramming1.SerialNumber = ((uint)(0u));
            this.ucRelayProgramming1.Size = new System.Drawing.Size(458, 211);
            this.ucRelayProgramming1.State = RelayControlLibrary.RelayProgrammingStates.Idle;
            this.ucRelayProgramming1.TabIndex = 112;
            this.ucRelayProgramming1.TransmitterEnabled = false;
            // 
            // uc8CheckBoxFlagsGEControl2
            // 
            this.uc8CheckBoxFlagsGEControl2.Location = new System.Drawing.Point(360, 431);
            this.uc8CheckBoxFlagsGEControl2.Name = "uc8CheckBoxFlagsGEControl2";
            this.uc8CheckBoxFlagsGEControl2.Names = null;
            this.uc8CheckBoxFlagsGEControl2.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsGEControl2.TabIndex = 111;
            // 
            // uc8CheckBoxFlagsGEControl1
            // 
            this.uc8CheckBoxFlagsGEControl1.Location = new System.Drawing.Point(360, 249);
            this.uc8CheckBoxFlagsGEControl1.Name = "uc8CheckBoxFlagsGEControl1";
            this.uc8CheckBoxFlagsGEControl1.Names = null;
            this.uc8CheckBoxFlagsGEControl1.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsGEControl1.TabIndex = 110;
            // 
            // ucCSVConverterCSVFile1
            // 
            this.ucCSVConverterCSVFile1.Location = new System.Drawing.Point(695, 11);
            this.ucCSVConverterCSVFile1.Name = "ucCSVConverterCSVFile1";
            this.ucCSVConverterCSVFile1.Size = new System.Drawing.Size(94, 84);
            this.ucCSVConverterCSVFile1.TabIndex = 109;
            // 
            // ucCalibration2
            // 
            this.ucCalibration2.Customer = RelayControlLibrary.Customers.DigitalGridDNP;
            this.ucCalibration2.Location = new System.Drawing.Point(185, 6);
            this.ucCalibration2.Name = "ucCalibration2";
            this.ucCalibration2.Size = new System.Drawing.Size(283, 225);
            this.ucCalibration2.TabIndex = 108;
            // 
            // uc8CheckBoxFlagsCommFlags2
            // 
            this.uc8CheckBoxFlagsCommFlags2.Location = new System.Drawing.Point(254, 431);
            this.uc8CheckBoxFlagsCommFlags2.Name = "uc8CheckBoxFlagsCommFlags2";
            this.uc8CheckBoxFlagsCommFlags2.Names = null;
            this.uc8CheckBoxFlagsCommFlags2.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsCommFlags2.TabIndex = 106;
            // 
            // uc8CheckBoxFlagsCommFlags1
            // 
            this.uc8CheckBoxFlagsCommFlags1.Location = new System.Drawing.Point(254, 249);
            this.uc8CheckBoxFlagsCommFlags1.Name = "uc8CheckBoxFlagsCommFlags1";
            this.uc8CheckBoxFlagsCommFlags1.Names = null;
            this.uc8CheckBoxFlagsCommFlags1.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsCommFlags1.TabIndex = 105;
            // 
            // uc8CheckBoxFlagsRelayStatus2
            // 
            this.uc8CheckBoxFlagsRelayStatus2.Location = new System.Drawing.Point(143, 431);
            this.uc8CheckBoxFlagsRelayStatus2.Name = "uc8CheckBoxFlagsRelayStatus2";
            this.uc8CheckBoxFlagsRelayStatus2.Names = null;
            this.uc8CheckBoxFlagsRelayStatus2.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsRelayStatus2.TabIndex = 104;
            // 
            // uc8CheckBoxFlagsRelayStatus1
            // 
            this.uc8CheckBoxFlagsRelayStatus1.Location = new System.Drawing.Point(143, 249);
            this.uc8CheckBoxFlagsRelayStatus1.Name = "uc8CheckBoxFlagsRelayStatus1";
            this.uc8CheckBoxFlagsRelayStatus1.Names = null;
            this.uc8CheckBoxFlagsRelayStatus1.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsRelayStatus1.TabIndex = 103;
            // 
            // uc8CheckBoxFlagsRelayFlags2
            // 
            this.uc8CheckBoxFlagsRelayFlags2.Location = new System.Drawing.Point(17, 431);
            this.uc8CheckBoxFlagsRelayFlags2.Name = "uc8CheckBoxFlagsRelayFlags2";
            this.uc8CheckBoxFlagsRelayFlags2.Names = null;
            this.uc8CheckBoxFlagsRelayFlags2.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsRelayFlags2.TabIndex = 102;
            // 
            // uc8CheckBoxFlagsRelayFlags1
            // 
            this.uc8CheckBoxFlagsRelayFlags1.Location = new System.Drawing.Point(17, 249);
            this.uc8CheckBoxFlagsRelayFlags1.Name = "uc8CheckBoxFlagsRelayFlags1";
            this.uc8CheckBoxFlagsRelayFlags1.Names = null;
            this.uc8CheckBoxFlagsRelayFlags1.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsRelayFlags1.TabIndex = 101;
            // 
            // ucCalibration1
            // 
            this.ucCalibration1.Location = new System.Drawing.Point(473, 95);
            this.ucCalibration1.Name = "ucCalibration1";
            this.ucCalibration1.Size = new System.Drawing.Size(302, 87);
            this.ucCalibration1.TabIndex = 82;
            // 
            // ucForceCustomerSwitch1
            // 
            this.ucForceCustomerSwitch1.Location = new System.Drawing.Point(467, 11);
            this.ucForceCustomerSwitch1.Name = "ucForceCustomerSwitch1";
            this.ucForceCustomerSwitch1.Size = new System.Drawing.Size(222, 52);
            this.ucForceCustomerSwitch1.TabIndex = 100;
            // 
            // tabPageEvents
            // 
            this.tabPageEvents.Controls.Add(this.buttonClearEvents);
            this.tabPageEvents.Controls.Add(this.buttonRQEventData);
            this.tabPageEvents.Controls.Add(this.panelEventSelect);
            this.tabPageEvents.Controls.Add(this.ucEventGraph7);
            this.tabPageEvents.Controls.Add(this.ucEventGraph6);
            this.tabPageEvents.Controls.Add(this.ucEventGraph5);
            this.tabPageEvents.Controls.Add(this.ucEventGraph4);
            this.tabPageEvents.Controls.Add(this.ucEventGraph3);
            this.tabPageEvents.Controls.Add(this.ucEventGraph2);
            this.tabPageEvents.Controls.Add(this.ucEventGraph1);
            this.tabPageEvents.Controls.Add(this.ucEventGraph0);
            this.tabPageEvents.Location = new System.Drawing.Point(4, 22);
            this.tabPageEvents.Name = "tabPageEvents";
            this.tabPageEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvents.Size = new System.Drawing.Size(991, 624);
            this.tabPageEvents.TabIndex = 6;
            this.tabPageEvents.Text = "Events";
            this.tabPageEvents.UseVisualStyleBackColor = true;
            // 
            // buttonClearEvents
            // 
            this.buttonClearEvents.Location = new System.Drawing.Point(717, 3);
            this.buttonClearEvents.Name = "buttonClearEvents";
            this.buttonClearEvents.Size = new System.Drawing.Size(133, 23);
            this.buttonClearEvents.TabIndex = 10;
            this.buttonClearEvents.Text = "Clear Events";
            this.buttonClearEvents.UseVisualStyleBackColor = true;
            this.buttonClearEvents.Click += new System.EventHandler(this.buttonClearEvents_Click);
            // 
            // buttonRQEventData
            // 
            this.buttonRQEventData.Location = new System.Drawing.Point(850, 3);
            this.buttonRQEventData.Name = "buttonRQEventData";
            this.buttonRQEventData.Size = new System.Drawing.Size(133, 23);
            this.buttonRQEventData.TabIndex = 9;
            this.buttonRQEventData.Text = "Request Event Data";
            this.buttonRQEventData.UseVisualStyleBackColor = true;
            this.buttonRQEventData.Click += new System.EventHandler(this.buttonRQEventData_Click);
            // 
            // panelEventSelect
            // 
            this.panelEventSelect.Controls.Add(this.radioButtonEvent7);
            this.panelEventSelect.Controls.Add(this.radioButtonEvent6);
            this.panelEventSelect.Controls.Add(this.radioButtonEvent5);
            this.panelEventSelect.Controls.Add(this.radioButtonEvent4);
            this.panelEventSelect.Controls.Add(this.radioButtonEvent3);
            this.panelEventSelect.Controls.Add(this.radioButtonEvent2);
            this.panelEventSelect.Controls.Add(this.radioButtonEvent1);
            this.panelEventSelect.Controls.Add(this.radioButtonEvent0);
            this.panelEventSelect.Location = new System.Drawing.Point(3, 0);
            this.panelEventSelect.Name = "panelEventSelect";
            this.panelEventSelect.Size = new System.Drawing.Size(708, 33);
            this.panelEventSelect.TabIndex = 1;
            // 
            // radioButtonEvent7
            // 
            this.radioButtonEvent7.AutoSize = true;
            this.radioButtonEvent7.Location = new System.Drawing.Point(643, 7);
            this.radioButtonEvent7.Name = "radioButtonEvent7";
            this.radioButtonEvent7.Size = new System.Drawing.Size(62, 17);
            this.radioButtonEvent7.TabIndex = 7;
            this.radioButtonEvent7.TabStop = true;
            this.radioButtonEvent7.Text = "Event 8";
            this.radioButtonEvent7.UseVisualStyleBackColor = true;
            this.radioButtonEvent7.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent6
            // 
            this.radioButtonEvent6.AutoSize = true;
            this.radioButtonEvent6.Location = new System.Drawing.Point(552, 7);
            this.radioButtonEvent6.Name = "radioButtonEvent6";
            this.radioButtonEvent6.Size = new System.Drawing.Size(62, 17);
            this.radioButtonEvent6.TabIndex = 6;
            this.radioButtonEvent6.TabStop = true;
            this.radioButtonEvent6.Text = "Event 7";
            this.radioButtonEvent6.UseVisualStyleBackColor = true;
            this.radioButtonEvent6.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent5
            // 
            this.radioButtonEvent5.AutoSize = true;
            this.radioButtonEvent5.Location = new System.Drawing.Point(461, 7);
            this.radioButtonEvent5.Name = "radioButtonEvent5";
            this.radioButtonEvent5.Size = new System.Drawing.Size(62, 17);
            this.radioButtonEvent5.TabIndex = 5;
            this.radioButtonEvent5.TabStop = true;
            this.radioButtonEvent5.Text = "Event 6";
            this.radioButtonEvent5.UseVisualStyleBackColor = true;
            this.radioButtonEvent5.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent4
            // 
            this.radioButtonEvent4.AutoSize = true;
            this.radioButtonEvent4.Location = new System.Drawing.Point(370, 7);
            this.radioButtonEvent4.Name = "radioButtonEvent4";
            this.radioButtonEvent4.Size = new System.Drawing.Size(62, 17);
            this.radioButtonEvent4.TabIndex = 4;
            this.radioButtonEvent4.TabStop = true;
            this.radioButtonEvent4.Text = "Event 5";
            this.radioButtonEvent4.UseVisualStyleBackColor = true;
            this.radioButtonEvent4.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent3
            // 
            this.radioButtonEvent3.AutoSize = true;
            this.radioButtonEvent3.Location = new System.Drawing.Point(279, 7);
            this.radioButtonEvent3.Name = "radioButtonEvent3";
            this.radioButtonEvent3.Size = new System.Drawing.Size(62, 17);
            this.radioButtonEvent3.TabIndex = 3;
            this.radioButtonEvent3.TabStop = true;
            this.radioButtonEvent3.Text = "Event 4";
            this.radioButtonEvent3.UseVisualStyleBackColor = true;
            this.radioButtonEvent3.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent2
            // 
            this.radioButtonEvent2.AutoSize = true;
            this.radioButtonEvent2.Location = new System.Drawing.Point(188, 7);
            this.radioButtonEvent2.Name = "radioButtonEvent2";
            this.radioButtonEvent2.Size = new System.Drawing.Size(62, 17);
            this.radioButtonEvent2.TabIndex = 2;
            this.radioButtonEvent2.TabStop = true;
            this.radioButtonEvent2.Text = "Event 3";
            this.radioButtonEvent2.UseVisualStyleBackColor = true;
            this.radioButtonEvent2.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent1
            // 
            this.radioButtonEvent1.AutoSize = true;
            this.radioButtonEvent1.Location = new System.Drawing.Point(97, 7);
            this.radioButtonEvent1.Name = "radioButtonEvent1";
            this.radioButtonEvent1.Size = new System.Drawing.Size(62, 17);
            this.radioButtonEvent1.TabIndex = 1;
            this.radioButtonEvent1.TabStop = true;
            this.radioButtonEvent1.Text = "Event 2";
            this.radioButtonEvent1.UseVisualStyleBackColor = true;
            this.radioButtonEvent1.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent0
            // 
            this.radioButtonEvent0.AutoSize = true;
            this.radioButtonEvent0.Location = new System.Drawing.Point(6, 7);
            this.radioButtonEvent0.Name = "radioButtonEvent0";
            this.radioButtonEvent0.Size = new System.Drawing.Size(62, 17);
            this.radioButtonEvent0.TabIndex = 0;
            this.radioButtonEvent0.TabStop = true;
            this.radioButtonEvent0.Text = "Event 1";
            this.radioButtonEvent0.UseVisualStyleBackColor = true;
            this.radioButtonEvent0.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // ucEventGraph7
            // 
            this.ucEventGraph7.CTRatio = 320;
            this.ucEventGraph7.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucEventGraph7.DelayToBFlag = ((uint)(0u));
            this.ucEventGraph7.DelayToFloat = ((uint)(0u));
            this.ucEventGraph7.EventNumber = ((uint)(0u));
            this.ucEventGraph7.EventTime = new System.DateTime(((long)(0)));
            this.ucEventGraph7.GEEnabled = false;
            this.ucEventGraph7.Location = new System.Drawing.Point(0, 33);
            this.ucEventGraph7.Name = "ucEventGraph7";
            this.ucEventGraph7.Protector277 = false;
            this.ucEventGraph7.Size = new System.Drawing.Size(988, 593);
            this.ucEventGraph7.TabIndex = 8;
            this.ucEventGraph7.Type = RelayControlLibrary.EventTypes.Trip;
            // 
            // ucEventGraph6
            // 
            this.ucEventGraph6.CTRatio = 320;
            this.ucEventGraph6.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucEventGraph6.DelayToBFlag = ((uint)(0u));
            this.ucEventGraph6.DelayToFloat = ((uint)(0u));
            this.ucEventGraph6.EventNumber = ((uint)(0u));
            this.ucEventGraph6.EventTime = new System.DateTime(((long)(0)));
            this.ucEventGraph6.GEEnabled = false;
            this.ucEventGraph6.Location = new System.Drawing.Point(0, 33);
            this.ucEventGraph6.Name = "ucEventGraph6";
            this.ucEventGraph6.Protector277 = false;
            this.ucEventGraph6.Size = new System.Drawing.Size(988, 593);
            this.ucEventGraph6.TabIndex = 7;
            this.ucEventGraph6.Type = RelayControlLibrary.EventTypes.Trip;
            // 
            // ucEventGraph5
            // 
            this.ucEventGraph5.CTRatio = 320;
            this.ucEventGraph5.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucEventGraph5.DelayToBFlag = ((uint)(0u));
            this.ucEventGraph5.DelayToFloat = ((uint)(0u));
            this.ucEventGraph5.EventNumber = ((uint)(0u));
            this.ucEventGraph5.EventTime = new System.DateTime(((long)(0)));
            this.ucEventGraph5.GEEnabled = false;
            this.ucEventGraph5.Location = new System.Drawing.Point(0, 33);
            this.ucEventGraph5.Name = "ucEventGraph5";
            this.ucEventGraph5.Protector277 = false;
            this.ucEventGraph5.Size = new System.Drawing.Size(988, 593);
            this.ucEventGraph5.TabIndex = 6;
            this.ucEventGraph5.Type = RelayControlLibrary.EventTypes.Trip;
            // 
            // ucEventGraph4
            // 
            this.ucEventGraph4.CTRatio = 320;
            this.ucEventGraph4.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucEventGraph4.DelayToBFlag = ((uint)(0u));
            this.ucEventGraph4.DelayToFloat = ((uint)(0u));
            this.ucEventGraph4.EventNumber = ((uint)(0u));
            this.ucEventGraph4.EventTime = new System.DateTime(((long)(0)));
            this.ucEventGraph4.GEEnabled = false;
            this.ucEventGraph4.Location = new System.Drawing.Point(0, 33);
            this.ucEventGraph4.Name = "ucEventGraph4";
            this.ucEventGraph4.Protector277 = false;
            this.ucEventGraph4.Size = new System.Drawing.Size(988, 593);
            this.ucEventGraph4.TabIndex = 5;
            this.ucEventGraph4.Type = RelayControlLibrary.EventTypes.Trip;
            // 
            // ucEventGraph3
            // 
            this.ucEventGraph3.CTRatio = 320;
            this.ucEventGraph3.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucEventGraph3.DelayToBFlag = ((uint)(0u));
            this.ucEventGraph3.DelayToFloat = ((uint)(0u));
            this.ucEventGraph3.EventNumber = ((uint)(0u));
            this.ucEventGraph3.EventTime = new System.DateTime(((long)(0)));
            this.ucEventGraph3.GEEnabled = false;
            this.ucEventGraph3.Location = new System.Drawing.Point(0, 33);
            this.ucEventGraph3.Name = "ucEventGraph3";
            this.ucEventGraph3.Protector277 = false;
            this.ucEventGraph3.Size = new System.Drawing.Size(988, 593);
            this.ucEventGraph3.TabIndex = 4;
            this.ucEventGraph3.Type = RelayControlLibrary.EventTypes.Trip;
            // 
            // ucEventGraph2
            // 
            this.ucEventGraph2.CTRatio = 320;
            this.ucEventGraph2.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucEventGraph2.DelayToBFlag = ((uint)(0u));
            this.ucEventGraph2.DelayToFloat = ((uint)(0u));
            this.ucEventGraph2.EventNumber = ((uint)(0u));
            this.ucEventGraph2.EventTime = new System.DateTime(((long)(0)));
            this.ucEventGraph2.GEEnabled = false;
            this.ucEventGraph2.Location = new System.Drawing.Point(0, 33);
            this.ucEventGraph2.Name = "ucEventGraph2";
            this.ucEventGraph2.Protector277 = false;
            this.ucEventGraph2.Size = new System.Drawing.Size(988, 593);
            this.ucEventGraph2.TabIndex = 3;
            this.ucEventGraph2.Type = RelayControlLibrary.EventTypes.Trip;
            // 
            // ucEventGraph1
            // 
            this.ucEventGraph1.CTRatio = 320;
            this.ucEventGraph1.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucEventGraph1.DelayToBFlag = ((uint)(0u));
            this.ucEventGraph1.DelayToFloat = ((uint)(0u));
            this.ucEventGraph1.EventNumber = ((uint)(0u));
            this.ucEventGraph1.EventTime = new System.DateTime(((long)(0)));
            this.ucEventGraph1.GEEnabled = false;
            this.ucEventGraph1.Location = new System.Drawing.Point(0, 33);
            this.ucEventGraph1.Name = "ucEventGraph1";
            this.ucEventGraph1.Protector277 = false;
            this.ucEventGraph1.Size = new System.Drawing.Size(988, 593);
            this.ucEventGraph1.TabIndex = 2;
            this.ucEventGraph1.Type = RelayControlLibrary.EventTypes.Trip;
            // 
            // ucEventGraph0
            // 
            this.ucEventGraph0.CTRatio = 320;
            this.ucEventGraph0.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucEventGraph0.DelayToBFlag = ((uint)(0u));
            this.ucEventGraph0.DelayToFloat = ((uint)(0u));
            this.ucEventGraph0.EventNumber = ((uint)(0u));
            this.ucEventGraph0.EventTime = new System.DateTime(((long)(0)));
            this.ucEventGraph0.GEEnabled = false;
            this.ucEventGraph0.Location = new System.Drawing.Point(0, 33);
            this.ucEventGraph0.Name = "ucEventGraph0";
            this.ucEventGraph0.Protector277 = false;
            this.ucEventGraph0.Size = new System.Drawing.Size(988, 593);
            this.ucEventGraph0.TabIndex = 0;
            this.ucEventGraph0.Type = RelayControlLibrary.EventTypes.Trip;
            // 
            // tabPageFlightRecorder
            // 
            this.tabPageFlightRecorder.Controls.Add(this.labelLiveDataTriggerTime);
            this.tabPageFlightRecorder.Controls.Add(this.ucLiveData1);
            this.tabPageFlightRecorder.Location = new System.Drawing.Point(4, 22);
            this.tabPageFlightRecorder.Name = "tabPageFlightRecorder";
            this.tabPageFlightRecorder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFlightRecorder.Size = new System.Drawing.Size(991, 624);
            this.tabPageFlightRecorder.TabIndex = 5;
            this.tabPageFlightRecorder.Text = "Live Data";
            this.tabPageFlightRecorder.UseVisualStyleBackColor = true;
            // 
            // labelLiveDataTriggerTime
            // 
            this.labelLiveDataTriggerTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelLiveDataTriggerTime.AutoSize = true;
            this.labelLiveDataTriggerTime.Location = new System.Drawing.Point(-3901, 6);
            this.labelLiveDataTriggerTime.Name = "labelLiveDataTriggerTime";
            this.labelLiveDataTriggerTime.Size = new System.Drawing.Size(0, 13);
            this.labelLiveDataTriggerTime.TabIndex = 1;
            this.labelLiveDataTriggerTime.Resize += new System.EventHandler(this.labelLiveEventTriggerTime_Resize);
            // 
            // ucLiveData1
            // 
            this.ucLiveData1.CTRatio = 320;
            this.ucLiveData1.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucLiveData1.GEEnabled = false;
            this.ucLiveData1.Location = new System.Drawing.Point(0, 22);
            this.ucLiveData1.Name = "ucLiveData1";
            this.ucLiveData1.Protector277 = false;
            this.ucLiveData1.Size = new System.Drawing.Size(991, 602);
            this.ucLiveData1.TabIndex = 0;
            // 
            // tabPageTransmitter
            // 
            this.tabPageTransmitter.Controls.Add(this.labelRelayDisconnected3);
            this.tabPageTransmitter.Controls.Add(this.ucTransmitter1);
            this.tabPageTransmitter.Location = new System.Drawing.Point(4, 22);
            this.tabPageTransmitter.Name = "tabPageTransmitter";
            this.tabPageTransmitter.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTransmitter.Size = new System.Drawing.Size(991, 624);
            this.tabPageTransmitter.TabIndex = 2;
            this.tabPageTransmitter.Text = "Transmitter Settings";
            this.tabPageTransmitter.UseVisualStyleBackColor = true;
            // 
            // labelRelayDisconnected3
            // 
            this.labelRelayDisconnected3.AutoSize = true;
            this.labelRelayDisconnected3.BackColor = System.Drawing.Color.Red;
            this.labelRelayDisconnected3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRelayDisconnected3.Location = new System.Drawing.Point(199, 502);
            this.labelRelayDisconnected3.Name = "labelRelayDisconnected3";
            this.labelRelayDisconnected3.Size = new System.Drawing.Size(151, 20);
            this.labelRelayDisconnected3.TabIndex = 70;
            this.labelRelayDisconnected3.Text = "Relay Disconnected";
            this.labelRelayDisconnected3.Visible = false;
            // 
            // ucTransmitter1
            // 
            this.ucTransmitter1.CTRatio = ((uint)(320u));
            this.ucTransmitter1.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucTransmitter1.DNPEnabled = false;
            this.ucTransmitter1.ForceDNPEnable = false;
            this.ucTransmitter1.FPGARevisionValid = true;
            this.ucTransmitter1.GEEnabled = false;
            this.ucTransmitter1.Location = new System.Drawing.Point(8, 6);
            this.ucTransmitter1.Name = "ucTransmitter1";
            this.ucTransmitter1.PacketLength = 30;
            this.ucTransmitter1.SerialNumber = 0;
            this.ucTransmitter1.Size = new System.Drawing.Size(869, 612);
            this.ucTransmitter1.TabIndex = 0;
            this.ucTransmitter1.WaterBugNoTransmitter = false;
            // 
            // tabPageMonitor
            // 
            this.tabPageMonitor.Controls.Add(this.checkBox277ProtectorPQ);
            this.tabPageMonitor.Controls.Add(this.labelSNPQMonitor);
            this.tabPageMonitor.Controls.Add(this.textBoxRelaySNControlPQ);
            this.tabPageMonitor.Controls.Add(this.textBoxCTRatioPQMonitor);
            this.tabPageMonitor.Controls.Add(this.checkBoxInTripRegion);
            this.tabPageMonitor.Controls.Add(this.textBoxTemperatureMonitoringPage);
            this.tabPageMonitor.Controls.Add(this.labelTemperatureMonitoringPage);
            this.tabPageMonitor.Controls.Add(this.buttonUpdateCTRatio);
            this.tabPageMonitor.Controls.Add(this.labelRelayTrippedOrClose);
            this.tabPageMonitor.Controls.Add(this.buttonToggleMonitor);
            this.tabPageMonitor.Controls.Add(this.labelCtRatioMonitor);
            this.tabPageMonitor.Controls.Add(this.ucPhasorGraph1);
            this.tabPageMonitor.Location = new System.Drawing.Point(4, 22);
            this.tabPageMonitor.Name = "tabPageMonitor";
            this.tabPageMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMonitor.Size = new System.Drawing.Size(991, 624);
            this.tabPageMonitor.TabIndex = 1;
            this.tabPageMonitor.Text = "PQ Monitor";
            this.tabPageMonitor.UseVisualStyleBackColor = true;
            // 
            // checkBox277ProtectorPQ
            // 
            this.checkBox277ProtectorPQ.AutoSize = true;
            this.checkBox277ProtectorPQ.Location = new System.Drawing.Point(545, 578);
            this.checkBox277ProtectorPQ.Name = "checkBox277ProtectorPQ";
            this.checkBox277ProtectorPQ.Size = new System.Drawing.Size(100, 17);
            this.checkBox277ProtectorPQ.TabIndex = 77;
            this.checkBox277ProtectorPQ.Text = "277 V Protector";
            this.checkBox277ProtectorPQ.UseVisualStyleBackColor = true;
            this.checkBox277ProtectorPQ.CheckedChanged += new System.EventHandler(this.checkBox277Protector_CheckedChanged_PQ);
            // 
            // labelSNPQMonitor
            // 
            this.labelSNPQMonitor.AutoSize = true;
            this.labelSNPQMonitor.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSNPQMonitor.Location = new System.Drawing.Point(157, 581);
            this.labelSNPQMonitor.Name = "labelSNPQMonitor";
            this.labelSNPQMonitor.Size = new System.Drawing.Size(60, 13);
            this.labelSNPQMonitor.TabIndex = 76;
            this.labelSNPQMonitor.Text = "Relay S/N:";
            // 
            // textBoxRelaySNControlPQ
            // 
            this.textBoxRelaySNControlPQ.Location = new System.Drawing.Point(223, 578);
            this.textBoxRelaySNControlPQ.MaxLength = 5;
            this.textBoxRelaySNControlPQ.Name = "textBoxRelaySNControlPQ";
            this.textBoxRelaySNControlPQ.ReadOnly = true;
            this.textBoxRelaySNControlPQ.Size = new System.Drawing.Size(74, 20);
            this.textBoxRelaySNControlPQ.TabIndex = 75;
            this.textBoxRelaySNControlPQ.Tag = "SN";
            // 
            // textBoxCTRatioPQMonitor
            // 
            this.textBoxCTRatioPQMonitor.Enabled = false;
            this.textBoxCTRatioPQMonitor.Location = new System.Drawing.Point(358, 578);
            this.textBoxCTRatioPQMonitor.Name = "textBoxCTRatioPQMonitor";
            this.textBoxCTRatioPQMonitor.ReadOnly = true;
            this.textBoxCTRatioPQMonitor.Size = new System.Drawing.Size(48, 20);
            this.textBoxCTRatioPQMonitor.TabIndex = 65;
            this.textBoxCTRatioPQMonitor.Text = "320";
            // 
            // checkBoxInTripRegion
            // 
            this.checkBoxInTripRegion.AutoCheck = false;
            this.checkBoxInTripRegion.AutoSize = true;
            this.checkBoxInTripRegion.ForeColor = System.Drawing.Color.Black;
            this.checkBoxInTripRegion.Location = new System.Drawing.Point(892, 577);
            this.checkBoxInTripRegion.Name = "checkBoxInTripRegion";
            this.checkBoxInTripRegion.Size = new System.Drawing.Size(93, 17);
            this.checkBoxInTripRegion.TabIndex = 49;
            this.checkBoxInTripRegion.Text = "In Trip Region";
            this.checkBoxInTripRegion.UseVisualStyleBackColor = true;
            // 
            // textBoxTemperatureMonitoringPage
            // 
            this.textBoxTemperatureMonitoringPage.Location = new System.Drawing.Point(92, 578);
            this.textBoxTemperatureMonitoringPage.Name = "textBoxTemperatureMonitoringPage";
            this.textBoxTemperatureMonitoringPage.ReadOnly = true;
            this.textBoxTemperatureMonitoringPage.Size = new System.Drawing.Size(56, 20);
            this.textBoxTemperatureMonitoringPage.TabIndex = 47;
            this.textBoxTemperatureMonitoringPage.TabStop = false;
            // 
            // labelTemperatureMonitoringPage
            // 
            this.labelTemperatureMonitoringPage.AutoSize = true;
            this.labelTemperatureMonitoringPage.Location = new System.Drawing.Point(3, 581);
            this.labelTemperatureMonitoringPage.Name = "labelTemperatureMonitoringPage";
            this.labelTemperatureMonitoringPage.Size = new System.Drawing.Size(86, 13);
            this.labelTemperatureMonitoringPage.TabIndex = 46;
            this.labelTemperatureMonitoringPage.Text = "Temperature (C):";
            // 
            // buttonUpdateCTRatio
            // 
            this.buttonUpdateCTRatio.Location = new System.Drawing.Point(681, 597);
            this.buttonUpdateCTRatio.Name = "buttonUpdateCTRatio";
            this.buttonUpdateCTRatio.Size = new System.Drawing.Size(100, 23);
            this.buttonUpdateCTRatio.TabIndex = 44;
            this.buttonUpdateCTRatio.Text = "Update CT Ratio";
            this.buttonUpdateCTRatio.UseVisualStyleBackColor = true;
            this.buttonUpdateCTRatio.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelRelayTrippedOrClose
            // 
            this.labelRelayTrippedOrClose.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelRelayTrippedOrClose.Location = new System.Drawing.Point(420, 571);
            this.labelRelayTrippedOrClose.Margin = new System.Windows.Forms.Padding(3);
            this.labelRelayTrippedOrClose.Name = "labelRelayTrippedOrClose";
            this.labelRelayTrippedOrClose.Padding = new System.Windows.Forms.Padding(1);
            this.labelRelayTrippedOrClose.Size = new System.Drawing.Size(100, 26);
            this.labelRelayTrippedOrClose.TabIndex = 36;
            this.labelRelayTrippedOrClose.Text = "Unkown";
            this.labelRelayTrippedOrClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonToggleMonitor
            // 
            this.buttonToggleMonitor.Location = new System.Drawing.Point(425, 599);
            this.buttonToggleMonitor.Name = "buttonToggleMonitor";
            this.buttonToggleMonitor.Size = new System.Drawing.Size(91, 23);
            this.buttonToggleMonitor.TabIndex = 35;
            this.buttonToggleMonitor.Text = "Toggle Monitor";
            this.buttonToggleMonitor.UseVisualStyleBackColor = true;
            this.buttonToggleMonitor.Click += new System.EventHandler(this.buttonToggleMonitor_Click);
            // 
            // labelCtRatioMonitor
            // 
            this.labelCtRatioMonitor.AutoSize = true;
            this.labelCtRatioMonitor.Location = new System.Drawing.Point(306, 582);
            this.labelCtRatioMonitor.Name = "labelCtRatioMonitor";
            this.labelCtRatioMonitor.Size = new System.Drawing.Size(52, 13);
            this.labelCtRatioMonitor.TabIndex = 31;
            this.labelCtRatioMonitor.Text = "CT Ratio:";
            // 
            // ucPhasorGraph1
            // 
            this.ucPhasorGraph1.BackColor = System.Drawing.Color.Transparent;
            this.ucPhasorGraph1.GEEnabled = false;
            this.ucPhasorGraph1.Location = new System.Drawing.Point(0, 0);
            this.ucPhasorGraph1.Name = "ucPhasorGraph1";
            this.ucPhasorGraph1.RealTimeMonitoring = false;
            this.ucPhasorGraph1.RevisionNumber = ((uint)(0u));
            this.ucPhasorGraph1.Size = new System.Drawing.Size(992, 572);
            this.ucPhasorGraph1.TabIndex = 45;
            // 
            // tabPageControl
            // 
            this.tabPageControl.Controls.Add(this.groupBoxLRLockoutMain);
            this.tabPageControl.Controls.Add(this.groupBoxLowVoltThres);
            this.tabPageControl.Controls.Add(this.groupBoxRelayStatus);
            this.tabPageControl.Controls.Add(this.groupBoxPhasingAndType);
            this.tabPageControl.Controls.Add(this.groupBoxNetworkCTRatio);
            this.tabPageControl.Controls.Add(this.groupBoxRelayFlags);
            this.tabPageControl.Controls.Add(this.ucSafeService1);
            this.tabPageControl.Controls.Add(this.panelOtherRelayControls);
            this.tabPageControl.Controls.Add(this.checkBox277Protector);
            this.tabPageControl.Controls.Add(this.ucTripMode2);
            this.tabPageControl.Controls.Add(this.ucCloseMode1);
            this.tabPageControl.Controls.Add(this.ucPumpMode1);
            this.tabPageControl.Location = new System.Drawing.Point(4, 22);
            this.tabPageControl.Name = "tabPageControl";
            this.tabPageControl.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageControl.Size = new System.Drawing.Size(991, 624);
            this.tabPageControl.TabIndex = 0;
            this.tabPageControl.Text = "Relay Settings";
            this.tabPageControl.UseVisualStyleBackColor = true;
            // 
            // groupBoxLRLockoutMain
            // 
            this.groupBoxLRLockoutMain.Controls.Add(this.textBoxLRLockoutStatusMain);
            this.groupBoxLRLockoutMain.Controls.Add(this.labelLRLockoutMain);
            this.groupBoxLRLockoutMain.Location = new System.Drawing.Point(448, 473);
            this.groupBoxLRLockoutMain.Name = "groupBoxLRLockoutMain";
            this.groupBoxLRLockoutMain.Size = new System.Drawing.Size(254, 49);
            this.groupBoxLRLockoutMain.TabIndex = 118;
            this.groupBoxLRLockoutMain.TabStop = false;
            this.groupBoxLRLockoutMain.Text = "Remote Command Lockout";
            this.groupBoxLRLockoutMain.Visible = false;
            // 
            // textBoxLRLockoutStatusMain
            // 
            this.textBoxLRLockoutStatusMain.Location = new System.Drawing.Point(123, 20);
            this.textBoxLRLockoutStatusMain.Name = "textBoxLRLockoutStatusMain";
            this.textBoxLRLockoutStatusMain.Size = new System.Drawing.Size(100, 20);
            this.textBoxLRLockoutStatusMain.TabIndex = 1;
            // 
            // labelLRLockoutMain
            // 
            this.labelLRLockoutMain.AutoSize = true;
            this.labelLRLockoutMain.Location = new System.Drawing.Point(13, 23);
            this.labelLRLockoutMain.Name = "labelLRLockoutMain";
            this.labelLRLockoutMain.Size = new System.Drawing.Size(100, 13);
            this.labelLRLockoutMain.TabIndex = 0;
            this.labelLRLockoutMain.Text = "RC Lockout Status:";
            // 
            // groupBoxLowVoltThres
            // 
            this.groupBoxLowVoltThres.Controls.Add(this.buttonRequestLowVotlageThres);
            this.groupBoxLowVoltThres.Controls.Add(this.numericUpDownLowVoltageThres);
            this.groupBoxLowVoltThres.Controls.Add(this.buttonSendLowVoltageThres);
            this.groupBoxLowVoltThres.Location = new System.Drawing.Point(11, 530);
            this.groupBoxLowVoltThres.Name = "groupBoxLowVoltThres";
            this.groupBoxLowVoltThres.Size = new System.Drawing.Size(331, 88);
            this.groupBoxLowVoltThres.TabIndex = 117;
            this.groupBoxLowVoltThres.TabStop = false;
            this.groupBoxLowVoltThres.Text = "Low Voltage Threshold";
            // 
            // buttonRequestLowVotlageThres
            // 
            this.buttonRequestLowVotlageThres.Location = new System.Drawing.Point(251, 30);
            this.buttonRequestLowVotlageThres.Name = "buttonRequestLowVotlageThres";
            this.buttonRequestLowVotlageThres.Size = new System.Drawing.Size(75, 23);
            this.buttonRequestLowVotlageThres.TabIndex = 116;
            this.buttonRequestLowVotlageThres.Text = "Request";
            this.buttonRequestLowVotlageThres.UseVisualStyleBackColor = true;
            this.buttonRequestLowVotlageThres.Click += new System.EventHandler(this.buttonRequestLowVotlageThres_Click);
            // 
            // numericUpDownLowVoltageThres
            // 
            this.numericUpDownLowVoltageThres.Location = new System.Drawing.Point(14, 33);
            this.numericUpDownLowVoltageThres.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericUpDownLowVoltageThres.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownLowVoltageThres.Name = "numericUpDownLowVoltageThres";
            this.numericUpDownLowVoltageThres.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownLowVoltageThres.TabIndex = 114;
            this.numericUpDownLowVoltageThres.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // buttonSendLowVoltageThres
            // 
            this.buttonSendLowVoltageThres.Location = new System.Drawing.Point(159, 30);
            this.buttonSendLowVoltageThres.Name = "buttonSendLowVoltageThres";
            this.buttonSendLowVoltageThres.Size = new System.Drawing.Size(75, 23);
            this.buttonSendLowVoltageThres.TabIndex = 113;
            this.buttonSendLowVoltageThres.Text = "Send";
            this.buttonSendLowVoltageThres.UseVisualStyleBackColor = true;
            this.buttonSendLowVoltageThres.Click += new System.EventHandler(this.buttonSendLowVoltageThres_Click);
            // 
            // groupBoxRelayStatus
            // 
            this.groupBoxRelayStatus.Controls.Add(this.labelNWPStatus);
            this.groupBoxRelayStatus.Controls.Add(this.checkBoxTripFlag);
            this.groupBoxRelayStatus.Controls.Add(this.checkBoxPhasingOkayFlag);
            this.groupBoxRelayStatus.Controls.Add(this.checkBoxPumping);
            this.groupBoxRelayStatus.Controls.Add(this.textBoxTemperature);
            this.groupBoxRelayStatus.Controls.Add(this.checkBoxDefaultsUsed);
            this.groupBoxRelayStatus.Controls.Add(this.checkBoxBlockedOpenFlag);
            this.groupBoxRelayStatus.Controls.Add(this.textBoxTripCount);
            this.groupBoxRelayStatus.Controls.Add(labelTemperature);
            this.groupBoxRelayStatus.Controls.Add(this.label4);
            this.groupBoxRelayStatus.Controls.Add(this.checkBoxBFlag);
            this.groupBoxRelayStatus.Controls.Add(this.checkBoxTrippingFlag);
            this.groupBoxRelayStatus.Controls.Add(this.checkBoxFloatFlag);
            this.groupBoxRelayStatus.Location = new System.Drawing.Point(599, 7);
            this.groupBoxRelayStatus.Name = "groupBoxRelayStatus";
            this.groupBoxRelayStatus.Size = new System.Drawing.Size(132, 244);
            this.groupBoxRelayStatus.TabIndex = 112;
            this.groupBoxRelayStatus.TabStop = false;
            this.groupBoxRelayStatus.Text = "Relay Status:";
            // 
            // labelNWPStatus
            // 
            this.labelNWPStatus.AutoSize = true;
            this.labelNWPStatus.Location = new System.Drawing.Point(9, 178);
            this.labelNWPStatus.Name = "labelNWPStatus";
            this.labelNWPStatus.Size = new System.Drawing.Size(85, 13);
            this.labelNWPStatus.TabIndex = 52;
            this.labelNWPStatus.Text = "NWP: Unknown";
            // 
            // checkBoxTripFlag
            // 
            this.checkBoxTripFlag.AutoCheck = false;
            this.checkBoxTripFlag.AutoSize = true;
            this.checkBoxTripFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxTripFlag.Location = new System.Drawing.Point(9, 19);
            this.checkBoxTripFlag.Name = "checkBoxTripFlag";
            this.checkBoxTripFlag.Size = new System.Drawing.Size(62, 17);
            this.checkBoxTripFlag.TabIndex = 0;
            this.checkBoxTripFlag.Text = "Tripped";
            this.checkBoxTripFlag.UseVisualStyleBackColor = true;
            // 
            // checkBoxPhasingOkayFlag
            // 
            this.checkBoxPhasingOkayFlag.AutoCheck = false;
            this.checkBoxPhasingOkayFlag.AutoSize = true;
            this.checkBoxPhasingOkayFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxPhasingOkayFlag.Location = new System.Drawing.Point(9, 135);
            this.checkBoxPhasingOkayFlag.Name = "checkBoxPhasingOkayFlag";
            this.checkBoxPhasingOkayFlag.Size = new System.Drawing.Size(92, 17);
            this.checkBoxPhasingOkayFlag.TabIndex = 50;
            this.checkBoxPhasingOkayFlag.Text = "Phasing Okay";
            this.checkBoxPhasingOkayFlag.UseVisualStyleBackColor = true;
            // 
            // checkBoxPumping
            // 
            this.checkBoxPumping.AutoCheck = false;
            this.checkBoxPumping.AutoSize = true;
            this.checkBoxPumping.ForeColor = System.Drawing.Color.Black;
            this.checkBoxPumping.Location = new System.Drawing.Point(9, 156);
            this.checkBoxPumping.Name = "checkBoxPumping";
            this.checkBoxPumping.Size = new System.Drawing.Size(90, 17);
            this.checkBoxPumping.TabIndex = 35;
            this.checkBoxPumping.Text = "Pump Protect";
            this.checkBoxPumping.UseVisualStyleBackColor = true;
            // 
            // textBoxTemperature
            // 
            this.textBoxTemperature.Location = new System.Drawing.Point(8, 217);
            this.textBoxTemperature.Name = "textBoxTemperature";
            this.textBoxTemperature.ReadOnly = true;
            this.textBoxTemperature.Size = new System.Drawing.Size(48, 20);
            this.textBoxTemperature.TabIndex = 51;
            // 
            // checkBoxDefaultsUsed
            // 
            this.checkBoxDefaultsUsed.AutoCheck = false;
            this.checkBoxDefaultsUsed.AutoSize = true;
            this.checkBoxDefaultsUsed.ForeColor = System.Drawing.Color.Black;
            this.checkBoxDefaultsUsed.Location = new System.Drawing.Point(9, 198);
            this.checkBoxDefaultsUsed.Name = "checkBoxDefaultsUsed";
            this.checkBoxDefaultsUsed.Size = new System.Drawing.Size(93, 17);
            this.checkBoxDefaultsUsed.TabIndex = 38;
            this.checkBoxDefaultsUsed.Text = "Defaults Used";
            this.checkBoxDefaultsUsed.UseVisualStyleBackColor = true;
            // 
            // checkBoxBlockedOpenFlag
            // 
            this.checkBoxBlockedOpenFlag.AutoCheck = false;
            this.checkBoxBlockedOpenFlag.AutoSize = true;
            this.checkBoxBlockedOpenFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxBlockedOpenFlag.Location = new System.Drawing.Point(9, 114);
            this.checkBoxBlockedOpenFlag.Name = "checkBoxBlockedOpenFlag";
            this.checkBoxBlockedOpenFlag.Size = new System.Drawing.Size(94, 17);
            this.checkBoxBlockedOpenFlag.TabIndex = 48;
            this.checkBoxBlockedOpenFlag.Text = "Blocked Open";
            this.checkBoxBlockedOpenFlag.UseVisualStyleBackColor = true;
            // 
            // textBoxTripCount
            // 
            this.textBoxTripCount.Location = new System.Drawing.Point(9, 86);
            this.textBoxTripCount.Name = "textBoxTripCount";
            this.textBoxTripCount.ReadOnly = true;
            this.textBoxTripCount.Size = new System.Drawing.Size(47, 20);
            this.textBoxTripCount.TabIndex = 43;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(62, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 44;
            this.label4.Text = "Trip Cycles";
            // 
            // checkBoxBFlag
            // 
            this.checkBoxBFlag.AutoCheck = false;
            this.checkBoxBFlag.AutoSize = true;
            this.checkBoxBFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxBFlag.Location = new System.Drawing.Point(9, 177);
            this.checkBoxBFlag.Name = "checkBoxBFlag";
            this.checkBoxBFlag.Size = new System.Drawing.Size(102, 17);
            this.checkBoxBFlag.TabIndex = 46;
            this.checkBoxBFlag.Text = "Protector Status";
            this.checkBoxBFlag.UseVisualStyleBackColor = true;
            this.checkBoxBFlag.Visible = false;
            // 
            // checkBoxTrippingFlag
            // 
            this.checkBoxTrippingFlag.AutoCheck = false;
            this.checkBoxTrippingFlag.AutoSize = true;
            this.checkBoxTrippingFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxTrippingFlag.Location = new System.Drawing.Point(9, 40);
            this.checkBoxTrippingFlag.Name = "checkBoxTrippingFlag";
            this.checkBoxTrippingFlag.Size = new System.Drawing.Size(64, 17);
            this.checkBoxTrippingFlag.TabIndex = 46;
            this.checkBoxTrippingFlag.Text = "Tripping";
            this.checkBoxTrippingFlag.UseVisualStyleBackColor = true;
            // 
            // checkBoxFloatFlag
            // 
            this.checkBoxFloatFlag.AutoCheck = false;
            this.checkBoxFloatFlag.AutoSize = true;
            this.checkBoxFloatFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxFloatFlag.Location = new System.Drawing.Point(9, 63);
            this.checkBoxFloatFlag.Name = "checkBoxFloatFlag";
            this.checkBoxFloatFlag.Size = new System.Drawing.Size(49, 17);
            this.checkBoxFloatFlag.TabIndex = 47;
            this.checkBoxFloatFlag.Text = "Float";
            this.checkBoxFloatFlag.UseVisualStyleBackColor = true;
            // 
            // groupBoxPhasingAndType
            // 
            this.groupBoxPhasingAndType.Controls.Add(this.labelConEdPowerRelay);
            this.groupBoxPhasingAndType.Controls.Add(this.label20);
            this.groupBoxPhasingAndType.Controls.Add(this.labelProtectorType);
            this.groupBoxPhasingAndType.Controls.Add(this.domainUpDownPhasings);
            this.groupBoxPhasingAndType.Controls.Add(this.labelGEWH);
            this.groupBoxPhasingAndType.Controls.Add(this.domainUpDownRelayType);
            this.groupBoxPhasingAndType.Controls.Add(this.buttonTypePhasingRestoreDefaults);
            this.groupBoxPhasingAndType.Controls.Add(this.buttonRelayType);
            this.groupBoxPhasingAndType.Location = new System.Drawing.Point(11, 338);
            this.groupBoxPhasingAndType.Name = "groupBoxPhasingAndType";
            this.groupBoxPhasingAndType.Size = new System.Drawing.Size(173, 121);
            this.groupBoxPhasingAndType.TabIndex = 111;
            this.groupBoxPhasingAndType.TabStop = false;
            this.groupBoxPhasingAndType.Text = "Relay Phasing and Type:";
            // 
            // labelConEdPowerRelay
            // 
            this.labelConEdPowerRelay.AutoSize = true;
            this.labelConEdPowerRelay.Location = new System.Drawing.Point(81, 16);
            this.labelConEdPowerRelay.Name = "labelConEdPowerRelay";
            this.labelConEdPowerRelay.Size = new System.Drawing.Size(37, 13);
            this.labelConEdPowerRelay.TabIndex = 51;
            this.labelConEdPowerRelay.Text = "Power";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(11, 16);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(64, 13);
            this.label20.TabIndex = 44;
            this.label20.Text = "Relay Type:";
            // 
            // labelProtectorType
            // 
            this.labelProtectorType.AutoSize = true;
            this.labelProtectorType.Location = new System.Drawing.Point(12, 72);
            this.labelProtectorType.Name = "labelProtectorType";
            this.labelProtectorType.Size = new System.Drawing.Size(80, 13);
            this.labelProtectorType.TabIndex = 50;
            this.labelProtectorType.Text = "Protector Type:";
            // 
            // domainUpDownPhasings
            // 
            this.domainUpDownPhasings.Items.Add("ABC");
            this.domainUpDownPhasings.Items.Add("ACB");
            this.domainUpDownPhasings.Items.Add("AutoDetect");
            this.domainUpDownPhasings.Location = new System.Drawing.Point(79, 39);
            this.domainUpDownPhasings.Name = "domainUpDownPhasings";
            this.domainUpDownPhasings.Size = new System.Drawing.Size(84, 20);
            this.domainUpDownPhasings.TabIndex = 47;
            // 
            // labelGEWH
            // 
            this.labelGEWH.AutoSize = true;
            this.labelGEWH.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelGEWH.Location = new System.Drawing.Point(98, 72);
            this.labelGEWH.Name = "labelGEWH";
            this.labelGEWH.Size = new System.Drawing.Size(28, 15);
            this.labelGEWH.TabIndex = 49;
            this.labelGEWH.Text = "WH";
            // 
            // domainUpDownRelayType
            // 
            this.domainUpDownRelayType.Items.Add("Power");
            this.domainUpDownRelayType.Items.Add("Sequence");
            this.domainUpDownRelayType.Location = new System.Drawing.Point(79, 14);
            this.domainUpDownRelayType.Name = "domainUpDownRelayType";
            this.domainUpDownRelayType.Size = new System.Drawing.Size(84, 20);
            this.domainUpDownRelayType.TabIndex = 43;
            this.domainUpDownRelayType.SelectedItemChanged += new System.EventHandler(this.domainUpDownRelayType_SelectedItemChanged);
            // 
            // buttonTypePhasingRestoreDefaults
            // 
            this.buttonTypePhasingRestoreDefaults.Location = new System.Drawing.Point(8, 97);
            this.buttonTypePhasingRestoreDefaults.Name = "buttonTypePhasingRestoreDefaults";
            this.buttonTypePhasingRestoreDefaults.Size = new System.Drawing.Size(96, 23);
            this.buttonTypePhasingRestoreDefaults.TabIndex = 48;
            this.buttonTypePhasingRestoreDefaults.Text = "Restore Defaults";
            this.buttonTypePhasingRestoreDefaults.UseVisualStyleBackColor = true;
            this.buttonTypePhasingRestoreDefaults.Click += new System.EventHandler(this.buttonTypePhasingRestoreDefaults_Click);
            // 
            // buttonRelayType
            // 
            this.buttonRelayType.Location = new System.Drawing.Point(119, 97);
            this.buttonRelayType.Name = "buttonRelayType";
            this.buttonRelayType.Size = new System.Drawing.Size(45, 23);
            this.buttonRelayType.TabIndex = 41;
            this.buttonRelayType.Text = "Send";
            this.buttonRelayType.UseVisualStyleBackColor = true;
            this.buttonRelayType.Click += new System.EventHandler(this.buttonRelayType_Click);
            // 
            // groupBoxNetworkCTRatio
            // 
            this.groupBoxNetworkCTRatio.Controls.Add(this.labelOver5);
            this.groupBoxNetworkCTRatio.Controls.Add(this.label27);
            this.groupBoxNetworkCTRatio.Controls.Add(this.buttonSendCTRatio);
            this.groupBoxNetworkCTRatio.Controls.Add(this.textBoxCTRatio);
            this.groupBoxNetworkCTRatio.Controls.Add(this.domainUpDownCTRatioM);
            this.groupBoxNetworkCTRatio.Location = new System.Drawing.Point(11, 263);
            this.groupBoxNetworkCTRatio.Name = "groupBoxNetworkCTRatio";
            this.groupBoxNetworkCTRatio.Size = new System.Drawing.Size(173, 69);
            this.groupBoxNetworkCTRatio.TabIndex = 110;
            this.groupBoxNetworkCTRatio.TabStop = false;
            this.groupBoxNetworkCTRatio.Text = "Network Protector CT Ratio:";
            // 
            // labelOver5
            // 
            this.labelOver5.AutoSize = true;
            this.labelOver5.Location = new System.Drawing.Point(57, 49);
            this.labelOver5.Name = "labelOver5";
            this.labelOver5.Size = new System.Drawing.Size(18, 13);
            this.labelOver5.TabIndex = 73;
            this.labelOver5.Text = "/5";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 20);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(35, 13);
            this.label27.TabIndex = 68;
            this.label27.Text = "Ratio:";
            // 
            // buttonSendCTRatio
            // 
            this.buttonSendCTRatio.Location = new System.Drawing.Point(84, 44);
            this.buttonSendCTRatio.Name = "buttonSendCTRatio";
            this.buttonSendCTRatio.Size = new System.Drawing.Size(75, 23);
            this.buttonSendCTRatio.TabIndex = 61;
            this.buttonSendCTRatio.Text = "Send";
            this.buttonSendCTRatio.UseVisualStyleBackColor = true;
            this.buttonSendCTRatio.Click += new System.EventHandler(this.buttonSendCTRatio_Click);
            // 
            // textBoxCTRatio
            // 
            this.textBoxCTRatio.Enabled = false;
            this.textBoxCTRatio.Location = new System.Drawing.Point(7, 46);
            this.textBoxCTRatio.Name = "textBoxCTRatio";
            this.textBoxCTRatio.Size = new System.Drawing.Size(48, 20);
            this.textBoxCTRatio.TabIndex = 64;
            this.textBoxCTRatio.Text = "320";
            this.textBoxCTRatio.Leave += new System.EventHandler(this.textBoxCTRatio_Leave);
            // 
            // domainUpDownCTRatioM
            // 
            this.domainUpDownCTRatioM.Items.Add("3500:5");
            this.domainUpDownCTRatioM.Items.Add("3000:5");
            this.domainUpDownCTRatioM.Items.Add("2500:5");
            this.domainUpDownCTRatioM.Items.Add("2000:5");
            this.domainUpDownCTRatioM.Items.Add("1600:5");
            this.domainUpDownCTRatioM.Items.Add("1200:5");
            this.domainUpDownCTRatioM.Items.Add("800:5");
            this.domainUpDownCTRatioM.Items.Add("Special");
            this.domainUpDownCTRatioM.Location = new System.Drawing.Point(47, 18);
            this.domainUpDownCTRatioM.Name = "domainUpDownCTRatioM";
            this.domainUpDownCTRatioM.Size = new System.Drawing.Size(61, 20);
            this.domainUpDownCTRatioM.TabIndex = 62;
            this.domainUpDownCTRatioM.Text = "1600:5";
            this.domainUpDownCTRatioM.SelectedItemChanged += new System.EventHandler(this.domainUpDownCTRatioM_SelectedItemChanged);
            // 
            // groupBoxRelayFlags
            // 
            this.groupBoxRelayFlags.Controls.Add(this.labelQuietMode);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxOffsetOkay);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxCalibrating);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxMathOverTime);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxBlockedCloseFlag);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxMathError);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxInInsensRegion);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxMonitorPhasors);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxFlag2);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxSequence);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxACB);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxFlag1);
            this.groupBoxRelayFlags.Controls.Add(this.checkBoxPowerSaveFlag);
            this.groupBoxRelayFlags.Location = new System.Drawing.Point(567, 521);
            this.groupBoxRelayFlags.Name = "groupBoxRelayFlags";
            this.groupBoxRelayFlags.Size = new System.Drawing.Size(421, 97);
            this.groupBoxRelayFlags.TabIndex = 109;
            this.groupBoxRelayFlags.TabStop = false;
            this.groupBoxRelayFlags.Text = "Relay Flags:";
            // 
            // labelQuietMode
            // 
            this.labelQuietMode.AutoSize = true;
            this.labelQuietMode.BackColor = System.Drawing.Color.Yellow;
            this.labelQuietMode.Location = new System.Drawing.Point(185, 79);
            this.labelQuietMode.Name = "labelQuietMode";
            this.labelQuietMode.Size = new System.Drawing.Size(62, 13);
            this.labelQuietMode.TabIndex = 52;
            this.labelQuietMode.Text = "Quiet Mode";
            this.labelQuietMode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelQuietMode_MouseDown);
            // 
            // checkBoxOffsetOkay
            // 
            this.checkBoxOffsetOkay.AutoCheck = false;
            this.checkBoxOffsetOkay.AutoSize = true;
            this.checkBoxOffsetOkay.ForeColor = System.Drawing.Color.Black;
            this.checkBoxOffsetOkay.Location = new System.Drawing.Point(6, 19);
            this.checkBoxOffsetOkay.Name = "checkBoxOffsetOkay";
            this.checkBoxOffsetOkay.Size = new System.Drawing.Size(82, 17);
            this.checkBoxOffsetOkay.TabIndex = 37;
            this.checkBoxOffsetOkay.Text = "Offset Okay";
            this.checkBoxOffsetOkay.UseVisualStyleBackColor = true;
            // 
            // checkBoxCalibrating
            // 
            this.checkBoxCalibrating.AutoCheck = false;
            this.checkBoxCalibrating.AutoSize = true;
            this.checkBoxCalibrating.ForeColor = System.Drawing.Color.Black;
            this.checkBoxCalibrating.Location = new System.Drawing.Point(106, 60);
            this.checkBoxCalibrating.Name = "checkBoxCalibrating";
            this.checkBoxCalibrating.Size = new System.Drawing.Size(75, 17);
            this.checkBoxCalibrating.TabIndex = 51;
            this.checkBoxCalibrating.Text = "Calibrating";
            this.checkBoxCalibrating.UseVisualStyleBackColor = true;
            // 
            // checkBoxMathOverTime
            // 
            this.checkBoxMathOverTime.AutoCheck = false;
            this.checkBoxMathOverTime.AutoSize = true;
            this.checkBoxMathOverTime.ForeColor = System.Drawing.Color.Black;
            this.checkBoxMathOverTime.Location = new System.Drawing.Point(285, 58);
            this.checkBoxMathOverTime.Name = "checkBoxMathOverTime";
            this.checkBoxMathOverTime.Size = new System.Drawing.Size(102, 17);
            this.checkBoxMathOverTime.TabIndex = 36;
            this.checkBoxMathOverTime.Text = "Math Over Time";
            this.checkBoxMathOverTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxBlockedCloseFlag
            // 
            this.checkBoxBlockedCloseFlag.AutoCheck = false;
            this.checkBoxBlockedCloseFlag.AutoSize = true;
            this.checkBoxBlockedCloseFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxBlockedCloseFlag.Location = new System.Drawing.Point(185, 38);
            this.checkBoxBlockedCloseFlag.Name = "checkBoxBlockedCloseFlag";
            this.checkBoxBlockedCloseFlag.Size = new System.Drawing.Size(94, 17);
            this.checkBoxBlockedCloseFlag.TabIndex = 49;
            this.checkBoxBlockedCloseFlag.Text = "Blocked Close";
            this.checkBoxBlockedCloseFlag.UseVisualStyleBackColor = true;
            // 
            // checkBoxMathError
            // 
            this.checkBoxMathError.AutoCheck = false;
            this.checkBoxMathError.AutoSize = true;
            this.checkBoxMathError.ForeColor = System.Drawing.Color.Black;
            this.checkBoxMathError.Location = new System.Drawing.Point(285, 38);
            this.checkBoxMathError.Name = "checkBoxMathError";
            this.checkBoxMathError.Size = new System.Drawing.Size(75, 17);
            this.checkBoxMathError.TabIndex = 39;
            this.checkBoxMathError.Text = "Math Error";
            this.checkBoxMathError.UseVisualStyleBackColor = true;
            // 
            // checkBoxInInsensRegion
            // 
            this.checkBoxInInsensRegion.AutoCheck = false;
            this.checkBoxInInsensRegion.AutoSize = true;
            this.checkBoxInInsensRegion.ForeColor = System.Drawing.Color.Black;
            this.checkBoxInInsensRegion.Location = new System.Drawing.Point(263, 19);
            this.checkBoxInInsensRegion.Name = "checkBoxInInsensRegion";
            this.checkBoxInInsensRegion.Size = new System.Drawing.Size(125, 17);
            this.checkBoxInInsensRegion.TabIndex = 47;
            this.checkBoxInInsensRegion.Text = "In Insensitive Reigon";
            this.checkBoxInInsensRegion.UseVisualStyleBackColor = true;
            // 
            // checkBoxMonitorPhasors
            // 
            this.checkBoxMonitorPhasors.AutoCheck = false;
            this.checkBoxMonitorPhasors.AutoSize = true;
            this.checkBoxMonitorPhasors.ForeColor = System.Drawing.Color.Black;
            this.checkBoxMonitorPhasors.Location = new System.Drawing.Point(6, 39);
            this.checkBoxMonitorPhasors.Name = "checkBoxMonitorPhasors";
            this.checkBoxMonitorPhasors.Size = new System.Drawing.Size(102, 17);
            this.checkBoxMonitorPhasors.TabIndex = 40;
            this.checkBoxMonitorPhasors.Text = "Monitor Phasors";
            this.checkBoxMonitorPhasors.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlag2
            // 
            this.checkBoxFlag2.AutoCheck = false;
            this.checkBoxFlag2.AutoSize = true;
            this.checkBoxFlag2.ForeColor = System.Drawing.Color.Black;
            this.checkBoxFlag2.Location = new System.Drawing.Point(106, 37);
            this.checkBoxFlag2.Name = "checkBoxFlag2";
            this.checkBoxFlag2.Size = new System.Drawing.Size(52, 17);
            this.checkBoxFlag2.TabIndex = 45;
            this.checkBoxFlag2.Text = "Flag2";
            this.checkBoxFlag2.UseVisualStyleBackColor = true;
            // 
            // checkBoxSequence
            // 
            this.checkBoxSequence.AutoCheck = false;
            this.checkBoxSequence.AutoSize = true;
            this.checkBoxSequence.ForeColor = System.Drawing.Color.Black;
            this.checkBoxSequence.Location = new System.Drawing.Point(6, 59);
            this.checkBoxSequence.Name = "checkBoxSequence";
            this.checkBoxSequence.Size = new System.Drawing.Size(75, 17);
            this.checkBoxSequence.TabIndex = 42;
            this.checkBoxSequence.Text = "Sequence";
            this.checkBoxSequence.UseVisualStyleBackColor = true;
            // 
            // checkBoxACB
            // 
            this.checkBoxACB.AutoCheck = false;
            this.checkBoxACB.AutoSize = true;
            this.checkBoxACB.ForeColor = System.Drawing.Color.Black;
            this.checkBoxACB.Location = new System.Drawing.Point(185, 19);
            this.checkBoxACB.Name = "checkBoxACB";
            this.checkBoxACB.Size = new System.Drawing.Size(47, 17);
            this.checkBoxACB.TabIndex = 43;
            this.checkBoxACB.Text = "ACB";
            this.checkBoxACB.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlag1
            // 
            this.checkBoxFlag1.AutoCheck = false;
            this.checkBoxFlag1.AutoSize = true;
            this.checkBoxFlag1.ForeColor = System.Drawing.Color.Black;
            this.checkBoxFlag1.Location = new System.Drawing.Point(106, 17);
            this.checkBoxFlag1.Name = "checkBoxFlag1";
            this.checkBoxFlag1.Size = new System.Drawing.Size(52, 17);
            this.checkBoxFlag1.TabIndex = 44;
            this.checkBoxFlag1.Text = "Flag1";
            this.checkBoxFlag1.UseVisualStyleBackColor = true;
            // 
            // checkBoxPowerSaveFlag
            // 
            this.checkBoxPowerSaveFlag.AutoCheck = false;
            this.checkBoxPowerSaveFlag.AutoSize = true;
            this.checkBoxPowerSaveFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxPowerSaveFlag.Location = new System.Drawing.Point(185, 59);
            this.checkBoxPowerSaveFlag.Name = "checkBoxPowerSaveFlag";
            this.checkBoxPowerSaveFlag.Size = new System.Drawing.Size(84, 17);
            this.checkBoxPowerSaveFlag.TabIndex = 45;
            this.checkBoxPowerSaveFlag.Text = "Power Save";
            this.checkBoxPowerSaveFlag.UseVisualStyleBackColor = true;
            // 
            // ucSafeService1
            // 
            this.ucSafeService1.CTRatio = 320;
            this.ucSafeService1.EnableSafeService = false;
            this.ucSafeService1.LoadingNewCode = false;
            this.ucSafeService1.Location = new System.Drawing.Point(734, 4);
            this.ucSafeService1.Name = "ucSafeService1";
            this.ucSafeService1.Size = new System.Drawing.Size(232, 250);
            this.ucSafeService1.TabIndex = 108;
            this.ucSafeService1.Voltage277State = false;
            // 
            // panelOtherRelayControls
            // 
            this.panelOtherRelayControls.Controls.Add(this.buttonClearCycleCount);
            this.panelOtherRelayControls.Controls.Add(this.buttonBlockAndTrip);
            this.panelOtherRelayControls.Controls.Add(this.buttonBlockedStateOpen);
            this.panelOtherRelayControls.Controls.Add(this.buttonRequestRelayParamaters);
            this.panelOtherRelayControls.Controls.Add(this.labelRelaySNControl);
            this.panelOtherRelayControls.Controls.Add(this.buttonUnblockOpen);
            this.panelOtherRelayControls.Controls.Add(this.textBoxRelaySNControl);
            this.panelOtherRelayControls.Controls.Add(this.labelRevision);
            this.panelOtherRelayControls.Controls.Add(this.labelFPGARevision);
            this.panelOtherRelayControls.Controls.Add(this.labelRelayRevision);
            this.panelOtherRelayControls.Controls.Add(this.buttonResetBothProc);
            this.panelOtherRelayControls.Controls.Add(this.labelRelayStateControlPage);
            this.panelOtherRelayControls.Controls.Add(this.buttonSendAll);
            this.panelOtherRelayControls.Controls.Add(this.labelBlockedOpenState);
            this.panelOtherRelayControls.Controls.Add(this.textBoxSaveStateName);
            this.panelOtherRelayControls.Controls.Add(this.buttonTripRelay);
            this.panelOtherRelayControls.Controls.Add(this.labelRelayDisconnected);
            this.panelOtherRelayControls.Controls.Add(this.comboBoxSavedStates);
            this.panelOtherRelayControls.Controls.Add(this.buttonSaveSetting);
            this.panelOtherRelayControls.Controls.Add(this.buttonDeleteSetting);
            this.panelOtherRelayControls.Location = new System.Drawing.Point(444, 268);
            this.panelOtherRelayControls.Name = "panelOtherRelayControls";
            this.panelOtherRelayControls.Size = new System.Drawing.Size(532, 181);
            this.panelOtherRelayControls.TabIndex = 77;
            // 
            // buttonClearCycleCount
            // 
            this.buttonClearCycleCount.Location = new System.Drawing.Point(3, 3);
            this.buttonClearCycleCount.Name = "buttonClearCycleCount";
            this.buttonClearCycleCount.Size = new System.Drawing.Size(111, 23);
            this.buttonClearCycleCount.TabIndex = 62;
            this.buttonClearCycleCount.Text = "Clear Cycle Count";
            this.buttonClearCycleCount.UseVisualStyleBackColor = true;
            this.buttonClearCycleCount.Click += new System.EventHandler(this.buttonClearCycleCount_Click);
            // 
            // buttonBlockAndTrip
            // 
            this.buttonBlockAndTrip.Location = new System.Drawing.Point(262, 60);
            this.buttonBlockAndTrip.Name = "buttonBlockAndTrip";
            this.buttonBlockAndTrip.Size = new System.Drawing.Size(121, 23);
            this.buttonBlockAndTrip.TabIndex = 76;
            this.buttonBlockAndTrip.Text = "Block and Trip Relay";
            this.buttonBlockAndTrip.UseVisualStyleBackColor = true;
            this.buttonBlockAndTrip.Click += new System.EventHandler(this.buttonBlockAndTrip_Click);
            // 
            // buttonBlockedStateOpen
            // 
            this.buttonBlockedStateOpen.Location = new System.Drawing.Point(411, 3);
            this.buttonBlockedStateOpen.Name = "buttonBlockedStateOpen";
            this.buttonBlockedStateOpen.Size = new System.Drawing.Size(111, 23);
            this.buttonBlockedStateOpen.TabIndex = 8;
            this.buttonBlockedStateOpen.Text = "Block Relay";
            this.buttonBlockedStateOpen.UseVisualStyleBackColor = true;
            this.buttonBlockedStateOpen.Click += new System.EventHandler(this.buttonBlockedState_Click);
            // 
            // buttonRequestRelayParamaters
            // 
            this.buttonRequestRelayParamaters.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRequestRelayParamaters.Location = new System.Drawing.Point(262, 116);
            this.buttonRequestRelayParamaters.Name = "buttonRequestRelayParamaters";
            this.buttonRequestRelayParamaters.Size = new System.Drawing.Size(141, 62);
            this.buttonRequestRelayParamaters.TabIndex = 33;
            this.buttonRequestRelayParamaters.Text = "Request Relay Params";
            this.buttonRequestRelayParamaters.UseVisualStyleBackColor = true;
            this.buttonRequestRelayParamaters.Click += new System.EventHandler(this.buttonRequestRelayParamaters_Click);
            // 
            // labelRelaySNControl
            // 
            this.labelRelaySNControl.AutoSize = true;
            this.labelRelaySNControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRelaySNControl.Location = new System.Drawing.Point(120, 8);
            this.labelRelaySNControl.Name = "labelRelaySNControl";
            this.labelRelaySNControl.Size = new System.Drawing.Size(60, 13);
            this.labelRelaySNControl.TabIndex = 74;
            this.labelRelaySNControl.Text = "Relay S/N:";
            // 
            // buttonUnblockOpen
            // 
            this.buttonUnblockOpen.Location = new System.Drawing.Point(411, 33);
            this.buttonUnblockOpen.Name = "buttonUnblockOpen";
            this.buttonUnblockOpen.Size = new System.Drawing.Size(111, 23);
            this.buttonUnblockOpen.TabIndex = 52;
            this.buttonUnblockOpen.Text = "Unblock Relay";
            this.buttonUnblockOpen.UseVisualStyleBackColor = true;
            this.buttonUnblockOpen.Click += new System.EventHandler(this.buttonUnblockOpen_Click);
            // 
            // textBoxRelaySNControl
            // 
            this.textBoxRelaySNControl.Location = new System.Drawing.Point(186, 5);
            this.textBoxRelaySNControl.MaxLength = 5;
            this.textBoxRelaySNControl.Name = "textBoxRelaySNControl";
            this.textBoxRelaySNControl.ReadOnly = true;
            this.textBoxRelaySNControl.Size = new System.Drawing.Size(87, 20);
            this.textBoxRelaySNControl.TabIndex = 73;
            this.textBoxRelaySNControl.Tag = "SN";
            // 
            // labelRevision
            // 
            this.labelRevision.AutoSize = true;
            this.labelRevision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelRevision.Location = new System.Drawing.Point(3, 86);
            this.labelRevision.Margin = new System.Windows.Forms.Padding(3);
            this.labelRevision.Name = "labelRevision";
            this.labelRevision.Padding = new System.Windows.Forms.Padding(1);
            this.labelRevision.Size = new System.Drawing.Size(4, 17);
            this.labelRevision.TabIndex = 54;
            this.labelRevision.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFPGARevision
            // 
            this.labelFPGARevision.AutoSize = true;
            this.labelFPGARevision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelFPGARevision.Location = new System.Drawing.Point(3, 145);
            this.labelFPGARevision.Margin = new System.Windows.Forms.Padding(3);
            this.labelFPGARevision.Name = "labelFPGARevision";
            this.labelFPGARevision.Padding = new System.Windows.Forms.Padding(1);
            this.labelFPGARevision.Size = new System.Drawing.Size(4, 17);
            this.labelFPGARevision.TabIndex = 72;
            this.labelFPGARevision.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRelayRevision
            // 
            this.labelRelayRevision.AutoSize = true;
            this.labelRelayRevision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelRelayRevision.Location = new System.Drawing.Point(3, 116);
            this.labelRelayRevision.Margin = new System.Windows.Forms.Padding(3);
            this.labelRelayRevision.Name = "labelRelayRevision";
            this.labelRelayRevision.Padding = new System.Windows.Forms.Padding(1);
            this.labelRelayRevision.Size = new System.Drawing.Size(4, 17);
            this.labelRelayRevision.TabIndex = 55;
            this.labelRelayRevision.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonResetBothProc
            // 
            this.buttonResetBothProc.Location = new System.Drawing.Point(262, 86);
            this.buttonResetBothProc.Name = "buttonResetBothProc";
            this.buttonResetBothProc.Size = new System.Drawing.Size(121, 23);
            this.buttonResetBothProc.TabIndex = 71;
            this.buttonResetBothProc.Text = "Reset Relay";
            this.buttonResetBothProc.UseVisualStyleBackColor = true;
            this.buttonResetBothProc.Click += new System.EventHandler(this.buttonResetBothProc_Click);
            // 
            // labelRelayStateControlPage
            // 
            this.labelRelayStateControlPage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelRelayStateControlPage.Location = new System.Drawing.Point(278, 3);
            this.labelRelayStateControlPage.Margin = new System.Windows.Forms.Padding(3);
            this.labelRelayStateControlPage.Name = "labelRelayStateControlPage";
            this.labelRelayStateControlPage.Padding = new System.Windows.Forms.Padding(1);
            this.labelRelayStateControlPage.Size = new System.Drawing.Size(100, 26);
            this.labelRelayStateControlPage.TabIndex = 63;
            this.labelRelayStateControlPage.Text = "Unkown";
            this.labelRelayStateControlPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSendAll
            // 
            this.buttonSendAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSendAll.Location = new System.Drawing.Point(415, 116);
            this.buttonSendAll.Name = "buttonSendAll";
            this.buttonSendAll.Size = new System.Drawing.Size(107, 62);
            this.buttonSendAll.TabIndex = 70;
            this.buttonSendAll.Text = "Send All";
            this.buttonSendAll.UseVisualStyleBackColor = true;
            this.buttonSendAll.Click += new System.EventHandler(this.buttonSendAll_Click);
            // 
            // labelBlockedOpenState
            // 
            this.labelBlockedOpenState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelBlockedOpenState.Location = new System.Drawing.Point(423, 62);
            this.labelBlockedOpenState.Margin = new System.Windows.Forms.Padding(3);
            this.labelBlockedOpenState.Name = "labelBlockedOpenState";
            this.labelBlockedOpenState.Padding = new System.Windows.Forms.Padding(1);
            this.labelBlockedOpenState.Size = new System.Drawing.Size(100, 26);
            this.labelBlockedOpenState.TabIndex = 64;
            this.labelBlockedOpenState.Text = "Unkown";
            this.labelBlockedOpenState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxSaveStateName
            // 
            this.textBoxSaveStateName.Location = new System.Drawing.Point(3, 60);
            this.textBoxSaveStateName.Name = "textBoxSaveStateName";
            this.textBoxSaveStateName.Size = new System.Drawing.Size(121, 20);
            this.textBoxSaveStateName.TabIndex = 69;
            // 
            // buttonTripRelay
            // 
            this.buttonTripRelay.Location = new System.Drawing.Point(262, 34);
            this.buttonTripRelay.Name = "buttonTripRelay";
            this.buttonTripRelay.Size = new System.Drawing.Size(121, 23);
            this.buttonTripRelay.TabIndex = 65;
            this.buttonTripRelay.Text = "Trip Relay";
            this.buttonTripRelay.UseVisualStyleBackColor = true;
            this.buttonTripRelay.Click += new System.EventHandler(this.buttonTripRelay_Click);
            // 
            // labelRelayDisconnected
            // 
            this.labelRelayDisconnected.AutoSize = true;
            this.labelRelayDisconnected.BackColor = System.Drawing.Color.Red;
            this.labelRelayDisconnected.Location = new System.Drawing.Point(419, 95);
            this.labelRelayDisconnected.Name = "labelRelayDisconnected";
            this.labelRelayDisconnected.Size = new System.Drawing.Size(103, 13);
            this.labelRelayDisconnected.TabIndex = 49;
            this.labelRelayDisconnected.Text = "Relay Disconnected";
            this.labelRelayDisconnected.Visible = false;
            // 
            // comboBoxSavedStates
            // 
            this.comboBoxSavedStates.FormattingEnabled = true;
            this.comboBoxSavedStates.Location = new System.Drawing.Point(3, 32);
            this.comboBoxSavedStates.Name = "comboBoxSavedStates";
            this.comboBoxSavedStates.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSavedStates.TabIndex = 66;
            this.comboBoxSavedStates.DropDown += new System.EventHandler(this.comboBoxSavedStates_DropDown);
            this.comboBoxSavedStates.SelectedIndexChanged += new System.EventHandler(this.comboBoxSavedStates_SelectedIndexChanged);
            this.comboBoxSavedStates.DropDownClosed += new System.EventHandler(this.comboBoxSavedStates_DropDownClosed);
            // 
            // buttonSaveSetting
            // 
            this.buttonSaveSetting.Location = new System.Drawing.Point(130, 58);
            this.buttonSaveSetting.Name = "buttonSaveSetting";
            this.buttonSaveSetting.Size = new System.Drawing.Size(94, 23);
            this.buttonSaveSetting.TabIndex = 68;
            this.buttonSaveSetting.Text = "Save Settings";
            this.buttonSaveSetting.UseVisualStyleBackColor = true;
            this.buttonSaveSetting.Click += new System.EventHandler(this.buttonSaveSetting_Click);
            // 
            // buttonDeleteSetting
            // 
            this.buttonDeleteSetting.Location = new System.Drawing.Point(130, 31);
            this.buttonDeleteSetting.Name = "buttonDeleteSetting";
            this.buttonDeleteSetting.Size = new System.Drawing.Size(94, 23);
            this.buttonDeleteSetting.TabIndex = 67;
            this.buttonDeleteSetting.Text = "Delete Setting";
            this.buttonDeleteSetting.UseVisualStyleBackColor = true;
            this.buttonDeleteSetting.Click += new System.EventHandler(this.buttonDeleteSetting_Click);
            // 
            // checkBox277Protector
            // 
            this.checkBox277Protector.AutoSize = true;
            this.checkBox277Protector.Location = new System.Drawing.Point(12, 480);
            this.checkBox277Protector.Name = "checkBox277Protector";
            this.checkBox277Protector.Size = new System.Drawing.Size(100, 17);
            this.checkBox277Protector.TabIndex = 75;
            this.checkBox277Protector.Text = "277 V Protector";
            this.checkBox277Protector.UseVisualStyleBackColor = true;
            this.checkBox277Protector.CheckedChanged += new System.EventHandler(this.checkBox277Protector_CheckedChanged);
            // 
            // ucTripMode2
            // 
            this.ucTripMode2.AutoSize = true;
            this.ucTripMode2.CTRatio = 320;
            this.ucTripMode2.Customer = RelayControlLibrary.Customers.None;
            this.ucTripMode2.Location = new System.Drawing.Point(8, 3);
            this.ucTripMode2.Name = "ucTripMode2";
            this.ucTripMode2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ucTripMode2.SequenceRelay = false;
            this.ucTripMode2.Size = new System.Drawing.Size(313, 262);
            this.ucTripMode2.TabIndex = 40;
            this.ucTripMode2.VersionNumber = 0;
            // 
            // ucCloseMode1
            // 
            this.ucCloseMode1.Customer = RelayControlLibrary.Customers.None;
            this.ucCloseMode1.Location = new System.Drawing.Point(318, 4);
            this.ucCloseMode1.Mode = RelayControlLibrary.CloseModes.None;
            this.ucCloseMode1.Name = "ucCloseMode1";
            this.ucCloseMode1.RelaxClose = false;
            this.ucCloseMode1.RelayRevisionNumber = ((uint)(0u));
            this.ucCloseMode1.Size = new System.Drawing.Size(286, 247);
            this.ucCloseMode1.TabIndex = 26;
            this.ucCloseMode1.Voltage277State = false;
            // 
            // ucPumpMode1
            // 
            this.ucPumpMode1.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucPumpMode1.Location = new System.Drawing.Point(187, 260);
            this.ucPumpMode1.Name = "ucPumpMode1";
            this.ucPumpMode1.PumpProtectEnabled = false;
            this.ucPumpMode1.PumpReason = RelayControlLibrary.PumpReasons.NoPump;
            this.ucPumpMode1.RelayRevisionNumber = ((uint)(0u));
            this.ucPumpMode1.Size = new System.Drawing.Size(254, 270);
            this.ucPumpMode1.TabIndex = 49;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageControl);
            this.tabControlMain.Controls.Add(this.tabPageMonitor);
            this.tabControlMain.Controls.Add(this.tabPageFlightRecorder);
            this.tabControlMain.Controls.Add(this.tabPageEvents);
            this.tabControlMain.Controls.Add(this.tabPageEngineering);
            this.tabControlMain.Controls.Add(this.tabPageTransmitter);
            this.tabControlMain.Controls.Add(this.tabPageTransmitterMonitoring);
            this.tabControlMain.Controls.Add(this.tabPageDNP);
            this.tabControlMain.Controls.Add(this.tabPageArcFault);
            this.tabControlMain.Controls.Add(this.tabPageShortRange);
            this.tabControlMain.Controls.Add(this.tabPageDNPData);
            this.tabControlMain.Controls.Add(this.tabPageDNPSecureAuth);
            this.tabControlMain.Location = new System.Drawing.Point(0, 27);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(999, 650);
            this.tabControlMain.TabIndex = 36;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            // 
            // tabPageDNP
            // 
            this.tabPageDNP.Controls.Add(this.buttonResetRelay2);
            this.tabPageDNP.Controls.Add(this.ucDNP1);
            this.tabPageDNP.Location = new System.Drawing.Point(4, 22);
            this.tabPageDNP.Name = "tabPageDNP";
            this.tabPageDNP.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDNP.Size = new System.Drawing.Size(991, 624);
            this.tabPageDNP.TabIndex = 9;
            this.tabPageDNP.Text = "DNP";
            this.tabPageDNP.UseVisualStyleBackColor = true;
            // 
            // buttonResetRelay2
            // 
            this.buttonResetRelay2.Location = new System.Drawing.Point(166, 415);
            this.buttonResetRelay2.Name = "buttonResetRelay2";
            this.buttonResetRelay2.Size = new System.Drawing.Size(75, 23);
            this.buttonResetRelay2.TabIndex = 1;
            this.buttonResetRelay2.Text = "Reset Relay";
            this.buttonResetRelay2.UseVisualStyleBackColor = true;
            this.buttonResetRelay2.Click += new System.EventHandler(this.buttonResetBothProc_Click);
            // 
            // ucDNP1
            // 
            this.ucDNP1.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucDNP1.Location = new System.Drawing.Point(8, 3);
            this.ucDNP1.Name = "ucDNP1";
            this.ucDNP1.Size = new System.Drawing.Size(983, 558);
            this.ucDNP1.TabIndex = 0;
            // 
            // tabPageArcFault
            // 
            this.tabPageArcFault.Controls.Add(this.buttonArcFaultStartMonitoring);
            this.tabPageArcFault.Controls.Add(this.ucArcFault1);
            this.tabPageArcFault.Location = new System.Drawing.Point(4, 22);
            this.tabPageArcFault.Name = "tabPageArcFault";
            this.tabPageArcFault.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageArcFault.Size = new System.Drawing.Size(991, 624);
            this.tabPageArcFault.TabIndex = 10;
            this.tabPageArcFault.Text = "Arc Fault";
            this.tabPageArcFault.UseVisualStyleBackColor = true;
            // 
            // buttonArcFaultStartMonitoring
            // 
            this.buttonArcFaultStartMonitoring.Location = new System.Drawing.Point(359, 394);
            this.buttonArcFaultStartMonitoring.Name = "buttonArcFaultStartMonitoring";
            this.buttonArcFaultStartMonitoring.Size = new System.Drawing.Size(118, 23);
            this.buttonArcFaultStartMonitoring.TabIndex = 4;
            this.buttonArcFaultStartMonitoring.Text = "Start Monitoring";
            this.buttonArcFaultStartMonitoring.UseVisualStyleBackColor = true;
            this.buttonArcFaultStartMonitoring.Click += new System.EventHandler(this.buttonArcFaultStartMonitoring_Click);
            // 
            // ucArcFault1
            // 
            this.ucArcFault1.Location = new System.Drawing.Point(8, 6);
            this.ucArcFault1.Name = "ucArcFault1";
            this.ucArcFault1.Size = new System.Drawing.Size(478, 429);
            this.ucArcFault1.TabIndex = 3;
            // 
            // tabPageShortRange
            // 
            this.tabPageShortRange.Controls.Add(this.ucShortRange1);
            this.tabPageShortRange.Location = new System.Drawing.Point(4, 22);
            this.tabPageShortRange.Name = "tabPageShortRange";
            this.tabPageShortRange.Size = new System.Drawing.Size(991, 624);
            this.tabPageShortRange.TabIndex = 11;
            this.tabPageShortRange.Text = "Sec Mon";
            this.tabPageShortRange.UseVisualStyleBackColor = true;
            // 
            // ucShortRange1
            // 
            this.ucShortRange1.Location = new System.Drawing.Point(8, 0);
            this.ucShortRange1.Name = "ucShortRange1";
            this.ucShortRange1.Size = new System.Drawing.Size(987, 624);
            this.ucShortRange1.TabIndex = 0;
            // 
            // tabPageDNPData
            // 
            this.tabPageDNPData.Controls.Add(this.buttonRequestDNPData);
            this.tabPageDNPData.Location = new System.Drawing.Point(4, 22);
            this.tabPageDNPData.Name = "tabPageDNPData";
            this.tabPageDNPData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDNPData.Size = new System.Drawing.Size(991, 624);
            this.tabPageDNPData.TabIndex = 12;
            this.tabPageDNPData.Text = "DNP Data";
            this.tabPageDNPData.UseVisualStyleBackColor = true;
            // 
            // buttonRequestDNPData
            // 
            this.buttonRequestDNPData.BackColor = System.Drawing.Color.Red;
            this.buttonRequestDNPData.Location = new System.Drawing.Point(859, 598);
            this.buttonRequestDNPData.Name = "buttonRequestDNPData";
            this.buttonRequestDNPData.Size = new System.Drawing.Size(124, 23);
            this.buttonRequestDNPData.TabIndex = 1;
            this.buttonRequestDNPData.Text = "Request DNP Data";
            this.buttonRequestDNPData.UseVisualStyleBackColor = false;
            this.buttonRequestDNPData.Click += new System.EventHandler(this.buttonRequestDNPData_Click);
            // 
            // tabPageDNPSecureAuth
            // 
            this.tabPageDNPSecureAuth.Controls.Add(this.ucDNPSAv51);
            this.tabPageDNPSecureAuth.Location = new System.Drawing.Point(4, 22);
            this.tabPageDNPSecureAuth.Name = "tabPageDNPSecureAuth";
            this.tabPageDNPSecureAuth.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDNPSecureAuth.Size = new System.Drawing.Size(991, 624);
            this.tabPageDNPSecureAuth.TabIndex = 13;
            this.tabPageDNPSecureAuth.Text = "DNP SAv5";
            this.tabPageDNPSecureAuth.UseVisualStyleBackColor = true;
            // 
            // ucDNPSAv51
            // 
            this.ucDNPSAv51.Location = new System.Drawing.Point(13, 6);
            this.ucDNPSAv51.Name = "ucDNPSAv51";
            this.ucDNPSAv51.SerialNumber = 0;
            this.ucDNPSAv51.Size = new System.Drawing.Size(978, 733);
            this.ucDNPSAv51.TabIndex = 0;
            // 
            // timerResponseTimeOut
            // 
            this.timerResponseTimeOut.Interval = 1000;
            this.timerResponseTimeOut.Tick += new System.EventHandler(this.timerResponseTimeOut_Tick);
            // 
            // timerScreenCapDelay
            // 
            this.timerScreenCapDelay.Tick += new System.EventHandler(this.timerScreenCapDelay_Tick);
            // 
            // timerFindRelayTimeout
            // 
            this.timerFindRelayTimeout.Interval = 500;
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 19200;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(999, 699);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainControl";
            this.Text = "Digital Grid Inc - Relay Control and Monitoring - BETA - 2009-07-24";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainControl_FormClosed);
            this.Load += new System.EventHandler(this.MainControl_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.tabPageTransmitterMonitoring.ResumeLayout(false);
            this.tabPageTransmitterMonitoring.PerformLayout();
            this.tabPageEngineering.ResumeLayout(false);
            this.groupBoxTimeConvert.ResumeLayout(false);
            this.groupBoxTimeConvert.PerformLayout();
            this.tabPageEvents.ResumeLayout(false);
            this.panelEventSelect.ResumeLayout(false);
            this.panelEventSelect.PerformLayout();
            this.tabPageFlightRecorder.ResumeLayout(false);
            this.tabPageFlightRecorder.PerformLayout();
            this.tabPageTransmitter.ResumeLayout(false);
            this.tabPageTransmitter.PerformLayout();
            this.tabPageMonitor.ResumeLayout(false);
            this.tabPageMonitor.PerformLayout();
            this.tabPageControl.ResumeLayout(false);
            this.tabPageControl.PerformLayout();
            this.groupBoxLRLockoutMain.ResumeLayout(false);
            this.groupBoxLRLockoutMain.PerformLayout();
            this.groupBoxLowVoltThres.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLowVoltageThres)).EndInit();
            this.groupBoxRelayStatus.ResumeLayout(false);
            this.groupBoxRelayStatus.PerformLayout();
            this.groupBoxPhasingAndType.ResumeLayout(false);
            this.groupBoxPhasingAndType.PerformLayout();
            this.groupBoxNetworkCTRatio.ResumeLayout(false);
            this.groupBoxNetworkCTRatio.PerformLayout();
            this.groupBoxRelayFlags.ResumeLayout(false);
            this.groupBoxRelayFlags.PerformLayout();
            this.panelOtherRelayControls.ResumeLayout(false);
            this.panelOtherRelayControls.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageDNP.ResumeLayout(false);
            this.tabPageArcFault.ResumeLayout(false);
            this.tabPageShortRange.ResumeLayout(false);
            this.tabPageDNPData.ResumeLayout(false);
            this.tabPageDNPSecureAuth.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private System.IO.Ports.SerialPort serialPort1;
        //private RelayControlLibrary.ucTripMode ucTripMode2;
        //private SineDisplayGraph.ucPhasorGraph ucPhasorGraph1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem OptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cOMPortToolStripMenuItem;
        private System.Windows.Forms.Timer timerSCITimeOut;
        private System.Windows.Forms.Timer timerCheckPortTime;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMain;
        private System.Windows.Forms.ToolStripMenuItem findRelayToolStripMenuItem;
        private System.Windows.Forms.Timer timerTimeOutCountdown;
        private System.Windows.Forms.Timer timerRegisterPolling;
        private System.Windows.Forms.ToolStripMenuItem enableAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAction;
        private System.Windows.Forms.ToolStripMenuItem acknowledgeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem eventActionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveEventsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadEventSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem liveDataActionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem requestLiveDataToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveLiveDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadLiveDataToolStripMenuItem;
        private MyPort serialPort1;
        private System.Windows.Forms.ToolStripMenuItem downloadEventFromRelayToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageTransmitterMonitoring;
        private System.Windows.Forms.TabPage tabPageEngineering;
        private System.Windows.Forms.Button buttonFPGAProcVersion;
        private System.Windows.Forms.Button buttonCauseEvent;
        private System.Windows.Forms.Button buttonSendTime;
        private RelayControlLibrary.ucManualCalibration ucCalibration1;
        private System.Windows.Forms.Button buttonUpdateDisplay;
        private System.Windows.Forms.Button buttonRSTRelay;
        private System.Windows.Forms.Button buttonResetMaster;
        private System.Windows.Forms.Button buttonRQRelayProcVersion;
        private System.Windows.Forms.Button buttonForceI;
        private System.Windows.Forms.Button buttonRequestRelayRegisters;
        private System.Windows.Forms.TabPage tabPageEvents;
        private System.Windows.Forms.Button buttonRQEventData;
        private System.Windows.Forms.Panel panelEventSelect;
        private System.Windows.Forms.RadioButton radioButtonEvent7;
        private System.Windows.Forms.RadioButton radioButtonEvent6;
        private System.Windows.Forms.RadioButton radioButtonEvent5;
        private System.Windows.Forms.RadioButton radioButtonEvent4;
        private System.Windows.Forms.RadioButton radioButtonEvent3;
        private System.Windows.Forms.RadioButton radioButtonEvent2;
        private System.Windows.Forms.RadioButton radioButtonEvent1;
        private System.Windows.Forms.RadioButton radioButtonEvent0;
        private SineDisplayGraph.ucEventGraph ucEventGraph7;
        private SineDisplayGraph.ucEventGraph ucEventGraph6;
        private SineDisplayGraph.ucEventGraph ucEventGraph5;
        private SineDisplayGraph.ucEventGraph ucEventGraph4;
        private SineDisplayGraph.ucEventGraph ucEventGraph3;
        private SineDisplayGraph.ucEventGraph ucEventGraph2;
        private SineDisplayGraph.ucEventGraph ucEventGraph1;
        private SineDisplayGraph.ucEventGraph ucEventGraph0;
        private System.Windows.Forms.TabPage tabPageFlightRecorder;
        private System.Windows.Forms.Label labelLiveDataTriggerTime;
        private SineDisplayGraph.ucLiveData ucLiveData1;
        private System.Windows.Forms.TabPage tabPageTransmitter;
        private RelayControlLibrary.ucTransmitter ucTransmitter1;
        private System.Windows.Forms.TabPage tabPageMonitor;
        private System.Windows.Forms.CheckBox checkBoxInTripRegion;
        private System.Windows.Forms.TextBox textBoxTemperatureMonitoringPage;
        private System.Windows.Forms.Label labelTemperatureMonitoringPage;
        private System.Windows.Forms.Button buttonUpdateCTRatio;
        private System.Windows.Forms.Label labelRelayTrippedOrClose;
        private System.Windows.Forms.Button buttonToggleMonitor;
        private System.Windows.Forms.Label labelCtRatioMonitor;
        private SineDisplayGraph.ucPhasorGraph ucPhasorGraph1;
        private System.Windows.Forms.TabPage tabPageControl;
        private System.Windows.Forms.Label labelFPGARevision;
        private System.Windows.Forms.Button buttonResetBothProc;
        private System.Windows.Forms.Button buttonSendAll;
        private System.Windows.Forms.TextBox textBoxSaveStateName;
        private System.Windows.Forms.Label labelRelayDisconnected;
        private System.Windows.Forms.Button buttonSaveSetting;
        private System.Windows.Forms.Button buttonDeleteSetting;
        private System.Windows.Forms.ComboBox comboBoxSavedStates;
        private System.Windows.Forms.Button buttonTripRelay;
        private System.Windows.Forms.Label labelBlockedOpenState;
        private System.Windows.Forms.Label labelRelayStateControlPage;
        private System.Windows.Forms.Button buttonClearCycleCount;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox textBoxCTRatio;
        private System.Windows.Forms.DomainUpDown domainUpDownCTRatioM;
        private System.Windows.Forms.Button buttonSendCTRatio;
        private System.Windows.Forms.Label labelRelayRevision;
        private System.Windows.Forms.Label labelRevision;
        private System.Windows.Forms.Button buttonUnblockOpen;
        private System.Windows.Forms.Button buttonTypePhasingRestoreDefaults;
        private System.Windows.Forms.Button buttonRelayType;
        private System.Windows.Forms.DomainUpDown domainUpDownRelayType;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.DomainUpDown domainUpDownPhasings;
        private RelayControlLibrary.ucPumpMode ucPumpMode1;
        private RelayControlLibrary.ucTripMode ucTripMode2;
        private System.Windows.Forms.CheckBox checkBoxPhasingOkayFlag;
        private System.Windows.Forms.TextBox textBoxTemperature;
        private System.Windows.Forms.CheckBox checkBoxBlockedOpenFlag;
        private System.Windows.Forms.CheckBox checkBoxBFlag;
        private System.Windows.Forms.CheckBox checkBoxFloatFlag;
        private System.Windows.Forms.CheckBox checkBoxTrippingFlag;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxTripCount;
        private System.Windows.Forms.CheckBox checkBoxTripFlag;
        private System.Windows.Forms.CheckBox checkBoxDefaultsUsed;
        private System.Windows.Forms.CheckBox checkBoxPumping;
        private System.Windows.Forms.CheckBox checkBoxCalibrating;
        private System.Windows.Forms.CheckBox checkBoxBlockedCloseFlag;
        private System.Windows.Forms.CheckBox checkBoxInInsensRegion;
        private System.Windows.Forms.CheckBox checkBoxFlag2;
        private System.Windows.Forms.CheckBox checkBoxFlag1;
        private System.Windows.Forms.CheckBox checkBoxPowerSaveFlag;
        private System.Windows.Forms.CheckBox checkBoxACB;
        private System.Windows.Forms.CheckBox checkBoxSequence;
        private System.Windows.Forms.CheckBox checkBoxMonitorPhasors;
        private System.Windows.Forms.CheckBox checkBoxMathError;
        private System.Windows.Forms.CheckBox checkBoxOffsetOkay;
        private System.Windows.Forms.CheckBox checkBoxMathOverTime;
        private System.Windows.Forms.Button buttonRequestRelayParamaters;
        private System.Windows.Forms.Button buttonBlockedStateOpen;
        private RelayControlLibrary.ucCloseMode ucCloseMode1;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.Timer timerResponseTimeOut;
        private System.Windows.Forms.ToolStripMenuItem sToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printScreenToolStripMenuItem;
        private System.Windows.Forms.Timer timerScreenCapDelay;
        private System.Windows.Forms.Label labelOver5;
        private System.Windows.Forms.Label labelQuietMode;
        private System.Windows.Forms.Label labelRelaySNControl;
        private System.Windows.Forms.TextBox textBoxRelaySNControl;
        private System.Windows.Forms.Timer timerFindRelayTimeout;
        private System.Windows.Forms.CheckBox checkBox277Protector;
        private System.Windows.Forms.TabPage tabPageDNP;
        private RelayControlLibrary.ucDNP ucDNP1;
        private System.Windows.Forms.Button buttonResetRelay2;
        private System.Windows.Forms.Button buttonToggleQuietMode;
        private System.Windows.Forms.Button buttonBlockAndTrip;
        private System.Windows.Forms.Button buttonClearEvents;
        private System.Windows.Forms.ToolStripMenuItem clearEventsToolStripMenuItem;
        private System.Windows.Forms.Label labelGEWH;
        private System.Windows.Forms.Label labelProtectorType;
        private System.Windows.Forms.Label labelRelayDisconnected3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelRelayDisconnected;
        private System.Windows.Forms.TabPage tabPageArcFault;
        private System.Windows.Forms.TabPage tabPageShortRange;
        private RelayControlLibrary.ucShortRange ucShortRange1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cTRatioCalculatorToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxTimeConvert;
        private System.Windows.Forms.TextBox textBoxTimeOutput;
        private System.Windows.Forms.TextBox textBoxTimeInput;
        private System.Windows.Forms.Button buttonTimeConvert;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelRelayDisconnected2;
        private RelayControlLibrary.ucTransmitterMonitoring ucTransmitterMonitoring1;
        private System.Windows.Forms.Panel panelOtherRelayControls;
        private System.Windows.Forms.Label labelConEdPowerRelay;
        private System.Windows.Forms.Label labelNWPStatus;
        private System.Windows.Forms.TabPage tabPageDNPData;
        private System.Windows.Forms.Button buttonRequestDNPData;
        private ucForceCustomerSwitch ucForceCustomerSwitch1;
        private RelayControlLibrary.uc8CheckBoxFlags uc8CheckBoxFlagsCommFlags2;
        private RelayControlLibrary.uc8CheckBoxFlags uc8CheckBoxFlagsCommFlags1;
        private RelayControlLibrary.uc8CheckBoxFlags uc8CheckBoxFlagsRelayStatus2;
        private RelayControlLibrary.uc8CheckBoxFlags uc8CheckBoxFlagsRelayStatus1;
        private RelayControlLibrary.uc8CheckBoxFlags uc8CheckBoxFlagsRelayFlags2;
        private RelayControlLibrary.uc8CheckBoxFlags uc8CheckBoxFlagsRelayFlags1;
        private RelayControlLibrary.ucCalibration ucCalibration2;
        private RelayControlLibrary.ucCSVConverterCSVFile ucCSVConverterCSVFile1;
        private RelayControlLibrary.ucSafeService ucSafeService1;
        private System.Windows.Forms.GroupBox groupBoxRelayFlags;
        private System.Windows.Forms.GroupBox groupBoxNetworkCTRatio;
        private System.Windows.Forms.GroupBox groupBoxPhasingAndType;
        private System.Windows.Forms.GroupBox groupBoxRelayStatus;
        private RelayControlLibrary.uc8CheckBoxFlags uc8CheckBoxFlagsGEControl2;
        private RelayControlLibrary.uc8CheckBoxFlags uc8CheckBoxFlagsGEControl1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelReceiverStatus;
        private System.Windows.Forms.Button buttonArcFaultStartMonitoring;
        private RelayControlLibrary.ucArcFault ucArcFault1;
        private RelayControlLibrary.ucRelayProgramming ucRelayProgramming1;
        private System.Windows.Forms.ToolStripMenuItem reprogramRelayFileSelectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableAutoloadToolStripMenuItem;
        private RelayControlLibrary.ucGeneralCommandHandler ucGeneralCommandHandler1;
        private System.Windows.Forms.TabPage tabPageDNPSecureAuth;
        private RelayDNPSecurity.ucDNPSAv5 ucDNPSAv51;
        private System.Windows.Forms.Label labelSNPQMonitor;
        private System.Windows.Forms.TextBox textBoxRelaySNControlPQ;
        private System.Windows.Forms.TextBox textBoxCTRatioPQMonitor;
        private System.Windows.Forms.CheckBox checkBox277ProtectorPQ;
        private System.Windows.Forms.NumericUpDown numericUpDownLowVoltageThres;
        private System.Windows.Forms.Button buttonSendLowVoltageThres;
        private System.Windows.Forms.Button buttonRequestLowVotlageThres;
        private System.Windows.Forms.GroupBox groupBoxLowVoltThres;
        private System.Windows.Forms.GroupBox groupBoxLRLockoutMain;
        private System.Windows.Forms.TextBox textBoxLRLockoutStatusMain;
        private System.Windows.Forms.Label labelLRLockoutMain;
    }
}

