using GraphicsServer.GSNet.Charting;
using Microsoft.Win32;
using MyFileIO;
using Newtonsoft.Json.Linq;
using NLog;
using PhasorDisplayGraph;
using RelayControlLibrary;
using RelayDNPSecurity;
using SavedSettings;
using SharedResources;
using SineDisplayGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace RelayControl
{ 

    public partial class MainControl : Form
    {
        private const int REV0_MASTER_REVISION = 100713;
        private const int REV1_MASTER_REVISION = 100713; //TEST might not need
        private const int SafeService_MASTER_REVISION = 160621;
        #pragma warning disable CS0414 // field assigned but value never used
        private string customerRevisionName = string.Empty;
        #pragma warning restore CS0414
        private UInt32 relayCodeRevisionNumber;
        private uint externalFileRevisionNumber;                //this will be read from the file to see what revision the program is currently working with.
        private const uint _version4FileRevisionNumber = 20110921;//20110610;            //update only when save data changes
        private const uint _version3FileRevisionNumber = 20100621;

        private RelayStatusRegister RelayStatus = new RelayStatusRegister();
        private RelayFlagsRegister RelayFlags = new RelayFlagsRegister();
        private delegate void booleanInvoke(bool b);
        #pragma warning disable CS0169 // field assigned but value never used
        private bool showCrossPhaseMsgOnce;
        #pragma warning restore CS0169
        //private bool initializeAutoLoad = true;
        private bool showMemFixMsg = true;

        public const string SavedDataPath = @"C:\DGI Systems\Relay\Saved Data\";
        private bool quietMode = false;  //turns off register polling - button for this
#pragma warning disable CS0414 // Field is assigned but its value is never used
        private bool checkedDNPEnable = false;
#pragma warning restore CS0414

        private Customers customer = Customers.None;

        private SyncPhase syncPhase = SyncPhase.Idle;
        private SectionBits requiredSections = SectionBits.None;
        private SectionBits receivedSections = SectionBits.None;

        private bool tCPConnection = false;
        private TCPComms tcpClient;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private bool pendingAutoloadAfterBackup = false;
        private bool skipAutoloadAfterDecline = false;
        private bool pendingRestoreAfterProgramming = false;
        private DateTime? backupStartedAtUtc = null;
        // Backup orchestration flags
        private bool backupInProgress = false;

        private bool backupExpectDnp = false;

        private bool backupGotRelayParams = false;
        private bool backupGotCalibration = false;
        private bool backupGotTx = false;
        private bool backupGotSafeService = false;
        private bool backupGotArcFault = false;

        // Add these fields near the other backup state fields
        private readonly object _backupCaptureLock = new object();
        private readonly List<KeyValuePair<string, byte[]>> _backupCaptureSections = new List<KeyValuePair<string, byte[]>>();
        private string _relayBackupPath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";
        private bool _backupCaptureActive;

#pragma warning disable CS0414 // The field is assigned but its value is never used
        private bool backupGotDnpSav5 = false;
        private bool backupGotDnpData = false;
#pragma warning restore CS0414


        private System.Windows.Forms.Timer backupTimeoutTimer;
        private const int BackupTimeoutMs = 60000; // 30s

        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                if (this.customer == value) return;
                System.Diagnostics.Trace.WriteLine($"[Customer] {this.customer} -> {value}");
                this.customer = value;
                this.ucPumpMode1.Customer = this.customer;
                this.ucTripMode2.Customer = this.customer;
                this.ucCloseMode1.Customer = this.customer;
                this.ucTransmitter1.Customer = this.customer;
                this.ucDNP.Customer = this.customer;
                this.ucForceCustomerSwitch1.Customer = this.customer;
                this.ucEventGraph0.Customer = this.customer;
                this.ucEventGraph1.Customer = this.customer;
                this.ucEventGraph2.Customer = this.customer;
                this.ucEventGraph3.Customer = this.customer;
                this.ucEventGraph4.Customer = this.customer;
                this.ucEventGraph5.Customer = this.customer;
                this.ucEventGraph6.Customer = this.customer;
                this.ucEventGraph7.Customer = this.customer;
                this.ucLiveData1.Customer = this.customer;
                this.ucTransmitterMonitoring1.Customer = this.customer;
                this.ucCalibration2.Customer = this.customer;
                this.ucRelayProgramming1.Customer = this.customer;

                if (this.dNPDIGITALGRIDData != null)
                    this.dNPDIGITALGRIDData.Customer = this.customer;
            }
        }

        private ToolTip toolTip = new ToolTip();
        private bool transmitterEnabled = false;
        private bool TransmitterEnabled
        {
            get { return this.transmitterEnabled; }
            set
            {
                if (value)
                {
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitter))
                        this.tabControlMain.TabPages.Add(this.tabPageTransmitter);

                    if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitterMonitoring))
                        this.tabControlMain.TabPages.Add(this.tabPageTransmitterMonitoring);
                }
                else
                {
                    if (this.tabControlMain.TabPages.Contains(this.tabPageTransmitter))
                        this.tabControlMain.TabPages.Remove(this.tabPageTransmitter);

                    if (this.tabControlMain.TabPages.Contains(this.tabPageTransmitterMonitoring))
                        this.tabControlMain.TabPages.Remove(this.tabPageTransmitterMonitoring);
                }

                this.transmitterEnabled = value;
                this.ucRelayProgramming1.TransmitterEnabled = value;
            }
        }

        private bool dNPEnabledSavedVal = false;
        private bool IsDnpCustomer()
        {
            return DnpCustomerPolicy.IsDnpCommCustomer(this.Customer);
        }

        private bool IsDnpCommSupported()
        {
            return this.masterRevision > REV0_MASTER_REVISION &&
                   IsDnpCustomer() &&
                   RelaySupportsDnp();
        }

        private void EnsureTabPageVisible(TabPage page, TabPage insertBefore = null)
        {
            if (this.tabControlMain.TabPages.Contains(page))
                return;

            if (insertBefore != null && this.tabControlMain.TabPages.Contains(insertBefore))
            {
                this.tabControlMain.TabPages.Insert(this.tabControlMain.TabPages.IndexOf(insertBefore), page);
            }
            else
            {
                this.tabControlMain.TabPages.Add(page);
            }
        }

        private void addDNPTabs()
        {
            bool showDnpTabs = IsDnpCustomer();
            if (!showDnpTabs)
                return;

            EnsureTabPageVisible(this.tabPageDNP, this.tabPageArcFault);
            EnsureTabPageVisible(this.tabPageDNPData, this.tabPageDNPSecureAuth);
            EnsureTabPageVisible(this.tabPageDNPSecureAuth);
        }

        private void DisposeDnpLiveDataControl()
        {
            if (this.dNPDIGITALGRIDData == null)
                return;

            this.dNPDIGITALGRIDData.Send -= standardizedSendData;
            this.dNPDIGITALGRIDData.PointChanged -= DNPDigitalGridData_PointChanged;

            if (this.tabPageDNPData.Controls.Contains(this.dNPDIGITALGRIDData))
                this.tabPageDNPData.Controls.Remove(this.dNPDIGITALGRIDData);

            this.dNPDIGITALGRIDData.Dispose();
            this.dNPDIGITALGRIDData = null;
        }

        private bool DNPEnabled
        {
            get { return this.dNPEnabledSavedVal; }
            set
            {
                bool canUseDnpComm = IsDnpCommSupported();

                // Enable path
                if (value &&
                    canUseDnpComm)
                {
                    addDNPTabs();
                    // Keep existing DNP point wiring behavior
                    setDNPTabPoints();

                    this.dNPEnabledSavedVal = true;
                    this.ucRelayProgramming1.DNPRelay = true;
                    return;
                }

                // Disable path (or unsupported customer / old revision / firmware without DNP)
                removeDNPTabs();

                this.dNPEnabledSavedVal = false;
                this.ucRelayProgramming1.DNPRelay = false;
            }
        }
        private void setDNPTabPoints()
        {
            // IMPORTANT:
            // Host the DNP user control on the tab the user is actually viewing ("DNP Live Data")
            TabPage host = this.tabPageDNPData;

            bool showDnpTabs = this.ucDNP != null && this.ucDNP.ShouldShowDnpTabs();

            if (!showDnpTabs)
            {
                logger.Info("DNP tabs not initialized because customer {0} is not Toronto Hydro.", this.Customer);
                DisposeDnpLiveDataControl();
                removeDNPTabs();
                return;
            }

            if (!IsDnpCommSupported())
            {
                logger.Info("DNP tabs disabled because DNP comm is not supported for customer {0} / relay revision.", this.Customer);
                DisposeDnpLiveDataControl();
                removeDNPTabs();
                this.dNPEnabledSavedVal = false;
                this.ucRelayProgramming1.DNPRelay = false;
                return;
            }

            addDNPTabs();
            DisposeDnpLiveDataControl();

            this.dNPDIGITALGRIDData = new ucDNPDIGITALGRIDData(this.Customer);
            this.dNPDIGITALGRIDData.RelayMasterRevision = (UInt32)masterRevision;
            this.dNPDIGITALGRIDData.Dock = DockStyle.Fill;
            this.dNPDIGITALGRIDData.Send += standardizedSendData;
            this.dNPDIGITALGRIDData.PointChanged += DNPDigitalGridData_PointChanged;

            host.SuspendLayout();
            host.Controls.Clear();

            host.Controls.Add(this.dNPDIGITALGRIDData);
            this.dNPDIGITALGRIDData.Dock = DockStyle.Fill;
            this.dNPDIGITALGRIDData.Visible = true;
            this.dNPDIGITALGRIDData.Enabled = true;
            this.dNPDIGITALGRIDData.BringToFront();

            host.ResumeLayout(true);
            host.PerformLayout();
            host.Refresh();

            System.Diagnostics.Debug.WriteLine(
                $"[DNP TAB] Host={host.Name}, Controls={host.Controls.Count}, " +
                $"HasCtrl={host.Controls.Contains(this.dNPDIGITALGRIDData)}, " +
                $"CtrlVisible={this.dNPDIGITALGRIDData?.Visible}, " +
                $"CtrlSize={this.dNPDIGITALGRIDData?.Size}, " +
                $"CtrlDock={this.dNPDIGITALGRIDData?.Dock}");

            UpdateDnpTabTitle();  // name them..
        }

        private bool gERelay = false;
        private bool GERelay
        {
            get { return this.gERelay; }
            set
            {
                labelGEWH.Text = value ? "GE" : "WH";

                gERelay = value;
                this.ucRelayProgramming1.GERelay = value;
                this.ucTransmitterMonitoring1.GEEnabled = value;
                ucTransmitter1.GERelay = value;
                this.ucLiveData1.GEEnabled = value;
                this.ucEventGraph0.GEEnabled = value;
                this.ucEventGraph1.GEEnabled = value;
                this.ucEventGraph2.GEEnabled = value;
                this.ucEventGraph3.GEEnabled = value;
                this.ucEventGraph4.GEEnabled = value;
                this.ucEventGraph5.GEEnabled = value;
                this.ucEventGraph6.GEEnabled = value;
                this.ucEventGraph7.GEEnabled = value;
            }
        }

        private bool SCITimedOut = false;
        private int savedSaveFileComboBoxWidth;
        public bool relayFound_forDNPdataMonitoring = false;

       

        public MainControl()
        {
            
            InitializeComponent();
            
            tabControlMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlMain.DrawItem += tabControlMain_DrawItem;

            // Get the version number
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            logger.Info("Version number: {0}", version);
            // Officially start everything
            this.MainControlInit();

#if TORONTO_HYDRO
            this.DNPEnabled = true;
            this.ucTransmitter1.DNPEnabled = true;
#endif

            tCPConnectionToolStripMenuItem.Visible = true;

        }


        private void tabControlMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tab = tabControlMain.TabPages[e.Index];
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Background of the selected tab title 
            //using (var backBrush = new SolidBrush(isSelected ? Color.FromArgb(135, 206, 250) : SystemColors.Control)) // selected tab title has a blue background
            using (var backBrush = new SolidBrush(isSelected ? Color.FromArgb(255, 215, 0) : SystemColors.Control)) // selected tab title has a gold colored background
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            // Text color: blue for selected, gray for others (adjust as needed)
            var textColor = isSelected ? Color.Black : SystemColors.ControlText; // Color.Black is the color of the selected tab title
            using (var textBrush = new SolidBrush(textColor))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.DrawString(tab.Text, e.Font, textBrush, e.Bounds, format);
            }

            // Optional focus rectangle
            e.DrawFocusRectangle();
        }
        public class BorderlessGroupBox : GroupBox
        {
            protected override void OnPaint(PaintEventArgs e)
            {
                // Do nothing → prevents default border drawing										
                e.Graphics.Clear(this.BackColor);

                if (!string.IsNullOrEmpty(this.Text))
                {
                    SizeF textSize = e.Graphics.MeasureString(this.Text, this.Font);
                    e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), 0, 0);
                }
            }
        }

        public void MainControlInit()
        {
            try
            {
                initializeDNPVoltageComboBox();
                this.restoreDefaultsTypeAndPhasing();
                this.initializeFromConfigFile();
                AutoReProgramR.AutoReProgramRelay = false;
                AutoReProgramF.AutoReProgramFPGA = false;


                this.ucTransmitter1.checkBoxDNPEnable.Checked = false;
                dnpUplinkK.dnpEnabledWithKit = false;
                applyTX.applyTxSettings = false;
                applyDNP.applyDNPSettings = false;

#if DEBUG
                this.initializeFromConfigFileDebug();
#endif

                this.initializeStatusFlags();
                SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
                this.labelQuietMode.Visible = false;
                this.initializeEventPage();

                if (!Directory.Exists(SavedDataPath)) // Create the Save Data path if it does not already exist
                {
                    Directory.CreateDirectory(SavedDataPath);
                }

                // CT Ratio on PQ monitor
                this.textBoxCTRatioPQMonitor.Visible = true;
                this.textBoxCTRatioPQMonitor.BringToFront();
                this.textBoxCTRatioPQMonitor.Text = textBoxCTRatio.Text;
                this.buttonUpdateCTRatio.Visible = false;

                this.savedSaveFileComboBoxWidth = this.comboBoxSavedStates.Width;
                this.initializeExternalFileRevisionNumber(); // Get the saved data version
                this.initializeSaveObject();                 // Check the save data to see

                tCPConnectionToolStripMenuItem.Visible = true;
                comboBox_RelayType.Visible = true;
                this.groupBoxLowVoltThres.Visible = false;

#if !(DEBUG || ENGINEERING)
                this.tabControlMain.TabPages.Remove(this.tabPageShortRange);
#endif

                // Pre-delivery / locked startup policy currently global
                this.enableAutoloadToolStripMenuItem.Checked = true;

                this.loadConfigurationToolStripMenuItem.Visible = false;

                statusNew.flagFromRelay = false;
                relayHBD.relayWithHBD = false; // considering non H Board relay until revision is received from master

                this.timerLiveEventAcknowledge.Interval = 250;
                this.timerLiveEventAcknowledge.SynchronizingObject = this;
                this.timerLiveEventAcknowledge.Elapsed += new System.Timers.ElapsedEventHandler(timerLiveEventAcknowledge_Tick);

                this.ucRelayProgramming1.AutoloadDeclined += UcRelayProgramming1_AutoloadDeclined;

                this.ucCloseMode1.Send += standardizedSendData;
                this.ucTripMode2.Send += standardizedSendData;
                this.ucCalibration1.Send += standardizedSendData;
                this.ucPumpMode1.Send += standardizedSendData;
                this.ucTransmitter1.Send += new ucTransmitter.SendEventHandler(ucTransmitter1_Send);
                this.ucDNP.Send += new ucDNP.SendEventHandler(ucDNP_Send);
                this.ucShortRange1.Send += standardizedSendData;
                this.ucTimeControl1.SendData += standardizedSendData;
                this.ucSafeService1.Send += standardizedSendData;
              
                this.ucRelayProgramming1.Send += new ucRelayProgramming.SendDelegate(Programming_Send);
                this.ucRelayProgramming1.BackupBeforeProgrammingRequested += UcRelayProgramming1_BackupBeforeProgrammingRequested;
                this.ucGeneralCommandHandler1.Send += standardizedSendData;

                this.ucDNPSAv51.Send += standardizedSendData;
                this.ucCalibration2.Send += new ucCalibration.SendHandler(ucCalibration2_Send);
                this.ucBlockControl1.Send += standardizedSendData;
                this.ucRemoteCommandBlock1.Send += standardizedSendData;
                ucPhasorRequest1.Send += standardizedSendData;

                this.ucCloseMode1.CloseControlException += this.standardExceptionMessage;
                this.ucTripMode2.TripControlException += this.standardExceptionMessage;
                this.ucPumpMode1.PumpControlException += this.standardExceptionMessage;
                this.ucTransmitter1.TransmitterException += this.standardExceptionMessage;
                this.ucShortRange1.ErrorHandler += this.standardExceptionMessage;
                this.ucRelayProgramming1.Error += this.standardExceptionMessage;
                this.ucDNP.DNPControlException += this.standardExceptionMessage;
                this.ucTimeControl1.TimeControlError += standardExceptionMessage;
                this.ucLiveData1.Error += this.standardExceptionMessage;
                this.ucSafeService1.SafeServiceException += this.standardExceptionMessage;
                this.ucCalibration2.CalibrationException += standardExceptionMessage;
                this.ucDNPSAv51.Error += standardExceptionMessage;
                this.ucDNPSAv5OSName2.Send += standardizedSendData;
                this.ucDNPSAv5Settings2.Send += standardizedSendData;
                ucBlockControl1.Error += standardExceptionMessage;
                ucRemoteCommandBlock1.Error += standardExceptionMessage;
                ucPhasorRequest1.Error += standardExceptionMessage;

                this.ucForceCustomerSwitch1.CustomerSwitch += new ucForceCustomerSwitch.CustomerSwitchHanlder(ucForceCustomerSwitch1_CustomerSwitch);
                this.ucLiveData1.PacketHandled += new ucLiveData.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucTransmitter1.CTChanged += new ucTransmitter.CTChangedHandler(ucTransmitter1_CTChanged);
                this.ucTransmitterMonitoring1.MonitoringStateChange += new ucTransmitterMonitoring.MonitoringControlHandler(ucTransmitterMonitoring1_MonitoringStateChange);

                ucRelayProgramming1.RelayTypeChanged += UcRelayProgramming1_RelayTypeChanged;

                this.ucEventGraph0.EventNumber = 0;
                this.ucEventGraph1.EventNumber = 1;
                this.ucEventGraph2.EventNumber = 2;
                this.ucEventGraph3.EventNumber = 3;
                this.ucEventGraph4.EventNumber = 4;
                this.ucEventGraph5.EventNumber = 5;
                this.ucEventGraph6.EventNumber = 6;
                this.ucEventGraph7.EventNumber = 7;

                this.ucEventGraph0.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph1.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph2.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph3.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph4.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph5.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph6.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph7.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);

                this.ucEventGraph0.EventGraphException += this.standardExceptionMessage;
                this.ucEventGraph1.EventGraphException += this.standardExceptionMessage;
                this.ucEventGraph2.EventGraphException += this.standardExceptionMessage;
                this.ucEventGraph3.EventGraphException += this.standardExceptionMessage;
                this.ucEventGraph4.EventGraphException += this.standardExceptionMessage;
                this.ucEventGraph5.EventGraphException += this.standardExceptionMessage;
                this.ucEventGraph6.EventGraphException += this.standardExceptionMessage;
                this.ucEventGraph7.EventGraphException += this.standardExceptionMessage;

                this.ucEventGraph0.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph1.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph2.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph3.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph4.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph5.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph6.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph7.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);

                this.ucEventGraph0.PopulatePhasorGraph += new ucEventGraph.ValuesForPhasorGraph(ucEventGraph_PopulatePhasorGraph);
                this.ucEventGraph1.PopulatePhasorGraph += new ucEventGraph.ValuesForPhasorGraph(ucEventGraph_PopulatePhasorGraph);
                this.ucEventGraph2.PopulatePhasorGraph += new ucEventGraph.ValuesForPhasorGraph(ucEventGraph_PopulatePhasorGraph);
                this.ucEventGraph3.PopulatePhasorGraph += new ucEventGraph.ValuesForPhasorGraph(ucEventGraph_PopulatePhasorGraph);
                this.ucEventGraph4.PopulatePhasorGraph += new ucEventGraph.ValuesForPhasorGraph(ucEventGraph_PopulatePhasorGraph);
                this.ucEventGraph5.PopulatePhasorGraph += new ucEventGraph.ValuesForPhasorGraph(ucEventGraph_PopulatePhasorGraph);
                this.ucEventGraph6.PopulatePhasorGraph += new ucEventGraph.ValuesForPhasorGraph(ucEventGraph_PopulatePhasorGraph);
                this.ucEventGraph7.PopulatePhasorGraph += new ucEventGraph.ValuesForPhasorGraph(ucEventGraph_PopulatePhasorGraph);

                this.ucLiveData1.DownloadComplete += new ucLiveData.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucLiveData1.PopulatePhasorGraph += new ucLiveData.ValuesForPhasorGraph(ucEventGraph_PopulatePhasorGraph);
                this.ucPhasorGraph1.RequestNewCycle += new ucPhasorGraph.RequestNewCycleHandler(ucPhasorGraph1_RequestNewCycle);

                this.radioButtonEvent0.Checked = true;
                this.initializeToolTip();
            }
            catch (Exception ex)
            {
                this.messageHandler("Error In Initialization", ex);
            }

            try
            {
                this.setComPortMenu(this.getPortNames());
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Port Menu ", ex);
            }

            try
            {
                this.timerCheckPortTime.Interval = 500;
                this.eventActionsToolStripMenuItem.Enabled = false;
                this.liveDataActionsToolStripMenuItem.Enabled = false;

#if DEBUG || ENGINEERING
        this.setCustomersRevisionName();
        this.noMonitoringVersion = false;
        this.buttonForceI.Visible = true;
        this.ucCalibration1.Visible = true;
        this.buttonUpdateDisplay.Visible = true;
        this.enableAll(true);
        this.tabPageFlightRecorder.Show();
        this.tabPageEvents.Show();
        this.ArcFaultEnabled = true;
        this.Customer = Customers.ENMAX;
#else
                this.setCustomersRevisionName();
                this.noMonitoringVersion = false;
                this.pauseMonitoring = false;
                this.ucCalibration1.Visible = true;
                this.buttonUpdateDisplay.Visible = false;
                this.enableAll(false);
                this.tabControlMain.TabPages.Remove(this.tabPageEngineering2);
                this.labelCtRatioMonitor.Visible = true;
                this.buttonForceI.Visible = false;
                this.buttonUpdateCTRatio.Visible = false;
                this.buttonRequestRelayRegisters.Visible = false;
                this.buttonResetMaster.Visible = false;
                this.buttonRQRelayProcVersion.Visible = false;
                this.groupBoxRelayFlags.Visible = false;
                this.enableAllToolStripMenuItem.Visible = true;
                this.button_dataStore.Enabled = true;
                this.button_dataStore.Visible = true;
                this.numericUpDown_PC_voltage.Enabled = false;
                this.groupBoxNetworkCTRatio.Location = new System.Drawing.Point(13, 13);
                this.groupBoxNetworkCTRatio.Size = new System.Drawing.Size(410, 356);
                this.tabControlMain.Size = new System.Drawing.Size(1535, 828);
                this.tabPageControl.Size = new System.Drawing.Size(1488, 797);

#if CONED
        this.ucTripMode2.Location = new System.Drawing.Point(472, 7);
#else
                this.ucTripMode2.Location = new System.Drawing.Point(458, 7);
                this.panelPCsettings.Enabled = false;
                this.panelPCsettings.Visible = false;
#endif

                this.ucCloseMode1.Location = new System.Drawing.Point(970, 7);
                this.ucPumpMode1.Location = new System.Drawing.Point(13, 406);
                this.ucSafeService1.Location = new System.Drawing.Point(390, 406);

#if (PSEG || CONED)
                this.grpBox_LightningCount.Enabled = true;
                this.grpBox_LightningCount.Visible = true;
                this.lblLC_Name.Enabled = true;
                this.lblLC_Name.Visible = true;
                this.lbl_LightningCount.Enabled = true;
                this.lbl_LightningCount.Visible = true;
#else
                this.grpBox_LightningCount.Enabled = false;
                this.grpBox_LightningCount.Visible = false;
                this.lblLC_Name.Enabled = false;
                this.lblLC_Name.Visible = false;
                this.lbl_LightningCount.Enabled = false;
                this.lbl_LightningCount.Visible = false;
#endif

#if CONED
        this.buttonSendAll.Location = new System.Drawing.Point(870, 470);
        this.buttonRequestRelayParamaters.Location = new System.Drawing.Point(760, 470);
#else
                this.buttonSendAll.Location = new System.Drawing.Point(1170, 470);
#endif

                this.lbl_Relaystatus_Open.Text = "Open ( OP )";
                this.lbl_Relayststatus_Close.Text = "Close ( CL )";
                this.lbl_Relayststatus_FB.Text = "Floating and Blocked Open ( FB )";
                this.lbl_Relayststatus_Float.Text = "Float ( FL )";
                this.lbl_Relayststatus_backfeed.Text = "Backfeed ( BF )";
                this.lbl_Relayststatus_BO.Text = "Blocked Open ( BO )";
                this.lbl_Relayststatus_FC.Text = "Failed to Close ( FC )";
                this.lbl_Relayststatus_Ib.Text = "Insensitive Backfeed ( IB )";
                this.lbl_Relayststatus_RC.Text = "Relax Close ( RC )";
                this.lbl_Relayststatus_PA.Text = "Pump Alarm ( PA )";
                this.lbl_Relayststatus_SL.Text = "Safe Service Mode Lockout ( SL )";
                this.lbl_Relayststatus_XP.Text = "Cross Phase ( XP )";

                this.button_dataStore.Enabled = false;
                this.button_dataStore.Visible = false;
                this.button_push.Enabled = false;
                this.button_push.Visible = false;

                this.textBoxSaveStateName.Location = new System.Drawing.Point(970, 600);
                this.buttonSaveSetting.Location = new System.Drawing.Point(1150, 600);
                this.comboBoxSavedStates.Location = new System.Drawing.Point(970, 650);
                this.buttonDeleteSetting.Location = new System.Drawing.Point(1150, 700);
                this.btn_LoadProfile.Location = new System.Drawing.Point(1150, 650);

                this.loadConfigurationToolStripMenuItem.Visible = false;
                this.ucTripMode2.buttonRestoreDefaults.Location = new System.Drawing.Point(100, 317);
                this.ucTripMode2.buttonSendTripData.Location = new System.Drawing.Point(253, 317);
                this.ucTripMode2.checkBoxTripOnPowerDown.Location = new System.Drawing.Point(15, 280);
                this.ucTripMode2.checkBoxEnableGullWing.Location = new System.Drawing.Point(15, 250);

                this.ucPumpMode1.labelPumpType.Enabled = false;
                this.ucPumpMode1.labelPumpType.Visible = false;
                this.ucPumpMode1.labelPumpTypeDisplay.Enabled = false;
                this.ucPumpMode1.labelPumpTypeDisplay.Visible = false;
                this.ucPumpMode1.labelEnable.Location = new System.Drawing.Point(285, 10);

#if BGE
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - Baltimore Gas & Electric";
                this.Customer = Customers.BGE;
#elif COMED
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - Commonwealth Edison";
                this.Customer = Customers.COMED;
#elif CONED
                this.Text = "DIGITALGRID, INC. - ALWAYS ON - 10.0.10.0 - Consolidated Edison";
                this.Customer = Customers.CONED;
#elif DOMINION
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - Dominion Energy";
                this.Customer = Customers.DOMINION;
#elif ENMAX
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - ENMAX";
                this.Customer = Customers.ENMAX;
#elif EVERSOURCE
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - EVERSOURCE";
                this.Customer = Customers.EVERSOURCE;
#elif LONDON_HYDRO
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - London Hydro";
                this.Customer = Customers.LONDON_HYDRO;
#elif ONCOR
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - ONCOR";
                this.Customer = Customers.ONCOR;
#elif PSEG
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - PSE&G";
                this.Customer = Customers.PSEG;
#elif SCE
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - Southern California Edison";
                this.Customer = Customers.SCE;
#elif SCL
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - Seattle City Lights";
                this.Customer = Customers.SCL;
#elif TAUNTON
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - Taunton Municipal Lighting Plant";
                this.Customer = Customers.TAUNTON;
#elif TORONTO_HYDRO
                this.Text = "DIGITALGRID, INC. - NWP Master Relay Software - 10.0.10.0 - TORONTO HYDRO";
                this.Customer = Customers.TORONTO_HYDRO;
#else
                this.Text = "DIGITALGRID, INC. - ";
                this.Customer = Customers.None;
#endif

                this.acknowledgeToolStripMenuItem1.Visible = false;
                this.checkBoxBlockedCloseFlag.Visible = false;
                this.checkBoxCalibrating.Visible = false;
                this.checkBoxInInsensRegion.Visible = false;
                this.labelConEdPowerRelay.Visible = false;

#if CONED
                this.groupBox_PC.Enabled = true;
                this.groupBox_PC.Visible = true;
                this.btn_PermCl_Active.Enabled = true;
                this.btn_PermCl_Active.Visible = true;

                // Keep ConEd layout specifics
                ucRemoteCommandBlock1.Visible = true;
                this.ucRemoteCommandBlock1.Visible = true;
                this.ucCloseMode1.Location = new System.Drawing.Point(1000, 7);
                this.groupBox_PC.Location = new System.Drawing.Point(1004, 406);
                this.groupBox_PC.Size = new System.Drawing.Size(470, 360);
                this.panelPCsettings.Location = new System.Drawing.Point(1000, 402);
                this.panelPCsettings.Size = new System.Drawing.Size(477, 366);

                this.btn_RestorePC_defaults.Location = new System.Drawing.Point(80, 295);
                this.btn_PC_Send.Location = new System.Drawing.Point(280, 295);

                this.buttonRequestRelayParamaters.Text = "Read";
                this.buttonSendAll.Text = "Program";

                this.buttonSaveSetting.Location = new System.Drawing.Point(780, 600);
                this.textBoxSaveStateName.Location = new System.Drawing.Point(780, 630);
                this.comboBoxSavedStates.Location = new System.Drawing.Point(780, 660);
                this.btn_LoadProfile.Location = new System.Drawing.Point(780, 690);
                this.buttonDeleteSetting.Location = new System.Drawing.Point(780, 720);

                this.btn_LoadProfile.Width = this.buttonSaveSetting.Width;
                this.buttonDeleteSetting.Width = this.buttonSaveSetting.Width;
                this.comboBoxSavedStates.Width = this.buttonSaveSetting.Width;
#else
                this.groupBox_PC.Enabled = false;
                this.groupBox_PC.Visible = false;
                this.btn_PermCl_Active.Enabled = false;
                this.btn_PermCl_Active.Visible = false;
#endif

#if TORONTO_HYDRO
                this.ucTransmitter1.checkBoxDNPEnable.Enabled = false;
                this.ucTransmitter1.checkBoxDNPEnable.Visible = false;
#elif (CONED || PSEG || ENMAX || ONCOR || SCE || EVERSOURCE)
                this.ucTransmitter1.checkBoxDNPEnable.Enabled = true;
                this.ucTransmitter1.checkBoxDNPEnable.Visible = true;
#else
                this.ucTransmitter1.checkBoxDNPEnable.Enabled = false;
                this.ucTransmitter1.checkBoxDNPEnable.Visible = false;
#endif
                this.ucTransmitterMonitoring1.groupBoxAnalog1.Location = new System.Drawing.Point(1150, 250);
                this.ucTransmitterMonitoring1.groupBoxAnalog2.Location = new System.Drawing.Point(1150, 500);
                this.ucTransmitterMonitoring1.groupBoxAnalogFlagValues.Location = new System.Drawing.Point(710, 400);
                this.ucTransmitterMonitoring1.groupBoxFlagStatus.Location = new System.Drawing.Point(710, 80);
                this.ucTransmitterMonitoring1.textBoxTransmitterTemp.Location = new System.Drawing.Point(250, 497);
                this.ucTransmitterMonitoring1.textBoxQBit.Location = new System.Drawing.Point(250, 420);
                this.ucTransmitterMonitoring1.lblTEMP.Location = new System.Drawing.Point(85, 500);
                this.ucTransmitterMonitoring1.labelQPres.Location = new System.Drawing.Point(118, 422);
                this.ucTransmitterMonitoring1.groupBoxGeneralSettings.Location = new System.Drawing.Point(42, 30);
                this.ucTransmitterMonitoring1.groupBoxGeneralSettings.Size = new System.Drawing.Size(420, 725);
                this.ucTransmitterMonitoring1.textBoxTransmitterSN.Location = new System.Drawing.Point(250, 56);
                this.ucTransmitterMonitoring1.textBoxTransmitterID.Location = new System.Drawing.Point(250, 150);
                this.ucTransmitterMonitoring1.panel_GenSet_sensorMon.Location = new System.Drawing.Point(40, 28);
                this.ucTransmitterMonitoring1.panel_GenSet_sensorMon.Size = new System.Drawing.Size(425, 730);
                this.ucTransmitterMonitoring1.panel_command_senorMon.Location = new System.Drawing.Point(1140, 27);
                this.ucTransmitterMonitoring1.panel_command_senorMon.Size = new System.Drawing.Size(365, 730);
                this.ucTransmitterMonitoring1.panel_read_sensorMon.Location = new System.Drawing.Point(660, 27);
                this.ucTransmitterMonitoring1.panel_read_sensorMon.Size = new System.Drawing.Size(300, 730);
                this.ucTransmitterMonitoring1.textBoxCTMult.Location = new System.Drawing.Point(250, 330);
                this.TransmitterEnabled = true;
                this.ArcFaultEnabled = false;

#if DNP
                this.DNPEnabled = true;
#else
                this.DNPEnabled = false;
#endif

                checkBox277DNPOutputs.Visible = false;
                checkBox277DNPOutputs.Enabled = false;
                checkBox277DNPOutputs.TabStop = false;
                checkBox277DNPOutputs.Checked = true;

                this.enableAllToolStripMenuItem.Visible = true;
#endif // DEBUG || ENGINEERING

                UpdateDnpCommStatusFromRelayState(this.DNPEnabled);

                // Set the Title / Caption of groupBoxes bold; child controls regular
                groupBox_RelayInfo.Font = new Font(groupBox_RelayInfo.Font, FontStyle.Bold);
                foreach (Control child in groupBox_RelayInfo.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                groupBoxRelayStatus.Font = new Font(groupBoxRelayStatus.Font, FontStyle.Bold);
                foreach (Control child in groupBoxRelayStatus.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                grpBox_RelayCommands.Font = new Font(grpBox_RelayCommands.Font, FontStyle.Bold);
                foreach (Control child in grpBox_RelayCommands.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                groupBox_FirmwareInfo.Font = new Font(groupBox_FirmwareInfo.Font, FontStyle.Bold);
                foreach (Control child in groupBox_FirmwareInfo.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucTripMode2.groupBoxTripModeSettings.Font = new Font(this.ucTripMode2.groupBoxTripModeSettings.Font, FontStyle.Bold);
                foreach (Control child in this.ucTripMode2.groupBoxTripModeSettings.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucCloseMode1.groupBoxCloseMode.Font = new Font(this.ucCloseMode1.groupBoxCloseMode.Font, FontStyle.Bold);
                foreach (Control child in this.ucCloseMode1.groupBoxCloseMode.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                groupBoxNetworkCTRatio.Font = new Font(groupBoxNetworkCTRatio.Font, FontStyle.Bold);
                foreach (Control child in groupBoxNetworkCTRatio.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                groupBox_PC.Font = new Font(groupBox_PC.Font, FontStyle.Bold);
                foreach (Control child in groupBox_PC.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucPumpMode1.groupBoxPumpMode.Font = new Font(this.ucPumpMode1.groupBoxPumpMode.Font, FontStyle.Bold);
                foreach (Control child in this.ucPumpMode1.groupBoxPumpMode.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucSafeService1.groupBoxSafeService.Font = new Font(this.ucSafeService1.groupBoxSafeService.Font, FontStyle.Bold);
                foreach (Control child in this.ucSafeService1.groupBoxSafeService.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucTransmitterMonitoring1.groupBoxGeneralSettings.Font = new Font(this.ucTransmitterMonitoring1.groupBoxGeneralSettings.Font, FontStyle.Bold);
                foreach (Control child in this.ucTransmitterMonitoring1.groupBoxGeneralSettings.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucTransmitterMonitoring1.groupBoxCurrentReadings.Font = new Font(this.ucTransmitterMonitoring1.groupBoxCurrentReadings.Font, FontStyle.Bold);
                foreach (Control child in this.ucTransmitterMonitoring1.groupBoxCurrentReadings.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucTransmitterMonitoring1.groupBoxVaultMonitoringCommands.Font = new Font(this.ucTransmitterMonitoring1.groupBoxVaultMonitoringCommands.Font, FontStyle.Bold);
                foreach (Control child in this.ucTransmitterMonitoring1.groupBoxVaultMonitoringCommands.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucTransmitterMonitoring1.groupBoxAnalog2.Font = new Font(this.ucTransmitterMonitoring1.groupBoxAnalog2.Font, FontStyle.Bold);
                foreach (Control child in this.ucTransmitterMonitoring1.groupBoxAnalog2.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucTransmitterMonitoring1.groupBoxAnalog1.Font = new Font(this.ucTransmitterMonitoring1.groupBoxAnalog1.Font, FontStyle.Bold);
                foreach (Control child in this.ucTransmitterMonitoring1.groupBoxAnalog1.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucTransmitterMonitoring1.groupBoxAnalogFlagValues.Font = new Font(this.ucTransmitterMonitoring1.groupBoxAnalogFlagValues.Font, FontStyle.Bold);
                foreach (Control child in this.ucTransmitterMonitoring1.groupBoxAnalogFlagValues.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucTransmitterMonitoring1.label2.Font = new Font(this.ucTransmitterMonitoring1.label2.Font, FontStyle.Regular);
                this.ucTransmitterMonitoring1.textBox_Input7.Font = new Font(this.ucTransmitterMonitoring1.textBox_Input7.Font, FontStyle.Regular);

                this.ucTransmitterMonitoring1.groupBoxFlagStatus.Font = new Font(this.ucTransmitterMonitoring1.groupBoxFlagStatus.Font, FontStyle.Bold);
                foreach (Control child in this.ucTransmitterMonitoring1.groupBoxFlagStatus.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucTransmitter1.grpBox_TXcommands.Font = new Font(this.ucTransmitter1.grpBox_TXcommands.Font, FontStyle.Bold);
                foreach (Control child in this.ucTransmitter1.grpBox_TXcommands.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucTransmitter1.grpBx_DNPSettings.Font = new Font(this.ucTransmitter1.grpBx_DNPSettings.Font, FontStyle.Bold);
                foreach (Control child in this.ucTransmitter1.grpBx_DNPSettings.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucDNP.groupBoxDNPSettings.Font = new Font(this.ucDNP.groupBoxDNPSettings.Font, FontStyle.Bold);
                foreach (Control child in this.ucDNP.groupBoxDNPSettings.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                this.ucPhasorGraph1.groupBoxTHD.Font = new Font(this.ucPhasorGraph1.groupBoxTHD.Font, FontStyle.Bold);
                foreach (Control child in this.ucPhasorGraph1.groupBoxTHD.Controls) child.Font = new Font(child.Font, FontStyle.Regular);

                // remove default blue highlight behavior
                comboBoxDNPVoltage.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox_RelayType.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox_Phasings.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox_PC.DropDownStyle = ComboBoxStyle.DropDownList;
                this.ucSafeService1.comboBox_DataViews.DropDownStyle = ComboBoxStyle.DropDownList;
                this.ucTripMode2.comboBox_TripStyle.DropDownStyle = ComboBoxStyle.DropDownList;
                this.ucTripMode2.comboBox_TripType.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                this.messageHandler("Error in Timer and Release Visibility", ex);
            }

            try
            {
                if (!this.noMonitoringVersion)
                    this.checkSavedLocationAndFindRelay();
            }
            catch (Exception ex)
            {
                this.messageHandler(ex.Message, ex.InnerException);
            }

            // startup backup only once, after relay-ready startup path
            // MainControl must not re-authorize or re-decline the relay programming lifecycle.
            // ucRelayProgramming owns the firmware state machine and auto-load decision path.
            if (!this.noMonitoringVersion)
            {
                // Intentionally no direct lifecycle enforcement here.
                // Relay startup backup / autoload is handled by ucRelayProgramming.
                logger.Info("MainControl startup: relay programming lifecycle delegated to ucRelayProgramming.");
            }
        }

        private void UcRelayProgramming1_RelayTypeChanged(object sender, RelayTypeChangedEventArgs e)
        {
            GERelay = e.GERelay;
        }

        private void initializeDNPVoltageComboBox()
        {
            comboBoxDNPVoltage.Items.Clear();
            comboBoxDNPVoltage.DisplayMember = "Name";
            comboBoxDNPVoltage.ValueMember = "Value";
            comboBoxDNPVoltage.DataSource = ProtectorVoltages.Voltages;
        }

        private void standardExceptionMessage(object o, ExceptionEventArgs eEA)
        {
            this.messageHandler(eEA.Title, eEA.InnerException);
        }

        private void UpdateDnpTabTitle()
        {
            bool isUplinkCustomer =
                this.Customer == Customers.ONCOR ||
                this.Customer == Customers.CONED ||
                this.Customer == Customers.EVERSOURCE ||
                this.Customer == Customers.PSEG ||
                this.Customer == Customers.ENMAX ||
                this.Customer == Customers.SCE;

            this.tabPageDNP.Text = isUplinkCustomer ? "DNP DB Settings" : "DNP Comm Settings";
        }

        private void setCustomersRevisionName()
        {

#if EVERSOURCE
            this.customerRevisionName = "EVERSOURCE";
#elif BGE
            customerRevisionName = "BGE";
#elif SCL
            this.customerRevisionName = "SCL";
#elif DOMINION
            this.customerRevisionName = "DOMINION";
#elif COMED
            this.customerRevisionName = "COMED";
#elif LONDON_HYDRO
            this.customerRevisionName = "LONDON_HYDRO";
#elif TAUNTON
            this.customerRevisionName = "TAUNTON";
#elif ENMAX
            this.customerRevisionName = "ENMAX";
#elif ONCOR
            this.customerRevisionName = "ONCOR";
#elif TORONTO_HYDRO
            this.customerRevisionName = "TORONTO_HYDRO";
#elif CONED
            this.customerRevisionName = "CONED";
#elif SCE
            this.customerRevisionName = "SCE";
#elif PSEG
            this.customerRevisionName = "PSEG";
#else
            this.customerRevisionName = "DIGITALGRID, INC";
#endif
        }

        private void initializeToolTip()
        {
            this.toolTip.SetToolTip(this.comboBoxSavedStates, "Recall previously saved states");
            this.toolTip.SetToolTip(this.comboBox_CTRatio, "Set to match the CT Ratio of the protector");
            this.toolTip.SetToolTip(this.comboBox_Phasings, "How to determine phasing of the protector");
            // this.toolTip.SetToolTip(this.domainUpDownRelayType, "Changes Relay Algorithm");
            this.toolTip.SetToolTip(this.comboBox_RelayType, "Changes Relay Algorithm");
            this.toolTip.SetToolTip(this.textBoxCTRatio, "Select 'Special' in above box to manually enter CT Ratio");
            this.toolTip.SetToolTip(this.textBoxSaveStateName, "Enter name to save current settings to file");
            this.toolTip.SetToolTip(this.comboBoxDNPVoltage, "Scales values on PQ Mon tab to protector voltage - now saved in relay, applies to DNP input values");
            this.toolTip.SetToolTip(this.checkBox277DNPOutputs, "Configures DNP outputs to scale to 277V");
            this.toolTip.SetToolTip(this.buttonClearCycleCount, "Reset Cycle Count to Zero");
            this.toolTip.SetToolTip(this.buttonDeleteSetting, "Remove the currently selected Saved State from the save file");
            this.toolTip.SetToolTip(this.buttonRequestRelayParamaters, "Download All Parameters to GUI");
            // this.toolTip.SetToolTip(this.buttonResetBothProc, "Reset the Relay");
            this.toolTip.SetToolTip(this.buttonRSTRelay, "Reset the Relay");
            this.toolTip.SetToolTip(this.buttonSaveSetting, "Save the Current Settings to the file under the name in the Save Setting box");
            this.toolTip.SetToolTip(this.buttonSendAll, "Upload all visible settings to the relay");
            this.toolTip.SetToolTip(this.buttonTripRelay, "Send a Remote Trip to the relay");
            this.toolTip.SetToolTip(this.btn_ClearPumpProtect, "Clears any active Pump Protect state");
            this.toolTip.SetToolTip(this.btn_RelaxClose, "Temporarily sets ReClose Voltage to 0.1V");
        }

        #region Relay Flags/Status

        private List<string> relayStatus1 = new List<string>();
        private List<string> relayStatus2 = new List<string>();
        private List<string> relayFlags1 = new List<string>();
        private List<string> relayFlags2 = new List<string>();
        private List<string> commFlags1 = new List<string>();
        private List<string> commFlags2 = new List<string>();
        private List<string> GEControl1 = new List<string>();
        private List<string> GEControl2 = new List<string>();

        private void initializeStatusFlags()
        {
            try
            {
                this.relayStatus1.Clear();
                this.relayStatus2.Clear();
                this.relayFlags1.Clear();
                this.relayFlags2.Clear();
                this.commFlags1.Clear();
                this.commFlags2.Clear();
                this.GEControl1.Clear();
                this.GEControl2.Clear();

                this.relayStatus1.Add("Command Lockout");
                this.relayStatus1.Add("BFlag Not Inv");
                this.relayStatus1.Add("Do Not Flash");
                this.relayStatus1.Add("All Params Received");
                this.relayStatus1.Add("Relax From Master");
                this.relayStatus1.Add("Pump Reason");
                this.relayStatus1.Add("Pump Reason");
                this.relayStatus1.Add("Pump Reason");
                this.uc8CheckBoxFlagsRelayStatus1.Names = this.relayStatus1;

                this.relayStatus2.Add("Trip Flag");
                this.relayStatus2.Add("Master Reset");
                this.relayStatus2.Add("Tripping");
                this.relayStatus2.Add("Float");
                this.relayStatus2.Add("Debug 3");
                this.relayStatus2.Add("Blocked Open");
                this.relayStatus2.Add("Phasing OK");
                this.relayStatus2.Add("Calibration Mode");
                this.uc8CheckBoxFlagsRelayStatus2.Names = this.relayStatus2;

                this.relayFlags1.Add("Pumping");
                this.relayFlags1.Add("Math Over Time");
                this.relayFlags1.Add("Safe Service Enabled");
                this.relayFlags1.Add("Default Values");
                this.relayFlags1.Add("Math Error");
                this.relayFlags1.Add("Monitor Phasors");
                this.relayFlags1.Add("Init Complete");
                this.relayFlags1.Add("Sequence Relay");
                this.uc8CheckBoxFlagsRelayFlags1.Names = this.relayFlags1;

                this.relayFlags2.Add("Phased ACB");
                this.relayFlags2.Add("Master Ready For Events");
                this.relayFlags2.Add("Voltage Checked");
                this.relayFlags2.Add("Relax Close Attempt");
                this.relayFlags2.Add("Flash Compare");
                this.relayFlags2.Add("B Flag");
                this.relayFlags2.Add("In Insensitive Region");
                this.relayFlags2.Add("In Trip Region");
                this.uc8CheckBoxFlagsRelayFlags2.Names = this.relayFlags2;

                this.commFlags1.Add("Trip");
                this.commFlags1.Add("Close");
                this.commFlags1.Add("Pumping");
                this.commFlags1.Add("Blocked Open");
                this.commFlags1.Add("Insensitive Backfeed");
                this.commFlags1.Add("Relax Close");
                this.commFlags1.Add("B Flag");
                this.commFlags1.Add("Flash Error");
                this.uc8CheckBoxFlagsCommFlags1.Names = this.commFlags1;

                this.commFlags2.Add("High Voltage");
                this.commFlags2.Add("Low Voltage Event");
                this.commFlags2.Add("Arc Flash Detected");
                this.commFlags2.Add("Arc Fault Detected");
                this.commFlags2.Add("Arc Fault Detected");
                this.commFlags2.Add("SEC Active");
                this.commFlags2.Add("Bad Close Curve");
                this.commFlags2.Add("Bad Trip Curve");
                this.uc8CheckBoxFlagsCommFlags2.Names = this.commFlags2;


                this.GEControl1.Add("Enabled");
                this.GEControl1.Add("VTCalibrated");
                this.GEControl1.Add("High Voltage");
                this.GEControl1.Add("BFlag Seen Open");
                this.GEControl1.Add("BFlag Seen Closed");
                this.GEControl1.Add("Common Phased");
                this.GEControl1.Add("Do Not Close");
                this.GEControl1.Add("Transitioning");
                this.uc8CheckBoxFlagsGEControl1.Names = this.GEControl1;

                this.GEControl2.Add("State Closed");
                this.GEControl2.Add("State Unkown");
                this.GEControl2.Add("State Opened");
                this.GEControl2.Add("Obv Close");
                this.GEControl2.Add("Obv Open");
                this.GEControl2.Add("Low Differential Voltage");
                this.GEControl2.Add("NA");
                this.GEControl2.Add("NA");
                this.uc8CheckBoxFlagsGEControl2.Names = this.GEControl2;
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Initializing Flags/Status Checkboxes", ex);
            }


        }
        #endregion

        void standardizedSendData(object o, SendEventArgs sEA)
        {
            if (!this.relayFound)
                return;

            string packetText = BitConverter.ToString(sEA.SendPacket.ToArray());
            string sourceText = o?.ToString() ?? "unknown";

 
            logger.Trace("Sending Data to Relay: {0}", packetText);

            if (sEA.WithAck)
            {
                if (!this.sendAll)
                {
                    this.sendPacketAck(sEA.SendPacket, sourceText);
                    //if (sEA.RequestAll)
                    //{
                        //this.requestAllData("standardizedSendData");
                        //this.parametersLoaded = true;
                    //}
                }
                else
                {
                    this.sendPacket(sEA.SendPacket);
                }
            }
            else
            {
                this.sendPacket(sEA.SendPacket);
            }
        }

        void ucForceCustomerSwitch1_CustomerSwitch(object sender, CustomerSwitchEventArgs cSEA)
        {
            this.Customer = cSEA.Customer;
        }

        private void makeConEdisonGUI()
        {
            if (this.Text.Contains(" - Memphis"))
                this.Text.Remove(this.Text.IndexOf(" - Memphis"));

            this.removeDNPTabs();
            this.tabControlMain.TabPages.Remove(this.tabPageTransmitter);
            this.tabControlMain.TabPages.Remove(this.tabPageTransmitterMonitoring);
            this.comboBox_Phasings.Visible = false;
            this.labelConEdPowerRelay.Visible = true;
            this.comboBox_RelayType.Visible = false;
            this.buttonTypePhasingRestoreDefaults.Visible = false;
            //this.buttonRelayType.Visible = false;
            this.removePumpProtect();

            this.checkBoxInTripRegion.Visible = false;
            this.serialPort1.Close();
            this.serialPort1.BaudRate = 19200;
            this.serialPort1.Open();
        }

        private void makeNonConEdGUI()
        {
            if (this.Text.Contains(" - Memphis"))
                this.Text.Remove(this.Text.IndexOf(" - Memphis"));

            this.comboBox_RelayType.Visible = false;
            this.comboBox_Phasings.Visible = false;
            this.labelConEdPowerRelay.Visible = false;
            this.buttonTypePhasingRestoreDefaults.Visible = true;
            // this.buttonRelayType.Visible = true;
            this.addPumpProtect();

            this.checkBoxInTripRegion.Visible = true;


        }

        #pragma warning disable CS0649 // never assigned; remains null in some build configs
        private ucDNPDIGITALGRIDData dNPDIGITALGRIDData;
        #pragma warning restore CS0649

        private bool otherPanelMovedForConEd = false;
        private void removePumpProtect()
        {
            if (this.ucPumpMode1.Visible || !this.ucPumpMode1.Enabled)
            {
                this.ucPumpMode1.Visible = false;
               // Point tempPoint = this.panelOtherRelayControls.Location;
               // tempPoint.X -= this.ucPumpMode1.Width;
               // this.panelOtherRelayControls.Location = tempPoint;
                this.otherPanelMovedForConEd = true;
            }
        }

        private void addPumpProtect()
        {
            if (!this.ucPumpMode1.Visible && this.otherPanelMovedForConEd)
            {
                this.otherPanelMovedForConEd = false;
               // Point tempPoint = this.panelOtherRelayControls.Location;
               // tempPoint.X += this.ucPumpMode1.Width;
               // this.panelOtherRelayControls.Location = tempPoint;
                this.ucPumpMode1.Visible = true;
            }
        }

        

        void ucTransmitterMonitoring1_MonitoringStateChange(object sender, TransmitterMonitoringEventArgs tMEA)
        {
            if (tMEA.EnableTransmitting)
                this.transmitterMonitoring = true;
            else
                this.transmitterMonitoring = false;
        }

        void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
            {
                Application.Exit();
            }
        }

        private void initializeExternalFileRevisionNumber()
        {
            ExternalFileRevisionNumber eFRN = new ExternalFileRevisionNumber();
            Stream stream;
            BinaryFormatter formatter;
            bool updateFail = false;

            if (!File.Exists(SavedDataPath + "rev.bin"))     //Rev does not exist, original software
            {
                this.externalFileRevisionNumber = 0;        //set revision number to zero
            }
            else                                            //rev does exist
            {
                stream = File.Open(SavedDataPath + "rev.bin", FileMode.Open);
                formatter = new BinaryFormatter();          //

                if (stream.Length != 0)
                    eFRN = (ExternalFileRevisionNumber)formatter.Deserialize(stream);

                this.externalFileRevisionNumber = eFRN.RevisionNumber;      //get the revision number

                stream.Close();
            }

            try
            {
                this.updateSaveFiles();
            }
            catch
            {
                updateFail = true;
                throw new Exception("Error updating to new file version");
            }
            //After all work has been done successfully, save the most current revision number
            if (!updateFail)
            {
                stream = File.Open(SavedDataPath + "rev.bin", FileMode.OpenOrCreate);
                formatter = new BinaryFormatter();

                eFRN.RevisionNumber = _version4FileRevisionNumber;

                formatter.Serialize(stream, eFRN);
                stream.Close();
            }
        }

        private void updateSaveFiles()
        {
            if (this.externalFileRevisionNumber == _version4FileRevisionNumber) //Files are current version
            {
                return;
            }

            if (externalFileRevisionNumber == 0)                             //before revision introduced
            {
                SavedSettingsv2 sSs2;
                SavedSettingV4 sSV4 = new SavedSettingV4();
                Stream stream;

                try
                {
                    string backUpFileSavePath = @"C:\DGI Systems\Relay\Saved Data\SavedSettingsBackUp" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".dgi";
                    if (File.Exists(@"C:\DGI Systems\Relay\SavedSettings.dgi"))
                    {
                        File.Copy(@"C:\DGI Systems\Relay\SavedSettings.dgi", backUpFileSavePath);  //backup the file before conversion
                        stream = File.Open(@"C:\DGI Systems\Relay\SavedSettings.dgi", FileMode.Open);
                    }
                    else if (File.Exists(@"C:\DGI Systems\Relay\Saved Data\SavedSettings.dgi"))
                    {
                        File.Copy(@"C:\DGI Systems\Relay\Saved Data\SavedSettings.dgi", backUpFileSavePath);  //backup the file before conversion
                        stream = File.Open(@"C:\DGI Systems\Relay\Saved Data\SavedSettings.dgi", FileMode.Open);
                    }
                    else
                        throw new Exception();
                }
                catch
                {
                    this.writeSaveObjectToFile();
                    return;
                }

                try
                {
                    BinaryFormatter bF = new BinaryFormatter();

                    sSs2 = (SavedSettingsv2)bF.Deserialize(stream);

                    for (int i = 0; i < sSs2.Settings.Count; ++i)
                    {
                        SavedSettingv2 sSV2 = sSs2.Settings[i];

                        sSV4 = new SavedSettingV4();

                        sSV4.Name = sSV2.Name;
                        sSV4.CTRatio = sSV2.CTRatio;
                        sSV4.Phasing = sSV2.Phasing;

                        sSV4.CloseSettings.CircleCloseEnabled = sSV2.CloseSettings.CircleCloseEnabled;
                        sSV4.CloseSettings.CloseDelay = sSV2.CloseSettings.CloseDelay;
                        sSV4.CloseSettings.Name = sSV2.CloseSettings.Name;
                        sSV4.CloseSettings.PhaseDetectionAngle = sSV2.CloseSettings.PhaseDetectionAngle;
                        sSV4.CloseSettings.PhaseDetectionOffset = sSV2.CloseSettings.PhaseDetectionOffset;
                        sSV4.CloseSettings.RecloseVolts = sSV2.CloseSettings.RecloseVolts;
                        sSV4.CloseSettings.TiltAngle = sSV2.CloseSettings.TiltAngle;
                        sSV4.CloseSettings.BlockedOverride = false;


                        sSV4.PumpSettings.CycleLimit = sSV2.PumpSettings.CycleLimit;
                        sSV4.PumpSettings.EnableRelayCycle = sSV2.PumpSettings.EnablePumpProtect;
                        sSV4.PumpSettings.EnableMotorCycle = false;
                        sSV4.PumpSettings.EnableMotorTimeout = false;
                        sSV4.PumpSettings.MotorCycleLimit = 5;
                        sSV4.PumpSettings.MotorTimeout = 10;
                        sSV4.PumpSettings.Name = sSV2.PumpSettings.Name;
                        sSV4.PumpSettings.NeverReclose = sSV2.PumpSettings.NeverReclose;
                        sSV4.PumpSettings.PumpProtectTime = sSV2.PumpSettings.PumpProtectTime;
                        sSV4.PumpSettings.PumpTime = sSV2.PumpSettings.PumpTime;

                        sSV4.RelayType = sSV2.RelayType;
                        sSV4.TripSettings.ExtendedTimeDelay = sSV2.TripSettings.ExtendedTimeDelay;
                        sSV4.TripSettings.GullWingAngle = sSV2.TripSettings.GullWingAngle;
                        sSV4.TripSettings.GullWingEnabled = sSV2.TripSettings.GullWingEnabled;
                        sSV4.TripSettings.InsensitiveCurrent = sSV2.TripSettings.InsensitiveCurrent;
                        sSV4.TripSettings.Name = sSV2.TripSettings.Name;
                        sSV4.TripSettings.SensitiveTrip = sSV2.TripSettings.SensitiveTrip;
                        sSV4.TripSettings.SensitiveTripDelay = sSV2.TripSettings.SensitiveTripDelay;
                        sSV4.TripSettings.TiltAngle = sSV2.TripSettings.TiltAngle;
                        sSV4.TripSettings.TimeDelay = sSV2.TripSettings.TimeDelay;
                        sSV4.TripSettings.TripMode = sSV2.TripSettings.TripMode;
                        sSV4.TripSettings.WattVarAngle = sSV2.TripSettings.WattVarAngle;
                        sSV4.TripSettings.WattVarCurrent = sSV2.TripSettings.WattVarCurrent;
                    }

                    this.writeSaveObjectToFile();
                }
                catch//file does not exist or is corrupt so just delete it if it does exist
                {
                    if (File.Exists(@"C:\DGI Systems\Relay\SavedSettings.dgi"))
                    {
                        File.Delete(@"C:\DGI Systems\Relay\SavedSettings.dgi");
                    }
                }
            }
            else if (externalFileRevisionNumber == _version3FileRevisionNumber)
            {

                SavedSettingV4 sSV4 = new SavedSettingV4();

                try
                {
                    string backUpFileSavePath = @"C:\DGI Systems\Relay\Saved Data\SavedSettingsBackUp" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".dgi";
                    if (File.Exists(@"C:\DGI Systems\Relay\Saved Data\SavedSettings.dgi"))
                        File.Copy(@"C:\DGI Systems\Relay\Saved Data\SavedSettings.dgi", backUpFileSavePath);  //backup the file before conversion

                    Stream stream = File.Open(@"C:\DGI Systems\Relay\Saved Data\SavedSettings.dgi", FileMode.OpenOrCreate);
                    BinaryFormatter formatter = new BinaryFormatter();

                    if (stream.Length != 0)
                        try
                        {
                            this.saveObjectV3 = (SavedSettingsV3)formatter.Deserialize(stream);
                        }
                        catch
                        {
                            throw new Exception("Error deserializing SavedSettingsV3");
                        }
                    stream.Close();

                    for (int i = 0; i < this.saveObjectV3.Settings.Count; ++i)
                    {
                        SavedSettingV3 sSV3 = this.saveObjectV3.Settings[i];

                        sSV4 = new SavedSettingV4();

                        sSV4.Name = sSV3.Name;
                        sSV4.CTRatio = sSV3.CTRatio;
                        sSV4.Phasing = sSV3.Phasing;
                        sSV4.PumpSettings = sSV3.PumpSettings;

                        sSV4.CloseSettings.CircleCloseEnabled = sSV3.CloseSettings.CircleCloseEnabled;
                        sSV4.CloseSettings.CloseDelay = sSV3.CloseSettings.CloseDelay;
                        sSV4.CloseSettings.Name = sSV3.CloseSettings.Name;
                        sSV4.CloseSettings.PhaseDetectionAngle = sSV3.CloseSettings.PhaseDetectionAngle;
                        sSV4.CloseSettings.PhaseDetectionOffset = sSV3.CloseSettings.PhaseDetectionOffset;
                        sSV4.CloseSettings.RecloseVolts = sSV3.CloseSettings.RecloseVolts;
                        sSV4.CloseSettings.TiltAngle = sSV3.CloseSettings.TiltAngle;
                        sSV4.CloseSettings.BlockedOverride = false;

                        sSV4.RelayType = sSV3.RelayType;

                        sSV4.TripSettings.ExtendedTimeDelay = sSV3.TripSettings.ExtendedTimeDelay;
                        sSV4.TripSettings.GullWingAngle = sSV3.TripSettings.GullWingAngle;
                        sSV4.TripSettings.GullWingEnabled = sSV3.TripSettings.GullWingEnabled;
                        sSV4.TripSettings.InsensitiveCurrent = sSV3.TripSettings.InsensitiveCurrent;
                        sSV4.TripSettings.Name = sSV3.TripSettings.Name;
                        sSV4.TripSettings.SensitiveTrip = sSV3.TripSettings.SensitiveTrip;
                        sSV4.TripSettings.SensitiveTripDelay = sSV3.TripSettings.SensitiveTripDelay;
                        sSV4.TripSettings.TiltAngle = sSV3.TripSettings.TiltAngle;
                        sSV4.TripSettings.TimeDelay = sSV3.TripSettings.TimeDelay;
                        sSV4.TripSettings.TripMode = sSV3.TripSettings.TripMode;
                        sSV4.TripSettings.WattVarAngle = sSV3.TripSettings.WattVarAngle;
                        sSV4.TripSettings.WattVarCurrent = sSV3.TripSettings.WattVarCurrent;

                        this.saveObject.AddState(sSV4);
                    }

                    this.writeSaveObjectToFile();
                }
                catch (Exception ex)//file does not exist or is corrupt so just delete it if it does exist
                {
                    throw new Exception("File Corrupt", ex);
                }
            }
        }

        private System.Timers.Timer timerLiveEventAcknowledge = new System.Timers.Timer();
        void ucEventGraph_DownloadComplete()
        {
            if (eventValue < downloadableEvents - 1)
            {
                eventValue++;
                focusEventGraphDownloading(eventValue);
                this.requestEventData(eventValue);
                return;
            }
            else
                eventValue = 0;

            if (this.downloadProgress != null)
                this.downloadProgress.Dispose();

            downloadEventsClicked = false;
            this.liveDataActionsToolStripMenuItem.Enabled = true;
            this.downloadingLiveData = false;
            this.timerTimeOutCountdown.Enabled = false;
            this.timerLiveEventAcknowledge.Enabled = false;
            this.acknowledge();
            this.toolStripStatusLabelMain.Text = "Ready";
            this.nackCount = 0;

            this.buttonRQEventData.Enabled = true;
            this.requestLiveDataToolStripMenuItem1.Enabled = true;
            this.buttonReqLiveData.Enabled = true;
            this.enableAll(true);
            this.monitoring(true);
            this.RegisterPolling(true);
            this.disableAllMonitoring();
        }

        void focusEventGraphDownloading(int eventNumber)
        {
            if (eventNumber == 0)
                this.radioButtonEvent0.Checked = true;
            else if (eventNumber == 1)
                this.radioButtonEvent1.Checked = true;
            else if (eventNumber == 2)
                this.radioButtonEvent2.Checked = true;
            else if (eventNumber == 3)
                this.radioButtonEvent3.Checked = true;
            else if (eventNumber == 4)
                this.radioButtonEvent4.Checked = true;
            else if (eventNumber == 5)
                this.radioButtonEvent5.Checked = true;
            else if (eventNumber == 6)
                this.radioButtonEvent6.Checked = true;
            else if (eventNumber == 7)
                this.radioButtonEvent7.Checked = true;
        }

        private bool phasorGraphTabSwitchCall = false;//if the phasorGraph called the switch, we don't want it to auto start monitoring

        void ucEventGraph_PopulatePhasorGraph(object sender, CompleteCycleEventArgs cCEA)
        {
            this.phasorGraphTabSwitchCall = true;
            if (this.ProgramState != ProgramStates.Running)
            {
                EnableTab(this.tabPageMonitor, true);
                this.buttonToggleMonitor.Enabled = false;
            }
            this.tabControlMain.SelectedTab = this.tabPageMonitor;

            this.ucPhasorGraph1.UpdateValuesFromWaves(cCEA);
        }

        void ucPhasorGraph1_RequestNewCycle(object sender, CycleInfoRequestEventArgs cIREA)
        {
            ucEventGraph workingGraph;

            switch (cIREA.EventNumber)
            {
                case 0:
                default:
                    workingGraph = this.ucEventGraph0;
                    break;
                case 1:
                    workingGraph = this.ucEventGraph1;
                    break;
                case 2:
                    workingGraph = this.ucEventGraph2;
                    break;
                case 3:
                    workingGraph = this.ucEventGraph3;
                    break;
                case 4:
                    workingGraph = this.ucEventGraph4;
                    break;
                case 5:
                    workingGraph = this.ucEventGraph5;
                    break;
                case 6:
                    workingGraph = this.ucEventGraph6;
                    break;
                case 7:
                    workingGraph = this.ucEventGraph7;
                    break;
                case 9999:
                    this.ucLiveData1.GetCycleInfo(cIREA);
                    return;

            }
            workingGraph.GetCycleInfo(cIREA);
        }

        private void MainControl_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
        }

        private List<string> getPortNames()
        {
            string[] tempPortNames = System.IO.Ports.SerialPort.GetPortNames();
            List<string> returnPortNames = new List<string>();

            for (int j = 0; j < tempPortNames.Length; ++j)
            {
                returnPortNames.Add(tempPortNames[j]);
            }

            returnPortNames.Sort();

            return returnPortNames;
        }

        private void checkSavedLocationAndFindRelay()
        {
            this.monitoring(false);
            this.RegisterPolling(false);

            try
            {
                this.savedFile = new MyFile(_savedFilePath);
                this.savedComPort = this.getSavedComPort(this.savedFile);
            }
            catch (Exception ex)
            {
                this.RegisterPolling(false);
                this.monitoring(false);
                throw new Exception("Error Reading from Saved File", ex);
            }

            try
            {
                this.portNames = getPortNames();
            }
            catch (Exception ex)
            {
                this.RegisterPolling(false);
                this.monitoring(false);
                throw new Exception("Error Setting Port Names", ex);
            }
            try
            {
                List<string> portNames = this.setComPortMenu(this.portNames);

                this.findRelay(this.portNames);

            }
            catch (Exception ex)
            {
                this.RegisterPolling(false);
                this.monitoring(false);
                throw new Exception("Error Starting Find Thread", ex);
            }
        }

        public bool noMonitoringVersion = false;
        private bool allEnabled = false;
        private delegate void enableTabControlCallBack(bool b);

        private void enableAll(bool b)
        {
            try
            {
                if (this.tabControlMain.InvokeRequired)
                {
                    enableTabControlCallBack eTCB = new enableTabControlCallBack(enableAll);
                    this.Invoke(eTCB, new object[] { b });
                }
                else
                {

                    if (!b)
                    {
                        EnableTab(this.tabPageControl, false);
                        EnableTab(this.tabPageDNP, false);
                        EnableTab(this.tabPageFlightRecorder, false);
                        EnableTab(this.tabPageMonitor, false);
                        EnableTab(this.tabPageShortRange, false);
                        EnableTab(this.tabPageTransmitter, false);
                        EnableTab(this.tabPageTransmitterMonitoring, false);
                        this.loadEventSetToolStripMenuItem.Enabled = false;
                        this.liveDataActionsToolStripMenuItem.Enabled = false;
                        this.saveEventsToolStripMenuItem.Enabled = false;
                        this.downloadEventFromRelayToolStripMenuItem.Enabled = false;
                        this.clearEventsToolStripMenuItem.Enabled = false;
                        this.buttonRQEventData.Enabled = false;
                        this.buttonClearEvents.Enabled = false;
                        this.buttonReqLiveData.Enabled = false;
                    }
                    else
                    {
                        EnableTab(this.tabPageControl, true);
                        EnableTab(this.tabPageDNP, true);
                        EnableTab(this.tabPageFlightRecorder, true);
                        EnableTab(this.tabPageMonitor, true);
                        EnableTab(this.tabPageShortRange, true);
                        this.ucTransmitter1.CTRatio = (uint)this.CTRatio;
                        EnableTab(this.tabPageTransmitter, true);
                        this.ucTransmitter1.CTRatio = (uint)this.CTRatio;
                        EnableTab(this.tabPageTransmitterMonitoring, true);
                        this.loadEventSetToolStripMenuItem.Enabled = true;
                        this.saveEventsToolStripMenuItem.Enabled = true;
                        this.downloadEventFromRelayToolStripMenuItem.Enabled = true;
                        this.clearEventsToolStripMenuItem.Enabled = true;
                        this.buttonRQEventData.Enabled = true;
                        this.buttonClearEvents.Enabled = true;
                        this.buttonReqLiveData.Enabled = true;
                        this.comboBox_CTRatio_SelectedItemChanged(this.comboBox_CTRatio, new EventArgs());
                    }

                    this.allEnabled = b;
                    this.ucTransmitter1.EnableControl();
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Enabling All Controls", ex);
            }
        }

        private static void EnableTab(TabPage page, bool enable)
        {
            EnableControls(page.Controls, enable);
        }


        private static void EnableControls(Control.ControlCollection ctls, bool enable)
        {
            foreach (Control ctl in ctls)
            {
                ctl.Enabled = enable;
            }
        }

        private void BeginBackupCapture()
        {
            lock (_backupCaptureLock)
            {
                _backupCaptureSections.Clear();
                _backupCaptureActive = true;
            }
        }

        private void CaptureBackupSection(string sectionName, byte[] data)
        {
            if (!dataBackup_fromRelay || !_backupCaptureActive || data == null)
                return;

            byte[] snapshot = (byte[])data.Clone();

            lock (_backupCaptureLock)
            {
                // Replace existing section if already captured, otherwise append
                for (int i = 0; i < _backupCaptureSections.Count; i++)
                {
                    if (_backupCaptureSections[i].Key.Equals(sectionName, StringComparison.OrdinalIgnoreCase))
                    {
                        _backupCaptureSections[i] = new KeyValuePair<string, byte[]>(sectionName, snapshot);
                        return;
                    }
                }

                _backupCaptureSections.Add(new KeyValuePair<string, byte[]>(sectionName, snapshot));
            }
        }

        private void ClearBackupCaptureState()
        {
            lock (_backupCaptureLock)
            {
                _backupCaptureSections.Clear();
                _backupCaptureActive = false;
            }
        }

        private void FlushBackupToDisk()
        {
            if (!_backupCaptureActive || _backupCaptureSections.Count == 0)
                return;

            // Ensure directory exists
            string directory = Path.GetDirectoryName(_relayBackupPath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Data residing in the relay :");
            sb.AppendLine(DateTime.UtcNow.ToString("O"));

            lock (_backupCaptureLock)
            {
                foreach (var section in _backupCaptureSections)
                {
                    sb.AppendLine(section.Key + ":");
                    foreach (byte b in section.Value)
                    {
                        sb.AppendLine(b.ToString());
                    }
                }
            }

            File.WriteAllText(_relayBackupPath, sb.ToString());

            ClearBackupCaptureState();
        }

        private void ResetFullParameterDownloadState(string caller)
        {
            logger.Warn("ResetFullParameterDownloadState caller={0}", caller);

            this.requestedAllParameters = false;
            this.ProgramState = ProgramStates.Running;
            this.sendAll = false;
            sendAllF.SendAllFlag = false;
            this.timerResponseTimeOut.Enabled = false;

            this.enableAll(true);
            screenD.screenDisable = false;
            this.UseWaitCursor = false;
            Application.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;
        }

        private SavedSettingV4 reprogrammingTempSettings = new SavedSettingV4();
        private bool loadingNewCode = false;
        private RelayProgrammingSendCommands currentReprogramState = RelayProgrammingSendCommands.RestartProgram;

        private void Programming_Send(object o, RelayProgrammingEventArgs rPEA)
        {
            this.currentReprogramState = rPEA.Command;

            logger.Trace("Programming Command: {0}", rPEA.Command);
            logger.Info($"Programming_Send RequestAll: requestedAllParameters(before)={this.requestedAllParameters}, ProgramState(before)={this.ProgramState}, loadingNewCode={this.loadingNewCode}");
            switch (rPEA.Command)
            {
                case RelayProgrammingSendCommands.RequestAll:
                    // 1) Always clear stale full-download state before entering a new cycle
                    if (this.ProgramState == ProgramStates.DownloadingAllParameters ||
                        this.requestedAllParameters)
                    {
                        logger.Warn("Stale parameter download state detected before RequestAll. Resetting.");
                        this.requestedAllParameters = false;
                        this.ProgramState = ProgramStates.Running;
                        this.sendAll = false;
                        sendAllF.SendAllFlag = false;
                        this.timerResponseTimeOut.Enabled = false;
                    }

                    // 2) Then check for real boot/backup/programming suppression
                    if (this.ucRelayProgramming1.State == RelayProgrammingStates.ReprogramSuccess)
                    {
                        logger.Info("Allowing RequestAll after ReprogramSuccess for post-programming restore.");
                        this.pendingAutoloadAfterBackup = false;
                        this.pendingRestoreAfterProgramming = true;
                        this.loadingNewCode = false;
                    }
                    else if (this.pendingAutoloadAfterBackup ||
                             this.ucRelayProgramming1.ReprogrammingInProgress ||
                             this.loadingNewCode ||
                             this.ucRelayProgramming1.State == RelayProgrammingStates.AutoLoadCheckBoot ||
                             this.ucRelayProgramming1.State == RelayProgrammingStates.CheckMasterBootCode ||
                             this.ucRelayProgramming1.State == RelayProgrammingStates.ManualLoadCheckBoot ||
                             this.ucRelayProgramming1.State == RelayProgrammingStates.LoadingMasterBootLoader ||
                             this.ucRelayProgramming1.State == RelayProgrammingStates.DoneLoadingMasterBootLoader ||
                             this.ucRelayProgramming1.ProgramBootCodeInProgress)
                    {
                        logger.Info(
                            "Suppressing RequestAll during backup/boot-check/programming transition. " +
                            "pendingAutoloadAfterBackup={0}, reprogrammingInProgress={1}, loadingNewCode={2}, state={3}",
                            this.pendingAutoloadAfterBackup,
                            this.ucRelayProgramming1.ReprogrammingInProgress,
                            this.loadingNewCode,
                            this.ucRelayProgramming1.State);

                        break;
                    }

                    // 3) Duplicate re-entry guard
                    if (this.ProgramState == ProgramStates.DownloadingAllParameters &&
                        this.requestedAllParameters)
                    {
                        logger.Warn("RequestAll already active; ignoring duplicate request.");
                        break;
                    }

                    this.requestedAllParameters = true;
                    this.ProgramState = ProgramStates.DownloadingAllParameters;
                    this.loadingNewCode = false;

                    //Thread.Sleep(3000);
                    clearRemoteBuffer();
                    //Thread.Sleep(1000);
                    requestRelayRevision();
                    break;
                case RelayProgrammingSendCommands.RestartProgram:
                    this.quietMode = false;
                    if (this.ucRelayProgramming1.State == RelayProgrammingStates.AutoLoadCheckBoot)
                    {
                        this.toolStripStatusLabelRelayDisconnected.Visible = false;
                        return;
                    }
                    this.toolStripStatusLabelRelayDisconnected.Visible = true;
                    break;
                case RelayProgrammingSendCommands.SaveSettings:
                    this.ucSafeService1.LoadingNewCode = true;
                    this.getAllSaveStates(this.reprogrammingTempSettings);
                    break;
                case RelayProgrammingSendCommands.TransmitterSettings:
                    this.ucTransmitter1.SetAllValues(rPEA.BytesToSend);
                    this.ucTransmitter1.SendTransmitterSettings();
                    UpdateDnpCommStatusFromRelayState(this.DNPEnabled);
                    break;
                case RelayProgrammingSendCommands.RawData:
                    this.ucSafeService1.LoadingNewCode = true;
                    this.enableAll(false);
                    this.toolStripStatusLabelRelayDisconnected.Visible = false;
                    this.loadingNewCode = true;
                    this.quietMode = true;
                    this.pauseMonitoring = true;
                    this.sendPacket(rPEA.BytesToSend);
                    break;
                case RelayProgrammingSendCommands.RecallSavedSettings:
                    // For future versions, this part should be checked because I am adding this for adding SafeService to the relay
                    this.ucSafeService1.SetDefaults();
                    ////
                    this.setAllValues(this.reprogrammingTempSettings);
                    this.sendAllParameters();
                    break;
                case RelayProgrammingSendCommands.DisableGERelayFix:
                    this.ucTransmitter1.GERelay = false;
                    this.ucTransmitter1.SendTransmitterSettings();
                    break;
                case RelayProgrammingSendCommands.EnableGERelayFix:
                    this.ucTransmitter1.GERelay = true;
                    this.ucTransmitter1.SendTransmitterSettings();
                    break;
            }
            logger.Info($"Programming_Send RequestAll: requestedAllParameters(after)={this.requestedAllParameters}, ProgramState(after)={this.ProgramState}");
        }

        private void ucTransmitter1_Send(SendEventArgs sEA)
        {
            if (sEA?.SendPacket == null || sEA.SendPacket.Length == 0)
            {
                logger.Warn("ucTransmitter1_Send called with empty packet.");
                return;
            }

            if (sEA.SendPacket[0] == 0x66)
            {
                this.sendPacket(sEA.SendPacket);

                bool showForceConfigPopup = !this.ucTransmitter1.FastModeActive;
                if (showForceConfigPopup)
                {
                    this.downloadProgress = new ProgressBarForm(
                        "Force Config Message",
                        "Sending Configuration Messages. Please Wait.",
                        140,
                        true);

                    this.downloadProgress.Done += new ProgressBarForm.ProgressBarEvent(downloadProgress_Done);
                    this.downloadProgress.ShowDialog();
                }

                return;
            }

            if (sEA.SendPacket[0] == (byte)'X') // explicit request packet
            {
                // keep explicit full read request behavior
                this.requestAllData("ucTransmitter1_Send");
                return;
            }

            // Normal TX settings apply path
            this.sendPacketAck(sEA.SendPacket, "Transmitter Settings Send");

            // Do NOT trigger full requestAllData here.
            // If immediate confirmation is needed, use targeted TX read instead.
            if (!this.pendingAutoloadAfterBackup &&
                !this.ucRelayProgramming1.ReprogrammingInProgress &&
                !this.loadingNewCode)
            {
                // optional targeted refresh:
                // this.requestTransmitterSettings();
            }
            else
            {
                logger.Info("Suppressing transmitter-triggered refresh during backup/programming transition.");
            }
        }

        private const byte DnpControlOpcode = (byte)'D';
        private const byte DnpDeadbandSubcode = (byte)'d';

        void ucDNP_Send(SendEventArgs sEA)
        {
            if (sEA.SendPacket == null || sEA.SendPacket.Length == 0)
            {
                logger.Warn("ucDNP_Send called with empty packet.");
                return;
            }

            bool isDnpApplyOrDeadbandPacket =
                sEA.SendPacket[0] == DnpControlOpcode &&
                sEA.SendPacket.Length > 1 &&
                (sEA.SendPacket[1] == (byte)'a' || sEA.SendPacket[1] == DnpDeadbandSubcode);

            if (isDnpApplyOrDeadbandPacket)
            {
                string caller = (sEA.SendPacket[1] == DnpDeadbandSubcode)
                    ? "DNP DeadBand Send"
                    : "DNP Settings Send";

                this.sendPacketAck(sEA.SendPacket, caller);

                // optional targeted refresh only (if needed)
                // this.requestDNPSettings();

                return;
            }

            this.sendPacket(sEA.SendPacket);
        }

        private bool RelaySupportsDnp()
        {
            string revision =
                !string.IsNullOrEmpty(receivedMasterRevision)
                    ? receivedMasterRevision
                    : this.ucRelayProgramming1.MasterRevisionString;

            return !string.IsNullOrEmpty(revision) &&
                   revision.Contains("DNP");
        }

        private void UpdateDnpCommStatusFromRelayState(bool relayDnpActive)
        {
            // Do not drive ucTransmitter1.DNPCommLabelStatus from MainControl.
            // The transmitter control owns DNP status from relay TX readback.
        }

        private Point PanelLocation = new Point(300, 12);
        private RelayControlLibrary.RelayMode relayMode = new RelayControlLibrary.RelayMode();

        private string[] TripCurveNames = new string[3] { "Angle Offset", "Magnitude", "No Curve" };
        private string[] CloseCurveNames = new string[2] { "Vertical", "Horizontal" };

        private RelayControlLibrary.TripCurveDefinition[] TripCurveDefinitions = new RelayControlLibrary.TripCurveDefinition[4];

        public bool SendConfirmed = true;

        private byte[] receiveArray = new byte[2000];
        private int rXWritePtr = 0;
        private int rXReadPtr = 0;
        private bool readSemaphoreTaken = false;

        private int nextRXArrayAddress(int Ptr)
        {
            int returnPtr;
            try
            {
                if (Ptr >= this.receiveArray.Length - 1)
                    returnPtr = 0;
                else
                    returnPtr = Ptr + 1;
                return returnPtr;
            }
            catch (Exception ex)
            {
                this.messageHandler("Error in RX Array Pointer Update", ex);
                return Ptr;
            }
        }
        private bool expectingAck = false;

        public bool IsExpectingAck => this.expectingAck;

        private void resetCommunicationInterface()
        {
            try
            {
                this.receiveArray.Initialize();
                this.rXWritePtr = this.rXReadPtr = 0;
                this.SendConfirmed = true;
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Reseting Communication Interface", ex);
            }
        }

        private Int32 byteCount = 0;

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            this.dataReceived();
        }

        private int testCount = 0;

        private void dataReceived()
        {
            byte lastByte = 0;
            bool dReceived = false;

            try
            {
                while (this.serialPort1.BytesToRead > 0)
                {
                    this.SendConfirmed = true;

                    lastByte = this.receiveArray[this.rXWritePtr] = (byte)this.serialPort1.ReadByte();
                    if (checkBoxSerialCommsDebugging.Checked)
                    {
                        //logger.Trace(new ASCIIEncoding().GetString(new byte[] { lastByte }));
                        if (lastByte == '-')
                        {
                            testCount++;
                            if (testCount == 15)
                            {
                                testCount = 0;
                                logger.Info(DateTime.Now.ToString());
                            }
                        }
                    }
                    if (lastByte == 0x06)
                    {
                        string ackCaller = null;
                        bool acceptedAck = false;

                        lock (_ackFlowLock)
                        {
                            if (!this.expectingAck)
                            {
                                logger.Warn("Duplicate ACK ignored. expectingAck=False, caller={0}", this.AcknowledgeCaller);
                            }
                            else
                            {
                                ackCaller = this.AcknowledgeCaller;
                                this.expectingAck = false;
                                this.AcknowledgeCaller = string.Empty;
                                acceptedAck = true;
                            }
                        }

                        if (acceptedAck)
                        {
                            this.SendConfirmed = true;
                            this.timerSCITimeOut.Enabled = false;

                            // IMPORTANT: keep this call
                            packetAcknowledged(true);

                            logger.Info("ACK received. caller={0}, expectingAck(after)={1}", ackCaller, this.expectingAck);

                            if (this.requestedAllParameters || this.ProgramState == ProgramStates.DownloadingAllParameters)
                            {
                                logger.Info("Drain blocked (ACK-RX-{0}): parameter download active. requestedAllParameters={1}, ProgramState={2}",
                                    ackCaller, this.requestedAllParameters, this.ProgramState);
                            }
                            else if (string.Equals(ackCaller, "CT Ratio Send", StringComparison.OrdinalIgnoreCase))
                            {
                                this.TryDrainPendingRelayTypePhasingSend("ACK-RX-CT");
                            }
                        }
                    }
                    if (lastByte == 0x0D)
                        dReceived = true;

                    this.rXWritePtr = this.nextRXArrayAddress(this.rXWritePtr);
                }
            }
            catch
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                this.resetCommunicationInterface();
                this.RegisterPolling(true);
            }

            if (clickbuttonCT.CTratioButton == true)
            {
                clickbuttonCT.CTratioButton = false;
                //this.cTRatioCalculatorToolStripMenuItem_Click(this, new EventArgs());

                this.Invoke(new Action(() =>
                {
                    CTRatioCaculator CTCalculator = new CTRatioCaculator();
                    CTCalculator.ShowDialog(this);
                }));
            }

            //Check to see if the last byte is a confirmation
            try
            {
                if (!this.readSemaphoreTaken || dReceived)//lastByte == 0x0D )
                {
                    this.BeginInvoke(new EventHandler(this.checkRawData));
                    dReceived = false;
                }
            }
            catch //(Exception ex)
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                this.resetCommunicationInterface();
                this.RegisterPolling(true);
            }

        }

        private void packetAcknowledged(bool b)
        {
            if (this.InvokeRequired)
            {
                booleanInvoke bI = new booleanInvoke(this.packetAcknowledged);
                this.Invoke(bI, new object[] { b });
            }
            else
            {
                this.ucRelayProgramming1.PacketAcknowledged(b);
            }
        }

        private void checkRawData(object sender, EventArgs e)
        {
            IncomingCommCommands command = IncomingCommCommands.Invalid;
            int packetSize = 0;
            int tempRXReadPtr = 0;
            int initialRXPtr = 0, commandAddress = 0;

            this.readSemaphoreTaken = true;
            try
            {
                while (this.rXReadPtr != this.rXWritePtr)
                {
                    command = IncomingCommCommands.Invalid;
                     
                    while (true)
                    {
                        //if (screenD.screenDisable == true)
                           // this.enableAll(false);
                        initialRXPtr = tempRXReadPtr = this.rXReadPtr;
                        //check to see if we have found a command or we have reached the end of the data
                        command = this.getCommand(this.receiveArray[tempRXReadPtr]);

                       //if(command == IncomingCommCommands.DNPMessage1)
                        //    MessageBox.Show("dnpMessage1 - 0x11 - to be processed for AIs 0 to 42");

                        
                        while (command == IncomingCommCommands.Invalid)
                        {
                            tempRXReadPtr = this.nextRXArrayAddress(tempRXReadPtr);
                            if (tempRXReadPtr == this.rXWritePtr)
                            {
                                this.readSemaphoreTaken = false;

                                return;
                            }
                            command = this.getCommand(this.receiveArray[tempRXReadPtr]);

                        }
                        commandAddress = tempRXReadPtr;

                        tempRXReadPtr = this.nextRXArrayAddress(tempRXReadPtr);
                        if (tempRXReadPtr == this.rXWritePtr)
                        {
                            this.rXReadPtr = initialRXPtr;
                            return;
                        }


                        if (command == IncomingCommCommands.StandardPacket)
                        {
                            // Check for modern packet method
                            if (receiveArray[tempRXReadPtr] != 0x55)
                            {
                                // Not actually one of them
                                this.rXReadPtr = this.nextRXArrayAddress(initialRXPtr);
                                command = IncomingCommCommands.Invalid;
                                break;
                            }
                            else
                            {
                                // Is one, and there is only one at this point so
                                // we'll say it is a success
                                labelKioskReceived.Text = _kioskCommandReceived;
                                labelKioskReceived.BackColor = Color.Green;
                            }
                        }
                        else if (command == IncomingCommCommands.Revision)
                        {
                            //check for special case of revision which has no length byte
                            packetSize = 37;

                            if ((char)this.receiveArray[tempRXReadPtr] != 'E') //checks second character
                            {
                                this.rXReadPtr = this.nextRXArrayAddress(initialRXPtr);
                                command = IncomingCommCommands.Invalid;
                                break;
                            }
                        }
                        else if (command == IncomingCommCommands.NoMemFix)
                        {
                            packetSize = 63;

                            if ((char)this.receiveArray[tempRXReadPtr] != 'L') //checks second character
                            {
                                this.rXReadPtr = this.nextRXArrayAddress(initialRXPtr);
                                command = IncomingCommCommands.Invalid;
                                break;
                            }
                        }
                        else if (command == IncomingCommCommands.FPGARevision)
                        {
                            packetSize = 20;
                            logger.Trace("FPGA Received ReprogrammingInProgress: {0}", ucRelayProgramming1.ReprogrammingInProgress);
                            if ((char)this.receiveArray[tempRXReadPtr] != 'P')
                            {
                                this.rXReadPtr = this.nextRXArrayAddress(initialRXPtr);
                                command = IncomingCommCommands.Invalid;
                                break;
                            }
                        }
                        else if (command == IncomingCommCommands.Boot)
                        {
                            packetSize = 12;
                            if ((char)this.receiveArray[tempRXReadPtr] != 'O')
                            {
                                this.rXReadPtr = this.nextRXArrayAddress(initialRXPtr);
                                command = IncomingCommCommands.Invalid;
                                break;
                            }
                        }
                        else //get the packet size from the next byte
                        {
                            packetSize = this.receiveArray[tempRXReadPtr];
                            tempRXReadPtr = this.nextRXArrayAddress(tempRXReadPtr);
                            if (tempRXReadPtr == this.rXWritePtr)
                            {
                                this.rXReadPtr = initialRXPtr;
                                return;
                            }
                            if (!this.lengthValid(command, packetSize))
                            {
                                //If the length is not valid it means it was not a valid command
                                //so we will check the next address
                                this.rXReadPtr = this.nextRXArrayAddress(initialRXPtr);//this.nextRXArrayAddress(this.rXReadPtr);
                                command = IncomingCommCommands.Invalid;
                                break;
                            }
                        }

                        if (!allPacketDataReceived(packetSize, tempRXReadPtr))
                        {
                            this.readSemaphoreTaken = false;
                            this.rXReadPtr = initialRXPtr;
                            return;
                        }

                        if (checkRXArrayIndex(packetSize, tempRXReadPtr))
                        {
                            break;
                        }
                        else
                        {
                            this.rXReadPtr = this.nextRXArrayAddress(initialRXPtr);
                            command = IncomingCommCommands.Invalid;
                            break;
                        }
                    }

                    if (command != IncomingCommCommands.Invalid)
                    {
                        byte[] packet = new byte[packetSize];

                        this.receiveArray[commandAddress] = 0;
                        packet = formulatedPacket(tempRXReadPtr, packetSize);

                        this.packetFormulated(packet, command);
                    }

                }
                this.readSemaphoreTaken = false;
            }
            catch (Exception ex)
            {
                if (command != IncomingCommCommands.DNPMessage1 &&
                    command != IncomingCommCommands.DNPMessage2 &&
                    command != IncomingCommCommands.DNPMessage3 &&
                    command != IncomingCommCommands.DNPMessage4 &&
                    command != IncomingCommCommands.DNPMessage5)
                {
                    this.messageHandler("Error Checking Raw Communication Data", ex);
                    this.RegisterPolling(true);
                }
            }
        }

        private bool lengthValid(IncomingCommCommands c, int i)
        {
            try
            {
                //logger.Trace("Checking Valid Length - OpCode: {0}, Length: {1}", c, i);
                switch (c)
                {
                    case IncomingCommCommands.DNPMessage1:
                    case IncomingCommCommands.DNPMessage2:
                    case IncomingCommCommands.DNPMessage3:
                    case IncomingCommCommands.DNPMessage4:
                    case IncomingCommCommands.DNPMessage5:
                        if (i == 252)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.ArcFaultData:
                        if (i == 40 || i == 42)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.ReceiverStatusCode:
                        if (i == 1)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.CalibrationComplete:
                        if (i == 2)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.CalibrationConstants:
                        if (i == 60)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.CurrentTime:
                        if (i == 4)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.EventDataPacket:
                        if (i == 133)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.EventTimes:
                        if (i == 48 || i == 80) //was 48
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.GeneralCommand:
                        if (i == 80)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.LiveDataPacket:
                        return false;
                    case IncomingCommCommands.PhasorUpdate:
                        if (i == 18)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.RelayParameters:
                        if (i == 88 || i == 90 || i == 94 || i == 95)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.RelayRegisters:
                        if (i == 10 || i == 6)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.NoMemFix:
                        if (i == 63)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.RelayRevision:
                        if (i == 32)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.PCdata:
                        if (i == 8)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.ATdata:
                        if (i == 10)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.Revision:
                        if (i == 38)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.FPGARevision:
                        if (i == 20)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.Boot:
                        if (i == 12)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.SafeService:
                        if (i == 20)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.SineGraphValue:
                        if (i == 6)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.Temperature:
                        if (i == 2)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.TransmitterSettings:
                        if (i == 30 || i == 32)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.TransmitterMonitor:
                        if (i == 7 || i == 18)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.TripOrCloseEvent:
                    case IncomingCommCommands.RelayStatusBits:
                        if (i == 1)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.FFTValue:
                        if (i == 12)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.DNPData:
                    case IncomingCommCommands.DNPSAv5:
                        if (i == 40 || i == 98)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.ShortRangeStrength:
                        if (i == 107)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.ShortRangeTransmit:
                        if (i == 31)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.LowVoltageThresReceived:
                        if (i == 2)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.Invalid:
                    default:
                        throw new Exception("Bad command to check length for");

                }
            }
            catch
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                this.resetCommunicationInterface();
                this.RegisterPolling(true);
                return false;
            }
        }
        private bool allPacketDataReceived(int packetLength, int tempRXReadPtr)
        {
            try
            {
                //check to see if the first byte is actually a packet Length
                //must step through so you don't point outside buffer
                if (tempRXReadPtr == this.rXWritePtr)
                {
                    return false;
                }
                for (int i = 0; i < packetLength; ++i)
                {
                    tempRXReadPtr = this.nextRXArrayAddress(tempRXReadPtr);
                    if (tempRXReadPtr == this.rXWritePtr)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                this.resetCommunicationInterface();
                this.RegisterPolling(true);
                return false;
            }
        }

        //Checks the value in the receive array %packetLength% after the command to check for a 0x0D
        private bool checkRXArrayIndex(int packetLength, int pointer)
        {
            try
            {
                int tempRXReadPtr = pointer;
                byte temp = 0;

                //check to see if the first byte is actually a packet Length
                //must step through so you don't point outside buffer
                if (tempRXReadPtr == this.rXWritePtr)
                {
                    return false;
                }
                temp = this.receiveArray[tempRXReadPtr];
                for (int i = 0; i < packetLength; ++i)
                {
                    tempRXReadPtr = this.nextRXArrayAddress(tempRXReadPtr);
                    if (tempRXReadPtr == this.rXWritePtr)
                    {
                        return false;
                    }
                    temp = this.receiveArray[tempRXReadPtr];
                }
                if (temp == 0x0D || temp == 0x0A)
                {
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                this.resetCommunicationInterface();
                this.RegisterPolling(true);
                return false;
            }
        }
        private IncomingCommCommands getCommand(byte value)
        {
            // 32 is Space
            //if (value >= 32)
            //    logger.Trace("Check Opcode: {0}", Convert.ToChar(value));
            //else
            //    logger.Trace("Check Opcode: (byte){0:X}", value);

            switch (value)
            {
                case 0xAA:
                    return IncomingCommCommands.StandardPacket;
                case 0x0E:
                    return IncomingCommCommands.SafeService;
                case 0x11:
                    return IncomingCommCommands.DNPMessage1; // Bianry Inputs
                case 0x12:
                    return IncomingCommCommands.DNPMessage2; // Binary Outputs
                case 0x13:
                    return IncomingCommCommands.DNPMessage3; // Analog Inputs ( 255 /4 = 63 ) 4 bytes per point data - first 63 AIs
                case 0x14:
                    return IncomingCommCommands.DNPMessage4; // Analog Inputs ( 255 /4 = 63 ) 4 bytes per point data - next 63 AIs
                case 0x20:
                    return IncomingCommCommands.DNPMessage5; // Analog Outputs 
                case 0x17:
                    return IncomingCommCommands.GeneralCommand;
                case (byte)'A':
                    return IncomingCommCommands.RelayStatusBits;
                case (byte)'a':
                    return IncomingCommCommands.ArcFaultData;
                case (byte)'B':
                    return IncomingCommCommands.Boot;
                case (byte)'c':
                    return IncomingCommCommands.CalibrationComplete;
                case (byte)'D':
                    return IncomingCommCommands.DNPData;
                case (byte)'d':
                    return IncomingCommCommands.TransmitterMonitor;
                case (byte)'E':
                    return IncomingCommCommands.TripOrCloseEvent;
                case (byte)'f':
                    return IncomingCommCommands.CalibrationConstants;
                case (byte)'F':
                    return IncomingCommCommands.FPGARevision;
                case (byte)'g':
                    return IncomingCommCommands.EventTimes;
                case (byte)'H':
                    return IncomingCommCommands.SineGraphValue;
                case (byte)'i':
                    return IncomingCommCommands.EventDataPacket;
                case (byte)'j':
                    return IncomingCommCommands.CurrentTime;
                case (byte)'K':
                    return IncomingCommCommands.ShortRangeTransmit;
                case (byte)'N':
                    return IncomingCommCommands.ShortRangeStrength;
                case (byte)'O':
                    return IncomingCommCommands.NoMemFix;
                case (byte)'P':
                    return IncomingCommCommands.PhasorUpdate;
                case (byte)'Q':
                    return IncomingCommCommands.RelayRevision;
                case (byte)'R':
                    return IncomingCommCommands.Revision;
                case (byte)'r':
                    return IncomingCommCommands.RelayRegisters;
                case (byte)'S':
                    return IncomingCommCommands.RelayParameters;
                case (byte)'s':
                    return IncomingCommCommands.DNPSAv5;
                case (byte)'t':
                    return IncomingCommCommands.Temperature;
                case (byte)'V':
                    return IncomingCommCommands.FFTValue;
                case (byte)'Y':
                    return IncomingCommCommands.TransmitterSettings;
                case (byte)'x':
                    return IncomingCommCommands.LiveDataPacket;
                case (byte)'?':
                    return IncomingCommCommands.LowVoltageThresReceived;
                case (byte)'~':
                    return IncomingCommCommands.PCdata;
                case (byte)'[':
                    return IncomingCommCommands.ATdata;
                default:
                    return IncomingCommCommands.Invalid;
            }
        }

        byte[] formulatedPacket(int index, int size)
        {
            try
            {
                byte[] returnArray = new byte[size];

                for (int i = 0; i < size; i++)
                {
                    if (index == this.receiveArray.Length)
                    {
                        index = 0;
                    }
                    returnArray[i] = receiveArray[index];

                    receiveArray[index] = 0;
                    ++index;
                }
                this.rXReadPtr = this.nextRXArrayAddress(index);
                return returnArray;
            }
            catch //(Exception ex)
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                this.resetCommunicationInterface();
                this.RegisterPolling(true);
                return new byte[1];

            }
        }

        private void packetFormulated(byte[] bytePacket, IncomingCommCommands command)
        {
            switch (command)
            {
                case IncomingCommCommands.DNPMessage1: // BI
                    if (this.dNPDIGITALGRIDData == null)
                    {
                        logger.Warn("Dropping DNPMessage1: live-data control not initialized.");
                        return;
                    }
                    this.dNPDataMessage(bytePacket, 1);
                    break;

                case IncomingCommCommands.DNPMessage2: // BO
                    if (this.dNPDIGITALGRIDData == null)
                    {
                        logger.Warn("Dropping DNPMessage2: live-data control not initialized.");
                        return;
                    }
                    this.dNPDataMessage(bytePacket, 2);
                    break;

                case IncomingCommCommands.DNPMessage3: // AI
                    if (this.dNPDIGITALGRIDData == null)
                    {
                        logger.Warn("Dropping DNPMessage3: live-data control not initialized.");
                        return;
                    }
                    this.dNPDataMessage(bytePacket, 3);
                    break;

                case IncomingCommCommands.DNPMessage4: // AI
                    if (this.dNPDIGITALGRIDData == null)
                    {
                        logger.Warn("Dropping DNPMessage4: live-data control not initialized.");
                        return;
                    }
                    this.dNPDataMessage(bytePacket, 4);
                    break;

                case IncomingCommCommands.DNPMessage5: // AO
                    if (this.dNPDIGITALGRIDData == null)
                    {
                        logger.Warn("Dropping DNPMessage5: live-data control not initialized.");
                        return;
                    }
                    this.dNPDataMessage(bytePacket, 5);
                    break;

                case IncomingCommCommands.GeneralCommand:
                    this.ucGeneralCommandHandler1.HandleCommand(bytePacket);
                    this.ucTransmitter1.RequestLightningCount();
                    break;

                case IncomingCommCommands.ArcFaultData:
                    if (dataBackup_fromRelay == true)
                    {
                        byte[] snapshot = new byte[Math.Min(bytePacket.Length, 40)];
                        Array.Copy(bytePacket, snapshot, snapshot.Length);
                        CaptureBackupSection("Arc Fault Parameters", snapshot);
                    }

                    this.setArcFaultData(bytePacket);
                    this.MarkSectionReceived(SectionBits.ArcFault);

                    if (backupInProgress)
                    {
                        backupGotArcFault = true;
                        TryCompleteBackup();
                    }
                    break;

                case IncomingCommCommands.Boot:
                    this.handleBootMessage(bytePacket);
                    break;

                case IncomingCommCommands.RelayStatusBits:
                    this.setRelayStatusBits(bytePacket);
                    break;

                case IncomingCommCommands.CalibrationComplete:
                    this.calibrationComplete(bytePacket);
                    break;

                case IncomingCommCommands.CalibrationConstants:
                    this.setCalibrationConstants(bytePacket);
                    this.MarkSectionReceived(SectionBits.Calibration);

                    backupGotCalibration = true;
                    TryCompleteBackup();
                    break;

                case IncomingCommCommands.CurrentTime:
                    this.storeCurrentTime(bytePacket);
                    break;

                case IncomingCommCommands.SafeService:
                    this.ucSafeService1.SetAll(bytePacket);

                    if (dataBackup_fromRelay == true)
                    {
                        byte[] snapshot = new byte[Math.Min(bytePacket.Length, 20)];
                        Array.Copy(bytePacket, snapshot, snapshot.Length);
                        CaptureBackupSection("Safe Service Data", snapshot);
                    }

                    this.MarkSectionReceived(SectionBits.SafeService);

                    backupGotSafeService = true;
                    TryCompleteBackup();
                    break;

                case IncomingCommCommands.ShortRangeStrength:
                case IncomingCommCommands.ShortRangeTransmit:
                    this.setShortRangeParameters(bytePacket);
                    break;

                case IncomingCommCommands.EventTimes:
                    this.storeTimesReceived(bytePacket);
                    break;

                case IncomingCommCommands.EventDataPacket:
                    this.eventLiveDataPacket(bytePacket);
                    break;

                case IncomingCommCommands.LiveDataPacket:
                    this.liveDataPacket(bytePacket);
                    break;

                case IncomingCommCommands.PhasorUpdate:
                    this.setPhasorValue(bytePacket);
                    break;

                case IncomingCommCommands.RelayParameters:
                    this.setRelayParameters(bytePacket);
                    this.MarkSectionReceived(SectionBits.RelayParams);

                    backupGotRelayParams = true;
                    TryCompleteBackup();
                    break;

                case IncomingCommCommands.RelayRegisters:
                    this.setRelayRegisters(bytePacket);
                    break;

                case IncomingCommCommands.NoMemFix:
                    this.DNPEnabled = false;
                    this.showNoMemFixMessage(bytePacket);
                    break;

                case IncomingCommCommands.RelayRevision:
                    this.setRelayRevisionLabel(bytePacket);
                    break;

                case IncomingCommCommands.PCdata:
                    this.setPermissiveCloseData(bytePacket);
                    break;

                case IncomingCommCommands.ATdata:
                    this.ucTripMode2.SetAdaptiveValuesFromPacket(bytePacket);
                    break;

                case IncomingCommCommands.Revision:
                    this.revisionReceived(bytePacket);
                    break;

                case IncomingCommCommands.FPGARevision:
                    this.setFPGARevision(bytePacket);
                    break;

                case IncomingCommCommands.Temperature:
                    this.setTemperature(bytePacket);
                    break;

                case IncomingCommCommands.TripOrCloseEvent:
                    this.trippedOrClosed(bytePacket);
                    break;

                case IncomingCommCommands.TransmitterSettings:
                    this.setTransmitterSettings(bytePacket);
                    {
                        UInt16 uTemp;
                        uTemp = bytePacket[1];
                        uTemp <<= 8;
                        uTemp += bytePacket[0];
                        this.textBox_TxID.Text = uTemp.ToString();
                    }

                    this.MarkSectionReceived(SectionBits.TxSettings);

                    backupGotTx = true;
                    TryCompleteBackup();
                    break;

                case IncomingCommCommands.TransmitterMonitor:
                    this.setTransmitterMonitorData(bytePacket);
                    break;

                case IncomingCommCommands.FFTValue:
                    this.setFFTValue(bytePacket);
                    break;

                case IncomingCommCommands.DNPData:
                    if (!ucRelayProgramming1.ProgramBootCodeInProgress)
                    {
                        bool wroteDnpBackup = this.setDNPSettings(bytePacket);
                        logger.Info("DNP backup write result: backupInProgress={0}, dataBackup_fromRelay={1}, wroteDnpBackup={2}",
                            backupInProgress,
                            dataBackup_fromRelay,
                            wroteDnpBackup);

                        if (this.syncPhase == SyncPhase.FullParameterDownload)
                        {
                            this.MarkSectionReceived(SectionBits.Dnp);
                        }

                        if (backupInProgress && wroteDnpBackup)
                        {
                            backupGotDnpData = true;
                            TryCompleteBackup();
                        }
                    }
                    break;

                case IncomingCommCommands.DNPSAv5:
                    if (dataBackupR.dataBackup_fromRelay == true)
                    {
                        byte[] snapshot = new byte[Math.Min(bytePacket.Length, 98)];
                        Array.Copy(bytePacket, snapshot, snapshot.Length);
                        CaptureBackupSection("DNPSAv5 Settings", snapshot);

                        dataBackupR.dataBackup_fromRelay = false;
                    }

                    this.ucDNPSAv51.Message(bytePacket);

                    if (this.syncPhase == SyncPhase.FullParameterDownload)
                    {
                        this.MarkSectionReceived(SectionBits.DnpSav5);
                    }

                    if (backupInProgress && dataBackupR.dataBackup_fromRelay == false)
                    {
                        backupGotDnpSav5 = true;
                        TryCompleteBackup();
                    }
                    break;

                case IncomingCommCommands.LowVoltageThresReceived:
                    this.SetLowVoltageThres(bytePacket);
                    break;

                case IncomingCommCommands.Invalid:
                default:
                    throw new Exception("bad command for function call");
            }
        }
        private RelayStatusCodeConverter relayStatusConverter = new RelayStatusCodeConverter();

        private void setRelayStatusBits(byte[] bytePacket)
        {
            /*
             Data coming from master uP with command "A" for bytePacket[0]
#define RELAYSTATUS_OP           0x00   Open
#define RELAYSTATUS_CL           0x01   Close
#define RELAYSTATUS_FB           0x02   Floating and Blocked open
#define RELAYSTATUS_FL           0x03   Float
#define RELAYSTATUS_BF           0x04   BackFeed
#define RELAYSTATUS_BO           0x05   Blocked Open
#define RELAYSTATUS_FC           0x06   Failed to Close
#define RELAYSTATUS_NR           0x07   Relay Not Responding
#define RELAYSTATUS_IB           0x08   Insensitive BackFeed
#define RELAYSTATUS_RC           0x09   Relax Close
#define RELAYSTATUS_PA           0x0A   Pump Alarm
#define RELAYSTATUS_SL           0x0B   Safe Service Lockout
#define RELAYSTATUS_XP           0x0C   Cross Phase
#define RELAYSTATUS_PC          0x0D   Permissive Close
             */
#if DEBUG
            this.relayStatusConverter.IncomingStatusCode = bytePacket[0];
            this.toolStripStatusLabelReceiverStatus.Text = this.relayStatusConverter.CurrentStatus;
            this.toolStripStatusLabelReceiverStatus.BackColor = this.relayStatusConverter.CurrentColor;
            this.toolStripStatusLabelReceiverStatus.ForeColor = this.relayStatusConverter.CurrentForeColor;
#else
            if (bytePacket[0] == 12)
            {
                DialogResult dR = DialogResult.OK;
                this.relayStatusConverter.IncomingStatusCode = bytePacket[0];
                this.toolStripStatusLabelReceiverStatus.Text = this.relayStatusConverter.CurrentStatus;
                this.toolStripStatusLabelReceiverStatus.BackColor = this.relayStatusConverter.CurrentColor;
                this.toolStripStatusLabelReceiverStatus.ForeColor = this.relayStatusConverter.CurrentForeColor;
                this.toolStripStatusLabelReceiverStatus.Visible = true;
                if (showCrossPhaseMsgOnce == false)
                {
                    showCrossPhaseMsgOnce = true;
                    if (dR.Equals(DialogResult.OK))
                    {
                        // The DialogResult is used so that if the window hasn't returned a value yet (meaning it is still open)
                        // it won't be displayed again.
                        dR = DialogResult.None;
                        dR = MessageBox.Show("Warning Relay is detecting cross phase condition", "Cross Phase Detected!");
                    }
                }
                this.lbl_Relayststatus_SL.BackColor = Color.Transparent;
                this.lbl_Relayststatus_backfeed.BackColor = Color.Transparent;
                this.lbl_Relayststatus_FC.BackColor = Color.Transparent;
            }
            else if (bytePacket[0] == 4) // BackFeed condition BF
            {
                this.relayStatusConverter.IncomingStatusCode = bytePacket[0];
                this.toolStripStatusLabelReceiverStatus.Text = this.relayStatusConverter.CurrentStatus;
                this.toolStripStatusLabelReceiverStatus.BackColor = this.relayStatusConverter.CurrentColor;
                this.toolStripStatusLabelReceiverStatus.ForeColor = this.relayStatusConverter.CurrentForeColor;
                this.lbl_Relayststatus_backfeed.BackColor = Color.FromArgb(0, 190, 0); // rgb for dark green color 
                this.lbl_Relayststatus_FC.BackColor = Color.Transparent;
                this.lbl_Relayststatus_SL.BackColor = Color.Transparent;
            }
            else if (bytePacket[0] == 6) // Failed to Close condition FC
            {
                this.relayStatusConverter.IncomingStatusCode = bytePacket[0];
                this.toolStripStatusLabelReceiverStatus.Text = this.relayStatusConverter.CurrentStatus;
                this.toolStripStatusLabelReceiverStatus.BackColor = this.relayStatusConverter.CurrentColor;
                this.toolStripStatusLabelReceiverStatus.ForeColor = this.relayStatusConverter.CurrentForeColor;
                this.lbl_Relayststatus_FC.BackColor = Color.DarkRed; 
                this.lbl_Relayststatus_SL.BackColor = Color.Transparent;
                this.lbl_Relayststatus_backfeed.BackColor = Color.Transparent;
            }
            else if (bytePacket[0] == 11) // Safe Service Lockout condition SL
            {
                this.relayStatusConverter.IncomingStatusCode = bytePacket[0];
                this.toolStripStatusLabelReceiverStatus.Text = this.relayStatusConverter.CurrentStatus;
                this.toolStripStatusLabelReceiverStatus.BackColor = this.relayStatusConverter.CurrentColor;
                this.toolStripStatusLabelReceiverStatus.ForeColor = this.relayStatusConverter.CurrentForeColor;
                this.lbl_Relayststatus_SL.BackColor = Color.Blue; //Color.Green;
                this.lbl_Relayststatus_backfeed.BackColor = Color.Transparent;
                this.lbl_Relayststatus_FC.BackColor = Color.Transparent;
            }
            else
            {
                showCrossPhaseMsgOnce = false;
                this.toolStripStatusLabelReceiverStatus.Visible = false;
                this.lbl_Relayststatus_SL.BackColor = Color.Transparent;
                this.lbl_Relayststatus_backfeed.BackColor = Color.Transparent;
                this.lbl_Relayststatus_FC.BackColor = Color.Transparent;
            }
#endif
        }

        private void dNPDataMessage(byte[] bytePacket, int p)
        {
            //MessageBox.Show("DNP Live Data packet received from master firmware for : " + p); // Only for testing - to be removed
            switch (p)
            {
                case 1:
                    this.dNPDIGITALGRIDData.setBinaryInputs(bytePacket);
                    break;
                case 2:
                    this.dNPDIGITALGRIDData.setBinaryOutputs(bytePacket);
                    break;
                case 3:
                    this.dNPDIGITALGRIDData.setAnalogInputs(bytePacket,13);
                    break;
                case 4:
                    this.dNPDIGITALGRIDData.setAnalogInputs(bytePacket, 14);
                    break;
                case 5:
                    this.dNPDIGITALGRIDData.setAnalogOutputs(bytePacket);
                    break;

            }
        }

        //Also for THD Values
        private double[] fFTValues = new double[128];
        private void setFFTValue(byte[] bytePacket)
        {
            try
            {
                Int32 temp;
                double real, imaginary;
                UInt16 index;
                PhasorTypes type;

                index = bytePacket[0];
                index <<= 8;
                index += bytePacket[1];

                logger.Info("FFT: " + (char)bytePacket[3] + ", " + (char)bytePacket[2]);

                type = RelayModeFunctions.PhasorTypeFrom((char)bytePacket[3], (char)bytePacket[2]);

                //this.labelFFTType.Text = type.ToString();
                temp = bytePacket[4];
                temp <<= 8;
                temp += bytePacket[5];
                temp <<= 8;
                temp += bytePacket[6];
                temp <<= 8;
                temp += bytePacket[7];

                real = (double)temp * (double)Constants.SixteenFracBits;

                if (index == 65535) //FFFF is for THD value
                {
                    this.ucPhasorGraph1.UpdateTHDValue(type, real);
                    return;
                }

                temp = bytePacket[8];
                temp <<= 8;
                temp += bytePacket[9];
                temp <<= 8;
                temp += bytePacket[10];
                temp <<= 8;
                temp += bytePacket[11];

                imaginary = (double)temp * (double)Constants.SixteenFracBits;

                real = Math.Sqrt(real * real + imaginary * imaginary);

                if (type == PhasorTypes.VnA)
                {
                    if (index >= fFTValues.Length)
                    {
                        index = (UInt16)(fFTValues.Length - 1);
                    }

                    this.fFTValues[index] = real;
                    if (index == 127)
                        index = 127;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FFTValue", ex);
            }
        }

        #region Flight Recorder Section
        private CalibrationConstant[] calibrationConstants = new CalibrationConstant[15];

        private void sendTime(DateTime dT)
        {
            byte[] sendPacket = new byte[6];
            long binaryDate = RelayModeFunctions.BinaryDate(dT);
            long temp;
            DateTime tempTime;

            sendPacket[0] = (byte)'j';
            sendPacket[1] = (byte)(binaryDate >> 24);
            sendPacket[2] = (byte)(binaryDate >> 16);
            sendPacket[3] = (byte)(binaryDate >> 8);
            sendPacket[4] = (byte)(binaryDate);
            sendPacket[5] = 0x0D;

            temp = binaryDate & 0x00000000FFFFFFFF;
            tempTime = RelayModeFunctions.DateFrom(temp);

            this.sendPacket(sendPacket);
        }

        private void storeCurrentTime(byte[] bytePacket)
        {
            DateTime tempDT = DateTime.UtcNow;
            EventBaseTime eBT = new EventBaseTime();

            UInt32 temp = bytePacket[3];
            temp <<= 8;
            temp += bytePacket[2];
            temp <<= 8;
            temp += bytePacket[1];
            temp <<= 8;
            temp += bytePacket[0];

            eBT.BinaryTime = temp;

            this.ucTimeControl1.RelayDateTimeUTC = eBT.SystemTime;
        }

        delegate void bytePacketCallback(byte[] bytePacket);

        private void storeTimesReceived(byte[] bytePacket)
        {
            if (this.InvokeRequired)
            {
                bytePacketCallback bPCB = new bytePacketCallback(storeTimesReceived);
                this.Invoke(bPCB, new object[] { bytePacket });
            }
            else
            {
                if (this.relayCodeRevisionNumber < 20111123)
                {
                    //TEST removed, not sure why it's in.  2 placed this.initialLiveEventRequest = false;
                    EventBaseTime eBT = new EventBaseTime();
                    UInt32 temp;

                    temp = bytePacket[3];
                    temp <<= 8;
                    temp += bytePacket[2];
                    temp <<= 8;
                    temp += bytePacket[1];
                    temp <<= 8;
                    temp += bytePacket[0];

                    eBT.BinaryTime = temp;

                    if (temp == 0xFFFFFFFF || temp == 0x00AE00EE)
                    {
                        this.ucEventGraph0.Type = EventTypes.NoEvent;
                    }
                    else// if (eBT.SystemTime != ucEventGraph0.EventTime)
                    {
                        this.ucEventGraph0.ClearAllGraphs();
                        this.ucEventGraph0.EventTime = eBT.SystemTime;
                        this.ucEventGraph0.Type = this.getEventType(bytePacket[4], bytePacket[5]);
                    }

                    temp = bytePacket[9];
                    temp <<= 8;
                    temp += bytePacket[8];
                    temp <<= 8;
                    temp += bytePacket[7];
                    temp <<= 8;
                    temp += bytePacket[6];

                    eBT.BinaryTime = temp;

                    if (temp == 0xFFFFFFFF || temp == 0x00AE00EE)
                    {
                        this.ucEventGraph1.Type = EventTypes.NoEvent;
                    }
                    else if (eBT.SystemTime != ucEventGraph1.EventTime)
                    {
                        this.ucEventGraph1.ClearAllGraphs();
                        this.ucEventGraph1.EventTime = eBT.SystemTime;
                        this.ucEventGraph1.Type = this.getEventType(bytePacket[10], bytePacket[11]);
                    }

                    temp = bytePacket[15];
                    temp <<= 8;
                    temp += bytePacket[14];
                    temp <<= 8;
                    temp += bytePacket[13];
                    temp <<= 8;
                    temp += bytePacket[12];

                    eBT.BinaryTime = temp;

                    if (temp == 0xFFFFFFFF || temp == 0x00AE00EE)
                    {
                        this.ucEventGraph2.Type = EventTypes.NoEvent;
                    }
                    else if (eBT.SystemTime != ucEventGraph2.EventTime)
                    {
                        this.ucEventGraph2.ClearAllGraphs();
                        this.ucEventGraph2.EventTime = eBT.SystemTime;
                        this.ucEventGraph2.Type = this.getEventType(bytePacket[16], bytePacket[17]);
                    }

                    temp = bytePacket[21];
                    temp <<= 8;
                    temp += bytePacket[20];
                    temp <<= 8;
                    temp += bytePacket[19];
                    temp <<= 8;
                    temp += bytePacket[18];

                    eBT.BinaryTime = temp;

                    if (temp == 0xFFFFFFFF || temp == 0x00AE00EE)
                    {
                        this.ucEventGraph3.Type = EventTypes.NoEvent;
                    }
                    else if (eBT.SystemTime != ucEventGraph3.EventTime)
                    {
                        this.ucEventGraph3.ClearAllGraphs();
                        this.ucEventGraph3.EventTime = eBT.SystemTime;
                        this.ucEventGraph3.Type = this.getEventType(bytePacket[22], bytePacket[23]);
                    }

                    temp = bytePacket[27];
                    temp <<= 8;
                    temp += bytePacket[26];
                    temp <<= 8;
                    temp += bytePacket[25];
                    temp <<= 8;
                    temp += bytePacket[24];

                    eBT.BinaryTime = temp;

                    if (temp == 0xFFFFFFFF || temp == 0x00AE00EE)
                    {
                        this.ucEventGraph4.Type = EventTypes.NoEvent;
                    }
                    else if (eBT.SystemTime != ucEventGraph4.EventTime)
                    {
                        this.ucEventGraph4.ClearAllGraphs();
                        this.ucEventGraph4.EventTime = eBT.SystemTime;
                        this.ucEventGraph4.Type = this.getEventType(bytePacket[28], bytePacket[29]);
                    }

                    temp = bytePacket[33];
                    temp <<= 8;
                    temp += bytePacket[32];
                    temp <<= 8;
                    temp += bytePacket[31];
                    temp <<= 8;
                    temp += bytePacket[30];

                    eBT.BinaryTime = temp;

                    if (temp == 0xFFFFFFFF || temp == 0x00AE00EE)
                    {
                        this.ucEventGraph5.Type = EventTypes.NoEvent;
                    }
                    else if (eBT.SystemTime != ucEventGraph5.EventTime)
                    {
                        this.ucEventGraph5.ClearAllGraphs();
                        this.ucEventGraph5.EventTime = eBT.SystemTime;
                        this.ucEventGraph5.Type = this.getEventType(bytePacket[34], bytePacket[35]);
                    }

                    temp = bytePacket[39];
                    temp <<= 8;
                    temp += bytePacket[38];
                    temp <<= 8;
                    temp += bytePacket[37];
                    temp <<= 8;
                    temp += bytePacket[36];

                    eBT.BinaryTime = temp;

                    if (temp == 0xFFFFFFFF || temp == 0x00AE00EE)
                    {
                        this.ucEventGraph6.Type = EventTypes.NoEvent;
                    }
                    else if (eBT.SystemTime != ucEventGraph6.EventTime)
                    {
                        this.ucEventGraph6.ClearAllGraphs();
                        this.ucEventGraph6.EventTime = eBT.SystemTime;
                        this.ucEventGraph6.Type = this.getEventType(bytePacket[40], bytePacket[41]);
                    }

                    temp = bytePacket[45];
                    temp <<= 8;
                    temp += bytePacket[44];
                    temp <<= 8;
                    temp += bytePacket[43];
                    temp <<= 8;
                    temp += bytePacket[42];

                    eBT.BinaryTime = temp;

                    if (temp == 0xFFFFFFFF || temp == 0x00AE00EE)
                    {
                        this.ucEventGraph7.Type = EventTypes.NoEvent;
                    }
                    else if (eBT.SystemTime != ucEventGraph7.EventTime)
                    {
                        this.ucEventGraph7.ClearAllGraphs();
                        this.ucEventGraph7.EventTime = eBT.SystemTime;
                        this.ucEventGraph7.Type = this.getEventType(bytePacket[46], bytePacket[47]);
                    }
                }
                else
                {
                    ucEventGraph workingGraph = this.ucEventGraph0;
                    int packetPtr = 0;
                    UInt32 temp;
                    EventBaseTime eBT = new EventBaseTime();

                    //TEST removed, not sure why it's in.  2 placed this.initialLiveEventRequest = false;

                    for (int i = 0; i < 8; ++i) //go through all 8 events
                    {
                        temp = bytePacket[packetPtr + 3];
                        temp <<= 8;
                        temp += bytePacket[packetPtr + 2];
                        temp <<= 8;
                        temp += bytePacket[packetPtr + 1];
                        temp <<= 8;
                        temp += bytePacket[packetPtr];

                        eBT.BinaryTime = temp;

                        if (temp == 0xFFFFFFFF || temp == 0x00AE00EE)
                        {
                            workingGraph.Type = EventTypes.NoEvent;
                        }
                        else if (eBT.SystemTime != workingGraph.EventTime)
                        {
                            workingGraph.ClearAllGraphs();
                            workingGraph.Type = this.getEventType(bytePacket[packetPtr + 4], bytePacket[packetPtr + 5]);
                            workingGraph.EventTime = eBT.SystemTime;
                        }
                        if (workingGraph.Type == EventTypes.Trip)
                        {
                            temp = bytePacket[packetPtr + 7];
                            temp <<= 8;
                            temp += bytePacket[packetPtr + 6];

                            workingGraph.DelayToBFlag = temp;

                            temp = bytePacket[packetPtr + 9];
                            temp <<= 8;
                            temp += bytePacket[packetPtr + 8];

                            workingGraph.DelayToFloat = temp;
                        }
                        workingGraph.SetLabel();
                        packetPtr += 10;
                        if (workingGraph == this.ucEventGraph0)
                        {
                            workingGraph = this.ucEventGraph1;
                        }
                        else if (workingGraph == this.ucEventGraph1)
                        {
                            workingGraph = this.ucEventGraph2;
                        }
                        else if (workingGraph == this.ucEventGraph2)
                        {
                            workingGraph = this.ucEventGraph3;
                        }
                        else if (workingGraph == this.ucEventGraph3)
                        {
                            workingGraph = this.ucEventGraph4;
                        }
                        else if (workingGraph == this.ucEventGraph4)
                        {
                            workingGraph = this.ucEventGraph5;
                        }
                        else if (workingGraph == this.ucEventGraph5)
                        {
                            workingGraph = this.ucEventGraph6;
                        }
                        else if (workingGraph == this.ucEventGraph6)
                        {
                            workingGraph = this.ucEventGraph7;
                        }
                    }
                }
                if (this.ucEventGraph0.Type == EventTypes.NoEvent)
                {
                    this.downloadProgress_Done(ProgressFormCompleteStates.Failure, "No Event To Download ");
                }
                this.requestCalibrationConstants();
            }
        }

        private void requestCalibrationConstants()
        {
            SendEventArgs sEA = new SendEventArgs(2);

            sEA.SendPacket[0] = 0x6C;  // 'l'
            sEA.SendPacket[1] = 0x0D;

            this.sendPacket(sEA.SendPacket);
        }

        private EventTypes getEventType(byte lsB, byte msB)
        {
            UInt16 temp = msB;

            temp <<= 8;
            temp += lsB;

            if ((temp & 1) == 1)
            {
                return EventTypes.Trip;
            }
            else if ((temp & 2) == 2)
            {
                return EventTypes.Close;
            }
            else if ((temp & 16) == 16)
            {
                return EventTypes.InInsensitiveRegion;
            }
            else if ((temp & 0x0200) == 0x0200)
            {
                return EventTypes.LowVoltage;
            }
            else
            {
                return EventTypes.UnidentifiedEvent;
            }
        }

        private CalibrationConstant CalVnA;
        private CalibrationConstant CalVnB;
        private CalibrationConstant CalVnC;
        private CalibrationConstant CalVtA;
        private CalibrationConstant CalVtB;
        private CalibrationConstant CalVtC;
        private CalibrationConstant CalIhA;
        private CalibrationConstant CalIhB;
        private CalibrationConstant CalIhC;
        private CalibrationConstant CalIA;
        private CalibrationConstant CalIB;
        private CalibrationConstant CalIC;

        private bool downloadingCanceled = false;

        private int eventValue = 0;
        private int downloadableEvents = 0;
        private void setCalibrationConstants(byte[] bytePacket)
        {
            eventValue = 0;
            downloadableEvents = 0;
            this.timerLiveEventAcknowledge.Enabled = false;

            focusEventGraphDownloading(eventValue);

            if (dataBackup_fromRelay == true)
            {
                byte[] snapshot = new byte[Math.Min(bytePacket.Length, 60)];
                Array.Copy(bytePacket, snapshot, snapshot.Length);
                CaptureBackupSection("Calibration Constants", snapshot);
            }

            for (int i = 0; i < 15; ++i)
            {
                if (this.calibrationConstants[i] == null)
                    this.calibrationConstants[i] = new CalibrationConstant();
                this.calibrationConstants[i].RawValue = 0;

                this.calibrationConstants[i].RawValue += (int)bytePacket[(4 * i)] << 16;
                this.calibrationConstants[i].RawValue += (int)bytePacket[(4 * i) + 1] << 24;
                this.calibrationConstants[i].RawValue += (int)bytePacket[(4 * i) + 2];
                this.calibrationConstants[i].RawValue += (int)bytePacket[(4 * i) + 3] << 8;
            }

            this.CalVnA = this.calibrationConstants[0];
            this.CalVnB = this.calibrationConstants[1];
            this.CalVnC = this.calibrationConstants[2];
            this.CalVtA = this.calibrationConstants[3];
            this.CalVtB = this.calibrationConstants[4];
            this.CalVtC = this.calibrationConstants[5];
            this.CalIhA = this.calibrationConstants[12];
            this.CalIhB = this.calibrationConstants[13];
            this.CalIhC = this.calibrationConstants[14];
            this.CalIA = this.calibrationConstants[9];
            this.CalIB = this.calibrationConstants[10];
            this.CalIC = this.calibrationConstants[11];

            if (!this.initialLiveEventRequest)
            {
                this.ucCalibration2.CalibrationConstants = this.calibrationConstants;
                this.ucCalibration2.PopulateTextBox();
                return;
            }
            this.initialLiveEventRequest = false;


            this.ucLiveData1.CalConstantIA = this.CalIA;
            this.ucLiveData1.CalConstantIB = this.CalIB;
            this.ucLiveData1.CalConstantIC = this.CalIC;
            this.ucLiveData1.CalConstantVnA = this.CalVnA;
            this.ucLiveData1.CalConstantVnB = this.CalVnB;
            this.ucLiveData1.CalConstantVnC = this.CalVnC;
            this.ucLiveData1.CalConstantVtA = this.CalVtA;
            this.ucLiveData1.CalConstantVtB = this.CalVtB;
            this.ucLiveData1.CalConstantVtC = this.CalVtC;

            this.setEventCalConstants(this.ucEventGraph0);
            this.setEventCalConstants(this.ucEventGraph1);
            this.setEventCalConstants(this.ucEventGraph2);
            this.setEventCalConstants(this.ucEventGraph3);
            this.setEventCalConstants(this.ucEventGraph4);
            this.setEventCalConstants(this.ucEventGraph5);
            this.setEventCalConstants(this.ucEventGraph6);
            this.setEventCalConstants(this.ucEventGraph7);

            if (!this.downloadingLiveData)
            {
                this.requestEventData(eventValue);
                setEventDownloadNumber();
                setEventTimer();
            }
            else
            {
                this.acknowledge();
                this.timerLiveEventAcknowledge.Start();
            }
        }

        void setEventDownloadNumber()
        {
            if (this.ucEventGraph0.Type == EventTypes.NoEvent)
                downloadableEvents = 0;
            else if (this.ucEventGraph1.Type == EventTypes.NoEvent)
                downloadableEvents = 1;
            else if (this.ucEventGraph2.Type == EventTypes.NoEvent)
                downloadableEvents = 2;
            else if (this.ucEventGraph3.Type == EventTypes.NoEvent)
                downloadableEvents = 3;
            else if (this.ucEventGraph4.Type == EventTypes.NoEvent)
                downloadableEvents = 4;
            else if (this.ucEventGraph5.Type == EventTypes.NoEvent)
                downloadableEvents = 5;
            else if (this.ucEventGraph6.Type == EventTypes.NoEvent)
                downloadableEvents = 6;
            else if (this.ucEventGraph7.Type == EventTypes.NoEvent)
                downloadableEvents = 7;
            else
                downloadableEvents = 8;
        }

        void setEventTimer()
        {
            int eventDownloadTime = 50;

            eventDownloadTime = getEventDownloadTime();
            if (ucEventGraph0.Type != EventTypes.NoEvent)
                this.downloadingDialogCountDown("Downloading", "Downloading ALL Events", eventDownloadTime, false);
        }

        private void setEventCalConstants(ucEventGraph ucEventGraph)
        {
            ucEventGraph.CalConstants.IA = this.CalIA;
            ucEventGraph.CalConstants.IB = this.CalIB;
            ucEventGraph.CalConstants.IC = this.CalIC;
            ucEventGraph.CalConstants.VnA = this.CalVnA;
            ucEventGraph.CalConstants.VnB = this.CalVnB;
            ucEventGraph.CalConstants.VnC = this.CalVnC;
            ucEventGraph.CalConstants.VtA = this.CalVtA;
            ucEventGraph.CalConstants.VtB = this.CalVtB;
            ucEventGraph.CalConstants.VtC = this.CalVtC;
        }

        private byte ackCount = 0;

        private void acknowledge()
        {
            if (this.portClosing)
                return;

            byte[] sendPacket = new byte[3];

            sendPacket[0] = 0x06;
            sendPacket[1] = ackCount;
            sendPacket[2] = 0x0D;

            ackCount++;

            this.sendPacket(sendPacket);
        }

        private bool portClosing = false;

        private void nAcknowledge()
        {
            this.resetSerialPort();

            byte[] sendPacket = new byte[2];

            sendPacket[0] = 0x15;
            sendPacket[1] = 0x0D;

            this.sendPacket(sendPacket);
        }

        #endregion

        private Thread closePort;

        private void resetSerialPort()
        {
            if (tCPConnection)
                return;
            string tempPortName = this.serialPort1.PortName;
            int baudRate = this.serialPort1.BaudRate;

            this.timerLiveEventAcknowledge.Enabled = false;
            if (this.portClosing == true)
                return;

            this.portClosing = true;
            if (this.serialPort1 != null)
            {
                this.serialPort1.DiscardInBuffer();
                this.serialPort1.DiscardOutBuffer();
                this.serialPort1.Close();
                this.serialPort1 = null;
            }

            this.portClosing = false;
            if (serialPort1 == null)
            {
                this.serialPort1 = new MyPort(this.components);
                this.serialPort1.BaudRate = baudRate;
                this.serialPort1.PortName = tempPortName;
                if (!this.serialPort1.IsOpen)
                {
                    this.serialPort1.Open();
                    this.clearSerialPortBuffers(this.serialPort1);
                }
                this.serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
            }
            else if (!this.serialPort1.IsOpen)
            {
                this.serialPort1.Open();
                this.clearSerialPortBuffers(this.serialPort1);
                this.serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
            }

            this.timerLiveEventAcknowledge.Enabled = true;
        }

        private void clearSerialPortBuffers(MyPort myPort)
        {
            if (myPort != null && myPort.IsOpen)
            {
                myPort.DiscardInBuffer();
                myPort.DiscardOutBuffer();
            }
        }

        /*
        private void closePortThread()
        {
            try
            {
                if (this.serialPort1 != null)
                {
                    this.serialPort1.DiscardInBuffer();
                    this.serialPort1.DiscardOutBuffer();
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Closing Port", ex);
            }

            try
            {
                this.BeginInvoke(new EventHandler(portClosed));
            }
            catch (Exception ex)
            {
                this.messageHandler("Error invoking port closure finished", ex);
            }

        }
        */
        private void portClosed(object sender, EventArgs e)
        {
            this.timerLiveEventAcknowledge.Enabled = false;
            this.portClosing = false;
        }

        private void showNoMemFixMessage(byte[] bytePacket)
        {
            if (showMemFixMsg)
            {
                showMemFixMsg = false;
                MessageBox.Show("Hardware incompatible with DNP. Return to vendor for UPGRADE", "Hardware needs to be UPDATED!");
            }
        }

        private void UcRelayProgramming1_AutoloadDeclined(object sender, EventArgs e)
        {
            // MainControl reacts only; it does not re-authorize or re-decline.
            logger.Info("MainControl: autoload declined by ucRelayProgramming; no second-cycle decision applied here.");
            // If you still need UI cleanup, do only minimal UI state cleanup here.
            // Do not reset or re-open programming approval logic in MainControl.
        }


        private void setRelayRevisionLabel(byte[] bytePacket)
        {
            try
            {
                string temp;
                temp = ASCIIEncoding.ASCII.GetString(bytePacket);
                string temp2 = temp.Remove(0, 23);
                this.relayCodeRevisionNumber = Convert.ToUInt32(temp2);
                logger.Info("Relay Revision: {0}", relayCodeRevisionNumber);

                this.ucPhasorGraph1.RevisionNumber = this.relayCodeRevisionNumber;
                this.ucPumpMode1.RelayRevisionNumber = this.relayCodeRevisionNumber;
                this.ucCloseMode1.RelayRevisionNumber = this.relayCodeRevisionNumber;
                this.ucEventGraph0.RelayRevisionNumber = this.relayCodeRevisionNumber;
                this.ucEventGraph1.RelayRevisionNumber = this.relayCodeRevisionNumber;
                this.ucEventGraph2.RelayRevisionNumber = this.relayCodeRevisionNumber;
                this.ucEventGraph3.RelayRevisionNumber = this.relayCodeRevisionNumber;
                this.ucEventGraph4.RelayRevisionNumber = this.relayCodeRevisionNumber;
                this.ucEventGraph5.RelayRevisionNumber = this.relayCodeRevisionNumber;
                this.ucEventGraph6.RelayRevisionNumber = this.relayCodeRevisionNumber;
                this.ucEventGraph7.RelayRevisionNumber = this.relayCodeRevisionNumber;
                this.ucRelayProgramming1.RelayRevisionNumber = this.relayCodeRevisionNumber;

                this.setLabelText(temp, this.labelRelayRevision);

                if (this.ProgramState == ProgramStates.DownloadingAllParameters)
                {
                    logger.Trace("Downloading All Parameters, loading new code: {0}", loadingNewCode);
                    this.timerResponseTimeOut.Enabled = false;
                    this.dataRetryCount = 0;
                    if (!loadingNewCode)
                        this.requestFPGARevision();
                }

                if (temp.Contains("GE"))
                {
                    this.GERelay = true;
                }
                else if (temp.Contains("WH"))
                {
                    this.GERelay = false;
                }
                else
                    this.GERelay = false;

                if (this.relayCodeRevisionNumber < 20130111)
                {
                    this.ucSafeService1.Visible = false;
                    // Added this for potential upgrade of software
                    this.ucSafeService1.LoadingNewCode = true;
                }
                else
                    this.ucSafeService1.Visible = true;
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Relay Revision Label", ex);
            }
        }

        private int savedSerialNumber = 0;
        private bool checkSerialNumber = false;

        private void setTransmitterSettings(byte[] bytePacket)
        {
            byte[] settings = new byte[bytePacket.Length];
            int tempI = 0;

            timerResponseTimeOut.Enabled = false;

            // update lightning count
            this.lbl_LightningCount.Text = lightC.lightningCount.ToString();

            this.ucTransmitter1.PacketLength = bytePacket.Length;
            //Thread.Sleep(1000);   // delay 1second
            if (dataBackup_fromRelay == true)
            {
                byte[] snapshot = new byte[Math.Min(bytePacket.Length, 32)];
                Array.Copy(bytePacket, snapshot, snapshot.Length);
                CaptureBackupSection("Transmitter Parameters", snapshot);
            }

            for (int i = 0; i < settings.Length; ++i)
            {
                settings[i] = bytePacket[i];
            }

            try
            {
                this.CTRatio = ((Int16)settings[7]) << 8;
                this.CTRatio += settings[6];

                this.updateCTRatio(this.CTRatio);
                this.updateCTRatioDomain(this.CTRatio, this.comboBox_CTRatio.SelectedIndex);
            }
            catch
            {
                dataBackupNW.dataBackup_nwProtectorDefaults = true;
                this.updateCTRatio(320);
                this.messageHandler("Bad CT Ratio", "Please resend correct CT Ratio");

                // Set defaults directly (no UI click-handler call)
                this.restoreDefaultsTypeAndPhasing();

                // Send once through the guarded ACK path
                this.SendCTRatioAndPhasing(requestAfter: false);
            }

            try
            {
                //Transmitter Page
                //ID
                tempI = bytePacket[1];
                tempI <<= 8;
                tempI += bytePacket[0];

                this.ucTransmitterMonitoring1.TransmitterID = tempI.ToString();

                //Serial Number
                tempI = bytePacket[3];
                tempI <<= 8;
                tempI += bytePacket[2];

                this.ucRelayProgramming1.SerialNumber = (UInt32)tempI;
                this.ucDNPSAv51.SerialNumber = tempI;

                // 3076 indicates default/fallback serial due to bad memory; do not use for GE/WH mismatch checks
                if (tempI != 3076)
                {
                    if (tempI >= 25000 && !GERelay)
                    {
                        messageHandler("GE Serial Number programmed with WH Firmware",
                            "Is this a GE Relay? If Yes, please manually reload with GE Software. If No, please contact DIGITALGRID");
#if !DEBUG
                        enableAll(false);
#endif
                    }
                    else if (tempI < 25000 && GERelay)
                    {
                        messageHandler("WH Serial Number programmed with GE Firmware",
                            "Is this a WH Relay? If Yes, please manually reload with WH Software. If No, please contact DIGITALGRID");
#if !DEBUG
                        enableAll(false);
#endif
                    }
                }

                if (this.savedSerialNumber != tempI && checkSerialNumber)
                {
                    this.checkSerialNumber = false;
                    this.savedSerialNumber = tempI;

                    logger.Warn(
                        "Serial number mismatch detected. savedSerialNumber={0}, newSerialNumber={1}. Triggering resync guard.",
                        this.savedSerialNumber,
                        tempI);

                    this.messageHandler(
                        "Relay serial number changed",
                        $"Expected serial {this.savedSerialNumber}, received {tempI}. Re-sync required.");

                    return;
                }

                this.checkSerialNumber = false;
                this.savedSerialNumber = tempI;

                //this.ucTransmitterMonitoring1.TransmitterSN = tempI.ToString();
                //CONED asked to display serial numbers in range of 900001 to 965535
                //Just so they can distinguish DGI relays
#if CONED
                /* this.ucTransmitterMonitoring1.TransmitterSN = (900000 + tempI).ToString();
                 this.textBoxRelaySNControl.Text = (900000+tempI).ToString();
                */
                //this.ucTransmitterMonitoring1.TransmitterSN = tempI.ToString();
                //this.textBoxRelaySNControl.Text = tempI.ToString();
                this.ucTransmitterMonitoring1.TransmitterSN = (900000 + tempI).ToString();
                this.textBoxRelaySNControl.Text = (900000 + tempI).ToString();
#else
                this.ucTransmitterMonitoring1.TransmitterSN = tempI.ToString();
                this.textBoxRelaySNControl.Text = tempI.ToString();
#endif
                this.textBoxRelaySNControlPQ.Text = textBoxRelaySNControl.Text;

                //Transmitter CT Ratio
                tempI = bytePacket[5];
                tempI <<= 8;
                tempI += bytePacket[4];

                this.ucTransmitterMonitoring1.CTMult = tempI.ToString();

                // TX uplink feature bit (independent from DNP comm capability)
                bool txUplinkFeatureEnabled = (bytePacket[28] & 0x04) == 0x04;

                this.ucTransmitter1.DNPEnabled = txUplinkFeatureEnabled;

                // DNP comm capability / status
                if (this.Customer == Customers.TORONTO_HYDRO)
                {
                    this.DNPEnabled = true;
                }
                else
                {
                    this.DNPEnabled = IsDnpCommSupported();
                }

                if ((bytePacket[28] & 0x08) == 0x08)
                {
                    this.TransmitterEnabled = true;
                }
                else
                {
                    if (this.fPGARevisionValid)
                    {
                        this.ucTransmitter1.FPGARevisionValid = true;
                        this.transmitterEnabled = true;
                    }
                    else
                    {
                        this.ucTransmitter1.FPGARevisionValid = false;
                        this.TransmitterEnabled = false;
                    }
                }

                if (this.masterRevision < 110602)
                {
                    this.GERelay = false;
                }

                this.setMonitoringPageFrequency(RelayModeFunctions.FrequencyFrom(bytePacket[8]));

                this.ucTransmitter1.SetAllValues(settings);


                // Pass the settings to the Programming part so it can update,  if need be
                if (this.ucRelayProgramming1.TransmitterPacket == null)
                    this.ucRelayProgramming1.TransmitterPacket = settings;

                if (this.ProgramState == ProgramStates.DownloadingAllParameters)
                {
                    if (this.relayCodeRevisionNumber >= 20130111)
                    {
                        if (dataBackup_fromRelay != true)
                            this.requestSafeServiceSettings();
                    }
                    else
                        this.parametersFinishedLoading();
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error in Transmitter Settings.  Please check and resend", ex);

                tempI = bytePacket[3];
                tempI <<= 8;
                tempI += bytePacket[2];

                if (tempI == 0 || tempI == 0xFFFF)
                {
                    tempI = 0;
                    this.messageHandler("Bad Serial Number", "Please Contact DIGITALGRID, INC.");
                }
                this.ucTransmitter1.SerialNumber = tempI;
                this.ucRelayProgramming1.SerialNumber = (UInt32)tempI;
                //this.textBoxTransmitterSN.Text = tempI.ToString();
                this.ucTransmitterMonitoring1.TransmitterSN = tempI.ToString();
                this.ucTransmitterMonitoring1.TransmitterSN = tempI.ToString();
                this.textBoxRelaySNControl.Text = tempI.ToString();
                this.textBoxRelaySNControlPQ.Text = textBoxRelaySNControl.Text;

                this.ucTransmitter1.SetDefaults(tempI);
                this.ucTransmitter1.SendTransmitterSettingsNew();
            }
        }

        private bool paramsReceivedLock = false;
        private void parametersFinishedLoading()
        {
            logger.Trace("Parameters Finished Loading");
            logger.Info(
                $"parametersFinishedLoading ENTER: requestedAllParameters={this.requestedAllParameters}, " +
                $"ProgramState={this.ProgramState}, pendingRestoreAfterProgramming={this.pendingRestoreAfterProgramming}, " +
                $"pendingAutoloadAfterBackup={this.pendingAutoloadAfterBackup}, " +
                $"backupInProgress={this.backupInProgress}, " +
                $"reprogrammingInProgress={this.ucRelayProgramming1.ReprogrammingInProgress}, " +
                $"loadingNewCode={this.loadingNewCode}");

            bool isFullSyncWorkflow =
                this.requestedAllParameters ||
                this.ProgramState == ProgramStates.DownloadingAllParameters ||
                this.pendingAutoloadAfterBackup ||
                this.pendingRestoreAfterProgramming ||
                this.backupInProgress ||
                this.ucRelayProgramming1.ReprogrammingInProgress;

            if (!isFullSyncWorkflow)
            {
                logger.Info(
                    "Bypassing full parameter completion for targeted config change. requestedAllParameters={0}, ProgramState={1}, sendAll={2}, sendAllFlag={3}, pendingAutoloadAfterBackup={4}, reprogrammingInProgress={5}, loadingNewCode={6}, pendingRestoreAfterProgramming={7}",
                    this.requestedAllParameters,
                    this.ProgramState,
                    this.sendAll,
                    sendAllF.SendAllFlag,
                    this.pendingAutoloadAfterBackup,
                    this.ucRelayProgramming1.ReprogrammingInProgress,
                    this.loadingNewCode,
                    this.pendingRestoreAfterProgramming
                );
                return;
            }

            if (this.badDataDetected == true)
            {
                this.parametersLoaded = false;
                this.messageHandler("Error", "Parameters Not Loaded Successfully");
                ResetFullParameterDownloadState("parametersFinishedLoading.badDataDetected");
                return;
            }

            // Only a real full-sync state should trigger the success popup.
            if (this.parametersLoaded)
            {
                // We deliberately DO NOT reset parametersLoaded here
                // because it is the one-shot latch for this full-download cycle.
                this.messageHandler("Parameters Loaded", "Parameters Loaded Successfully");
                ResetFullParameterDownloadState("parametersFinishedLoading.parametersLoaded");
                return;
            }

            if (paramsReceivedLock)
                return;

            this.requestedAllParameters = false;
            this.timerResponseTimeOut.Enabled = false;
            this.ProgramState = ProgramStates.Running;
            paramsReceivedLock = true;

            try
            {
                if (pendingRestoreAfterProgramming)
                {
                    logger.Info("POST-PROGRAM RESTORE: entering pendingRestoreAfterProgramming block");

                    this.enableAll(false);
                    Application.UseWaitCursor = true;
                    System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;

                    logger.Info("POST-PROGRAM RESTORE: before WriteBackUpData_FileToRelay");
                    this.WriteBackUpData_FileToRelay();
                    logger.Info("POST-PROGRAM RESTORE: after WriteBackUpData_FileToRelay");

                    dataB.oldDataBackup = false;

                    backupInProgress = false;
                    pendingAutoloadAfterBackup = false;
                    pendingRestoreAfterProgramming = false;
                    backupGotRelayParams = false;

                    if (backupTimeoutTimer != null)
                        backupTimeoutTimer.Stop();

                    this.ucRelayProgramming1.ReprogrammingInProgress = false;

                    MessageBox.Show("Programming complete", "Programming Complete");

                    this.ucRelayProgramming1.HideProgrammingProgress();

                    this.monitoring(true);
                    this.RegisterPolling(true);
                    this.requestRelayRegisters();

                    ResetFullParameterDownloadState("parametersFinishedLoading.pendingRestoreAfterProgramming");
                    return;
                }

                if (backupInProgress)
                {
                    logger.Info("parametersFinishedLoading: backup still in progress; deferring autoload/normal comms decision.");
                    return;
                }

                if (this.ucRelayProgramming1.ReprogrammingInProgress &&
                    !pendingRestoreAfterProgramming &&
                    !pendingAutoloadAfterBackup)
                {
                    logger.Info("Parameters finished loading during active reprogramming with no pending restore/autoload continuation; skipping autoload re-entry.");
                    return;
                }

                if (!pendingAutoloadAfterBackup)
                {
                    if (this.ucRelayProgramming1.ReprogrammingInProgress || this.loadingNewCode)
                    {
                        logger.Info("Parameters finished loading but programming transition is still active; suppressing normal communications.");
                    }
                    else
                    {
                        logger.Info("Parameters finished loading with no pending autoload continuation; restoring normal communications only.");
                        this.monitoring(true);
                        this.RegisterPolling(true);
                        this.requestRelayRegisters();
                    }

                    ResetFullParameterDownloadState("parametersFinishedLoading.noPendingAutoloadAfterBackup");
                    return;
                }

                if (skipAutoloadAfterDecline)
                {
                    logger.Info("User declined autoload; clearing pending migration state and restoring normal communications.");
                    pendingAutoloadAfterBackup = false;

                    this.monitoring(true);
                    this.RegisterPolling(true);
                    this.requestRelayRegisters();

                    skipAutoloadAfterDecline = false;

                    ResetFullParameterDownloadState("parametersFinishedLoading.skipAutoloadAfterDecline");
                    return;
                }

                pendingAutoloadAfterBackup = false;

                logger.Info("CALLER: parametersFinishedLoading -> InitializeAutoload()");
                bool autoloadAccepted = this.ucRelayProgramming1.InitializeAutoload();

                if (!autoloadAccepted)
                {
                    logger.Info("User declined autoload during pending update check. Restoring normal communications.");
                    skipAutoloadAfterDecline = true;

                    this.monitoring(true);
                    this.RegisterPolling(true);
                    this.requestRelayRegisters();

                    ResetFullParameterDownloadState("parametersFinishedLoading.autoloadDeclined");
                    return;
                }

                if (this.pendingAutoloadAfterBackup ||
                    this.ucRelayProgramming1.ReprogrammingInProgress ||
                    this.loadingNewCode)
                {
                    logger.Info("InitializeAutoload() completed but normal communications remain suppressed for programming transition.");
                    ResetFullParameterDownloadState("parametersFinishedLoading.programmingTransitionStillActive");
                    return;
                }
                else
                {
                    logger.Info("InitializeAutoload() completed without a pending backup continuation. Restoring normal communications.");
                    this.monitoring(true);
                    this.RegisterPolling(true);
                    this.requestRelayRegisters();

                    ResetFullParameterDownloadState("parametersFinishedLoading.autoloadCompleted");
                    return;
                }
            }
            finally
            {
                paramsReceivedLock = false;
            }
        }

        private bool IsRelayProgrammingLifecycleActive()
        {
            return this.ucRelayProgramming1 != null &&
                   (this.ucRelayProgramming1.ReprogrammingInProgress ||
                    this.ucRelayProgramming1.ProgramBootCodeInProgress ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.LoadingMasterBootLoader ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.LoadingMasterCode ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.LoadingMasterData ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.LoadingRelayCode ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.LoadingRelayData ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.LoadingFPGACode ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.WaitingForBootMaster ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.WaitingForBootRelay ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.WaitingForBootFPGA ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.AutoLoadCheckBoot ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.ManualLoadCheckBoot ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.CheckMasterBootCode ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.ReloadMasterBoot ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.ReprogramSuccess ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.Finalized ||
                    this.ucRelayProgramming1.State == RelayProgrammingStates.WaitForAllData);
        }

        /*
        private void startMonitoringWBSettings()
        {
            this.groupBoxLRLockoutMain.Visible = false;
        }

        private void CheckTransmitterTab()
        {
            if (this.tabControlMain.SelectedTab == this.tabPageTransmitter)
                tabControlMain_SelectedIndexChanged(null, null);
        }
        */

        //private void updateCTRatioDomain(int CT_ratio, DomainUpDown dUP)
        private void updateCTRatioDomain(int CT_ratio, int comboBoxCT)
        {
            switch (CT_ratio)
            {
                case 160:
                    //this.setDomainIndex(7, comboBoxCT);
                    this.comboBox_CTRatio.SelectedIndex = 7;
                    break;
                case 240:
                    //this.setDomainIndex(6, comboBoxCT);
                    this.comboBox_CTRatio.SelectedIndex = 6;
                    break;
                case 320:
                    //this.setDomainIndex(5, comboBoxCT);
                    this.comboBox_CTRatio.SelectedIndex = 5;
                    break;
                case 400:
                    //this.setDomainIndex(4, comboBoxCT);
                    this.comboBox_CTRatio.SelectedIndex = 4;
                    break;
                case 500:
                    //this.setDomainIndex(3, comboBoxCT);
                    this.comboBox_CTRatio.SelectedIndex = 3;
                    break;
                case 600:
                    //this.setDomainIndex(2, comboBoxCT);
                    this.comboBox_CTRatio.SelectedIndex = 2;
                    break;
                case 700:
                    //this.setDomainIndex(1, comboBoxCT);
                    this.comboBox_CTRatio.SelectedIndex = 1;
                    break;
                case 750:
                    //this.setDomainIndex(0, comboBoxCT);
                    this.comboBox_CTRatio.SelectedIndex = 0;
                    break;
                default:
                    //this.setDomainIndex(8, comboBoxCT);
                    this.comboBox_CTRatio.SelectedIndex = 8;
                    break;
            }
        }

        private delegate void setDomainIndexCallBack(int i, DomainUpDown dUP);

        private void setDomainIndex(int i, DomainUpDown dUP)
        {
            if (dUP.InvokeRequired)
            {
                setDomainIndexCallBack sTB = new setDomainIndexCallBack(setDomainIndex);
                this.Invoke(sTB, new object[] { i, dUP });
            }
            else
            {
                dUP.SelectedIndex = i;
            }
        }

        private delegate void setListBoxIndexCallBack(int selectedIndex, ListBox p);

        private void setListBoxIndex(int selectedIndex, ListBox p)
        {
            if (p.InvokeRequired)
            {
                setListBoxIndexCallBack sLBI = new setListBoxIndexCallBack(this.setListBoxIndex);
                this.Invoke(sLBI, new object[] { selectedIndex, p });
            }
            else
            {
                p.SelectedIndex = selectedIndex;
            }
        }

        private delegate void showLabelCallBack(bool b, Label l);

        private void showLabel(bool b, Label l)
        {
            try
            {
                if (l.InvokeRequired)
                {
                    showLabelCallBack sLCB = new showLabelCallBack(this.showLabel);
                    this.Invoke(sLCB, new object[] { b, l });
                }
                else
                {
                    l.Visible = b;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error on Label", "Error Invoking Lable Visibility on: " + l.Name + " " + ex.Message);
            }
        }

        private delegate void showTSSLCallBack(bool b, ToolStripStatusLabel tSSL);

        private void showLabel(bool b, ToolStripStatusLabel tSSL)
        {
            try
            {
                if (this.statusStripMain.InvokeRequired)
                {
                    showTSSLCallBack sLCB = new showTSSLCallBack(this.showLabel);
                    this.Invoke(sLCB, new object[] { b, tSSL });
                }
                else
                {
                    tSSL.Visible = b;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error on Label", "Error Invoking Label Visibility on: " + tSSL.Name + " " + ex.Message);
            }
        }

        private void setDNPShortucutCheck()
        {
            this.checkedDNPEnable = true;
        }

        private void checkDNPEnabled()
        {
            bool dnpCommSupported = IsDnpCommSupported();

            this.DNPEnabled = dnpCommSupported;

            if (!dnpCommSupported &&
                (ucTransmitter1.DNPEnabled || ucTransmitter1.CheckDNPEnable))
            {
                this.ucTransmitter1.DNPEnabled = false;
            }
        }

        private void removeDNPTabs()
        {
            if (this.tabControlMain.TabPages.Contains(this.tabPageDNPSecureAuth))
                this.tabControlMain.TabPages.Remove(this.tabPageDNPSecureAuth);

            if (this.tabControlMain.TabPages.Contains(this.tabPageDNP))
                this.tabControlMain.TabPages.Remove(this.tabPageDNP);

            if (this.tabControlMain.TabPages.Contains(this.tabPageDNPData))
                this.tabControlMain.TabPages.Remove(this.tabPageDNPData);

            DisposeDnpLiveDataControl();
            this.enableDNPMonitoring(false);
        }

        private void setRelayRegisters(byte[] bytePacket)
        {
            this.expectingRelayRegisters = false;
            if (ucRelayProgramming1.ProgramBootCodeInProgress == true)
            {
                this.quietMode = true;
                this.pauseMonitoring = true;
                return;
            }


            setRelayStatusLabels(bytePacket);
        }

        private void setRelayStatusLabels(byte[] bytePacket)
        {
            this.registersReceived = true;
            this.showLabel(false, this.labelRelayDisconnected);
            this.showLabel(false, this.labelRelayDisconnected2);
            this.showLabel(false, this.labelRelayDisconnected3);
            this.showLabel(false, this.toolStripStatusLabelRelayDisconnected);

            this.missedMonitoringCount = 0;
            this.ucShortRange1.relayFound_forRNCMonitoring = true;
            this.relayFound_forDNPdataMonitoring = true;
            if (this.checkSerialNumber == true)
                this.requestTransmitterSettings();

            this.enableFlagsAndStatus(true);
            try
            {
                this.uc8CheckBoxFlagsRelayFlags1.SetValues(bytePacket[1]);
                this.uc8CheckBoxFlagsRelayFlags2.SetValues(bytePacket[0]);
                this.uc8CheckBoxFlagsRelayStatus1.SetValues(bytePacket[3]);//
                this.uc8CheckBoxFlagsRelayStatus2.SetValues(bytePacket[2]);//
                this.uc8CheckBoxFlagsCommFlags1.SetValues(bytePacket[5]);
                this.uc8CheckBoxFlagsCommFlags2.SetValues(bytePacket[4]);
                this.uc8CheckBoxFlagsGEControl1.SetValues(bytePacket[7]);
                this.uc8CheckBoxFlagsGEControl2.SetValues(bytePacket[6]);

                if ((bytePacket[5] & 16) == 16) // Insensitive Backfeed
                    this.lbl_Relayststatus_Ib.BackColor = Color.LightGreen; 
                else if ((bytePacket[5] & 16) != 16) // Not in Insensitive Backfeed
                    this.lbl_Relayststatus_Ib.BackColor = Color.Transparent;

                if ((bytePacket[2] & 64) == 64) // Phasing OK
                    this.lbl_Relayststatus_XP.BackColor = Color.FromArgb(64, 64, 64); // rgb for dark grey color 

                if ((bytePacket[5] & 2) == 2) // Close
                    this.lbl_Relayststatus_Close.BackColor = Color.Red; 

                if ((bytePacket[5] & 32) == 32) // Relax Close
                {
                    this.lbl_Relayststatus_RC.BackColor = Color.FromArgb(255, 204, 204); // rgb for light red color 
                    modeRC.relaxMode = true;
                }
                else if ((bytePacket[5] & 32) != 32) // Not in Relax Close Mode
                    modeRC.relaxMode = false;

                byte b = bytePacket[0];
                if ((b & 128) == 128)
                {
                    this.setCheckedValue(true, this.checkBoxInTripRegion);
                    this.toolTip.SetToolTip(this.checkBoxInTripRegion, "Trip Phasor currently in Trip Region");
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxInTripRegion);
                    this.toolTip.SetToolTip(this.checkBoxInTripRegion, "Trip Phasor currently not in Trip Region");
                }
                if ((b & 64) == 64)
                {
                    this.setCheckedValue(true, this.checkBoxInInsensRegion);
                    this.toolTip.SetToolTip(this.checkBoxInInsensRegion, "Trip Phasor currently in Insensitive Region");
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxInInsensRegion);
                    this.toolTip.SetToolTip(this.checkBoxInInsensRegion, "Trip Phasor currently not in Insensitive Region");
                }
                if ((b & 32) == 32)
                {
                    // this.setCheckedValue(true, this.checkBoxBFlag);
                    // this.labelNWPStatus.Text = "NWP: Open";
                    this.txtBox_NWPposition.Text = "Open";
                }
                else
                {
                    // this.setCheckedValue(false, this.checkBoxBFlag);
                    //this.labelNWPStatus.Text = "NWP: Closed";
                    this.txtBox_NWPposition.Text = "Closed";
                }
                if ((b & 16) == 16)
                {
                    this.setCheckedValue(true, this.checkBoxFlag2);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxFlag2);
                }
                if ((b & 8) == 8)
                {
                    this.setCheckedValue(true, this.checkBoxFlag1);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxFlag1);
                }
                if ((b & 1) == 1)
                {
                    RelayStatus.ACB = true;
                }
                else
                {
                    RelayStatus.ACB = false;
                }
                this.setCheckedValue(RelayStatus.ACB, this.checkBoxACB);

                b = bytePacket[1];
                if ((b & 128) == 128)
                {
                    RelayStatus.SequenceRelay = true;
                }
                else
                {
                    RelayStatus.SequenceRelay = false;
                }
                this.setCheckedValue(RelayStatus.SequenceRelay, this.checkBoxSequence);

                if ((b & 32) == 32)
                {
                    RelayStatus.MonitorPhasors = true;
                }
                else
                {
                    RelayStatus.MonitorPhasors = false;
                }
                this.setCheckedValue(RelayStatus.MonitorPhasors, this.checkBoxMonitorPhasors);

                if ((b & 16) == 16)
                {
                    RelayStatus.MathError = true;
                }
                else
                {
                    RelayStatus.MathError = false;
                }
                this.setCheckedValue(RelayStatus.MathError, this.checkBoxMathError);
                if ((b & 8) == 8)
                {
                    RelayStatus.BadOffset = true;
                    //   this.toolTip.SetToolTip(this.checkBoxDefaultsUsed, "Problem with Relay Flash detected and the Default Parameters are currently being used");
                }
                else
                {
                    RelayStatus.BadOffset = false;
                    // this.toolTip.SetToolTip(this.checkBoxDefaultsUsed, "Non Defaults Parameters being used");
                }
                // this.setCheckedValue(RelayStatus.BadOffset, this.checkBoxDefaultsUsed);
                if ((b & 4) == 4)
                {
                    RelayStatus.SafeServiceEnabled = true;
                    this.ucSafeService1.EnableSafeService = true;
                }
                else
                {
                    RelayStatus.SafeServiceEnabled = false;
                    this.ucSafeService1.EnableSafeService = false;
                }
                this.setCheckedValue(RelayStatus.SafeServiceEnabled, this.checkBoxOffsetOkay);
                if ((b & 2) == 2)
                {
                    RelayStatus.MathTimeOver = true;
                }
                else
                {
                    RelayStatus.MathTimeOver = false;
                }
                this.setCheckedValue(RelayStatus.MathTimeOver, this.checkBoxMathOverTime);

                if ((b & 1) == 1)
                {
                    RelayStatus.Pumping = true;
                    this.ucPumpMode1.PumpProtectEnabled = true;
                    //  this.toolTip.SetToolTip(this.checkBoxPumping, "Relay in Pump Protect State");
                }
                else
                {
                    RelayStatus.Pumping = false;
                    this.ucPumpMode1.PumpProtectEnabled = false;
                    //  this.toolTip.SetToolTip(this.checkBoxPumping, "Relay not in Pump Protect State");
                }
                // this.setCheckedValue(RelayStatus.Pumping, this.checkBoxPumping);

                b = bytePacket[2];

                if ((b & 128) == 128)
                {
                    RelayFlags.CalibrationMode = true;
                }
                else
                {
                    RelayFlags.CalibrationMode = false;
                }
                this.setCheckedValue(RelayFlags.CalibrationMode, this.checkBoxCalibrating);

                if ((b & 64) == 64)
                {
                    RelayFlags.PhasingOkay = true;
                    // this.toolTip.SetToolTip(this.checkBoxPhasingOkayFlag, "Relay has determined phasing of protector and it is OK");
                    this.lbl_Relayststatus_XP.BackColor = Color.Transparent;
                }
                else
                {
                    RelayFlags.PhasingOkay = false;
                    // this.toolTip.SetToolTip(this.checkBoxPhasingOkayFlag, "Relay has yet to determine phasing of the protector or it is crossed phased");
                    this.lbl_Relayststatus_XP.BackColor = Color.FromArgb(64, 64, 64); // rgb for dark grey color //Color.Green;
                }

                //this.setCheckedValue(RelayFlags.PhasingOkay, this.checkBoxPhasingOkayFlag);

                if ((b & 32) == 32)
                {
                    RelayFlags.BlockedOpen = true;
                    ucBlockControl1.RelayBlocked = true;
                }
                else
                {
                    RelayFlags.BlockedOpen = false;
                    ucBlockControl1.RelayBlocked = false;
                }
                //  this.setCheckedValue(RelayFlags.BlockedOpen, this.checkBoxBlockedOpenFlag);

                if ((b & 16) == 16)
                {
                    RelayFlags.BlockedClose = true;
                }
                else
                {
                    RelayFlags.BlockedClose = false;
                }
                this.setCheckedValue(RelayFlags.BlockedClose, this.checkBoxBlockedCloseFlag);

                if ((b & 8) == 8)
                {
                    RelayFlags.FloatCondition = true;
                    //  this.toolTip.SetToolTip(this.checkBoxFloatFlag, "Relay is currently in the Float state");
                }
                else
                {
                    RelayFlags.FloatCondition = false;
                    // this.toolTip.SetToolTip(this.checkBoxFloatFlag, "Relay not in the Float state");
                }
                //  this.setCheckedValue(RelayFlags.FloatCondition, this.checkBoxFloatFlag);

                if ((b & 4) == 4)
                {
                    RelayFlags.Tripping = true;
                    //  this.toolTip.SetToolTip(this.checkBoxTrippingFlag, "Relay is pulsing Trip Contacts");
                }
                else
                {
                    RelayFlags.Tripping = false;
                }
                // this.setCheckedValue(RelayFlags.Tripping, this.checkBoxTrippingFlag);

                if ((b & 2) == 2)
                {
                    RelayFlags.PowerSave = true;
                }
                else
                {
                    RelayFlags.PowerSave = false;
                }
                this.setCheckedValue(RelayFlags.PowerSave, this.checkBoxPowerSaveFlag);

                if ((b & 1) == 1)
                {
                    RelayFlags.Open = true;
                    // this.toolTip.SetToolTip(this.checkBoxTripFlag, "Relay is in Trip State");
                    // this.toolTip.SetToolTip(this.checkBoxTrippingFlag, "Relay is done with initial pulsing of trip contact");
                }
                else
                {
                    RelayFlags.Open = false;
                    //  this.toolTip.SetToolTip(this.checkBoxTripFlag, "Relay is not in Trip State");
                    //  this.toolTip.SetToolTip(this.checkBoxTrippingFlag, "Relay is not in Trip State");
                }
                // this.setCheckedValue(RelayFlags.Open, this.checkBoxTripFlag);

                if (this.relayCodeRevisionNumber >= 20100625)
                {
                    b = (byte)(bytePacket[3] >> 5);
                    switch (b)
                    {
                        case 0:
                            this.ucPumpMode1.PumpReason = PumpReasons.NoPump;
                            break;
                        case 1:
                            this.ucPumpMode1.PumpReason = PumpReasons.RelayCallLimit;
                            break;
                        case 2:
                            this.ucPumpMode1.PumpReason = PumpReasons.MotorPump;
                            break;
                        case 3:
                            this.ucPumpMode1.PumpReason = PumpReasons.MotorTimeout;
                            break;
                        default:
                            this.ucPumpMode1.SendPumpMode();
                            throw new Exception(b.ToString() + " is not a valid Pump Reason byte value");


                    }
                }

                //Handle two different versions of tripcount
                if (this.relayCodeRevisionNumber < 20100602)
                {
                    Int16 tripCount = bytePacket[3];

                    this.setTextBox(tripCount.ToString(), this.textBoxTripCount);
                }
                else
                {
                    b = bytePacket[3];

                    this.setPumpReason(bytePacket[3]);
                    UInt32 cycleCount = bytePacket[8];
                    cycleCount <<= 8;
                    cycleCount += bytePacket[9];

                    this.setTextBox(cycleCount.ToString(), this.textBoxTripCount);
                }
                this.toolTip.SetToolTip(this.textBoxTripCount, "Number of Times Relay has called for a Trip and a Close");

                if (this.RelayFlags.Tripping || this.RelayFlags.Open)
                {
                    setLabelText("Open", this.labelRelayTrippedOrClose);
                    setBackgroundColor(Color.Green, this.labelRelayTrippedOrClose);
                    
                    this.lbl_Relaystatus_Open.BackColor = Color.FromArgb(0, 255, 0); // rgb for green color
                    this.lbl_Relayststatus_Float.BackColor = Color.Transparent;
                    this.lbl_Relayststatus_RC.BackColor = Color.Transparent;
                    this.lbl_Relayststatus_Close.BackColor = Color.Transparent;
                }
                else if ((this.RelayFlags.FloatCondition) && (modeRC.relaxMode == false)) //else if (this.RelayFlags.FloatCondition)
                {
                    setLabelText("Float", this.labelRelayTrippedOrClose);
                    setBackgroundColor(Color.Yellow, this.labelRelayTrippedOrClose);
                    
                    this.lbl_Relaystatus_Open.BackColor = Color.Transparent;
                    this.lbl_Relayststatus_Float.BackColor = Color.Yellow; //Color.Green;
                    this.lbl_Relayststatus_RC.BackColor = Color.Transparent;
                    this.lbl_Relayststatus_Close.BackColor = Color.Transparent;
                }
                else
                {
                    setLabelText("Close", this.labelRelayTrippedOrClose);
                    setBackgroundColor(Color.Red, this.labelRelayTrippedOrClose);
                   
                    this.lbl_Relaystatus_Open.BackColor = Color.Transparent;
                    this.lbl_Relayststatus_Float.BackColor = Color.Transparent;
                    /*
                                        if (this.btn_PermCl_Active.BackColor == Color.Yellow)
                                            this.lbl_Relayststatus_Close.BackColor = Color.Green;
                                        else
                                            this.lbl_Relayststatus_RC.BackColor = Color.Green;
                    */
                }

                if ((blockedO.blockedOpen == true) && (lbl_Relayststatus_Float.BackColor == Color.Green))
                {
                    this.lbl_Relayststatus_Float.BackColor = Color.Transparent;
                    this.lbl_Relayststatus_BO.BackColor = Color.Transparent;
                    this.lbl_Relayststatus_FB.BackColor = Color.Green;
                }
                else if ((blockedO.blockedOpen == true) && (lbl_Relayststatus_Float.BackColor != Color.Green))
                {
                    this.lbl_Relayststatus_Float.BackColor = Color.Transparent;
                    this.lbl_Relayststatus_BO.BackColor = Color.LightBlue; //Color.Green;
                    this.lbl_Relayststatus_FB.BackColor = Color.Transparent;
                }
                else if (blockedO.blockedOpen == false)
                {
                    this.lbl_Relayststatus_BO.BackColor = Color.Transparent;
                    this.lbl_Relayststatus_FB.BackColor = Color.Transparent;
                }

                if (pumpOK.pumpStatus == true)
                    this.lbl_Relayststatus_PA.BackColor = Color.Transparent;
                else if (pumpOK.pumpStatus == false)
                    this.lbl_Relayststatus_PA.BackColor = Color.Orange; //Color.Green;

                b = bytePacket[3]; //   2 / 3
                if ((b & 1) == 1)
                {
                    ucRemoteCommandBlock1.CommandsBlocked = true;
                }
                else
                {
                    ucRemoteCommandBlock1.CommandsBlocked = false;
                }




                b = bytePacket[5]; // this byte will probably have the PC bit


                if ((b & 32) == 32)
                {
                    this.ucCloseMode1.RelaxClose = true;
                }
                else
                {
                    this.ucCloseMode1.RelaxClose = false;
                }

                b = bytePacket[4];

                if ((b & 32) == 32)
                {
                    this.ucTransmitterMonitoring1.WaterBugActive = true;
                }
                else
                {
                    this.ucTransmitterMonitoring1.WaterBugActive = false;
                }

                if ((b & 16) == 16)
                {
                    this.btn_PermCl_Active.BackColor = Color.Yellow;
                }
                else
                {
                    if (btn_PermCl_Active.BackColor == Color.Yellow)
                    {
                        this.btn_PermCl_Active.BackColor = Color.Transparent;
                        this.SendPCData(); // once PC comes out of its active time, send the float time in allowed range to the master processor
                    }
                    else
                        this.btn_PermCl_Active.BackColor = Color.Transparent;
                }

            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Relay Register Values", ex);
            }

            this.requestTemperature();
        }

        private PumpReasons setPumpReason(byte p)
        {
            int pumpReason = (int)p >> 5;

            switch (pumpReason)
            {
                case 0:
                    return PumpReasons.NoPump;
                case 1:
                    return PumpReasons.RelayCallLimit;
                case 2:
                    return PumpReasons.MotorPump;
                case 3:
                    return PumpReasons.MotorTimeout;
                default:
                    this.ucPumpMode1.buttonRestoreDefaults_Click(this, new EventArgs());
                    this.ucPumpMode1.SendPumpMode();
                    throw new Exception(pumpReason.ToString() + " is a bad value for Pump Reason");
            }
        }

        private void setTemperature(byte[] bytePacket)
        {
            if (this.InvokeRequired)
            {
                bytePacketCallback bPCB = new bytePacketCallback(this.setTemperature);
                this.Invoke(bPCB, new object[] { bytePacket });
            }
            else
            {
                Int16 temperature;

                temperature = (sbyte)bytePacket[1];
                temperature <<= 8;
                temperature += bytePacket[0];
                //  this.textBoxTemperature.Text = temperature.ToString();
                this.textBoxTemperatureMonitoringPage.Text = temperature.ToString();
            }
        }

        private void clearTemperatureBoxes()
        {
            //this.textBoxTemperature.Text = "";
            this.textBoxTemperatureMonitoringPage.Text = "";
        }

        private bool parametersLoaded = false;
        private bool badDataDetected = false;

        private void setRelayParameters(byte[] bytePacket)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Int32 temp;
            decimal tempM;
            double tempD;
            byte[] closePacket = new byte[13];
            this.badDataDetected = false;

            try
            {
                if (bytePacket.Length == 95) //A ConEd Relay
                {
                    if (bytePacket[94] != 1) //A non-1 value indicates an error in the special ConEd Byte
                    {
                        this.messageHandler("Bad Relay Code", "ConEd Relay Code Mismatch");
                    }
                    else
                    {
#if !DEBUG
                        if (this.Customer != Customers.CONED) //if the GUI is not currently in ConEd mode
                        {
                            this.Customer = Customers.CONED;
                        }
#endif
                    }
                }
                else                        //a non-ConEd Relay
                {
                }

                if (dataBackup_fromRelay == true)
                {
                    byte[] snapshot = new byte[Math.Min(bytePacket.Length, 94)];
                    Array.Copy(bytePacket, snapshot, snapshot.Length);
                    CaptureBackupSection("Relay Parameters", snapshot);
                }

                //Reclose Voltage Btyes - Vertical
                temp = bytePacket[1];
                temp <<= 8;
                temp += bytePacket[0];
                tempM = (decimal)temp * Constants.TwelveFracBits;
                closePacket[0] = bytePacket[0];
                closePacket[1] = bytePacket[1];

                //Tilt Angle Bytes - Vertical
                temp = bytePacket[3];
                temp <<= 8;
                temp += bytePacket[2];
                closePacket[2] = bytePacket[2];
                closePacket[3] = bytePacket[3];

                if (temp == 0)
                {
                    tempD = 90;
                }
                else
                {
                    temp <<= 16;
                    temp >>= 16;        //converts 16 bit negative to 32 bit negative 
                    tempD = (double)((decimal)temp * Constants.EightFracBits);
                    tempD = Math.Atan(tempD);
                    tempD = RelayModeFunctions.RadiansToDegrees(tempD);
                }

                if (tempD < 0)
                {
                    tempD += 180;
                }

                //Phasing Voltage Bytes - Horizontal
                temp = bytePacket[5];
                temp <<= 8;
                temp += bytePacket[4];
                tempM = (decimal)temp * Constants.TwelveFracBits;
                closePacket[4] = bytePacket[4];
                closePacket[5] = bytePacket[5];

                //Phase Detect Angle Bytes - Horizontal
                temp = bytePacket[7];
                temp <<= 8;
                temp += bytePacket[6];
                temp <<= 16;
                temp >>= 16;     //converts 16 bit negative to 32 bit negative
                closePacket[6] = bytePacket[6];
                closePacket[7] = bytePacket[7];

                tempD = (double)((decimal)temp * Constants.TwelveFracBits);
                tempD = Math.Atan(tempD);
                tempD = RelayModeFunctions.RadiansToDegrees(tempD);

                //Close Mode

                closePacket[10] = bytePacket[8];

                //Close Time Delay Data
                temp = bytePacket[11];
                temp <<= 8;
                temp += bytePacket[10];

                closePacket[8] = bytePacket[10];
                closePacket[9] = bytePacket[11];
                closePacket[11] = bytePacket[12];
                closePacket[12] = bytePacket[13];
                
                this.ucCloseMode1.SetAllValues(closePacket);
            }
            catch (Exception ex)
            {
                this.badDataDetected = true;
                this.messageHandler("Error in Relay Close Data", ex);
                //this.resetCloseData();
                this.ucCloseMode1.sendCloseData();
            }

            try
            {
                //Set the Trip Mode
                byte[] tripPacket;
                if (this.masterRevision >= 110609)
                    tripPacket = new byte[24];
                else //old version
                    tripPacket = new byte[22];
                double tripAngle;
                TripModes tempTM;
                tempTM = RelayModeFunctions.TripModeFrom((char)bytePacket[14]);//8
                tripPacket[0] = bytePacket[14];


                //TimeDelay
                temp = bytePacket[17];//13
                temp <<= 8;
                temp += bytePacket[16];//12
                tripPacket[1] = bytePacket[16];
                tripPacket[2] = bytePacket[17];

                //ExtendedDelay
                temp = bytePacket[19];

                //SensitiveTripDelay
                temp = bytePacket[18];

                tripPacket[3] = bytePacket[18];
                tripPacket[4] = bytePacket[19];

                //Sensitive Trip Setting
                temp = bytePacket[31];
                temp <<= 8;
                temp += bytePacket[30];
                temp <<= 8;
                temp += bytePacket[23];
                temp <<= 8;
                temp += bytePacket[22];
                tripPacket[5] = bytePacket[22];
                tripPacket[6] = bytePacket[23];
                tripPacket[11] = bytePacket[30];
                tripPacket[12] = bytePacket[31];

                tempD = (double)((decimal)temp * Constants.SixteenFracBits);
                tempD *= -1000d;

                //Sensitive Angle
                temp = bytePacket[25];
                temp <<= 8;
                temp += bytePacket[24];
                tripPacket[7] = bytePacket[24];
                tripPacket[8] = bytePacket[25];

                if (temp == 0)
                {
                    tempD = 90;
                }
                else
                {
                    temp <<= 16;
                    temp >>= 16;    //converts 16 bit negative number to 32 bit negative
                    tempD = (double)((decimal)temp * Constants.EightFracBits);
                    tempD = Math.Atan(tempD);
                    tempD = RelayModeFunctions.RadiansToDegrees(tempD);
                }

                if (tempD < 0)
                    tempD += 180d;
                //Wing Trip (curve 1)
                //Insensitive Trip (IT), Mag of trip curve 3

                tripAngle = tempD;
                temp = bytePacket[55];
                temp <<= 8;
                temp += bytePacket[54];

                tripPacket[17] = bytePacket[54];
                tripPacket[18] = bytePacket[55];

                tempD = (double)((decimal)temp * Constants.TenFracBits);

                //Instantenous Current (IC), Magnitude of 4 trip curve
                temp = bytePacket[67];
                temp <<= 8;
                temp += bytePacket[66];
                tripPacket[9] = bytePacket[66];
                tripPacket[10] = bytePacket[67];

                tempD = (double)((decimal)temp * Constants.TenFracBits);

                //Watt-Var enable current

                tripPacket[13] = bytePacket[78];
                tripPacket[14] = bytePacket[79];

                temp = bytePacket[79];
                temp <<= 8;
                temp += bytePacket[78];

                tempD = (double)((decimal)temp * Constants.TenFracBits);

                //Watt Var Angle

                tripPacket[15] = bytePacket[72];
                tripPacket[16] = bytePacket[73];

                temp = bytePacket[73];
                temp <<= 8;
                temp += bytePacket[72];
                temp <<= 16;
                temp >>= 16;

                if (temp == 0)
                {
                    tempD = 90;
                }
                else
                {
                    temp <<= 16;
                    temp >>= 16;    //converts 16 bit negative number to 32 bit negative
                    tempD = (double)((decimal)temp * Constants.EightFracBits);
                    tempD = Math.Atan(tempD);
                    tempD = RelayModeFunctions.RadiansToDegrees(tempD);
                }

                tempD = tripAngle - tempD;

                if (tempD > 90)
                    tempD -= 180d;

                //wing trip 
                tripPacket[19] = bytePacket[32];  //'O' or 'N'

                tripPacket[20] = bytePacket[36]; //Angle bytes
                tripPacket[21] = bytePacket[37];
                if (this.masterRevision >= 110609)
                {
                    tripPacket[22] = bytePacket[92];
                    tripPacket[23] = bytePacket[93];
                }

                this.ucTripMode2.SetAllValue(tripPacket);
            }
            catch (Exception ex)
            {
                this.badDataDetected = true;
                this.messageHandler("Error in Relay Trip Setting Data", ex);


                // Do not overwrite relay settings on read/parse failure.
                // Leave current state alone so the operator can retry.
            }
            try
            {
                // ABC/ACB or CONED-specific phasing encoding
                temp = 0x07 & bytePacket[80];
                this.conedPhasing = 0;

                if (this.Customer == Customers.CONED)
                {
                    // Preserve CONED behavior (legacy ASCII-style values may be present)
                    if (temp > 48 && temp <= 54)
                    {
                        this.conedPhasing = (uint)temp;
                        this.comboBox_Phasings.SelectedIndex = 0; // safe display default
                       
                    }
                    else if (temp == 0)
                    {
                        this.comboBox_Phasings.SelectedIndex = 0;
                    }
                    else if (temp == 1)
                    {
                        this.comboBox_Phasings.SelectedIndex = 1;
                    }
                    else
                    {
                        this.restoreDefaultsTypeAndPhasing();
                        this.SendCTRatioAndPhasing(requestAfter: false);
                    }
                }
                else
                {
                    if (temp == 1)
                    {
                        this.comboBox_Phasings.SelectedIndex = 1;
                      
                    }
                    else if (temp == 0)
                    {
                        this.comboBox_Phasings.SelectedIndex = 0;
                        
                    }
                    else if (temp > 48 && temp <= 54) // CONED-style encoding seen on non-CONED build
                    {
#if !DOMINION
                        this.comboBox_Phasings.SelectedIndex = 0;
                        this.conedPhasing = (uint)temp;
                        
#endif
                    }
                    else
                    {
                        this.restoreDefaultsTypeAndPhasing();
                       
                        this.SendCTRatioAndPhasing(requestAfter: false);   
                    }
                }
            }
            catch (Exception ex)
            {
                dataBackupNW.dataBackup_nwProtectorDefaults = true;
                this.badDataDetected = true;
                this.messageHandler("Phase Issue", ex);
                this.restoreDefaultsTypeAndPhasing();
                this.SendCTRatioAndPhasing(requestAfter: false);
            }

            try
            {
                // Select the Voltage Level
                ProtectorVoltageBits bits = (ProtectorVoltageBits)bytePacket[80];
                ProtectorVoltage voltage;

                voltage = ProtectorVoltages.GetVoltage(bits);

                comboBoxDNPVoltage.SelectedItem = voltage;
            }
            catch (Exception ex)
            {
                dataBackupNW.dataBackup_nwProtectorDefaults = true;
                this.badDataDetected = true;
                this.messageHandler("Trouble setting 277V Bit", ex);
                this.restoreDefaultsTypeAndPhasing();
                //this.buttonRelayType_Click(this, new EventArgs());
                this.SendCTRatioAndPhasing(requestAfter: false);
            }

            try
            {
                // Force on for all customers (UI hidden)
                checkBox277DNPOutputs.Checked = true;
            }
            catch (Exception ex)
            {
                this.badDataDetected = true;
                this.messageHandler("Trouble setting 277V Output Bit", ex);
                this.restoreDefaultsTypeAndPhasing();
                //this.buttonRelayType_Click(this, new EventArgs());
                this.SendCTRatioAndPhasing(requestAfter: false);
            }

            try
            {
                //Power or Sequence
                temp = bytePacket[81];  //69
                if (temp == 'S')
                {
                    this.ucTripMode2.SequenceRelay = true;
                    this.comboBox_RelayType.SelectedIndex = 1;
                    labelConEdPowerRelay.Text = "Sequence";
                }
                else if (temp == 'P')
                {
                    this.ucTripMode2.SequenceRelay = false;
                    this.comboBox_RelayType.SelectedIndex = 0;
                    labelConEdPowerRelay.Text = "Power";
                }
                else
                {
                   // this.messageHandler("Setting default values for Relay Type", "'" + Convert.ToChar(temp).ToString() + " " + "Invalid value for phasing received from relay");
                    this.restoreDefaultsTypeAndPhasing();
                    this.SendCTRatioAndPhasing(requestAfter: false);
                }
            }
            catch (Exception ex)
            {
                dataBackupNW.dataBackup_nwProtectorDefaults = true;
                this.badDataDetected = true;
                this.messageHandler("Error in Relay Type Data", ex);
                this.comboBox_RelayType.SelectedIndex = 0;
                this.restoreDefaultsTypeAndPhasing();
                this.SendCTRatioAndPhasing(requestAfter: false);
            }
            try
            {
                //Pump Mode Packet
                byte[] pumpPacket = new byte[7];
                pumpPacket[0] = bytePacket[82]; //70
                pumpPacket[1] = bytePacket[83];
                pumpPacket[2] = bytePacket[84];
                pumpPacket[3] = bytePacket[85];
                pumpPacket[4] = bytePacket[86];

                if (this.relayCodeRevisionNumber >= 20100625)
                {
                    pumpPacket[5] = bytePacket[88];
                    pumpPacket[6] = bytePacket[89];
                }
                this.ucPumpMode1.SetAllValues(pumpPacket);
            }
            catch (Exception ex)
            {
                this.badDataDetected = true;
                this.messageHandler("Error Setting Pump Data", ex);
                this.ucPumpMode1.buttonRestoreDefaults_Click(this, new EventArgs());
                this.ucPumpMode1.SendPumpMode();
            }

            //Control Parameters 
            try
            {

            }
            catch (Exception ex)
            {
                this.badDataDetected = true;
                this.messageHandler("Error Handling Control Parameters Data", ex);
            }

            if (this.ProgramState == ProgramStates.DownloadingAllParameters)
            {
                if (dataBackup_fromRelay != true)
                    this.requestTransmitterSettings();
            }
        }

        private uint conedPhasing;

#pragma warning disable CS0414
        private bool _phasingWarningShownThisApplyAll = false;
        private bool _paramsLoadedShownThisApplyAll = false;
#pragma warning restore CS0414

        public void ResetAutoloadDeclineState()
        {
            this.skipAutoloadAfterDecline = false;
        }

        /*
        private void defaultTripSettings()
        {
            try
            {
                byte[] tripPacket = new byte[19];
                tripPacket[0] = (byte)'S';
                tripPacket[1] = 150;        //Time Delay Low Byte
                tripPacket[2] = 0;          //Time Delay High Byte
                tripPacket[3] = 6;          //Sensitive Delay Low Byte
                tripPacket[4] = 0;          //Sensitive Delay High Byte
                tripPacket[5] = 20;//236;   //Sens Trip low Byte
                tripPacket[6] = 254;//1;    //Sens Trip Mid Low Byte
                tripPacket[7] = 0;          //Sens Angle low byte (90 degrees)
                tripPacket[8] = 0;          //Send Angle High Byte
                tripPacket[9] = 128;        //Magnitude Low Byte
                tripPacket[10] = 2;//2;   //Magnitude High Byte
                tripPacket[11] = 255;         //Sens Trip Mid High Byte
                tripPacket[12] = 255;         //Sens Trip High Byte
                tripPacket[13] = 0;
                tripPacket[14] = 10;
                tripPacket[15] = 108;
                tripPacket[16] = 255;
                tripPacket[17] = 0;
                tripPacket[18] = 10;

                this.ucTripMode2.SetAllValue(tripPacket);
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Default Trip Settings", ex);
            }
        }
        */

        /*
        private void resetCloseData()
        {
            try
            {
                byte[] closePacket = new byte[10];

                closePacket[0] = 0x66;         //reclose voltage
                closePacket[1] = 0x01;
                closePacket[2] = 0x92;
                closePacket[3] = 0xF4;
                closePacket[4] = 0x66;
                closePacket[5] = 0x00;
                closePacket[6] = 0xEA;
                closePacket[7] = 0xFF;
                closePacket[8] = 0x06;
                closePacket[9] = 0x00;
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Default Close Data", ex);
            }
        }
        */

        private void handleBootMessage(byte[] bytePacket)
        {
            this.receiveArray = new byte[1000];
            this.rXReadPtr = this.rXWritePtr = 0;
            var Bootstr = System.Text.Encoding.Default.GetString(bytePacket);
            Bootstr = Bootstr.Substring(0, Bootstr.Length - 1);

            setBootRevision(Bootstr);
            this.ucRelayProgramming1.BootReceived(Bootstr);
        }

        private void setBootRevision(string BootRevision)
        {
            BootRevision = BootRevision.Trim(new Char[] { ' ', 'O', 'T' });
            if (BootRevision.Contains("I") || BootRevision.Contains("D"))
            {
                this.labelBootRevision.Text = "BOOT REVISION DNP " + BootRevision;
            }
            else
            {
                this.labelBootRevision.Text = "BOOT REVISION " + BootRevision;
            }

            this.labelBootRevision.Visible = true;
        }

        //private string revision;
        private uint masterRevision;

        private string receivedMasterRevision;
        private void revisionReceived(byte[] bytePacket)
        {
            string revision;

#pragma warning disable CS0168 // variable declared but never used
            string trim_rev;
#pragma warning restore CS0168

            try
            {
                revision = "R";
                revision += ASCIIEncoding.ASCII.GetString(bytePacket);
                if (!revision.Contains("MASTER"))
                    return;

                this.masterRevision = getMasterRevisionNumber(revision);
                this.ucTransmitter1.MasterRevisionNumber = this.masterRevision;

                this.ucRelayProgramming1.MasterRevisionString = revision;
                this.ucRelayProgramming1.MasterRevisionNumber = (UInt32)this.masterRevision;

                if (this.ucRelayProgramming1.CompareMasterRevisionToGUI())
                {
                    if (skipAutoloadAfterDecline)
                    {
                        logger.Info("Skipping pending autoload after prior user decline.");
                    }
                }

                if (this.dNPDIGITALGRIDData != null)
                    this.dNPDIGITALGRIDData.RelayMasterRevision = (UInt32)masterRevision;

                receivedMasterRevision = revision;

                if (revision.Contains("HBD"))
                {
                    relayHBD.relayWithHBD = true;
                }

                if (this.Customer == Customers.None)
                {
                    this.Customer = Customers.ENMAX;
                }

                // Keep build/customer assignment stable during runtime.
                // (This prevents CONED/others from being overwritten to COMED/ENMAX paths.)
                bool dnpCommSupported = IsDnpCommSupported();
                this.DNPEnabled = dnpCommSupported;
                this.checkDNPEnabled();

#if CONED
        this.ucRelayProgramming1.setConEdFiles();
#endif

                this.handleNewMasterRevision();
                this.setLabelText(this.ucRelayProgramming1.MasterRevisionString, this.labelRevision);
                this.relayFound = true;
                ucShortRange1.relayFound_forRNCMonitoring = true;
                this.relayFound_forDNPdataMonitoring = true;
                ucRelayProgramming1.ActiveRelay = true;
                this.saveComPort();

                if (this.ProgramState == ProgramStates.CheckingForRelay && !ucRelayProgramming1.ReprogrammingInProgress)
                {
                    this.enableAll(true);
                    this.toolStripStatusLabelRelayDisconnected.Visible = false;

                    if (!tCPConnection)
                        this.toolStripStatusLabelMain.Text = "Relay Found on " + this.serialPort1.PortName;

                    this.timerCheckPortTime.Enabled = false;
                    this.requestAllData("relay-connect-serial");
                }
            }
            catch (Exception ex)
            {
                ucShortRange1.relayFound_forRNCMonitoring = false;
                this.relayFound_forDNPdataMonitoring = false;
                this.messageHandler("Error Setting Label: " + this.labelRevision, ex);
            }
        }

        private void handleNewMasterRevision()
        {
            if (this.masterRevision < 110601)
            {
                this.buttonClearEvents.Visible = false;
                clearEventsToolStripMenuItem.Visible = false;
            }
            this.ucTripMode2.VersionNumber = this.masterRevision;
        }

        private uint getMasterRevisionNumber(string revision)
        {
            uint returnInt;
            revision = revision.Remove(0, 32); 

            try
            {
                returnInt = Convert.ToUInt32(revision);
            }
            catch
            {
                return this.masterRevision;
            }
            return returnInt;
        }

        private bool fPGARevisionValid = true;

        private void setFPGARevision(byte[] bytePacket)
        {
            string fPGARevision;
            try
            {
                fPGARevision = "F";
                fPGARevision += ASCIIEncoding.ASCII.GetString(bytePacket);
                logger.Info("FPGA Revision: {0}", fPGARevision);
                if ((bytePacket[13] == 0xFF && bytePacket[14] == 0xFF && bytePacket[15] == 0xFF &&
                   bytePacket[16] == 0xFF && bytePacket[17] == 0xFF && bytePacket[18] == 0xFF) ||
                   (bytePacket[13] == 0x00 && bytePacket[14] == 0x00 && bytePacket[15] == 0x00 &&
                    bytePacket[16] == 0x00 && bytePacket[17] == 0x00 && bytePacket[18] == 0x00))
                {
                    this.labelFPGARevision.Hide();
                    // Pass zero to the reprogramming just incase the FPGA code has been corrupt.  Passing all F's is interpreted as a "high" date.

                    this.ucRelayProgramming1.FPGARevisionNumber = 0;
                    this.ucTransmitter1.FPGARevisionValid = false;
                    this.fPGARevisionValid = false;
                }
                else
                {
                    this.ucTransmitter1.FPGARevisionValid = true;
                    this.fPGARevisionValid = true;
                    this.labelFPGARevision.Show();

                    try
                    {
                        UInt32 tempU = Convert.ToUInt32(fPGARevision.Substring(13));
                        this.ucRelayProgramming1.FPGARevisionNumber = tempU;
                    }
                    catch
                    {
                        // This means the revision number is messed up so we should program it
                        this.ucRelayProgramming1.FPGARevisionNumber = 0;
                    }

                }
                this.setLabelText(fPGARevision, this.labelFPGARevision);

                if (this.ProgramState == ProgramStates.DownloadingAllParameters)
                {
                    if (!ucRelayProgramming1.ReprogrammingInProgress)
                        this.requestRelayParameters();
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Label: " + this.labelFPGARevision, ex);
            }
        }

        private void trippedOrClosed(byte[] bytePacket)
        {
            try
            {
                char type;

                type = (char)bytePacket[0];
                switch (type)
                {
                    case 'T':
                        setLabelText("Open", this.labelRelayTrippedOrClose);
                        setBackgroundColor(Color.Green, this.labelRelayTrippedOrClose);
                        //   setLabelText("Open", this.labelRelayStateControlPage);
                        //   setBackgroundColor(Color.Green, this.labelRelayStateControlPage);
                        break;
                    case 'C':
                        setLabelText("Close", this.labelRelayTrippedOrClose);
                        setBackgroundColor(Color.Red, this.labelRelayTrippedOrClose);
                        //   setLabelText("Close", this.labelRelayStateControlPage);
                        //   setBackgroundColor(Color.Red, this.labelRelayStateControlPage);
                        break;
                    case 'F':
                        setLabelText("Float", this.labelRelayTrippedOrClose);
                        setBackgroundColor(Color.Yellow, this.labelRelayTrippedOrClose);
                        //  setLabelText("Float", this.labelRelayStateControlPage);
                        //  setBackgroundColor(Color.Yellow, this.labelRelayStateControlPage);
                        break;
                    default:
                        setLabelText("Error", this.labelRelayTrippedOrClose);
                        setBackgroundColor(Color.SaddleBrown, this.labelRelayTrippedOrClose);
                        //  setLabelText("Error", this.labelRelayStateControlPage);
                        //  setBackgroundColor(Color.SaddleBrown, this.labelRelayStateControlPage);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error in Trip/Close Event", ex);
            }
        }

        private decimal ClampToNumericRange(decimal value, NumericUpDown control)
        {
            if (value < control.Minimum) return control.Minimum;
            if (value > control.Maximum) return control.Maximum;
            return value;
        }

        private void setPermissiveCloseData(byte[] bytePacket)
        {
            if (bytePacket == null || bytePacket.Length < 8) return;
#if CONED
            try
            {
                if ((bytePacket[1] & 0x01) == 1)
                {
                    this.lbl_PermissiveClose_Status.BackColor = Color.Yellow;
                    this.lbl_PermissiveClose_Status.Text = "ENABLED";
                }
                else
                {
                    this.lbl_PermissiveClose_Status.BackColor = Color.White;
                    this.lbl_PermissiveClose_Status.Text = "Disabled";
                }

                decimal floatTime = bytePacket[3];
                decimal activeTime = bytePacket[5];

                UInt16 raw = (UInt16)((bytePacket[6] << 8) | bytePacket[7]); // swapped
                decimal voltage = raw * Constants.TwelveFracBits;
              

                this.numericUpDown_PC_floatTime.Value = ClampToNumericRange(floatTime, this.numericUpDown_PC_floatTime);
                this.numericUpDown_PC_activeTime.Value = ClampToNumericRange(activeTime, this.numericUpDown_PC_activeTime);
                this.numericUpDown_PC_voltage.Value = ClampToNumericRange(voltage, this.numericUpDown_PC_voltage);

                if (_pcApplyPendingConfirmation)
                {
                    _pcApplyPendingConfirmation = false;

                    if (!_paramsLoadedShownThisApplyAll)
                    {
                        _paramsLoadedShownThisApplyAll = true;
                        MessageBox.Show(
                            "Parameters Loaded Successfully",
                            "Parameters Loaded",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                _pcApplyPendingConfirmation = false;
                this.messageHandler("Error in Permissive Close data received from the relay", ex);
            }
#endif
        }

        private delegate void setTextBoxCallBack(string s, TextBox tB);

        private void setShortRangeParameters(byte[] bytePacket)
        {
            this.ucShortRange1.SetAll(bytePacket);
            if (dataBackup_fromRelay == true)
            {
                byte[] snapshot = new byte[Math.Min(bytePacket.Length, 1)];
                if (bytePacket.Length > 30)
                    snapshot[0] = bytePacket[30];

                CaptureBackupSection("XS", snapshot);
            }

        }
        private void StartStartupBackupOnce()
        {
            if (backupInProgress)
            {
                logger.Info("Startup backup already in progress; skipping duplicate startup backup.");
                return;
            }

            if (this.ucRelayProgramming1 != null)
            {
                // Defer to ucRelayProgramming lifecycle.
                // MainControl should not decide whether the programming flow should continue.
                logger.Info("MainControl: deferring relay lifecycle continuation to ucRelayProgramming.");
            }

            logger.Info("Starting one-time startup backup before autoload logic.");
            BackUpRelayDatatoFile();   // do not set pendingAutoloadAfterBackup here
        }

        private void setTextBox(string s, TextBox tB)
        {
            try
            {
                if (tB.InvokeRequired)
                {
                    setTextBoxCallBack sTB = new setTextBoxCallBack(setTextBox);
                    this.Invoke(sTB, new object[] { s, tB });
                }
                else
                {
                    tB.Text = s;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Invoking TextBox: " + tB.Name, ex);
            }
        }

        private delegate void setCheckedValueCallBack(bool b, CheckBox cB);

        private void setCheckedValue(bool b, CheckBox cB)
        {
            try
            {
                if (cB.InvokeRequired)
                {
                    setCheckedValueCallBack updateCB = new setCheckedValueCallBack(setCheckedValue);
                    this.Invoke(updateCB, new object[] { b, cB });
                }
                else
                {
                    cB.Checked = b;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Invoking CheckBox: " + cB.Name, ex);
            }
        }

        public delegate void setLabelColorCallBack(Color c, Label l);

        private void setBackgroundColor(Color c, Label l)
        {
            try
            {
                if (l.InvokeRequired)
                {
                    setLabelColorCallBack b = new setLabelColorCallBack(setBackgroundColor);
                    this.Invoke(b, new object[] { c, l });
                }
                else
                {
                    l.BackColor = c;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Invoking Color For Trip/Close", ex);
            }
        }

        private int missedMonitoringCount = 0;
        private bool phasorReceived = false;

        private bool phasorSettingErrorShown;

        private void setPhasorValue(byte[] packet)
        {
            if (packet == null || packet.Length < 14)
            {
                logger.Warn("setPhasorValue: invalid packet length={0}", packet == null ? 0 : packet.Length);
                return;
            }

            long realValue;
            long imaginaryValue;
            long rMS;
            PhasorTypes phasorType = PhasorTypes.None;

            this.phasorReceived = true;
            this.missedMonitoringCount = 0;

            logger.Info("Incoming Phasor: " + (char)packet[0] + ", " + (char)packet[1]);
            phasorType = RelayModeFunctions.PhasorTypeFrom((char)packet[0], (char)packet[1]);

            try
            {
                if (phasorType != PhasorTypes.None)
                {
                    realValue = ((Int32)packet[2]) << 24;
                    realValue += ((Int32)packet[3]) << 16;
                    realValue += ((Int32)packet[4]) << 8;
                    realValue += ((int)packet[5]);

                    imaginaryValue = (int)packet[6] << 24;
                    imaginaryValue += (int)packet[7] << 16;
                    imaginaryValue += (int)packet[8] << 8;
                    imaginaryValue += (int)packet[9];

                    rMS = (int)packet[10] << 24;
                    rMS += (int)packet[11] << 16;
                    rMS += (int)packet[12] << 8;
                    rMS += (int)packet[13];

                    if (this.pQMonitoringEnabled)
                        this.ucPhasorGraph1.ValuesForUpdate(phasorType, realValue, imaginaryValue, this.CTRatio, rMS);

                    if (this.transmitterMonitoring)
                        this.setTransmitterPhasorValues(phasorType, realValue, imaginaryValue, this.CTRatio, rMS);

                    // only send it to this if monitoring is not going on, so that it doesn't get every
                    // phasor that comes in during monitoring.
                    if (!this.pQMonitoringEnabled)
                        ucPhasorRequest1.SetPhasorValues(packet[0], packet[1], realValue, imaginaryValue, rMS);

                    // Clear the error flag once we successfully processed a valid phasor packet.
                    this.phasorSettingErrorShown = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error Setting Phasor Value. phasorType={0}, packet={1}",
                    phasorType,
                    BitConverter.ToString(packet));

                if (!this.phasorSettingErrorShown)
                {
                    this.phasorSettingErrorShown = true;
                    this.messageHandler("Error Setting Phasor Value", ex);
                }
            }
        }

        private void setTransmitterPhasorValues(PhasorTypes phasorType, long realValue, long imaginaryValue, int p, long rMS)
        {
            this.ucTransmitterMonitoring1.SetTransmitterPhasorValues(phasorType, realValue, imaginaryValue, p, rMS);
        }

        private void cOMPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tSMI = (ToolStripMenuItem)sender;

            if (this.serialPort1.IsOpen)
            {
                this.clearSerialPortBuffers(this.serialPort1);
                this.serialPort1.Close();
            }

            try
            {
                this.serialPort1.BaudRate = 19200;
                this.serialPort1.PortName = tSMI.Text;
                this.serialPort1.Open();
                //this.clearSerialPortBuffers(this.serialPort1);
#if DEBUG
                return;
#endif
            }
            catch (Exception ex)
            {
                this.messageHandler("Something wrong with serial Port: " + this.serialPort1.PortName + ", Port not Open", ex);

                this.toolStripStatusLabelMain.Text = "Port Error";
                return;
            }
        }

        private int CTRatio = 320;

        private void requestRelayRevision()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'Q';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);
        }

        private void requestFPGARevision()
        {
            logger.Trace("requestFPGARevision()");
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'n';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);
        }

        private void buttonUpdateDisplay_Click(object sender, EventArgs e)
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'u';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);
        }

        private void buttonToggleMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                this.ucPhasorGraph1.CTRatio = this.CTRatio;     //put this in because you can now change the CT ratio if you are viewing event data on the PQ monitoring page

                this.missedMonitoringCount = 0;
                if (this.pQMonitoringEnabled)
                {
                    this.disableAllMonitoring();
                }
                else
                {
                    this.requestPhasorData();
                    this.everyOtherMonitor = false;
                    this.pQMonitoringEnabled = true;
                    this.buttonToggleMonitor.Text = "Stop Monitoring";
                    this.ucPhasorGraph1.RealTimeMonitoring = true;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error In Toggling Monitor Mode", ex);
            }
        }

        private bool pQMonitoringEnabled = false;
        private void disableAllMonitoring()
        {
            this.ucShortRange1.StopMonitoring();
            this.pQMonitoringEnabled = false;
            this.buttonToggleMonitor.Text = "Start Monitoring";
            this.ucPhasorGraph1.RealTimeMonitoring = false;
            this.requestingDNPData = false;
        }

        private void enableAllMonitoring()
        {
            this.pQMonitoringEnabled = true;
            this.buttonToggleMonitor.Text = "Stop Monitoring";
            this.ucPhasorGraph1.RealTimeMonitoring = true;
        }

        private void monitoring(bool b)
        {
            try
            {
                this.disableAllMonitoring();

                if (!b)
                {
                    this.setButtonText("Start Monitoring", this.buttonToggleMonitor);
                }
                else
                {
                    this.pQMonitoringEnabled = true;
                    this.setButtonText("Stop Monitoring", this.buttonToggleMonitor);
                }
            }
            catch (Exception ex)
            {
                //this.timerMonitorRate.Enabled = false;
                this.disableAllMonitoring();
                this.setButtonText("Start Monitoring", this.buttonToggleMonitor);
                this.messageHandler("Error In Actual Monitor Toggle", ex);
            }
        }

        private void requestPhasorData()
        {
            this.phasorReceived = false;
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'p';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);
        }

        private delegate void setLabelTextCallBack(string s, Label l);

        private void setLabelText(string s, Label l)
        {
            try
            {
                if (l.InvokeRequired)
                {
                    setLabelTextCallBack b = new setLabelTextCallBack(setLabelText);
                    this.Invoke(b, new object[] { s, l });
                }
                else
                {
                    l.Text = s;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Invoking Label: " + l.Name, ex);
            }
        }

        private delegate void setButtonTextCallBack(string s, Button b);

        private void setButtonText(string s, Button b)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    setButtonTextCallBack sTCB = new setButtonTextCallBack(setButtonText);
                    this.Invoke(sTCB, new object[] { s, b });
                }
                else
                {
                    b.Text = s;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Invoking Button: " + b.Name, ex);
            }
        }

        uint dataRetryCount = 0;

        private void timerFindRelayTimeout_Tick(object sender, EventArgs e)
        {
            try
            {
                timerFindRelayTimeout.Enabled = false;

                if (this.expectingRelayRevision)
                {
                    if (this.dataRetryCount == 3)
                    {
                        this.noResponseError("Unable to get Relay Revision");
                    }
                    else
                    {
                        this.dataRetryCount++;
                        this.requestRelayRevision();
                    }
                }
                else if (this.expectingFPGARevision)
                {
                    if (this.dataRetryCount == 3)
                    {
                        this.noResponseError("Unable to get FPGA Revision");
                    }
                    else
                    {
                        this.dataRetryCount++;
                        this.requestFPGARevision();
                    }
                }
                else if (this.expectingRelayParameters)
                {
                    if (this.dataRetryCount == 3)
                    {
                        this.noResponseError("Unable to get Relay Parameters");
                    }
                    else
                    {
                        this.dataRetryCount++;
                        this.requestAllData("timerFindRelayTimeout_Tick");
                    }
                }
                else if (this.expectingRelayRegisters)
                {
                    if (this.dataRetryCount == 3)
                    {
                        this.noResponseError("Unable to get Relay Registers");
                    }
                    else
                    {
                        this.dataRetryCount++;
                        this.requestRelayRegisters();
                    }
                }
                else if (this.expectingTransmitterSettings)
                {
                    if (this.dataRetryCount == 3)
                    {
                        this.noResponseError("Unable to get Transmitter Settings");
                    }
                    else
                    {
                        this.dataRetryCount++;
                        this.requestTransmitterSettings();
                    }
                }
                else
                {
                    this.noResponseError(this.AcknowledgeCaller);
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error In Time Out", ex.Message);
            }
        }

        private void timerSCITimeOut_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timerSCITimeOut.Enabled = false;

                bool hadPendingAck;
                string timedOutCaller;

                lock (_ackFlowLock)
                {
                    hadPendingAck = this.expectingAck;
                    timedOutCaller = this.AcknowledgeCaller;

                    if (hadPendingAck)
                    {
                        this.expectingAck = false;
                        this.AcknowledgeCaller = string.Empty;
                    }
                }

                if (hadPendingAck)
                {
                    logger.Warn("SCI timeout: clearing pending ACK state. caller={0}", timedOutCaller);
                    this.TryDrainPendingRelayTypePhasingSend("SCI-TIMEOUT");
                }
                else
                {
                    logger.Info("SCI timeout tick with no pending ACK state.");
                }

                if (this.expectingRelayRevision)
                {
                    if (this.dataRetryCount == 3)
                        this.noResponseError("Unable to get Relay Revision");
                    else
                    {
                        this.dataRetryCount++;
                        this.requestRelayRevision();
                    }
                }
                else if (this.expectingFPGARevision)
                {
                    if (this.dataRetryCount == 3)
                        this.noResponseError("Unable to get FPGA Revision");
                    else
                    {
                        this.dataRetryCount++;
                        this.requestFPGARevision();
                    }
                }
                else if (this.expectingRelayParameters)
                {
                    if (this.dataRetryCount == 3)
                        this.noResponseError("Unable to get Relay Parameters");
                    else
                    {
                        this.dataRetryCount++;
                        this.requestAllData("timerSCITimeOut_Tick");
                    }
                }
                else if (this.expectingRelayRegisters)
                {
                    if (this.dataRetryCount == 3)
                        this.noResponseError("Unable to get Relay Registers");
                    else
                    {
                        this.dataRetryCount++;
                        this.requestRelayRegisters();
                    }
                }
                else if (this.expectingTransmitterSettings)
                {
                    if (this.dataRetryCount == 3)
                        this.noResponseError("Unable to get Transmitter Settings");
                    else
                    {
                        this.dataRetryCount++;
                        this.requestTransmitterSettings();
                    }
                }
                else
                {
                    this.ucTripMode2.SendTimedOut = true;
                    this.ucCloseMode1.SendTimedOut = true;
                    this.SCITimedOut = true;
                    this.noResponseError("Last operation did not complete properly");
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error In Time Out", ex.Message);
            }
        }

        private void noResponseError(string p)
        {
            if (this.loadingNewCode)
                return;
            this.monitoring(false);
            this.RegisterPolling(false);
            this.timerFindRelayTimeout.Enabled = false;
            this.setButtonText("Start Monitoring", this.buttonToggleMonitor);
            this.messageHandler(p, new Exception("SCI Timeout"));
        }

        private bool requestedAllParameters = false;
        private void buttonRequestRelayParamaters_Click(object sender, EventArgs e)
        {
            this.enableAll(false);
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            this.requestedAllParameters = true;
            this.ProgramState = ProgramStates.DownloadingAllParameters;
            screenD.screenDisable = true;

            try
            {
                this.BeginFullParameterDownload("buttonRequestRelayParamaters_Click");
                this.requestAllData("buttonRequestRelayParamaters_Click");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Request All failed");
                this.messageHandler("Request All Failed", ex);

                // reset only on synchronous failure
                ResetFullParameterDownloadState("buttonRequestRelayParamaters_Click:catch");
            }
            // no finally reset here
        }


        private void requestRelayParameters()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'S';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);

            // request low-voltage threshold explicitly; avoid UI click side effects
            this.requestLowVoltageThreshold();
        }

        /*
        private void requestRelayParameters()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'S';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);

            buttonRequestLowVotlageThres_Click(null, null);
        }*/

        private void requestDNPSettings()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'U';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);

            UpdateDnpCommStatusFromRelayState(this.DNPEnabled);

        }

        private void requestTransmitterSettings()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'X';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);

            UpdateDnpCommStatusFromRelayState(this.DNPEnabled);

        }

        private void buttonRequestRelayRegisters_Click(object sender, EventArgs e)
        {
            this.requestRelayRegisters();
        }

        private void requestRelayRegisters()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'r';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            if (ucRelayProgramming1.ReprogrammingInProgress)
            {
                this.timerRegisterPolling.Enabled = false;
                return;
            }

            this.expectingRelayRegisters = true;

            if (this.ProgramState != ProgramStates.DownloadingAllParameters)     //checked so it does not start the timer during initial download, but starts it everytime the program is running
                this.timerRegisterPolling.Enabled = true;

            this.sendPacket(sendArray);
        }

        private void requestSafeServiceSettings()
        {
            byte[] sendArray = new byte[2];

            sendArray[0] = 0x0E;
            sendArray[1] = 0x0D;

            this.sendPacket(sendArray);
        }


        private void requestTemperature()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'t';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);
        }
        private void buttonTypePhasingRestoreDefaults_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            screenD.screenDisable = true;

            try
            {
                this.SendRelayPhasingAndType();
            }
            finally
            {
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                screenD.screenDisable = false;
            }
        }

        private void SendRelayPhasingAndType()
        {
            logger.Info(
                "SendRelayPhasingAndType called. sendAllFlag={0}, sendAll={1}, customer={2}, relayTypeSelected={3}, phasingSelected={4}",
                sendAllF.SendAllFlag,
                this.sendAll,
                this.Customer,
                this.comboBox_RelayType?.SelectedItem?.ToString() ?? "<null>",
                this.comboBox_Phasings?.SelectedItem?.ToString() ?? "<null>"
            );

            try
            {
                byte[] packet = new byte[4];
                packet[0] = (byte)'s';

                var relayType = this.comboBox_RelayType?.SelectedItem?.ToString();
                if (relayType == "Sequence")
                    packet[1] = (byte)'S';
                else if (relayType == "Power")
                    packet[1] = (byte)'P';
                else
                    packet[1] = (byte)'P';

                var phasing = this.comboBox_Phasings?.SelectedItem?.ToString();
                if (this.Customer != Customers.CONED)
                {
                    if (phasing == "ABC : CAB : BCA")
                        packet[2] = 0x00;
                    else if (phasing == "CBA : BAC : ACB")
                        packet[2] = 0x01;
                    else
                        packet[2] = 0x00;
                }
                else
                {
                    packet[2] = (byte)this.conedPhasing;
                }

                try
                {
                    packet[2] |= (byte)protectorVoltage.SetBit;
                }
                catch (Exception ex)
                {
                    messageHandler("Problem Setting Protector Voltage bits", ex);
                }

                try
                {
                    packet[2] |= 0x10;
                }
                catch (Exception ex)
                {
                    messageHandler("Problem setting 277 V Outputs bit", ex);
                }

                packet[3] = 0x0D;

                logger.Info("Relay Type/Phasing packet prepared. packet=[{0}]", BitConverter.ToString(packet));

                // IMPORTANT: do not call sendPacketAck directly here.
                // Use the queue/ACK guard path.
                this.QueueRelayTypePhasingSendIfAllowed(packet, "Relay Type Send");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error Setting Relay Type");
                this.messageHandler("Error Setting Relay Type", ex);
            }
        }

        //private void buttonRelayType_Click(object sender, EventArgs e)
        public void SendCTRatioAndPhasing(bool requestAfter)
        {
            try
            {
                lock (_ackFlowLock)
                {
                    // This must be the CT-ratio ACK owner.
                    this.sendCTRatio(requestAfter);

                    // Do NOT force a relay/phasing send here unless it is actually queued.
                    // The real drain is triggered when the CT ACK is received and processAckReceived()
                    // decides it is safe to send the queued relay type/phasing packet.
                    //this.pendingRelayTypePhasingPacket = null;
                    //this.pendingRelayTypePhasingCaller = null;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error setting CT ratio / relay type", ex);
            }
        }

        private void UcRelayProgramming1_BackupBeforeProgrammingRequested(object sender, EventArgs e)
        {
            // MainControl should not own the backup lifecycle decision.
            logger.Info("MainControl: ucRelayProgramming requested backup before programming; MainControl is orchestration-only.");
            // If backup orchestration still exists, keep it strictly UI/command-level only.
            // Do not additionally set autoload bookkeeping in MainControl.
        }

        private void restoreDefaultsTypeAndPhasing()
        {
            
            comboBoxDNPVoltage.SelectedItem = ProtectorVoltages.GetVoltage();
            checkBox277DNPOutputs.Checked = true;

            // Phasings - 0 - ABC, 1 - ACB

            // RelayType - 1 = Sequence, 0 = Power

            /*  CTRatio
             * 0    3750
             * 1    3500
             * 2    3000
             * 3    2500
             * 4    2000
             * 5    1600
             * 6    1200
             * 7    800
             * 8   "Special"
             */

#if BGE
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 1;
            this.comboBox_CTRatio.SelectedIndex = 5;
#elif COMED
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 1;
            this.comboBox_CTRatio.SelectedIndex = 2;
#elif CONED
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 0;
            this.comboBox_CTRatio.SelectedIndex = 5;
#elif DOMINION
            this.comboBox_RelayType.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 1;
            this.comboBox_CTRatio.SelectedIndex = 2;
            //not sure about next line.
            labelConEdPowerRelay.Visible = false;
#elif ENMAX
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 0;
            this.comboBox_CTRatio.SelectedIndex = 2;
            // not sure about next line?
            labelConEdPowerRelay.Visible = false;
#elif EVERSOURCE
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 1;
            this.comboBox_CTRatio.SelectedIndex = 5;
#elif LONDON_HYDRO
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 1;
            this.comboBox_CTRatio.SelectedIndex = 5;
#elif ONCOR
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 1;
            this.comboBox_CTRatio.SelectedIndex = 5;
#elif PSEG
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 1;
            this.comboBox_CTRatio.SelectedIndex = 5;
#elif SCE
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 1;
            this.comboBox_CTRatio.SelectedIndex = 5;
#elif SCL
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 1;
            this.comboBox_CTRatio.SelectedIndex = 2;
#elif TAUNTON
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 0;
            this.comboBox_CTRatio.SelectedIndex = 5;
#elif TORONTO_HYDRO
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 1;
            this.comboBox_CTRatio.SelectedIndex = 5;
#else
            this.comboBox_Phasings.SelectedIndex = 0;
            this.comboBox_RelayType.SelectedIndex = 1;
            this.comboBox_CTRatio.SelectedIndex = 5;
#endif
        }

        private bool pauseMonitoring = false;
        private void buttonForceI_Click(object sender, EventArgs e)
        {
            byte[] packet = new byte[3];

            packet[0] = (byte)'z';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacket(packet);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] packet = new byte[4];

            packet[0] = (byte)'Z';
            packet[1] = (byte)this.CTRatio;
            packet[2] = (byte)(this.CTRatio >> 8);
            packet[3] = 0x0D;

            this.sendPacket(packet);
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setComPortMenu(this.getPortNames());
        }

        private List<string> setComPortMenu(List<string> portNames)
        {
            try
            {
                int i = 0;
                this.cOMPortToolStripMenuItem.DropDownItems.Clear();
                foreach (string s in portNames)
                {
                    this.cOMPortToolStripMenuItem.DropDownItems.Add(s);
                    this.cOMPortToolStripMenuItem.DropDownItems[i].Click += new System.EventHandler(this.cOMPortToolStripMenuItem_Click);
                    ++i;
                }
                return portNames;
            }
            catch (Exception ex)
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                throw new Exception("Error Getting ComPort Values", ex);
            }
        }

        #region Find Relay

        private delegate string[] stringArrayCallBack();
        private string savedComPort;

        private Thread threadFindRelay;
        private bool relayFound = false;
        private ProgramStates ProgramState = ProgramStates.CheckingForRelay;
        private List<string> portNames;


        private void findRelay(object pN)
        {
            try
            {
                this.ProgramState = ProgramStates.CheckingForRelay;
                this.portNames = (List<string>)pN;

                if (this.portNames.Contains(this.savedComPort))         //make sure saved port exists
                {
                    this.portNames.Remove(this.savedComPort);
                    this.portNames.Insert(0, this.savedComPort);        //move it to the top so it is the first checked
                }
                this.checkPortsForRelay();
            }
            catch (Exception ex)
            {
                this.messageHandler("Error In findRelay()", ex);

            }

            return;
        }

        private void checkPortsForRelay()
        {
            string currentPort;
            string errorMessage;

            ucRelayProgramming1.NotPollingPort = true;

            if (this.ProgramState == ProgramStates.Running) //if it is running don't check for the ports
                return;

            if (this.portNames.Count == 0)           //no relay found
            {

                this.ProgramState = ProgramStates.FailedToFindRelay;

                errorMessage = "Unable To Locate Relay.";
                this.relayNotFound();
                this.RegisterPolling(false);

                ucRelayProgramming1.NotPollingPort = false;

                this.messageHandler("No Relay Found", new Exception(errorMessage));
                return;
            }

            currentPort = this.portNames[0];


            this.toolStripStatusLabelMain.Text = "Checking " + currentPort + " for Relay";

            if (this.checkPortAvailability(currentPort))
            {
                this.clearRemoteBuffer();
                this.requestMasterRevisionNumber();
                this.timerCheckPortTime.Dispose();
                this.timerCheckPortTime = new System.Windows.Forms.Timer();
                this.timerCheckPortTime.Tick += new EventHandler(timerCheckPortTime_Tick);
                this.timerCheckPortTime.Interval = 500;
                this.timerCheckPortTime.Start();
            }
            else
            {
                this.portNames.RemoveAt(0);                     //if the port is unavailable, remove and call this function again
                this.checkPortsForRelay();
            }
        }

        private void RegisterPolling(bool p)
        {
            logger.Trace("Register Polling: {0}", p);
            this.pauseMonitoring = !p;
            this.ucTimeControl1.EnablePolling(p);
        }

        private bool checkPortAvailability(string s)
        {
            try
            {
                this.clearSerialPortBuffers(this.serialPort1);
                this.serialPort1.Close();
                this.serialPort1.PortName = s;
                this.serialPort1.Open();
                this.clearSerialPortBuffers(this.serialPort1);
            }
            catch
            {
                return false;
            }

            return this.serialPort1.IsOpen;
        }

        private void checkPortForRelay()
        {

            this.toolStripStatusLabelMain.Text = "Checking " + this.serialPort1.PortName + " for Relay";

            try
            {
                this.serialPort1.Open();
                this.clearSerialPortBuffers(this.serialPort1);
                if (!this.serialPort1.IsOpen)
                {
                    return;
                }

                this.clearRemoteBuffer();
                Thread.Sleep(1000);
                this.requestMasterRevisionNumber();
                this.timerCheckPortTime.Dispose();
                this.timerCheckPortTime = new System.Windows.Forms.Timer();
                this.timerCheckPortTime.Tick += new EventHandler(timerCheckPortTime_Tick);
                this.timerCheckPortTime.Interval = 500;
                this.timerCheckPortTime.Start();


            }
            catch (Exception ex)
            {
                this.messageHandler("Error Checking Port For Relay", ex);
            }
        }

        private void clearRemoteBuffer()
        {
            byte[] packet = new byte[3];

            packet[0] = 0x0D;

            packet[1] = 0x0D;
            packet[2] = 0x0D;

            this.sendPacket(packet);
        }

        private void requestMasterRevisionNumber()
        {

            byte[] packet = new byte[3];

            packet[0] = (byte)'R';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacket(packet);
        }

        private bool firstPortCheckAttempt = true;

        private void timerCheckPortTime_Tick(object sender, EventArgs e)
        {

            this.timerCheckPortTime.Enabled = false;
            if (this.ProgramState == ProgramStates.Running)  //if program is in the standard running mode, this does not apply
                return;

            if (this.firstPortCheckAttempt)              //if it is the first attempt, simply try again
            {
                try
                {
                    this.firstPortCheckAttempt = false;
                    this.checkPortForRelay();
                }
                catch (Exception ex)
                {
                    this.messageHandler("Error On First Port Check", ex);
                }
            }
            else
            {
                try
                {
                    this.firstPortCheckAttempt = true;      //if it is the second attempt, remove the current port from the list and try next port.
                    if (this.portNames.Count > 0)
                    {
                        this.portNames.RemoveAt(0);
                        this.serialPort1.Close();
                        this.serialPort1.BaudRate = 19200;
                        this.serialPort1.Open();
                        this.checkPortsForRelay();
                    }
                }
                catch (Exception ex)
                {
                    this.messageHandler("Error On Second Port Check", ex);
                }
            }

            //this.relayCheckTimeOut = true;
        }

        private bool expectingRelayRevision = false;
        private bool expectingRelayParameters = false;
        private bool expectingTransmitterSettings = false;
        private bool expectingRelayRegisters = false;
        private bool expectingFPGARevision = false;

        delegate void requestAllCallBack();
        private void MarkSectionReceived(SectionBits section)
        {
            this.receivedSections |= section;

            logger.Info(
                "Section received: {0}, receivedMask={1}, requiredMask={2}, syncPhase={3}",
                section,
                this.receivedSections,
                this.requiredSections,
                this.syncPhase);

            if (this.syncPhase != SyncPhase.FullParameterDownload)
                return;

            // Already completed this cycle: ignore repeated SafeService / late packets
            if (this.parametersLoaded)
            {
                logger.Info(
                    "Full parameter completion already processed; ignoring additional section. section={0}, received={1}, required={2}",
                    section,
                    this.receivedSections,
                    this.requiredSections);
                return;
            }

            if ((this.receivedSections & this.requiredSections) == this.requiredSections)
            {
                this.parametersLoaded = true;

                logger.Info(
                    "Full parameter set complete: firing parametersFinishedLoading(). received={0}, required={1}",
                    this.receivedSections,
                    this.requiredSections);

                this.parametersFinishedLoading();
            }
        }

        private void BeginFullParameterDownload(string caller)
        {
            this.requestedAllParameters = true;
            this.ProgramState = ProgramStates.DownloadingAllParameters;

            // new phase-based tracker
            this.syncPhase = SyncPhase.FullParameterDownload;
            this.requiredSections = SectionBits.RelayParams
                                  | SectionBits.TxSettings;

            this.receivedSections = SectionBits.None;

            // reset the existing completion latch for this cycle
            this.parametersLoaded = false;

            logger.Info(
                "BeginFullParameterDownload caller={0}, phase={1}, required={2}, received={3}",
                caller,
                this.syncPhase,
                this.requiredSections,
                this.receivedSections);
        }

        private bool ShouldRestoreFromBackup(string sectionName)
        {
            if (string.Equals(sectionName, "CalibrationConstants", StringComparison.OrdinalIgnoreCase))
            {
                logger.Info("Backup restore: skipping CalibrationConstants (live apply only; not replayed from backup).");
                return false;
            }

            return true;
        }
        private void requestAllData(string caller = "unknown")
        {
            if (this.sendAll || sendAllF.SendAllFlag)
            {
                logger.Info("requestAllData SUPPRESSED: sendAll active. caller={0}", caller);
                return;
            }

            logger.Info("requestAllData caller={0} ...", caller);
            logger.Info(
                "requestAllData ENTRY: requestedAllParameters={0}, ProgramState={1}, sendAll={2}, loadingNewCode={3}, backupInProgress={4}, pendingAutoloadAfterBackup={5}, pendingRestoreAfterProgramming={6}",
                requestedAllParameters,
                ProgramState,
                this.sendAll,
                loadingNewCode,
                backupInProgress,
                pendingAutoloadAfterBackup,
                pendingRestoreAfterProgramming
            );

            this.requestedAllParameters = true;
            this.requestMasterRevisionNumber();
            this.requestAllDataNoMasterRev();
        }

        private void requestAllDataNoMasterRev()
        {
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            logger.Info(
                "requestAllDataNoMasterRev ENTER: requestedAllParameters={0}, ProgramState={1}, pendingRestoreAfterProgramming={2}, pendingAutoloadAfterBackup={3}, backupInProgress={4}, loadingNewCode={6}",
                requestedAllParameters,
                ProgramState,
                pendingRestoreAfterProgramming,
                pendingAutoloadAfterBackup,
                backupInProgress,
                loadingNewCode
            );

            if (this.InvokeRequired)
            {
                // Use BeginInvoke so we do not block the calling thread.
                this.BeginInvoke(new Action(requestAllDataNoMasterRev));
                return;
            }

            this.requestedAllParameters = true;
            this.ProgramState = ProgramStates.DownloadingAllParameters;

            if (!ucRelayProgramming1.ReprogrammingInProgress)
            {
                logger.Debug("Requesting Relay Revision");
                clearRemoteBuffer();
                this.requestRelayRevision();
                logger.Trace("Setting timerResponseTimeOut from requestAllDataNoMasterRev");
                this.timerResponseTimeOut.Enabled = true;
            }
        }

        private void relayNotFound()
        {
            this.monitoring(false);
            this.RegisterPolling(false);
            this.toolStripStatusLabelMain.Text = "No Relay Found";
            this.serialPort1.Close();
        }

        #endregion

        private void findRelayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string errorMessage = "Initial";

            try
            {
                errorMessage = "Error Disabling All";
                this.enableAll(false);
                errorMessage = "Error Setting Variables";
                this.relayFound = false;

                errorMessage = "Error Checking For Relay";
                this.checkSavedLocationAndFindRelay();
            }
            catch (Exception ex)
            {
                this.messageHandler(errorMessage, ex);
            }
        }

        private void buttonRQRelayProcVersion_Click(object sender, EventArgs e)
        {
            this.requestRelayRevision();
        }

        #region FileIO
        private MyFile savedFile;
        private const string _savedFilePath = @"C:\DGI Systems\Relay\Saved.txt";

        private string getSavedComPort(MyFile mF)
        {
            try
            {
                string comPort;

                comPort = mF.ReadWholeFile();

                if (comPort == "")
                {
                    comPort = "";
                }
                else
                {
                    while (comPort.Contains("\r") || comPort.Contains("\n"))
                        comPort = comPort.Remove(comPort.Length - 1, 1);
                }
                return comPort;
            }
            catch (Exception ex)
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                throw new Exception("Error Getting Port From Saved File", ex);
            }
        }

        private void saveComPort()
        {
            this.savedFile.WriteWholeFile(this.serialPort1.PortName);
        }
        #endregion

        private string AcknowledgeCaller = "";
        private void sendPacketAck(byte[] bytePacket, string caller)
        {
            if (tCPConnection)
                tcpClient.SendPacket(bytePacket);
            else
                sendPacketAckComm(bytePacket, caller);
        }

        private void sendPacketAckComm(byte[] bytePacket, string caller)
        {
            string errorMessage = "None";

            // Prevent stale overwrite of active ACK owner
            if (this.expectingAck && !string.IsNullOrEmpty(this.AcknowledgeCaller) &&
                !string.Equals(this.AcknowledgeCaller, caller, StringComparison.Ordinal))
            {
                logger.Warn(
                    "ACK already active for caller={0}; rejecting send from caller={1}. expectingAck={2}",
                    this.AcknowledgeCaller, caller, this.expectingAck);
                return;
            }

            this.AcknowledgeCaller = caller;

            try
            {
                logger.Info("sendPacketAckComm ENTER caller={0}, expectingAck={1}, loadingNewCode={2}, sciTimedOut={3}, packet=[{4}]",
                    caller, this.expectingAck, this.loadingNewCode, this.SCITimedOut, BitConverter.ToString(bytePacket));

                if (checkBoxSerialCommsDebugging.Checked)
                {
                    logger.Trace(String.Format("Sending Packet: {0}", BitConverter.ToString(bytePacket)));
                }

                this.SCITimedOut = false;

                var waitStart = DateTime.UtcNow;
                while (this.expectingAck)
                {
                    if ((DateTime.UtcNow - waitStart).TotalMilliseconds > 1000)
                    {
                        logger.Warn("sendPacketAckComm waiting for ACK. caller={0}, elapsedMs={1}, expectingAck={2}, loadingNewCode={3}, sciTimedOut={4}",
                            caller,
                            (int)(DateTime.UtcNow - waitStart).TotalMilliseconds,
                            this.expectingAck,
                            this.loadingNewCode,
                            this.SCITimedOut);
                        waitStart = DateTime.UtcNow;
                    }

                    Application.DoEvents();
                }

                logger.Info("sendPacketAckComm wait complete. caller={0}, expectingAck={1}, loadingNewCode={2}, sciTimedOut={3}",
                    caller, this.expectingAck, this.loadingNewCode, this.SCITimedOut);

                if (this.SCITimedOut)
                {
                    logger.Warn("sendPacketAckComm early return because SCITimedOut. caller={0}", caller);
                    return;
                }

                errorMessage = "Error Checking if Port is open";
                if (!this.serialPort1.IsOpen)
                {
                    logger.Warn("sendPacketAckComm port reopened. caller={0}, port={1}, baud={2}",
                        caller, this.serialPort1.PortName, this.serialPort1.BaudRate);

                    string comPort = this.serialPort1.PortName;
                    int baudRate = this.serialPort1.BaudRate;

                    this.monitoring(false);
                    this.RegisterPolling(false);
                    this.enableAll(false);
                    this.clearSerialPortBuffers(this.serialPort1);
                    this.serialPort1.Dispose();
                    GC.Collect();

                    this.serialPort1 = new MyPort(this.components);
                    this.serialPort1.PortName = comPort;
                    this.serialPort1.BaudRate = baudRate;
                    this.serialPort1.Open();
                    this.clearSerialPortBuffers(this.serialPort1);
                }

                errorMessage = "Error Writing To Port";
                Application.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;

                logger.Info("sendPacketAckComm writing packet. caller={0}, packet=[{1}]",
                    caller, BitConverter.ToString(bytePacket));

                this.serialPort1.Write(bytePacket, 0, bytePacket.Length);

                // Only set the ACK-in-flight after the actual send succeeds.
                this.expectingAck = true;
                this.timerSCITimeOut.Enabled = true;

                logger.Info("sendPacketAckComm set expectingAck=TRUE. caller={0}", caller);
                logger.Info("sendPacketAckComm enabled timerSCITimeOut. caller={0}", caller);
                logger.Info("sendPacketAckComm EXIT caller={0}", caller);

                return;
            }
            catch (Exception ex)
            {
                this.expectingAck = false;
                this.AcknowledgeCaller = string.Empty;
                this.timerSCITimeOut.Enabled = false;

                logger.Error(ex, "sendPacketAckComm EXCEPTION. caller={0}, expectingAck={1}, loadingNewCode={2}, sciTimedOut={3}",
                    caller, this.expectingAck, this.loadingNewCode, this.SCITimedOut);

                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;

                this.monitoring(false);
                this.RegisterPolling(false);
                this.enableAll(false);
                this.clearSerialPortBuffers(this.serialPort1);
                GC.Collect();
                this.messageHandler(errorMessage + ", Please Check Port.", ex);
            }
        }

        private byte[] pendingRelayTypePhasingPacket;
        private string pendingRelayTypePhasingCaller;

        private void QueueRelayTypePhasingSendIfAllowed(byte[] packet, string caller)
        {
            // If a real ACK is outstanding, do not allow the queued packet to race ahead.
            if (this.expectingAck)
            {
                logger.Info("Queued Relay Type/Phasing send after ACK still active. expectingAck=True, caller={0}", caller);
                this.pendingRelayTypePhasingPacket = packet;
                this.pendingRelayTypePhasingCaller = caller;
                return;
            }

            if (this.requestedAllParameters || this.ProgramState == ProgramStates.DownloadingAllParameters)
            {
                logger.Info("Delayed Relay Type/Phasing send because parameter download active. caller={0}", caller);
                this.pendingRelayTypePhasingPacket = packet;
                this.pendingRelayTypePhasingCaller = caller;
                return;
            }

            this.sendPacketAckComm(packet, caller);
        }

        private void TryDrainPendingRelayTypePhasingSend(string origin)
        {
            if (this.expectingAck)
            {
                logger.Info("Drain deferred because ACK still active. origin={0}", origin);
                return;
            }

            if (this.pendingRelayTypePhasingPacket == null)
            {
                logger.Info("No queued relay/phasing send to drain. origin={0}", origin);
                return;
            }

            if (this.requestedAllParameters || this.ProgramState == ProgramStates.DownloadingAllParameters)
            {
                logger.Info("Drain blocked: parameter download active. origin={0}, caller={1}", origin, this.pendingRelayTypePhasingCaller);
                return;
            }

            byte[] packet = this.pendingRelayTypePhasingPacket;
            string caller = this.pendingRelayTypePhasingCaller;

            this.pendingRelayTypePhasingPacket = null;
            this.pendingRelayTypePhasingCaller = null;

            logger.Info("Draining queued relay/phasing send. origin={0}, caller={1}", origin, caller);
            this.sendPacketAckComm(packet, caller);
        }

        private void processAckReceived(string caller)
        {
            if (!this.expectingAck)
            {
                logger.Warn("Duplicate ACK ignored. expectingAck=False, caller={0}", caller);
                return;
            }

            if (!string.IsNullOrEmpty(this.AcknowledgeCaller) &&
                !string.Equals(this.AcknowledgeCaller, caller, StringComparison.Ordinal))
            {
                logger.Warn("ACK caller mismatch. active={0}, incoming={1}; ignoring stale ACK.",
                    this.AcknowledgeCaller, caller);
                return;
            }

            logger.Info("ACK received. caller={0}, expectingAck(after)=False", caller);

            this.expectingAck = false;
            this.timerSCITimeOut.Enabled = false;
            this.AcknowledgeCaller = string.Empty;

            if (this.requestedAllParameters || this.ProgramState == ProgramStates.DownloadingAllParameters ||
                this.loadingNewCode || this.backupInProgress)
            {
                logger.Info("Drain blocked (ACK-RX-{0}): parameter download active. requestedAllParameters={1}, ProgramState={2}",
                    caller, this.requestedAllParameters, this.ProgramState);
                return;
            }

            this.TryDrainPendingRelayTypePhasingSend("ACK-RX-" + caller);
        }

        private void sendPacket(byte[] bytePacket)
        {
            if (tCPConnection)
                tcpClient.SendPacket(bytePacket);
            else
                sendComPacket(bytePacket);
        }

        private void sendComPacket(byte[] bytePacket)
        {
            string errorMessage = "None";
            if (checkBoxSerialCommsDebugging.Checked)
            {
                logger.Trace("Send OpCode: {0}", Convert.ToChar(bytePacket[0]));
                logger.Trace(String.Format("Sending Packet: {0}", BitConverter.ToString(bytePacket)));
            }
            try
            {
                errorMessage = "Error Checking if Port is open";
                if (!this.serialPort1.IsOpen)
                {
                    string comPort = this.serialPort1.PortName;
                    int baudRate = this.serialPort1.BaudRate;

                    this.monitoring(false);
                    this.RegisterPolling(false);
                    this.enableAll(false);
                    this.clearSerialPortBuffers(this.serialPort1);
                    GC.Collect();

                    this.serialPort1 = new MyPort(this.components);
                    this.serialPort1.PortName = comPort;
                    this.serialPort1.BaudRate = baudRate;
                    this.serialPort1.Open();
                }

                errorMessage = "Error Checking For Bytes left to read";

                ++this.byteCount;
                errorMessage = "Error Writing To Port";

                this.serialPort1.Write(bytePacket, 0, bytePacket.Length);

                errorMessage = "Error At End of Routine";
                return;
            }
            catch (Exception ex)
            {
                this.portLost = true;
                this.monitoring(false);
                this.enableAll(false);
                this.serialPort1.Dispose();
                this.messageHandler(errorMessage + " Please Check Port and Restart the Program", ex);
            }
        }

        private bool monitorPort = false;
        private delegate void updatePortBoxCallBack(byte b, Color c);

        #region Calibration

        void ucCalibration2_Send(object sender, SendEventArgs sEA)
        {
            if (sEA.SendPacket[0] == (byte)'m' || sEA.SendPacket[0] == (byte)'c') //For low or high Cal
            {
                DialogResult dR = new YesNoMessageBoxResized("Calibration", "This may take a few moments to complete.\r\n\nCalibrate Unit?", "Yes", "No").ShowDialog();
                if (dR == DialogResult.Yes)
                {
                    this.sendPacket(sEA.SendPacket);
                    this.downloadingDialogCountDown("Calibrating ", "Calibration", 60);
                }
            }
            else
                this.sendPacket(sEA.SendPacket);
        }


        private void startCalibrationTimer()
        {
            this.activeStatusBarCountDown("Calibration Relay ", 60);
        }


        private void calibrationComplete(byte[] bytePacket)
        {
            string s;
            this.expectingAck = false;
            this.timerSCITimeOut.Enabled = false;
            this.monitoring(false);

            if (this.downloadProgress != null)
            {
                this.downloadProgress.Dispose();
            }
            //if (bytePacket[1] == 1)
            //{
            //    s = "Error During Calibration, Please Check Input Values";

            //    this.toolStripStatusLabelMain.Text = "Ready";
            //    this.messageHandler("Calibration", s);
            //}
            //else 
            if (bytePacket[1] == 2)
            { // calibration constants are saved / stored from relay uP to master uP
                DialogResult msg = new YesNoMessageBoxResized("Calibration Complete", "Calibration Values Saved", "ok").ShowDialog();
            }
            else if (bytePacket[1] == 0)
            {
                bool tempBool1, tempBool2;
                tempBool1 = this.monitorPort;
                tempBool2 = this.timerRegisterPolling.Enabled;

                this.monitoring(false);
                this.RegisterPolling(false);
                DialogResult dR = new YesNoMessageBoxResized("Calibration Complete", "Save Calibration Constants?", "Yes", "No").ShowDialog();
                if (dR == DialogResult.Yes)
                    this.sendSaveCalibration(); // tells master to send the calibration constants that are saved in its flash
                this.monitoring(tempBool1);
                this.RegisterPolling(tempBool2);
                this.toolStripStatusLabelMain.Text = "Ready";
            }
            else
            {
                s = "Error During Calibration, Error Code : " + bytePacket[1].ToString();

                this.toolStripStatusLabelMain.Text = "Ready";
                this.messageHandler("Calibration", s);
            }
            this.enableAll(true);
            this.monitoring(true);
            this.RegisterPolling(true);
        }

        private void sendSaveCalibration()
        { // asks the master to send the calibration data saved in it memory
            byte[] packet = new byte[3];

            packet[0] = (byte)'a';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacketAck(packet, "Save Calibration Send");
        }

        private Int16 calibrationCount = 0;
        private string statusLabel = "Calibrating Relay ";
        private int statusTickCount = 60;

        private void timerCalibrationTimer_Tick(object sender, EventArgs e)
        {
            this.calibrationCount++;
            this.monitoring(false);
            if (this.calibrationCount == statusTickCount)
            {
                this.calibrationCount = 0;
                this.timerTimeOutCountdown.Enabled = false;
                this.timerLiveEventAcknowledge.Enabled = false;

                this.messageHandler(statusLabel, "Relay Response Timed Out.");
                this.requestLiveDataToolStripMenuItem1.Enabled = true;
                this.buttonRQEventData.Enabled = true;
                this.buttonReqLiveData.Enabled = true;
                this.toolStripStatusLabelMain.Text = "Ready";
            }
            else if (this.calibrationCount % 5 == 0)
            {
                this.toolStripStatusLabelMain.Text = this.statusLabel;
            }
            else
            {
                this.toolStripStatusLabelMain.Text += ".";
            }
        }

        #endregion

        private void serialPort1_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            this.clearSerialPortBuffers(this.serialPort1);
            this.serialPort1.Close();
        }

        private void buttonSendCTRatio_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            screenD.screenDisable = true;

            try
            {
                this.SendCTRatioAndPhasing(requestAfter : false);
            }
            finally
            {
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                screenD.screenDisable = false;
            }
        }
        private void buttonResetMaster_Click(object sender, EventArgs e)
        {
            this.resetMaster();
        }

        private void resetMaster()
        {
            byte[] packet = new byte[3];

            packet[0] = (byte)'b';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacket(packet);
        }

        // Fields
        private readonly object _ackFlowLock = new object();
#pragma warning disable CS0414
        private bool _pendingRelayTypePhasingSend = false;
        private int _drainInProgress = 0;
#pragma warning restore CS0414

#pragma warning disable CS0169
        private DateTime _pendingRelayTypePhasingQueuedAtUtc;
#pragma warning restore CS0169



        // Optional: if you can, make this volatile or always access under lock
        //private string AcknowledgeCaller = string.Empty;

        private void sendCTRatio(bool requestAfter)
        {
            logger.Info("sendCTRatio ENTER: CTRatio={0}, expectingAck={1}, loadingNewCode={2}, sciTimedOut={3}",
                this.CTRatio, this.expectingAck, this.loadingNewCode, this.SCITimedOut);

            byte[] packet = new byte[4];
            packet[0] = (byte)'Z';
            packet[1] = (byte)this.CTRatio;
            packet[2] = (byte)(this.CTRatio >> 8);
            packet[3] = 0x0D;

            if (this.expectingAck)
            {
                logger.Warn("sendCTRatio skipped because expectingAck is already TRUE. caller=CT Ratio Send");
                return;
            }

            this.sendPacketAck(packet, "CT Ratio Send");
        }

        private void buttonClearCycleCount_Click(object sender, EventArgs e)
        {
            byte[] packet = new byte[3];

            packet[0] = (byte)'O';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacket(packet);
        }

        private void textBoxCTRatio_Leave(object sender, EventArgs e)
        {
            int cTRatio5 = this.CTRatio;

            if (this.comboBox_CTRatio.SelectedIndex == 8)
            {
                try
                {
                    cTRatio5 = Convert.ToInt16(this.textBoxCTRatio.Text);
                    if (this.Customer == Customers.CONED)
                    {
                        if (cTRatio5 < 0)
                            cTRatio5 = -cTRatio5;

                        if (cTRatio5 > 12750)
                        {
                            this.textBoxCTRatio.Text = "12750";
                            cTRatio5 = 12750;
                        }
                        else if (cTRatio5 < 5)
                        {
                            cTRatio5 = 5;
                            this.textBoxCTRatio.Text = "5";
                        }
                    }
                    else
                    {
                        if (cTRatio5 > 65535)
                        {
                            this.textBoxCTRatio.Text = "65535";
                            cTRatio5 = 65535;
                        }
                        if (cTRatio5 < 5)
                        {
                            this.textBoxCTRatio.Text = "5";
                            cTRatio5 = 5;
                        }
                    }
                    this.updateCTRatio(cTRatio5 / 5);
                }
                catch
                {
                    this.textBoxCTRatio.Leave -= new System.EventHandler(textBoxCTRatio_Leave);
                    this.messageHandler("Invalid CT Ratio.", new Exception("Can't set CT Ratio above 12750.  Please check CT Ratio and send to relay."));
                    this.updateCTRatio(this.CTRatio);
                    this.textBoxCTRatio.Leave += new EventHandler(textBoxCTRatio_Leave);
                }
            }
        }

        //private void domainUpDownCTRatioM_SelectedItemChanged(object sender, EventArgs e)
        private void comboBox_CTRatio_SelectedItemChanged(object sender, EventArgs e)
        {
            //DomainUpDown dUD = (DomainUpDown)sender;
            int ratio = this.CTRatio;
            int ratio5 = this.CTRatio * 5;

            //switch (dUD.SelectedIndex)
            switch (this.comboBox_CTRatio.SelectedIndex)
            {
                case 7:
                    this.CTRatio = ratio = 160;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    this.comboBox_CTRatio.Text = ratio5.ToString();
                    break;
                case 6:
                    this.CTRatio = ratio = 240;
                    ratio5 = ratio * 5;
                    //this.CTRatio = ratio;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    this.comboBox_CTRatio.Text = ratio5.ToString();
                    break;
                case 5:
                    this.CTRatio = ratio = 320;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    this.comboBox_CTRatio.Text = ratio5.ToString();
                    break;
                case 4:
                    this.CTRatio = ratio = 400;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    this.comboBox_CTRatio.Text = ratio5.ToString();
                    break;
                case 3:
                    this.CTRatio = ratio = 500;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    this.comboBox_CTRatio.Text = ratio5.ToString();
                    break;
                case 2:
                    this.CTRatio = ratio = 600;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    this.comboBox_CTRatio.Text = ratio5.ToString();
                    break;
                case 1:
                    this.CTRatio = ratio = 700;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    this.comboBox_CTRatio.Text = ratio5.ToString();
                    break;
                case 0: //3750 : 5
                    this.CTRatio = ratio = 750;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    this.comboBox_CTRatio.Text = ratio5.ToString();
                    break;
                case 8:
                    try
                    {
                        this.textBoxCTRatio.Text = ratio5.ToString();
                    }
                    catch
                    {

                    }
                    this.textBoxCTRatio.Enabled = true;
                    break;
                default:
                    this.CTRatio = ratio = 160;
                    ratio5 = 160 * 5;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    break;
            }

            this.updateCTRatio(ratio);
        }

        private void buttonTripRelay_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.messageHandler("Trip Relay", "Do you really want to trip the relay?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            byte[] bytePacket;
            TripModeDefinition tMD = new TripModeDefinition(TripModes.RemoteTrip);

            if (dr == DialogResult.Yes)
            {
                bytePacket = RelayModeFunctions.BytePacketFor(tMD);
                this.sendPacket(bytePacket);
            }
        }

        private void buttonBlockAndTrip_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.messageHandler("Trip Relay", "Do you really want to block open and trip the relay?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            byte[] bytePacket;
            TripModeDefinition tMD = new TripModeDefinition(TripModes.RemoteTrip);

            if (dr == DialogResult.Yes)
            {
                ucBlockControl1.SendBlockState(true);

                bytePacket = RelayModeFunctions.BytePacketFor(tMD);
                this.sendPacket(bytePacket);
            }
        }
       
        private void resetBothProcs()
        {
            byte[] packet = new byte[3];

            packet[0] = (byte)'B';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacket(packet);
        }

        private bool registersReceived = false;
        private bool portLost = false;
        private bool everyOtherMonitor = false;
        private ulong transmitterMonitoringCount = 0;
        private bool temp = true;
        private void timerRegisterPolling_Tick(object sender, EventArgs e)
        {
            this.timerRegisterPolling.Enabled = false;

            if (this.pauseMonitoring)// || this.noMonitoringVersion)
            {
                this.timerRegisterPolling.Enabled = true;
                this.pauseTransmitterMonitoring();
                return;
            }

            if (ucRelayProgramming1.ReprogrammingInProgress)
            {
                this.timerRegisterPolling.Enabled = false;
                return;
            }

            if (!this.pendingAutoloadAfterBackup &&
                !this.ucRelayProgramming1.ReprogrammingInProgress &&
                this.ucRelayProgramming1.State != RelayProgrammingStates.ReprogramSuccess)
            {
                this.loadingNewCode = false;
            }

            if (!this.serialPort1.IsOpen && !tCPConnection)
            {
                if (this.portLost)
                {
                    this.relayFound = false;
                    if (this.checkPortAvailability(this.serialPort1.PortName))
                        this.checkPortForRelay();
                    if (this.relayFound)
                    {
                        this.requestRelayRevision();
                        this.enableAll(true);
                        this.RegisterPolling(true);
                        this.portLost = false;
                    }
                }
                else
                {
                    this.portLost = true;
                    this.enableAll(false);
                    this.monitoring(false);
                    this.serialPort1.Dispose();
                    this.messageHandler("Error on Port", "Port Is Closed, Please Restart Program");
                    this.toolStripStatusLabelMain.Text = "No Relay Found";
                }

                return;
            }
            if (!this.registersReceived)
            {
                setLabelText("Unknown", this.labelRelayTrippedOrClose);
                setBackgroundColor(Color.Transparent, this.labelRelayTrippedOrClose);
                

                this.enableFlagsAndStatus(false);

                if (!this.relayFound)
                    this.requestMasterRevisionNumber();
            }

            if (!this.quietMode)
            {
                if (this.pendingAutoloadAfterBackup || this.ucRelayProgramming1.ReprogrammingInProgress || this.loadingNewCode)
                {
                    logger.Info("Suppressing register polling during backup/programming transition.");
                }
                else
                {
                    this.registersReceived = false;
                    this.requestRelayRegisters();
                    this.labelQuietMode.Visible = false;
                }
            }
            else
            {
                if (this.pendingAutoloadAfterBackup || this.ucRelayProgramming1.ReprogrammingInProgress || this.loadingNewCode)
                {
                    logger.Info("Suppressing Quiet Mode popup during backup/programming transition.");
                }
                else
                {
                    this.messageHandler("Set To Quiet Mode", "Unset Quiet Mode");
                }

                this.labelQuietMode.Visible = true;
            }

            if (this.arcFaultMonitoringEnabled)
            {
                this.arcFault_requestArcFaultMonitoring();
            }
            ulong timeTemp = 0;

            if (this.requestingDNPData)
            {
                this.requestDNPData();
            }

            if (this.everyOtherMonitor)
            {
                if (!this.phasorReceived)
                {
                    // if (this.relayFound_forDNPdataMonitoring)
                    // {
                    //     this.enableDNPMonitoring(this.tabControlMain.SelectedTab == this.tabPageDNPData);
                    //     temp = true;
                    // }
                    if (this.missedMonitoringCount >= 2)
                    {
                        this.ucShortRange1.relayFound_forRNCMonitoring = false;
                        this.relayFound_forDNPdataMonitoring = false;

                        if ((this.missedMonitoringCount == 2) && (temp == true) && (this.relayFound_forDNPdataMonitoring == false))
                        {
                            this.enableDNPMonitoring(false);
                            temp = false;
                            //string text = "Relay not found. Please check for its Power and then Request for the DNP data ";
                            string text = "Relay not found. Please check for its Power and Connection.";
                            MessageBox.Show(text);

                        }

                        if (this.transmitterMonitoring || this.pQMonitoringEnabled)
                        {
                            this.pauseTransmitterMonitoring();
                            this.disableAllMonitoring(); //pause_error testing

                            if (this.relayFound == true && this.toolStripStatusLabelRelayDisconnected.Visible == false)
                            {
                                if (this.customer == Customers.TORONTO_HYDRO)
                                {
                                    this.ucTransmitterMonitoring1.TransmitterMonitoring = false;
                                }
                                else
                                {
                                    this.ucTransmitterMonitoring1.TransmitterMonitoring = true;
                                }
                                enableAllMonitoring();
                                missedMonitoringCount = 3;

                            }
                        }
                    }
                    else
                    {
                        this.missedMonitoringCount++;
                        if (this.pQMonitoringEnabled || this.transmitterMonitoring)
                            this.requestPhasorData();
                        if (this.transmitterMonitoring)                  //request the data again if we haven't timed out yet.
                            this.requestTransmitterMonitorData();
                    }
                }
                else
                {
                    this.everyOtherMonitor = false;
                    if (this.pQMonitoringEnabled)
                        this.requestPhasorData();

                    if (this.transmitterMonitoring)
                    {
                        this.requestTransmitterMonitorData();
                        this.requestPhasorData();

                        timeTemp = transmitterMonitoringCount % 60;
                        //this.textBoxTimeElapsedSeconds.Text = timeTemp.ToString("00");
                        this.ucTransmitterMonitoring1.TimeElapsedSeconds = timeTemp.ToString("00");

                        timeTemp = (transmitterMonitoringCount / 60) % 60;
                        //this.textBoxTimeElapsedMinutes.Text = timeTemp.ToString("00");
                        this.ucTransmitterMonitoring1.TimeElapsedMinutes = timeTemp.ToString("00");

                        timeTemp = transmitterMonitoringCount / 3600;
                        //this.textBoxTimeElapsedHours.Text = timeTemp.ToString();
                        this.ucTransmitterMonitoring1.TimeElapsedHours = timeTemp.ToString();

                        transmitterMonitoringCount += 3;
                        return;
                    }
                }
            }
            else
            {
                this.everyOtherMonitor = true;
            }
        }

        private void enableFlagsAndStatus(bool b)
        {
            this.enableCheckBox(b, this.checkBoxACB);
            //  this.enableCheckBox(b, this.checkBoxDefaultsUsed);
            this.enableCheckBox(b, this.checkBoxBlockedCloseFlag);
            // this.enableCheckBox(b, this.checkBoxBlockedOpenFlag);
            this.enableCheckBox(b, this.checkBoxCalibrating);
            //  this.enableCheckBox(b, this.checkBoxFloatFlag);
            this.enableCheckBox(b, this.checkBoxMathError);
            this.enableCheckBox(b, this.checkBoxMathOverTime);
            this.enableCheckBox(b, this.checkBoxMonitorPhasors);
            this.enableCheckBox(b, this.checkBoxOffsetOkay);
            // this.enableCheckBox(b, this.checkBoxPhasingOkayFlag);
            this.enableCheckBox(b, this.checkBoxPowerSaveFlag);
            // this.enableCheckBox(b, this.checkBoxPumping);
            this.enableCheckBox(b, this.checkBoxSequence);
            this.enableCheckBox(b, this.checkBoxFlag1);
            this.enableCheckBox(b, this.checkBoxFlag2);
            //  this.enableCheckBox(b, this.checkBoxBFlag);
            // this.labelNWPStatus.Enabled = b;
            if (!b)
            {
                // this.labelNWPStatus.Text = "NWP: Unknown";
                this.txtBox_NWPposition.Text = "Unknown";
            }
            this.enableCheckBox(b, this.checkBoxInInsensRegion);
            this.enableCheckBox(b, this.checkBoxInTripRegion);
            // this.enableCheckBox(b, this.checkBoxTripFlag);
            //  this.enableCheckBox(b, this.checkBoxTrippingFlag);

            this.showLabel(!b, this.labelRelayDisconnected);
            this.showLabel(!b, this.labelRelayDisconnected2);
            this.showLabel(!b, this.labelRelayDisconnected3);
            this.showLabel(!b, this.toolStripStatusLabelRelayDisconnected);

            if (!b)
            {
                this.setTextBox("", this.textBoxTripCount);
                this.ucPumpMode1.PumpReason = PumpReasons.NoPump;
                this.ucPumpMode1.PumpProtectEnabled = false;
                this.checkSerialNumber = true;
            }
        }

        private delegate void enableCheckBoxCallback(bool b, CheckBox cB);

        private void enableCheckBox(bool b, CheckBox checkBox)
        {
            if (checkBox.InvokeRequired)
            {
                enableCheckBoxCallback eCBC = new enableCheckBoxCallback(this.enableCheckBox);
                this.Invoke(eCBC, new object[] { b, checkBox });
            }
            else
            {
                checkBox.Enabled = b;
            }
        }

        #region Event Page

        private IDictionary eventDictionary = new Dictionary<object, int>();

        private void initializeEventPage()
        {
            this.initializeEventDictionary();

            this.ucEventGraph0.Type = EventTypes.NoEvent;
            this.ucEventGraph1.Type = EventTypes.NoEvent;
            this.ucEventGraph2.Type = EventTypes.NoEvent;
            this.ucEventGraph3.Type = EventTypes.NoEvent;
            this.ucEventGraph4.Type = EventTypes.NoEvent;
            this.ucEventGraph5.Type = EventTypes.NoEvent;
            this.ucEventGraph6.Type = EventTypes.NoEvent;
            this.ucEventGraph7.Type = EventTypes.NoEvent;
        }

        private void initializeEventDictionary()
        {
            this.eventDictionary.Add(this.radioButtonEvent0, 0);
            this.eventDictionary.Add(this.radioButtonEvent1, 1);
            this.eventDictionary.Add(this.radioButtonEvent2, 2);
            this.eventDictionary.Add(this.radioButtonEvent3, 3);
            this.eventDictionary.Add(this.radioButtonEvent4, 4);
            this.eventDictionary.Add(this.radioButtonEvent5, 5);
            this.eventDictionary.Add(this.radioButtonEvent6, 6);
            this.eventDictionary.Add(this.radioButtonEvent7, 7);
        }

        private void radioButtonEventSelect_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rB = (RadioButton)sender;

            int i = (int)this.eventDictionary[rB];

            switch (i)
            {
                default:
                case 0:
                    this.ucEventGraph0.BringToFront();
                    break;
                case 1:
                    this.ucEventGraph1.BringToFront();
                    break;
                case 2:
                    this.ucEventGraph2.BringToFront();
                    break;
                case 3:
                    this.ucEventGraph3.BringToFront();
                    break;
                case 4:
                    this.ucEventGraph4.BringToFront();
                    break;
                case 5:
                    this.ucEventGraph5.BringToFront();
                    break;
                case 6:
                    this.ucEventGraph6.BringToFront();
                    break;
                case 7:
                    this.ucEventGraph7.BringToFront();
                    break;
            }
        }

        private void buttonReqLiveData_Click(object sender, EventArgs e)
        {
            // string text = " Getting LIVE data from the realy ! ";
            // MessageBox.Show(text);
            this.requestLiveDataToolStripMenuItem1_Click(sender, e);
        }

        private bool initialLiveEventRequest = false;
        private void buttonRQEventData_Click(object sender, EventArgs e)
        {
            this.requestEventDownload();
        }

        private void downloadEventFromRelayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.requestEventDownload();
        }

        private bool downloadEventsClicked = false;
        void requestEventDownload()
        {
            this.downloadingCanceled = false;
            this.buttonRQEventData.Enabled = false;
            this.requestLiveDataToolStripMenuItem1.Enabled = false;

            this.requestTransmitterSettings();
            DialogResult dR = this.messageHandler("Continue?", "This will take a while. \r\n Please Be Patient.", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (dR != DialogResult.OK)
            {
                this.buttonRQEventData.Enabled = true;
                this.requestLiveDataToolStripMenuItem1.Enabled = true;
                return;
            }

            downloadEventsClicked = true;

            this.ucEventGraph0.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph1.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph2.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph3.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph4.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph5.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph6.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph7.RelayID = this.ucTransmitter1.TXSettings.ID;

            this.downloadingLiveData = false;
            this.initialLiveEventRequest = true;
            this.requestEventTimes();
            this.timerLiveEventAcknowledge.Enabled = true;
            this.monitoring(false);
            this.RegisterPolling(false);
        }

        int getEventDownloadTime()
        {
            int downloadTime = 100;// 50;
            if (tCPConnection)
                downloadTime = 100;

            downloadTime *= downloadableEvents;

            return downloadTime;
        }

        private DateTime liveDataTriggerTime;
        private void requestLiveDataToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if ((this.serialPort1 == null || !this.serialPort1.IsOpen) && !tCPConnection)
                return;

            this.downloadingCanceled = false;
            this.requestLiveDataToolStripMenuItem1.Enabled = false;
            this.buttonRQEventData.Enabled = false;

            this.liveDataTriggerTime = DateTime.UtcNow;

            this.labelLiveDataTriggerTime.Text = DateTime.Now.ToString();

            DialogResult dR = this.messageHandler("Downloading Live Data", "Downloading Data.  \r\nThis will take a while.  Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);


            if (dR == DialogResult.OK)
            {
                this.tabControlMain.SelectedTab = this.tabPageFlightRecorder;
                this.ackCount = 0;
                this.liveEventTimedOutCount = 0;
                this.nAckCount = 0;
                this.downloadingLiveData = true;
                this.initialLiveEventRequest = true;
                this.requestLiveData();
                this.monitoring(false);
                this.RegisterPolling(false);
                this.timerLiveEventAcknowledge.Start();
            }
            else
            {
                this.downloadingLiveData = false;
                this.requestLiveDataToolStripMenuItem1.Enabled = true;
                this.buttonRQEventData.Enabled = true;
            }
        }

        private void activeStatusBarCountDown(string label, int halfSecondTickCounts)
        {
            this.statusLabel = label;
            this.toolStripStatusLabelMain.Text = this.statusLabel;
            this.calibrationCount = 0;
            this.statusTickCount = halfSecondTickCounts;
            this.timerTimeOutCountdown.Enabled = true;
        }

        private void requestEventData(int eventValue)
        {
            byte[] sendPacket = new byte[3];

            sendPacket[0] = (byte)'k';

            if (eventValue == 0 && this.ucEventGraph0.Type != EventTypes.NoEvent)
            {
                sendPacket[1] = 0x01;
            }
            else if (eventValue == 1 && this.ucEventGraph1.Type != EventTypes.NoEvent)
            {
                sendPacket[1] = 0x02;
            }
            else if (eventValue == 2 && this.ucEventGraph2.Type != EventTypes.NoEvent)
            {
                sendPacket[1] = 0x03;
            }
            else if (eventValue == 3 && this.ucEventGraph3.Type != EventTypes.NoEvent)
            {
                sendPacket[1] = 0x04;
            }
            else if (eventValue == 4 && this.ucEventGraph4.Type != EventTypes.NoEvent)
            {
                sendPacket[1] = 0x05;
            }
            else if (eventValue == 5 && this.ucEventGraph5.Type != EventTypes.NoEvent)
            {
                sendPacket[1] = 0x06;
            }
            else if (eventValue == 6 && this.ucEventGraph6.Type != EventTypes.NoEvent)
            {
                sendPacket[1] = 0x07;
            }
            else if (eventValue == 7 && this.ucEventGraph7.Type != EventTypes.NoEvent)
            {
                sendPacket[1] = 0x08;
            }
            else
            {
                return;
            }

            sendPacket[2] = 0x0D;

            this.sendPacket(sendPacket);
        }

        private void requestEventTimes()
        {
            byte[] sendPacket = new byte[3];

            sendPacket[0] = (byte)'h';
            sendPacket[1] = 0x55;
            sendPacket[2] = 0x0D;

            this.sendPacket(sendPacket);
        }

        private void eventLiveDataPacket(byte[] bytePacket)
        {
            this.timerLiveEventAcknowledge.Enabled = false;
            if (this.downloadingCanceled)
                return;
            if (bytePacket[0] == 0 && this.downloadingLiveData)
                this.liveDataPacket(bytePacket);
            else if (bytePacket[0] > 0 && bytePacket[0] <= 8 && !this.downloadingLiveData)
                this.eventDataPacket(bytePacket);
            else
            {
                this.nAcknowledge();
                this.timerLiveEventAcknowledge.Start();
            }
        }

        //FOR TEST!!!!
        private int getIndexOffset(byte cycleNumber, byte half)
        {
            int returnInt;

            if (cycleNumber >= 65)
                throw new Exception(cycleNumber.ToString() + " is a bad cycle number.");

            returnInt = (cycleNumber - 1) * 128;

            if (half == 1)
            {
                return returnInt;
            }
            else if (half == 0)
            {
                return returnInt + 64;
            }
            else
            {
                return returnInt;
            }
        }

        private void eventDataPacket(byte[] bytePacket)
        {
            string errorString = "Begin Event Packet";
            ucEventGraph workingGraph;
            try
            {
                //Set Event to Update
                switch (bytePacket[0])
                {
                    case 1:
                        workingGraph = this.ucEventGraph0;
                        break;
                    case 2:
                        workingGraph = this.ucEventGraph1;
                        break;
                    case 3:
                        workingGraph = this.ucEventGraph2;
                        break;
                    case 4:
                        workingGraph = this.ucEventGraph3;
                        break;
                    case 5:
                        workingGraph = this.ucEventGraph4;
                        break;
                    case 6:
                        workingGraph = this.ucEventGraph5;
                        break;
                    case 7:
                        workingGraph = this.ucEventGraph6;
                        break;
                    case 8:
                        workingGraph = this.ucEventGraph7;
                        break;
                    default:
                        errorString = "Bad Event Number";
                        throw new Exception();
                }
                errorString = "Passing Data to Event Block";
                workingGraph.SetAll(bytePacket);
            }
            catch (Exception ex)
            {
                this.messageHandler(errorString, ex);
            }

        }

        #endregion

        #region Live Data

        private void liveDataPacket(byte[] bytePacket)
        {
            this.ucLiveData1.PacketHandler(bytePacket);
        }

        #endregion

        #region Error Handling
        /// <summary>
        /// Handles errors by temporarily disabling inputs and stopping monitoring to avoid overflows
        /// </summary>
        /// <param name="title">String to display in title bar</param>
        /// <param name="ex">Actually Exception</param>
        private void messageHandler(string title, Exception ex)
        {
            this.messageHandler(title, ex.Message);
        }

        private void messageHandler(string title, string message)
        {
            bool tempBool, tempBool2, tempBool3;

            if (this.loadingNewCode && title != "Relay Programming")
                return;

            tempBool = this.pQMonitoringEnabled;
            tempBool2 = this.allEnabled;
            tempBool3 = !this.pauseMonitoring;
            try
            {
                this.enableAll(false);
                this.monitoring(false);
                this.RegisterPolling(false);

                MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);//, MessageBoxOptions.ServiceNotification);

                this.enableAll(tempBool2);
                this.monitoring(tempBool);
                this.RegisterPolling(tempBool3);
            }
            catch (Exception exc)
            {
                this.RegisterPolling(false);
                MessageBox.Show(exc.Message, "Error In Message Box");
                this.RegisterPolling(tempBool3);
            }
        }

        private DialogResult messageHandler(string title, string message, MessageBoxButtons messageBoxButtons)
        {
            bool tempBool, tempBool2, tempBool3;
            DialogResult dR;

            tempBool = this.pQMonitoringEnabled;
            tempBool2 = this.allEnabled;
            tempBool3 = !this.pauseMonitoring;

            dR = DialogResult.No;

            try
            {
                this.monitoring(false);
                this.enableAll(false);
                this.RegisterPolling(false);

                dR = MessageBox.Show(message, title, messageBoxButtons, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);//, MessageBoxOptions.ServiceNotification);

                this.monitoring(tempBool);
                this.enableAll(tempBool2);
                this.RegisterPolling(tempBool3);
            }
            catch (Exception exc)
            {
                this.RegisterPolling(false);
                this.messageHandler("Error In Dialog Box", exc);
                this.RegisterPolling(tempBool3);
            }

            return dR;
        }

        private DialogResult messageHandler(string title, string message, MessageBoxButtons messageBoxButtons, MessageBoxIcon messageBoxIcon, MessageBoxDefaultButton messageBoxDefaultButton)
        {
            bool tempBool, tempBool2, tempBool3;
            DialogResult dR;

            tempBool = this.pQMonitoringEnabled;
            tempBool2 = this.allEnabled;
            tempBool3 = !this.pauseMonitoring;

            dR = DialogResult.No;

            try
            {
                this.enableAll(false);
                this.monitoring(false);
                this.RegisterPolling(false);

                dR = MessageBox.Show(message, title, messageBoxButtons, messageBoxIcon, messageBoxDefaultButton);//, MessageBoxOptions.ServiceNotification);

                this.monitoring(tempBool);
                this.enableAll(tempBool2);
                this.RegisterPolling(tempBool3);
            }
            catch (Exception exc)
            {
                this.RegisterPolling(false);
                this.messageHandler("Error In Dialog Box", exc);
                this.RegisterPolling(tempBool3);
            }

            return dR;
        }

        #endregion

        private void buttonEnableAll_Click(object sender, EventArgs e)
        {
            this.enableAll(true);
        }

        private void enableAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.enableAll(true);
        }

        private float[] generateSineWave(int samples, float amplitude)
        {
            float[] returnArray = new float[samples];
            double temp;
            for (int i = 0; i < samples; i++)
            {
                temp = (double)i / (double)samples;
                temp = Math.PI * temp * 2d;
                temp = (double)amplitude * Math.Sin(temp);
                returnArray[i] = (float)temp;
            }

            return returnArray;
        }
        private SavedSettingsV4 saveObject = new SavedSettingsV4();
        private SavedSettingsV3 saveObjectV3 = new SavedSettingsV3();

        private void buttonSaveSetting_Click(object sender, EventArgs e)
        {
            SavedSettingV4 sS = new SavedSettingV4();
            this.data_BackUp_reprogramingOldRelay();
            try
            {
                if (!validTextBoxValue(this.textBoxSaveStateName.Text))
                {
                    throw new Exception("Bad Save Setting Name");
                }

                sS.Name = this.textBoxSaveStateName.Text;

#if DOMINION
                sS.Name += " SN:" + this.textBoxRelaySNControl.Text + "/";
                sS.Name += "ID:" + this.ucTransmitter1.TXSettings.ID + "/";
                sS.Name += "Date:" + DateTime.Now.ToString("yyyyMMdd") + "/";
                sS.Name += "Time:" + DateTime.Now.ToString("HH:mm");
#endif


                this.getAllSaveStates(sS);
                this.saveObject.AddState(sS);
                this.writeSaveObjectToFile();

            }
            catch (Exception ex)
            {
                this.messageHandler("Error Saving Setting", ex);
            }
        }

        private bool validTextBoxValue(string p)
        {
            try
            {
                string s = this.textBoxSaveStateName.Text;
                if (s == "" || s == null)
                    throw new Exception();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void writeSaveObjectToFile()
        {
            Stream stream = File.Open(@"C:\DGI Systems\Relay\Saved Data\SavedSettings.dgi", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, this.saveObject);
            stream.Close();
            this.initializeSaveObject();
        }

        private void initializeSaveObject()
        {
            {
                SavedSettingV4 sS = new SavedSettingV4();
                int i = 0;

                try
                {
                    using (Stream stream = File.Open(@"C:\DGI Systems\Relay\Saved Data\SavedSettings.dgi", FileMode.OpenOrCreate))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();

                        if (stream.Length != 0)
                        {
                            try
                            {
                                this.saveObject = (SavedSettingsV4)formatter.Deserialize(stream);
                            }
                            catch (Exception)
                            {
                                // Recover from bad/old/corrupt saved settings without crashing startup.
                                // Keep defaults and continue startup.
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.messageHandler("Error Opening Save File", ex);
                }

                try
                {
                    this.comboBoxSavedStates.Items.Clear();
                    if (this.saveObject == null)
                        return;

                    for (i = 0; i < this.saveObject.Settings.Count; ++i)
                    {
                        if (this.saveObject.Settings[i].Name != null && this.saveObject.Settings[i].Name != "")
                        {
                            this.comboBoxSavedStates.Items.Add(this.saveObject.Settings[i].Name);
                        }
                    }
                    this.comboBoxSavedStates.Text = "Select Profile"; //"";
                }
                catch (Exception ex)
                {
                    this.messageHandler("Error Populating Saved States ComboBox", ex);
                }
            }
        }

        private void buttonDeleteSetting_Click(object sender, EventArgs e)
        {
            //if (this.comboBoxSavedStates.Text != "" && this.comboBoxSavedStates.Text != null)
            if (this.comboBoxSavedStates.Text != "Select Profile" && this.comboBoxSavedStates.Text != "" && this.comboBoxSavedStates.Text != null)
                this.saveObject.DeleteState(this.comboBoxSavedStates.Text);

            this.writeSaveObjectToFile();
            this.initializeSaveObject();
        }

        private void getAllSaveStates(SavedSettingV4 sS)
        {
            sS.TripSettings = this.ucTripMode2.GetSavedState();
            sS.PumpSettings = this.ucPumpMode1.GetSavedState();
            sS.CloseSettings = this.ucCloseMode1.GetSavedState();
#if DNP
            sS.DNPSettings = this.ucDNP.GetSavedState();
#endif
            sS.SafeServiceSettings = this.ucSafeService1.GetSavedState();
            sS.CTRatio = this.CTRatio;
            sS.Phasing = this.comboBox_Phasings.SelectedIndex;
            sS.RelayType = this.comboBox_RelayType.SelectedIndex;
            sS.V277Protector = protectorVoltage.SetBit.HasFlag(ProtectorVoltageBits.V277);
            sS.V600Protector = protectorVoltage.SetBit.HasFlag(ProtectorVoltageBits.V600);
            sS.ProtectorVoltageOutputs = true;

        }

        private void comboBox_LoadProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            SavedSettingV4 sS = new SavedSettingV4();

            if (this.comboBoxSavedStates.SelectedItem == null)
                return;
           
            try
            {
                foreach (SavedSettingV4 s in this.saveObject.Settings)
                {
                    if (s.Name.Equals(this.comboBoxSavedStates.SelectedItem))
                    {
                        sS = s;
                        break;
                    }
                }
                this.setAllValues(sS);
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Selecting Saved Setting", ex);
            }
        }
                
        private void comboBoxSavedStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            SavedSettingV4 sS = new SavedSettingV4();

            if (this.comboBoxSavedStates.SelectedItem == null)
                return;

            try
            {
                foreach (SavedSettingV4 s in this.saveObject.Settings)
                {
                    if (s.Name.Equals(this.comboBoxSavedStates.SelectedItem))
                    {
                        sS = s;
                        break;
                    }
                }
                this.setAllValues(sS);
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Selecting Saved Setting", ex);
            }
        }

        private void setAllValues(SavedSettingV4 sS)
        {
            this.ucTripMode2.SetAllValues(sS.TripSettings);
            this.ucCloseMode1.SetAllValues(sS.CloseSettings);
            this.ucPumpMode1.SetAllValues(sS.PumpSettings);
#if DNP
            if (sS.DNPSettings.LinkLayerConfirm == null)
            {
                sS.DNPSettings = new DNPSaveStateV4();
            }
            else
            {
                this.ucDNP.SetAllValues(sS.DNPSettings);
            }
#endif
            if (sS.SafeServiceSettings == null)
            {
                sS.SafeServiceSettings = new SafeServiceSavedState();
            }
            else
            {
                this.ucSafeService1.SetAllValues(sS.SafeServiceSettings);
            }

            this.updateCTRatioDomain(this.CTRatio, this.comboBox_CTRatio.SelectedIndex);
            this.updateCTRatio(sS.CTRatio);

            this.comboBox_Phasings.SelectedIndex = sS.Phasing;
            this.comboBox_RelayType.SelectedIndex = sS.RelayType;
            if (sS.V277Protector)
                comboBoxDNPVoltage.SelectedItem =
                    ProtectorVoltages.GetVoltage(ProtectorVoltageBits.V277);
            else if (sS.V600Protector)
                comboBoxDNPVoltage.SelectedItem =
                    ProtectorVoltages.GetVoltage(ProtectorVoltageBits.V600);
            else
                comboBoxDNPVoltage.SelectedItem =
                    ProtectorVoltages.GetVoltage(new ProtectorVoltageBits());

        }

        private bool sendAll = false;

        // In buttonSendAll_Click: wrap in try/finally and only restore UI state here
        private void buttonSendAll_Click(object sender, EventArgs e)
        {
            _phasingWarningShownThisApplyAll = false;
            _paramsLoadedShownThisApplyAll = false;

            var dr = MessageBox.Show(
                "The relay is updating its critical parameters ",
                "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1);

            if (dr != DialogResult.OK)
                return;

            this.enableAll(false);
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                this.sendAllParameters();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Apply All failed");
                this.messageHandler("Apply All Failed", ex);

                // only on immediate failure, restore UI here
                this.sendAll = false;
                sendAllF.SendAllFlag = false;
                this.Enabled = true;
                Application.UseWaitCursor = false;
                this.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                this.enableAll(true);
                this.tabControlMain.Enabled = true;
            }
        }

        private bool WaitForAckIdle(int timeoutMs, string step, int stableIdleMs = 75)
        {
            var total = System.Diagnostics.Stopwatch.StartNew();
            var idle = new System.Diagnostics.Stopwatch();

            while (total.ElapsedMilliseconds < timeoutMs)
            {
                if (!this.expectingAck)
                {
                    if (!idle.IsRunning)
                        idle.Start();

                    if (idle.ElapsedMilliseconds >= stableIdleMs)
                    {
                        logger.Info($"WaitForAckIdle OK step={step}, elapsedMs={total.ElapsedMilliseconds}, stableIdleMs={stableIdleMs}");
                        return true;
                    }
                }
                else
                {
                    // ACK became busy again; reset idle stability timer
                    if (idle.IsRunning)
                        idle.Reset();
                }

                Application.DoEvents(); // WinForms pump
                Thread.Sleep(10);
            }

            logger.Info($"WaitForAckIdle TIMEOUT step={step}, expectingAck={this.expectingAck}, elapsedMs={total.ElapsedMilliseconds}, stableIdleMs={stableIdleMs}, idleMs={idle.ElapsedMilliseconds}");
            return false;
        }

        private void sendAllParameters()
        {
            sendAllF.SendAllFlag = true;
            this.sendAll = true;

            try
            {
                this.ucTripMode2.SendTripMode(requestAllAfterWrite: false);
                WaitForAckIdle(3000, "After TripMode");

                this.ucCloseMode1.sendCloseData(requestAllAfterWrite: false);
                WaitForAckIdle(3000, "After CloseMode");

#if CONED
        this.SendPCData(requestAllAfterWrite: false);
        WaitForAckIdle(3000, "After PCData");
#endif

                this.SendCTRatioAndPhasing(requestAfter: false);
                WaitForAckIdle(3000, "After CTRatioAndPhasing");

                this.ucPumpMode1.SendPumpMode(requestAllAfterWrite: false);
                WaitForAckIdle(3000, "After PumpMode");

#if DNP
                if (this.Customer == Customers.TORONTO_HYDRO || dnpUplinkK.dnpEnabledWithKit)
                    this.ucTransmitter1.ForceDNPEnable = true;
#endif

                if (this.relayCodeRevisionNumber >= 20130111)
                {
                    this.ucSafeService1.SendSafeService(requestAllAfterWrite: true);
                    WaitForAckIdle(3000, "After SafeService");
                }
            }
            finally
            {
                this.sendAll = false;
                sendAllF.SendAllFlag = false;
            }

            if (!this.loadingNewCode)
            {
                BeginFullParameterDownload(nameof(sendAllParameters));
                this.requestAllData("sendAllParameters");
            }
        }

        public void SendDefaultsToMaster()
        {
            this.restoreDefaultsTypeAndPhasing();
            this.ucSafeService1.SetDefaults();
            this.sendAllParameters();
        }

        private bool readyToGetCycleData = true;

        private void requestLiveData()
        {
            byte[] sendPacket = new byte[3];

            if (!this.readyToGetCycleData)
                return;

            this.ucLiveData1.ClearAllGraphs();
            this.monitoring(false);
            this.RegisterPolling(false);

            sendPacket[0] = 0x67;
            sendPacket[1] = 0x55;
            sendPacket[2] = 0x0D;
            this.sendPacket(sendPacket);

            this.RegisterPolling(true);

            this.downloadingLiveData = true;

            this.downloadingDialogCountDown("Downloading", "Downloading Live Data",
                tCPConnection ? 420 : 210, false);
        }

        private bool dnpBaudInitialized = false;

        private void ApplyCustomerDnpBaudOnce()
        {
            if (dnpBaudInitialized) return;
            if (!this.DNPEnabled) return;
            if (!DnpCustomerPolicy.IsDnpCommCustomer(this.Customer)) return;

            int dnpBaudIndex = DnpCustomerPolicy.Uses9600DefaultBaud(this.Customer) ? 3 : 4;
            this.ucDNP.SetDnpBaudIndex(dnpBaudIndex);
            dnpBaudInitialized = true;
        }

        private ProgressBarForm downloadProgress; 
        
        private void downloadingDialogCountDown(string title, string label, int halfSecondCounts, bool dialog)
        {
            this.enableAll(false);

            if (this.Customer == Customers.CONED) //baud rate half speed
                halfSecondCounts <<= 1;

            this.downloadProgress = new ProgressBarForm(title, label, halfSecondCounts, dialog);
            this.downloadProgress.Done += new ProgressBarForm.ProgressBarEvent(downloadProgress_Done);
            if (!dialog)
            {
                this.downloadProgress.Show();
            }
            else
                this.downloadProgress.ShowDialog();
        }

        private void downloadingDialogCountDown(string formText, string title, int halfSecondCounts)
        {
            this.enableAll(false);

            if (this.Customer == Customers.CONED) //baud rate half speed
                halfSecondCounts <<= 1;
            this.downloadProgress = new ProgressBarForm(formText, title, halfSecondCounts);
            this.downloadProgress.Done += new ProgressBarForm.ProgressBarEvent(downloadProgress_Done);
            this.downloadProgress.Show();
        }

        void downloadProgress_Done(ProgressFormCompleteStates b, string s)
        {
            //True Means it TimedOut
            string temp = "null";

            if (downloadEventsClicked != true)
                temp = this.downloadProgress.Text;
            else if (ucEventGraph0.Type != EventTypes.NoEvent)
            {
                temp = this.downloadProgress.Text;
            }

            this.downloadingCanceled = true;

            this.requestLiveDataToolStripMenuItem1.Enabled = true;
            this.buttonRQEventData.Enabled = true;
            this.buttonReqLiveData.Enabled = true;

            if (temp != "null")
                this.downloadProgress.Dispose();
            this.timerLiveEventAcknowledge.Enabled = false;

            switch (b)
            {
                case ProgressFormCompleteStates.AcceptableTimeOut:
                    break;
                case ProgressFormCompleteStates.Cancelled:
                    this.messageHandler(temp + " Canceled", s);
                    this.sendCancelCommand();
                    break;
                case ProgressFormCompleteStates.Failure:
                    this.messageHandler("Progress Failure", s);
                    break;
                case ProgressFormCompleteStates.Success:
                    break;
                case ProgressFormCompleteStates.TimeOut:
                    if (sendAll == false)
                    {
                        this.messageHandler(temp + " Timed Out.", new Exception("Error " + temp));
                    }
                    break;
            }
            this.sendAll = false;
            this.enableAll(true);
            this.monitoring(true);
            this.RegisterPolling(true);
            downloadEventsClicked = false;

            //=====================Remove throbber and enable everything disaplayed on the screen=====================
            Application.UseWaitCursor = false;
            System.Windows.Forms.Cursor.Current = Cursors.Default;
            this.enableAll(true);
            //========================================================================================================
        }

        private void sendCancelCommand()
        {
            byte[] packet = new byte[3];

            packet[0] = (byte)'o';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacket(packet);
        }

        private void acknowledgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.acknowledge();
        }

        void ucTransmitter1_CTChanged(object sender, EventArgs e)
        {
            this.updateCTRatioDomain(this.CTRatio, this.comboBox_CTRatio.SelectedIndex);
            this.updateCTRatio((int)this.ucTransmitter1.CTRatio);
        }

        private void updateCTRatio(int ratio)
        {
            // Valid range in "ratio" units (display is ratio * 5)
            // 5..12750 display => 1..2550 ratio, but your defaults imply practical min of 160.
            if (ratio <= 0)
            {
                this.messageHandler("Bad CT Ratio Value",
                    new Exception("Can't set CT Ratio to zero or negative.\r\nSending a valid value to relay. Please check."));
                ratio = 320;
                this.comboBox_CTRatio.SelectedIndex = 5; // 320 => 1600 display
            }
            else if (ratio > 2550) // 2550 * 5 = 12750
            {
                this.messageHandler("Bad CT Ratio Value",
                    new Exception("Can't set CT Ratio above 12750.\r\nPlease send a valid value to relay."));
                ratio = 320;
                this.comboBox_CTRatio.SelectedIndex = 5; // 320 => 1600 display
            }

            int ratio5 = ratio * 5;

            this.ucPhasorGraph1.CTRatio = ratio;
            this.ucTripMode2.CTRatio = ratio;
            this.ucEventGraph0.CTRatio = ratio;
            this.ucEventGraph1.CTRatio = ratio;
            this.ucEventGraph2.CTRatio = ratio;
            this.ucEventGraph3.CTRatio = ratio;
            this.ucEventGraph4.CTRatio = ratio;
            this.ucEventGraph5.CTRatio = ratio;
            this.ucEventGraph6.CTRatio = ratio;
            this.ucEventGraph7.CTRatio = ratio;
            this.ucLiveData1.CTRatio = ratio;
            this.ucTransmitter1.CTRatio = (uint)ratio;
            this.ucSafeService1.CTRatio = ratio;
            this.CTRatio = ratio;

            this.setCTRatioTransmitterPage(ratio);
            this.textBoxCTRatio.Text = ratio5.ToString();
            this.comboBox_CTRatio.Text = ratio5.ToString();
            this.textBoxCTRatioPQMonitor.Visible = true;
            this.textBoxCTRatioPQMonitor.BringToFront();
            this.textBoxCTRatioPQMonitor.Text = this.textBoxCTRatio.Text;
        }

        private bool downloadingLiveData = true;

        private void acknowledgeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.acknowledge();
        }

        private void labelLiveEventTriggerTime_Resize(object sender, EventArgs e)
        {
            this.labelLiveDataTriggerTime.Left = (this.tabPageFlightRecorder.Width / 2) - (this.labelLiveDataTriggerTime.Size.Width / 2);
        }

        private void buttonRSTRelay_Click(object sender, EventArgs e)
        {
            byte[] packet = new byte[3];

            packet[0] = (byte)'B';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacket(packet);
        }

        private int liveEventTOStored = 0;
        private int liveEventTimedOutCount
        {
            get { return this.liveEventTOStored; }
            set
            {
                this.updateLiveEventTimedOutStatus(value);
            }
        }

        private void updateLiveEventTimedOutStatus(int value)
        {
            this.liveEventTOStored = value;
        }

        private int nAckCountStored = 0;
        private int nAckCount
        {
            get { return this.nAckCountStored; }
            set
            {
                this.updateNAckCount(value);
            }
        }

        private void updateNAckCount(int value)
        {
            this.nAckCountStored = value;
        }

        public int nackCount = 0;
        private void timerLiveEventAcknowledge_Tick(object sender, EventArgs e)
        {
            this.timerLiveEventAcknowledge.Enabled = false;

            if (this.initialLiveEventRequest)
            {
                this.initialLiveEventRequest = false;
                this.downloadProgress_Done(ProgressFormCompleteStates.Failure, "No Response From Relay.\r\nPlease Check Connection.");
                return;
            }

            this.liveEventTimedOutCount++;
            if (this.downloadingCanceled)
            {
                return;
            }
            this.nackCount++;
            this.nAcknowledge();
            this.timerLiveEventAcknowledge.Start();
        }

        private void liveEvent_PacketHandled(object sender, PacketHandledEventArgs e)
        {
            this.timerLiveEventAcknowledge.Enabled = false;

            if (e.Successful)
            {
                this.acknowledge();
            }
            else
            {
                this.nAckCount++;
                this.nAcknowledge();
            }
            this.timerLiveEventAcknowledge.Start();
        }

        private void data_BackUp_reprogramingOldRelay()
        {
            //string path = @"C:\DGI Systems\Relay\Saved Data\RelayData.txt";
            string path = @"C:\DGI Systems\Relay\Saved Data\RelayData " + String.Format("{0:yyyyMMdd_HHmmss}", DateTime.Now) + ".txt";

            TextWriter tw = new StreamWriter(path, true);
            tw.WriteLine("Data currently residing in the relay :");
            tw.Close();
        }

        private void MainControl_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucTransmitter1.RestoreFastModeOnShutdown();
            this.disableAllMonitoring();
            this.pauseTransmitterMonitoring();
            this.ucShortRange1.StopMonitoring();
            this.monitoring(false);

            if (this.serialPort1 != null)
            {
                this.clearSerialPortBuffers(this.serialPort1);
                this.serialPort1.Dispose();
            }
            if (this.threadFindRelay != null)
            {
                if (this.threadFindRelay.IsAlive)
                {
                    this.threadFindRelay.Abort();
                }
                this.threadFindRelay = null;
            }
            if (this.closePort != null)
            {
                if (this.closePort.IsAlive)
                {
                    this.closePort.Abort();
                }
                this.closePort = null;
            }
            Application.Exit();
        }

        private void saveEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ucEventGraph0.Saveable || this.ucEventGraph1.Saveable ||
                this.ucEventGraph2.Saveable || this.ucEventGraph4.Saveable ||
                this.ucEventGraph4.Saveable || this.ucEventGraph5.Saveable ||
                this.ucEventGraph6.Saveable || this.ucEventGraph7.Saveable)
            {
                this.saveEventsToFile();
            }
            else
            {
                this.messageHandler("Not Saved", "Please Download Event Data Before Saving");
            }
        }

        private void saveEventsToFile()
        {
            try
            {
                SavedEventSet sES = this.getSavedEventSet();

                SaveFileDialog saveEventsDialog = new SaveFileDialog();
                saveEventsDialog.Title = "Save Event Set";
                saveEventsDialog.InitialDirectory = @"C:\DGI Systems\Relay\Saved Data";
                saveEventsDialog.Filter = "Event File |*.evt";
                saveEventsDialog.ShowDialog();

                if (saveEventsDialog.FileName != "")
                {
                    Stream stream = File.Open(saveEventsDialog.FileName, FileMode.Create);
                    BinaryFormatter bF = new BinaryFormatter();
                    bF.Serialize(stream, sES);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Saving Event Data", ex);
            }
        }

        private SavedEventSet getSavedEventSet()
        {
            SavedEventSet sES = new SavedEventSet();

            try
            {
                sES.Events[0] = this.getSingleEventData(ucEventGraph0);
                sES.Events[1] = this.getSingleEventData(ucEventGraph1);
                sES.Events[2] = this.getSingleEventData(ucEventGraph2);
                sES.Events[3] = this.getSingleEventData(ucEventGraph3);
                sES.Events[4] = this.getSingleEventData(ucEventGraph4);
                sES.Events[5] = this.getSingleEventData(ucEventGraph5);
                sES.Events[6] = this.getSingleEventData(ucEventGraph6);
                sES.Events[7] = this.getSingleEventData(ucEventGraph7);

            }
            catch (Exception ex)
            {
                this.messageHandler("Error Retrieving Event Set Data", ex);
            }

            return sES;
        }

        private SavedSingleEvent getSingleEventData(ucEventGraph uEG)
        {
            SavedSingleEvent sSE = new SavedSingleEvent();

            sSE.ID = uEG.RelayID;
            sSE.Type = uEG.Type;
            sSE.Time = uEG.EventTime;
            sSE.VtA = uEG.GetSineWave(PhasorTypes.VtA);
            sSE.VtB = uEG.GetSineWave(PhasorTypes.VtB);
            sSE.VtC = uEG.GetSineWave(PhasorTypes.VtC);
            sSE.VnA = uEG.GetSineWave(PhasorTypes.VnA);
            sSE.VnB = uEG.GetSineWave(PhasorTypes.VnB);
            sSE.VnC = uEG.GetSineWave(PhasorTypes.VnC);
            sSE.IA = uEG.GetSineWave(PhasorTypes.IA);
            sSE.IB = uEG.GetSineWave(PhasorTypes.IB);
            sSE.IC = uEG.GetSineWave(PhasorTypes.IC);

            return sSE;
        }

        private void loadEventSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavedEventSet sES;
            OpenFileDialog oFD = new OpenFileDialog();
            BinaryFormatter bF;

            try
            {
                oFD.CheckFileExists = true;
                oFD.CheckPathExists = true;
                oFD.InitialDirectory = @"C:\DGI Systems\Relay\Saved Data";
                oFD.Title = "Open Event Saved File.";
                oFD.DefaultExt = ".evt";
                oFD.Multiselect = false;
                oFD.Filter = "Event Files (.evt)|*.evt";
                oFD.ShowDialog();

                if (oFD.FileName.Contains(".evt"))
                {
                    using (Stream stream = File.Open(oFD.FileName, FileMode.Open))
                    {
                        bF = new BinaryFormatter();
                        sES = (SavedEventSet)bF.Deserialize(stream);
                    }
                    this.storeEventSetData(sES);
                    this.tabControlMain.SelectedTab = this.tabPageEvents;
                }
                else
                {
                    this.messageHandler("Bad File Extension", oFD.FileName + " is not a valid File Name.");
                    return;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Attempting to Load Event Set Data", ex);
            }
        }

        private void storeEventSetData(SavedEventSet sES)
        {
            try
            {
                this.storeSingleEventData(sES.Events[0], this.ucEventGraph0);
                this.storeSingleEventData(sES.Events[1], this.ucEventGraph1);
                this.storeSingleEventData(sES.Events[2], this.ucEventGraph2);
                this.storeSingleEventData(sES.Events[3], this.ucEventGraph3);
                this.storeSingleEventData(sES.Events[4], this.ucEventGraph4);
                this.storeSingleEventData(sES.Events[5], this.ucEventGraph5);
                this.storeSingleEventData(sES.Events[6], this.ucEventGraph6);
                this.storeSingleEventData(sES.Events[7], this.ucEventGraph7);
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Storing Event Set Data", ex);
            }
        }

        private void storeSingleEventData(SavedSingleEvent sSE, ucEventGraph uEG)
        {
            try
            {
                uEG.RelayID = sSE.ID;
                uEG.EventTime = sSE.Time;
                uEG.Type = sSE.Type;
                uEG.SetSineWave(PhasorTypes.IA, sSE.IA);
                uEG.SetSineWave(PhasorTypes.IB, sSE.IB);
                uEG.SetSineWave(PhasorTypes.IC, sSE.IC);
                uEG.SetSineWave(PhasorTypes.VtA, sSE.VtA);
                uEG.SetSineWave(PhasorTypes.VtB, sSE.VtB);
                uEG.SetSineWave(PhasorTypes.VtC, sSE.VtC);
                uEG.SetSineWave(PhasorTypes.VnA, sSE.VnA);
                uEG.SetSineWave(PhasorTypes.VnB, sSE.VnB);
                uEG.SetSineWave(PhasorTypes.VnC, sSE.VnC);
                uEG.Saveable = true;
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Storing Single Event Data", ex);
            }
        }



        private void saveLiveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.ucLiveData1.Saveable)
                {
                    this.messageHandler("Can't Save", "Please Download Data Before Trying to Save");
                    return;
                }

                SavedSingleEvent sSE = this.getLiveDataForSave();
                SaveFileDialog saveEventsDialog = new SaveFileDialog();
                saveEventsDialog.Title = "Save Live Data";
                saveEventsDialog.InitialDirectory = @"C:\DGI Systems\Relay\Saved Data";
                saveEventsDialog.Filter = "Live Data File |*.ldf";
                saveEventsDialog.ShowDialog();

                if (saveEventsDialog.FileName != "")
                {
                    using (Stream stream = File.Open(saveEventsDialog.FileName, FileMode.Create))
                    {
                        BinaryFormatter bF = new BinaryFormatter();
                        bF.Serialize(stream, sSE);
                    }
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Saving Event Data", ex);
            }
        }

        private SavedSingleEvent getLiveDataForSave()
        {
            SavedSingleEvent sSE = new SavedSingleEvent();

            sSE.Time = this.liveDataTriggerTime;
            sSE.ID = this.ucTransmitter1.TXSettings.ID;
            sSE.IA = this.ucLiveData1.GetSineWave(PhasorTypes.IA);
            sSE.IB = this.ucLiveData1.GetSineWave(PhasorTypes.IB);
            sSE.IC = this.ucLiveData1.GetSineWave(PhasorTypes.IC);
            sSE.VtA = this.ucLiveData1.GetSineWave(PhasorTypes.VtA);
            sSE.VtB = this.ucLiveData1.GetSineWave(PhasorTypes.VtB);
            sSE.VtC = this.ucLiveData1.GetSineWave(PhasorTypes.VtC);
            sSE.VnA = this.ucLiveData1.GetSineWave(PhasorTypes.VnA);
            sSE.VnB = this.ucLiveData1.GetSineWave(PhasorTypes.VnB);
            sSE.VnC = this.ucLiveData1.GetSineWave(PhasorTypes.VnC);

            return sSE;
        }

        private void loadLiveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavedSingleEvent sSE;
            OpenFileDialog oFD = new OpenFileDialog();
            BinaryFormatter bF;

            try
            {
                oFD.CheckFileExists = true;
                oFD.CheckPathExists = true;
                oFD.InitialDirectory = @"C:\DGI Systems\Relay\Saved Data\";
                oFD.Title = "Open Event Saved File.";
                oFD.DefaultExt = ".ldf";
                oFD.Filter = "Live Data Files (.ldf)|*.ldf";
                DialogResult dR = oFD.ShowDialog();

                if (dR != DialogResult.OK)
                {
                    oFD.Dispose();
                    return;
                }
                if (oFD.FileName.Contains(".ldf"))
                {
                    using (Stream stream = File.Open(oFD.FileName, FileMode.Open))
                    {
                        bF = new BinaryFormatter();

                        sSE = (SavedSingleEvent)bF.Deserialize(stream);
                    }
                    this.storeLiveData(sSE);

                    this.tabControlMain.SelectedTab = this.tabPageFlightRecorder;
                }
                else
                {
                    this.messageHandler("Bad File Extension", oFD.FileName + " is not a valid File Name.");
                    return;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Attempting to Load Event Set Data", ex);
            }
        }

        private void storeLiveData(SavedSingleEvent sSE)
        {
            this.ucLiveData1.ClearAllGraphs();

            this.labelLiveDataTriggerTime.Text = "ID Number : " + sSE.ID + " Triggered - " + sSE.Time.ToString();
            this.ucLiveData1.ID = sSE.ID;
            this.ucLiveData1.SetSineWave(PhasorTypes.IA, sSE.IA);
            this.ucLiveData1.SetSineWave(PhasorTypes.IB, sSE.IB);
            this.ucLiveData1.SetSineWave(PhasorTypes.IC, sSE.IC);
            this.ucLiveData1.SetSineWave(PhasorTypes.VtA, sSE.VtA);
            this.ucLiveData1.SetSineWave(PhasorTypes.VtB, sSE.VtB);
            this.ucLiveData1.SetSineWave(PhasorTypes.VtC, sSE.VtC);
            this.ucLiveData1.SetSineWave(PhasorTypes.VnA, sSE.VnA);
            this.ucLiveData1.SetSineWave(PhasorTypes.VnB, sSE.VnB);
            this.ucLiveData1.SetSineWave(PhasorTypes.VnC, sSE.VnC);
            this.ucLiveData1.Saveable = true;
        }

        private void buttonSendTime_Click(object sender, EventArgs e)
        {
            this.sendTime(DateTime.UtcNow);
        }

        private void buttonCauseEvent_Click(object sender, EventArgs e)
        {
            byte[] sendPacket = new byte[2];

            sendPacket[0] = 0x01;
            sendPacket[1] = 0x0D;

            this.sendPacket(sendPacket);
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ucRelayProgramming1.ReprogrammingInProgress)
                return;

            if (this.tabControlMain.SelectedTab == this.tabPageDNPSecureAuth)
            {
                //ucDNPSAv51.ShowDNPSAV5Error = true;
                //this.ucDNPSAv51.RequestAllData();
            }
            if (this.tabControlMain.SelectedTab != this.tabPageArcFault)
            {
                this.arcFaultEnableMonitoring(false);
            }
            else
            {
                this.arcFaultEnableMonitoring(true);
            }
            if (this.tabControlMain.SelectedTab == this.tabPageFlightRecorder)
            {
                this.eventActionsToolStripMenuItem.Enabled = false;
                this.liveDataActionsToolStripMenuItem.Enabled = true;
                this.requestLiveDataToolStripMenuItem1.Enabled = true;
            }
            else if (this.tabControlMain.SelectedTab == this.tabPageEvents)
            {
                this.eventActionsToolStripMenuItem.Enabled = true;
                this.liveDataActionsToolStripMenuItem.Enabled = false;
                this.radioButtonEvent0.Select();
            }
            else if (this.tabControlMain.SelectedTab == this.tabPageMonitor)
            {
                if (this.timerRegisterPolling.Enabled)
                {
                    this.eventActionsToolStripMenuItem.Enabled = false;
                    this.liveDataActionsToolStripMenuItem.Enabled = false;
                    // Do not do this if everything is disabled
                    if (this.allEnabled)
                    {
                        if (!this.phasorGraphTabSwitchCall)
                        {
                            this.requestPhasorData();
                            this.everyOtherMonitor = false;
                            this.pQMonitoringEnabled = true;
                            this.ucPhasorGraph1.RealTimeMonitoring = true;
                            this.buttonToggleMonitor.Text = "Stop Monitoring";
                        }
                    }
                }
            }
            else if (this.tabControlMain.SelectedTab == this.tabPageTransmitterMonitoring || this.tabControlMain.SelectedTab == this.tabPageTransmitter)
            {
                this.eventActionsToolStripMenuItem.Enabled = false;
                this.liveDataActionsToolStripMenuItem.Enabled = false;
                if (this.allEnabled)
                {
                    this.RegisterPolling(true);
                    this.transmitterMonitoring = true;
                    this.ucTransmitterMonitoring1.TransmitterMonitoring = true;
                    this.requestTransmitterMonitorData();
                }

                this.requestPhasorData();
                this.everyOtherMonitor = false;
            }
            else if (this.tabControlMain.SelectedTab == this.tabPageControl)
            {
                this.transmitterMonitoring = false;
            }
            else if (this.tabControlMain.SelectedTab == this.tabPage1)  // Relay Monitoring tab
            {
                // read lightning count by default at start up
                //this.btn_getLC_Click(this, new EventArgs());
            }
            else
            {
                this.eventActionsToolStripMenuItem.Enabled = false;
                this.liveDataActionsToolStripMenuItem.Enabled = false;
            }


            if (this.tabControlMain.SelectedTab != this.tabPageMonitor && this.tabControlMain.SelectedTab != this.tabPageTransmitter
                & this.tabControlMain.SelectedTab != this.tabPageControl)
            {
                this.disableAllMonitoring();
            }
            if (this.tabControlMain.SelectedTab != this.tabPageTransmitterMonitoring && this.tabControlMain.SelectedTab != this.tabPageTransmitter
                && this.tabControlMain.SelectedTab != this.tabPageControl)
            {
                this.pauseTransmitterMonitoring();
            }

            if (this.tabControlMain.SelectedTab != this.tabPageShortRange)
            {
                this.ucShortRange1.DisableMonitoring();
            }

            if (this.relayFound_forDNPdataMonitoring)
                this.enableDNPMonitoring(this.tabControlMain.SelectedTab == this.tabPageDNPData);
            else
            {
                //string text = "Relay not found. Please check for its Power and then Request for the DNP data ";
                string text = "Relay not found. Please check for its Power and Connection.";
                MessageBox.Show(text);
                this.enableDNPMonitoring(false);
            }
            this.phasorGraphTabSwitchCall = false; //deset so next time it does not think it was called from the phasorGraph
            if (this.tabControlMain.SelectedTab == this.tabPageDNPData)
            {
                this.enableDNPMonitoring(true); // or existing flow
            }
        }

        private void buttonFPGAProcVersion_Click(object sender, EventArgs e)
        {
            this.requestFPGARevision();
        }

        private void timerResponseTimeOut_Tick(object sender, EventArgs e)
        {
            this.timerResponseTimeOut.Enabled = false;
            if (this.requestedAllParameters && !this.loadingNewCode)
            {
                this.messageHandler("Response Time Out", "Please Check Connection");

                Application.UseWaitCursor = false;
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                this.enableAll(true);
            }
        }

        private bool transmitterMonitoring
        {
            get => tempTM;
            set
            {
                tempTM = value;
            }
        }
        private bool tempTM = false;


        private void buttonStartMonitoring_Click(object sender, EventArgs e)
        {
            this.startTransmitterMonitoring();
        }

        private void buttonPauseMonitoring_Click(object sender, EventArgs e)
        {
            this.pauseTransmitterMonitoring();
        }

        private void startTransmitterMonitoring()
        {
            this.RegisterPolling(true);
            this.transmitterMonitoring = true;
            this.ucTransmitterMonitoring1.TransmitterMonitoring = true;
            this.requestTransmitterMonitorData();
        }

        private void pauseTransmitterMonitoring()
        {
            this.transmitterMonitoring = false;
            this.ucTransmitterMonitoring1.TransmitterMonitoring = false;
        }



        private float RMS(long f, long g)
        {
            double tempF, tempG;

            if (f >= 0x80000000)
            {
                f = 0x80000000 - f;  //get the difference between the two values
                f = 0x80000000 + f;  //Add the difference to 0x8000 to get the converted value
                f = 0 - f;       //make it negative.
            }
            if (g >= 0x80000000)
            {
                g = 0x80000000 - g;  //get the difference between the two values
                g = 0x80000000 + g;  //Add the difference to 0x8000 to get the converted value
                g = 0 - g;       //make it negative.
            }
            tempF = (double)f;
            tempG = (double)g;

            tempF = tempF * (double)Constants.TwelveFracBits;
            tempG = tempG * (double)Constants.TwelveFracBits;

            tempF = Math.Pow(tempF, 2) + Math.Pow(tempG, 2);
            tempF = Math.Sqrt(tempF);

            return (float)tempF;
        }

        void setCTRatioTransmitterPage(int ratio)
        {
            this.ucTransmitterMonitoring1.CTRatio = ratio;
        }

        private void listBoxCTSizeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lB = (ListBox)sender;
            int tempCTRatio = 0;

            switch (lB.SelectedIndex)
            {
                case 0:
                    tempCTRatio = 750;      //3750
                    break;
                case 1:
                    tempCTRatio = 700;      //3500
                    break;
                case 2:
                    tempCTRatio = 600;      //3000
                    break;
                case 3:
                    tempCTRatio = 500;      //2500
                    break;
                case 4:
                    tempCTRatio = 400;      //2000
                    break;
                case 5:
                    tempCTRatio = 320;      //1600
                    break;
                case 6:
                    tempCTRatio = 240;      //1200
                    break;
                case 7:
                    tempCTRatio = 160;      //800
                    break;
            }
            this.updateCTRatio(tempCTRatio);
        }

        private void requestTransmitterMonitorData()
        {
            byte[] sendPacket = new byte[3];

            sendPacket[0] = (byte)'q';
            sendPacket[1] = 0x55;
            sendPacket[2] = 0x0D;

            this.sendPacket(sendPacket);
        }

        private void setMonitoringPageFrequency(Frequencies freq)
        {
            this.ucTransmitterMonitoring1.Frequency = freq;
        }

        private void setTransmitterMonitorData(byte[] bytePacket)
        {
            this.ucTransmitterMonitoring1.SetAll(bytePacket);
            this.ucTransmitter1.setMonitoringData(bytePacket);
        }

        private void setWBdataMain(byte[] bytePacket)
        {
            if ((bytePacket[15] & 0x08) == 0x08)
                this.textBoxLRLockoutStatusMain.Text = "Locked!";
            else
                this.textBoxLRLockoutStatusMain.Text = "Not Locked";
        }

        private bool setDNPSettings(byte[] bytePacket)
        {
            bool wroteBackupSection = false;

            if (dataBackup_fromRelay == true)
            {
                byte[] snapshot = new byte[Math.Min(bytePacket.Length, 98)];
                Array.Copy(bytePacket, snapshot, snapshot.Length);
                CaptureBackupSection("DNP Data", snapshot);
                wroteBackupSection = true;
            }

            try
            {
                if (this.ProgramState == ProgramStates.DownloadingAllParameters)
                {
                    this.parametersFinishedLoading();
                }

                bool isDnpSupported = IsDnpCommSupported();

                if (!isDnpSupported)
                {
                    logger.Warn("Ignoring DNP settings packet because DNP comm is not supported for current customer/revision.");
                    UpdateDnpCommStatusFromRelayState(false);
                }
                else
                {
                    this.ucTransmitter1.SetAll(bytePacket);
                    this.ucDNP.SetAll(bytePacket);

                    UpdateDnpCommStatusFromRelayState(true);
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting DNPData", ex);
            }

            return wroteBackupSection;
        }

        #region Screen Save & Print
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]

        private static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest,
          int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private Bitmap memoryImage;
        private void CaptureScreen()
        {
            try
            {
                Graphics mygraphics = this.CreateGraphics();
                Size s = this.ClientSize;
                memoryImage = new Bitmap(s.Width, s.Height, mygraphics);

                Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                IntPtr dc1 = mygraphics.GetHdc();
                IntPtr dc2 = memoryGraphics.GetHdc();
                BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
                mygraphics.ReleaseHdc(dc1);
                memoryGraphics.ReleaseHdc(dc2);
            }
            catch (Exception ex)
            {
                this.messageHandler("Error in CaptureScreen", ex);
            }
        }

        private void mnuFileSaveScreen_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JPEG Image|*.jpg" + "|Bitmap Image|*.bmp" +
                  "|Gif Image|*.gif" + "|Windows Image|*.emf" + "|PNG Image|*.png" + "|Tiff Image|*.tiff";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.InitialDirectory = @"C:\DGI Systems\Relay\Saved Data";
                saveFileDialog.Title = "Save Screen";
                this.saveFileDialogVisible = true;
                if ((saveFileDialog.ShowDialog() == DialogResult.OK) && (saveFileDialog.FileName != ""))
                {
                    ImageFormat imgFormat;
                    switch (saveFileDialog.FilterIndex)
                    {
                        case 1:
                            imgFormat = ImageFormat.Jpeg;
                            break;
                        case 2:
                            imgFormat = ImageFormat.Bmp;
                            break;
                        case 3:
                            imgFormat = ImageFormat.Gif;
                            break;
                        case 4:
                            imgFormat = ImageFormat.Emf;
                            break;
                        case 5:
                            imgFormat = ImageFormat.Png;
                            break;
                        case 6:
                            imgFormat = ImageFormat.Tiff;
                            break;
                        default:
                            imgFormat = ImageFormat.Png;
                            break;
                    }
                    //TimerControl myTimer = new TimerControl();
                    // wait for the dialog box to disappear, otherwise, the dialog box will also be included in.

                    this.timerScreenCapDelay.Enabled = true;
                    while (this.saveFileDialogVisible)
                    {
                        Application.DoEvents();
                    }
                    this.CaptureScreen();
                    memoryImage.Save(saveFileDialog.FileName, imgFormat);
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error In FileSaveScreen_Click", ex);
            }
        }

        private bool saveFileDialogVisible = false;

        private void mnuFilePrintScreen_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDocument pdocScreen = new PrintDocument();
                pdocScreen.PrintPage += new PrintPageEventHandler(this.pdocScreen_PrintPage);

                PrintDialog dialog = new PrintDialog();
                dialog.AllowPrintToFile = false;
                dialog.Document = pdocScreen;
                pdocScreen.DefaultPageSettings.Landscape = true;
                pdocScreen.DefaultPageSettings.Margins = new Margins(200, 200, 200, 200);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // wait for the dialog box to disappear, otherwise, the dialog box will also be included in.
                    this.timerScreenCapDelay.Enabled = true;
                    while (this.saveFileDialogVisible)
                    {
                        Application.DoEvents();
                    }
                    this.CaptureScreen();
                    pdocScreen.Print();
                }
                pdocScreen = null;
            }
            catch (Exception ex)
            {
                this.messageHandler("Error in FilePrintScreen_Click", ex);
            }

        }

        // Specifies what happens when the PrintPage event is raised.
        private void pdocScreen_PrintPage(object sender, PrintPageEventArgs ev)
        {
            // Draw a picture.
            ev.Graphics.DrawImage(this.memoryImage, ev.Graphics.VisibleClipBounds);
            // Indicate that this is the last page to print.
            ev.HasMorePages = false;
        }

        private void timerScreenCapDelay_Tick(object sender, EventArgs e)
        {
            this.saveFileDialogVisible = false;
            this.timerScreenCapDelay.Enabled = false;
        }
        #endregion

        private void buttonQuietMode_Click(object sender, EventArgs e)
        {
            if (this.quietMode)
            {
                this.quietMode = false;
                this.labelQuietMode.Visible = false;
                this.requestRelayRegisters();
            }
            else
            {
                this.quietMode = true;
                this.labelQuietMode.Visible = true;
            }
        }

        private void labelQuietMode_MouseDown(object sender, MouseEventArgs e)
        {
            this.requestRelayRegisters();
        }

        private ProtectorVoltage protectorVoltage =
            ProtectorVoltages.GetVoltage();

        private void buttonClearEvents_Click(object sender, EventArgs e)
        {
            this.clearEvents();
        }

        private void clearEvents()
        {
            DialogResult dr = this.messageHandler("Clear Events", "Clearing the events will take some time and the relay will temporarily be unresponsive to commands.\r\nDo you really want to clear all the events?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                byte[] bytePacket = new byte[3];

                bytePacket[0] = 0x14;
                bytePacket[1] = 0x55;
                bytePacket[2] = 0x0D;

                this.sendPacket(bytePacket);

                this.ucEventGraph0.Type = EventTypes.NoEvent;
                this.ucEventGraph1.Type = EventTypes.NoEvent;
                this.ucEventGraph2.Type = EventTypes.NoEvent;
                this.ucEventGraph3.Type = EventTypes.NoEvent;
                this.ucEventGraph4.Type = EventTypes.NoEvent;
                this.ucEventGraph5.Type = EventTypes.NoEvent;
                this.ucEventGraph6.Type = EventTypes.NoEvent;
                this.ucEventGraph7.Type = EventTypes.NoEvent;

                this.ucEventGraph0.ClearAllGraphs();
                this.ucEventGraph1.ClearAllGraphs();
                this.ucEventGraph2.ClearAllGraphs();
                this.ucEventGraph3.ClearAllGraphs();
                this.ucEventGraph4.ClearAllGraphs();
                this.ucEventGraph5.ClearAllGraphs();
                this.ucEventGraph6.ClearAllGraphs();
                this.ucEventGraph7.ClearAllGraphs();

                this.downloadingDialogCountDown("Erasing", "Erasing Event Data ", 20, true);
            }
        }

        private void clearEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.clearEvents();
        }



        #region ArcFault

        private bool arcFaultEnabled = false;
        private bool ArcFaultEnabled
        {
            get { return this.arcFaultEnabled; }
            set
            {
                this.arcFaultEnabled = value;
                if (value)
                {
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageArcFault))
                    {
                        this.tabControlMain.TabPages.Add(this.tabPageArcFault);
                    }
                }
                else
                {
                    if (this.tabControlMain.TabPages.Contains(this.tabPageArcFault))
                    {
                        this.tabControlMain.TabPages.Remove(this.tabPageArcFault);
                    }
                }
            }
        }

        private void setArcFaultData(byte[] bytePacket)
        {
            this.ucArcFault1.SetAll(bytePacket);
        }

        private void arcFault_requestArcFaultMonitoring()
        {
            byte[] sendPacket = new byte[42];

            sendPacket[0] = (byte)'E';
            sendPacket[1] = 2;          // 2 is the command for monitoring data
            sendPacket[2] = 0;
            sendPacket[41] = 0x0D;

            this.sendPacket(sendPacket);
        }

        private bool arcFaultMonitoringEnabled = false;

        private void buttonArcFaultStartMonitoring_Click(object sender, EventArgs e)
        {
            this.arcFaultEnableMonitoring(!this.arcFaultMonitoringEnabled);
        }

        private void arcFaultEnableMonitoring(bool e)
        {
            if (e)
            {
                this.arcFaultMonitoringEnabled = true;
                this.buttonArcFaultStartMonitoring.Text = "Stop Monitoring";
            }
            else
            {
                this.arcFaultMonitoringEnabled = false;
                this.buttonArcFaultStartMonitoring.Text = "Start Monitoring";
            }
        }
        #endregion
        /*
        private void cTRatioCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CTRatioCaculator CTCalculator = new CTRatioCaculator();

            CTCalculator.ShowDialog(this);
        }
        */
        private void buttonTimeConvert_Click(object sender, EventArgs e)
        {
            EventBaseTime eBT = new EventBaseTime();

            try
            {
                uint value = Convert.ToUInt32(this.textBoxTimeInput.Text);

                eBT.BinaryTime = value;
                this.textBoxTimeOutput.Text = eBT.SystemTime.ToString();

                this.textBoxTimeOutput.Text = eBT.ToString();
            }
            catch
            {
                MessageBox.Show("Bad Time Input Value");
            }
        }

        private bool requestingDNPData = false;

        private void buttonRequestDNPData_Click(object sender, EventArgs e)
        {
            if (this.relayFound_forDNPdataMonitoring == true)
            {
                this.enableDNPMonitoring(!this.requestingDNPData);
            }
            else
            {
                this.enableDNPMonitoring(false);
                this.relayFound_forDNPdataMonitoring = false;
                string text = "Relay not found. Please check for its Power and then Request for the DNP data ";
                MessageBox.Show(text);
            }
        }

        private void DNPDigitalGridData_PointChanged(object o, DNPPointEventArgs eA)
        {
            this.enableDNPMonitoring(false);
        }

        private void enableDNPMonitoring(bool val)
        {
            // OFF state is default. Only poll when explicitly started.
            if (val)
            {
                this.buttonRequestDNPData.Text = "Stop Requesting Data";
                this.buttonRequestDNPData.BackColor = Color.Green;
            }
            else
            {
                this.buttonRequestDNPData.Text = "Start Requesting Data"; // changed
                this.buttonRequestDNPData.BackColor = Color.Red;
            }

            this.requestingDNPData = val;
        }

        private void requestDNPData()
        {
          //  MessageBox.Show("sending commad to get DNP libe data from master processor"); // Only for testing - to be removed
            byte[] sendPacket = new byte[3];

            sendPacket[0] = 0x05;
            sendPacket[1] = 0x55;
            sendPacket[2] = 0x0D;

            this.sendPacket(sendPacket);
        }

        private void reprogramRelayFileSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManualUpdate.usingManualMode = true;
            this.ResetAutoloadDeclineState();
            this.ucRelayProgramming1.StartManualForcedUpdate();
        }


        private void enableAutoloadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dataBackupR.dataBackup_fromRelay == false)
                this.editConfigFile();
        }

        private void editConfigFile()
        {
            ProgramConfig pC = new ProgramConfig();

            pC.Data.AutoLoadEnabled = this.enableAutoloadToolStripMenuItem.Checked;

            pC.SaveConfigFile();
        }

        private void initializeFromConfigFile()
        {
            ProgramConfig pC = new ProgramConfig();

            this.enableAutoloadToolStripMenuItem.Checked = pC.Data.AutoLoadEnabled;
        }

        //private void domainUpDownRelayType_SelectedItemChanged(object sender, EventArgs e)
        private void comboBox_RelayType_SelectedItemChanged(object sender, EventArgs e)
        {
            if (this.comboBox_RelayType.SelectedIndex == 0)
            {
                labelConEdPowerRelay.Text = "Power";
                this.ucTripMode2.SequenceRelay = false;
            }
            else
            {
                labelConEdPowerRelay.Text = "Sequence";
                this.ucTripMode2.SequenceRelay = true;
            }
        }


        private void comboBoxSavedStates_DropDown(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;
            int width = senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;

            int newWidth;
            foreach (string s in ((ComboBox)sender).Items)
            {
                newWidth = (int)g.MeasureString(s, font).Width
                    + vertScrollBarWidth;
                if (width < newWidth)
                {
                    width = newWidth;
                }
            }
            senderComboBox.DropDownWidth = width;

        }

        private void comboBoxSavedStates_DropDownClosed(object sender, EventArgs e)
        {
            this.comboBoxSavedStates.Width = this.savedSaveFileComboBoxWidth;
        }

        private void buttonSendLowVoltageThres_Click(object sender, EventArgs e)
        {
            logger.Info("buttonSendLowVoltageThres_Click ENTER");

            try
            {
                SetBusyUi(true);

                if (numericUpDownLowVoltageThres.Value > numericUpDownLowVoltageThres.Maximum ||
                    numericUpDownLowVoltageThres.Value < numericUpDownLowVoltageThres.Minimum)
                {
                    MessageBox.Show(
                        $"Low Voltage Threshold Value must be between {numericUpDownLowVoltageThres.Minimum} and {numericUpDownLowVoltageThres.Maximum}");
                    return;
                }

                var sEA = new SendEventArgs(4);
                sEA.SendPacket[0] = Convert.ToByte('&');
                sEA.SendPacket[1] = 0x55;
                sEA.SendPacket[2] = Convert.ToByte(numericUpDownLowVoltageThres.Value);
                sEA.SendPacket[3] = 0x0D;

                // keep behavior explicit; low-voltage write does NOT require full requestAll
                sEA.WithAck = false;
                sEA.RequestAll = false;

                // guard: MainControl must not send relay commands while the relay programming lifecycle is active
                if (IsRelayProgrammingLifecycleActive())
                {
                    logger.Warn("MainControl suppressed relay send while ucRelayProgramming owns active lifecycle.");
                    return;
                }

                this.standardizedSendData(this, sEA);
                logger.Info("LowVoltage send dispatched");

                if (!this.sendAll)
                {
                    // targeted refresh only
                    this.requestLowVoltageThreshold();
                    logger.Info("requestLowVoltageThreshold dispatched");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error sending Low Voltage Threshold Value");
                MessageBox.Show("Error sending Low Voltage Threshold Value");
            }
            finally
            {
                SetBusyUi(false);
                logger.Info("buttonSendLowVoltageThres_Click EXIT");
            }
        }

        void SetLowVoltageThres(byte[] bytePacket)
        {
            try
            {
                numericUpDownLowVoltageThres.Value = bytePacket[1];
            }
            catch
            {
                MessageBox.Show("Bad value in requested Low Voltage Threshold");
            }
        }
        private void buttonRequestLowVotlageThres_Click(object sender, EventArgs e)
        {
            this.requestLowVoltageThreshold();
        }

        private void requestLowVoltageThreshold()
        {
            try
            {
                byte[] sendArray = new byte[3];
                sendArray[0] = (byte)'?';
                sendArray[1] = 0x55;
                sendArray[2] = 0x0D;

                this.sendPacket(sendArray);
            }
            catch
            {
                MessageBox.Show("Error Requesting Low Voltage Threshold Value");
            }
        }
        /*
        private void buttonRequestLowVotlageThres_Click(object sender, EventArgs e)
        {
            SendEventArgs sEA = new SendEventArgs(3);

            try
            {
                sEA.SendPacket[0] = Convert.ToByte('?');
                sEA.SendPacket[1] = 0x55;
                sEA.SendPacket[2] = 0x0D;

                this.sendPacket(sEA.SendPacket);
            }
            catch
            {
                MessageBox.Show("Error Requesting Low Voltage Threshold Value");
            }
        }
        */

        private void initializeFromConfigFileDebug()
        {
            ProgramConfigDebug pC = new ProgramConfigDebug();

            this.ucRelayProgramming1.ReprogramBootCodeAuto = pC.Data.ReprogramBoot;
        }

        private void tCPConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new TCPConnectionForm())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    tcpClient = new TCPComms(form.IPAddress, form.Port);
                    tcpClient.TCPCommsException += standardExceptionMessage;
                    if (tcpClient.Connect())
                    {
                        handleSuccessfulTCPConnection();
                    }
                }
            }
        }

        private void handleSuccessfulTCPConnection()
        {
            toolStripStatusLabelMain.Text = String.Format("TCP Connect: {0}:{1}", tcpClient.IPAddress.ToString(), tcpClient.Port);
            tcpClient.DataReceived += TcpClient_DataReceived;
            tCPConnection = true;
            this.requestAllData("handleSuccessfulTCPConnection");
        }

        private void TcpClient_DataReceived(object o, TCPCommsEventArgs tCPCEA)
        {
            bool dReceived = false;

            try
            {
                foreach (byte b in tCPCEA.IncomingData)
                {
                    this.SendConfirmed = true;
                    this.receiveArray[this.rXWritePtr] = b;
                    if (b == 0x06)
                    {
                        if (!this.expectingAck)
                        {
                            logger.Warn("Duplicate TCP ACK ignored. expectingAck=False, caller={0}", this.AcknowledgeCaller);
                        }
                        else
                        {
                            string ackCaller = this.AcknowledgeCaller;

                            this.SendConfirmed = true;
                            this.expectingAck = false;
                            packetAcknowledged(true);
                            this.timerSCITimeOut.Enabled = false;

                            logger.Info("TCP ACK received. caller={0}, expectingAck(after)={1}", ackCaller, this.expectingAck);

                            this.AcknowledgeCaller = string.Empty;

                            if (string.Equals(ackCaller, "CT Ratio Send", StringComparison.OrdinalIgnoreCase))
                            {
                                this.TryDrainPendingRelayTypePhasingSend("ACK-RX-CT-TCP");
                            }
                        }
                    }
                    if (b == 0x0D)
                        dReceived = true;

                    this.rXWritePtr = this.nextRXArrayAddress(this.rXWritePtr);
                }
            }
            catch
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                this.resetCommunicationInterface();
                this.RegisterPolling(true);
            }
            //Check to see if the last byte is a confirmation
            try
            {
                if (!this.readSemaphoreTaken || dReceived)//lastByte == 0x0D )
                {
                    this.BeginInvoke(new EventHandler(this.checkRawData));
                    dReceived = false;
                }
            }
            catch
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                this.resetCommunicationInterface();
                this.RegisterPolling(true);
            }
        }

        private readonly string _waitingForKioskCommand = "Waiting For Kiosk Command";
        private readonly string _kioskCommandReceived = "Kiosk Command Received";

        private void buttonTest_Click(object sender, EventArgs e)
        {
            byte[] packet = new byte[3];

            packet[0] = Convert.ToByte('I');
            packet[1] = 0x55;
            packet[2] = 0x0D;

            labelKioskReceived.Text = _waitingForKioskCommand;
            labelKioskReceived.BackColor = Color.Yellow;
            sendPacket(packet);
        }

        private void comboBoxDNPVoltage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedVoltage = (ProtectorVoltage)comboBoxDNPVoltage.SelectedItem;

            protectorVoltage = ProtectorVoltages.GetVoltage(selectedVoltage.SetBit);

            updateProtectorVoltage(protectorVoltage);
        }

        private void updateProtectorVoltage(ProtectorVoltage value)
        {
#if COMED || LONDON_HYDRO
            ucCloseMode1.ProtectorVoltage = protectorVoltage;
            ucSafeService1.ProtectorVoltage = protectorVoltage;
#endif
            ucTransmitterMonitoring1.ProtectorVoltage = protectorVoltage;
            ucPhasorGraph1.ProtectorVoltage = protectorVoltage;

            ucEventGraph0.ProtectorVoltage = protectorVoltage;
            ucEventGraph1.ProtectorVoltage = protectorVoltage;
            ucEventGraph2.ProtectorVoltage = protectorVoltage;
            ucEventGraph3.ProtectorVoltage = protectorVoltage;
            ucEventGraph4.ProtectorVoltage = protectorVoltage;
            ucEventGraph5.ProtectorVoltage = protectorVoltage;
            ucEventGraph6.ProtectorVoltage = protectorVoltage;
            ucEventGraph7.ProtectorVoltage = protectorVoltage;
            ucLiveData1.ProtectorVoltage = protectorVoltage;
        }

        public delegate void SendHandler(object sender, SendEventArgs sEA);
        public event SendHandler Send;

        private void OnSend(object sender, SendEventArgs sEA)
        {
            if (Send != null)
            {
                Send(this, sEA);
            }
            else
            {
                this.errorHandler(new Exception("OnSend Not Set for writing backup Parameters to master processor"), "In OnSend");
            }
        }
        public delegate void ExceptionHandler(object o, ExceptionEventArgs eEA);

        public event ExceptionHandler CalibrationException;

        private void errorHandler(Exception ex, string title)
        {
            if (CalibrationException != null)
            {
                CalibrationException(this, new ExceptionEventArgs(ex, title));
            }
            else
            {
                throw new Exception("No Exception Handler For Trip Control");
            }
        }

        public bool dataBackup_fromRelay = false;

        private void button_dataStore_Click(object sender, EventArgs e)
        {
            BackUpRelayDatatoFile();
        }

        private void BackUpRelayDatatoFile()
        {
            // Pull data from relay master uP if its firmware is less than rev 10.
            // Since rev 10 onwards there are storage changes for memory-corruption protection.
            bool relayHasDnp = IsDnpCommSupported();

            string filePath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";

            try
            {
                // IMPORTANT:
                // Starting backup should NOT arm autoload continuation yet.
                // Autoload continuation is only armed after successful backup completion.
                pendingAutoloadAfterBackup = false;

                // Start in-memory capture for this backup session.
                BeginBackupCapture();

                dataBackupD.dataBackup_withDNP = relayHasDnp;
                StartBackupTracking(relayHasDnp);

                // READ/REQUEST FROM MASTER PROCESSOR AND CAPTURE IN INCOMING DATA FUNCTIONS
                dataBackup_fromRelay = true;
                dataBackupR.dataBackup_fromRelay = true;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    logger.Info("Existing backup file deleted: {0}", filePath);
                }

                // Optional header placeholder; final file content is written by FlushBackupToDisk() on success.
                File.WriteAllText(
                    filePath,
                    "Data residing in the relay :" + Environment.NewLine +
                    "TimestampUtc: " + backupStartedAtUtc.Value.ToString("O", CultureInfo.InvariantCulture) + Environment.NewLine +
                    "MasterRevision: " + (this.ucRelayProgramming1.MasterRevisionString ?? "UNKNOWN") + Environment.NewLine);

                this.ucShortRange1.Request_SignalStrength();
                this.requestRelayParameters();
                this.requestCalibrationConstants();
                this.requestTransmitterSettings();
                this.requestSafeServiceSettings();

                // Prefer explicit request for backup capture instead of toggling monitor mode
                this.arcFault_requestArcFaultMonitoring();

                if (backupExpectDnp)
                {
                    logger.Info("DNP detected, but DNP settings backup is being skipped for rev9 to rev10 compatibility.");
                    // this.requestDNPSettings();      // 'U'
                    // this.RequestDNPSav5Settings();  // 'D'+'s'
                }

                logger.Info("Backup requests dispatched. Waiting for backup completion/timeout callbacks.");
            }
            catch (Exception ex)
            {
                // Ensure backup state is clean on failure
                backupInProgress = false;
                if (backupTimeoutTimer != null)
                    backupTimeoutTimer.Stop();

                dataBackup_fromRelay = false;
                dataBackupR.dataBackup_fromRelay = false;

                // Clear in-memory capture state on failure
                ClearBackupCaptureState();

                // Do not continue autoload on startup backup failure
                pendingAutoloadAfterBackup = false;

                logger.Error(ex, "Pull data backup failed while dispatching backup requests.");
                throw new Exception("Pull data backup failed", ex);
            }
        }

        private void WriteBackUpData_FileToRelay()
        {
            // Guard: only run legacy backup restore during actual migration/autoload programming.
            // This avoids accidental restore side effects from normal UI actions.
            if (!(pendingAutoloadAfterBackup || pendingRestoreAfterProgramming || this.ucRelayProgramming1.ReprogrammingInProgress))
            {
                logger.Info("Skipping backup restore: not in migration/programming flow.");
                return;
            }

            // Push data backed up in RelayData_Backup.txt from rev9-or-older relay
            // to relay master uP (for rev10+ firmware mapping changes).
            const string filePath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";

            if (!File.Exists(filePath))
            {
                this.messageHandler("Backup Restore", "Backup file not found. Cannot restore.");
                return;
            }

            if (!checkValidDataBackup())
            {
                this.messageHandler("Backup Restore", "Backup file validation failed. Restore aborted.");
                return;
            }

            try
            {
                logger.Info("Starting backup restore to relay.");

                // Live calibration is required during normal app operation.
                // Backup restore should not replay calibration from RelayData_Backup.txt unless explicitly intended.
                logger.Info("Backup restore: skipping CalibrationConstants replay from RelayData_Backup.txt (live apply only).");

                this.writeCloseModeDataBackUp_ToMaster();
                this.writeTripModeDataBackUp_ToMaster();
                this.writeNWProtectorDataBackUp_ToMaster();
                this.writePumpModeDataBackUp_ToMaster();
                this.writeSafeServiceDataBackUp_ToMaster();
                this.writeTransmitterDataBackUp_ToMaster();
                logger.Info("Skipping DNP restore for rev9 to rev10 compatibility.");
                this.writeArcFaultDataBackUp_ToMaster();

                logger.Info("Backup restore completed.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Backup restore failed.");
                this.messageHandler("Backup Restore Failed", ex);
            }
        }


        private void RequestDNPSav5Settings()
        {
            byte[] sendArray = new byte[98];

            sendArray[0] = (byte)'D';
            sendArray[1] = (byte)'s';
            sendArray[97] = 0x0D;

            this.sendPacket(sendArray);
        }

        private void writeCloseModeDataBackUp_ToMaster()
        {
            if (dataBackupCM.dataBackup_closeModeDefaults)
            {
                // Fallback to defaults when old backup values are flagged bad
                this.ucCloseMode1.buttonRestoreDefaults_Click(this, new EventArgs());
                this.ucCloseMode1.sendCloseData();
                return;
            }

            const string filePath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";
            string[] lines = File.ReadAllLines(filePath);

            // Find section start
            int relayParamsHeader = Array.FindIndex(lines, l => string.Equals(l?.Trim(), "Relay Parameters:", StringComparison.Ordinal));
            if (relayParamsHeader < 0)
                throw new Exception("Relay Parameters section not found in backup file.");

            // Need at least 94 bytes after header for relay parameter block
            int relayStart = relayParamsHeader + 1;
            if (relayStart + 93 >= lines.Length)
                throw new Exception("Relay Parameters section is incomplete.");

            byte ReadByteAt(int idx)
            {
                if (idx < 0 || idx >= lines.Length)
                    throw new Exception($"Backup index out of range at line index {idx}.");

                if (!byte.TryParse(lines[idx]?.Trim(), out byte value))
                    throw new Exception($"Invalid byte value at line index {idx}: '{lines[idx]}'.");

                return value;
            }

            // Pull relay parameter bytes into local array [0..93]
            byte[] rp = new byte[94];
            for (int i = 0; i < rp.Length; i++)
                rp[i] = ReadByteAt(relayStart + i);

            // -------- Packet 'C' (close curve bytes from relay params C_byte1..C_byte8) --------
            // Existing code swaps odd/even from file sequence into packet slots.
            byte[] packet_C = new byte[10];
            packet_C[0] = (byte)'C';

            // rp[0..7] correspond to first 8 relay-param bytes written in backup.
            // Preserve legacy mapping:
            // cnt odd -> packet_C[cnt+1], cnt even -> packet_C[cnt-1]
            for (int cnt = 1; cnt <= 8; cnt++)
            {
                byte b = rp[cnt - 1];
                if ((cnt % 2) != 0)
                    packet_C[cnt + 1] = b;
                else
                    packet_C[cnt - 1] = b;
            }

            packet_C[9] = 0x0D;
            this.sendPacket(packet_C);

            // -------- Packet 'M''C' (Mclose bytes) --------
            byte[] packet_MC = new byte[8];
            packet_MC[0] = (byte)'M';

            // Legacy mapping using next 6 bytes from relay params block (originally lines after first 8)
            // rp[8..13] => mapped with same odd/even swap
            for (int cnt = 1; cnt <= 6; cnt++)
            {
                byte b = rp[8 + (cnt - 1)];
                if ((cnt % 2) != 0)
                    packet_MC[cnt + 1] = b;
                else
                    packet_MC[cnt - 1] = b;
            }

            packet_MC[7] = 0x0D;

            // Debug popup before restore-send.
            // Verify which byte contains circle-close in 'C' packet.
            // Start with index 8/10 depending your protocol; adjust after first run.
           
            this.sendPacket(packet_MC);
        }

        private void writeTripModeDataBackUp_ToMaster()
        {
            if (dataBackupTM.dataBackup_tripModeDefaults)
            {
                // Fallback to defaults when old trip data is flagged bad
                this.ucTripMode2.buttonRestoreDefaults_Click(this, new EventArgs());
                this.ucTripMode2.SendTripMode();
                return;
            }

            const string filePath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";
            string[] lines = File.ReadAllLines(filePath);

            int relayParamsHeader = Array.FindIndex(lines, l => string.Equals(l?.Trim(), "Relay Parameters:", StringComparison.Ordinal));
            if (relayParamsHeader < 0)
                throw new Exception("Relay Parameters section not found in backup file.");

            int relayStart = relayParamsHeader + 1;
            if (relayStart + 93 >= lines.Length)
                throw new Exception("Relay Parameters section is incomplete.");

            byte ReadByteAtLineIndex(int idx)
            {
                if (idx < 0 || idx >= lines.Length)
                    throw new Exception($"Backup line index out of range: {idx}");
                if (!byte.TryParse(lines[idx]?.Trim(), out var b))
                    throw new Exception($"Invalid byte at line index {idx}: '{lines[idx]}'");
                return b;
            }

            // Pull relay-parameter section bytes [0..93]
            byte[] rp = new byte[94];
            for (int i = 0; i < rp.Length; i++)
                rp[i] = ReadByteAtLineIndex(relayStart + i);

            // ---------------- Mtrip ('M' + 6 bytes + CR) from rp[14..19] ----------------
            // Original code started here after relay params + close blocks.
            byte[] packet_MT = new byte[8];
            packet_MT[0] = (byte)'M';
            // Fill packet indices [1..6] via legacy mapping
            for (int cnt = 1; cnt <= 6; cnt++)
            {
                byte b = rp[14 + (cnt - 1)];
                if ((cnt % 2) != 0) packet_MT[cnt + 1] = b;
                else packet_MT[cnt - 1] = b;
            }
            packet_MT[7] = 0x0D;
            this.sendPacket(packet_MT);
            Thread.Sleep(1000);

            // ---------------- T0..T4 each 12 bytes ----------------
            // In relay params block these are contiguous after Mtrip:
            // T0: rp[20..31], T1: rp[32..43], T2: rp[44..55], T3: rp[56..67], T4: rp[68..79]
            void SendTPacket(int tNumber, int rpStart)
            {
                byte[] p = new byte[14];
                p[0] = (byte)'T';
                for (int cnt = 1; cnt <= 12; cnt++)
                {
                    byte b = rp[rpStart + (cnt - 1)];
                    if ((cnt % 2) != 0) p[cnt + 1] = b;
                    else p[cnt - 1] = b;
                }
                p[13] = 0x0D;
                this.sendPacket(p);
                Thread.Sleep(1000);
            }

            SendTPacket(0, 20);
            SendTPacket(1, 32);
            SendTPacket(2, 44);
            SendTPacket(3, 56);
            SendTPacket(4, 68);

            // ---------------- MS packet (dummy_PC_param_bytes from rp[92], rp[93]) ----------------
            byte[] packet_MS = new byte[8];
            packet_MS[0] = (byte)'M';
            packet_MS[1] = (byte)'S';
            packet_MS[2] = rp[92];   // first dummy byte
            packet_MS[3] = rp[93];   // second dummy byte
            packet_MS[4] = 0;
            packet_MS[5] = 0;
            packet_MS[6] = 0;
            packet_MS[7] = 0x0D;

            this.sendPacket(packet_MS);
            Thread.Sleep(1000);
        }

        private void writeNWProtectorDataBackUp_ToMaster()
        {
            if (dataBackupNW.dataBackup_nwProtectorDefaults)
            {
                // Old NW Protector data flagged bad/out-of-range -> apply defaults
                // Set defaults directly (no UI click-handler call)
                this.restoreDefaultsTypeAndPhasing();
                this.SendCTRatioAndPhasing(requestAfter: false);
                return;
            }

            const string filePath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";
            string[] lines = File.ReadAllLines(filePath);

            int relayParamsHeader = Array.FindIndex(lines, l => string.Equals(l?.Trim(), "Relay Parameters:", StringComparison.Ordinal));
            if (relayParamsHeader < 0)
                throw new Exception("Relay Parameters section not found in backup file.");

            int relayStart = relayParamsHeader + 1;
            if (relayStart + 93 >= lines.Length)
                throw new Exception("Relay Parameters section is incomplete.");

            byte ReadByteAt(int idx)
            {
                if (idx < 0 || idx >= lines.Length)
                    throw new Exception($"Backup index out of range at line index {idx}.");
                if (!byte.TryParse(lines[idx]?.Trim(), out byte value))
                    throw new Exception($"Invalid byte at line index {idx}: '{lines[idx]}'");
                return value;
            }

            // relay_type bytes in relay param block:
            // rp[80] = relay_type_byte2
            // rp[81] = relay_type_byte1
            byte relayTypeByte2 = ReadByteAt(relayStart + 80);
            byte relayTypeByte1 = ReadByteAt(relayStart + 81);

            byte[] packet_s = new byte[4];
            packet_s[0] = (byte)'s';
            packet_s[1] = relayTypeByte1; // keep wire order expected by receiver
            packet_s[2] = relayTypeByte2;
            packet_s[3] = 0x0D;

            this.sendPacket(packet_s);
            Thread.Sleep(1000);
        }

        private void writePumpModeDataBackUp_ToMaster()
        {
            if (dataBackupPM.dataBackup_pumpModeDefaults)
            {
                // Old pump data flagged bad/out-of-range -> apply defaults
                this.ucPumpMode1.buttonRestoreDefaults_Click(this, new EventArgs());
                this.ucPumpMode1.SendPumpMode();
                return;
            }

            const string filePath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";
            string[] lines = File.ReadAllLines(filePath);

            int relayParamsHeader = Array.FindIndex(lines, l => string.Equals(l?.Trim(), "Relay Parameters:", StringComparison.Ordinal));
            if (relayParamsHeader < 0)
                throw new Exception("Relay Parameters section not found in backup file.");

            int relayStart = relayParamsHeader + 1;
            if (relayStart + 93 >= lines.Length)
                throw new Exception("Relay Parameters section is incomplete.");

            byte ReadByteAt(int idx)
            {
                if (idx < 0 || idx >= lines.Length)
                    throw new Exception($"Backup index out of range at line index {idx}.");
                if (!byte.TryParse(lines[idx]?.Trim(), out byte value))
                    throw new Exception($"Invalid byte at line index {idx}: '{lines[idx]}'");
                return value;
            }

            // Pump bytes in relay-param block are rp[82..89]
            byte[] p = new byte[8];
            for (int i = 0; i < 8; i++)
                p[i] = ReadByteAt(relayStart + 82 + i);

            byte[] packet_G = new byte[10];
            packet_G[0] = (byte)'G';

            // Preserve original mapping logic:
            // cnt 1..2 and 7..8 direct to same index
            // cnt 3..6 odd/even swap
            for (int cnt = 1; cnt <= 8; cnt++)
            {
                byte b = p[cnt - 1];

                if ((cnt < 3) || (cnt >= 7))
                {
                    packet_G[cnt] = b;
                }
                else
                {
                    if ((cnt % 2) != 0) packet_G[cnt + 1] = b;
                    else packet_G[cnt - 1] = b;
                }
            }

            packet_G[9] = 0x0D;
            this.sendPacket(packet_G);
            Thread.Sleep(1000);
        }

        private void writeSafeServiceDataBackUp_ToMaster()
        {
            if (dataBackupSSM.dataBackup_safeServiceDefaults)
            {
                // Old safe-service data flagged bad/out-of-range -> apply defaults
                this.ucSafeService1.buttonRestoreDefaults_Click(this, new EventArgs());
                this.ucSafeService1.SendSafeService(requestAllAfterWrite: false);
                return;
            }

            const string filePath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";
            string[] lines = File.ReadAllLines(filePath);

            int safeHeader = Array.FindIndex(lines, l => string.Equals(l?.Trim(), "Safe Service Data:", StringComparison.Ordinal));
            if (safeHeader < 0)
                throw new Exception("Safe Service Data section not found in backup file.");

            int safeStart = safeHeader + 1;
            if (safeStart + 19 >= lines.Length)
                throw new Exception("Safe Service Data section is incomplete.");

            byte ReadByteAt(int idx)
            {
                if (idx < 0 || idx >= lines.Length)
                    throw new Exception($"Backup index out of range at line index {idx}.");
                if (!byte.TryParse(lines[idx]?.Trim(), out byte value))
                    throw new Exception($"Invalid byte at line index {idx}: '{lines[idx]}'");
                return value;
            }

            byte[] packet_F = new byte[22];
            packet_F[0] = 0x0F;

            for (int cnt = 1; cnt <= 20; cnt++)
                packet_F[cnt] = ReadByteAt(safeStart + (cnt - 1));

            packet_F[21] = 0x0D;
            this.sendPacket(packet_F);
            Thread.Sleep(1000);
        }

        private void writeTransmitterDataBackUp_ToMaster()
        {
            if (dataBackupTX.dataBackup_txDefaults)
            {
                // Old TX data flagged bad/out-of-range -> apply defaults
                this.ucTransmitter1.buttonRestoreDefaults_Click(this, new EventArgs());
                this.ucTransmitter1.SendTransmitterSettings();
                return;
            }

            const string filePath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";
            string[] lines = File.ReadAllLines(filePath);

            int txHeader = Array.FindIndex(lines, l => string.Equals(l?.Trim(), "Transmitter Parameters:", StringComparison.Ordinal));
            if (txHeader < 0)
                throw new Exception("Transmitter Parameters section not found in backup file.");

            int txStart = txHeader + 1;
            if (txStart + 31 >= lines.Length)
                throw new Exception("Transmitter Parameters section is incomplete.");

            byte ReadByteAt(int idx)
            {
                if (idx < 0 || idx >= lines.Length)
                    throw new Exception($"Backup index out of range at line index {idx}.");
                if (!byte.TryParse(lines[idx]?.Trim(), out byte value))
                    throw new Exception($"Invalid byte at line index {idx}: '{lines[idx]}'");
                return value;
            }

            byte[] packet_Y = new byte[34];
            packet_Y[0] = (byte)'Y';

            // 32 TX bytes (includes ID + SN bytes)
            for (int cnt = 1; cnt <= 32; cnt++)
                packet_Y[cnt] = ReadByteAt(txStart + (cnt - 1));

            packet_Y[33] = 0x0D;
            this.sendPacket(packet_Y);
            Thread.Sleep(1000);
        }

        private void writeDNPDataBackUp_ToMaster()
        {
            // NOTE: use DNP default flag, not TX flag
            if (dataBackupD.dataBackup_dnpDefaults)
            {
                this.ucDNP.buttonDefaults_Click(this, new EventArgs());
                this.ucDNP.buttonSendAllDNPSettings_Click(this, new EventArgs());
                return;
            }

            const string filePath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";
            string[] lines = File.ReadAllLines(filePath);

            int dnpHeader = Array.FindIndex(
                lines,
                l => string.Equals(l?.Trim(), "DNP Data:", StringComparison.Ordinal));

            // Missing DNP section: ignore (non-DNP backup / older backup format)
            if (dnpHeader < 0)
            {
                // optional: status/log message instead of modal popup
                // this.messageHandler("Backup Restore", "DNP Data section not found; skipping DNP restore.");
                return;
            }

            int dnpStart = dnpHeader + 1;
            if (dnpStart + 97 >= lines.Length)
                throw new Exception("DNP Data section is incomplete (expected 98 bytes).");

            byte ReadByteAt(int idx)
            {
                if (idx < 0 || idx >= lines.Length)
                    throw new Exception($"Backup index out of range at line index {idx}.");

                if (!byte.TryParse(lines[idx]?.Trim(), out byte value))
                    throw new Exception($"Invalid byte at line index {idx}: '{lines[idx]}'");

                return value;
            }

            byte[] dnp = new byte[98];
            for (int i = 0; i < dnp.Length; i++) dnp[i] = ReadByteAt(dnpStart + i);

            byte[] packet_D = new byte[98];
            packet_D[0] = (byte)'D';
            packet_D[1] = 97;
            packet_D[2] = 0;
            packet_D[3] = 0;

            packet_D[7] = dnp[0];
            packet_D[8] = dnp[1];
            packet_D[4] = dnp[2];
            packet_D[5] = dnp[3];

            for (int i = 0; i < 88; i++) packet_D[9 + i] = dnp[4 + i];

            packet_D[97] = 0x0D;
            this.sendPacket(packet_D);
            Thread.Sleep(1000);
        }

        private void writeDNPSAv5SettingsDataBackUp_ToMaster()
        {
            if (dataBackupTX.dataBackup_txDefaults == false) //if not loading SAv5 defaults - and loading old SAv5 params back to the relay
            {
                //=======================      ( 'D' + 'S' )      ===================================================
                byte[] packet_DS = new byte[98];
                string lineRead;
                StreamReader srDS = new StreamReader("C:\\DGI Systems\\Relay\\Saved Data\\RelayData_Backup.txt");
                int DS = 1; // go to the beginning of the data backup file
                while (DS <= 312)
                {
                    lineRead = srDS.ReadLine(); //Read the next line untill we reach the begining of data with command 'D' + 'S'
                    DS++;
                }

                packet_DS[0] = 68;    // 'D'
                /* packet_DS[1] = 83;    // 'S'  
                 packet_DS[2] = 0;
                 packet_DS[3] = 8;     // Aggressive mode enabled + SHA1 enabled + authentication enabled + Key change algorithm selection
                 packet_DS[4] = 0;
                 packet_DS[5] = 20;
                 packet_DS[6] = 0;
                 packet_DS[7] = 8;
                 packet_DS[8] = 7;
                 packet_DS[9] = 160;
                 packet_DS[10] = 15;
                 packet_DS[11] = 5;
                 packet_DS[12] = 2;
                 packet_DS[13] = 3;
                 packet_DS[14] = 0;
                 packet_DS[15] = 5;
                 packet_DS[16] = 0;
                 packet_DS[17] = 5;
                 packet_DS[18] = 0;
                 packet_DS[19] = 3;
                 packet_DS[20] = 0;
                 packet_DS[21] = 3;
                 packet_DS[22] = 0;
                 packet_DS[23] = 100;
                 packet_DS[24] = 0;
                 packet_DS[25] = 100;
                 packet_DS[26] = 0;
                 packet_DS[27] = 100;
                 packet_DS[28] = 0;
                 packet_DS[29] = 100;
                 packet_DS[30] = 0;
                 packet_DS[31] = 10;
                 packet_DS[32] = 0;
                 packet_DS[33] = 2;
                 packet_DS[34] = 0;
                 packet_DS[35] = 10;
                 packet_DS[36] = 0;
                 packet_DS[37] = 100;
                 packet_DS[38] = 0;
                 packet_DS[39] = 10;
                 packet_DS[40] = 0;
                 packet_DS[41] = 5;
                 packet_DS[42] = 0;
                 packet_DS[43] = 1;
                 packet_DS[44] = 0;
                 packet_DS[45] = 1;
                 for (int cnt = 46; cnt <= 96; cnt++)
                 {
                     packet_DS[cnt] = 0;
                 }
                */

                lineRead = srDS.ReadLine(); //Read the next line
                packet_DS[1] = Convert.ToByte(lineRead);

                for (int cnt = 2; cnt <= 3; cnt++)
                {
                    lineRead = srDS.ReadLine(); //Read the next line
                    if ((cnt % 2) != 0)//odd numbered ?
                        packet_DS[2] = Convert.ToByte(lineRead);
                    else
                        packet_DS[3] = Convert.ToByte(lineRead);
                }
                for (int cnt = 4; cnt <= 5; cnt++)
                {
                    lineRead = srDS.ReadLine(); //Read the next line
                    if ((cnt % 2) != 0)//odd numbered ?
                        packet_DS[4] = Convert.ToByte(lineRead);
                    else
                        packet_DS[5] = Convert.ToByte(lineRead);
                }
                for (int cnt = 7; cnt <= 96; cnt++)
                {
                    lineRead = srDS.ReadLine(); //Read the next line
                    packet_DS[cnt] = Convert.ToByte(lineRead);
                }


                packet_DS[97] = 0x0D;
                this.sendPacket(packet_DS);
                Thread.Sleep(1000);   // 1 second delay
            }//if not loading SAv5 defaults - and loading old SAv5 params back to the relay
            else
            {
                // since SAv5 parameters from old firmware rev was found out to be bad / corrupt / out of range
                // do not load that data ( which is backed up in the file)
                // instead load the default SAv5 params to the relay with the new firmware
                this.ucDNPSAv5Settings2.buttonDefault_Click(this, new EventArgs());
                this.ucDNPSAv5Settings2.buttonSendSettings_Click(this, new EventArgs());
            }
        }

        private void writeArcFaultDataBackUp_ToMaster()
        {
            const string filePath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";
            string[] lines = File.ReadAllLines(filePath);

            int arcHeader = Array.FindIndex(lines, l => string.Equals(l?.Trim(), "Arc Fault Parameters:", StringComparison.Ordinal));
            if (arcHeader < 0) throw new Exception("Arc Fault Parameters section not found in backup file.");

            int arcStart = arcHeader + 1;
            if (arcStart + 39 >= lines.Length) throw new Exception("Arc Fault Parameters section is incomplete (expected 40 bytes).");

            byte ReadByteAt(int idx)
            {
                if (idx < 0 || idx >= lines.Length) throw new Exception($"Backup index out of range at line index {idx}.");
                if (!byte.TryParse(lines[idx]?.Trim(), out byte value)) throw new Exception($"Invalid byte at line index {idx}: '{lines[idx]}'");
                return value;
            }

            byte[] packet_E = new byte[42];
            packet_E[0] = (byte)'E';

            for (int cnt = 1; cnt <= 40; cnt++)
                packet_E[cnt] = ReadByteAt(arcStart + (cnt - 1));

            packet_E[41] = 0x0D;
            this.sendPacket(packet_E);
            Thread.Sleep(1000);
        }

        private void writeCalibrationDataBackUp_ToMaster()
        {
            if (dataBackupPM.dataBackup_pumpModeDefaults == false) //if not loading calibration constant defaults - and loading old calibration back to the relay
            {
                //=======================      pump_mode_enabled to timeout_on_breaker_close ( G )      ===================================================
                byte[] packet_Cal = new byte[62];// [60];
                string lineRead;
                StreamReader srCal = new StreamReader("C:\\DGI Systems\\Relay\\Saved Data\\RelayData_Backup.txt");
                int Cal = 1; // go to the beginning of the data backup file
                while (Cal <= 98)
                {
                    lineRead = srCal.ReadLine(); //Read the next line untill we reach the begining of data with command 'J' for calibration
                    Cal++;
                }

                packet_Cal[0] = 74;   // 'J' calibration data
                /*packet_Cal[1] = 0;    
                  packet_Cal[2] = 8;    
                  packet_Cal[3] = 108;    
                  packet_Cal[4] = 2;
                  packet_Cal[5] = 0;    
                  packet_Cal[6] = 8;
                  packet_Cal[7] = 108;    
                  packet_Cal[8] = 2;  
                  */

                for (int cnt = 1; cnt <= 60; cnt++)
                {
                    lineRead = srCal.ReadLine(); //Read the next line
                    if ((cnt % 2) != 0)//odd numbered ?
                        packet_Cal[cnt + 1] = Convert.ToByte(lineRead);
                    else
                        packet_Cal[cnt - 1] = Convert.ToByte(lineRead);
                }

                packet_Cal[61] = 0x0D;
                this.sendPacket(packet_Cal);
                Thread.Sleep(1000);   // 1 second delay
            }//if not loading calibration constant defaults - and loading old calibration constants back to the relay
            else
            {
                // since calibration constants from old firmware rev was found out to be bad / corrupt / out of range
                // do not load those calibration constants ( which are backed up in the file)
                // instead load the defaults calibration constants to the relay with the new firmware
                this.ucPumpMode1.buttonRestoreDefaults_Click(this, new EventArgs());
                this.ucPumpMode1.SendPumpMode();
            }
        }

        private bool checkValidDataBackup()
        {
            try
            {
                const string filePath = @"C:\DGI Systems\Relay\Saved Data\RelayData_Backup.txt";

                if (!File.Exists(filePath))
                {
                    logger.Warn("Backup validation failed: file not found: {0}", filePath);
                    return false;
                }

                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length == 0)
                {
                    logger.Warn("Backup validation failed: file is empty.");
                    return false;
                }

                bool hasHeader =
                    lines.Any(l => l != null && l.Trim().Equals("Data residing in the relay :", StringComparison.OrdinalIgnoreCase));

                bool hasRelayParams = lines.Any(l => l != null && l.Trim().Equals("Relay Parameters:", StringComparison.Ordinal));
                bool hasCal = lines.Any(l => l != null && l.Trim().Equals("Calibration Constants:", StringComparison.Ordinal));
                bool hasTx = lines.Any(l => l != null && l.Trim().Equals("Transmitter Parameters:", StringComparison.Ordinal));
                bool hasDnp = lines.Any(l => l != null && l.Trim().Equals("DNP Data:", StringComparison.Ordinal));
                bool hasSafeService = lines.Any(l => l != null && l.Trim().Equals("Safe Service Data:", StringComparison.Ordinal));
                bool hasArc = lines.Any(l => l != null && l.Trim().Equals("Arc Fault Parameters:", StringComparison.Ordinal));

                bool hasSav5 = lines.Any(l => l != null && l.Trim().Equals("DNPSAv5 Settings:", StringComparison.Ordinal));

                bool baseOk = hasHeader && hasRelayParams && hasCal && hasTx && hasSafeService && hasArc;

                if (!baseOk)
                {
                    logger.Warn(
                        "Backup validation failed. Header={0}, Relay={1}, Cal={2}, Tx={3}, SafeService={4}, Arc={5}",
                        hasHeader, hasRelayParams, hasCal, hasTx, hasSafeService, hasArc);
                    return false;
                }

                if (dataBackupD.dataBackup_withDNP)
                {
                    if (!hasDnp)
                    {
                        logger.Info("DNP Data section not present by design for rev9 to rev10 compatibility.");
                    }

                    if (!hasSav5)
                    {
                        logger.Info("DNPSAv5 Settings section not present by design for rev9 to rev10 compatibility.");
                    }
                }

                logger.Info("Backup validation passed.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Backup validation exception.");
                return false;
            }
        }

        public decimal GetFixed_12FracBits(decimal value)
        {
            Int16 temp;
            temp = (Int16)(value / Constants.TwelveFracBits);
            return (decimal)temp;
        }


        private void btn_RestorePC_defaults_Click(object sender, EventArgs e)
        {
           // MessageBox.Show("Restore default values for Permissive Close");
            this.numericUpDown_PC_floatTime.Value = 38;
            this.numericUpDown_PC_activeTime.Value = 15;
            this.numericUpDown_PC_voltage.Value = 5;
            this.comboBox_PC.SelectedIndex = 0;
            this.btn_PermCl_Active.BackColor = Color.Transparent;
        }

        private void SetBusyUi(bool busy)
        {
            if (this.IsDisposed) return;

            if (this.InvokeRequired)
            {
                BeginInvoke((Action)(() => SetBusyUi(busy)));
                return;
            }

            screenD.screenDisable = busy;
            Application.UseWaitCursor = busy;
            Cursor.Current = busy ? Cursors.WaitCursor : Cursors.Default;
            this.UseWaitCursor = busy;
            this.Refresh();
        }

        private void btn_PC_Send_Click(object sender, EventArgs e)
        {
            logger.Info("btn_PC_Send_Click ENTER");

            try
            {
                SetBusyUi(true);
                logger.Info("SetBusyUi(true) done");

                _pcApplyPendingConfirmation = true;
                SendPCData(requestAllAfterWrite: false);

                logger.Info("SendPCData returned");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "btn_PC_Send_Click EXCEPTION");
                throw;
            }
            finally
            {
                logger.Info("finally -> SetBusyUi(false)");
                SetBusyUi(false);
                logger.Info("finally complete");
            }
        }

        // Add field in MainControl class

#pragma warning disable CS0414
        bool _pcApplyPendingConfirmation = false;
#pragma warning restore CS0414



        private void SendPCData(bool requestAllAfterWrite = false)
        {
            logger.Info($"SendPCData ENTER requestAllAfterWrite={requestAllAfterWrite}");

            UInt16 rawVoltage = (UInt16)Math.Round(
                numericUpDown_PC_voltage.Value / Constants.TwelveFracBits,
                MidpointRounding.AwayFromZero);

            byte[] packet = new byte[7];
            packet[0] = (byte)'}';
            packet[1] = (comboBox_PC.SelectedIndex == 0) ? (byte)1 : (byte)0;
            packet[2] = (byte)numericUpDown_PC_floatTime.Value;
            packet[3] = (byte)numericUpDown_PC_activeTime.Value;
            packet[4] = (byte)(rawVoltage & 0xFF);         // LOW
            packet[5] = (byte)((rawVoltage >> 8) & 0xFF);  // HIGH
            packet[6] = 0x0D;

            logger.Info($"pcPacket={BitConverter.ToString(packet)}");

            var sea = new SendEventArgs(packet.Length)
            {
                SendPacket = packet,
                WithAck = true,
                RequestAll = requestAllAfterWrite
            };

            // guard: MainControl must not send relay commands while the relay programming lifecycle is active
            if (IsRelayProgrammingLifecycleActive())
            {
                logger.Warn("MainControl suppressed relay send while ucRelayProgramming owns active lifecycle.");
                return;
            }

            this.standardizedSendData(this, sea); // or OnSend path if PC lives in a UC
            logger.Info("PC packet sent");

            if (!requestAllAfterWrite)
            {
                // targeted refresh only, no blocking sleep
                this.request_PCdata();
                logger.Info("request_PCdata sent");
            }

            logger.Info("SendPCData EXIT");
        }

        private void request_PCdata()
        {
            byte[] packet = new byte[3];
            packet[0] = 0x7E; // '~'
            packet[1] = 0x55; // 'U'
            packet[2] = 0x0D;

            this.sendPacket(packet);   // <-- not sendPacketAck
        }

        public void send_PC_active()
        {
            if (btn_PermCl_Active.BackColor == Color.Transparent)
            {
                this.btn_PermCl_Active.BackColor = Color.Yellow;

                decimal tempVoltage = GetFixed_12FracBits(numericUpDown_PC_voltage.Value);
                this.comboBox_PC.SelectedIndex = 0; // Enable Permissive Close

                byte[] packet = new byte[7]; //permissivePacketSize
                packet[0] = (byte)'}';
                packet[1] = 1;               // Enable Permissive Close
                packet[2] = 0;               // special - only for Permissive Close
                packet[3] = (byte)numericUpDown_PC_activeTime.Value;
                packet[4] = (byte)((int)tempVoltage & 0xFF);           // LOW byte//(byte)(((int)tempVoltage >> 8) & 0x00FF);
                packet[5] = (byte)(((int)tempVoltage >> 8) & 0xFF);    // HIGH byte//(byte)((int)tempVoltage & 0x00FF);
                packet[6] = 0x0D;

                this.sendPacket(packet);
            }
            else if (btn_PermCl_Active.BackColor == Color.Yellow)
            {

            }
        }

        private void request_ATdata()
        {
            // asks master to send Adaptive Trip data to the APP
            byte[] packet = new byte[3];

            packet[0] = (byte)'[';
            packet[1] = (byte)'U';
            packet[2] = 0x0D;

            this.sendPacket(packet);
        }

        private void btn_RelaxClose_Click(object sender, EventArgs e)
        {
            relaxCloseC.RelaxCloseClick = true;
            this.ucCloseMode1.sendRelaxClose();
        }

        private void btn_ClearPumpProtect_Click(object sender, EventArgs e)
        {
            this.ucPumpMode1.sendClearPumpProtect();
        }

        private void btn_PermCl_Active_Click(object sender, EventArgs e)
        {
            this.send_PC_active();
        }

        private void button_push_Click(object sender, EventArgs e)
        {
            WriteBackUpData_FileToRelay();
        }

        private void ucRemoteCommandBlock1_Load(object sender, EventArgs e)
        {

        }

        private void resetRelayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.messageHandler("Reset Relay", "Do you really want to reset the relay?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                this.resetBothProcs();
            }
        }

        private void btn_getLC_Click(object sender, EventArgs e)
        {
            SendEventArgs sEA = new SendEventArgs(82);

            sEA.SendPacket[0] = 0x17;
            sEA.SendPacket[2] = 0x10;
            sEA.SendPacket[4] = 0x01;
            sEA.SendPacket[81] = 0x0D;

            this.sendPacket(sEA.SendPacket);
        }

        private void btn_clrLC_Click(object sender, EventArgs e)
        {
            var response = MessageBox.Show("Are you sure you want to clear the count?","Clear Lightning Count?", MessageBoxButtons.YesNo);

            if (response == DialogResult.Yes)
            {
                SendEventArgs sEA = new SendEventArgs(82);

                sEA.SendPacket[0] = 0x17;
                sEA.SendPacket[2] = 0x10;
                sEA.SendPacket[81] = 0x0D;

                this.sendPacket(sEA.SendPacket);
            }
        }

        private void btn_LoadProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sending all parameters from the selected profile to the Relay");
            this.buttonSendAll_Click(this, new EventArgs());
        }
        private void StartBackupTracking(bool expectDnp)
        {
            // Reset state for a fresh backup session
            backupInProgress = true;
            backupExpectDnp = expectDnp;

            backupGotRelayParams = false;
            backupGotCalibration = false;
            backupGotTx = false;
            backupGotSafeService = false;
            backupGotArcFault = false;
            backupGotDnpData = false;
            backupGotDnpSav5 = false;

            backupStartedAtUtc = DateTime.UtcNow;

            if (backupTimeoutTimer == null)
            {
                backupTimeoutTimer = new System.Windows.Forms.Timer();
                backupTimeoutTimer.Interval = BackupTimeoutMs;
                backupTimeoutTimer.Tick += backupTimeoutTimer_Tick;
            }
            else
            {
                // Ensure interval is always in sync with config constant
                backupTimeoutTimer.Interval = BackupTimeoutMs;
            }

            // Restart timeout window for this run
            backupTimeoutTimer.Stop();
            backupTimeoutTimer.Start();

            logger.Info(
                "Backup tracking started. ExpectDnp={0}, TimeoutMs={1}, StartedAtUtc={2:O}",
                backupExpectDnp,
                BackupTimeoutMs,
                backupStartedAtUtc);
        }

        private void backupTimeoutTimer_Tick(object sender, EventArgs e)
        {
            backupTimeoutTimer.Stop();

            if (!backupInProgress)
                return;

            var elapsedMs = backupStartedAtUtc.HasValue
                ? (int)(DateTime.UtcNow - backupStartedAtUtc.Value).TotalMilliseconds
                : 0;
            var missing = GetBackupMissingSectionsSummary();

            logger.Warn(
                "Backup timeout. ElapsedMs={0}, MissingSections={1}",
                elapsedMs,
                missing);

            CompleteBackupAndContinue(
                $"Backup timeout reached ({BackupTimeoutMs / 1000}s). " +
                $"Continuing with available data. Missing: {missing}");
        }
        private string GetBackupMissingSectionsSummary()
        {
            var missing = new List<string>();

            if (!backupGotRelayParams) missing.Add("RelayParams");
            if (!backupGotCalibration) missing.Add("Calibration");
            if (!backupGotTx) missing.Add("Transmitter");
            if (!backupGotSafeService) missing.Add("SafeService");
            if (!backupGotArcFault) missing.Add("ArcFault");

            return missing.Count == 0 ? "None" : string.Join(", ", missing);
        }


        public void RestoreNormalCommsAfterAutoloadDecline(string caller = "unknown")
        {
            logger.Info(
                "RestoreNormalCommsAfterAutoloadDecline ENTER caller={0} | pre: pendingAutoloadAfterBackup={1}, pendingRestoreAfterProgramming={2}, backupInProgress={3}, loadingNewCode={4}, quietMode={5}, pauseMonitoring={6}",
                caller,
                pendingAutoloadAfterBackup,
                pendingRestoreAfterProgramming,
                backupInProgress,
                this.loadingNewCode,
                this.quietMode,
                this.pauseMonitoring);

            pendingAutoloadAfterBackup = false;
            pendingRestoreAfterProgramming = false;
            backupInProgress = false;
            skipAutoloadAfterDecline = false;
            this.ucRelayProgramming1.AutoloadAcceptedPendingBackup = false;

            this.loadingNewCode = false;
            this.quietMode = false;
            this.pauseMonitoring = false;

            if (backupTimeoutTimer != null)
                backupTimeoutTimer.Stop();

            this.UseWaitCursor = false;
            Application.UseWaitCursor = false;
            System.Windows.Forms.Cursor.Current = Cursors.Default;
            this.enableAll(true);

            this.monitoring(true);
            this.RegisterPolling(true);
            this.requestRelayRegisters();

            logger.Info(
                "RestoreNormalCommsAfterAutoloadDecline EXIT caller={0} | post: pendingAutoloadAfterBackup={1}, pendingRestoreAfterProgramming={2}, backupInProgress={3}, loadingNewCode={4}, quietMode={5}, pauseMonitoring={6}",
                caller,
                pendingAutoloadAfterBackup,
                pendingRestoreAfterProgramming,
                backupInProgress,
                this.loadingNewCode,
                this.quietMode,
                this.pauseMonitoring);
        }

        private bool IsBackupComplete()
        {
            bool commonDone =
                backupGotRelayParams &&
                backupGotCalibration &&
                backupGotTx &&
                backupGotSafeService &&
                backupGotArcFault;

            if (!commonDone)
                return false;

            if (backupExpectDnp)
            {
                logger.Info("DNP was detected, but DNP backup completion is being skipped for rev9 to rev10 compatibility.");
            }

            return true;
        }

        private void TryCompleteBackup()
        {
            if (backupInProgress && IsBackupComplete())
            {
                CompleteBackupAndContinue(null);
            }
        }

        private void CompleteBackupAndContinue(string timeoutMessageOrNull)
        {
            bool wasInProgress = backupInProgress;
            backupInProgress = false;

            if (backupTimeoutTimer != null)
                backupTimeoutTimer.Stop();

            dataBackup_fromRelay = false;
            dataBackupR.dataBackup_fromRelay = false;

            var elapsedMs = backupStartedAtUtc.HasValue
                ? (int)(DateTime.UtcNow - backupStartedAtUtc.Value).TotalMilliseconds
                : 0;
            var missing = GetBackupMissingSectionsSummary();

            logger.Info(
                "Backup complete. WasInProgress={0}, ElapsedMs={1}, ExpectDnp={2}, MissingSections={3}",
                wasInProgress,
                elapsedMs,
                backupExpectDnp,
                missing);

            if (!string.IsNullOrEmpty(timeoutMessageOrNull))
            {
                pendingAutoloadAfterBackup = false;
                MessageBox.Show(timeoutMessageOrNull);
                logger.Warn("Backup ended with timeout/partial data; autoload continuation not armed.");
                return;
            }
            // Write once at successful completion, not in each packet handler.
            FlushBackupToDisk();
            pendingAutoloadAfterBackup = true;


            this.ucRelayProgramming1.AutoloadAcceptedPendingBackup = false;

            logger.Info(
                "Backup completed successfully. ReprogrammingInProgress={0}, pendingAutoloadAfterBackup={1}, pendingRestoreAfterProgramming={2}, AutoloadAcceptedPendingBackup={3}",
                this.ucRelayProgramming1.ReprogrammingInProgress,
                pendingAutoloadAfterBackup,
                pendingRestoreAfterProgramming,
                this.ucRelayProgramming1.AutoloadAcceptedPendingBackup);

            this.ucRelayProgramming1.ResumeAutoloadAfterBackup();

            logger.Info(
                "After ResumeAutoloadAfterBackup: pendingAutoloadAfterBackup={0}, AutoloadAcceptedPendingBackup={1}, reprogrammingInProgress={2}, state={3}",
                pendingAutoloadAfterBackup,
                this.ucRelayProgramming1.AutoloadAcceptedPendingBackup,
                this.ucRelayProgramming1.ReprogrammingInProgress,
                this.ucRelayProgramming1.State);

            if (this.ucRelayProgramming1.ReprogrammingInProgress)
            {
                pendingAutoloadAfterBackup = false;
                logger.Info("Autoload continuation consumed; programming started.");
            }
            else if (pendingAutoloadAfterBackup &&
                     (this.ucRelayProgramming1.State == RelayProgrammingStates.AutoLoadCheckBoot ||
                      this.ucRelayProgramming1.State == RelayProgrammingStates.ManualLoadCheckBoot ||
                      this.ucRelayProgramming1.State == RelayProgrammingStates.CheckMasterBootCode ||
                      this.ucRelayProgramming1.State == RelayProgrammingStates.LoadingMasterBootLoader ||
                      this.ucRelayProgramming1.State == RelayProgrammingStates.DoneLoadingMasterBootLoader ||
                      this.ucRelayProgramming1.ProgramBootCodeInProgress))
            {
                logger.Info(
                    "Autoload continuation still in boot-check/boot-repair handoff; keeping comm suppression active. " +
                    "State={0}, pendingAutoloadAfterBackup={1}, programBootCodeInProgress={2}",
                    this.ucRelayProgramming1.State,
                    pendingAutoloadAfterBackup,
                    this.ucRelayProgramming1.ProgramBootCodeInProgress);
            }
            else
            {
                logger.Info("Autoload continuation deferred with no active boot-check; restoring normal UI/monitoring.");
                pendingAutoloadAfterBackup = false;
                this.loadingNewCode = false;
                this.quietMode = false;
                this.pauseMonitoring = false;

                this.UseWaitCursor = false;
                Application.UseWaitCursor = false;
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                this.enableAll(true);

                this.monitoring(true);
                this.RegisterPolling(true);
                this.requestRelayRegisters();
            }
        }
    }

    public partial class MyPort : SerialPort
    {
        public MyPort(System.ComponentModel.IContainer iC) : base(iC)
        {
        }
        public new void Open()
        {
            try
            {
                base.Open();

                /*
                ** because of the issue with the FTDI USB serial device,
                ** the call to the stream's finalize is suppressed
                **
                ** it will be un-suppressed in Dispose if the stream
                ** is still good
                */
                GC.SuppressFinalize(BaseStream);
            }
            catch { }
        }

        public new void Dispose()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (base.Container != null))
            {
                base.Container.Dispose();
            }

            try
            {
                /*
                ** because of the issue with the FTDI USB serial device,
                ** the call to the stream's finalize is suppressed
                **
                ** an attempt to un-suppress the stream's finalize is made
                ** here, but if it fails, the exception is caught and
                ** ignored
                */
                GC.ReRegisterForFinalize(BaseStream);

                base.Dispose(disposing);
            }
            catch { }

            //base.Dispose(disposing);
        }
    }

    [Serializable()]

    public class SavedSettingsv2 : ISerializable
    {
        public SavedSettingsv2()
        {
        }

        public List<SavedSettingv2> Settings = new List<SavedSettingv2>();

        public SavedSettingsv2(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Settings = (List<SavedSettingv2>)info.GetValue("Settings", typeof(List<SavedSettingv2>));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Saved Settings.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Settings", this.Settings);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Saved Settings Get Object Data", ex);
            }
        }

        public void AddState(SavedSettingv2 sS)
        {
            int i = 0;
            for (i = 0; i < this.Settings.Count; ++i)
            {
                if (this.Settings[i].Name == sS.Name)
                {
                    this.Settings[i] = sS;
                    break;
                }
            }

            if (i == this.Settings.Count)
            {
                this.Settings.Add(sS);
            }

            this.Settings.Sort(delegate (SavedSettingv2 sS1, SavedSettingv2 sS2) { return sS1.Name.CompareTo(sS2.Name); });
        }

        public void DeleteState(string name)
        {
            foreach (SavedSettingv2 sS in this.Settings)
            {
                if (sS.Name == name)
                {
                    this.Settings.Remove(sS);
                    break;
                }
            }
        }

        public void DeleteState(SavedSettingv2 sS)
        {
            try
            {
                if (this.Settings.Count > 0)
                {
                    this.Settings.Remove(sS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Deleting Saved State", ex);
            }
        }

    }

    [Serializable()]

    public class SavedSettingsV3 : ISerializable
    {
        public SavedSettingsV3()
        {
        }

        public List<SavedSettingV3> Settings = new List<SavedSettingV3>();

        public SavedSettingsV3(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Settings = (List<SavedSettingV3>)info.GetValue("Settings", typeof(List<SavedSettingV3>));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Saved Settings.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Settings", this.Settings);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Saved Settings Get Object Data", ex);
            }
        }

        public void AddState(SavedSettingV3 sS)
        {
            int i = 0;
            for (i = 0; i < this.Settings.Count; ++i)
            {
                if (this.Settings[i].Name == sS.Name)
                {
                    this.Settings[i] = sS;
                    break;
                }
            }

            if (i == this.Settings.Count)
            {
                this.Settings.Add(sS);
            }

            this.Settings.Sort(delegate (SavedSettingV3 sS1, SavedSettingV3 sS2) { return sS1.Name.CompareTo(sS2.Name); });
        }

        public void DeleteState(string name)
        {
            foreach (SavedSettingV3 sS in this.Settings)
            {
                if (sS.Name == name)
                {
                    this.Settings.Remove(sS);
                    break;
                }
            }
        }

        public void DeleteState(SavedSettingV3 sS)
        {
            try
            {
                if (this.Settings.Count > 0)
                {
                    this.Settings.Remove(sS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Deleting Saved State", ex);
            }
        }

    }

    [Serializable()]

    public class SavedSettingsV4 : ISerializable
    {
        public SavedSettingsV4()
        {
        }

        public List<SavedSettingV4> Settings = new List<SavedSettingV4>();

        public SavedSettingsV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Settings = (List<SavedSettingV4>)info.GetValue("Settings", typeof(List<SavedSettingV4>));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Saved Settings.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Settings", this.Settings);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Saved Settings Get Object Data", ex);
            }
        }

        public void AddState(SavedSettingV4 sS)
        {
            int i = 0;
            for (i = 0; i < this.Settings.Count; ++i)
            {
                if (this.Settings[i].Name == sS.Name)
                {
                    this.Settings[i] = sS;
                    break;
                }
            }

            if (i == this.Settings.Count)
            {
                this.Settings.Add(sS);
            }

            this.Settings.Sort(delegate (SavedSettingV4 sS1, SavedSettingV4 sS2) { return sS1.Name.CompareTo(sS2.Name); });
        }

        public void DeleteState(string name)
        {
            foreach (SavedSettingV4 sS in this.Settings)
            {
                if (sS.Name == name)
                {
                    this.Settings.Remove(sS);
                    break;
                }
            }
        }

        public void DeleteState(SavedSettingV4 sS)
        {
            try
            {
                if (this.Settings.Count > 0)
                {
                    this.Settings.Remove(sS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Deleting Saved State", ex);
            }
        }

    }

    [Serializable()]
    public class SavedSettingv2 : ISerializable
    {
        public SavedSettingv2()
        {
        }

        public string Name;
        public TripModeSavedState TripSettings = new TripModeSavedState();
        public CloseModeSaveState CloseSettings = new CloseModeSaveState();
        public PumpModeSavedState PumpSettings = new PumpModeSavedState();
        public int CTRatio;
        public int RelayType;
        public int Phasing;

        public SavedSettingv2(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripSettings = (TripModeSavedState)info.GetValue("Trip Settings", typeof(TripModeSavedState));
                this.CloseSettings = (CloseModeSaveState)info.GetValue("Close Settings", typeof(CloseModeSaveState));
                this.PumpSettings = (PumpModeSavedState)info.GetValue("Pump Settings", typeof(PumpModeSavedState));
                this.CTRatio = (UInt16)info.GetValue("CTRatio", typeof(UInt16));
                this.RelayType = (char)info.GetValue("Relay Type", typeof(char));
                this.Phasing = (UInt16)info.GetValue("Phasing", typeof(UInt16));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Single Saved Setting v2.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", this.Name);
                info.AddValue("Trip Settings", this.TripSettings);
                info.AddValue("Close Settings", this.CloseSettings);
                info.AddValue("Pump Settings", this.PumpSettings);
                info.AddValue("CTRatio", this.CTRatio);
                info.AddValue("Relay Type", this.RelayType);
                info.AddValue("Phasing", this.Phasing);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Sing Saved Setting Get Object Data", ex);
            }
        }
    }

    [Serializable()]
    public class SavedSettingV3 : ISerializable
    {
        public SavedSettingV3()
        {
        }

        public string Name;
        public TripModeSavedState TripSettings = new TripModeSavedState();
        public CloseModeSaveState CloseSettings = new CloseModeSaveState();
        public PumpModeSavedStateV2 PumpSettings = new PumpModeSavedStateV2();
        public int CTRatio;
        public int RelayType;
        public int Phasing;

        public SavedSettingV3(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripSettings = (TripModeSavedState)info.GetValue("Trip Settings", typeof(TripModeSavedState));
                this.CloseSettings = (CloseModeSaveState)info.GetValue("Close Settings", typeof(CloseModeSaveState));
                this.PumpSettings = (PumpModeSavedStateV2)info.GetValue("Pump Settings", typeof(PumpModeSavedStateV2));
                this.CTRatio = (UInt16)info.GetValue("CTRatio", typeof(UInt16));
                this.RelayType = (char)info.GetValue("Relay Type", typeof(char));
                this.Phasing = (UInt16)info.GetValue("Phasing", typeof(UInt16));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Single Saved Setting V3.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", this.Name);
                info.AddValue("Trip Settings", this.TripSettings);
                info.AddValue("Close Settings", this.CloseSettings);
                info.AddValue("Pump Settings", this.PumpSettings);
                info.AddValue("CTRatio", this.CTRatio);
                info.AddValue("Relay Type", this.RelayType);
                info.AddValue("Phasing", this.Phasing);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Sing Saved Setting Get Object Data", ex);
            }
        }
    }

    [Serializable()]
    public class SavedSettingV4 : ISerializable
    {
        public SavedSettingV4()
        {
        }

        public string Name;
        public TripModeSavedStateV4 TripSettings = new TripModeSavedStateV4();
        public CloseModeSaveStateV4 CloseSettings = new CloseModeSaveStateV4(); //was "v1" (no version#)
        public PumpModeSavedStateV2 PumpSettings = new PumpModeSavedStateV2();
        public int CTRatio;
        public int RelayType;
        public int Phasing;
        [OptionalField]
        public SafeServiceSavedState SafeServiceSettings = new SafeServiceSavedState();
#if DNP
        [OptionalField]
        public DNPSaveStateV4 DNPSettings = new DNPSaveStateV4();
#endif
        [OptionalField]
        public bool V277Protector;

        [OptionalField]
        public bool ProtectorVoltageOutputs;

        [OptionalField]
        public bool V600Protector;

        public SavedSettingV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripSettings = (TripModeSavedStateV4)info.GetValue("Trip Settings", typeof(TripModeSavedStateV4));
                this.CloseSettings = (CloseModeSaveStateV4)info.GetValue("Close Settings", typeof(CloseModeSaveStateV4));
                this.PumpSettings = (PumpModeSavedStateV2)info.GetValue("Pump Settings", typeof(PumpModeSavedStateV2));

#if DNP
                try
                {
                    this.DNPSettings = (DNPSaveStateV4)info.GetValue("DNP Settings", typeof(DNPSaveStateV4));
                }
                catch
                {
                    this.DNPSettings = new DNPSaveStateV4();
                }
#endif

                try
                {
                    this.SafeServiceSettings = (SafeServiceSavedState)info.GetValue("Safe Service Settings", typeof(SafeServiceSavedState));
                }
                catch
                {
                    this.SafeServiceSettings = new SafeServiceSavedState();
                }
                this.CTRatio = (UInt16)info.GetValue("CTRatio", typeof(UInt16));
                this.RelayType = (int)info.GetValue("Relay Type", typeof(int));
                this.Phasing = (int)info.GetValue("Phasing", typeof(int));
                try
                {
                    V277Protector = (bool)info.GetValue("V277Protector", typeof(bool));
                }
                catch
                {

                    V277Protector = false;
                }

                try
                {
                    ProtectorVoltageOutputs = true;
                }
                catch
                {

                    V277Protector = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Single Saved Setting V4.", ex);
            }
        }


        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", this.Name);
                info.AddValue("Trip Settings", this.TripSettings);
                info.AddValue("Close Settings", this.CloseSettings);
                info.AddValue("Pump Settings", this.PumpSettings);
#if DNP
                info.AddValue("DNP Settings", this.DNPSettings);
#endif
                info.AddValue("CTRatio", this.CTRatio);
                info.AddValue("Relay Type", this.RelayType);
                info.AddValue("Phasing", this.Phasing);
                info.AddValue("Safe Service Settings", this.SafeServiceSettings);
                info.AddValue("V277Protector", V277Protector);
                info.AddValue("V277Outputs", ProtectorVoltageOutputs);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Sing Saved Setting Get Object Data", ex);
            }
        }
    }
}