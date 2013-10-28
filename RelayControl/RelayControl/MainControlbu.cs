using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using CharlieSerialIO;
using System.Collections;
using PhasorDisplayGraph;
using RelayControlLibrary;
using SineDisplayGraph;
using MyFileIO;
using System.IO.Ports;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;

namespace RelayControl
{
    public partial class MainControl : Form
    {
        public MainControl()
        {
            try
            {
                InitializeComponent();
                this.initializeEventPage();
                this.initializeSaveObject();

                this.timerLiveEventAcknowledge.Interval = 250;
                this.timerLiveEventAcknowledge.SynchronizingObject = this;
                this.timerLiveEventAcknowledge.Elapsed +=new System.Timers.ElapsedEventHandler(timerLiveEventAcknowledge_Tick);
                this.updatePortMonitorBox(0x11, Color.Red);
                this.ucCloseMode1.Send += new ucCloseMode.SendHandler(ucCloseMode1_Send);
                this.ucTripMode2.Send += new ucTripMode.SendEventHandler(ucTripMode2_Send);
                this.ucCalibration1.Send += new ucCalibration.SendHandler(ucCalibration1_Send);
                this.ucPumpMode1.Send += new ucPumpMode.SendEventHandler(ucPumpMode1_Send);
                this.ucTransmitter1.Send += new ucTransmitter.SendEventHandler(ucTransmitter1_Send);
                this.ucCloseMode1.CloseControlException += new ucCloseMode.ExceptionHandler(ucCloseMode1_CloseControlException);
                this.ucTripMode2.TripControlException += new ucTripMode.ExceptionHandler(ucTripMode2_TripControlException);
                this.ucPumpMode1.PumpControlException += new ucPumpMode.ExceptionHandler(ucPumpMode1_PumpControlException);
                this.ucTransmitter1.TransmitterException += new ucTransmitter.ExceptionHandler(ucTransmitter1_TransmitterException);
                this.ucLiveData1.Error += new ucLiveData.ErrorHandler(ucLiveData1_Error);
                this.ucLiveData1.PacketHandled += new ucLiveData.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph0.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph1.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph2.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph3.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph4.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph5.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph6.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph7.PacketHandled += new ucEventGraph.PacketHandledHandler(liveEvent_PacketHandled);
                this.ucEventGraph0.EventGraphException += new ucEventGraph.ExceptionHandler(ucEventGraph_EventGraphException);
                this.ucEventGraph1.EventGraphException += new ucEventGraph.ExceptionHandler(ucEventGraph_EventGraphException);
                this.ucEventGraph2.EventGraphException += new ucEventGraph.ExceptionHandler(ucEventGraph_EventGraphException);
                this.ucEventGraph3.EventGraphException += new ucEventGraph.ExceptionHandler(ucEventGraph_EventGraphException);
                this.ucEventGraph4.EventGraphException += new ucEventGraph.ExceptionHandler(ucEventGraph_EventGraphException);
                this.ucEventGraph5.EventGraphException += new ucEventGraph.ExceptionHandler(ucEventGraph_EventGraphException);
                this.ucEventGraph6.EventGraphException += new ucEventGraph.ExceptionHandler(ucEventGraph_EventGraphException);
                this.ucEventGraph7.EventGraphException += new ucEventGraph.ExceptionHandler(ucEventGraph_EventGraphException);
                this.ucEventGraph0.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph1.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph2.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph3.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph4.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph5.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph6.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucEventGraph7.DownloadComplete += new ucEventGraph.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.ucLiveData1.DownloadComplete += new ucLiveData.DownloadCompleteHandler(ucEventGraph_DownloadComplete);
                this.radioButtonEvent0.Checked = true;
            }
            catch (Exception ex)
            {
                this.messageHandler("Error In Initialization", ex);
            }
            try
            {
                this.setComPortMenu();
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Port Menu", ex);
            }
            try
            {
                this.timerSCITimeOut.Interval = 1000;
                this.timerCheckPortTime.Interval = 1000;
                this.domainUpDownCTRatio.SelectedIndex = 2;  //1600:5

                #if DEBUG
                this.testVersion = false;
                this.buttonForceI.Visible = true;
                this.buttonSaveCalibration.Visible = true;
                this.ucCalibration1.Visible = true;
                //this.buttonTest.Visible = true;
                this.buttonUpdateDisplay.Visible = true;
                this.enableAll(true);
                this.tabPageFlightRecorder.Show();
                this.tabPagePortMonitor.Show();
                this.tabPageEvents.Show();
                this.Text = "Digital Grid Inc. - Relay Control and Monitoring Engineering 2010-03-23";
                this.eventActionsToolStripMenuItem.Enabled = false;
                this.liveDataActionsToolStripMenuItem.Enabled = false;
                
                #else
                this.testVersion = false;
                this.pauseMonitoring = false;
                this.ucCalibration1.Visible = false;
                //this.buttonTest.Visible = false;
                this.buttonUpdateDisplay.Visible = false;
                this.enableAll(false);
                //this.tabControlMain.TabPages.Remove(this.tabPageEvents);
                this.tabControlMain.TabPages.Remove(this.tabPagePortMonitor);
                //this.tabControlMain.TabPages.Remove(this.tabPageFlightRecorder);
                this.tabControlMain.TabPages.Remove(this.tabPageEngineering);
                this.tabControlMain.TabPages.Remove(this.tabPageSineGraphs);
                this.labelCtRatioMonitor.Visible = false;
                this.domainUpDownCTRatio.Visible = false;
                this.buttonForceI.Visible = false;
                this.buttonSaveCalibration.Visible = false;
                this.buttonUpdateCTRatio.Visible = false;
                this.buttonBlockRelayClosedState.Visible = false;
                this.buttonCalHigh.Visible = false;
                //this.buttonRequestRelayParamaters.Visible = false;
                this.buttonRequestRelayRegisters.Visible = false;
                this.buttonResetMaster.Visible = false;
                this.buttonRQRelayProcVersion.Visible = false;
                this.buttonSaveCalibration.Visible = false;
                this.buttonStartCal.Visible = false;
                this.buttonUnblockClosed.Visible = false;
                this.buttonUpdateDisplay.Visible = false;
                this.panel1.Size = new Size(112, this.panel1.Size.Height);
                this.domainUpDownCTRatioM.Items.RemoveAt(7);
                this.Text = "Digital Grid Inc. - Relay Control and Monitoring";
                this.labelByteCount.Visible = false;
                this.enableAllToolStripMenuItem.Visible = false;
                this.labelRelayType.Visible = false;
                this.Text = "Digital Grid Inc. - Relay Control and Monitoring 2010-03-23";
                this.textBoxCTRatio.Visible = false;
                this.acknowledgeToolStripMenuItem1.Visible = false;
                this.checkBoxBlockedCloseFlag.Visible = false;
                this.checkBoxCalibrating.Visible = false;
                this.checkBoxInInsensRegion.Visible = false;
                //this.checkBoxInTripRegion.Visible = false;
                this.panel1.Visible = false;
                this.label2.Visible = false;

                #endif

            }
            catch (Exception ex)
            {
                this.messageHandler("Error in Timer and Release Visibility", ex);
            }
            try
            {
                if(!this.testVersion)
                    this.checkSavedLocationAndFindRelay();
            }
            catch (Exception ex)
            {
                this.messageHandler(ex.Message, ex.InnerException);
            }
            
            this.timerRegisterPolling = new System.Windows.Forms.Timer();
            this.timerRegisterPolling.Interval = 1500;
            this.timerRegisterPolling.Enabled = true;
            this.timerRegisterPolling.Tick += new EventHandler(timerRegisterPolling_Tick);

        }

        private System.Timers.Timer timerLiveEventAcknowledge = new System.Timers.Timer();
        void ucEventGraph_DownloadComplete()
        {
            if(this.downloadProgress != null)
                this.downloadProgress.Dispose();

            this.downloadingLiveData = false;
            this.timerTimeOutCountdown.Enabled = false;
            this.timerLiveEventAcknowledge.Enabled = false;
            this.acknowledge();
            this.toolStripStatusLabelMain.Text = "Ready";

            this.requestLiveDataToolStripMenuItem1.Enabled = true;
            this.buttonRQEventData.Enabled = true;
            this.enableAll(true);
            this.monitoring(true);
            this.RegisterPolling(true);
        }

        void ucEventGraph_EventGraphException(Exception ex)
        {
            this.messageHandler("Error In Event Graph", ex);
        }

        void ucLiveData1_Error(Exception ex, string s)
        {
            this.messageHandler("Error In Live Data", ex);
        }

        void ucTransmitter1_TransmitterException(Exception ex)
        {
            this.messageHandler("Error in Transmitter Control", ex);
        }

        void ucTripMode2_TripControlException(Exception ex)
        {
            this.messageHandler("Error In Trip Control", ex);
        }

        void ucCloseMode1_CloseControlException(Exception ex)
        {
            this.messageHandler("Error In Close Control", ex);
        }

        void ucPumpMode1_PumpControlException(Exception ex)
        {
            this.messageHandler("Error In Pump Control", ex);
        }

