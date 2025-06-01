using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Linq;

namespace RelayControlLibrary
{
    #region Structs
    public struct RelayStatusRegister
    {
        public bool MonitorPhasors;
        public bool MathError;
        public bool BadOffset;
        public bool SafeServiceEnabled;
        public bool MathTimeOver;
        public bool Pumping;
        public bool PumpProtectEnabled;
        public bool InitComplete;
        public bool SequenceRelay;
        public bool ACB;
        public bool AutoPhasingDetect;
    }

    public struct RelayFlagsRegister
    {
        public bool Open;
        public Int16 TripCycles;
        public bool PowerSave;
        public bool Tripping;
        public bool FloatCondition;
        public bool BlockedClose;
        public bool BlockedOpen;
        public bool PhasingOkay;
        public bool CalibrationMode;
    }
    #endregion

    #region Enums

    public enum ProgramStates
    {
        CheckingForRelay,
        FailedToFindRelay,
        DownloadingAllParameters,
        Running
    }

    public enum BlockModes
    {
        Blocked,
        Unblocked,
        BlockedClosed,
        UnblockedClosed
    }

    public enum CloseModes
    {
        CircleClose,
        Normal,
        RelaxClose,
        CircleAndRelax,
        PermissiveClose,
        None
    }

    public enum ComputedPhasorGroups
    {
        Power,
        Sequence,
        Ieff,
        DifferentialVoltages,
        DifferentialSequence
    }

    public enum CloseCurveTypes
    {
        Vertical,
        Horizontal
    }

    public enum Customers
    {
        NonConEd,
        NonConEdGE,
        ConEdison,
        Memphis,
        DIGITALGRID,
        DIGITALGRIDDNP,
        DNPwithPLC,
        SMUD,
        PEPCO,
        Dominion,
        Atlanta,
        Oncor,
        LondonH,
        SCE,
        TorontoHydro,
        None
    }

    public enum EventTypes
    {
        Trip,
        Close,
        Float,
        Transient,
        InInsensitiveRegion,
        LowVoltage,
        UnidentifiedEvent,
        NoEvent
    }

    public enum Frequencies
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    public enum IncomingCommCommands
    {
        ArcFaultData,
        Boot,
        ReceiverStatusCode,
        CalibrationComplete,
        CalibrationConstants,
        CurrentTime,
        EventDataPacket,
        EventTimes,
        FPGARevision,
        GeneralCommand,
        LiveDataPacket,
        PhasorUpdate,
        RelayParameters,
        RelayRegisters,
        RelayRevision,
        RelayStatusBits,
        Revision,
        SineGraphValue,
        //TerminationChar,
        Temperature,
        TransmitterSettings,
        TransmitterMonitor,
        TripOrCloseEvent,
        FFTValue,
        DNPData,
        DNPSAv5,
        SafeService,
        ShortRangeStrength,
        ShortRangeTransmit,
        DNPMessage1,
        DNPMessage2,
        DNPMessage3,
        DNPMessage4,
        LowVoltageThresReceived,
        NoMemFix,
        Invalid,
        StandardPacket
    }

    public enum Phases
    {
        A,
        B,
        C,
        TotalAverage,
        Effective,
        PositiveSeq,
        NegativeSeq
    }

    public enum PhaseTypes
    {
        NetworkVoltage,
        TransformerVoltage,
        DifferentialVoltage,
        Current,
        Power
    }

    public enum PhasorTypes
    {
        VtA,
        VtB,
        VtC,
        VnA,
        VnB,
        VnC,
        IA,
        IB,
        IC,
        Ieff,
        VtP,
        VtN,
        VnP,
        VnN,
        IN,
        IP,
        PA,
        PB,
        PC,
        PT,
        VdA,
        VdB,
        VdC,
        VdT,
        VdP,
        VdN,
        None
    }

    public enum PumpReasons
    {
        RelayCallLimit,
        MotorTimeout,
        MotorPump,
        NoPump
    }

    public enum RawPhasorGroups
    {
        Tripped,
        Closed
    }

    public enum TripCurveTypes
    {
        OffsetAngle,
        Magnitude,
        WattVar,
        NoCurve
    }

    public enum TripModes
    {
        Sensitive,
        Insensitive,
        TimeDelay,
        WattVar,
        //Adaptive,
        RemoteTrip
    }

    #endregion

    #region Classes

    public static class Constants
    {
        public const decimal FixedPointConversion = .000244140625m;
        public const decimal MaxFixedPointValue = 128m;
        public const decimal MinFixedPointValue = -128m;
        public const decimal SixFracBits = 0.015625m;
        public const decimal SevenFracBits = 0.0078125m;
        public const decimal EightFracBits = 0.00390625m;
        public const decimal TenFracBits = 0.0009765625m;
        public const decimal TwelveFracBits = 0.000244140625m;
        public const decimal SixteenFracBits = 0.0000152587890625m;
        public const byte DummyData = 0;
        public const long IntZeroTime = 633846816000000000;//August 1, 2010b;
        public const float Protector277Convert = 2.216f;
    }

