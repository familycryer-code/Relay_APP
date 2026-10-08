using GraphicsServer.GSNet.Widgets;
using NLog;
using SharedResources;
using System;
using System.CodeDom;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
//using System.Linq;
//using System.Net.NetworkInformation;
//using System.Resources;
//using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;

namespace RelayControlLibrary
{
    public partial class ucRelayProgramming : UserControl
    {
        public ucRelayProgramming()
        {
            InitializeComponent();

            // Set this string to match code date below
            this.fPGACode.Date = Properties.Resources.FPGARevisionDisplay;
            this.programmingForm.FormClosed += programmingForm_FormClosed;
            this.currentRelayLog.MPRevision = _masterCodeRevisionNumber.ToString();
            this.currentRelayLog.RPRevision = _relayCodeRevisionNumber.ToString();
            this.currentRelayLog.FPGARevision = _fPGACodeRevisionNumber.ToString();

            this.initializeToolTip();
            this.initializeCustomerComboBox();
            this.initializeCustomerFileSets();
        }

        void programmingForm_FormClosed(object o, FormClosedEventArgs fCEA)
        {
            this.programmingForm = new formProgrammingProgess();
            this.programmingForm.FormClosed += programmingForm_FormClosed;
        }

        public void HideProgrammingProgress()
        {
            if (this.programmingForm != null && !this.programmingForm.IsDisposed)
            {
                this.programmingForm.CurrentTask = string.Empty;
                this.programmingForm.Hide();
                logger.Info("POST-PROGRAM RESTORE: programming progress form hidden after completion acknowledgement.");
            }
        }

        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public bool ActiveRelay { get; set; }
        // These need to be updated when new files are used

        private static UInt32 _masterCodeRevisionNumber = Convert.ToUInt32(Properties.Resources.MasterRevision);
        private static UInt32 _relayCodeRevisionNumber = Convert.ToUInt32(Properties.Resources.RelayRevision);
        private static UInt32 _fPGACodeRevisionNumber = Convert.ToUInt32(Properties.Resources.FPGARevision);
        private static UInt32 _bootCodeRevisionNumber = Convert.ToUInt32(Properties.Resources.BootRevision);
        //private static UInt32 _safeService_MASTER_REVISION = 160621;
        //private static UInt32 _rEV1_MASTER_REVISION = 100713;

        private uint tempBootAddress = 0;
        private bool programBootCodeOnly = false;
        private bool programBootCodeStart = false;
        private bool programBootCodeInProgress = false;
        private UInt32 masterBootRevisionNumberReceived = 0;
        //private bool revTooLowErrorAlreadyShown = false;
        private bool dontShowRelayUpgradeMessage = false;
        private bool masterBootRevisionSet = false;
        private bool askToUgradeShown = false;
        private bool firmwareUpgradeAcceptedThisCycle = false;
        private bool reprogramBootCodeAuto = false;
        private string bootStartUpChar = "0";
        private bool wrongBootCodeLoaded = false;

#pragma warning disable CS0414 // The field is assigned but its value is never used
        private bool reloadBootWithPrompt = false;
#pragma warning restore CS0414



        private bool programMasterBootFileSelect = false;
        private string masterBootStringReceived = "0";
        private bool autoLoad = false;
        private string masterRevisionString = "";
        private bool notPollingPort = false;
        private DialogResult upgradeAutoDR = DialogResult.No;
        private bool reprogrammingInProgress = false;
        public bool AutoloadAcceptedPendingBackup { get; set; } = false;
        private bool startWarningAcknowledgedThisCycle = false;


        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                logger.Trace("Customer: {0}", value);
                this.customer = value;
            }
        }
        public RelayProgrammingStates State
        {
            get { return this.state; }
            set
            {
                logger.Trace(String.Format("State: {0}", value.ToString()));
                this.state = value;
            }
        }

        public UInt32 MasterBootRevisionNumberReceived
        {
            get { return this.masterBootRevisionNumberReceived; }
            set
            {
                this.masterBootRevisionNumberReceived = value;
            }
        }

        public string MasterRevisionString
        {
            get { return this.masterRevisionString; }
            set
            {
                this.masterRevisionString = value;
            }
        }

        public bool NotPollingPort
        {
            get { return this.notPollingPort; }
            set
            {
                this.notPollingPort = value;
            }
        }

        public bool ReprogrammingInProgress
        {
            get { return this.reprogrammingInProgress; }
            set
            {
                this.reprogrammingInProgress = value;
            }
        }

        public void startReloadingJustBoot()
        {
            this.programmingForm.ClearAllChecks();
            reloadBootWithPrompt = true;
            this.state = RelayProgrammingStates.CheckMasterBootCode;
            sendReset();
        }

        public bool ProgramBootCodeStart
        {
            get { return this.programBootCodeStart; }
            set
            {
                bool staleBootContinuation =
                    this.State == RelayProgrammingStates.AutoLoadCheckBoot ||
                    this.State == RelayProgrammingStates.ManualLoadCheckBoot ||
                    this.State == RelayProgrammingStates.CheckMasterBootCode ||
                    (this.masterBootRevisionSet &&
                     this.masterBootRevisionNumberReceived > 0 &&
                     this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber);

                logger.Info(
                    "ProgramBootCodeStart setter ENTRY: state={0}, programBootCodeInProgress={1}, reprogrammingInProgress={2}, staleBootContinuation={3}, masterBootRevisionSet={4}, masterBootRevisionNumberReceived={5}",
                    this.State,
                    this.programBootCodeInProgress,
                    this.reprogrammingInProgress,
                    staleBootContinuation,
                    this.masterBootRevisionSet,
                    this.masterBootRevisionNumberReceived);

                if (value == false)
                {
                    this.programBootCodeStart = false;
                    return;
                }

                // Full manual reload is allowed to start boot. Only block non-full manual paths.
                if (ManualUpdate.usingManualMode && !this.manualReload)
                {
                    logger.Warn("ProgramBootCodeStart blocked: manual mode without full reload intent.");
                    this.programBootCodeStart = false;
                    return;
                }

                if (this.programBootCodeInProgress)
                {
                    logger.Warn("Boot code already in progress; ignoring duplicate ProgramBootCodeStart.");
                    this.programBootCodeStart = false;
                    return;
                }

                if (this.reprogrammingInProgress && !staleBootContinuation)
                {
                    logger.Warn("Ignoring ProgramBootCodeStart because normal programming is active.");
                    this.programBootCodeStart = false;
                    return;
                }

                this.programBootCodeStart = true;

                if (this.programBootCodeStart)
                {
                    if (!this.programBootCodeInProgress)
                    {
                        bool manualOverride = false;

                       
                        if (!this.manualReload && !this.CanRepairBoot(manualOverride))
                        {
                            logger.Warn(
                                "ProgramBootCodeStart blocked: boot state is not valid for repair. manualOverride={0}, bootSet={1}, bootRev={2}",
                                manualOverride,
                                this.masterBootRevisionSet,
                                this.masterBootRevisionNumberReceived);

                            this.programBootCodeStart = false;
                            return;
                        }

                        if (!EnsureProgrammingStartWarningAcknowledged())
                        {
                            logger.Warn("ProgramBootCodeStart blocked: start warning not acknowledged.");
                            this.programBootCodeStart = false;
                            return;
                        }

                        logger.Info("ProgramBootCodeStart setter: about to call ProgramBootCode()");
                        ProgramBootCode();
                    }

                    this.programBootCodeStart = false;
                }
            }
        }


        public bool ProgramBootCodeInProgress
        {
            get { return this.programBootCodeInProgress; }
            set
            {
                this.programBootCodeInProgress = value;
            }
        }

        public bool ReprogramBootCodeAuto
        {
            get { return this.reprogramBootCodeAuto; }
            set
            {
                this.reprogramBootCodeAuto = value;
            }
        }

        public void ProgramBootCode()
        {
            logger.Info("ProgramBootCode() entered");
            MasterBootLoaderStart();
        }

        private bool EnsureProgrammingStartWarningAcknowledged()
        {
            if (this.startWarningAcknowledgedThisCycle)
                return true;

            DialogResult dr = ShowProgrammingStartWarning();
            logger.Info("POPUP RESULT: START_WARNING_DO_NOT_REMOVE_PORT result={0}", dr);

            if (dr != DialogResult.OK)
            {
                this.startWarningAcknowledgedThisCycle = false;
                this.autoLoad = false;
                this.reprogrammingInProgress = false;
                return false;
            }

            this.startWarningAcknowledgedThisCycle = true;
            return true;
        }

        //private bool forceUpdateOnce = false;
        private bool forceRelayUpdate = false;
        public bool ForceRelayUpdate
        {
            get { return this.forceRelayUpdate; }
            set
            {
                this.forceRelayUpdate = value;
            }
        }

        private string forceUpdateReason = "Generic";
        public string ForceUpdateReason
        {
            get { return this.forceUpdateReason; }
            set
            {
                this.forceUpdateReason = value;
            }
        }

        public delegate void SendDelegate(object o, RelayProgrammingEventArgs rPEA);
        public event SendDelegate Send;

        public delegate void ErrorHandler(object o, ExceptionEventArgs eEA);
        public event ErrorHandler Error;

        public delegate void BackupBeforeProgrammingHandler(object sender, EventArgs e);
#pragma warning disable CS0067
        public event BackupBeforeProgrammingHandler BackupBeforeProgrammingRequested;
