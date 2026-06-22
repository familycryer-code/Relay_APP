using NLog;
using SharedResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

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

        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public bool ActiveRelay { get; set; }
        // These need to be updated when new files are used
#if DEBUG && !DG288_TESTFIXTURE_GUI
        private static UInt32 _masterCodeRevisionNumber = 999999;
        private static UInt32 _relayCodeRevisionNumber = 99999999;
        private static UInt32 _fPGACodeRevisionNumber = 1212071;
        private static UInt32 _bootCodeRevisionNumber = 99999999;
#else
        private static UInt32 _masterCodeRevisionNumber = Convert.ToUInt32(Properties.Resources.MasterRevision);
        private static UInt32 _relayCodeRevisionNumber = Convert.ToUInt32(Properties.Resources.RelayRevision);
        private static UInt32 _fPGACodeRevisionNumber = Convert.ToUInt32(Properties.Resources.FPGARevision);
        private static UInt32 _bootCodeRevisionNumber = Convert.ToUInt32(Properties.Resources.BootRevision);
#endif
        private static UInt32 _safeService_MASTER_REVISION = 160621;
        private static UInt32 _rEV1_MASTER_REVISION = 100713;

        private uint tempBootAddress = 0;
        private bool programBootCodeOnly = false;
        private bool programBootCodeStart = false;
        private bool programBootCodeInProgress = false;
        private UInt32 masterBootRevisionNumberReceived = 0;
        private bool revTooLowErrorAlreadyShown = false;
        private bool dontShowRelayUpgradeMessage = false;
        private bool masterBootRevisionSet = false;
        private bool askToUgradeShown = false;
        private bool reprogramBootCodeAuto = false;
        private string bootStartUpChar = "0";
        private bool wrongBootCodeLoaded = false;
        private bool reloadBootWithPrompt = false;
        private bool programMasterBootFileSelect = false;
        private string masterBootStringReceived = "0";
        private bool autoLoad = false;
        private string masterRevisionString = "";
        private bool notPollingPort = false;
        private bool wrongRelayTypeAutoLoad = false;
        private DialogResult upgradeAutoDR = DialogResult.No;
        private bool reprogrammingInProgress = false;

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
                if (ManualUpdate.usingManualMode == true)
                {
                    this.programmingForm.ClearAllChecks();
                }
                this.programBootCodeStart = value;
                if (programBootCodeStart == true)
                {
                    if (programBootCodeInProgress == false)
                        ProgramBootCode();
                    ProgramBootCodeStart = false;
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
            MasterBootLoaderStart();
        }

        private bool forceUpdateOnce = false;
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

                if (this.serialNumber >= 25000 ^ this.gERelay) //25k and up are GE Serial Numbers
                    this.gERelaySerialMatch = false;
                else
                    this.gERelaySerialMatch = true;
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

                if (this.gERelay ^ this.serialNumber >= 25000)
                    this.gERelaySerialMatch = false;
                else
                    this.gERelaySerialMatch = true;
            }
        }
        public bool DNPRelay
        {
            get { return this.dNPRelay; }
            set
            {
#if ATLANTA || ONCOR
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
                /* 
                    // 012345 is the value loaded in the boot loader
                     if (value == 012345 || value == 121116)
                     this.loadMasterFirst = true;
                */
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
                    if (value != this.remoteMasterRevisionNumber)
                    {
                        MessageBox.Show("Please Contact DIGITALGRID, INC. and ship relay back to factor for upgrade", "Relay Upgrade");
                        this.firstCheckForUpdate = false;
                        this.remoteMasterRevisionNumber = value;
                    }
                }

                this.remoteMasterRevisionNumber = value;

                // Check to make sure that it only checks while idle so that we don't accident reset it during reloads
                if (this.State == RelayProgrammingStates.Idle)
                {
                    setWrongRelayTypeAutoLoad();
#if DNP
                    if ((this.remoteMasterRevisionNumber < _masterCodeRevisionNumber) || wrongRelayTypeAutoLoad)
                        this.reprogramMaster = true;
                    else
                        this.reprogramMaster = false;
#else
    #if !BOSTON
                        if ((this.remoteMasterRevisionNumber < _masterCodeRevisionNumber) || wrongRelayTypeAutoLoad)
                            this.reprogramMaster = true;
                        else
                            this.reprogramMaster = false;
    #elif BOSTON
                        if(this.remoteMasterRevisionNumber < _masterCodeRevisionNumber) 
                            this.reprogramMaster = true;
                        else
                            this.reprogramMaster = false;
    #endif

#endif
                }

                // This section handles rebooting the relay to get to the next loading section.

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
                // Check manual reload here because it should reload regardless of the relative age
                if (this.remoteRelayRevisionNumber < _relayCodeRevisionNumber || manualReload)
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

                if (manualReload || (this.remoteFPGARevisionNumber < _fPGACodeRevisionNumber && this.TransmitterEnabled))
                    this.reprogramFPGA = true;
                else
                    this.reprogramFPGA = false;

            }
        }


        private bool reprogramMaster = false;
        private bool reprogramRelay = false;
        private bool reprogramFPGA = false;
        // initiaLoad is required because loading the relay from the boot code requires loading master first.  Once loaded, it is safer to load relay code first.
        private bool loadMasterFirst = false;
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
                CustomerLoadFiles regular = new CustomerLoadFiles(Customers.DIGITALGRID);
                regular.FPGAFile.DataBytes = RelayControlLibrary.Properties.Resources.FPGAdata;
                regular.MasterFileGE = RelayControlLibrary.Properties.Resources.MasterProcessor;
                regular.MasterFileGEDNP = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_GE;
                regular.MasterFileWH = RelayControlLibrary.Properties.Resources.MasterProcessor;
                regular.MasterFileWHDNP = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP;
                regular.MasterFileAtlantaDNPGE = RelayControlLibrary.Properties.Resources.MasterProcessor_Atlanta_DNP_GE;
                regular.MasterFileAtlantaDNPWH = RelayControlLibrary.Properties.Resources.MasterProcessor_Atlanta_DNP;
                regular.MasterFileDNPPLC = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_PLC;
                regular.RelayFileGE = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                regular.RelayFileWH = RelayControlLibrary.Properties.Resources.RelayProcessor;
                regular.RelayFileAtlantaWH = RelayControlLibrary.Properties.Resources.RelayProcessorAtlantaGE;
                regular.RelayFileAtlantaGE = RelayControlLibrary.Properties.Resources.RelayProcessorAtlanta;

                if (relayHBD.relayWithHBD == true)
                {
                    regular.MasterFileConEdHBD = RelayControlLibrary.Properties.Resources.MasterProcessor_ConEd_HBD;
                }
                else if (relayHBD.relayWithHBD == false) // SEC
                {
                    regular.MasterFileConEdSEC = RelayControlLibrary.Properties.Resources.MasterProcessor_ConEd_SEC;
                }

                CustomerLoadFiles workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.DIGITALGRIDDNP));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.DIGITALGRID));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.Dominion));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.Memphis));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.NonConEd));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.None));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.PEPCO));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.Atlanta));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.SMUD));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.DIGITALGRIDDNP));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.DNPwithPLC));
                copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile.MasterFileWHDNP = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_SMUD;
                workingLoadFile.MasterFileGEDNP = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_GE_SMUD;
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

        private void copyCustomerLoadFiles(CustomerLoadFiles destination, CustomerLoadFiles source)
        {
            destination.FPGAFile = source.FPGAFile;
            destination.MasterFileGE = source.MasterFileGE;
            destination.MasterFileGEDNP = source.MasterFileGEDNP;
            destination.MasterFileAtlantaDNPGE = source.MasterFileAtlantaDNPGE;
            destination.MasterFileAtlantaDNPWH = source.MasterFileAtlantaDNPWH;
            destination.MasterFileDNPPLC = source.MasterFileDNPPLC;
            destination.MasterFileWH = source.MasterFileWH;
            destination.MasterFileWHDNP = source.MasterFileWHDNP;
            destination.RelayFileGE = source.RelayFileGE;
            destination.RelayFileWH = source.RelayFileWH;
            destination.RelayFileAtlantaGE = source.RelayFileAtlantaGE;
            destination.RelayFileAtlantaWH = source.RelayFileAtlantaWH;
        }

        public void InitializeAutoload()
        {
            logger.Trace("InitializeAutoLoad");
            this.reprogramBootCodeAuto = true;
#if !BOSTON
            if ((askToUgradeShown == false && CompareMasterRevisionToGUI()) || setWrongRelayTypeAutoLoad())
            {
                if (!askToUgradeShown)
                {
                    this.showAutoLoadDialog();
                }
            }
#elif BOSTON
            if(askToUgradeShown == false && CompareMasterRevisionToGUI()) 
            {
                if (!askToUgradeShown)
                {
                    this.showAutoLoadDialog();
                }
            }
#endif

            if (upgradeAutoDR == DialogResult.Yes)
            {
                if (CompareMasterRevisionToGUI() && reprogramBootCodeAuto == true)
                {
                    this.autoLoad = true;

                    if (!this.MasterBootRevisionSet())
                        return;

                    if ((this.CheckForBootCodeUpdate() && masterBootRevisionSet == true) || (this.CheckForProperBootCodeAutoUpdate() && this.masterBootRevisionSet == true))
                    {
                        this.UpgradeBootCode();
                    }
                }
                else
                {
                    this.autoLoad = false;
                }
            }
            else if (!CompareMasterRevisionToGUI())
            {
                this.autoLoad = false;
            }

            if ((dontShowRelayUpgradeMessage == false && this.ProgramBootCodeInProgress == false && this.masterBootRevisionSet && this.upgradeAutoDR == DialogResult.Yes) || !askToUgradeShown)
                this.CheckForUpdate();

        }

        private bool MasterBootRevisionSet()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (CompareMasterRevisionToGUI())
            {
                if (masterBootRevisionSet == false)
                {
                    this.state = RelayProgrammingStates.AutoLoadCheckBoot;
                    this.sendReset();
                    Thread.Sleep(1000);
                    return false;
                }
                else
                    return true;
            }
            else
                return true;
        }

        private void showAutoLoadDialog()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            // MessageBox.Show("wrongRelayTypeAutoLoad : " + wrongRelayTypeAutoLoad); // Only for testing - to be removed
            if (!this.wrongRelayTypeAutoLoad)
                this.setWrongRelayTypeAutoLoad();

