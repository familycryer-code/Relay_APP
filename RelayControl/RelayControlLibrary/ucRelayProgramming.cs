using GraphicsServer.GSNet.Widgets;
using NLog;
using SharedResources;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;


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
        private bool bootFirstAutoContinuation = false;
        private bool programBootCodeInProgress = false;
        private UInt32 masterBootRevisionNumberReceived = 0;
        private bool masterBootRevisionSet = false;

        private StartupIntent startupIntent = StartupIntent.None;

        private bool reprogramBootCodeAuto = false;

        private string bootStartUpChar = "0";

        private bool programMasterBootFileSelect = false;
        private string masterBootStringReceived = "0";
        private bool autoLoad = false;
        private bool autoLoadRequested = false;      // approval/session latch (can persist)
        private string masterRevisionString = "";
        private bool notPollingPort = false;
        private bool startWarningShownThisCycle = false;
        private bool reprogrammingInProgress = false;
        private bool bootLoadDone = false;

        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                logger.Trace("Customer: {0}", value);
                this.customer = value;
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
            this.state = RelayProgrammingStates.CheckMasterBootCode;
            sendReset();
        }

        public bool ProgramBootCodeInProgress
        {
            get { return this.programBootCodeInProgress; }
            set
            {
                this.programBootCodeInProgress = value;
            }
        }
        /*
        private void RequestBootCodeUpload()
        {
            logger.Info(
                "RequestBootCodeUpload: state={0}, programBootCodeInProgress={1}, reprogrammingInProgress={2}, startupIntent={3}, backupIsPending={4}",
                this.state,
                this.programBootCodeInProgress,
                this.reprogrammingInProgress,
                this.startupIntent,
                this.backupIsPending);

            if (this.programBootCodeInProgress)
            {
                logger.Warn("RequestBootCodeUpload ignored because boot upload is already active.");
                return;
            }

            bool staleBootContinuation =
                this.state == RelayProgrammingStates.AutoLoadCheckBoot ||
                this.state == RelayProgrammingStates.ManualLoadCheckBoot ||
                this.state  == RelayProgrammingStates.CheckMasterBootCode ||
                (this.masterBootRevisionSet &&
                 this.masterBootRevisionNumberReceived > 0 &&
                 this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber);

            if (this.reprogrammingInProgress && !staleBootContinuation)
            {
                logger.Warn("RequestBootCodeUpload ignored because normal programming is active.");
                return;
            }

            bool manualFlowActive =
                ManualUpdate.usingManualMode ||
                this.backupIsPending ||
                this.state == RelayProgrammingStates.ManualLoadCheckBoot ||
                this.state == RelayProgrammingStates.ManualPortSelectionWaitingForBoot;

            if (manualFlowActive)
            {
                logger.Info("RequestBootCodeUpload manual path: launching boot upload directly.");
                this.ProgramBootCode();
                return;
            }

            if (!this.bootNeedsUpdateAutoProgramming())
            {
                logger.Warn(
                    "RequestBootCodeUpload blocked: boot repair is not required. bootSet={0}, bootRev={1}",
                    this.masterBootRevisionSet,
                    this.masterBootRevisionNumberReceived);
                return;
            }

            logger.Info("RequestBootCodeUpload auto path: boot repair required.");
            this.ProgramBootCode();
        }
        */

        /*
        public void ProgramBootCode()
        {
            if (this.programBootCodeInProgress)
                return;

            this.programBootCodeInProgress = true;
            MasterBootLoaderStart();
        }
        */

        public bool ReprogramBootCodeAuto
        {
            get { return this.reprogramBootCodeAuto; }
            set
            {
                this.reprogramBootCodeAuto = value;
            }
        }

        public delegate void SendDelegate(object o, RelayProgrammingEventArgs rPEA);
        public event SendDelegate Send;

        public delegate void ErrorHandler(object o, ExceptionEventArgs eEA);
        public event ErrorHandler Error;

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
                //UInt32 previousValue = this.remoteMasterRevisionNumber;
                this.remoteMasterRevisionNumber = value;
            }
             
        }

        public UInt32 RelayRevisionNumber
        {
            get { return this.remoteRelayRevisionNumber; }
            set
            {
                this.remoteRelayRevisionNumber = value;
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
            }
        }


        private bool reprogramMaster = false;
        private bool reprogramRelay = false;
        private bool reprogramFPGA = false;
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

        private void RefreshPendingFirmwareFromCurrentRevisions()
        {
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

        private void ContinueManualAfterBootCheck()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

          
            logger.Info("ManualLoadCheckBoot: boot revision acceptable; continuing manual update.");
            this.RunProgrammingStateMachine();
        }

        private bool ContinueAutoloadAfterBootCheck()
        {
            // not sure we need this but..
            if (this.bootNeedsUpdateAutoProgramming())
            {
                this.bootFirstAutoContinuation = true;

                logger.Info(
                    "Boot repair required; launching boot repair path. bootFirstAutoContinuation={0}, pending(M={1},R={2},F={3})",
                    this.bootFirstAutoContinuation,
                    this.reprogramMaster,
                    this.reprogramRelay,
                    this.reprogramFPGA);

              
                this.MasterBootLoaderStart();
                return true;
            }

            this.bootFirstAutoContinuation = false;
            logger.Info("Autoload approved and allowed to proceed.");
            this.autoLoad = true;
            
            this.autoLoadRequested = true;
               
            this.RunProgrammingStateMachine();
            return true;
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

        public delegate void BackupBeforeProgrammingHandler(object sender, BackupBeforeProgrammingEventArgs e);
        public event BackupBeforeProgrammingHandler BackupBeforeProgrammingRequested;

        private void RaiseBackupBeforeProgrammingRequested([System.Runtime.CompilerServices.CallerMemberName] string caller = "unknown")
        {
            BackupBeforeProgrammingRequested?.Invoke(this, new BackupBeforeProgrammingEventArgs(caller));
        }

        public void BackupOnly()
        {
            logger.Info("BackUpOnly: backUpOnly gated");
            this.startupIntent = StartupIntent.BackupOnly; // this really means we are checking..
            RaiseBackupBeforeProgrammingRequested(nameof(BackupOnly));
        }
        public void InitializeAutoload()
        {
            logger.Info("InitializeAutoload: autoload gated");
            this.startupIntent = StartupIntent.AutoApproved; // this really means we are checking..
            RaiseBackupBeforeProgrammingRequested(nameof(InitializeAutoload));
        }

        public event Action<bool> ManualForcedUpdatePromptRequested;
        public void StartManualForcedUpdate()
        {
            this.LogFlow("StartManualForcedUpdate ENTER", nameof(StartManualForcedUpdate));

            this.RefreshPendingFirmwareFromCurrentRevisions();
            this.LogFlow("StartManualForcedUpdate EXIT: no prompt handler registered.", nameof(StartManualForcedUpdate));
            ManualUpdate.usingManualMode = false;

            if (this.IsProgrammingActiveOrInFlight())
            {
                logger.Warn("InitializeAutoload ignored because programming is already active.");
                this.LogFlow("StartManualForcedUpdate EXIT: programming already active/in-flight.", nameof(StartManualForcedUpdate));
                return;
            }

            if (this.ManualForcedUpdatePromptRequested != null)
            {
                this.ManualForcedUpdatePromptRequested(true);
                return;
            }
        }

        public void OnManualPromptsApproved()
        {
            this.LogFlow("OnManualPromptsApproved", nameof(OnManualPromptsApproved));

            //this.RefreshPendingFirmwareFromCurrentRevisions();
            this.setProgrammingFiles();

            ManualUpdate.usingManualMode = true;
            this.startupIntent = StartupIntent.ManualApproved;

            this.LogFlow("StartManualForcedUpdate: approved; resuming manual flow after backup.", nameof(StartManualForcedUpdate));
            this.ResumeManualAfterBackup();
        }

        public void HandleManualDeclined()
        {
            this.LogFlow("HandleManualDeclined", nameof(HandleManualDeclined));
            ManualUpdate.usingManualMode = false;
            this.ResetAutoloadState("HandleManualDeclined");
        }

        public void ResumeManualAfterBackup()
        {
            logger.Info("ResumeManualAfterBackup ENTER");

            if (this.startupIntent != StartupIntent.ManualApproved)
            {
                logger.Info(
                    "ResumeManualAfterBackup blocked: startupIntent={0}, no manual approval in active state.",
                    this.startupIntent);
                return;
            }

            if (this.IsProgrammingActiveOrInFlight())
            {
                logger.Warn("ResumeManualAfterBackup suppressed; programming already active.");
                return;
            }

            //this.RefreshPendingFirmwareFromCurrentRevisions();

            if (!this.AnyFirmwarePending())
            {
                logger.Info("ResumeManualAfterBackup: no firmware pending; exiting.");
                this.ResetAutoloadState("ResumeManualAfterBackup");
                return;
            }

            logger.Info("ResumeManualAfterBackup start programming Manual mode.");
            this.RunProgrammingStateMachine();
        }

        public void ResumeAutoloadAfterBackup()
        {
            logger.Info("ResumeAutoloadAfterBackup ENTER");

            if (this.startupIntent != StartupIntent.AutoApproved)
            {
                logger.Info("ResumeAutoloadAfterBackup blocked: startupIntent not AutoApproved.");
                return;
            }

            if (this.IsProgrammingActiveOrInFlight())
            {
                logger.Warn("ResumeAutoloadAfterBackup suppressed; programming already active.");
                return;
            }

            //this.RefreshPendingFirmwareFromCurrentRevisions();

            if (!this.AnyFirmwarePending())
            {
                logger.Info("ResumeAutoloadAfterBackup: no firmware pending; exiting.");
                this.ResetAutoloadState("ResumeAutoloadAfterBackup");
                this.RaiseAutoloadDeclined();
                return;
            }

        
            logger.Info("ResumeAutoloadAfterBackup approved path -> re-entering autoload startup flow.");
            this.startAutoLoad();
        }

        // in ucRelayProgramming
        public void DeclineAutoload(string caller, string reason)
        {
            this.ResetAutoloadState(caller);
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

            logger.Info("setProgrammingFiles OK: masterLen={0}, relayLen={1}, GERelay={2}, customer={3}",
                this.masterCode?.FileString?.Length ?? 0,
                this.relayCode?.FileString?.Length ?? 0,
                this.GERelay,
                customerDisplayName);
        }


        formProgrammingProgess programmingForm = new formProgrammingProgess();

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

        private void addGERelayToTransmitterPacket(bool b)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (b)
                this.TransmitterPacket[28] |= 0x10;
            else
                this.TransmitterPacket[28] &= 0xEF;
        }

        private bool IsManualFlowActive()
        {
            return ManualUpdate.usingManualMode
                || this.state == RelayProgrammingStates.ManualLoadCheckBoot;
        }

        // inside ucRelayProgramming
        private bool deferredAutoloadRetry = false;

        public void DeferAutoloadUntilUiReady()
        {
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                this.deferredAutoloadRetry = true;
                return;
            }

            if (this.deferredAutoloadRetry)
            {
                this.deferredAutoloadRetry = false;
                this.startAutoLoad();
            }
        }

        private bool autoPromptsApprovedThisCycle = false;
        private bool autoloadDeferredUntilUiReady = false;

        public bool AutoloadDeferredUntilUiReady
        {
            get { return this.autoloadDeferredUntilUiReady; }
            set { this.autoloadDeferredUntilUiReady = value; }
        }

        public void TryResumeDeferredAutoload()
        {
            logger.Info("TryResumeDeferredAutoload called. autoloadDeferredUntilUiReady={0}, IsDisposed={1}, IsHandleCreated={2}",
                this.autoloadDeferredUntilUiReady, this.IsDisposed, this.IsHandleCreated);

            if (this.IsDisposed || !this.IsHandleCreated)
            {
                this.autoloadDeferredUntilUiReady = true;
                return;
            }

            if (this.autoloadDeferredUntilUiReady)
            {
                this.autoloadDeferredUntilUiReady = false;
                logger.Info("Calling startAutoLoad() from deferred retry");
                this.startAutoLoad();
            }
        }

        public event Action AutoloadPromptRequested;

        public bool waitingForAutoloadApproval = false;

        private void startAutoLoad()
        {
            this.LogFlow("startAutoLoad ENTER", nameof(startAutoLoad));

            if (!this.autoPromptsApprovedThisCycle)
            {
                if (this.waitingForAutoloadApproval)
                {
                    this.LogFlow("startAutoLoad EXIT: waiting for autoload approval.", nameof(startAutoLoad));
                    return;
                }

                this.waitingForAutoloadApproval = true;
                this.LogFlow("startAutoLoad: requesting UI prompt flow.", nameof(startAutoLoad));
                this.AutoloadPromptRequested?.Invoke();
                return;
            }

            if (this.IsManualFlowActive())
            {
                this.LogFlow("startAutoLoad EXIT: manual flow is active.", nameof(startAutoLoad));
                return;
            }

            if (this.serialNumberError)
            {
                this.LogFlow("startAutoLoad EXIT: serial number error detected.", nameof(startAutoLoad));
                this.RaiseAutoloadDeclined();
                return;
            }

            this.setProgrammingFiles();
            this.RefreshPendingFirmwareFromCurrentRevisions();

            if (!this.AnyFirmwarePending())
            {
                this.LogFlow("startAutoLoad EXIT: no firmware pending.", nameof(startAutoLoad));
                this.RaiseAutoloadDeclined();
                return;
            }

            if (!this.autoPromptsApprovedThisCycle)
            {
                this.LogFlow("startAutoLoad: requesting UI prompt flow.", nameof(startAutoLoad));
                this.waitingForAutoloadApproval = true;
                this.AutoloadPromptRequested?.Invoke();
                return;
            }

            bool bootUnknown = !this.masterBootRevisionSet || this.masterBootRevisionNumberReceived <= 0;
            bool bootOld = this.masterBootRevisionSet &&
                           this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber;

            if (bootUnknown)
            {
                this.LogFlow("startAutoLoad ACTION: boot unknown -> AutoLoadCheckBoot + sendReset.", nameof(startAutoLoad));
                this.state = RelayProgrammingStates.AutoLoadCheckBoot;
                this.sendReset();
                return;
            }

            if (bootOld)
            {
                this.LogFlow("startAutoLoad ACTION: boot old -> CheckProperMasterBootCode().", nameof(startAutoLoad));
                this.CheckProperMasterBootCode();

                if (this.bootFirstAutoContinuation)
                {
                    this.LogFlow("startAutoLoad EXIT: boot repair scheduled/active.", nameof(startAutoLoad));
                    return;
                }
            }

            this.LogFlow("startAutoLoad ACTION: boot acceptable -> AutoLoadCheckBoot + sendReset.", nameof(startAutoLoad));
            this.state = RelayProgrammingStates.AutoLoadCheckBoot;
            this.sendReset();
        }

        public void OnAutoloadPromptsApproved()
        {
            this.LogFlow("OnAutoloadPromptsApproved", nameof(OnAutoloadPromptsApproved));
            this.waitingForAutoloadApproval = false;
            this.autoPromptsApprovedThisCycle = true;

            this.startAutoLoad();
        }

        public void RaiseAutoloadDeclined()
        {
            this.LogFlow("RaiseAutoloadDeclined", nameof(RaiseAutoloadDeclined));
            this.waitingForAutoloadApproval = false;
            this.autoPromptsApprovedThisCycle = false;
            this.ResetAutoloadState("RaiseAutoloadDeclined");
            AutoloadDeclined?.Invoke(this, EventArgs.Empty);
        }
        /*
        public void PrepForBoot()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            switch (this.state)
            {
                case RelayProgrammingStates.WaitingForMaster:
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

                default:
                    break;
            }

            this.labelState.Text = "Waiting For Boot";
            logger.Trace("Waiting For Boot - " + this.state.ToString());
        }
        */

        public void SetTransmitterPacket(byte[] bytePacket)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.TransmitterPacket = bytePacket;
        }
        private void SendTransmitterSettings()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (this.TransmitterPacket == null)
            {
                logger.Trace("No transmitter settings to send.");
                return;
            }

            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.BytesToSend = this.TransmitterPacket;
            rPEA.Command = RelayProgrammingSendCommands.TransmitterSettings;

            this.onSend(rPEA);
            this.TransmitterPacket = null;

            logger.Trace("Updated Transmitter Settings with Stored Settings");
        }

        private delegate void booleanInvoke(bool b);

        private void onSend(RelayProgrammingEventArgs rPEA)
        {
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            logger.Info(String.Format("Command: {0}", rPEA.Command));

            if (rPEA.Command == RelayProgrammingSendCommands.RequestAll)
            {
                
                    if (this.IsProgrammingActiveOrInFlight())
                    {
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
                switch (this.state)
                {
                    case RelayProgrammingStates.Idle:
                    default:
                        break;

                    case RelayProgrammingStates.LoadingMasterCode:
                    case RelayProgrammingStates.LoadingMasterData:
                        logger.Info("AckM, ");
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.sendNextMasterPacket();
                        break;

                    case RelayProgrammingStates.LoadingRelayCode:
                    case RelayProgrammingStates.LoadingRelayData:
                        logger.Info("AckR, ");
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.sendNextRelayPacket();
                        break;

                    case RelayProgrammingStates.LoadingFPGACode:
                        logger.Info("AckF, ");
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.sendNextFPGAPacket();
                        break;

                    case RelayProgrammingStates.LoadingRelayBootLoader:
                        logger.Info("Ack Unknown, ");
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        //this.sendRelayBootCode();  will need to figure this out
                        break;

                    case RelayProgrammingStates.LoadingMasterBootLoader:
                        logger.Trace("AckU, ");
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 5000;
                        this.timerTimeout.Start();
                        this.sendMasterBootCode();
                        break;
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error in Packet Acknowledge", ex);
            }
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
            logger.Info("Boot Received - {0}", this.state);

            bool programmingContext =
                this.reprogrammingInProgress ||
                this.programBootCodeInProgress ||
                this.autoLoad ||
                ManualUpdate.usingManualMode ||
                this.bootFirstAutoContinuation ||
                this.state == RelayProgrammingStates.AutoLoadCheckBoot ||
                this.state == RelayProgrammingStates.ManualLoadCheckBoot ||
                this.state == RelayProgrammingStates.CheckMasterBootCode ||
                this.state == RelayProgrammingStates.LoadingMasterBootLoader ||
                this.state == RelayProgrammingStates.ReloadMasterBoot;

            if (programmingContext)
            {
                this.programmingForm.Show();
            }

            string rawBoot = bootReceived ?? string.Empty;
            string normalizedBoot = rawBoot.Trim();
            this.masterBootStringReceived = normalizedBoot;

            string numericPart = Regex.Match(normalizedBoot, @"\d+").Value;
            this.bootStartUpChar = rawBoot.Length >= 3 ? rawBoot.Substring(3, 1) : "0";

            if (!string.IsNullOrEmpty(numericPart) &&
                UInt32.TryParse(numericPart, out UInt32 parsedRevision))
            {
                this.masterBootRevisionNumberReceived = parsedRevision;
                this.masterBootRevisionSet = true;
                logger.Info("BootReceived parsed revision: {0}", parsedRevision);
            }
            else
            {
                this.masterBootRevisionNumberReceived = 0;
                this.masterBootRevisionSet = false;
                logger.Warn("BootReceived: non-numeric/unknown boot revision. Raw='{0}'", normalizedBoot);
            }

            switch (this.state)
            {
                case RelayProgrammingStates.WaitingForMaster:
                case RelayProgrammingStates.WaitingForBootMaster:
                case RelayProgrammingStates.WaitingForBootFPGA:
                case RelayProgrammingStates.LoadingMasterCode:
                case RelayProgrammingStates.LoadingMasterData:
                case RelayProgrammingStates.LoadingRelayData:
                case RelayProgrammingStates.LoadingFPGACode:
                case RelayProgrammingStates.LoadingMasterBootLoader:
                case RelayProgrammingStates.LoadingRelayBootLoader:
                case RelayProgrammingStates.ClearingBootLoader:
                    logger.Info("BootReceived ignored; current state is already an active transfer state. state={0}", this.state);
                    break;

                case RelayProgrammingStates.CheckMasterBootCode:
                    logger.Info("CheckMasterBootCode");
                    this.CheckProperMasterBootCode();
                    break;

                case RelayProgrammingStates.AutoLoadCheckBoot:
                    logger.Info(
                        "AutoLoadCheckBoot: entering continuation evaluation. , masterBootRevisionSet={0}, masterBootRevisionNumberReceived={1}",
                        this.masterBootRevisionSet,
                        this.masterBootRevisionNumberReceived);
                    this.ContinueAutoloadAfterBootCheck();
                    break;

                case RelayProgrammingStates.ManualLoadCheckBoot:
                    logger.Info(
                        "ManualLoadCheckBoot: entering continuation evaluation. , masterBootRevisionSet={0}, masterBootRevisionNumberReceived={1}",
                        this.masterBootRevisionSet,
                        this.masterBootRevisionNumberReceived);
                    this.ContinueManualAfterBootCheck();
                    break;

                case RelayProgrammingStates.ReloadMasterBoot:
                    this.timerTimeout.Stop();
                    Thread.Sleep(3000);
                    this.programBootCodeInProgress = false;
                    this.MasterBootLoaderStart();
                    break;
                case RelayProgrammingStates.LoadingRelayCode:
                    this.timerTimeout.Stop();
                    this.sendRelayTransferPacket();
                    break;
                default:
                    break;
            }
        }

        private void sendNextRelayPacket()
        {
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            if (this.state == RelayProgrammingStates.LoadingRelayCode)
            {
                try
                {

                    rPEA.BytesToSend = new byte[1024];
                    this.programmingForm.CurrentTask = "Loading Relay Code";

                    if (this.relayCode.CodeBytes.Count == 0)
                    {
                        this.state = RelayProgrammingStates.LoadingRelayData;
                        this.programmingForm.RelayCodeComplete = true;
                        this.programmingForm.CurrentTask = "Loading Relay Data";
                        logger.Info("Loading Relay Data");
                        this.programmingForm.Maximum = this.relayCode.NumberOfDataBlocks;

                        this.sendNextRelayPacket();
                        return;
                    }

                    for (int i = 0; i < 1024; i++)
                    {
                        rPEA.BytesToSend[i] = this.relayCode.CodeBytes[0];
                        this.relayCode.CodeBytes.RemoveAt(0);
                    }

                    // build 512 bytes
                    Int32 temp = Convert.ToInt32(this.labelCodeCount.Text);
                    temp++;
                    this.programmingForm.ProgressValue = temp;
                    this.labelCodeCount.Text = temp.ToString();

                    logger.Info("RC, ");

                    this.onSend(rPEA);
                }
                catch (Exception ex)
                {
                    this.errorHandler("Error Sending Next Relay Code", ex);
                }
            }
            else if (this.state == RelayProgrammingStates.LoadingRelayData)
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

                logger.Info("RD, ");
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

            if (this.state == RelayProgrammingStates.LoadingRelayBootLoader)
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

        private void ResetAutoloadState(string caller = "unknown")
        {
            this.LogFlow("ResetAutoloadState ENTER | pre", caller);

            this.reprogrammingInProgress = false;
            this.programBootCodeInProgress = false;

            this.NotPollingPort = false;
            this.autoLoad = false;
            this.autoLoadRequested = false;
            this.autoPromptsApprovedThisCycle = false;
            this.bootFirstAutoContinuation = false;

            this.reprogramMaster = false;
            this.reprogramRelay = false;
            this.reprogramFPGA = false;

            this.bootLoadDone = false;
            this.startupIntent = StartupIntent.None;

            this.LogFlow("ResetAutoloadState EXIT | post", caller);
        }

        public void LogFlow(string message, string caller)
        {
            logger.Info(
                "{0} | caller={1} | state={2} | startupIntent={3} | " +
                "reprogrammingInProgress={4} | programBootCodeInProgress={5} | " +
                "autoPromptsApprovedThisCycle={6} | bootFirstAutoContinuation={7} | " +
                "serialNumberError={8} | " +
                "masterBootRevisionSet={9} | masterBootRevisionNumberReceived={10} | requiredBootRevision={11} | " +
                "activeRelay={12} | " +
                "currentMasterRevision={13} | currentRelayRevision={14} | currentFPGARevision={15} | " +
                "resourceMasterRevision={16} | resourceRelayRevision={17} | resourceFPGARevision={18} | " +
                "autoLoad={19} | reprogramMaster={20} | reprogramRelay={21} | reprogramFPGA={22} | bootLoadDone={23}",
                message,
                string.IsNullOrWhiteSpace(caller) ? "unknown" : caller,
                this.state,
                this.startupIntent,
                this.reprogrammingInProgress,
                this.programBootCodeInProgress,
                this.autoPromptsApprovedThisCycle,
                this.bootFirstAutoContinuation,
                this.serialNumberError,
                this.masterBootRevisionSet,
                this.masterBootRevisionNumberReceived,
                _bootCodeRevisionNumber,
                this.ActiveRelay,
                this.MasterRevisionNumber,
                this.RelayRevisionNumber,
                this.FPGARevisionNumber,
                _masterCodeRevisionNumber,
                _relayCodeRevisionNumber,
                _fPGACodeRevisionNumber,
                this.autoLoad,
                this.reprogramMaster,
                this.reprogramRelay,
                this.reprogramFPGA,
                this.bootLoadDone);
        }
        private void MasterBootLoaderStart()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            logger.Info("MasterBootLoaderStart: begin at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));

            this.programBootCodeInProgress = true;
            this.reprogrammingInProgress = true;

            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;
            this.state = RelayProgrammingStates.LoadingMasterBootLoader;


            int temp = 0;

            this.failCount = 0;

            this.setProgrammingFiles();

            if ((this.masterCode.FileString == "" || this.masterCode.FileString == null))
            {
                this.dontReloadFromResource = false;
                this.setProgrammingFiles();
            }

            if (this.state == RelayProgrammingStates.LoadingMasterBootLoader)
            {
                rPEA.BytesToSend = new byte[4];

                this.parseBootLoaderSFile(this.masterCode);

                if (this.masterCode.CodeBytes == null || this.masterCode.CodeBytes.Count == 0)
                {
                    logger.Error("MasterBootLoaderStart aborted: parsed boot payload is empty.");
                    this.programBootCodeInProgress = false;
                    this.reprogrammingInProgress = false;
                    this.state = RelayProgrammingStates.Idle;
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
                
                logger.Info("MasterBootLoaderStart: sending #U at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
                logger.Info("MasterBootLoaderStart sending #U packet: {0}", BitConverter.ToString(rPEA.BytesToSend));

                this.onSend(rPEA);

                logger.Info("MasterBootLoaderStart: #U packet sent at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
                Thread.Sleep(2000);  // wait for relay to send READY
                confirmProgramMasterBoot();
            }
        }

        private void confirmProgramMasterBoot()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            this.failCount = 0;

            if (this.state == RelayProgrammingStates.LoadingMasterBootLoader)
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

                // short settle after YES so bootloader can enter data receive mode
                Thread.Sleep(250);

                logger.Info("confirmProgramMasterBoot: starting boot payload upload at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        private DateTime? bootAckDeadline = null;
        private int bootAckRetryCount = 0;
        private const int BootAckTimeoutMs = 3000;
        private const int BootAckMaxRetries = 2;

        private void CheckBootAckTimeout()
        {
            if (this.state != RelayProgrammingStates.LoadingMasterBootLoader)
                return;

            if (!this.bootAckDeadline.HasValue)
                return;

            if (DateTime.Now <= this.bootAckDeadline.Value)
                return;

            this.bootAckRetryCount++;

            logger.Warn(
                "BOOT ACK TIMEOUT: retry={0}/{1}, deadline={2}, now={3}, remainingBytes={4}, state={5}",
                this.bootAckRetryCount,
                BootAckMaxRetries,
                this.bootAckDeadline.Value.ToString("HH:mm:ss.fff"),
                DateTime.Now.ToString("HH:mm:ss.fff"),
                this.masterCode != null && this.masterCode.CodeBytes != null ? this.masterCode.CodeBytes.Count : -1,
                this.state);

            this.bootAckDeadline = null;

            if (this.bootAckRetryCount <= BootAckMaxRetries)
            {
                logger.Warn("BOOT ACK TIMEOUT: retrying current boot block send.");
                this.sendMasterBootCode();
            }
            else
            {
                logger.Error("BOOT ACK TIMEOUT: max retries exceeded; aborting bootloader upload.");

                this.programBootCodeInProgress = false;
                this.reprogrammingInProgress = false;
                this.state = RelayProgrammingStates.Idle;

                // optional: route through your existing error pipeline
                // this.errorHandler("Bootloader ACK timeout exceeded retries.", null);
            }
        }

        private void sendMasterBootCode()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            logger.Info("sendMasterBootCode: entered at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));

            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            this.failCount = 0;

            if (this.state != RelayProgrammingStates.LoadingMasterBootLoader)
                return;

            this.programmingForm.CurrentTask = "Loading Master Boot";
            Int32 temp = Convert.ToInt32(this.labelCodeCount.Text);

            try
            {
                if (this.masterCode.CodeBytes == null || this.masterCode.CodeBytes.Count == 0)
                {
                    logger.Info("sendMasterBootCode: no remaining boot bytes; completing bootloader phase.");
                    this.bootAckDeadline = null;
                    this.bootAckRetryCount = 0;
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

                // Start ACK wait window for this block
                this.bootAckDeadline = DateTime.Now.AddMilliseconds(BootAckTimeoutMs);

                logger.Info(
                    "sendMasterBootCode: sending boot block length={0} at {1}, ackDeadline={2}, remainingBytesAfterBuild={3}",
                    rPEA.BytesToSend.Length,
                    DateTime.Now.ToString("HH:mm:ss.fff"),
                    this.bootAckDeadline.Value.ToString("HH:mm:ss.fff"),
                    this.masterCode.CodeBytes.Count);

                this.onSend(rPEA);

                logger.Info("sendMasterBootCode: boot block sent at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));

                this.programmingForm.ProgressValue = temp;
                temp++;
                this.labelCodeCount.Text = temp.ToString();
                logger.Trace("BL, ");
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Sending Next Master Boot Data", ex);
            }
        }

        private void sendNextMasterPacket()
        {
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            this.failCount = 0;

            if (this.state == RelayProgrammingStates.LoadingMasterCode)
            {

                try
                {
                    this.programmingForm.CurrentTask = "Loading Master Code";

                    rPEA.BytesToSend = new byte[1024];

                    if (this.masterCode.CodeBytes.Count == 0)
                    {
                        this.state = RelayProgrammingStates.LoadingMasterData;
                        this.programmingForm.MasterCodeComplete = true;
                        if (this.masterCode.WithParameters)
                            this.programmingForm.Maximum = this.masterCode.NumberOfDataBlocks;
                        else
                            this.programmingForm.Maximum = this.masterCode.NonParameterCount;

                        this.programmingForm.CurrentTask = "Loading Master Data";
                        logger.Info("Loading Master Data");
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
                    logger.Info("MC, ");
                }
                catch (Exception ex)
                {
                    this.errorHandler("Error Sending Next Master Data", ex);
                }
            }
            else if (this.state == RelayProgrammingStates.LoadingMasterData)
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
                logger.Info("MD, ");
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
                this.programmingForm.CurrentTask = "Loading FPGA Code";

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

            this.timerTimeout.Stop();
            this.programmingForm.FPGAComplete = true;

            this.reprogramFPGA = false;

            this.reprogrammingInProgress = false;

            logger.Info(
                "doneLoadingMaster complete: reprogramMaster={0}, reprogramRelay={1}, reprogramFPGA={2}, state={3}",
                this.reprogramMaster,
                this.reprogramRelay,
                this.reprogramFPGA,
                this.state);

            // Re-enter central dispatcher/engine to choose the next required stage.
            this.RunProgrammingStateMachine();
        }

        private void doneLoadingRelay()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            this.timerTimeout.Stop();
            this.programmingForm.RelayDataComplete = true;
            this.programmingForm.RelayCodeComplete = true;

            // Consume only the stage that just finished.
            this.reprogramRelay = false;

            this.reprogrammingInProgress = false;

            logger.Info(
                "doneLoadingRelay complete: reprogramMaster={0}, reprogramRelay={1}, reprogramFPGA={2}, autoLoad={3}, startupIntent={4}, state={5}",
                this.reprogramMaster,
                this.reprogramRelay,
                this.reprogramFPGA,
                this.autoLoad,
                this.startupIntent,
                this.state);

            // Hand off all next-stage decisions to the central engine.
            this.RunProgrammingStateMachine();
        }
        private void doneLoadingMaster()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            this.timerTimeout.Stop();
            this.programmingForm.MasterDataComplete = true;

            // Mark only the stage that just completed.
            this.reprogramMaster = false;

            this.reprogrammingInProgress = false;

            logger.Info(
                "doneLoadingMaster complete: reprogramMaster={0}, reprogramRelay={1}, reprogramFPGA={2}, state={3}",
                this.reprogramMaster,
                this.reprogramRelay,
                this.reprogramFPGA,
                this.state);

            // Re-enter central dispatcher/engine to choose the next required stage.
            this.RunProgrammingStateMachine(); // later rename to AdvanceProgrammingState / RunProgrammingEngine
        }

        private void allReprogramingDone()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            bool wasAutoLoad = this.autoLoad;
            bool wasManualFlow =
                this.startupIntent == StartupIntent.ManualApproved ||
                this.state == RelayProgrammingStates.ManualLoadCheckBoot ||
                ManualUpdate.usingManualMode;

            LogFlow("allReprogramingDone","allReprogrammingDone");

            // End-of-programming stage cleanup (safe before requestAll)
            this.reprogrammingInProgress = false;
            this.programBootCodeInProgress = false;
            this.bootFirstAutoContinuation = false;
            this.bootLoadDone = false;

            this.programmingForm.CurrentTask = "Finalizing Update";
            this.state = RelayProgrammingStates.ReprogramSuccess;
            this.timerTimeout.Stop();

            // Orchestrated flow: hand off to RequestAll -> AllParametersReceived -> FinalizeReprogram
            if (wasAutoLoad || wasManualFlow)
            {
                logger.Info("allReprogramingDone: final success path -> issuing final requestAll().");
                this.requestAll();
                return;
            }

            this.state = RelayProgrammingStates.Idle;
        }

        public event Action FinalSuccessPopupRequested;

        public void finalizeReprogram()
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

            this.reprogrammingInProgress = false;
            this.programBootCodeInProgress = false;
            this.bootFirstAutoContinuation = false;
            this.autoLoad = false;
            this.bootLoadDone = false;

            // Parent (MainControl) owns all popups
            FinalSuccessPopupRequested?.Invoke();

            this.programmingForm.ClearAllChecks();
            logger.Trace("Reprogram Completed Successfully");

            this.state = RelayProgrammingStates.Idle;
            dataB.oldDataBackup = true;
            this.restartProgram();

            this.ResetAutoloadState("FinalizeReprogram");
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

        public bool IsProgrammingActiveOrInFlight()
        {
            return 
                   this.state == RelayProgrammingStates.WaitingForMaster
                || this.state == RelayProgrammingStates.WaitingForBootMaster
                || this.state == RelayProgrammingStates.WaitingForBootRelay
                || this.state == RelayProgrammingStates.WaitingForBootFPGA
                || this.state == RelayProgrammingStates.LoadingMasterBootLoader
                || this.state == RelayProgrammingStates.LoadingMasterCode
                || this.state == RelayProgrammingStates.LoadingMasterData
                || this.state == RelayProgrammingStates.LoadingRelayCode
                || this.state == RelayProgrammingStates.LoadingRelayData
                || this.state == RelayProgrammingStates.LoadingFPGACode
                || this.state == RelayProgrammingStates.AutoLoadCheckBoot
                || this.state == RelayProgrammingStates.ManualLoadCheckBoot
                || this.state == RelayProgrammingStates.CheckMasterBootCode
                || this.state == RelayProgrammingStates.ReloadMasterBoot
                || this.state == RelayProgrammingStates.ReprogramSuccess
                || this.state == RelayProgrammingStates.Finalized
                || this.state == RelayProgrammingStates.WaitForAllData;

            //|| this.programBootCodeInProgress
            //this.reprogrammingInProgress
        }

        private void doneLoadingMasterBootLoader()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            bool bootOnlyAutoContinuation = this.bootFirstAutoContinuation;
            bool pendingMaster = this.reprogramMaster;
            bool pendingRelay = this.reprogramRelay;
            bool pendingFPGA = this.reprogramFPGA;

            bool manualFlowActive =
                this.startupIntent == StartupIntent.ManualApproved ||
                ManualUpdate.usingManualMode;

            this.enableButtons(true);
            this.timerTimeout.Stop();
            this.programmingForm.MasterBootComplete = true;
            this.bootLoadDone = true;

            // True terminal case only
            if (bootOnlyAutoContinuation && !pendingMaster && !pendingRelay && !pendingFPGA)
            {
                this.programBootCodeInProgress = false;
                this.reprogrammingInProgress = false;

                logger.Info("COMPLETE PATH doneLoadingMasterBootLoader: auto boot-only continuation finalization.");
                this.programmingForm.Hide();
                Application.DoEvents();
                this.restartProgram();
                this.state = RelayProgrammingStates.Idle;
                return;
            }

            // Keep lifecycle active while handing off to post-boot gate.
            this.reprogrammingInProgress = false;
            this.programBootCodeInProgress = false;

            if (manualFlowActive)
            {
                this.state = RelayProgrammingStates.ManualLoadCheckBoot;
            }
            else if (this.autoLoad)
            {
                this.state = RelayProgrammingStates.AutoLoadCheckBoot;
            }
            else
            {
                logger.Warn("doneLoadingMasterBootLoader: no active programming flow; returning to idle.");
                this.reprogrammingInProgress = false;
                this.state = RelayProgrammingStates.Idle;
                return;
            }

            logger.Info("doneLoadingMasterBootLoader: boot payload complete; entering {0}.", this.state);
            // No reset here; downstream stage start owns reset.
        }


        private bool bootNeedsUpdateAutoProgramming()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (this.programBootCodeInProgress)
                return false;

            bool manualFlowActive =
                this.state == RelayProgrammingStates.ManualLoadCheckBoot ||
                ManualUpdate.usingManualMode;

           
            // Manual full-update: unknown boot => force boot-first.
            // Auto flow: unknown boot => do not force here; let explicit boot-read path resolve it.
            if (!this.masterBootRevisionSet || this.masterBootRevisionNumberReceived <= 0)
            {
                logger.Info(
                    "Boot revision unknown. manualFlowActive={0}, bootSet={1}, bootRev={2}",
                    manualFlowActive,
                    this.masterBootRevisionSet,
                    this.masterBootRevisionNumberReceived);

                return manualFlowActive;
            }

            bool outdated = this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber;

            logger.Info(
                "Boot load required check: manualFlowActive={0}, bootRev={1}, required={2}, outdated={3}",
                manualFlowActive,
                this.masterBootRevisionNumberReceived,
                _bootCodeRevisionNumber,
                outdated);

            return outdated;
        }

        private void sendNonTransmitterSettings()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            if (!this.useDefaultSettings)
                rPEA.Command = RelayProgrammingSendCommands.RecallSavedSettings;
            else
                rPEA.Command = RelayProgrammingSendCommands.RestoreDefaults;

            this.onSend(rPEA);
        }

        private void sendMasterTransferPacket()
        {
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            this.state = RelayProgrammingStates.LoadingMasterCode;

            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.BytesToSend = new byte[4];

            rPEA.BytesToSend[0] = 0x55; //'U'
            rPEA.BytesToSend[1] = this.masterCode.NumberOfCodeBlocks;
            rPEA.BytesToSend[2] = this.masterCode.WithParameters
                ? this.masterCode.NumberOfDataBlocks
                : this.masterCode.NonParameterCount;
            rPEA.BytesToSend[3] = 0x0D;

            this.onSend(rPEA);
            this.labelState.Text = "Sent Master Transfer Packet";
            logger.Info("Sent Master Transfer Packet");

            this.timerTimeout.Stop();
            this.timerTimeout.Interval = 7000;
            this.timerTimeout.Start();

            // NO ACK coming
        }

        private void sendRelayTransferPacket()
        {
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            this.state = RelayProgrammingStates.LoadingRelayCode;

            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.BytesToSend = new byte[4];

            rPEA.BytesToSend[0] = 0x64; // 'd'
            rPEA.BytesToSend[1] = this.relayCode.NumberOfCodeBlocks;

            // Band-aid: if NonParameterCount is zero but DataBytes exist, derive block count from payload.
            byte declaredDataBlocks = this.relayCode.WithParameters
                ? this.relayCode.NumberOfDataBlocks
                : this.relayCode.NonParameterCount;

            if (!this.relayCode.WithParameters &&
                declaredDataBlocks == 0 &&
                this.relayCode.DataBytes != null &&
                this.relayCode.DataBytes.Count > 0)
            {
                declaredDataBlocks = (byte)Math.Ceiling(this.relayCode.DataBytes.Count / 512.0);
                logger.Warn(
                    "Relay transfer header band-aid applied: NonParameterCount=0 but DataBytes={0}; forcing dataBlocks={1}",
                    this.relayCode.DataBytes.Count,
                    declaredDataBlocks);
            }

            rPEA.BytesToSend[2] = declaredDataBlocks;
            rPEA.BytesToSend[3] = 0x0D;

            logger.Warn(
                "Relay transfer header: codeBlocks={0}, dataBlocks={1}, codeBytes={2}, dataBytes={3}, withParameters={4}, nonParamCount={5}",
                this.relayCode.NumberOfCodeBlocks,
                declaredDataBlocks,
                this.relayCode.CodeBytes.Count,
                this.relayCode.DataBytes.Count,
                this.relayCode.WithParameters,
                this.relayCode.NonParameterCount
            );

            this.onSend(rPEA);
            this.labelState.Text = "Sent Relay Transfer Packet";
            logger.Info("Sent Relay Transfer Packet");

            this.timerTimeout.Stop();
            this.timerTimeout.Interval = 7000;
            this.timerTimeout.Start();
        }

        private void sendFPGATransferPacket()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            this.state = RelayProgrammingStates.LoadingFPGACode;

            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.BytesToSend = new byte[2];

            rPEA.BytesToSend[0] = 0x46; // 'F'
            rPEA.BytesToSend[1] = 0x0D;

            this.onSend(rPEA);
            this.labelState.Text = "Sent FPGA Transfer Packet";
            logger.Trace("Sent FPGA Transfer Packet");
            
            this.timerTimeout.Stop();
            this.timerTimeout.Interval = 7000;
            this.timerTimeout.Start();

            // NO ACK coming
        }

        private void CheckProperMasterBootCode()
        {
            bool manualFlowActive =
                this.state == RelayProgrammingStates.ManualLoadCheckBoot ||
                ManualUpdate.usingManualMode;

            if (manualFlowActive)
            {
                logger.Info("Manual flow active: bypassing auto boot-repair decision logic.");
                this.bootFirstAutoContinuation = false;
                return;
            }

            if (this.programBootCodeInProgress)
            {
                logger.Warn("CheckProperMasterBootCode suppressed: boot repair already active.");
                return;
            }

            bool staleBootContinuation =
                this.state == RelayProgrammingStates.AutoLoadCheckBoot ||
                this.state == RelayProgrammingStates.ManualLoadCheckBoot ||
                this.state == RelayProgrammingStates.CheckMasterBootCode;

            if (this.reprogrammingInProgress &&
                !staleBootContinuation &&
                !this.programBootCodeInProgress)
            {
                logger.Warn("Ignoring boot decision because normal programming is already active.");
                return;
            }

            logger.Info("Boot load required (unknown or older revision).");

            // Auto path only: boot-first continuation gate.
            this.bootFirstAutoContinuation = true;

            if (!this.programBootCodeInProgress)
            {
                logger.Info(
                    "CheckProperMasterBootCode: launching boot repair. bootFirstAutoContinuation={0}, pending(M={1},R={2},F={3})",
                    this.bootFirstAutoContinuation,
                    this.reprogramMaster,
                    this.reprogramRelay,
                    this.reprogramFPGA);

                this.MasterBootLoaderStart();
            }
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
            logger.Info(
    "parseSFile summary: file={0}, codeBytes={1}, dataBytes={2}, codeBytes%1024={3}, codeBytes%2048={4}, dataBytes%512={5}, dataBytes%1024={6}, numberOfCodeBlocks={7}, numberOfDataBlocks={8}, nonParamCount={9}, withParameters={10}",
    rPD.FileName ?? "<resource>",
    rPD.CodeBytes.Count,
    rPD.DataBytes.Count,
    rPD.CodeBytes.Count % 1024,
    rPD.CodeBytes.Count % 2048,
    rPD.DataBytes.Count % 512,
    rPD.DataBytes.Count % 1024,
    rPD.NumberOfCodeBlocks,
    rPD.NumberOfDataBlocks,
    rPD.NonParameterCount,
    rPD.WithParameters
);
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
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

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
                    this.state = RelayProgrammingStates.Idle;
                    return;
                }
            }

            this.reprogrammingInProgress = true;

            this.parseSFile(this.relayCode);

            int temp = this.relayCode.NumberOfCodeBlocks * 2;
            this.labelCodeTotal.Text = temp.ToString();
            if(this.relayCode.WithParameters)
            {
                this.labelDataTotal.Text = this.relayCode.NumberOfDataBlocks.ToString();
                logger.Info("DATA)");
            }
            else
            {
                this.labelDataTotal.Text = this.relayCode.NonParameterCount.ToString();
                logger.Info("NO DATA)");
            }

            this.programmingForm.Maximum = temp;
            this.programmingForm.CurrentTask = "Loading Relay Code";

            
            this.labelDataTotal.Text = this.relayCode.NumberOfDataBlocks.ToString();

            this.programmingForm.Maximum = temp;
            this.programmingForm.CurrentTask = "Loading Relay Code";
            this.labelDataCount.Text = "0";
            this.labelCodeCount.Text = "0";

            this.enableButtons(false);
            this.sendRelayReset();  // Send BU
            this.waitForBootReady("startRelayProgramming");
            this.sendRelayTransferPacket();
        }

        private void startRelayBootLoaderProgramming()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            this.failCount = 0;

            if (this.state == RelayProgrammingStates.LoadingRelayBootLoader)
            {
                rPEA.BytesToSend = new byte[3];

                rPEA.BytesToSend[0] = Convert.ToByte('Y');
                rPEA.BytesToSend[1] = Convert.ToByte('E');
                rPEA.BytesToSend[2] = Convert.ToByte('S');

                logger.Info("startRelayBootLoaderProgramming: delaying before YES at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
                Thread.Sleep(250); // small settle delay after initial #U

                logger.Info("startRelayBootLoaderProgramming: sending YES at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
                logger.Info("startRelayBootLoaderProgramming: sending YES payload: {0}", BitConverter.ToString(rPEA.BytesToSend));
                this.onSend(rPEA);
                logger.Info("startRelayBootLoaderProgramming: YES sent at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));

                // short settle after YES so bootloader can enter data receive mode
                Thread.Sleep(250);

                logger.Info("startRelayBootLoaderProgramming: starting boot payload upload at {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        private void startFPGAProgramming()
        {
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            
            this.reprogrammingInProgress = true;

            this.transmitterEnabled = true;  // TBD
            this.parseFPGAFile(this.fPGACode);

            this.programmingForm.CurrentTask = "Loading FPGA";
            this.programmingForm.Maximum = 96;
            
            
            this.labelCodeCount.Text = "0";
            this.labelCodeTotal.Text = "96";
          

            this.enableButtons(false);

            this.sendReset("startFPGAProgramming");
            this.waitForBootReady("startFPGAProgramming");

            this.sendFPGATransferPacket();

        }

        private void startMasterProgramming()
        {
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if ((this.masterCode.FileString == "" || this.masterCode.FileString == null))
            {
                MessageBox.Show("No Master File Loaded");
                this.state = RelayProgrammingStates.Idle;
                return;
            }

            this.reprogrammingInProgress = true;

            this.parseSFile(this.masterCode);

              // do not block comms..
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

            this.enableButtons(false);

            this.sendReset("startMasterProgramming");
            this.waitForBootReady("startMasterProgramming");

            this.sendMasterTransferPacket();
        } 

        private void startMasterProgrammingWithParameters()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if ((this.masterCode.FileString == "" || this.masterCode.FileString == null))
            {
                MessageBox.Show("No Master File Loaded");
                this.state = RelayProgrammingStates.Idle;
                return;
            }
            this.masterCode.WithParameters = true;

            this.parseSFile(this.masterCode);

            this.state = RelayProgrammingStates.WaitingForBootMaster;
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
            this.enableButtons(false);
            this.sendReset();
        }

        private void RunProgrammingStateMachine()
        {
            this.LogFlow("RunProgrammingStateMachine ENTER", nameof(RunProgrammingStateMachine));

            if (!this.autoLoad && !ManualUpdate.usingManualMode)
            {
                this.LogFlow("RunProgrammingStateMachine EXIT: neither autoLoad nor manual mode active.", nameof(RunProgrammingStateMachine));
                return;
            }

            // 1) Pre-flight gate
            if (!this.CanStartProgramming())
            {
                this.LogFlow(
                    $"RunProgrammingStateMachine blocked: CanStartProgramming() false. autoLoad={this.autoLoad}, manualMode={ManualUpdate.usingManualMode}",
                    nameof(RunProgrammingStateMachine));
                return;
            }

            // 2) Suppress while bootloader is actively loading
            if (this.programBootCodeInProgress ||
                this.state == RelayProgrammingStates.LoadingMasterBootLoader)
            {
                this.LogFlow(
                    $"RunProgrammingStateMachine suppressed while bootloader flow is active. state={this.state}, programBootCodeInProgress={this.programBootCodeInProgress}",
                    nameof(RunProgrammingStateMachine));
                return;
            }

            // 5) Ensure payloads exist before payload stage
            if (!this.dontReloadFromResource &&
                (string.IsNullOrWhiteSpace(this.masterCode.FileString) ||
                 (this.reprogramRelay && string.IsNullOrWhiteSpace(this.relayCode.FileString))))
            {
                this.LogFlow("RunProgrammingStateMachine: programming payload missing; reloading embedded resource files.", nameof(RunProgrammingStateMachine));
                this.setProgrammingFiles();
            }

            if (this.reprogramMaster && string.IsNullOrWhiteSpace(this.masterCode.FileString))
            {
                this.LogFlow("RunProgrammingStateMachine aborted: master payload is empty.", nameof(RunProgrammingStateMachine));
           

                if (this.IsManualFlowActive())
                    ManualUpdate.usingManualMode = false;

                this.autoLoad = false;
                this.state = RelayProgrammingStates.Idle;
                return;
            }

            if (this.reprogramRelay && string.IsNullOrWhiteSpace(this.relayCode.FileString))
            {
                this.LogFlow("RunProgrammingStateMachine aborted: relay payload is empty.", nameof(RunProgrammingStateMachine));
               

                if (this.IsManualFlowActive())
                    ManualUpdate.usingManualMode = false;

                this.autoLoad = false;
                this.state = RelayProgrammingStates.Idle;
                return;
            }

            // 6) Refresh pending from revisions
            // this.RefreshPendingFirmwareFromCurrentRevisions();

            bool bootUnknown = !this.masterBootRevisionSet || this.masterBootRevisionNumberReceived <= 0;
            bool bootOld = this.masterBootRevisionSet &&
                           this.masterBootRevisionNumberReceived < _bootCodeRevisionNumber;

            bool manualFlowActive = this.IsManualFlowActive();

            this.LogFlow(
                $"FINAL ORDER CHECK | autoLoad={this.autoLoad}, manualFlowActive={manualFlowActive}, bootSet={this.masterBootRevisionSet}, bootRev={this.masterBootRevisionNumberReceived}, requiredBootRevision={_bootCodeRevisionNumber}, bootUnknown={bootUnknown}, bootOld={bootOld}",
                nameof(RunProgrammingStateMachine));

            // 7) Manual boot-first (keep as requested)
            if (ManualUpdate.usingManualMode && !this.bootLoadDone)
            {
                this.LogFlow(
                    $"RunProgrammingStateMachine: manual full-update requires boot-first. bootUnknown={bootUnknown}, bootOld={bootOld}, bootRev={this.masterBootRevisionNumberReceived}, required={_bootCodeRevisionNumber}",
                    nameof(RunProgrammingStateMachine));

                this.bootFirstAutoContinuation = false;
               
                this.MasterBootLoaderStart();
                return;
            }

            // 8) Auto boot-first continuation
            if (this.autoLoad && this.bootFirstAutoContinuation)
            {
                this.LogFlow("RunProgrammingStateMachine: auto boot-first continuation path requested.", nameof(RunProgrammingStateMachine));
                
                this.MasterBootLoaderStart();
                return;
            }

            // 9) Payload stage selection
            if (this.reprogramMaster)
            {
                this.LogFlow("RunProgrammingStateMachine: master update selected -> start master programming.", nameof(RunProgrammingStateMachine));
               
                this.startMasterProgramming();
                return;
            }

            if (this.reprogramRelay)
            {
                this.LogFlow("RunProgrammingStateMachine: relay update selected -> start relay programming.", nameof(RunProgrammingStateMachine));
              
                this.startRelayProgramming();
                return;
            }

            if (this.reprogramFPGA)
            {
                this.LogFlow("RunProgrammingStateMachine: FPGA update selected -> start FPGA programming.", nameof(RunProgrammingStateMachine));
                this.startFPGAProgramming();
                return;
            }

            // 10) Nothing selected: this is terminal
            this.LogFlow("RunProgrammingStateMachine: no programming branch selected; finalizing.", nameof(RunProgrammingStateMachine));
  
            this.allReprogramingDone();
        }


        /*
        private void resetMasterProgramming()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrEmpty(this.masterCode?.FileString))
            {
                MessageBox.Show("No Master File Loaded");
                this.state = RelayProgrammingStates.Idle;
                return;
            }

            // IMPORTANT:
            // If we are already in active master transfer, do NOT reset/reboot.
            // A reset here causes WaitingForBootMaster loops and restarts progress.
            if (this.state == RelayProgrammingStates.LoadingMasterCode ||
                this.state == RelayProgrammingStates.LoadingMasterData)
            {
                logger.Warn(
                    "resetMasterProgramming suppressed during active master transfer. state={0}",
                    this.state);

                // Keep transfer lifecycle alive; do not clear progress/UI.
                this.reprogrammingInProgress = true;
                this.programBootCodeInProgress = false;
                return;
            }

            // Parse only for true restart/recovery path.
            this.parseSFile(this.masterCode);

            this.reprogrammingInProgress = false;
            this.programBootCodeInProgress = false;

            this.state = RelayProgrammingStates.WaitingForBootMaster;
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
        */

        public StartupIntent CurrentStartupIntent => this.startupIntent;

        public enum StartupIntent
        {
            None,
            BackupOnly,
            AutoApproved,
            ManualApproved
        }

        private bool CanStartProgramming()
        {
            return this.autoLoad || ManualUpdate.usingManualMode;
        }

        /*private void resetProgrammingRelay()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrEmpty(this.relayCode?.FileString))
            {
                MessageBox.Show("No Relay File Loaded");
                this.state = RelayProgrammingStates.Idle;
                return;
            }

            if (this.state == RelayProgrammingStates.LoadingRelayCode ||
                this.state == RelayProgrammingStates.LoadingRelayData)
            {
                logger.Warn(
                    "resetProgrammingRelay suppressed during active relay transfer. state={0}",
                    this.state);

                this.reprogrammingInProgress = true;
                this.programBootCodeInProgress = false;
                return;
            }

            this.parseSFile(this.relayCode);

            this.programmingForm.CurrentTask = "Loading Relay Code Retrying - Waiting For Boot - Please Wait";
            this.programmingForm.RelayCodeComplete = false;
            this.programmingForm.RelayDataComplete = false;

            this.state = RelayProgrammingStates.WaitingForBootRelay;

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

            if (this.state == RelayProgrammingStates.LoadingFPGACode ||
                this.state == RelayProgrammingStates.WaitingForBootFPGA)
            {
                logger.Warn(
                    "resetProgrammingFPGA suppressed during active FPGA transfer. state={0}",
                    this.state);

                this.reprogrammingInProgress = true;
                this.programBootCodeInProgress = false;
                return;
            }

            this.parseFPGAFile(this.fPGACode);

            this.programmingForm.CurrentTask = "Loading FPGA Code - Waiting For Boot - Please Wait";
            this.programmingForm.FPGAComplete = false;

            this.state = RelayProgrammingStates.WaitingForBootFPGA;

            this.labelCodeTotal.Text = "96";
            this.labelDataTotal.Text = "0";
            this.labelCodeCount.Text = "0";
            this.labelDataCount.Text = "0";

            this.sendRelayReset();

            if (programmingForm.MasterBootComplete)
                Thread.Sleep(1000);
        }
        */

        
        private DateTime _lastResetSentUtc = DateTime.MinValue;
        private const int MasterHandoffSettleMs = 50;

        public void waitForBootReady(string label)
        {
            var elapsedMs = (DateTime.UtcNow - _lastResetSentUtc).TotalMilliseconds;
            if (elapsedMs < MasterHandoffSettleMs)
            {
                int wait = (int)(MasterHandoffSettleMs - elapsedMs);
                logger.Info("{0}: settling {1}ms before transfer packet", label, wait);
                Thread.Sleep(wait);
            }
        }

        public void sendReset([System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;
            rPEA.BytesToSend = new byte[] { (byte)'b', (byte)'U', 0x0D };

            logger.Info(
                "UC_RESET_DIAG | caller={0} | state={1} | reprogrammingInProgress={2} | programBootCodeInProgress={3} | notPollingPort={4} | autoLoad={5} | backupIsPending={6} | payload={7}",
                caller,
                this.state,
                this.reprogrammingInProgress,
                this.programBootCodeInProgress,
                this.notPollingPort,
                this.autoLoad,
                BitConverter.ToString(rPEA.BytesToSend)
            );

            this.onSend(rPEA);
            _lastResetSentUtc = DateTime.UtcNow;
        }

        private void sendRelayReset([System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {

            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;
            rPEA.BytesToSend = new byte[3];

            rPEA.BytesToSend[0] = (byte)'B';
            rPEA.BytesToSend[1] = (byte)'U';
            rPEA.BytesToSend[2] = 0x0D;

            logger.Info(
                "UC_RESET_DIAG | caller={0} | state={1} | reprogrammingInProgress={2} | programBootCodeInProgress={3} | notPollingPort={4} | autoLoad={5} | backupIsPending={6} | payload={7}",
                caller,
                this.state,
                this.reprogrammingInProgress,
                this.programBootCodeInProgress,
                this.notPollingPort,
                this.autoLoad,
                BitConverter.ToString(rPEA.BytesToSend)
            );

            this.onSend(rPEA);
            _lastResetSentUtc = DateTime.UtcNow;
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
            this.startFPGAProgramming();
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


        #endregion
        private DateTime lastAckUtc = DateTime.MinValue;
        private int currentBlockIndex = -1;
        private int currentPacketIndex = -1;
        private int currentBytesRemaining = -1;
        private string lastTxHex = string.Empty;
        private string lastRxHex = string.Empty;
        public void timerTimeout_Tick(object sender, EventArgs e)
        {
            logger.Info("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            var elapsedSinceLastAckMs = this.lastAckUtc == default
        ? -1
        : (DateTime.UtcNow - this.lastAckUtc).TotalMilliseconds;

            logger.Warn(
                "TIMEOUT SNAPSHOT: state={0}, timerIntervalMs={1}, elapsedSinceLastAckMs={2}, " +
                "currentBlockIndex={3}, currentPacketIndex={4}, bytesRemaining={5}, " +
                "lastTx={6}, lastRx={7}, reprogrammingInProgress={8}, programBootCodeInProgress={9}, " +
                "notPollingPort={10}, masterRev={11}, relayRev={12}, fpgaRev={13}",
                this.state,
                this.timerTimeout.Interval,
                elapsedSinceLastAckMs,
                this.currentBlockIndex,
                this.currentPacketIndex,
                this.currentBytesRemaining,
                this.lastTxHex,
                this.lastRxHex,
                this.ReprogrammingInProgress,
                this.ProgramBootCodeInProgress,
                this.NotPollingPort,
                this.MasterRevisionNumber,
                this.RelayRevisionNumber,
                this.FPGARevisionNumber
            );



            this.labelState.Text = "Time Out";
            this.programmingForm.CurrentTask = "Loading...........";
            this.timerTimeout.Stop();

            // this is the try again
            switch (this.state)
            {
                case RelayProgrammingStates.LoadingMasterBootLoader:
                case RelayProgrammingStates.WaitingForBootMaster:
                case RelayProgrammingStates.ReloadMasterBoot:
                case RelayProgrammingStates.CheckMasterBootCode:
                case RelayProgrammingStates.AutoLoadCheckBoot:
                case RelayProgrammingStates.ManualLoadCheckBoot:
                    logger.Warn("Bootloader timeout; restarting bootloader flow. state={0}", this.state);
                    this.state = RelayProgrammingStates.ReloadMasterBoot;
                    this.MasterBootLoaderStart();
                    return;
                case RelayProgrammingStates.LoadingMasterCode:
                case RelayProgrammingStates.LoadingMasterData:
                    logger.Warn("Master payload timeout; resending current master data. state={0}", this.state);
                    // resend the current master block / transfer packet
                    this.startMasterProgramming();
                    this.timerTimeout.Interval = 10000;
                    this.timerTimeout.Start();
                    return;
                
                case RelayProgrammingStates.LoadingRelayCode:
                case RelayProgrammingStates.LoadingRelayData:
                    logger.Warn("Relay payload timeout; resending current relay data. state={0}", this.state);
                    this.startRelayProgramming();
                    this.timerTimeout.Interval = 10000;
                    this.timerTimeout.Start();
                    return;

                case RelayProgrammingStates.LoadingFPGACode:
                    logger.Warn("FPGA payload timeout; resending current FPGA data. state={0}", this.state);
                    this.startFPGAProgramming();  // or your FPGA resend method
                    this.timerTimeout.Interval = 10000;
                    this.timerTimeout.Start();
                    return;
                default:
                    return;
            }
        }

        /*
        private void startManualBootCodeLoad()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            // Manual flow must never trigger auto boot-first continuation.
            this.bootFirstAutoContinuation = false;

            this.programBootCodeInProgress = true;
            this.timerTimeout.Stop();
            Thread.Sleep(3000);

            programMasterBootFileSelect = false;
            this.MasterBootLoaderStart();
        }
        */
        private void buttonStartAutoLoad_Click(object sender, EventArgs e)
        {
            this.InitializeAutoload();
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
            //this.InitialAutoLoadFiles();
        }
        
        private void buttonLoadNewest_Click(object sender, EventArgs e)
        {
            DialogResult dR = new CustomYesNoDialog("GE or WH Select", "Is this a GE or WH style relay?", "GE", "WH").ShowDialog();

            if (dR == DialogResult.Yes)
                internalGESetter = true;
            else
                internalGESetter = false;

            this.programmingForm.ClearAllChecks();
        }
        
        private void buttonFixBootLoader_Click(object sender, EventArgs e)
        {
            this.startRelayBootLoaderProgramming();
        }

        public void AllParametersReceived()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (this.state == RelayProgrammingStates.WaitForAllData)
            {
                logger.Trace("ParamsReceived-WaitingForAllData");
                this.state = RelayProgrammingStates.Finalized;
                this.SendTransmitterSettings();
                this.sendNonTransmitterSettings();
                this.finalizeReprogram();
            }
            else if (this.state == RelayProgrammingStates.ReprogramSuccess)
            {
                logger.Info("AllParametersReceived: ReprogramSuccess -> Finalized");
                this.state = RelayProgrammingStates.Finalized;
                this.finalizeReprogram();
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
            this.bootFirstAutoContinuation = false;
            this.programBootCodeInProgress = true;
            this.MasterBootLoaderStart();
        }
    }

   

    public class BackupBeforeProgrammingEventArgs : EventArgs
    {
        public string Caller { get; }

        public BackupBeforeProgrammingEventArgs(string caller)
        {
            Caller = caller ?? "unknown";
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

        /*
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
        */

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
        Idle, // quiescent/no active programming flow

        WaitingForBootMaster, // waiting for master CPU boot before sending master transfer packet (BootReceived + PrepForBoot master bucket)
        WaitingForBootRelay,  // waiting for relay CPU boot before relay transfer packet (BootReceived + PrepForBoot relay bucket)
        WaitingForBootFPGA,   // waiting for FPGA boot/reset readiness before FPGA transfer (BootReceived + PrepForBoot FPGA bucket)
        WaitingForMaster,   // waiting for Master boot/reset readiness before master transfer (BootReceived + PrepForBoot master bucket)

        LoadingMasterCode, // actively sending master code blocks (BootReceived retry path + PrepForBoot master bucket)
        LoadingMasterData, // actively sending master data blocks (PrepForBoot master bucket)

        LoadingRelayCode, // actively sending relay code blocks (BootReceived retry path + PrepForBoot relay bucket)
        LoadingRelayData, // actively sending relay data blocks (PrepForBoot relay bucket)

        LoadingFPGACode, // actively sending FPGA blocks (BootReceived + PrepForBoot FPGA bucket)

        ReprogramSuccess, // transfer stages complete; finalization/requestAll phase
        Finalized, // final parameter/writeback complete; ready for finalizeReprogram cleanup

        ClearingBootLoader, // relay bootloader clear/transition state
        RequestAll, // orchestration/request-all state marker
        WaitForAllData, // waiting for all parameter data before finalize

        LoadingRelayBootLoader, // relay bootloader payload transfer
        LoadingMasterBootLoader, // master bootloader payload transfer (#U/YES + blocks)

        ManualLoadCheckBoot, // post-boot reset readback gate for manual flow; BootReceived continuation decision
        AutoLoadCheckBoot, // post-boot reset readback gate for autoload flow; BootReceived continuation decision

        CheckMasterBootCode, // explicit boot revision decision state (BootReceived routes to CheckProperMasterBootCode)
        ReloadMasterBoot // timeout/retry trigger state to relaunch boot upload
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