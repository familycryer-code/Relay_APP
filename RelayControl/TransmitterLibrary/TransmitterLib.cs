using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;

namespace TransmitterLibrary
{
  /// <summary>
  /// enumDgNetCommPort is for the convienent COMM port naming.
  /// </summary>
  public enum enumDgNetCommPort
  {
    COMM1 = 1,
    COMM2 = 2,
    COMM3 = 3,
    COMM4 = 4,
    COMM5 = 5,
    COMM6 = 6,
    COMM7 = 7,
    COMM8 = 8
  }
  public enum enumUploadByte
  {
    CMD_TAG = 0,        // FIXED: 65, 41H
    CMD_KEY1 = 1,       // FIXED: 85, 55H
    CMD_KEY2 = 2,       // FIXED: 170, AAH
    ID_LSB = 3,         // ID, LSB
    ID_MSB = 4,         // ID, MSB
    COLOR = 5,          // FREQUENCY
    CT_LSB = 6,         // CT, LSB
    CT_MSB = 7,         // CT, MSB
    FPPS = 8,           // FLAG POSTING POSITIONS
    ALARM_FLAGS = 9,    // FLAG ALARMS ENABLE/DISABLE
    ALARM_OTHER = 10,   // OVER VOLT, PUMP, AN1, AN2, UNDER VOLT, CURRENT ALARMS ENABLE/DISABLE
    CURTH = 11,         // CURRENT THRESHOLD HIGH
    CURTL = 12,         // CURRENT THRESHOLD LOW
    VOLTH = 13,         // VOLTAGE THRESHOLD HIGH
    VOLTL = 14,         // VOLTAGE THRESHOLD LOW  
    AN1TH = 15,         // ANALOG1 THRESHOLD
    AN2TH = 16,         // ANALOG2 THRESHOLD
    ANAS = 17,          // ANALOG ALARM SETTINGS, SUCH AS OVER/UNDER
    TYPE1_FREQ = 18,    // TYPE1 MESSAGING FREQUENCY (OPERATE MODE)
    MUXBOX_FREQ = 19,    // MUXBOX MESSAGING FREQUENCY
    TYPE2_FREQ = 20,     // TYPE2 MESSAGING FREQUENCY
    CONFIG_FREQ = 21,    // CONFIG/CONFIG1 MESSAGING FREQUENCY
    ALARM_BST_CNT = 22,  // ALARM BURST COUNT
    ALARM_BST_INT = 23,  // ALARM BURST INTERVAL
    CONFIG_BST_CNT = 24, // COFIG/CONFIG1 BURST COUNT
    CONFIG_BST_INT = 25, // CONFIG/CONFIG1 BURST INTERVAL
    VNA_CALIB = 26,      // VNA CALIBRATION
    VNB_CALIB = 27,      // VNB CALIBRATION
    VNC_CALIB = 28,      // VNC CALIBRATION
    VTA_CALIB = 29,      // VTA CALIBRATION
    VTB_CALIB = 30,      // VTB CALIBRATION
    VTC_CALIB = 31,      // VTC CALIBRATION
    IA_CALIB = 32,       // IA CALIBRATION
    IB_CALIB = 33,       // IB CALIBRATION
    IC_CALIB = 34,       // IC CALIBRATION
    TEMP_CALIB = 35,     // TEMPERATURE CALIBRATION
    PHA_CALIB = 36,      // PHA CALIBRATION, DEFAULT: 0
    PHB_CALIB = 37,      // PHB CALIBRATION, DEFAULT: 0
    MESSAGE_MODE = 38,   // MESSAGING MODE: EMULATOR, EXPANDED
    PHC_CALIB = 39,      // PHC CALIBRATION: 0, 00H
    RESERVE1 = 40,       // RESERVED 1: 0, 00H, USED BY CARL INTERNALLY
    RESERVE2 = 41,       // RESERVED 2: 100, 64H, USED BY CARL INTERNALLY
    SN_LSB = 42,         // Serial Number LSB
    SN_MSB = 43,         // Serial Number MSB
    FLAG_SOURCE = 44     // FLAG SOURCE
  }
  public enum enumDownloadByte
  {
    ID_LSB,         // ID, LSB
    ID_MSB,         // ID, MSB
    COLOR,          // FREQUENCY
    CT_LSB,         // CT, LSB
    CT_MSB,         // CT, MSB
    FPPS,           // FLAG POSTING POSITIONS
    ALARM_FLAGS,    // FLAG ALARMS ENABLE/DISABLE
    ALARM_OTHER,    // OVER VOLT, PUMP, AN1, AN2, UNDER VOLT, CURRENT ALARMS ENABLE/DISABLE
    CURTH,          // CURRENT THRESHOLD HIGH
    CURTL,          // CURRENT THRESHOLD LOW
    VOLTH,          // VOLTAGE THRESHOLD HIGH
    VOLTL,          // VOLTAGE THRESHOLD LOW
    AN1TH,          // ANALOG1 THRESHOLD
    AN2TH,          // ANALOG2 THRESHOLD
    ANAS,           // ANALOG ALARM SETTINGS
    TYPE1_FREQ,     // TYPE1 MESSAGING FREQUENCY
    MUXBOX_FREQ,    // MUXBOX MESSAGING FREQUENCY
    TYPE2_FREQ,     // TYPE2 MESSAGING FREQUENCY
    CONFIG_FREQ,    // CONFIG/CONFIG1 MESSAGING FREQUENCY
    ALARM_BST_CNT,  // ALARM BURST COUNT
    ALARM_BST_INT,  // ALARM BURST INTERVAL
    CONFIG_BST_CNT, // COFIG/CONFIG1 BURST COUNT
    CONFIG_BST_INT, // CONFIG/CONFIG1 BURST INTERVAL
    VNA_CALIB,      // VNA CALIBRATION
    VNB_CALIB,      // VNB CALIBRATION
    VNC_CALIB,      // VNC CALIBRATION
    VTA_CALIB,      // VTA CALIBRATION
    VTB_CALIB,      // VTB CALIBRATION
    VTC_CALIB,      // VTC CALIBRATION
    IA_CALIB,       // IA CALIBRATION
    IB_CALIB,       // IB CALIBRATION
    IC_CALIB,       // IC CALIBRATION
    TEMP_CALIB,     // TEMPERATURE CALIBRATION
    PHA_CALIB,      // PHA CALIBRATION
    PHB_CALIB,      // PHB CALIBRATION
    MESSAGE_MODE,   // MESSAGING MODE: EMULATOR, EXPANDED
    PHC_CALIB,      // PHC CALIBRATION
    RESERVE1,       // RESERVED 1: 0, 00H, USED BY CARL INTERNALLY
    RESERVE2,       // RESERVED 2: 100, 64H, USED BY CARL INTERNALLY
    SN_LSB,         // Serial Number LSB
    SN_MSB,         // Serial Number MSB
    FLAG_SOURCE     // FLAG SOURCE
  }
  public enum enumMonitoringByte
  {
    ID_LSB,         // ID, LSB
    ID_MSB,         // ID, MSB
    COLOR,          // FREQUENCY
    CT_LSB,         // CT, LSB
    CT_MSB,         // CT, MSB
    IA_INTEGER,     // IA, THE WHOLE NUMBER   
    IA_DECIMAL,     // IA, THE DECIMAL NUMBER IN HUNDREDTHS
    IB_INTEGER,     // IB, THE WHOLE NUMBER   
    IB_DECIMAL,     // IB, THE DECIMAL NUMBER IN HUNDREDTHS
    IC_INTEGER,     // IC, THE WHOLE NUMBER   
    IC_DECIMAL,     // IC, THE DECIMAL NUMBER IN HUNDREDTHS
    VNA_LSB,        // VNA, LSB: THE MOST LSB IS 1/4
    VNA_MSB,        // VNA, MSB
    VNB_LSB,        // VNB, LSB: THE MOST LSB IS 1/4
    VNB_MSB,        // VNB, MSB
    VNC_LSB,        // VNC, LSB: THE MOST LSB IS 1/4
    VNC_MSB,        // VNC, MSB
    VTA_LSB,        // VTA, LSB: THE MOST LSB IS 1/4
    VTA_MSB,        // VTA, MSB
    VTB_LSB,        // VTB, LSB: THE MOST LSB IS 1/4
    VTB_MSB,        // VTB, MSB
    VTC_LSB,        // VTC, LSB: THE MOST LSB IS 1/4
    VTC_MSB,        // VTC, MSB
    AN1,            // ANALOG1
    AN2,            // ANALOG2
    XMTR_PWR,       // TRANSMITTER OUTPUT POWER
    FPPS,           // FLAG POSTING POSITIONS
    ALARMS,         // VOLTAGE, PHASE ERROR ALARMS
    PHA_LSB,        // PHA, LSB
    PHA_MSB,        // PHA, MSB
    PHB_LSB,        // PHB, LSB
    PHB_MSB,        // PHB, MSB
    PHC_LSB,        // PHC, LSB
    PHC_MSB,        // PHC, MSB
    TEMP,           // Analog3, temperature
    LEARNED         // ABF TX Features
  }
  public enum enumMessageType
  {
    TYPE1,
    MUXBOX,
    TYPE2,
    CONFIG,
    CONFIG1
  }
  public enum enumCalibReadingByte
  {
    IA_INTEGER,     // IA, THE WHOLE NUMBER   
    IA_DECIMAL,     // IA, THE DECIMAL NUMBER IN HUNDREDTHS
    IB_INTEGER,     // IB, THE WHOLE NUMBER   
    IB_DECIMAL,     // IB, THE DECIMAL NUMBER IN HUNDREDTHS
    IC_INTEGER,     // IC, THE WHOLE NUMBER   
    IC_DECIMAL,     // IC, THE DECIMAL NUMBER IN HUNDREDTHS
    VNA_LSB,        // VNA, LSB: THE MOST LSB IS 1/4
    VNA_MSB,        // VNA, MSB
    VNB_LSB,        // VNB, LSB: THE MOST LSB IS 1/4
    VNB_MSB,        // VNB, MSB
    VNC_LSB,        // VNC, LSB: THE MOST LSB IS 1/4
    VNC_MSB,        // VNC, MSB
    VTA_LSB,        // VTA, LSB: THE MOST LSB IS 1/4
    VTA_MSB,        // VTA, MSB
    VTB_LSB,        // VTB, LSB: THE MOST LSB IS 1/4
    VTB_MSB,        // VTB, MSB
    VTC_LSB,        // VTC, LSB: THE MOST LSB IS 1/4
    VTC_MSB,        // VTC, MSB
    TEMP            // TEMPERATURE
  }
  public class TransmitterLib
  {
    public TransmitterLib()
    {
    }
    // private static string ApplicationRoot = @"C:\DGI Systems\Transmitter\wbext\";
    private static string ApplicationRoot = @"C:\DGI Systems\Transmitter\";
    //
    // Application Folders
    //
    public static string DefaultAdminFolder = ApplicationRoot + @"adm\";
    public static string DefaultLogFileFolder = ApplicationRoot + @"log\";
    public static string DefaultTempFileFolder = ApplicationRoot + @"tmp\";
    public static string DefaultUserFileFolder = ApplicationRoot + @"usr\";

