using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GraphicsServer.GSNet.SeriesData;
using GraphicsServer.GSNet.Charting;
using GraphicsServer.GSNet.Widgets;

namespace RelayControlLibrary
{
    public partial class ucTransmitterMonitoring : UserControl
    {
        #region Initialization

        public ucTransmitterMonitoring()
        {
            InitializeComponent();
            myInitialize();
        }

        private void myInitialize()
        {
            this.myChartLoads.Visible = false;
            this.myChartVoltages.Visible = false;
            this.myPSIWidgetA1.Visible = false;
            this.myPSIWidgetA2.Visible = false;
            this.myTempWidgetA1.Visible = false;
            this.myTempWidgetA2.Visible = false;
            this.myThermometerA1.Visible = false;
            this.myThermometerA2.Visible = false;
            this.initializeChart(this.myChartLoads.Chart);
            this.initializeChart(this.myChartVoltages.Chart);
            this.graphingValues.Tables.Add();
            this.graphingValues.Tables[0].Columns.Add("VnA", typeof(float));
            this.graphingValues.Tables[0].Columns.Add("VnB", typeof(float));
            this.graphingValues.Tables[0].Columns.Add("VnC", typeof(float));
            this.graphingValues.Tables[0].Columns.Add("VtA", typeof(float));
            this.graphingValues.Tables[0].Columns.Add("VtB", typeof(float));
            this.graphingValues.Tables[0].Columns.Add("VtC", typeof(float));
            this.graphingValues.Tables[0].Columns.Add("IA", typeof(float));
            this.graphingValues.Tables[0].Columns.Add("IB", typeof(float));
            this.graphingValues.Tables[0].Columns.Add("IC", typeof(float));
            this.graphingValues.Tables[0].Columns.Add("SampleNumber", typeof(UInt64));
            this.listBoxA1SensorSelect.SelectedIndex = 2;
            this.listBoxA2SensorSelect.SelectedIndex = 2;

#if (chicago || MADISON || DEBUG) && !Enmax
            this.listBoxA2SensorSelect.SelectedIndex = 0;
            groupBoxAnalogFlagValues.Visible = true;
#elif Enmax && !DEBUG
            this.listBoxA1SensorSelect.SelectedIndex = 0;
            this.listBoxA2SensorSelect.SelectedIndex = 2;
            this.textBoxCa.Visible = false;
            this.textBoxDa.Visible = false;
            this.textBoxEa.Visible = false;
            this.textBoxFa.Visible = false;
            this.textBoxGa.Visible = false;
            this.labelCa.Visible = false;
            this.labelDa.Visible = false;
            this.labelEa.Visible = false;
            this.labelFa.Visible = false;
            this.labelGa.Visible = false;
            this.labelHa.Text = "Oil Level";
            this.labelHa.Location = new Point(4, 142);
#elif DEBUG && Enmax
            this.listBoxA1SensorSelect.SelectedIndex = 0;
            this.listBoxA2SensorSelect.SelectedIndex = 2;
#else
            groupBoxAnalogFlagValues.Visible = false;
#endif

#if Enmax && !DEBUG
            this.checkBoxFlagStatusH.Visible = false;
#endif

#if DG288_TESTFIXTURE_GUI
            groupBoxCurrentReadings.Visible = false;
            groupBox17.Visible = false;
            groupBoxVoltageReadings.Visible = false;
            this.listBoxA1SensorSelect.SelectedIndex = 3;
            this.listBoxA2SensorSelect.SelectedIndex = 3;
#endif
            this.checkBoxFrequenceBlue.Visible = false;
            this.checkBoxFrequencyGreen.Visible = false;
            this.checkBoxFrequencyRed.Visible = false;
            this.checkBoxFrequencyYellow.Visible = false;
            this.checkBoxVoltageLow.Visible = false;
            this.checkBoxPhaseError.Visible = false;
            this.lblBackfeedA.Visible = false;
            this.lblBackfeedB.Visible = false;
            this.lblBackfeedC.Visible = false;

            this.checkBoxFrequenceBlue.Checked = true;
            this.checkBoxFrequencyGreen.Checked = true;
            this.checkBoxFrequencyRed.Checked = true;
            this.checkBoxFrequencyYellow.Checked = true;

#if chicago && !DG288_TESTFIXTURE_GUI
            this.textBoxGa.Visible = false;
            this.textBoxHa.Visible = false;

            this.checkBoxFlagStatusC.Visible = false;
            this.checkBoxFlagStatusD.Visible = false;
            this.checkBoxFlagStatusE.Visible = false;
            this.checkBoxFlagStatusF.Visible = false;

            this.labelGa.Visible = false;
            this.labelHa.Visible = false;
#elif MADISON && !DEBUG
            this.textBoxCa.Visible = false;
            this.textBoxDa.Visible = false;
            this.textBoxEa.Visible = false;
            this.textBoxFa.Visible = false;
            this.textBoxGa.Visible = false;
            this.textBoxHa.Visible = false;

            this.labelCa.Visible = false;
            this.labelDa.Visible = false;
            this.labelEa.Visible = false;
            this.labelFa.Visible = false;
            this.labelGa.Visible = false;
            this.labelHa.Visible = false;
#endif
#if PSEG
            this.listBoxA2SensorSelect.SelectedItem = "Oil Temperature";
#endif
        }

