#include "tmwdtime.h"
//#include "sdnpsesn.h"
#ifndef DNPVariables
#define DNPVariables

#define DNPCustomerDigitalGrid	0x00
#define DNPCustomerMemphis		0x01
#define DNPCustomerConEd		0x02
#define DNPCustomerTest			0x03

#define DNP_ENTROPY_SIZE 		50

#ifdef MEMPHIS
#define DNPCustomer			DNPCustomerDigitalGrid //TEST
#endif

#ifdef CONED
#define DNPCustomer			DNPCustomerConEd
#endif

#ifndef DNPCustomer
#define DNPCustomer 		DNPCustomerDigitalGrid
#endif


#if DNPCustomer == DNPCustomerMemphis
#define AnalogOutQuantity	3
#define AnalogInQuantity	31
#define BinaryInQuantity	39
#define BinaryOutQuantity	4
#define IINQuantity			16

#elif DNPCustomer == DNPCustomerDigitalGrid

#define AnalogOutQuantity	23
#define AnalogInQuantity	69
#define BinaryInQuantity	14
#define BinaryOutQuantity	20
#define IINQuantity			16
#define SecurityQuantity	18 // Per DNP SAv5 spec.

#elif DNPCustomer == DNPCustomerTest

#define AnalogOutQuantity	5
#define AnalogInQuantity	5
#define BinaryInQuantity	5
#define BinaryOutQuantity	5
#define IINQuantity			5


#endif

#define ANALOG_EVENT_INCLUDED TMWDEFS_TRUE
#define BINARY_EVENT_INCLUDED TMWDEFS_TRUE


typedef union uDNPPOINTCONTROL
{
	unsigned int word;
	struct
	{
		unsigned int Enabled : 1, EventEnabled :1, EventClass : 2, ControlEnabled : 1, VariableEvent : 1, NotFirstDataRead :1, AngleValue :1,:8;
	};
}DNPPOINTCONTROL;

typedef struct DNPBINARYPOINT
{
	unsigned char Value;
	DNPPOINTCONTROL Control;
}BinaryPoint;

typedef struct DNPANALOGINPOINT
{
	int Value;
	int savedOldValue;
	DNPPOINTCONTROL Control;
}AnalogPointIn;

typedef struct DNPANALOGOUTPOINT
{
	int Value;
	DNPPOINTCONTROL Control;
}AnalogPointOut;


typedef struct tagCORDIC_TABLE {
  double K;
  double phase_rads;
} CORDIC_TABLE;

typedef struct DataStructCopy
{
	int C_word[4];

	int Mclose_word[3];
	int Mtrip_word[3];

	int T0_word[6];
	int T1_word[6];
	int T2_word[6];
	int T3_word[6];
	int T4_word[6];

	unsigned int relay_type;
	unsigned int pump_mode, pump_time, protect_time;
	unsigned int relay_close_parameters;
//		unsint end_of_relay_parameters;
	unsigned int breaker_open_cycles_counter;
}DATACOPY;

typedef union
{
	unsigned int word;
	struct
	{
		unsigned int 
		LinkLayerConfirm	: 2,
		SelfAddress			: 1,
		UnsolicitedAllowed	: 1,
		TerminationResistor	: 1,
		MemphisStage		: 3;
		
	};
}DNPCONTROLWORD;