    public static string DefaultCEFolder = @"C:\CE\";
    public static string TxFirmwareFile = DefaultAdminFolder + "Xmitter.dll";
    

    public const byte STX = 15;
    public const byte ETX = 4;
    public const byte DLE = 5;
    public const byte DC4 = 20;
    public const byte CR = 13;

    public const int TxSoftwareVersionByteCount = 8; // INCLUDING "DC4", ex: 08-0707
    public const int CalibReadingByteCount = 20;  // INCLUDING "DC4"

    //public const int Calib_Timer_Interval = 2; // 2 seconds
    //public const int VaultMonitor_Timer_Interval = 4; // 3 seconds
    public static int GetCalibTimerInterval()
    {
      return 3;
    }
    public static int GetVaultMonitorTimerInterval()
    {
      return 4;
    }

    public static int GetUploadByteCount(float txVersion) // with A Key1 Key2, Download bytes + 3 bytes
    {
      if (txVersion == 7.2f)
        return 46; // three extra bytes: serial number + 1byte for flag source
      else if (txVersion == 7.1f) // two extra bytes: serial number, 
        return 45;
      else
        return 43;
    }
    public static int GetDownloadByteCount(float txVersion)
    {
      if (txVersion == 7.2f) // // three extra bytes: serial number + 1byte for flag source
        return 43;
      else if (txVersion == 7.1f) // two extra bytes: serial number
        return 42;
      else
        return 40;
    }
    public static int GetMonitoringByteCount(float txVersion)
    {
      if (txVersion >= 7.1f) // two extra bytes: learned, temperature
        return 37;
      else
        return 35;
    }

