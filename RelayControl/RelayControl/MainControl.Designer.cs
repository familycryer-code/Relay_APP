namespace RelayControl
{
    partial class MainControl
    {
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
       // private void InitializeComponent()
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label labelTemperature;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainControl));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findRelayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reprogramRelayFileSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tCPConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.ucCSVConverterCSVFile1 = new RelayControlLibrary.ucCSVConverterCSVFile();
            this.ucTimeControl1 = new RelayControlLibrary.ucTimeControl();
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
            this.ucCalibration2 = new RelayControlLibrary.ucCalibration();
            this.uc8CheckBoxFlagsCommFlags2 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.uc8CheckBoxFlagsCommFlags1 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.uc8CheckBoxFlagsRelayStatus2 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.uc8CheckBoxFlagsRelayStatus1 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.uc8CheckBoxFlagsRelayFlags2 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.uc8CheckBoxFlagsRelayFlags1 = new RelayControlLibrary.uc8CheckBoxFlags();
            this.ucCalibration1 = new RelayControlLibrary.ucManualCalibration();
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
            this.buttonReqLiveData = new System.Windows.Forms.Button();
            this.tabPageFlightRecorder = new System.Windows.Forms.TabPage();
            this.labelLiveDataTriggerTime = new System.Windows.Forms.Label();
            this.ucLiveData1 = new SineDisplayGraph.ucLiveData();
            this.tabPageTransmitter = new System.Windows.Forms.TabPage();
            this.labelRelayDisconnected3 = new System.Windows.Forms.Label();
            this.ucTransmitter1 = new RelayControlLibrary.ucTransmitter();
            this.tabPageMonitor = new System.Windows.Forms.TabPage();
            this.labelSNPQMonitor = new System.Windows.Forms.Label();
            this.textBoxRelaySNControlPQ = new System.Windows.Forms.TextBox();
            this.textBoxCTRatioPQMonitor = new System.Windows.Forms.TextBox();
            this.checkBoxInTripRegion = new System.Windows.Forms.CheckBox();
            this.buttonUpdateCTRatio = new System.Windows.Forms.Button();
            this.labelRelayTrippedOrClose = new System.Windows.Forms.Label();
            this.buttonToggleMonitor = new System.Windows.Forms.Button();
            this.labelCtRatioMonitor = new System.Windows.Forms.Label();
            this.ucPhasorGraph1 = new SineDisplayGraph.ucPhasorGraph();
            this.textBoxTemperatureMonitoringPage = new System.Windows.Forms.TextBox();
            this.labelTemperatureMonitoringPage = new System.Windows.Forms.Label();
            this.tabPageControl = new System.Windows.Forms.TabPage();
            this.button_dataStore = new System.Windows.Forms.Button();
            this.pictureBox_SendAll = new System.Windows.Forms.PictureBox();
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
            this.labelDNPVoltage = new System.Windows.Forms.Label();
            this.comboBoxDNPVoltage = new System.Windows.Forms.ComboBox();
            this.checkBox277DNPOutputs = new System.Windows.Forms.CheckBox();
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
            this.ucRemoteCommandBlock1 = new RelayControlLibrary.ucRemoteCommandBlock();
            this.ucBlockControl1 = new RelayControlLibrary.ucBlockControl();
            this.labelBootRevision = new System.Windows.Forms.Label();
            this.buttonClearCycleCount = new System.Windows.Forms.Button();
            this.buttonBlockAndTrip = new System.Windows.Forms.Button();
            this.buttonRequestRelayParamaters = new System.Windows.Forms.Button();
            this.labelRelaySNControl = new System.Windows.Forms.Label();
            this.textBoxRelaySNControl = new System.Windows.Forms.TextBox();
            this.labelRevision = new System.Windows.Forms.Label();
            this.labelFPGARevision = new System.Windows.Forms.Label();
            this.labelRelayRevision = new System.Windows.Forms.Label();
            this.buttonResetBothProc = new System.Windows.Forms.Button();
            this.labelRelayStateControlPage = new System.Windows.Forms.Label();
            this.buttonSendAll = new System.Windows.Forms.Button();
            this.textBoxSaveStateName = new System.Windows.Forms.TextBox();
            this.buttonTripRelay = new System.Windows.Forms.Button();
            this.labelRelayDisconnected = new System.Windows.Forms.Label();
            this.comboBoxSavedStates = new System.Windows.Forms.ComboBox();
            this.buttonSaveSetting = new System.Windows.Forms.Button();
            this.buttonDeleteSetting = new System.Windows.Forms.Button();
            this.ucTripMode2 = new RelayControlLibrary.ucTripMode();
            this.ucCloseMode1 = new RelayControlLibrary.ucCloseMode();
            this.ucPumpMode1 = new RelayControlLibrary.ucPumpMode();
            this.ucCoverFlags1 = new RelayControlLibrary.ucCoverFlags();
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
            this.tabPageEngineering2 = new System.Windows.Forms.TabPage();
            this.commTradeConverter1 = new RelayControlLibrary.CommTradeConverter();
            this.labelKioskReceived = new System.Windows.Forms.Label();
            this.checkBoxSerialCommsDebugging = new System.Windows.Forms.CheckBox();
            this.buttonTest = new System.Windows.Forms.Button();
            this.ucPhasorRequest1 = new RelayControlLibrary.ucPhasorRequest();
            this.timerResponseTimeOut = new System.Windows.Forms.Timer(this.components);
            this.timerScreenCapDelay = new System.Windows.Forms.Timer(this.components);
            this.timerFindRelayTimeout = new System.Windows.Forms.Timer(this.components);
            this.timer_SendAll_GIF = new System.Windows.Forms.Timer(this.components);
            this.ucDNPSAv5OSName2 = new RelayDNPSecurity.ucDNPSAv5OSName();
            this.ucDNPSAv5Settings2 = new RelayDNPSecurity.ucDNPSAv5Settings();
            this.ucForceCustomerSwitch1 = new RelayControl.ucForceCustomerSwitch();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SendAll)).BeginInit();
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
            this.tabPageEngineering2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTemperature
            // 
            labelTemperature.AutoSize = true;
            labelTemperature.Location = new System.Drawing.Point(6, 237);
            labelTemperature.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelTemperature.Name = "labelTemperature";
            labelTemperature.Size = new System.Drawing.Size(61, 15);
            labelTemperature.TabIndex = 50;
            labelTemperature.Text = "Temp (C):";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OptionsToolStripMenuItem,
            this.toolStripMenuItemAction,
            this.acknowledgeToolStripMenuItem1,
            this.sToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.loadConfigurationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1166, 24);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // OptionsToolStripMenuItem
            // 
            this.OptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cOMPortToolStripMenuItem,
            this.findRelayToolStripMenuItem,
            this.enableAllToolStripMenuItem,
            this.reprogramRelayFileSelectToolStripMenuItem,
            this.tCPConnectionToolStripMenuItem});
            this.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
            this.OptionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.OptionsToolStripMenuItem.Text = "Options";
            this.OptionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
            // 
            // cOMPortToolStripMenuItem
            // 
            this.cOMPortToolStripMenuItem.Name = "cOMPortToolStripMenuItem";
            this.cOMPortToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.cOMPortToolStripMenuItem.Text = "COM Port";
            // 
            // findRelayToolStripMenuItem
            // 
            this.findRelayToolStripMenuItem.Name = "findRelayToolStripMenuItem";
            this.findRelayToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.findRelayToolStripMenuItem.Text = "Find Relay";
            this.findRelayToolStripMenuItem.Click += new System.EventHandler(this.findRelayToolStripMenuItem_Click);
            // 
            // enableAllToolStripMenuItem
            // 
            this.enableAllToolStripMenuItem.Name = "enableAllToolStripMenuItem";
            this.enableAllToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.enableAllToolStripMenuItem.Text = "Enable All";
            this.enableAllToolStripMenuItem.Click += new System.EventHandler(this.enableAllToolStripMenuItem_Click);
            // 
            // reprogramRelayFileSelectToolStripMenuItem
            // 
            this.reprogramRelayFileSelectToolStripMenuItem.Name = "reprogramRelayFileSelectToolStripMenuItem";
            this.reprogramRelayFileSelectToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.reprogramRelayFileSelectToolStripMenuItem.Text = "Reprogram Relay";
            this.reprogramRelayFileSelectToolStripMenuItem.Click += new System.EventHandler(this.reprogramRelayFileSelectToolStripMenuItem_Click);
            // 
            // tCPConnectionToolStripMenuItem
            // 
            this.tCPConnectionToolStripMenuItem.Name = "tCPConnectionToolStripMenuItem";
            this.tCPConnectionToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.tCPConnectionToolStripMenuItem.Text = "TCPConnection";
            this.tCPConnectionToolStripMenuItem.Click += new System.EventHandler(this.tCPConnectionToolStripMenuItem_Click);
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
            this.timerSCITimeOut.Interval = 1000;
            this.timerSCITimeOut.Tick += new System.EventHandler(this.timerSCITimeOut_Tick);
            // 
            // timerCheckPortTime
            // 
            this.timerCheckPortTime.Interval = 500;
            this.timerCheckPortTime.Tick += new System.EventHandler(this.timerCheckPortTime_Tick);
            // 
            // statusStripMain
            // 
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMain,
            this.toolStripStatusLabelRelayDisconnected,
            this.toolStripStatusLabelReceiverStatus});
            this.statusStripMain.Location = new System.Drawing.Point(0, 856);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 17, 0);
            this.statusStripMain.Size = new System.Drawing.Size(1166, 22);
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
            this.tabPageTransmitterMonitoring.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageTransmitterMonitoring.Controls.Add(this.ucTransmitterMonitoring1);
            this.tabPageTransmitterMonitoring.Controls.Add(this.labelRelayDisconnected2);
            this.tabPageTransmitterMonitoring.Location = new System.Drawing.Point(4, 24);
            this.tabPageTransmitterMonitoring.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageTransmitterMonitoring.Name = "tabPageTransmitterMonitoring";
            this.tabPageTransmitterMonitoring.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageTransmitterMonitoring.Size = new System.Drawing.Size(1449, 910);
            this.tabPageTransmitterMonitoring.TabIndex = 8;
            this.tabPageTransmitterMonitoring.Text = "Sensor Monitoring";
            // 
            // ucTransmitterMonitoring1
            // 
            this.ucTransmitterMonitoring1.BackColor = System.Drawing.SystemColors.Control;
            this.ucTransmitterMonitoring1.CTMult = "";
            this.ucTransmitterMonitoring1.CTRatio = 320;
            this.ucTransmitterMonitoring1.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucTransmitterMonitoring1.Frequency = RelayControlLibrary.Frequencies.Red;
            this.ucTransmitterMonitoring1.GEEnabled = false;
            this.ucTransmitterMonitoring1.Location = new System.Drawing.Point(4, 0);
            this.ucTransmitterMonitoring1.Margin = new System.Windows.Forms.Padding(4);
            this.ucTransmitterMonitoring1.Name = "ucTransmitterMonitoring1";
            this.ucTransmitterMonitoring1.Size = new System.Drawing.Size(1144, 664);
            this.ucTransmitterMonitoring1.TabIndex = 86;
            this.ucTransmitterMonitoring1.TimeElapsedHours = "";
            this.ucTransmitterMonitoring1.TimeElapsedMinutes = "";
            this.ucTransmitterMonitoring1.TimeElapsedSeconds = "";
            this.ucTransmitterMonitoring1.TransmitterID = "";
            this.ucTransmitterMonitoring1.TransmitterMonitoring = false;
            this.ucTransmitterMonitoring1.TransmitterSN = "";
            this.ucTransmitterMonitoring1.WaterBugActive = false;
            // 
            // labelRelayDisconnected2
            // 
            this.labelRelayDisconnected2.AutoSize = true;
            this.labelRelayDisconnected2.BackColor = System.Drawing.Color.Red;
            this.labelRelayDisconnected2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRelayDisconnected2.Location = new System.Drawing.Point(491, 694);
            this.labelRelayDisconnected2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRelayDisconnected2.Name = "labelRelayDisconnected2";
            this.labelRelayDisconnected2.Size = new System.Drawing.Size(151, 20);
            this.labelRelayDisconnected2.TabIndex = 85;
            this.labelRelayDisconnected2.Text = "Relay Disconnected";
            this.labelRelayDisconnected2.Visible = false;
            // 
            // tabPageEngineering
            // 
            this.tabPageEngineering.Controls.Add(this.ucCSVConverterCSVFile1);
            this.tabPageEngineering.Controls.Add(this.ucTimeControl1);
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
            // ucCSVConverterCSVFile1
            // 
            this.ucCSVConverterCSVFile1.Location = new System.Drawing.Point(457, 267);
            this.ucCSVConverterCSVFile1.Margin = new System.Windows.Forms.Padding(4);
            this.ucCSVConverterCSVFile1.Name = "ucCSVConverterCSVFile1";
            this.ucCSVConverterCSVFile1.Size = new System.Drawing.Size(94, 84);
            this.ucCSVConverterCSVFile1.TabIndex = 109;
            // 
            // ucTimeControl1
            // 
            this.ucTimeControl1.Location = new System.Drawing.Point(695, 6);
            this.ucTimeControl1.Margin = new System.Windows.Forms.Padding(4);
            this.ucTimeControl1.Name = "ucTimeControl1";
            this.ucTimeControl1.Size = new System.Drawing.Size(245, 101);
            this.ucTimeControl1.TabIndex = 119;
            // 
            // ucGeneralCommandHandler1
            // 
            this.ucGeneralCommandHandler1.Location = new System.Drawing.Point(557, 197);
            this.ucGeneralCommandHandler1.Margin = new System.Windows.Forms.Padding(4);
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
            this.groupBoxTimeConvert.Location = new System.Drawing.Point(775, 106);
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
            this.ucRelayProgramming1.ActiveRelay = false;
            this.ucRelayProgramming1.Customer = RelayControlLibrary.Customers.None;
            this.ucRelayProgramming1.DNPRelay = true;
            this.ucRelayProgramming1.ForceRelayUpdate = false;
            this.ucRelayProgramming1.ForceUpdateReason = "Generic";
            this.ucRelayProgramming1.FPGARevisionNumber = ((uint)(0u));
            this.ucRelayProgramming1.GERelay = false;
            this.ucRelayProgramming1.Location = new System.Drawing.Point(525, 413);
            this.ucRelayProgramming1.Margin = new System.Windows.Forms.Padding(4);
            this.ucRelayProgramming1.MasterBootRevisionNumberReceived = ((uint)(0u));
            this.ucRelayProgramming1.MasterRevisionNumber = ((uint)(0u));
            this.ucRelayProgramming1.MasterRevisionString = "";
            this.ucRelayProgramming1.Name = "ucRelayProgramming1";
            this.ucRelayProgramming1.NotPollingPort = false;
            this.ucRelayProgramming1.ProgramBootCodeInProgress = false;
            this.ucRelayProgramming1.ProgramBootCodeStart = false;
            this.ucRelayProgramming1.RelayRevisionNumber = ((uint)(0u));
            this.ucRelayProgramming1.ReprogramBootCodeAuto = false;
            this.ucRelayProgramming1.ReprogrammingInProgress = false;
            this.ucRelayProgramming1.SerialNumber = ((uint)(0u));
            this.ucRelayProgramming1.Size = new System.Drawing.Size(458, 211);
            this.ucRelayProgramming1.State = RelayControlLibrary.RelayProgrammingStates.Idle;
            this.ucRelayProgramming1.TabIndex = 112;
            this.ucRelayProgramming1.TransmitterEnabled = false;
            // 
            // uc8CheckBoxFlagsGEControl2
            // 
            this.uc8CheckBoxFlagsGEControl2.Location = new System.Drawing.Point(360, 431);
            this.uc8CheckBoxFlagsGEControl2.Margin = new System.Windows.Forms.Padding(4);
            this.uc8CheckBoxFlagsGEControl2.Name = "uc8CheckBoxFlagsGEControl2";
            this.uc8CheckBoxFlagsGEControl2.Names = null;
            this.uc8CheckBoxFlagsGEControl2.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsGEControl2.TabIndex = 111;
            // 
            // uc8CheckBoxFlagsGEControl1
            // 
            this.uc8CheckBoxFlagsGEControl1.Location = new System.Drawing.Point(360, 249);
            this.uc8CheckBoxFlagsGEControl1.Margin = new System.Windows.Forms.Padding(4);
            this.uc8CheckBoxFlagsGEControl1.Name = "uc8CheckBoxFlagsGEControl1";
            this.uc8CheckBoxFlagsGEControl1.Names = null;
            this.uc8CheckBoxFlagsGEControl1.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsGEControl1.TabIndex = 110;
            // 
            // ucCalibration2
            // 
            this.ucCalibration2.Customer = RelayControlLibrary.Customers.DIGITALGRIDDNP;
            this.ucCalibration2.Location = new System.Drawing.Point(185, 6);
            this.ucCalibration2.Margin = new System.Windows.Forms.Padding(4);
            this.ucCalibration2.Name = "ucCalibration2";
            this.ucCalibration2.Size = new System.Drawing.Size(283, 225);
            this.ucCalibration2.TabIndex = 108;
            // 
            // uc8CheckBoxFlagsCommFlags2
            // 
            this.uc8CheckBoxFlagsCommFlags2.Location = new System.Drawing.Point(254, 431);
            this.uc8CheckBoxFlagsCommFlags2.Margin = new System.Windows.Forms.Padding(4);
            this.uc8CheckBoxFlagsCommFlags2.Name = "uc8CheckBoxFlagsCommFlags2";
            this.uc8CheckBoxFlagsCommFlags2.Names = null;
            this.uc8CheckBoxFlagsCommFlags2.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsCommFlags2.TabIndex = 106;
            // 
            // uc8CheckBoxFlagsCommFlags1
            // 
            this.uc8CheckBoxFlagsCommFlags1.Location = new System.Drawing.Point(254, 249);
            this.uc8CheckBoxFlagsCommFlags1.Margin = new System.Windows.Forms.Padding(4);
            this.uc8CheckBoxFlagsCommFlags1.Name = "uc8CheckBoxFlagsCommFlags1";
            this.uc8CheckBoxFlagsCommFlags1.Names = null;
            this.uc8CheckBoxFlagsCommFlags1.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsCommFlags1.TabIndex = 105;
            // 
            // uc8CheckBoxFlagsRelayStatus2
            // 
            this.uc8CheckBoxFlagsRelayStatus2.Location = new System.Drawing.Point(143, 431);
            this.uc8CheckBoxFlagsRelayStatus2.Margin = new System.Windows.Forms.Padding(4);
            this.uc8CheckBoxFlagsRelayStatus2.Name = "uc8CheckBoxFlagsRelayStatus2";
            this.uc8CheckBoxFlagsRelayStatus2.Names = null;
            this.uc8CheckBoxFlagsRelayStatus2.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsRelayStatus2.TabIndex = 104;
            // 
            // uc8CheckBoxFlagsRelayStatus1
            // 
            this.uc8CheckBoxFlagsRelayStatus1.Location = new System.Drawing.Point(143, 249);
            this.uc8CheckBoxFlagsRelayStatus1.Margin = new System.Windows.Forms.Padding(4);
            this.uc8CheckBoxFlagsRelayStatus1.Name = "uc8CheckBoxFlagsRelayStatus1";
            this.uc8CheckBoxFlagsRelayStatus1.Names = null;
            this.uc8CheckBoxFlagsRelayStatus1.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsRelayStatus1.TabIndex = 103;
            // 
            // uc8CheckBoxFlagsRelayFlags2
            // 
            this.uc8CheckBoxFlagsRelayFlags2.Location = new System.Drawing.Point(17, 431);
            this.uc8CheckBoxFlagsRelayFlags2.Margin = new System.Windows.Forms.Padding(4);
            this.uc8CheckBoxFlagsRelayFlags2.Name = "uc8CheckBoxFlagsRelayFlags2";
            this.uc8CheckBoxFlagsRelayFlags2.Names = null;
            this.uc8CheckBoxFlagsRelayFlags2.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsRelayFlags2.TabIndex = 102;
            // 
            // uc8CheckBoxFlagsRelayFlags1
            // 
            this.uc8CheckBoxFlagsRelayFlags1.Location = new System.Drawing.Point(17, 249);
            this.uc8CheckBoxFlagsRelayFlags1.Margin = new System.Windows.Forms.Padding(4);
            this.uc8CheckBoxFlagsRelayFlags1.Name = "uc8CheckBoxFlagsRelayFlags1";
            this.uc8CheckBoxFlagsRelayFlags1.Names = null;
            this.uc8CheckBoxFlagsRelayFlags1.Size = new System.Drawing.Size(170, 182);
            this.uc8CheckBoxFlagsRelayFlags1.TabIndex = 101;
            // 
            // ucCalibration1
            // 
            this.ucCalibration1.Location = new System.Drawing.Point(471, 104);
            this.ucCalibration1.Margin = new System.Windows.Forms.Padding(4);
            this.ucCalibration1.Name = "ucCalibration1";
            this.ucCalibration1.Size = new System.Drawing.Size(302, 87);
            this.ucCalibration1.TabIndex = 82;
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
            this.tabPageEvents.Location = new System.Drawing.Point(4, 24);
            this.tabPageEvents.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageEvents.Name = "tabPageEvents";
            this.tabPageEvents.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageEvents.Size = new System.Drawing.Size(1449, 910);
            this.tabPageEvents.TabIndex = 6;
            this.tabPageEvents.Text = "Events";
            this.tabPageEvents.UseVisualStyleBackColor = true;
            // 
            // buttonClearEvents
            // 
            this.buttonClearEvents.Location = new System.Drawing.Point(836, 4);
            this.buttonClearEvents.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClearEvents.Name = "buttonClearEvents";
            this.buttonClearEvents.Size = new System.Drawing.Size(155, 26);
            this.buttonClearEvents.TabIndex = 10;
            this.buttonClearEvents.Text = "Clear Events";
            this.buttonClearEvents.UseVisualStyleBackColor = true;
            this.buttonClearEvents.Click += new System.EventHandler(this.buttonClearEvents_Click);
            // 
            // buttonRQEventData
            // 
            this.buttonRQEventData.Location = new System.Drawing.Point(991, 4);
            this.buttonRQEventData.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRQEventData.Name = "buttonRQEventData";
            this.buttonRQEventData.Size = new System.Drawing.Size(155, 26);
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
            this.panelEventSelect.Location = new System.Drawing.Point(4, 0);
            this.panelEventSelect.Margin = new System.Windows.Forms.Padding(4);
            this.panelEventSelect.Name = "panelEventSelect";
            this.panelEventSelect.Size = new System.Drawing.Size(826, 38);
            this.panelEventSelect.TabIndex = 1;
            // 
            // radioButtonEvent7
            // 
            this.radioButtonEvent7.AutoSize = true;
            this.radioButtonEvent7.Location = new System.Drawing.Point(750, 8);
            this.radioButtonEvent7.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonEvent7.Name = "radioButtonEvent7";
            this.radioButtonEvent7.Size = new System.Drawing.Size(65, 19);
            this.radioButtonEvent7.TabIndex = 7;
            this.radioButtonEvent7.TabStop = true;
            this.radioButtonEvent7.Text = "Event 8";
            this.radioButtonEvent7.UseVisualStyleBackColor = true;
            this.radioButtonEvent7.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent6
            // 
            this.radioButtonEvent6.AutoSize = true;
            this.radioButtonEvent6.Location = new System.Drawing.Point(644, 8);
            this.radioButtonEvent6.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonEvent6.Name = "radioButtonEvent6";
            this.radioButtonEvent6.Size = new System.Drawing.Size(65, 19);
            this.radioButtonEvent6.TabIndex = 6;
            this.radioButtonEvent6.TabStop = true;
            this.radioButtonEvent6.Text = "Event 7";
            this.radioButtonEvent6.UseVisualStyleBackColor = true;
            this.radioButtonEvent6.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent5
            // 
            this.radioButtonEvent5.AutoSize = true;
            this.radioButtonEvent5.Location = new System.Drawing.Point(538, 8);
            this.radioButtonEvent5.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonEvent5.Name = "radioButtonEvent5";
            this.radioButtonEvent5.Size = new System.Drawing.Size(65, 19);
            this.radioButtonEvent5.TabIndex = 5;
            this.radioButtonEvent5.TabStop = true;
            this.radioButtonEvent5.Text = "Event 6";
            this.radioButtonEvent5.UseVisualStyleBackColor = true;
            this.radioButtonEvent5.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent4
            // 
            this.radioButtonEvent4.AutoSize = true;
            this.radioButtonEvent4.Location = new System.Drawing.Point(431, 8);
            this.radioButtonEvent4.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonEvent4.Name = "radioButtonEvent4";
            this.radioButtonEvent4.Size = new System.Drawing.Size(65, 19);
            this.radioButtonEvent4.TabIndex = 4;
            this.radioButtonEvent4.TabStop = true;
            this.radioButtonEvent4.Text = "Event 5";
            this.radioButtonEvent4.UseVisualStyleBackColor = true;
            this.radioButtonEvent4.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent3
            // 
            this.radioButtonEvent3.AutoSize = true;
            this.radioButtonEvent3.Location = new System.Drawing.Point(326, 8);
            this.radioButtonEvent3.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonEvent3.Name = "radioButtonEvent3";
            this.radioButtonEvent3.Size = new System.Drawing.Size(65, 19);
            this.radioButtonEvent3.TabIndex = 3;
            this.radioButtonEvent3.TabStop = true;
            this.radioButtonEvent3.Text = "Event 4";
            this.radioButtonEvent3.UseVisualStyleBackColor = true;
            this.radioButtonEvent3.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent2
            // 
            this.radioButtonEvent2.AutoSize = true;
            this.radioButtonEvent2.Location = new System.Drawing.Point(220, 8);
            this.radioButtonEvent2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonEvent2.Name = "radioButtonEvent2";
            this.radioButtonEvent2.Size = new System.Drawing.Size(65, 19);
            this.radioButtonEvent2.TabIndex = 2;
            this.radioButtonEvent2.TabStop = true;
            this.radioButtonEvent2.Text = "Event 3";
            this.radioButtonEvent2.UseVisualStyleBackColor = true;
            this.radioButtonEvent2.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent1
            // 
            this.radioButtonEvent1.AutoSize = true;
            this.radioButtonEvent1.Location = new System.Drawing.Point(113, 8);
            this.radioButtonEvent1.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonEvent1.Name = "radioButtonEvent1";
            this.radioButtonEvent1.Size = new System.Drawing.Size(65, 19);
            this.radioButtonEvent1.TabIndex = 1;
            this.radioButtonEvent1.TabStop = true;
            this.radioButtonEvent1.Text = "Event 2";
            this.radioButtonEvent1.UseVisualStyleBackColor = true;
            this.radioButtonEvent1.CheckedChanged += new System.EventHandler(this.radioButtonEventSelect_CheckedChanged);
            // 
            // radioButtonEvent0
            // 
            this.radioButtonEvent0.AutoSize = true;
            this.radioButtonEvent0.Location = new System.Drawing.Point(7, 8);
            this.radioButtonEvent0.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonEvent0.Name = "radioButtonEvent0";
            this.radioButtonEvent0.Size = new System.Drawing.Size(65, 19);
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
            this.ucEventGraph7.Location = new System.Drawing.Point(0, 38);
            this.ucEventGraph7.Margin = new System.Windows.Forms.Padding(4);
            this.ucEventGraph7.Name = "ucEventGraph7";
            this.ucEventGraph7.Size = new System.Drawing.Size(1152, 684);
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
            this.ucEventGraph6.Location = new System.Drawing.Point(0, 38);
            this.ucEventGraph6.Margin = new System.Windows.Forms.Padding(4);
            this.ucEventGraph6.Name = "ucEventGraph6";
            this.ucEventGraph6.Size = new System.Drawing.Size(1152, 684);
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
            this.ucEventGraph5.Location = new System.Drawing.Point(0, 38);
            this.ucEventGraph5.Margin = new System.Windows.Forms.Padding(4);
            this.ucEventGraph5.Name = "ucEventGraph5";
            this.ucEventGraph5.Size = new System.Drawing.Size(1152, 684);
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
            this.ucEventGraph4.Location = new System.Drawing.Point(0, 38);
            this.ucEventGraph4.Margin = new System.Windows.Forms.Padding(4);
            this.ucEventGraph4.Name = "ucEventGraph4";
            this.ucEventGraph4.Size = new System.Drawing.Size(1152, 684);
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
            this.ucEventGraph3.Location = new System.Drawing.Point(0, 38);
            this.ucEventGraph3.Margin = new System.Windows.Forms.Padding(4);
            this.ucEventGraph3.Name = "ucEventGraph3";
            this.ucEventGraph3.Size = new System.Drawing.Size(1152, 684);
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
            this.ucEventGraph2.Location = new System.Drawing.Point(0, 38);
            this.ucEventGraph2.Margin = new System.Windows.Forms.Padding(4);
            this.ucEventGraph2.Name = "ucEventGraph2";
            this.ucEventGraph2.Size = new System.Drawing.Size(1152, 684);
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
            this.ucEventGraph1.Location = new System.Drawing.Point(0, 38);
            this.ucEventGraph1.Margin = new System.Windows.Forms.Padding(4);
            this.ucEventGraph1.Name = "ucEventGraph1";
            this.ucEventGraph1.Size = new System.Drawing.Size(1152, 684);
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
            this.ucEventGraph0.Location = new System.Drawing.Point(0, 38);
            this.ucEventGraph0.Margin = new System.Windows.Forms.Padding(4);
            this.ucEventGraph0.Name = "ucEventGraph0";
            this.ucEventGraph0.Size = new System.Drawing.Size(1152, 684);
            this.ucEventGraph0.TabIndex = 0;
            this.ucEventGraph0.Type = RelayControlLibrary.EventTypes.Trip;
            // 
            // buttonReqLiveData
            // 
            this.buttonReqLiveData.Location = new System.Drawing.Point(991, 0);
            this.buttonReqLiveData.Margin = new System.Windows.Forms.Padding(4);
            this.buttonReqLiveData.Name = "buttonReqLiveData";
            this.buttonReqLiveData.Size = new System.Drawing.Size(155, 24);
            this.buttonReqLiveData.TabIndex = 0;
            this.buttonReqLiveData.Text = "Request LIVE Data";
            this.buttonReqLiveData.UseVisualStyleBackColor = true;
            this.buttonReqLiveData.Click += new System.EventHandler(this.buttonReqLiveData_Click);
            // 
            // tabPageFlightRecorder
            // 
            this.tabPageFlightRecorder.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageFlightRecorder.Controls.Add(this.buttonReqLiveData);
            this.tabPageFlightRecorder.Controls.Add(this.labelLiveDataTriggerTime);
            this.tabPageFlightRecorder.Controls.Add(this.ucLiveData1);
            this.tabPageFlightRecorder.Location = new System.Drawing.Point(4, 24);
            this.tabPageFlightRecorder.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageFlightRecorder.Name = "tabPageFlightRecorder";
            this.tabPageFlightRecorder.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageFlightRecorder.Size = new System.Drawing.Size(1449, 910);
            this.tabPageFlightRecorder.TabIndex = 5;
            this.tabPageFlightRecorder.Text = "Live Data";
            // 
            // labelLiveDataTriggerTime
            // 
            this.labelLiveDataTriggerTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelLiveDataTriggerTime.AutoSize = true;
            this.labelLiveDataTriggerTime.Location = new System.Drawing.Point(-7809, 7);
            this.labelLiveDataTriggerTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLiveDataTriggerTime.Name = "labelLiveDataTriggerTime";
            this.labelLiveDataTriggerTime.Size = new System.Drawing.Size(0, 15);
            this.labelLiveDataTriggerTime.TabIndex = 1;
            this.labelLiveDataTriggerTime.Resize += new System.EventHandler(this.labelLiveEventTriggerTime_Resize);
            // 
            // ucLiveData1
            // 
            this.ucLiveData1.CTRatio = 320;
            this.ucLiveData1.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucLiveData1.GEEnabled = false;
            this.ucLiveData1.Location = new System.Drawing.Point(0, 25);
            this.ucLiveData1.Margin = new System.Windows.Forms.Padding(4);
            this.ucLiveData1.Name = "ucLiveData1";
            this.ucLiveData1.Size = new System.Drawing.Size(1156, 695);
            this.ucLiveData1.TabIndex = 0;
            // 
            // tabPageTransmitter
            // 
            this.tabPageTransmitter.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageTransmitter.Controls.Add(this.labelRelayDisconnected3);
            this.tabPageTransmitter.Controls.Add(this.ucTransmitter1);
            this.tabPageTransmitter.Location = new System.Drawing.Point(4, 24);
            this.tabPageTransmitter.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageTransmitter.Name = "tabPageTransmitter";
            this.tabPageTransmitter.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageTransmitter.Size = new System.Drawing.Size(1449, 910);
            this.tabPageTransmitter.TabIndex = 2;
            this.tabPageTransmitter.Text = "Transmitter Settings";
            // 
            // labelRelayDisconnected3
            // 
            this.labelRelayDisconnected3.AutoSize = true;
            this.labelRelayDisconnected3.BackColor = System.Drawing.Color.Red;
            this.labelRelayDisconnected3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRelayDisconnected3.Location = new System.Drawing.Point(232, 579);
            this.labelRelayDisconnected3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRelayDisconnected3.Name = "labelRelayDisconnected3";
            this.labelRelayDisconnected3.Size = new System.Drawing.Size(151, 20);
            this.labelRelayDisconnected3.TabIndex = 70;
            this.labelRelayDisconnected3.Text = "Relay Disconnected";
            this.labelRelayDisconnected3.Visible = false;
            // 
            // ucTransmitter1
            // 
            this.ucTransmitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ucTransmitter1.CTRatio = ((uint)(320u));
            this.ucTransmitter1.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucTransmitter1.DNPCoverFlags = ((byte)(0));
            this.ucTransmitter1.DNPEnabled = false;
            this.ucTransmitter1.ForceDNPEnable = false;
            this.ucTransmitter1.FPGARevisionValid = true;
            this.ucTransmitter1.GERelay = false;
            this.ucTransmitter1.Location = new System.Drawing.Point(10, 7);
            this.ucTransmitter1.Margin = new System.Windows.Forms.Padding(4);
            this.ucTransmitter1.Name = "ucTransmitter1";
            this.ucTransmitter1.PacketLength = 30;
            this.ucTransmitter1.SerialNumber = 0;
            this.ucTransmitter1.Size = new System.Drawing.Size(1014, 706);
            this.ucTransmitter1.TabIndex = 0;
            this.ucTransmitter1.WaterBugNoTransmitter = false;
            // 
            // tabPageMonitor
            // 
            this.tabPageMonitor.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageMonitor.Controls.Add(this.labelSNPQMonitor);
            this.tabPageMonitor.Controls.Add(this.textBoxRelaySNControlPQ);
            this.tabPageMonitor.Controls.Add(this.textBoxCTRatioPQMonitor);
            this.tabPageMonitor.Controls.Add(this.checkBoxInTripRegion);
            this.tabPageMonitor.Controls.Add(this.buttonUpdateCTRatio);
            this.tabPageMonitor.Controls.Add(this.labelRelayTrippedOrClose);
            this.tabPageMonitor.Controls.Add(this.buttonToggleMonitor);
            this.tabPageMonitor.Controls.Add(this.labelCtRatioMonitor);
            this.tabPageMonitor.Controls.Add(this.ucPhasorGraph1);
            this.tabPageMonitor.Location = new System.Drawing.Point(4, 24);
            this.tabPageMonitor.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageMonitor.Name = "tabPageMonitor";
            this.tabPageMonitor.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageMonitor.Size = new System.Drawing.Size(1449, 910);
            this.tabPageMonitor.TabIndex = 1;
            this.tabPageMonitor.Text = "PQ Monitor";
            // 
            // labelSNPQMonitor
            // 
            this.labelSNPQMonitor.AutoSize = true;
            this.labelSNPQMonitor.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSNPQMonitor.Location = new System.Drawing.Point(183, 625);
            this.labelSNPQMonitor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSNPQMonitor.Name = "labelSNPQMonitor";
            this.labelSNPQMonitor.Size = new System.Drawing.Size(60, 13);
            this.labelSNPQMonitor.TabIndex = 76;
            this.labelSNPQMonitor.Text = "Relay S/N:";
            // 
            // textBoxRelaySNControlPQ
            // 
            this.textBoxRelaySNControlPQ.Location = new System.Drawing.Point(260, 623);
            this.textBoxRelaySNControlPQ.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxRelaySNControlPQ.MaxLength = 5;
            this.textBoxRelaySNControlPQ.Name = "textBoxRelaySNControlPQ";
            this.textBoxRelaySNControlPQ.ReadOnly = true;
            this.textBoxRelaySNControlPQ.Size = new System.Drawing.Size(85, 21);
            this.textBoxRelaySNControlPQ.TabIndex = 75;
            this.textBoxRelaySNControlPQ.Tag = "SN";
            // 
            // textBoxCTRatioPQMonitor
            // 
            this.textBoxCTRatioPQMonitor.Enabled = false;
            this.textBoxCTRatioPQMonitor.Location = new System.Drawing.Point(417, 625);
            this.textBoxCTRatioPQMonitor.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCTRatioPQMonitor.Name = "textBoxCTRatioPQMonitor";
            this.textBoxCTRatioPQMonitor.ReadOnly = true;
            this.textBoxCTRatioPQMonitor.Size = new System.Drawing.Size(56, 21);
            this.textBoxCTRatioPQMonitor.TabIndex = 65;
            this.textBoxCTRatioPQMonitor.Text = "320";
            // 
            // checkBoxInTripRegion
            // 
            this.checkBoxInTripRegion.AutoCheck = false;
            this.checkBoxInTripRegion.AutoSize = true;
            this.checkBoxInTripRegion.ForeColor = System.Drawing.Color.Black;
            this.checkBoxInTripRegion.Location = new System.Drawing.Point(820, 625);
            this.checkBoxInTripRegion.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxInTripRegion.Name = "checkBoxInTripRegion";
            this.checkBoxInTripRegion.Size = new System.Drawing.Size(103, 19);
            this.checkBoxInTripRegion.TabIndex = 49;
            this.checkBoxInTripRegion.Text = "In Trip Region";
            this.checkBoxInTripRegion.UseVisualStyleBackColor = true;
            // 
            // buttonUpdateCTRatio
            // 
            this.buttonUpdateCTRatio.Location = new System.Drawing.Point(794, 689);
            this.buttonUpdateCTRatio.Margin = new System.Windows.Forms.Padding(4);
            this.buttonUpdateCTRatio.Name = "buttonUpdateCTRatio";
            this.buttonUpdateCTRatio.Size = new System.Drawing.Size(116, 26);
            this.buttonUpdateCTRatio.TabIndex = 44;
            this.buttonUpdateCTRatio.Text = "Update CT Ratio";
            this.buttonUpdateCTRatio.UseVisualStyleBackColor = true;
            this.buttonUpdateCTRatio.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelRelayTrippedOrClose
            // 
            this.labelRelayTrippedOrClose.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelRelayTrippedOrClose.Location = new System.Drawing.Point(490, 623);
            this.labelRelayTrippedOrClose.Margin = new System.Windows.Forms.Padding(4);
            this.labelRelayTrippedOrClose.Name = "labelRelayTrippedOrClose";
            this.labelRelayTrippedOrClose.Padding = new System.Windows.Forms.Padding(1);
            this.labelRelayTrippedOrClose.Size = new System.Drawing.Size(116, 30);
            this.labelRelayTrippedOrClose.TabIndex = 36;
            this.labelRelayTrippedOrClose.Text = "Unkown";
            this.labelRelayTrippedOrClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonToggleMonitor
            // 
            this.buttonToggleMonitor.Location = new System.Drawing.Point(696, 621);
            this.buttonToggleMonitor.Margin = new System.Windows.Forms.Padding(4);
            this.buttonToggleMonitor.Name = "buttonToggleMonitor";
            this.buttonToggleMonitor.Size = new System.Drawing.Size(106, 26);
            this.buttonToggleMonitor.TabIndex = 35;
            this.buttonToggleMonitor.Text = "Toggle Monitor";
            this.buttonToggleMonitor.UseVisualStyleBackColor = true;
            this.buttonToggleMonitor.Click += new System.EventHandler(this.buttonToggleMonitor_Click);
            // 
            // labelCtRatioMonitor
            // 
            this.labelCtRatioMonitor.AutoSize = true;
            this.labelCtRatioMonitor.Location = new System.Drawing.Point(357, 625);
            this.labelCtRatioMonitor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCtRatioMonitor.Name = "labelCtRatioMonitor";
            this.labelCtRatioMonitor.Size = new System.Drawing.Size(57, 15);
            this.labelCtRatioMonitor.TabIndex = 31;
            this.labelCtRatioMonitor.Text = "CT Ratio:";
            // 
            // ucPhasorGraph1
            // 
            this.ucPhasorGraph1.BackColor = System.Drawing.SystemColors.Control;
            this.ucPhasorGraph1.Location = new System.Drawing.Point(0, 0);
            this.ucPhasorGraph1.Margin = new System.Windows.Forms.Padding(4);
            this.ucPhasorGraph1.Name = "ucPhasorGraph1";
            this.ucPhasorGraph1.RealTimeMonitoring = false;
            this.ucPhasorGraph1.RevisionNumber = ((uint)(0u));
            this.ucPhasorGraph1.Size = new System.Drawing.Size(1158, 660);
            this.ucPhasorGraph1.TabIndex = 45;
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
            // tabPageControl
            // 
            this.tabPageControl.BackColor = System.Drawing.Color.Transparent;
            this.tabPageControl.Controls.Add(this.button_dataStore);
            this.tabPageControl.Controls.Add(this.pictureBox_SendAll);
            this.tabPageControl.Controls.Add(this.groupBoxLRLockoutMain);
            this.tabPageControl.Controls.Add(this.groupBoxLowVoltThres);
            this.tabPageControl.Controls.Add(this.groupBoxRelayStatus);
            this.tabPageControl.Controls.Add(this.groupBoxPhasingAndType);
            this.tabPageControl.Controls.Add(this.groupBoxNetworkCTRatio);
            this.tabPageControl.Controls.Add(this.groupBoxRelayFlags);
            this.tabPageControl.Controls.Add(this.ucSafeService1);
            this.tabPageControl.Controls.Add(this.panelOtherRelayControls);
            this.tabPageControl.Controls.Add(this.ucTripMode2);
            this.tabPageControl.Controls.Add(this.ucCloseMode1);
            this.tabPageControl.Controls.Add(this.ucPumpMode1);
            this.tabPageControl.Controls.Add(this.ucCoverFlags1);
            this.tabPageControl.Location = new System.Drawing.Point(4, 24);
            this.tabPageControl.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageControl.Name = "tabPageControl";
            this.tabPageControl.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageControl.Size = new System.Drawing.Size(1449, 910);
            this.tabPageControl.TabIndex = 0;
            this.tabPageControl.Text = "Relay Settings";
            // 
            // button_dataStore
            // 
            this.button_dataStore.Location = new System.Drawing.Point(994, 567);
            this.button_dataStore.Name = "button_dataStore";
            this.button_dataStore.Size = new System.Drawing.Size(133, 23);
            this.button_dataStore.TabIndex = 121;
            this.button_dataStore.Text = "Temp_Data_Store";
            this.button_dataStore.UseVisualStyleBackColor = true;
            this.button_dataStore.Click += new System.EventHandler(this.button_dataStore_Click);
            // 
            // pictureBox_SendAll
            // 
            this.pictureBox_SendAll.Enabled = false;
            this.pictureBox_SendAll.Image = global::RelayControl.Properties.Resources.Throbber_SendAll;
            this.pictureBox_SendAll.InitialImage = global::RelayControl.Properties.Resources.Throbber_SendAll1;
            this.pictureBox_SendAll.Location = new System.Drawing.Point(469, 624);
            this.pictureBox_SendAll.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_SendAll.Name = "pictureBox_SendAll";
            this.pictureBox_SendAll.Size = new System.Drawing.Size(133, 118);
            this.pictureBox_SendAll.TabIndex = 120;
            this.pictureBox_SendAll.TabStop = false;
            this.pictureBox_SendAll.UseWaitCursor = true;
            this.pictureBox_SendAll.Visible = false;
            // 
            // groupBoxLRLockoutMain
            // 
            this.groupBoxLRLockoutMain.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxLRLockoutMain.Controls.Add(this.textBoxLRLockoutStatusMain);
            this.groupBoxLRLockoutMain.Controls.Add(this.labelLRLockoutMain);
            this.groupBoxLRLockoutMain.Location = new System.Drawing.Point(522, 546);
            this.groupBoxLRLockoutMain.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxLRLockoutMain.Name = "groupBoxLRLockoutMain";
            this.groupBoxLRLockoutMain.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxLRLockoutMain.Size = new System.Drawing.Size(297, 56);
            this.groupBoxLRLockoutMain.TabIndex = 118;
            this.groupBoxLRLockoutMain.TabStop = false;
            this.groupBoxLRLockoutMain.Text = "Remote Command Lockout";
            this.groupBoxLRLockoutMain.Visible = false;
            // 
            // textBoxLRLockoutStatusMain
            // 
            this.textBoxLRLockoutStatusMain.Location = new System.Drawing.Point(144, 23);
            this.textBoxLRLockoutStatusMain.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxLRLockoutStatusMain.Name = "textBoxLRLockoutStatusMain";
            this.textBoxLRLockoutStatusMain.Size = new System.Drawing.Size(116, 21);
            this.textBoxLRLockoutStatusMain.TabIndex = 1;
            // 
            // labelLRLockoutMain
            // 
            this.labelLRLockoutMain.AutoSize = true;
            this.labelLRLockoutMain.Location = new System.Drawing.Point(15, 26);
            this.labelLRLockoutMain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLRLockoutMain.Name = "labelLRLockoutMain";
            this.labelLRLockoutMain.Size = new System.Drawing.Size(110, 15);
            this.labelLRLockoutMain.TabIndex = 0;
            this.labelLRLockoutMain.Text = "RC Lockout Status:";
            // 
            // groupBoxLowVoltThres
            // 
            this.groupBoxLowVoltThres.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxLowVoltThres.Controls.Add(this.buttonRequestLowVotlageThres);
            this.groupBoxLowVoltThres.Controls.Add(this.numericUpDownLowVoltageThres);
            this.groupBoxLowVoltThres.Controls.Add(this.buttonSendLowVoltageThres);
            this.groupBoxLowVoltThres.Location = new System.Drawing.Point(13, 611);
            this.groupBoxLowVoltThres.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxLowVoltThres.Name = "groupBoxLowVoltThres";
            this.groupBoxLowVoltThres.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxLowVoltThres.Size = new System.Drawing.Size(386, 101);
            this.groupBoxLowVoltThres.TabIndex = 117;
            this.groupBoxLowVoltThres.TabStop = false;
            this.groupBoxLowVoltThres.Text = "Low Voltage Threshold";
            // 
            // buttonRequestLowVotlageThres
            // 
            this.buttonRequestLowVotlageThres.Location = new System.Drawing.Point(293, 35);
            this.buttonRequestLowVotlageThres.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRequestLowVotlageThres.Name = "buttonRequestLowVotlageThres";
            this.buttonRequestLowVotlageThres.Size = new System.Drawing.Size(88, 26);
            this.buttonRequestLowVotlageThres.TabIndex = 116;
            this.buttonRequestLowVotlageThres.Text = "Request";
            this.buttonRequestLowVotlageThres.UseVisualStyleBackColor = true;
            this.buttonRequestLowVotlageThres.Click += new System.EventHandler(this.buttonRequestLowVotlageThres_Click);
            // 
            // numericUpDownLowVoltageThres
            // 
            this.numericUpDownLowVoltageThres.Location = new System.Drawing.Point(17, 38);
            this.numericUpDownLowVoltageThres.Margin = new System.Windows.Forms.Padding(4);
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
            this.numericUpDownLowVoltageThres.Size = new System.Drawing.Size(140, 21);
            this.numericUpDownLowVoltageThres.TabIndex = 114;
            this.numericUpDownLowVoltageThres.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // buttonSendLowVoltageThres
            // 
            this.buttonSendLowVoltageThres.Location = new System.Drawing.Point(186, 35);
            this.buttonSendLowVoltageThres.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSendLowVoltageThres.Name = "buttonSendLowVoltageThres";
            this.buttonSendLowVoltageThres.Size = new System.Drawing.Size(88, 26);
            this.buttonSendLowVoltageThres.TabIndex = 113;
            this.buttonSendLowVoltageThres.Text = "Send";
            this.buttonSendLowVoltageThres.UseVisualStyleBackColor = true;
            this.buttonSendLowVoltageThres.Click += new System.EventHandler(this.buttonSendLowVoltageThres_Click);
            // 
            // groupBoxRelayStatus
            // 
            this.groupBoxRelayStatus.BackColor = System.Drawing.Color.Transparent;
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
            this.groupBoxRelayStatus.Location = new System.Drawing.Point(699, 8);
            this.groupBoxRelayStatus.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxRelayStatus.Name = "groupBoxRelayStatus";
            this.groupBoxRelayStatus.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxRelayStatus.Size = new System.Drawing.Size(154, 281);
            this.groupBoxRelayStatus.TabIndex = 112;
            this.groupBoxRelayStatus.TabStop = false;
            this.groupBoxRelayStatus.Text = "Relay Status:";
            // 
            // labelNWPStatus
            // 
            this.labelNWPStatus.AutoSize = true;
            this.labelNWPStatus.Location = new System.Drawing.Point(10, 205);
            this.labelNWPStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNWPStatus.Name = "labelNWPStatus";
            this.labelNWPStatus.Size = new System.Drawing.Size(93, 15);
            this.labelNWPStatus.TabIndex = 52;
            this.labelNWPStatus.Text = "NWP: Unknown";
            // 
            // checkBoxTripFlag
            // 
            this.checkBoxTripFlag.AutoCheck = false;
            this.checkBoxTripFlag.AutoSize = true;
            this.checkBoxTripFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxTripFlag.Location = new System.Drawing.Point(10, 22);
            this.checkBoxTripFlag.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxTripFlag.Name = "checkBoxTripFlag";
            this.checkBoxTripFlag.Size = new System.Drawing.Size(68, 19);
            this.checkBoxTripFlag.TabIndex = 0;
            this.checkBoxTripFlag.Text = "Tripped";
            this.checkBoxTripFlag.UseVisualStyleBackColor = true;
            // 
            // checkBoxPhasingOkayFlag
            // 
            this.checkBoxPhasingOkayFlag.AutoCheck = false;
            this.checkBoxPhasingOkayFlag.AutoSize = true;
            this.checkBoxPhasingOkayFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxPhasingOkayFlag.Location = new System.Drawing.Point(10, 156);
            this.checkBoxPhasingOkayFlag.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxPhasingOkayFlag.Name = "checkBoxPhasingOkayFlag";
            this.checkBoxPhasingOkayFlag.Size = new System.Drawing.Size(101, 19);
            this.checkBoxPhasingOkayFlag.TabIndex = 50;
            this.checkBoxPhasingOkayFlag.Text = "Phasing Okay";
            this.checkBoxPhasingOkayFlag.UseVisualStyleBackColor = true;
            // 
            // checkBoxPumping
            // 
            this.checkBoxPumping.AutoCheck = false;
            this.checkBoxPumping.AutoSize = true;
            this.checkBoxPumping.ForeColor = System.Drawing.Color.Black;
            this.checkBoxPumping.Location = new System.Drawing.Point(10, 180);
            this.checkBoxPumping.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxPumping.Name = "checkBoxPumping";
            this.checkBoxPumping.Size = new System.Drawing.Size(100, 19);
            this.checkBoxPumping.TabIndex = 35;
            this.checkBoxPumping.Text = "Pump Protect";
            this.checkBoxPumping.UseVisualStyleBackColor = true;
            // 
            // textBoxTemperature
            // 
            this.textBoxTemperature.Location = new System.Drawing.Point(85, 234);
            this.textBoxTemperature.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTemperature.Name = "textBoxTemperature";
            this.textBoxTemperature.ReadOnly = true;
            this.textBoxTemperature.Size = new System.Drawing.Size(56, 21);
            this.textBoxTemperature.TabIndex = 51;
            // 
            // checkBoxDefaultsUsed
            // 
            this.checkBoxDefaultsUsed.AutoCheck = false;
            this.checkBoxDefaultsUsed.AutoSize = true;
            this.checkBoxDefaultsUsed.Enabled = false;
            this.checkBoxDefaultsUsed.ForeColor = System.Drawing.Color.Black;
            this.checkBoxDefaultsUsed.Location = new System.Drawing.Point(19, 263);
            this.checkBoxDefaultsUsed.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxDefaultsUsed.Name = "checkBoxDefaultsUsed";
            this.checkBoxDefaultsUsed.Size = new System.Drawing.Size(103, 19);
            this.checkBoxDefaultsUsed.TabIndex = 38;
            this.checkBoxDefaultsUsed.Text = "Defaults Used";
            this.checkBoxDefaultsUsed.UseVisualStyleBackColor = true;
            this.checkBoxDefaultsUsed.Visible = false;
            // 
            // checkBoxBlockedOpenFlag
            // 
            this.checkBoxBlockedOpenFlag.AutoCheck = false;
            this.checkBoxBlockedOpenFlag.AutoSize = true;
            this.checkBoxBlockedOpenFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxBlockedOpenFlag.Location = new System.Drawing.Point(10, 131);
            this.checkBoxBlockedOpenFlag.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxBlockedOpenFlag.Name = "checkBoxBlockedOpenFlag";
            this.checkBoxBlockedOpenFlag.Size = new System.Drawing.Size(103, 19);
            this.checkBoxBlockedOpenFlag.TabIndex = 48;
            this.checkBoxBlockedOpenFlag.Text = "Blocked Open";
            this.checkBoxBlockedOpenFlag.UseVisualStyleBackColor = true;
            // 
            // textBoxTripCount
            // 
            this.textBoxTripCount.Location = new System.Drawing.Point(81, 102);
            this.textBoxTripCount.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTripCount.Name = "textBoxTripCount";
            this.textBoxTripCount.ReadOnly = true;
            this.textBoxTripCount.Size = new System.Drawing.Size(54, 21);
            this.textBoxTripCount.TabIndex = 43;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(7, 105);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 15);
            this.label4.TabIndex = 44;
            this.label4.Text = "Trip Cycles";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // checkBoxBFlag
            // 
            this.checkBoxBFlag.AutoCheck = false;
            this.checkBoxBFlag.AutoSize = true;
            this.checkBoxBFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxBFlag.Location = new System.Drawing.Point(10, 204);
            this.checkBoxBFlag.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxBFlag.Name = "checkBoxBFlag";
            this.checkBoxBFlag.Size = new System.Drawing.Size(112, 19);
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
            this.checkBoxTrippingFlag.Location = new System.Drawing.Point(10, 46);
            this.checkBoxTrippingFlag.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxTrippingFlag.Name = "checkBoxTrippingFlag";
            this.checkBoxTrippingFlag.Size = new System.Drawing.Size(71, 19);
            this.checkBoxTrippingFlag.TabIndex = 46;
            this.checkBoxTrippingFlag.Text = "Tripping";
            this.checkBoxTrippingFlag.UseVisualStyleBackColor = true;
            // 
            // checkBoxFloatFlag
            // 
            this.checkBoxFloatFlag.AutoCheck = false;
            this.checkBoxFloatFlag.AutoSize = true;
            this.checkBoxFloatFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxFloatFlag.Location = new System.Drawing.Point(10, 73);
            this.checkBoxFloatFlag.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxFloatFlag.Name = "checkBoxFloatFlag";
            this.checkBoxFloatFlag.Size = new System.Drawing.Size(53, 19);
            this.checkBoxFloatFlag.TabIndex = 47;
            this.checkBoxFloatFlag.Text = "Float";
            this.checkBoxFloatFlag.UseVisualStyleBackColor = true;
            // 
            // groupBoxPhasingAndType
            // 
            this.groupBoxPhasingAndType.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxPhasingAndType.Controls.Add(this.labelDNPVoltage);
            this.groupBoxPhasingAndType.Controls.Add(this.comboBoxDNPVoltage);
            this.groupBoxPhasingAndType.Controls.Add(this.checkBox277DNPOutputs);
            this.groupBoxPhasingAndType.Controls.Add(this.labelConEdPowerRelay);
            this.groupBoxPhasingAndType.Controls.Add(this.label20);
            this.groupBoxPhasingAndType.Controls.Add(this.labelProtectorType);
            this.groupBoxPhasingAndType.Controls.Add(this.domainUpDownPhasings);
            this.groupBoxPhasingAndType.Controls.Add(this.labelGEWH);
            this.groupBoxPhasingAndType.Controls.Add(this.domainUpDownRelayType);
            this.groupBoxPhasingAndType.Controls.Add(this.buttonTypePhasingRestoreDefaults);
            this.groupBoxPhasingAndType.Controls.Add(this.buttonRelayType);
            this.groupBoxPhasingAndType.Location = new System.Drawing.Point(13, 390);
            this.groupBoxPhasingAndType.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxPhasingAndType.Name = "groupBoxPhasingAndType";
            this.groupBoxPhasingAndType.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxPhasingAndType.Size = new System.Drawing.Size(202, 184);
            this.groupBoxPhasingAndType.TabIndex = 111;
            this.groupBoxPhasingAndType.TabStop = false;
            this.groupBoxPhasingAndType.Text = "Relay Phasing and Type:";
            // 
            // labelDNPVoltage
            // 
            this.labelDNPVoltage.AutoSize = true;
            this.labelDNPVoltage.Location = new System.Drawing.Point(8, 79);
            this.labelDNPVoltage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDNPVoltage.Name = "labelDNPVoltage";
            this.labelDNPVoltage.Size = new System.Drawing.Size(103, 15);
            this.labelDNPVoltage.TabIndex = 78;
            this.labelDNPVoltage.Text = "Protector Voltage:";
            // 
            // comboBoxDNPVoltage
            // 
            this.comboBoxDNPVoltage.FormattingEnabled = true;