        #endregion

        #region Variables

        private Customers customer = Customers.NonConEd;
        public Customers Customer
        {
            get { return this.customer; }
            set
            {
                if (value != this.customer)
                {
                    this.customer = value;
                    this.setCustomer();
                }
            }
        }

        private void setCustomer()
        {
            if (this.GEEnabled)
            {
#if !DEBUG
                this.labelVtA.Visible = false;
                this.labelVtB.Visible = false;
                this.labelVtC.Visible = false;
                this.textBoxVtA.Visible = false;
                this.textBoxVtB.Visible = false;
                this.textBoxVtC.Visible = false;
#endif
            }
            else
            {
                this.labelVtA.Visible = true;
                this.labelVtB.Visible = true;
                this.labelVtC.Visible = true;
                this.textBoxVtA.Visible = true;
                this.textBoxVtB.Visible = true;
                this.textBoxVtC.Visible = true;
            }
        }

        private bool protector277 = false;
        public bool Protector277
        {
            get { return this.protector277; }
            set
            {
                this.protector277 = value;
                this.updateChart(this.protector277);
            }
        }

        private bool transmitterMonitoring = false;
        public bool TransmitterMonitoring
        {
            get { return this.transmitterMonitoring; }
            set
            {
                this.transmitterMonitoring = value;
                if (value)
                    this.startTransmitterMonitoring();
                else
                    this.pauseTransmitterMonitoring();
            }
        }

        private int cTRatio = 320;
        public int CTRatio
        {
            get { return this.cTRatio; }
            set
            {
                this.cTRatio = value;
                this.setCTRatio(value);
            }
        }

        private string cTMult = "";
        public string CTMult
        {
            get { return this.cTMult; }
            set
            {
                this.cTMult = value;
                this.textBoxCTMult.Text = value;
                //this.setCTRatio(value);
            }
        }

        private Frequencies frequency = Frequencies.Red;
        public Frequencies Frequency
        {
            get { return this.frequency; }
            set
            {
                this.frequency = value;
                this.setFrequency(value);
            }
        }

        float convert277 = 2.216f;

        private Series seriesVnA = new Series("VnA");
        private Series seriesVnB = new Series("VnB");
        private Series seriesVnC = new Series("VnC");
        private Series seriesVtA = new Series("VtA");
        private Series seriesVtB = new Series("VtB");
        private Series seriesVtC = new Series("VtC");
        private Series seriesX1 = new Series("X1%");
        private Series seriesX2 = new Series("X2%");
        private Series seriesX3 = new Series("X3%");
        private Series seriesSampleNumber = new Series("SampleNumber");
        private DataSet graphingValues = new DataSet();
        private DataView dVGraphingValues = new DataView();
        private DataRow workingRow;
        private UInt64 sampleNumber = 0;

        private bool firstDataSeen = false;
        private TransmitterMeterValues transmitterMeterValuesA1 = new TransmitterMeterValues();
        private TransmitterMeterValues transmitterMeterValuesA2 = new TransmitterMeterValues();

        private string transmitterID = "";
        public string TransmitterID
        {
            get { return this.transmitterID; }
            set
            {
                this.transmitterID = value;
                this.textBoxTransmitterID.Text = value;
            }
        }

        private string transmitterSN = "";
        public string TransmitterSN
        {
            get { return this.transmitterSN; }
            set
            {
                this.transmitterSN = value;
                this.textBoxTransmitterSN.Text = value;
            }
        }

        private string timeElapsedSeconds = "";
        public string TimeElapsedSeconds
        {
            get { return this.timeElapsedSeconds; }
            set
            {
                this.timeElapsedSeconds = value;
                this.textBoxTimeElapsedSeconds.Text = value;
            }
        }

        private string timeElapsedMinutes = "";
        public string TimeElapsedMinutes
        {
            get { return this.timeElapsedMinutes; }
            set
            {
                this.timeElapsedMinutes = value;
                this.textBoxTimeElapsedMinutes.Text = value;
            }
        }

        private string timeElapsedHours = "";
        public string TimeElapsedHours
        {
            get { return this.timeElapsedHours; }
            set
            {
                this.timeElapsedHours = value;
                this.textBoxTimeElapsedHours.Text = value;
            }
        }