#pragma warning restore CS0067


        public byte[] TransmitterPacket;



        public UInt32 SerialNumber
        {
            get { return this.serialNumber; }
            set
            {
                if (value != 0 && value != this.serialNumber)
                {
                    logger.Trace("Serial Number: " + value.ToString());
                    logger.Trace(DateTime.UtcNow.ToString());
                }

                this.currentRelayLog.SerialNumber = this.serialNumber = value;
                //if ((value > 32767 || value == 0) && !this.serialNumberError && this.MasterRevisionNumber != 0)
                if ((value > 65535 || value == 0) && !this.serialNumberError && this.MasterRevisionNumber != 0)
                {
                    this.serialNumberError = true;
                    MessageBox.Show("Serial Number Error", "Error with Serial Number, \r\nPlease Contact DIGITALGRID, INC.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.serialNumberError = false;
                }

                // 3076 = default/fallback serial when relay memory is bad; exclude from GE/WH comparisons
                if (this.serialNumber == 3076)
                {
                    this.gERelaySerialMatch = true;
                }
                else if ((this.serialNumber >= 25000) ^ this.gERelay) //25k and up are GE Serial Numbers
                {
                    this.gERelaySerialMatch = false;
                }
                else
                {
                    this.gERelaySerialMatch = true;
                }
            }
        }

        public event EventHandler<RelayTypeChangedEventArgs> RelayTypeChanged;

        // This property added because we have to alert the main form that the type of the relay
        // has changed when the type of the relay has been manually selected for reprogramming.
        // If not, the main form will potentially generate an serial number mismatch error.
        private bool internalGESetter
        {
            set
            {
                GERelay = value;
                RelayTypeChangedEventArgs rTCEA = new RelayTypeChangedEventArgs(value);
                RelayTypeChanged?.Invoke(this, rTCEA);
            }
        }

        public bool GERelay
        {
            get { return this.gERelay; }
            set
            {
                this.currentRelayLog.GERelay = this.gERelay = value;

                // 3076 = default/fallback serial when relay memory is bad; exclude from GE/WH comparisons
                if (this.serialNumber == 3076)
                {
                    this.gERelaySerialMatch = true;
                }
                else if (this.gERelay ^ (this.serialNumber >= 25000))
                {
                    this.gERelaySerialMatch = false;
                }
                else
                {
                    this.gERelaySerialMatch = true;
                }
            }
        }
        public bool DNPRelay
        {
            get { return this.dNPRelay; }
            set
            {
#if ONCOR
                this.dNPRelay = true;
#else
                this.dNPRelay = value;
                this.currentRelayLog.DNPRelay = value;
#endif
            }
        }
        public bool TransmitterEnabled
        {
            get { return this.transmitterEnabled; }
            set
            {
                this.transmitterEnabled = value;
                this.programmingForm.TransmitterPresent = value;
                this.currentRelayLog.FPGAPresent = value;
                if (!value)
                    this.reprogramFPGA = false;
                else
                {
                    if (this.remoteFPGARevisionNumber < _fPGACodeRevisionNumber)
                    {
                        this.reprogramFPGA = true;
                        this.transmitterEnabled = true;
                    }
                }
            }
        }

        public UInt32 MasterRevisionNumber
        {
            get { return this.remoteMasterRevisionNumber; }
            set
            {
                UInt32 previousValue = this.remoteMasterRevisionNumber;
                this.remoteMasterRevisionNumber = value;

                // Prevent setter-driven relatch while the relay is already in a boot/programming flow.
                if (this.IsProgrammingActiveOrInFlight())
                {
                    logger.Info(
                        "MasterRevisionNumber setter suppressed during active run. state={0}, autoLoad={1}, reprogrammingInProgress={2}, programBootCodeInProgress={3}, value={4}",
                        this.State,
                        this.autoLoad,
                        this.reprogrammingInProgress,
                        this.programBootCodeInProgress,
                        value);

                    return;
                }

                switch (this.State)
                {
                    case RelayProgrammingStates.LoadingMasterCode:
                        this.programmingForm.CurrentTask = "Failed to Respond to Transfer Command";
                        this.startMasterProgramming();
                        break;

                    case RelayProgrammingStates.ReprogramSuccess:
                        this.timerTimeout.Stop();
                        restartProgram();
                        break;

                    case RelayProgrammingStates.Finalized:
                        logger.Trace("Finalized, state master rev");
                        this.State = RelayProgrammingStates.Idle;
                        break;

                    case RelayProgrammingStates.RequestAll:
                        this.State = RelayProgrammingStates.WaitForAllData;
                        break;
                }

                if (value < 100713)
                {
                    if (value != previousValue)
                    {
                        MessageBox.Show("Please Contact DIGITALGRID, INC. and ship relay back to factor for upgrade", "Relay Upgrade");
                        this.firstCheckForUpdate = false;
                    }
                }

                // Only recompute pending master reprogram when idle / not already in a runtime flow.
                if (this.State == RelayProgrammingStates.Idle)
                {
                    this.reprogramMaster = (this.remoteMasterRevisionNumber < _masterCodeRevisionNumber);
                }

                // Continuation sequence only when not already in a programming cycle.
                if (this.autoLoad && this.State == RelayProgrammingStates.WaitingForBootMaster)
                    this.startMasterProgramming();
                else if (this.autoLoad && this.State == RelayProgrammingStates.WaitingForBootRelay)
                    this.startRelayProgramming();
                else if (this.autoLoad && this.State == RelayProgrammingStates.WaitingForBootFPGA && this.transmitterEnabled)
                    this.programFPGA();
            }
        }

        public UInt32 RelayRevisionNumber
        {
            get { return this.remoteRelayRevisionNumber; }
            set
            {
                this.remoteRelayRevisionNumber = value;

                // Prevent setter-driven relatch while a run is active.
                if (this.IsProgrammingActiveOrInFlight())
                {
                    logger.Info(
                        "RelayRevisionNumber setter suppressed during active run. state={0}, autoLoad={1}, reprogrammingInProgress={2}, programBootCodeInProgress={3}, value={4}",
                        this.State,
                        this.autoLoad,
                        this.reprogrammingInProgress,
                        this.programBootCodeInProgress,
                        value);

                    return;
                }

                // Manual reload still forces reprogram, but only when the state machine is not in mid-run.
                if (this.remoteRelayRevisionNumber < _relayCodeRevisionNumber || this.manualReload)
                    this.reprogramRelay = true;
                else
                    this.reprogramRelay = false;
            }
        }

        public UInt32 FPGARevisionNumber
        {
            get { return this.remoteFPGARevisionNumber; }
            set
            {
                if (value == 0xFFFFFF)
                    this.remoteFPGARevisionNumber = 0;
                else
                    this.remoteFPGARevisionNumber = value;

                // Prevent setter-driven relatch while a run is active.
                if (this.IsProgrammingActiveOrInFlight())
                {
                    logger.Info(
                        "FPGARevisionNumber setter suppressed during active run. state={0}, autoLoad={1}, reprogrammingInProgress={2}, programBootCodeInProgress={3}, value={4}",
                        this.State,
                        this.autoLoad,
                        this.reprogrammingInProgress,
                        this.programBootCodeInProgress,
                        value);

                    return;
                }

                if (this.manualReload || (this.remoteFPGARevisionNumber < _fPGACodeRevisionNumber && this.TransmitterEnabled))
                    this.reprogramFPGA = true;
                else
                    this.reprogramFPGA = false;
            }
        }


        private bool reprogramMaster = false;
        private bool reprogramRelay = false;
        private bool reprogramFPGA = false;
#pragma warning disable CS0414 // The field is assigned but its value is never used
        private bool resumeProgrammingAfterBackup = false;
#pragma warning restore CS0414
        // initiaLoad is required because loading the relay from the boot code requires loading master first.  Once loaded, it is safer to load relay code first.

        private bool gERelay = false;
        private bool gERelaySerialMatch = true;
        private bool dNPRelay = false;
        private bool transmitterEnabled = false;
        private RelayProgrammingStates state;
        private RelayProgrammingData masterCode = new RelayProgrammingData(1024);
        private RelayProgrammingData relayCode = new RelayProgrammingData(1024);
        private FPGAProgrammingData fPGACode = new FPGAProgrammingData();
        public UInt32 remoteMasterRevisionNumber = 0;
        private UInt32 remoteRelayRevisionNumber = 0;
        private UInt32 remoteFPGARevisionNumber = 0;
        private UInt32 failCount = 0;
        private UInt32 serialNumber;
        private bool useDefaultSettings = false;
        private bool manualReload = false;
        private bool dontReloadFromResource = false;
        private bool serialNumberError = false;
        private ToolTip toolTip = new ToolTip();
        private Customers customer = Customers.None;
        private List<CustomerLoadFiles> customersFiles = new List<CustomerLoadFiles>();

        private CodeReloaderSingleRelay currentRelayLog = new CodeReloaderSingleRelay();

        private void initializeToolTip()
        {
            this.toolTip.SetToolTip(this.buttonFirstLoad, "Use Customer Firmware to Program Relay with only Bootcode");
            this.toolTip.SetToolTip(this.buttonLoadNewest, "Selects the newest files in the /output/ folders and programs relay");
            this.toolTip.SetToolTip(this.buttonProgramRelay, "Programs Just the Relay Processor");
            this.toolTip.SetToolTip(this.buttonProgramMaster, "Programs Just the Master Processor");
            this.toolTip.SetToolTip(this.buttonProgramFPGA, "Programs Just the FPGA code");
            this.toolTip.SetToolTip(this.buttonStartAutoLoad, "Autoloads entire relay with either selected files, or files that will auto-load for customers");
        }

        private void initializeCustomerComboBox()
        {
            this.comboBoxCustomer.Items.Clear();

            try
            {
                // Create an array for 

                foreach (var item in Enum.GetValues(typeof(Customers)))
                {
                    this.comboBoxCustomer.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Populating Customer ComboBox", ex);
            }
        }

        /// <summary>
        /// Creates the list of 
        /// </summary>
        private void initializeCustomerFileSets()
        {
            try
            {
                this.customersFiles.Clear();

                foreach (var cust in Enum.GetValues(typeof(Customers)))
                {
                    this.customersFiles.Add(new CustomerLoadFiles((Customers)cust));
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Creating List of all Customer Load files", ex);
            }

            try
            {
                Action<Customers, string, string, string, string> setFiles = (customer, masterWh, masterGe, relayWh, relayGe) =>
                {
                    var lf = this.customersFiles.Find(x => x.Customer.Equals(customer));
                    if (lf == null) return;

                    lf.FPGAFile.DataBytes = RelayControlLibrary.Properties.Resources.FPGAdata;

                    lf.MasterFileWH = masterWh;
                    lf.MasterFileGE = masterGe;
                    lf.MasterFileWHDNP = masterWh;
                    lf.MasterFileGEDNP = masterGe;
                    lf.MasterFileDNPPLC = masterWh;

                    lf.RelayFileWH = relayWh;
                    lf.RelayFileGE = relayGe;
                };

                setFiles(Customers.BGE,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);

                setFiles(Customers.COMED,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);

                setFiles(Customers.CONED,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__CONED_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__CONED_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_conEdison_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_conEdison_20260127);

                setFiles(Customers.DOMINION,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);

                setFiles(Customers.ENMAX,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);

                setFiles(Customers.EVERSOURCE,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);

                setFiles(Customers.LONDON_HYDRO,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);

                setFiles(Customers.ONCOR,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__ONCOR_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__ONCOR_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);

                setFiles(Customers.PSEG,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);

                setFiles(Customers.SCE,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__SCE_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__SCE_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);

                setFiles(Customers.SCL,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);

                setFiles(Customers.TAUNTON,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__DGI_SEC_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);

                setFiles(Customers.TORONTO_HYDRO,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__TORONTO_HYDRO_HBD_260222,
                    RelayControlLibrary.Properties.Resources.MasterProcessor__TORONTO_HYDRO_HBD_GE_260222,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_20260127,
                    RelayControlLibrary.Properties.Resources.RelayProcessor_GE_20260127);
            }
            catch (Exception ex)
            {
                this.errorHandler("Error creating Customer File Lists", ex);
            }
        }

        public void setConEdFiles()
        {
            this.initializeCustomerFileSets(); // update programming files for ConEd based on what is currently stored in the relay
        }

        private bool AnyFirmwarePending()
        {
            return this.reprogramMaster || this.reprogramRelay || this.reprogramFPGA;
        }

        public event EventHandler AutoloadDeclined;

        private void RaiseAutoloadDeclined()
        {
            AutoloadDeclined?.Invoke(this, EventArgs.Empty);
        }

        private void RefreshPendingFirmwareFromCurrentRevisions()
        {
            if (ManualUpdate.usingManualMode)
            {
                logger.Info("Manual update path: forcing full update; bypassing version-based pending refresh.");
                this.reprogramMaster = true;
                this.reprogramRelay = true;
                this.reprogramFPGA = this.transmitterEnabled;
                return;
            }

            this.reprogramMaster =
                this.remoteMasterRevisionNumber > 0 &&
                this.remoteMasterRevisionNumber < _masterCodeRevisionNumber;

            this.reprogramRelay =
                this.remoteRelayRevisionNumber > 0 &&
                this.remoteRelayRevisionNumber < _relayCodeRevisionNumber;

            this.reprogramFPGA =
                this.transmitterEnabled &&
                this.remoteFPGARevisionNumber > 0 &&
                this.remoteFPGARevisionNumber < _fPGACodeRevisionNumber;

            logger.Info(
                "RefreshPendingFirmwareFromCurrentRevisions | masterRemote={0}, masterReq={1}, relayRemote={2}, relayReq={3}, fpgaRemote={4}, fpgaReq={5}, tx={6} => pending(M={7},R={8},F={9})",
                this.remoteMasterRevisionNumber, _masterCodeRevisionNumber,
                this.remoteRelayRevisionNumber, _relayCodeRevisionNumber,
                this.remoteFPGARevisionNumber, _fPGACodeRevisionNumber,
                this.transmitterEnabled,
                this.reprogramMaster, this.reprogramRelay, this.reprogramFPGA);
        }

        private bool ContinueAutoloadAfterBootCheck()
        {
            this.RefreshPendingFirmwareFromCurrentRevisions();
            logger.Info(
                "ContinueAutoloadAfterBootCheck ENTER: state={0}, bootSet={1}, bootRev={2}, bootOld={3}, pendingFirmware={4}, approved={5}",
                this.State,
                this.masterBootRevisionSet,
                this.masterBootRevisionNumberReceived,
                this.masterBootRevisionNumberReceived > 0 && this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber,
                this.AnyFirmwarePending(),
                this.IsBootUpdateApproved());

            if (this.IsBootOnlyManualRequired())
            {
                logger.Info("Boot-only/manual exit: no real firmware update pending; returning to idle.");
                this.ResetAutoloadState();
                this.RaiseAutoloadDeclined();
                return false;
            }

            if (!this.AnyFirmwarePending())
            {
                logger.Info("No firmware pending after boot check; ending autoload flow cleanly.");
                this.ResetAutoloadState();
                this.RaiseAutoloadDeclined();
                return false;
            }

            bool bootNeedsLoad = this.IsBootLoadRequired();

            if (bootNeedsLoad && !this.IsBootUpdateApproved())
            {
                logger.Info("Boot needs repair/update but approval is not yet set; prompting now.");

                // This is the missing step in the current flow.
                this.showAutoLoadDialog();

                if (!this.IsBootUpdateApproved())
                {
                    logger.Info("User declined autoload after fresh boot read; restoring normal comms state.");
                    this.ResetAutoloadState();   // critical
                    this.RaiseAutoloadDeclined();
                    this.NotPollingPort = false; // if your comm loop checks this
                    return false;
                }
            }

            if (!bootNeedsLoad)
            {
                if (!this.AnyFirmwarePending())
                {
                    logger.Info("No firmware pending after fresh boot read; stopping cleanly.");
                    this.ResetAutoloadState();
                    this.RaiseAutoloadDeclined();
                    return false;
                }

                // NEW: enforce full approval dialog sequence first
                if (!this.IsBootUpdateApproved())
                {
                    logger.Info("Boot current but firmware pending; requesting full autoload approval dialogs.");
                    this.showAutoLoadDialog();

                    if (!this.IsBootUpdateApproved())
                    {
                        logger.Info("User declined firmware update after boot check.");
                        this.ResetAutoloadState();
                        this.RaiseAutoloadDeclined();
                        return false;
                    }
                }

                // Existing start warning gate
                if (!this.startWarningAcknowledgedThisCycle)
                {
                    logger.Info("Approval granted; showing programming start warning.");
                    if (!EnsureProgrammingStartWarningAcknowledged())
                    {
                        return false;
                    }
                }

                logger.Info("Boot is current and approved; continuing standard firmware flow.");
                this.autoLoad = true;
                this.startProgramming();
                return true;
            }

            this.CheckProperMasterBootCode();

            if (this.programBootCodeOnly || this.wrongBootCodeLoaded)
            {
                logger.Info("Boot repair is active or required; entering warning/start path.");
                this.UpgradeBootCode();
                return true;
            }

            logger.Info("Boot validation passed; continuing firmware update path.");
            this.autoLoad = true;
            this.startProgramming();
            return true;
        }

        public bool InitializeAutoload()
        {
            logger.Trace("InitializeAutoLoad");

            if (this.IsBootOnlyManualRequired())
            {
                logger.Info("Boot-only repair required; suppressing autoload path in InitializeAutoload.");
                this.ResetAutoloadState();
                this.RaiseAutoloadDeclined();
                return false;
            }

            // same-cycle guard: no re-prompt if already approved or already handled
            if (this.askToUgradeShown || this.firmwareUpgradeAcceptedThisCycle || this.upgradeAutoDR == DialogResult.Yes)
            {
                logger.Info("InitializeAutoload: approval already handled this cycle; skipping duplicate prompt.");
                return true;
            }

            this.firmwareUpgradeAcceptedThisCycle = false;

            if (this.AutoloadAcceptedPendingBackup)
            {
                logger.Info("InitializeAutoload suppressed because backup is still pending.");
                return true;
            }

            if (!this.askToUgradeShown)
                this.reprogramBootCodeAuto = true;

            bool needsUpdate = CompareMasterRevisionToGUI();

            if (!this.askToUgradeShown && needsUpdate)
                this.showAutoLoadDialog();

            if (this.upgradeAutoDR == DialogResult.Yes)
            {
                if (needsUpdate && this.reprogramBootCodeAuto)
                {
                    this.autoLoad = true;

                    if (!this.MasterBootRevisionSet())
                        return true;

                    if ((this.CheckForBootCodeUpdate() && this.masterBootRevisionSet) ||
                        (this.CheckForProperBootCodeAutoUpdate() && this.masterBootRevisionSet))
                    {
                        this.UpgradeBootCode();
                    }
                }
                else
                {
                    this.autoLoad = false;
                }
            }
            else
            {
                this.reprogramBootCodeAuto = false;
                this.NotPollingPort = false;
                this.ResetAutoloadState();
                this.RaiseAutoloadDeclined();
                this.askToUgradeShown = true;
                return false;
            }

            if ((!this.dontShowRelayUpgradeMessage &&
                 !this.ProgramBootCodeInProgress &&
                 this.masterBootRevisionSet &&
                 this.upgradeAutoDR == DialogResult.Yes) ||
                !this.askToUgradeShown)
            {
                this.CheckForUpdate();
            }

            return true;
        }

        private bool MasterBootRevisionSet()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            // Backup gate is a hard stop before any further boot/programming progression.
            if (this.AutoloadAcceptedPendingBackup)
            {
                logger.Info("Deferring boot revision reset because backup is still pending.");
                return false;
            }

            // If the relay is in a boot-only/manual state, do not continue normal autoload flow.
            if (this.IsBootOnlyManualRequired())
            {
                logger.Info("Boot-only/manual path required; skipping standard autoload continuation.");
                this.State = RelayProgrammingStates.Idle;
                return false;
            }

            // If boot is stale or unknown, do not continue to normal firmware flow.
            if (this.IsBootLoadRequired())
            {
                bool bootUnknown = !this.masterBootRevisionSet || this.masterBootRevisionNumberReceived <= 0;
                bool bootOld = this.masterBootRevisionSet &&
                               this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber;

                logger.Info(
                    "Boot revision is not ready for normal flow. bootUnknown={0}, bootOld={1}, current={2}, required={3}",
                    bootUnknown,
                    bootOld,
                    this.masterBootRevisionNumberReceived,
                    _bootCodeRevisionNumber);

                
                this.state = RelayProgrammingStates.AutoLoadCheckBoot;
                this.sendReset();
                return false;
            }

            // Boot is current enough; normal continuation is allowed.
            logger.Info(
                "Boot revision is current enough; continue normal programming flow. current={0}, required={1}",
                this.masterBootRevisionNumberReceived,
                _bootCodeRevisionNumber);

            return true;
        }

        private DialogResult showManualLoadDialog(bool forcedFullUpdate)
        {
            DialogResult result = DialogResult.No;
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            Cursor previousCursor = Cursor.Current;
            bool previousUseWait = Application.UseWaitCursor;

            try
            {
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;

                logger.Info("POPUP SHOW: GE_WH_SELECT");
                result = new CustomYesNoDialog(
                    "GE or WH Select",
                    "Is this a GE or WH style relay?",
                    "GE",
                    "WH"
                ).ShowDialog();
                logger.Info("POPUP RESULT: GE_WH_SELECT result={0}", result);

                this.internalGESetter = (result == DialogResult.Yes);

                logger.Info("POPUP SHOW: CONFIRM_UPDATE_10MIN");

                string confirmMessage = forcedFullUpdate
                    ? "Please confirm update request.\r\n" +
                      "This will reprogram Boot, Master, Relay, and FPGA regardless of current versions.\r\n" +
                      "Relay update can take up to 10 minutes to complete."
                    : "Please confirm update request.\r\n" +
                      "Relay update can take up to 10 minutes to complete.";

                DialogResult confirmResult = MessageBox.Show(
                    confirmMessage,
                    "Confirm Update Request",
                    MessageBoxButtons.YesNo);

                logger.Info("POPUP RESULT: CONFIRM_UPDATE_10MIN result={0}", confirmResult);

                return confirmResult;
            }
            finally
            {
                Application.UseWaitCursor = previousUseWait;
                Cursor.Current = previousCursor ?? Cursors.Default;
            }
        }

        private void showAutoLoadDialog()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (this.askToUgradeShown || this.firmwareUpgradeAcceptedThisCycle || this.upgradeAutoDR == DialogResult.Yes)
            {
                logger.Info("showAutoLoadDialog suppressed; approval already handled for this cycle.");
                return;
            }

            Cursor previousCursor = Cursor.Current;
            bool previousUseWait = Application.UseWaitCursor;

            try
            {
                // Force normal cursor while user must click dialogs
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;

                this.upgradeAutoDR = DialogResult.No;

                if (!ManualUpdate.usingManualMode)
                {
                    this.upgradeAutoDR = showAutoLoadUpdateMessage();

                    if (this.upgradeAutoDR != DialogResult.Yes)
                    {
                        this.askToUgradeShown = true;
                        this.firmwareUpgradeAcceptedThisCycle = false;
                        logger.Info("showAutoLoadDialog: user declined AUTOLOAD_NEWER_FW; forcing ResetAutoloadState().");
                        this.ResetAutoloadState();
                        this.RaiseAutoloadDeclined();
                        return;
                    }
                }

                logger.Info("POPUP SHOW: GE_WH_SELECT");
                DialogResult relayTypeDR = new CustomYesNoDialog(
                    "GE or WH Select",
                    "Is this a GE or WH style relay?",
                    "GE",
                    "WH").ShowDialog();
                logger.Info("POPUP RESULT: GE_WH_SELECT result={0}", relayTypeDR);

                internalGESetter = (relayTypeDR == DialogResult.Yes);

                logger.Info("POPUP SHOW: CONFIRM_UPDATE_10MIN");
                this.upgradeAutoDR = MessageBox.Show(
                    "Please confirm update request.\r\nRelay update can take up to 10 minutes to complete.",
                    "Confirm Update Request",
                    MessageBoxButtons.YesNo);
                logger.Info("POPUP RESULT: CONFIRM_UPDATE_10MIN result={0}", this.upgradeAutoDR);

                this.askToUgradeShown = true;
                this.firmwareUpgradeAcceptedThisCycle = (this.upgradeAutoDR == DialogResult.Yes);
            }
            finally
            {
                // Always restore cursor state
                Application.UseWaitCursor = previousUseWait;
                Cursor.Current = previousCursor ?? Cursors.Default;
            }
        }

        private bool finalSuccessPopupShownThisCycle = false;

        private void ShowFinalSuccessPopupOnce()
        {
            if (this.finalSuccessPopupShownThisCycle)
                return;

            this.finalSuccessPopupShownThisCycle = true;

            logger.Info("POPUP SHOW: REPROGRAM_SUCCESS");
            MessageBox.Show("Reprogram Completed Successfully", "Reprogramming Completed Successfully!");
            logger.Info("POPUP RESULT: REPROGRAM_SUCCESS result=Shown");
        }

        private DialogResult ShowProgrammingStartWarning()
        {
            logger.Info("ShowProgrammingStartWarning ENTER");
            logger.Info("POPUP SHOW: START_WARNING_DO_NOT_REMOVE_PORT");

            const string message =
                "Relay update is starting.\r\n\r\n" +
                "The relay may temporarily disconnect and reconnect during this process.\r\n" +
                "Progress will be shown in the programming window.\r\n\r\n" +
                "Do not remove the port, power down the relay, let the computer sleep, or click around the GUI until the update finishes.";

            DialogResult result = MessageBox.Show(
                message,
                "Relay Update In Progress",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1);

            logger.Info("POPUP RESULT: START_WARNING_DO_NOT_REMOVE_PORT result={0}", result);
            logger.Info($"ShowProgrammingStartWarning EXIT result={result}");

            return result;
        }

        private void UpgradeBootCode()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.dontShowRelayUpgradeMessage = true;

            this.programmingForm.ClearAllChecks();

            // was: DialogResult warningBootDR = ShowProgrammingStartWarning();
            if (!EnsureProgrammingStartWarningAcknowledged())
            {
                this.autoLoad = false;
                this.dontShowRelayUpgradeMessage = false;
                this.firmwareUpgradeAcceptedThisCycle = false;
                this.upgradeAutoDR = DialogResult.No;
                return;
            }

            this.dontShowRelayUpgradeMessage = false;

            this.firmwareUpgradeAcceptedThisCycle = true;
            this.upgradeAutoDR = DialogResult.Yes;
            this.autoLoad = true;

            logger.Info("UpgradeBootCode: boot repair approved; deferring normal firmware start until after bootloader reset and fresh boot read.");
        }

        private DialogResult showAutoLoadUpdateMessage()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            logger.Info("POPUP SHOW: AUTOLOAD_NEWER_FW");

            DialogResult dR = MessageBox.Show(
                "Newer Firmware is available to update the Relay. It is necessary that the update be completed.\r\nClick Yes to begin update",
                "Relay Code Updater",
                MessageBoxButtons.YesNo);

            logger.Info("POPUP RESULT: AUTOLOAD_NEWER_FW result={0}", dR);
            return dR;
        }

        public void InitialAutoLoadFiles()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            DialogResult dR;

            dR = MessageBox.Show("Do you want to attempt to reprogram the Relay?", "Initial Auto Reload", MessageBoxButtons.YesNo);

            if (dR != DialogResult.Yes)
                return;

            if (dR == DialogResult.No)
                return;

            this.dontReloadFromResource = false;
            this.useDefaultSettings = true;

            switch (this.customer)
            {
                default:
                    dR = new CustomYesNoDialog("GE or WH Select", "Is this a GE or WH style relay?", "GE", "WH").ShowDialog();
                    if (dR == DialogResult.Yes)
                        internalGESetter = true;
                    else
                        internalGESetter = false;
                    break;
            }
            reprogramRelay = true;