#if ENMAX && !DEBUG
            this.checkSafeServiceMaster();
#endif
            if (ManualUpdate.usingManualMode == false)
            {
                this.upgradeAutoDR = showAutoLoadUpdateMessage();
            }
#if !DEBUG
            if (upgradeAutoDR == DialogResult.Yes && notPollingPort)
                this.upgradeAutoDR = checkDNPPLCMessage(upgradeAutoDR);
#endif
            if (!this.dontReloadFromResource && upgradeAutoDR == DialogResult.Yes)
                this.upgradeAutoDR = MessageBox.Show("Please confirm update request.\r\nRelay update can take up to 5 minutes to complete.", "Confirm Update Request", MessageBoxButtons.YesNo);

            this.askToUgradeShown = true;
        }

        private void ForceUpgradeCheck()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (this.remoteMasterRevisionNumber <= _rEV1_MASTER_REVISION && this.revTooLowErrorAlreadyShown == false)
            {
                MessageBox.Show("Relay Upgrade", "To upgrade relay, please contact DIGITALGRID, INC. and return relay to factory.");
                this.revTooLowErrorAlreadyShown = true;
            }
            else if (this.remoteMasterRevisionNumber < _safeService_MASTER_REVISION)
            {
                this.forceRelayUpdate = true;
                this.forceUpdateReason = "Safe Service";
            }
            else
            {
                this.forceRelayUpdate = false;
            }
        }

        private void checkSafeServiceMaster()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (this.remoteMasterRevisionNumber <= _rEV1_MASTER_REVISION && this.revTooLowErrorAlreadyShown == false)
            {
                MessageBox.Show("Relay Upgrade", "To upgrade relay, please contact DIGITALGRID, INC. return relay to factory.");
                this.revTooLowErrorAlreadyShown = true;
            }
            else if (this.remoteMasterRevisionNumber < _safeService_MASTER_REVISION)
            {
                MessageBox.Show("The Relay Software is outdated and must be upgraded for the Safe Service Mode Indicator attachment to function properly!", "Relay Must be Upgraded for Safe Service Mode Indicator!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpgradeBootCode()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.dontShowRelayUpgradeMessage = true;

            DialogResult warningBootDR = new DialogResult();

            this.programmingForm.ClearAllChecks();

            warningBootDR = MessageBox.Show("Please do not remove the port, turn off the computer, power down the relay, let the computer sleep or click around the GUI during the upgrade process", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

            if (warningBootDR == DialogResult.OK)
            {
                this.dontShowRelayUpgradeMessage = false;
                Thread.Sleep(3000);
                this.ProgramBootCodeStart = true;
            }
        }

        private DialogResult showAutoLoadUpdateMessage()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            DialogResult dR,dR1;

            dR1 = new CustomYesNoDialog("GE or WH Select", "Is this a GE or WH style relay?", "GE", "WH").ShowDialog();
            if (dR1 == DialogResult.Yes)
                internalGESetter = true;
            else
                internalGESetter = false;

            dR = MessageBox.Show("Newer Firmware is available to update the Relay. It is necessary that the update be completed.\r\nClick Yes to begin update", "Relay Code Updater", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            return dR;
        }

        private DialogResult checkDNPPLCMessage(DialogResult dR)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (masterRevisionString.Contains("DNP"))
            {
#if !DNP
                //show that it is dnp relay on plc gui
                dR = MessageBox.Show("Warning: This is a PLC only program and has been connected to a DNP/PLC relay. It is recommended you use the proper program and that you do not downgrade to PLC only. Would you like to proceed?", "Different Type of Relay", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
#endif
            }
            else
            {
#if DNP
                //show that it is PLC relay on DNP PLC GUI
                dR = MessageBox.Show("Warning: This is a DNP/PLC program and has been connected to a PLC relay. It is recommended you use the proper program if you don't want to change the relay to a DNP/PLC relay. Would you like to proceed?", "Different Type of Relay", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
#endif
            }
            return dR;
        }

        private bool setWrongRelayTypeAutoLoad()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            // MessageBox.Show("masterRevisionString : " + masterRevisionString); // Only for testing - to be removed
            if (masterRevisionString != "")
            {
                if (masterRevisionString.Contains("DNP"))
                {
#if !DNP
                    this.wrongRelayTypeAutoLoad = true;
                    return wrongRelayTypeAutoLoad;
#endif
                }
                else
                {
#if DNP
                    wrongRelayTypeAutoLoad = true;
                    return wrongRelayTypeAutoLoad;
#endif
                }
            }

            return false;
        }

        public void InitialAutoLoadFiles()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            DialogResult dR;

            dR = MessageBox.Show("Do you want to attempt to reprogram the Relay?", "Initial Auto Reload", MessageBoxButtons.YesNo);

            if (dR != DialogResult.Yes)
                return;

            if (notPollingPort)
                dR = checkDNPPLCMessage(dR);

            if (dR == DialogResult.No)
                return;

            this.dontReloadFromResource = false;
            this.useDefaultSettings = true;

            switch (this.customer)
            {
                case Customers.Memphis:

                    internalGESetter = false;
                    break;
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
#if DEBUG
            dR = new CustomYesNoDialog("Select Communication Type", "Does this have DNP?", "Yes", "No").ShowDialog();
            if (dR == DialogResult.Yes)
            {
                this.DNPRelay = true;

                dR = new CustomYesNoDialog("Select Communication Type", "Does this use PLC?", "Yes", "No").ShowDialog();

                if (dR == DialogResult.Yes)
                {
                    this.reprogramFPGA = true;
                    this.TransmitterEnabled = true;
                }
                else
                    this.TransmitterEnabled = false;
            }
            else
            {
                this.DNPRelay = false;
                dR = new CustomYesNoDialog("Select Communication Type", "Does this use PLC?", "Yes", "No").ShowDialog();

                if (dR == DialogResult.Yes)
                {
                    this.reprogramFPGA = true;
                    this.TransmitterEnabled = true;
                }
                else
                {
                    this.reprogramFPGA = false;
                    this.TransmitterEnabled = false;
                }
            }


#endif
            if (this.notPollingPort)
            {
                this.CheckForProperBootCodeManualUpdate();
                if (masterBootRevisionSet == true)
                    this.startManualReloadWithBootCheck();
            }
            else
            {
                this.startManualReload();
            }

        }

        private void startManualReloadWithBootCheck()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.loadMasterFirst = true;
            this.masterCode.WithParameters = false;

            setManualReloadVars();
            this.CheckForProperBootCodeManualUpdate();

            this.programmingForm.ClearAllChecks();

            if (masterBootRevisionSet == true)
            {
                this.setProgrammingFiles();
                this.startProgramming();
            }
        }

        private void startManualReload()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.loadMasterFirst = true;
            this.masterCode.WithParameters = false;

            this.manualReload = true;
            setManualReloadVars();
            this.programmingForm.ClearAllChecks();

            this.setProgrammingFiles();
            this.startProgramming();
        }

        private void setManualReloadVars()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.autoLoad = true;
            this.reprogramMaster = true;
            this.reprogramRelay = true;
            this.askToUgradeShown = true;
        }

        private void checkDNP()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