#if !CONED
            this.comboBoxDNPVoltage.Items.AddRange(new object[] {
            "125",
            "277",
            "347"});
#else
            this.comboBoxDNPVoltage.Items.AddRange(new object[] {
            "125",
            "277"});
#endif

            this.comboBoxDNPVoltage.Location = new System.Drawing.Point(119, 75);
            this.comboBoxDNPVoltage.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxDNPVoltage.Name = "comboBoxDNPVoltage";
            this.comboBoxDNPVoltage.Size = new System.Drawing.Size(75, 23);
            this.comboBoxDNPVoltage.TabIndex = 77;
            this.comboBoxDNPVoltage.SelectedIndexChanged += new System.EventHandler(this.comboBoxDNPVoltage_SelectedIndexChanged);
            // 
            // checkBox277DNPOutputs
            // 
            this.checkBox277DNPOutputs.AutoSize = true;
            this.checkBox277DNPOutputs.Location = new System.Drawing.Point(10, 99);
            this.checkBox277DNPOutputs.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox277DNPOutputs.Name = "checkBox277DNPOutputs";
            this.checkBox277DNPOutputs.Size = new System.Drawing.Size(179, 19);
            this.checkBox277DNPOutputs.TabIndex = 76;
            this.checkBox277DNPOutputs.Text = "Convert DNP Output Voltage";
            this.checkBox277DNPOutputs.UseVisualStyleBackColor = true;
            // 
            // labelConEdPowerRelay
            // 
            this.labelConEdPowerRelay.AutoSize = true;
            this.labelConEdPowerRelay.BackColor = System.Drawing.Color.Transparent;
            this.labelConEdPowerRelay.Location = new System.Drawing.Point(94, 19);
            this.labelConEdPowerRelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelConEdPowerRelay.Name = "labelConEdPowerRelay";
            this.labelConEdPowerRelay.Size = new System.Drawing.Size(42, 15);
            this.labelConEdPowerRelay.TabIndex = 51;
            this.labelConEdPowerRelay.Text = "Power";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(13, 19);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(70, 15);
            this.label20.TabIndex = 44;
            this.label20.Text = "Relay Type:";
            // 
            // labelProtectorType
            // 
            this.labelProtectorType.AutoSize = true;
            this.labelProtectorType.Location = new System.Drawing.Point(13, 125);
            this.labelProtectorType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProtectorType.Name = "labelProtectorType";
            this.labelProtectorType.Size = new System.Drawing.Size(88, 15);
            this.labelProtectorType.TabIndex = 50;
            this.labelProtectorType.Text = "Protector Type:";
            // 
            // domainUpDownPhasings
            // 
            this.domainUpDownPhasings.Items.Add("ABC : CAB : BCA");
            this.domainUpDownPhasings.Items.Add("CBA : BAC : ACB");
            this.domainUpDownPhasings.Location = new System.Drawing.Point(52, 45);
            this.domainUpDownPhasings.Margin = new System.Windows.Forms.Padding(4);
            this.domainUpDownPhasings.Name = "domainUpDownPhasings";
            this.domainUpDownPhasings.Size = new System.Drawing.Size(133, 21);
            this.domainUpDownPhasings.TabIndex = 47;
            // 
            // labelGEWH
            // 
            this.labelGEWH.AutoSize = true;
            this.labelGEWH.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelGEWH.Location = new System.Drawing.Point(113, 125);
            this.labelGEWH.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGEWH.Name = "labelGEWH";
            this.labelGEWH.Size = new System.Drawing.Size(29, 17);
            this.labelGEWH.TabIndex = 49;
            this.labelGEWH.Text = "WH";
            // 
            // domainUpDownRelayType
            // 
            this.domainUpDownRelayType.Items.Add("Power");
            this.domainUpDownRelayType.Items.Add("Sequence");
            this.domainUpDownRelayType.Location = new System.Drawing.Point(92, 16);
            this.domainUpDownRelayType.Margin = new System.Windows.Forms.Padding(4);
            this.domainUpDownRelayType.Name = "domainUpDownRelayType";
            this.domainUpDownRelayType.Size = new System.Drawing.Size(98, 21);
            this.domainUpDownRelayType.TabIndex = 43;
            this.domainUpDownRelayType.SelectedItemChanged += new System.EventHandler(this.domainUpDownRelayType_SelectedItemChanged);
            // 
            // buttonTypePhasingRestoreDefaults
            // 
            this.buttonTypePhasingRestoreDefaults.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.buttonTypePhasingRestoreDefaults.Location = new System.Drawing.Point(8, 154);
            this.buttonTypePhasingRestoreDefaults.Margin = new System.Windows.Forms.Padding(4);
            this.buttonTypePhasingRestoreDefaults.Name = "buttonTypePhasingRestoreDefaults";
            this.buttonTypePhasingRestoreDefaults.Size = new System.Drawing.Size(134, 26);
            this.buttonTypePhasingRestoreDefaults.TabIndex = 48;
            this.buttonTypePhasingRestoreDefaults.Text = "Restore Factory Defaults";
            this.buttonTypePhasingRestoreDefaults.UseVisualStyleBackColor = true;
            this.buttonTypePhasingRestoreDefaults.Click += new System.EventHandler(this.buttonTypePhasingRestoreDefaults_Click);
            // 
            // buttonRelayType
            // 
            this.buttonRelayType.Location = new System.Drawing.Point(142, 154);
            this.buttonRelayType.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRelayType.Name = "buttonRelayType";
            this.buttonRelayType.Size = new System.Drawing.Size(52, 26);
            this.buttonRelayType.TabIndex = 41;
            this.buttonRelayType.Text = "Send";
            this.buttonRelayType.UseVisualStyleBackColor = true;
            this.buttonRelayType.Click += new System.EventHandler(this.buttonRelayType_Click);
            // 
            // groupBoxNetworkCTRatio
            // 
            this.groupBoxNetworkCTRatio.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxNetworkCTRatio.Controls.Add(this.labelOver5);
            this.groupBoxNetworkCTRatio.Controls.Add(this.label27);
            this.groupBoxNetworkCTRatio.Controls.Add(this.buttonSendCTRatio);
            this.groupBoxNetworkCTRatio.Controls.Add(this.textBoxCTRatio);
            this.groupBoxNetworkCTRatio.Controls.Add(this.domainUpDownCTRatioM);
            this.groupBoxNetworkCTRatio.Location = new System.Drawing.Point(13, 304);
            this.groupBoxNetworkCTRatio.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxNetworkCTRatio.Name = "groupBoxNetworkCTRatio";
            this.groupBoxNetworkCTRatio.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxNetworkCTRatio.Size = new System.Drawing.Size(202, 80);
            this.groupBoxNetworkCTRatio.TabIndex = 110;
            this.groupBoxNetworkCTRatio.TabStop = false;
            this.groupBoxNetworkCTRatio.Text = "Network Protector CT Ratio:";
            // 
            // labelOver5
            // 
            this.labelOver5.AutoSize = true;
            this.labelOver5.Location = new System.Drawing.Point(66, 56);
            this.labelOver5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOver5.Name = "labelOver5";
            this.labelOver5.Size = new System.Drawing.Size(17, 15);
            this.labelOver5.TabIndex = 73;
            this.labelOver5.Text = "/5";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(7, 23);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(39, 15);
            this.label27.TabIndex = 68;
            this.label27.Text = "Ratio:";
            // 
            // buttonSendCTRatio
            // 
            this.buttonSendCTRatio.Location = new System.Drawing.Point(98, 51);
            this.buttonSendCTRatio.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSendCTRatio.Name = "buttonSendCTRatio";
            this.buttonSendCTRatio.Size = new System.Drawing.Size(88, 26);
            this.buttonSendCTRatio.TabIndex = 61;
            this.buttonSendCTRatio.Text = "Send";
            this.buttonSendCTRatio.UseVisualStyleBackColor = true;
            this.buttonSendCTRatio.Click += new System.EventHandler(this.buttonSendCTRatio_Click);
            // 
            // textBoxCTRatio
            // 
            this.textBoxCTRatio.Enabled = false;
            this.textBoxCTRatio.Location = new System.Drawing.Point(8, 53);
            this.textBoxCTRatio.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCTRatio.Name = "textBoxCTRatio";
            this.textBoxCTRatio.Size = new System.Drawing.Size(56, 21);
            this.textBoxCTRatio.TabIndex = 64;
            this.textBoxCTRatio.Text = "320";
            this.textBoxCTRatio.Leave += new System.EventHandler(this.textBoxCTRatio_Leave);
            // 
            // domainUpDownCTRatioM
            // 
            this.domainUpDownCTRatioM.Items.Add("3750:5");
            this.domainUpDownCTRatioM.Items.Add("3500:5");
            this.domainUpDownCTRatioM.Items.Add("3000:5");
            this.domainUpDownCTRatioM.Items.Add("2500:5");
            this.domainUpDownCTRatioM.Items.Add("2000:5");
            this.domainUpDownCTRatioM.Items.Add("1600:5");
            this.domainUpDownCTRatioM.Items.Add("1200:5");
            this.domainUpDownCTRatioM.Items.Add("800:5");
            this.domainUpDownCTRatioM.Items.Add("Special");
            this.domainUpDownCTRatioM.Location = new System.Drawing.Point(55, 21);
            this.domainUpDownCTRatioM.Margin = new System.Windows.Forms.Padding(4);
            this.domainUpDownCTRatioM.Name = "domainUpDownCTRatioM";
            this.domainUpDownCTRatioM.Size = new System.Drawing.Size(71, 21);
            this.domainUpDownCTRatioM.TabIndex = 62;
            this.domainUpDownCTRatioM.Text = "1600:5";
            this.domainUpDownCTRatioM.SelectedItemChanged += new System.EventHandler(this.domainUpDownCTRatioM_SelectedItemChanged);
            // 
            // groupBoxRelayFlags
            // 
            this.groupBoxRelayFlags.BackColor = System.Drawing.Color.Transparent;
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
            this.groupBoxRelayFlags.Location = new System.Drawing.Point(662, 602);
            this.groupBoxRelayFlags.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxRelayFlags.Name = "groupBoxRelayFlags";
            this.groupBoxRelayFlags.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxRelayFlags.Size = new System.Drawing.Size(491, 112);
            this.groupBoxRelayFlags.TabIndex = 109;
            this.groupBoxRelayFlags.TabStop = false;
            this.groupBoxRelayFlags.Text = "Relay Flags:";
            // 
            // labelQuietMode
            // 
            this.labelQuietMode.AutoSize = true;
            this.labelQuietMode.BackColor = System.Drawing.Color.Yellow;
            this.labelQuietMode.Location = new System.Drawing.Point(216, 91);
            this.labelQuietMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelQuietMode.Name = "labelQuietMode";
            this.labelQuietMode.Size = new System.Drawing.Size(71, 15);
            this.labelQuietMode.TabIndex = 52;
            this.labelQuietMode.Text = "Quiet Mode";
            this.labelQuietMode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelQuietMode_MouseDown);
            // 
            // checkBoxOffsetOkay
            // 
            this.checkBoxOffsetOkay.AutoCheck = false;
            this.checkBoxOffsetOkay.AutoSize = true;
            this.checkBoxOffsetOkay.ForeColor = System.Drawing.Color.Black;
            this.checkBoxOffsetOkay.Location = new System.Drawing.Point(7, 22);
            this.checkBoxOffsetOkay.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxOffsetOkay.Name = "checkBoxOffsetOkay";
            this.checkBoxOffsetOkay.Size = new System.Drawing.Size(87, 19);
            this.checkBoxOffsetOkay.TabIndex = 37;
            this.checkBoxOffsetOkay.Text = "Offset Okay";
            this.checkBoxOffsetOkay.UseVisualStyleBackColor = true;
            // 
            // checkBoxCalibrating
            // 
            this.checkBoxCalibrating.AutoCheck = false;
            this.checkBoxCalibrating.AutoSize = true;
            this.checkBoxCalibrating.ForeColor = System.Drawing.Color.Black;
            this.checkBoxCalibrating.Location = new System.Drawing.Point(123, 69);
            this.checkBoxCalibrating.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxCalibrating.Name = "checkBoxCalibrating";
            this.checkBoxCalibrating.Size = new System.Drawing.Size(85, 19);
            this.checkBoxCalibrating.TabIndex = 51;
            this.checkBoxCalibrating.Text = "Calibrating";
            this.checkBoxCalibrating.UseVisualStyleBackColor = true;
            // 
            // checkBoxMathOverTime
            // 
            this.checkBoxMathOverTime.AutoCheck = false;
            this.checkBoxMathOverTime.AutoSize = true;
            this.checkBoxMathOverTime.ForeColor = System.Drawing.Color.Black;
            this.checkBoxMathOverTime.Location = new System.Drawing.Point(332, 67);
            this.checkBoxMathOverTime.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxMathOverTime.Name = "checkBoxMathOverTime";
            this.checkBoxMathOverTime.Size = new System.Drawing.Size(113, 19);
            this.checkBoxMathOverTime.TabIndex = 36;
            this.checkBoxMathOverTime.Text = "Math Over Time";
            this.checkBoxMathOverTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxBlockedCloseFlag
            // 
            this.checkBoxBlockedCloseFlag.AutoCheck = false;
            this.checkBoxBlockedCloseFlag.AutoSize = true;
            this.checkBoxBlockedCloseFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxBlockedCloseFlag.Location = new System.Drawing.Point(216, 44);
            this.checkBoxBlockedCloseFlag.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxBlockedCloseFlag.Name = "checkBoxBlockedCloseFlag";
            this.checkBoxBlockedCloseFlag.Size = new System.Drawing.Size(104, 19);
            this.checkBoxBlockedCloseFlag.TabIndex = 49;
            this.checkBoxBlockedCloseFlag.Text = "Blocked Close";
            this.checkBoxBlockedCloseFlag.UseVisualStyleBackColor = true;
            // 
            // checkBoxMathError
            // 
            this.checkBoxMathError.AutoCheck = false;
            this.checkBoxMathError.AutoSize = true;
            this.checkBoxMathError.ForeColor = System.Drawing.Color.Black;
            this.checkBoxMathError.Location = new System.Drawing.Point(332, 44);
            this.checkBoxMathError.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxMathError.Name = "checkBoxMathError";
            this.checkBoxMathError.Size = new System.Drawing.Size(84, 19);
            this.checkBoxMathError.TabIndex = 39;
            this.checkBoxMathError.Text = "Math Error";
            this.checkBoxMathError.UseVisualStyleBackColor = true;
            // 
            // checkBoxInInsensRegion
            // 
            this.checkBoxInInsensRegion.AutoCheck = false;
            this.checkBoxInInsensRegion.AutoSize = true;
            this.checkBoxInInsensRegion.ForeColor = System.Drawing.Color.Black;
            this.checkBoxInInsensRegion.Location = new System.Drawing.Point(307, 22);
            this.checkBoxInInsensRegion.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxInInsensRegion.Name = "checkBoxInInsensRegion";
            this.checkBoxInInsensRegion.Size = new System.Drawing.Size(139, 19);
            this.checkBoxInInsensRegion.TabIndex = 47;
            this.checkBoxInInsensRegion.Text = "In Insensitive Reigon";
            this.checkBoxInInsensRegion.UseVisualStyleBackColor = true;
            // 
            // checkBoxMonitorPhasors
            // 
            this.checkBoxMonitorPhasors.AutoCheck = false;
            this.checkBoxMonitorPhasors.AutoSize = true;
            this.checkBoxMonitorPhasors.ForeColor = System.Drawing.Color.Black;
            this.checkBoxMonitorPhasors.Location = new System.Drawing.Point(7, 45);
            this.checkBoxMonitorPhasors.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxMonitorPhasors.Name = "checkBoxMonitorPhasors";
            this.checkBoxMonitorPhasors.Size = new System.Drawing.Size(116, 19);
            this.checkBoxMonitorPhasors.TabIndex = 40;
            this.checkBoxMonitorPhasors.Text = "Monitor Phasors";
            this.checkBoxMonitorPhasors.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlag2
            // 
            this.checkBoxFlag2.AutoCheck = false;
            this.checkBoxFlag2.AutoSize = true;
            this.checkBoxFlag2.ForeColor = System.Drawing.Color.Black;
            this.checkBoxFlag2.Location = new System.Drawing.Point(123, 43);
            this.checkBoxFlag2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxFlag2.Name = "checkBoxFlag2";
            this.checkBoxFlag2.Size = new System.Drawing.Size(57, 19);
            this.checkBoxFlag2.TabIndex = 45;
            this.checkBoxFlag2.Text = "Flag2";
            this.checkBoxFlag2.UseVisualStyleBackColor = true;
            // 
            // checkBoxSequence
            // 
            this.checkBoxSequence.AutoCheck = false;
            this.checkBoxSequence.AutoSize = true;
            this.checkBoxSequence.ForeColor = System.Drawing.Color.Black;
            this.checkBoxSequence.Location = new System.Drawing.Point(7, 68);
            this.checkBoxSequence.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxSequence.Name = "checkBoxSequence";
            this.checkBoxSequence.Size = new System.Drawing.Size(82, 19);
            this.checkBoxSequence.TabIndex = 42;
            this.checkBoxSequence.Text = "Sequence";
            this.checkBoxSequence.UseVisualStyleBackColor = true;
            // 
            // checkBoxACB
            // 
            this.checkBoxACB.AutoCheck = false;
            this.checkBoxACB.AutoSize = true;
            this.checkBoxACB.ForeColor = System.Drawing.Color.Black;
            this.checkBoxACB.Location = new System.Drawing.Point(216, 22);
            this.checkBoxACB.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxACB.Name = "checkBoxACB";
            this.checkBoxACB.Size = new System.Drawing.Size(49, 19);
            this.checkBoxACB.TabIndex = 43;
            this.checkBoxACB.Text = "ACB";
            this.checkBoxACB.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlag1
            // 
            this.checkBoxFlag1.AutoCheck = false;
            this.checkBoxFlag1.AutoSize = true;
            this.checkBoxFlag1.ForeColor = System.Drawing.Color.Black;
            this.checkBoxFlag1.Location = new System.Drawing.Point(123, 20);
            this.checkBoxFlag1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxFlag1.Name = "checkBoxFlag1";
            this.checkBoxFlag1.Size = new System.Drawing.Size(57, 19);
            this.checkBoxFlag1.TabIndex = 44;
            this.checkBoxFlag1.Text = "Flag1";
            this.checkBoxFlag1.UseVisualStyleBackColor = true;
            // 
            // checkBoxPowerSaveFlag
            // 
            this.checkBoxPowerSaveFlag.AutoCheck = false;
            this.checkBoxPowerSaveFlag.AutoSize = true;
            this.checkBoxPowerSaveFlag.ForeColor = System.Drawing.Color.Black;
            this.checkBoxPowerSaveFlag.Location = new System.Drawing.Point(216, 68);
            this.checkBoxPowerSaveFlag.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxPowerSaveFlag.Name = "checkBoxPowerSaveFlag";
            this.checkBoxPowerSaveFlag.Size = new System.Drawing.Size(91, 19);
            this.checkBoxPowerSaveFlag.TabIndex = 45;
            this.checkBoxPowerSaveFlag.Text = "Power Save";
            this.checkBoxPowerSaveFlag.UseVisualStyleBackColor = true;
            // 
            // ucSafeService1
            // 
            this.ucSafeService1.BackColor = System.Drawing.Color.Transparent;
            this.ucSafeService1.CTRatio = 320;
            this.ucSafeService1.LoadingNewCode = false;
            this.ucSafeService1.Location = new System.Drawing.Point(857, 5);
            this.ucSafeService1.Margin = new System.Windows.Forms.Padding(4);
            this.ucSafeService1.Name = "ucSafeService1";
            this.ucSafeService1.Size = new System.Drawing.Size(270, 289);
            this.ucSafeService1.TabIndex = 108;
            // 
            // panelOtherRelayControls
            // 
            this.panelOtherRelayControls.BackColor = System.Drawing.Color.Transparent;
            this.panelOtherRelayControls.Controls.Add(this.ucRemoteCommandBlock1);
            this.panelOtherRelayControls.Controls.Add(this.ucBlockControl1);
            this.panelOtherRelayControls.Controls.Add(this.labelBootRevision);
            this.panelOtherRelayControls.Controls.Add(this.buttonClearCycleCount);
            this.panelOtherRelayControls.Controls.Add(this.buttonBlockAndTrip);
            this.panelOtherRelayControls.Controls.Add(this.buttonRequestRelayParamaters);
            this.panelOtherRelayControls.Controls.Add(this.labelRelaySNControl);
            this.panelOtherRelayControls.Controls.Add(this.textBoxRelaySNControl);
            this.panelOtherRelayControls.Controls.Add(this.labelRevision);
            this.panelOtherRelayControls.Controls.Add(this.labelFPGARevision);
            this.panelOtherRelayControls.Controls.Add(this.labelRelayRevision);
            this.panelOtherRelayControls.Controls.Add(this.buttonResetBothProc);
            this.panelOtherRelayControls.Controls.Add(this.labelRelayStateControlPage);
            this.panelOtherRelayControls.Controls.Add(this.buttonSendAll);
            this.panelOtherRelayControls.Controls.Add(this.textBoxSaveStateName);
            this.panelOtherRelayControls.Controls.Add(this.buttonTripRelay);
            this.panelOtherRelayControls.Controls.Add(this.labelRelayDisconnected);
            this.panelOtherRelayControls.Controls.Add(this.comboBoxSavedStates);
            this.panelOtherRelayControls.Controls.Add(this.buttonSaveSetting);
            this.panelOtherRelayControls.Controls.Add(this.buttonDeleteSetting);
            this.panelOtherRelayControls.Location = new System.Drawing.Point(522, 310);
            this.panelOtherRelayControls.Margin = new System.Windows.Forms.Padding(4);
            this.panelOtherRelayControls.Name = "panelOtherRelayControls";
            this.panelOtherRelayControls.Size = new System.Drawing.Size(620, 241);
            this.panelOtherRelayControls.TabIndex = 77;
            // 
            // ucRemoteCommandBlock1
            // 
            this.ucRemoteCommandBlock1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ucRemoteCommandBlock1.CommandsBlocked = false;
            this.ucRemoteCommandBlock1.Location = new System.Drawing.Point(448, 81);
            this.ucRemoteCommandBlock1.Margin = new System.Windows.Forms.Padding(4);
            this.ucRemoteCommandBlock1.Name = "ucRemoteCommandBlock1";
            this.ucRemoteCommandBlock1.Size = new System.Drawing.Size(147, 80);
            this.ucRemoteCommandBlock1.TabIndex = 80;
            // 
            // ucBlockControl1
            // 
            this.ucBlockControl1.Location = new System.Drawing.Point(454, 0);
            this.ucBlockControl1.Margin = new System.Windows.Forms.Padding(4);
            this.ucBlockControl1.Name = "ucBlockControl1";
            this.ucBlockControl1.RelayBlocked = false;
            this.ucBlockControl1.Size = new System.Drawing.Size(122, 77);
            this.ucBlockControl1.TabIndex = 79;
            // 
            // labelBootRevision
            // 
            this.labelBootRevision.AutoSize = true;
            this.labelBootRevision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelBootRevision.Location = new System.Drawing.Point(4, 186);
            this.labelBootRevision.Margin = new System.Windows.Forms.Padding(4);
            this.labelBootRevision.Name = "labelBootRevision";
            this.labelBootRevision.Padding = new System.Windows.Forms.Padding(1);
            this.labelBootRevision.Size = new System.Drawing.Size(4, 19);
            this.labelBootRevision.TabIndex = 77;
            this.labelBootRevision.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelBootRevision.Visible = false;
            // 
            // buttonClearCycleCount
            // 
            this.buttonClearCycleCount.Location = new System.Drawing.Point(4, 4);
            this.buttonClearCycleCount.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClearCycleCount.Name = "buttonClearCycleCount";
            this.buttonClearCycleCount.Size = new System.Drawing.Size(130, 26);
            this.buttonClearCycleCount.TabIndex = 62;
            this.buttonClearCycleCount.Text = "Clear Cycle Count";
            this.buttonClearCycleCount.UseVisualStyleBackColor = true;
            this.buttonClearCycleCount.Click += new System.EventHandler(this.buttonClearCycleCount_Click);
            // 
            // buttonBlockAndTrip
            // 
            this.buttonBlockAndTrip.Location = new System.Drawing.Point(305, 81);
            this.buttonBlockAndTrip.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBlockAndTrip.Name = "buttonBlockAndTrip";
            this.buttonBlockAndTrip.Size = new System.Drawing.Size(141, 26);
            this.buttonBlockAndTrip.TabIndex = 76;
            this.buttonBlockAndTrip.Text = "Block and Trip Relay";
            this.buttonBlockAndTrip.UseVisualStyleBackColor = true;
            this.buttonBlockAndTrip.Click += new System.EventHandler(this.buttonBlockAndTrip_Click);
            // 
            // buttonRequestRelayParamaters
            // 
            this.buttonRequestRelayParamaters.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRequestRelayParamaters.Location = new System.Drawing.Point(290, 185);
            this.buttonRequestRelayParamaters.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRequestRelayParamaters.Name = "buttonRequestRelayParamaters";
            this.buttonRequestRelayParamaters.Size = new System.Drawing.Size(164, 49);
            this.buttonRequestRelayParamaters.TabIndex = 33;
            this.buttonRequestRelayParamaters.Text = "Read";
            this.buttonRequestRelayParamaters.UseVisualStyleBackColor = true;
            this.buttonRequestRelayParamaters.Click += new System.EventHandler(this.buttonRequestRelayParamaters_Click);
            // 
            // labelRelaySNControl
            // 
            this.labelRelaySNControl.AutoSize = true;
            this.labelRelaySNControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRelaySNControl.Location = new System.Drawing.Point(140, 9);
            this.labelRelaySNControl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRelaySNControl.Name = "labelRelaySNControl";
            this.labelRelaySNControl.Size = new System.Drawing.Size(60, 13);
            this.labelRelaySNControl.TabIndex = 74;
            this.labelRelaySNControl.Text = "Relay S/N:";
            // 
            // textBoxRelaySNControl
            // 
            this.textBoxRelaySNControl.Location = new System.Drawing.Point(217, 6);
            this.textBoxRelaySNControl.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxRelaySNControl.MaxLength = 5;
            this.textBoxRelaySNControl.Name = "textBoxRelaySNControl";
            this.textBoxRelaySNControl.ReadOnly = true;
            this.textBoxRelaySNControl.Size = new System.Drawing.Size(101, 21);
            this.textBoxRelaySNControl.TabIndex = 73;
            this.textBoxRelaySNControl.Tag = "SN";
            // 
            // labelRevision
            // 
            this.labelRevision.AutoSize = true;
            this.labelRevision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelRevision.Location = new System.Drawing.Point(4, 99);
            this.labelRevision.Margin = new System.Windows.Forms.Padding(4);
            this.labelRevision.Name = "labelRevision";
            this.labelRevision.Padding = new System.Windows.Forms.Padding(1);
            this.labelRevision.Size = new System.Drawing.Size(4, 19);
            this.labelRevision.TabIndex = 54;
            this.labelRevision.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFPGARevision
            // 
            this.labelFPGARevision.AutoSize = true;
            this.labelFPGARevision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelFPGARevision.Location = new System.Drawing.Point(4, 157);
            this.labelFPGARevision.Margin = new System.Windows.Forms.Padding(4);
            this.labelFPGARevision.Name = "labelFPGARevision";
            this.labelFPGARevision.Padding = new System.Windows.Forms.Padding(1);
            this.labelFPGARevision.Size = new System.Drawing.Size(4, 19);
            this.labelFPGARevision.TabIndex = 72;
            this.labelFPGARevision.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRelayRevision
            // 
            this.labelRelayRevision.AutoSize = true;
            this.labelRelayRevision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelRelayRevision.Location = new System.Drawing.Point(4, 127);
            this.labelRelayRevision.Margin = new System.Windows.Forms.Padding(4);
            this.labelRelayRevision.Name = "labelRelayRevision";
            this.labelRelayRevision.Padding = new System.Windows.Forms.Padding(1);
            this.labelRelayRevision.Size = new System.Drawing.Size(4, 19);
            this.labelRelayRevision.TabIndex = 55;
            this.labelRelayRevision.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonResetBothProc
            // 
            this.buttonResetBothProc.Location = new System.Drawing.Point(305, 111);
            this.buttonResetBothProc.Margin = new System.Windows.Forms.Padding(4);
            this.buttonResetBothProc.Name = "buttonResetBothProc";
            this.buttonResetBothProc.Size = new System.Drawing.Size(141, 26);
            this.buttonResetBothProc.TabIndex = 71;
            this.buttonResetBothProc.Text = "Reset Relay";
            this.buttonResetBothProc.UseVisualStyleBackColor = true;
            this.buttonResetBothProc.Click += new System.EventHandler(this.buttonResetBothProc_Click);
            // 
            // labelRelayStateControlPage
            // 
            this.labelRelayStateControlPage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelRelayStateControlPage.Location = new System.Drawing.Point(325, 6);
            this.labelRelayStateControlPage.Margin = new System.Windows.Forms.Padding(4);
            this.labelRelayStateControlPage.Name = "labelRelayStateControlPage";
            this.labelRelayStateControlPage.Padding = new System.Windows.Forms.Padding(1);
            this.labelRelayStateControlPage.Size = new System.Drawing.Size(130, 32);
            this.labelRelayStateControlPage.TabIndex = 63;
            this.labelRelayStateControlPage.Text = "Unkown";
            this.labelRelayStateControlPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSendAll
            // 
            this.buttonSendAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.buttonSendAll.Location = new System.Drawing.Point(458, 186);
            this.buttonSendAll.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSendAll.Name = "buttonSendAll";
            this.buttonSendAll.Size = new System.Drawing.Size(147, 49);
            this.buttonSendAll.TabIndex = 70;
            this.buttonSendAll.Text = "Program";
            this.buttonSendAll.UseVisualStyleBackColor = true;
            this.buttonSendAll.Click += new System.EventHandler(this.buttonSendAll_Click);
            // 
            // textBoxSaveStateName
            // 
            this.textBoxSaveStateName.Location = new System.Drawing.Point(4, 69);
            this.textBoxSaveStateName.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSaveStateName.Name = "textBoxSaveStateName";
            this.textBoxSaveStateName.Size = new System.Drawing.Size(140, 21);
            this.textBoxSaveStateName.TabIndex = 69;
            // 
            // buttonTripRelay
            // 
            this.buttonTripRelay.Location = new System.Drawing.Point(305, 51);
            this.buttonTripRelay.Margin = new System.Windows.Forms.Padding(4);
            this.buttonTripRelay.Name = "buttonTripRelay";
            this.buttonTripRelay.Size = new System.Drawing.Size(141, 26);
            this.buttonTripRelay.TabIndex = 65;
            this.buttonTripRelay.Text = "Trip Relay";
            this.buttonTripRelay.UseVisualStyleBackColor = true;
            this.buttonTripRelay.Click += new System.EventHandler(this.buttonTripRelay_Click);
            // 
            // labelRelayDisconnected
            // 
            this.labelRelayDisconnected.AutoSize = true;
            this.labelRelayDisconnected.BackColor = System.Drawing.Color.Red;
            this.labelRelayDisconnected.Location = new System.Drawing.Point(455, 167);
            this.labelRelayDisconnected.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRelayDisconnected.Name = "labelRelayDisconnected";
            this.labelRelayDisconnected.Size = new System.Drawing.Size(116, 15);
            this.labelRelayDisconnected.TabIndex = 49;
            this.labelRelayDisconnected.Text = "Relay Disconnected";
            this.labelRelayDisconnected.Visible = false;
            // 
            // comboBoxSavedStates
            // 
            this.comboBoxSavedStates.FormattingEnabled = true;
            this.comboBoxSavedStates.Location = new System.Drawing.Point(4, 37);
            this.comboBoxSavedStates.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxSavedStates.Name = "comboBoxSavedStates";
            this.comboBoxSavedStates.Size = new System.Drawing.Size(140, 23);
            this.comboBoxSavedStates.TabIndex = 66;
            this.comboBoxSavedStates.DropDown += new System.EventHandler(this.comboBoxSavedStates_DropDown);
            this.comboBoxSavedStates.SelectedIndexChanged += new System.EventHandler(this.comboBoxSavedStates_SelectedIndexChanged);
            this.comboBoxSavedStates.DropDownClosed += new System.EventHandler(this.comboBoxSavedStates_DropDownClosed);
            // 
            // buttonSaveSetting
            // 
            this.buttonSaveSetting.Location = new System.Drawing.Point(151, 67);
            this.buttonSaveSetting.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSaveSetting.Name = "buttonSaveSetting";
            this.buttonSaveSetting.Size = new System.Drawing.Size(109, 26);
            this.buttonSaveSetting.TabIndex = 68;
            this.buttonSaveSetting.Text = "Save Settings";
            this.buttonSaveSetting.UseVisualStyleBackColor = true;
            this.buttonSaveSetting.Click += new System.EventHandler(this.buttonSaveSetting_Click);
            // 
            // buttonDeleteSetting
            // 
            this.buttonDeleteSetting.Location = new System.Drawing.Point(151, 36);
            this.buttonDeleteSetting.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDeleteSetting.Name = "buttonDeleteSetting";
            this.buttonDeleteSetting.Size = new System.Drawing.Size(109, 26);
            this.buttonDeleteSetting.TabIndex = 67;
            this.buttonDeleteSetting.Text = "Delete Setting";
            this.buttonDeleteSetting.UseVisualStyleBackColor = true;
            this.buttonDeleteSetting.Click += new System.EventHandler(this.buttonDeleteSetting_Click);
            // 
            // ucTripMode2
            // 
            this.ucTripMode2.AutoSize = true;
            this.ucTripMode2.BackColor = System.Drawing.Color.Transparent;
            this.ucTripMode2.CTRatio = 320;
            this.ucTripMode2.Customer = RelayControlLibrary.Customers.None;
            this.ucTripMode2.Location = new System.Drawing.Point(-6, 0);
            this.ucTripMode2.Margin = new System.Windows.Forms.Padding(4);
            this.ucTripMode2.Name = "ucTripMode2";
            this.ucTripMode2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ucTripMode2.SequenceRelay = false;
            this.ucTripMode2.Size = new System.Drawing.Size(375, 300);
            this.ucTripMode2.TabIndex = 40;
            this.ucTripMode2.VersionNumber = ((uint)(0u));
            // 
            // ucCloseMode1
            // 
            this.ucCloseMode1.BackColor = System.Drawing.Color.Transparent;
            this.ucCloseMode1.Customer = RelayControlLibrary.Customers.None;
            this.ucCloseMode1.Location = new System.Drawing.Point(368, 0);
            this.ucCloseMode1.Margin = new System.Windows.Forms.Padding(4);
            this.ucCloseMode1.Mode = RelayControlLibrary.CloseModes.None;
            this.ucCloseMode1.Name = "ucCloseMode1";
            this.ucCloseMode1.RelaxClose = false;
            this.ucCloseMode1.RelayRevisionNumber = ((uint)(0u));
            this.ucCloseMode1.Size = new System.Drawing.Size(333, 302);
            this.ucCloseMode1.TabIndex = 26;
            // 
            // ucPumpMode1
            // 
            this.ucPumpMode1.BackColor = System.Drawing.Color.Transparent;
            this.ucPumpMode1.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucPumpMode1.Location = new System.Drawing.Point(218, 300);
            this.ucPumpMode1.Margin = new System.Windows.Forms.Padding(4);
            this.ucPumpMode1.Name = "ucPumpMode1";
            this.ucPumpMode1.PumpProtectEnabled = false;
            this.ucPumpMode1.PumpReason = RelayControlLibrary.PumpReasons.NoPump;
            this.ucPumpMode1.RelayRevisionNumber = ((uint)(0u));
            this.ucPumpMode1.Size = new System.Drawing.Size(297, 311);
            this.ucPumpMode1.TabIndex = 49;
            // 
            // ucCoverFlags1
            // 
            this.ucCoverFlags1.BackColor = System.Drawing.Color.Transparent;
            this.ucCoverFlags1.Location = new System.Drawing.Point(379, 596);
            this.ucCoverFlags1.Margin = new System.Windows.Forms.Padding(4);
            this.ucCoverFlags1.Name = "ucCoverFlags1";
            this.ucCoverFlags1.Size = new System.Drawing.Size(251, 128);
            this.ucCoverFlags1.TabIndex = 119;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageControl);
            this.tabControlMain.Controls.Add(this.tabPageMonitor);
            this.tabControlMain.Controls.Add(this.tabPageFlightRecorder);
            this.tabControlMain.Controls.Add(this.tabPageEvents);
            this.tabControlMain.Controls.Add(this.tabPageTransmitter);
            this.tabControlMain.Controls.Add(this.tabPageTransmitterMonitoring);
            this.tabControlMain.Controls.Add(this.tabPageDNP);
            this.tabControlMain.Controls.Add(this.tabPageArcFault);
            this.tabControlMain.Controls.Add(this.tabPageShortRange);
            this.tabControlMain.Controls.Add(this.tabPageDNPData);
            this.tabControlMain.Controls.Add(this.tabPageDNPSecureAuth);
            this.tabControlMain.Location = new System.Drawing.Point(0, 39);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1457, 938);
            this.tabControlMain.TabIndex = 36;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            // 
            // tabPageDNP
            // 
            this.tabPageDNP.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDNP.Controls.Add(this.buttonResetRelay2);
            this.tabPageDNP.Controls.Add(this.ucDNP1);
            this.tabPageDNP.Location = new System.Drawing.Point(4, 24);
            this.tabPageDNP.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageDNP.Name = "tabPageDNP";
            this.tabPageDNP.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageDNP.Size = new System.Drawing.Size(1449, 910);
            this.tabPageDNP.TabIndex = 9;
            this.tabPageDNP.Text = "DNP";
            // 
            // buttonResetRelay2
            // 
            this.buttonResetRelay2.Enabled = false;
            this.buttonResetRelay2.Location = new System.Drawing.Point(70, 479);
            this.buttonResetRelay2.Margin = new System.Windows.Forms.Padding(4);
            this.buttonResetRelay2.Name = "buttonResetRelay2";
            this.buttonResetRelay2.Size = new System.Drawing.Size(88, 26);
            this.buttonResetRelay2.TabIndex = 1;
            this.buttonResetRelay2.Text = "Reset Relay";
            this.buttonResetRelay2.UseVisualStyleBackColor = true;
            this.buttonResetRelay2.Visible = false;
            this.buttonResetRelay2.Click += new System.EventHandler(this.buttonResetBothProc_Click);
            // 
            // ucDNP1
            // 
            this.ucDNP1.BackColor = System.Drawing.SystemColors.Control;
            this.ucDNP1.Customer = RelayControlLibrary.Customers.NonConEd;
            this.ucDNP1.DNPLabelStatus = false;
            this.ucDNP1.Location = new System.Drawing.Point(10, 4);
            this.ucDNP1.Margin = new System.Windows.Forms.Padding(4);
            this.ucDNP1.Name = "ucDNP1";
            this.ucDNP1.Size = new System.Drawing.Size(1147, 644);
            this.ucDNP1.TabIndex = 0;
            // 
            // tabPageArcFault
            // 
            this.tabPageArcFault.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageArcFault.Controls.Add(this.buttonArcFaultStartMonitoring);
            this.tabPageArcFault.Controls.Add(this.ucArcFault1);
            this.tabPageArcFault.Location = new System.Drawing.Point(4, 24);
            this.tabPageArcFault.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageArcFault.Name = "tabPageArcFault";
            this.tabPageArcFault.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageArcFault.Size = new System.Drawing.Size(1449, 910);
            this.tabPageArcFault.TabIndex = 10;
            this.tabPageArcFault.Text = "Arc Fault";
            // 
            // buttonArcFaultStartMonitoring
            // 
            this.buttonArcFaultStartMonitoring.Location = new System.Drawing.Point(419, 455);
            this.buttonArcFaultStartMonitoring.Margin = new System.Windows.Forms.Padding(4);
            this.buttonArcFaultStartMonitoring.Name = "buttonArcFaultStartMonitoring";
            this.buttonArcFaultStartMonitoring.Size = new System.Drawing.Size(137, 26);
            this.buttonArcFaultStartMonitoring.TabIndex = 4;
            this.buttonArcFaultStartMonitoring.Text = "Start Monitoring";
            this.buttonArcFaultStartMonitoring.UseVisualStyleBackColor = true;
            this.buttonArcFaultStartMonitoring.Click += new System.EventHandler(this.buttonArcFaultStartMonitoring_Click);
            // 
            // ucArcFault1
            // 
            this.ucArcFault1.Location = new System.Drawing.Point(10, 7);
            this.ucArcFault1.Margin = new System.Windows.Forms.Padding(4);
            this.ucArcFault1.Name = "ucArcFault1";
            this.ucArcFault1.Size = new System.Drawing.Size(557, 495);
            this.ucArcFault1.TabIndex = 3;
            // 
            // tabPageShortRange
            // 
            this.tabPageShortRange.Controls.Add(this.ucShortRange1);
            this.tabPageShortRange.Location = new System.Drawing.Point(4, 24);
            this.tabPageShortRange.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageShortRange.Name = "tabPageShortRange";
            this.tabPageShortRange.Size = new System.Drawing.Size(1449, 910);
            this.tabPageShortRange.TabIndex = 11;
            this.tabPageShortRange.Text = "Sec Mon";
            this.tabPageShortRange.UseVisualStyleBackColor = true;
            // 
            // ucShortRange1
            // 
            this.ucShortRange1.BackColor = System.Drawing.SystemColors.Control;
            this.ucShortRange1.Location = new System.Drawing.Point(10, 0);
            this.ucShortRange1.Margin = new System.Windows.Forms.Padding(4);
            this.ucShortRange1.Name = "ucShortRange1";
            this.ucShortRange1.Size = new System.Drawing.Size(1152, 720);
            this.ucShortRange1.TabIndex = 0;
            // 
            // tabPageDNPData
            // 
            this.tabPageDNPData.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDNPData.Controls.Add(this.buttonRequestDNPData);
            this.tabPageDNPData.Location = new System.Drawing.Point(4, 24);
            this.tabPageDNPData.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageDNPData.Name = "tabPageDNPData";
            this.tabPageDNPData.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageDNPData.Size = new System.Drawing.Size(1449, 910);
            this.tabPageDNPData.TabIndex = 12;
            this.tabPageDNPData.Text = "DNP Data";
            // 
            // buttonRequestDNPData
            // 
            this.buttonRequestDNPData.BackColor = System.Drawing.Color.Red;
            this.buttonRequestDNPData.Location = new System.Drawing.Point(1002, 690);
            this.buttonRequestDNPData.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRequestDNPData.Name = "buttonRequestDNPData";
            this.buttonRequestDNPData.Size = new System.Drawing.Size(144, 26);
            this.buttonRequestDNPData.TabIndex = 1;
            this.buttonRequestDNPData.Text = "Request DNP Data";
            this.buttonRequestDNPData.UseVisualStyleBackColor = false;
            this.buttonRequestDNPData.Click += new System.EventHandler(this.buttonRequestDNPData_Click);
            // 
            // tabPageDNPSecureAuth
            // 
            this.tabPageDNPSecureAuth.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDNPSecureAuth.Controls.Add(this.ucDNPSAv51);
            this.tabPageDNPSecureAuth.Location = new System.Drawing.Point(4, 24);
            this.tabPageDNPSecureAuth.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageDNPSecureAuth.Name = "tabPageDNPSecureAuth";
            this.tabPageDNPSecureAuth.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageDNPSecureAuth.Size = new System.Drawing.Size(1449, 910);
            this.tabPageDNPSecureAuth.TabIndex = 13;
            this.tabPageDNPSecureAuth.Text = "DNP SAv5";
            // 
            // ucDNPSAv51
            // 
            this.ucDNPSAv51.BackColor = System.Drawing.SystemColors.Control;
            this.ucDNPSAv51.Location = new System.Drawing.Point(15, 7);
            this.ucDNPSAv51.Margin = new System.Windows.Forms.Padding(4);
            this.ucDNPSAv51.Name = "ucDNPSAv51";
            this.ucDNPSAv51.SerialNumber = 0;
            this.ucDNPSAv51.ShowDNPSAV5Error = true;
            this.ucDNPSAv51.Size = new System.Drawing.Size(1141, 846);
            this.ucDNPSAv51.TabIndex = 0;
            // 
            // tabPageEngineering2
            // 
            this.tabPageEngineering2.Controls.Add(this.commTradeConverter1);
            this.tabPageEngineering2.Controls.Add(this.labelKioskReceived);
            this.tabPageEngineering2.Controls.Add(this.checkBoxSerialCommsDebugging);
            this.tabPageEngineering2.Controls.Add(this.buttonTest);
            this.tabPageEngineering2.Controls.Add(this.ucPhasorRequest1);
            this.tabPageEngineering2.Location = new System.Drawing.Point(4, 22);
            this.tabPageEngineering2.Name = "tabPageEngineering2";
            this.tabPageEngineering2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEngineering2.Size = new System.Drawing.Size(991, 624);
            this.tabPageEngineering2.TabIndex = 14;
            this.tabPageEngineering2.Text = "Engineer 2";
            this.tabPageEngineering2.UseVisualStyleBackColor = true;
            // 
            // commTradeConverter1
            // 
            this.commTradeConverter1.Location = new System.Drawing.Point(27, 180);
            this.commTradeConverter1.Margin = new System.Windows.Forms.Padding(4);
            this.commTradeConverter1.Name = "commTradeConverter1";
            this.commTradeConverter1.Size = new System.Drawing.Size(406, 247);
            this.commTradeConverter1.TabIndex = 4;
            // 
            // labelKioskReceived
            // 
            this.labelKioskReceived.AutoSize = true;
            this.labelKioskReceived.BackColor = System.Drawing.Color.Yellow;
            this.labelKioskReceived.Location = new System.Drawing.Point(293, 76);
            this.labelKioskReceived.Name = "labelKioskReceived";
            this.labelKioskReceived.Size = new System.Drawing.Size(140, 13);
            this.labelKioskReceived.TabIndex = 3;
            this.labelKioskReceived.Text = "Waiting For Kiosk Command";
            // 
            // checkBoxSerialCommsDebugging
            // 
            this.checkBoxSerialCommsDebugging.AutoSize = true;
            this.checkBoxSerialCommsDebugging.Checked = true;
            this.checkBoxSerialCommsDebugging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSerialCommsDebugging.Location = new System.Drawing.Point(399, 15);
            this.checkBoxSerialCommsDebugging.Name = "checkBoxSerialCommsDebugging";
            this.checkBoxSerialCommsDebugging.Size = new System.Drawing.Size(180, 17);
            this.checkBoxSerialCommsDebugging.TabIndex = 2;
            this.checkBoxSerialCommsDebugging.Text = "Enable Serial Comms Debugging";
            this.checkBoxSerialCommsDebugging.UseVisualStyleBackColor = true;
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(293, 15);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(86, 58);
            this.buttonTest.TabIndex = 1;
            this.buttonTest.Text = "Send Test Command";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // ucPhasorRequest1
            // 
            this.ucPhasorRequest1.Location = new System.Drawing.Point(8, 6);
            this.ucPhasorRequest1.Margin = new System.Windows.Forms.Padding(4);
            this.ucPhasorRequest1.Name = "ucPhasorRequest1";
            this.ucPhasorRequest1.Size = new System.Drawing.Size(279, 168);
            this.ucPhasorRequest1.TabIndex = 0;
            // 
            // timerResponseTimeOut
            // 
            this.timerResponseTimeOut.Interval = 5000;
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
            // ucDNPSAv5OSName2
            // 
            this.ucDNPSAv5OSName2.Location = new System.Drawing.Point(0, 0);
            this.ucDNPSAv5OSName2.Margin = new System.Windows.Forms.Padding(4);
            this.ucDNPSAv5OSName2.Name = "ucDNPSAv5OSName2";
            this.ucDNPSAv5OSName2.OSName = "DIGITALGRID, INC. DNP Relay Serial Number: DIGITALGRID, INC. DNP Relay";
            this.ucDNPSAv5OSName2.RequestOSNameClicked = false;
            this.ucDNPSAv5OSName2.Size = new System.Drawing.Size(605, 82);
            this.ucDNPSAv5OSName2.TabIndex = 0;
            // 
            // ucDNPSAv5Settings2
            // 
            this.ucDNPSAv5Settings2.AuthenticationEnabled = false;
            this.ucDNPSAv5Settings2.Location = new System.Drawing.Point(0, 0);
            this.ucDNPSAv5Settings2.Margin = new System.Windows.Forms.Padding(4);
            this.ucDNPSAv5Settings2.Name = "ucDNPSAv5Settings2";
            this.ucDNPSAv5Settings2.Size = new System.Drawing.Size(979, 298);
            this.ucDNPSAv5Settings2.TabIndex = 0;
            // 
            // ucForceCustomerSwitch1
            // 
            this.ucForceCustomerSwitch1.Location = new System.Drawing.Point(467, 11);
            this.ucForceCustomerSwitch1.Margin = new System.Windows.Forms.Padding(4);
            this.ucForceCustomerSwitch1.Name = "ucForceCustomerSwitch1";
            this.ucForceCustomerSwitch1.Size = new System.Drawing.Size(222, 52);
            this.ucForceCustomerSwitch1.TabIndex = 100;
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 19200;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1166, 878);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.menuStrip1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainControl";
            this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring - BETA - 2009-07-24";
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SendAll)).EndInit();
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
            this.tabPageEngineering2.ResumeLayout(false);
            this.tabPageEngineering2.PerformLayout();
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
        private System.Windows.Forms.Button buttonReqLiveData;
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
       // private RelayControlLibrary.ucTransmitterMonitoring ucTransmitterMonitoring2;
        private RelayControlLibrary.ucTransmitter ucTransmitter2;
        private System.Windows.Forms.TabPage tabPageMonitor;
        private System.Windows.Forms.CheckBox checkBoxInTripRegion;
        private System.Windows.Forms.TextBox textBoxTemperatureMonitoringPage;
        private System.Windows.Forms.Label labelTemperatureMonitoringPage;
        private System.Windows.Forms.Button buttonUpdateCTRatio;
        private System.Windows.Forms.Label labelRelayTrippedOrClose;
        private System.Windows.Forms.Button buttonToggleMonitor;
        private System.Windows.Forms.Label labelCtRatioMonitor;
        public SineDisplayGraph.ucPhasorGraph ucPhasorGraph1;
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
        private System.Windows.Forms.Label labelRelayStateControlPage;
        private System.Windows.Forms.Button buttonClearCycleCount;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox textBoxCTRatio;
        private System.Windows.Forms.DomainUpDown domainUpDownCTRatioM;
        private System.Windows.Forms.Button buttonSendCTRatio;
        private System.Windows.Forms.Label labelRelayRevision;
        private System.Windows.Forms.Label labelRevision;
        private System.Windows.Forms.Button buttonTypePhasingRestoreDefaults;
        private System.Windows.Forms.Button buttonRelayType;
        private System.Windows.Forms.DomainUpDown domainUpDownRelayType;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.DomainUpDown domainUpDownPhasings;
        private RelayControlLibrary.ucPumpMode ucPumpMode1;
        private RelayControlLibrary.ucTripMode ucTripMode2;
        //private RelayControlLibrary.SendAll_Message_PopUp SendAll_Message_PopUp1;
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
        private System.Windows.Forms.TabPage tabPageDNP;
        public RelayControlLibrary.ucDNP ucDNP1;
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
        public RelayDNPSecurity.ucDNPSAv5 ucDNPSAv51;
        public RelayDNPSecurity.ucDNPSAv5OSName ucDNPSAv5OSName2;
        public RelayDNPSecurity.ucDNPSAv5Settings ucDNPSAv5Settings2;
        private System.Windows.Forms.Label labelSNPQMonitor;
        private System.Windows.Forms.TextBox textBoxRelaySNControlPQ;
        private System.Windows.Forms.TextBox textBoxCTRatioPQMonitor;
        private System.Windows.Forms.NumericUpDown numericUpDownLowVoltageThres;
        private System.Windows.Forms.Button buttonSendLowVoltageThres;
        private System.Windows.Forms.Button buttonRequestLowVotlageThres;
        private System.Windows.Forms.GroupBox groupBoxLowVoltThres;
        private System.Windows.Forms.GroupBox groupBoxLRLockoutMain;
        private System.Windows.Forms.TextBox textBoxLRLockoutStatusMain;
        private System.Windows.Forms.Label labelLRLockoutMain;
        private RelayControlLibrary.ucTimeControl ucTimeControl1;
        private RelayControlLibrary.ucCoverFlags ucCoverFlags1;
        private System.Windows.Forms.Label labelBootRevision;
        private RelayControlLibrary.ucBlockControl ucBlockControl1;
        private RelayControlLibrary.ucRemoteCommandBlock ucRemoteCommandBlock1;
        private System.Windows.Forms.TabPage tabPageEngineering2;
        private RelayControlLibrary.ucPhasorRequest ucPhasorRequest1;
        private System.Windows.Forms.CheckBox checkBox277DNPOutputs;
        private System.Windows.Forms.ToolStripMenuItem tCPConnectionToolStripMenuItem;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.CheckBox checkBoxSerialCommsDebugging;
        private System.Windows.Forms.Label labelDNPVoltage;
        private System.Windows.Forms.ComboBox comboBoxDNPVoltage;
        private System.Windows.Forms.Label labelKioskReceived;
        private RelayControlLibrary.CommTradeConverter commTradeConverter1;
        private System.ComponentModel.IContainer components;
        public RelayControlLibrary.ucTransmitter ucTransmitter1;
        private System.Windows.Forms.PictureBox pictureBox_SendAll;
        private System.Windows.Forms.Timer timer_SendAll_GIF;
        private System.Windows.Forms.Button button_dataStore;
    }
}