typedef union DNPSETTINGS
{

	unsigned int Word[49];
#if DNPCustomer == DNPCustomerMemphis	
	struct
	{	
		DNPCONTROLWORD 	ControlWord;
		unsigned long 	UnsolTimeout;
		unsigned int 	FragmentSize;
		unsigned int 	Destination;
		unsigned int 	Source;
		unsigned int 	UnsolMaxRetries; //7
		unsigned char 	MaxEvents;
		unsigned char 	Holder; //just a position holder to keep other things aligned for comms //8
		unsigned char	BinaryEventEnables[6];	//11
		unsigned char	AnalogEventEnables[13]; 
		unsigned char	Holder2;				//17
		unsigned int	DeadBandVoltage;		
		unsigned int	DeadBandTHD;
		unsigned int	DeadBandCurrent;
		unsigned int	DeadBandTemperature;
		unsigned int	DeadBandOdometer;
		unsigned int	DeadBandRMSDifferentialVoltage;
		unsigned int	DeadBandRealDifferentialVoltage;
		unsigned int	DeadBandCurrentAngle;
		unsigned int	DeadBandPhasekW;
		unsigned int	DeadBandPhasekVAR;
		unsigned int	DeadBandPhasekVA;
		unsigned int	DeadBandTotalkW;
		unsigned int	DeadBandTotalkVAkVAR;
		unsigned int	DeadBandAnalog1;			//31
		unsigned int	DeadBandAnalog2;
		unsigned int	DeadBandAnalog3;
		unsigned int	DeadBandAnalog4;
		unsigned int	RC_word[1];
	};
#elif DNPCustomer == DNPCustomerDigitalGrid
	struct
	{
		unsigned int settingWord[15];
		unsigned char DBSettings[38];
	};
	struct
	{	
		DNPCONTROLWORD 	ControlWord;
		unsigned long 	UnsolTimeout;
		unsigned int 	FragmentSize;
		unsigned int 	Destination;
		unsigned int 	Source;
		unsigned int 	UnsolMaxRetries;
		unsigned char 	MaxEvents;
		unsigned char 	Holder; //just a position holder to keep other things aligned for comms
		unsigned char	BinaryEventEnables[2]; //9
		unsigned char	AnalogEventEnables[10];
		//unsigned char	Holder2;
		unsigned char	DBVoltage;		
		unsigned char	DBVoltageAngle;
		unsigned char	DBApparentDiffVoltage;
		unsigned char	DBDiffVoltageAngle;
		unsigned char	DBAvgDiffAngle;
		unsigned char	DBAvgDiffVolt;
		unsigned char	DBRealDiffVolt;
		unsigned char	DBAvgRealDiffVolt;
		unsigned char	DBCurrent;
		unsigned char	DBCurrentAngle;
		unsigned char	DBEffCurrent;
		unsigned char	DBEffCurrentAngle;
		unsigned char	DBPositiveSequenceCurrent;
		unsigned char	DBPositiveSequenceCurrentAngle;
		unsigned char	DBNegativeSequenceCurrent;
		unsigned char	DBNegativeSequenceCurrentAngle;
		unsigned char	DBPhasekVA;
		unsigned char	DBPhasekVAAngle;
		unsigned char	DBAveragekVA;
		unsigned char	DBAveragekVAAngle;
		unsigned char	DBPhasekW;
		unsigned char	DBAvgPhasekW;
		unsigned char	DBDiffPositiveSequence;
		unsigned char	DBDiffPositiveSequenceAngle;
		unsigned char	DBDiffNegativeSequence;
		unsigned char	DBDiffNegativeSequenceAngle;
		unsigned char	DBVoltagePositiveSequence;
		unsigned char	DBVoltagePositiveSequenceAngle;
		unsigned char	DBVoltageNegativeSequence;
		unsigned char	DBVoltageNegativeSequenceAngle;
		unsigned char	DBTHDVoltage;
		unsigned char	DBTHDCurrent;
		unsigned char	DBTemperature;
		unsigned char	DBOdometer;
		unsigned char	DBAnalog1;			
		unsigned char	DBAnalog2;
		unsigned char	DBAnalog3;
		unsigned char	DBAnalog4;
	};
#endif

}DNPSettings;


/*
typedef union BINARYOUTPUTCHAR
{
	unsigned char Full;
	unsigned char Value :1, :7;
}BinOutputChar;
*/