#if DNP
            this.DNPRelay = true;
#else
            this.DNPRelay = false;
#endif
        }

        private DialogResult askIfDNPRelay()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            return new CustomYesNoDialog("Select Communication Type", "Does this have DNP?", "Yes", "No").ShowDialog();
        }

        private void setDNPRelay(DialogResult dR)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (dR == DialogResult.Yes)
            {
                this.DNPRelay = true;
                this.TransmitterEnabled = false;
            }
        }

        private void determineIfTransmitterRelay()
        {
            DialogResult dR;
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            switch (this.customer)
            {
                case Customers.Memphis:
                    this.TransmitterEnabled = false;
                    break;
                default:
                    dR = new CustomYesNoDialog("Select Communication Type", "Does this use PLC?", "Yes", "No").ShowDialog();

                    if (dR == DialogResult.Yes)
                    {
                        this.reprogramFPGA = true;
                        this.TransmitterEnabled = true;
                    }
                    else
                    {
                        this.reprogramFPGA = false;
                        this.TransmitterEnabled = false;
                    }
                    break;
            }
        }

        private void setProgrammingFiles()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (this.dontReloadFromResource || RelayProgrammingStates.Idle != this.state)
                return;

            CustomerLoadFiles cLF = this.customersFiles.Find(x => x.Customer.Equals(this.customer));

            checkDNP();

#if (DOMINION || DEBUG || NU || BOSTON || SEATTLE || PSEG || BGE) && !DNP
            //  MessageBox.Show("Comes here. Take MasterProcessor.S as the firmware build"); // Only for testing - to be removed
            this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor;
            this.textBoxMasterFileName.Text = "Master Relay From Resource";

            if (this.GERelay)
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource " + this.customer.ToString();
            }
            else
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();
            }
#endif

#if (PSEG && DNP)
            this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_ConEd_SEC;
            this.textBoxMasterFileName.Text = "Master Relay From Resource";

            if (this.GERelay)
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource " + this.customer.ToString();
            }
            else
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();
            }
#endif
#if (ENMAX || CONED || TORONTO_HYDRO) && DNP
            if (GERelay)
            {
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_PLC_GE;
                this.textBoxMasterFileName.Text = "Master Relay DNP with PLC GE Resource";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource " + this.customer.ToString();
            }
            else
            {
                /*    this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_PLC;
                   this.textBoxMasterFileName.Text = "Master Relay DNP with PLC Resource";

                   this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                   this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();
               */

                if (relayHBD.relayWithHBD == true)
                {
                    this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_ConEd_HBD;
                }
                else if (relayHBD.relayWithHBD == false) // SEC
                {
                    this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_ConEd_SEC;
                }
#if TORONTO_HYDRO
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_ConEd_HBD;
#elif ENMAX
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_PLC;
#endif

                this.textBoxMasterFileName.Text = "Master Relay DNP with PLC Resource";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();

            }

            if (this.transmitterEnabled)
            {
                this.parseFPGAFile(this.fPGACode);
                this.textBoxFPGAFile.Text = "FPGA Code From Resource";
            }
#endif

#if CHICAGO
            this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessorChicago;
            this.textBoxMasterFileName.Text = "Master Relay Chicago";

            if (GERelay)
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource " + this.customer.ToString();
            }
            else
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();
            }

            if (this.transmitterEnabled)
            {
                this.parseFPGAFile(this.fPGACode);
                this.textBoxFPGAFile.Text = "FPGA Code From Resource";
            }

            this.parseSFile(this.masterCode);
            this.parseSFile(this.relayCode);

            logger.Trace("MP: " + this.textBoxMasterFileName.Text + " RP: " + this.textBoxRelayFileName.Text + " FPGA: " + this.textBoxFPGAFile.Text);
            return;
#endif

#if LONDONH
            this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessorLondonH;
            this.textBoxMasterFileName.Text = "Master Relay LondonH";

            if (GERelay)
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource " + this.customer.ToString();
            }
            else
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();
            }

            if (this.transmitterEnabled)
            {
                this.parseFPGAFile(this.fPGACode);
                this.textBoxFPGAFile.Text = "FPGA Code From Resource";
            }

            this.parseSFile(this.masterCode);
            this.parseSFile(this.relayCode);

            logger.Trace("MP: " + this.textBoxMasterFileName.Text + " RP: " + this.textBoxRelayFileName.Text + " FPGA: " + this.textBoxFPGAFile.Text);
            return;
#endif