    //public const int UploadByteCount = 43;        // INCLUDING "DC4"
    //public const int DownloadByteCount = 40;      // INCLUDING "DC4"
    //public const int MonitoringByteCount = 35;    // INCLUDING "DC4"

    public static string GetDONSoftVersion()
    {
      return "Version 5.0.2";
    }
    public static string SyncDateTimeString = "TIME " + string.Format("{0:MM/dd/yy HH:mm}", DateTime.Now);
    public const int AnalogLookupLength = 114;
    /// <summary>
    /// The Analog Lookup Table, used for Digitalgrid Temperature sensor model: DG500
    /// </summary>
    public static int[] AnalogLookup = 
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
    public static int GetOilTemperatureFromAnalog(byte bAN1)
    {
      double oil_temp = 1.5993 * bAN1 - 5.0982;
      //oil_temp += 0.5;
      return (int)(oil_temp);
    }
    public static int GetTankPressureFromAnalog(byte bAN2)
    {
      double tank_pressure = 0.1754 * bAN2 - 2.11;
      //tank_pressure += 0.5;
      return (int)(tank_pressure);
    }

    public static string GetCalibrationSound()
    {
      return DefaultAdminFolder + "LASER.WAV"; //  CHIMES.WAV
    }
    public static string GetMonitoringSound()
    {
      return DefaultAdminFolder + "NOTIFY.WAV"; //"LASER.WAV"; //  CHIMES.WAV
    }
    public static string GetLogFile()
    {
      return DefaultLogFileFolder + "myTransmitter.log";
    }
    public static string GetParameterSchemaFile()
    {
      return DefaultAdminFolder + "Parameter.xsd";
    }
    public static string GetUploadSchemaFile()
    {
      return DefaultAdminFolder + "Upload.xsd";
    }
    public static string GetCalibReadingSchemaFile()
    {
      return DefaultAdminFolder + "CalibrationReading.xsd";
    }
    public static string GetVaultMonitorSchemaFile()
    {
      return DefaultAdminFolder + "VaultMonitor.xsd";
    }
    public static string GetVaultGraphSchemaFile()
    {
      return DefaultAdminFolder + "VaultGraphics.xsd";
    }
    public static string GetBAEUploadSchemaFile()
    {
      return DefaultAdminFolder + "baeUpload.xsd";
    }
    public static string GetBAEParameterSchemaFile()
    {
      return DefaultAdminFolder + "baeParameter.xsd";
    }