typedef union DNPINITVARIABLE
{
	unsigned int word;
	struct
	{
		unsigned int FirstExternalPointRead :1, FirstRegisterRead :1, FirstCommVectorRead :1, InitComplete :1, TimerInitComplete :1, :11;
	};
};

#if DNPCustomer == DNPCustomerDigitalGrid
union DNPDataStruct
{
	struct DigitalGridData
	{
			//Binary Inputs (outputs from this device)
			//Order is important for pointer additions to be right.
			union BinaryInputs
			{
				BinaryPoint	BinaryInputs0;
				struct
				{	
					BinaryPoint	CallingForTrip,
								CallingForClose,
								Floating,
								BlockedOpen,
								RelayPhasedOK,
								PumpProtectLockout,
								BFlag,
								DefaultsLoaded,
								PhasedACB,
								InsensitiveBackFeedDetected,
								DigitalIn1,
								DigitalIn2,
								DigitalIn3,
								DigitalIn4;
				};
			};
			//Binary Outputs (Inputs into this device)
			union BinaryOutputs
			{
				/*BinOutputChar*/
				BinaryPoint BinaryOutputs0;
				struct
				{		
					/*BinOutputChar*/
					BinaryPoint	sRemoteTrip,
								sRelaxClose,
								sBlockOpen,
								sTripModeSensitive,
								sTripModeInsensitive,
								sTripModeTimeDelay,
								sTripModeWattVar,
								sTripModeTripOnPowerDown,
								sTripModeEnableTrimCurve,
								sCloseModeCircleClose,
								sCloseModeOverrideBlockedOnDeadNetwork,
								sSequenceRelay,
								sPumpModeRelayCycle,
								sPumpModeRelayMotorCycle,
								sPumpModeRelayMotorTimeout,
								sPumpModeNeverReclose,
								sClearPumpProtect,
								sClearCycleCount,
								sDigitalOutCntl1,
								sDigitalOutCntl2;
				};
			};
		union analogInputs
		{
			AnalogPointIn	AnalogInputs;
			struct
			{
				
				AnalogPointIn	RelaySerialNo,
								RelayVersionNo,
								Phase1TransformerVoltage,					
								Phase2TransformerVoltage,
								Phase3TransformerVoltage,
								Phase1TransformerVoltageAngle,
								Phase2TransformerVoltageAngle,
								Phase3TransformerVoltageAngle,
								Phase1NetworkVoltage,
								Phase2NetworkVoltage,
								Phase3NetworkVoltage,
								Phase1NetworkVoltageAngle,
								Phase2NetworkVoltageAngle,
								Phase3NetworkVoltageAngle,		//13
								Phase1DifferentialVoltageRMS,
								Phase2DifferentialVoltageRMS,
								Phase3DifferentialVoltageRMS,
								Phase1DifferentialVoltageAngle,
								Phase2DifferentialVoltageAngle,
								Phase3DifferentialVoltageAngle,
								AverageDifferentialVoltage,
								AverageDifferentialVoltageAngle,
								Phase1DifferentialVoltageReal,
								Phase2DifferentialVoltageReal,
								Phase3DifferentialVoltageReal,
								AverageDifferentialVoltageReal,
								Phase1Current,					//26
								Phase2Current,
								Phase3Current,
								Phase1CurrentAngle,
								Phase2CurrentAngle,
								Phase3CurrentAngle,
								EffectiveCurrent,
								EffectiveCurrentAngle,
								CurrentPositiveSequence,
								CurrentPositiveSequenceAngle,
								CurrentNegativeSequence,
								CurrentNegativeSequenceAngle,
								Phase1VA,			//Apparent //38
								Phase2VA,
								Phase3VA,
								Phase1PowerAngle,
								Phase2PowerAngle,
								Phase3PowerAngle,
								AveragePower,
								AveragePowerAngle,
								Phase1Power,		//Real
								Phase2Power,						
								Phase3Power,
								DifferentialVoltagePositiveSequence, //49
								DifferentialVoltagePositiveSequenceAngle,
								DifferentialVoltageNegativeSequence,
								DifferentialVoltageNegativeSequenceAngle,
								NetworkVoltagePositiveSequence,
								NetworkVoltagePositiveSequenceAngle,
								NetworkVoltageNegativeSequence,
								NetworkVoltageNegativeSequenceAngle,
								Phase1NetworkTHD,
								Phase2NetworkTHD,
								Phase3NetworkTHD,
								Phase1CurrentTHD,
								Phase2CurrentTHD,
								Phase3CurrentTHD,
								RelayTemperature,
								RelayCycleCounter,
								Aux1,								
								Aux2,
								Aux3,
								Aux4;
			};		
		};
		