        #endregion

        #region Monitoring Control

        private void pauseTransmitterMonitoring()
        {
            this.textBoxTimeElapsedHours.Text = "";
            this.textBoxTimeElapsedMinutes.Text = "";
            this.textBoxTimeElapsedSeconds.Text = "";
            this.sampleNumber = 0;
            this.buttonStartMonitoring.Enabled = true;
            this.buttonPauseMonitoring.Enabled = false;
            this.transmitterMonitoring = false;
            this.seriesVnA = new Series("VnA");
            this.seriesVnB = new Series("VnB");
            this.seriesVnC = new Series("VnC");
            this.seriesVtA = new Series("VtA");
            this.seriesVtB = new Series("VtB");
            this.seriesVtC = new Series("VtC");
            this.seriesX1 = new Series("X1%");
            this.seriesX2 = new Series("X2%");
            this.seriesX3 = new Series("X3%");
            this.disableMonitoring();
        }

        private void startTransmitterMonitoring()
        {
            this.buttonStartMonitoring.Enabled = false;
            this.buttonPauseMonitoring.Enabled = true;
            this.transmitterMonitoring = true;
            this.enableMonitoring();

#if !WATERBUG
            this.myChartVoltages.Visible = true;
            this.myChartLoads.Visible = true;
#endif
            this.enableMonitoring();
        }

        #endregion

        #region Communication

        private void messageHandler(string s, Exception ex)
        {
        }

        delegate void bytePacketCallback(byte[] bytePacket);

        public void SetAll(byte[] bytePacket)
        {
            if (this.InvokeRequired)
            {
                bytePacketCallback bPCB = new bytePacketCallback(setAll);
                this.Invoke(bPCB, new object[] { bytePacket });
            }
            else
            {
                this.setAll(bytePacket);
            }
        }

        private void setAll(byte[] bytePacket)
        {
            int localTemp, powerPercent, monByteLength = 0;

            //TransmitterPower
            localTemp = bytePacket[1];
            localTemp <<= 8;
            localTemp += bytePacket[0];

            localTemp >>= 7;                //Align 12 bit ADC value then get MS Byte
            localTemp = localTemp & 0x00FF;

            if (localTemp <= 9)
                powerPercent = 0;
            else if (localTemp <= 13)
                powerPercent = 20;
            else if (localTemp <= 18)
                powerPercent = 30;
            else if (localTemp <= 28)
                powerPercent = 40;
            else if (localTemp <= 38)
                powerPercent = 50;
            else if (localTemp <= 48)
                powerPercent = 60;
            else if (localTemp <= 58)
                powerPercent = 70;
            else if (localTemp <= 68)
                powerPercent = 80;
            else if (localTemp <= 78)
                powerPercent = 90;
            else
                powerPercent = 100;
            this.textBoxTransmitterOutputPower.Text = powerPercent.ToString();

            //Transmitter Temperature

            localTemp = bytePacket[3];
            localTemp <<= 8;
            localTemp += bytePacket[2];

            this.textBoxTransmitterTemp.Text = localTemp.ToString();

            //Transmitter Flags A is LSB
            if ((bytePacket[6] & 1) == 1)
            {
                this.checkBoxFlagStatusA.Checked = true;
            }
            else
            {
                this.checkBoxFlagStatusA.Checked = false;
            }

            if ((bytePacket[6] & 2) == 2)
            {
                this.checkBoxFlagStatusB.Checked = true;
            }
            else
            {
                this.checkBoxFlagStatusB.Checked = false;
            }
            if ((bytePacket[6] & 4) == 4)
            {
                this.checkBoxFlagStatusC.Checked = true;
            }
            else
            {
                this.checkBoxFlagStatusC.Checked = false;
            }
            if ((bytePacket[6] & 8) == 8)
            {
                this.checkBoxFlagStatusD.Checked = true;
            }
            else
            {
                this.checkBoxFlagStatusD.Checked = false;
            }
            if ((bytePacket[6] & 16) == 16)
            {
                this.checkBoxFlagStatusE.Checked = true;
            }
            else
            {
                this.checkBoxFlagStatusE.Checked = false;
            }
            if ((bytePacket[6] & 32) == 32)
            {
                this.checkBoxFlagStatusF.Checked = true;
            }
            else
            {
                this.checkBoxFlagStatusF.Checked = false;
            }
            if ((bytePacket[6] & 64) == 64)
            {
                this.checkBoxFlagStatusG.Checked = true;
            }
            else
            {
                this.checkBoxFlagStatusG.Checked = false;
            }
            if ((bytePacket[6] & 128) == 128)
            {
                this.checkBoxFlagStatusH.Checked = true;
            }
            else
            {
                this.checkBoxFlagStatusH.Checked = false;
            }

            this.transmitterMeterValuesA1.RawValue = (int)bytePacket[4];
            this.transmitterMeterValuesA2.RawValue = (int)bytePacket[5];

            ((Thermometer)this.myTempWidgetA1.Widget.DeviceList.GetDevice(0)).Value = this.transmitterMeterValuesA1.DGITemp;
            ((Meter)this.myPSIWidgetA1.Widget.DeviceList.GetDevice(0)).NeedleList.GetNeedle(0).Value = this.transmitterMeterValuesA1.Pressure;
            ((Meter)this.myThermometerA1.Widget.DeviceList.GetDevice(0)).NeedleList.GetNeedle(0).Value = this.transmitterMeterValuesA1.OilTemp;

            ((Thermometer)this.myTempWidgetA2.Widget.DeviceList.GetDevice(0)).Value = this.transmitterMeterValuesA2.DGITemp;
            ((Meter)this.myPSIWidgetA2.Widget.DeviceList.GetDevice(0)).NeedleList.GetNeedle(0).Value = this.transmitterMeterValuesA2.Pressure;
            ((Meter)this.myThermometerA2.Widget.DeviceList.GetDevice(0)).NeedleList.GetNeedle(0).Value = this.transmitterMeterValuesA2.OilTemp;

            this.setA1Value();
            this.setA2Value();

            this.myPSIWidgetA2.Invalidate();
            this.myTempWidgetA2.Invalidate();
            this.myThermometerA2.Invalidate();
            this.myPSIWidgetA1.Invalidate();
            this.myTempWidgetA1.Invalidate();
            this.myThermometerA1.Invalidate();

            //CDEFGH Q set

            monByteLength = bytePacket.Length;

            if (monByteLength != 7)
            {
                this.textBoxCa.Text = bytePacket[7].ToString();
                this.textBoxDa.Text = bytePacket[8].ToString();
                this.textBoxEa.Text = bytePacket[9].ToString();
                this.textBoxFa.Text = bytePacket[10].ToString();
                this.textBoxGa.Text = bytePacket[11].ToString();
                this.textBoxHa.Text = bytePacket[12].ToString();
                if (this.waterBugActive)
                    this.textBoxQBit.Text = ((bytePacket[15] & 0x01) == 1) ? "LOW" : "OK";
                else
                    this.textBoxQBit.Text = "N/A";
            }
        }