    public static string GetBAEDefaultXmlFile()
    {
      return DefaultAdminFolder + "baeDefaultSettings.xml";
    }

    public static string GetDefaultXmlFile()
    {
      return DefaultAdminFolder + "DefaultSettings.xml";
    }
    public static string GetCEDefaultXmlFile()
    {
      return DefaultAdminFolder + "ceDefaultSettings.xml";
    }

    public static string GetSettingsXmlFile()
    {
      return DefaultTempFileFolder + "TransmitterSettings.xml";
    }
    public static string GetDownloadXmlFile()
    {
      return DefaultTempFileFolder + "DownloadSettings.xml";
    }
    public static string GetBAEDownloadXmlFile()
    {
      return DefaultTempFileFolder + "baeDownloadSettings.xml";
    }
    public static string GetUploadXmlFile()
    {
      return DefaultTempFileFolder + "UploadSettings.xml";
    }
    public static string GetBAEUploadXmlFile()
    {
      return DefaultTempFileFolder + "baeUploadSettings.xml";
    }
    public static string GetCalibrationReadingXmlFile()
    {
      return DefaultTempFileFolder + "CalibrationReading.xml";
    }
    public static string GetVaultMonitorXmlFile()
    {
      return DefaultTempFileFolder + "VaultMonitor.xml";
    }
    public static string GetVaultGraphicsXmlFile()
    {
      return DefaultTempFileFolder + "VaultGraphics.xml";
    }
    public static string GetDefaultTempTextFile()
    {
      return DefaultTempFileFolder + "temp.txt";
    }
    public static string GetDefaultTempXmlFile()
    {
      return DefaultTempFileFolder + "temp.xml";
    }
    public static void CreateDefaultSettings()
    {
      DataSet dsDefault = new DataSet();
      dsDefault.ReadXmlSchema(GetParameterSchemaFile());
      DataTable dtDefault = dsDefault.Tables[0];
      DataRow drDefault = dtDefault.NewRow();
      drDefault["ID"] = 1023;
      drDefault["MESSAGE_MODE"] = 1; // Expanded
      drDefault["COLOR"] = "BLUE (50kHz)";
      drDefault["CT"] = 120;
      drDefault["FPPS_A"] = false; // Open
      drDefault["FPPS_B"] = true; // Close
      drDefault["FPPS_C"] = true; // Close
      drDefault["FPPS_D"] = true; // Close
      drDefault["FPPS_E"] = true; // Close
      drDefault["FPPS_F"] = true; // Close
      drDefault["FPPS_G"] = true; // Close
      drDefault["FPPS_H"] = true; // Close
      drDefault["ALARM_A"] = false; // Disabled
      drDefault["ALARM_B"] = false; // Disabled
      drDefault["ALARM_C"] = false; // Disabled
      drDefault["ALARM_D"] = false; // Disabled
      drDefault["ALARM_E"] = false; // Disabled
      drDefault["ALARM_F"] = false; // Disabled
      drDefault["ALARM_G"] = false; // Disabled
      drDefault["ALARM_H"] = false; // Disabled
      drDefault["ALARM_VOLH"] = false; // Disabled
      drDefault["ALARM_VOLL"] = false; // Disabled
      drDefault["ALARM_CURH"] = false; // Disabled
      drDefault["ALARM_PUMP"] = false; // Disabled
      drDefault["ALARM_AN1"] = false; // Disabled
      drDefault["ALARM_AN2"] = false; // Disabled
      drDefault["CURTH"] = 100;
      drDefault["CURTL"] = 75;
      drDefault["VOLTH"] = 135;
      drDefault["VOLTL"] = 110;
      drDefault["AN1TH"] = 100;
      drDefault["AN2TH"] = 100;
      drDefault["ANAS"] = 3;
      drDefault["TYPE1_FREQ"] = 1; // @every 160 seconds
      drDefault["MUXBOX_FREQ"] = 15; // @every 15 minutes
      drDefault["TYPE2_FREQ"] = 3; // @every 3 hours
      drDefault["CONFIG_FREQ"] = 23; // @every 23 hours
      drDefault["ALARM_BST_CNT"] = 4; // 4 bursts
      drDefault["ALARM_BST_INT"] = 20; // 20 seconds
      drDefault["CONFIG_BST_CNT"] = 4; // 4 bursts
      drDefault["CONFIG_BST_INT"] = 30; // 30 seconds
      drDefault["VNA_CALIB"] = 0;
      drDefault["VNB_CALIB"] = 0;
      drDefault["VNC_CALIB"] = 0;
      drDefault["VTA_CALIB"] = 0;
      drDefault["VTB_CALIB"] = 0;
      drDefault["VTC_CALIB"] = 0;
      drDefault["IA_CALIB"] = 0;
      drDefault["IB_CALIB"] = 0;
      drDefault["IC_CALIB"] = 0;
      drDefault["TEMP_CALIB"] = 0;
      dtDefault.Rows.Add(drDefault);
      dsDefault.WriteXml(GetDefaultXmlFile(), XmlWriteMode.WriteSchema);
    }