#if !DNP
            // This is a non-DNP, transmitter Enabled Relay
            this.TransmitterEnabled = true;
            this.DNPRelay = false;
            this.reprogramFPGA = true;
#elif DNP
            this.DNPRelay = true;
#if PLC
            this.TransmitterEnabled = true;
            this.reprogramFPGA = true;
#else
            this.TransmitterEnabled = false;
            this.reprogramFPGA = false;
#endif
#endif

        }

        public void StartManualForcedUpdate()
        {
            this.startManualReloadWithBootCheck();
        }

        public void ResumeAutoloadAfterBackup()
        {
            logger.Info("ResumeAutoloadAfterBackup ENTER");

            this.RefreshPendingFirmwareFromCurrentRevisions();

            if (this.reprogrammingInProgress || this.programBootCodeInProgress)
            {
                logger.Warn("ResumeAutoloadAfterBackup suppressed; programming already active.");
                return;
            }

            if (this.AutoloadAcceptedPendingBackup)
            {
                logger.Warn("ResumeAutoloadAfterBackup suppressed; backup gate still latched.");
                return;
            }

            if (this.IsBootOnlyManualRequired())
            {
                logger.Info("Boot-only/manual path required; aborting resume and returning to idle.");
                this.ResetAutoloadState();
                this.RaiseAutoloadDeclined();
                return;
            }

            if (!this.AnyFirmwarePending())
            {
                logger.Info("ResumeAutoloadAfterBackup: no firmware pending; exiting idle.");
                this.ResetAutoloadState();
                this.RaiseAutoloadDeclined();
                return;
            }

            // 1) If boot is unknown, force a fresh boot read before showing any approval popup.
            if (this.IsBootLoadRequired() && !this.masterBootRevisionSet)
            {
                logger.Info("Boot unknown after backup; forcing fresh boot read before any approval prompt.");
                this.State = RelayProgrammingStates.AutoLoadCheckBoot;
                this.sendReset();
                return;
            }

            // 2) If boot is known and still stale, then this is the real prompt condition.
            if (this.IsBootLoadRequired() && this.masterBootRevisionSet && !this.IsBootUpdateApproved())
            {
                logger.Info("Boot stale after fresh read; requesting approval before resume.");
                this.showAutoLoadDialog();

                if (this.upgradeAutoDR != DialogResult.Yes)
                {
                    logger.Info("User declined autoload after backup.");
                    this.ResetAutoloadState();
                    this.RaiseAutoloadDeclined();
                    return;
                }

                this.firmwareUpgradeAcceptedThisCycle = true;
                this.upgradeAutoDR = DialogResult.Yes;
            }

            // 3) Boot is valid or user approved; continue the flow.
            logger.Info("Approved path after backup -> requesting fresh boot read.");
            this.State = RelayProgrammingStates.AutoLoadCheckBoot;
            this.sendReset();
        }

        // 2) Remove dead locals in manual reload path
        private void startManualReloadWithBootCheck()
        {
            logger.Info("Manual update path: forced full firmware update, bypassing boot-check logic.");

            this.manualReload = true;
            this.reloadBootWithPrompt = false;
            this.autoLoad = false;

            this.reprogramMaster = true;
            this.reprogramRelay = true;
            this.reprogramFPGA = this.transmitterEnabled;

            this.askToUgradeShown = false;

            this.upgradeAutoDR = this.showManualLoadDialog(true);

            if (this.upgradeAutoDR != DialogResult.Yes)
            {
                this.firmwareUpgradeAcceptedThisCycle = false;
                logger.Info("Manual path: user declined forced full update.");
                return;
            }

            if (!EnsureProgrammingStartWarningAcknowledged())
            {
                logger.Info("Manual path: user cancelled start warning; aborting.");
                return;
            }

            this.programmingForm.ClearAllChecks();
            this.setProgrammingFiles();

            // Full manual update must start with BOOT first, then continue with master/relay/fpga
            this.programBootCodeOnly = false;
            this.ProgramBootCodeStart = true;
            return;
        }


        private void setProgrammingFiles()
        {
            logger.Trace("Method: {0}", nameof(setProgrammingFiles));
            if (this.dontReloadFromResource)
                return;

            CustomerLoadFiles cLF = null;
            string customerDisplayName = null;

#if DEBUG || ENGINEERING
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.ENMAX));
            customerDisplayName = "ENMAX";
#elif BGE
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.BGE));
            customerDisplayName = "BGE";
#elif COMED
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.COMED));
            customerDisplayName = "COMED";
#elif CONED
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.CONED));
            customerDisplayName = "CONED";
#elif DOMINION
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.DOMINION));
            customerDisplayName = "DOMINION";
#elif ENMAX
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.ENMAX));
            customerDisplayName = "ENMAX";
#elif EVERSOURCE
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.EVERSOURCE));
            customerDisplayName = "EVERSOURCE";
#elif LONDON_HYDRO
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.LONDON_HYDRO));
            customerDisplayName = "LONDON_HYDRO";
#elif ONCOR
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.ONCOR));
            customerDisplayName = "ONCOR";
#elif PSEG
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.PSEG));
            customerDisplayName = "PSEG";
#elif SCE
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.SCE));
            customerDisplayName = "SCE";
#elif SCL
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.SCL));
            customerDisplayName = "SCL";
#elif TAUNTON
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.TAUNTON));
            customerDisplayName = "TAUNTON";
#elif TORONTO_HYDRO
            cLF = this.customersFiles.Find(x => x.Customer.Equals(Customers.TORONTO_HYDRO));
            customerDisplayName = "TORONTO_HYDRO";
#else
            throw new Exception("No supported customer build symbol is defined.");