    public class PumpDefinition
    {
        public byte EnableSendByte
        {
            get { return this.enableSendByte; }
        }
        public byte Cycles;
        public Int16 PumpTime
        {
            get { return this.pumpTime; }
            set
            {
                Int16 temp;
                temp = (Int16)(value * 4);

                this.pumpTime = value;

                this.PumpTimeHigh = (byte)(temp >> 8);
                this.PumpTimeLow = (byte)temp;
            }
        }
        public byte PumpTimeHigh;
        public byte PumpTimeLow;
        public Int16 PumpProtectTime
        {
            get { return this.pumpProtectTime; }
            set
            {
                Int16 temp;
                temp = value;
                this.pumpProtectTime = (Int16)temp;
                this.PumpProtectTimeHigh = (byte)(temp >> 8);
                this.PumpProtectTimeLow = (byte)temp;
            }
        }
        public byte PumpProtectTimeHigh;
        public byte PumpProtectTimeLow;
        public byte MotorCycles;
        public byte MotorTimeout;
        public bool MotorTimeoutEnabled
        {
            get { return this.motorCycleEnabled; }
            set
            {
                this.motorTimeoutEnabled = value;
                this.setOverallEnable();
            }
        }
        public bool MotorCycleEnabled
        {
            get { return this.motorCycleEnabled; }
            set
            {
                this.motorCycleEnabled = value;
                this.setOverallEnable();
            }
        }
        public bool RelayCycleEnabled
        {
            get { return this.relayCycleEnabled; }
            set
            {
                this.relayCycleEnabled = value;
                this.setOverallEnable();
            }
        }
        public bool AlarmOnly
        {
            get { return this.alarmOnly; }
            set
            {
                this.alarmOnly = value;
                this.setOverallEnable();
            }
        }

        private byte enableSendByte;
        private Int16 pumpTime;
        private Int16 pumpProtectTime;
        private bool relayCycleEnabled;
        private bool motorCycleEnabled;
        private bool motorTimeoutEnabled;
        private bool alarmOnly = false;

        private void setOverallEnable()
        {
            //0x02 bit contains ClearPumpMode in relay
            //0x10 bit contains OverridOnDeadNetwork in relay
            if (this.motorTimeoutEnabled)
            {
                this.enableSendByte = (byte)(this.enableSendByte | 0x08);
            }
            else
            {
                this.enableSendByte = (byte)(this.enableSendByte & 0xF7);
            }

            if (this.motorCycleEnabled)
            {
                this.enableSendByte = (byte)(this.enableSendByte | 0x04);
            }
            else
            {
                this.enableSendByte = (byte)(this.enableSendByte & 0xFB);
            }

            if (this.relayCycleEnabled)
            {
                this.enableSendByte = (byte)(this.enableSendByte | 0x01);
            }
            else
            {
                this.enableSendByte = (byte)(this.enableSendByte & 0xFE);
            }

            if (this.alarmOnly)
            {
                this.enableSendByte = (byte)(this.enableSendByte | 0x20);
            }
            else
            {
                this.enableSendByte = (byte)(this.enableSendByte & 0xDF);
            }
        }
    }

    public class RelayMode
    {
        public RelayMode()
        {
            this.Mode = TripModes.Sensitive;
            this.BlockMode = BlockModes.Unblocked;
            this.CloseMode = CloseModes.Normal;
        }


        public TripModes Mode;
        public CloseModes CloseMode;
        public BlockModes BlockMode;
    }

    public class FlagPolarities
    {


        public FlagPolarities()
        {
            this.a = false;
            this.b = false;
            this.c = false;
            this.d = false;
            this.e = false;
            this.f = false;
            this.g = false;
            this.h = false;
        }

        private bool a;
        public bool A
        {
            get { return this.a; }
            set
            {
                this.a = value;
                if (value)
                {
                    this.byteValue = (byte)(this.byteValue | (byte)0x01);
                }
                else
                {
                    this.byteValue = (byte)(this.byteValue & (byte)0xFE);
                }
            }
        }

        private bool b;
        public bool B
        {
            get { return this.b; }
            set
            {
                this.b = value;
                if (value)
                {
                    this.byteValue = (byte)(this.byteValue | (byte)0x02);
                }
                else
                {
                    this.byteValue = (byte)(this.byteValue & (byte)0xFD);
                }
            }
        }

        private bool c;
        public bool C
        {
            get { return this.c; }
            set
            {
                this.c = value;
                if (value)
                {
                    this.byteValue = (byte)(this.byteValue | (byte)0x04);
                }
                else
                {
                    this.byteValue = (byte)(this.byteValue & (byte)0xFB);
                }
            }
        }

        private bool d;
        public bool D
        {
            get { return this.d; }
            set
            {
                this.d = value;
                if (value)
                {
                    this.byteValue = (byte)(this.byteValue | (byte)0x08);
                }
                else
                {
                    this.byteValue = (byte)(this.byteValue & (byte)0xF7);
                }
            }
        }

        private bool e;
        public bool E
        {
            get { return this.e; }
            set
            {
                this.e = value;
                if (value)
                {
                    this.byteValue = (byte)(this.byteValue | (byte)0x10);
                }
                else
                {
                    this.byteValue = (byte)(this.byteValue & (byte)0xEF);
                }
            }
        }

        private bool f;
        public bool F
        {
            get { return this.f; }
            set
            {
                this.f = value;
                if (value)
                {
                    this.byteValue = (byte)(this.byteValue | (byte)0x20);
                }
                else
                {
                    this.byteValue = (byte)(this.byteValue & (byte)0xDF);
                }
            }
        }

        private bool g;
        public bool G
        {
            get { return this.g; }
            set
            {
                this.g = value;
                if (value)
                {
                    this.byteValue = (byte)(this.byteValue | (byte)0x40);
                }
                else
                {
                    this.byteValue = (byte)(this.byteValue & (byte)0xBF);
                }
            }
        }

        private bool h;
        public bool H
        {
            get { return this.h; }
            set
            {
                this.h = value;
                if (value)
                {
                    this.byteValue = (byte)(this.byteValue | (byte)0x80);
                }
                else
                {
                    this.byteValue = (byte)(this.byteValue & (byte)0x7F);
                }
            }
        }