    public static byte NegByte2TwoComplement(sbyte negByte)
    {
      try
      {
        byte b = Convert.ToByte(Math.Abs(negByte));
        b = Convert.ToByte(b ^ 0xff);
        b++;
        return b;
      }
      catch (Exception Exp)
      {
        LogTxEvents("NegByte2TwoComplement()> " + Exp.Message);
        return 0;
      }
    }

    public static void LogTxEvents(string sMessage)
    {
      try
      {
        // File.AppendText: if the specified file does not exist, it is created. If the file
        // does exist, write operations to the StreamWriter append text to the file.
        using (StreamWriter logOut = File.AppendText(GetLogFile()))
        {
          // Time-Stamps
          logOut.WriteLine(String.Format("{0:d-MMM-yy HH:mm:ss}", DateTime.Now));
          logOut.Write(sMessage + "\r\n\r\n");
          logOut.Close();
        }
      }
      catch
      {
      }
    }
    public static void MaintainLogFiles()
    {
      try
      {
        if (!Directory.Exists(DefaultLogFileFolder))
          Directory.CreateDirectory(DefaultLogFileFolder);
        if (!Directory.Exists(DefaultUserFileFolder))
          Directory.CreateDirectory(DefaultUserFileFolder);
        if (!Directory.Exists(DefaultTempFileFolder))
          Directory.CreateDirectory(DefaultTempFileFolder);
        //if (!Directory.Exists(DefaultCEFolder))
        //  Directory.CreateDirectory(DefaultCEFolder);
        if (File.Exists(GetLogFile()))
        {
          FileInfo logFileInfo = new FileInfo(GetLogFile());
          if (logFileInfo.Length > 8388608) // 8M
          {
            // Keep at most 10 Log Files
            if (File.Exists(GetLogFile() + ".009"))
              File.Delete(GetLogFile() + ".009");
            for (int i = 8; i > 0; i--)// .008->.007, ..., .001->.002
            {
              if (File.Exists(GetLogFile() + "." + String.Format("{0:d3}", i)))
                File.Move(GetLogFile() + "." + String.Format("{0:d3}", i),
                  GetLogFile() + "." + String.Format("{0:d3}", i + 1));
            }
            // Finally, 
            File.Move(GetLogFile(), GetLogFile() + ".001");
          } // > 8M
        } // File.Exists
      }
      catch (Exception Exp)
      {
        LogTxEvents("TransmitterLib.dll>MaintainLogFiles> " + Exp.Message);
      }
    }

