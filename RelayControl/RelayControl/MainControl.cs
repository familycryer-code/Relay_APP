using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections;
using PhasorDisplayGraph;
using RelayControlLibrary;
using SineDisplayGraph;
using System.IO.Ports;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using Microsoft.Win32;
using MyFileIO;
using SavedSettings;
using SharedResources;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using NLog;

namespace RelayControl
{

    public partial class MainControl : Form
    {
        private const int REV0_MASTER_REVISION = 100713;
        private const int REV1_MASTER_REVISION = 100713; //TEST might not need
        private const int SafeService_MASTER_REVISION = 160621;
        private string customerRevisionName = "";
        private UInt32 relayCodeRevisionNumber;
        private uint externalFileRevisionNumber;                //this will be read from the file to see what revision the program is currently working with.
        private const uint _version4FileRevisionNumber = 20110921;//20110610;            //update only when save data changes
        private const uint _version3FileRevisionNumber = 20100621;

        private RelayStatusRegister RelayStatus = new RelayStatusRegister();
        private RelayFlagsRegister RelayFlags = new RelayFlagsRegister();
        private delegate void booleanInvoke(bool b);
        private bool showCrossPhaseMsgOnce = false;
        private bool initializeAutoLoad = true;
        private bool showMemFixMsg = true;

        public const string SavedDataPath = @"C:\DGI Systems\Relay\Saved Data\";
        private bool quietMode = false;  //turns off register polling - button for this
        private bool checkedDNPEnable = false;

        private Customers customer = Customers.None;

        private bool tCPConnection = false;
        private TCPComms tcpClient;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                if (this.customer != value)
                {

                    this.customer = value;
                    this.ucPumpMode1.Customer = this.customer;
                    this.ucTripMode2.Customer = this.customer;
                    this.ucCloseMode1.Customer = this.customer;
                    this.ucTransmitter1.Customer = this.customer;
                    this.ucDNP1.Customer = this.customer;
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

#if ATLANTA
                    this.ucRelayProgramming1.Customer = Customers.Atlanta;
#elif ONCOR
                    this.ucRelayProgramming1.Customer = Customers.Oncor;
#else
                    this.ucRelayProgramming1.Customer = this.customer;
#endif

                    if (this.dNPDIGITALGRIDData != null)
                        this.dNPDIGITALGRIDData.Customer = this.customer;

                    if (this.customer == Customers.ConEdison) { }
                    //this.makeConEdisonGUI();
                    else if (this.customer == Customers.Memphis)
                        this.makeMemphisGUI();
                }
            }
        }

        private ToolTip toolTip = new ToolTip();
        private bool transmitterEnabled = false;
        private bool TransmitterEnabled
        {
            get { return this.transmitterEnabled; }
            set
            {
#if !DEBUG
                if (value && !this.DNPEnabled && this.Customer != Customers.ConEdison)
                {
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitter))
                    {
                        this.tabControlMain.TabPages.Add(this.tabPageTransmitter);
                        this.tabControlMain.TabPages.Add(this.tabPageTransmitterMonitoring);
                    }
                }
                else
                {
//#if ((!PLC && DNP) && !ONCOR)
#if ((!PLC && DNP) && !ONCOR && !TORONTO_HYDRO)
                    if (this.tabControlMain.TabPages.Contains(this.tabPageTransmitter))
                    {
                        this.tabControlMain.TabPages.Remove(this.tabPageTransmitter);
                        this.tabControlMain.TabPages.Remove(this.tabPageTransmitterMonitoring);
                    }
#endif
                }