		union analogOutputs
		{
			AnalogPointOut	AnalogOutputs;
			struct
			{
				AnalogPointOut	sTripModeSensitiveTimeDelay,
								sTripModeSensitiveTripCurrent,
								sTripModeTiltAngle,
								sTripModeInsensitiveTripCurrent,
								sTripModeInstantaneousTripCurrent,
								sTripModeTimeDelayTripTimeDelay,
								sTripModeExtendedDelay,
								sTripModeWattVarCurrent,
								sTripModeWattVarAngle,
								sTripModeTripStyle,						//0 hold, 1 pulse, 2 single attempt
								sTripModeTrimAngle,
								sCloseModeCloseTimeDelay,
								sCloseModeRecloseVoltage,
								sCloseModeCloseTiltAngle,
								sCloseModePhaseDetectionVoltage,
								sCloseModePhaseDetectionAngle,
								sCTRatio,
								sPhasingMode,							//0 - ABC, 1 - ACB, 2 Autodetect
								sPumpModeRelayCycleLimit,
								sPumpModeRelayCycleTime,
								sPumpModeMotorCycles,
								sPumpModeMotorTimeout,
								sPumpModeLockoutTime;
			};
		};
	};
};

struct EventRangeVariables
{

		
	int		Voltage,
			VoltageAngle,
			AparrentDiffVolt,
			AparrentDiffVoltAngle,
			AvgAparrentDiffVolt,
			AvgAparrentDiffVoltAngle,
			RealDiffVolt,
			AvgRealDiffVolt,
			Current,
			CurrentAngle,
			EffCurrent,
			EffCurrentAngle,
			CurrentPositiveSequence,
			CurrentPositiveSequenceAngle,
			CurrentNegativeSequence,
			CurrentNegativeSequenceAngle,
			PhaseKVA,
			PhaseKVAAngle,
			AvgPhaseKVA,
			AvgPhaseKVAAngle,
			PhaseKW,
			AvgPhaseKW,
			DiffVoltPositiveSequence,
			DiffVoltPositiveSequenceAngle,
			DiffVoltNegativeSequence,
			DiffVoltNegativeSequenceAngle,
			VoltPositiveSequence,
			VoltPositiveSequenceAngle,
			VoltNegativeSequence,
			VoltNegativeSequenceAngle,
			VoltageTHD,
			CurrentTHD,
			Temperature,
			Odometer,
			Analog1,
			Analog2,
			Analog3,
			Analog4;
};