#endif

            if (cLF == null)
                throw new Exception("Customer load files not initialized for active build symbol.");

            if (this.GERelay)
            {
                this.masterCode.FileString = cLF.MasterFileGE;
                this.textBoxMasterFileName.Text = "GE Master Relay From Resource " + customerDisplayName;

                this.relayCode.FileString = cLF.RelayFileGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource " + customerDisplayName;
            }
            else
            {
                this.masterCode.FileString = cLF.MasterFileWH;
                this.textBoxMasterFileName.Text = "WH Master Relay From Resource " + customerDisplayName;

                this.relayCode.FileString = cLF.RelayFileWH;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + customerDisplayName;
            }

            if (string.IsNullOrWhiteSpace(this.masterCode.FileString))
                throw new Exception("Master firmware resource was not loaded for customer " + customerDisplayName);

            if (string.IsNullOrWhiteSpace(this.relayCode.FileString))
                throw new Exception("Relay firmware resource was not loaded for customer " + customerDisplayName);

            if (this.transmitterEnabled)
            {
                this.parseFPGAFile(this.fPGACode);
                this.textBoxFPGAFile.Text = "FPGA Code From Resource";
            }

            this.parseSFile(this.masterCode);
            this.parseSFile(this.relayCode);

            logger.Trace("MP: " + this.textBoxMasterFileName.Text +
                         " RP: " + this.textBoxRelayFileName.Text +
                         " FPGA: " + this.textBoxFPGAFile.Text);
        }


        formProgrammingProgess programmingForm = new formProgrammingProgess();

        private bool firstCheckForUpdate = true;

        public void CheckForUpdate()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            bool bootStillStale =
                this.masterBootRevisionSet &&
                this.masterBootRevisionNumberReceived > 0 &&
                this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber;

            if (bootStillStale)
            {
                logger.Info(
                    "CheckForUpdate suppressed: boot still stale (rev={0}, required={1}); waiting for fresh BootReceived.",
                    this.masterBootRevisionNumberReceived,
                    _bootCodeRevisionNumber);
                return;
            }

            // NEW GUARD: boot-only repair is manual-only; never enter autoload flow.
            if (this.IsBootOnlyManualRequired())
            {
                logger.Info("Boot-only repair required; suppressing autoload path in CheckForUpdate.");
                this.ResetAutoloadState();
                this.RaiseAutoloadDeclined();
                return;
            }

            if (!this.firstCheckForUpdate)
                return;

            this.firstCheckForUpdate = false;

            if (!(this.reprogramFPGA || this.reprogramMaster || this.reprogramRelay))
            {
                logger.Trace("No Updated Needed");
                return;
            }

            this.transmitterEnabled = true;

            // Keep existing serial/type mismatch prompt behavior
            if (!this.gERelaySerialMatch && !this.serialNumberError)
                this.askIfGERelay();

            this.setProgrammingFiles();

            // OLD-BOOT / deferred autoload continuation must use full dialog sequence:
            // Newer Firmware -> GE/WH -> confirm -> then startAutoLoad (which shows warning popup).
            bool bootOld = this.masterBootRevisionSet &&
                           this.masterBootRevisionNumberReceived > 0 &&
                           this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber;

            bool deferredOldBootFlow = bootOld && !ManualUpdate.usingManualMode;

            if (deferredOldBootFlow)
            {
                logger.Info(
                    "CheckForUpdate: old boot detected; upgradeAcceptedThisCycle={0}. BootRev={1}, Required={2}",
                    this.firmwareUpgradeAcceptedThisCycle,
                    this.masterBootRevisionNumberReceived,
                    _bootCodeRevisionNumber);

                if (this.firmwareUpgradeAcceptedThisCycle)
                {
                    logger.Info("CheckForUpdate: stale boot but upgrade already accepted in this cycle; continuing without re-prompt.");
                }
                else
                {
                    this.askToUgradeShown = false;
                    this.upgradeAutoDR = DialogResult.No;

                    this.showAutoLoadDialog();

                    if (this.upgradeAutoDR != DialogResult.Yes)
                    {
                        logger.Info("CheckForUpdate: user declined autoload in full dialog flow. Forcing ResetAutoloadState().");
                        this.ResetAutoloadState();
                        this.RaiseAutoloadDeclined();
                        return;
                    }

                    this.firmwareUpgradeAcceptedThisCycle = true;
                }
            }

            this.startAutoLoad();
        }

        public bool CheckForBootCodeUpdate()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (masterBootRevisionNumberReceived < _bootCodeRevisionNumber)
                return true;
            else
                return false;
        }

        public bool CompareMasterRevisionToGUI()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            //MessageBox.Show("remoteMasterRevisionNumber : " + remoteMasterRevisionNumber + " AND _masterCodeRevisionNumber : " + _masterCodeRevisionNumber + " wrongRelayTypeAutoLoad : " + wrongRelayTypeAutoLoad); // Only for testing - to be removed
            if (remoteMasterRevisionNumber < _masterCodeRevisionNumber)
            {
                return true;
            }
            else
                return false;
        }

        private void askIfGERelay()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            DialogResult dR = new CustomYesNoDialog(
                "Serial Number and Relay Type Mismatch",
                "Is this a GE or WH style relay?",
                "GE",
                "WH").ShowDialog();

            if (dR == DialogResult.Yes)
            {
                // 3076 = default/fallback serial when relay memory is bad; exclude from GE/WH serial validity checks
                if (this.serialNumber != 3076 &&
                    (this.serialNumber < 25000 || this.serialNumber > 32767 || this.serialNumber == 0)) //25k and up are GE serial Numbers
                {
                    MessageBox.Show("Bad Serial Number!", "Problem with Serial Number. \r\nPlease Contact DIGITALGRID, INC.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.serialNumberError = true;
                }

                internalGESetter = true;
                this.addGERelayToTransmitterPacket(true);
            }
            else
            {
                // 3076 = default/fallback serial when relay memory is bad; exclude from GE/WH serial validity checks
                if (this.serialNumber != 3076 && (this.serialNumber >= 25000 || this.serialNumber == 0))
                {
                    this.serialNumberError = true;
                    MessageBox.Show("Bad Serial Number!", "Problem with Serial Number. \r\nPlease Contact DIGITALGRID, INC.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                internalGESetter = false;
                this.addGERelayToTransmitterPacket(false);
            }
        }

        private void addGERelayToTransmitterPacket(bool b)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (b)
                this.TransmitterPacket[28] |= 0x10;
            else
                this.TransmitterPacket[28] &= 0xEF;
        }


        // 1) Replace repeated pending-update logic with the helper
        private void startAutoLoad()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (this.IsBootOnlyManualRequired())
            {
                logger.Info("Boot-only repair required; suppressing startAutoLoad autoload flow; manual programming required.");
                this.ResetAutoloadState();
                this.RaiseAutoloadDeclined();
                return;
            }

            if (this.reprogrammingInProgress || this.programBootCodeInProgress)
            {
                logger.Info("startAutoLoad suppressed; programming already active. Continuing current flow.");
                return;
            }

            if (this.State == RelayProgrammingStates.DoneLoadingMasterBootLoader ||
                this.State == RelayProgrammingStates.Idle)
            {
                this.reprogrammingInProgress = false;
                this.programBootCodeInProgress = false;
                this.autoLoad = false;
            }

            if (this.firmwareUpgradeAcceptedThisCycle &&
                (this.AutoloadAcceptedPendingBackup || this.reprogrammingInProgress || this.programBootCodeInProgress))
            {
                logger.Info("startAutoLoad suppressed: autoload already accepted for this cycle.");
                return;
            }

            if (!this.AnyFirmwarePending())
            {
                logger.Info("startAutoLoad: no updates required; skipping prompts and programming.");
                this.ResetAutoloadState();
                this.RaiseAutoloadDeclined();
                return;
            }

            DialogResult dR;

            if (this.serialNumberError)
            {
                return;
            }

            if (ManualUpdate.usingManualMode)
            {
                dR = this.showManualLoadDialog(true);
            }
            else if (forceRelayUpdate == false && askToUgradeShown == false && programmingForm.MasterBootComplete == false)
            {
                this.showAutoLoadDialog();
                dR = this.upgradeAutoDR;
                this.firmwareUpgradeAcceptedThisCycle = (this.upgradeAutoDR == DialogResult.Yes);
                this.askToUgradeShown = true;
            }
            else
            {
                dR = DialogResult.Yes;
            }

            if (dR == DialogResult.Yes)
            {
                if (!this.dontReloadFromResource && programmingForm.MasterBootComplete == false)
                {
                    if (!EnsureProgrammingStartWarningAcknowledged())
                    {
                        this.autoLoad = false;
                        return;
                    }
                }

                RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
                rPEA.Command = RelayProgrammingSendCommands.SaveSettings;
                this.onSend(rPEA);

                if (!this.dontReloadFromResource && programmingForm.MasterBootComplete == false)
                {
                    logger.Info("Startup backup policy active: skipping backup trigger in startAutoLoad.");
                    this.AutoloadAcceptedPendingBackup = false;
                }

                logger.Trace("User Verified Programming Start");

                bool bootUnknown = !this.masterBootRevisionSet || this.masterBootRevisionNumberReceived <= 0;
                bool bootOld = this.masterBootRevisionSet &&
                               this.masterBootRevisionNumberReceived > 0 &&
                               this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber;

                // HARDENING: auto path must not continue normal flow with unknown boot state.
                // Force a fresh boot read first, then let AutoLoadCheckBoot decide.
                if (bootUnknown && !ManualUpdate.usingManualMode)
                {
                    logger.Warn("startAutoLoad: boot revision unknown in auto mode; deferring programming until fresh boot read.");
                    this.State = RelayProgrammingStates.AutoLoadCheckBoot;
                    this.sendReset();
                    return;
                }

                if (bootOld)
                {
                    this.CheckProperMasterBootCode();

                    if (this.programBootCodeOnly || this.wrongBootCodeLoaded)
                    {
                        logger.Info("startAutoLoad: boot repair already scheduled/active; exiting without re-entry.");
                        return;
                    }
                }

                Thread.Sleep(500);
                this.autoLoad = true;

                if (!programmingForm.MasterBootComplete)
                    this.programmingForm.ClearAllChecks();

                this.startProgramming();

                if (!this.programmingForm.Visible)
                    this.programmingForm.Show();
            }
            else
            {
                this.ResetAutoloadState();
                this.RaiseAutoloadDeclined();
            }
        }

        public void PrepForBoot()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            switch (this.State)
            {
                case RelayProgrammingStates.LoadingMasterData:
                case RelayProgrammingStates.LoadingMasterCode:
                case RelayProgrammingStates.WaitingForBootMaster:
                    this.resetMasterProgramming();
                    break;
                case RelayProgrammingStates.LoadingRelayCode:
                case RelayProgrammingStates.LoadingRelayData:
                case RelayProgrammingStates.WaitingForBootRelay:
                    this.resetProgrammingRelay();
                    break;
                case RelayProgrammingStates.WaitingForBootFPGA:
                case RelayProgrammingStates.LoadingFPGACode:
                    this.resetProgrammingFPGA();
                    break;
            }
            this.labelState.Text = "Waiting For Boot";
            logger.Trace("Waiting For Boot - " + this.state.ToString());
        }

        public void SetTransmitterPacket(byte[] bytePacket)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.TransmitterPacket = bytePacket;
        }

        private void SendTransmitterSettings()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            if (this.TransmitterPacket != null || this.manualReload)
            {
                rPEA.BytesToSend = this.TransmitterPacket;
                rPEA.Command = RelayProgrammingSendCommands.TransmitterSettings;

                if (this.TransmitterPacket != null)
                    this.onSend(rPEA);

                this.TransmitterPacket = null;

                logger.Trace("Updated Transmitter Settings with Stored Settings");
            }
        }

        private delegate void booleanInvoke(bool b);

        private void onSend(RelayProgrammingEventArgs rPEA)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            logger.Trace(String.Format("Command: {0}", rPEA.Command));

            // Allow RequestAll in final success/finalization states.
            if (rPEA.Command == RelayProgrammingSendCommands.RequestAll)
            {
                bool isFinalizationState =
                    this.State == RelayProgrammingStates.ReprogramSuccess ||
                    this.State == RelayProgrammingStates.Finalized ||
                    this.State == RelayProgrammingStates.WaitForAllData;

                if (!isFinalizationState &&
                    (this.State == RelayProgrammingStates.LoadingMasterBootLoader ||
                     this.State == RelayProgrammingStates.LoadingMasterCode ||
                     this.State == RelayProgrammingStates.LoadingMasterData ||
                     this.State == RelayProgrammingStates.LoadingRelayCode ||
                     this.State == RelayProgrammingStates.LoadingRelayData ||
                     this.State == RelayProgrammingStates.LoadingFPGACode ||
                     this.programBootCodeInProgress ||
                     this.reprogrammingInProgress ||
                     this.autoLoad))
                {
                    logger.Warn(
                        "Suppressing RequestAll during bootloader/programming handoff. state={0}, programBootCodeInProgress={1}, reprogrammingInProgress={2}, autoLoad={3}",
                        this.State,
                        this.programBootCodeInProgress,
                        this.reprogrammingInProgress,
                        this.autoLoad);
                    return;
                }
            }

            if (rPEA.Command == RelayProgrammingSendCommands.RestartProgram)
                Thread.Sleep(500);
            else
                Thread.Sleep(75);

            if (this.Send != null)
                this.Send(this, rPEA);
        }
        private void packetAcknowledged(bool b)
        {

            try
            {
                switch (this.State)
                {
                    default:
                    case RelayProgrammingStates.Idle:
                        break;
                    case RelayProgrammingStates.LoadingMasterCode:
                    case RelayProgrammingStates.LoadingMasterData:
                        logger.Trace("AckM, ");
                        if (this.State == RelayProgrammingStates.WaitingForBootMaster)
                            this.State = RelayProgrammingStates.LoadingMasterCode;
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.sendNextMasterPacket();
                        break;
                    case RelayProgrammingStates.LoadingRelayCode:
                    case RelayProgrammingStates.LoadingRelayData:
                        logger.Trace("AckR, ");
                        if (this.State == RelayProgrammingStates.WaitingForBootRelay)
                            this.State = RelayProgrammingStates.LoadingRelayCode;
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.sendNextRelayPacket();
                        break;
                    case RelayProgrammingStates.LoadingFPGACode:
                        logger.Trace("AckF, ");
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.sendNextFPGAPacket();
                        break;
                    case RelayProgrammingStates.ClearingBootLoader:
                        logger.Trace("AckU, ");
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.State = RelayProgrammingStates.LoadingRelayBootLoader;
                        this.sendNextRelayBootLoaderPacket();
                        break;
                    case RelayProgrammingStates.LoadingMasterBootLoader:
                        logger.Info("packetAcknowledged: LoadingMasterBootLoader ACK observed at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
                        logger.Trace("AckU, ");
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();

                        this.State = RelayProgrammingStates.LoadingMasterBootLoader;
                        logger.Info("packetAcknowledged: calling sendMasterBootCode() at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
                        this.sendMasterBootCode();
                        break;
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error in Packet Acknowledge", ex);
            }
        }

        private bool IsBootUpdateApproved()
        {
            return this.firmwareUpgradeAcceptedThisCycle
                || this.upgradeAutoDR == DialogResult.Yes;
        }

        public void PacketAcknowledged(bool b)
        {
            try
            {
                this.packetAcknowledged(b);
            }
            catch (Exception ex)
            {
                this.errorHandler("Error In CallBack Delegate in RelayProgramming", ex);
            }
        }

        public void BootReceived(string bootReceived)
        {
            this.labelState.Text = "Boot Received";
            if (this.autoLoad && !this.programmingForm.Visible && this.state != RelayProgrammingStates.AutoLoadCheckBoot)
                this.programmingForm.Show();

            logger.Trace("Boot Received - " + this.state.ToString());

            string rawBoot = bootReceived ?? string.Empty;
            string normalizedBoot = rawBoot.Trim();

            // Extract numeric portion if present.
            // Examples:
            //   "BOOT C 260116" -> 260116
            //   "BOOT 260116"   -> 260116
            //   "BOOT A BYPASS" -> no numeric portion
            string numericPart = Regex.Match(normalizedBoot, @"\d+").Value;

            this.masterBootStringReceived = normalizedBoot;
            this.bootStartUpChar = rawBoot.Length >= 3 ? rawBoot.Substring(3, 1) : "0";

            if (!string.IsNullOrEmpty(numericPart) && UInt32.TryParse(numericPart, out UInt32 parsedRevision))
            {
                this.masterBootRevisionNumberReceived = parsedRevision;
                this.masterBootRevisionSet = true;
                logger.Info("BootReceived parsed revision: {0}", parsedRevision);
            }
            else
            {
                // Critical safety behavior: unknown remains unknown.
                this.masterBootRevisionNumberReceived = 0;
                this.masterBootRevisionSet = false;
                logger.Warn("BootReceived: non-numeric/unknown boot revision. Raw='{0}'", normalizedBoot);
            }

            // Manual flow: do not evaluate boot repair or boot approval logic.
            if (this.manualReload && !this.notPollingPort && !this.programBootCodeInProgress)
            {
                logger.Info("BootReceived manual path: skipping boot repair logic; manual update is full-update mode.");
            }

            switch (this.state)
            {
                case RelayProgrammingStates.ManualPortSelectionWaitingForBoot:
                    this.startAutoLoad();
                    break;

                case RelayProgrammingStates.WaitingForBootMaster:
                    this.programmingForm.CurrentTask = "Loading Master Code";
                    this.sendMasterTransferPacket();
                    break;

                case RelayProgrammingStates.LoadingMasterCode:
                    if (this.failCount == 5)
                        this.programmingForm.CurrentTask = "Relay Did Not Respond To Master Packet - Wait for a while and then Try Manually Resetting Relay";

                    this.failCount++;
                    this.sendMasterTransferPacket();
                    break;

                case RelayProgrammingStates.WaitingForBootRelay:
                case RelayProgrammingStates.LoadingRelayCode:
                    this.programmingForm.CurrentTask = "Loading Relay Code ";
                    this.timerTimeout.Stop();
                    this.timerTimeout.Interval = 1500;
                    this.timerTimeout.Start();
                    this.sendRelayTransferPacket();
                    break;

                case RelayProgrammingStates.WaitingForBootFPGA:
                case RelayProgrammingStates.LoadingFPGACode:
                    this.programmingForm.CurrentTask = "Loading FPGA Code";
                    this.sendFPGATransferPacket();
                    break;

                case RelayProgrammingStates.CheckMasterBootCode:
                    logger.Trace("CheckMasterBootCode");
                    this.state = RelayProgrammingStates.Idle;
                    this.CheckProperMasterBootCode();
                    break;

                case RelayProgrammingStates.AutoLoadCheckBoot:
                    logger.Info(
                        "AutoLoadCheckBoot: entering continuation evaluation. AutoloadAcceptedPendingBackup={0}, masterBootRevisionSet={1}, masterBootRevisionNumberReceived={2}",
                        this.AutoloadAcceptedPendingBackup,
                        this.masterBootRevisionSet,
                        this.masterBootRevisionNumberReceived);

                    this.state = RelayProgrammingStates.Idle;

                    if (this.AutoloadAcceptedPendingBackup)
                    {
                        logger.Info("Boot read completed while backup still pending; leaving autoload in backup-controlled flow.");
                        break;
                    }

                    logger.Info("Calling ContinueAutoloadAfterBootCheck()");
                    this.ContinueAutoloadAfterBootCheck();
                    break;

                case RelayProgrammingStates.ManualLoadCheckBoot:
                    logger.Trace("ManualLoadCheckBoot");
                    this.state = RelayProgrammingStates.Idle;
                    this.startManualReloadWithBootCheck();
                    break;

                case RelayProgrammingStates.ReloadMasterBoot:
                    this.timerTimeout.Stop();
                    Thread.Sleep(3000);
                    this.programBootCodeInProgress = false;
                    this.ProgramBootCodeStart = true;
                    break;

                default:
                    break;
            }
        }


        private void sendNextRelayPacket()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            if (this.State == RelayProgrammingStates.LoadingRelayCode)
            {
                try
                {

                    rPEA.BytesToSend = new byte[1024];
                    this.programmingForm.CurrentTask = "Loading Relay Code e";
                    if (this.relayCode.CodeBytes.Count == 0)
                    {
                        this.State = RelayProgrammingStates.LoadingRelayData;
                        this.programmingForm.RelayCodeComplete = true;
                        this.programmingForm.CurrentTask = "Loading Relay Data";
                        logger.Trace("Loading Relay Data");
                        this.programmingForm.Maximum = this.relayCode.NumberOfDataBlocks;

                        this.sendNextRelayPacket();
                        return;
                    }

                    for (int i = 0; i < 1024; i++)
                    {
                        rPEA.BytesToSend[i] = this.relayCode.CodeBytes[0];
                        this.relayCode.CodeBytes.RemoveAt(0);
                    }

                    Int32 temp = Convert.ToInt32(this.labelCodeCount.Text);

                    this.programmingForm.ProgressValue = temp;
                    temp++;
                    this.labelCodeCount.Text = temp.ToString();

                    logger.Trace("RC, ");

                    this.onSend(rPEA);
                }
                catch (Exception ex)
                {
                    this.errorHandler("Error Sending Next Relay Code", ex);
                }
            }
            else if (this.State == RelayProgrammingStates.LoadingRelayData)
            {
                this.programmingForm.CurrentTask = "Loading Relay Data";
                rPEA.BytesToSend = new byte[512];

                if (this.relayCode.DataBytes.Count == 0)
                {
                    this.doneLoadingRelay();
                    return;
                }

                for (int i = 0; i < 512; i++)
                {
                    rPEA.BytesToSend[i] = this.relayCode.DataBytes[0];
                    this.relayCode.DataBytes.RemoveAt(0);
                }

                Int32 temp = Convert.ToInt32(this.labelDataCount.Text);

                logger.Trace("RD, ");
                this.onSend(rPEA);

                this.programmingForm.ProgressValue = temp;
                temp++;

                this.labelDataCount.Text = temp.ToString();

            }
            else
            {
                throw new Exception("Called send NextRelayPacket from wrong state: " + this.state.ToString());
            }
        }

        private void sendNextRelayBootLoaderPacket()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            this.failCount = 0;

            if (this.State == RelayProgrammingStates.LoadingRelayBootLoader)
            {

                if (this.relayCode.CodeBytes.Count == 0)
                {
                    // We are done
                    this.doneLoadingRelayBootLoader();
                }

                rPEA.BytesToSend = new byte[37];

                // Set up the OPCODE and term character for send packet
                rPEA.BytesToSend[0] = 0x81;
                rPEA.BytesToSend[1] = 34;
                rPEA.BytesToSend[36] = 0x0D;

                for (int i = 0; i < 34; i++)
                {
                    rPEA.BytesToSend[i + 2] = this.relayCode.CodeBytes[0];
                    this.relayCode.CodeBytes.RemoveAt(0);
                }

                logger.Trace("BL, ");

                this.onSend(rPEA);
            }
        }

        private void ResetAutoloadState([CallerMemberName] string caller = null)
        {
            logger.Info(
                "ResetAutoloadState ENTER caller={0} | pre: state={1}, autoLoad={2}, reprogrammingInProgress={3}, programBootCodeInProgress={4}, firmwareUpgradeAcceptedThisCycle={5}, upgradeAutoDR={6}, askToUgradeShown={7}, AutoloadAcceptedPendingBackup={8}, NotPollingPort={9}",
                caller,
                this.State,
                this.autoLoad,
                this.reprogrammingInProgress,
                this.programBootCodeInProgress,
                this.firmwareUpgradeAcceptedThisCycle,
                this.upgradeAutoDR,
                this.askToUgradeShown,
                this.AutoloadAcceptedPendingBackup,
                this.NotPollingPort);

            this.autoLoad = false;
            this.reprogrammingInProgress = false;
            this.programBootCodeInProgress = false;
            this.firmwareUpgradeAcceptedThisCycle = false;
            this.upgradeAutoDR = DialogResult.No;
            this.askToUgradeShown = false;
            this.AutoloadAcceptedPendingBackup = false;

            // NEW: clear warning-per-cycle gate
            this.startWarningAcknowledgedThisCycle = false;
            this.startWarningShownThisCycle = false; // only if this field exists

            this.NotPollingPort = false; // defensive: ensure comm loop can resume
            this.State = RelayProgrammingStates.Idle;

            logger.Info(
                "ResetAutoloadState EXIT caller={0} | post: state={1}, autoLoad={2}, reprogrammingInProgress={3}, programBootCodeInProgress={4}, firmwareUpgradeAcceptedThisCycle={5}, upgradeAutoDR={6}, askToUgradeShown={7}, AutoloadAcceptedPendingBackup={8}, NotPollingPort={9}",
                caller,
                this.State,
                this.autoLoad,
                this.reprogrammingInProgress,
                this.programBootCodeInProgress,
                this.firmwareUpgradeAcceptedThisCycle,
                this.upgradeAutoDR,
                this.askToUgradeShown,
                this.AutoloadAcceptedPendingBackup,
                this.NotPollingPort);
        }

        private void MasterBootLoaderStart()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            logger.Info("MasterBootLoaderStart: begin at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));

            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            int temp = 0;

            this.failCount = 0;

            this.setProgrammingFiles();

            if ((this.masterCode.FileString == "" || this.masterCode.FileString == null))
            {
                this.dontReloadFromResource = false;
                this.setProgrammingFiles();
            }

            this.programBootCodeInProgress = true;
            this.reprogrammingInProgress = true;
            this.State = RelayProgrammingStates.LoadingMasterBootLoader;

            if (this.State == RelayProgrammingStates.LoadingMasterBootLoader)
            {
                rPEA.BytesToSend = new byte[4];

                this.parseBootLoaderSFile(this.masterCode);

                if (this.masterCode.CodeBytes == null || this.masterCode.CodeBytes.Count == 0)
                {
                    logger.Error("MasterBootLoaderStart aborted: parsed boot payload is empty.");
                    this.programBootCodeInProgress = false;
                    this.reprogrammingInProgress = false;
                    this.State = RelayProgrammingStates.Idle;
                    return;
                }

                temp = (this.masterCode.NumberOfCodeBlocks * 2) - 1;
                this.labelCodeTotal.Text = temp.ToString();
                this.labelDataTotal.Text = "0";

                this.programmingForm.Maximum = temp;
                this.labelDataCount.Text = "0";
                this.labelCodeCount.Text = "0";
                this.enableButtons(false);

                rPEA.BytesToSend[0] = 0x23; // '#'
                rPEA.BytesToSend[1] = 0x55; // 'U'
                rPEA.BytesToSend[2] = 32; //can be 32 or 24
                rPEA.BytesToSend[3] = 0x0D;

                logger.Info("MasterBootLoaderStart: delaying before bootloader start for reset settle time at {0}",
                    DateTime.Now.ToString("HH:mm:ss.fff"));
                Thread.Sleep(3000); // test timing margin after reset

                logger.Info("MasterBootLoaderStart: sending #U at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
                logger.Info("MasterBootLoaderStart sending #U packet: {0}", BitConverter.ToString(rPEA.BytesToSend));
                this.onSend(rPEA);
                logger.Info("MasterBootLoaderStart: #U packet sent at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));

                confirmProgramMasterBoot();
            }
        }

        private void confirmProgramMasterBoot()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            this.failCount = 0;

            if (this.State == RelayProgrammingStates.LoadingMasterBootLoader)
            {
                rPEA.BytesToSend = new byte[3];

                rPEA.BytesToSend[0] = Convert.ToByte('Y');
                rPEA.BytesToSend[1] = Convert.ToByte('E');
                rPEA.BytesToSend[2] = Convert.ToByte('S');

                logger.Info("confirmProgramMasterBoot: delaying before YES at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
                Thread.Sleep(250); // small settle delay after initial #U

                logger.Info("confirmProgramMasterBoot: sending YES at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
                logger.Info("confirmProgramMasterBoot sending YES payload: {0}", BitConverter.ToString(rPEA.BytesToSend));
                this.onSend(rPEA);
                logger.Info("confirmProgramMasterBoot: YES sent at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        private void sendMasterBootCode()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            logger.Info("sendMasterBootCode: entered at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));

            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            this.failCount = 0;

            if (this.State != RelayProgrammingStates.LoadingMasterBootLoader)
                return;

            this.programmingForm.CurrentTask = "Loading Master Boot";
            Int32 temp = Convert.ToInt32(this.labelCodeCount.Text);

            try
            {
                if (this.masterCode.CodeBytes == null || this.masterCode.CodeBytes.Count == 0)
                {
                    logger.Info("sendMasterBootCode: no remaining boot bytes; completing bootloader phase.");
                    this.doneLoadingMasterBootLoader();
                    return;
                }

                rPEA.BytesToSend = new byte[1024];

                int bytesToCopy = Math.Min(1024, this.masterCode.CodeBytes.Count);

                for (int i = 0; i < bytesToCopy; i++)
                {
                    rPEA.BytesToSend[i] = this.masterCode.CodeBytes[0];
                    this.masterCode.CodeBytes.RemoveAt(0);
                }

                for (int i = bytesToCopy; i < 1024; i++)
                {
                    rPEA.BytesToSend[i] = 0xFF;
                }

                logger.Info("sendMasterBootCode: sending boot block length={0} at {1}", rPEA.BytesToSend.Length, DateTime.Now.ToString("HH:mm:ss.fff"));
                this.onSend(rPEA);
                logger.Info("sendMasterBootCode: boot block sent at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));

                this.programmingForm.ProgressValue = temp;
                temp++;
                this.labelCodeCount.Text = temp.ToString();
                logger.Trace("BL, ");

                if (this.masterCode.CodeBytes.Count == 0)
                {
                    logger.Info("sendMasterBootCode: final boot block sent; completing bootloader phase.");
                    this.doneLoadingMasterBootLoader();
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Sending Next Master Boot Data", ex);
            }
        }

        private void sendNextMasterPacket()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            this.failCount = 0;

            if (this.State == RelayProgrammingStates.LoadingMasterCode)
            {

                try
                {
                    this.programmingForm.CurrentTask = "Loading Master Code";

                    rPEA.BytesToSend = new byte[1024];

                    if (this.masterCode.CodeBytes.Count == 0)
                    {
                        this.State = RelayProgrammingStates.LoadingMasterData;
                        this.programmingForm.MasterCodeComplete = true;
                        if (this.masterCode.WithParameters)
                            this.programmingForm.Maximum = this.masterCode.NumberOfDataBlocks;
                        else
                            this.programmingForm.Maximum = this.masterCode.NonParameterCount;

                        this.programmingForm.CurrentTask = "Loading Master Data";
                        logger.Trace("Loading Master Data");
                        this.sendNextMasterPacket();
                        return;
                    }

                    for (int i = 0; i < 1024; i++)
                    {
                        rPEA.BytesToSend[i] = this.masterCode.CodeBytes[0];
                        this.masterCode.CodeBytes.RemoveAt(0);
                    }
                    Int32 temp = Convert.ToInt32(this.labelCodeCount.Text);
                    this.programmingForm.ProgressValue = temp;
                    temp++;
                    this.labelCodeCount.Text = temp.ToString();
                    logger.Trace("MC, ");
                }
                catch (Exception ex)
                {
                    this.errorHandler("Error Sending Next Master Data", ex);
                }
            }
            else if (this.State == RelayProgrammingStates.LoadingMasterData)
            {
                this.programmingForm.CurrentTask = "Loading Master Data";
                rPEA.BytesToSend = new byte[512];
                Int32 temp = Convert.ToInt32(this.labelDataCount.Text);

                if (this.masterCode.DataBytes.Count == 0 || (temp == this.masterCode.NonParameterCount && !this.masterCode.WithParameters))
                {
                    logger.Trace("");
                    this.doneLoadingMaster();
                    return;
                }

                for (int i = 0; i < 512; i++)
                {
                    rPEA.BytesToSend[i] = this.masterCode.DataBytes[0];
                    this.masterCode.DataBytes.RemoveAt(0);
                }



                temp++;
                this.programmingForm.ProgressValue = temp;
                this.labelDataCount.Text = temp.ToString();
                logger.Trace("MD, ");
            }
            else
            {
                throw new Exception("Called sendNextMasterPacket from wrong state: " + this.state.ToString());
            }

            this.onSend(rPEA);
        }

        private void sendNextFPGAPacket()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            try
            {
                rPEA.BytesToSend = new byte[1024];

                Int32 temp = Convert.ToInt32(this.labelCodeCount.Text);

                if (this.fPGACode.SendIndex >= 98304)
                {
                    doneLoadingFPGA();
                    return;
                }

                for (int i = 0; i < 1024; i++)
                {
                    rPEA.BytesToSend[i] = this.fPGACode.DataBytes[this.fPGACode.SendIndex];
                    this.fPGACode.SendIndex++;
                }


                this.programmingForm.ProgressValue = temp;
                temp++;
                this.labelCodeCount.Text = temp.ToString();
                logger.Trace("FP, ");

                this.onSend(rPEA);
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Sending Next FPGA Data", ex);
            }
        }

        private void doneLoadingFPGA()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (!this.programMasterBootFileSelect)
                this.programmingForm.Hide();

            this.reprogramFPGA = false;   // consume final stage
            this.programmingForm.FPGAComplete = true;
            this.timerTimeout.Stop();
            logger.Trace("");
            logger.Trace("Done Loading FPGA");

            logger.Info("MasterBootFileSelect: {0}", this.programMasterBootFileSelect);
            if (this.autoLoad && !this.programMasterBootFileSelect)
                this.allReprogramingDone();
            else if (this.autoLoad && this.programMasterBootFileSelect)
                startManualBootCodeLoad();
        }

        private void doneLoadingRelay()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.timerTimeout.Stop();

            if (this.autoLoad)
            {
                this.programmingForm.RelayDataComplete = true;

                if (this.reprogramMaster)
                {
                    this.reprogramMaster = false;   // consume current stage
                    logger.Info("COMPLETE FLOW doneLoadingRelay: chaining to master code load");
                    this.parseSFile(this.masterCode);
                    this.State = RelayProgrammingStates.WaitingForBootMaster;
                    this.programmingForm.Maximum = this.masterCode.NumberOfCodeBlocks * 2;
                    this.programmingForm.CurrentTask = "Loading Master Code";
                    logger.Trace("Loading Master Code");
                    this.timerTimeout.Start();
                    return;
                }

                if (this.reprogramFPGA)
                {
                    this.reprogramFPGA = false;     // consume current stage
                    logger.Info("COMPLETE FLOW doneLoadingRelay: chaining to FPGA load");
                    this.programmingForm.MasterDataComplete = true;
                    this.programmingForm.MasterCodeComplete = true;
                    this.parseFPGAFile(this.fPGACode);
                    this.State = RelayProgrammingStates.WaitingForBootFPGA;
                    this.programmingForm.Maximum = 96;
                    this.programmingForm.CurrentTask = "Loading FPGA";
                    logger.Trace("Loading FPGA");
                    this.timerTimeout.Start();
                    return;
                }

                logger.Info("COMPLETE FLOW doneLoadingRelay: before allReprogramingDone");
                this.allReprogramingDone();
                logger.Info("COMPLETE FLOW doneLoadingRelay: after allReprogramingDone");
                return;
            }

            logger.Info("COMPLETE FLOW doneLoadingRelay: non-autoload before allReprogramingDone");
            this.allReprogramingDone();
            logger.Info("COMPLETE FLOW doneLoadingRelay: non-autoload after allReprogramingDone");
        }

        private bool IsBootLoadRequired()
        {
            // Safe policy:
            // unknown boot revision is not a valid reason to start boot programming
            if (!this.masterBootRevisionSet || this.masterBootRevisionNumberReceived <= 0)
            {
                logger.Info(
                    "Boot revision unknown; not eligible for automatic boot repair. bootSet={0}, bootRev={1}",
                    this.masterBootRevisionSet,
                    this.masterBootRevisionNumberReceived);

                return false;
            }

            bool outdated = this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber;

            logger.Info(
                "Boot load required check: bootRev={0}, required={1}, outdated={2}",
                this.masterBootRevisionNumberReceived,
                _bootCodeRevisionNumber,
                outdated);

            return outdated;
        }
        private bool IsBootOnlyManualRequired()
        {
            // Only manual-only when:
            // 1) boot is stale/unknown
            // 2) no explicit approval exists
            // 3) there is no real firmware work pending
            if (!this.IsBootLoadRequired())
                return false;

            if (this.IsBootUpdateApproved())
                return false;

            return !this.AnyFirmwarePending();
        }


        private void doneLoadingMaster()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.timerTimeout.Stop();
            this.programmingForm.MasterDataComplete = true;

            // master stage finished
            this.reprogramMaster = false;

            bool stagedFlow = this.autoLoad || ManualUpdate.usingManualMode;

            logger.Info(
                "doneLoadingMaster NEXT STAGE decision: stagedFlow={0}, reprogramRelay={1}, reprogramFPGA={2}, manualMode={3}, autoLoad={4}",
                stagedFlow,
                this.reprogramRelay,
                this.reprogramFPGA,
                ManualUpdate.usingManualMode,
                this.autoLoad);

            if (stagedFlow)
            {
                if (this.reprogramRelay)
                {
                    this.reprogramRelay = false;
                    this.parseSFile(this.relayCode);
                    this.programmingForm.CurrentTask = "Loading Relay Code";
                    logger.Trace("Loading Relay Code");
                    this.programmingForm.Maximum = this.relayCode.NumberOfCodeBlocks * 2;
                    this.State = RelayProgrammingStates.WaitingForBootRelay;
                    this.timerTimeout.Start();
                    return;
                }

                if (this.reprogramFPGA)
                {
                    this.reprogramFPGA = false;
                    this.parseFPGAFile(this.fPGACode);
                    this.programmingForm.RelayCodeComplete = true;
                    this.programmingForm.RelayDataComplete = true;
                    this.programmingForm.CurrentTask = "Loading FPGA";
                    logger.Trace("Loading FPGA");
                    this.programmingForm.Maximum = 96;
                    this.State = RelayProgrammingStates.WaitingForBootFPGA;
                    this.timerTimeout.Start();
                    return;
                }

                // No relay/fpga stage pending -> finish without falsely marking FPGA complete.
                logger.Trace("DoneLoadingMaster 2");
                this.State = RelayProgrammingStates.Idle;

                this.allReprogramingDone();
                return;
            }

            this.allReprogramingDone();
        }

        private void allReprogramingDone()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            bool wasAutoLoad = this.autoLoad;

            logger.Info($"allReprogramingDone: autoLoad={this.autoLoad}, state={this.State}, reprogrammingInProgress={this.reprogrammingInProgress}");

            // Single reset point for autoload state
            this.ResetAutoloadState();

            // Keep this only if you intentionally want to suppress re-prompting on the next UI cycle
            this.askToUgradeShown = true;

            if (wasAutoLoad || ManualUpdate.usingManualMode)
            {
                this.programmingForm.CurrentTask = "Finalizing Update";
                this.State = RelayProgrammingStates.ReprogramSuccess;
                this.timerTimeout.Stop();

                logger.Info("allReprogramingDone: final success path -> issuing final RequestAll inline; skipping FinalizeReprogram().");
                this.requestAll();
                return;
            }

            logger.Trace("All Loading Done, idle");
            this.State = RelayProgrammingStates.Idle;

            // Inline completion to avoid re-entering FinalizeReprogram() from this path
            logger.Info("COMPLETE PATH allReprogramingDone: inline finalization.");
            this.programmingForm.Hide();

            this.ReprogrammingInProgress = false;
            this.autoLoad = false;
            this.firstCheckForUpdate = false;
            this.firmwareUpgradeAcceptedThisCycle = false;

            this.ShowFinalSuccessPopupOnce();

            this.programmingForm.ClearAllChecks();
            logger.Trace("Reprogram Completed Successfully");

            if (ManualUpdate.usingManualMode == true)
            {
                ManualUpdate.usingManualMode = false;
            }

            this.state = RelayProgrammingStates.Idle;
            dataB.oldDataBackup = true;
            restartProgram();

            this.startWarningShownThisCycle = false;
            this.finalSuccessPopupShownThisCycle = false;
        }

        private void requestAll()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RequestAll;
            this.onSend(rPEA);
        }

        private void doneLoadingRelayBootLoader()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            this.enableButtons(true);

            rPEA.Command = RelayProgrammingSendCommands.RestartProgram;
            this.onSend(rPEA);
        }

        private bool IsProgrammingActiveOrInFlight()
        {
            return this.reprogrammingInProgress
                || this.programBootCodeInProgress
                || this.autoLoad
                || this.State == RelayProgrammingStates.LoadingMasterBootLoader
                || this.State == RelayProgrammingStates.LoadingMasterCode
                || this.State == RelayProgrammingStates.LoadingMasterData
                || this.State == RelayProgrammingStates.LoadingRelayCode
                || this.State == RelayProgrammingStates.LoadingRelayData
                || this.State == RelayProgrammingStates.LoadingFPGACode
                || this.State == RelayProgrammingStates.WaitingForBootMaster
                || this.State == RelayProgrammingStates.WaitingForBootRelay
                || this.State == RelayProgrammingStates.WaitingForBootFPGA
                || this.State == RelayProgrammingStates.AutoLoadCheckBoot
                || this.State == RelayProgrammingStates.ManualLoadCheckBoot
                || this.State == RelayProgrammingStates.CheckMasterBootCode
                || this.State == RelayProgrammingStates.ReloadMasterBoot;
        }

        private void doneLoadingMasterBootLoader()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            this.State = RelayProgrammingStates.DoneLoadingMasterBootLoader;

            this.enableButtons(true);
            this.timerTimeout.Stop();
            logger.Trace("Done Loading Master Boot");
            this.programmingForm.MasterBootComplete = true;

            // Preserve branch intent before clearing flags.
            bool bootOnlyFlow = this.programBootCodeOnly;

            // We are not done with the overall firmware cycle.  The relay has just rebooted
            // after the bootloader upload, so we must wait for a fresh boot read before
            // deciding whether to continue master/relay/FPGA programming or repair boot code.
            this.wrongBootCodeLoaded = false;
            this.programBootCodeOnly = false;
            this.programBootCodeStart = false;
            this.programBootCodeInProgress = false;
            

            // IMPORTANT:
            // Keep the current cycle's approval and active programming state intact.
            // The bootloader upload is part of the same operation, not a new cycle.
            // We intentionally do NOT clear:
            //   this.firmwareUpgradeAcceptedThisCycle
            //   this.askToUgradeShown
            //   this.reprogrammingInProgress
            //   this.autoLoad
            //
            // Those flags are used to carry the update decision and state through the relay reset.
            // Re-prompting / clearing them here causes the same update cycle to be re-entered incorrectly.

            this.reloadBootWithPrompt = false;

            bool keepMasterPending = this.reprogramMaster;
            bool keepRelayPending = this.reprogramRelay;
            bool keepFpgaPending = this.reprogramFPGA;

            logger.Info(
                "doneLoadingMasterBootLoader gate: bootOnlyFlow={0}, keepMasterPending={1}, keepRelayPending={2}, keepFpgaPending={3}, bootRevSet={4}, bootRev={5}",
                bootOnlyFlow,
                keepMasterPending,
                keepRelayPending,
                keepFpgaPending,
                this.masterBootRevisionSet,
                this.masterBootRevisionNumberReceived);

            if (bootOnlyFlow && !keepMasterPending && !keepRelayPending && !keepFpgaPending)
            {
                logger.Info("COMPLETE PATH doneLoadingMasterBootLoader: boot-only flow finalization.");
                this.programmingForm.Hide();
                System.Windows.Forms.Application.DoEvents();

                this.restartProgram();

                this.State = RelayProgrammingStates.Idle;
                return;
            }

            // Clear stale boot value so the next read is authoritative.
            this.masterBootRevisionSet = false;
            this.masterBootRevisionNumberReceived = 0;

            // The relay is resetting here. Give it a short settle window before requesting
            // the fresh BOOT read. Without this delay, we can read stale/half-reset state.
            const int resetReadbackDelayMs = 3000;
            logger.Info("doneLoadingMasterBootLoader: waiting {0} ms for relay reset/readback to settle.", resetReadbackDelayMs);
            Thread.Sleep(resetReadbackDelayMs);

            // Trigger the original boot-read sequence again.
            // When BootReceived() fires, it will land in AutoLoadCheckBoot and ContinueAutoloadAfterBootCheck()
            // will decide whether to continue firmware programming or re-enter boot repair.
            this.state = RelayProgrammingStates.AutoLoadCheckBoot;
            this.sendReset();

            logger.Info("doneLoadingMasterBootLoader: boot read re-triggered after reset; waiting for fresh BootReceived()");
        }

        public void FinalizeReprogram()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (this.state != RelayProgrammingStates.Finalized &&
                this.state != RelayProgrammingStates.ReprogramSuccess &&
                this.state != RelayProgrammingStates.WaitForAllData)
            {
                logger.Warn("FinalizeReprogram ignored; state={0}", this.state);
                return;
            }

            logger.Info("COMPLETE PATH FinalizeReprogram: before programmingForm.Hide");
            this.programmingForm.Hide();
            logger.Info("COMPLETE PATH FinalizeReprogram: after programmingForm.Hide");

            this.ReprogrammingInProgress = false;

            this.autoLoad = false;
           
            this.firstCheckForUpdate = false;
            this.firmwareUpgradeAcceptedThisCycle = false;

            this.ShowFinalSuccessPopupOnce();

            this.programmingForm.ClearAllChecks();
            logger.Trace("Reprogram Completed Successfully");

            if (ManualUpdate.usingManualMode == true)
            {
                ManualUpdate.usingManualMode = false;
            }

            this.state = RelayProgrammingStates.Idle;
            dataB.oldDataBackup = true;
            restartProgram();

            this.startWarningShownThisCycle = false;
            this.finalSuccessPopupShownThisCycle = false;
        }