#endif
                this.transmitterEnabled = value;
                this.ucRelayProgramming1.TransmitterEnabled = value;
            }
        }
        private bool blockDNPEnableFromTransmitterSavedVal = false;
        private bool blockDNPEnableFromTransmitterSettings
        {
            get { return this.blockDNPEnableFromTransmitterSavedVal; }
            set
            {
                this.blockDNPEnableFromTransmitterSavedVal = value;
            }
        }
        private bool dNPEnabledSavedVal = false;
        private bool DNPEnabled
        {
            get { return this.dNPEnabledSavedVal; }
            set
            {
                if (value == true && this.masterRevision > REV0_MASTER_REVISION)
                {
#if !WATERBUG
                    if (this.Customer == Customers.SMUD)
                        this.TransmitterEnabled = false;
                    
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageDNP))
                    {
//#if !LONDONH
#if !LONDONH && !DIGITALGRID && !DOMINION
                        this.tabControlMain.TabPages.Add(this.tabPageDNP);
                        this.tabControlMain.TabPages.Add(this.tabPageDNPData);                      
#endif
                    }
                    if (this.customer != Customers.Memphis) 
                    {
                        if (this.tabPageDNPData.Controls.Contains(this.dNPMemphisData))
                        {
                            this.tabPageDNPData.Controls.Remove(this.dNPMemphisData);
                            this.dNPMemphisData.Dispose();
                        }
                        // if ((this.customer == Customers.DIGITALGRIDDNP || this.customer == Customers.DNPwithPLC || this.Customer == Customers.DIGITALGRID || this.Customer == Customers.Atlanta || this.Customer == Customers.Oncor) && !this.tabPageDNPData.Controls.Contains(this.dNPDIGITALGRIDData))
                        // if ((this.customer == Customers.DIGITALGRIDDNP || this.customer == Customers.DNPwithPLC || this.Customer == Customers.DIGITALGRID || this.Customer == Customers.Atlanta || this.Customer == Customers.ConEdison || this.Customer == Customers.Oncor) && !this.tabPageDNPData.Controls.Contains(this.dNPDIGITALGRIDData))
                        if ((this.customer == Customers.DIGITALGRIDDNP || this.customer == Customers.DNPwithPLC || this.Customer == Customers.DIGITALGRID || this.Customer == Customers.Atlanta || this.Customer == Customers.ConEdison || this.Customer == Customers.SCE || this.Customer == Customers.Oncor || this.Customer == Customers.TorontoHydro) && !this.tabPageDNPData.Controls.Contains(this.dNPDIGITALGRIDData))                        
                        {
                            setDNPTabPoints();
                        }

#if ENMAX && !DNP
                        if(this.receivedMasterRevision.Contains("DNP"))
                        {
                            setDNPTabPoints();
                        }
#endif
                        if (!this.tabControlMain.TabPages.Contains(this.tabPageDNPSecureAuth))
                        {
//#if !LONDONH && !DIGITALGRID
#if !LONDONH && !DIGITALGRID && !DOMINION
                            this.tabControlMain.TabPages.Add(this.tabPageDNPSecureAuth);
#endif
                        }
                    }
                    else // Memphis style or London Hydro
                    {
                        if (this.tabControlMain.TabPages.Contains(this.tabPageDNPSecureAuth))
                            this.tabControlMain.TabPages.Remove(this.tabPageDNPSecureAuth);
                    }
                    this.dNPEnabledSavedVal = value;
                    this.ucRelayProgramming1.DNPRelay = value;
#endif
                    }
                    else
                {
                    if (this.tabControlMain.TabPages.Contains(this.tabPageDNPSecureAuth))
                        this.tabControlMain.TabPages.Remove(this.tabPageDNPSecureAuth);
                    if (this.tabControlMain.TabPages.Contains(this.tabPageDNP))
                    {
                        this.tabControlMain.TabPages.Remove(this.tabPageDNP);
                        this.tabControlMain.TabPages.Remove(this.tabPageDNPData);
                    }
                    this.dNPEnabledSavedVal = false;
                    this.ucRelayProgramming1.DNPRelay = false;
                }

            }

        }

        private void setDNPTabPoints()
        {
            //#if (!DIGITALGRID || ONCOR)
#if ((!DIGITALGRID || ONCOR) && !DOMINION)
            this.dNPDIGITALGRIDData = new ucDNPDIGITALGRIDData(this.customer);
            this.tabPageDNPData.Controls.Add(this.dNPDIGITALGRIDData);
            this.dNPDIGITALGRIDData.RelayMasterRevision = (UInt32)masterRevision;
            this.dNPDIGITALGRIDData.Location = new Point(0, 0);
            this.dNPDIGITALGRIDData.Send += standardizedSendData;
            this.dNPDIGITALGRIDData.PointChanged += DNPDigitalGridData_PointChanged;
            this.dNPDIGITALGRIDData.Show();
#elif (DIGITALGRID && !ONCOR) || DOMINION
            if (this.tabControlMain.TabPages.Contains(this.tabPageDNPSecureAuth))
                this.tabControlMain.TabPages.Remove(this.tabPageDNPSecureAuth);
            if (this.tabControlMain.TabPages.Contains(this.tabPageDNP))
            {
                this.tabControlMain.TabPages.Remove(this.tabPageDNP);
                this.tabControlMain.TabPages.Remove(this.tabPageDNPData);
            }
            this.dNPEnabledSavedVal = false;
            this.ucRelayProgramming1.DNPRelay = false;
#endif
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
            // Get the version number
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            logger.Info("Version number: {0}", version);
            // Officially start everything
            this.MainControlInit();
//#if !DEBUG
//            tCPConnectionToolStripMenuItem.Visible = false;
#if DNP
            tCPConnectionToolStripMenuItem.Visible = true;
#endif
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
#if DEBUG
                    this.initializeFromConfigFileDebug();
                    labelConEdPowerRelay.Visible = false;
#endif
                this.initializeStatusFlags();
                SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
                this.labelQuietMode.Visible = false;
                this.initializeEventPage();
                if (!Directory.Exists(SavedDataPath))  //Create the Save Data path if it does not already exist
                {
                    Directory.CreateDirectory(SavedDataPath);
                }
                //CT Ratio on PQ monitor
                this.textBoxCTRatioPQMonitor.Text = textBoxCTRatio.Text;
                this.buttonUpdateCTRatio.Visible = false;

                this.savedSaveFileComboBoxWidth = this.comboBoxSavedStates.Width;
                #if !SeattleTest
                    this.initializeExternalFileRevisionNumber(); //Get the saved data version
                    this.initializeSaveObject();            //Check the save data to see
                #endif

                #if PSEG
                    tCPConnectionToolStripMenuItem.Visible = true;
                    #if DNP
                        #if !DEBUG
                            domainUpDownRelayType.Visible = false;
                            labelConEdPowerRelay.Visible = true;
                        #else
                            labelConEdPowerRelay.Visible = false;
                            domainUpDownRelayType.Visible = true;
                        #endif
                    #endif
                #endif

                #if ATLANTA
                    this.groupBoxLowVoltThres.Visible = true;
                #else
                    this.groupBoxLowVoltThres.Visible = false;
#endif
                //#if LONDONH
#if (!DIGITALGRID || DIGITALGRIDDNP)
                     this.tabControlMain.TabPages.Remove(this.tabPageShortRange);
#endif

                /* #if (DIGITALGRID && !ONCOR)
                     this.ucShortRange1.Enabled = true;
                     this.ucShortRange1.Visible = true;
                     this.tabControlMain.TabPages.Add(this.tabPageShortRange);
                 #endif
                */
#if (ONCOR || TORONTO_HYDRO)
                this.ucShortRange1.Enabled = false;
                this.ucShortRange1.Visible = false;
                this.tabControlMain.TabPages.Remove(this.tabPageShortRange);
                this.loadConfigurationToolStripMenuItem.Visible = false;
                this.enableAutoloadToolStripMenuItem.Checked = true;
#endif
#if PSEG
                
                    this.ucShortRange1.Enabled = false;
                    this.ucShortRange1.Visible = false;
                    this.tabControlMain.TabPages.Remove(this.tabPageShortRange);
#endif
                statusNew.flagFromRelay = false;
                this.timerLiveEventAcknowledge.Interval = 250;
                this.timerLiveEventAcknowledge.SynchronizingObject = this;
                this.timerLiveEventAcknowledge.Elapsed += new System.Timers.ElapsedEventHandler(timerLiveEventAcknowledge_Tick);
                this.ucCloseMode1.Send += standardizedSendData;
                this.ucTripMode2.Send += standardizedSendData;
                this.ucCalibration1.Send += standardizedSendData;
                this.ucPumpMode1.Send += standardizedSendData;
                this.ucTransmitter1.Send += new ucTransmitter.SendEventHandler(ucTransmitter1_Send);
                this.ucDNP1.Send += new ucDNP.SendEventHandler(ucDNP1_Send);
                this.ucShortRange1.Send += standardizedSendData;
                this.ucTimeControl1.SendData += standardizedSendData;
                this.ucSafeService1.Send += standardizedSendData;
                this.ucRelayProgramming1.Send += new ucRelayProgramming.SendDelegate(Programming_Send);
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
                this.ucDNP1.DNPControlException += this.standardExceptionMessage;
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
                #if DG288_TESTFIXTURE_GUI
                    if (this.tabControlMain.TabPages.Contains(this.tabPageDNPSecureAuth))
                        this.tabControlMain.TabPages.Remove(this.tabPageDNPSecureAuth);
                    if (this.tabControlMain.TabPages.Contains(this.tabPageDNP))
                        this.tabControlMain.TabPages.Remove(this.tabPageDNP);
                    if(this.tabControlMain.TabPages.Contains(this.tabPageDNPData))
                        this.tabControlMain.TabPages.Remove(this.tabPageDNPData);
                    if (this.tabControlMain.TabPages.Contains(this.tabPageTransmitter))
                        this.tabControlMain.TabPages.Remove(this.tabPageTransmitter);
                    if (this.tabControlMain.TabPages.Contains(this.tabPageEvents))
                        this.tabControlMain.TabPages.Remove(this.tabPageEvents);
                    if (this.tabControlMain.TabPages.Contains(this.tabPageFlightRecorder))
                        this.tabControlMain.TabPages.Remove(this.tabPageFlightRecorder);
                    if (this.tabControlMain.TabPages.Contains(this.tabPageMonitor))
                        this.tabControlMain.TabPages.Remove(this.tabPageMonitor);
                    if (this.tabControlMain.TabPages.Contains(this.tabPageEngineering))
                        this.tabControlMain.TabPages.Remove(this.tabPageEngineering);
                    if (this.tabControlMain.TabPages.Contains(this.tabPageArcFault))
                        this.tabControlMain.TabPages.Remove(this.tabPageArcFault);
                    if (this.tabControlMain.TabPages.Contains(this.tabPageShortRange))
                        this.tabControlMain.TabPages.Remove(this.tabPageShortRange);
                    if (tabControlMain.TabPages.Contains(tabPageEngineering2))
                        tabControlMain.TabPages.Remove(tabPageEngineering2);

                    this.dNPEnabledSavedVal = false;
                    this.ucRelayProgramming1.DNPRelay = false;

                    this.ucTripMode2.Visible = false;
                    this.ucCloseMode1.Visible = false;
                    this.groupBoxNetworkCTRatio.Visible = false;
                    this.panelOtherRelayControls.Visible = false;
                    this.ucPumpMode1.Visible = false;
                    this.groupBoxLowVoltThres.Visible = false;
                    this.groupBoxPhasingAndType.Visible = false;
                    this.checkBox277Protector.Visible = false;
                    this.checkBox277Protector.Checked = false;
                    this.checkBox277DNPOutputs.Visible = false;
                    this.groupBoxLRLockoutMain.Visible = false;
                    this.groupBoxRelayFlags.Visible = false;
                    this.groupBoxRelayStatus.Visible = false;
                    checkBoxReprogramBootAuto.Visible = false;

                    this.tabPageControl.Text = "Safe Service";

                    this.ucSafeService1.Location = new Point(tabPageControl.Width / 3, tabPageControl.Height / 4);
                

                    this.tabPageTransmitterMonitoring.Refresh();
                    this.toolStripMenuItemAction.Visible = false;
                    this.acknowledgeToolStripMenuItem1.Visible = false;
                    this.toolsToolStripMenuItem.Visible = false;
                    this.enableAutoloadToolStripMenuItem.Checked = false;
                #endif

                #if (DOMINION && !DEBUG) || (ENMAX && !DEBUG) || (BGE && !DEBUG)
                    this.loadConfigurationToolStripMenuItem.Visible = false;
                    this.enableAutoloadToolStripMenuItem.Checked = true;
                #endif
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
                this.messageHandler("Error Setting Port Menu", ex);
            }
            try
            {
                this.timerCheckPortTime.Interval = 500;
                this.eventActionsToolStripMenuItem.Enabled = false;
                this.liveDataActionsToolStripMenuItem.Enabled = false;


                #if SeattleTest
                    this.noMonitoringVersion = true;
                    this.tabControlMain.TabPages.Remove(this.tabPageArcFault);
                    this.tabControlMain.TabPages.Remove(this.tabPageControl);
                    this.tabControlMain.TabPages.Remove(this.tabPageDNP);
                    this.tabControlMain.TabPages.Remove(this.tabPageEngineering);
                    //this.tabControlMain.TabPages.Remove(this.tabPageEvents);
                    this.tabControlMain.TabPages.Remove(this.tabPageFlightRecorder);
                    this.tabControlMain.TabPages.Remove(this.tabPageMonitor);
                    this.tabControlMain.TabPages.Remove(this.tabPageShortRange);
                    this.tabControlMain.TabPages.Remove(this.tabPageTransmitter);
                    this.tabControlMain.TabPages.Remove(this.tabPageTransmitterMonitoring);
                    this.eventActionsToolStripMenuItem.Enabled = true;
                    this.sToolStripMenuItem.Enabled = false;
                    this.OptionsToolStripMenuItem.Enabled = false;
                    this.acknowledgeToolStripMenuItem1.Visible = false;
                    this.buttonRQEventData.Enabled = false;
                    this.buttonClearEvents.Enabled = false;
                    this.downloadEventFromRelayToolStripMenuItem.Visible = false;
                    this.clearEventsToolStripMenuItem.Visible = false;
                    this.saveEventsToolStripMenuItem.Visible = false;
                    this.Text = "DIGITALGRID, INC. - Relay Control Seattle Test Program" + Properties.Resources._RevisionDate;// 2011-10-28";
                    this.toolStripStatusLabelMain.Text = "";
                    this.searchForRelay = false;
                #elif DEBUG
                    this.setCustomersRevisionName();

                    this.noMonitoringVersion = false;
                    this.buttonForceI.Visible = true;
                    this.ucCalibration1.Visible = true;
                    this.buttonUpdateDisplay.Visible = true;
                    this.enableAll(true);
                    this.tabPageFlightRecorder.Show();
                    this.tabPageEvents.Show();
                    #if !DG288_TESTFIXTURE_GUI
                                    //this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + Properties.Resources._RevisionDate + " - " + customerRevisionName + " - Version: " + Assembly.GetEntryAssembly().GetName().Version + " Debug";
                                    this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.3.0.1" + "Debug";
                    #elif DG288_TESTFIXTURE_GUI
                                    this.Text = "DIGITALGRID, INC. - Transmitter Monitoring " + Properties.Resources._RevisionDate + " - " + customerRevisionName;
                    #endif
                    this.ArcFaultEnabled = true;
                    this.Customer = Customers.DIGITALGRID;

                #elif WATERBUG
                    this.noMonitoringVersion = false;
                    this.buttonForceI.Visible = true;
                    this.tabControlMain.TabPages.Clear();
                    this.tabControlMain.TabPages.Add(this.tabPageTransmitterMonitoring);
                    this.groupBoxGeneralSettings.Visible = false;
                    this.groupBoxAdvancedReadings.Visible = false;
                    this.groupBoxCurrentReadings.Visible = false;
                    this.groupBoxGeneralSettings.Visible = false;
                    this.groupBoxPowerDirectionalFlow.Visible = false;
                    this.groupBoxRelayProgramming.Visible = false;
                    this.groupBoxVoltageReadings.Visible = false;
                    this.groupBoxAmpCalibration.Visible = false;
                    this.groupBox17.Visible = false;
                    this.DNPEnabled = false;
                    this.toolStripMenuItemAction.Visible = false;
                    this.sToolStripMenuItem.Visible = false;
                    this.acknowledgeToolStripMenuItem1.Visible = false;
                    this.Text = "DIGITALGRID, INC. - Waterbury Testing Software " + Properties.Resources._RevisionDate;// 2011-05-16";

                    this.listBoxA1SensorSelect.SelectedItem = "DGI Temperature";
                    this.listBoxA2SensorSelect.SelectedItem = "DGI Temperature";
                    this.listBoxA1SensorSelect.Enabled = false;
                    this.listBoxA2SensorSelect.Enabled = false;
                    this.checkBoxFlagStatusA.Visible = false;
                    this.checkBoxFlagStatusB.Visible = false;
                    this.tabPageTransmitterMonitoring.Text = "Monitoring";
                    this.findRelayToolStripMenuItem.Text = "Find Test Set";
                    this.groupBoxVaultMonitoringCommands.Text = "Monitoring Commands";
                    this.enableAllToolStripMenuItem.Visible = false;
                                this.ArcFaultEnabled = false;
                #else
                this.setCustomersRevisionName();
                this.noMonitoringVersion = false;
                this.pauseMonitoring = false;
                this.ucCalibration1.Visible = true;// false;
                this.buttonUpdateDisplay.Visible = false;
                this.enableAll(false);
                this.tabControlMain.TabPages.Remove(this.tabPageEngineering2);
                this.labelCtRatioMonitor.Visible = true;
                this.buttonForceI.Visible = false; 
                this.buttonUpdateCTRatio.Visible = false;
                this.buttonRequestRelayRegisters.Visible = false;
                this.buttonResetMaster.Visible = false;
                this.buttonRQRelayProcVersion.Visible = false;
                this.buttonUpdateDisplay.Visible = false;
                this.groupBoxRelayFlags.Visible = false;
                this.enableAllToolStripMenuItem.Visible = true;
#if LONDONH
                                this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.3.52.4" + " LONDON HYDRO ";
                                this.Customer = Customers.LondonH;
#elif CONED
                         this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.3.1.2" + " CONED ";
#elif SCE
                         this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.4.53.0" + " Southern California Edison ";
                         this.Customer = Customers.SCE;
#elif PSEG
                         this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.4.0.1" + " PSE&G ";
#elif ENMAX
                         this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.4.0.1" + " ENMAX ";
#elif ONCOR
                         this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.4.1.0" + " ONCOR ";
#elif DIGITALGRID
                this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.4.0.1" + " DigtalGrid Production Engineering ";
#elif TORONTO_HYDRO
                this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.4.1.0" + " TORONTOHYDRO ";
                this.Customer = Customers.TorontoHydro;
#elif DOMINION
                         this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.4.1.0" + " DOMINION ";
#else
                this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.3.1.2" + " ONCOR ";
#endif
                // this.Text = "DIGITALGRID, INC. - Relay Control and Monitoring " + " - Version: " + "3.3.0.1" + this.Customer;
                this.acknowledgeToolStripMenuItem1.Visible = false;
                this.checkBoxBlockedCloseFlag.Visible = false;
                this.checkBoxCalibrating.Visible = false;
                this.checkBoxInInsensRegion.Visible = false;
                #if DNP && !ENMAX
                    // this.TransmitterEnabled = false;
                    this.TransmitterEnabled = true;
                #else
                    TransmitterEnabled = true;
                #endif

                this.ArcFaultEnabled = false;
#if NU
                    checkBox277DNPOutputs.Visible = false;
#endif
#if CONED
                this.Customer = Customers.ConEdison;
                ucRemoteCommandBlock1.Visible = false;
                this.ucRemoteCommandBlock1.Visible = false;
                if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitter))
                    this.tabControlMain.TabPages.Add(this.tabPageTransmitter);
                if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitterMonitoring))
                    this.tabControlMain.TabPages.Add(this.tabPageTransmitterMonitoring);