#if TAUNTON
            this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessorTaunton;
            this.textBoxMasterFileName.Text = "Master Relay Taunton";

            if (GERelay)
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource " + this.customer.ToString();
            }
            else
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();
            }

            if (this.transmitterEnabled)
            {
                this.parseFPGAFile(this.fPGACode);
                this.textBoxFPGAFile.Text = "FPGA Code From Resource";
            }

            this.parseSFile(this.masterCode);
            this.parseSFile(this.relayCode);

            logger.Trace("MP: " + this.textBoxMasterFileName.Text + " RP: " + this.textBoxRelayFileName.Text + " FPGA: " + this.textBoxFPGAFile.Text);
            return;
#endif

#if MADISON
            this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessorMadison;
            this.textBoxMasterFileName.Text = "Master Relay Madison";

            if (GERelay)
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource " + this.customer.ToString();
            }
            else
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();
            }

            if (this.transmitterEnabled)
            {
                this.parseFPGAFile(this.fPGACode);
                this.textBoxFPGAFile.Text = "FPGA Code From Resource";
            }

            this.parseSFile(this.masterCode);
            this.parseSFile(this.relayCode);

            logger.Trace("MP: " + this.textBoxMasterFileName.Text + " RP: " + this.textBoxRelayFileName.Text + " FPGA: " + this.textBoxFPGAFile.Text);
            return;
#endif

#if ATLANTA && DNP
            

            if (GERelay)
            {
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_Atlanta_DNP_GE;
                this.textBoxMasterFileName.Text = "Master Atlanta Relay DNP GE";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorAtlantaGE;
                this.textBoxRelayFileName.Text = "GE Atlanta Relay From Resource " + this.customer.ToString();
            }
            else
            {
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_Atlanta_DNP;
                this.textBoxMasterFileName.Text = "Master Atlanta Relay DNP";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorAtlanta;
                this.textBoxRelayFileName.Text = "WH Atlanta Relay From Resource " + this.customer.ToString();
            }

            if (this.transmitterEnabled)
            {
                this.parseFPGAFile(this.fPGACode);
                this.textBoxFPGAFile.Text = "FPGA Code From Resource";
            }

            this.parseSFile(this.masterCode);
            this.parseSFile(this.relayCode);

            logger.Trace("MP: " + this.textBoxMasterFileName.Text + " RP: " + this.textBoxRelayFileName.Text + " FPGA: " + this.textBoxFPGAFile.Text);
            return;
#endif

#if ONCOR && DNP
            if (GERelay)
            {
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_Oncor_GE;
                this.textBoxMasterFileName.Text = "Master Atlanta Relay DNP GE";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                this.textBoxRelayFileName.Text = "GE Atlanta Relay From Resource " + this.customer.ToString();
            }
            else
            {
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_Oncor;
                this.textBoxMasterFileName.Text = "Master Atlanta Relay DNP";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                this.textBoxRelayFileName.Text = "WH Atlanta Relay From Resource " + this.customer.ToString();
            }

            if (this.transmitterEnabled)
            {
                this.parseFPGAFile(this.fPGACode);
                this.textBoxFPGAFile.Text = "FPGA Code From Resource";
            }

            this.parseSFile(this.masterCode);
            this.parseSFile(this.relayCode);

            logger.Trace("MP: " + this.textBoxMasterFileName.Text + " RP: " + this.textBoxRelayFileName.Text + " FPGA: " + this.textBoxFPGAFile.Text);
            return;
#endif

#if MEMPHIS
            if (GERelay)
            {
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_GE;
                this.textBoxMasterFileName.Text = "Master Relay GE with DNP From Resource ";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource" + this.customer.ToString();
            }
            else
            {
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessorMemphis;
                this.textBoxMasterFileName.Text = "Master Relay WH with DNP From Resource";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();
            }
#endif

#if SMUD
            if (GERelay)
            {
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessorSMUDGE;
                this.textBoxMasterFileName.Text = "Master Relay GE with DNP From Resource ";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorSMUDGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource" + this.customer.ToString();
            }
            else
            {
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessorSMUD;
                this.textBoxMasterFileName.Text = "Master Relay WH with DNP From Resource";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorSMUD;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();
            }
#endif

#if (DNP && (!ENMAX && !PSEG) && !CONED && !TORONTO_HYDRO)
            if (GERelay)
            {
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_GE;
                this.textBoxMasterFileName.Text = "Master Relay GE with DNP From Resource ";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource" + this.customer.ToString();
            }
            else
            {
                this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP;
                this.textBoxMasterFileName.Text = "Master Relay WH with DNP From Resource";

                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();
            }
#endif

#if ENMAX && !DNP
            this.masterCode.FileString = RelayControlLibrary.Properties.Resources.MasterProcessorEnmaxPLC;
            this.textBoxMasterFileName.Text = "Master Relay LondonH";

            if (GERelay)
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                this.textBoxRelayFileName.Text = "GE Relay From Resource " + this.customer.ToString();
            }
            else
            {
                this.relayCode.FileString = RelayControlLibrary.Properties.Resources.RelayProcessor;
                this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();
            }


            this.parseFPGAFile(this.fPGACode);
            this.textBoxFPGAFile.Text = "FPGA Code From Resource";


            this.parseSFile(this.masterCode);
            this.parseSFile(this.relayCode);

            logger.Trace("MP: " + this.textBoxMasterFileName.Text + " RP: " + this.textBoxRelayFileName.Text + " FPGA: " + this.textBoxFPGAFile.Text);
            return;
#endif

            if (this.transmitterEnabled)
            {
                this.parseFPGAFile(this.fPGACode);
                this.textBoxFPGAFile.Text = "FPGA Code From Resource";
            }

            this.parseSFile(this.masterCode);
            this.parseSFile(this.relayCode);

            logger.Trace("MP: " + this.textBoxMasterFileName.Text + " RP: " + this.textBoxRelayFileName.Text + " FPGA: " + this.textBoxFPGAFile.Text);
        }


        formProgrammingProgess programmingForm = new formProgrammingProgess();

        private bool firstCheckForUpdate = true;

        public void CheckForUpdate()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (forceRelayUpdate == false)
            {
                if (!this.firstCheckForUpdate)
                    return;

                this.firstCheckForUpdate = false;
                if (this.reprogramFPGA || this.reprogramMaster || this.reprogramRelay)
                {
#if !DNP || ENMAX
                    this.transmitterEnabled = true;
#endif

                    if (!this.gERelaySerialMatch && !this.serialNumberError)
                        this.askIfGERelay();
                    this.setProgrammingFiles();

                    this.startAutoLoad();
                }
                else
                    logger.Trace("No Updated Needed");
            }
            else
            {
                forceRelayToUpdate();
            }
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
#if !BOSTON
#if DNP
            if ((remoteMasterRevisionNumber < _masterCodeRevisionNumber) || wrongRelayTypeAutoLoad)
#else
            if ((remoteMasterRevisionNumber < _masterCodeRevisionNumber) || wrongRelayTypeAutoLoad)
#endif
            {
                return true;
            }
            else
                return false;
#elif BOSTON

            if (remoteMasterRevisionNumber < _masterCodeRevisionNumber) 
            {
                return true;
            }
            else
                return false;