#elif (DNPCustomer == DNPCustomerMemphis)
union DNPDataStruct
{
	struct DigitalGridData
	{
			//Binary Inputs (outputs from this device)
			//Order is important for pointer additions to be right.
			union BinaryInputs
			{
				BinaryPoint	BinaryInputs0;
				struct
				{	
					BinaryPoint		BIDummy,
									RelayFailure,					//look into
									CallingForTrip,
									CallingForClose,
									Floating,
									BlockedOpen,
									RelaxedCloseEnabled,
									NetworkVoltsTooLowToClose,		//not sure
									DifferentialVoltsTooLowToClose,
									PhaseAngleIncorrectToClose,		
									PumpProtectLockout,
									BFlag,
									DigitalIn1,
									DigitalIn2,
									DigitalIn3,
									DigitalIn4, 
									//End P1
									BIDummy2,
									RemoteClose,					//Linked To Relax Close
									RemoteTrip,						//kind of pointless
									DBlockedOpen,					//duplicate
									SequenceRelay,
									GERelayEnabled,
									Frequency,						//hard code to 0 = 60hz, 1 = 50hz
									CircleCloseEnabled,					
									TimeDelayModeEnabled,
									InsensitiveModeEnabled,
									WattVarModeEnabled,
									DPumpProtect,
									SensitiveModeEnabled,
									DInsensitiveModeEnabled,		//duplicate above
									VoltageConstantEnabled,
									DefaultsLoaded,
									//End p3
									DigitalOut1,
									DigitalOut2,
									DigitalOut3,
									DigitalOut4;
									//End P5
									
				};
			};
			//Binary Outputs (Inputs into this device)
			union BinaryOutputs
			{
				/*BinOutputChar*/
				BinaryPoint BinaryOutputs0;
				struct
				{		
					/*BinOutputChar*/
					BinaryPoint		sBODummy,
									sRemoteTrip,		
									sBlockOpen,
									sRelaxClose,
									//End P1
									sGetEventLog,		
									sCaptureWaveForm,
									//End P4
									sDigitalOutCntl1,
									sDigitalOutCntl2,
									sDigitalOutCntl3,
									sDigitalOutCntl4;
									//End P5
									
				};
			};

		union analogInputs
		{
			AnalogPointIn		AnalogInputs;
			struct
			{
				
				AnalogPointIn		AIDummySpacer,						//0
									DeviceAddress,
									RelaySerialNoHigh,
									RelaySerialNoLow,
									RelayCycleCounter,
									Manufacturer,
									Phase1Current,
									Phase2Current,
									Phase3Current,
									Phase1NetworkVoltage,
									Phase2NetworkVoltage,
									//10
									Phase3NetworkVoltage,
									Phase1TransformerVoltage,
									Phase2TransformerVoltage,
									Phase3TransformerVoltage,			
									Phase1DifferentialVoltageRMS,
									Phase2DifferentialVoltageRMS,
									Phase3DifferentialVoltageRMS,
									Phase1DifferentialVoltageReal,
									Phase2DifferentialVoltageReal,
									Phase3DifferentialVoltageReal,
									//20
									Phase1PowerAngle,
									Phase2PowerAngle,
									Phase3PowerAngle,
									Phase1NetworkTHD,
									Phase2NetworkTHD,
									Phase3NetworkTHD,
									Phase1CurrentTHD,
									Phase2CurrentTHD,
									Phase3CurrentTHD,
									//End P1
									RelayTemperature,
									//30
									Phase1Power,		//Real
									Phase2Power,						
									Phase3Power,						
									Phase1VAR,			//Reactive
									Phase2VAR,
									Phase3VAR,
									Phase1VA,			//Apparent
									Phase2VA,
									Phase3VA,
									TotalPower,							
									//40
									TotalVAR,
									TotalVA,
									MasterSoftwareRevision,
									SystemVoltage,				//?
									CTRatio,
									//End P2
									CloseTiltAngle,				//think this may be right one
									SensitiveTripSettingPercent,		
									TimeDelaySeconds,
									InstantTripCurrent,			
									RecloseVolts,
									PhasingRecloseAngle,				//might be above
									SensitiveTripSettingAmps,
									InsensitiveTripCurrentAmps,
									ExtendedDelay,
									RecloseTimeDelay,
									SensitiveTripDelay,
									NetworkLowerLimitToClose,	//no idea what this is
									PumpRelayTimeLimit,
									WattVarCurrent,
									WattVarAngle,
									PhaseCompensation,
									DNetworkVoltsTooLowToClose,	//no idea
									PhaseDetectionVolts,
									TripPulses,					//no idea what this is
									DD_RecloseAngle,			//mightbeAbove
									SensitiveTripAngle,
									RelaxedCloseVolts,			
									RelaxedClosePhasingAngle,			
									RelaxedCloseActiveTime,		//ignore
									RelaxedPhaseDetectionVolts,	
									ClosePenalty,
									UpperLimit,
									LowerLimit,
									PumpRelayCycleLimit,
									DPumpRelayTimeLimit,
									PumpProtectTime,
									RelayDeEnergizedAction,		
									RelayFailurePosition,	
									RecloseAlgorithm,			//either 1 or 6
									PhaseSensitivity,			//need to do 0 off? 1 = ABC 2 = ACB
									PumpProtectionMode,
									//End P3
									NumberOfEvents,			
									WaveCaptureSampleRate,
									NumberOfCyclesEvent,
									//End P4 - also includes waveform data outside of this map
									Aux1,								
									Aux2,
									Aux3,
									Aux4,
									Aux5,
									Aux6,
									Aux7,
									Aux8,
									Aux9,
									RelaySpare1,
									RelaySpare2,
									RelaySpare3,
									RelaySpare4,
									RelaySpare5,
									RelaySpare6,
									RelaySpare7,
									RelaySpare8;
									//End P5									
			};
		};
	
		
		union analogOutputs
		{
			AnalogPointOut	AnalogOutputs;
			struct
			{
				AnalogPointOut	sDummy,					
								sEventNumber,
								sEventNumber2;
			};
		};
		