#elif SCE
                this.Customer = Customers.SCE;
                ucRemoteCommandBlock1.Visible = false;
                this.ucRemoteCommandBlock1.Visible = false;
                if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitter))
                    this.tabControlMain.TabPages.Add(this.tabPageTransmitter);
                if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitterMonitoring))
                    this.tabControlMain.TabPages.Add(this.tabPageTransmitterMonitoring); 
#elif TORONTO_HYDRO
                this.Customer = Customers.TorontoHydro;
                ucRemoteCommandBlock1.Visible = false;
                this.ucRemoteCommandBlock1.Visible = false;
                if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitter))
                    this.tabControlMain.TabPages.Add(this.tabPageTransmitter);
                if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitterMonitoring))
                    this.tabControlMain.TabPages.Add(this.tabPageTransmitterMonitoring);
#elif MEMPHIS
                this.Customer = Customers.Memphis;

#elif GERELAY
                            this.Customer = Customers.NonConEdGE;
                            this.DNPEnabled = false;
                            this.enableAllToolStripMenuItem.Visible = true;
                            this.TransmitterEnabled = true;
#elif DNP
                this.Customer = Customers.DIGITALGRIDDNP;
                            this.DNPEnabled = true;
                #if ATLANTA //|| ONCOR
                               ucRemoteCommandBlock1.Visible = false;
                                this.ucRemoteCommandBlock1.Visible = false;
                                if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitter))
                                    this.tabControlMain.TabPages.Add(this.tabPageTransmitter);
                                if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitterMonitoring))
                                    this.tabControlMain.TabPages.Add(this.tabPageTransmitterMonitoring);
                                if (!this.tabControlMain.TabPages.Contains(this.tabPageShortRange))
                                    this.tabControlMain.TabPages.Add(this.tabPageShortRange);
                #endif
            #else
                            this.Customer = Customers.NonConEd;
                            this.DNPEnabled = false;
                            this.TransmitterEnabled = true;
                            this.enableAllToolStripMenuItem.Visible = true;
            #endif
    #endif
                this.enableAllToolStripMenuItem.Visible = true;

#if DNP
#if DEBUG
                    if (this.tabControlMain.TabPages.Contains(this.tabPageDNPSecureAuth))
                        this.tabControlMain.TabPages.Remove(this.tabPageDNPSecureAuth);
                    if (this.tabControlMain.TabPages.Contains(this.tabPageDNP))
                        this.tabControlMain.TabPages.Remove(this.tabPageDNP);
                    if (this.tabControlMain.TabPages.Contains(this.tabPageDNPData))
                        this.tabControlMain.TabPages.Remove(this.tabPageDNPData);

#endif
//#if (!DIGITALGRID || DIGITALGRIDDNP)
#if (!DIGITALGRID || DIGITALGRIDDNP || ONCOR || TORONTO_HYDRO)
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageDNP))
                        this.tabControlMain.TabPages.Add(this.tabPageDNP);
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageDNPData))
                        this.tabControlMain.TabPages.Add(this.tabPageDNPData);
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageDNPSecureAuth))
                        this.tabControlMain.TabPages.Add(this.tabPageDNPSecureAuth);
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitter))
                        this.tabControlMain.TabPages.Add(this.tabPageTransmitter);
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageTransmitterMonitoring))
                        this.tabControlMain.TabPages.Add(this.tabPageTransmitterMonitoring);
#endif
#if (DIGITALGRID && !ONCOR)
    //#if (DIGITALGRIDDNP)
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageShortRange)) // Secondary Monitoring tab
                        this.tabControlMain.TabPages.Add(this.tabPageShortRange);
#endif

#else
                checkBox277DNPOutputs.Visible = false;
#endif

#if DEBUG || CHICAGO || LONDONH
                this.toolStripStatusLabelReceiverStatus.Visible = true;