        private void MainControl_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
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
                List<string>portNames = this.setComPortMenu();
            }
            catch (Exception ex)
            {
                this.RegisterPolling(false);
                this.monitoring(false);
                throw new Exception("Error Setting Port Names", ex);
            }
            try
            {
                List<string> portNames = this.setComPortMenu();
                this.threadFindRelay = new Thread(new ParameterizedThreadStart(this.findRelay));
                this.threadFindRelay.IsBackground = true;
                this.threadFindRelay.Start(portNames);
            }
            catch (Exception ex)
            {
                this.RegisterPolling(false);
                this.monitoring(false);
                throw new Exception("Error Starting Find Thread", ex);
            }
        }

        public bool testVersion = false;
        private bool allEnabled = false;
        private delegate void enableTabControlCallBack(bool b);
        
        private void enableAll(bool b)
        {
            try
            {
                if(this.tabControlMain.InvokeRequired)
                {
                    enableTabControlCallBack eTCB = new enableTabControlCallBack(enableAll);
                    this.Invoke(eTCB, new object[] {b});
                }
                else
                {
                    if(!b)
                        this.tabControlMain.TabIndex = 0;
                    this.tabControlMain.Enabled = b;
                    //this.cOMPortToolStripMenuItem.Enabled = b;
                    this.allEnabled = b;
                    this.timerMonitorRate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Enabling All Controls", ex);
            }
        }

        void ucTransmitter1_Send(SendEventArgs sEA)
        {
            if(sEA.SendPacket[0] == (byte)'X')  //requestPacket
                this.sendPacket(sEA.SendPacket);
            else
            {
                this.sendPacketAck(sEA.SendPacket);

            //this.buttonRequestRelayRegisters_Click(this, new EventArgs());
                this.buttonRequestRelayParamaters_Click(this, new EventArgs());
                this.parametersLoaded = true;
            }
        }

        void ucPumpMode1_Send(SendEventArgs sEA)
        {
            this.sendPacketAck(sEA.SendPacket);
            //this.sendPacket(sEA.SendPacket);

            if(sEA.SendPacket[1] < 2 && !this.sendAll)
            {
                this.buttonRequestRelayRegisters_Click(this, new EventArgs());
                this.buttonRequestRelayParamaters_Click(this, new EventArgs());
                this.parametersLoaded = true;
            }

        }


        void ucCalibration1_Send(object sender, SendEventArgs sEA)
        {
            this.sendPacket(sEA.SendPacket);
        }

        void ucTripMode2_Send(SendEventArgs sEA)
        {
            this.sendPacketAck(sEA.SendPacket);
            //this.sendPacket(sEA.SendPacket);
            if(sEA.SendPacket[0] == 0x4D && !this.sendAll)
            {
                this.buttonRequestRelayRegisters_Click(this, new EventArgs());
                this.buttonRequestRelayParamaters_Click(this, new EventArgs());
                this.parametersLoaded = true;
            }
        }

        
        void ucCloseMode1_Send(object sender, SendEventArgs sEA)
        {
            this.sendPacketAck(sEA.SendPacket);
            //this.sendPacket(sEA.SendPacket);
            if(sEA.SendPacket[0] == 0x4D  && !this.sendAll)
            {
                this.buttonRequestRelayRegisters_Click(this, new EventArgs());
                this.buttonRequestRelayParamaters_Click(this, new EventArgs());
                this.parametersLoaded = true;
            }
        }

        private Point PanelLocation = new Point(300, 12);
        private RelayControlLibrary.RelayMode relayMode = new RelayControlLibrary.RelayMode();

        /*
        private PointF[] sinWaveF(int arraySize, float amplitude, int phase)
        {
            PointF[] returnArray = new PointF[arraySize];

            for (int i = 0; i < arraySize; ++i)
            {
                int iAdjusted = (i + phase) % arraySize;
                double angle = ((double)iAdjusted / (double)arraySize) * 360d;
                double radians = Math.PI * angle / 180d;
                returnArray[i].X = i;
                returnArray[i].Y = (float)Math.Sin(radians) * 100f;
            }

            return returnArray;
        }
        */
        private string[] TripCurveNames = new string[3] { "Angle Offset", "Magnitude", "No Curve" };
        private string[] CloseCurveNames = new string[2] { "Vertical", "Horizontal" };

        private RelayControlLibrary.TripCurveDefinition[] TripCurveDefinitions = new RelayControlLibrary.TripCurveDefinition[4];

        public bool SendConfirmed = true;

        private void buttonUnblockOpen_Click(object sender, EventArgs e)
        {
            this.sendBlockStated(false);
        }

        private void buttonBlockedState_Click(object sender, EventArgs e)
        {
            this.sendBlockStated(true);
            
        }

        private void sendBlockStated(bool b)
        {
            if(b)
                this.sendPacketAck(RelayModeFunctions.BytePacketFor(BlockModes.Blocked));
                //this.sendPacket(RelayModeFunctions.BytePacketFor(BlockModes.Blocked));
            else
                this.sendPacketAck(RelayModeFunctions.BytePacketFor(BlockModes.Unblocked));
                //this.sendPacket(RelayModeFunctions.BytePacketFor(BlockModes.Unblocked));

            this.requestRelayRegisters();
        }

        private byte[] receiveArray = new byte[1000];
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
            finally
            {
                //this.resetCommunicationInterface();
            }
            
        }

        private int temp = 0;
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

        private void dataReceived()
        {
            byte lastByte = 0;
            bool dReceived = false;
            
            try
            {
                while (temp < 209 && this.serialPort1.BytesToRead > 0)
                {
                    this.SendConfirmed = true;
                    lastByte = this.receiveArray[this.rXWritePtr] = (byte)this.serialPort1.ReadByte();
                    //if(this.monitorPort)
                        //this.updatePortMonitorBox(lastByte, Color.Blue);
                    if (lastByte == 0x06)// && this.expectingAck)
                    {
                        this.SendConfirmed = true;
                        this.expectingAck = false;
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
                if(!this.readSemaphoreTaken || dReceived)//lastByte == 0x0D )
                {
                    temp = 0;
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

        private void checkRawData(object sender, EventArgs e)
        {
            IncomingCommCommands command = IncomingCommCommands.Invalid;
            int packetSize = 0;
            int tempRXReadPtr = 0;
            int initialRXPtr = 0, commandAddress = 0;
            
            this.readSemaphoreTaken = true;
            try
            {
                while(this.rXReadPtr != this.rXWritePtr)
                {
                    command = IncomingCommCommands.Invalid;
                    
                    while(true)
                    {

                        initialRXPtr = tempRXReadPtr = this.rXReadPtr;
                        //check to see if we have found a command or we have reached the end of the data
                        command = this.getCommand(this.receiveArray[tempRXReadPtr]);
                        while(command == IncomingCommCommands.Invalid)
                        {
                            tempRXReadPtr = this.nextRXArrayAddress(tempRXReadPtr);
                            if(tempRXReadPtr == this.rXWritePtr)
                            {
                                this.readSemaphoreTaken = false;
                                
                                return;
                            }
                            command = this.getCommand(this.receiveArray[tempRXReadPtr]);
                            
                        }
                        commandAddress = tempRXReadPtr;

                        tempRXReadPtr = this.nextRXArrayAddress(tempRXReadPtr);
                        if(tempRXReadPtr == this.rXWritePtr)
                        {
                            this.rXReadPtr = initialRXPtr;
                            return;
                        }

                        //check for special case of revision which has no length byte
                        if(command == IncomingCommCommands.Revision)
                        {
                            packetSize = 37;

                            if((char)this.receiveArray[tempRXReadPtr] != 'E')
                            {
                                this.rXReadPtr = this.nextRXArrayAddress(initialRXPtr);
                                command = IncomingCommCommands.Invalid;
                                break;
                            }
                        }
                        else if(command == IncomingCommCommands.FPGARevision)
                        {
                            packetSize = 20;
                            if((char)this.receiveArray[tempRXReadPtr] != 'P')
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
                            if(!this.lengthValid(command, packetSize))
                            {
                                //If the length is not valid it means it was not a valid command
                                //so we will check the next address
                                this.rXReadPtr = this.nextRXArrayAddress(initialRXPtr);//this.nextRXArrayAddress(this.rXReadPtr);
                                command = IncomingCommCommands.Invalid;
                                break;
                            }

                        }

                        if(!allPacketDataReceived(packetSize, tempRXReadPtr))
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

                    if(command != IncomingCommCommands.Invalid)
                    {
                        
                        byte[] packet = new byte[packetSize];

                        //TAKEN OUT FOR TEST!!!!!
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
            finally
            {
                //this.resetCommunicationInterface();
            }
        }

        private bool lengthValid(IncomingCommCommands c, int i)
        {
            try
            {
                switch (c)
                {
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
                        if (i == 48)
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
                        if (i == 88)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.RelayRegisters:
                        if (i == 6)
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
                        if (i == 30)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.TripOrCloseEvent:
                        if(i == 1)
                            return true;
                        else
                            return false;
                    case IncomingCommCommands.Invalid:
                    default:
                        throw new Exception("Bad command to check length for");

                }
            }
            catch //(Exception ex)
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
            catch //(Exception ex)
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
            this.expectingRelayParameters = false;
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
                    if(tempRXReadPtr == this.rXWritePtr)
                    {
                        return false;
                    }
                    temp = this.receiveArray[tempRXReadPtr];
                }
                if(temp == 0x0D || temp == 0x0A)
                {
                    return true;
                }
                else
                    return false;
            }
            catch //(Exception ex)
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
            switch(value)
            {
                case (byte)'c':
                    return IncomingCommCommands.CalibrationComplete;
                case (byte)'E':
                    return IncomingCommCommands.TripOrCloseEvent;
                case (byte)'f':
                    return IncomingCommCommands.CalibrationConstants;
                case(byte)'F':
                    return IncomingCommCommands.FPGARevision;
                case (byte)'g':
                    return IncomingCommCommands.EventTimes;
                case (byte)'H':
                    return IncomingCommCommands.SineGraphValue;
                case (byte)'i':
                    return IncomingCommCommands.EventDataPacket;
                case (byte)'j':
                    return IncomingCommCommands.CurrentTime;
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
                case (byte)'t':
                    return IncomingCommCommands.Temperature;
                case (byte)'Y':
                    return IncomingCommCommands.TransmitterSettings;
                case (byte)'x':
                    return IncomingCommCommands.LiveDataPacket;
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

                    //TAKEN OUT FOR TEST
                    //receiveArray[index] = 0;
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

            switch(command)
            {   
                case IncomingCommCommands.CalibrationComplete:
                    this.calibrationComplete(bytePacket);
                    break;
                case IncomingCommCommands.CalibrationConstants:
                    this.setCalibrationConstants(bytePacket);
                    break;
                case IncomingCommCommands.CurrentTime:
                    this.storeCurrentTime(bytePacket);
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
                case IncomingCommCommands.RelayRevision:
                    this.setRelayRevisionLabel(bytePacket);
                    break;
                case IncomingCommCommands.Revision:
                    this.revisionReceived(bytePacket);
                    break;
                case IncomingCommCommands.FPGARevision:
                    this.setFPGARevision(bytePacket);
                    break;
                case IncomingCommCommands.SineGraphValue:
                    this.setSineGraphValue(bytePacket);
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
                case IncomingCommCommands.Invalid:
                default:
                    throw new Exception("bad command for function call");
            }
        }




        #region Flight Recorder Section

        private DateTime savedCurrentTime;
        //private DateTime timeDifference;
        private long intTimeDifference;

        //private Int32[] CalibrationConstats = new Int32[8];
        private CalibrationConstant[] calibrationConstants = new CalibrationConstant[12];

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
            DateTime tempDT = DateTime.Now;
            long temp = bytePacket[0];
            temp <<= 8;
            temp += bytePacket[1];
            temp <<= 8;
            temp += bytePacket[2];
            temp <<= 8;
            temp += bytePacket[3];

            this.savedCurrentTime = RelayModeFunctions.DateFrom(temp);
            this.intTimeDifference = RelayModeFunctions.BinaryDate(this.savedCurrentTime) - temp;
        }
        
        delegate void bytePacketCallback(byte[] bytePacket);

        private void storeTimesReceived(byte[] bytePacket)
        {
            if(this.InvokeRequired)
            {
                bytePacketCallback bPCB = new bytePacketCallback(storeTimesReceived);
                this.Invoke(bPCB, new object[] { bytePacket });
            }
            else
            {
                this.initialLiveEventRequest = false;
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

                if (eBT.SystemTime != ucEventGraph0.EventTime)
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

                if (eBT.SystemTime != ucEventGraph1.EventTime)
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

                if (eBT.SystemTime != ucEventGraph2.EventTime)
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
                if (eBT.SystemTime != ucEventGraph3.EventTime)
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
                if (eBT.SystemTime != ucEventGraph4.EventTime)
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
                if (eBT.SystemTime != ucEventGraph5.EventTime)
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
                if (eBT.SystemTime != ucEventGraph6.EventTime)
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
                if (eBT.SystemTime != ucEventGraph7.EventTime)
                {
                    this.ucEventGraph7.ClearAllGraphs();
                    this.ucEventGraph7.EventTime = eBT.SystemTime;
                    this.ucEventGraph7.Type = this.getEventType(bytePacket[46], bytePacket[47]);
                }

                if((this.radioButtonEvent0.Checked && this.ucEventGraph0.Type == EventTypes.NoEvent) ||
                    (this.radioButtonEvent1.Checked && this.ucEventGraph1.Type == EventTypes.NoEvent) ||
                    (this.radioButtonEvent2.Checked && this.ucEventGraph2.Type == EventTypes.NoEvent) ||
                    (this.radioButtonEvent3.Checked && this.ucEventGraph3.Type == EventTypes.NoEvent) ||
                    (this.radioButtonEvent4.Checked && this.ucEventGraph4.Type == EventTypes.NoEvent) ||
                    (this.radioButtonEvent5.Checked && this.ucEventGraph5.Type == EventTypes.NoEvent) ||
                    (this.radioButtonEvent6.Checked && this.ucEventGraph6.Type == EventTypes.NoEvent) ||
                    (this.radioButtonEvent7.Checked && this.ucEventGraph7.Type == EventTypes.NoEvent))
                {
                    this.downloadProgress_Done(false, "No Event To Download");
                }
                this.requestCalibrationConstants();
            }
        }

        private EventTypes getEventType(byte lsB, byte msB)
        {
            UInt16 temp = msB;

            temp <<= 8;
            temp += lsB;

            if((temp & 1) == 1)
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
            else
            {
                return EventTypes.NoEvent;
            }
            /*
            switch(temp)
            {
                case 0:
                    return EventTypes.Trip;
                case 1:
                    return EventTypes.Close;
                case 2:
                    return EventTypes.Float;
                case 3:
                    return EventTypes.InInsensitiveRegion;
                case 4:
                    return EventTypes.Transient;
                default:
                    return EventTypes.NoEvent;
            }
            */
        }

        private CalibrationConstant CalVnA;
        private CalibrationConstant CalVnB;
        private CalibrationConstant CalVnC;
        private CalibrationConstant CalVtA;
        private CalibrationConstant CalVtB;
        private CalibrationConstant CalVtC;
        private CalibrationConstant CalIA;
        private CalibrationConstant CalIB;
        private CalibrationConstant CalIC;

        private bool downloadingCanceled = false;

        private void setCalibrationConstants(byte[] bytePacket)
        {
            this.timerLiveEventAcknowledge.Enabled = false;
            this.initialLiveEventRequest = false;
            for(int i = 0; i < 12; ++i)
            {
                if(this.calibrationConstants[i] == null)
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
            this.CalIA = this.calibrationConstants[9];
            this.CalIB = this.calibrationConstants[10];
            this.CalIC = this.calibrationConstants[11];

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

            if(!this.downloadingLiveData)
                this.requestEventData();
            else 
            {
                this.acknowledge();
                this.timerLiveEventAcknowledge.Start();
            }
            
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
            
            /*
            while(this.portClosing)
            {
                Application.DoEvents();
            }
            */
            if(this.portClosing)
                return;

            this.fromAckTest = true;
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
            string tempPortName = this.serialPort1.PortName;
            int baudRate = this.serialPort1.BaudRate;

            this.timerLiveEventAcknowledge.Enabled = false;
            if(this.portClosing == true)
                return;
            this.portClosing = true;

            
            //this.closePort = new Thread(new ThreadStart(closePortThread));

            //this.closePort.Start();

            this.serialPort1.DiscardInBuffer();
            this.serialPort1.DiscardOutBuffer();
            /*
            while(this.portClosing)
            {
                Application.DoEvents();
            }
            /*
            this.serialPort1.DiscardInBuffer();
            this.serialPort1.DiscardOutBuffer();

            this.serialPort1.DataReceived -= this.serialPort1_DataReceived;

            this.serialPort1.Close();
            this.serialPort1.Dispose();

            GC.Collect();
            */
            this.portClosing = false;
            if(serialPort1 == null)
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
            if(myPort != null && myPort.IsOpen)
            {
                myPort.DiscardInBuffer();
                myPort.DiscardOutBuffer();
            }
        }

        private void closePortThread()
        {
            try
            {
                if(this.serialPort1 != null)
                {
                    //this.serialPort1.DataReceived -= this.serialPort1_DataReceived;

                    this.serialPort1.DiscardInBuffer();
                    this.serialPort1.DiscardOutBuffer();
                    /*
                    this.serialPort1.Close();
                    this.serialPort1.Dispose();
                    this.serialPort1 = null;
                    GC.Collect();
                     * */
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

        private int testPortClosedCount = 0;

        private void portClosed(object sender, EventArgs e)
        {
            this.timerLiveEventAcknowledge.Enabled = false;
            this.testPortClosedCount++;
            this.portClosing = false;
        }

        private bool lastDataIn = false;
        private void setSineGraphValue(byte[] bytePacket)
        {
            float value;
            Int32 index, temp;
            PhasorTypes pT;

            try
            {
                pT = RelayModeFunctions.PhasorTypeFrom((char)bytePacket[0], (char)bytePacket[1]);
                if(pT == PhasorTypes.None)
                {
                    this.resetCommunicationInterface();
                    return;
                }
                index = bytePacket[3];
                temp = bytePacket[4];
                temp <<= 8;
                temp += bytePacket[5];
                
                if(temp >= 0x8000)
                {
                    temp = unchecked ((Int32)0xFFFF0000 + temp);
                }
                
                if(pT == PhasorTypes.VnA || pT == PhasorTypes.VnB || pT == PhasorTypes.VnC ||
                    pT == PhasorTypes.VdA || pT == PhasorTypes.VdC || pT == PhasorTypes.VdB)
                {
                    value = (float)temp * (float)Constants.SixFracBits;
                    this.ucSineGraph1.AddValueV(value, pT, index);
                }
                else
                {
                    value = (float)temp * (float)Constants.TenFracBits;
                    this.ucSineGraph1.AddValueI(value, pT, index);
                    if(pT == PhasorTypes.IC && index == 127)
                        this.lastDataIn = true;
                }
            }
            catch
            {
            }
            
        }

        

        private void setRelayRevisionLabel(byte[] bytePacket)
        {
            try
            {
                string temp;
                this.expectingRelayRevision = false;
                temp = ASCIIEncoding.ASCII.GetString(bytePacket);
                //revision = revision.Remove(revision.Length) - 3);

                this.setLabelText(temp, this.labelRelayRevision);
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Relay Revision Label", ex);
            }
            //this.loadedRevision = true;
        }

        private void setTransmitterSettings(byte[] bytePacket)
        {
            try
            {
                byte[] settings = new byte[30];
                this.expectingTransmitterSettings = false;

                

                for (int i = 0; i < settings.Length; ++i)
                {
                    settings[i] = bytePacket[i];
                }

                this.CTRatio = ((Int16)settings[7]) << 8;
                this.CTRatio += settings[6];

                this.updateCTRatioDomain(this.CTRatio, this.domainUpDownCTRatio);
                this.updateCTRatioDomain(this.CTRatio, this.domainUpDownCTRatioM);
                this.textBoxCTRatio.Text = this.CTRatio.ToString();

                this.ucTransmitter1.SetAllValues(settings);

                
            }
            catch (Exception ex)
            {
                this.messageHandler("Error In Transmitter Settings", ex);
                
            }
            //this.loadedTransmitterSettings = true;
        }

        private void updateCTRatioDomain( int CT_ratio, DomainUpDown dUP)
        {
            switch(CT_ratio)
            {
                case 160:
                    this.setDomainIndex(0, dUP);
                    break;
                case 240:
                    this.setDomainIndex(1, dUP);
                    break;
                case 320:
                    this.setDomainIndex(2, dUP);
                    break;
                case 400:
                    this.setDomainIndex(3, dUP);
                    break;
                case 500:
                    this.setDomainIndex(4, dUP);
                    break;
                case 600:
                    this.setDomainIndex(5, dUP);
                    break;
                case 700:
                    this.setDomainIndex(6, dUP);
                    break;
                default:
                    this.setDomainIndex(7, dUP);
                    break;
            }
        }

        private delegate void setDomainIndexCallBack(int i, DomainUpDown dUP);

        private void setDomainIndex(int i, DomainUpDown dUP)
        {
            try
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
            catch (Exception ex)
            {
                this.messageHandler("Error Invoking Domiain Index: " + dUP.Name, ex);
            }

        }
        
        private delegate void showLabelCallBack(bool b, Label l);

        private void showLabel(bool b, Label l)
        {
            try
            {
                if(l.InvokeRequired)
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
                this.messageHandler("Error Invoking Label Visibility on: " + l.Name, ex);
            }
        }

        private RelayStatusRegister RelayStatus = new RelayStatusRegister();
        private RelayFlagsRegister RelayFlags = new RelayFlagsRegister();
        private void setRelayRegisters(byte[] bytePacket)
        {
            this.registersReceived = true;
            this.expectingRelayRegisters = false;
            this.showLabel(false, this.labelRelayDisconnected);
            this.enableFlagsAndStatus(true);
            try
            {
                byte b = bytePacket[0];
                if ((b & 128) == 128)
                {
                    this.setCheckedValue(true, this.checkBoxInTripRegion);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxInTripRegion);
                }
                if ((b & 64) == 64)
                {
                    this.setCheckedValue(true, this.checkBoxInInsensRegion);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxInInsensRegion);
                }
                if ((b & 32) == 32)
                {
                    this.setCheckedValue(true, this.checkBoxBFlag);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxBFlag);
                }
                if ((b & 16) == 16)
                {
                    this.setCheckedValue(true, this.checkBoxTest2);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxTest2);
                }
                if((b & 8) == 8)
                {
                    this.setCheckedValue(true, this.checkBoxTest1);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxTest1);
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
                /*
                if(RelayStatus.ACB)
                {
                    this.setDomainIndex(1, this.domainUpDownPhasings);
                }
                else
                {
                    this.setDomainIndex(0, this.domainUpDownPhasings);
                }
                */

                b = bytePacket[1];
                if((b & 128) == 128)
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
                }
                else
                {
                    RelayStatus.BadOffset = false;
                }
                this.setCheckedValue(RelayStatus.BadOffset, this.checkBoxBadOffset);
                if ((b & 4) == 4)
                {
                    RelayStatus.OffsetOkay = true;
                }
                else
                {
                    RelayStatus.OffsetOkay = false;
                }
                this.setCheckedValue(RelayStatus.OffsetOkay, this.checkBoxOffsetOkay);
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
                }
                else
                {
                    RelayStatus.Pumping = false;
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
                }
                else
                {
                    RelayFlags.PhasingOkay = false;
                }
                this.setCheckedValue(RelayFlags.PhasingOkay, this.checkBoxPhasingOkayFlag);

                if ((b & 32) == 32)
                {
                    RelayFlags.BlockedOpen = true;
                    this.setLabelText("Blocked Open", this.labelBlockedOpenState);
                }
                else
                {
                    RelayFlags.BlockedOpen = false;
                    this.setLabelText("Unblocked Open", this.labelBlockedOpenState);
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
                }
                else
                {
                    RelayFlags.FloatCondition = false;
                }
                this.setCheckedValue(RelayFlags.FloatCondition, this.checkBoxFloatFlag);

                if ((b & 4) == 4)
                {
                    RelayFlags.Tripping = true;
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
                }
                else
                {
                    RelayFlags.Open = false;
                }
                this.setCheckedValue(RelayFlags.Open, this.checkBoxTripFlag);

                Int16 tripCount = bytePacket[3];
                
                this.setTextBox(tripCount.ToString(), this.textBoxTripCount);

                if(this.RelayFlags.Tripping || this.RelayFlags.Open)
                {
                    setLabelText("Open", this.labelRelayTrippedOrClose);
                    setBackgroundColor(Color.Green, this.labelRelayTrippedOrClose);
                    setLabelText("Open", this.labelRelayStateControlPage);
                    setBackgroundColor(Color.Green, this.labelRelayStateControlPage);
                    //this.ucPhasorGraph1.ClearAllLabels();
                }
                else if (this.RelayFlags.FloatCondition)
                {
                    setLabelText("Float", this.labelRelayTrippedOrClose);
                    setBackgroundColor(Color.Yellow, this.labelRelayTrippedOrClose);
                    setLabelText("Float", this.labelRelayStateControlPage);
                    setBackgroundColor(Color.Yellow, this.labelRelayStateControlPage);
                    //this.ucPhasorGraph1.ClearAllLabels();
                }
                else
                {
                    setLabelText("Closed", this.labelRelayTrippedOrClose);
                    setBackgroundColor(Color.Red, this.labelRelayTrippedOrClose);
                    setLabelText("Closed", this.labelRelayStateControlPage);
                    setBackgroundColor(Color.Red, this.labelRelayStateControlPage);
                    //this.ucPhasorGraph1.ClearAllLabels();
                }

                b = bytePacket[5];
                
                if ((b & 128) == 128)
                {
                    this.setCheckedValue(true, this.checkBoxMismatch);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxMismatch);
                }
                /*
                if ((b & 64) == 64)
                {
                    
                }
                else
                {
                    
                }
                */
                if ((b & 32) == 32)
                {
                    this.setCheckedValue(true, this.checkBoxCommFlagsRC);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxCommFlagsRC);
                }

                if ((b & 16) == 16)
                {
                    this.setCheckedValue(true, this.checkBoxCommFlagsInsensitiveBF);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxCommFlagsInsensitiveBF);
                }

                if ((b & 8) == 8)
                {
                    this.setCheckedValue(true, this.checkBoxCommFlagsBlocked);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxCommFlagsBlocked);
                }

                if ((b & 4) == 4)
                {
                    this.setCheckedValue(true, this.checkBoxCommFlagsPumping);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxCommFlagsPumping);
                }

                if ((b & 2) == 2)
                {
                    this.setCheckedValue(true, this.checkBoxCommFlagsClose);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxCommFlagsClose);
                }

                if ((b & 1) == 1)
                {
                    this.setCheckedValue(true, this.checkBoxCommFlagsTrip);
                }
                else
                {
                    this.setCheckedValue(false, this.checkBoxCommFlagsTrip);
                }
                
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Relay Register Values", ex);
            }
            //this.loadedRegisters = true;
            this.requestTemperature();
        }

        private void setTemperature(byte[] bytePacket)
        {
            if(this.InvokeRequired)
            {
                bytePacketCallback bPCB = new bytePacketCallback(this.setTemperature);
                this.Invoke(bPCB, new object[] { bytePacket });
            }
            else
            {
                Int16 temperature;

                temperature = bytePacket[1];
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

        private void setRelayParameters(byte[] bytePacket)
        {
            Int32 temp; 
            decimal tempM;
            double tempD;
            byte[] closePacket = new byte[11];
            bool badDataDetected = false;

            try
            {
                this.expectingRelayParameters = false;
                //Reclose Voltage Btyes - Vertical
                temp = bytePacket[1];
                temp <<= 8;
                temp += bytePacket[0];
                tempM = (decimal)temp * Constants.TwelveFracBits;
                closePacket[0] = bytePacket[0];
                closePacket[1] = bytePacket[1];
                
                this.setLabelText(String.Format("{0:0.00}", tempM) + " V", this.labelCloseRecloseVolts);
            
                //Tilt Angle Bytes - Vertical
                temp = bytePacket[3];
                temp <<= 8;
                temp += bytePacket[2];
                closePacket[2] = bytePacket[2];
                closePacket[3] = bytePacket[3];

                if(temp == 0)
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

                if(tempD < 0)
                {
                    tempD += 180;
                }
                this.setLabelText(String.Format("{0}", Math.Round(tempD, 0)) + " Degrees", this.labelCloseTilt);

                //Phasing Voltage Bytes - Horizontal
                temp = bytePacket[5];
                temp <<= 8;
                temp += bytePacket[4];
                tempM = (decimal)temp * Constants.TwelveFracBits;
                closePacket[4] = bytePacket[4];
                closePacket[5] = bytePacket[5];

                this.setLabelText(String.Format("{0:0.00}", tempM) + " V", this.labelClosePhaseDetectOffset);

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

                this.setLabelText(String.Format("{0}", Math.Round(tempD, 0)) + " Degrees", this.labelClosePhaseDetectAngle);

                //Close Mode
                if((char)bytePacket[8] == 'C')
                {
                    //this.ucCloseMode1.Mode = CloseModes.CircleClose;
                    closePacket[10] = (byte)'C';;
                    this.setLabelText("Circle Close", this.labelCloseMode);
                }
                else if ((char)bytePacket[8] == 'N')
                {
                    //this.ucCloseMode1.Mode = CloseModes.Normal;
                    closePacket[10] = (byte)'N';
                    this.setLabelText("Normal", this.labelCloseMode);
                }
                else
                    this.setLabelText("Error", this.labelCloseMode);

                
                //Close Time Delay Data
                temp = bytePacket[11];
                temp <<= 8;
                temp += bytePacket[10];

                this.setLabelText(temp.ToString() + " cycles" , this.labelCloseTimeDelay);
                closePacket[8] = bytePacket[10];
                closePacket[9] = bytePacket[11];
                this.ucCloseMode1.SetAllValues(closePacket);
            }
            catch (Exception ex)
            {
                badDataDetected = true;
                this.messageHandler("Error in Relay Close Data", ex);
                this.resetCloseData();
            }

            try
            {
                //Set the Trip Mode
                byte[] tripPacket = new byte[22];
                double tripAngle;
                TripModes tempTM;
                tempTM = RelayModeFunctions.TripModeFrom((char)bytePacket[14]);//8
                tripPacket[0] = bytePacket[14];

                //tempTM = TripModes.Sensitive;

                this.setLabelText(tempTM.ToString(), this.labelTripMode);

                //TimeDelay
                temp = bytePacket[17];//13
                temp <<= 8;
                temp += bytePacket[16];//12
                tripPacket[1] = bytePacket[16];
                tripPacket[2] = bytePacket[17];

                this.setLabelText(temp.ToString() + " s", this.labelTripTimeDelay);

                //ExtendedDelay
                temp = bytePacket[19];
                this.setLabelText(temp.ToString() + " s", this.labelExtendedTD);

                //SensitiveTripDelay
                temp = bytePacket[18];

                this.setLabelText(temp.ToString() + " cycles", this.labelTripSensitiveTimeDelay);
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
                tripPacket[5]   = bytePacket[22];
                tripPacket[6]   = bytePacket[23];
                tripPacket[11]  = bytePacket[30];
                tripPacket[12]  = bytePacket[31];

                tempD = (double)((decimal)temp * Constants.SixteenFracBits);
                tempD *= -1000d;
                this.setLabelText(String.Format("{0:0.0}", Math.Round(tempD, 1)) + " mA", this.labelTripSensitiveTrip);

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

                if(tempD < 0)
                    tempD += 180d;
                this.setLabelText(String.Format("{0:0}", Math.Round(tempD, 0)) + " Degrees", this.labelTripAngle);
                //Wing Trip (curve 1)
                //Insensitive Trip (IT), Mag of trip curve 3

                tripAngle = tempD;
                temp = bytePacket[55];
                temp <<= 8;
                temp += bytePacket[54];

                tripPacket[17] = bytePacket[54];
                tripPacket[18] = bytePacket[55];

                tempD = (double)((decimal)temp * Constants.TenFracBits);
                this.setLabelText(String.Format("{0:0.00}", Math.Round(tempD, 1)) + " A", this.label18);
                

                //Instantenous Current (IC), Magnitude of 4 trip curve
                temp = bytePacket[67];
                temp <<= 8;
                temp += bytePacket[66];
                tripPacket[9] = bytePacket[66];
                tripPacket[10] = bytePacket[67];


                tempD = (double)((decimal)temp * Constants.TenFracBits);

                this.setLabelText(String.Format("{0:0.00}", Math.Round(tempD, 1)) + " A", this.labelIC);
                
                
                //Watt-Var enable current

                tripPacket[13] = bytePacket[78];
                tripPacket[14] = bytePacket[79];

                temp = bytePacket[79];
                temp <<= 8;
                temp += bytePacket[78];

                tempD = (double)((decimal)temp * Constants.TenFracBits);
                this.setLabelText(String.Format("{0:0.00}", Math.Round(tempD, 1)) + " A", this.labelWattVarCurrent);

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

                this.setLabelText(String.Format("{0:0}", Math.Round(tempD, 0)) + " Degrees", this.labelWattVarAngle);

                

                //wing trip 
                tripPacket[19] = bytePacket[32];  //'O' or 'N'

                tripPacket[20] = bytePacket[36]; //Angle bytes
                tripPacket[21] = bytePacket[37];

                this.ucTripMode2.SetAllValue(tripPacket);

            }
            catch (Exception ex)
            {
                badDataDetected = true;
                this.messageHandler("Error in Relay Trip Setting Data", ex);                
                this.defaultTripSettings();
            }
            try
            {
                //ABC or ACB
                temp = bytePacket[80];
                if(temp == 2)
                {
                    this.setDomainIndex(2, this.domainUpDownPhasings);
                }
                else if(temp == 1)    
                {
                    this.setDomainIndex(1, this.domainUpDownPhasings);
                }
                else if (temp == 0)
                {
                    this.setDomainIndex(0, this.domainUpDownPhasings);
                }
                else
                {
                    throw new Exception(temp.ToString() + " is not a valid value for Phasing");
                }
            }
            catch (Exception ex)
            {
                badDataDetected = true;
                this.messageHandler("Phase Issue", ex);
            }
            try
            {
                //Power or Sequence
                temp = bytePacket[81];  //69
                if(temp == 'S')
                {   
                    this.setLabelText("Sequence", this.labelRelayType);
                    this.setDomainIndex(1, this.domainUpDownRelayType);
                }
                else if (temp == 'P')
                {
                    this.setLabelText("Power", this.labelRelayType);
                    this.setDomainIndex(0, this.domainUpDownRelayType);
                }
                else
                {
                    this.setLabelText("Error", this.labelRelayType);
                    throw new Exception("'" + Convert.ToChar(temp).ToString() + "' is not a valid Relay Type character.");
                }
            }
            catch (Exception ex)
            {
                badDataDetected = true;
                this.messageHandler("Error in Relay Type Data", ex);
                this.setDomainIndex(0, this.domainUpDownRelayType);
            }
            try
            {
                //Pump Mode Packet
                byte[] pumpPacket = new byte[5];
                pumpPacket[0] = bytePacket[82]; //70
                pumpPacket[1] = bytePacket[83];
                pumpPacket[2] = bytePacket[84];
                pumpPacket[3] = bytePacket[85];
                pumpPacket[4] = bytePacket[86];
                this.ucPumpMode1.SetAllValues(pumpPacket);
            }
            catch (Exception ex)
            {
                badDataDetected = true;
                this.messageHandler("Error Setting Pump Data", ex);
                this.defaultPumpData();
            }

            if(this.parametersLoaded  && badDataDetected == false)
            {
                this.parametersLoaded = false;
                this.messageHandler("Parameters Loaded", "Parameters Loaded Successfully");
            }
            else if(badDataDetected == true)
            {
                this.parametersLoaded = false;
                this.messageHandler("Error", "Parameters NOT Loaded Successfully");
            }
            //this.loadedParameters = true;
            
        }

        private void defaultPumpData()
        {
            try
            {
                byte[] pumpPacket = new byte[5];
                pumpPacket[0] = 0;                  //not enabled
                pumpPacket[1] = 3;                  //cycle limit
                pumpPacket[2] = 120;                  //pump time low byte
                pumpPacket[3] = 0;                //pump time high byte (x4)
                pumpPacket[4] = 15;                 //protect time
                this.ucPumpMode1.SetAllValues(pumpPacket);
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Default Pump Data", ex);            }
            }

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

        private string revision;

        private void revisionReceived(byte[] bytePacket)
        {
            try
            {
                revision = "R";
                revision += ASCIIEncoding.ASCII.GetString(bytePacket);
                //revision = revision.Remove(revision.Length) - 3);

                this.setLabelText(revision, this.labelRevision);
                this.relayFound = true;
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Label: " + this.labelRevision, ex);
            }
            finally 
            {
                //this.relayFound = true;
            }
        }

        private void setFPGARevision(byte[] bytePacket)
        {
            try
            {
                this.expectingFPGARevision = false;
                revision = "F";
                revision += ASCIIEncoding.ASCII.GetString(bytePacket);
                this.setLabelText(revision, this.labelFPGARevision);
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
                //this.ucPhasorGraph1.ClearAllLabels();
                switch (type)
                {
                    case 'T':
                        //if(this.labelRelayTrippedOrClose.Text == "Tripped")
                          //  this.labelRelayTrippedOrClose.Text = "Doh";
                        setLabelText("Open", this.labelRelayTrippedOrClose);
                        setBackgroundColor(Color.Green, this.labelRelayTrippedOrClose);
                        setLabelText("Open", this.labelRelayStateControlPage);
                        setBackgroundColor(Color.Green, this.labelRelayStateControlPage);
                        break;
                    case 'C':
                        //if (this.labelRelayTrippedOrClose.Text == "Closed")
                          //  this.labelRelayTrippedOrClose.Text = "Doh";
                        setLabelText("Closed", this.labelRelayTrippedOrClose);
                        setBackgroundColor(Color.Red, this.labelRelayTrippedOrClose);
                        setLabelText("Closed", this.labelRelayStateControlPage);
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

        private void setTextBox(string s, TextBox tB)
        {
            try
            {
                if(tB.InvokeRequired)
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

        private void setPhasorValue(byte[] packet)
        {
            long realValue;
            long imaginaryValue, rMS;
            PhasorTypes phasorType = PhasorTypes.None;
            
            phasorType = RelayModeFunctions.PhasorTypeFrom((char)packet[0], (char)packet[1]);

            try
            {
                if(phasorType != PhasorTypes.None)
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
                    switch (phasorType)
                    {
                        case PhasorTypes.VtA:
                        case PhasorTypes.VtB:
                        case PhasorTypes.VtC:
                        case PhasorTypes.VnA:
                        case PhasorTypes.VnB:
                        case PhasorTypes.VnC:
                        case PhasorTypes.VdA:
                        case PhasorTypes.VdB:
                        case PhasorTypes.VdC:
                        case PhasorTypes.VdT:
                        case PhasorTypes.VtN:
                        case PhasorTypes.VtP:
                        case PhasorTypes.VnP:
                        case PhasorTypes.VnN:
                        case PhasorTypes.VdN:
                        case PhasorTypes.VdP:
                            if(rMS < 0 || rMS > 0xFA000)
                            {
                                this.resetCommunicationInterface();
                                return;
                            }
                            break;
                        case PhasorTypes.IA:
                        case PhasorTypes.IB:
                        case PhasorTypes.IC:
                        case PhasorTypes.IN:
                        case PhasorTypes.IP:
                        case PhasorTypes.Ieff:
                            if (rMS < 0 || rMS > 0x140000)
                            {
                                this.resetCommunicationInterface();
                                return;
                            }
                            break;
                        case PhasorTypes.PA:
                        case PhasorTypes.PB:
                        case PhasorTypes.PC:
                        case PhasorTypes.PT:
                            if (rMS < 0 || rMS > 0x1388000)
                            {
                                this.resetCommunicationInterface();
                                return;
                            }
                            break;
                        case PhasorTypes.None:
                        default:
                            this.resetCommunicationInterface();
                            break;
                    }
                    this.ucPhasorGraph1.ValuesForUpdate(phasorType, realValue, imaginaryValue, this.CTRatio, rMS);
                    
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Phasor Value", ex);
            }
        }

        private void cOMPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tSMI = (ToolStripMenuItem)sender;
            
            if(this.serialPort1.IsOpen)
            {
                this.clearSerialPortBuffers(this.serialPort1);
                this.serialPort1.Close();
            }
            
            try
            {
                this.serialPort1.PortName = tSMI.Text;
                this.serialPort1.Open();
                this.clearSerialPortBuffers(this.serialPort1);
            }
            catch (Exception ex)
            {
                this.messageHandler("Something wrong with serial Port: " + this.serialPort1.PortName + ", Port not Open", ex);

                this.toolStripStatusLabelMain.Text = "Port Error";
                return;
            }
            try
            {
                if(this.testVersion)
                    return;
                this.toolStripStatusLabelMain.Text = this.serialPort1.PortName + " selected. - No Relay Found";
                this.relayFound = false;
                if(this.checkPortAvailability(tSMI.Text))
                    this.checkPortForRelay();

                if(this.relayFound)
                {
                    this.requestAllData();
                    this.toolStripStatusLabelMain.Text = "Relay Found on " + this.serialPort1.PortName;
                }
                else
                {
                    this.enableAll(false);
                    this.toolStripStatusLabelMain.Text = "No Relay Found on " + this.serialPort1.PortName;
                }

            }
            catch (Exception ex)
            {
                this.messageHandler("Error Accessing Tool String Status Label", ex);
            }
        }

        private void buttonUnblockClosed_Click(object sender, EventArgs e)
        {
            this.sendBlockClosedState(false);
        }



        private void buttonBlockRelayClosedState_Click(object sender, EventArgs e)
        {
            this.sendBlockClosedState(true);
        }

        private void sendBlockClosedState(bool b)
        {
            if(b)
                this.sendPacketAck(RelayModeFunctions.BytePacketFor(BlockModes.BlockedClosed));
                //this.sendPacket(RelayModeFunctions.BytePacketFor(BlockModes.BlockedClosed));
            else
                this.sendPacketAck(RelayModeFunctions.BytePacketFor(BlockModes.UnblockedClosed));
                //this.sendPacket(RelayModeFunctions.BytePacketFor(BlockModes.UnblockedClosed));

            this.requestRelayRegisters();

        }
        private int CTRatio = 320;

        private void domainUpDownCTRatio_SelectedItemChanged(object sender, EventArgs e)
        {
            DomainUpDown dUP = (DomainUpDown)sender;

            switch (dUP.Text)
            {
                case "800:5":
                    this.CTRatio = 160;
                    break;
                case "1200:5":
                    this.CTRatio = 240;
                    break;
                case "1600:5":
                    this.CTRatio = 320;
                    break;
                case "2000:5":
                    this.CTRatio = 400;
                    break;
                case "2500:5":
                    this.CTRatio = 500;
                    break;
                case "3000:5":
                    this.CTRatio = 600;
                    break;
                case "3500:5":
                    this.CTRatio = 700;
                    break;
                case "Special":
                    //this.CTRatio = 1;
                    break;
                default:
                    break;
            }
            this.ucPhasorGraph1.CTChanged(this.CTRatio);
            this.ucTripMode2.CTRatio = this.CTRatio;
            this.ucEventGraph0.CTRatio = this.CTRatio;
            this.ucEventGraph1.CTRatio = this.CTRatio;
            this.ucEventGraph2.CTRatio = this.CTRatio;
            this.ucEventGraph3.CTRatio = this.CTRatio;
            this.ucEventGraph4.CTRatio = this.CTRatio;
            this.ucEventGraph5.CTRatio = this.CTRatio;
            this.ucEventGraph6.CTRatio = this.CTRatio;
            this.ucEventGraph7.CTRatio = this.CTRatio;
            this.ucLiveData1.CTRatio = this.CTRatio;

            this.domainUpDownCTRatioM.SelectedIndex = dUP.SelectedIndex;
        }

        private void requestRelayRevision()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'Q';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;
            
            this.sendPacket(sendArray);
            if(this.expectingRelayRevision)
                this.timerSCITimeOut.Enabled = true;
            
        }

        private void requestFPGARevision()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'n';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);
            if (this.expectingRelayRevision)
                this.timerSCITimeOut.Enabled = true;
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
                this.timerMonitorRate.Interval = 3000;
                if (!this.pausePhasorMonitoring)  //this.timerMonitorRate.Enabled)
                {
                    //this.timerMonitorRate.Enabled = false;
                    this.pausePhasorMonitoring = true;
                    this.buttonToggleMonitor.Text = "Start Monitoring";
                }
                else
                {
                    this.pollMonitoring();
                    this.pausePhasorMonitoring = false;
                    //this.timerMonitorRate.Enabled = true;
                    this.buttonToggleMonitor.Text = "Stop Monitoring";
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error In Toggling Monitor Mode", ex);
            }
        }

        private bool pausePhasorMonitoring = false;
        private void monitoring(bool b)
        {
            try
            {
                this.pausePhasorMonitoring = !b;
                //this.timerMonitorRate.Interval = 2000;
                //this.timerMonitorRate.Enabled = b;
                
                if(!b)
                {
                    this.setButtonText("Start Monitoring", this.buttonToggleMonitor);
                }
                else
                {
                    this.timerMonitorRate.Enabled = true;
                    this.setButtonText("Stop Monitoring", this.buttonToggleMonitor);
                }
            }
            catch (Exception ex)
            {
                this.timerMonitorRate.Enabled = false;
                this.pausePhasorMonitoring = true;
                this.setButtonText("Start Monitoring", this.buttonToggleMonitor);
                this.messageHandler("Error In Actual Monitor Toggle", ex);
            }
        }

        private void pollMonitoring()
        {
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
                    this.Invoke(b, new object[] { s , l});
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
                if(this.InvokeRequired)
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
        
        private void timerSCITimeOut_Tick(object sender, EventArgs e)
        {
            try
            {
                this.monitoring(false);
                this.RegisterPolling(false);
                this.timerSCITimeOut.Enabled = false;
                this.timerMonitorRate.Enabled = false;
                this.setButtonText("Start Monitoring", this.buttonToggleMonitor);
                this.messageHandler("Last operation did not complete properly", new Exception("SCI Timeout"));
                this.RegisterPolling(true);
            }
            catch (Exception ex)
            {
                this.messageHandler("Error In Time Out", ex.Message);
            }
        }

        private void buttonRequestRelayParamaters_Click(object sender, EventArgs e)
        {
            this.requestRelayParameters();
        }

        private void requestRelayParameters()
        {
            byte[] sendArray = new byte[3];
            
            sendArray[0] = (byte)'S';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);
            if(this.expectingRelayParameters)
                this.timerSCITimeOut.Enabled = true;
            
            
        }

        private void requestTransmitterSettings()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'X';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);
            if(this.expectingTransmitterSettings)
                this.timerSCITimeOut.Enabled = true;
            
        }

        private void buttonRequestRelayRegisters_Click(object sender, EventArgs e)
        {
            this.requestRelayRegisters();
            this.requestTemperature();
        }

        private void requestRelayRegisters()
        {
            byte[] sendArray = new byte[3];

            sendArray[0] = (byte)'r';
            sendArray[1] = 0x55;
            sendArray[2] = 0x0D;

            this.sendPacket(sendArray);
            if(this.expectingRelayRegisters)
                this.timerSCITimeOut.Enabled = true;
            else
                this.timerRegisterPolling.Enabled = true;
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
            try
            {
                byte[] packet = new byte[4];

                packet[0] = (byte)'s';
                try
                {
                    if(this.domainUpDownRelayType.SelectedItem.ToString() == "Sequence")
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
                    if(this.domainUpDownPhasings.SelectedItem.ToString() == "ABC")
                        packet[2] = 0x00;
                    else if (this.domainUpDownPhasings.SelectedItem.ToString() == "ACB")
                        packet[2] = 0x01;
                    else if (this.domainUpDownPhasings.SelectedItem.ToString() == "AutoDetect")
                        packet[2] = 0x02;
                    else
                        throw new Exception(packet[2].ToString() + " is a bad Phasing");
                }
                catch
                {
                    this.messageHandler("No Phasing Selected", new Exception("Please Select Phasing"));
                    return;
                }
         

                packet[3] = 0x0D;
                this.sendPacketAck(packet);

                Thread.Sleep(100);
                if (!this.sendAll)
                {
                    this.buttonRequestRelayParamaters_Click(this, new EventArgs());
                    this.parametersLoaded = true;
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Setting Relay Type", ex);   
            }

        }

        private void buttonTypePhasingRestoreDefaults_Click(object sender, EventArgs e)
        {
            this.domainUpDownPhasings.SelectedIndex = 0;
            this.domainUpDownRelayType.SelectedIndex = 0;
            this.buttonRelayType_Click(this, new EventArgs());
        }

        private void buttonTestTimer_Click(object sender, EventArgs e)
        {
            this.timerSCITimeOut.Enabled = false;
        }

        private void timerMonitorRate_Tick(object sender, EventArgs e)
        {
            if(this.lastDataIn == true)
                this.readyToGetSineData = true;

            if(this.pausePhasorMonitoring || this.testVersion)
                return;
            this.pollMonitoring();
            //this.requestRelayRegisters();
            //this.buttonRequestRelayRegisters_Click(this, new EventArgs());
        }

        private bool pauseMonitoring = false;
        private void buttonForceI_Click(object sender, EventArgs e)
        {
            //this.sendSaveCalibration();
            //this.pauseMonitoring = !this.pauseMonitoring;
            
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
            this.setComPortMenu();
        }

        private List<string> setComPortMenu()
        {
            try
            {
                string[] tempPortNames = System.IO.Ports.SerialPort.GetPortNames();
                List<string> portNames = new List<string>();

                for(int j = 0; j < tempPortNames.Length; ++j)
                {
                    portNames.Add(tempPortNames[j]);
                }

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
        private bool relayCheckTimeOut = false;
        private void findRelay(object pN)
        {
            List<string> portNames = (List<string>)pN;
            string errorMessage = "Initial";

            try
            {
                errorMessage = "Error Retrieving Port From File";
                if(portNames.Contains(this.savedComPort))
                {
                    if(this.checkPortAvailability(this.savedComPort))
                    {
                        errorMessage = "Error Checking Port For Relay On Saved Port";
                        this.checkPortForRelay();
                    }
                }

                errorMessage = "Error Checking Additional Ports For Relay";
                if(!this.relayFound)
                {
                    foreach (string s in portNames)
                    {
                        if(this.checkPortAvailability(s))
                        {
                            this.checkPortForRelay();
                            
                            if(relayFound)
                            {
                                break;
                            }
                        }
                    }
                }

                errorMessage = "Error Saving Com Port and Requesting Data";
                if(this.relayFound)
                {
                    this.saveComPort();
                    errorMessage = "Error Requesting All Data";
                    this.requestAllData();
                    this.RegisterPolling(true);
                }
                else
                {
                    errorMessage = "Unable To Locate Relay.";
                    this.relayNotFound();
                    this.RegisterPolling(false);
                }
            }
            catch //(Exception ex)
            {
                this.messageHandler("No Relay Found", new Exception(errorMessage));
            }
        }

        private void RegisterPolling(bool p)
        {
            this.pauseMonitoring = !p;
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
            return true;
        }

        private void checkPortForRelay()
        {
            this.toolStripStatusLabelMain.Text = "Checking " + this.serialPort1.PortName + " for Relay";
            //this.relayFound = false;
            try
            {
                this.serialPort1.Open();
                this.clearSerialPortBuffers(this.serialPort1);
                if(!this.serialPort1.IsOpen)
                {
                    return;
                }
                for (int i = 0; i < 2; ++i)
                {
                    this.relayCheckTimeOut = false;
                    this.requestMasterRevisionNumber();
                    this.timerCheckPortTime.Dispose();
                    this.timerCheckPortTime = new System.Windows.Forms.Timer();
                    this.timerCheckPortTime.Tick += new EventHandler(timerCheckPortTime_Tick);
                    this.timerCheckPortTime.Interval = 1000;
                    //this.timerCheckPortTime.Enabled = true;
                    this.timerCheckPortTime.Start();

                    while (!this.relayFound && !this.relayCheckTimeOut)
                    {
                        Application.DoEvents();
                    }
                    if(this.relayFound)
                        break;
                }
                this.relayCheckTimeOut = false;
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Checking Port For Relay", ex);
            }
        }

        private void requestMasterRevisionNumber()
        {
            byte[] packet = new byte[3];

            packet[0] = (byte)'R';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacket(packet);
 
        }

        private void timerCheckPortTime_Tick(object sender, EventArgs e)
        {
            this.timerCheckPortTime.Enabled = false;
            this.relayCheckTimeOut = true;
        }

        private bool expectingRelayRevision = false;
        private bool expectingRelayParameters = false;
        private bool expectingTransmitterSettings = false;
        private bool expectingRelayRegisters = false;
        private bool expectingFPGARevision = false;

        private void requestAllData()
        {

            this.expectingRelayRevision = true;
            //this.loadedParameters = false;
            
            this.requestRelayRevision();
            
            
            /*
            while(this.expectingRelayRevision && this.timerSCITimeOut.Enabled)
            {
                Application.DoEvents();
            }
            */
            if(!this.timerSCITimeOut.Enabled)
            {
                this.messageHandler("Failed To Get Relay Revision Number.  Please Try Again.", new Exception("SCI Timed Out"));
                return;
            }
            this.timerSCITimeOut.Enabled = false;

            this.expectingFPGARevision = true;

            this.requestFPGARevision();


            if (!this.timerSCITimeOut.Enabled)
            {
                this.messageHandler("Failed To Get FPGA Revision Parameters.  Please Try Again.", new Exception("SCI Timed Out"));
                return;
            }

            this.timerSCITimeOut.Enabled = false;


            this.expectingRelayParameters = true;

            /*
            while (!this.loadedRevision)
            {
                Application.DoEvents();
            }
            */
            this.expectingRelayParameters = true;
            //this.loadedRegisters = false;
 
            this.requestRelayParameters();
            /*
            while (this.expectingRelayParameters && this.timerSCITimeOut.Enabled)
            {
                Application.DoEvents();
            }
            */
            if (!this.timerSCITimeOut.Enabled)
            {
                this.messageHandler("Failed To Get Relay Parameters.  Please Try Again.", new Exception("SCI Timed Out"));
                return;
            }

            

            this.timerSCITimeOut.Enabled = false;
            
            
            

            /*
            while (!this.loadedParameters)
            {
                Application.DoEvents();
            }
            */
            this.expectingRelayRegisters = true;
            //this.loadedRegisters = false;
            this.requestRelayRegisters();
            /*
            while (this.expectingRelayRegisters && this.timerSCITimeOut.Enabled)
            {
                Application.DoEvents();
            }
             * */
            if (!this.timerSCITimeOut.Enabled)//&& this.loadedRegisters == false)
            {
                this.messageHandler("Failed To Get Relay Registers.  Please Try Again.", new Exception("SCI Timed Out"));
                return;
            }
            this.timerSCITimeOut.Enabled = false;
            /*
            while (!this.loadedRegisters)
            {
                Application.DoEvents();
            }
            */
            Thread.Sleep(100);
            this.expectingTransmitterSettings = true;
            this.requestTransmitterSettings();
            /*
            while (this.expectingTransmitterSettings && this.timerSCITimeOut.Enabled)
            {
                Application.DoEvents();
            }
            */
            if (!this.timerSCITimeOut.Enabled)
            {
                this.messageHandler("Failed To Get Transmitter Settings.  Please Try Again.", new Exception("SCI Timed Out"));
                return;
            }
            this.timerSCITimeOut.Enabled = false;

            this.enableAll(true);
            this.toolStripStatusLabelMain.Text = "Relay found on " + this.serialPort1.PortName;

            this.sendTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
        }

        private void relayNotFound()
        {
            this.monitoring(false);
            this.RegisterPolling(false);
            this.toolStripStatusLabelMain.Text = "No Relay Found";
            this.threadFindRelay.Abort();
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
                this.relayCheckTimeOut = false;

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
        //private MyFile monitoringFile;
        private const string _monitorFilePath = @"C:\DGI Systems\Relay\Saved.txt";
        private const string _savedFilePath = @"C:\DGI Systems\Relay\Saved.txt";

        private string getSavedComPort(MyFile mF)
        {
            try
            {
                string comPort;
                
                comPort = mF.ReadWholeFile();
                
                if(comPort == "")
                {
                    comPort = "";   
                }
                else
                {
                    comPort = comPort.Remove(comPort.Length - 2, 2);
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

        //private bool comPortSaved = false;

        private void saveComPort()
        {
            this.savedFile.WriteWholeFile(this.serialPort1.PortName);
            //this.comPortSaved = true;
        }
        #endregion

        private void sendPacketAck(byte[] bytePacket)
        {
            bool sendSemaphore = false;
            string errorMessage = "None";
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
                    this.serialPort1.Dispose();
                    GC.Collect();
                    
                    this.serialPort1 = new MyPort(this.components);
                    this.serialPort1.PortName = comPort;
                    this.serialPort1.BaudRate = baudRate;
                    this.serialPort1.Open();
                    this.clearSerialPortBuffers(this.serialPort1);

                    //if(this.serialPort1 != null)
                    //    this.serialPort1.Open();
                    //this.messageHandler("Serial Port Closed", new Exception("Serial Port Closed"));
                    //return;
                }

                errorMessage = "Error Checking For Bytes left to read";
                /*
                while (this.serialPort1.BytesToWrite > 0 || sendSemaphore || this.SendConfirmed == false)
                {
                    Application.DoEvents();
                }
                 * */
                sendSemaphore = true;
                ++this.byteCount;



                errorMessage = "Error Writing To Port";
                this.serialPort1.Write(bytePacket, 0, bytePacket.Length);
                this.expectingAck = true;

                errorMessage = "Error At End of Routine";
                this.timerSCITimeOut.Enabled = true;
                /*
                while (this.expectingAck == true && this.timerSCITimeOut.Enabled == true)//(this.SendConfirmed == false && this.timerSCITimeOut.Enabled == true)
                {
                    Application.DoEvents();
                }
                 */
                this.timerSCITimeOut.Enabled = false;
                sendSemaphore = false;
                return;
            }
            catch (Exception ex)
            {
                this.messageHandler(errorMessage + ", Please Check Port.", ex);
            }
        }

        private Int32 sendPacketTestCount = 0;
        private byte[] savedPacketTest;
        private int savedPacketNumberTest = 1;
        private bool fromAckTest = false;

        private void sendPacket(byte[] bytePacket)
        {
            bool sendSemaphore = false;
            string errorMessage = "None";
            
            /*
            if(bytePacket[0] != 0x68 && bytePacket[0] != 0x6B && bytePacket[0] != 0x66 && bytePacket[0] != 0x06 && bytePacket[0] != 0x67)
                return;
             */
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
                    //this.serialPort1.Dispose();
                    GC.Collect();

                    this.serialPort1 = new MyPort(this.components);
                    this.serialPort1.PortName = comPort;
                    this.serialPort1.BaudRate = baudRate;
                    this.serialPort1.Open();

                    //this.clearSerialPortBuffers(this.serialPort1);
                    //if (this.serialPort1 != null)
                        
                    //this.messageHandler("Serial Port Closed", new Exception("Serial Port Closed"));
                    //return;
                }

                errorMessage = "Error Checking For Bytes left to read";
                /*
                while (this.serialPort1.BytesToWrite > 0 || sendSemaphore || this.SendConfirmed == false)
                {
                    Application.DoEvents();
                }
                */
                sendSemaphore = true;
                ++this.byteCount;



                errorMessage = "Error Writing To Port";
                this.serialPort1.Write(bytePacket, 0, bytePacket.Length);
                if(!this.fromAckTest)
                    this.fromAckTest = false;
                this.savedPacketTest = bytePacket;
                if(this.savedPacketNumberTest == this.savedIndexOffsetTest && bytePacket[0] == 0x06)
                    this.savedPacketNumberTest = 0;
                this.savedPacketNumberTest = this.savedIndexOffsetTest;
                this.sendPacketTestCount++;
                //this.expectingAck = true;

                errorMessage = "Error At End of Routine";
                this.timerSCITimeOut.Enabled = true;
                /*
                while (this.SendConfirmed == false && this.timerSCITimeOut.Enabled == true)
                {
                    Application.DoEvents();
                }
                 * */
                this.timerSCITimeOut.Enabled = false;
                sendSemaphore = false;
                return;
            }
            catch (Exception ex)
            {
                this.messageHandler(errorMessage + " Please Check Port.", ex);
            }
            
            if(this.monitorPort)
            {
                foreach (byte b in bytePacket)
                {
                    this.updatePortMonitorBox(b, Color.Red);
                }
            }    
        }

        private bool monitorPort = false;
        private delegate void updatePortBoxCallBack(byte b, Color c);

        private void updatePortMonitorBox(byte b, Color c)
        {
            try
            {
                if(this.richTextBoxPortMonitor.InvokeRequired)
                {
                    updatePortBoxCallBack uCB = new updatePortBoxCallBack(updatePortMonitorBox);
                    this.Invoke(uCB, new object[] { b , c });
                }
                else
                {
                    Int16 i = b;

                    string displayString = String.Format("{0000000:X}", b, c);
                    if(displayString.Length != 2)
                        displayString = "0" + displayString + " ";
                    else
                        displayString += " ";
                    
                    this.richTextBoxPortMonitor.Text += displayString;
                    this.richTextBoxPortMonitor.Select(this.richTextBoxPortMonitor.Text.Length - displayString.Length, displayString.Length);
                    this.richTextBoxPortMonitor.SelectionColor = c;
                        
                    
                }
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Invoking Port Monitor", ex);
            }
        }

        private void buttonPortMonitorReset_Click(object sender, EventArgs e)
        {
            this.richTextBoxPortMonitor.Text = "";
        }
        #region Calibration
        private void startCalibrationTimer()
        {
            this.activeStatusBarCountDown("Calibration Relay ", 60);
        }

        private void buttonStartCal_Click(object sender, EventArgs e)
        {
            byte[] packet = new byte[3];

            DialogResult dR = this.messageHandler("Calibration", "This may take a few moments to complete.\r\nCalibrate Unit?", MessageBoxButtons.YesNo);

            if(dR == DialogResult.Yes)
            {
                packet[0] = (byte)'c';
                packet[1] = 0x55;
                packet[2] = 0x0D;
            

                this.sendPacket(packet);
                //this.startCalibrationTimer();
                this.downloadingDialogCountDown("Calibrating ", "Calibration", 60);
            }
        }

        

        private void buttonCalHigh_Click(object sender, EventArgs e)
        {
            byte[] packet = new byte[3];
            DialogResult dR = this.messageHandler("Calibration", "This may take a few moments to complete.\r\nCalibrate Unit?", MessageBoxButtons.YesNo);
            
            if(dR == DialogResult.Yes)
            {
                packet[0] = (byte)'m';
                packet[1] = 0x55;
                packet[2] = 0x0D;
            
                this.sendPacket(packet);
                //this.startCalibrationTimer();
                this.downloadingDialogCountDown("Calibrating ", "Calibration", 60);
            }
        }

        private void calibrationComplete(byte[] bytePacket)
        {
            string s;
            this.expectingAck = false;
            this.timerSCITimeOut.Enabled = false;
            this.monitoring(false);
            

            if(this.downloadProgress != null)
            {
                this.downloadProgress.Dispose();
            }
            if (bytePacket[1] == 1)
            {
                s = "Error During Calibration, Please Check Input Values";
                
                this.toolStripStatusLabelMain.Text = "Ready";
                this.messageHandler("Calibration", s);
            }
            else if (bytePacket[1] == 2)
            {
                
                s = "Calibration Values Saved";
                this.messageHandler("Calibration", s);
            }
            else
            {
                bool tempBool1, tempBool2;
                tempBool1 = this.monitorPort;
                tempBool2 = this.timerRegisterPolling.Enabled;

                this.monitoring(false);
                this.RegisterPolling(false);
                DialogResult dR = this.messageHandler("Save Calibration Constants?", "Calibration Complete", MessageBoxButtons.YesNo);
                if(dR == DialogResult.Yes)
                    this.sendSaveCalibration();
                this.monitoring(tempBool1);
                this.RegisterPolling(tempBool2);
                this.toolStripStatusLabelMain.Text = "Ready";
            }
            this.enableAll(true);
            this.monitoring(true);
            this.RegisterPolling(true);
            
        }

        

        private void buttonSaveCalibration_Click(object sender, EventArgs e)
        {
            sendSaveCalibration();
        }

        private void sendSaveCalibration()
        {
            
            byte[] packet = new byte[3];

            packet[0] = (byte)'a';
            packet[1] = 0x55;
            packet[2] = 0x0D;
            
            this.sendPacketAck(packet);
            //this.sendPacket(packet);
        }

        private Int16 calibrationCount = 0;
        private string statusLabel = "Calibrating Relay ";
        private int statusTickCount = 60;

        private void timerCalibrationTimer_Tick(object sender, EventArgs e)
        {
            this.calibrationCount++;
            this.monitoring(false);
            if( this.calibrationCount == statusTickCount)
            {
                this.calibrationCount = 0;
                this.timerTimeOutCountdown.Enabled = false;
                this.timerLiveEventAcknowledge.Enabled = false;
                //this.messageHandler("Timed Out", new Exception("No Response From Relay"));
                
                this.messageHandler(statusLabel, "Relay Response Timed Out.");
                this.requestLiveDataToolStripMenuItem1.Enabled = true;
                this.buttonRQEventData.Enabled = true;

                this.toolStripStatusLabelMain.Text = "Ready";
            }
            else if(this.calibrationCount%5 == 0)
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

        private bool readyToGetSineData = true;
        private void buttonRQWaveData_Click(object sender, EventArgs e)
        {
            byte[] packet = new byte[3];

            if(!this.readyToGetSineData)
                return;

            this.readyToGetSineData = false;
            this.lastDataIn = false;
            this.ucSineGraph1.ClearAllValues();

            this.monitoring(false);
            this.RegisterPolling(false);
            packet[0] = (byte)'H';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacket(packet);
            this.RegisterPolling(true);
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
            byte[] packet = new byte[4];

            packet[0] = (byte)'Z';
            packet[1] = (byte)this.CTRatio;
            packet[2] = (byte)(this.CTRatio >> 8);
            packet[3] = 0x0D;

            this.sendPacketAck(packet);

            if(!this.sendAll)
            {
                this.buttonRequestRelayParamaters_Click(this, new EventArgs());
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

            //this.sendPacketAck(packet);
            this.sendPacket(packet);
        }

        private void textBoxCTRatio_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.domainUpDownCTRatio.SelectedIndex == 7)
                {
                    this.CTRatio = Convert.ToInt16(this.textBoxCTRatio.Text);
                    this.ucPhasorGraph1.CTChanged(this.CTRatio);
                }
            }
            catch
            {
                this.textBoxCTRatio.Text = this.CTRatio.ToString();
            }
        }

        private void domainUpDownCTRatioM_SelectedItemChanged(object sender, EventArgs e)
        {
            DomainUpDown dUD = (DomainUpDown)sender;
            int ratio = this.CTRatio;

            switch (dUD.SelectedIndex)
            {
                case 0:
                    ratio = 160;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio.ToString();
                    break;
                case 1:
                    ratio = 240;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio.ToString();
                    break;
                case 2:
                    ratio = 320;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio.ToString();
                    break;
                case 3:
                    ratio = 400;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio.ToString();
                    break;
                case 4:
                    ratio = 500;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio.ToString();
                    break;
                case 5:
                    ratio = 600;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio.ToString();
                    break;
                case 6:
                    ratio = 700;
                    this.textBoxCTRatio.Enabled = false;
                    this.textBoxCTRatio.Text = ratio.ToString();
                    break;
                case 7:
                    
                    try
                    {
                        this.textBoxCTRatio.Text = ratio.ToString();
                    }
                    catch
                    {
                        
                    }
                    
                    this.textBoxCTRatio.Enabled = true;
                    break;
                default:
                    ratio = 160;
                    break;
            }
            this.CTRatio = ratio;
            this.domainUpDownCTRatio.SelectedIndex = dUD.SelectedIndex;
        }

        private void buttonTripRelay_Click(object sender, EventArgs e)
        {
            
            DialogResult dr = this.messageHandler("Trip Relay", "Do you really want to trip the relay?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            
            byte[] bytePacket;
            TripModeDefinition tMD = new TripModeDefinition(TripModes.RemoteTrip);

            if(dr == DialogResult.Yes)
            {
                bytePacket = RelayModeFunctions.BytePacketFor(tMD);
                //this.sendPacketAck(bytePacket);
                this.sendPacket(bytePacket);
            }
            this.requestRelayRegisters();
        }

        private void buttonResetBothProc_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.messageHandler("Reset Relay", "Do you really want to reset the relay?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            
            if(dr == DialogResult.Yes)
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

        private void timerRegisterPolling_Tick(object sender, EventArgs e)
        {
            this.labelByteCount.Text = this.byteCount.ToString();
            //return;
            this.timerRegisterPolling.Enabled = false;
            if (this.pauseMonitoring || this.testVersion)
            {
                this.timerRegisterPolling.Enabled = true;
                return;
            }

            if (!this.serialPort1.IsOpen)
            {
                if(this.portLost)
                {
                    this.relayFound = false;
                    if(this.checkPortAvailability(this.serialPort1.PortName))
                        this.checkPortForRelay();
                    if(this.relayFound)
                    {
                        this.requestAllData();
                        this.enableAll(true);
                        this.RegisterPolling(true);
                        this.portLost = false;
                        
                    }
                }
                else
                {
                    this.portLost = true;
                    this.enableAll(false);
                    this.messageHandler("Error on Port", "Port Is Closed");
                    this.toolStripStatusLabelMain.Text = "No Relay Found";
                    
                }

                return;
            }

            
            if(!this.registersReceived)
            {
                setLabelText("Unknown", this.labelRelayTrippedOrClose);
                setBackgroundColor(Color.Transparent, this.labelRelayTrippedOrClose);
                setLabelText("Unknown", this.labelRelayStateControlPage);
                setBackgroundColor(Color.Transparent, this.labelRelayStateControlPage);
                this.clearTemperatureBoxes();
                
                this.enableFlagsAndStatus(false);
                
            }
            this.registersReceived = false;
            
            //this.enableTripStateLables(true);
            this.requestRelayRegisters();
        }

        private void enableFlagsAndStatus(bool b)
        {
            if(!b)
            {
                this.setCheckedValue(b, this.checkBoxACB);
                this.setCheckedValue(b, this.checkBoxBadOffset);
                this.setCheckedValue(b, this.checkBoxBlockedCloseFlag);
                this.setCheckedValue(b, this.checkBoxBlockedOpenFlag);
                this.setCheckedValue(b, this.checkBoxCalibrating);
                this.setCheckedValue(b, this.checkBoxFloatFlag);
                this.setCheckedValue(b, this.checkBoxMathError);
                this.setCheckedValue(b, this.checkBoxMathOverTime);
                this.setCheckedValue(b, this.checkBoxMonitorPhasors);
                this.setCheckedValue(b, this.checkBoxOffsetOkay);
                this.setCheckedValue(b, this.checkBoxPhasingOkayFlag);
                this.setCheckedValue(b, this.checkBoxPowerSaveFlag);
                this.setCheckedValue(b, this.checkBoxPumping);
                this.setCheckedValue(b, this.checkBoxSequence);
                this.setCheckedValue(b, this.checkBoxTest1);
                this.setCheckedValue(b, this.checkBoxTest2);
                this.setCheckedValue(b, this.checkBoxBFlag);
                this.setCheckedValue(b, this.checkBoxInInsensRegion);
                this.setCheckedValue(b, this.checkBoxInTripRegion);
                this.setCheckedValue(b, this.checkBoxTripFlag);
                this.setCheckedValue(b, this.checkBoxTrippingFlag);
            }

            this.enableCheckBox(b, this.checkBoxACB);
            this.enableCheckBox(b, this.checkBoxBadOffset);
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
            this.enableCheckBox(b, this.checkBoxTest1);
            this.enableCheckBox(b, this.checkBoxTest2);
            this.enableCheckBox(b, this.checkBoxBFlag);
            this.enableCheckBox(b, this.checkBoxInInsensRegion);
            this.enableCheckBox(b, this.checkBoxInTripRegion);
            this.enableCheckBox(b, this.checkBoxTripFlag);
            this.enableCheckBox(b, this.checkBoxTrippingFlag);
            
            this.showLabel(!b, this.labelRelayDisconnected);
        }

        private delegate void enableCheckBoxCallback(bool b, CheckBox cB);

        private void enableCheckBox(bool b, CheckBox checkBox)
        {
            if(checkBox.InvokeRequired)
            {
                enableCheckBoxCallback eCBC = new enableCheckBoxCallback(this.enableCheckBox);
                this.Invoke(eCBC, new object[] {b, checkBox});
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

        private bool initialLiveEventRequest = false;
        private void buttonRQEventData_Click(object sender, EventArgs e)
        {
            this.requestEventDownload();
        }

        private void downloadEventFromRelayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.requestEventDownload();
        }

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

            this.ucEventGraph0.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph1.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph2.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph3.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph4.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph5.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph6.RelayID = this.ucTransmitter1.TXSettings.ID;
            this.ucEventGraph7.RelayID = this.ucTransmitter1.TXSettings.ID;

            this.downloadingDialogCountDown("Downloading Event Data ", 50);
            this.downloadingLiveData = false;
            this.initialLiveEventRequest = true;
            this.requestEventTimes();
            this.timerLiveEventAcknowledge.Enabled = true;
            this.monitoring(false);
            this.RegisterPolling(false);
        }

        private DateTime liveDataTriggerTime;
        private void requestLiveDataToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.serialPort1 == null || !this.serialPort1.IsOpen)
                return;

            this.downloadingCanceled = false;
            this.requestLiveDataToolStripMenuItem1.Enabled = false;
            this.buttonRQEventData.Enabled = false;

            DialogResult dR = this.messageHandler("Downloading Live Data", "Downloading Data.  \r\nThis will take a while.  Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            this.liveDataTriggerTime = DateTime.Now;

            this.labelLiveDataTriggerTime.Text = DateTime.Now.ToString();

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

        private void requestEventData()
        {
            byte[] sendPacket = new byte[3];

            sendPacket[0] = (byte)'k';

            if(this.radioButtonEvent0.Checked)
            {
                //this.ucEventGraph0.ClearAllGraphs();
                sendPacket[1] = 0x01;
            }
            else if (this.radioButtonEvent1.Checked)
            {
                //this.ucEventGraph1.ClearAllGraphs();
                sendPacket[1] = 0x02;
            }
            else if (this.radioButtonEvent2.Checked)
            {
                //this.ucEventGraph2.ClearAllGraphs();
                sendPacket[1] = 0x03;
            }
            else if (this.radioButtonEvent3.Checked)
            {
                //this.ucEventGraph3.ClearAllGraphs();
                sendPacket[1] = 0x04;
            }
            else if (this.radioButtonEvent4.Checked)
            {
                //this.ucEventGraph4.ClearAllGraphs();
                sendPacket[1] = 0x05;
            }
            else if (this.radioButtonEvent5.Checked)
            {
                //this.ucEventGraph5.ClearAllGraphs();
                sendPacket[1] = 0x06;
            }
            else if (this.radioButtonEvent6.Checked)
            {
                //this.ucEventGraph6.ClearAllGraphs();
                sendPacket[1] = 0x07;
            }
            else if (this.radioButtonEvent7.Checked)
            {
                //this.ucEventGraph7.ClearAllGraphs();
                sendPacket[1] = 0x08;
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

        private int savedIndexOffsetTest = 0;

        private void eventLiveDataPacket(byte[] bytePacket)
        {
            this.timerLiveEventAcknowledge.Enabled = false;
            if(this.downloadingCanceled)
                return;
            //this.savedIndexOffsetTest = this.getIndexOffset(bytePacket[3], bytePacket[4]);
            if(bytePacket[0] == 0 && this.downloadingLiveData)
                this.liveDataPacket(bytePacket);
            else if(bytePacket[0] > 0 && bytePacket[0] <= 8 && !this.downloadingLiveData)
                this.eventDataPacket(bytePacket);
            else
            {
                this.nAcknowledge();
                this.timerLiveEventAcknowledge.Start();
            }
            //
                //this.messageHandler("Live/Event Data Packet", bytePacket[0].ToString() + " is not a valid Live/Event Data Packet Event number.");
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
                switch(bytePacket[0])
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
                //if(!this.downloadingCanceled)
                    //this.acknowledge();
            }
            catch (Exception ex)
            {
                this.messageHandler(errorString, ex);
            }

        }

        private void buttonReqCalConstants_Click(object sender, EventArgs e)
        {
            this.requestCalibrationConstants();
        }

        private void requestCalibrationConstants()
        {
            byte[] sendPacket = new byte[2];

            sendPacket[0] = 0x6C; 
            //sendPacket[1] = 0x55;
            sendPacket[1] = 0x0D;

            this.sendPacket(sendPacket);
        }

        #endregion

        #region Live Data

        private void liveDataPacket(byte[] bytePacket)
        {
            
            this.ucLiveData1.PacketHandler(bytePacket);
            //PhasorTypes pT = RelayModeFunctions.PhasorTypeFrom((char)bytePacket[1], (char)bytePacket[2]);
            
            //if(bytePacket[3] == 64 && bytePacket[4] == 0 && pT == PhasorTypes.IC)
                //SHould have something here.
            
            //if(!this.downloadingCanceled)
                //this.acknowledge();
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

            tempBool = !this.pausePhasorMonitoring;
            tempBool2 = this.allEnabled;
            tempBool3 = !this.pauseMonitoring;
            try
            {
                this.enableAll(false);
                this.monitoring(false);
                this.RegisterPolling(false);

                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);//, MessageBoxOptions.ServiceNotification);

                
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

            tempBool = !this.pausePhasorMonitoring;
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

            tempBool = !this.pausePhasorMonitoring;
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

        private void buttonFFT_Click(object sender, EventArgs e)
        {
            float[] working = this.ucSineGraph1.GetWaveArray(PhasorTypes.VnA, 128);

            //working = this.generateSineWave(128, 169.70562748477140585620264690516f);

            /*
            foreach (float f in working)
            {
                this.ucSineGraph1.AddValue(f, PhasorTypes.VnA);
            }
            this.ucSineGraph1.Invalidate();
            */
            float[] temp = new float[128];

            for (int i = 0; i < working.Length; ++i)
            {
                temp[i] = working[i];//* 2.54f;
            }
            this.ucFrequencyAnalysis1.Values = temp;
            
            

        }

        private float[] generateSineWave(int samples, float amplitude)
        {
            float[] returnArray = new float[samples];
            double temp;
            for (int i = 0; i < samples; i++)
            {
                temp = (double)i/(double)samples;
                temp = Math.PI * temp * 2d;
                temp = (double)amplitude * Math.Sin(temp);
                returnArray[i] = (float)temp;
            }

            return returnArray;
        }
        //private SavedSettingsv1 saveObject = new SavedSettingsv1();
        private SavedSettingsv2 saveObject = new SavedSettingsv2();


        /*
        private void buttonSaveSetting_Click(object sender, EventArgs e)
        {
            SavedSettingv1 sS = new SavedSettingv1();

            try
            {

                if(!validTextBoxValue(this.textBoxSaveStateName.Text))
                {
                    throw new Exception("Bad Save Setting Name");
                }

                sS.Name = this.textBoxSaveStateName.Text;
                this.getAllSaveStates(sS);
                this.saveObject.AddState(sS);
                this.writeSaveObjectToFile();
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Saving Setting", ex);
            }
        }
        */
        private void buttonSaveSetting_Click(object sender, EventArgs e)
        {
            SavedSettingv2 sS = new SavedSettingv2();

            try
            {

                if (!validTextBoxValue(this.textBoxSaveStateName.Text))
                {
                    throw new Exception("Bad Save Setting Name");
                }

                sS.Name = this.textBoxSaveStateName.Text;
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
            Stream stream = File.Open(@"C:\DGI Systems\Relay\SavedSettings.dgi", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, this.saveObject);
            stream.Close();
            this.initializeSaveObject();
        }

        /*
        private void initializeSaveObject()
        {
            SavedSettingv1 sS = new SavedSettingv1();
            int i = 0;

            try
            {
                Stream stream = File.Open(@"C:\DGI Systems\Relay\SavedSettings.dgi", FileMode.OpenOrCreate);
                BinaryFormatter formatter = new BinaryFormatter();

                if(stream.Length != 0)
                    this.saveObject = (SavedSettings)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Opening Save File", ex);
            }

            try
            {
                this.comboBoxSavedStates.Items.Clear();
                if(this.saveObject == null)
                    return;

                for (i = 0; i < this.saveObject.Settings.Count; ++i)
                {
                    if(this.saveObject.Settings[i].Name != null && this.saveObject.Settings[i].Name != "")
                        this.comboBoxSavedStates.Items.Add(this.saveObject.Settings[i].Name);
                }
                this.comboBoxSavedStates.Text = "";
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Populating Saved States ComboBox", ex);
            }
        }
        */

        private void initializeSaveObject()
        {
            SavedSettingv2 sS = new SavedSettingv2();
            int i = 0;

            try
            {
                Stream stream = File.Open(@"C:\DGI Systems\Relay\SavedSettings.dgi", FileMode.OpenOrCreate);
                BinaryFormatter formatter = new BinaryFormatter();

                if (stream.Length != 0)
                    this.saveObject = (SavedSettingsv2)formatter.Deserialize(stream);
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

        private void buttonDeleteSetting_Click(object sender, EventArgs e)
        {
            if(this.comboBoxSavedStates.Text != "" && this.comboBoxSavedStates.Text != null)
                this.saveObject.DeleteState(this.comboBoxSavedStates.Text);

            this.writeSaveObjectToFile();
            this.initializeSaveObject();
        }

        /*
        private void getAllSaveStates(SavedSettingv1 sS)
        {
            sS.TripSettings = this.ucTripMode2.GetSavedState();
            sS.PumpSettings = this.ucPumpMode1.GetSavedState();
            sS.CloseSettings = this.ucCloseMode1.GetSavedState();
        }
        */
        private void getAllSaveStates(SavedSettingv2 sS)
        {
            sS.TripSettings = this.ucTripMode2.GetSavedState();
            sS.PumpSettings = this.ucPumpMode1.GetSavedState();
            sS.CloseSettings = this.ucCloseMode1.GetSavedState();
            sS.CTRatio = this.CTRatio;
            sS.Phasing = this.domainUpDownPhasings.SelectedIndex;
            sS.RelayType = this.domainUpDownRelayType.SelectedIndex;
        }

        /*
        private void comboBoxSavedStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            SavedSettingv1 sS = new SavedSettingv1();

            if(this.comboBoxSavedStates.SelectedItem == null)
                return;

            try
            {
                foreach(SavedSettingv1 s in this.saveObject.Settings)
                {
                    if(s.Name.Equals(this.comboBoxSavedStates.SelectedItem))
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
        */

        private void comboBoxSavedStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            SavedSettingv2 sS = new SavedSettingv2();

            if (this.comboBoxSavedStates.SelectedItem == null)
                return;

            try
            {
                foreach (SavedSettingv2 s in this.saveObject.Settings)
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

        /*
        private void setAllValues(SavedSettingv1 sS)
        {
            this.ucTripMode2.SetAllValues(sS.TripSettings);
            this.ucCloseMode1.SetAllValues(sS.CloseSettings);
            this.ucPumpMode1.SetAllValues(sS.PumpSettings);
        }
        */

        private void setAllValues(SavedSettingv2 sS)
        {
            this.ucTripMode2.SetAllValues(sS.TripSettings);
            this.ucCloseMode1.SetAllValues(sS.CloseSettings);
            this.ucPumpMode1.SetAllValues(sS.PumpSettings);

            this.CTRatio = sS.CTRatio;
            this.updateCTRatioDomain(sS.CTRatio, this.domainUpDownCTRatio);
            this.updateCTRatioDomain(sS.CTRatio, this.domainUpDownCTRatioM);
            this.textBoxCTRatio.Text = sS.CTRatio.ToString();

            this.domainUpDownPhasings.SelectedIndex = sS.Phasing;
            this.domainUpDownRelayType.SelectedIndex = sS.RelayType;
        }

        private bool sendAll = false;

        private void buttonSendAll_Click(object sender, EventArgs e)
        {
            this.sendAll = true;
            this.buttonRelayType_Click(this, new EventArgs());
            this.buttonSendCTRatio_Click(this, new EventArgs());
            this.ucTripMode2.buttonSendTripMode_Click(this, new EventArgs());
            this.ucCloseMode1.buttonSendCloseData_Click(this, new EventArgs());
            this.ucPumpMode1.buttonSend_Click(this, new EventArgs());
            this.sendAll = false;
            this.requestRelayParameters();
            this.parametersLoaded = true;
        }

        private void requestLiveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private bool readyToGetCycleData = true;

        private void requestLiveData()
        {
            byte[] sendPacket = new byte[3];

            
            if(!this.readyToGetCycleData)
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
            this.downloadingDialogCountDown("Downloading Flight Recorder Data ", 210);
        }

        private ProgressBarForm downloadProgress;
        private void downloadingDialogCountDown(string label, int halfSecondCounts)
        {
            this.enableAll(false);
            this.downloadProgress = new ProgressBarForm(label, halfSecondCounts);
            this.downloadProgress.Done += new ProgressBarForm.ProgressBarEvent(downloadProgress_Done);
            this.downloadProgress.Show();
        }

        private void downloadingDialogCountDown(string formText, string title, int halfSecondCounts)
        {
            this.enableAll(false);
            this.downloadProgress = new ProgressBarForm(formText, title, halfSecondCounts);
            this.downloadProgress.Done += new ProgressBarForm.ProgressBarEvent(downloadProgress_Done);
            this.downloadProgress.Show();
        }


        void downloadProgress_Done(bool b, string s)
        {
            //True Means it TimedOut
            string temp = this.downloadProgress.Text;

            this.downloadingCanceled = true;

            this.requestLiveDataToolStripMenuItem1.Enabled = true;
            this.buttonRQEventData.Enabled = true;

            this.downloadProgress.Dispose();
            this.timerLiveEventAcknowledge.Enabled = false;

            if(b)
            {
                this.messageHandler(temp + " Timed Out.", new Exception("Error " + temp));
            }
            else
            {
                this.messageHandler(temp + " Canceled", s);//new Exception("Downloading Canceled"));
            }

            this.enableAll(true);
            this.monitoring(true);
            this.RegisterPolling(true);
        }

        private void acknowledgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.ucLiveData1.ClearAllGraphs();
            this.acknowledge();
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

            packet[0] = (byte)'y';
            packet[1] = 0x55;
            packet[2] = 0x0D;

            this.sendPacket(packet);
        }

        private int testCount1 = 0;
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
            this.textBoxTimeOutCount.Text = value.ToString();
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
            this.textBoxNAckCount.Text = value.ToString();
        }

        private void timerLiveEventAcknowledge_Tick(object sender, EventArgs e)
        {
            this.timerLiveEventAcknowledge.Enabled = false;

            if(this.initialLiveEventRequest)
            {
                this.initialLiveEventRequest = false;
                this.downloadProgress_Done(false, "No Response From Relay.\r\nPlease Check Connection.");
                return;
            }

            this.liveEventTimedOutCount++;
            if(this.downloadingCanceled)
            {
                return;
            }
            
            this.nAcknowledge();
            this.timerLiveEventAcknowledge.Start();
        }

        private void liveEvent_PacketHandled(object sender, PacketHandledEventArgs e)
        {
            this.timerLiveEventAcknowledge.Enabled = false;
            
            if(e.Successful)
            {
                testCount1++;
                
                this.acknowledge();
            }
            else
            {
                this.nAckCount++;
                //this.downloadingLiveData = false;
                this.nAcknowledge();
            }
            this.timerLiveEventAcknowledge.Start();
        }

        private void MainControl_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(this.serialPort1 != null)
            {
                this.clearSerialPortBuffers(this.serialPort1);
                this.serialPort1.Dispose();
            }
            if(this.threadFindRelay != null)
            {
                if(this.threadFindRelay.IsAlive)
                {
                    this.threadFindRelay.Abort();
                }
                this.threadFindRelay = null;
            }
            if(this.closePort != null)
            {
                if(this.closePort.IsAlive)
                {
                    this.closePort.Abort();
                }
                this.closePort = null;
            }
            Application.Exit();
        }

        private void saveEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.ucEventGraph0.Saveable && this.ucEventGraph1.Saveable &&
                this.ucEventGraph2.Saveable && this.ucEventGraph4.Saveable &&
                this.ucEventGraph4.Saveable && this.ucEventGraph5.Saveable &&
                this.ucEventGraph6.Saveable && this.ucEventGraph7.Saveable &&
                this.ucEventGraph0.Type != EventTypes.NoEvent)
            {
                this.saveEventsToFile();
            }
            else
            {
                this.messageHandler("Not Saved", "Please Download All Event Data Before Saving");
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

                if(saveEventsDialog.FileName != "")
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
            //uEG.ClearAllGraphs();
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
                oFD.ShowDialog();

                if(oFD.FileName.Contains(".evt"))
                {
                    using(Stream stream = File.Open(oFD.FileName, FileMode.Open))
                    {
                        bF = new BinaryFormatter();

                        sES = (SavedEventSet)bF.Deserialize(stream);
                        //stream.Close();
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
                if(!this.ucLiveData1.Saveable)
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
                    using(Stream stream = File.Open(saveEventsDialog.FileName, FileMode.Create))
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
                oFD.InitialDirectory = @"C:\DGI Systems\Relay\Saved Data";
                oFD.Title = "Open Event Saved File.";
                DialogResult dR = oFD.ShowDialog();
                
                if(dR != DialogResult.OK)
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

            this.labelLiveDataTriggerTime.Text = "ID Number : " + sSE.ID + " Triggered - " +sSE.Time.ToString();
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
            //this.sendTime(DateTime.Now);
            this.sendTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));//new DateTime(2009, 8, 1));
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
            if(this.tabControlMain.SelectedTab == this.tabPageFlightRecorder)
            {
                this.eventActionsToolStripMenuItem.Enabled = false;
                this.liveDataActionsToolStripMenuItem.Enabled = true;
            }
            else if(this.tabControlMain.SelectedTab == this.tabPageEvents)
            {
                this.eventActionsToolStripMenuItem.Enabled = true;
                this.liveDataActionsToolStripMenuItem.Enabled = false;
            }
            else
            {
                this.eventActionsToolStripMenuItem.Enabled = false;
                this.liveDataActionsToolStripMenuItem.Enabled = false;
            }

        }

        

        private void buttonFPGAProcVersion_Click(object sender, EventArgs e)
        {
            this.requestFPGARevision();
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
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
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
            }
            catch
            {
            }

            base.Dispose(disposing);
        }
    }

    
    [Serializable()]

    public class SavedSettingsv1 : ISerializable
    {
        public SavedSettingsv1()
        {
        }
        
        public List<SavedSettingv1> Settings = new List<SavedSettingv1>();

        public SavedSettingsv1(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Settings = (List<SavedSettingv1>)info.GetValue("Settings", typeof(List<SavedSettingv1>));
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

        public void AddState(SavedSettingv1 sS)
        {
            int i = 0;
            for (i = 0; i < this.Settings.Count; ++i)
            {
                if(this.Settings[i].Name == sS.Name)
                {
                    this.Settings[i] = sS;
                    break;
                }
            }

            if(i == this.Settings.Count)
            {
                this.Settings.Add(sS);
            }

            this.Settings.Sort(delegate(SavedSettingv1 sS1, SavedSettingv1 sS2) { return sS1.Name.CompareTo(sS2.Name); });
        }

        public void DeleteState(string name)
        {
            foreach(SavedSettingv1 sS in this.Settings)
            {
                if(sS.Name == name)
                {
                    this.Settings.Remove(sS);
                    break;
                }
            }
        }

        public void DeleteState(SavedSettingv1 sS)
        {
            try
            {
                if(this.Settings.Count > 0)
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

            this.Settings.Sort(delegate(SavedSettingv2 sS1, SavedSettingv2 sS2) { return sS1.Name.CompareTo(sS2.Name); });
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
    public class SavedSettingv1 : ISerializable
    {
        public SavedSettingv1()
        {
        }

        public string Name;
        public TripModeSavedState TripSettings;
        public CloseModeSaveState CloseSettings;
        public PumpModeSavedState PumpSettings;

        public SavedSettingv1(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripSettings = (TripModeSavedState)info.GetValue("Trip Settings", typeof(TripModeSavedState));
                this.CloseSettings = (CloseModeSaveState)info.GetValue("Close Settings", typeof(CloseModeSaveState));
                this.PumpSettings = (PumpModeSavedState)info.GetValue("Pump Settings", typeof(PumpModeSavedState));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Single Saved Setting.", ex);
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Sing Saved Setting Get Object Data", ex);
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
        public TripModeSavedState TripSettings;
        public CloseModeSaveState CloseSettings;
        public PumpModeSavedState PumpSettings;
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
                throw new Exception("Error Instantiating Overall Single Saved Setting.", ex);
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
    
}