		union dateTime
		{
			AnalogPointIn DateTimeAI;
			struct
			{
				AnalogPointIn DayOfWeek,
								Month,
								Year,
								Hour,
								Minute,
								Second,
								MilliSecond,
								DayOfMonth;
			};
		};
		
	};
};
/*
struct SavedDeadBandVariables
{
	long	Phase1NetworkVoltage,
			Phase2NetworkVoltage,
			Phase3NetworkVoltage,
			Phase1TransformerVoltage,
			Phase2TransformerVoltage,
			Phase3TransformerVoltage,
			Phase1Current,
			Phase2Current,
			Phase3Current,
			Phase1NetworkTHD,
			Phase2NetworkTHD,
			Phase3NetworkTHD,
			Phase1CurrentTHD,
			Phase2CurrentTHD,
			Phase3CurrentTHD,
			Analog1,
			Analog2,
			Analog3,
			Analog4,
			Temperature;
};
*/
struct EventRangeVariables
{
	int		Voltage,
			THD,
			Current,
			Temperature,
			Odometer,
			DifferentialVoltage,
			RealDiffVoltage,
			CurrentAngle,
			PhaseKW,
			PhaseKVAR,
			PhaseKVA,
			TotalKW,
			TotalKVAR,
			Analog1,
			Analog2,
			Analog3,
			Analog4;
};

#endif

typedef struct DateStruct
{
	unsigned int Year;
	unsigned char Month;
	unsigned char DayOfMonth;
	unsigned char DayOfWeek;
	unsigned char Hour;
	unsigned char Minute;
	unsigned char MSecAndSec;
}Date;


//Only should be setting ConfigurationCorrupt, LocalMode, and DeviceTrouble
typedef union
{
	unsigned int Full;
	struct
	{	
		unsigned short IIN1;
		unsigned short IIN2;
	};
	struct
	{
		
		unsigned int	AllStations :1, C1Available :1, C2Available :1, C3Available :1,
						TimeSyncRequired :1, LocalMode :1, DeviceTrouble :1, DeviceRestart :1,
						FunctionUnknown :1, ObjectUnknown :1, ParameterError :1, BufferOverflow :1,
						AlreadyExecuting :1, ConfigurationCorrupt :1, XIIN2_6 :1, XIIN2_7 :1;
	};
	struct
	{
		
		unsigned int 	IIN1_0 :1, IIN1_1 :1, IIN1_2 :1, IIN1_3 :1, 
						IIN1_4 :1, IIN1_5 :1, IIN1_6 :1, IIN1_7 :1,
						IIN2_0 :1, IIN2_1 :1, IIN2_2 :1, IIN2_3 :1, 
						IIN2_4 :1, IIN2_5 :1, IIN2_6 :1, IIN2_7 :1;
	};
}IINDef;