#pragma warning disable CS0414
        private bool startWarningShownThisCycle = false;
#pragma warning restore CS0414
       

        private bool CanRepairBoot(bool manualOverride)
        {
            bool bootUnknown =
                !this.masterBootRevisionSet ||
                this.masterBootRevisionNumberReceived <= 0;

            bool bootOutdated =
                this.masterBootRevisionSet &&
                this.masterBootRevisionNumberReceived > 0 &&
                this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber;

            // Manual full reload is not a boot-repair path. Let the full-update flow proceed.
            if (ManualUpdate.usingManualMode && !this.manualReload)
            {
                logger.Warn("Boot repair blocked (manual): manual update without full reload intent.");
                return false;
            }

            // If this is full manual reload, boot repair is intentionally not being evaluated.
            if (this.manualReload)
            {
                logger.Info("CanRepairBoot: manual full reload path enabled; boot will be started as part of full update.");
                return true;
            }

            if (!manualOverride)
            {
                if (bootUnknown)
                {
                    logger.Warn("Boot repair blocked (auto): boot revision unknown/invalid.");
                    return false;
                }

                if (!bootOutdated)
                {
                    logger.Info("Boot repair not required (auto): boot is current/newer.");
                    return false;
                }

                logger.Info("Boot repair allowed (auto): boot is outdated.");
                return true;
            }

            logger.Warn("Boot repair blocked: manual override not allowed for this path.");
            return false;
        }

        private void sendNonTransmitterSettings()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            // Return if manual reload because we don't have settings
            logger.Trace("Send Non Transmitter Settings");
            if (this.manualReload)
                return;

            if (!this.useDefaultSettings)
                rPEA.Command = RelayProgrammingSendCommands.RecallSavedSettings;
            else
                rPEA.Command = RelayProgrammingSendCommands.RestoreDefaults;

            this.onSend(rPEA);
        }

        private void sendMasterTransferPacket()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.BytesToSend = new byte[4];

            rPEA.BytesToSend[0] = 0x55; // 'U'
            rPEA.BytesToSend[1] = this.masterCode.NumberOfCodeBlocks;
            if (this.masterCode.WithParameters)
                rPEA.BytesToSend[2] = this.masterCode.NumberOfDataBlocks;
            else
                rPEA.BytesToSend[2] = this.masterCode.NonParameterCount;
            rPEA.BytesToSend[3] = 0x0D;

            Thread.Sleep(5);
            if (this.Send != null)
            {
                this.onSend(rPEA);
                this.labelState.Text = "Sent Master Transfer Packet";
                logger.Trace("Sent Master Transfer Packet");
            }
            this.timerTimeout.Stop();
            this.timerTimeout.Interval = 7000;
            this.timerTimeout.Start();

            this.State = RelayProgrammingStates.LoadingMasterCode;
        }

        private void sendRelayTransferPacket()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.BytesToSend = new byte[4];

            this.timerTimeout.Stop();

            rPEA.BytesToSend[0] = 0x64; // 'd'
            rPEA.BytesToSend[1] = this.relayCode.NumberOfCodeBlocks;
            rPEA.BytesToSend[2] = this.relayCode.NumberOfDataBlocks;
            rPEA.BytesToSend[3] = 0x0D;

            if (this.Send != null)
            {
                this.onSend(rPEA);
                this.labelState.Text = "Sent Relay Transfer Packet";
                logger.Trace("Sent Relay Transfer Packet");
            }

            this.timerTimeout.Interval = 7000;
            this.timerTimeout.Start();
        }

        private void sendFPGATransferPacket()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.BytesToSend = new byte[2];

            rPEA.BytesToSend[0] = 0x46; // 'F'
            rPEA.BytesToSend[1] = 0x0D;

            this.State = RelayProgrammingStates.LoadingFPGACode;
            Thread.Sleep(5);
            if (this.Send != null)
            {
                this.onSend(rPEA);
                this.labelState.Text = "Sent FPGA Transfer Packet";
                logger.Trace("Sent FPGA Transfer Packet");
            }
            this.timerTimeout.Stop();
            this.timerTimeout.Interval = 7000;
            this.timerTimeout.Start();
        }

        private void CheckProperMasterBootCode()
        {
            if (ManualUpdate.usingManualMode)
            {
                logger.Info("Manual update path: bypassing boot repair decision logic.");
                this.wrongBootCodeLoaded = false;
                this.programBootCodeOnly = false;
                return;
            }

            bool manualOverride = false;

            if (!this.CanRepairBoot(manualOverride))
            {
                logger.Info(
                    "CheckProperMasterBootCode: boot repair deferred/blocked. bootSet={0}, bootRev={1}",
                    this.masterBootRevisionSet,
                    this.masterBootRevisionNumberReceived);

                this.wrongBootCodeLoaded = false;
                this.programBootCodeOnly = false;
                return;
            }

            if (this.programBootCodeInProgress)
            {
                logger.Warn("CheckProperMasterBootCode suppressed: boot repair already active.");
                return;
            }

            bool staleBootContinuation =
                this.State == RelayProgrammingStates.AutoLoadCheckBoot ||
                this.State == RelayProgrammingStates.ManualLoadCheckBoot ||
                this.State == RelayProgrammingStates.CheckMasterBootCode;

            if (this.reprogrammingInProgress &&
                !staleBootContinuation &&
                !this.programBootCodeInProgress)
            {
                logger.Warn("Ignoring boot decision because normal programming is already active.");
                return;
            }

            bool bootNeedsLoad = this.IsBootLoadRequired();

            if (!bootNeedsLoad)
            {
                this.wrongBootCodeLoaded = false;
                this.programBootCodeOnly = false;
                return;
            }

            if (!this.IsBootUpdateApproved())
            {
                logger.Info("CheckProperMasterBootCode: repair suppressed; waiting for explicit approval.");
                this.wrongBootCodeLoaded = false;
                this.programBootCodeOnly = false;
                this.programBootCodeStart = false;
                this.programBootCodeInProgress = false;
                return;
            }

            logger.Info("Boot load required (unknown or older revision).");
            this.wrongBootCodeLoaded = true;
            this.programBootCodeOnly = !(this.reprogramMaster || this.reprogramRelay || this.reprogramFPGA);

            if (!this.programBootCodeInProgress)
            {
                logger.Info(
                    "CheckProperMasterBootCode: launching boot repair. programBootCodeOnly={0}, pending(M={1},R={2},F={3})",
                    this.programBootCodeOnly,
                    this.reprogramMaster,
                    this.reprogramRelay,
                    this.reprogramFPGA);

                this.ProgramBootCodeStart = true;
            }
        }

        private bool CheckForProperBootCodeAutoUpdate()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            reloadBootWithPrompt = false;
            CheckProperMasterBootCode();
            return wrongBootCodeLoaded;
        }
       

        private void restartProgram()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            this.enableButtons(true);
            rPEA.Command = RelayProgrammingSendCommands.RestartProgram;

            this.onSend(rPEA);
        }

        private void useRelaySFile(string fileName)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                using (StreamReader sR = new StreamReader(fileName, Encoding.ASCII))
                {
                    this.relayCode.FileString = sR.ReadToEnd();
                }
                this.parseSFile(this.relayCode);
                return;
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Opening File for Master S File", ex);
                return;
            }
        }

        private void useMasterSFile(string fileName)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                using (StreamReader sR = new StreamReader(fileName, Encoding.ASCII))
                {
                    this.masterCode.FileString = sR.ReadToEnd();
                }
                this.parseSFile(this.masterCode);
                return;
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Opening File for Master S File", ex);
                return;
            }
        }

        private void parseFPGAFile(FPGAProgrammingData fPD)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                if (this.fPGACode.UseFile && this.fPGACode.FileName == "")
                {
                    throw new Exception("No FPGA File Loaded");
                }

                CustomerLoadFiles cLF = this.customersFiles.Find(x => x.Customer.Equals(this.customer));

                this.fPGACode.SendIndex = 0;
                if (this.fPGACode.UseFile)
                    fPD.DataBytes = File.ReadAllBytes(fPD.FileName);
                else
                    fPD.DataBytes = cLF.FPGAFile.DataBytes;

                fPD.AddDate();
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Parsing FPGA File", ex);
            }
        }

        private void parseBootLoaderSFile(RelayProgrammingData rPD)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                using (StringReader sR = new StringReader(rPD.FileString))
                {
                    try
                    {
                        UInt16 length;
                        UInt32 address;

                        // Remove the First Line
                        string s = sR.ReadLine();

                        // Loop to get up to part of file that contains the bootloader
                        do
                        {
                            s = sR.ReadLine();

                            // Next 2 indicate the length, in hex

                            length = Convert.ToUInt16(s.Substring(2, 2), 16);

                            // Next 5 bytes are the address

                            address = Convert.ToUInt32(s.Substring(4, 8), 16);
                        }
                        while (address < 0x80000); // Address where the BootLoader starts

                        rPD.CodeBytes = new List<byte>();
                        tempBootAddress = address - 0x20;

                        // Might want to check this address
                        while (address < 0x88000)
                        {
                            // Add the data
                            // Remove length and address
                            s = s.Remove(0, 12);

                            if (address != (tempBootAddress + 0x20)) //try to fill in gaps to 82000 then to 88000 with FFs
                            {

                                while (tempBootAddress < 0x82000 - 0x20)
                                {
                                    tempBootAddress = tempBootAddress + 0x20;
                                    for (int i = 0; i < 32; i++)
                                        rPD.CodeBytes.Add((byte)0xFF);
                                }

                                while (tempBootAddress < 0x87FFC - 0x40 && tempBootAddress > 0x82000)
                                {
                                    tempBootAddress = tempBootAddress + 0x20;
                                    for (int i = 0; i < 32; i++)
                                        rPD.CodeBytes.Add((byte)0xFF);
                                }

                                tempBootAddress = address;
                            }
                            else
                            {
                                tempBootAddress = address;
                            }

                            length = Convert.ToUInt16(length - 5);

                            if (address != 0x87FFC)
                            {
                                for (int i = 0; i < length; i++)
                                {
                                    rPD.CodeBytes.Add(Convert.ToByte(s.Substring(i * 2, 2), 16));


                                }
                                if (length != 32)
                                {
                                    for (; length < 32; length++)
                                        rPD.CodeBytes.Add((byte)0xFF);
                                }
                            }
                            else if (address == 0x87FFC) //added special case for boot code ( dec 557052 )
                            {
                                for (int j = 0; j <= 27; j++)
                                    rPD.CodeBytes.Add((byte)0xFF);

                                for (int j = 0; j <= 3; j++)
                                    rPD.CodeBytes.Add(Convert.ToByte(s.Substring(j * 2, 2), 16));

                                break;
                            }

                            try
                            {
                                s = sR.ReadLine();
                            }
                            catch
                            {
                                break;
                            }

                            // Next 2 indicate the length, in hex

                            length = Convert.ToUInt16(s.Substring(2, 2), 16);

                            // Next 5 bytes are the address

                            address = Convert.ToUInt32(s.Substring(4, 8), 16);

                        }

                    }
                    catch (Exception ex)
                    {
                        this.errorHandler("Error Parsing S File for BootLoader", ex);
                    }
                }

            }
            catch (Exception ex)
            {
                this.errorHandler("Error Parsing S File for Boot Loader", ex);
            }
        }

        private void addBootLoaderAddress(List<byte> list, UInt32 address)
        {
            byte tempLSB, tempMSB;

            // Shift the address over by one to match word address (vs byte address)
            // And get high and low bytes
            address >>= 1;

            tempLSB = (byte)address;
            address >>= 8;
            tempMSB = (byte)address;

            list.Add(tempMSB);
            list.Add(tempLSB);

            return;
        }

        private void parseSFile(RelayProgrammingData rPD)
        {
            // Initialize the two data arrays
            rPD.CodeBytes = new List<byte>();
            rPD.DataBytes = new List<byte>();
            try
            {
                using (StringReader sR = new StringReader(rPD.FileString))
                {
                    try
                    {
                        // Remove the first line
                        string s = sR.ReadLine();
                        // Get the first line in
                        UInt32 workingAddress = 0;

                        s = sR.ReadLine();
                        // This loops handles the code data
                        while (s.StartsWith("S3"))
                        {
                            // Next 2 indicate the length, in hex

                            UInt16 length = Convert.ToUInt16(s.Substring(2, 2), 16);

                            // Next 8 characters are the address

                            UInt32 address = Convert.ToUInt32(s.Substring(4, 8), 16);

                            if (address >= 0x040000)
                            {
                                // We are outside of the program so we are done
                                break;
                            }
                            // Fill in empty code space with FFs.
                            for (int i = 0; i < address - workingAddress; i++)
                            {
                                rPD.CodeBytes.Add((byte)0xFF);
                            }

                            workingAddress = address;



                            //Remove length and address
                            s = s.Remove(0, 12);

                            // next (length - 5) are the data bytes

                            for (int i = 0; i < length - 5; i++)
                            {
                                rPD.CodeBytes.Add(Convert.ToByte(s.Substring(i * 2, 2), 16));
                                workingAddress++;
                            }

                            // Read Next Line
                            try
                            {
                                s = sR.ReadLine();
                            }
                            catch
                            {
                                break;
                            }
                        }

                        while (rPD.CodeBytes.Count % 2048 != 0)
                        {
                            rPD.CodeBytes.Add((byte)0xFF);
                        }

                        // This loop gets to the constant data

                        while (s.StartsWith("S3"))
                        {
                            // Remove the first 2 chars

                            // 5 bytes are the address

                            UInt32 address = Convert.ToUInt32(s.Substring(4, 8), 16);

                            if (address >= 0x088000)
                            {
                                // We have gotten to the const data
                                break;
                            }
                            // Read Next Line
                            try
                            {
                                s = sR.ReadLine();
                            }
                            catch
                            {
                                break;
                            }

                        }

                        // This loop handles the constant data
                        UInt32 clearAddress = Convert.ToUInt32(s.Substring(4, 8), 16);
                        while (clearAddress < 0x4008000)
                        {
                            s = sR.ReadLine();
                            clearAddress = Convert.ToUInt32(s.Substring(4, 8), 16);
                        }
                        workingAddress = 0x04008000;
                        rPD.NonParameterCount = 0;

                        while (s.StartsWith("S3"))
                        {
                            // Next 2 indicate the length, in hex

                            UInt16 length = Convert.ToUInt16(s.Substring(2, 2), 16);

                            // Next 4 bytes are the address

                            UInt32 address = Convert.ToUInt32(s.Substring(4, 8), 16);

                            if (address >= 0x0400FE00 && rPD.NonParameterCount == 0)
                            {
                                // Set the number of blocks to send when it loading WITHOUT parameters

                                UInt32 temp = workingAddress - 0x04008000;

                                rPD.NonParameterCount = (byte)(temp / 512);
                                // Add 1 if there is a remainder
                                if (temp % 512 != 0)
                                    rPD.NonParameterCount++;

                            }
                            if (address >= 0x04010000)
                            {
                                // We are outside of the dat so we are done
                                break;
                            }

                            // Fill in empty code space with FFs.
                            for (int i = 0; i < address - workingAddress; i++)
                            {
                                rPD.DataBytes.Add((byte)0xFF);
                            }

                            workingAddress = address;



                            //Remove length and address
                            s = s.Remove(0, 12);

                            // next length - 5 are the data bytes

                            for (int i = 0; i < length - 5; i++)
                            {
                                rPD.DataBytes.Add(Convert.ToByte(s.Substring(i * 2, 2), 16));
                                workingAddress++;
                            }

                            // Read Next Line
                            try
                            {
                                s = sR.ReadLine();
                            }
                            catch
                            {
                                break;
                            }
                        }

                        while (rPD.DataBytes.Count % 1024 != 0)
                        {
                            rPD.DataBytes.Add((byte)0xFF);
                        }

                    }
                    catch (Exception ex)
                    {
                        this.errorHandler("Error parsing Master S File", ex);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Opening File for Master S File", ex);
                return;
            }
        }

        private void errorHandler(string p, Exception ex)
        {
            ExceptionEventArgs eEA = new ExceptionEventArgs(ex, "Relay Programming");
            if (Error != null)
                this.Error(this, eEA);
        }

        #region Button Events

        private void buttonSelectMasterSFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFD = new OpenFileDialog();
            DialogResult dR = DialogResult.None;

            try
            {
                oFD.CheckPathExists = true;
                oFD.CheckFileExists = true;
                oFD.Filter = "s files (*.s)|*.s";
                oFD.InitialDirectory = @"C:\Freescale\RelayMasterProcessor\output\";
                oFD.Multiselect = false;
                oFD.Title = "Select Master Relay S File";

                dR = oFD.ShowDialog();
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Opening Master Relay s File", ex);
            }

            if (dR == DialogResult.OK)
            {
                this.masterCode.FileName = oFD.FileName;
                this.useMasterSFile(oFD.FileName);
                this.textBoxMasterFileName.Text = oFD.FileName;
                this.dontReloadFromResource = true;
                logger.Trace("Selected Master File");
            }
        }

        private void buttonSelectRelaySFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFD = new OpenFileDialog();
            DialogResult dR = DialogResult.None;

            try
            {
                oFD.CheckPathExists = true;
                oFD.CheckFileExists = true;
                oFD.Filter = "s files (*.s)|*.s";
                oFD.InitialDirectory = @"C:\Freescale\RelayProcessor\output\";
                oFD.Multiselect = false;
                oFD.Title = "Select Relay Processor S File";

                dR = oFD.ShowDialog();
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Opening Relay Processor S File", ex);
            }

            if (dR == DialogResult.OK)
            {
                logger.Trace("Selected Relay File");
                this.useRelaySFile(oFD.FileName);
                this.textBoxRelayFileName.Text = oFD.FileName;
                this.dontReloadFromResource = true;
            }
        }

        private void buttonSelectFPGAFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFD = new OpenFileDialog();
            DialogResult dR = DialogResult.None;

            try
            {
                oFD.CheckPathExists = true;
                oFD.CheckFileExists = true;
                oFD.Filter = "rbf files (*.rbf)|*.rbf";
                oFD.InitialDirectory = @"C:\Freescale\FPGA Files";
                oFD.Multiselect = false;
                oFD.Title = "Select FPGA RBF File";

                dR = oFD.ShowDialog();

                if (dR == DialogResult.OK)
                {
                    this.fPGACode.FileName = oFD.FileName;
                    this.fPGACode.Date = oFD.FileName.Substring(oFD.FileName.Length - 10, 6);
                    this.textBoxFPGAFile.Text = oFD.FileName;
                    this.fPGACode.UseFile = true;
                    this.parseFPGAFile(this.fPGACode);
                    this.dontReloadFromResource = true;
                    logger.Trace("Selected FPGA File");
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Opening FPGA RBF File", ex);
            }
        }

        private void startRelayProgramming()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(this.relayCode.FileString))
            {
                if (!this.dontReloadFromResource)
                {
                    logger.Info("startRelayProgramming: relay resource not loaded; attempting to reload programming files from embedded resources.");
                    this.setProgrammingFiles();
                }

                if (string.IsNullOrWhiteSpace(this.relayCode.FileString))
                {
                    MessageBox.Show("No Relay File Loaded");
                    this.State = RelayProgrammingStates.Idle;
                    return;
                }
            }

            this.parseSFile(this.relayCode);

            this.State = RelayProgrammingStates.LoadingRelayCode;
            int temp = this.relayCode.NumberOfCodeBlocks * 2;
            this.programmingForm.Maximum = temp;
            this.programmingForm.CurrentTask = "Loading Relay Code a";
            this.labelCodeTotal.Text = temp.ToString();
            this.labelDataTotal.Text = this.relayCode.NumberOfDataBlocks.ToString();
            this.labelDataCount.Text = "0";
            this.labelCodeCount.Text = "0";
            this.sendRelayReset();
            timerTimeout.Stop();
            timerTimeout.Interval = 1500;
            timerTimeout.Start();
            this.enableButtons(false);
        }

        private void startRelayBootLoaderProgramming()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if ((this.relayCode.FileString == "" || this.relayCode.FileString == null))
            {
                MessageBox.Show("No Relay File Loaded");
                this.State = RelayProgrammingStates.Idle;
                return;
            }

            this.State = RelayProgrammingStates.ClearingBootLoader;
            this.parseBootLoaderSFile(this.relayCode);
            int temp = this.relayCode.CodeBytes.Count / 34;

            this.labelCodeTotal.Text = temp.ToString();
        }

        private void startMasterProgramming()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if ((this.masterCode.FileString == "" || this.masterCode.FileString == null))
            {
                MessageBox.Show("No Master File Loaded");
                this.State = RelayProgrammingStates.Idle;
                return;
            }

            this.parseSFile(this.masterCode);

            this.State = RelayProgrammingStates.WaitingForBootMaster;
            int temp = this.masterCode.NumberOfCodeBlocks * 2;
            this.labelCodeTotal.Text = temp.ToString();

            if (this.masterCode.WithParameters)
            {
                this.labelDataTotal.Text = this.masterCode.NumberOfDataBlocks.ToString();
            }
            else
            {
                this.labelDataTotal.Text = this.masterCode.NonParameterCount.ToString();
            }
            this.programmingForm.Maximum = temp;
            this.labelDataCount.Text = "0";
            this.labelCodeCount.Text = "0";
            this.sendReset();
            if (programmingForm.MasterBootComplete)
                Thread.Sleep(1000);
            this.enableButtons(false);
        }

        private void startMasterProgrammingWithParameters()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if ((this.masterCode.FileString == "" || this.masterCode.FileString == null))
            {
                MessageBox.Show("No Master File Loaded");
                this.State = RelayProgrammingStates.Idle;
                return;
            }
            this.masterCode.WithParameters = true;

            this.parseSFile(this.masterCode);

            this.State = RelayProgrammingStates.WaitingForBootMaster;
            int temp = this.masterCode.NumberOfCodeBlocks * 2;
            this.labelCodeTotal.Text = temp.ToString();
            if (this.masterCode.WithParameters)
            {
                this.labelDataTotal.Text = this.masterCode.NumberOfDataBlocks.ToString();
            }
            else
            {
                this.labelDataTotal.Text = this.masterCode.NonParameterCount.ToString();
            }
            this.programmingForm.Maximum = temp;
            this.labelDataCount.Text = "0";
            this.labelCodeCount.Text = "0";
            this.sendReset();
            this.enableButtons(false);
        }

        private void startProgramming()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            // NEW GUARD: never start normal firmware while bootloader upload/reset is in progress.
            if (this.programBootCodeInProgress ||
                this.State == RelayProgrammingStates.LoadingMasterBootLoader ||
                this.State == RelayProgrammingStates.DoneLoadingMasterBootLoader)
            {
                logger.Warn(
                    "startProgramming suppressed while bootloader flow is active. state={0}, programBootCodeInProgress={1}",
                    this.State,
                    this.programBootCodeInProgress);
                return;
            }

            if (!EnsureProgrammingStartWarningAcknowledged())
            {
                logger.Warn("startProgramming blocked: start warning not acknowledged.");
                return;
            }

            if (this.AutoloadAcceptedPendingBackup)
            {
                logger.Warn("startProgramming blocked: backup still pending.");
                return;
            }

            bool bootUnknown = !this.masterBootRevisionSet ||
                              this.masterBootRevisionNumberReceived <= 0;
            bool bootOld = this.masterBootRevisionSet &&
                           this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber;



            logger.Info(
                "FINAL ORDER CHECK | autoLoad={0}, manualReload={1}, bootSet={2}, bootRev={3}, bootRequired={4}, bootUnknown={5}, bootOld={6}",
                this.autoLoad,
                this.manualReload,
                this.masterBootRevisionSet,
                this.masterBootRevisionNumberReceived,
                 _bootCodeRevisionNumber,
                bootUnknown,
                bootOld);

            resumeProgrammingAfterBackup = false;
            this.reprogrammingInProgress = true;

            if (!this.dontReloadFromResource)
            {
                if (string.IsNullOrWhiteSpace(this.masterCode.FileString) ||
                    (this.reprogramRelay && string.IsNullOrWhiteSpace(this.relayCode.FileString)))
                {
                    logger.Warn("startProgramming: programming payload missing; reloading embedded resource files.");
                    this.setProgrammingFiles();
                }
            }

            if (this.reprogramMaster && string.IsNullOrWhiteSpace(this.masterCode.FileString))
            {
                logger.Error("startProgramming aborted: master payload is empty.");
                this.reprogrammingInProgress = false;
                this.autoLoad = false;
                this.State = RelayProgrammingStates.Idle;
                return;
            }

            if (this.reprogramRelay && string.IsNullOrWhiteSpace(this.relayCode.FileString))
            {
                logger.Error("startProgramming aborted: relay payload is empty.");
                this.reprogrammingInProgress = false;
                this.autoLoad = false;
                this.State = RelayProgrammingStates.Idle;
                return;
            }

            if (this.programBootCodeOnly)
            {
                this.ProgramBootCodeStart = true;
                return;
            }

            if (this.reprogramMaster)
            {
                logger.Info("startProgramming: master update selected -> start master programming");
                this.startMasterProgramming();
                return;
            }

            if (this.reprogramRelay)
            {
                logger.Info("startProgramming: relay update selected -> start relay programming");
                this.startRelayProgramming();
                return;
            }

            if (this.reprogramFPGA)
            {
                logger.Info("startProgramming: FPGA update selected -> start FPGA programming");
                this.programFPGA();
                return;
            }

            logger.Warn("startProgramming: no programming branch selected; clearing state.");
            this.reprogrammingInProgress = false;
            this.autoLoad = false;
            this.State = RelayProgrammingStates.Idle;
        }



        private void resetMasterProgramming()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (this.masterCode.FileString == "" || this.masterCode.FileString == null)
            {
                MessageBox.Show("No Master File Loaded");
                this.State = RelayProgrammingStates.Idle;
                return;
            }
            this.parseSFile(this.masterCode);

            this.State = RelayProgrammingStates.WaitingForBootMaster;
            this.programmingForm.MasterCodeComplete = false;
            this.programmingForm.MasterDataComplete = false;
            this.programmingForm.CurrentTask = "Loading Master File - Waiting for Boot - Please Wait";

            int temp = this.masterCode.NumberOfCodeBlocks * 2;
            this.labelCodeTotal.Text = temp.ToString();
            if (this.masterCode.WithParameters)
                this.labelDataTotal.Text = this.masterCode.NumberOfDataBlocks.ToString();
            else
                this.labelDataTotal.Text = this.masterCode.NonParameterCount.ToString();
            this.labelDataCount.Text = "0";
            this.labelCodeCount.Text = "0";

            this.sendReset();
        }

        private void resetProgrammingRelay()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (this.relayCode.FileString == "" || this.relayCode.FileString == null)
            {
                MessageBox.Show("No Relay File Loaded");
                this.State = RelayProgrammingStates.Idle;
                return;
            }
            this.parseSFile(this.relayCode);

            this.programmingForm.CurrentTask = "Loading Relay Code Retrying - Waiting For Boot - Please Wait";
            this.programmingForm.RelayCodeComplete = false;
            this.programmingForm.RelayDataComplete = false;

            this.State = RelayProgrammingStates.LoadingRelayCode;

            int temp = this.relayCode.NumberOfCodeBlocks * 2;
            this.labelCodeTotal.Text = temp.ToString();
            this.labelDataTotal.Text = this.relayCode.NumberOfDataBlocks.ToString();
            this.labelDataCount.Text = "0";
            this.labelCodeCount.Text = "0";

            this.timerTimeout.Stop();
            this.timerTimeout.Interval = 10000;
            this.timerTimeout.Start();
            this.sendRelayReset();
        }

        private void resetProgrammingFPGA()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.parseFPGAFile(this.fPGACode);

            this.programmingForm.CurrentTask = "Loading FPGA Code - Waiting For Boot - Please Wait";
            this.programmingForm.FPGAComplete = false;

            this.State = RelayProgrammingStates.LoadingFPGACode;

            this.labelCodeTotal.Text = "96";
            this.labelDataTotal.Text = "0";
            this.labelCodeCount.Text = "0";
            this.labelDataCount.Text = "0";

            this.sendRelayReset();
            if (programmingForm.MasterBootComplete)
                Thread.Sleep(1000);
        }

        private void sendQuietMode()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

        }

        private void sendRelayReset()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;
            rPEA.BytesToSend = new byte[3];

            rPEA.BytesToSend[0] = (byte)'B';
            rPEA.BytesToSend[1] = (byte)'U';
            rPEA.BytesToSend[2] = 0x0D;

            this.onSend(rPEA);
        }

        private void sendReset()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            bool userInitiatedPath = ManualUpdate.usingManualMode;

            bool stateAllowsReset =
                this.State == RelayProgrammingStates.AutoLoadCheckBoot ||
                this.State == RelayProgrammingStates.ManualLoadCheckBoot ||
                this.State == RelayProgrammingStates.CheckMasterBootCode ||
                this.State == RelayProgrammingStates.WaitingForBootMaster ||
                this.State == RelayProgrammingStates.WaitingForBootRelay ||
                this.State == RelayProgrammingStates.WaitingForBootFPGA;

            bool activeProgramming =
                this.autoLoad ||
                this.reprogrammingInProgress ||
                this.programBootCodeInProgress;

            // Hard block: never reset during an active programming run unless we are already
            // in a known boot/programming handoff state.
            if (!userInitiatedPath && activeProgramming && !stateAllowsReset)
            {
                logger.Warn(
                    "sendReset suppressed: active programming in invalid state. state={0}, autoLoad={1}, reprogrammingInProgress={2}, programBootCodeInProgress={3}",
                    this.State,
                    this.autoLoad,
                    this.reprogrammingInProgress,
                    this.programBootCodeInProgress);

                return;
            }

            // Also block any raw reset from a non-state-machine context.
            if (!userInitiatedPath && !stateAllowsReset)
            {
                logger.Warn(
                    "sendReset suppressed: invalid reset state. state={0}",
                    this.State);

                return;
            }

            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;
            rPEA.BytesToSend = new byte[3];

            rPEA.BytesToSend[0] = (byte)'b';
            rPEA.BytesToSend[1] = (byte)'U';
            rPEA.BytesToSend[2] = 0x0D;

            this.onSend(rPEA);
        }

        private void buttonProgramRelay_Click(object sender, EventArgs e)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.autoLoad = false;
            this.dontReloadFromResource = true;
            this.startRelayProgramming();
            this.programmingForm.ClearAllChecks();
            this.enableButtons(false);
        }

        private void buttonProgramMaster_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Parameter Select", "With Parameters?", MessageBoxButtons.YesNo);

            this.dontReloadFromResource = true;
            this.autoLoad = false;

            this.programmingForm.ClearAllChecks();

            if (dr == DialogResult.Yes)
                this.startMasterProgrammingWithParameters();
            else
            {
                this.masterCode.WithParameters = false;
                this.startMasterProgramming();
            }


        }
        #region FPGA

        private void buttonProgramFPGA_Click(object sender, EventArgs e)
        {
            this.autoLoad = false;

            this.dontReloadFromResource = true;
            this.programmingForm.ClearAllChecks();
            this.programFPGA();
        }

        private void programFPGA()
        {
            
            this.reprogramFPGA = true;
            this.transmitterEnabled = true;
            this.parseFPGAFile(this.fPGACode);
            this.State = RelayProgrammingStates.WaitingForBootFPGA;
            this.programmingForm.CurrentTask = "Loading FPGA";
            this.programmingForm.Maximum = 96;
            this.State = RelayProgrammingStates.WaitingForBootFPGA;
            this.labelCodeCount.Text = "0";
            this.labelCodeTotal.Text = "96";
            this.sendReset();
            if (programmingForm.MasterBootComplete)
                Thread.Sleep(1000);
        }

        #endregion


        private void enableButtons(bool b)
        {
            return;
            //this.buttonProgramMaster.Enabled = b;
            //this.buttonProgramRelay.Enabled = b;
            //this.buttonSelectMasterSFile.Enabled = b;
            //this.buttonSelectRelaySFile.Enabled = b;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.PrepForBoot();
        }

        #endregion

        private void timerTimeout_Tick(object sender, EventArgs e)
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            //if (ManualUpdate.usingManualMode == false)
            //{
            this.labelState.Text = "Time Out";
            this.programmingForm.CurrentTask = "Timed Out - Restarting";
            logger.Trace("Timed Out in State " + this.state);
            this.timerTimeout.Stop();
            this.timerTimeout.Interval = 20000;
            this.timerTimeout.Start();


            switch (this.State)
            {
                case RelayProgrammingStates.LoadingMasterData:
                case RelayProgrammingStates.LoadingMasterCode:
                case RelayProgrammingStates.WaitingForBootMaster:
                    this.State = RelayProgrammingStates.WaitingForBootMaster;
                    this.PrepForBoot();
                    break;
                case RelayProgrammingStates.LoadingRelayCode:
                case RelayProgrammingStates.LoadingRelayData:
                case RelayProgrammingStates.WaitingForBootRelay:
                    this.State = RelayProgrammingStates.WaitingForBootRelay;
                    this.PrepForBoot();
                    break;
                case RelayProgrammingStates.LoadingFPGACode:
                case RelayProgrammingStates.WaitingForBootFPGA:
                    this.State = RelayProgrammingStates.WaitingForBootFPGA;
                    this.PrepForBoot();
                    break;
                case RelayProgrammingStates.ReprogramSuccess:
                    this.timerTimeout.Stop();
                    this.manualReload = false;
                    this.programmingForm.Hide();
                    break;

                case RelayProgrammingStates.LoadingMasterBootLoader:
                    this.state = RelayProgrammingStates.ReloadMasterBoot;
                    break;
            }

            //}
        }

        private void startManualBootCodeLoad()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.programBootCodeOnly = true;
            this.autoLoad = false;
            this.manualReload = false;
            this.timerTimeout.Stop();
            Thread.Sleep(3000);
            programMasterBootFileSelect = false;
            ProgramBootCodeStart = true;
        }

        private void buttonStartAutoLoad_Click(object sender, EventArgs e)
        {
            this.useDefaultSettings = false;
            this.dontReloadFromResource = true;
            this.programmingForm.ClearAllChecks();
            this.setProgrammingFiles();

            //insert if statement for prograaming selection here //MaterialTextBox.Text.Trim().Length == 0
            if (this.textBoxFPGAFile.Text.Trim().Length != 0)
                this.reprogramFPGA = this.transmitterEnabled;
            else
                this.reprogramFPGA = false;

            if (this.textBoxRelayFileName.Text.Trim().Length != 0)
                this.reprogramRelay = true;
            else
                this.reprogramRelay = false;


            if (this.textBoxMasterFileName.Text.Trim().Length != 0)
                this.reprogramMaster = true;
            else
                this.reprogramMaster = false;

            if (this.textBoxFPGAFile.Text.Trim().Length != 0 || this.textBoxRelayFileName.Text.Trim().Length != 0 ||
                this.textBoxMasterFileName.Text.Trim().Length != 0)
            {
                this.autoLoad = true;
                this.setProgrammingFiles();
                this.startAutoLoad();
            }

            if (this.textBoxFPGAFile.Text.Trim().Length == 0 && this.textBoxRelayFileName.Text.Trim().Length == 0 &&
                this.textBoxMasterFileName.Text.Trim().Length == 0)
            {
                DialogResult result = MessageBox.Show("No files selected. Do you want to program all using defaults?", "Warning",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    this.dontReloadFromResource = false;
                    this.programmingForm.ClearAllChecks();
                    this.reprogramFPGA = this.transmitterEnabled;
                    this.reprogramRelay = true;
                    this.reprogramMaster = true;

                    this.setProgrammingFiles();
                    this.startAutoLoad();
                }
            }
        }

        private void buttonClearAllProgrammingFields_Click(object sender, EventArgs e)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.textBoxFPGAFile.Text = "";
            this.textBoxRelayFileName.Text = "";
            this.textBoxMasterFileName.Text = "";
        }

        private void buttonLoadDefaultResourceSFiles_Click(object sender, EventArgs e)
        {
            this.dontReloadFromResource = false;
            this.setProgrammingFiles();
        }

        private void buttonFirstLoad_Click(object sender, EventArgs e)
        {
            this.programmingForm.ClearAllChecks();
            this.InitialAutoLoadFiles();
        }

        private void buttonLoadNewest_Click(object sender, EventArgs e)
        {
            DialogResult dR = new CustomYesNoDialog("GE or WH Select", "Is this a GE or WH style relay?", "GE", "WH").ShowDialog();

            if (dR == DialogResult.Yes)
                internalGESetter = true;
            else
                internalGESetter = false;

            //this.selectNewestMasterFirmware();
           // this.selectNewestRelayFirmware();
            //this.selectNewestFPGAFirmware();

            this.programmingForm.ClearAllChecks();
        }

        private void buttonFixBootLoader_Click(object sender, EventArgs e)
        {
            this.startRelayBootLoaderProgramming();
        }

        public void AllParametersReceived()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (this.State == RelayProgrammingStates.WaitForAllData)
            {
                logger.Trace("ParamsReceived-WaitingForAllData");
                this.State = RelayProgrammingStates.Finalized;
                this.SendTransmitterSettings();
                this.sendNonTransmitterSettings();
                this.FinalizeReprogram();
            }
            else if (this.State == RelayProgrammingStates.ReprogramSuccess)
            {
                logger.Info("AllParametersReceived: ReprogramSuccess -> Finalized");
                this.state = RelayProgrammingStates.Finalized;
                this.FinalizeReprogram();
            }
        }

        private void comboBoxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.customer = (Customers)this.comboBoxCustomer.SelectedItem;

            if (this.customer == Customers.None || this.customer == Customers.CONED)
            {
                MessageBox.Show("Please Select a Different Customer");
                this.customer = Customers.None;
            }

        }

        private void buttonProgramMasterBootCode_Click(object sender, EventArgs e)
        {
            this.ProgramBootCodeStart = true;
            programBootCodeOnly = true;
        }
    }

    public class FPGAProgrammingData
    {
        public FPGAProgrammingData()
        {
        }

        public bool UseFile = false;
        public string FileName;
        public int SendIndex = 0;
        public byte[] DataBytes = new byte[98304];
        public string Date
        {
            get { return this.date; }
            set
            {
                this.date = value;
                this.setDateArray();
            }
        }

        private byte[] dateArray = new byte[6];
        private string date = "000000";

        public void AddDate()
        {
            byte[] tempArray = new byte[98304];

            this.DataBytes.CopyTo(tempArray, 0);
            this.dateArray.CopyTo(tempArray, 98024);

            this.DataBytes = tempArray;
        }

        private void setDateArray()
        {
            this.dateArray[0] = (byte)this.date[1];
            this.dateArray[1] = (byte)this.date[0];
            this.dateArray[2] = (byte)this.date[3];
            this.dateArray[3] = (byte)this.date[2];
            this.dateArray[4] = (byte)this.date[5];
            this.dateArray[5] = (byte)this.date[4];
        }
    }

    public class RelayBootLoaderCode
    {
        public RelayBootLoaderCode()
        {
        }

        public byte[] DataBytes;
        public int SendIndex = 0;
    }

    public class RelayProgrammingData
    {
        public RelayProgrammingData(int flashBlockSize)
        {
            this.blockSize = flashBlockSize;
            this.DataBytes = new List<byte>();
            this.CodeBytes = new List<byte>();
        }

        public string FileName;
        public string FileString;
        public bool WithParameters = false;

        private List<byte> initializeBeginningData(int outside, int inside)
        {
            List<byte> returnList = new List<byte>();

            for (int i = 0; i < outside; i++)
            {
                for (int j = 0; j < inside; j++)
                {
                    returnList.Add(0xFF);
                }
            }
            return returnList;

        }
        private List<byte> initializeBeginningCode(int outside, int inside)
        {
            List<byte> returnList = new List<byte>();

            for (int i = 0; i < outside; i++)
            {
                for (int j = 0; j < inside; j++)
                {
                    returnList.Add(0xFF);
                }
            }
            return returnList;
        }

        public List<byte> DataBytes;
        public List<byte> CodeBytes;

        public byte NonParameterCount;
        public byte NumberOfDataBlocks
        {
            get
            {
                return (byte)(this.DataBytes.Count / (this.blockSize / 2));
            }
        }
        public byte NumberOfCodeBlocks
        {
            get
            {
                return (byte)(this.CodeBytes.Count / (this.blockSize * 2));
            }
        }

        private int blockSize = 1024;

    }

    public enum RelayProgrammingSendCommands
    {
        RawData,
        RestartProgram,
        TransmitterSettings,
        ResetPort,
        QueitModeEnable,
        SaveSettings,
        RecallSavedSettings,
        RestoreDefaults,
        RequestAll,
        EnableGERelayFix,
        DisableGERelayFix,
        Idle
    }
    public class RelayProgrammingEventArgs : EventArgs
    {
        public RelayProgrammingEventArgs()
        {
        }

        public byte[] BytesToSend;

        public RelayProgrammingSendCommands Command;
    }

    public enum RelayProgrammingStates
    {

        Idle,
        ManualPortSelectionWaitingForBoot,
        WaitingForBootMaster,
        WaitingForBootRelay,
        WaitingForBootFPGA,
        LoadingMasterCode,
        LoadingMasterData,
        LoadingRelayCode,
        LoadingRelayData,
        LoadingFPGACode,
        ReprogramSuccess,
        Finalized,
        ClearingBootLoader,
        RequestAll,
        WaitForAllData,
        LoadingRelayBootLoader,
        LoadingMasterBootLoader,
        ManualLoadCheckBoot,
        AutoLoadCheckBoot,
        DoneLoadingMasterBootLoader,
        CheckMasterBootCode,
        ReloadMasterBoot
    }

    public class CodeReloaderSingleRelay
    {
        public CodeReloaderSingleRelay()
        {
        }

        public UInt32 SerialNumber;
        public bool GERelay = false;
        public bool DNPRelay = false;
        public bool FPGAPresent = false;
        public string MPRevision;
        public string RPRevision;
        public string FPGARevision;

        public string GetLogString()
        {
            string returnString = DateTime.UtcNow.ToString();

            returnString += " - SN: " + this.SerialNumber.ToString();
            returnString += " - MPRev: " + this.MPRevision.ToString();
            returnString += " - RPRev: " + this.RPRevision.ToString();
            if (this.FPGAPresent)
                returnString += " - FPGARev: " + this.FPGARevision.ToString();

            return returnString;

        }
    }

    public class CustomerLoadFiles
    {
        public CustomerLoadFiles(Customers customer)
        {
            this.Customer = customer;
        }

        public CustomerLoadFiles(CustomerLoadFiles cLF)
        {
            this.Customer = cLF.Customer;
            this.FPGAFile = cLF.FPGAFile;

            this.MasterFileGE = cLF.MasterFileGE;
            this.MasterFileWH = cLF.MasterFileWH;

            this.MasterFileConEdHBD = cLF.MasterFileConEdHBD;
            this.MasterFileConEdSEC = cLF.MasterFileConEdSEC;

            this.RelayFileGE = cLF.RelayFileGE;
            this.RelayFileWH = cLF.RelayFileWH;
        }

        public Customers Customer = Customers.None;
        public UInt32 RelayRevision;
        public UInt32 MasterRevision;
        public UInt32 MasterDNPRevision;

        public string MasterFileGE;
        public string MasterFileWH;
        public string MasterFileGEDNP;
        public string MasterFileWHDNP;
        public string MasterFileDNPPLC;

        public string MasterFileConEdHBD;
        public string MasterFileConEdSEC;

        public string RelayFileWH;
        public string RelayFileGE;

        public FPGAProgrammingData FPGAFile = new FPGAProgrammingData();
    }

    public class RelayTypeChangedEventArgs : EventArgs
    {
        public RelayTypeChangedEventArgs(bool gERelay)
        {
            GERelay = gERelay;
        }

        public bool GERelay { get; set; }
    }
}