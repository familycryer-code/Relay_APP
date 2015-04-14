using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;
using System.Linq;

namespace RelayControlLibrary
{
    public partial class ucRelayProgramming : UserControl
    {
        public ucRelayProgramming()
        {
            InitializeComponent();

            // Set this string to match code date below
            this.fPGACode.Date = _fPGACodeRevisionNumber.ToString();
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
        
        // These need to be updated when new files are used
#if DEBUG
        private static UInt32 _masterCodeRevisionNumber = 999999;
        private static UInt32 _masterDNPRevisionNumber = 999999;
        private static UInt32 _relayCodeRevisionNumber = 99999999;
        private static UInt32 _fPGACodeRevisionNumber = 121207;
#else
        private static UInt32 _masterCodeRevisionNumber = 150331;
		private static UInt32 _masterDNPRevisionNumber = 140814;
        private static UInt32 _relayCodeRevisionNumber = 20150413;
        private static UInt32 _fPGACodeRevisionNumber = 121207;
#endif

        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                this.customer = value;
            }
        }
        public RelayProgrammingStates State
        {
            get { return this.state; }
            set
            {
                this.writeLineToTraceFile(value.ToString());
                this.state = value;
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
                    this.initializeTraceFile();
                    this.writeLineToTraceFile("Serial Number: " + value.ToString());
                    this.writeLineToTraceFile(DateTime.UtcNow.ToString());
                }

                this.currentRelayLog.SerialNumber = this.serialNumber = value;
                if ((value > 32767 || value == 0) && !this.serialNumberError && this.MasterRevisionNumber != 0)
                {
                    this.serialNumberError = true;
                    MessageBox.Show("Serial Number Error", "Error with Serial Number, \r\nPlease Contact DigialGrid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        public bool GEEnabled
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
            get { return this.dNPRelay;}
            set
            {
#if ATLANTA
                this.dNPRelay = true;
#else
                this.dNPRelay = value;
                if (this.dNPRelay)
                {
                    this.fPGACode.Date = "\0\0\0\0\0\0";
                }
                this.currentRelayLog.DNPRelay = value;
#endif
            }
        }
        public bool TransmitterEnabled
        {
            get { return this.transmitterEnabled; }
            set{
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
                // 012345 is the value loaded in the boot loader
                if (value == 012345 || value == 121116)
                    this.loadMasterFirst = true;
               
                // If it is a rev 1/0. it shouldn't be upgraded
                /*
                if (value < 100713)
                {
                    if (value != this.remoteMasterRevisionNumber)
                    {
                        MessageBox.Show("Please Contact DigitalGrid Inc and ship relay back to factor for upgrade", "Relay Upgrade");
                        this.firstCheckForUpdate = false;
                        this.remoteMasterRevisionNumber = value;
                    }
                    
                    return;
                }
                 */
                switch (this.State)
                {
                    case RelayProgrammingStates.LoadingMasterCode:
                        this.programmingForm.CurrentTask = "Failed to Respond to Transfer Command";
                        this.startMasterProgramming();
                        break;
                    case RelayProgrammingStates.ReprogramSuccess:
                        this.timerTimeout.Stop();
                        this.State = RelayProgrammingStates.RequestAll;
                        this.requestAll();
                        break;
                    case RelayProgrammingStates.Finalized:
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
                        MessageBox.Show("Please Contact DigitalGrid Inc and ship relay back to factor for upgrade", "Relay Upgrade");
                        this.firstCheckForUpdate = false;
                        this.remoteMasterRevisionNumber = value;
                    }
                }

                this.remoteMasterRevisionNumber = value;
                
                // Check to make sure that it only checks while idle so that we don't accident reset it during reloads
                if(this.State == RelayProgrammingStates.Idle)
                {
                    if (this.DNPRelay)
                    {
                        if (this.remoteMasterRevisionNumber < _masterDNPRevisionNumber)
                            this.reprogramMaster = true;
                        else
                            this.reprogramMaster = false;
                    }
                    else
                    {
                        if (this.remoteMasterRevisionNumber < _masterCodeRevisionNumber)
                            this.reprogramMaster = true;
                        else
                            this.reprogramMaster = false;
                    }
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
            get { return this.remoteRelayRevisionNumber;}
            set
            {
                this.remoteRelayRevisionNumber = value;
                if (this.remoteRelayRevisionNumber < _relayCodeRevisionNumber)
                    this.reprogramRelay = true;
                else
                    this.reprogramRelay = false;
            }
        }
        public UInt32 FPGARevisionNumber
        {
            get { return this.remoteFPGARevisionNumber;}
            set
            {
                if (value == 0xFFFFFF)
                    this.remoteFPGARevisionNumber = 0;
                else
                    this.remoteFPGARevisionNumber = value;

                if (this.remoteFPGARevisionNumber < _fPGACodeRevisionNumber && this.TransmitterEnabled)
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
        private bool autoLoad = false;
        private UInt32 remoteMasterRevisionNumber = 0;
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

        private const string _logPath = @"C:\DGI Systems\Relay\Log\";
        private string traceFile;

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
                CustomerLoadFiles regular = new CustomerLoadFiles(Customers.DigitalGrid);
                regular.FPGAFile.DataBytes = RelayControlLibrary.Properties.Resources.FPGAdata;
                regular.MasterFileGE = RelayControlLibrary.Properties.Resources.MasterProcessor;
                regular.MasterFileGEDNP = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_GE;
                regular.MasterFileWH = RelayControlLibrary.Properties.Resources.MasterProcessor;
                regular.MasterFileWHDNP = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP;
                regular.RelayFileGE = RelayControlLibrary.Properties.Resources.RelayProcessorGE;
                regular.RelayFileWH = RelayControlLibrary.Properties.Resources.RelayProcessor;

                CustomerLoadFiles workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.DigitalGridDNP));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.DigitalGrid));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.Dominion));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.Memphis));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.NonConEd));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile  = this.customersFiles.Find(x => x.Customer.Equals(Customers.None));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile = this.customersFiles.Find(x => x.Customer.Equals(Customers.PEPCO));
                this.copyCustomerLoadFiles(workingLoadFile, regular);

                workingLoadFile  = this.customersFiles.Find(x => x.Customer.Equals(Customers.SMUD));
                this.copyCustomerLoadFiles(workingLoadFile, regular);
                workingLoadFile.MasterFileWHDNP = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_SMUD;
                workingLoadFile.MasterFileGEDNP = RelayControlLibrary.Properties.Resources.MasterProcessor_with_DNP_GE_SMUD;
            }
            catch (Exception ex)
            {
                this.errorHandler("Error creating Customer File Lists", ex);
            }
        }

        private void copyCustomerLoadFiles(CustomerLoadFiles destination, CustomerLoadFiles source)
        {
            destination.FPGAFile = source.FPGAFile;
            destination.MasterFileGE = source.MasterFileGE;
            destination.MasterFileGEDNP = source.MasterFileGEDNP;
            destination.MasterFileWH = source.MasterFileWH;
            destination.MasterFileWHDNP = source.MasterFileWHDNP;
            destination.RelayFileGE = source.RelayFileGE;
            destination.RelayFileWH = source.RelayFileWH;
        }


        public void InitialAutoLoadFiles()
        {
            DialogResult dR;

            dR = MessageBox.Show("Do you want to attempt to reprogram the Relay?", "Initial Auto Reload", MessageBoxButtons.YesNo);

            if (dR != DialogResult.Yes)
                return;

            this.dontReloadFromResource = false;
            this.useDefaultSettings = true;

            switch(this.customer)
            {
                case Customers.Memphis:
                    this.GEEnabled = false;
                    break;
                default:
                    dR = new CustomYesNoDialog("GE or WH Select", "Is this a GE or WH style relay?", "GE", "WH").ShowDialog();
                    if (dR == DialogResult.Yes)
                        this.GEEnabled = true;
                    else
                        this.GEEnabled = false;
                    break;
            }
            /*
            switch (this.customer)
            {
                case Customers.Memphis:
                    this.DNPRelay = true;
                    break;
                default:
                    dR = this.askIfDNPRelay();

                    break;

            }
            */
#if BASICRELEASE
            // This is a non-DNP, transmitter Enabled Relay
            this.TransmitterEnabled = true;
            this.DNPRelay = false;
            this.reprogramFPGA = true;
#endif
#if DEBUG
            dR = new CustomYesNoDialog("Select Communication Type", "Does this have DNP?", "Yes", "No").ShowDialog();
            if (dR == DialogResult.Yes)
            {
                this.DNPRelay = true;
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
            this.loadMasterFirst = true;
            this.masterCode.WithParameters = false;
            this.manualReload = true;
            this.autoLoad = true;
            this.reprogramMaster = true;
            this.reprogramRelay = true;
            this.setProgrammingFiles();
            this.startProgramming();
        }

        private DialogResult askIfDNPRelay()
        {
            return new CustomYesNoDialog("Select Communication Type", "Does this have DNP?", "Yes", "No").ShowDialog();
        }

        private void setDNPRelay(DialogResult dR)
        {
            if (dR == DialogResult.Yes)
            {
                this.DNPRelay = true;
                this.TransmitterEnabled = false;
            }
        }

        private void determineIfTransmitterRelay()
        {
            DialogResult dR;

            switch(this.customer)
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
            if (this.dontReloadFromResource || RelayProgrammingStates.Idle != this.state)
                return;

            CustomerLoadFiles cLF = this.customersFiles.Find(x => x.Customer.Equals(this.customer));
            if (this.DNPRelay)
            {
                if (this.GEEnabled)
                {
                    this.masterCode.FileString = cLF.MasterFileGEDNP;
                    this.textBoxMasterFileName.Text = "Master Relay GE with DNP From Resource " + this.customer.ToString();

                    this.relayCode.FileString = cLF.RelayFileGE;
                    this.textBoxRelayFileName.Text = "GE Relay From Resource " + this.customer.ToString();
                }
                else // Westinghouse DNP
                {
                    this.masterCode.FileString = cLF.MasterFileWHDNP;
                    this.textBoxMasterFileName.Text = "Master Relay WH with DNP From Resource " + this.customer.ToString();

                    this.relayCode.FileString = cLF.RelayFileWH;
                    this.textBoxRelayFileName.Text = "WH Relay From Resource " + this.customer.ToString();

                }
            }
            else // Non-DNP
            {
                
                if (this.GEEnabled)
                {
                    this.masterCode.FileString = cLF.MasterFileWH;
                    this.textBoxMasterFileName.Text = "Master Relay GE From Resource";

                    this.relayCode.FileString = cLF.RelayFileGE;
                    this.textBoxRelayFileName.Text = "GE Relay From Resource";
                }
                else
                {
                    this.masterCode.FileString = cLF.MasterFileWH;
                    this.textBoxMasterFileName.Text = "Master Relay WH From Resource";

                    this.relayCode.FileString = cLF.RelayFileWH;
                    this.textBoxRelayFileName.Text = "WH Relay From Resource";
                }
            }

            if (this.transmitterEnabled)
            {
                this.parseFPGAFile(this.fPGACode);
                this.textBoxFPGAFile.Text = "FPGA Code From Resource";
            }

            this.parseSFile(this.masterCode);
            this.parseSFile(this.relayCode);

            this.writeLineToTraceFile("MP: " + this.textBoxMasterFileName.Text + " RP: " + this.textBoxRelayFileName.Text + " FPGA: " + this.textBoxFPGAFile.Text);
        }


        formProgrammingProgess programmingForm = new formProgrammingProgess();

        private bool firstCheckForUpdate = true;

        public void CheckForUpdate()
        {
            if (!this.firstCheckForUpdate)
                return;

            this.firstCheckForUpdate = false;
            if (this.reprogramFPGA || this.reprogramMaster || this.reprogramRelay)
            {
                this.transmitterEnabled = true;
           
                if (!this.gERelaySerialMatch && !this.serialNumberError)
                    this.askIfGERelay();
                this.setProgrammingFiles();

                this.startAutoLoad();
            }
            else
                this.writeLineToTraceFile("No Updated Needed");
        }

        private void askIfGERelay()
        {
            if (this.loadMasterFirst)
                return;

            DialogResult dR = dR = new CustomYesNoDialog("Serial Number and Relay Type Mismatch", "Is this a GE or WH style relay?", "GE", "WH").ShowDialog();

            if (dR == DialogResult.Yes)
            {
                if (this.serialNumber < 25000 || this.serialNumber > 32767 || this.serialNumber == 0) //25k and up are GE serial Numbers
                {
                    MessageBox.Show("Bad Serial Number!", "Problem with Serial Number. \r\nPlease Contact DigitalGrid Inc.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.serialNumberError = true;
                }

                this.GEEnabled = true;
                this.addGERelayToTransmitterPacket(true);
            }
            else
            {
                if (this.serialNumber >= 25000 || this.serialNumber == 0)
                {
                    this.serialNumberError = true;
                    MessageBox.Show("Bad Serial Number!", "Problem with Serial Number. \r\nPlease Contact DigitalGrid Inc.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                /*
                RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

                rPEA.Command = RelayPorgrammingSendCommands.DisableGERelayFix;
                if (this.Send != null)
                    this.Send(this, rPEA);
                */

                this.GEEnabled = false;
                this.addGERelayToTransmitterPacket(false);
            }
        }

        private void addGERelayToTransmitterPacket(bool b)
        {
            if(b)
                this.TransmitterPacket[28] |= 0x10;
            else
                this.TransmitterPacket[28] &= 0xEF;
        }

        
        private void startAutoLoad()
        {
            DialogResult dR;
            if (this.serialNumberError)
                return;

            dR = MessageBox.Show("Would you like to Update Relay Code?", "Relay Code Updater", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (dR == DialogResult.Yes)
            {
                // If we aren't loading from resource, don't bother asking this question
                
                if (!this.dontReloadFromResource)
                    dR = MessageBox.Show("Are You Sure?  This will take a while.", "Are You Sure?", MessageBoxButtons.YesNo);

                if (dR == DialogResult.Yes)
                {
                    RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

                    rPEA.Command = RelayPorgrammingSendCommands.SaveSettings;

                    this.onSend(rPEA);
                    
                    // If we aren't loading from resource, don't bother warning
                    if (!this.dontReloadFromResource)
                        MessageBox.Show("Please do not remove the port, turn off the computer, power down the relay, let the computer sleep or click around the GUI during the upgrade process");

                    this.writeLineToTraceFile("User Verified Programming Start");

                    this.autoLoad = true;
                    this.programmingForm.ClearAllChecks();
                    this.startProgramming();
                    if(!this.programmingForm.Visible)
                        this.programmingForm.ShowDialog();
                }
            }
            else
                this.autoLoad = false;
        }

        public void PrepForBoot()
        {
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
            this.writeLineToTraceFile("Waiting For Boot - " + this.state.ToString());
        }

        public void SetTransmitterPacket(byte[] bytePacket)
        {
            this.TransmitterPacket = bytePacket;
        }

        private void SendTransmitterSettings()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            if (this.TransmitterPacket != null || this.manualReload)
            {
                rPEA.BytesToSend = this.TransmitterPacket;
                rPEA.Command = RelayPorgrammingSendCommands.TransmitterSettings;

                if (this.TransmitterPacket != null)
                    this.onSend(rPEA);

                this.TransmitterPacket = null;

                this.writeLineToTraceFile("Updated Transmitter Settings with Stored Settings");
            }
        }

        private delegate void booleanInvoke(bool b);

        private void onSend(RelayProgrammingEventArgs rPEA)
        {
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
                        this.writeStringToTraceFile("AckM, ");
                        if (this.State == RelayProgrammingStates.WaitingForBootMaster)
                            this.State = RelayProgrammingStates.LoadingMasterCode;
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.sendNextMasterPacket();
                        break;
                    case RelayProgrammingStates.LoadingRelayCode:
                    case RelayProgrammingStates.LoadingRelayData:
                        this.writeStringToTraceFile("AckR, ");
                        if (this.State == RelayProgrammingStates.WaitingForBootRelay)
                            this.State = RelayProgrammingStates.LoadingRelayCode;
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.sendNextRelayPacket();
                        break;
                    case RelayProgrammingStates.LoadingFPGACode:
                        this.writeStringToTraceFile("AckF, ");
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.sendNextFPGAPacket();
                        break;
                    case RelayProgrammingStates.ClearingBootLoader:
                        this.writeStringToTraceFile("AckU, ");
                        this.timerTimeout.Stop();
                        this.timerTimeout.Interval = 1500;
                        this.timerTimeout.Start();
                        this.State = RelayProgrammingStates.LoadingBootLoader;
                        this.sendNextBootLoaderPacket();
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
                if (this.InvokeRequired)
                {
                    booleanInvoke bI = new booleanInvoke(this.PacketAcknowledged);
                    this.Invoke(bI, new object[] { b });
                }
                else
                {
                    if (this.programmingForm.InvokeRequired)
                    {
                        booleanInvoke bI = new booleanInvoke(this.PacketAcknowledged);
                        programmingForm.Invoke(bI, new object[] { b });
                    }
                    else
                        this.packetAcknowledged(b);
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error In CallBack Delegate in RelayProgramming", ex);
            }
        }

        public void BootReceived()
        {
            this.labelState.Text = "Boot Received";
            if(this.autoLoad && !this.programmingForm.Visible)
                this.programmingForm.ShowDialog();

            this.writeLineToTraceFile("Boot Received - " + this.state.ToString());

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
                    if(this.failCount == 5)
                        this.programmingForm.CurrentTask = "Relay Did Not Respond To Master Packet - Try Manually Resetting Relay Or Just Wait";

                    this.failCount++;
                    this.sendMasterTransferPacket();
                    break;
                case RelayProgrammingStates.WaitingForBootRelay:
                case RelayProgrammingStates.LoadingRelayCode:
                    this.programmingForm.CurrentTask = "Loading Relay Code";
                    this.sendRelayTransferPacket();
                    break;
                case RelayProgrammingStates.WaitingForBootFPGA:
                case RelayProgrammingStates.LoadingFPGACode:
                    this.programmingForm.CurrentTask = "Loading FPGA Code";
                    this.sendFPGATransferPacket();
                    break;
                default:
                    break;
            }
        }

        private void masterFinished()
        {
            this.writeLineToTraceFile("Master Finished Loading");
            if (autoLoad)
            {
                this.State = RelayProgrammingStates.WaitingForBootRelay;
            }
            else
                this.enableButtons(true);
        }


        private void sendNextRelayPacket()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayPorgrammingSendCommands.RawData;

            if (this.State == RelayProgrammingStates.LoadingRelayCode)
            {
                try
                {

                    rPEA.BytesToSend = new byte[1024];
                    this.programmingForm.CurrentTask = "Loading Relay Code";
                    if (this.relayCode.CodeBytes.Count == 0)
                    {
                        this.State = RelayProgrammingStates.LoadingRelayData;
                        this.programmingForm.RelayCodeComplete = true;
                        this.programmingForm.CurrentTask = "Loading Relay Data";
                        this.writeLineToTraceFile("Loading Relay Data");
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

                    this.writeStringToTraceFile("RC, ");
                    
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

                this.writeStringToTraceFile("RD, ");
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

        private void sendNextBootLoaderPacket()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayPorgrammingSendCommands.RawData;

            this.failCount = 0;

            if (this.State == RelayProgrammingStates.LoadingBootLoader)
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

                this.writeStringToTraceFile("BL, ");

                this.onSend(rPEA);
            }
        }

        private void sendNextMasterPacket()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayPorgrammingSendCommands.RawData;
            
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
                        this.writeLineToTraceFile("Loading Master Data");
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
                    this.writeStringToTraceFile("MC, ");
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
                    this.writeLineToTraceFile("");
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
                this.writeStringToTraceFile("MD, ");
            }
            else
            {
                throw new Exception("Called sendNextMasterPacket from wrong state: " + this.state.ToString());
            }

            this.onSend(rPEA);
        }

        private void sendNextFPGAPacket()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayPorgrammingSendCommands.RawData;

            try
            {
                rPEA.BytesToSend = new byte[1024];

                Int32 temp = Convert.ToInt32(this.labelCodeCount.Text);

                if (this.fPGACode.SendIndex >= 98304)
                {
                    this.programmingForm.FPGAComplete = true;
                    this.programmingForm.Hide();
                    this.timerTimeout.Stop();
                    this.writeLineToTraceFile("");
                    this.writeLineToTraceFile("Done Loading FPGA");
                    if (this.autoLoad)
                        this.allReprogramingDone();
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
                this.writeStringToTraceFile("FP, ");
                
                this.onSend(rPEA);
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Sending Next FPGA Data", ex);
            }
        }

        private void doneLoadingRelay()
        {
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
                        this.writeLineToTraceFile("Loading Master Code");
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
                        this.writeLineToTraceFile("Loading FPGA");
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
                if (this.autoLoad)
                {
                    this.programmingForm.RelayDataComplete = true;
                    if (this.reprogramFPGA)
                    {
                        this.parseFPGAFile(this.fPGACode);
                        this.State = RelayProgrammingStates.WaitingForBootFPGA;
                        this.programmingForm.Maximum = 96;
                        this.programmingForm.CurrentTask = "Loading FPGA";
                        this.writeLineToTraceFile("Loading FPGA");
                    }
                    else
                        this.allReprogramingDone();
                }
                else
                    this.allReprogramingDone();
            }
        }

        private void doneLoadingMaster()
        {
            this.timerTimeout.Stop();
            this.programmingForm.MasterDataComplete = true;

            if (!this.loadMasterFirst)
            {
                if (this.autoLoad)
                {
                    if (this.reprogramRelay)
                    {
                        this.parseSFile(this.relayCode);
                        this.programmingForm.CurrentTask = "Loading Relay Code";
                        this.programmingForm.Maximum = this.relayCode.NumberOfCodeBlocks * 2;
                        this.State = RelayProgrammingStates.WaitingForBootRelay;
                        this.timerTimeout.Start();
                    }
                    if (this.reprogramFPGA)
                    {
                        this.parseFPGAFile(this.fPGACode);
                        this.programmingForm.RelayCodeComplete = true;
                        this.programmingForm.RelayDataComplete = true;
                        this.programmingForm.CurrentTask = "Loading FPGA";
                        this.writeLineToTraceFile("Loading FPGA");
                        this.programmingForm.Maximum = 96;
                        this.State = RelayProgrammingStates.WaitingForBootFPGA;
                        this.timerTimeout.Start();
                    }
                    else
                    {
                        this.programmingForm.FPGAComplete = true;
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
                        this.programmingForm.CurrentTask = "Loading Relay Code";
                        this.writeLineToTraceFile("Loading Relay Code");
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
                        this.writeLineToTraceFile("Loading FPGA");
                        this.programmingForm.Maximum = 96;
                        this.State = RelayProgrammingStates.WaitingForBootFPGA;
                        this.timerTimeout.Start();
                    }
                    else
                    {
                        this.programmingForm.FPGAComplete = true;
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
            this.writeLineToTraceFile("All Loading Done");
            if (this.autoLoad)
            {
                this.State = RelayProgrammingStates.ReprogramSuccess;
                this.timerTimeout.Stop();
                this.timerTimeout.Interval = 2000;
                this.timerTimeout.Start();
            }
            else
            {
                this.State = RelayProgrammingStates.Idle;
                this.finalizeReprogram();
            }
            
        }

        private void requestAll()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayPorgrammingSendCommands.RequestAll;
            // Delay put in so I don't send before processor is ready.
            Thread.Sleep(2000);

            this.onSend(rPEA);
        }

        private void finalizeReprogram()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            this.enableButtons(true);
            this.programmingForm.Hide();
            
            rPEA.Command = RelayPorgrammingSendCommands.RestartProgram;
          
            this.onSend(rPEA);

            this.autoLoad = false;
            this.loadMasterFirst = false;
            this.firstCheckForUpdate = false;

            MessageBox.Show("Reprogram Completed Successfully", "Reprogramming Completed Successfully!");
            this.requestAll();
        }

        private void doneLoadingRelayBootLoader()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            this.enableButtons(true);

            rPEA.Command = RelayPorgrammingSendCommands.RestartProgram;
            this.onSend(rPEA);
        }

        public void FinalizeReprogram()
        {
            if (this.state == RelayProgrammingStates.Finalized)
            {
                MessageBox.Show("Reprogram Completed Successfully", "Reprogramming Completed Successfully!");
                this.writeLineToTraceFile("Reprogam Completed Successfully");
                this.logUpdate();
                this.state = RelayProgrammingStates.Idle;
            }
            
        }

        private void logUpdate()
        {
            try
            {
                if (!Directory.Exists(@"C:\DGI Systems\Relay\Log\"))
                    Directory.CreateDirectory(@"C:\DGI Systems\Relay\Log\");
                if(!File.Exists(@"C:\DGI Systems\Relay\Log\UpdateLog.txt"))
                    using (File.Create(@"C:\DGI Systems\Relay\Log\UpdateLog.txt")) { };
                using (StreamWriter sW = new StreamWriter(@"C:\DGI Systems\Relay\Log\UpdateLog.txt", true))
                {
                    sW.WriteLine(this.currentRelayLog.GetLogString());
                }
            }
            catch
            {
                MessageBox.Show("Error Writing Log File", "Could Not Write to LogFile.\r\nMaybe it is Open");
            }
        }

        private void initializeTraceFile()
        {
            try
            {
#if !DEBUG 
                return;
#endif
                this.traceFile = _logPath + "RelayUpdate_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmss") + ".txt";

                if (!Directory.Exists(_logPath))
                    Directory.CreateDirectory(_logPath);

                if (!File.Exists(this.traceFile))
                    using (File.Create(this.traceFile)) { };
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Initializing Trace File", ex);
            }
        }

        private bool fileWritingAllowed = true;

        private void writeLineToTraceFile(string s)
        {
            try
            {
#if !DEBUG 
                return;
#endif
                if(!fileWritingAllowed)
                    return;
                if (!File.Exists(this.traceFile))
                    return;
                using (StreamWriter sW = new StreamWriter(this.traceFile, true, Encoding.ASCII))
                {
                    sW.WriteLine(s);
                }
            }
            catch(Exception ex)
            {
                this.fileWritingAllowed = false;
                this.errorHandler("Error Writing To Trace File", ex);
            }
        }

        private void writeStringToTraceFile(string s)
        {
            try
            {
                string tempString = "";
                bool writeNewLine = false;
#if !DEBUG 
                return;
#endif

                if (!fileWritingAllowed)
                    return;

                if (!File.Exists(this.traceFile))
                    return;

                using (StreamReader sR = new StreamReader(this.traceFile, Encoding.ASCII))
                {
                    while(sR.Peek() >= 0)
                    {
                        tempString = sR.ReadLine();
                    }

                    if (tempString.Length >= 80)
                        writeNewLine = true;
                }

                using (StreamWriter sW = new StreamWriter(this.traceFile, true, Encoding.ASCII))
                {
                    if (writeNewLine)
                        sW.WriteLine();
                    sW.Write(s);
                }
            }
            catch (Exception ex)
            {
                this.fileWritingAllowed = false;
                this.errorHandler("Error Writing To Trace File", ex);
            }
        }
        
        private void sendNonTransmitterSettings()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            // Return if manual reload because we don't have settings
            this.writeLineToTraceFile("Send Non Transmitter Settings");
            if (this.manualReload)
                return;

            if (!this.useDefaultSettings)
                rPEA.Command = RelayPorgrammingSendCommands.RecallSavedSettings;
            else
                rPEA.Command = RelayPorgrammingSendCommands.RestoreDefaults;

            this.onSend(rPEA);
        }

        private void sendMasterTransferPacket()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.BytesToSend = new byte[4];

            rPEA.BytesToSend[0] = 0x55; // 'U'
            rPEA.BytesToSend[1] = this.masterCode.NumberOfCodeBlocks;
            if(this.masterCode.WithParameters)
                rPEA.BytesToSend[2] = this.masterCode.NumberOfDataBlocks;
            else
                rPEA.BytesToSend[2] = this.masterCode.NonParameterCount;
            rPEA.BytesToSend[3] = 0x0D;

            Thread.Sleep(5);
            if (this.Send != null)
            {
                this.onSend(rPEA);
                this.labelState.Text = "Sent Master Transfer Packet";
                this.writeLineToTraceFile("Sent Master Transfer Packet");
            }
            this.timerTimeout.Stop();
            this.timerTimeout.Interval = 7000;
            this.timerTimeout.Start();

            this.State = RelayProgrammingStates.LoadingMasterCode;
        }

        private void sendRelayTransferPacket()
        {
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
                this.writeLineToTraceFile("Sent Relay Transfer Packet");
            }
            
            this.timerTimeout.Interval = 7000;
            this.timerTimeout.Start();
        }

        private void sendFPGATransferPacket()
        {
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
                this.writeLineToTraceFile("Sent FPGA Transfer Packet");
            }
            this.timerTimeout.Stop();
            this.timerTimeout.Interval = 7000;
            this.timerTimeout.Start();
        }

        private void useRelaySFile(string fileName)
        {
            StreamReader sR;
            try
            {
                sR = new StreamReader(fileName, Encoding.ASCII);
                this.relayCode.FileString = sR.ReadToEnd();
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
            StreamReader sR;
            try
            {
                sR = new StreamReader(fileName, Encoding.ASCII);
                this.masterCode.FileString = sR.ReadToEnd();
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
                        
                        // Might want to check this address
                        while (address < 0x88000)
                        {
                        
                            // Add the address
                            this.addBootLoaderAddress(rPD.CodeBytes, address);
                        
                            // Add the data
                            // Remove length and address
                            s = s.Remove(0, 12);

                            // next (length - 5) are the data bytes

                            for (int i = 0; i < 32; i++)
                            {
                                try
                                {
                                    rPD.CodeBytes.Add(Convert.ToByte(s.Substring(i * 2, 2), 16));
                                }
                                catch
                                {
                                    // Fill in extra space with FF
                                    rPD.CodeBytes.Add(0xFF);
                                }
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
                this.writeLineToTraceFile("Selected Master File");
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
                this.writeLineToTraceFile("Selected Relay File");
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
                    this.writeLineToTraceFile("Selected FPGA File");
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Opening FPGA RBF File", ex);
            }
        }

        private void startRelayProgramming()
        {
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
            this.programmingForm.CurrentTask = "Loading Relay Code";
            this.labelCodeTotal.Text = temp.ToString();
            this.labelDataTotal.Text = this.relayCode.NumberOfDataBlocks.ToString();
            this.labelDataCount.Text = "0";
            this.labelCodeCount.Text = "0";
            this.sendRelayReset();
            this.enableButtons(false);
        }

        private void startRelayBootLoaderProgramming()
        {
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
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            rPEA.Command = RelayPorgrammingSendCommands.RawData;

            rPEA.BytesToSend = new byte[3];

            rPEA.BytesToSend[0] = 0x80;
            rPEA.BytesToSend[1] = 0x55;
            rPEA.BytesToSend[2] = 0x0D;

            this.onSend(rPEA);
        }

        private void startMasterProgramming()
        {
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
            this.enableButtons(false);
        }

        private void startMasterProgrammingWithParameters()
        {
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
            if(!this.loadMasterFirst)
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
                    this.programmingForm.CurrentTask = "Loading Relay Code";
                }
            
                int temp = this.relayCode.NumberOfCodeBlocks * 2;
                this.labelCodeTotal.Text = temp.ToString();

                this.labelDataTotal.Text = this.relayCode.NumberOfDataBlocks.ToString();
                this.labelDataCount.Text = "0";
                this.labelCodeCount.Text = "0";
                this.sendRelayReset();
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
                if(this.masterCode.WithParameters)
                    this.labelDataTotal.Text = this.masterCode.NumberOfDataBlocks.ToString();
                else
                    this.labelDataTotal.Text = this.masterCode.NonParameterCount.ToString();
                this.labelDataCount.Text = "0";
                this.labelCodeCount.Text = "0";
                this.sendReset();
                this.enableButtons(false);
            }
        }



        private void resetMasterProgramming()
        {
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
            this.programmingForm.CurrentTask = "Failed Loading Master Retrying - Waiting For Boot";

            int temp = this.masterCode.NumberOfCodeBlocks * 2;
            this.labelCodeTotal.Text = temp.ToString();
            if(this.masterCode.WithParameters)
                this.labelDataTotal.Text = this.masterCode.NumberOfDataBlocks.ToString();
            else
                this.labelDataTotal.Text = this.masterCode.NonParameterCount.ToString();
            this.labelDataCount.Text = "0";
            this.labelCodeCount.Text = "0";

            this.sendReset();
        }

        private void resetProgrammingRelay()
        {
            if (this.relayCode.FileString == "" || this.relayCode.FileString == null)
            {
                MessageBox.Show("No Relay File Loaded");
                this.State = RelayProgrammingStates.Idle;
                return;
            }
            this.parseSFile(this.relayCode);

            this.programmingForm.CurrentTask = "Failed Loading Relay Code Retrying - Waiting For Boot";
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
            this.parseFPGAFile(this.fPGACode);

            this.programmingForm.CurrentTask = "Failed Loading FPGA Code.  Retrying - Waiting For Boot";
            this.programmingForm.FPGAComplete = false;

            this.State = RelayProgrammingStates.LoadingFPGACode;

            this.labelCodeTotal.Text = "96";
            this.labelDataTotal.Text = "0";
            this.labelCodeCount.Text = "0";
            this.labelDataCount.Text = "0";

            this.sendRelayReset();
        }

        private void sendQuietMode()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

        }

        private void sendRelayReset()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayPorgrammingSendCommands.RawData;
            rPEA.BytesToSend = new byte[3];

            rPEA.BytesToSend[0] = (byte)'B';
            rPEA.BytesToSend[1] = (byte)'U';
            rPEA.BytesToSend[2] = 0x0D;

            this.onSend(rPEA);
        }

        private void sendReset()
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();
            rPEA.Command = RelayPorgrammingSendCommands.RawData;
            rPEA.BytesToSend = new byte[3];

            rPEA.BytesToSend[0] = (byte)'b';
            rPEA.BytesToSend[1] = (byte)'U';
            rPEA.BytesToSend[2] = 0x0D;

            this.onSend(rPEA);
        }

        private void buttonProgramRelay_Click(object sender, EventArgs e)
        {
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
            this.PrepForBoot();
        }

        #endregion

        private void timerTimeout_Tick(object sender, EventArgs e)
        {
            RelayProgrammingEventArgs rPEA = new RelayProgrammingEventArgs();

            this.labelState.Text = "Time Out";
            this.programmingForm.CurrentTask = "Timed Out - Restarting";
            this.writeLineToTraceFile("Timed Out in State " + this.state);
            this.timerTimeout.Stop();
            this.timerTimeout.Interval = 10000;
            this.timerTimeout.Start();

            switch(this.State)
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
                    this.State = RelayProgrammingStates.RequestAll;
                    this.requestAll();
                    break;
                
            }

            
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
                this.GEEnabled = true;
            else
                this.GEEnabled = false;

            this.selectNewestMasterFirmware();
            this.selectNewestRelayFirmware();
            this.selectNewestFPGAFirmware();

            this.programmingForm.ClearAllChecks();
        }

        private void selectNewestRelayFirmware()
        {
#if DEBUG
            if (Directory.Exists(@"C:\Freescale\RelayProcessor\output\"))
            {

                DirectoryInfo dI = new DirectoryInfo(@"C:\Freescale\RelayProcessor\output\");
                FileInfo[] fI;

               
                if(this.GEEnabled)
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
            if (this.State == RelayProgrammingStates.WaitForAllData)
            {
                this.State = RelayProgrammingStates.Finalized;
                this.SendTransmitterSettings();
                this.sendNonTransmitterSettings();
                this.finalizeReprogram();
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
        /*

        private void buttonClearAllProgrammingFields_Click(object sender, EventArgs e)
        {
            this.textBoxFPGAFile.Text = "";
            this.textBoxRelayFileName.Text = "";
            this.textBoxMasterFileName.Text = "";
        }

        private void buttonLoadDefaultResourceSFiles_Click(object sender, EventArgs e)
        {
            this.dontReloadFromResource = false;
            this.setProgrammingFiles();
        }
         */
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

    public enum RelayPorgrammingSendCommands
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
        DisableGERelayFix
    }
    public class RelayProgrammingEventArgs : EventArgs
    {
        public RelayProgrammingEventArgs()
        {
        }

        public byte[] BytesToSend;

        public RelayPorgrammingSendCommands Command;
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
        LoadingBootLoader
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
        public string RelayFileWH;
        public string RelayFileGE;
        public FPGAProgrammingData FPGAFile = new FPGAProgrammingData();
    }
}