typedef struct sUSERCONTEXT
{
	unsigned int stuff;
}USERCONTEXT;

typedef struct sDNPCONTROLBITS
{
	unsigned int InProgress :1, DNPTimerRollOver :1, :15;
}DNPControlBits;

/*IIN Octets 3 and 4 of the application response header. Bits are named IINx-y. x=1 for first transmitted octet and 2 for
second transmitted octet. y=bit number where 0=least significant bit. Refer to the Basic Four Application Layer
section 3.6 and the Subset Definitions section 4.1.1 for detailed explanations of the IIN bits. The following is a
summary of the definitions.
IIN1-1 Class 1 event data available. Can be set at any time and does not indicate an error condition.
IIN1-2 Class 2 event data available. Can be set at any time and does not indicate an error condition.
IIN1-3 Class 3 event data available. Can be set at any time and does not indicate an error condition.
IIN1-4 Time synchronization required. Can be set at any time and does not indicate an error condition.
IIN1-5 Local mode. Set if some points are uncontrollable via DNP.
IIN1-7 Device restart. Set only under specific conditions. Does not indicate an error condition.
IIN2-0 Function Unknown. Generally means that the function code (octet 2 of the request header) cannot be processed.
IIN2-1 Object Unknown. Generally means that the function code could be processed but the object group / variation
could not be processed.
IIN2-2 Parameter Error. Generally indicates that both the function code and object group / variation could be processed
but that the qualifier / range field is in error.
*/

typedef union FIXED16_16
{
	long Full16;
	struct part16_16
	{
		long	Fraction16: 16,
				Integer16: 16;
	};
};
#endif   
   
//"public" functions, should be put in function prototypes at some point
void DNPMain(void);
void DNPPopulateEntropyArray();
void InitializeDNP(void);
void InitializeDNPTimer(void);
void DNPSetMemphisStage(void);
void DNPSendMessage(char * message);
void DNPSendMessageUInt(unsigned int value);
void DNPConvertRelayTimeToDNP(unsigned long eventSeconds, unsigned long mSecTimer, TMWDTIME *date);
void DNPConvertRelayTimeToMemphisDNPData(unsigned long eventSeconds, unsigned long mSecTimer);
unsigned long DNPConvertDNPTimeToRelay(TMWDTIME *date);
unsigned short * GetIIN(void); 
void relay_data(unsigned int *);
long multiplyBy1Convert(long data);
long multiplyBy10Convert(long data);
long multiplyBy100Convert(long data);
long divideBy10Convert(long data);
long divideBy100Convert(long data);
void convertFlashMemoryToDNPDB(void);
//TEST remove all double double getAngle(long tangent, double negativeAdjustValue);
//long convertToDNPLong(double value, double multiplier);
//double angleFromRadians(double radians, double negativeAdjustValue);
//double roundDouble(double value);
long convertMasterRevisionStringToNumber(void);
void SaveParameters(void);
void ResetMyParameters(void);
void SetInProgress(unsigned int inProgress);
void DNP_command(unsigned int *dataPtr);
void SaveDNPSettings();
void setSelfAddress(unsigned int *dataPtr);
void setUnsolicited(unsigned int *dataPtr);
void setLinkLayerConfirm(unsigned int *dataPtr);
void setUnsolTimeout(unsigned int *dataPtr);