        private byte byteValue;
        public byte ByteValue
        {
            get { return this.byteValue; }
            set
            {
                this.byteValue = value;
                this.setBoolValues(value);
            }
        }

        private void setBoolValues(byte b)
        {
            if ((b & 1) == 1)
            {
                this.a = true;
            }
            else
            {
                this.a = false;
            }
            if ((b & 2) == 2)
            {
                this.b = true;
            }
            else
            {
                this.b = false;
            }
            if ((b & 4) == 4)
            {
                this.c = true;
            }
            else
            {
                this.c = false;
            }
            if ((b & 8) == 8)
            {
                this.d = true;
            }
            else
            {
                this.d = false;
            }
            if ((b & 16) == 16)
            {
                this.e = true;
            }
            else
            {
                this.e = false;
            }
            if ((b & 32) == 32)
            {
                this.f = true;
            }
            else
            {
                this.f = false;
            }
            if ((b & 64) == 64)
            {
                this.g = true;
            }
            else
            {
                this.g = false;
            }
            if ((b & 128) == 128)
            {
                this.h = true;
            }
            else
            {
                this.h = false;
            }
        }
    }

    public class TransmitterSettings
    {
        public TransmitterSettings()
        {
            this.SendPacket[0] = (byte)'Y';
            this.SendPacket[31] = 0x0D;
            this.FlagPolarity = new FlagPolarities();
        }
        private int packetLength = 30;
        public int PacketLength
        {
            get { return this.packetLength; }
            set
            {
                this.packetLength = value;
                this.SendPacket = new byte[packetLength + 2];
                this.SendPacket[0] = (byte)'Y';
                this.SendPacket[packetLength + 1] = 0x0D;
            }
        }

        private UInt16 iD;
        public UInt16 ID
        {
            get { return this.iD; }
            set
            {
                if (value < 1 || value > 1023)
                {
                    throw new Exception(value.ToString() + " is a bad ID Value.  ID Value must be between 1 and 1023");
                }
                else
                {
                    this.iD = value;
                    this.SendPacket[1] = (byte)value;
                    this.SendPacket[2] = (byte)(value >> 8);
                }
            }
        }

        private UInt16 serialNumber;
        public UInt16 SerialNumber
        {
            get { return this.serialNumber; }
            set
            {
                this.serialNumber = value;
                this.SendPacket[3] = (byte)value;
                this.SendPacket[4] = (byte)(value >> 8);
            }
        }

        private UInt16 tXCTRatio;
        public UInt16 TXCTRatio
        {
            get { return this.tXCTRatio; }
            set
            {
                if (value < 59 || value > 335)
                {
                    throw new Exception(value.ToString() + " is an invalid value for CT Ratio.  CT Ratio must be between 60 and 335");
                }
                else
                {
                    this.tXCTRatio = value;
                    this.SendPacket[5] = (byte)value;
                    this.SendPacket[6] = (byte)(value >> 8);
                }
            }
        }

        private UInt16 cTRatio;
        public UInt16 CTRatio
        {
            get { return this.cTRatio; }
            set
            {
                this.cTRatio = value;
                this.SendPacket[7] = (byte)value;
                this.SendPacket[8] = (byte)(value >> 8);
            }
        }