        public delegate void MonitoringControlHandler(object sender, TransmitterMonitoringEventArgs tMEA);

        public event MonitoringControlHandler MonitoringStateChange;

        private void enableMonitoring()
        {
            TransmitterMonitoringEventArgs tMEA = new TransmitterMonitoringEventArgs(true);
            if (MonitoringStateChange != null)
                MonitoringStateChange(this, tMEA);
        }

        private void disableMonitoring()
        {
            TransmitterMonitoringEventArgs tMEA = new TransmitterMonitoringEventArgs(false);
            if (MonitoringStateChange != null)
                MonitoringStateChange(this, tMEA);
        }


        #endregion

        #region Analog Graphs

        private void setA1Value()
        {
            if (this.listBoxA1SensorSelect.Text == "Oil Temperature")
            {
                this.textBoxA1Analog1.Text = this.transmitterMeterValuesA1.OilTemp.ToString();
            }
            else if (this.listBoxA1SensorSelect.Text == "Tank Pressure")
            {
                this.textBoxA1Analog1.Text = this.transmitterMeterValuesA1.Pressure.ToString();
            }
            else if (this.listBoxA1SensorSelect.Text == "DGI Temperature")
            {
                this.textBoxA1Analog1.Text = this.transmitterMeterValuesA1.DGITemp.ToString();
            }
            else if (this.listBoxA1SensorSelect.Text == "Raw Number")
            {
                this.textBoxA1Analog1.Text = this.transmitterMeterValuesA1.RawValue.ToString();
            }
            else
            {
                throw new Exception("Invalid ListBox Value: " + this.listBoxA1SensorSelect.Text + " in " + this.listBoxA1SensorSelect.ToString());
            }
        }

        private void listBoxA1SensorSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lB = (ListBox)sender;
            string selectedItem = (string)lB.SelectedItem;

            this.setA1Value();

            if (selectedItem.Equals("Oil Temperature"))
            {
                this.myPSIWidgetA1.Visible = false;
                this.myTempWidgetA1.Visible = false;
                this.myThermometerA1.Visible = true;
            }
            else if (selectedItem.Equals("Tank Pressure"))
            {
                this.myPSIWidgetA1.Visible = true;
                this.myTempWidgetA1.Visible = false;
                this.myThermometerA1.Visible = false;
            }
            else if (selectedItem.Equals("DGI Temperature"))
            {
                this.myPSIWidgetA1.Visible = false;
                this.myTempWidgetA1.Visible = true;
                this.myThermometerA1.Visible = false;
            }
            else
            {
                this.myPSIWidgetA1.Visible = false;
                this.myTempWidgetA1.Visible = false;
                this.myThermometerA1.Visible = false;
            }
        }