void handleTripCycles(unsigned int *dataPtr);
void handleCommVectorData(unsigned int *dataPtr);
void handleRevisionNumber(unsigned int *dataPtr);
void storeRegisters(unsigned int *dataPtr);
long dNPLongFrom8_8IQ(unsigned int i, unsigned int q);
long dNPLongFrom8_8_40(unsigned int i, unsigned int q);
long dNPLongFrom5_11IQ(unsigned int i, unsigned int q);
long dNPCurrentFrom4_12(unsigned int data);
long dNPLongFrom8_8(unsigned int data);
long dNPLongFrom8_8Memphis(unsigned int data);
long dNPLongFrom8_8diffReal(unsigned int data);
long dNPLongFrom9_7(unsigned int data);
long dNPLongFrom8_8diffRMS(unsigned int real, unsigned int imaginary);
long dNPLongFrom5_11(unsigned int data);
long dNPLongFrom5_11kW(unsigned int data, unsigned int data2);
long dNPLongFrom5_11kW_1500(unsigned int data, unsigned int data2);
long dNPLongFrom5_11kW_1000(unsigned int data, unsigned int data2);
long totalKW(void);
int *	DNPEventPoint(unsigned short point);
int DNPEventData(int * dNPPoint);

void DNPCheckExternalDigitalFlags(TMWDTIME *time);
void DNPCheckExternalAnalogs(TMWDTIME *time);
long DNPConvertExternalAnalog(int workingCurrent);

//Event Functions
unsigned int	InEventRegion(int * dataPoint);
unsigned int	DNPVariableAnalogPoint(AnalogPointIn * dataPoint);
void setScalingFactors(void);
void selectProperEvent(void);
union FIXED16_16	DIV16_16(union FIXED16_16, union FIXED16_16);
union FIXED16_16	MULT16_16(union FIXED16_16 a, union FIXED16_16 b);
union FIXED16_16	Sqrt16(union FIXED16_16 a);
unsigned int 		signalSwitch(int * point);
int					convertNetworkVoltageReading(int data, unsigned int * calPtr);
int					convertDifferentialVoltageReading(int * data, unsigned int * calPtrN, unsigned int *calPtrT);
int					convertCurrentReading(int data, unsigned int * calPtr);


char * ConvertSerialNumberToString(unsigned int number);

#if DNPCustomer == DNPCustomerMemphis
long totalVAR(void);
long totalVA(void);
#endif
int memphisTemperature(int temp);


//double radiansFromAngle(double angle);
//long getTangent(double angle);
long DIV20_12(long dividend, long divisor);

unsigned int isLeapYear(unsigned int year);

TMWTYPES_BOOL SelectBinaryControl(void *pPoint, TMWTYPES_UCHAR value);
TMWTYPES_BOOL OperateBinaryControl(void *pPoint, TMWTYPES_UCHAR value);
TMWTYPES_UCHAR SelectAnalogControl(void *pPoint, int value);
TMWTYPES_UCHAR OperateAnalogControl(void *pPoint, int value);
TMWTYPES_BOOL EventCheckBinary(unsigned char *point, TMWTYPES_UCHAR *pFlags);

extern union DNPINITVARIABLE DNPInit;
extern TMWDTIME time;

extern TMWDTIME CurrentTime;
extern USERCONTEXT myUserContext;
extern unsigned long DNPTimer;
extern union event_clock;
const extern unsigned long RelayTimeConversion;
extern union DNPDataStruct DNPData;
extern DATACOPY MyParameters;
extern IINDef IIN;

extern union FIXED16_16 scalingFactor;
extern int eventRange;

//extern CORDIC_TABLE mp_cordic_table[21];
const extern int m_max_L;
//TEST remove all double const extern double m_mag_scale;

extern int DNPAnalogInQuantity;
extern int DNPAnalogOutQuantity;
extern int DNPBinaryInQuantity;
extern int DNPBinaryOutQuantity;

extern unsigned char DNPEntropy[DNP_ENTROPY_SIZE];