    private static string GetComPortXmlFile()
    {
      return DefaultAdminFolder + "ComPortInfo.xml";
    }
    public static enumDgNetCommPort GetCommPort()
    {
      try
      {
        // Read ComPortInfo.xml to a Dataset
        DataSet myComPortSet = new DataSet();
        myComPortSet.ReadXml(GetComPortXmlFile(), XmlReadMode.ReadSchema);
        int iPortNumber = Convert.ToInt16(myComPortSet.Tables[0].Rows[0][0]);
        myComPortSet = null;
        enumDgNetCommPort pn;
        switch (iPortNumber)
        {
          case 1:
            pn = enumDgNetCommPort.COMM1;
            break;
          case 2:
            pn = enumDgNetCommPort.COMM2;
            break;
          case 3:
            pn = enumDgNetCommPort.COMM3;
            break;
          case 4:
            pn = enumDgNetCommPort.COMM4;
            break;
          case 5:
            pn = enumDgNetCommPort.COMM5;
            break;
          case 6:
            pn = enumDgNetCommPort.COMM6;
            break;
          case 7:
            pn = enumDgNetCommPort.COMM7;
            break;
          case 8:
            pn = enumDgNetCommPort.COMM8;
            break;
          default:
            pn = enumDgNetCommPort.COMM1;
            break;
        }
        return pn;
      }
      catch (Exception Exp)
      {
        LogTxEvents("TransmitterLibrary.dll>GetCommPort> " + Exp.Message);
        return enumDgNetCommPort.COMM1;
      }
    }
    public static void SaveCommPort(enumDgNetCommPort pn)
    {
      try
      {
        // Read ComPortInfo.xml to a Dataset
        DataSet myComPortSet = new DataSet();
        myComPortSet.ReadXml(GetComPortXmlFile(), XmlReadMode.ReadSchema);
        int iPortNumber = 1;
        switch (pn)
        {
          case enumDgNetCommPort.COMM1:
            iPortNumber = 1;
            break;
          case enumDgNetCommPort.COMM2:
            iPortNumber = 2;
            break;
          case enumDgNetCommPort.COMM3:
            iPortNumber = 3;
            break;
          case enumDgNetCommPort.COMM4:
            iPortNumber = 4;
            break;
          case enumDgNetCommPort.COMM5:
            iPortNumber = 5;
            break;
          case enumDgNetCommPort.COMM6:
            iPortNumber = 6;
            break;
          case enumDgNetCommPort.COMM7:
            iPortNumber = 7;
            break;
          case enumDgNetCommPort.COMM8:
            iPortNumber = 8;
            break;
          default:
            iPortNumber = 1;
            break;
        }
        myComPortSet.Tables[0].Rows[0][0] = iPortNumber;
        myComPortSet.WriteXml(GetComPortXmlFile(), XmlWriteMode.WriteSchema);
        myComPortSet = null;
      }
      catch (Exception Exp)
      {
        LogTxEvents("TransmitterLibrary.dll>SaveCommPort> " + Exp.Message);
      }

    }

  }
}