        private void listBoxA2SensorSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lB = (ListBox)sender;
            string selectedItem = (string)lB.SelectedItem;

            this.setA2Value();

            if (selectedItem.Equals("Oil Temperature"))
            {
                this.myPSIWidgetA2.Visible = false;
                this.myTempWidgetA2.Visible = false;
                this.myThermometerA2.Visible = true;
            }
            else if (selectedItem.Equals("Tank Pressure"))
            {
                this.myPSIWidgetA2.Visible = true;
                this.myTempWidgetA2.Visible = false;
                this.myThermometerA2.Visible = false;
            }
            else if (selectedItem.Equals("DGI Temperature"))
            {
                this.myPSIWidgetA2.Visible = false;
                this.myTempWidgetA2.Visible = true;
                this.myThermometerA2.Visible = false;
            }
            else
            {
                this.myPSIWidgetA2.Visible = false;
                this.myTempWidgetA2.Visible = false;
                this.myThermometerA2.Visible = false;
            }
        }

        private void setA2Value()
        {
            if (this.listBoxA2SensorSelect.Text == "Oil Temperature")
            {
                this.textBoxA2Analog2.Text = this.transmitterMeterValuesA2.OilTemp.ToString();
            }
            else if (this.listBoxA2SensorSelect.Text == "Tank Pressure")
            {
                this.textBoxA2Analog2.Text = this.transmitterMeterValuesA2.Pressure.ToString();
            }
            else if (this.listBoxA2SensorSelect.Text == "DGI Temperature")
            {
                this.textBoxA2Analog2.Text = this.transmitterMeterValuesA2.DGITemp.ToString();
            }
            else if (this.listBoxA2SensorSelect.Text == "Raw Number")
            {
                this.textBoxA2Analog2.Text = this.transmitterMeterValuesA2.RawValue.ToString();
            }
            else
            {
                throw new Exception("Invalid ListBox Value: " + this.listBoxA2SensorSelect.Text + " in " + this.listBoxA2SensorSelect.ToString());
            }
        }

        #endregion

        #region Charts

        private void initializeChart(Chart chart)
        {
            try
            {
                chart.ChartTitle.IsVisible = false;
                chart.ChartTitle.IsSelectable = false;
                chart.Grid.IsSelectable = false;
                chart.Legend.IsSelectable = false;

                chart.Grid.AxisX.LabelProperties.RotateAngle = 40;
                chart.Grid.AxisX.LabelOffset = -5;
                chart.Grid.AxisX.LabelProperties.Font = new Font("Tahoma", 8, FontStyle.Regular);
                chart.AnnotationList.GetAnnotation(0).IsSelectable = false;
                chart.AnnotationList.GetAnnotation(1).IsSelectable = false;

                // Chart's other properties
                chart.ChartEventsToEnable.EnableChartMouseClickEvent = false;
                chart.ChartEventsToEnable.EnableMarkerMouseClickEvent = false;
                chart.RunTimeProperties.ContextMenuItems = ContextMenuItem.None;
                //Remove any previously loaded series
                chart.RemoveAllSeries();
            }
            catch (Exception ex)
            {
                this.messageHandler("Error Initializing Charts", ex);
            }
        }

        void addValueToSeries(Series series, float value)
        {
            if (series.DataPointCount(SeriesComponent.Y) == 20)
            {
                for (int i = 0; i < series.DataPointCount(SeriesComponent.Y) - 1; ++i)
                {
                    series.SetValue(SeriesComponent.Y, i, series.GetValue(SeriesComponent.Y, i + 1));
                }
                series.SetValue(SeriesComponent.Y, 19, value);
            }
            else
            {
                series.SetValue(SeriesComponent.Y, series.DataPointCount(SeriesComponent.Y), value);
            }
        }

        private void updateAllSeries()
        {
            this.dVGraphingValues.Table = this.graphingValues.Tables[0];

            DataView temp = this.dVGraphingValues;
            DataRow dR = this.dVGraphingValues.Table.Rows[0];
            DataViewDataProvider graphingDVP = new DataViewDataProvider(this.dVGraphingValues);

            this.seriesVnA.BindComponent(SeriesComponent.Y, graphingDVP, "VnA");
            this.seriesVnB.BindComponent(SeriesComponent.Y, graphingDVP, "VnB");
            this.seriesVnC.BindComponent(SeriesComponent.Y, graphingDVP, "VnC");
            this.seriesVtA.BindComponent(SeriesComponent.Y, graphingDVP, "VtA");
            this.seriesVtB.BindComponent(SeriesComponent.Y, graphingDVP, "VtB");
            this.seriesVtC.BindComponent(SeriesComponent.Y, graphingDVP, "VtC");
            this.seriesSampleNumber.BindComponent(SeriesComponent.Label, graphingDVP, "SampleNumber");

            this.seriesX1.BindComponent(SeriesComponent.Y, graphingDVP, "IA");
            this.seriesX2.BindComponent(SeriesComponent.Y, graphingDVP, "IB");
            this.seriesX3.BindComponent(SeriesComponent.Y, graphingDVP, "IC");

            this.myChartVoltages.Chart.RemoveAllSeries();
            this.myChartVoltages.Chart.AddSeries(this.seriesVnA);
            this.myChartVoltages.Chart.AddSeries(this.seriesVnB);
            this.myChartVoltages.Chart.AddSeries(this.seriesVnC);
            this.myChartVoltages.Chart.AddSeries(this.seriesVtA);
            this.myChartVoltages.Chart.AddSeries(this.seriesVtB);
            this.myChartVoltages.Chart.AddSeries(this.seriesVtC);
            this.myChartVoltages.Chart.AddSeries(this.seriesSampleNumber);
            this.myChartVoltages.Chart.Grid.AxisX.LabelSeries = this.seriesSampleNumber;

            this.myChartLoads.Chart.RemoveAllSeries();
            this.myChartLoads.Chart.AddSeries(this.seriesX1);
            this.myChartLoads.Chart.AddSeries(this.seriesX2);
            this.myChartLoads.Chart.AddSeries(this.seriesX3);
            this.myChartLoads.Chart.AddSeries(this.seriesSampleNumber);
            this.myChartLoads.Chart.Grid.AxisX.LabelSeries = this.seriesSampleNumber;

            this.myChartLoads.Chart.RecalcLayout();
            this.myChartVoltages.Chart.RecalcLayout();
        }

        private void updateChart(bool protector277)
        {
            if (protector277)
            {
                this.myChartVoltages.Chart.Grid.AxisY.MaxAxisValueUser = 300;
                this.myChartVoltages.Chart.Grid.AxisY.MinAxisValueUser = 220;
            }
            else
            {
                this.myChartVoltages.Chart.Grid.AxisY.MaxAxisValueUser = 140;
                this.myChartVoltages.Chart.Grid.AxisY.MinAxisValueUser = 100;
            }
        }

        #endregion

        public void SetTransmitterPhasorValues(PhasorTypes phasorType, long realValue, long imaginaryValue, int p, long rMS)
        {
            float rMSTemp = this.convertRMSI(rMS) * this.CTRatio;
            float percentageTemp;
            float angleTemp;
            float voltageRMSTemp = (float)Math.Sqrt(convertRMSV(realValue) * convertRMSV(realValue) + convertRMSV(imaginaryValue) * convertRMSV(imaginaryValue));

            if (voltageRMSTemp < 5f)
            {
                voltageRMSTemp = 0.0f;
            }
            angleTemp = (float)RelayControlLibrary.RelayModeFunctions.RadiansToDegrees(Math.Atan((double)(this.convertRMSV(imaginaryValue) / this.convertRMSV(realValue))));

            if (this.protector277)
            {
                voltageRMSTemp *= this.convert277;
            }

            if (realValue == 0)
            {
                if (imaginaryValue < 0)
                    angleTemp = -90;
                else
                    angleTemp = 90;
            }
            else if (realValue < 0)
            {
                if (imaginaryValue > 0)
                {
                    angleTemp = 180 + angleTemp;
                }
                else
                {
                    angleTemp = angleTemp - 180;
                }
            }

            if (angleTemp < 0)
            {
                angleTemp = 360 + angleTemp;
            }

            percentageTemp = this.convertRMSI(rMS) / .05f;
            if (percentageTemp < .342f)
            {
                percentageTemp = 0.0f;
                rMSTemp = 0.0f;
            }

            percentageTemp = percentageTemp * (float.Parse(cTMult) * (float)0.01); //   CT / (mult * 1/100)

            switch (phasorType)
            {
                case PhasorTypes.VnA:
                    if (!this.firstDataSeen)
                        break;

                    this.textBoxVnA.Text = voltageRMSTemp.ToString("0.00");
                    this.workingRow["VnA"] = voltageRMSTemp;

                    break;
                case PhasorTypes.VnB:
                    if (!this.firstDataSeen)
                        break;
                    this.textBoxVnB.Text = voltageRMSTemp.ToString("0.00");
                    this.workingRow["VnB"] = voltageRMSTemp;
                    break;
                case PhasorTypes.VnC:
                    if (!this.firstDataSeen)
                        break;
                    this.textBoxVnC.Text = voltageRMSTemp.ToString("0.00");
                    this.workingRow["VnC"] = voltageRMSTemp;
                    break;
                case PhasorTypes.VtA:
                    this.firstDataSeen = true;

                    if (this.graphingValues.Tables[0].Rows.Count == 20)
                        this.graphingValues.Tables[0].Rows.RemoveAt(0);
                    this.workingRow = this.graphingValues.Tables[0].Rows.Add();
#if !DEBUG
                    if (!this.GEEnabled)
#endif
                    {
                        this.textBoxVtA.Text = voltageRMSTemp.ToString("0.00");
                        this.workingRow["VtA"] = voltageRMSTemp;
                    }
                    this.workingRow["SampleNumber"] = this.sampleNumber++;
                    break;
                case PhasorTypes.VtB:
                    if (!this.firstDataSeen)
                        break;
#if !DEBUG
                    if (!this.GEEnabled)
#endif
                    {
                        this.textBoxVtB.Text = voltageRMSTemp.ToString("0.00");
                        this.workingRow["VtB"] = voltageRMSTemp;
                    }
                    break;
                case PhasorTypes.VtC:
                    if (!this.firstDataSeen)
                        break;
#if !DEBUG
                    if (!this.GEEnabled)
#endif
                    {
                        this.textBoxVtC.Text = voltageRMSTemp.ToString("0.00");
                        this.workingRow["VtC"] = voltageRMSTemp;
                    }
                    break;
                case PhasorTypes.IA:
                    if (!this.firstDataSeen)
                        break;
                    this.textBoxX1Percent.Text = percentageTemp.ToString("0.00");
                    this.textBoxX1Amp.Text = rMSTemp.ToString("0.00");
                    this.workingRow["IA"] = percentageTemp;
                    break;
                case PhasorTypes.IB:
                    if (!this.firstDataSeen)
                        break;
                    this.textBoxX2Percent.Text = percentageTemp.ToString("0.00");
                    this.textBoxX2Amp.Text = rMSTemp.ToString("0.00");
                    this.workingRow["IB"] = percentageTemp;
                    break;
                case PhasorTypes.IC:
                    if (!this.firstDataSeen)
                        break;
                    this.textBoxX3Percent.Text = percentageTemp.ToString("0.00");
                    this.textBoxX3Amp.Text = rMSTemp.ToString("0.00");
                    this.workingRow["IC"] = percentageTemp;
                    this.updateAllSeries();
                    break;
                case PhasorTypes.PA:
                    if (!this.firstDataSeen)
                        break;
                    if (Convert.ToDouble(this.textBoxX1Percent.Text) == 0)
                        this.textBoxPhaseAngleA.Text = "No Read";
                    else
                        this.textBoxPhaseAngleA.Text = angleTemp.ToString("0");
                    break;
                case PhasorTypes.PB:
                    if (!this.firstDataSeen)
                        break;
                    if (Convert.ToDouble(this.textBoxX2Percent.Text) == 0)
                        this.textBoxPhaseAngleB.Text = "No Read";
                    else
                        this.textBoxPhaseAngleB.Text = angleTemp.ToString("0");
                    break;
                case PhasorTypes.PC:
                    if (!this.firstDataSeen)
                        break;
                    if (Convert.ToDouble(this.textBoxX3Percent.Text) == 0)
                        this.textBoxPhaseAngleC.Text = "No Read";
                    else
                        this.textBoxPhaseAngleC.Text = angleTemp.ToString("0");
                    this.firstDataSeen = false;
                    break;
                default:
                    break;
            }
        }

        private void setCTRatio(int ratio)
        {
            int displayValue = ratio * 5;

            this.textBoxTransmitterCTRatio.Text = displayValue.ToString() + "/5";
        }

        private void setFrequency(Frequencies freq)
        {
            this.checkBoxFrequenceBlue.Visible = false;
            this.checkBoxFrequencyGreen.Visible = false;
            this.checkBoxFrequencyRed.Visible = false;
            this.checkBoxFrequencyYellow.Visible = false;
            switch (freq)
            {
                case Frequencies.Blue:
                    this.checkBoxFrequenceBlue.Visible = true;
                    break;
                case Frequencies.Green:
                    this.checkBoxFrequencyGreen.Visible = true;
                    break;
                case Frequencies.Red:
                    this.checkBoxFrequencyRed.Visible = true;
                    break;
                case Frequencies.Yellow:
                    this.checkBoxFrequencyYellow.Visible = true;
                    break;
                default:
                    throw new Exception("Bad Frequency For Monitoring page");
            }
        }

        private float convertRMSV(long rMS)
        {
            float returnFloat;

            returnFloat = (float)rMS;                     //convert to a float
            returnFloat = returnFloat * (float)Constants.TwelveFracBits;

            return returnFloat;
        }

        private float convertRMSI(long rMS)
        {
            float returnFloat;

            returnFloat = (float)rMS;                     //convert to a float
            returnFloat = returnFloat * (float)Constants.SixteenFracBits;

            return returnFloat;
        }

        private void buttonStartMonitoring_Click(object sender, EventArgs e)
        {
            this.startTransmitterMonitoring();
        }

        private void buttonPauseMonitoring_Click(object sender, EventArgs e)
        {
            this.pauseTransmitterMonitoring();
        }

        private bool gEEnabled = false;
        public bool GEEnabled
        {
            get { return this.gEEnabled; }
            set
            {
                this.gEEnabled = value;
                this.setCustomer();
            }
        }

        private bool waterBugActive = false;
        public bool WaterBugActive
        {
            get { return this.waterBugActive; }
            set
            {
                this.waterBugActive = value;
            }
        }
    }

    public class TransmitterMonitoringEventArgs : EventArgs
    {
        public TransmitterMonitoringEventArgs(bool b)
        {
            this.EnableTransmitting = b;
        }

        public bool EnableTransmitting = false;
    }

    public class TransmitterMeterValues
    {
        private int rawValue;
        private int oilTemp;
        private int dGITemp;
        private int pressure;

        public int RawValue
        {
            get { return this.rawValue; }
            set
            {
                this.rawValue = value;
                this.setPressure(value);
                this.setDGITemp(value);
                this.setOilTemp(value);
            }
        }

        public int OilTemp
        {
            get { return this.oilTemp; }
        }

        public int DGITemp
        {
            get { return this.dGITemp; }
        }

        public int Pressure
        {
            get { return this.pressure; }
        }

        private void setOilTemp(int value)
        {
            this.oilTemp = this.getOilTemperatureFromAnalog(value);
        }

        private void setDGITemp(int value)
        {
            this.dGITemp = (int)value >= this.AnalogLookup.Length ? 155 : AnalogLookup[value];
        }

        private void setPressure(int value)
        {
            this.pressure = this.getTankPressureFromAnalog(value);
        }

        private int getOilTemperatureFromAnalog(int bAN1)
        {
#if Enmax //consider adding for chicago as well
            double oil_temp = .0393701 * bAN1 * 32;
            return (int)(oil_temp);
#else
            double oil_temp = 1.5993 * bAN1 - 5.0982; //coned gauge only 
            //oil_temp += 0.5;
            return (int)(oil_temp);
#endif
        }

        private int getTankPressureFromAnalog(int bAN2)
        {
            double tank_pressure = 0.1754 * bAN2 - 2.11; //coned gauge only
            //tank_pressure += 0.5;
            return (int)(tank_pressure);
        }
#if Enmax
        private int[] AnalogLookup = 
		{ // 15 columns
			0,   1,   2,   3,   4,   5,  10,  13,  15,  18,  20,  23,  25,  27,  29, // row 1
			30,  32,  34,  35,  37,  38,  39,  40,  41,  42,  43,  44,  45,  46,  47, // row 2
			48,  49,  50,  51,  52,  53,  54,  55,  56,  57,  58,  59,  60,  60,  61, // row 3
			62,  63,  64,  65,  66,  67,  68,  69,  70,  70,  71,  72,  73,  74,  75, // row 4
			75,  76,  77,  78,  79,  80,  80,  81,  82,  83,  84,  85,  85,  86,  87, // row 5
			88,  89,  90,  91,  92,  93,  94,  95,  97,  98,  99, 100, 102, 103, 104, // row 6
			105, 107, 108, 109, 110, 112, 114, 115, 117, 119, 120, 122, 124, 125, 128, // row 7
			130, 133, 135, 138, 140, 143, 145, 150, 155 // plus 9
		};
#else
        private int[] AnalogLookup = 
		{ // 15 columns
			0,   0,   0,   0,   3,   5,  10,  13,  15,  18,  20,  23,  25,  27,  29, // row 1
			30,  32,  34,  35,  37,  38,  39,  40,  41,  42,  43,  44,  45,  46,  47, // row 2
			48,  49,  50,  51,  52,  53,  54,  55,  56,  57,  58,  59,  60,  60,  61, // row 3
			62,  63,  64,  65,  66,  67,  68,  69,  70,  70,  71,  72,  73,  74,  75, // row 4
			75,  76,  77,  78,  79,  80,  80,  81,  82,  83,  84,  85,  85,  86,  87, // row 5
			88,  89,  90,  91,  92,  93,  94,  95,  97,  98,  99, 100, 102, 103, 104, // row 6
			105, 107, 108, 109, 110, 112, 114, 115, 117, 119, 120, 122, 124, 125, 128, // row 7
			130, 133, 135, 138, 140, 143, 145, 150, 155 // plus 9
		};
#endif

    }
}