#endif

        }

        private void forceRelayToUpdate()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (forceUpdateOnce == false)
            {
                forceUpdateOnce = true;
                if (this.forceUpdateReason == "Generic")
                {
                    MessageBox.Show("The Relay Software is outdated and must be upgraded for the relay to function properly!", "Relay Must be Upgraded!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (this.forceUpdateReason == "Safe Service")
                {
                    MessageBox.Show("The Relay Software is outdated and must be upgraded for the Safe Service Mode Indicator attachment to function properly!", "Relay Must be Upgraded for Safe Service Mode Indicator!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (!this.gERelaySerialMatch && !this.serialNumberError)
                    this.askIfGERelay();
                this.setProgrammingFiles();
                this.startAutoLoad();
            }

        }

        private void askIfGERelay()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (this.loadMasterFirst)
                return;

            DialogResult dR = dR = new CustomYesNoDialog("Serial Number and Relay Type Mismatch", "Is this a GE or WH style relay?", "GE", "WH").ShowDialog();

            if (dR == DialogResult.Yes)
            {
                if (this.serialNumber < 25000 || this.serialNumber > 32767 || this.serialNumber == 0) //25k and up are GE serial Numbers
                {
                    MessageBox.Show("Bad Serial Number!", "Problem with Serial Number. \r\nPlease Contact DIGITALGRID, INC.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.serialNumberError = true;
                }

                internalGESetter = true;
                this.addGERelayToTransmitterPacket(true);
            }
            else
            {
                if (this.serialNumber >= 25000 || this.serialNumber == 0)
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


        private void startAutoLoad()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            DialogResult dR;
            if (this.serialNumberError)
                return;

            if (forceRelayUpdate == false && askToUgradeShown == false && programmingForm.MasterBootComplete == false)
            {
                askToUgradeShown = true;
                dR = showAutoLoadUpdateMessage();
#if !DEBUG
                if (dR == DialogResult.Yes)
                    dR = checkDNPPLCMessage(dR);
#endif
            }
            else
            {
                dR = DialogResult.Yes;
            }

            if (dR == DialogResult.Yes)
            {
                // If we aren't loading from resource, don't bother asking this question

                if (forceRelayUpdate == false && programmingForm.MasterBootComplete == false && this.reprogramMaster == false)
                {
                    if (!this.dontReloadFromResource)
                        dR = MessageBox.Show("Are You Sure?  This will take a while.", "Are You Sure?", MessageBoxButtons.YesNo);
                }
                else
                {
                    dR = DialogResult.Yes;
                }


                if (dR == DialogResult.Yes)
                {
                    RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

                    rPEA.Command = RelayProgrammingSendCommands.SaveSettings;

                    this.onSend(rPEA);

                    // If we aren't loading from resource, don't bother warning
                    if (!this.dontReloadFromResource && programmingForm.MasterBootComplete == false)
                        MessageBox.Show("Please do not remove the port, turn off the computer, power down the relay, let the computer sleep or click around the GUI during the upgrade process");

                    logger.Trace("User Verified Programming Start");
                    Thread.Sleep(500);
                    this.autoLoad = true;

                    if (!programmingForm.MasterBootComplete)
                        this.programmingForm.ClearAllChecks();
                    this.startProgramming();
                    if (!this.programmingForm.Visible)
                        this.programmingForm.ShowDialog();
                }
            }
            else
            {
                this.autoLoad = false;
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
            if (rPEA.Command == RelayProgrammingSendCommands.RestartProgram)
                Thread.Sleep(500); // Put in so I don't go too fast for the processor
            else
                Thread.Sleep(75); // Put in so I don't go too fast for the processor
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
                        logger.Trace("AckU, ");
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.State = RelayProgrammingStates.LoadingMasterBootLoader;
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
            this.labelState.Text = "Boot Received";
            if (this.autoLoad && !this.programmingForm.Visible && this.state != RelayProgrammingStates.AutoLoadCheckBoot)
                this.programmingForm.ShowDialog();

            logger.Trace("Boot Received - " + this.state.ToString());

            masterBootStringReceived = bootReceived.Substring(5);
            bootStartUpChar = bootReceived.Substring(3, 1);

            if (IsDigitsOnly(masterBootStringReceived))
            {
                masterBootRevisionNumberReceived = UInt32.Parse(masterBootStringReceived);
            }

            masterBootRevisionSet = true;

            if (manualReload && !notPollingPort && !programBootCodeInProgress)
                this.CheckForProperBootCodeManualUpdate();

            switch (state)
            {
                case RelayProgrammingStates.ManualPortSelectionWaitingForBoot:
                    this.startProgramming();
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
                    this.programmingForm.CurrentTask = "Loading Relay Code d";
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
                    CheckProperMasterBootCode();
                    break;
                case RelayProgrammingStates.AutoLoadCheckBoot:
                    logger.Trace("AutoLoadCheckBoot");
                    this.state = RelayProgrammingStates.Idle;
                    InitializeAutoload();
                    break;
                case RelayProgrammingStates.ManualLoadCheckBoot:
                    logger.Trace("manualLoadCheckBoot");
                    this.state = RelayProgrammingStates.Idle;
                    startManualReloadWithBootCheck();
                    break;
                case RelayProgrammingStates.ReloadMasterBoot:
                    this.timerTimeout.Stop();
                    Thread.Sleep(3000);
                    programBootCodeInProgress = false;
                    this.ProgramBootCodeStart = true;
                    break;
                default:
                    break;
            }
        }


        bool IsDigitsOnly(string str)
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void masterFinished()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            logger.Trace("Master Finished Loading");
            if (autoLoad)
            {
                this.State = RelayProgrammingStates.WaitingForBootRelay;
            }
            else
                this.enableButtons(true);
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

        private void MasterBootLoaderStart()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
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

                logger.Trace("BL, ");

                this.onSend(rPEA);

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
                this.onSend(rPEA);
            }
        }

        private void sendMasterBootCode()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RawData;

            // const int BOOTLOADER_PACKET_SIZE = 1024;
            // const int BOOTLOADER_PACKET_COUNT = 24;
            // const int BOOTLOADER_TOTAL_SIZE = BOOTLOADER_PACKET_SIZE * BOOTLOADER_PACKET_COUNT;

            this.failCount = 0;

            // while (this.masterCode.CodeBytes.Count < BOOTLOADER_TOTAL_SIZE)
            //    this.masterCode.CodeBytes.Add(0xFF);

            if (this.State == RelayProgrammingStates.LoadingMasterBootLoader)
            {
                this.programmingForm.CurrentTask = "Loading Master Boot";
                Int32 temp = Convert.ToInt32(this.labelCodeCount.Text);

                try
                {
                    rPEA.BytesToSend = new byte[1024];

                    if (this.masterCode.CodeBytes.Count >= 0)
                    {
                        for (int i = 0; i < 1024; i++)
                        {
                            rPEA.BytesToSend[i] = this.masterCode.CodeBytes[0];
                            this.masterCode.CodeBytes.RemoveAt(0);
                        }
                    }

                    if (this.State == RelayProgrammingStates.LoadingMasterBootLoader)
                        this.onSend(rPEA);

                    if (temp == programmingForm.Maximum)
                    {
                        this.doneLoadingMasterBootLoader();
                    }
                    else
                    {
                        this.programmingForm.ProgressValue = temp;
                        temp++;
                        this.labelCodeCount.Text = temp.ToString();
                        logger.Trace("BL, ");
                    }
                }
                catch (Exception ex)
                {
                    this.errorHandler("Error Sending Next Master Boot Data", ex);
                }
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

            if (!this.loadMasterFirst)
            {
                if (this.autoLoad)
                {
                    this.programmingForm.RelayDataComplete = true;
                    if (this.reprogramMaster)
                    {
                        this.parseSFile(this.masterCode);
                        this.State = RelayProgrammingStates.WaitingForBootMaster;
                        this.programmingForm.Maximum = this.masterCode.NumberOfCodeBlocks * 2;
                        this.programmingForm.CurrentTask = "Loading Master Code";
                        logger.Trace("Loading Master Code");
                        this.timerTimeout.Start();
                    }
                    else if (this.reprogramFPGA)
                    {
                        this.programmingForm.MasterDataComplete = true;
                        this.programmingForm.MasterCodeComplete = true;
                        this.parseFPGAFile(this.fPGACode);
                        this.State = RelayProgrammingStates.WaitingForBootFPGA;
                        this.programmingForm.Maximum = 96;
                        this.programmingForm.CurrentTask = "Loading FPGA";
                        logger.Trace("Loading FPGA");
                        this.timerTimeout.Start();
                    }
                    else
                        this.allReprogramingDone();
                }
                else
                    this.allReprogramingDone();
            }
            else
            {
                if (true)//this.autoLoad)
                {
                    this.programmingForm.RelayDataComplete = true;
                    if (this.reprogramFPGA)
                    {
                        this.parseFPGAFile(this.fPGACode);
                        this.State = RelayProgrammingStates.WaitingForBootFPGA;
                        this.programmingForm.Maximum = 96;
                        this.programmingForm.CurrentTask = "Loading FPGA";
                        logger.Trace("Loading FPGA");
                        this.timerTimeout.Start();
                    }
                    else if (this.programMasterBootFileSelect)
                        startManualBootCodeLoad();
                    else
                        this.allReprogramingDone();
                }
                else
                    this.allReprogramingDone();
            }
        }

        private void doneLoadingMaster()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.timerTimeout.Stop();
            this.programmingForm.MasterDataComplete = true;

            if (!this.loadMasterFirst)
            {
                if (this.autoLoad)
                {
                    if (this.reprogramFPGA)
                    {
                        this.parseFPGAFile(this.fPGACode); //todo
                        this.programmingForm.RelayCodeComplete = true;
                        this.programmingForm.RelayDataComplete = true;
                        this.programmingForm.CurrentTask = "Loading FPGA";
                        logger.Trace("Loading FPGA");
                        this.programmingForm.Maximum = 96;
                        this.State = RelayProgrammingStates.WaitingForBootFPGA;
                        this.sendReset();
                        Thread.Sleep(1000);
                        this.timerTimeout.Start();
                    }
                    else
                    {
                        this.programmingForm.FPGAComplete = true;
                        logger.Trace("doneloadingmaster");
                        this.State = RelayProgrammingStates.Idle;

                        this.allReprogramingDone();
                    }
                }
                else
                {
                    this.allReprogramingDone();
                }
            }
            else
            {
                if (this.autoLoad)
                {
                    if (this.reprogramRelay)
                    {
                        this.parseSFile(this.relayCode);
                        this.programmingForm.CurrentTask = "Loading Relay Code g";
                        logger.Trace("Loading Relay Code g");
                        this.programmingForm.Maximum = this.relayCode.NumberOfCodeBlocks * 2;
                        this.State = RelayProgrammingStates.WaitingForBootRelay;
                        this.timerTimeout.Start();
                    }
                    else if (this.reprogramFPGA)
                    {
                        this.parseFPGAFile(this.fPGACode);
                        this.programmingForm.RelayCodeComplete = true;
                        this.programmingForm.RelayDataComplete = true;
                        this.programmingForm.CurrentTask = "Loading FPGA";
                        logger.Trace("Loading FPGA");
                        this.programmingForm.Maximum = 96;
                        this.State = RelayProgrammingStates.WaitingForBootFPGA;
                        this.timerTimeout.Start();
                    }
                    else if (this.programMasterBootFileSelect)
                        startManualBootCodeLoad();
                    else
                    {
                        this.programmingForm.FPGAComplete = true;
                        logger.Trace("DoneLoadingMaster 2");
                        this.State = RelayProgrammingStates.Idle;

                        this.allReprogramingDone();
                    }
                }
                else
                {
                    this.allReprogramingDone();
                }
            }
        }

        private void allReprogramingDone()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            logger.Trace(String.Format("autoload: {0}", autoLoad));

            this.reprogrammingInProgress = false;
            if (this.autoLoad)
            {
                this.State = RelayProgrammingStates.ReprogramSuccess;
                this.timerTimeout.Stop();
                this.requestAll();
            }
            else
            {
                logger.Trace("All Loading Done, idle");
                this.State = RelayProgrammingStates.Idle;
                this.askToUgradeShown = true;
                this.finalizeReprogram();
            }
        }

        private void requestAll()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayProgrammingSendCommands.RequestAll;
            this.onSend(rPEA);
        }

        private void finalizeReprogram()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.enableButtons(true);
            this.programmingForm.Hide();

            this.autoLoad = false;
            this.loadMasterFirst = false;
            this.firstCheckForUpdate = false;

            MessageBox.Show("Reprogram Completed Successfully", "Reprogramming Completed Successfully!");
            this.programmingForm.ClearAllChecks();
            rPEA.Command = RelayProgrammingSendCommands.RestartProgram;

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

        private void doneLoadingMasterBootLoader()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            this.State = RelayProgrammingStates.DoneLoadingMasterBootLoader;

            this.enableButtons(true);
            this.timerTimeout.Stop();
            logger.Trace("");
            logger.Trace("Done Loading Master Boot");
            this.programmingForm.MasterBootComplete = true;

            Thread.Sleep(3000); //must delay before sending any other commands on completion!

            programBootCodeInProgress = false;
            programmingForm.MasterBootComplete = true;

            if (programBootCodeOnly)
            {
                this.programmingForm.Hide();

                allReprogramingDone();

                this.restartProgram();
                this.requestAll();
            }


            if (!programBootCodeOnly)
            {
                firstCheckForUpdate = true;
                /* if((ManualUpdate.usingManualMode == true) && (this.remoteRelayRevisionNumber < _relayCodeRevisionNumber) )
                     reprogramRelay = false;
                 else
                     reprogramRelay = true;
                */
                reprogramRelay = true;
                CheckForUpdate();
            }

        }

        public void FinalizeReprogram()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (this.state == RelayProgrammingStates.Finalized)
            {
                this.programmingForm.Hide();
                MessageBox.Show("Reprogram Completed Successfully", "Reprogramming Completed Successfully!");
                this.programmingForm.ClearAllChecks();
                logger.Trace("Reprogam Completed Successfully");
                if (ManualUpdate.usingManualMode == true)
                {
                    ManualUpdate.usingManualMode = false;
                }
                logger.Trace("FinalizeReprogram");
                this.state = RelayProgrammingStates.Idle;
                this.autoLoad = false;
                this.loadMasterFirst = false;
                this.firstCheckForUpdate = false;
                dataB.oldDataBackup = true;
                restartProgram();
            }
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

        public void CheckProperMasterBootCode()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            bool properBootCode = false;

            bool checkBootDate = true;

            if (programBootCodeInProgress)
                return;

            if (!masterBootRevisionSet && !reloadBootWithPrompt)
            {
                if (manualReload && notPollingPort)
                {
                    this.state = RelayProgrammingStates.ManualLoadCheckBoot;
                    sendReset();
                }
                else if (autoLoad && !manualReload)
                {
                    this.state = RelayProgrammingStates.AutoLoadCheckBoot;
                    sendReset();
                }
                return;
            }

            wrongBootCodeLoaded = false;

            if (masterBootStringReceived == "BYPASS")
            {
                if (reloadBootWithPrompt == true)
                {
                    this.reloadBootWithPrompt = false;
                    wrongBootCodeShorcutMsg();
                }
                else
                {
                    this.wrongBootCodeLoaded = true;
                }
            }
            else
            {
                if (bootStartUpChar == "I" || bootStartUpChar == "D")
                {
#if !DNP
                    if (reloadBootWithPrompt == true)
                    {
                        if (!this.masterRevisionString.Contains("DNP"))
                        {
                            this.DNPRelay = false;
                            this.reloadBootWithPrompt = false;
                            wrongBootCodeShorcutMsg();
                        }
                        else
                        {
                            properBootCode = true;
                            checkBootDate = false;
                        }
                    }
                    else
                    {
                        this.wrongBootCodeLoaded = true;
                    }
#elif DNP
                    properBootCode = true;
#endif
                }
                else if (bootStartUpChar == "H" || bootStartUpChar == "C")
                {
#if DNP
                    if (reloadBootWithPrompt == true)
                    {
                        if (this.masterRevisionString.Contains("DNP"))
                        {
                            this.DNPRelay = true;
                            this.reloadBootWithPrompt = false;
                            wrongBootCodeShorcutMsg();
                        }
                        else
                        {
                            properBootCode = true;
                            checkBootDate = false;
                        }

                    }
                    else
                    {
                        this.wrongBootCodeLoaded = true;
                    }
#else
                    properBootCode = true;
#endif
                }


                if (programBootCodeInProgress)
                    return;

                if (properBootCode == true && reloadBootWithPrompt == true)
                {
                    this.wrongBootCodeLoaded = false;
                    this.reloadBootWithPrompt = false;
                    if (checkBootDate)
                        checkBootCodeforProperDate();
                    else
                    {
                        MessageBox.Show("Boot code correct", "Correct Boot code loaded");
                        Thread.Sleep(500);
                        restartProgram();
                    }
                }
                else if (reloadBootWithPrompt == false && manualReload == true)
                {
                    checkBootCodeforProperDate();
                }
                else
                {
                    reprogramRelay = true;
                }

                this.reloadBootWithPrompt = false;
            }
        }

        private void wrongBootCodeShorcutMsg()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            DialogResult dR;

            dR = new YesNoMessageBoxResized("Wrong Boot Code", "Wrong Boot Code Loaded. Would you like to fix the boot code?", "Yes", "No").ShowDialog();

            if (dR == DialogResult.Yes)
            {
                this.programBootCodeOnly = true;
                dR = MessageBox.Show("Please do not remove the port, turn off the computer, power down the relay, let the computer sleep or click around the GUI during the upgrade process", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                Thread.Sleep(3000); //need this delay here
                ProgramBootCodeStart = true;
            }
            else
            {
                this.programBootCodeOnly = false;
                Thread.Sleep(500);
                restartProgram();
            }
        }

        private bool CheckForProperBootCodeAutoUpdate()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            reloadBootWithPrompt = false;
            CheckProperMasterBootCode();
            return wrongBootCodeLoaded;
        }

        private void CheckForProperBootCodeManualUpdate()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.manualReload = true;
            reloadBootWithPrompt = false;
            CheckProperMasterBootCode();
            if (this.notPollingPort && !this.programBootCodeInProgress)
                Thread.Sleep(1000);
        }

        private void checkBootCodeforProperDate()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            DialogResult dR;

            if (!manualReload)
            {
                if (masterBootRevisionNumberReceived < _bootCodeRevisionNumber)
                {
                    dR = new YesNoMessageBoxResized("Boot code out of date", "The boot code is out of date. Would you like to update?", "Yes", "No").ShowDialog();

                    if (dR == DialogResult.Yes)
                    {

                        this.programBootCodeOnly = true;
                        dR = MessageBox.Show("Please do not remove the port, turn off the computer, power down the relay, let the computer sleep or click around the GUI during the upgrade process", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        Thread.Sleep(3000); //need this delay here
                        this.ProgramBootCodeStart = true;
                    }
                    else
                    {
                        this.programBootCodeOnly = false;
                        Thread.Sleep(500);
                        restartProgram();
                    }
                }
                else
                {
                    MessageBox.Show("Boot code correct", "Correct Boot code loaded");
                    Thread.Sleep(500);
                    restartProgram();
                }
            }
            else if (manualReload)
            {
                if (masterBootRevisionNumberReceived < _bootCodeRevisionNumber)
                {
                    programMasterBootFileSelect = true;
                }
                else
                {
                    if (wrongBootCodeLoaded)
                        programMasterBootFileSelect = true;
                    else
                        programMasterBootFileSelect = false;
                }
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
            if ((this.relayCode.FileString == "" || this.relayCode.FileString == null))
            {
                MessageBox.Show("No Relay File Loaded");
                this.State = RelayProgrammingStates.Idle;
                return;
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

        private void sendBootLoaderClearMemory()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            rPEA.Command = RelayProgrammingSendCommands.RawData;

            rPEA.BytesToSend = new byte[3];

            rPEA.BytesToSend[0] = 0x80;
            rPEA.BytesToSend[1] = 0x55;
            rPEA.BytesToSend[2] = 0x0D;

            this.onSend(rPEA);
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
            if ((wrongRelayTypeAutoLoad && !masterRevisionString.Contains("DNP")) || programmingForm.MasterBootComplete)
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
            this.reprogrammingInProgress = true;
            if (!this.loadMasterFirst)
            {
                if (this.autoLoad)
                {
                    if (!this.reprogramRelay)
                    {
                        this.programmingForm.RelayCodeComplete = true;
                        this.programmingForm.RelayDataComplete = true;

                        if (this.reprogramMaster)
                            this.startMasterProgramming();
                        else if (this.reprogramFPGA)
                        {
                            this.programmingForm.MasterCodeComplete = true;
                            this.programmingForm.MasterDataComplete = true;
                            this.programFPGA();
                        }
                        else
                            MessageBox.Show("Nothing to Program");
                        return;
                    }
                }

                if ((this.relayCode.FileString == "" || this.masterCode.FileString == null))
                {
                    MessageBox.Show("No Relay File Loaded");
                    this.State = RelayProgrammingStates.Idle;
                    return;
                }

                this.parseSFile(this.relayCode);



                this.State = RelayProgrammingStates.LoadingRelayCode;
                if (this.autoLoad)
                {
                    this.programmingForm.Maximum = this.relayCode.NumberOfCodeBlocks * 2;
                    this.programmingForm.CurrentTask = "Loading Relay Code b";
                }

                int temp = this.relayCode.NumberOfCodeBlocks * 2;
                this.labelCodeTotal.Text = temp.ToString();

                this.labelDataTotal.Text = this.relayCode.NumberOfDataBlocks.ToString();
                this.labelDataCount.Text = "0";
                this.labelCodeCount.Text = "0";
                this.sendRelayReset();
                Thread.Sleep(100);
                this.enableButtons(false);

                if (this.autoLoad && this.DNPRelay == false)
                    this.masterCode.WithParameters = true;
                else
                    this.masterCode.WithParameters = false;
            }
            else
            {
                if (this.autoLoad)
                {
                    if (!this.reprogramMaster)
                    {
                        this.programmingForm.MasterCodeComplete = true;
                        this.programmingForm.MasterDataComplete = true;

                        if (this.reprogramRelay)
                            this.startRelayProgramming();
                        else if (this.reprogramFPGA)
                        {
                            this.programmingForm.RelayCodeComplete = true;
                            this.programmingForm.RelayDataComplete = true;
                            this.programFPGA();
                        }
                        else
                            MessageBox.Show("Nothing to Program");
                        return;
                    }
                }

                if ((this.masterCode.FileString == "" || this.masterCode.FileString == null))
                {
                    MessageBox.Show("No Master File Loaded");
                    this.State = RelayProgrammingStates.Idle;
                    return;
                }
                // Auto-load always does with parameters.  For now


                this.parseSFile(this.masterCode);

                this.State = RelayProgrammingStates.LoadingMasterCode;
                if (this.autoLoad)
                {
                    this.programmingForm.Maximum = this.masterCode.NumberOfCodeBlocks * 2;
                    this.programmingForm.CurrentTask = "Loading Master Code";
                }
                int temp = this.masterCode.NumberOfCodeBlocks * 2;
                this.labelCodeTotal.Text = temp.ToString();
                if (this.masterCode.WithParameters)
                    this.labelDataTotal.Text = this.masterCode.NumberOfDataBlocks.ToString();
                else
                    this.labelDataTotal.Text = this.masterCode.NonParameterCount.ToString();
                this.labelDataCount.Text = "0";
                this.labelCodeCount.Text = "0";
                if (ActiveRelay)
                    this.sendReset();
                this.enableButtons(false);
            }
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
            if ((wrongRelayTypeAutoLoad && !masterRevisionString.Contains("DNP")) || programmingForm.MasterBootComplete)
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
            if (wrongRelayTypeAutoLoad || programmingForm.MasterBootComplete)
                Thread.Sleep(1000);
        }

        #endregion


        private void enableButtons(bool b)
        {
            return;
            this.buttonProgramMaster.Enabled = b;
            this.buttonProgramRelay.Enabled = b;
            this.buttonSelectMasterSFile.Enabled = b;
            this.buttonSelectRelaySFile.Enabled = b;
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
            this.timerTimeout.Interval = 10000;
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
                this.startProgramming();
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

            this.selectNewestMasterFirmware();
            this.selectNewestRelayFirmware();
            this.selectNewestFPGAFirmware();

            this.programmingForm.ClearAllChecks();
        }

        private void selectNewestRelayFirmware()
        {
            logger.Trace("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
#if DEBUG
            if (Directory.Exists(@"C:\Freescale\RelayProcessor\output\"))
            {

                DirectoryInfo dI = new DirectoryInfo(@"C:\Freescale\RelayProcessor\output\");
                FileInfo[] fI;


                if (GERelay)
                    fI = dI.GetFiles("*GE*.s");
                else
                    fI = dI.GetFiles("*WH*.s");

                if (fI.Length == 0)
                {
                    this.errorHandler("No Relay S File", new Exception("No Relay S File"));
                    return;
                }

                fI.OrderByDescending(f => f.Name);


                this.relayCode.FileName = fI[fI.Length - 1].FullName;
                this.useRelaySFile(fI[fI.Length - 1].FullName);
                this.textBoxRelayFileName.Text = fI[fI.Length - 1].FullName;
            }
            else
            {
                this.errorHandler("No Standard Relay Freescale Directory to search", new Exception("No Standard Relay Freescale Directory to search"));
            }
#endif
        }

        private void selectNewestFPGAFirmware()
        {
#if DEBUG
            if (Directory.Exists(@"C:\Freescale\RelayMasterProcessor\output\"))
            {
                DirectoryInfo dI = new DirectoryInfo(@"C:\Freescale\FPGA Files\");
                FileInfo[] fI = dI.GetFiles("*.rbf");

                if (fI.Length == 0)
                {
                    this.errorHandler("No FPGA File", new Exception("No FPGA file"));
                    return;
                }

                fI.OrderByDescending(f => f.Name);

                this.fPGACode.FileName = fI[fI.Length - 1].FullName;
                this.parseFPGAFile(this.fPGACode);
                this.textBoxFPGAFile.Text = fI[fI.Length - 1].FullName;
            }
            else
            {
                this.errorHandler("No Standard FPGA Freescale Directory to search", new Exception("No Standard FPGA Freescale Directory to search"));
            }
#endif
        }

        private void selectNewestMasterFirmware()
        {
#if DEBUG
            if (Directory.Exists(@"C:\Freescale\RelayMasterProcessor\output\"))
            {
                DirectoryInfo dI = new DirectoryInfo(@"C:\Freescale\RelayMasterProcessor\output\");
                FileInfo[] fI = dI.GetFiles("*.s");

                if (fI.Length == 0)
                {
                    this.errorHandler("No Master S File", new Exception("No Master S File"));
                    return;
                }

                fI.OrderByDescending(f => f.Name);

                this.masterCode.FileName = fI[fI.Length - 1].FullName;
                this.useMasterSFile(fI[fI.Length - 1].FullName);
                this.textBoxMasterFileName.Text = fI[fI.Length - 1].FullName;
            }
            else
            {
                this.errorHandler("No Standard Freescale Directory to search", new Exception("No Standard Freescale Directory to search"));
            }
#endif
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
                this.finalizeReprogram();
            }
            else if (this.State == RelayProgrammingStates.ReprogramSuccess)
            {
                this.state = RelayProgrammingStates.Finalized;
                this.FinalizeReprogram();
            }
        }

        private void comboBoxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.customer = (Customers)this.comboBoxCustomer.SelectedItem;

            if (this.customer == Customers.None || this.customer == Customers.ConEdison)
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
            this.MasterFileGEDNP = cLF.MasterFileGEDNP;
            this.MasterFileWH = cLF.MasterFileWH;
            this.MasterFileWHDNP = cLF.MasterFileWHDNP;
            this.MasterFileAtlantaDNPGE = cLF.MasterFileAtlantaDNPGE;
            this.MasterFileAtlantaDNPWH = cLF.MasterFileAtlantaDNPWH;
            this.RelayFileGE = cLF.RelayFileGE;
            this.RelayFileWH = cLF.RelayFileWH;
            this.RelayFileAtlantaWH = cLF.RelayFileAtlantaWH;
            this.RelayFileAtlantaGE = cLF.RelayFileAtlantaGE;
        }

        public Customers Customer = Customers.None;
        public UInt32 RelayRevision;
        public UInt32 MasterRevision;
        public UInt32 MasterDNPRevision;
        public string MasterFileGE;
        public string MasterFileWH;
        public string MasterFileGEDNP;
        public string MasterFileWHDNP;
        public string RelayFileWH;
        public string RelayFileGE;
        public string RelayFileAtlantaWH;
        public string RelayFileAtlantaGE;
        public string MasterFileDNPPLC;
        public string MasterFileAtlantaDNPGE;
        public string MasterFileAtlantaDNPWH;
        public string MasterFileConEdHBD;
        public string MasterFileConEdSEC;
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