        private Frequencies frequency;
        public Frequencies Frequency
        {
            get { return this.frequency; }
            set
            {
                try
                {
                    this.SendPacket[9] = RelayModeFunctions.ByteFrom(value);
                    this.frequency = value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private FlagPolarities flagPolarity;
        public FlagPolarities FlagPolarity
        {
            get { return flagPolarity; }
            set
            {
                this.flagPolarity = value;
                this.SendPacket[10] = this.flagPolarity.ByteValue;
            }
        }

        private byte enableFlagAlarms;
        public byte EnableFlagAlarms
        {
            get { return this.enableFlagAlarms; }
            set
            {
                this.enableFlagAlarms = value;
                this.SendPacket[11] = value;
            }
        }

        private byte enableOtherAlarms;
        public byte EnableOtherAlarms
        {
            get { return this.enableOtherAlarms; }
            set
            {
                this.enableOtherAlarms = value;
                this.SendPacket[12] = value;
            }
        }

        private byte currentThresholdHigh;
        public byte CurrentThresholdHigh
        {
            get { return this.currentThresholdHigh; }
            set
            {
                if (value > 200)
                {
                    throw new Exception(value.ToString() + " is not a valid High Current Threshold value.  Must be equal to or less than 200");
                }
                else
                {
                    this.currentThresholdHigh = value;
                    this.SendPacket[13] = value;
                }
            }
        }

        private byte currentThresholdLow;
        public byte CurrentThresholdLow
        {
            get { return this.currentThresholdLow; }
            set
            {
                if (value > 100)
                {
                    throw new Exception(value.ToString() + " is not a valid Low Current Threshold value.  Must be equal to or less than 100");
                }
                else
                {
                    this.currentThresholdLow = value;
                    this.SendPacket[14] = value;
                }
            }
        }

        private byte voltageThresholdHigh;
        public byte VoltageThresholdHigh
        {
            get { return this.voltageThresholdHigh; }
            set
            {
                if (value > 200)
                {
                    throw new Exception(value.ToString() + " is not a valid High Voltage Threshold value.  Must be equal to or less than 200");
                }
                else
                {
                    this.voltageThresholdHigh = value;
                    this.SendPacket[15] = value;
                }
            }
        }

        private byte voltageThresholdLow;
        public byte VoltageThresholdLow
        {
            get { return this.voltageThresholdLow; }
            set
            {
                if (value > 200)
                {
                    throw new Exception(value.ToString() + " is not a valid Low Voltage Threshold value.  Must be equal to or less than 200");
                }
                else
                {
                    this.voltageThresholdLow = value;
                    this.SendPacket[16] = value;
                }
            }
        }

        private sbyte a1Threshold;
        public sbyte A1Threshold
        {
            get { return this.a1Threshold; }
            set
            {
                if (value > 127)
                {
                    throw new Exception(value.ToString() + " is not a valid A1 Threshold value.  Must be equal to or less than 127");
                }
                else
                {
                    this.a1Threshold = value;
                    this.SendPacket[17] = (byte)value;
                }
            }
        }

        private sbyte a2Threshold;
        public sbyte A2Threshold
        {
            get { return this.a2Threshold; }
            set
            {
                if (value > 127)
                {
                    throw new Exception(value.ToString() + " is not a valid A2 Threshold value.  Must be equal to or less than 127");
                }
                else
                {
                    this.a2Threshold = value;
                    this.SendPacket[18] = (byte)value;
                }
            }
        }

        private byte analogAlarmSense;
        public byte AnalogAlarmSense
        {
            get { return this.analogAlarmSense; }
            set
            {
                this.analogAlarmSense = value;
                this.SendPacket[19] = (byte)value;
            }
        }

        private byte messagePeriod;
        public byte MessagePeriod
        {
            get { return this.messagePeriod; }
            set
            {
                switch (value)
                {
                    case 0:
                    case 1:
                    case 2:
                        break;
                    default:
                        throw new Exception(value.ToString() + " is not a valid Message Period value.");
                }

                this.messagePeriod = value;
                this.SendPacket[20] = value;
            }
        }

        private byte muxPeriod;
        public byte MuxPeriod
        {
            get { return this.muxPeriod; }
            set
            {
                if (value != 0xFF && (value == 0 || value > 60))
                    throw new Exception(value.ToString() + " is not a valid Mux Period.");

                this.muxPeriod = value;
                this.SendPacket[21] = value;
            }
        }

        private byte type2MessagePeriod;
        public byte Type2MessagePeriod
        {
            get { return this.type2MessagePeriod; }
            set
            {
                if ((value > 23 || value == 0) && value != 0xFF)
                    throw new Exception(value.ToString() + " is not a valid Type2 Message Period.");

                this.type2MessagePeriod = value;
                this.SendPacket[22] = value;
            }
        }

        private byte configMessagePeriod;
        public byte ConfigMessagePeriod
        {
            get { return this.configMessagePeriod; }
            set
            {
                //if(value != 23 && value != 0xFF)
                //    throw new Exception(value.ToString() + " is not a valid Config Message Period.  Must be 23.");

                this.configMessagePeriod = value;
                this.SendPacket[23] = value;
            }
        }

        private byte alarmBurstCount;
        public byte AlarmBurstCount
        {
            get { return this.alarmBurstCount; }
            set
            {
                if (value < 2 || value > 8)
                    throw new Exception(value.ToString() + " is not a valid Alarm Burst Count.  Must be a value from 2 to 8.");

                this.alarmBurstCount = value;
                this.SendPacket[24] = value;
            }
        }

        private byte alarmSpacing;
        public byte AlarmSpacing
        {
            get { return this.alarmSpacing; }
            set
            {
                if (value == 0)
                    throw new Exception("Alarm Spacing value must be greater than 0");

                this.alarmSpacing = value;
                this.SendPacket[25] = value;
            }
        }

        private byte otherMessageBurstCount;
        public byte OtherMessageBurstCount
        {
            get { return this.otherMessageBurstCount; }
            set
            {
                if (value < 2 || value > 8)
                    throw new Exception(value.ToString() + " is not a valid Other Message Burst Count.  Must be a value from 2 to 8.");

                this.otherMessageBurstCount = value;
                this.SendPacket[26] = value;
            }
        }

        private byte otherMessageBurstInterval;
        public byte OtherMessageBurstInterval
        {
            get { return this.otherMessageBurstInterval; }
            set
            {
                if (value == 0)
                    throw new Exception("Other Message Burst Interval value must be greater than 0");

                this.otherMessageBurstInterval = value;
                this.SendPacket[27] = value;
            }
        }

        private sbyte temperatureCalibration;
        public sbyte TemperatureCalibration
        {
            get { return this.temperatureCalibration; }
            set
            {
                this.temperatureCalibration = value;
                //this.SendPacket[28] = (byte)value;
            }
        }

        private bool extendedPLCMessage = false;
        public bool ExtendedPLCMessage
        {
            get => extendedPLCMessage;
            set
            {
                extendedPLCMessage = value;
                SendPacket[28] = value ? (byte)1 : (byte)0;
            }
        }

        private byte type1MessageLength;
        public byte Type1MessageLength
        {
            get { return this.type1MessageLength; }
            set
            {
                this.type1MessageLength = value;
                this.SendPacket[29] = value;
            }

        }

        private byte dataFromWaterbug;
        public byte DataFromWaterBug
        {
            get { return this.dataFromWaterbug; }
            set
            {
                this.dataFromWaterbug = value;
                this.SendPacket[30] = value;
            }
        }

        public void SetValues(byte[] bA)
        {
            for (int i = 0; i < 30; ++i)
            {
                SendPacket[i + 1] = bA[i];
            }
        }

        public byte[] SendPacket = new byte[32];

        public void SetFlagPolartityByte()
        {
            this.SendPacket[10] = this.flagPolarity.ByteValue;
        }

        private byte lEDSpeed = 20;
        public byte LEDSpeed
        {
            get { return this.lEDSpeed; }
            set
            {
                this.lEDSpeed = value;
                this.SendPacket[31] = value;
            }
        }
    }

    public class TripCurveDefinition
    {
        public TripCurveDefinition(TripCurveTypes tCT)
        {
            CurveType = tCT;
            this.Offset = 7.5m;       //7.5
            this.Tilt = 90;          //0 Degrees
            this.Magnitude = 2.5m;    //2.5
            this.CodomainMinimum = 0X80;
            this.CodomainMaximum = 0x7F;
        }

        public TripCurveTypes CurveType;

        //offset variables
        private decimal offset;
        /// <summary>
        /// In mA
        /// </summary>
        public decimal Offset
        {
            get { return this.offset; }
            set
            {
                if (value >= 0 && value <= 5000)
                {
                    this.offset = value;

                    Int32 temp;
                    temp = (Int32)Math.Round((this.offset / 1000m) / Constants.SixteenFracBits);
                    temp = (Int32)(-temp);
                    this.OffsetHighByte = (byte)(temp >> 8);
                    this.OffsetLowByte = (byte)(temp);
                    if (this.CurveType == TripCurveTypes.OffsetAngle)
                    {
                        this.MagnitudeHighByte = (byte)(temp >> 24);
                        this.MagnitudeLowByte = (byte)(temp >> 16);
                    }
                }
                else
                {
                    throw new Exception(value.ToString() + " is out of range 1 to 5000");
                }
            }
        }
        public byte OffsetHighByte;
        public byte OffsetLowByte;

        //Tilt Variables
        private decimal tilt;               //-5 to 5, 1 degree steps  (-5 = 0, 5 = 10)
        public decimal Tilt
        {
            get { return this.tilt; }
            set
            {
                double radians;
                if (value >= 5 && value <= 175) //postive angle is clockwise rotation from 90
                {
                    if (value == 90)
                    {
                        this.tilt = 90;
                        this.TiltHighByte = 0;
                        this.TiltLowByte = 0;
                    }
                    else
                    {
                        this.tilt = value;
                        radians = (double)value * Math.PI / 180d;

                        this.TiltTangent = (decimal)Math.Tan(radians);

                        Int16 temp;

                        temp = (Int16)Math.Round((this.TiltTangent / Constants.EightFracBits));
                        this.TiltHighByte = this.highByte(temp);
                        this.TiltLowByte = this.lowByte(temp);
                    }

                }
                else
                    throw new Exception(value.ToString() + " is out of range 10 to 170");
            }

        }
        public decimal TiltTangent;
        public byte TiltHighByte;
        public byte TiltLowByte;

        //range variables
        private decimal codomainMinimum;
        public decimal CodomainMinimum
        {
            get { return this.codomainMinimum; }
            set
            {
                Int16 temp;

                this.codomainMinimum = value;
                temp = (Int16)(value / Constants.SevenFracBits);
                this.CodomainMinHighByte = highByte(temp);
                this.CodomainMinLowByte = lowByte(temp);
            }
        }
        public byte CodomainMinHighByte;
        public byte CodomainMinLowByte;

        private decimal codomainMaximum;
        public decimal CodomainMaximum
        {
            get { return this.codomainMaximum; }
            set
            {
                Int16 temp;

                this.codomainMaximum = value;
                temp = (Int16)(value / Constants.SevenFracBits);
                this.CodomainMaxHighByte = highByte(temp);
                this.CodomainMaxLowByte = lowByte(temp);
            }
        }
        public byte CodomainMaxHighByte;
        public byte CodomainMaxLowByte;


        //Magnitude Variables
        private decimal magnitude;      //.1 to 15, .1 steps
        public decimal Magnitude
        {
            get { return this.magnitude; }
            set
            {
                if (value >= 0m && value <= 15m)
                {
                    Int16 temp;

                    this.magnitude = value;
                    temp = (Int16)(this.magnitude / Constants.TenFracBits); // 0.0009765625m;

                    this.MagnitudeHighByte = this.highByte(temp);
                    this.MagnitudeLowByte = this.lowByte(temp);
                }
                else
                    throw new Exception(value.ToString() + " is out of range .1 to 15");
            }
        }
        public byte MagnitudeHighByte;
        public byte MagnitudeLowByte;

        public byte CurveNumber;

        public byte[] BytePacket()
        {
            return RelayModeFunctions.BytePacketFor(this, CurveNumber);
        }

        public byte highByte(decimal d)
        {
            Int32 temp;
            byte result;

            temp = (Int32)(d / Constants.FixedPointConversion);
            result = (byte)(temp >> 24);

            return result;
        }

        public byte midHighByte(decimal d)
        {
            Int32 temp;
            byte result;

            temp = (Int32)(d / Constants.FixedPointConversion);
            result = (byte)(temp >> 16);

            return result;
        }

        public byte midLowByte(decimal d)
        {
            Int32 temp;
            byte result;

            temp = (Int32)(d / Constants.FixedPointConversion);
            result = (byte)(temp >> 8);

            return result;
        }

        public byte lowByte(decimal d)
        {
            Int32 temp;
            byte result;

            temp = (Int32)(d / Constants.FixedPointConversion);
            result = (byte)temp;

            return result;
        }

        public byte highByte(Int16 i)
        {
            return (byte)(i >> 8);
        }

        public byte lowByte(Int16 i)
        {
            return (byte)(i & 0xFF);
        }
    }

    public class TripModeDefinition
    {
        public TripModeDefinition(TripModes tM)
        {
            this.Mode = tM;
        }

        private TripModes mode;
        public TripModes Mode
        {
            get { return this.mode; }
            set
            {
                this.mode = value;
            }
        }
        private int timeDelay;
        public int TimeDelay            //Time Delay
        {
            get
            {
                return timeDelay;
            }
            set
            {
                timeDelay = value;
                TimeDelayHighByte = RelayModeFunctions.HighByte(value);
                TimeDelayLowByte = RelayModeFunctions.LowByte(value);
            }
        }
        public byte TimeDelayHighByte;
        public byte TimeDelayLowByte;

        private int sensitiveTimeDelay;
        public int SensitiveTimeDelay                //Sensitive Delay
        {
            get { return this.sensitiveTimeDelay; }
            set
            {

                this.sensitiveTimeDelay = value;
                this.SensitiveTimeDelayHighByte = (byte)(value >> 8);
                this.SensitiveTimeDelayLowByte = (byte)value;
            }
        }

        private int extendedDelay = 0;
        public int ExtendedDelay
        {
            get { return this.extendedDelay; }
            set
            {
                this.extendedDelay = value;
            }
        }

        public byte SensitiveTimeDelayHighByte;
        public byte SensitiveTimeDelayLowByte;

        void setSensitiveValues()
        {
            int tempValue;

            if (this.mode == TripModes.Sensitive || this.mode == TripModes.WattVar)
            {
                tempValue = this.sensitiveTimeDelay;
            }
            else
            {
                tempValue = (this.extendedDelay * 60);
            }

            this.SensitiveTimeDelayHighByte = RelayModeFunctions.HighByte(tempValue);
            this.SensitiveTimeDelayLowByte = RelayModeFunctions.LowByte(tempValue);
        }
    }

    public class CloseModeDefinition
    {
        public CloseModeDefinition(CloseModes cM)
        {
            this.CloseMode = cM;

        }
        public CloseModes CloseMode;

        private int timeDelay;
        public int TimeDelay
        {
            get
            {
                return timeDelay;
            }
            set
            {
                this.timeDelay = value;
                TimeDelayHighByte = RelayModeFunctions.HighByte(value);
                TimeDelayLowByte = RelayModeFunctions.LowByte(value);
            }
        }

        public byte TimeDelayHighByte;
        public byte TimeDelayLowByte;

        public UInt16 DataBits;
        private bool overrideBlockedClose = false;

        public bool OverrideBlockedClose
        {
            get { return this.overrideBlockedClose; }
            set
            {
                this.overrideBlockedClose = value;
                if (value)
                    this.DataBits = (UInt16)(this.DataBits | (UInt16)1);
                else
                    this.DataBits = (UInt16)(this.DataBits & (UInt16)0xFFFE);
            }
        }

    }

    public class CloseCurveDefinition
    {
        public CloseCurveDefinition() { }

        private decimal recloseVolts;               //Vertical Line offset
        public decimal RecloseVolts                 //.1 to 10.0 V in .1 steps = 1
        {
            get
            {
                return this.recloseVolts;
            }
            set
            {
                this.recloseVolts = value;
                if (value < 0m || value > 10m)
                {
                    throw new Exception("Reclose Voltage Out Of Range");
                }
                UInt16 temp;
                //8 fract bits
                temp = (UInt16)(value / Constants.TwelveFracBits);
                this.recloseVoltsByteHigh = this.highByte(temp);
                this.recloseVoltsByteLow = this.lowByte(temp);
            }
        }
        private byte recloseVoltsByteHigh;
        public byte RecloseVoltsByteHigh
        {
            get { return this.recloseVoltsByteHigh; }
        }
        private byte recloseVoltsByteLow;
        public byte RecloseVoltsByteLow
        {
            get { return this.recloseVoltsByteLow; }
        }

        private decimal phaseDetectAngle;               //Horizontal line angle
        public decimal PhaseDetectAngle                //-25 to 5
        {
            get { return this.phaseDetectAngle; }
            set
            {
                double radians;

                if (value < -25 || value > 5)
                    throw new Exception("Reclose Angle Value out of Range");
                this.phaseDetectAngle = value;

                Int16 temp;
                radians = (double)value * Math.PI / 180d;
                this.PhaseDetectTangent = (decimal)Math.Tan(radians);

                temp = (Int16)(this.PhaseDetectTangent / Constants.TwelveFracBits);

                //Ten Fractional Bits

                this.phaseDetectTangentHighByte = this.highByte(temp);
                this.phaseDetectTangentLowByte = this.lowByte(temp);
            }
        }
        public decimal PhaseDetectTangent;
        private byte phaseDetectTangentHighByte;
        public byte PhaseDetectTangentHighByte
        {
            get { return this.phaseDetectTangentHighByte; }
        }
        private byte phaseDetectTangentLowByte;
        public byte PhaseDetectTangentLowByte
        {
            get { return this.phaseDetectTangentLowByte; }
        }


        private decimal phasingOffset = .4m;
        public decimal PhasingOffset
        {
            get { return this.phasingOffset; }
            set
            {
                this.phasingOffset = value;
                UInt16 temp;
                temp = (UInt16)(value / Constants.TwelveFracBits);

                this.PhasingOffsetHighByte = (byte)(temp >> 8);
                this.PhasingOffsetLowByte = (byte)temp;
            }
        }
        public byte PhasingOffsetHighByte = 0x00; //12 fraction bits
        public byte PhasingOffsetLowByte = 0x66;

        private decimal tiltAngle;              //Horizontal Angle
        public decimal TiltAngle                //-5 to 5 in 1 degree steps
        {
            get { return this.tiltAngle; }
            set
            {
                Int16 temp;

                if (value < 85 || value > 95)
                    throw new Exception("Phasing Angle Out Of Range");
                this.tiltAngle = value;

                double radians;

                if (this.tiltAngle == 90)
                {
                    temp = 0;
                }
                else
                {
                    radians = (double)this.tiltAngle * Math.PI / 180d;
                    this.tiltAngleTangent = (decimal)Math.Tan(radians);

                    //ten fract bits
                    temp = (Int16)(this.tiltAngleTangent / Constants.EightFracBits);
                }
                this.tiltAngleTangentHighByte = this.highByte(temp);
                this.tiltAngleTangentLowByte = this.lowByte(temp);
            }
        }
        private decimal tiltAngleTangent;
        public decimal TiltAngleTangent
        {
            get { return this.tiltAngleTangent; }
        }
        private byte tiltAngleTangentHighByte;
        public byte TiltAngleTangentHighByte
        {
            get { return this.tiltAngleTangentHighByte; }
        }
        private byte tiltAngleTangentLowByte;
        public byte TiltAngleTangentLowByte
        {
            get { return this.tiltAngleTangentLowByte; }
        }

        public byte[] BytePacket()
        {
            return RelayModeFunctions.BytePacketFor(this);
        }

        public byte highByte(Int16 i)
        {
            return (byte)(i >> 8);
        }

        public byte lowByte(Int16 i)
        {
            return (byte)(i & 0xFF);
        }

        public byte highByte(UInt16 i)
        {
            return (byte)(i >> 8);
        }

        public byte lowByte(UInt16 i)
        {
            return (byte)(i & 0xFF);
        }
    }

    public class TripCurveDrawObject
    {
        public TripCurveDrawObject()
        {
        }

        public TripCurveTypes Type;
        public PointF begin;
        public PointF end;
        public float magnitude;
    }

    public class LineF
    {
        public LineF()
        {
        }

        public PointF Begin;
        public PointF End;
    }

    public class ArcF
    {
        public ArcF()
        {
        }

        public RectangleF Rect;
        public float BeginAngle;
        public float SweepAngle;
    }

    public class CalibrationConstant
    {
        public CalibrationConstant()
        {
        }

        private Int32 rawValue;
        public Int32 RawValue
        {
            get { return this.rawValue; }
            set
            {
                this.rawValue = value;
                this.realValue = this.rawValue * (float)Constants.TwelveFracBits;
            }
        }

        private float realValue;
        public float RealValue
        {
            get { return this.realValue; }
            set { this.realValue = value; }
        }
    }

    public class CalibrationConstants
    {
        public CalibrationConstants()
        {
        }

        public CalibrationConstant VnA;
        public CalibrationConstant VnB;
        public CalibrationConstant VnC;
        public CalibrationConstant VtA;
        public CalibrationConstant VtB;
        public CalibrationConstant VtC;
        public CalibrationConstant IA;
        public CalibrationConstant IB;
        public CalibrationConstant IC;

    }

    public class ReferenceSineWave
    {
        private static float[] lUT;
        private static float amplitude;
        public static float RMS;

        static ReferenceSineWave()
        {
            lUT = new float[128];
            amplitude = 125f;
            for (int i = 0; i < lUT.Length; ++i)
            {
                lUT[i] = amplitude * (float)Math.Sin(Math.PI * 2d * (double)i / (double)lUT.Length);
            }

            RMS = amplitude / (float)Math.Sqrt(2);
        }

        public static float LookUp(int index)
        {
            return lUT[index];
        }
    }

    public class EventBaseTime
    {
        public EventBaseTime() { }

        public DateTime SystemTime
        {
            get { return this.systemTime; }
            set
            {


                this.systemTime = value;

                TimeSpan tempTimeSpan = value.Subtract(BaseTime);
                this.binaryTime = (UInt32)tempTimeSpan.TotalSeconds;
            }
        }
        public UInt32 BinaryTime
        {
            get { return this.binaryTime; }
            set
            {
                this.binaryTime = value;
                this.systemTime = BaseTime.AddSeconds(value);
                this.systemTime = this.systemTime.ToLocalTime();
            }
        }

        public readonly DateTime BaseTime = new DateTime(2009, 8, 1); //August 1, 2009 
        private UInt32 binaryTime;
        private DateTime systemTime;
    }

    public class EventData
    {
        public EventData()
        {

        }

        public EventTypes Type;
        public EventBaseTime Time;
        public double[] Values = new double[2048];
    }

    public class PacketHandledEventArgs
    {
        public bool Successful = false;

        #region Constructors

        public PacketHandledEventArgs()
        {
        }
        public PacketHandledEventArgs(bool b)
        {
            this.Successful = b;
        }

        #endregion
    }

    #endregion

    #region Serializable Classes
    [Serializable()]
    public class SavedEventSet : ISerializable
    {
        public SavedSingleEvent[] Events = new SavedSingleEvent[8];

        public SavedEventSet()
        {
            for (int i = 0; i < Events.Length; ++i)
            {
                Events[i] = new SavedSingleEvent();
            }
        }

        public SavedEventSet(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Events = (SavedSingleEvent[])info.GetValue("Events", typeof(SavedSingleEvent[]));
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Serialization of SavedEventSet", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Events", this.Events);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetObjectData of SavedEventSet", ex);
            }
        }
    }