#endif
                this.ucDNP1.DNPLabelStatus = ucTransmitter1.CheckDNPEnable;
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

        private void setCustomersRevisionName()
        {
#if NU
#if NUCREW
            this.customerRevisionName = "Eversource Crew";
#else
            this.customerRevisionName = "Eversource Engineering";
#endif
#elif BGE
            customerRevisionName = "BGE";
#elif BOSTON
            this.customerRevisionName = "Boston Eversource";
#elif SEATTLE
            this.customerRevisionName = "Seattle";
#elif DOMINION
            this.customerRevisionName = "Dominion";
#elif CHICAGO
            this.customerRevisionName = "Chicago";
#elif LONDONH
            this.customerRevisionName = "London Hydro";
#elif TAUNTON
            this.customerRevisionName = "Taunton";
#elif ENMAX && !DNP
            this.customerRevisionName = "Enmax PLC";
#elif ENMAX && DNP
            this.customerRevisionName = "Enmax DNP and PLC";
#elif MADISON
            this.customerRevisionName = "Madison";
#elif ATLANTA
            this.customerRevisionName = "Atlanta";
#elif ONCOR
            this.customerRevisionName = "Oncor";
#elif TORONTO_HYDRO
            this.customerRevisionName = "Toronto Hydro";
#elif DG288_TESTFIXTURE_GUI
            this.customerRevisionName = "DG-288 TestFixture";
#elif SMUD
            this.customerRevisionName = "SMUD";
#elif PSEG && DNP
            this.customerRevisionName = "PSEG with DNP";
#elif PSEG
            this.customerRevisionName = "PSEG";
#else
            this.customerRevisionName = "";
#endif
        }

        private void initializeToolTip()
        {
            this.toolTip.SetToolTip(this.comboBoxSavedStates, "Recall previously saved states");
            this.toolTip.SetToolTip(this.domainUpDownCTRatioM, "Set to match the CT Ratio of the protector");
            this.toolTip.SetToolTip(this.domainUpDownPhasings, "How to determine phasing of the protector");
            this.toolTip.SetToolTip(this.domainUpDownRelayType, "Changes Relay Algorithm");
            this.toolTip.SetToolTip(this.textBoxCTRatio, "Select 'Special' in above box to manually enter CT Ratio");
            this.toolTip.SetToolTip(this.textBoxSaveStateName, "Enter name to save current settings to file");
            this.toolTip.SetToolTip(this.comboBoxDNPVoltage, "Scales values on PQ Mon tab to protector voltage - now saved in relay, applies to DNP input values");
            this.toolTip.SetToolTip(this.checkBox277DNPOutputs, "Configures DNP outputs to scale to 277V");
            this.toolTip.SetToolTip(this.buttonClearCycleCount, "Reset Cycle Count to Zero");
            this.toolTip.SetToolTip(this.buttonDeleteSetting, "Remove the currently selected Saved State from the save file");
            this.toolTip.SetToolTip(this.buttonRequestRelayParamaters, "Download All Parameters to GUI");
            this.toolTip.SetToolTip(this.buttonResetBothProc, "Reset the Relay");
            this.toolTip.SetToolTip(this.buttonRSTRelay, "Reset the Relay");
            this.toolTip.SetToolTip(this.buttonSaveSetting, "Save the Current Settings to the file under the name in the Save Setting box");
            this.toolTip.SetToolTip(this.buttonSendAll, "Upload all visible settings to the relay");
            this.toolTip.SetToolTip(this.buttonTripRelay, "Send a Remote Trip to the relay");
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
                this.commFlags2.Add("Waterbug Active");
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

            logger.Trace("Sending Data to Relay: {0}", BitConverter.ToString(sEA.SendPacket.ToArray()));
            if (sEA.WithAck)
            {
                if (!this.sendAll)
                {
                    this.sendPacketAck(sEA.SendPacket, o.ToString());
                    if (sEA.RequestAll)
                    {
                        this.requestAllData();
                        this.parametersLoaded = true;
                    }
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

            this.tabControlMain.TabPages.Remove(this.tabPageDNP);
            this.tabControlMain.TabPages.Remove(this.tabPageDNPData);
            this.tabControlMain.TabPages.Remove(this.tabPageTransmitter);
            this.tabControlMain.TabPages.Remove(this.tabPageTransmitterMonitoring);
            this.domainUpDownPhasings.Visible = false;
            this.labelConEdPowerRelay.Visible = true;
            this.domainUpDownRelayType.Visible = false;
            this.buttonTypePhasingRestoreDefaults.Visible = false;
            this.buttonRelayType.Visible = false;
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

            this.domainUpDownRelayType.Visible = true;
            this.domainUpDownPhasings.Visible = true;
            this.labelConEdPowerRelay.Visible = false;
            this.buttonTypePhasingRestoreDefaults.Visible = true;
            this.buttonRelayType.Visible = true;
            this.addPumpProtect();

            this.checkBoxInTripRegion.Visible = true;


        }
        private ucMemphisDNPData dNPMemphisData;
        private ucDNPDIGITALGRIDData dNPDIGITALGRIDData;

        private void makeMemphisGUI()
        {
            this.Customer = Customers.Memphis;

            if (this.tabPageDNPData.Controls.Contains(this.dNPDIGITALGRIDData))
            {
                this.tabPageDNPData.Controls.Remove(this.dNPDIGITALGRIDData);
                this.dNPDIGITALGRIDData.Dispose();
            }
            if (!this.tabPageDNPData.Controls.Contains(this.dNPMemphisData))
            {
                this.dNPMemphisData = new ucMemphisDNPData();

                this.tabPageDNPData.Controls.Add(this.dNPMemphisData);
                this.dNPMemphisData.Send += standardizedSendData;
                this.dNPMemphisData.Location = new Point(0, 0);
                this.dNPMemphisData.Show();
            }
            if (this.tabControlMain.TabPages.Contains(this.tabPageDNPSecureAuth))
                this.tabControlMain.TabPages.Remove(this.tabPageDNPSecureAuth);
            this.makeNonConEdGUI();
            if (!this.Text.Contains("Memphis"))
                this.Text += " - Memphis";
        }

        private bool otherPanelMovedForConEd = false;
        private void removePumpProtect()
        {
            if (this.ucPumpMode1.Visible || !this.ucPumpMode1.Enabled)
            {
                this.ucPumpMode1.Visible = false;
                Point tempPoint = this.panelOtherRelayControls.Location;
                tempPoint.X -= this.ucPumpMode1.Width;
                this.panelOtherRelayControls.Location = tempPoint;
                this.otherPanelMovedForConEd = true;
            }
        }

        private void addPumpProtect()
        {
            if (!this.ucPumpMode1.Visible && this.otherPanelMovedForConEd)
            {
                this.otherPanelMovedForConEd = false;
                Point tempPoint = this.panelOtherRelayControls.Location;
                tempPoint.X += this.ucPumpMode1.Width;
                this.panelOtherRelayControls.Location = tempPoint;
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
            this.pictureBox_SendAll.Image = Properties.Resources.Throbber_SendAll;
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
#if SeattleTest
                    return;
#endif
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
                        this.domainUpDownCTRatioM_SelectedItemChanged(this.domainUpDownCTRatioM, new EventArgs()); //put this in to properly grey out CT Ratio box when necessary
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

        private SavedSettingV4 reprogrammingTempSettings = new SavedSettingV4();
        private bool loadingNewCode = false;
        private RelayProgrammingSendCommands currentReprogramState = RelayProgrammingSendCommands.RestartProgram;

        private void Programming_Send(object o, RelayProgrammingEventArgs rPEA)
        {
            this.currentReprogramState = rPEA.Command;

            logger.Trace("Programming Command: {0}", rPEA.Command);
            switch (rPEA.Command)
            {
                case RelayProgrammingSendCommands.RequestAll:
                    this.ProgramState = ProgramStates.DownloadingAllParameters;
                    loadingNewCode = false;
                    Thread.Sleep(6000);
                    clearRemoteBuffer();
                    Thread.Sleep(1000);
                    requestRelayRevision();
                    break;
                case RelayProgrammingSendCommands.RestartProgram:
                    this.quietMode = false;
                    this.toolStripStatusLabelRelayDisconnected.Visible = true;
                    break;
                case RelayProgrammingSendCommands.SaveSettings:
                    this.ucSafeService1.LoadingNewCode = true;
                    this.getAllSaveStates(this.reprogrammingTempSettings);
                    break;
                case RelayProgrammingSendCommands.TransmitterSettings:
                    this.ucTransmitter1.SetAllValues(rPEA.BytesToSend);
                    this.ucTransmitter1.SendTransmitterSettings();
                    if (DNPEnabled)
                        ucDNP1.SendAllDNPSettings();
                    this.ucDNP1.DNPLabelStatus = ucTransmitter1.CheckDNPEnable;
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
        }

        void ucTransmitter1_Send(SendEventArgs sEA)
        {
            if (sEA.SendPacket[0] == 0x66)
            {
                this.sendPacket(sEA.SendPacket);
                this.downloadProgress = new ProgressBarForm("Force Config Message", "Sending Configuration Messages. Please Wait.", 140, true);
                this.downloadProgress.Done += new ProgressBarForm.ProgressBarEvent(downloadProgress_Done);
                this.downloadProgress.ShowDialog();
            }
            else if (sEA.SendPacket[0] == (byte)'X')  //requestPacket
            {
                this.requestAllData();
            }
            else
            {
                this.sendPacketAck(sEA.SendPacket, "Transmitter Settings Send");

                if (!this.loadingNewCode)
                    this.requestAllData();

                this.parametersLoaded = true;
            }
        }

        void ucPumpMode1_Send(SendEventArgs sEA)
        {
            this.sendPacketAck(sEA.SendPacket, "Pump Mode Send");

            if (sEA.SendPacket[1] != 2 && !this.sendAll)
            {
                this.requestRelayRegisters();
                this.requestAllData();
                this.parametersLoaded = true;
            }
        }

        void ucDNP1_Send(SendEventArgs sEA)
        {
#if MEMPHIS
            this.sendPacket(sEA.SendPacket);
#else
            if (sEA.SendPacket[0] != 0x55)
                this.sendPacketAck(sEA.SendPacket, "DNP Control");
            else
                this.sendPacket(sEA.SendPacket);
#endif
            if (!this.sendAll && sEA.SendPacket[0] != 0x55)
            {
                requestRelayRegisters();
                this.requestAllData();
                this.parametersLoaded = true;
                this.ucTransmitter1.ForceDNPEnable = true;
                Thread.Sleep(100);
                this.ucTransmitter1.SendTransmitterSettings();
                this.ucDNP1.DNPLabelStatus = ucTransmitter1.CheckDNPEnable;

            }
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
                        this.SendConfirmed = true;
                        this.expectingAck = false;
                        packetAcknowledged(true);
                        this.timerSCITimeOut.Enabled = false;
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

                        initialRXPtr = tempRXReadPtr = this.rXReadPtr;
                        //check to see if we have found a command or we have reached the end of the data
                        command = this.getCommand(this.receiveArray[tempRXReadPtr]);
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
                this.messageHandler("Error Checking Raw Communication Data", ex);
                this.RegisterPolling(true);
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
                    return IncomingCommCommands.DNPMessage1;
                case 0x12:
                    return IncomingCommCommands.DNPMessage2;
                case 0x13:
                    return IncomingCommCommands.DNPMessage3;
                case 0x14:
                    return IncomingCommCommands.DNPMessage4;
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
                case IncomingCommCommands.DNPMessage1:
                    this.dNPDataMessage(bytePacket, 1);
                    break;
                case IncomingCommCommands.DNPMessage2:
                    this.dNPDataMessage(bytePacket, 2);
                    break;
                case IncomingCommCommands.DNPMessage3:
                    this.dNPDataMessage(bytePacket, 3);
                    break;
                case IncomingCommCommands.DNPMessage4:
                    this.dNPDataMessage(bytePacket, 4);
                    break;
                case IncomingCommCommands.GeneralCommand:
                    this.ucGeneralCommandHandler1.HandleCommand(bytePacket);
                    break;
                case IncomingCommCommands.ArcFaultData:
                    this.setArcFaultData(bytePacket);
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
                    break;
                case IncomingCommCommands.CurrentTime:
                    this.storeCurrentTime(bytePacket);
                    break;
                case IncomingCommCommands.SafeService:
                    this.ucSafeService1.SetAll(bytePacket);
                    if (this.ProgramState == ProgramStates.DownloadingAllParameters)
                    {
                        if (this.DNPEnabled && receivedMasterRevision.Contains("DNP") && !ucRelayProgramming1.ProgramBootCodeInProgress)
                            this.requestDNPSettings();
                        else
                        {
                            this.parametersFinishedLoading();
                        }
                    }
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
                        Thread.Sleep(1000);  // 2.5 seconds
                        this.setDNPSettings(bytePacket);
                    }
                    break;
                case IncomingCommCommands.DNPSAv5:
                    this.ucDNPSAv51.Message(bytePacket);
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
#if DEBUG
            this.relayStatusConverter.IncomingStatusCode = bytePacket[0];
            this.toolStripStatusLabelReceiverStatus.Text = this.relayStatusConverter.CurrentStatus;
            this.toolStripStatusLabelReceiverStatus.BackColor = this.relayStatusConverter.CurrentColor;
            this.toolStripStatusLabelReceiverStatus.ForeColor = this.relayStatusConverter.CurrentForeColor;
#elif !ATLANTA
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
            }
            else
            {
                showCrossPhaseMsgOnce = false;
                this.toolStripStatusLabelReceiverStatus.Visible = false;
            }
#endif
        }

        private void dNPDataMessage(byte[] bytePacket, int p)
        {
            if (this.dNPMemphisData != null)
                this.dNPMemphisData.SetAll(bytePacket, p);
            if (this.dNPDIGITALGRIDData != null)
                this.dNPDIGITALGRIDData.SetAll(bytePacket, p);
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
                    this.downloadProgress_Done(ProgressFormCompleteStates.Failure, "No Event To Download");
                }
                this.requestCalibrationConstants();
            }
        }

        private void requestCalibrationConstants()
        {
            SendEventArgs sEA = new SendEventArgs(2);

            sEA.SendPacket[0] = 0x6C;
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

            this.ucTransmitter1.PacketLength = bytePacket.Length;


            for (int i = 0; i < settings.Length; ++i)
            {
                settings[i] = bytePacket[i];
            }

            try
            {
                this.CTRatio = ((Int16)settings[7]) << 8;
                this.CTRatio += settings[6];

                this.updateCTRatio(this.CTRatio);
                this.updateCTRatioDomain(this.CTRatio, this.domainUpDownCTRatioM);

            }
            catch
            {
                this.updateCTRatio(320);
                this.messageHandler("Bad CT Ratio", "Please resend correct CT Ratio");
                this.buttonTypePhasingRestoreDefaults_Click(this, new EventArgs());
                this.buttonRelayType_Click(this, new EventArgs());
            }

            try
            {
                //Transmitter Page
                //ID
                tempI = bytePacket[1];
                tempI <<= 8;
                tempI += bytePacket[0];

                this.ucTransmitterMonitoring1.TransmitterID = tempI.ToString();
            //    this.ucTransmitterMonitoring2.TransmitterID = tempI.ToString();
                //Serial Number
                tempI = bytePacket[3];
                tempI <<= 8;
                tempI += bytePacket[2];

                this.ucRelayProgramming1.SerialNumber = (UInt32)tempI;
                this.ucDNPSAv51.SerialNumber = tempI;

                if (tempI >= 25000 && !GERelay)
                {
                    messageHandler("GE Serial Number programmed with WH Firmware", "Is this a GE Relay? If Yes, please manually reload with GE Software. If No, please contact DIGITALGRID");
#if !DEBUG
                    enableAll(false);
#endif
                }
                else if (tempI < 25000 && GERelay)
                {
                    messageHandler("WH Serial Number programmed with GE Firmware", "Is this a WH Relay? If Yes, please manually reload with WH Software. If No, please contact DIGITALGRID");
#if !DEBUG
                    enableAll(false);
#endif
                }

                if (this.savedSerialNumber != tempI && checkSerialNumber) //check to see if it matches old serial num
                {
                    this.checkSerialNumber = false;
                    this.savedSerialNumber = tempI;
                    this.blockDNPEnableFromTransmitterSettings = false;
                    this.ucTransmitterMonitoring1.TransmitterSN = tempI.ToString();

                    this.textBoxRelaySNControl.Text = tempI.ToString();
                    this.textBoxRelaySNControlPQ.Text = textBoxRelaySNControl.Text;

                    this.requestedAllParameters = true;
                    this.requestMasterRevisionNumber();
                    this.ProgramState = ProgramStates.DownloadingAllParameters;

                    this.requestRelayRevision();
                    logger.Trace("Setting timerResponseTimeOut from setTransmitterSettings");
                    this.timerResponseTimeOut.Enabled = true;

                    return;
                }

                this.checkSerialNumber = false;
                this.savedSerialNumber = tempI;

                this.ucTransmitterMonitoring1.TransmitterSN = tempI.ToString();
             //   this.ucTransmitterMonitoring2.TransmitterSN = tempI.ToString();
                this.textBoxRelaySNControl.Text = tempI.ToString();
                this.textBoxRelaySNControlPQ.Text = textBoxRelaySNControl.Text;

                //Transmitter CT Ratio
                tempI = bytePacket[5];
                tempI <<= 8;
                tempI += bytePacket[4];

                this.ucTransmitterMonitoring1.CTMult = tempI.ToString();
              //  this.ucTransmitterMonitoring2.CTMult = tempI.ToString();
                //DNP Enabled
                if (!this.blockDNPEnableFromTransmitterSettings)
                {
                    if ((bytePacket[28] & 0x04) == 0x04)
                    {
                        this.DNPEnabled = true;
                    }
                    else
                    {
                        this.DNPEnabled = false;
                    }
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
                
#if DNP && ATLANTA
                this.ucCoverFlags1.setDNPCoverFlags(settings);
#endif
                // Pass the settings to the Programming part so it can update,  if need be
                if (this.ucRelayProgramming1.TransmitterPacket == null)
                    this.ucRelayProgramming1.TransmitterPacket = settings;

                if (this.ProgramState == ProgramStates.DownloadingAllParameters)
                {
                    if (this.relayCodeRevisionNumber >= 20130111)
                        this.requestSafeServiceSettings();
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
                this.ucTransmitter1.buttonTX_Click(this, new EventArgs());
            }
        }

        private bool paramsReceivedLock = false;
        private void parametersFinishedLoading()
        {
            logger.Trace("parameters finished loading");
            
            if (this.parametersLoaded && this.badDataDetected == false)
            {
                this.parametersLoaded = false;
                this.messageHandler("Parameters Loaded", "Parameters Loaded Successfully");
               // this.SendAll_Message_PopUp1.Visible = false;
                sendAllF.SendAllFlag = false;
            }
            else if (this.badDataDetected == true)
            {
                this.parametersLoaded = false;
                this.messageHandler("Error", "Parameters NOT Loaded Successfully");
            }
            else if (this.requestedAllParameters || this.ProgramState == ProgramStates.DownloadingAllParameters)
            {
                this.requestedAllParameters = false;
                this.timerResponseTimeOut.Enabled = false;

                if(!paramsReceivedLock)
                {
                    paramsReceivedLock = true;
                 //   this.SendAll_Message_PopUp1.Visible = false;
                    this.messageHandler("Data Recieved", "All Parameters Received");
                    paramsReceivedLock = false;
                }

                if (ucSafeService1.SendSSModeFlag_Send == true)
                {
                    this.ucSafeService1.SendAll();
                    this.ucSafeService1.SendSSModeFlag_Send = false;
                    this.timerResponseTimeOut.Enabled = false;
                }
            }

            
            if (ucRelayProgramming1.State == RelayProgrammingStates.ReprogramSuccess)
            {
                logger.Debug("-------------------------------Resetting ShortRange Parameters");
                this.ucShortRange1.ResetThreshold();
            }
            this.ucRelayProgramming1.AllParametersReceived();


            this.ProgramState = ProgramStates.Running;

            this.requestRelayRegisters();

            this.enableAll(true);
            this.RegisterPolling(true);

#if WATERBUG
            this.toolStripStatusLabelMain.Text = "Test Set found on " + this.serialPort1.PortName;
#else
            if (!tCPConnection)
                this.toolStripStatusLabelMain.Text = "Relay found on " + this.serialPort1.PortName;
#endif

#if !DEBUG
            this.sendTime(DateTime.UtcNow);
#endif

            CheckTransmitterTab();

            startMonitoringWBSettings();

            if (!initializeAutoLoad && !ucRelayProgramming1.ReprogrammingInProgress)
            {
                this.checkDNPEnabled();
            }

            if (this.enableAutoloadToolStripMenuItem.Checked && initializeAutoLoad)
            {
                initializeAutoLoad = false;
                ucRelayProgramming1.InitializeAutoload();
            }
        }

        private void startMonitoringWBSettings()
        {
#if MADISON || DG288_TESTFIXTURE_GUI
                if (this.allEnabled)
                    this.ucTransmitterMonitoring1.TransmitterMonitoring = true;

                this.requestPhasorData();
                this.everyOtherMonitor = false;
#endif

#if MADISON
                this.groupBoxLRLockoutMain.Visible = true;
#else
            this.groupBoxLRLockoutMain.Visible = false;
#endif
        }

        private void CheckTransmitterTab()
        {
            if (this.tabControlMain.SelectedTab == this.tabPageTransmitter)
                tabControlMain_SelectedIndexChanged(null, null);
        }

        private void updateCTRatioDomain(int CT_ratio, DomainUpDown dUP)
        {
            switch (CT_ratio)
            {
                case 160:
                    this.setDomainIndex(7, dUP);
                    break;
                case 240:
                    this.setDomainIndex(6, dUP);
                    break;
                case 320:
                    this.setDomainIndex(5, dUP);
                    break;
                case 400:
                    this.setDomainIndex(4, dUP);
                    break;
                case 500:
                    this.setDomainIndex(3, dUP);
                    break;
                case 600:
                    this.setDomainIndex(2, dUP);
                    break;
                case 700:
                    this.setDomainIndex(1, dUP);
                    break;
                case 750:
                    this.setDomainIndex(0, dUP);
                    break;
                default:
                    this.setDomainIndex(8, dUP);
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
            bool sendProperDNPValue = false;

            if (!this.checkedDNPEnable)
                this.checkedDNPEnable = true;
            else
                return;

#if DNP
            if (receivedMasterRevision.Contains("DNP"))
            {
                if (!ucTransmitter1.DNPEnabled || !ucTransmitter1.CheckDNPEnable)
                {
                    this.ucTransmitter1.DNPEnabled = true;
                    sendProperDNPValue = true;
                }
                this.ucRelayProgramming1.DNPRelay = true;
            }
#else
            if (!receivedMasterRevision.Contains("DNP"))
            {
                if (ucTransmitter1.DNPEnabled || ucTransmitter1.CheckDNPEnable)
                {
                    this.ucTransmitter1.DNPEnabled = false;
                    sendProperDNPValue = true;
                }

                this.ucRelayProgramming1.DNPRelay = false;
                this.removeDNPTabs();
            }
#endif

            if (sendProperDNPValue)
            {
                this.ucTransmitter1.SendTransmitterSettings();
                Thread.Sleep(100);
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
        }

        private void setRelayRegisters(byte[] bytePacket)
        {
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
                this.uc8CheckBoxFlagsRelayStatus1.SetValues(bytePacket[3]);
                this.uc8CheckBoxFlagsRelayStatus2.SetValues(bytePacket[2]);
                this.uc8CheckBoxFlagsCommFlags1.SetValues(bytePacket[5]);
                this.uc8CheckBoxFlagsCommFlags2.SetValues(bytePacket[4]);
                this.uc8CheckBoxFlagsGEControl1.SetValues(bytePacket[7]);
                this.uc8CheckBoxFlagsGEControl2.SetValues(bytePacket[6]);

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
                    this.setCheckedValue(true, this.checkBoxBFlag);
                    this.labelNWPStatus.Text = "NWP: Open";
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxBFlag);
                    this.labelNWPStatus.Text = "NWP: Closed";
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
                    this.toolTip.SetToolTip(this.checkBoxDefaultsUsed, "Problem with Relay Flash detected and the Default Parameters are currently being used");
                }
                else
                {
                    RelayStatus.BadOffset = false;
                    this.toolTip.SetToolTip(this.checkBoxDefaultsUsed, "Non Defaults Parameters being used");
                }
                this.setCheckedValue(RelayStatus.BadOffset, this.checkBoxDefaultsUsed);
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
                    this.toolTip.SetToolTip(this.checkBoxPumping, "Relay in Pump Protect State");
                }
                else
                {
                    RelayStatus.Pumping = false;
                    this.ucPumpMode1.PumpProtectEnabled = false;
                    this.toolTip.SetToolTip(this.checkBoxPumping, "Relay not in Pump Protect State");
                }
                this.setCheckedValue(RelayStatus.Pumping, this.checkBoxPumping);

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
                    this.toolTip.SetToolTip(this.checkBoxPhasingOkayFlag, "Relay has determined phasing of protector and it is OK");
                }
                else
                {
                    RelayFlags.PhasingOkay = false;
                    this.toolTip.SetToolTip(this.checkBoxPhasingOkayFlag, "Relay has yet to determine phasing of the protector or it is crossed phased");
                }

                this.setCheckedValue(RelayFlags.PhasingOkay, this.checkBoxPhasingOkayFlag);

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
                this.setCheckedValue(RelayFlags.BlockedOpen, this.checkBoxBlockedOpenFlag);

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
                    this.toolTip.SetToolTip(this.checkBoxFloatFlag, "Relay is currently in the Float state");
                }
                else
                {
                    RelayFlags.FloatCondition = false;
                    this.toolTip.SetToolTip(this.checkBoxFloatFlag, "Relay not in the Float state");
                }
                this.setCheckedValue(RelayFlags.FloatCondition, this.checkBoxFloatFlag);

                if ((b & 4) == 4)
                {
                    RelayFlags.Tripping = true;
                    this.toolTip.SetToolTip(this.checkBoxTrippingFlag, "Relay is pulsing Trip Contacts");
                }
                else
                {
                    RelayFlags.Tripping = false;
                }
                this.setCheckedValue(RelayFlags.Tripping, this.checkBoxTrippingFlag);

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
                    this.toolTip.SetToolTip(this.checkBoxTripFlag, "Relay is in Trip State");
                    this.toolTip.SetToolTip(this.checkBoxTrippingFlag, "Relay is done with initial pulsing of trip contact");
                }
                else
                {
                    RelayFlags.Open = false;
                    this.toolTip.SetToolTip(this.checkBoxTripFlag, "Relay is not in Trip State");
                    this.toolTip.SetToolTip(this.checkBoxTrippingFlag, "Relay is not in Trip State");
                }
                this.setCheckedValue(RelayFlags.Open, this.checkBoxTripFlag);

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
                            this.ucPumpMode1.buttonSend_Click(this, new EventArgs());
                            this.ucPumpMode1.buttonSend_Click(this, new EventArgs());
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
                    setLabelText("Open", this.labelRelayStateControlPage);
                    setBackgroundColor(Color.Green, this.labelRelayStateControlPage);
                }
                else if (this.RelayFlags.FloatCondition)
                {
                    setLabelText("Float", this.labelRelayTrippedOrClose);
                    setBackgroundColor(Color.Yellow, this.labelRelayTrippedOrClose);
                    setLabelText("Float", this.labelRelayStateControlPage);
                    setBackgroundColor(Color.Yellow, this.labelRelayStateControlPage);
                }
                else
                {
                    setLabelText("Close", this.labelRelayTrippedOrClose);
                    setBackgroundColor(Color.Red, this.labelRelayTrippedOrClose);
                    setLabelText("Close", this.labelRelayStateControlPage);
                    setBackgroundColor(Color.Red, this.labelRelayStateControlPage);
                }

                b = bytePacket[3];
                if ((b & 1) == 1)
                {
                    ucRemoteCommandBlock1.CommandsBlocked = true;
                }
                else
                {
                    ucRemoteCommandBlock1.CommandsBlocked = false;
                }

                b = bytePacket[5];


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
                    this.ucPumpMode1.buttonSend_Click(this, new EventArgs());
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
                this.textBoxTemperature.Text = temperature.ToString();
                this.textBoxTemperatureMonitoringPage.Text = temperature.ToString();
            }
        }

        private void clearTemperatureBoxes()
        {
            this.textBoxTemperature.Text = "";
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
                        if (this.Customer != Customers.ConEdison) //if the GUI is not currently in ConEd mode
                        {
                            this.Customer = Customers.ConEdison;
                        }
#endif
                    }
                }
                else                        //a non-ConEd Relay
                {
                }

            //    for (int index = 0; index < 94; ++index)
            //        MessageBox.Show(bytePacket[index].ToString() + " byte#" + index + " bytePacket[index] Relay Parameters coming from master");// Only for testing - to be removed
                //MessageBox.Show(bytePacket[1].ToString() + " bytePacket[1] Relay Parameters coming from master");// Only for testing - to be removed

                //Reclose Voltage Btyes - Vertical
                temp = bytePacket[1];
                temp <<= 8;
                temp += bytePacket[0];
                tempM = (decimal)temp * Constants.TwelveFracBits;
                closePacket[0] = bytePacket[0];
                closePacket[1] = bytePacket[1];


             //   MessageBox.Show(bytePacket[2].ToString() + " bytePacket[2] Relay Parameters coming from master");// Only for testing - to be removed
             //   MessageBox.Show(bytePacket[3].ToString() + " bytePacket[3] Relay Parameters coming from master");// Only for testing - to be removed

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
                this.ucCloseMode1.buttonSendCloseData_Click(this, new EventArgs());
                this.ucCloseMode1.buttonSendCloseData_Click(this, new EventArgs());
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
                this.defaultTripSettings();
                this.ucTripMode2.buttonSendTripMode_Click(this, new EventArgs());
            }
            try
            {
                // ABC or ACB
                // The bottom three bits of the packet
                temp = 0x07 & bytePacket[80];                
                if (this.customer != Customers.ConEdison)
                {
                    this.conedPhasing = 0; //For when debug is running with coned, the values are different so 
                    /* if (temp == 2)
                     {
 #if !DOMINION                                                         
                         this.setDomainIndex(2, this.domainUpDownPhasings);
 #endif
                     }*/
                     //else
                    if (temp == 1)
                    {
                        this.setDomainIndex(1, this.domainUpDownPhasings);
                    }
                    else if (temp == 0)
                    {
                        this.setDomainIndex(0, this.domainUpDownPhasings);
                    }
                    else if (temp > 48 && temp <= 54) //if it is a coned relay, the phasing will be in ASCII
                    {
#if !DOMINION
                        // this.setDomainIndex(2, this.domainUpDownPhasings);
                        this.setDomainIndex(0, this.domainUpDownPhasings);
                        this.conedPhasing = (uint)temp;
                        //this.Customer = Customers.ConEdison;
#endif
                    }
                    else
                    {
                     //   throw new Exception(temp.ToString() + " is not a valid value for Phasing");
                      //  this.messageHandler("Invalid value for phasing received from relay", "Setting default values for phasing");
                        this.messageHandler("Setting default values for phasing", "temp.ToString()" + " " + "Invalid value for phasing received from relay");
                        this.restoreDefaultsTypeAndPhasing();
                        this.buttonRelayType_Click(this, new EventArgs());
                    }
                }
            }
            catch (Exception ex)
            {
                this.badDataDetected = true;
                this.messageHandler("Phase Issue", ex);
                this.restoreDefaultsTypeAndPhasing();
                this.buttonRelayType_Click(this, new EventArgs());
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
                this.badDataDetected = true;
                this.messageHandler("Trouble setting 277V Bit", ex);
                this.restoreDefaultsTypeAndPhasing();
                this.buttonRelayType_Click(this, new EventArgs());
            }

            try
            {
                // Check the 277V Output bit
                checkBox277DNPOutputs.Checked = (0x10 & bytePacket[80]) == 0x10 ? true : false;
            }
            catch (Exception ex)
            {
                this.badDataDetected = true;
                this.messageHandler("Trouble setting 277V Output Bit", ex);
                this.restoreDefaultsTypeAndPhasing();
                this.buttonRelayType_Click(this, new EventArgs());
            }

            try
            {
                //Power or Sequence
                temp = bytePacket[81];  //69
                if (temp == 'S')
                {
                    this.ucTripMode2.SequenceRelay = true;
                    this.setDomainIndex(1, this.domainUpDownRelayType);
                    labelConEdPowerRelay.Text = "Sequence";
                }
                else if (temp == 'P')
                {
                    this.ucTripMode2.SequenceRelay = false;
                    this.setDomainIndex(0, this.domainUpDownRelayType);
                    labelConEdPowerRelay.Text = "Power";
                }
                else
                {
                    
                    //throw new Exception("'" + Convert.ToChar(temp).ToString() + "' is not a valid Relay Type character.");
                    this.messageHandler("Setting default values for Relay Type", "'" + Convert.ToChar(temp).ToString()  + " " + "Invalid value for phasing received from relay");
                    this.restoreDefaultsTypeAndPhasing();
                    this.buttonRelayType_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                this.badDataDetected = true;
                this.messageHandler("Error in Relay Type Data", ex);
                this.setDomainIndex(0, this.domainUpDownRelayType);
                this.restoreDefaultsTypeAndPhasing();
                this.buttonRelayType_Click(this, new EventArgs());
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
                this.ucPumpMode1.buttonSend_Click(this, new EventArgs());
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
                this.requestTransmitterSettings();
            }
        }

        private uint conedPhasing;

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
            try
            {
                revision = "R";
                revision += ASCIIEncoding.ASCII.GetString(bytePacket);
                if (!revision.Contains("MASTER") && !revision.Contains("REVERBERATOR"))
                    return;

                this.masterRevision = getMasterRevisionNumber(revision);
                this.ucRelayProgramming1.MasterRevisionString = revision;
                this.ucRelayProgramming1.MasterRevisionNumber = (UInt32)this.masterRevision;

                if (this.dNPDIGITALGRIDData != null)
                    this.dNPDIGITALGRIDData.RelayMasterRevision = (UInt32)masterRevision;

                receivedMasterRevision = revision;

                switch (this.customer)
                {
                    case Customers.Memphis:
                        this.DNPEnabled = true;
                        this.blockDNPEnableFromTransmitterSettings = true;
                        this.makeMemphisGUI();
                        this.TransmitterEnabled = false;
                        break;
                    default:
                        // A DNP Relay
                        if (revision.Contains("DNP"))
                        {
                            this.DNPEnabled = true;
                            this.blockDNPEnableFromTransmitterSettings = true;

                            // With PLC
                            if (revision.Contains("PLC"))
                            {
                                this.Customer = Customers.DNPwithPLC;
                            }
                            else if (revision.Contains("ATLANTA"))
                            {
                                this.Customer = Customers.Atlanta;
                            }
                            else if (revision.Contains("ONCOR"))
                            {
                                this.Customer = Customers.Oncor;
                            }
                            else if (revision.Contains("SMUD"))
                            {
                                this.Customer = Customers.SMUD;
                            }
                            else
                            {
                                this.Customer = Customers.DIGITALGRIDDNP;
                            }
                            if (revision.Contains("MEMPHIS") && this.Customer != Customers.Memphis)
                                this.makeMemphisGUI();
                        }
                        break;
                }

                if (this.Customer == Customers.None)
                    this.Customer = Customers.DIGITALGRID;

                this.handleNewMasterRevision();
                this.setLabelText(revision, this.labelRevision);
                this.relayFound = true;
                ucShortRange1.relayFound_forRNCMonitoring = true;
                this.relayFound_forDNPdataMonitoring = true;
                ucRelayProgramming1.ActiveRelay = true;
                this.saveComPort();

                if (this.ProgramState == ProgramStates.CheckingForRelay && !ucRelayProgramming1.ReprogrammingInProgress)
                {
                    this.enableAll(true);
                    if (!tCPConnection)
                        this.toolStripStatusLabelMain.Text = "Relay Found on " + this.serialPort1.PortName;
                    this.timerCheckPortTime.Enabled = false;
                    this.requestAllDataNoMasterRev();
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
            revision = revision.Remove(0, 32); //TEST changed from , 31

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
                    if (this.customer != Customers.SMUD)
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
                        setLabelText("Open", this.labelRelayStateControlPage);
                        setBackgroundColor(Color.Green, this.labelRelayStateControlPage);
                        break;
                    case 'C':
                        setLabelText("Close", this.labelRelayTrippedOrClose);
                        setBackgroundColor(Color.Red, this.labelRelayTrippedOrClose);
                        setLabelText("Close", this.labelRelayStateControlPage);
                        setBackgroundColor(Color.Red, this.labelRelayStateControlPage);
                        break;
                    case 'F':
                        setLabelText("Float", this.labelRelayTrippedOrClose);
                        setBackgroundColor(Color.Yellow, this.labelRelayTrippedOrClose);
                        setLabelText("Float", this.labelRelayStateControlPage);
                        setBackgroundColor(Color.Yellow, this.labelRelayStateControlPage);
                        break;
                    default:
                        setLabelText("Error", this.labelRelayTrippedOrClose);
                        setBackgroundColor(Color.SaddleBrown, this.labelRelayTrippedOrClose);
                        setLabelText("Error", this.labelRelayStateControlPage);
                        setBackgroundColor(Color.SaddleBrown, this.labelRelayStateControlPage);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error in Trip/Close Event", ex);
            }
        }

        private delegate void setTextBoxCallBack(string s, TextBox tB);

        private void setShortRangeParameters(byte[] bytePacket)
        {
            this.ucShortRange1.SetAll(bytePacket);
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
        private void setPhasorValue(byte[] packet)
        {
            long realValue;
            long imaginaryValue, rMS;
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

                    // only send it to this if monitoring is not going on, so that it doens't get every
                    // phasor that comes in during monitoring.
                    if (!this.pQMonitoringEnabled)
                        ucPhasorRequest1.SetPhasorValues(packet[0], packet[1], realValue, imaginaryValue, rMS);
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Phasor Value", ex);
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
                        this.requestAllData();
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
                this.expectingAck = false;
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
                        this.requestAllData();
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
            this.requestAllData();
        }

        private void requestRelayParameters()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'S';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);

            buttonRequestLowVotlageThres_Click(null, null);
        }

        private void requestDNPSettings()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'U';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);

            this.ucDNP1.DNPLabelStatus = ucTransmitter1.CheckDNPEnable;
        }

        private void requestTransmitterSettings()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'X';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);
            this.ucDNP1.DNPLabelStatus = ucTransmitter1.CheckDNPEnable;
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

        private void buttonRelayType_Click(object sender, EventArgs e)
        {
            var choice = DialogResult.Cancel;

            if (sendAllF.SendAllFlag == false)
            {
                choice = MessageBox.Show("Sending Network Protector and Phasing Parameters as set in the APP to the Relay", "Send?", MessageBoxButtons.OKCancel);
            }
            if ((choice == DialogResult.OK) || (sendAllF.SendAllFlag == true))
            {
                
                try
                {
                    byte[] packet = new byte[4];

                    packet[0] = (byte)'s';
                    try
                    {
                        if (this.domainUpDownRelayType.SelectedItem.ToString() == "Sequence")
                            packet[1] = (byte)'S';
                        else if (this.domainUpDownRelayType.SelectedItem.ToString() == "Power")
                            packet[1] = (byte)'P';
                    }
                    catch
                    {
                        this.messageHandler("No Relay Type Selected", new Exception("Please Select Relay Type"));
                        return;
                    }
                    try
                    {
                        if (this.Customer != Customers.ConEdison)
                        {
                            //if (this.domainUpDownPhasings.SelectedItem.ToString() == "ABC")
                            if (this.domainUpDownPhasings.SelectedItem.ToString() == "ABC : CAB : BCA")
                                packet[2] = 0x00;
                            //else if (this.domainUpDownPhasings.SelectedItem.ToString() == "ACB")//
                            else if (this.domainUpDownPhasings.SelectedItem.ToString() == "CBA : BAC : ACB")
                                packet[2] = 0x01;
                            //else if (this.domainUpDownPhasings.SelectedItem.ToString() == "AutoDetect")
                            //    packet[2] = 0x02;
                            else
                                throw new Exception(packet[2].ToString() + " is a bad Phasing");
                        }
                        else
                        {
                            packet[2] = (byte)this.conedPhasing;
                        }
                    }
                    catch
                    {
                        this.messageHandler("No Phasing Selected", new Exception("Please Select Phasing"));
                        return;
                    }

                    try
                    {
                        // set proper bit voltage protector Voltage
                        packet[2] |= (byte)protectorVoltage.SetBit;

                    }
                    catch (Exception ex)
                    {
                        messageHandler("Problem Setting Protector Voltage bits", ex);
                    }

                    try
                    {
                        if (checkBox277DNPOutputs.Checked)
                            packet[2] |= 0x10;
                    }
                    catch (Exception ex)
                    {
                        messageHandler("Problem setting 277 V Outputs bit", ex);
                    }

                    packet[3] = 0x0D;

                    this.sendPacketAck(packet, "Relay Type Send");

                    Thread.Sleep(100);
                    if (!this.sendAll)
                    {
                        this.requestAllData();
                        this.parametersLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    this.messageHandler("Error Setting Relay Type", ex);
                }
            }// if ((choice == DialogResult.OK) || (sendAllF.SendAllFlag == true))
        }

        private void buttonTypePhasingRestoreDefaults_Click(object sender, EventArgs e)
        {
            this.restoreDefaultsTypeAndPhasing();
        }

        private void restoreDefaultsTypeAndPhasing()
        {
            // 1 = Sequence, 0 - Power
            // 0 - ABC, 1 - ACB, 2 - AutoDetect
            comboBoxDNPVoltage.SelectedItem = ProtectorVoltages.GetVoltage();
            checkBox277DNPOutputs.Checked = false;
#if DOMINION
            this.domainUpDownRelayType.SelectedIndex = 1;
            this.domainUpDownPhasings.SelectedIndex = 0;// 2;
            //this.domainUpDownRelayType.SelectedIndex = 0;
            this.domainUpDownRelayType.SelectedIndex = 1;
            labelConEdPowerRelay.Visible = false;
#elif LONDONH || BGE
            this.domainUpDownPhasings.SelectedIndex = 0;//2;
            this.domainUpDownRelayType.SelectedIndex = 1;
#elif ENMAX || (PSEG && !DNP) || TAUNTON
            this.domainUpDownPhasings.SelectedIndex = 0;//2;
            //this.domainUpDownRelayType.SelectedIndex = 0;
            this.domainUpDownRelayType.SelectedIndex = 1;
            labelConEdPowerRelay.Visible = false;
#elif PSEG && DNP
            this.domainUpDownPhasings.SelectedIndex = 0;//2;
            this.domainUpDownRelayType.SelectedIndex = 1;
            labelConEdPowerRelay.Text = "Sequence";
#elif BOSTON || NU || SEATTLE || CHICAGO || MADISON || MEMPHIS
            this.domainUpDownPhasings.SelectedIndex = 0;
            this.domainUpDownRelayType.SelectedIndex = 1;
#elif LONDONH
            this.domainUpDownPhasings.SelectedIndex = 1;
            this.domainUpDownRelayType.SelectedIndex = 1;
//#elif ONCOR
//            this.domainUpDownPhasings.SelectedIndex = 0;//2;
//            this.domainUpDownRelayType.SelectedIndex = 1;
#elif CONED
            this.domainUpDownPhasings.SelectedIndex = 0;
            this.domainUpDownRelayType.SelectedIndex = 0;
#else
            this.domainUpDownPhasings.SelectedIndex = 0;// 2;
            this.domainUpDownRelayType.SelectedIndex = 1;
#endif
            /*
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
#if SEATTLE || DOMINION || CHICAGO || ATLANTA || ENMAX || MADISON || MEMPHIS //|| LONDONH
            this.domainUpDownCTRatioM.SelectedIndex = 2;
#elif CONED || LONDONH || ONCOR
            this.domainUpDownCTRatioM.SelectedIndex = 5;
#else
            this.domainUpDownCTRatioM.SelectedIndex = 5;
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
#if WATERBUG
                errorMessage = "Unable To Locate Test Set.";
#else
                errorMessage = "Unable To Locate Relay.";
#endif
                this.relayNotFound();
                this.RegisterPolling(false);

                ucRelayProgramming1.NotPollingPort = false;

                this.messageHandler("No Relay Found", new Exception(errorMessage));
                return;
            }

            currentPort = this.portNames[0];

#if WATERBUG
            this.toolStripStatusLabelMain.Text = "Checking " + currentPort + " for Test Set";
#else
            this.toolStripStatusLabelMain.Text = "Checking " + currentPort + " for Relay";
#endif
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

            //this.portNames.RemoveAt(0);
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
#if WATERBUG
            this.toolStripStatusLabelMain.Text = "Checking " + this.serialPort1.PortName + " for Test Set";
#else
            this.toolStripStatusLabelMain.Text = "Checking " + this.serialPort1.PortName + " for Relay";
#endif

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

        private void requestAllData()
        {
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.requestedAllParameters = true;
            this.requestMasterRevisionNumber();
            this.requestAllDataNoMasterRev();
        }

        private void requestAllDataNoMasterRev()
        {
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (this.InvokeRequired)
            {
                requestAllCallBack rACB = new requestAllCallBack(this.requestAllData);
                this.Invoke(rACB);
            }
            else
            {
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
            return;
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

            AcknowledgeCaller = caller;
            try
            {
                if (checkBoxSerialCommsDebugging.Checked)
                {
                    logger.Trace(String.Format("Sending Packet: {0}", BitConverter.ToString(bytePacket)));
                }
                this.SCITimedOut = false;

                while (this.expectingAck && !this.loadingNewCode)
                {
                    Application.DoEvents();
                }

                if (this.SCITimedOut)               //if the SCI Timed out while waiting for the ACK
                    return;

                errorMessage = "Error Checking if Port is open";
                if (!this.serialPort1.IsOpen)
                {
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

                errorMessage = "Error Checking For Bytes left to read";
                ++this.byteCount;

                errorMessage = "Error Writing To Port";
                this.serialPort1.Write(bytePacket, 0, bytePacket.Length);
                this.expectingAck = true;

                errorMessage = "Error At End of Routine";
                this.timerSCITimeOut.Enabled = true;
                return;
            }
            catch (Exception ex)
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                this.enableAll(false);
                this.clearSerialPortBuffers(this.serialPort1);
                GC.Collect();
                this.messageHandler(errorMessage + ", Please Check Port.", ex);
            }
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
            {
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
                    this.sendSaveCalibration();
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
        {
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

        private void buttonSendCTRatio_Click(object sender, EventArgs e)
        {
            this.sendCTRatio();
        }

        private void sendCTRatio()
        {
            byte[] packet = new byte[4];

            packet[0] = (byte)'Z';
            packet[1] = (byte)this.CTRatio;
            packet[2] = (byte)(this.CTRatio >> 8);
            packet[3] = 0x0D;

            this.sendPacketAck(packet, "CT Ratio Send");

            if (!this.sendAll)
            {
                this.requestAllData();
                this.parametersLoaded = true;
            }
        }

        private void buttonMakeRetarded_Click(object sender, EventArgs e)
        {
            byte[] packet = new byte[3];

            packet[0] = (byte)'L';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacket(packet);
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

            if (this.domainUpDownCTRatioM.SelectedIndex == 8)
            {
                try
                {
                    cTRatio5 = Convert.ToInt16(this.textBoxCTRatio.Text);
                    if (this.Customer == Customers.ConEdison)
                    {
                        if (cTRatio5 < 0)
                            cTRatio5 = -cTRatio5;

                        cTRatio5 = cTRatio5 - (cTRatio5 % 50); //make sure it is divisible by 50

                        if (cTRatio5 > 12750)
                        {
                            this.textBoxCTRatio.Text = "12750";
                            cTRatio5 = 12750;
                        }
                        else if (cTRatio5 < 50)
                        {
                            cTRatio5 = 50;
                            this.textBoxCTRatio.Text = "50";
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

        private void domainUpDownCTRatioM_SelectedItemChanged(object sender, EventArgs e)
        {
            DomainUpDown dUD = (DomainUpDown)sender;
            int ratio = this.CTRatio;
            int ratio5 = this.CTRatio * 5;

            switch (dUD.SelectedIndex)
            {
                case 7:
                    ratio = 160;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    break;
                case 6:
                    ratio = 240;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    break;
                case 5:
                    ratio = 320;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    break;
                case 4:
                    ratio = 400;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    break;
                case 3:
                    ratio = 500;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    break;
                case 2:
                    ratio = 600;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    break;
                case 1:
                    ratio = 700;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
                    break;
                case 0:
                    ratio = 750;
                    ratio5 = ratio * 5;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio5.ToString();
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
                    ratio = 160;
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

        private void buttonResetBothProc_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.messageHandler("Reset Relay", "Do you really want to reset the relay?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                this.resetBothProcs();
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

            this.loadingNewCode = false;
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
                setLabelText("Unknown", this.labelRelayStateControlPage);
                setBackgroundColor(Color.Transparent, this.labelRelayStateControlPage);
                //this.clearTemperatureBoxes();

                this.enableFlagsAndStatus(false);

                if (!this.relayFound)
                    this.requestMasterRevisionNumber();
            }

            if (!this.quietMode)
            {
                this.registersReceived = false;
                this.requestRelayRegisters();
                this.labelQuietMode.Visible = false;
            }
            else
            {
#if !DEBUG
                this.messageHandler("Set To Quiet Mode", "Unset Quiet Mode");
#endif
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

                        if ((this.missedMonitoringCount == 2) && (temp == true) && (this.relayFound_forDNPdataMonitoring == false) )
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
                                this.ucTransmitterMonitoring1.TransmitterMonitoring = true;
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
            this.enableCheckBox(b, this.checkBoxDefaultsUsed);
            this.enableCheckBox(b, this.checkBoxBlockedCloseFlag);
            this.enableCheckBox(b, this.checkBoxBlockedOpenFlag);
            this.enableCheckBox(b, this.checkBoxCalibrating);
            this.enableCheckBox(b, this.checkBoxFloatFlag);
            this.enableCheckBox(b, this.checkBoxMathError);
            this.enableCheckBox(b, this.checkBoxMathOverTime);
            this.enableCheckBox(b, this.checkBoxMonitorPhasors);
            this.enableCheckBox(b, this.checkBoxOffsetOkay);
            this.enableCheckBox(b, this.checkBoxPhasingOkayFlag);
            this.enableCheckBox(b, this.checkBoxPowerSaveFlag);
            this.enableCheckBox(b, this.checkBoxPumping);
            this.enableCheckBox(b, this.checkBoxSequence);
            this.enableCheckBox(b, this.checkBoxFlag1);
            this.enableCheckBox(b, this.checkBoxFlag2);
            this.enableCheckBox(b, this.checkBoxBFlag);
            this.labelNWPStatus.Enabled = b;
            if (!b)
                this.labelNWPStatus.Text = "NWP: Unknown";
            this.enableCheckBox(b, this.checkBoxInInsensRegion);
            this.enableCheckBox(b, this.checkBoxInTripRegion);
            this.enableCheckBox(b, this.checkBoxTripFlag);
            this.enableCheckBox(b, this.checkBoxTrippingFlag);

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

           // this.liveDataTriggerTime = DateTime.UtcNow;

           // this.labelLiveDataTriggerTime.Text = DateTime.UtcNow.ToString();

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
                    Stream stream = File.Open(@"C:\DGI Systems\Relay\Saved Data\SavedSettings.dgi", FileMode.OpenOrCreate);
                    BinaryFormatter formatter = new BinaryFormatter();

                    if (stream.Length != 0)
                        try
                        {
                            this.saveObject = (SavedSettingsV4)formatter.Deserialize(stream);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error deserializing SavedSettingsV4", ex);
                        }
                    stream.Close();
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
                            this.comboBoxSavedStates.Items.Add(this.saveObject.Settings[i].Name);
                    }
                    this.comboBoxSavedStates.Text = "";
                }
                catch (Exception ex)
                {
                    this.messageHandler("Error Populating Saved States ComboBox", ex);
                }
            }
        }

        private void buttonDeleteSetting_Click(object sender, EventArgs e)
        {
            if (this.comboBoxSavedStates.Text != "" && this.comboBoxSavedStates.Text != null)
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
            sS.DNPSettings = this.ucDNP1.GetSavedState();
#endif
            sS.SafeServiceSettings = this.ucSafeService1.GetSavedState();
            sS.CTRatio = this.CTRatio;
            sS.Phasing = this.domainUpDownPhasings.SelectedIndex;
            sS.RelayType = this.domainUpDownRelayType.SelectedIndex;
            sS.V277Protector = protectorVoltage.SetBit.HasFlag(ProtectorVoltageBits.V277);
            sS.V600Protector = protectorVoltage.SetBit.HasFlag(ProtectorVoltageBits.V600);
            sS.ProtectorVoltageOutputs = checkBox277DNPOutputs.Checked;

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
                this.ucDNP1.SetAllValues(sS.DNPSettings);
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

            this.updateCTRatioDomain(sS.CTRatio, this.domainUpDownCTRatioM);
            this.updateCTRatio(sS.CTRatio);

            this.domainUpDownPhasings.SelectedIndex = sS.Phasing;
            this.domainUpDownRelayType.SelectedIndex = sS.RelayType;
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

        private void buttonSendAll_Click(object sender, EventArgs e)
        {
            DialogResult SendAll_DelayAlertDR = new DialogResult();
            SendAll_DelayAlertDR = MessageBox.Show("Please have patience. The relay is updating its critical parameters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (SendAll_DelayAlertDR == DialogResult.OK)
            {
                this.enableAll(false);
                Application.UseWaitCursor = true; //keeps waitcursor even when the thread ends.
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor; //Normal mode of setting waitcursor
                // this.pictureBox_SendAll.Enabled = true;
                //this.pictureBox_SendAll.Visible = true;
                //this.downloadingDialogCountDown("Please have patience. The relay is updating its critical parameters", "", 3);
                this.sendAllParameters();
                Application.UseWaitCursor = false;
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                this.enableAll(true);
                //this.pictureBox_SendAll.Enabled = false;
                // this.pictureBox_SendAll.Visible = false;
            }
        }

        private void sendAllParameters()
        {
            //string text = "Please wait. Relay Parameters are being updated. This will take few seconds !";
            //MessageBox.Show(text);
            sendAllF.SendAllFlag = true;
            /*   this.SendAll_Message_PopUp1.WindowState = System.Windows.Forms.FormWindowState.Normal; //System.Windows.Forms.FormWindowState.Minimized;
               this.SendAll_Message_PopUp1.BringToFront();
               this.SendAll_Message_PopUp1.Enabled = true;
               this.SendAll_Message_PopUp1.Visible = true;
             */
         //  this.downloadingDialogCountDown("", "Please have patience. The relay is updating its critical parameters", 5, true);

#if DNP
                   this.ucDNPSAv5OSName2.buttonGenerateName.Enabled = true;
                   this.ucDNPSAv5OSName2.newOSname();
                   //Thread.Sleep(100);  // 100 milliseconds
                   //Thread.Sleep(500);   // .5 seconds
                   Thread.Sleep(834);   // 2.5 seconds
                   this.ucDNPSAv5OSName2.sendOSName();
                   //Thread.Sleep(100);  // 100 milliseconds
                   //Thread.Sleep(500);   // .5 seconds
                   Thread.Sleep(834);   // 2.5 seconds
                   this.ucDNPSAv5Settings2.setDefaults();
                   //Thread.Sleep(100);  // 100 milliseconds
                   //Thread.Sleep(500);   // .5 seconds
                   Thread.Sleep(834);   // 2.5 seconds
                   this.ucDNPSAv5Settings2.sendSettings();
                   //Thread.Sleep(100);  // 100 milliseconds
                   //Thread.Sleep(500);   // .5 seconds
                   Thread.Sleep(834);   // 2.5 seconds
            
#endif
            this.sendAll = true;
            this.ucTripMode2.buttonSendTripMode_Click(this, new EventArgs());
            //Thread.Sleep(100);  // 100 milliseconds
            Thread.Sleep(834);   // 2.5 seconds
            this.ucCloseMode1.buttonSendCloseData_Click(this, new EventArgs());
            //Thread.Sleep(100);  // 100 milliseconds
            Thread.Sleep(834);   // 2.5 seconds
            this.buttonRelayType_Click(this, new EventArgs());
            //Thread.Sleep(100);  // 100 milliseconds
            Thread.Sleep(834);   // 2.5 seconds
            this.buttonSendCTRatio_Click(this, new EventArgs());
            //Thread.Sleep(100);  // 100 milliseconds
            Thread.Sleep(834);   // 2.5 seconds
            this.ucPumpMode1.buttonSend_Click(this, new EventArgs());
            //Thread.Sleep(100);  // 100 milliseconds
            Thread.Sleep(834);   // 2.5 seconds
             
#if ATLANTA
            this.buttonSendLowVoltageThres_Click(this, new EventArgs());
            Thread.Sleep(100);
#endif

#if DNP
            if (receivedMasterRevision.Contains("DNP"))
                this.ucTransmitter1.ForceDNPEnable = true;
#endif
            if (this.relayCodeRevisionNumber >= 20130111 || this.loadingNewCode)
            {
                this.ucSafeService1.SendAll();
                //Thread.Sleep(100);  // 100 milliseconds
                Thread.Sleep(834);   // 1 seconds
            }

#if DNP && ATLANTA
            this.ucTransmitter1.DNPCoverFlags = this.ucCoverFlags1.getDNPCoverFlagsByte();
            //this.ucTransmitterMonitoring1.setPolarityFromRelaySettings(); //this.ucTransmitter1.setPolarityFromRelaySettings();
            this.ucTransmitter1.setPolarityFromRelaySettings();
            this.ucTransmitter1.SendTransmitterSettings();
            Thread.Sleep(100);
#endif

            //this.sendAll = false;
            if (!this.loadingNewCode)
                this.requestAllData();

            this.parametersLoaded = true;
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

            this.downloadingDialogCountDown("Downloading", "Downloading Live Data ",
                tCPConnection ? 420 : 210, false);
        }

        private ProgressBarForm downloadProgress;
        private void downloadingDialogCountDown(string title, string label, int halfSecondCounts, bool dialog)
        {
            this.enableAll(false);

            if (this.Customer == Customers.ConEdison) //baud rate half speed
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

            if (this.Customer == Customers.ConEdison) //baud rate half speed
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
            this.updateCTRatioDomain((int)this.ucTransmitter1.CTRatio, this.domainUpDownCTRatioM);
            this.updateCTRatio((int)this.ucTransmitter1.CTRatio);
        }

        private void updateCTRatio(int ratio)
        {
            int ratio5 = ratio * 5;

            if (ratio5 == 0)
            {
                this.messageHandler("Bad CT Ratio Value", new Exception("Can't set CT Ratio to zero. \r\n Sending a valid value to relay.  Please Check."));
                this.CTRatio = 320;
                ratio = 320;
                ratio5 = 1600;
                this.domainUpDownCTRatioM.SelectedIndex = 4;


            }
            if (ratio5 > 12750)
            {
                this.messageHandler("Bad CT Ratio Value", new Exception("Can't Set CT Ratio above 12750. \r\n Please send a valid value to relay"));
                this.CTRatio = 320;
                ratio = 320;
                ratio5 = 1600;
                this.domainUpDownCTRatioM.SelectedIndex = 4;
                this.updateCTRatio(ratio5);
            }
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
            this.textBoxCTRatioPQMonitor.Text = textBoxCTRatio.Text;
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

        private void MainControl_FormClosed(object sender, FormClosedEventArgs e)
        {
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
                ucDNPSAv51.ShowDNPSAV5Error = true;
                this.ucDNPSAv51.RequestAllData();
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
                    this.ucTransmitterMonitoring1.TransmitterMonitoring = true;

                this.requestPhasorData();
                this.everyOtherMonitor = false;
            }
            else if (this.tabControlMain.SelectedTab == this.tabPageControl)
            {
                this.transmitterMonitoring = false;
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
             //   this.SendAll_Message_PopUp1.Visible = false;
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
            this.ucTransmitterMonitoring1.TransmitterMonitoring = true;
        }

        private void pauseTransmitterMonitoring()
        {
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
#if MADISON
            if (this.masterRevision >= 150521)
                this.setWBdataMain(bytePacket);
#endif
        }

        private void setWBdataMain(byte[] bytePacket)
        {
            if ((bytePacket[15] & 0x08) == 0x08)
                this.textBoxLRLockoutStatusMain.Text = "Locked!";
            else
                this.textBoxLRLockoutStatusMain.Text = "Not Locked";
        }

        private void setDNPSettings(byte[] bytePacket)
        {
            byte memphisStage = 0;
            try
            {
                if (this.ProgramState == ProgramStates.DownloadingAllParameters)
                {
                    this.parametersFinishedLoading();
                }
#if !WATERBUG
                if (this.DNPEnabled)
                    this.ucDNP1.SetAll(bytePacket);

                memphisStage = (byte)(bytePacket[0] & 0xE0);
                memphisStage >>= 5;

                if (this.DNPEnabled && this.customer == Customers.Memphis && this.dNPMemphisData != null)
                    this.dNPMemphisData.MemphisStage = (uint)memphisStage;
#endif
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting DNPData", ex);
            }
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
#if !DG288_TESTFIXTURE_GUI
                        this.tabControlMain.TabPages.Add(this.tabPageArcFault);
#endif
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

        private void cTRatioCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CTRatioCaculator CTCalculator = new CTRatioCaculator();

            CTCalculator.ShowDialog(this);
        }

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
            if (val)
            {
                this.buttonRequestDNPData.Text = "Stop Requesting Data";
                this.buttonRequestDNPData.BackColor = Color.Green;
            }
            else
            {
                this.buttonRequestDNPData.Text = "Request DNP Data";
                this.buttonRequestDNPData.BackColor = Color.Red;
             /*   if (this.relayFound_forDNPdataMonitoring == false)
                {
                    this.enableDNPMonitoring(false);
                    this.relayFound_forDNPdataMonitoring = false;
                    string text = "Relay not found. Please check for its Power and then start the Monitoring ";
                    MessageBox.Show(text);
                }
             */
            }
            this.requestingDNPData = val;
        }

        private void requestDNPData()
        {
            byte[] sendPacket = new byte[3];

            sendPacket[0] = 0x05;
            sendPacket[1] = 0x55;
            sendPacket[2] = 0x0D;

            this.sendPacket(sendPacket);
        }

        private void reprogramRelayFileSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ucRelayProgramming1.InitialAutoLoadFiles();
            this.checkedDNPEnable = false;
        }

        private void enableAutoloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

        private void domainUpDownRelayType_SelectedItemChanged(object sender, EventArgs e)
        {
            if (this.domainUpDownRelayType.SelectedIndex == 0)
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
            SendEventArgs sEA = new SendEventArgs(4);

            try
            {
                if (numericUpDownLowVoltageThres.Value <= numericUpDownLowVoltageThres.Maximum &&
                    numericUpDownLowVoltageThres.Value >= numericUpDownLowVoltageThres.Minimum)
                {
                    sEA.SendPacket[0] = Convert.ToByte('&');
                    sEA.SendPacket[1] = 0x55;
                    sEA.SendPacket[2] = Convert.ToByte(numericUpDownLowVoltageThres.Value);
                    sEA.SendPacket[3] = 0x0D;

                    this.sendPacket(sEA.SendPacket);
                }
                else
                {
                    MessageBox.Show("Low Voltage Threshold Value must be between %i and %i" +
                        numericUpDownLowVoltageThres.Minimum + " and " + numericUpDownLowVoltageThres.Maximum);
                }

                if (!this.sendAll)
                {
                    Thread.Sleep(100);
                    this.requestAllData();
                    this.parametersLoaded = true;
                }

            }
            catch
            {
                MessageBox.Show("Error sending Low Voltage Threshold Value");
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
            this.requestedAllParameters = true;
            this.requestMasterRevisionNumber();
            this.requestAllDataNoMasterRev();
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
                        this.SendConfirmed = true;
                        this.expectingAck = false;
                        logger.Info("Ack");
                        packetAcknowledged(true);
                        this.timerSCITimeOut.Enabled = false;
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
#if CHICAGO || LONDONH
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
                    ProtectorVoltageOutputs = (bool)info.GetValue("V277Outputs", typeof(bool));
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