    [Serializable()]
    public class ExternalFileRevisionNumber : ISerializable
    {
        public uint RevisionNumber = 0;

        public ExternalFileRevisionNumber()
        {

        }

        public ExternalFileRevisionNumber(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.RevisionNumber = (uint)info.GetValue("RevisionNumber", typeof(uint));
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Serialization of RevisionNumber", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("RevisionNumber", this.RevisionNumber);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetObjectData of ExternalFileRevisionNumber", ex);
            }
        }
    }

    [Serializable()]
    public class SavedSingleEvent : ISerializable
    {
        public EventTypes Type;
        public DateTime Time;
        public UInt16 ID;
        public float[] VtA;
        public float[] VtB;
        public float[] VtC;
        public float[] VnA;
        public float[] VnB;
        public float[] VnC;
        public float[] IA;
        public float[] IB;
        public float[] IC;


        public SavedSingleEvent()
        {
        }

        public SavedSingleEvent(int arraySize)
        {
            VtA = new float[arraySize];
            VtB = new float[arraySize];
            VtC = new float[arraySize];
            VnA = new float[arraySize];
            VnB = new float[arraySize];
            VnC = new float[arraySize];
            IA = new float[arraySize];
            IB = new float[arraySize];
            IC = new float[arraySize];
        }

        public SavedSingleEvent(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Type = (EventTypes)info.GetValue("Type", typeof(EventTypes));
                this.Time = (DateTime)info.GetValue("Time", typeof(DateTime));
                this.ID = (UInt16)info.GetValue("ID", typeof(UInt16));
                this.VtA = (float[])info.GetValue("VtA", typeof(float[]));
                this.VtB = (float[])info.GetValue("VtB", typeof(float[]));
                this.VtC = (float[])info.GetValue("VtC", typeof(float[]));
                this.VnA = (float[])info.GetValue("VnA", typeof(float[]));
                this.VnB = (float[])info.GetValue("VnB", typeof(float[]));
                this.VnC = (float[])info.GetValue("VnC", typeof(float[]));
                this.IA = (float[])info.GetValue("IA", typeof(float[]));
                this.IB = (float[])info.GetValue("IB", typeof(float[]));
                this.IC = (float[])info.GetValue("IC", typeof(float[]));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Saved Single Event", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Type", this.Type);
                info.AddValue("Time", this.Time);
                info.AddValue("ID", this.ID);
                info.AddValue("VtA", this.VtA);
                info.AddValue("VtB", this.VtB);
                info.AddValue("VtC", this.VtC);
                info.AddValue("VnA", this.VnA);
                info.AddValue("VnB", this.VnB);
                info.AddValue("VnC", this.VnC);
                info.AddValue("IA", this.IA);
                info.AddValue("IB", this.IB);
                info.AddValue("IC", this.IC);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetObjectData of SavedSingleEvent", ex);
            }
        }
    }
    #endregion

    public enum ProtectorVoltageEnum
    {
        V125,
        V277,
        V346
    }
    public class ProtectorVoltage
    {
        public ProtectorVoltage(string name, ProtectorVoltageEnum value, decimal scaling, ProtectorVoltageBits bit)
        {
            Name = name;
            Value = value;
            Scaling = scaling;
            SetBit = bit;
        }
        public string Name { get; }
        public ProtectorVoltageEnum Value { get; }

        public decimal Scaling { get; }

        public ProtectorVoltageBits SetBit { get; }
    }

    public static class ProtectorVoltages
    {
        public static BindingList<ProtectorVoltage> Voltages { get; } =
            new BindingList<ProtectorVoltage> {
                new ProtectorVoltage("125V", ProtectorVoltageEnum.V125, 1.0m, new ProtectorVoltageBits()),
                new ProtectorVoltage("277V", ProtectorVoltageEnum.V277, 2.216m, ProtectorVoltageBits.V277),
                new ProtectorVoltage("347V", ProtectorVoltageEnum.V346, 2.771m, ProtectorVoltageBits.V600)
            };

        public static ProtectorVoltage GetVoltage(ProtectorVoltageBits bits)
        {
            bits = bits & (ProtectorVoltageBits.V277 | ProtectorVoltageBits.V600);
            var voltage = Voltages.FirstOrDefault(x => x.SetBit.HasFlag(bits));

            if (voltage == null)
                voltage =
                    Voltages.First<ProtectorVoltage>(x => x.Value == ProtectorVoltageEnum.V125);

            return voltage;
        }

        public static ProtectorVoltage GetVoltage()
        {
            return Voltages.First<ProtectorVoltage>(x => x.Value == ProtectorVoltageEnum.V125);
        }
    }

    public static class screenD
    {
        public static bool screenDisable;
    }

    public static class powerP
    {
        public static int pwrPer;
    }

    public static class manualP
    {
        public static bool manualProgramming;
    }
    public static class oneTimeRelayDataF
    {
        public static bool oneTimeRelayDataFinish;
    }

    public static class flagP
    {
        public static byte transmitterFlagPolarity;
    }

    public static class flagS
    {
        public static byte flagSettings;
    }

    public static class statusNew
    {
        public static bool flagFromRelay;
    }

    public static class relayHBD
    {
        public static bool relayWithHBD;
    }


    public static class AutoReProgramR
    {
        public static bool AutoReProgramRelay;
    }
    public static class AutoReProgramF
    {
        public static bool AutoReProgramFPGA;
    }

    public static class sendAllF
    {
        public static bool SendAllFlag;
    }

    public static class dataBackupR
    {
        public static bool dataBackup_fromRelay;
    }

    public static class GeWhF
    {
        public static bool GeWh;
    }

    [Flags]
    public enum ProtectorVoltageBits
    {
        PhasingBit1 = 1,
        PhasingBit2 = 2,
        PhasingBit3 = 4,
        V277 = 8,
        DNPOutputConvert = 16,
        V600 = 32
    }
}
