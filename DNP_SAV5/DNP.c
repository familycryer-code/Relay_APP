#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <cmath>
#include <math.h>
#include "peripheral registers.h"
#include "function_prototypes.h"
#include "tmwdb.h"
#include "tmwpltmr.h"
#include "tmwtarg.h"
#include "dnpchnl.h"
#include "sdnpsesn.h"
#include "tmwdtime.h"
#include "DNP.h"
#include "oddball.h"
#include "definitions.h"
#include "cordic.h"
#include "sdnpo032.h"
#include "sdnpo002.h"
#include "myCryptoFunctions.h"

/*
Calls into the SCL library
x = done

x tmwtimer_initialize 
x tmwappl_initApplication
x tmwtarg_initConfig 
x dnpchnl_initConfig 
x dnpchnl_openChannel 
x sdnpsesn_initConfig 
x sdnpsesn_openSession 
x tmwpltmr_checkTimer 
x tmwappl_checkForInput 
x sdnponnn_addEvent 
*/
#ifdef DNP_Relay

const char DNP_version[] = {"DNP VERSION 130910/r/n"};
const long conver5100_8_8 = 5100;
long long testLL;

DNPCONTROLWORD DNPControlWord;
#if SDNPDATA_SUPPORT_OBJ120 

//unsigned char DNPEntropy[DNP_ENTROPY_SIZE];
void myInitSecureAuthentication(SDNPSESN_CONFIG *pSesnConfig); 
/* These are the default user keys the test harnessuses for testing 
* DO NOT USE THESE IN A REAL DEVICE 
*/
static TMWTYPES_UCHAR defaultUserKey1[] = { 
0x49, 0xC8, 0x7D, 0x5D, 0x90, 0x21, 0x7A, 0xAF, 
0xEC, 0x80, 0x74, 0xeb, 0x71, 0x52, 0xfd, 0xb5 
}; 
static TMWTYPES_UCHAR defaultUserKeyOther[] = { 
0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 
0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff 
}; 
/* This one is used by the Authority and the Outstation. 
*/
static TMWTYPES_UCHAR authoritySymCertKey[] ={ 
0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 
0x09, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 
0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 
0x09, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 
}; 
#endif

const CORDIC_TABLE mp_cordic_table[21] =
{
	1.00000, 0.783132, 0.500000, 0.463648,
	0.250000, 0.244979, 0.125000, 0.125000,
	6.250000e-02, 6.250000e-02, 3.125000e-02, 3.125000e-02,
	1.562500e-02, 1.562500e-02, 7.812500e-03, 7.812500e-03,
	3.906250e-03, 3.906250e-03, 1.953125e-03, 1.953125e-03,
	9.765625e-04, 9.765625e-04, 4.882812e-04, 4.882812e-04,
	2.441406e-04, 2.441406e-04, 1.220703e-04, 1.220703e-04,
	6.103516e-05, 6.103516e-05, 3.051758e-05, 3.051758e-05,
	1.525879e-05, 1.525879e-05, 7.629395e-06, 7.629395e-06,
	3.814697e-06, 3.814697e-06, 1.907349e-06, 1.907349e-06, 
	9.536743e-07, 9.536743e-07
};

const int m_max_L = 20;
//const double m_mag_scale = 0.60725303019262066117709927372538;

typedef union tdRELAYFLAGS
{
	unsigned int Full;
	struct rfBit
	{
		unsigned int Pumping :1, MathTimeOver :1, OffsetOkay :1, DefaultValues :1, MathError :1, 
					 MonitorPhasors :1, InitComplete :1, SequenceRelay :1, ACB :1, 
					 PumpProtectEnabled :1, VoltageChecked :1, Test1 :1, FlashCompare :1, BFlag :1, InInsensitiveRegion : 1, InTripRegion :1;	
	};
	
}RELAYFLAGS;

typedef union typeCOMMFLAGS
{
	unsigned int Full;
	struct bit
	{
		unsigned int 	Trip :1, Close :1, Pumping :1, BlockedOpen :1, InsensitiveBackfeed :1, 
						RelaxedClose :1, BFlag :1, FlashError : 1, HighVoltage :1, Transient : 1, 
						IncreaseCycleCount : 1, BadPumpMode :1, BadRelayType :1, BadMode :1, 
						BadCloseCurve :1, BadTripCurve :1;
	};
}COMMFLAGS;

typedef union R_STAT_REG
{
	unsigned int Full;
	struct rsgBit
	{
		unsigned int Test1 :1, Test2 :1, Test3 :1, Test4 :1, Test5 :1, PumpReason : 3, TripFlag:1, PowerSaveFlag:1, Tripping:1, Float: 1, 
					BlockedClosed :1, BlockedOpen :1, PhasingOK :1, Calibration :1;
	};
}RELAYSTATUS;

//TEST look closely at pumpenable because it changed from char to word

typedef union tdPUMPENABLE
{
	unsigned int word;
	struct pEBit
	{
		unsigned int RelayCycle :1, ClearPumpMode: 1, MotorCycle :1, MotorTimeout :1, :12;		
	};
}PUMPENABLE;

TMWDTIME CurrentTime;
TMWAPPL *myApplContext;
TMWCHNL *mySclChannel;
TMWSESN *mySclSession;
DNPCHNL_CONFIG myDNPConfig;
DNPTPRT_CONFIG myTprtConfig;
TMWTARG_CONFIG myTargConfig;
TMWPHYS_CONFIG myPhysConfig;
DNPLINK_CONFIG myLinkConfig;
SDNPSESN_CONFIG mySesnConfig;
IINDef IIN;
DNPControlBits DNPControl;
USERCONTEXT myUserContext;
TMWTYPES_CHAR pKeyTEST[32];
int *myIOCnfg;
union DNPDataStruct DNPData;
//struct SavedDeadBandVariables SavedDeadBand;
struct EventRangeVariables EventTriggerRange;
DATACOPY MyParameters;
DNPSettings MyDNPSettings;

int DNPAnalogInQuantity = AnalogInQuantity;
int DNPAnalogOutQuantity = AnalogOutQuantity;
int DNPBinaryInQuantity = BinaryInQuantity;
int DNPBinaryOutQuantity = BinaryOutQuantity;

unsigned long DNPTimer = 1;
unsigned long DNP1SecTimer = 0;
union DNPINITVARIABLE DNPInit = { 0 };
const unsigned long RelayTimeConversion = 302400000; //Standard Time of 8/1/2009 12:00am in seconds
const extern unsigned long YearSec[128];
const extern unsigned long MonthSec[12];
const extern unsigned long LeapMonthSec[12];
const extern unsigned long secondsInDay;

int eventRange;
union FIXED16_16 scalingFactor;
union recorderevent *workingEvent;
int eventStartingPoint;

void initializeSesnConfig(void);
void initializeDNPVariables(void);
void initializeDNPConfig(void);
void DNPInitializeBinaryEventsFromMemory(void);
void DNPInitializeAnanlogEventsFromMemory(void);
void DNPSet8Enables(BinaryPoint * pointArray, unsigned char enableChar, unsigned int numberOfPoints);
void DNPSet8AnalogEnables(AnalogPointIn * pointArray, unsigned char enableChar, unsigned char numberOfPoints);

void setAll(unsigned int *dataPtr);
void setDeadBandLimits(unsigned int *dataPtr);
void setBinaryEventEnables(unsigned int *dataPtr);
void setAnalogEventEnables(unsigned int *dataPtr);
void checkVariableAnalogEvent(AnalogPointIn *point, TMWTYPES_UCHAR flags, TMWDTIME *time, long range);
void checkBinaryEvent(BinaryPoint * point, unsigned char oldValue, unsigned char newValue, TMWTYPES_UCHAR flags, TMWDTIME *time);
void setFragmentSize(unsigned int *dataPtr);
void setSourceAddress(unsigned int *dataPtr);
void setDestinationAddress(unsigned int *dataPtr);
void setMaxEvents(unsigned int *dataPtr);
void setUnsolRetries(unsigned int *dataPtr);
void setTerminationResistor(unsigned int *dataPtr);
void initializeConstantDNPVariables(void);
void DNPSetNumberOfEvents(void);
void DNPSetEventDateTime(long eventNumber);

union recorderevent * selectedEvent(int eventNumber);
TMWTYPES_UCHAR OperateAnalogControlMemphis(void *pPoint, int value);
TMWTYPES_UCHAR SelectAnalogControlMemphis(void *pPoint, int value);
unsigned int DNPCurrentHighEnoughForTHD(unsigned int i);

int dNPAddAngles(long a, long b);
//TEST remove all double long dNPConvert10FractMult10(long data);

char testArray[50];
void InitializeDNP(void)
{
	TMWTYPES_USHORT userNumber;
	TMWTYPES_USHORT keyLength;
	
	
	
	
	///TEST
	unsigned short *testSize;
	int i = 0;
	*testSize  = 22;
	for (i = 0 ; i < 50; i++)
	{
		testArray[i] = i;
	}
	memset(testArray, 0, 50);
	/*
	DNPData.RelaySerialNo.Value = 1234;
	
	sdnpdata_authGetOSName(&testArray[50], &testArray[0], testSize);
	return;
	////
	*/
	
	InitializeSAv5();
	
	convertFlashMemoryToDNPDB();
	
	ResetMyParameters();
	
	if(!DNPInit.InitComplete)
	{

		InitializeDNPTimer();

		revision_request();
		
		tmwtimer_initialize();


		
		myApplContext = tmwappl_initApplication();
		
		// Initialize channel configuration to defaults 
		tmwtarg_initConfig(&myTargConfig);
		
		dnpchnl_initConfig(&myDNPConfig, &myTprtConfig, &myLinkConfig, &myPhysConfig);
		myLinkConfig.networkType = DNPLINK_NETWORK_NO_IP;
		
		initializeDNPConfig();

		
		
		mySclChannel = dnpchnl_openChannel(myApplContext, &myDNPConfig, &myTprtConfig, &myLinkConfig, &myPhysConfig, &myIOCnfg, &myTargConfig);
		
		initializeSesnConfig();
		
		
		#if SDNPDATA_SUPPORT_OBJ120 
		
		
		/* 	Set this to true to enable DNP3 Secure A
			bool myUseAuthentication = true; 
		/* If Secure Authentication is to be enable*/
		
 		myInitSecureAuthentication(&mySesnConfig); 
		#endif 
		
		mySclSession = (TMWSESN *)sdnpsesn_openSession(mySclChannel, &mySesnConfig, &myUserContext);
		
		
		
		#if SDNPCNFG_SUPPORT_SA_VERSION5 
		/* 	In SAv5 we no longer use an array of user configuration, instead you can add one user at a time. 
		* 	The Authority can also tell the master to add a user using a globally unique user name 
		* 	and instruct the master to send the update key and role (permissions) for that user over DNP to the outstation. 
		*/

		if(!mySesnConfig.authConfig.operateInV2Mode)
		{ 
			userNumber = 1; 
			/* Add the user to the sdnp library */
			sdnpsesn_addAuthUser(mySclSession, userNumber); 
			
			
			/* If using simulated database in tmwcrypto, add the User Update Key for the default user (1) to it. 
			* This key should really should be in YOUR crypto database not the simulated one in tmwcrypto.c. 
			* You would not normally need to call tmwcrypto_setKeyData to add it to your own database. 
			*/
			//if(!tmwcrypto_setKeyData(TMWDEFS_NULL, TMWCRYPTO_USER_UPDATE_KEY, (void *)userNumber, defaultUserKey1, 16)) 
			//{ 
				/* failed to add key */
			//} 
			/* If using simulated database in tmwcrypto, add the User Update Key for the second user number. 
			* This really should be in YOUR crypto database not the simulated one. 
			*/
			userNumber = 100; 
			/* Add the user to the sdnp library */
			sdnpsesn_addAuthUser(mySclSession, userNumber); 
			/* If using simulated database in tmwcrypto, add the User Update Key to it. 
			* This key should really should be in YOUR crypto database not the simulated one in tmwcrypto.c. 
			* You would not normally need to call tmwcrypto_setKeyData to add it to your own database. 
			*/
			//if(!tmwcrypto_setKeyData(TMWDEFS_NULL, TMWCRYPTO_USER_UPDATE_KEY, (void *)userNumber, defaultUserKeyOther, 16)) 
			//{ 
				/* failed to add key */
			//} 
			/* Configure other values in YOUR crypto database to allow remote user key and role update from Master.*/
			/* This sample uses the same values that the Test Harness Outstation uses by 
			default */
			/* Outstation name must be configured in both Master and Outstation. 
			* This is already set in sdnpsim database 
			* OutstationName = "SDNP Outstation"; 
			*/
			/* If using simulated database in tmwcrypto, configure the Authority Certification Symmetric Key to it. 
			* This key really should be in YOUR crypto database not the simulated one in tmwcrypto.c. 
			* You would not normally need to call tmwcrypto_setKeyData to add it to your own database. 
			* This key is used by the Central Authority, not the master, but this sample is acting as the Authority. 
			*/
			//if(!tmwcrypto_setKeyData(TMWDEFS_NULL, TMWCRYPTO_AUTH_CERT_SYM_KEY, TMWDEFS_NULL, (TMWTYPES_UCHAR *)&authoritySymCertKey, 32)) 
			//{ 
				/* failed to add key */
			//} 
			/* If using simulated database in tmwcrypto, configure Outstation Private Key when Asymmetric Key Update is supported. 
			* This really should be in YOUR crypto database not the simulated one. 
			* You would not normally need to call tmwcrypto_setKeyData to add it to your own database. 
			*/
			//sprintf( pKey, "TMWTestOSAsymPrvKey.pem"); 
			//keyLength = strlen(pKeyTEST); 
			//if(!tmwcrypto_setKeyData(TMWDEFS_NULL, TMWCRYPTO_OS_ASYM_PRV_KEY, TMWDEFS_NULL, (TMWTYPES_UCHAR *)pKeyTEST, keyLength)) 
			//{ 

			//} 
			/* If using simulated database in tmwcrypto, configure Authority Public Key when Asymmetric Key Update is supported 
			* This really should be in YOUR crypto database not the simulated one. 
			* You would not normally need to call tmwcrypto_setKeyData to add it to your own database. 
			*/

			//keyLength = strlen(pKeyTEST); 
			//if(!tmwcrypto_setKeyData(TMWDEFS_NULL, TMWCRYPTO_AUTH_ASYM_PUB_KEY, TMWDEFS_NULL, (TMWTYPES_UCHAR *)pKeyTEST, keyLength)) 
			//{ 
				/* failed to add key */
			//} 
		} 
#endif 
		//Termination Resistor
		//A 0 on DB5 would enable the termination.
		//by default flash is zero so i just switch it to make the termination
		//disabled by default
		
		GPIOC_Data_reg.bit.DB5 = !MyDNPSettings.ControlWord.TerminationResistor;
		
		DNPInit.InitComplete = 1;
	}
	
	initializeConstantDNPVariables();
	initializeDNPVariables();
	
	DNPInitializeBinaryEventsFromMemory();
	DNPInitializeAnanlogEventsFromMemory();
}

#if SDNPDATA_SUPPORT_OBJ120 
void myInitSecureAuthentication(SDNPSESN_CONFIG *pSesnConfig) 
{ 
	InitializeSAv5();
	/* set this to TMWDEFS_TRUE to use SAv2 implementation */
	//TMWTYPES_BOOL myUseSAv2 = TMWDEFS_FALSE; 
	//matrixSslOpen();
	/* Enable DNP3 Secure Authentication support */
	pSesnConfig->authenticationEnabled = TMWDEFS_TRUE; 
	/* NOTE: Secure Authentication Version 2 (SAv2) will not function properly without implementing 
	* the functions sdnpdata_authxxx() in sdnpdata.c 
	* SAv5 also requires utils/tmwcrypto which uses OpenSSL as a sample implementation. 
	*/
	/* For SAv2 configure the same user numbers and update keys for each user 
	* number on both master and outstation devices. These must be configured before the 
	* session is opened. 
	* SAv5 allows User Update Keys to also be sent to the outstation over DNP. 
	*/
	/* Example configuration. Some of these may be the default values, 
	* but are shown here as an example of what can be set. 
	*/
	pSesnConfig->authenticationEnabled = TMWDEFS_TRUE;
	pSesnConfig->authConfig.extraDiags = TMWDEFS_TRUE; 
	pSesnConfig->authConfig.aggressiveModeSupport = TMWDEFS_TRUE; 
	pSesnConfig->authConfig.maxKeyChangeCount = 1000; 
	// 120 seconds is very short for demonstration purposes only.
	pSesnConfig->authConfig.keyChangeInterval = 120000; 
	pSesnConfig->authConfig.assocId = 0; 
} 
#endif 

void InitializeDNPTimer()
{
	//timer b0 for ms counter

	timerB0_load = 0xEA60;							//0xEA60 = .001s (1ms)
	timerB0_control.bit.count_mode = 1; 			//1 = count falling edge
	timerB0_control.bit.primary_count_src = 0b1000; //0b1000 = peripheral clock / 1
	timerB0_control.bit.second_src = 0; 			//0 = none
	timerB0_control.bit.once = 0; 					//1 = count once
	timerB0_control.bit.length = 1; 				//1 = reload load after count
	timerB0_control.bit.count_direction = 1; 		//1 = count down
	timerB0_control.bit.co_chan_init = 0; 			//0 = use just primary count
	timerB0_control.bit.output_mode = 0b011; 		//b011 = toggle on compare.
	
	timerB0_status.bit.MSTR = 0;
	timerB0_status.bit.TCFIE = 1;
	
	Intrpt_Priority_Reg7.bit.TMRB0 = 1;
	
	DNPInit.TimerInitComplete = 1;
}

void initializeDNPConfig()
{
	if(MyDNPSettings.ControlWord.LinkLayerConfirm == 2)
		myLinkConfig.confirmMode = TMWDEFS_LINKCNFM_ALWAYS;		//2
	else if (MyDNPSettings.ControlWord.LinkLayerConfirm == 1)
		myLinkConfig.confirmMode = TMWDEFS_LINKCNFM_SOMETIMES;	//1
	else
		myLinkConfig.confirmMode = TMWDEFS_LINKCNFM_NEVER;	
	
	myDNPConfig.rxFragmentSize = MyDNPSettings.FragmentSize;
	myDNPConfig.txFragmentSize = MyDNPSettings.FragmentSize;
	
	if(myDNPConfig.txFragmentSize == 0 || myDNPConfig.txFragmentSize > 1024)
	{
		myDNPConfig.rxFragmentSize = MyDNPSettings.FragmentSize = 1024;
		myDNPConfig.txFragmentSize = MyDNPSettings.FragmentSize = 1024;
	}
	
	#if DNPCustomer == DNPCustomerMemphis
	DNPSetMemphisStage();
	#endif
}

void initializeSesnConfig()
{
	sdnpsesn_initConfig(&mySesnConfig);
	
	if(MyDNPSettings.ControlWord.SelfAddress == 1)
		mySesnConfig.enableSelfAddress = TMWDEFS_TRUE;
	else
		mySesnConfig.enableSelfAddress = TMWDEFS_FALSE;
	
	mySesnConfig.allowMultiCROBRequests = TMWDEFS_FALSE;
	
	if(MyDNPSettings.ControlWord.UnsolicitedAllowed == 1)
	{
		mySesnConfig.unsolAllowed = TMWDEFS_TRUE;
		
	}
	else
	{
		mySesnConfig.unsolAllowed = TMWDEFS_TRUE;//TEST TMWDEFS_FALSE;
		mySesnConfig.unsolClassMask = 0; //All disabled
	}
	
	mySesnConfig.applConfirmTimeout = MyDNPSettings.UnsolTimeout;
	
	
	if(mySesnConfig.applConfirmTimeout < 1000)
	{
		
		mySesnConfig.applConfirmTimeout = 1000;
		MyDNPSettings.UnsolTimeout = 1000;
		SaveDNPSettings();
	}
	
	mySesnConfig.destination = MyDNPSettings.Destination;
	if(mySesnConfig.destination == 0)
		mySesnConfig.destination = MyDNPSettings.Destination = 3;
	
	mySesnConfig.source = MyDNPSettings.Source;
	if(mySesnConfig.source == 0)
		mySesnConfig.source = MyDNPSettings.Source = 4;
	

	mySesnConfig.unsolRetryDelay = 0;
	mySesnConfig.unsolMaxRetries = 20;
	

	if(MyDNPSettings.UnsolMaxRetries == 0)
	{
		//Make infinite
		mySesnConfig.unsolOfflineRetryDelay = mySesnConfig.unsolRetryDelay; //this makes infinite retries
	}
	else
	{
		mySesnConfig.unsolMaxRetries = MyDNPSettings.UnsolMaxRetries;
		mySesnConfig.unsolOfflineRetryDelay = TMWDEFS_DAYS(31);
	}
	
	mySesnConfig.unsolClass1MaxEvents = MyDNPSettings.MaxEvents;
	mySesnConfig.unsolClass2MaxEvents = MyDNPSettings.MaxEvents;
	mySesnConfig.unsolClass3MaxEvents = MyDNPSettings.MaxEvents;
}

void initializeConstantDNPVariables(void)
{
#if DNPCustomer == DNPCustomerMemphis
	DNPData.Manufacturer.Value = 3;
	DNPData.DeviceAddress.Value = MyDNPSettings.Source;
	DNPData.RelaySerialNoHigh.Value = 0;
	//DNPData.RemoteRelaySettingEnabled = 1;
	DNPData.GERelayEnabled.Value = 0;
	DNPData.Frequency.Value = 0;
	DNPData.SystemVoltage.Value = 125;
	DNPData.TripPulses.Value = 3;
	DNPData.RelaxedCloseVolts.Value = 1;
	DNPData.RelaxedClosePhasingAngle.Value = -5;
	DNPData.RelaxedCloseActiveTime.Value = 0;
	DNPData.RelaxedPhaseDetectionVolts.Value = 0;
	DNPData.ClosePenalty.Value = 1;
	DNPData.UpperLimit.Value = 10;
	DNPData.LowerLimit.Value = 10;
	DNPData.RelayFailurePosition.Value = 2;
	DNPData.RecloseAlgorithm.Value = 0;
	DNPData.Phase1PowerAngle.Control.AngleValue = 1;		//Angle Value put in to cover cases where 359 far away from 0, when in reality it isn't
	DNPData.Phase2PowerAngle.Control.AngleValue = 1;
	DNPData.Phase3PowerAngle.Control.AngleValue = 1;
#else if DNPCustomer == DNPCustomerDigitalGrid
	DNPData.Phase1TransformerVoltageAngle.Control.AngleValue = 1;
	DNPData.Phase2TransformerVoltageAngle.Control.AngleValue = 1;
	DNPData.Phase3TransformerVoltageAngle.Control.AngleValue = 1;
	DNPData.Phase1NetworkVoltageAngle.Control.AngleValue = 1;
	DNPData.Phase2NetworkVoltageAngle.Control.AngleValue = 1;
	DNPData.Phase3NetworkVoltageAngle.Control.AngleValue = 1;
	DNPData.Phase1DifferentialVoltageAngle.Control.AngleValue = 1;
	DNPData.Phase2DifferentialVoltageAngle.Control.AngleValue = 1;
	DNPData.Phase3DifferentialVoltageAngle.Control.AngleValue = 1;
	DNPData.AverageDifferentialVoltageAngle.Control.AngleValue = 1;
	DNPData.Phase1CurrentAngle.Control.AngleValue = 1;
	DNPData.Phase2CurrentAngle.Control.AngleValue = 1;
	DNPData.Phase3CurrentAngle.Control.AngleValue = 1;
	DNPData.EffectiveCurrentAngle.Control.AngleValue = 1;
	DNPData.CurrentPositiveSequenceAngle.Control.AngleValue = 1;
	DNPData.CurrentNegativeSequenceAngle.Control.AngleValue = 1;
	DNPData.Phase1PowerAngle.Control.AngleValue = 1;
	DNPData.Phase2PowerAngle.Control.AngleValue = 1;
	DNPData.Phase3PowerAngle.Control.AngleValue = 1;
	DNPData.AveragePowerAngle.Control.AngleValue = 1;
	DNPData.DifferentialVoltagePositiveSequenceAngle.Control.AngleValue = 1;
	DNPData.DifferentialVoltageNegativeSequenceAngle.Control.AngleValue = 1;
	DNPData.NetworkVoltagePositiveSequenceAngle.Control.AngleValue = 1;
	DNPData.NetworkVoltageNegativeSequenceAngle.Control.AngleValue = 1;
	
#endif

}

void initializeDNPVariables(void)
{
#if DNPCustomer == DNPCustomerMemphis
	
	DNPData.WaveCaptureSampleRate.Value = 120;
	DNPData.NumberOfCyclesEvent.Value = 8;
	
	setScalingFactors();
	
	EventTriggerRange.Voltage = MyDNPSettings.DeadBandVoltage * 10;
	EventTriggerRange.THD = MyDNPSettings.DeadBandTHD;
	EventTriggerRange.Current = MyDNPSettings.DeadBandCurrent;
	EventTriggerRange.Temperature = MyDNPSettings.DeadBandTemperature;
	EventTriggerRange.Odometer = MyDNPSettings.DeadBandOdometer;
	EventTriggerRange.DifferentialVoltage = MyDNPSettings.DeadBandRMSDifferentialVoltage;
	EventTriggerRange.RealDiffVoltage = MyDNPSettings.DeadBandRealDifferentialVoltage;
	EventTriggerRange.CurrentAngle = MyDNPSettings.DeadBandCurrentAngle;
	EventTriggerRange.PhaseKW = MyDNPSettings.DeadBandPhasekW;
	EventTriggerRange.PhaseKVAR = MyDNPSettings.DeadBandPhasekVAR;
	EventTriggerRange.PhaseKVA = MyDNPSettings.DeadBandPhasekVA;
	EventTriggerRange.TotalKW = MyDNPSettings.DeadBandTotalkW;
	EventTriggerRange.TotalKVAR = MyDNPSettings.DeadBandTotalkVAkVAR;
	EventTriggerRange.Analog1 = MyDNPSettings.DeadBandAnalog1;
	EventTriggerRange.Analog2 = MyDNPSettings.DeadBandAnalog2;
	EventTriggerRange.Analog3 = MyDNPSettings.DeadBandAnalog3;
	EventTriggerRange.Analog4 = MyDNPSettings.DeadBandAnalog4;
	
#else
	EventTriggerRange.Voltage = MyDNPSettings.DBVoltage * 10;		
	EventTriggerRange.VoltageAngle = MyDNPSettings.DBVoltageAngle * 10;
	EventTriggerRange.AparrentDiffVolt = MyDNPSettings.DBApparentDiffVoltage;
	EventTriggerRange.AparrentDiffVoltAngle = MyDNPSettings.DBDiffVoltageAngle * 10;
	EventTriggerRange.AvgAparrentDiffVolt = MyDNPSettings.	DBAvgDiffAngle * 10;
	EventTriggerRange.AvgAparrentDiffVoltAngle = MyDNPSettings.DBAvgDiffVolt;
	EventTriggerRange.RealDiffVolt = MyDNPSettings.DBRealDiffVolt;
	EventTriggerRange.AvgRealDiffVolt = MyDNPSettings.DBAvgRealDiffVolt;
	EventTriggerRange.Current = MyDNPSettings.DBCurrent * 10;
	EventTriggerRange.CurrentAngle = MyDNPSettings.DBCurrentAngle * 10;
	EventTriggerRange.EffCurrent = MyDNPSettings.DBEffCurrent * 10;
	EventTriggerRange.EffCurrentAngle = MyDNPSettings.DBEffCurrentAngle * 10;
	EventTriggerRange.CurrentPositiveSequence = MyDNPSettings.DBPositiveSequenceCurrent * 10;
	EventTriggerRange.CurrentPositiveSequenceAngle = MyDNPSettings.DBPositiveSequenceCurrentAngle * 10;
	EventTriggerRange.CurrentNegativeSequence = MyDNPSettings.DBNegativeSequenceCurrent * 10;
	EventTriggerRange.CurrentNegativeSequenceAngle = MyDNPSettings.DBNegativeSequenceCurrentAngle * 10;
	EventTriggerRange.PhaseKVA = MyDNPSettings.DBPhasekVA ;
	EventTriggerRange.PhaseKVAAngle = MyDNPSettings.DBPhasekVAAngle * 10;
	EventTriggerRange.AvgPhaseKVA = MyDNPSettings.DBAveragekVA;
	EventTriggerRange.AvgPhaseKVAAngle = MyDNPSettings.DBAveragekVAAngle * 10;
	EventTriggerRange.PhaseKW = MyDNPSettings.DBPhasekW;
	EventTriggerRange.AvgPhaseKW = MyDNPSettings.DBAvgPhasekW;
	EventTriggerRange.DiffVoltPositiveSequence = MyDNPSettings.DBDiffPositiveSequence;
	EventTriggerRange.DiffVoltPositiveSequenceAngle = MyDNPSettings.DBDiffPositiveSequenceAngle * 10;
	EventTriggerRange.DiffVoltNegativeSequence = MyDNPSettings.DBDiffNegativeSequence;
	EventTriggerRange.DiffVoltNegativeSequenceAngle = MyDNPSettings.DBDiffNegativeSequenceAngle * 10;
	EventTriggerRange.VoltPositiveSequence = MyDNPSettings.DBVoltagePositiveSequence * 10;
	EventTriggerRange.VoltPositiveSequenceAngle = MyDNPSettings.DBVoltagePositiveSequenceAngle * 10;
	EventTriggerRange.VoltNegativeSequence = MyDNPSettings.DBVoltageNegativeSequence * 10;
	EventTriggerRange.VoltNegativeSequenceAngle = MyDNPSettings.DBVoltageNegativeSequenceAngle * 10;
	EventTriggerRange.VoltageTHD = MyDNPSettings.DBTHDVoltage * 10;
	EventTriggerRange.CurrentTHD = MyDNPSettings.DBTHDCurrent * 10;
	EventTriggerRange.Temperature = MyDNPSettings.DBTemperature;
	EventTriggerRange.Odometer = MyDNPSettings.DBOdometer;
	EventTriggerRange.Analog1 = MyDNPSettings.DBAnalog1 * 10;
	EventTriggerRange.Analog2 = MyDNPSettings.DBAnalog2 * 10;
	EventTriggerRange.Analog3 = MyDNPSettings.DBAnalog3 * 10;
	EventTriggerRange.Analog4 = MyDNPSettings.DBAnalog4 * 10;

#endif
}

void DNPSetMemphisStage(void)
{	
	switch(MyDNPSettings.ControlWord.MemphisStage)
	{
		case 0:
		case 1:
			DNPAnalogInQuantity = 30;
			DNPAnalogOutQuantity = 0;
			DNPBinaryInQuantity = 16;
			DNPBinaryOutQuantity = 4;
			break;
		case 2:
			DNPAnalogInQuantity = 45;
			DNPAnalogOutQuantity = 0;
			DNPBinaryInQuantity = 16;
			DNPBinaryOutQuantity = 4;
			break;
		case 3:
			DNPAnalogInQuantity = 81;
			DNPAnalogOutQuantity = 0;
			DNPBinaryInQuantity = 36;
			DNPBinaryOutQuantity = 4;
			break;
		case 4:
			DNPAnalogInQuantity = 20008;
			DNPAnalogOutQuantity = 3;
			DNPBinaryInQuantity = 36;
			DNPBinaryOutQuantity = 6;
			break;
		default:
		case 5:
			DNPAnalogInQuantity = 20008;
			DNPAnalogOutQuantity = 3;
			DNPBinaryInQuantity = 36;
			DNPBinaryOutQuantity = 10;
			break;
	}
}

void DNPInitializeBinaryEventsFromMemory()
{
	unsigned int i = 0;
	unsigned int j = 0;
	
#if DNPCustomer == DNPCustomerMemphis
	DNPSet8Enables(&DNPData.BIDummy, MyDNPSettings.BinaryEventEnables[0], 8);
	DNPSet8Enables(&DNPData.DifferentialVoltsTooLowToClose, MyDNPSettings.BinaryEventEnables[1], 8);
	DNPSet8Enables(&DNPData.BIDummy2, MyDNPSettings.BinaryEventEnables[2], 8);
	DNPSet8Enables(&DNPData.TimeDelayModeEnabled, MyDNPSettings.BinaryEventEnables[3], 8);
	DNPSet8Enables(&DNPData.DigitalOut1, MyDNPSettings.BinaryEventEnables[4], 4);
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPSet8Enables(&DNPData.BinaryInputs0 , MyDNPSettings.BinaryEventEnables[0], 8);
	DNPSet8Enables(&DNPData.BinaryInputs0 + 8, MyDNPSettings.BinaryEventEnables[1], 7);
#else
	
#endif
}

void DNPSet8Enables(BinaryPoint * pointArray, unsigned char enableChar, unsigned int numberOfPoints)
{
	int i;
	for(i = 0; i < numberOfPoints; i++)
	{
		pointArray->Control.EventEnabled = enableChar & 0x01;
		enableChar >>= 1;
		pointArray++;
	}
}

void DNPSet8AnalogEnables(AnalogPointIn * pointArray, unsigned char enableChar, unsigned char numberOfPoints)
{
	int i;
	for(i = 0; i < numberOfPoints; i++)
	{
		pointArray->Control.EventEnabled = enableChar & 0x01;
		enableChar >>= 1;
		pointArray++;	
	}
}

void DNPInitializeAnanlogEventsFromMemory(void)
{
#if DNPCustomer == DNPCustomerDigitalGrid
	DNPSet8AnalogEnables(&DNPData.AnalogInputs, MyDNPSettings.AnalogEventEnables[0], 8);
	DNPSet8AnalogEnables(&DNPData.AnalogInputs + 8, MyDNPSettings.AnalogEventEnables[1], 8);
	DNPSet8AnalogEnables(&DNPData.AnalogInputs + 16, MyDNPSettings.AnalogEventEnables[2], 8);
	DNPSet8AnalogEnables(&DNPData.AnalogInputs + 24, MyDNPSettings.AnalogEventEnables[3], 8);
	DNPSet8AnalogEnables(&DNPData.AnalogInputs + 32, MyDNPSettings.AnalogEventEnables[4], 8);
	DNPSet8AnalogEnables(&DNPData.AnalogInputs + 40, MyDNPSettings.AnalogEventEnables[5], 8);
	DNPSet8AnalogEnables(&DNPData.AnalogInputs + 48, MyDNPSettings.AnalogEventEnables[6], 8);
	DNPSet8AnalogEnables(&DNPData.AnalogInputs + 56, MyDNPSettings.AnalogEventEnables[7], 8);
	DNPSet8AnalogEnables(&DNPData.AnalogInputs + 64, MyDNPSettings.AnalogEventEnables[8], 5);
#endif
}


TMWDTIME time;

void DNPMain(void)
{
	static unsigned long savedDNPTimer = 0;
	long tempLong;
	
	if(DNPInit.InitComplete == 0)
	{
		InitializeDNP();
		return;
	}	
	
	if((DNPTimer >= savedDNPTimer + 50) || ((DNPTimer < savedDNPTimer) && (0xFFFFFFFF - savedDNPTimer + DNPTimer >= 50)))
	{
		
		DNPPopulateEntropyArray();
		
		if(0xFFFFFFFF - savedDNPTimer + DNPTimer >= 50)
			DNPControl.DNPTimerRollOver = 1;
		
		savedDNPTimer = DNPTimer;		
	
		DNPConvertRelayTimeToDNP(event_clock.long_time, DNPTimer%1000, &time);
		
		DNPCheckExternalDigitalFlags(&time);
		DNPCheckExternalAnalogs(&time);
		tmwpltmr_checkTimer();
		DNPSetNumberOfEvents();
		tmwappl_checkForInput(myApplContext);
		
#if DNPCustomer == DNPCustomerMemphis
		DNPData.RelayTemperature.Value = current.chip_temp * 10;
		checkVariableAnalogEvent(&DNPData.RelayTemperature, DNPDEFS_DBAS_FLAG_ON_LINE, &time, EventTriggerRange.Temperature);
#else
		DNPData.RelayTemperature.Value = current.chip_temp;
#endif
		DNPData.RelayCycleCounter.Value = cycle_counter;
		checkVariableAnalogEvent(&DNPData.RelayCycleCounter, DNPDEFS_DBAS_FLAG_ON_LINE, &time, EventTriggerRange.Odometer);
		
		
		if(changed_relay_parameters)
  		{
  			ResetMyParameters();
  		}

	}
}

void DNPPopulateEntropyArray()
{
	static unsigned int entropyI = 0;
	
	// uses the 4 LSBs of VnB and VnC to get "entropy"
	
	//DNPEntropy[entropyI] = (char)(ADCA_result[1] >> 3);
	//DNPEntropy[entropyI] <<= 4;
	//DNPEntropy[entropyI] += (char)(ADCA_result[2] >> 3);
	
	entropyI++;
	if(entropyI == DNP_ENTROPY_SIZE)
		entropyI = 0;
}

void DNPSendMessageUInt(unsigned int value)
{
	unsigned int divisor, temp, test;
	signed int i;
	char buf[6];
	
	temp = value;
	for(i = 4, divisor = 1; i >= 0; temp /= 10, i--)
	{
		test = temp%10;
		switch(temp%10)
		{
			case 0:
				buf[i] = '0';
				break;
			case 1:
				buf[i] = '1';
				break;
			case 2:
				buf[i] = '2';
				break;
			case 3:
				buf[i] = '3';
				break;
			case 4:
				buf[i] = '4';
				break;
			case 5:
				buf[i] = '5';
				break;
			case 6:
				buf[i] = '6';
				break;
			case 7:
				buf[i] = '7';
				break;
			case 8:
				buf[i] = '8';
				break;
			case 9:
				buf[i] = '9';
				break;
		}
	}
	buf[5] = 0;
	
	DNPSendMessage(buf);
}

void DNPSendMessage(char * message)
{

	while(txQ1_in != txQ1_out)
	{
	}
	
	COP_CTR = 0x5555;
  	COP_CTR = 0xAAAA;
	send_opto_msg(message);
	
}

//Should be called when I get new mS time from DNP or at initialization
/*
Date GetDate(void)
{
	Date date;
	
	return date;
}
*/

unsigned short * GetIIN(void)
{
	return &IIN.IIN1;	
}

void SetInProgress(unsigned int inProgress)
{

	DNPControl.InProgress = inProgress;
}

unsigned int isLeapYear(unsigned int year)
{

	if(year%4 == 0)
	{
		if(year%100 == 0)
		{
			if(year%400 == 0)
			{
				return TRUE;
			}
			else
				return FALSE;
		}
		else
		{
			return TRUE;
		}
	}
	else
	{
		return FALSE;
	}
}

void relay_data(unsigned int *dataPtr)
{

	switch(*dataPtr)
	{
		case 0x7205: //SendRegisters
		case 0x4603: //Event/Nonevent Data updated if needed
			storeRegisters(++dataPtr);
			break;
		case 0x5110: //sendRevisionNumber
			handleRevisionNumber(++dataPtr);
			break;
		case 0x0D40: //CommVectorData
			handleCommVectorData(++dataPtr);
			break;
		case 0x4902:
			//handleTripCycles(++dataPtr);
			break;
		default:
			break;
	}
	return;
}

void storeRegisters(unsigned int *dataPtr)
{
	
	RELAYFLAGS rFlags;
	RELAYSTATUS status;
	COMMFLAGS cFlags;
	TMWDTIME time;
	TMWTYPES_UCHAR flags;
	
	if(DNPInit.InitComplete)
		flags = DNPDEFS_DBAS_FLAG_ON_LINE;
	else
		flags = DNPDEFS_DBAS_FLAG_RESTART | DNPDEFS_DBAS_FLAG_ON_LINE;
	
	DNPConvertRelayTimeToDNP(event_clock.long_time, DNPTimer%1000, &time);
	DNPSetNumberOfEvents();
	
	//Comm flags
	cFlags.Full = *dataPtr;
	
	checkBinaryEvent(&DNPData.CallingForClose, DNPData.CallingForClose.Value, cFlags.Close, flags, &time);
	DNPData.CallingForClose.Value = cFlags.Close;
	
	
#if DNPCustomer == DNPCustomerMemphis
	checkBinaryEvent(&DNPData.RelaxedCloseEnabled, DNPData.RelaxedCloseEnabled.Value, cFlags.RelaxedClose, flags, &time);
	DNPData.sRelaxClose.Value = DNPData.RemoteClose.Value = DNPData.RelaxedCloseEnabled.Value = cFlags.RelaxedClose;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sRelaxClose.Value = cFlags.RelaxedClose;
#else
	DNPData.RelaxedCloseEnabled = cFlags.RelaxedClose;
#endif
	
	checkBinaryEvent(&DNPData.CallingForTrip, DNPData.CallingForTrip.Value, cFlags.Trip, flags, &time);
	DNPData.CallingForTrip.Value = cFlags.Trip;
	
	if(!cFlags.Trip && !cFlags.Close)
	{
		checkBinaryEvent(&DNPData.Floating, DNPData.Floating.Value, 1, flags, &time);
		DNPData.Floating.Value = 1;
	}
	else
	{
		checkBinaryEvent(&DNPData.Floating, DNPData.Floating.Value, 0, flags, &time);
		DNPData.Floating.Value = 0;
	}
	
	checkBinaryEvent(&DNPData.BlockedOpen, DNPData.BlockedOpen.Value, cFlags.BlockedOpen, flags, &time);
	DNPData.BlockedOpen.Value = cFlags.BlockedOpen;
#if DNPCustomer == DNPCustomerMemphis
	DNPData.DBlockedOpen.Value = DNPData.BlockedOpen.Value;
#endif
	DNPData.sBlockOpen.Value = DNPData.BlockedOpen.Value;
	
	dataPtr++;
	
	//relay flags.
	rFlags.Full = *dataPtr;
	
#if DNPCustomer == DNPCustomerDigitalGrid
	checkBinaryEvent(&DNPData.DefaultsLoaded, DNPData.DefaultsLoaded.Value, rFlags.DefaultValues, flags, &time);
	DNPData.DefaultsLoaded.Value = rFlags.DefaultValues;
#endif
	
	checkBinaryEvent(&DNPData.PumpProtectLockout, DNPData.PumpProtectLockout.Value, rFlags.Pumping, flags, &time);
	DNPData.PumpProtectLockout.Value = rFlags.Pumping;
	
	checkBinaryEvent(&DNPData.DefaultsLoaded, DNPData.DefaultsLoaded.Value, rFlags.DefaultValues, flags, &time);
	DNPData.DefaultsLoaded.Value = rFlags.DefaultValues;
	
#if DNPCustomer == DNPCustomerMemphis
	DNPData.DPumpProtect.Value = DNPData.PumpProtectLockout.Value;
	
	checkBinaryEvent(&DNPData.SequenceRelay, DNPData.SequenceRelay.Value, rFlags.SequenceRelay, flags, &time);
	DNPData.SequenceRelay.Value = rFlags.SequenceRelay;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sSequenceRelay.Value = rFlags.SequenceRelay;
#endif
	

#if DNPCustomer == DNPCustomerDigitalGrid
	checkBinaryEvent(&DNPData.PhasedACB, DNPData.PhasedACB.Value, rFlags.ACB, flags, &time);
	DNPData.PhasedACB.Value = rFlags.ACB;
#endif

	DNPData.BFlag.Value = DNPData.DigitalIn1.Value;
	
#if DNPCustomer == DNPCustomerDigitalGrid
	checkBinaryEvent(&DNPData.InsensitiveBackFeedDetected, DNPData.InsensitiveBackFeedDetected.Value, rFlags.InInsensitiveRegion, flags, &time);
	DNPData.InsensitiveBackFeedDetected.Value = rFlags.InInsensitiveRegion;
#endif
	
	dataPtr++;
	
	//relay status
	
#if DNPCustomer == DNPCustomerDigitalGrid
	status.Full = *dataPtr;
	
	checkBinaryEvent(&DNPData.RelayPhasedOK, DNPData.RelayPhasedOK.Value, status.PhasingOK, flags, &time);
	DNPData.RelayPhasedOK.Value = status.PhasingOK;
#endif;
	dataPtr++;
	
	DNPInit.FirstRegisterRead = 1;
}

void handleCommVectorData(unsigned int *dataPtr)
{
	
	TMWDTIME time;
	TMWTYPES_UCHAR flags;
	
	if(DNPInit.InitComplete)
		flags = DNPDEFS_DBAS_FLAG_ON_LINE;
	else
		flags = DNPDEFS_DBAS_FLAG_RESTART | DNPDEFS_DBAS_FLAG_ON_LINE;
	
	DNPConvertRelayTimeToDNP(event_clock.long_time, DNPTimer%1000, &time);
#if DNPCustomer == DNPCustomerMemphis
	DNPData.Phase1NetworkVoltage.Value = dNPLongFrom8_8Memphis(dataPtr[0]);
	checkVariableAnalogEvent(&DNPData.Phase1NetworkVoltage, flags, &time, EventTriggerRange.Voltage);
	DNPData.Phase2NetworkVoltage.Value = dNPLongFrom8_8Memphis(dataPtr[1]);
	checkVariableAnalogEvent(&DNPData.Phase2NetworkVoltage, flags, &time, EventTriggerRange.Voltage);
	DNPData.Phase3NetworkVoltage.Value = dNPLongFrom8_8Memphis(dataPtr[2]);
	checkVariableAnalogEvent(&DNPData.Phase3NetworkVoltage, flags, &time, EventTriggerRange.Voltage);

	
	DNPData.Phase1TransformerVoltage.Value = dNPLongFrom8_8Memphis(dataPtr[3]);
	checkVariableAnalogEvent(&DNPData.Phase1TransformerVoltage, flags, &time, EventTriggerRange.Voltage);
	DNPData.Phase2TransformerVoltage.Value = dNPLongFrom8_8Memphis(dataPtr[4]);
	checkVariableAnalogEvent(&DNPData.Phase2TransformerVoltage, flags, &time, EventTriggerRange.Voltage);
	DNPData.Phase3TransformerVoltage.Value = dNPLongFrom8_8Memphis(dataPtr[5]);
	checkVariableAnalogEvent(&DNPData.Phase3TransformerVoltage, flags, &time, EventTriggerRange.Voltage);
	
	
	DNPData.Phase1Current.Value = dNPCurrentFrom4_12(dataPtr[6]);
	checkVariableAnalogEvent(&DNPData.Phase1Current, flags, &time, EventTriggerRange.Current);
	DNPData.Phase2Current.Value = dNPCurrentFrom4_12(dataPtr[7]);
	checkVariableAnalogEvent(&DNPData.Phase2Current, flags, &time, EventTriggerRange.Current);
	DNPData.Phase3Current.Value = dNPCurrentFrom4_12(dataPtr[8]);
	checkVariableAnalogEvent(&DNPData.Phase3Current, flags, &time, EventTriggerRange.Current);

	DNPData.Phase1Power.Value = dNPLongFrom5_11kW(dataPtr[9], dataPtr[0]);	//Real
	checkVariableAnalogEvent(&DNPData.Phase1Power, flags, &time, EventTriggerRange.PhaseKW);
	DNPData.Phase2Power.Value = dNPLongFrom5_11kW(dataPtr[12], dataPtr[1]);
	checkVariableAnalogEvent(&DNPData.Phase2Power, flags, &time, EventTriggerRange.PhaseKW);
	DNPData.Phase3Power.Value = dNPLongFrom5_11kW(dataPtr[15], dataPtr[2]);
	checkVariableAnalogEvent(&DNPData.Phase3Power, flags, &time, EventTriggerRange.PhaseKW);
	
	DNPData.Phase1VAR.Value = dNPLongFrom5_11kW(dataPtr[10], dataPtr[0]);	//Imaginary
	checkVariableAnalogEvent(&DNPData.Phase1VAR, flags, &time, EventTriggerRange.PhaseKVAR);
	DNPData.Phase2VAR.Value = dNPLongFrom5_11kW(dataPtr[13], dataPtr[1]);
	checkVariableAnalogEvent(&DNPData.Phase2VAR, flags, &time, EventTriggerRange.PhaseKVAR);
	DNPData.Phase3VAR.Value = dNPLongFrom5_11kW(dataPtr[16], dataPtr[2]);
	checkVariableAnalogEvent(&DNPData.Phase3VAR, flags, &time, EventTriggerRange.PhaseKVAR);
	
	DNPData.Phase1VA.Value = dNPLongFrom5_11kW(dataPtr[6]>>1, dataPtr[0]); //Apparent
	checkVariableAnalogEvent(&DNPData.Phase1VA, flags, &time, EventTriggerRange.PhaseKVA);
	DNPData.Phase2VA.Value = dNPLongFrom5_11kW(dataPtr[7]>>1, dataPtr[1]);
	checkVariableAnalogEvent(&DNPData.Phase2VA, flags, &time, EventTriggerRange.PhaseKVA);
	DNPData.Phase3VA.Value = dNPLongFrom5_11kW(dataPtr[8]>>1, dataPtr[2]);
	checkVariableAnalogEvent(&DNPData.Phase3VA, flags, &time, EventTriggerRange.PhaseKVA);
	
	DNPData.TotalPower.Value = totalKW();
	checkVariableAnalogEvent(&DNPData.TotalPower, flags, &time, EventTriggerRange.TotalKW);
	DNPData.TotalVAR.Value = totalVAR();
	checkVariableAnalogEvent(&DNPData.TotalVAR, flags, &time, EventTriggerRange.TotalKVAR);
	DNPData.TotalVA.Value = totalVA();
	checkVariableAnalogEvent(&DNPData.TotalVA, flags, &time, EventTriggerRange.TotalKVAR);
	
	DNPData.Phase1PowerAngle.Value = dNPLongFrom5_11IQ(dataPtr[9], dataPtr[10]);
	if(DNPData.Phase1Current.Value > 150)		//Make sure current is large enough before making events on angle
		checkVariableAnalogEvent(&DNPData.Phase1PowerAngle, flags, &time, EventTriggerRange.CurrentAngle);
	else
		DNPData.Phase1PowerAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase2PowerAngle.Value = dNPLongFrom5_11IQ(dataPtr[12], dataPtr[13]);
	if(DNPData.Phase2Current.Value > 150)		//Make sure current is large enough before making events on angle
		checkVariableAnalogEvent(&DNPData.Phase2PowerAngle, flags, &time, EventTriggerRange.CurrentAngle);
	else
		DNPData.Phase2PowerAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase3PowerAngle.Value = dNPLongFrom5_11IQ(dataPtr[15], dataPtr[16]);
	if(DNPData.Phase3Current.Value > 150)		//Make sure current is large enough before making events on angle
		checkVariableAnalogEvent(&DNPData.Phase3PowerAngle, flags, &time, EventTriggerRange.CurrentAngle);
	else
		DNPData.Phase3PowerAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase1DifferentialVoltageReal.Value = dNPLongFrom8_8diffReal(dataPtr[28]);		//real
	checkVariableAnalogEvent(&DNPData.Phase1DifferentialVoltageReal, flags, &time, EventTriggerRange.RealDiffVoltage);
	DNPData.Phase2DifferentialVoltageReal.Value = dNPLongFrom8_8diffReal(dataPtr[29]);
	checkVariableAnalogEvent(&DNPData.Phase2DifferentialVoltageReal, flags, &time, EventTriggerRange.RealDiffVoltage);
	DNPData.Phase3DifferentialVoltageReal.Value = dNPLongFrom8_8diffReal(dataPtr[30]);
	checkVariableAnalogEvent(&DNPData.Phase3DifferentialVoltageReal, flags, &time, EventTriggerRange.RealDiffVoltage);
	
	DNPData.Phase1DifferentialVoltageRMS.Value = dNPLongFrom8_8diffRMS(dataPtr[31], dataPtr[32]);		//RMS
	checkVariableAnalogEvent(&DNPData.Phase1DifferentialVoltageRMS, flags, &time, EventTriggerRange.DifferentialVoltage);
	
	DNPData.Phase2DifferentialVoltageRMS.Value = dNPLongFrom8_8diffRMS(dataPtr[33], dataPtr[34]);
	checkVariableAnalogEvent(&DNPData.Phase2DifferentialVoltageRMS, flags, &time, EventTriggerRange.DifferentialVoltage);
	
	DNPData.Phase3DifferentialVoltageRMS.Value = dNPLongFrom8_8diffRMS(dataPtr[35], dataPtr[36]);
	checkVariableAnalogEvent(&DNPData.Phase3DifferentialVoltageRMS, flags, &time, EventTriggerRange.DifferentialVoltage);
	
	DNPData.NetworkVoltsTooLowToClose.Value = 0;
	
	if(DNPData.Floating.Value == 1 && !DNPData.PumpProtectLockout.Value && (dNPLongFrom9_7(dataPtr[52]) < DNPData.RecloseVolts.Value) && !DNPData.BlockedOpen.Value) //52 is where VdP is
		DNPData.DifferentialVoltsTooLowToClose.Value = 1;
	else
		DNPData.DifferentialVoltsTooLowToClose.Value = 0;
	
	if(DNPData.Floating.Value == 1 && !DNPData.PumpProtectLockout.Value && DNPData.DifferentialVoltsTooLowToClose.Value == 0 && !DNPData.BlockedOpen.Value)
		DNPData.PhaseAngleIncorrectToClose.Value = 1;
	else
		DNPData.PhaseAngleIncorrectToClose.Value = 0;
	
	DNPData.Phase1NetworkTHD.Value = dNPLongFrom5_11(dataPtr[58]);
	if(DNPData.Phase1NetworkVoltage.Value > 1000)
		checkVariableAnalogEvent(&DNPData.Phase1NetworkTHD, flags, &time, EventTriggerRange.THD);
	DNPData.Phase2NetworkTHD.Value = dNPLongFrom5_11(dataPtr[59]);
	if(DNPData.Phase1NetworkVoltage.Value > 1000)
		checkVariableAnalogEvent(&DNPData.Phase2NetworkTHD, flags, &time, EventTriggerRange.THD);
	DNPData.Phase3NetworkTHD.Value = dNPLongFrom5_11(dataPtr[60]);
	if(DNPData.Phase1NetworkVoltage.Value > 1000)
		checkVariableAnalogEvent(&DNPData.Phase3NetworkTHD, flags, &time, EventTriggerRange.THD);
	DNPData.Phase1CurrentTHD.Value = dNPLongFrom5_11(dataPtr[61]);
	if(DNPCurrentHighEnoughForTHD(DNPData.Phase1Current.Value))
		checkVariableAnalogEvent(&DNPData.Phase1CurrentTHD, flags, &time, EventTriggerRange.THD);
	DNPData.Phase2CurrentTHD.Value  = dNPLongFrom5_11(dataPtr[62]);
	if(DNPCurrentHighEnoughForTHD(DNPData.Phase2Current.Value))
		checkVariableAnalogEvent(&DNPData.Phase2CurrentTHD, flags, &time, EventTriggerRange.THD);
	DNPData.Phase3CurrentTHD.Value = dNPLongFrom5_11(dataPtr[63]);
	if(DNPCurrentHighEnoughForTHD(DNPData.Phase3Current.Value))
		checkVariableAnalogEvent(&DNPData.Phase3CurrentTHD, flags, &time, EventTriggerRange.THD);
#else
	DNPData.Phase1NetworkVoltage.Value = dNPLongFrom8_8(dataPtr[0]);
	checkVariableAnalogEvent(&DNPData.Phase1NetworkVoltage, flags, &time, EventTriggerRange.Voltage);
	DNPData.Phase2NetworkVoltage.Value = dNPLongFrom8_8(dataPtr[1]);
	checkVariableAnalogEvent(&DNPData.Phase2NetworkVoltage, flags, &time, EventTriggerRange.Voltage);
	DNPData.Phase3NetworkVoltage.Value = dNPLongFrom8_8(dataPtr[2]);
	checkVariableAnalogEvent(&DNPData.Phase3NetworkVoltage, flags, &time, EventTriggerRange.Voltage);
	
	DNPData.Phase1TransformerVoltageAngle.Value = 0;
	
	DNPData.Phase2NetworkVoltageAngle.Value = dNPLongFrom5_11IQ(dataPtr[18], dataPtr[19]);
	if(DNPData.Phase2NetworkVoltage.Value > 1000)		//Make sure voltage is large enough before making events on angle, 1000 = 100V
		checkVariableAnalogEvent(&DNPData.Phase2NetworkVoltageAngle, flags, &time, EventTriggerRange.VoltageAngle);
	else
		DNPData.Phase2NetworkVoltageAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase3NetworkVoltageAngle.Value = dNPLongFrom5_11IQ(dataPtr[20], dataPtr[21]);
	if(DNPData.Phase3NetworkVoltage.Value > 1000)		//Make sure voltage is large enough before making events on angle, 1000 = 100V
		checkVariableAnalogEvent(&DNPData.Phase3NetworkVoltageAngle, flags, &time, EventTriggerRange.VoltageAngle);
	else
		DNPData.Phase3NetworkVoltageAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase1TransformerVoltage.Value = dNPLongFrom8_8(dataPtr[3]);
	checkVariableAnalogEvent(&DNPData.Phase1TransformerVoltage, flags, &time, EventTriggerRange.Voltage);
	DNPData.Phase2TransformerVoltage.Value = dNPLongFrom8_8(dataPtr[4]);
	checkVariableAnalogEvent(&DNPData.Phase2TransformerVoltage, flags, &time, EventTriggerRange.Voltage);
	DNPData.Phase3TransformerVoltage.Value = dNPLongFrom8_8(dataPtr[5]);
	checkVariableAnalogEvent(&DNPData.Phase3TransformerVoltage, flags, &time, EventTriggerRange.Voltage);
	
	DNPData.Phase1TransformerVoltageAngle.Value = dNPLongFrom5_11IQ(dataPtr[22], dataPtr[23]);
	if(DNPData.Phase1TransformerVoltage.Value > 1000)		//Make sure voltage is large enough before making events on angle, 1000 = 100V
		checkVariableAnalogEvent(&DNPData.Phase1TransformerVoltageAngle, flags, &time, EventTriggerRange.VoltageAngle);
	else
		DNPData.Phase1TransformerVoltageAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase2TransformerVoltageAngle.Value = dNPLongFrom5_11IQ(dataPtr[24], dataPtr[25]);
	if(DNPData.Phase2TransformerVoltage.Value > 1000)		//Make sure voltage is large enough before making events on angle, 1000 = 100V
		checkVariableAnalogEvent(&DNPData.Phase2TransformerVoltageAngle, flags, &time, EventTriggerRange.VoltageAngle);
	else
		DNPData.Phase2TransformerVoltageAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase3TransformerVoltageAngle.Value = dNPLongFrom5_11IQ(dataPtr[26], dataPtr[27]);
	if(DNPData.Phase3TransformerVoltage.Value > 1000)		//Make sure voltage is large enough before making events on angle, 1000 = 100V
		checkVariableAnalogEvent(&DNPData.Phase3TransformerVoltageAngle, flags, &time, EventTriggerRange.VoltageAngle);
	else
		DNPData.Phase3TransformerVoltageAngle.Control.NotFirstDataRead = 0;
	
	//Differential Vltages
	DNPData.Phase1DifferentialVoltageRMS.Value = dNPLongFrom8_8diffRMS(dataPtr[31], dataPtr[32]);		//RMS
	checkVariableAnalogEvent(&DNPData.Phase1DifferentialVoltageRMS, flags, &time, EventTriggerRange.AparrentDiffVolt);
	
	DNPData.Phase2DifferentialVoltageRMS.Value = dNPLongFrom8_8diffRMS(dataPtr[33], dataPtr[34]);
	checkVariableAnalogEvent(&DNPData.Phase2DifferentialVoltageRMS, flags, &time, EventTriggerRange.AparrentDiffVolt);
	
	DNPData.Phase3DifferentialVoltageRMS.Value = dNPLongFrom8_8diffRMS(dataPtr[35], dataPtr[36]);
	checkVariableAnalogEvent(&DNPData.Phase3DifferentialVoltageRMS, flags, &time, EventTriggerRange.AparrentDiffVolt);
	
	DNPData.AverageDifferentialVoltage.Value = (DNPData.Phase1DifferentialVoltageRMS.Value + DNPData.Phase2DifferentialVoltageRMS.Value + DNPData.Phase3DifferentialVoltageRMS.Value) /3;//dNPLongFrom8_8diffRMS((dataPtr[31] + dataPtr[33] + dataPtr[35]), (dataPtr[32] + dataPtr[34] + dataPtr[36])) / 3;
	checkVariableAnalogEvent(&DNPData.AverageDifferentialVoltage, flags, &time, EventTriggerRange.AvgAparrentDiffVolt);
	
	DNPData.Phase1DifferentialVoltageAngle.Value = dNPLongFrom5_11IQ(dataPtr[31], dataPtr[32]);
	if(DNPData.Phase1DifferentialVoltageRMS.Value > 10)		//Make sure voltage is large enough before making events on angle, 1000 = 100V
		checkVariableAnalogEvent(&DNPData.Phase1DifferentialVoltageAngle, flags, &time, EventTriggerRange.AparrentDiffVoltAngle);
	else
		DNPData.Phase1DifferentialVoltageAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase2DifferentialVoltageAngle.Value = dNPLongFrom5_11IQ(dataPtr[33], dataPtr[34]);
	if(DNPData.Phase2DifferentialVoltageRMS.Value > 10)		//Make sure voltage is large enough before making events on angle, 1000 = 100V
		checkVariableAnalogEvent(&DNPData.Phase2DifferentialVoltageAngle, flags, &time, EventTriggerRange.AparrentDiffVoltAngle);
	else
		DNPData.Phase2DifferentialVoltageAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase3DifferentialVoltageAngle.Value = dNPLongFrom5_11IQ(dataPtr[35], dataPtr[36]);
	if(DNPData.Phase3DifferentialVoltageRMS.Value > 10)		//Make sure voltage is large enough before making events on angle, 1000 = 100V
		checkVariableAnalogEvent(&DNPData.Phase3DifferentialVoltageAngle, flags, &time, EventTriggerRange.AparrentDiffVoltAngle);
	else
		DNPData.Phase3DifferentialVoltageAngle.Control.NotFirstDataRead = 0;
	
	DNPData.AverageDifferentialVoltageAngle.Value = (DNPData.Phase1DifferentialVoltageAngle.Value + 
													DNPData.Phase2NetworkVoltageAngle.Value + DNPData.Phase2DifferentialVoltageAngle.Value +
													DNPData.Phase3NetworkVoltageAngle.Value + DNPData.Phase3DifferentialVoltageAngle.Value) / 3;
	if(DNPData.AverageDifferentialVoltage.Value > 10)		//Make sure voltage is large enough before making events on angle, 1000 = 100V
		checkVariableAnalogEvent(&DNPData.AverageDifferentialVoltageAngle, flags, &time, EventTriggerRange.AvgAparrentDiffVoltAngle);
	else
		DNPData.AverageDifferentialVoltageAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase1DifferentialVoltageReal.Value = dNPLongFrom8_8diffReal(dataPtr[28]);		//real
	checkVariableAnalogEvent(&DNPData.Phase1DifferentialVoltageReal, flags, &time, EventTriggerRange.RealDiffVolt);
	DNPData.Phase2DifferentialVoltageReal.Value = dNPLongFrom8_8diffReal(dataPtr[29]);
	checkVariableAnalogEvent(&DNPData.Phase2DifferentialVoltageReal, flags, &time, EventTriggerRange.RealDiffVolt);
	DNPData.Phase3DifferentialVoltageReal.Value = dNPLongFrom8_8diffReal(dataPtr[30]);
	checkVariableAnalogEvent(&DNPData.Phase3DifferentialVoltageReal, flags, &time, EventTriggerRange.RealDiffVolt);
	
	DNPData.AverageDifferentialVoltageReal.Value = (DNPData.Phase1DifferentialVoltageReal.Value + DNPData.Phase2DifferentialVoltageReal.Value + DNPData.Phase3DifferentialVoltageReal.Value) / 3;		//real
	checkVariableAnalogEvent(&DNPData.AverageDifferentialVoltageReal, flags, &time, EventTriggerRange.AvgRealDiffVolt);
	
	//Currents
	DNPData.Phase1Current.Value = dNPCurrentFrom4_12(dataPtr[6]);
	checkVariableAnalogEvent(&DNPData.Phase1Current, flags, &time, EventTriggerRange.Current);
	DNPData.Phase2Current.Value = dNPCurrentFrom4_12(dataPtr[7]);
	checkVariableAnalogEvent(&DNPData.Phase2Current, flags, &time, EventTriggerRange.Current);
	DNPData.Phase3Current.Value = dNPCurrentFrom4_12(dataPtr[8]);
	checkVariableAnalogEvent(&DNPData.Phase3Current, flags, &time, EventTriggerRange.Current);
	
	DNPData.Phase1CurrentAngle.Value = dNPAddAngles(dNPLongFrom5_11IQ(dataPtr[9], dataPtr[10]), (signed int)DNPData.Phase1NetworkVoltageAngle.Value);
	if(DNPData.Phase1Current.Value > 150)		//Make sure current is large enough before making events on angle
		checkVariableAnalogEvent(&DNPData.Phase1CurrentAngle, flags, &time, EventTriggerRange.CurrentAngle);
	else
		DNPData.Phase1CurrentAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase2CurrentAngle.Value = dNPAddAngles(dNPLongFrom5_11IQ(dataPtr[12], dataPtr[13]), (signed int)DNPData.Phase2NetworkVoltageAngle.Value);
	if(DNPData.Phase2Current.Value > 150)		//Make sure current is large enough before making events on angle
		checkVariableAnalogEvent(&DNPData.Phase2CurrentAngle, flags, &time, EventTriggerRange.CurrentAngle);
	else
		DNPData.Phase2CurrentAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase3CurrentAngle.Value = dNPAddAngles(dNPLongFrom5_11IQ(dataPtr[15], dataPtr[16]), (signed int)DNPData.Phase3NetworkVoltageAngle.Value);
	if(DNPData.Phase3Current.Value > 150)		//Make sure current is large enough before making events on angle
		checkVariableAnalogEvent(&DNPData.Phase3CurrentAngle, flags, &time, EventTriggerRange.CurrentAngle);
	else
		DNPData.Phase3CurrentAngle.Control.NotFirstDataRead = 0;
	
	DNPData.EffectiveCurrent.Value = dNPCurrentFrom4_12(dataPtr[37]);
	checkVariableAnalogEvent(&DNPData.EffectiveCurrent, flags, &time, EventTriggerRange.EffCurrent);
	
	DNPData.EffectiveCurrentAngle.Value = dNPLongFrom5_11IQ(dataPtr[38], dataPtr[39]);
	if(DNPData.EffectiveCurrent.Value > 150)
		checkVariableAnalogEvent(&DNPData.EffectiveCurrentAngle, flags, &time, EventTriggerRange.EffCurrentAngle);
	else
		DNPData.EffectiveCurrent.Control.NotFirstDataRead = 0; //Set to Zero so that it won't trigger an event when it comes back to a valid read amount

	DNPData.CurrentPositiveSequence.Value = dNPCurrentFrom4_12(dataPtr[40]);
	checkVariableAnalogEvent(&DNPData.CurrentPositiveSequence, flags, &time, EventTriggerRange.CurrentPositiveSequence);
	
	DNPData.CurrentPositiveSequenceAngle.Value = dNPLongFrom5_11IQ(dataPtr[41], dataPtr[42]);
	if(DNPData.CurrentPositiveSequence.Value > 150)
		checkVariableAnalogEvent(&DNPData.CurrentPositiveSequenceAngle, flags, &time, EventTriggerRange.CurrentPositiveSequenceAngle);
	else
		DNPData.CurrentPositiveSequence.Control.NotFirstDataRead = 0;
	
	DNPData.CurrentNegativeSequence.Value = dNPCurrentFrom4_12(dataPtr[43]);
	checkVariableAnalogEvent(&DNPData.CurrentNegativeSequence, flags, &time, EventTriggerRange.CurrentNegativeSequence);
	
	DNPData.CurrentNegativeSequenceAngle.Value = dNPLongFrom5_11IQ(dataPtr[44], dataPtr[45]);
	if(DNPData.CurrentNegativeSequence.Value > 150)
		checkVariableAnalogEvent(&DNPData.CurrentNegativeSequenceAngle, flags, &time, EventTriggerRange.CurrentNegativeSequenceAngle);
	else
		DNPData.CurrentNegativeSequence.Control.NotFirstDataRead = 0;
	
	//Power
	DNPData.Phase1Power.Value = dNPLongFrom5_11kW(dataPtr[9], dataPtr[0]);
	checkVariableAnalogEvent(&DNPData.Phase1Power, flags, &time, EventTriggerRange.PhaseKW); 
	DNPData.Phase2Power.Value = dNPLongFrom5_11kW(dataPtr[12], dataPtr[1]);
	checkVariableAnalogEvent(&DNPData.Phase2Power, flags, &time, EventTriggerRange.PhaseKW);
	DNPData.Phase3Power.Value = dNPLongFrom5_11kW(dataPtr[15], dataPtr[2]);
	checkVariableAnalogEvent(&DNPData.Phase3Power, flags, &time, EventTriggerRange.PhaseKW);
	
	DNPData.Phase1PowerAngle.Value = dNPLongFrom5_11IQ(dataPtr[9], dataPtr[10]);
	if(DNPData.Phase1Current.Value > 150)		//Make sure current is large enough before making events on angle
		checkVariableAnalogEvent(&DNPData.Phase1PowerAngle, flags, &time, EventTriggerRange.PhaseKVAAngle);
	
	DNPData.Phase2PowerAngle.Value = dNPLongFrom5_11IQ(dataPtr[12], dataPtr[13]);
	if(DNPData.Phase2Current.Value > 150)
		checkVariableAnalogEvent(&DNPData.Phase2PowerAngle, flags, &time, EventTriggerRange.PhaseKVAAngle);
	
	DNPData.Phase3PowerAngle.Value = dNPLongFrom5_11IQ(dataPtr[15], dataPtr[16]);
	if(DNPData.Phase3Current.Value > 150)
		checkVariableAnalogEvent(&DNPData.Phase3PowerAngle, flags, &time, EventTriggerRange.PhaseKVAAngle);
	
	DNPData.AveragePower.Value = (DNPData.Phase1VA.Value + DNPData.Phase2VA.Value + DNPData.Phase3VA.Value) / 3;
	checkVariableAnalogEvent(&DNPData.AveragePower, flags, &time, EventTriggerRange.AvgPhaseKVA);
	
	DNPData.AveragePowerAngle.Value = (DNPData.Phase1PowerAngle.Value + DNPData.Phase2PowerAngle.Value + DNPData.Phase3PowerAngle.Value) / 3;
	if(DNPData.Phase1Current.Value > 150)		//Make sure current is large enough before making events on angle
		checkVariableAnalogEvent(&DNPData.AveragePowerAngle, flags, &time, EventTriggerRange.AvgPhaseKVAAngle);
	
	
	//DNPData.Phase1VAR = dNPLongFrom5_11kW(dataPtr[10], dataPtr[0]);
	//DNPData.Phase2VAR = dNPLongFrom5_11kW(dataPtr[13], dataPtr[1]);
	//DNPData.Phase3VAR = dNPLongFrom5_11kW(dataPtr[16], dataPtr[2]);
	
	DNPData.Phase1VA.Value = dNPLongFrom5_11kW(dataPtr[6]>>1, dataPtr[0]);
	checkVariableAnalogEvent(&DNPData.Phase1VA, flags, &time, EventTriggerRange.PhaseKVA);
	DNPData.Phase2VA.Value = dNPLongFrom5_11kW(dataPtr[7]>>1, dataPtr[1]);
	checkVariableAnalogEvent(&DNPData.Phase2VA, flags, &time, EventTriggerRange.PhaseKVA);
	DNPData.Phase3VA.Value = dNPLongFrom5_11kW(dataPtr[8]>>1, dataPtr[2]);
	checkVariableAnalogEvent(&DNPData.Phase3VA, flags, &time, EventTriggerRange.PhaseKVA);
	
	
	

	DNPData.Phase1NetworkVoltageAngle.Value = 0;
	DNPData.Phase2NetworkVoltageAngle.Value = dNPLongFrom8_8IQ(dataPtr[18], dataPtr[19]);
	checkVariableAnalogEvent(&DNPData.Phase2NetworkVoltageAngle, flags, &time, EventTriggerRange.VoltageAngle);
	DNPData.Phase3NetworkVoltageAngle.Value = dNPLongFrom8_8IQ(dataPtr[20], dataPtr[21]);
	checkVariableAnalogEvent(&DNPData.Phase3NetworkVoltageAngle, flags, &time, EventTriggerRange.VoltageAngle);

	
	DNPData.Phase1DifferentialVoltageReal.Value = dNPLongFrom8_8diffReal(dataPtr[28]);
	checkVariableAnalogEvent(&DNPData.Phase1DifferentialVoltageReal, flags, &time, EventTriggerRange.RealDiffVolt);
	DNPData.Phase2DifferentialVoltageReal.Value = dNPLongFrom8_8diffReal(dataPtr[29]);
	checkVariableAnalogEvent(&DNPData.Phase2DifferentialVoltageReal, flags, &time, EventTriggerRange.RealDiffVolt);
	DNPData.Phase3DifferentialVoltageReal.Value = dNPLongFrom8_8diffReal(dataPtr[30]);
	checkVariableAnalogEvent(&DNPData.Phase3DifferentialVoltageReal, flags, &time, EventTriggerRange.RealDiffVolt);
	
	DNPData.NetworkVoltagePositiveSequence.Value = dNPLongFrom8_8(dataPtr[46] << 1);
	checkVariableAnalogEvent(&DNPData.NetworkVoltagePositiveSequence, flags, &time, EventTriggerRange.VoltPositiveSequence);
	
	DNPData.NetworkVoltagePositiveSequenceAngle.Value = dNPLongFrom8_8IQ(dataPtr[47], dataPtr[48]);
	if(DNPData.NetworkVoltagePositiveSequence.Value > 75)
		checkVariableAnalogEvent(&DNPData.NetworkVoltagePositiveSequenceAngle, flags, &time, EventTriggerRange.VoltPositiveSequenceAngle);
	else
		DNPData.NetworkVoltagePositiveSequenceAngle.Control.NotFirstDataRead = 0;
	
	
	DNPData.NetworkVoltageNegativeSequence.Value = dNPLongFrom8_8(dataPtr[49] << 1);
	checkVariableAnalogEvent(&DNPData.NetworkVoltageNegativeSequence, flags, &time, EventTriggerRange.VoltNegativeSequence);
	DNPData.NetworkVoltageNegativeSequenceAngle.Value = dNPLongFrom8_8IQ(dataPtr[50], dataPtr[51]);
	if(DNPData.NetworkVoltageNegativeSequence.Value > 75)
		checkVariableAnalogEvent(&DNPData.NetworkVoltageNegativeSequenceAngle, flags, &time, EventTriggerRange.VoltNegativeSequenceAngle);
	else
		DNPData.NetworkVoltageNegativeSequenceAngle.Control.NotFirstDataRead = 0;
	
	DNPData.DifferentialVoltagePositiveSequence.Value = dNPLongFrom8_8(dataPtr[52] << 1);
	checkVariableAnalogEvent(&DNPData.DifferentialVoltagePositiveSequence, flags, &time, EventTriggerRange.DiffVoltPositiveSequence);
	DNPData.DifferentialVoltagePositiveSequenceAngle.Value = dNPLongFrom8_8IQ(dataPtr[53], dataPtr[54]);
	if(DNPData.DifferentialVoltagePositiveSequence.Value > 15)
		checkVariableAnalogEvent(&DNPData.DifferentialVoltagePositiveSequenceAngle, flags, &time, EventTriggerRange.DiffVoltPositiveSequenceAngle);
	else
		DNPData.DifferentialVoltagePositiveSequenceAngle.Control.NotFirstDataRead = 0;
	
	DNPData.DifferentialVoltageNegativeSequence.Value = dNPLongFrom8_8(dataPtr[55] << 1);
	checkVariableAnalogEvent(&DNPData.DifferentialVoltageNegativeSequence, flags, &time, EventTriggerRange.DiffVoltNegativeSequence);
	DNPData.DifferentialVoltageNegativeSequenceAngle.Value = dNPLongFrom8_8IQ(dataPtr[56], dataPtr[57]);
	if(DNPData.DifferentialVoltageNegativeSequence.Value > 15)
		checkVariableAnalogEvent(&DNPData.DifferentialVoltageNegativeSequenceAngle, flags, &time, EventTriggerRange.DiffVoltNegativeSequenceAngle);
	else
		DNPData.DifferentialVoltageNegativeSequenceAngle.Control.NotFirstDataRead = 0;
	
	DNPData.Phase1NetworkTHD.Value = dNPLongFrom5_11(dataPtr[58]);
	if(DNPData.Phase1NetworkVoltage.Value > 1000) 
		checkVariableAnalogEvent(&DNPData.Phase1NetworkTHD, flags, &time, EventTriggerRange.VoltageTHD);
	else
		DNPData.Phase1NetworkTHD.Control.NotFirstDataRead = 0;
	
	DNPData.Phase2NetworkTHD.Value = dNPLongFrom5_11(dataPtr[59]);
	if(DNPData.Phase2NetworkVoltage.Value > 1000) 
		checkVariableAnalogEvent(&DNPData.Phase2NetworkTHD, flags, &time, EventTriggerRange.VoltageTHD);
	else
		DNPData.Phase2NetworkTHD.Control.NotFirstDataRead = 0;
	
	DNPData.Phase3NetworkTHD.Value = dNPLongFrom5_11(dataPtr[60]);
	if(DNPData.Phase3NetworkVoltage.Value > 1000) 
		checkVariableAnalogEvent(&DNPData.Phase3NetworkTHD, flags, &time, EventTriggerRange.VoltageTHD);
	else
		DNPData.Phase3NetworkTHD.Control.NotFirstDataRead = 0;
	
	
	if(DNPData.Phase1Current.Value * 5 / DNPData.sCTRatio.Value > 10) //TEST not sure what value should be
	{
		DNPData.Phase1CurrentTHD.Value = dNPLongFrom5_11(dataPtr[61]);	
		checkVariableAnalogEvent(&DNPData.Phase1CurrentTHD, flags, &time, EventTriggerRange.CurrentTHD);
	}
	else
	{
		DNPData.Phase1CurrentTHD.Value = 0;	
		DNPData.Phase1CurrentTHD.Control.NotFirstDataRead = 0;
	}
	
	
	if(DNPData.Phase2Current.Value * 5 / DNPData.sCTRatio.Value > 10) //TEST not sure what value should be
	{
		DNPData.Phase2CurrentTHD.Value = dNPLongFrom5_11(dataPtr[62]);	
		checkVariableAnalogEvent(&DNPData.Phase2CurrentTHD, flags, &time, EventTriggerRange.CurrentTHD);
	}
	else
	{
		
		DNPData.Phase2CurrentTHD.Value = dNPLongFrom5_11(dataPtr[62]);	
		DNPData.Phase2CurrentTHD.Control.NotFirstDataRead = 0;
	}
	
	if(DNPData.Phase3Current.Value * 5 / DNPData.sCTRatio.Value > 10) //TEST not sure what value should be
	{
		DNPData.Phase3CurrentTHD.Value = dNPLongFrom5_11(dataPtr[63]);
		checkVariableAnalogEvent(&DNPData.Phase3CurrentTHD, flags, &time, EventTriggerRange.CurrentTHD);
	}
	else
	{
		DNPData.Phase3CurrentTHD.Value = 0;
		DNPData.Phase3CurrentTHD.Control.NotFirstDataRead = 0;
	}
	
#endif

	DNPInit.FirstCommVectorRead = 1;
	
}

long dNPLongFrom8_8Memphis(unsigned int data)
{
	
	long temp;
	
	temp = (unsigned int)data;
	temp *= 10;
	
	//Rounding
	temp >>= 7;
	temp += temp & 1;
	temp >>= 1;
	
	return temp;
}

//Has 10x mult in it
long dNPLongFrom8_8(unsigned int data)
{
	long temp;
	
	temp = (unsigned int)data;
	temp *= 10;
	
	//Rounding
	temp >>= 7;
	temp += temp & 1;
	temp >>= 1;
	
	return temp;
}

long dNPLongFrom8_8diffReal(unsigned int data)
{
	long temp;
#if DNPCustomer == DNPCustomerMemphis
	temp = (signed int)data;
	temp *= 10;
	
	//Rounding
	temp >>= 7;
	temp += temp & 1;
	temp >>= 1;
#else
	temp = (signed int)data;
	temp *= 10;
	
	//Rounding
	temp >>= 7;
	temp += temp & 1;
	temp >>= 1;
#endif	
	return temp;
}

long dNPLongFrom9_7(unsigned int data)
{
	long temp = 0;
	//TEST remove all double
	/*
	double dTemp = 0.078125d; //7 fractional bits with 10x in it 
	
	dTemp *= (double)data;
	
	temp = (long)dTemp;
	*/
	return temp;
}

int dNPAddAngles(long a, long b)
{
	a = a + b;
	while(a > 1800 || a < -1800)
	{
		if(a > 1800)
			a -= 3600;
		else if(a < -1790)
			a += 3600;
	}
	
	return (int)a;
}

long dNPLongFrom8_8diffRMS(unsigned int real, unsigned int imaginary)
{
	union FIXED16_16 real16, imaginary16, RMS;
	int temp;
#if DNPCustomer == DNPCustomerMemphis

	real16.Full16 = (signed)real;
	real16.Full16 <<= 8;  //comes in as 8_8, convert to 16_16
	imaginary16.Full16 = (signed)imaginary;
	imaginary16.Full16 <<= 8;
	
	real16 = MULT16_16(real16, real16);
	imaginary16 = MULT16_16(imaginary16, imaginary16);
	real16.Full16 += imaginary16.Full16;
	
	RMS = Sqrt16(real16);
	RMS.Full16 *= 10;
#elif DNPCustomer == DNPCustomerDigitalGrid
	//TEST needs to be checked
	real16.Full16 = (signed)real;
	real16.Full16 <<= 8;  //comes in as 8_8, convert to 16_16
	imaginary16.Full16 = (signed)imaginary;
	imaginary16.Full16 <<= 8;
	
	real16 = MULT16_16(real16, real16);
	imaginary16 = MULT16_16(imaginary16, imaginary16);
	real16.Full16 += imaginary16.Full16;
	
	RMS = Sqrt16(real16);
	RMS.Full16 *= 10;
#else
	temp = (signed int)data;
	temp *= 10;
	
	//Rounding
	temp >>= 7;
	temp += temp & 1;
	temp >>= 1;
#endif	
	return RMS.Integer16;
}

union FIXED16_16 Sqrt16(union FIXED16_16 value)
{
	register unsigned long root, remHi, remLo, testDiv, count;
	int sign = 0;
	root = 0; 			// Clear root
	remHi = 0;			// Clear high part of partial remainder
	remLo = 0;			// Get argument into low part of partial remainder
	count = 23; 		// Load loop counter
						// Change count to make it work for certain Fixed Point algorithm
	
	if(value.Full16 < 0)
	{
		sign = 1;
		remLo = -value.Full16;
	}
	else
		remLo = value.Full16;
	
	
	do 
	{
		remHi = (remHi<<2) | (remLo>>30); 
		remLo <<= 2; 						// get 2 bits of arg
		root <<= 1; 						// Get ready for the next bit in the root
		testDiv = (root << 1) + 1; 			// Test radical
		if (remHi >= testDiv) 
		{
			remHi -= testDiv;
			root++;
		}
	} while (count-- != 0);
	
	if(sign == 1)
	{
		value.Full16 = -root;	
	}
	else
		value.Full16 = root;
	
	
	return value;
}


long dNPCurrentFrom4_12(unsigned int data)
{
	long temp = 0;
	//TEST remove all double
	/*
#if DNPCustomer == DNPCustomerMemphis
	double dTemp;
	
	//mult by 0.000244140625d to convert from 4_12
	//divide by five (mult by .2) for CT Ratio, to avoid unnecessary division
	//combined to 0.000048828125
	
	dTemp = (unsigned int)data * 0.000048828125;
	
	dTemp *= (double)(DNPData.CTRatio.Value);
	
	temp = dTemp;
#else
	//This was changed already to make something unsigned, if we get back
	//here things have to change.
	temp = (unsigned int)data;
	temp *= (DNPData.sCTRatio.Value / 5);
	temp *= 10;
	
	//rounding
	temp >>= 11;
	temp += temp & 1;
	temp >>= 1;
#endif
	*/
	return temp;
}

unsigned int DNPCurrentHighEnoughForTHD(unsigned int i)
{
	/*
	double dTemp;
	//TEST remove all double
	i *= 5;
#if DNPCustomer == DNPCustomerMemphis
	i /= DNPData.CTRatio.Value;	//Convert to relay amps
#else
	i /= DNPData.sCTRatio.Value;	//Convert to relay amps
#endif
	
	if(i >= 3)
		return 1;
	else
		return 0;
	*/
	return i;
}

long dNPLongFrom5_11IQ(unsigned int i, unsigned int q)
{
	//TEST removed all doubles
	return i;
	/*
	double localI, localQ;

#if DNPCustomer == DNPCustomerMemphis
	localI = (double)(signed int)i * 0.048828125d; 
	localQ = (double)(signed int)q * 0.048828125d;
	
	//cordic_get_mag_phase(localI, localQ, &localQ, &localI);
	
	localI = angleFromRadians(localI, 0);
	localI *= 10.0;
	//Rounding
	
	localI = roundDouble(localI);
	
	if(localI < 0)
	{
		localI += 3600;
	}
	
	if((int)localI == 3600)
	{
		localI = 0;
	}
	
#else
	localI = (double)(signed int)i * 0.048828125d; 
	localQ = (double)(signed int)q * 0.048828125d;
	
	cordic_get_mag_phase(localI, localQ, &localQ, &localI);
	
	localI = angleFromRadians(localI, 0);
	localI *= 10.0;
	//Rounding
	
	localI = roundDouble(localI);
	
	if((int)localI == 3600)
	{
		localI = 0;
	}
#endif
	return localI;
	*/
}

long dNPLongFrom8_8IQ(unsigned int i, unsigned int q)
{
	return i;
	/*
	//TEST remove all doubles
	double localI, localQ;
	
	localI = (double)(signed int)i * 0.048828125d;
	localQ = (double)(signed int)q * 0.048828125d;
	
	cordic_get_mag_phase(localI, localQ, &localQ, &localI);
	
	localI = angleFromRadians(localI, 0);
	
	localI *= 10.0;
	//Rounding
	
	localI = roundDouble(localI);
	
	if(localI == 3600)
	{
		localI = 0;
	}
	
	return localI;
	*/
}

long dNPLongFrom8_8_40(unsigned int i, unsigned int q)
{
//TEST remove all doubles
	return i;
/*
	double localI, localQ;
	
	localI = (double)(signed int)i * 0.00390625d;	//convert from 2^-8
	localQ = (double)(signed int)q * 0.00390625d;
	
	cordic_get_mag_phase(localI, localQ, &localQ, &localI); //localQ holds mag
	
	//localQ = roundDouble(localQ);
	
	localQ *= 1638.4d; //convert to 40 for full range 2^16
	
	return localQ;
	*/
}


long dNPLongFrom5_11(unsigned int data)
{
	long temp;
#if DNPCustomer == DNPCustomerMemphis
	temp = (unsigned int)data;
	//temp *= (DNPData.CTRatio / 5);
	temp *= 10;
	
	//rounding
	temp >>= 10;
	temp += temp & 1;
	temp >>= 1;
	
#else
	temp = (unsigned int)data;
	//temp *= (DNPData.CTRatio / 5);
	temp *= 100;
	
	//rounding
	temp >>= 10;
	temp += temp & 1;
	temp >>= 1;
#endif	
	return temp;
}


//data is real current, data2 is voltage

long dNPLongFrom5_11kW(unsigned int amperage, unsigned int voltage) //kW
{
	long temp, temp2;
#if DNPCustomer == DNPCustomerMemphis
	temp = (signed int)amperage;
	temp *= (DNPData.CTRatio.Value / 5);
	
	//rounding
	temp >>= 10;
	temp += temp & 1;
	temp >>= 1;
	
	temp2 = (unsigned int)voltage;
	
	//Rounding
	temp2 >>= 7;
	temp2 += temp2 & 1;
	temp2 >>= 1;
	
	temp *= temp2;
	
	temp /= 1000; //to kW
#elif DNPCustomer == DNPCustomerDigitalGrid
	//TEST needs to be checked
	temp = (signed int)amperage;
	temp *= (DNPData.sCTRatio.Value / 5);
	
	//rounding
	temp >>= 10;
	temp += temp & 1;
	temp >>= 1;
	
	temp2 = (unsigned int)voltage;
	
	//Rounding
	temp2 >>= 7;
	temp2 += temp2 & 1;
	temp2 >>= 1;
	
	temp *= temp2;
	
	temp /= 1000; //to kW
#else
	temp = (unsigned int)data;
	//temp *= (DNPData.CTRatio / 5);
	temp *= 100;
	
	//rounding
	temp >>= 10;
	temp += temp & 1;
	temp >>= 1;
#endif	
	return temp;
}

//data is real current, data2 is voltage

long dNPLongFrom5_11kW_1500(unsigned int data, unsigned int data2)
{
	//TEST remove all double
	/*
	double localI, localQ;
	
	//convert from 5_11 and 8_8
	localI = (double)(signed int)data * 0.00048828125d;
	localQ = (double)(signed int)data2 * 0.00390625d;
	
	localQ *= localI;
	
	//convert for CT Ratio
#if DNPCustomer == DNPCustomerMemphis
	localQ *= (double)(DNPData.CTRatio.Value / 5);
	//convert to memphis range AND divide by 1000;
	 
	localQ *= 0.0436906667d;//43.69066667d for 1500 over range of 2^16
	//need weird divide by 2 for some reason, not sure
	//localQ *= 0.0218453333d;//43.69066667d for 1500 over range of 2^16
#else
	//TEST needs to be checked
	localQ *= (double)(DNPData.sCTRatio.Value / 5);
#endif
	
	
	return localQ;
	*/
	return data;

}

long totalKW(void)
{
	//TEST remove all double
	/*
	double returnValue = 0;
	returnValue = (signed long)DNPData.Phase1Power.Value + (signed long)DNPData.Phase2Power.Value + (signed long)DNPData.Phase3Power.Value;
	//desired LSB value =  5000 / 2^16 = 0.0762939453125
	//currently LSB value = 1500 / 2^16 = 0.02288818359375
	//.3
	
	//returnValue *= 0.33333333d;
	
	return returnValue;
	*/
	return 1;
}
#if DNPCustomer == DNPCustomerMemphis

long totalVAR(void)
{
//TEST remove all double

	long returnValue = 0;
	
	returnValue = (signed long)DNPData.Phase1VAR.Value + (signed long)DNPData.Phase2VAR.Value + (signed long)DNPData.Phase3VAR.Value;
	//desired LSB value =  10000 / 2^16 = 0.152587890625
	//currently LSB value = 1000 / 2^16 = 0.0152587890625
	//.1 conversion
	
	//returnValue *= 0.33333d;
	return returnValue;
	
}

long totalVA(void)
{
	long returnValue = 0;
	
	returnValue = DNPData.Phase1VA.Value + DNPData.Phase2VA.Value + DNPData.Phase3VA.Value;
	//desired LSB value =  10000 / 2^16 = 0.152587890625
	//currently LSB value = 1500 / 2^16 = 0.02288818359375
	//.15 conversion
	
	//returnValue *= 0.3333333d;
	
	return returnValue;
}

#endif

long dNPLongFrom5_11kW_1000(unsigned int data, unsigned int data2)
{
//TEST remove all double
/*
	double localI, localQ;
	
	//convert from 5_11 and 8_8
	localI = (double)(signed int)data * 0.00048828125d;
	localQ = (double)(signed int)data2 * 0.00390625d;
	
	localQ *= localI;
	
	//convert for CT Ratio
#if DNPCustomer == DNPCustomerMemphis
	localQ *= (double)(DNPData.CTRatio.Value / 5);
	//convert to memphis range AND divide by 1000;
	localQ *= 0.065536d;//0.0152587890625 for 1000 over range of 2^16
#elif DNPCustomer == DNPCustomerDigitalGrid
	//TEST needs to be checked
	localQ *= (double)(DNPData.sCTRatio.Value / 5);
#endif
	return localQ;
	*/
	return data;
}

/*
typedef TMWTYPES_UCHAR DNPDEFS_DBAS_FLAG;
#define DNPDEFS_DBAS_FLAG_OFF_LINE      0x00
#define DNPDEFS_DBAS_FLAG_ON_LINE       0x01
#define DNPDEFS_DBAS_FLAG_RESTART       0x02
#define DNPDEFS_DBAS_FLAG_COMM_LOST     0x04
#define DNPDEFS_DBAS_FLAG_REMOTE_FORCED 0x08
#define DNPDEFS_DBAS_FLAG_LOCAL_FORCED  0x10
#define DNPDEFS_DBAS_FLAG_CHATTER       0x20
#define DNPDEFS_DBAS_FLAG_CNTR_ROLLOVER 0x20
#define DNPDEFS_DBAS_FLAG_OVER_RANGE    0x20
#define DNPDEFS_DBAS_FLAG_REFERENCE_CHK 0x40
#define DNPDEFS_DBAS_FLAG_BINARY_ON     0x80
#define DNPDEFS_DBAS_FLAG_BINARY_OFF    0x00
*/
void convertFlashMemoryToDNPDB(void)
{
	char tempChar;
	unsigned short pointNumber;
	long tempLong;
	//TEST remove all double double tempDouble;
	PUMPENABLE pumpEnables;
	TMWDTIME time;
	//TMWTYPES_ANALOG_VALUE *pValue;
	TMWTYPES_UCHAR flags;
	union FIXED16_16 tempFixed;
	
	
	if(DNPInit.InitComplete)
		flags = DNPDEFS_DBAS_FLAG_ON_LINE;
	else
		flags = DNPDEFS_DBAS_FLAG_RESTART | DNPDEFS_DBAS_FLAG_ON_LINE;
	
	DNPConvertRelayTimeToDNP(event_clock.long_time, DNPTimer%1000, &time);
	
	//CTRatio

	tempLong = (signed char)RAM_parameters.CT_msbyte;
	tempLong <<= 8;
	tempLong += RAM_parameters.CT_lsbyte;
	tempLong *= 5;
	//checkAnalogEvent(&DNPData.CTRatio, DNPData.CTRatio, tempLong, flags, &time);

#if DNPCustomer == DNPCustomerMemphis
	DNPData.CTRatio.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sCTRatio.Value = tempLong;
#endif

	

	//Reclose Volts	
#if DNPCustomer == DNPCustomerMemphis
	tempLong = RAM_parameters.C_word[0];	//-12 offset
	tempLong = multiplyBy10Convert(tempLong>>1);
	tempLong /= 10;
	DNPData.RecloseVolts.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid

	tempLong = RAM_parameters.C_word[0];	//-12 offset
	tempLong = multiplyBy10Convert(tempLong);

	DNPData.sCloseModeRecloseVoltage.Value = tempLong;
#endif
	
	

	
	if(RAM_parameters.Mclose_byte2 == 'r' || RAM_parameters.Mclose_byte2 == 's')
		tempChar = 1;
	else
		tempChar = 0;

#if DNPCustomer == DNPCustomerMemphis
	DNPData.RemoteClose.Value = DNPData.sRelaxClose.Value = DNPData.RelaxedCloseEnabled.Value = tempChar;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sRelaxClose.Value = tempChar;
#else
	DNPData.sRelaxedClose = DNPData.RelaxedCloseEnabled = tempChar;
#endif
	
	//Close Tilt Angle
	tempLong = (signed int)RAM_parameters.C_word[1];
	tempLong <<= 4;
	if(tempLong == 0)
		tempLong = 90;
	else
		tempLong = 0;//TEST remove double tempLong = convertToDNPLong(getAngle(tempLong, 180), 1);

#if DNPCustomer == DNPCustomerMemphis
	DNPData.CloseTiltAngle.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sCloseModeCloseTiltAngle.Value = tempLong;
#endif
	
#if DNPCustomer != DNPCustomerMemphis && DNPCustomer != DNPCustomerDigitalGrid
	DNPData.sRecloseAngle.Value = DNPData.CloseTiltAngle;
#endif

#if DNPCustomer == DNPCustomerMemphis
	DNPData.DD_RecloseAngle.Value = DNPData.CloseTiltAngle.Value;
#endif
	
	
	//Phasing Volts
	tempLong = (signed int)RAM_parameters.C_word[2];
	#if DNPCustomer == DNPCustomerMemphis
	tempLong = multiplyBy10Convert(tempLong >> 1);
	tempLong /= 10;
	#else
	tempLong = multiplyBy10Convert(tempLong);
	#endif

#if DNPCustomer == DNPCustomerMemphis	
	DNPData.PhaseDetectionVolts.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sCloseModePhaseDetectionVoltage.Value = tempLong;
#else
	DNPData.PhaseDetectionVolts.Value = tempLong;
	DNPData.sPhasingVoltage = DNPData.PhaseDetectionVolts;
#endif

	//Phasing Angle
	
	tempLong = (signed int)RAM_parameters.C_word[3];
	//TEST remove all double tempLong = convertToDNPLong(getAngle(tempLong, 0), 1);
#if DNPCustomer == DNPCustomerMemphis
	DNPData.PhasingRecloseAngle.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sCloseModePhaseDetectionAngle.Value = tempLong;
#endif
	
	//Close Mode Type
#if DNPCustomer == DNPCustomerMemphis

	if(RAM_parameters.Mclose_byte2 == 'C' || RAM_parameters.Mclose_byte2 == 's')
	{
		DNPData.RecloseAlgorithm.Value = 6;
		tempChar = 1;
	}
	else
	{
		DNPData.RecloseAlgorithm.Value = 1;
		tempChar = 0;
	}
	DNPData.CircleCloseEnabled.Value = tempChar;
#elif DNPCustomer == DNPCustomerDigitalGrid
	if(RAM_parameters.Mclose_byte2 == 'C' || RAM_parameters.Mclose_byte2 == 's')
	{
		tempChar = 1;
	}
	else
	{
		tempChar = 0;
	}
	DNPData.sCloseModeCircleClose.Value = tempChar;
#endif
	
	//Close Mode Delay
	
	//DNPData.RecloseTimeDelay = RAM_parameters.Mclose_byte3;
	//DNPData.RecloseTimeDelay <<= 8;
	//DNPData.RecloseTimeDelay += RAM_parameters.Mclose_byte4;
	tempLong = RAM_parameters.Mclose_byte3;
	tempLong <<= 8;
	tempLong += RAM_parameters.Mclose_byte4;
	
#if DNPCustomer == DNPCustomerMemphis
	DNPData.RecloseTimeDelay.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sCloseModeCloseTimeDelay.Value = tempLong;
#else 
	DNPData.sRecloseTimeDelay.Value = DNPData.RecloseTimeDelay.Value;
#endif
	//Trip Mode

#if DNPCustomer == DNPCustomerMemphis
	
	switch((char)RAM_parameters.Mtrip_byte2)
	{
		case 'S':
			DNPData.WattVarModeEnabled.Value = 0;
			DNPData.TimeDelayModeEnabled.Value = 0;
			DNPData.InsensitiveModeEnabled.Value = 0;
			DNPData.SensitiveModeEnabled.Value = 1;
			break;
		case 'W':
			DNPData.WattVarModeEnabled.Value = 1;
			DNPData.TimeDelayModeEnabled.Value = 0;
			DNPData.InsensitiveModeEnabled.Value = 0;
			DNPData.SensitiveModeEnabled.Value = 0;
			break;
		case 'T':
			DNPData.WattVarModeEnabled.Value = 0;
			DNPData.TimeDelayModeEnabled.Value = 1;
			DNPData.InsensitiveModeEnabled.Value = 0;
			DNPData.SensitiveModeEnabled.Value = 0;
			break;
		case 'I':
			DNPData.WattVarModeEnabled.Value = 0;
			DNPData.TimeDelayModeEnabled.Value = 0;
			DNPData.InsensitiveModeEnabled.Value = 1;
			DNPData.SensitiveModeEnabled.Value = 0;
			break;
		default:
			DNPData.WattVarModeEnabled.Value = 0;
			DNPData.TimeDelayModeEnabled.Value = 0;
			DNPData.InsensitiveModeEnabled.Value = 0;
			DNPData.SensitiveModeEnabled.Value = 0;
			IIN.ParameterError = 1;
			break;
	}
#elif DNPCustomer == DNPCustomerDigitalGrid
	switch((char)RAM_parameters.Mtrip_byte2)
	{
		case 'S':
			DNPData.sTripModeWattVar.Value = 0;
			DNPData.sTripModeTimeDelay.Value = 0;
			DNPData.sTripModeInsensitive.Value = 0;
			DNPData.sTripModeSensitive.Value = 1;
			break;
		case 'W':
			DNPData.sTripModeWattVar.Value = 1;
			DNPData.sTripModeTimeDelay.Value = 0;
			DNPData.sTripModeInsensitive.Value = 0;
			DNPData.sTripModeSensitive.Value = 0;
			break;
		case 'T':
			DNPData.sTripModeWattVar.Value = 0;
			DNPData.sTripModeTimeDelay.Value = 1;
			DNPData.sTripModeInsensitive.Value = 0;
			DNPData.sTripModeSensitive.Value = 0;
			break;
		case 'I':
			DNPData.sTripModeWattVar.Value = 0;
			DNPData.sTripModeTimeDelay.Value = 0;
			DNPData.sTripModeInsensitive.Value = 1;
			DNPData.sTripModeSensitive.Value = 0;
			break;
		default:
			DNPData.sTripModeWattVar.Value = 0;
			DNPData.sTripModeTimeDelay.Value = 0;
			DNPData.sTripModeInsensitive.Value = 0;
			DNPData.sTripModeSensitive.Value = 0;
			IIN.ParameterError = 1;
			break;
	}
#endif

#if DNPCustomer == DNPCustomerMemphis
	DNPData.DInsensitiveModeEnabled.Value = DNPData.InsensitiveModeEnabled.Value;
#endif
	//Time Delay
	tempLong = RAM_parameters.Mtrip_byte3;
	tempLong <<= 8;
	tempLong += RAM_parameters.Mtrip_byte4;
	
#if DNPCustomer == DNPCustomerMemphis
	DNPData.TimeDelaySeconds.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sTripModeTimeDelayTripTimeDelay.Value = tempLong;
#endif
	
#if DNPCustomer != DNPCustomerMemphis && DNPCustomer != DNPCustomerDigitalGrid
	DNPData.sTimeDelay.Value = DNPData.TimeDelaySeconds.Value;
#endif
	
	//Insantaneous trip
	tempLong = (unsigned char)RAM_parameters.T3_byte11;
	tempLong <<= 8;
	tempLong += (unsigned char)RAM_parameters.T3_byte12;
	
#if DNPCustomer == DNPCustomerMemphis
	tempLong = multiplyBy1Convert(tempLong);
	
	DNPData.InstantTripCurrent.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	//TEST remove all double tempLong = dNPConvert10FractMult10(tempLong);
	
	DNPData.sTripModeInstantaneousTripCurrent.Value = (unsigned int)tempLong;
#endif
	
	
#if DNPCustomer != DNPCustomerMemphis && DNPCustomer != DNPCustomerDigitalGrid
	DNPData.sInstantCurrent.Value = DNPData.InstantTripCurrent.Value;
#endif
	
	//Inensitive Trip Current
	
#if DNPCustomer == DNPCustomerMemphis

	tempLong = (unsigned char)RAM_parameters.T2_byte11;
	tempLong <<= 8;
	tempLong += (unsigned char)RAM_parameters.T2_byte12;
	tempLong <<= 6;
	
	//TEST remove all double tempDouble = (double)tempLong * 0.0000030517578125d; //0.0000152587890625 / 5 (for CT Ratio)
	//TEST remove all double tempDouble *= (double)DNPData.CTRatio.Value; //For amperage
	
	DNPData.InsensitiveTripCurrentAmps.Value = (long)tempDouble;
#else
	tempLong = (unsigned char)RAM_parameters.T2_byte11;
	tempLong <<= 8;
	tempLong += (unsigned char)RAM_parameters.T2_byte12;
	//TEST remove all double tempLong = dNPConvert10FractMult10(tempLong);
	
	DNPData.sTripModeInsensitiveTripCurrent.Value = (unsigned int)tempLong; 
#endif	
	
	//Watt-Var Current
	
	tempLong = RAM_parameters.T4_byte11;
	tempLong <<= 8;
	tempLong += RAM_parameters.T4_byte12;
	
	
#if DNPCustomer == DNPCustomerMemphis
	tempLong <<= 6;
	//TEST remove all double tempDouble = (double)tempLong * 0.0000030517578125d; //0.0000152587890625 / 5 (for CT Ratio)
	//TEST remove all double tempDouble *= (double)DNPData.CTRatio.Value; //For amperage
	DNPData.WattVarCurrent.Value = (long)tempDouble;
#elif DNPCustomer == DNPCustomerDigitalGrid
	//TEST remove all double tempLong = dNPConvert10FractMult10(tempLong);
	DNPData.sTripModeWattVarCurrent.Value = (long)tempLong;
#endif

	
	
	
	//Sensitive Trip Angle
	tempLong = (signed char)RAM_parameters.T0_byte5;
	tempLong <<= 8;
	tempLong += RAM_parameters.T0_byte6;
	tempLong <<= 4;
	
	if(tempLong == 0)
		tempLong = 90;
	else
		tempLong = 0; //TEST Remove all double tempLong = convertToDNPLong(getAngle(tempLong, 180), 1);
	
#if DNPCustomer == DNPCustomerMemphis
	DNPData.SensitiveTripAngle.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sTripModeTiltAngle.Value = tempLong;
#endif
	
	
	//Watt-Var Angle
	
	tempLong = (signed char)RAM_parameters.T4_byte5;
	tempLong <<= 8;
	tempLong += RAM_parameters.T4_byte6;
	tempLong <<= 4;
	//TEST Remove all double tempLong = convertToDNPLong(getAngle(tempLong, 0), 1);
	
#if DNPCustomer == DNPCustomerMemphis
	tempLong = DNPData.SensitiveTripAngle.Value - tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	tempLong = DNPData.sTripModeTiltAngle.Value - tempLong;
#endif
	
	if(tempLong == 90)
	{
		tempLong = 0;
	}
	
	if(tempLong > 90)
		tempLong -= 180;
	
#if DNPCustomer == DNPCustomerMemphis
	DNPData.WattVarAngle.Value = -tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sTripModeWattVarAngle.Value = -tempLong;
#else
	DNPData.sWattVarAngle = DNPData.WattVarAngle;
#endif
	
	//Trim Curve Enable
	if(RAM_parameters.T1_byte2 == 'O')
		tempChar = 1;
	else
		tempChar = 0;
	
#if DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sTripModeEnableTrimCurve.Value = tempChar;
#elif DNPCustomer != DNPCustomerMemphis
	DNPData.TrimCurveEnabled.Value = tempChar;
#endif
	
	//Trim Curve Angle
	tempLong = (signed char)RAM_parameters.T1_byte5;
	tempLong <<= 8;
	tempLong += RAM_parameters.T1_byte6;
	tempLong <<= 4;
	if(tempLong == 0)
		tempLong = 90;
	else
		tempLong = 0; //TEST remove all double tempLong = convertToDNPLong(getAngle(tempLong, 180), 1);
#if DNPCustomer == DNPCustomerDigitalGrid	
	DNPData.sTripModeTrimAngle.Value = tempLong;
#endif
	
	//Extended Delay 
	tempLong = RAM_parameters.Mtrip_byte5;
	
	//checkAnalogEvent(&DNPData.ExtendedDelay, DNPData.ExtendedDelay, tempLong, flags, &time);
#if DNPCustomer == DNPCustomerMemphis
	DNPData.ExtendedDelay.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sTripModeExtendedDelay.Value = tempLong;
#else DNPCustomer != DNPCustomerMemphis
	DNPData.sExtendedTimeDelay = DNPData.ExtendedDelay;
#endif
	
	//Sensitive Trip
#if DNPCustomer == DNPCustomerMemphis
	//ADJUST
	tempLong = (signed char)RAM_parameters.T0_byte11;
	tempLong <<= 8;
	tempLong += RAM_parameters.T0_byte12;
	tempLong <<= 8;
	tempLong += RAM_parameters.T0_byte3;
	tempLong <<= 8;
	tempLong += RAM_parameters.T0_byte4;
	//tempLong = -DNPData.SensitiveTripSetting;
	tempLong = -tempLong;
	
	//TEST remove all double tempDouble = (double)tempLong * 0.000030517578125d; //0.0000152587890625 / 5 (for CT Ratio) * 10
	//TEST remove all double tempDouble *= (double)DNPData.CTRatio.Value; //For amperage

	tempLong = multiplyBy100Convert(tempLong); //For percenate3ge
	
	DNPData.SensitiveTripSettingPercent.Value = tempLong;

	DNPData.SensitiveTripSettingAmps.Value = (long)tempDouble;
	
#elif DNPCustomer == DNPCustomerDigitalGrid

	tempLong = (signed char)RAM_parameters.T0_byte11;
	tempLong <<= 8;
	tempLong += RAM_parameters.T0_byte12;
	tempLong <<= 8;
	tempLong += RAM_parameters.T0_byte3;
	tempLong <<= 8;
	tempLong += RAM_parameters.T0_byte4;
	tempLong = -tempLong;
	
	//TEST remove all double tempDouble = (double)tempLong * 0.0152587890625;//0.0000030517578125d; //0.0000152587890625 / 5 (for CT Ratio)
	//TEST remove all double tempDouble = roundDouble(tempDouble);

	//TEST remove all double tempDouble = roundDouble(tempDouble);DNPData.sTripModeSensitiveTripCurrent.Value = (long)tempDouble;
	
#else
	tempLong = (signed char)RAM_parameters.T0_byte11;
	tempLong <<= 8;
	tempLong += RAM_parameters.T0_byte12;
	tempLong <<= 8;
	tempLong += RAM_parameters.T0_byte3;
	tempLong <<= 8;
	tempLong += RAM_parameters.T0_byte4;
	//tempLong = -DNPData.SensitiveTripSetting;
	tempLong = multiplyBy100Convert(tempLong);
	DNPData.sSensitiveTripSetting = DNPData.SensitiveTripSetting = tempLong;

	
#endif
	
	
	//SensitiveTripDelay
	tempLong = RAM_parameters.Mtrip_byte6;
#if DNPCustomer == DNPCustomerMemphis
	DNPData.SensitiveTripDelay.Value = tempLong;
#elif DNPCUstomer == DNPCustomerDigitalGrid
	DNPData.sTripModeSensitiveTimeDelay.Value = tempLong;
#else
	DNPData.sSensitiveTimeDelay = DNPData.SensitiveTripDelay;
#endif
	//Pump Modes Enable
	pumpEnables.word = RAM_parameters.pump_mode_enabled;
	
	tempChar = pumpEnables.RelayCycle;
	tempChar = pumpEnables.MotorTimeout;
	tempChar = pumpEnables.MotorCycle;
	
#if DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sPumpModeRelayCycle.Value 			= pumpEnables.RelayCycle;
	DNPData.sPumpModeRelayMotorCycle.Value 		= pumpEnables.MotorCycle;
	DNPData.sPumpModeRelayMotorTimeout.Value	= pumpEnables.MotorTimeout;
#elif DNPCustomer != DNPCustomerMemphis
	DNPData.RelayCyclePumpEnabled.Value = pumpEnables.RelayCycle;
	DNPData.MotorCyclePumpEnabled.Value = pumpEnables.MotorCycle;
	DNPData.MotorTimeoutPumpEnabled.Value = pumpEnables.MotorTimeout;
#endif

#if DNPCustomer == DNPCustomerMemphis	
	DNPData.PumpProtectionMode.Value = 0;
	if(pumpEnables.RelayCycle)
	{
		DNPData.PumpProtectionMode.Value = 1;
	}
	if(pumpEnables.MotorTimeout || pumpEnables.MotorCycle)
	{
		DNPData.PumpProtectionMode.Value += 2;
	}
#endif

	//Trip On Power Down and Trip Style
#if DNPCustomer == DNPCustomerDigitalGrid
	if(RAM_parameters.dummy_PC_param_bytes[0] & 0x04 == 0x04)
		DNPData.sTripModeTripOnPowerDown.Value = 0;					//Reversed For Backwards Compatibility with default setting
	else
		DNPData.sTripModeTripOnPowerDown.Value = 1;
#endif
	
	//Pump Relay Cycle Limit
	tempLong = RAM_parameters.pump_cycles;
	//checkAnalogEvent(&DNPData.PumpRelayCycleLimit, DNPData.PumpRelayCycleLimit, tempLong, flags, &time);
#if DNPCustomer == DNPCustomerMemphis
	DNPData.PumpRelayCycleLimit.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sPumpModeRelayCycleLimit.Value = tempLong;
#else
	DNPData.sPumpRelayCycleLimit.Value = DNPData.PumpRelayCycleLimit.Value;
#endif
	
	//Pump Relay Time Limit
	tempLong = RAM_parameters.pump_time_MSbyte;
	tempLong <<= 8;
	tempLong += RAM_parameters.pump_time_LSbyte;
	tempLong >>= 2;  //It is in quarter seconds, display as seconds
	
#if DNPCustomer == DNPCustomerMemphis
	DNPData.PumpRelayTimeLimit.Value = tempLong;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sPumpModeRelayCycleTime.Value = tempLong;
#else
	DNPData.sPumpRelayTimeLimit.Value = DNPData.PumpRelayTimeLimit.Value;
#endif
	
#if DNPCustomer == DNPCustomerMemphis
	DNPData.DPumpRelayTimeLimit.Value = DNPData.PumpRelayTimeLimit.Value;
#endif
	
	//Pump Motor Timeout and Motor Cycles
	DNPData.sPumpModeMotorTimeout.Value = RAM_parameters.timeout_on_breaker_close / 10;
	DNPData.sPumpModeMotorCycles.Value = RAM_parameters.pumping_count_on_close;
	//Pump Protect Time
	tempLong = RAM_parameters.protect_time_MSbyte;
	tempLong <<= 8;
	tempLong += RAM_parameters.protect_time_LSbyte;
	//checkAnalogEvent(&DNPData.PumpProtectTime, DNPData.PumpProtectTime, tempLong, flags, &time);
#if DNPCustomer == DNPCustomerMemphis
	DNPData.PumpProtectTime.Value = tempLong;
	
	if(DNPData.PumpProtectTime.Value == 0)
		tempChar = 1;
	else
		tempChar = 0;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sPumpModeLockoutTime.Value = RAM_parameters.protect_time;
	if(DNPData.sPumpModeLockoutTime.Value == 0)
		tempChar = 1;
	else
		tempChar = 0;
#else
	DNPData.sPumpProtectTime.Value = DNPData.PumpProtectTime.Value;
#endif
	
	//Pump Motor Cycles
	
#if DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sPumpModeNeverReclose.Value = tempChar;
#elif DNPCustomer != DNPCustomerMemphis
	DNPData.NeverRecloseEnabled.Value = tempChar;	
#endif

#if DNPCustomer == DNPCustomerMemphis
	//Master Revision Number
	DNPData.MasterSoftwareRevision.Value = 2;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.RelayVersionNo.Value = 2;
#endif

	//Serial Number
#if DNPCustomer == DNPCustomerMemphis
	DNPData.RelaySerialNoLow.Value = RAM_parameters.serial_number;
	DNPData.RelaySerialNoHigh.Value = 0;
#else
	DNPData.RelaySerialNo.Value = RAM_parameters.serial_number;
#endif
	
	tempChar = RAM_parameters.relay_type & 0x00FF;

	changed_relay_parameters = 0;
	
	//Phase Sensitivity:  0 = ABC, 1 = ACB 2 = Auto in relay software
	tempChar = RAM_parameters.relay_type_byte2;
#if DNPCustomer == DNPCustomerMemphis
	DNPData.PhaseSensitivity.Value = (tempChar + 1)%3;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sPhasingMode.Value = tempChar;
#endif
	
	//Relay Type
	if(RAM_parameters.relay_type_byte1 == 'S')
		tempChar = 1;
	else
		tempChar = 0;
#if DNPCustomer == DNPCustomerMemphis
	DNPData.SequenceRelay.Value = tempChar;
#elif DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sSequenceRelay.Value = tempChar;
#endif
	
	//Trip On Power Down
#if DNPCustomer == DNPCustomerMemphis
	if((RAM_parameters.dummy_PC_param_bytes[0] & 0x04) == 0x04)
		DNPData.RelayDeEnergizedAction.Value = 2;
	else
		DNPData.RelayDeEnergizedAction.Value = 1;
#elif DNPCustomer == DNPCustomerDigitalGrid
	if((RAM_parameters.dummy_PC_param_bytes[0] & 0x04) == 0x04)  //TEST make sure not backwards
		DNPData.sTripModeTripOnPowerDown.Value = 1;
	else
		DNPData.sTripModeTripOnPowerDown.Value = 0;
#endif

#if DNPCustomer == DNPCustomerDigitalGrid
	DNPData.sTripModeTripStyle.Value = RAM_parameters.dummy_PC_param_bytes[0];
	
#endif;
}

void checkBinaryEvent(BinaryPoint * point, unsigned char oldValue, unsigned char newValue, TMWTYPES_UCHAR flags, TMWDTIME *time)
{
	unsigned int pointNumber;
	char testChar;
	
	if(!DNPInit.InitComplete || !DNPInit.FirstCommVectorRead || !DNPInit.FirstRegisterRead || !DNPInit.FirstExternalPointRead)
		return;
	
	if(newValue != oldValue && point->Control.EventEnabled)
	{
		
		pointNumber = point - &DNPData.BinaryInputs0;
		
#if SDNPDATA_SUPPORT_OBJ2
		if(newValue)	
			sdnpo002_addEvent(mySclSession, pointNumber, DNPDEFS_DBAS_FLAG_BINARY_ON | flags, time);
		else
			sdnpo002_addEvent(mySclSession, pointNumber, DNPDEFS_DBAS_FLAG_BINARY_OFF | flags, time);
#endif

	}
}


void checkVariableAnalogEvent(AnalogPointIn *point, TMWTYPES_UCHAR flags, TMWDTIME *time, long range)
{
	TMWTYPES_ANALOG_VALUE pValue;
	int pointNumber;
	int tempValue, tempOld;
	
	if(!DNPInit.InitComplete || !DNPInit.FirstCommVectorRead || !DNPInit.FirstRegisterRead || !DNPInit.FirstExternalPointRead)  //If we have initialized yet, do nothing
	{	
		return;
	}
	
	if(!point->Control.NotFirstDataRead)		//if zero, it is the first time we have looked at this valid OR we shouldn't check for the event
	{
		point->Control.NotFirstDataRead = 1;
		point->savedOldValue = point->Value;
		return;
	}
	
	if(point->Control.AngleValue)
	{
		tempValue = point->Value - point->savedOldValue;
		
		if(tempValue > 1800 || tempValue < -1800)		//Impossible angle differential
		{
			tempValue = point->Value;
			
			if(point->savedOldValue > 0) //value positive, other is negative
			{
				tempOld = point->savedOldValue - 3600;
			}
			else
			{
				tempOld = point->savedOldValue + 3600;
			}
		}
		else
		{
			tempValue = point->Value;
			tempOld = point->savedOldValue;
		}
	}
	else
	{
		tempValue = point->Value;
		tempOld = point->savedOldValue;
	}
		
#if DNPCustomer == DNPCustomerMemphis
	if(((tempValue <= tempOld - range) || (tempValue >= tempOld + range)) && range != 0)// point->Control.EventEnabled)
#else 
	if(((tempValue <= tempOld - range) || (tempValue >= tempOld + range)) && range != 0 && point->Control.EventEnabled)
#endif
	{
		pValue.type = TMWTYPES_ANALOG_TYPE_LONG;
		pValue.value.lval = (signed long)point->Value;
		pointNumber = point - &DNPData.AnalogInputs;
#if SDNPDATA_SUPPORT_OBJ32		
		sdnpo032_addEvent(mySclSession, pointNumber, &pValue, flags, time);
#endif
		point->savedOldValue = point->Value;
	}
}

void handleRevisionNumber(unsigned int *dataPtr)
{
	unsigned char * relayVersion = (unsigned char *)dataPtr;
	unsigned int i = 0;
	
	//DNPData.RelaySoftwareRevision = 0;
	/*
	for(i = 24; i < 32; i += 2)
	{
		DNPData.RelaySoftwareRevision *= 10;
		i++;
		//if(i != 31)
		{
			switch(relayVersion[i])
			{
				case '0':
					DNPData.RelaySoftwareRevision += 0;
					break;
				case '1':
					DNPData.RelaySoftwareRevision += 1;
					break;
				case '2':
					DNPData.RelaySoftwareRevision += 2;
					break;
				case '3':
					DNPData.RelaySoftwareRevision += 3;
					break;
				case '4':
					DNPData.RelaySoftwareRevision += 4;
					break;
				case '5':
					DNPData.RelaySoftwareRevision += 5;
					break;
				case '6':
					DNPData.RelaySoftwareRevision += 6;
					break;
				case '7':
					DNPData.RelaySoftwareRevision += 7;
					break;
				case '8':
					DNPData.RelaySoftwareRevision += 8;
					break;
				case '9':
					DNPData.RelaySoftwareRevision += 9;
					break;
				default:
					DNPData.RelaySoftwareRevision += 0;
					break;
			}
		}
		
		DNPData.RelaySoftwareRevision *= 10;
		i--;
		switch(relayVersion[i])
		{
			case '0':
				DNPData.RelaySoftwareRevision += 0;
				break;
			case '1':
				DNPData.RelaySoftwareRevision += 1;
				break;
			case '2':
				DNPData.RelaySoftwareRevision += 2;
				break;
			case '3':
				DNPData.RelaySoftwareRevision += 3;
				break;
			case '4':
				DNPData.RelaySoftwareRevision += 4;
				break;
			case '5':
				DNPData.RelaySoftwareRevision += 5;
				break;
			case '6':
				DNPData.RelaySoftwareRevision += 6;
				break;
			case '7':
				DNPData.RelaySoftwareRevision += 7;
				break;
			case '8':
				DNPData.RelaySoftwareRevision += 8;
				break;
			case '9':
				DNPData.RelaySoftwareRevision += 9;
				break;
			default:
				DNPData.RelaySoftwareRevision += 0;
				break;
		}
	}
	*/
}
/*
long convertToDNPLong(double value, double multiplier)
{
	
	value *= multiplier;
	return (long)value;	
}
*/
//Int is a 2^-12 Fixed Point value
//double getAngle(long tangent, double negativeAdjustValue)
//{
	//TEST remove all doubles
	/*
	double temp = (double)tangent * 0.00244140625d;
	double mag, phase;
	
	cordic_get_mag_phase(10.0d, temp, &mag, &phase);
	
	temp = angleFromRadians(phase, negativeAdjustValue);
	
	//Rounding
	
	temp = roundDouble(temp);
	/*
	temp = (int)mag;		//Set it to the int value
	if(abs(mag - temp) >= 0.5d)
		mag = ceil(mag);
	else
		mag = floor(mag);
	*/
//	return temp;
//}
/*
//TEST remove all double 
long getTangent(double angle)
{
	double cos, sin;
	long tan;
	
	if(angle == 90)
	{
		return 0;
	}
	
	cordic_get_cos_sin(radiansFromAngle(angle), &cos, &sin);
	
	//cordic_get_cos_sin(double desired_phase_rads, double *p_cos, double *p_sin)
	
	sin *= 4096.0;
	cos *= 4096.0;
	
	tan = DIV20_12((long)sin, (long)cos);
	
	return (long)tan;
}
*/
/*
//TEST remove all double 
double radiansFromAngle(double angle)
{
	double returnValue = 0;
	
	
	returnValue = angle * 0.017453292523928398600458869749734d;
	
	return returnValue;
}
*/
//This will assume that the LONG value being passed is double * 4096 (to represent
//12 bits of fraction.
long DIV20_12(long dividend, long divisor)
{

	unsigned long quotient = 0, highQuotient = 0;
	long temp;
	long returnVal;
	long tempDividend;
	long tempDivisor;
	char sign = 0;
	int i, j;
	
	if(dividend < 0)
	{
		tempDividend = -dividend;
		sign = 1;
	}
	else
	{
		tempDividend = dividend;
	}
	
	if(divisor < 0)
	{
		tempDivisor = -divisor;
		sign ^= 1;
	}
	else
	{
		tempDivisor = divisor;
	}
	
	for(i = 0; i < 32; ++i)
	{
		temp = tempDivisor << i;
		if(0x80000000 == (0x80000000 & temp))
		{
			break;	
		}
	}
	
	tempDivisor <<= i - 1;
	
	for(j = -i + 1; j < 32; ++j)
	{	
		temp = tempDividend - tempDivisor;
		if(temp > 0)
		{
			++quotient;
			tempDividend = temp;
		}
		tempDivisor >>= 1;
		
		if(0x80000000 == (0x80000000 & quotient))
		{
			++highQuotient;
		}
		
		quotient <<= 1;
		highQuotient <<= 1;
	}
	
	quotient >>= 20;			//fine
	highQuotient <<= 11;
	
	
	returnVal = quotient + highQuotient;
	if(sign == 1)
		returnVal = -returnVal;
	return returnVal;
};

/*
//TEST remove all double 
double roundDouble(double value)
{
	double temp;

	if(value < 0)
	{
		temp = value - (int)value;
		if(temp > -0.5d)
			temp = (int)value;
		else
			temp = (int)value - 1;
	}
	else
	{
		temp = value - (int)value;
		if(temp > 0.5d)
			temp = (int)value + 1;
		else
			temp = (int)value;
	}
	return temp;
}
double angleFromRadians(double radians, double negativeAdjustValue)
{
	double returnValue = 0;
	
	returnValue = radians * 57.2957795d;
	
	if(returnValue < 0)
	{
		returnValue += negativeAdjustValue;
	}
	
	return returnValue;
}
*/
long multiplyBy1Convert(long data)
{
	#if DNPCustomer == DNPCustomerMemphis
	//TEST remove all double long temp = roundDouble((double)data * 0.01953125d);//0.0009765625d * 20.0d * 1.0d;  2^10 * convert to percentage for 5000
	#else
	long temp = (long)10 * (long)data;
	
	//rounding
	temp >>= 11;
	temp += temp & 1; 
	temp >>= 1;
	#endif
	return temp;
}
/*
//TEST remove all double 
long dNPConvert10FractMult10(long data)
{
	long temp;
	double tempDouble = (double)data * 0.009765625d;
	 
	temp = roundDouble((double)data * 	0.009765625d);
	return temp;
}
*/
//data should be formatted -12
long multiplyBy10Convert(long data)
{
	
	//40960 10 in fixed point -12
	#if DNPCustomer == DNPCustomerMemphis
	//TEST remove all double long temp = roundDouble((double)data * 0.048828125d);//0.000244140625d * 20.0d * 10.0d;  2^12 * convert to percentage for 5000
	#else
	//TEST remove all double long temp = roundDouble((double)data * 0.002441421731824090808205713560988d);	
	#endif
	
	//TEST remove all double return temp;
	return data;
}

//data should be formatted -16
long multiplyBy100Convert(long data)
{
	
	#if DNPCustomer == DNPCustomerMemphis
	long temp = 0;
	//TEST remove all double double dTemp = (double)data * 0.030517578125;//with 20.0d included//0.00152587890625d; //convert from 2^-16 the mult by 100
	
	//100% = 5000 = 1388000 in 2^-16
	// 1/5000 = 0.0002

	//dTemp *= 20.0d; //convert to percent * 100
	
	//TEST remove all double temp = dTemp;
	
	#else
	//rounding
	long temp = (long)100 * (long)data;
	temp >>= 15;
	temp += temp & 1; 
	temp >>= 1;
	#endif
	return temp;
}

long divideBy10Convert(long data)
{
	//TEST remove all double 
	/*
	#if DNPCustomer == DNPCustomerMemphis
	double dTemp = (double)data * 20.48d; //inverse of mult10convert above.
	data = dTemp;
	#else
	
	data <<= 12;
	data /= 10;
	#endif
	*/
	return data;
}

long divideBy100Convert(long data)
{
	#if DNPCustomer == DNPCustomerMemphis
	double dTemp = (double)data * 32.768d; //inverse of mult100convert above.
	data = dTemp;
	#else
	//TEST remove all double double dTemp = roundDouble((double)data * 409.6d); //inverse of mult10convert above.
	//TEST remove all double data = dTemp;
	#endif;
	
	return data;
}

void SaveParameters(void)
{
#if DNPCustomer == DNPCustomerMemphis
	store_relay_parameters((unsigned int *)(&(MyParameters.C_word[0])), DNPData.CTRatio.Value / 5);
#elif DNPCustomer == DNPCustomerDigitalGrid
	store_relay_parameters((unsigned int *)(&(MyParameters.C_word[0])), DNPData.sCTRatio.Value / 5);
#endif
	
	convertFlashMemoryToDNPDB();
}

void ResetMyParameters(void)
{
	unsigned int i = 0;
	
	MyParameters.C_word[0] = RAM_parameters.C_word[0];
	MyParameters.C_word[1] = RAM_parameters.C_word[1];
	MyParameters.C_word[2] = RAM_parameters.C_word[2];
	MyParameters.C_word[3] = RAM_parameters.C_word[3];
	MyParameters.Mclose_word[0] = RAM_parameters.Mclose_word[0];
	MyParameters.Mclose_word[1] = RAM_parameters.Mclose_word[1];
	MyParameters.Mclose_word[2] = RAM_parameters.Mclose_word[2];
	MyParameters.Mtrip_word[0] = RAM_parameters.Mtrip_word[0];
	MyParameters.Mtrip_word[1] = RAM_parameters.Mtrip_word[1];
	MyParameters.Mtrip_word[2] = RAM_parameters.Mtrip_word[2];
	
	MyParameters.T0_word[0] = RAM_parameters.T0_word[0];
	MyParameters.T0_word[1] = RAM_parameters.T0_word[1];
	MyParameters.T0_word[2] = RAM_parameters.T0_word[2];
	MyParameters.T0_word[3] = RAM_parameters.T0_word[3];
	MyParameters.T0_word[4] = RAM_parameters.T0_word[4];
	MyParameters.T0_word[5] = RAM_parameters.T0_word[5];
	
	MyParameters.T1_word[0] = RAM_parameters.T1_word[0];
	MyParameters.T1_word[1] = RAM_parameters.T1_word[1];
	MyParameters.T1_word[2] = RAM_parameters.T1_word[2];
	MyParameters.T1_word[3] = RAM_parameters.T1_word[3];
	MyParameters.T1_word[4] = RAM_parameters.T1_word[4];
	MyParameters.T1_word[5] = RAM_parameters.T1_word[5];
	
	MyParameters.T2_word[0] = RAM_parameters.T2_word[0];
	MyParameters.T2_word[1] = RAM_parameters.T2_word[1];
	MyParameters.T2_word[2] = RAM_parameters.T2_word[2];
	MyParameters.T2_word[3] = RAM_parameters.T2_word[3];
	MyParameters.T2_word[4] = RAM_parameters.T2_word[4];
	MyParameters.T2_word[5] = RAM_parameters.T2_word[5];
	
	MyParameters.T3_word[0] = RAM_parameters.T3_word[0];
	MyParameters.T3_word[1] = RAM_parameters.T3_word[1];
	MyParameters.T3_word[2] = RAM_parameters.T3_word[2];
	MyParameters.T3_word[3] = RAM_parameters.T3_word[3];
	MyParameters.T3_word[4] = RAM_parameters.T3_word[4];
	MyParameters.T3_word[5] = RAM_parameters.T3_word[5];

	MyParameters.T4_word[0] = RAM_parameters.T4_word[0];
	MyParameters.T4_word[1] = RAM_parameters.T4_word[1];
	MyParameters.T4_word[2] = RAM_parameters.T4_word[2];
	MyParameters.T4_word[3] = RAM_parameters.T4_word[3];
	MyParameters.T4_word[4] = RAM_parameters.T4_word[4];
	MyParameters.T4_word[5] = RAM_parameters.T4_word[5];	
	
	MyParameters.relay_type = RAM_parameters.relay_type;
	MyParameters.pump_mode = RAM_parameters.pump_mode;
	MyParameters.pump_time = RAM_parameters.pump_time;
	MyParameters.protect_time = RAM_parameters.protect_time;
	MyParameters.relay_close_parameters = RAM_parameters.relay_close_parameters;
	
	
	for(i = 0; i < 49; i++)
	{	
		MyDNPSettings.Word[i] = RAM_parameters.DNP_params[i];
	}
	convertFlashMemoryToDNPDB();
}


TMWTYPES_BOOL SelectBinaryControl(void *pPoint, TMWTYPES_UCHAR value)
{
	unsigned char *point = (unsigned char *)pPoint;
	TMWTYPES_BOOL returnBool;

	if(point == &DNPData.sRemoteTrip.Value)
	{
		return TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sRelaxClose.Value)
	{
		return TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sBlockOpen.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
#if DNPCustomer == DNPCustomerDigitalGrid
	else if(point == &DNPData.sDigitalOutCntl1.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sDigitalOutCntl2.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sTripModeSensitive.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sTripModeInsensitive.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sTripModeTimeDelay.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sTripModeWattVar.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sTripModeTripOnPowerDown.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sTripModeEnableTrimCurve.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sCloseModeOverrideBlockedOnDeadNetwork.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sSequenceRelay.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sPumpModeRelayCycle.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sPumpModeRelayMotorCycle.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sPumpModeRelayMotorTimeout.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sPumpModeNeverReclose.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if(point == &DNPData.sClearPumpProtect.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if (point == &DNPData.sCloseModeCircleClose.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
	else if (point == &DNPData.sClearCycleCount.Value)
	{
		returnBool = TMWDEFS_TRUE;	
	}
#endif
#if DNPCustomer == DNPCustomerMemphis
	else if (point == &DNPData.sCaptureWaveForm.Value)
	{
		returnBool = TMWDEFS_TRUE;
	}
#endif
	else
	{
		returnBool = TMWDEFS_FALSE;
	}
	
	return returnBool;
}


TMWTYPES_BOOL OperateBinaryControl(void *pPoint, TMWTYPES_UCHAR value)
{
	unsigned char *point = (unsigned char *)pPoint;
	TMWTYPES_BOOL returnBool = TMWDEFS_FALSE;
	TMWDTIME time;
	TMWTYPES_UCHAR flags;

	if(DNPInit.InitComplete)
		flags = DNPDEFS_DBAS_FLAG_ON_LINE;
	else
		flags = DNPDEFS_DBAS_FLAG_RESTART | DNPDEFS_DBAS_FLAG_ON_LINE;

	DNPConvertRelayTimeToDNP(event_clock.long_time, DNPTimer%1000, &time);
	  	
	  
	if(point == &DNPData.sRemoteTrip.Value)
	{
		if(value == 1)//DNPData.sRemoteTrip == 1)
		{
			relay_command = trip;
			do_relay_command();
			returnBool = TMWDEFS_FALSE;
		}
	}
	else if(point == &DNPData.sBlockOpen.Value)
	{
		if(value == 1)
		{
			DNPData.BlockedOpen.Value = 1;
			DNPData.sBlockOpen.Value = 1;
			relay_command = block;
			do_relay_command();
			returnBool = TMWDEFS_TRUE;
		}
		if(value == 0)
		{
			DNPData.BlockedOpen.Value = 0;
			DNPData.sBlockOpen.Value = 0;
			relay_command = unblock;
			do_relay_command();
			returnBool = TMWDEFS_TRUE;
		}
#if DNPCustomer == DNPCustomerMemphis
		DNPData.DBlockedOpen.Value = DNPData.BlockedOpen.Value;
		DNPData.sBlockOpen.Value = DNPData.BlockedOpen.Value;
#endif
	}
	else if(point == &DNPData.sDigitalOutCntl1.Value)
	{
		if(value == 1) 
			returnBool = TMWDEFS_TRUE;
		else
			returnBool = TMWDEFS_FALSE;

		DNPData.sDigitalOutCntl1.Value = value;
	}
	else if(point == &DNPData.sDigitalOutCntl2.Value)
	{
		if(value == 1)
			returnBool = TMWDEFS_TRUE;
		else
			returnBool = TMWDEFS_FALSE;

		DNPData.sDigitalOutCntl2.Value = value;
	}
	else if(point == &DNPData.sRelaxClose.Value)
	{
		if(value == 1)
		{
			DNPData.sRelaxClose.Value			= 1;
#if DNPCustomer == DNPCustomerMemphis
			DNPData.RelaxedCloseEnabled.Value	= 1;
			if(DNPData.CircleCloseEnabled.Value)
#else
			if(DNPData.sCloseModeCircleClose.Value)
#endif
				MyParameters.Mclose_word[0] = 0x4373;
			else
				MyParameters.Mclose_word[0] = 0x4372;

			MyParameters.Mclose_word[1] = 0x0006;
			MyParameters.Mclose_word[2] = 0x0000;

			MyParameters.C_word[0]		= 0x0000;
			MyParameters.C_word[1]		= 0x0000;
			MyParameters.C_word[2]		= 0x0000;
			MyParameters.C_word[3]		= MyParameters.C_word[3];//0xFE9A;

		}
		else
		{
			DNPData.sRelaxClose.Value			= 0;
			
#if DNPCustomer == DNPCustomerMemphis
			DNPData.RelaxedCloseEnabled.Value	= 0;
			if(DNPData.CircleCloseEnabled.Value)
				MyParameters.Mclose_word[0] = 0x4343;
			else
				MyParameters.Mclose_word[0] = 0x434E;
#elif DNPCustomer == DNPCustomerDigitalGrid
			if(DNPData.sCloseModeCircleClose.Value)
				MyParameters.Mclose_word[0] = 0x4343;
			else
				MyParameters.Mclose_word[0] = 0x434E;
#endif

			MyParameters.Mclose_word[1] = MyParameters.Mclose_word[1];//0x0006;
			MyParameters.Mclose_word[2] = 0x0000;

			MyParameters.C_word[0]		= 0x1800;
			MyParameters.C_word[1]		= 0x0000;
			MyParameters.C_word[2]		= 0x0000;
			MyParameters.C_word[3]		= MyParameters.C_word[3];//0xFE9A;
		}
		SaveParameters();
#if DNPCustomer == DNPCustomerMemphis
		DNPData.RemoteClose.Value = DNPData.sRelaxClose.Value;
#endif
		returnBool				= TMWDEFS_TRUE;
	}
#if DNPCustomer == DNPCustomerMemphis
	else if(point == &DNPData.sCaptureWaveForm.Value)
	{
		if(value == 1)
		{
			make_event();
		}
		returnBool	= TMWDEFS_TRUE;
	}
#endif

#if DNPCustomer != DNPCustomerMemphis
		/*
	  	else if(point == &DNPData.sUnRelaxClose)
	  	{
	  		if(value == 1)
	  		{
	  			//Call for UnRelax Close
	  			relay_command = r_unrelax;
	  			do_relay_command();
	  			returnBool = TMWDEFS_FALSE;
	  			
				MyParameters.Mclose_word[0] = 0x434E;
				MyParameters.Mclose_word[1] = 0x0006;
				MyParameters.Mclose_word[2] = 0x0000;
				
				MyParameters.C_word[0]		= 0x1800;
				MyParameters.C_word[1]		= 0x0000;
				MyParameters.C_word[2]		= 0x0000;
				MyParameters.C_word[3]		= 0xFE9A;
	  		}
	  		SaveParameters();
	  	}
	  	*/
	  	
	  	else if(point == &DNPData.sCloseModeCircleClose.Value)
	  	{
	  		if(value == 1)
	  		{
	  			if(((MyParameters.Mclose_word[0] & 0x00FF) == 0x72)  || ((MyParameters.Mclose_word[0] & 0x00FF) == 0x73))   //if it is current a relax close
	  			{	  				
		  			MyParameters.Mclose_word[0] = MyParameters.Mclose_word[0] & 0xFF00;
		  			MyParameters.Mclose_word[0] += 0x73; //Change it to an 's' for Relax and Circle Close
	  			}
	  			else
	  			{
	  				MyParameters.Mclose_word[0] = MyParameters.Mclose_word[0] & 0xFF00;
		  			MyParameters.Mclose_word[0] += 0x43; //Change it to a 'C' for Circle Close
	  			}
	  			
	  			returnBool = TMWDEFS_TRUE;
	  		}
	  		else
	  		{
	  			if(((MyParameters.Mclose_word[0] & 0x00FF) == 0x72)  || ((MyParameters.Mclose_word[0] & 0x00FF) == 0x73))
	  			{
	  				MyParameters.Mclose_word[0] = MyParameters.Mclose_word[0] & 0xFF00;
		  			MyParameters.Mclose_word[0] += 0x72; //Changing it to an r for relax Close
	  			}
	  			else
	  			{
		  			MyParameters.Mclose_word[0] = MyParameters.Mclose_word[0] & 0xFF00;
		  			MyParameters.Mclose_word[0] += 0x4E; //Changing it to an N
	  			}
	  		}
	  		
	  		DNPData.sCloseModeCircleClose.Value = value;
#if DNPCustomer != DNPCustomerDigitalGrid
			DNPData.sCloseModeCircleClose.Value = DNPData.CircleCloseEnabled.Value = value;
#endif
			
			SaveParameters();
	  	}
	  	
	  	else if(point == &DNPData.sTripModeEnableTrimCurve.Value)
	  	{
	  		if(value == 1)//DNPData.sTrimCurveEnable == 1)
	  		{
	  			MyParameters.T1_word[0] = 0x314F;
	  			MyParameters.T1_word[1] = MyParameters.T0_word[1];
	  			
	  			MyParameters.T1_word[2] = -MyParameters.T0_word[2];  //Reverse the Angle
	  			MyParameters.T1_word[3] = 0x7FFF;	//Max pos
	  			MyParameters.T1_word[4] = 0x0000;   //0
	  			MyParameters.T1_word[5] = MyParameters.T0_word[5];
	  			
	  			//Adjust t0 word to correct range
	  			MyParameters.T0_word[3] = 0x0000;	//0 to
	  			MyParameters.T0_word[4] = 0x8000;	//Max Neg
	  			//DNPData.TrimCurveEnable = 0;
	  			returnBool = TMWDEFS_TRUE;
	  		}
	  		else
	  		{
	  			MyParameters.T1_word[0] = MyParameters.T1_word[0] & 0xFF00;
	  			MyParameters.T1_word[0] += 0x4E; //Chaning it to none or 'N'
	  			//To Full Range for t0 word
	  			MyParameters.T0_word[3] = 0x7FFF;	//Max Pos to
	  			MyParameters.T0_word[4] = 0x8000;	//Max Neg
	  			returnBool = TMWDEFS_FALSE;
	  		}
	  		
	  		DNPData.sTripModeEnableTrimCurve.Value = value;
#if DNPCustomer != DNPCustomerDigitalGrid
			DNPData.TrimCurveEnabled.Value = value;
#endif			
			SaveParameters();
	  	}
	  	
	  	/*
	  	else if(point == &DNPData.sACB)
	  	{
	  		//if(DNPData.ACB == 1)
	  		{
	  			//LSB of relay_type
	  			MyParameters.relay_type = 0xFF00 & MyParameters.relay_type;
	  			MyParameters.relay_type += value;//DNPData.sACB;
	  			//DNPData.ACB = 0;
	  			if(*point == 1)
	  			{
	  				returnBool = TMWDEFS_TRUE;
	  			}
	  			else
	  			{
	  				returnBool = TMWDEFS_FALSE;
	  			}
	  		}
	  		
			DNPData.sACB = DNPData.PhasedACB = value;
			
			SaveParameters();
	  	}
	  	*/
	  	
	  	else if(point == &DNPData.sPumpModeRelayCycle.Value)
	  	{
	  		if(value == 1)//DNPData.sRelayCyclePump == 1)
	  		{
	  			MyParameters.pump_mode = MyParameters.pump_mode | 0x0001;
	  		}
	  		else
	  		{
	  			MyParameters.pump_mode = MyParameters.pump_mode & 0xFFFE;
	  		}
	  		DNPData.sPumpModeRelayCycle.Value = value;
#if DNPCustomer != DNPCustomerDigitalGrid
			DNPData.RelayCyclePumpEnabled.Value = value;
#endif;
			
			SaveParameters();
	  	}
	  	else if(point == &DNPData.sPumpModeRelayMotorCycle.Value)
	  	{
	  		if(value == 1)//DNPData.sMotorCyclePump == 1)
	  		{
	  			MyParameters.pump_mode = MyParameters.pump_mode | 0x0004;
	  		}
	  		else
	  		{
	  			MyParameters.pump_mode = MyParameters.pump_mode & 0xFFFB;
	  		}
	  		DNPData.sPumpModeRelayMotorCycle.Value = value;
	  		
#if DNPCustomer != DNPCustomerDigitalGrid	  		
			DNPData.MotorCyclePumpEnabled.Value = value;
#endif		
			SaveParameters();
	  	}
	  	else if(point == &DNPData.sPumpModeRelayMotorTimeout.Value)
	  	{
	  		if(value == 1)
	  		{
	  			MyParameters.pump_mode = MyParameters.pump_mode | 0x0008;
	  		}
	  		else
	  		{
	  			MyParameters.pump_mode = MyParameters.pump_mode & 0xFFF7;
	  		}
	  		
	  		
			DNPData.sPumpModeRelayMotorTimeout.Value = value;
			
#if DNPCustomer != DNPCustomerDigitalGrid	  		
			DNPData.MotorTimeoutPumpEnabled.Value = value;
#endif
			
			SaveParameters();
	  	}
	  	
	  	else if(point == &DNPData.sPumpModeNeverReclose.Value)
	  	{
	  		if(value == 1)
	  			MyParameters.protect_time = 0;
	  		else
	  			MyParameters.protect_time = 15;
	  		
			DNPData.sPumpModeNeverReclose.Value = value;
#if DNPCustomer != DNPCustomerDigitalGrid			
			DNPData.NeverRecloseEnabled.Value = value;
#endif
			
			SaveParameters();
	  	}
	  	else if(point == &DNPData.sClearPumpProtect.Value)
	  	{
	  		if(value)
	  		{
	  			relay_command = clear_pump;
	  			do_relay_command();
	  			returnBool = TMWDEFS_FALSE;
	  		}
	  	}
	  	else if(point == &DNPData.sClearCycleCount.Value)
	  	{
	  		if(value == 1) 
	  		{
	  			relay_command = clear_cycle_count;
	  			do_relay_command();
				returnBool = TMWDEFS_FALSE;
	  		}
	  	}
	  	else if (point == &DNPData.sTripModeSensitive.Value)
	  	{
	  		if(value == 1)
	  		{
	  			
	  			DNPData.sTripModeSensitive.Value		= 1;
	  			DNPData.sTripModeWattVar.Value			= 0;
	  			DNPData.sTripModeTimeDelay.Value	= 0;
	  			DNPData.sTripModeInsensitive.Value	= 0;
	  			returnBool 					= TMWDEFS_TRUE;
	  			
	  			
	  			MyParameters.Mtrip_word[0] = 0x5453;  //'T' 'S'
	  			
#if DNPCustomer != DNPCustomerDigitalGrid
	  			DNPData.SensitiveModeEnabled		= 1;
	  			DNPData.WattVarModeEnabled			= 0;
	  			DNPData.TimeDelayModeEnabled		= 0;
	  			DNPData.InsensitiveModeEnabled		= 0;
#endif
	  		}
	  		else
	  		{
	  			DNPData.sTripModeSensitive.Value		= 1;
	  			DNPData.sTripModeWattVar.Value		= 0;
	  			DNPData.sTripModeTimeDelay.Value	= 0;
	  			DNPData.sTripModeInsensitive.Value	= 0;
	  			returnBool 					= TMWDEFS_FALSE;
	  			
	  			
	  			MyParameters.Mtrip_word[0] = 0x5453;  //'T' 'S'
	  	
#if DNPCustomer != DNPCustomerDigitalGrid	  			
				DNPData.SensitiveModeEnabled		= 1;
	  			DNPData.WattVarModeEnabled			= 0;
	  			DNPData.TimeDelayModeEnabled		= 0;
	  			DNPData.InsensitiveModeEnabled		= 0;
#endif
	  		}
	  		SaveParameters();	
	  	}
	  	
	  	else if (point == &DNPData.sTripModeWattVar.Value)
	  	{
	  		if(value == 1)
	  		{
	  			
	  			DNPData.sTripModeSensitive.Value		= 0;
	  			DNPData.sTripModeWattVar.Value		= 1;
	  			DNPData.sTripModeTimeDelay.Value	= 0;
	  			DNPData.sTripModeInsensitive.Value	= 0;
	  			returnBool 					= TMWDEFS_TRUE;
	  			
	  			
	  			MyParameters.Mtrip_word[0] = 0x5457;  //'T' 'W'
#if DNPCustomer != DNPCustomerDigitalGrid
				DNPData.SensitiveModeEnabled		= 0;
	  			DNPData.WattVarModeEnabled			= 1;
	  			DNPData.TimeDelayModeEnabled		= 0;
	  			DNPData.InsensitiveModeEnabled		= 0;
#endif
	  		}
	  		else
	  		{
	  			DNPData.sTripModeSensitive.Value		= 1;
	  			DNPData.sTripModeWattVar.Value		= 0;
	  			DNPData.sTripModeTimeDelay.Value	= 0;
	  			DNPData.sTripModeInsensitive.Value	= 0;
	  			returnBool 					= TMWDEFS_FALSE;
	  			
	  			
	  			MyParameters.Mtrip_word[0] = 0x5453;  //'T' 'S'
	  			
#if DNPCustomer != DNPCustomerDigitalGrid	  			
	  			DNPData.SensitiveModeEnabled		= 1;
	  			DNPData.WattVarModeEnabled			= 0;
	  			DNPData.TimeDelayModeEnabled		= 0;
	  			DNPData.InsensitiveModeEnabled		= 0;
#endif
	  		}
	  		SaveParameters();	
	  	}
	  	else if (point == &DNPData.sTripModeTimeDelay.Value)
	  	{
	  		if(value == 1)
	  		{
	  			
	  			DNPData.sTripModeSensitive.Value		= 0;
	  			DNPData.sTripModeWattVar.Value		= 0;
	  			DNPData.sTripModeTimeDelay.Value	= 1;
	  			DNPData.sTripModeInsensitive.Value	= 0;
	  			returnBool 					= TMWDEFS_TRUE;
	  			
	  			
	  			MyParameters.Mtrip_word[0] = 0x5454;  //'T' 'T'

#if DNPCustomer != DNPCustomerDigitalGrid	  			
	  			DNPData.SensitiveModeEnabled		= 0;
	  			DNPData.WattVarModeEnabled			= 0;
	  			DNPData.TimeDelayModeEnabled		= 1;
	  			DNPData.InsensitiveModeEnabled		= 0;
#endif
	  		}
	  		else
	  		{
	  			DNPData.sTripModeSensitive.Value		= 1;
	  			DNPData.sTripModeWattVar.Value		= 0;
	  			DNPData.sTripModeTimeDelay.Value	= 0;
	  			DNPData.sTripModeInsensitive.Value	= 0;
	  			returnBool 					= TMWDEFS_FALSE;
	  			
#if DNPCustomer != DNPCustomerDigitalGrid
	  			DNPData.SensitiveModeEnabled		= 1;
	  			DNPData.WattVarModeEnabled			= 0;
	  			DNPData.TimeDelayModeEnabled		= 0;
	  			DNPData.InsensitiveModeEnabled		= 0;
#endif						
	  			MyParameters.Mtrip_word[0] = 0x5453;  //'T' 'S'
	  		}
	  		SaveParameters();
	  	}
	  	else if (point == &DNPData.sTripModeInsensitive.Value)
	  	{
	  		if(value == 1)
	  		{
	  			
	  			DNPData.sTripModeSensitive.Value		= 0;
	  			DNPData.sTripModeWattVar.Value		= 0;
	  			DNPData.sTripModeTimeDelay.Value	= 0;
	  			DNPData.sTripModeInsensitive.Value	= 1;
	  			returnBool 					= TMWDEFS_TRUE;
	  			
	  			
	  			MyParameters.Mtrip_word[0] = 0x5449;  //'T' 'I'
	  			
#if DNPCustomer != DNPCustomerDigitalGrid
	  			DNPData.SensitiveModeEnabled		= 0;
	  			DNPData.WattVarModeEnabled			= 0;
	  			DNPData.TimeDelayModeEnabled		= 0;
	  			DNPData.InsensitiveModeEnabled		= 1;
#endif
	  		}
	  		else
	  		{
	  			DNPData.sTripModeSensitive.Value		= 1;
	  			DNPData.sTripModeWattVar.Value		= 0;
	  			DNPData.sTripModeTimeDelay.Value	= 0;
	  			DNPData.sTripModeInsensitive.Value	= 0;
	  			returnBool 					= TMWDEFS_FALSE;
	  			
	  			
	  			MyParameters.Mtrip_word[0] = 0x5453;  //'T' 'S'
	  			
#if DNPCustomer != DNPCustomerDigitalGrid
	  			DNPData.SensitiveModeEnabled		= 1;
	  			DNPData.WattVarModeEnabled			= 0;
	  			DNPData.TimeDelayModeEnabled		= 0;
	  			DNPData.InsensitiveModeEnabled		= 0;
#endif
	  		}
	  		SaveParameters();
	  	}
	  	else if (point == &DNPData.sTripModeTripOnPowerDown.Value)
	  	{
	  		///TEST make sure this actually saves.
	  		if(value)
	  		{
	  			DNPData.sTripModeTripOnPowerDown.Value = 1;
	  			
	  			RAM_parameters.dummy_PC_param_bytes[0] &= 0xFB;
	  			
	  			returnBool = TMWDEFS_TRUE;	
	  		}
	  		else
	  		{
	  			DNPData.sTripModeTripOnPowerDown.Value = 1;
	  			RAM_parameters.dummy_PC_param_bytes[0] |= 0x04;
	  			
	  			returnBool = TMWDEFS_FALSE;
	  		}
	  		SaveParameters();
	  	}
	  	else if(point == &DNPData.sCloseModeOverrideBlockedOnDeadNetwork.Value)
	  	{
	  		if(value)
	  		{
	  			DNPData.sCloseModeOverrideBlockedOnDeadNetwork.Value = 1;
	  			MyParameters.Mclose_word[2] |= 0x0001;
	  		}
	  		else
	  		{
	  			DNPData.sCloseModeOverrideBlockedOnDeadNetwork.Value = 0;
	  			MyParameters.Mclose_word[2] &= 0xFFFE;
	  		}
	  		SaveParameters();
	  	}
	  	else if(point == &DNPData.sSequenceRelay.Value)
		{
			MyParameters.relay_type &= 0x00FF; //clear the byte that contains the date for relay type
			if(value)
			{
				DNPData.sSequenceRelay.Value = 1;
				MyParameters.relay_type |= 0x5300; //Change to S
			}
			else
			{
				DNPData.sSequenceRelay.Value = 0;
				MyParameters.relay_type |= 0x5000; //Change to P
			}
			SaveParameters();
		}
#endif	  	
	  	else 
	  	{
			//TEST TO DO
	  		returnBool = 0;//TMWDEFS_CROB_NOT_SUPPORTED;
	  	}

	  return returnBool;
}


TMWTYPES_UCHAR SelectAnalogControlMemphis(void *pPoint, int value)
{
	TMWTYPES_UCHAR returnValue = DNPDEFS_CTLSTAT_UNDEFINED;
	AnalogPointOut *point = (AnalogPointOut *)pPoint;
	
	
#if DNPCustomer == DNPCustomerMemphis
	DNPSetNumberOfEvents();
	if(point == &DNPData.sEventNumber || point == &DNPData.sEventNumber2)
	{
		if(value > 0 && value <= 8 && value <= DNPData.NumberOfEvents.Value)
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		else
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
	}
#endif
	return returnValue;
}


TMWTYPES_UCHAR SelectAnalogControl(void *pPoint, int value)
{
	TMWTYPES_UCHAR returnValue = DNPDEFS_CTLSTAT_UNDEFINED;
	
#if DNPCustomer == DNPCustomerMemphis
	returnValue = SelectAnalogControlMemphis(pPoint, value);
#else

	AnalogPointOut *point = (AnalogPointOut *)pPoint;
	long tempLong = 0;
	int tempInt = 0;
	
	if(point == &DNPData.sCloseModeRecloseVoltage)
	{
		if(value >= 1 && value <= 100)
		{
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sCloseModeCloseTiltAngle)
	{
		if(value >= 85 && value <= 95)
		{
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sCloseModeCloseTimeDelay)
	{
		if(value >= 0 && value <= 65535)
		{
			
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sCloseModePhaseDetectionVoltage)
	{
		if(value >= 0 && value <= 4)
		{
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sCloseModePhaseDetectionAngle)
	{
		if(value >= -25 && value <= 5)
		{	
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeTimeDelayTripTimeDelay)
	{
		if(value >= 0 && value <= 9999)
		{	
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeInstantaneousTripCurrent)
	{

		if(value >= 1 && value <= 150)
		{	
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeExtendedDelay)
	{
		if(value >= 0 && value <= 255)	
		{
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeInsensitiveTripCurrent)
	{
		if(value >= 1 && value <= 150)
		{	
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeWattVarCurrent)
	{
		if(value >= 1 && value <= 150)
		{	
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeWattVarAngle)
	{
		if(value >= -80 && value <= 80)
		{

			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if (point == &DNPData.sTripModeTrimAngle)
	{
		if(value >= 85 && value <= 95)
		{

			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeTripStyle)
	{
		if(value >= 0 && value <= 2)
		{	
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeTrimAngle)
	{
		if(value >= 85 && value <= 95)
		{	
			
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeSensitiveTripCurrent)
	{
		if(value >= 1 && value <= 5000)
		{
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeSensitiveTimeDelay)
	{
		if(value >= 0 && value <= 255)
		{

			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	
	else if(point == &DNPData.sTripModeTiltAngle)
	{
		if(value >= 85 && value <= 95)
		{
			//DNPData.SensitiveTripAngle = value;
			DNPData.sTripModeTiltAngle.Value = value;
			//tempLong = getTangent(value);
			//tempLong >>= 4;
			//MyParameters.T0_word[2] = tempLong;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
		
	}
	else if(point == &DNPData.sCTRatio)
	{
		if(value >= 5 && value <= 3500)
		{
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sPhasingMode)
	{
		if(value >= 0 && value <= 2)
		{
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	/*
	else if(point == &DNPData.sPhaseCompensation)
	{

		returnValue = DNPDEFS_CTLSTAT_SUCCESS;
	}
	*/
	else if(point == &DNPData.sPumpModeRelayCycleLimit)
	{
		if(value >= 3 && value <= 20)
		{
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sPumpModeRelayCycleTime)
	{
		if(value >= 30 && value <= 300)
		{

			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sPumpModeMotorTimeout)
	{
		if(value >= 1 && value <= 25)
		{
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sPumpModeMotorCycles)
	{
		if(value >= 2 && value <= 255)
		{
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sPumpModeLockoutTime)
	{
		if(value >= 10 && value <= 120)
		{	
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else
	{
		returnValue = DNPDEFS_CTLSTAT_UNDEFINED;
	}
#endif
	return returnValue;	
}

TMWTYPES_UCHAR OperateAnalogControlMemphis(void *pPoint, int value)
{
	TMWTYPES_UCHAR returnValue = DNPDEFS_CTLSTAT_UNDEFINED;
	AnalogPointOut *point = (AnalogPointOut *)pPoint;
	long tempLong;
#if DNPCustomer == DNPCustomerMemphis
	if(point == &DNPData.sEventNumber || point == &DNPData.sEventNumber2)
	{
		if((long)value > DNPData.NumberOfEvents.Value)
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
		else
		{
			DNPData.sEventNumber.Value = value;
			DNPData.sEventNumber2.Value = value;
			DNPSetEventDateTime(DNPData.sEventNumber.Value);
			
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
	}
#endif
	return returnValue;
}

TMWTYPES_UCHAR OperateAnalogControl(void *pPoint, int value)
{
	TMWTYPES_UCHAR returnValue = DNPDEFS_CTLSTAT_UNDEFINED;
	
	AnalogPointOut *point = (AnalogPointOut *)pPoint;
	long tempLong = 0;
	int tempInt;
	
#if DNPCustomer == DNPCustomerMemphis
	returnValue = OperateAnalogControlMemphis(pPoint, value);
#else
	if(point == &DNPData.sCloseModeRecloseVoltage)
	{
		if(value >= 0 && value <= 100)
		{
			
			DNPData.sCloseModeRecloseVoltage.Value = value;	
			//DNPData.RecloseVolts = value;
		
			tempLong = DNPData.sCloseModeRecloseVoltage.Value;
			tempLong <<= 12;
			tempLong /= 10;
			MyParameters.C_word[0] = tempLong;
			//MyParameters.C_word[0] = divideBy10Convert(DNPData.RecloseVolts);
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;			
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sCloseModeCloseTiltAngle)
	{
		if(value >= 85 && value <= 95)
		{
			DNPData.sCloseModeCloseTiltAngle.Value = value;
			//DNPData.CloseTiltAngle = value;
			
			//TEST remove all Double tempLong = getTangent(value);
			tempLong >>= 4;
			MyParameters.C_word[1] = tempLong;
			//MyParameters.C_word[1] >>= 4;  //Shift by 4 for proper line up.
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sCloseModeCloseTimeDelay)
	{
		if(value >= 0 && value <= 65535)
		{
			
			DNPData.sCloseModeCloseTimeDelay.Value = value;
			//DNPData.RecloseTimeDelay = value;
			MyParameters.Mclose_word[1] = value;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sCloseModePhaseDetectionVoltage)
	{
		if(value >= 0 && value <= 4)
		{
			
			DNPData.sCloseModePhaseDetectionVoltage.Value = value;
			//DNPData.PhaseDetectionVolts = value;
			MyParameters.C_word[2] = divideBy10Convert(DNPData.sCloseModePhaseDetectionVoltage.Value);
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sCloseModePhaseDetectionAngle)
	{
		if(value >= -25 && value <= 5)
		{	
			DNPData.sCloseModePhaseDetectionAngle.Value = value;
			//DNPData.PhaseDetectionAngle = value;
			//TEST remove all double MyParameters.C_word[3] = getTangent(value);
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeTimeDelayTripTimeDelay)
	{
		if(value >= 0 && value <= 9999)
		{	
			DNPData.sTripModeTimeDelayTripTimeDelay.Value = value;
			//DNPData.TimeDelaySeconds = value;
			MyParameters.Mtrip_word[1] = value;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeInstantaneousTripCurrent)
	{
		if(value >= 1 && value <= 150)
		{	
			DNPData.sTripModeInstantaneousTripCurrent.Value = value;
			//DNPData.InstantTripCurrent = value;
			//DNPData.sInsensitiveCurrent = value;
			//DNPData.InsensitiveTripCurrent = value;
			
			tempLong = divideBy10Convert(value);
			tempLong >>= 2;
			MyParameters.T3_word[5] = (int)tempLong;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeInsensitiveTripCurrent)
	{
		if(value >= 1 && value <= 150)
		{
			//DNPData.sInstantCurrent = value;
			//DNPData.InstantTripCurrent = value;
			DNPData.sTripModeInsensitiveTripCurrent.Value = value;
			//DNPData.InsensitiveTripCurrent = value;
			
			tempLong = divideBy10Convert(value);
			tempLong >>= 2;
			MyParameters.T2_word[5] = (int)tempLong;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeWattVarCurrent)
	{
		if(value >= 1 && value <= 150)
		{
			DNPData.sTripModeWattVarCurrent.Value = value;
			//DNPData.WattVarCurrent = value;
			tempLong = divideBy10Convert(value);
			tempLong >>= 2;
			MyParameters.T4_word[5] = tempLong;
			//MyParameters.T4_word[5] = divideBy10Convert(DNPData.WattVarCurrent);
			//MyParameters.T4_word[5] >>= 2;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeWattVarAngle)
	{
		if(value >= -80 && value <= 80)
		{
			
			DNPData.sTripModeWattVarAngle.Value = value;
			//DNPData.WattVarAngle = value;
			//TEST remove all double tempLong = getTangent(value + (int)DNPData.sTripModeTiltAngle.Value);
			tempLong >>= 4;
			//MyParameters.T4_byte6 = (char)tempLong;
			//MyParameters.T4_byte5 = (char)tempLong >> 8;				
			
			MyParameters.T4_word[2] = tempLong;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if (point == &DNPData.sTripModeTrimAngle)
	{
		if(value >= 85 && value <= 95)
		{
			DNPData.sTripModeTrimAngle.Value = value;
			//DNPData.TrimCurveAngle = value;
			//TEST remove all double tempLong = getTangent(value);
			tempLong >>= 4;
			MyParameters.T1_word[2] = tempLong;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeExtendedDelay)
	{
		if(value >= 0 && value <= 255)
		{
			DNPData.sTripModeExtendedDelay.Value = value;
			//DNPData.ExtendedDelay = value;
			MyParameters.Mtrip_word[2] = MyParameters.Mtrip_word[2] & 0x00FF;
			MyParameters.Mtrip_word[2] += (value << 8);
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeSensitiveTripCurrent)
	{
		//10 mA steps
		if(value >= 1 && value <= 5000)
		{
			
			DNPData.sTripModeSensitiveTripCurrent.Value = value;
			//DNPData.SensitiveTripSetting = value;
			tempLong = divideBy100Convert(value);
			tempLong = -tempLong;
			MyParameters.T0_word[1] = (signed int)tempLong;
			MyParameters.T0_word[5] = (signed int)(tempLong >> 16);
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeSensitiveTimeDelay)
	{
		if(value >= 0 && value <= 255)
		{
			DNPData.sTripModeSensitiveTimeDelay.Value = value;
			//DNPData.SensitiveTripDelay = value;
			MyParameters.Mtrip_word[2] = MyParameters.Mtrip_word[2] & 0xFF00;
			MyParameters.Mtrip_word[2] += value;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	
	else if(point == &DNPData.sTripModeTiltAngle)
	{
		if(value >= 85 && value <= 95)
		{
			DNPData.sTripModeTiltAngle.Value = value;
			//DNPData.sSensitiveTiltAngle = value;
			// TEST remove all double tempLong = getTangent(value);
			tempLong >>= 4;
			MyParameters.T0_word[2] = tempLong;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
		
	}
	
	else if(point == &DNPData.sCTRatio)
	{
		if(value >= 5 && value <= 3500)
		{
			DNPData.sCTRatio.Value = value;
			//DNPData.CTRatio = value;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	/*
	else if(point == &DNPData.sPhaseCompensation)
	{
		DNPData.sPhaseCompensation = value;
		DNPData.PhaseCompensation = value;
		returnValue = DNPDEFS_CTLSTAT_SUCCESS;
	}
	*/
	else if(point == &DNPData.sPumpModeRelayCycleLimit)
	{
		if(value >= 3 && value <= 20)
		{
			DNPData.sPumpModeRelayCycleLimit.Value = value;
			//DNPData.PumpRelayCycleLimit = value;
			MyParameters.pump_mode = MyParameters.pump_mode & 0x00FF;
			MyParameters.pump_mode += (value << 8);
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sPumpModeRelayCycleTime)
	{
		if(value >= 30 && value <= 300)
		{
			DNPData.sPumpModeRelayCycleTime.Value = value;
			//DNPData.PumpRelayTimeLimit = value;
			MyParameters.pump_time = (value << 2);
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sPumpModeMotorTimeout)
	{
		if(value >= 1 && value <= 25)
		{
			DNPData.sPumpModeMotorTimeout.Value = value;
			//DNPData.PumpMotorTimeout = value;
			value *= 10;
			MyParameters.relay_close_parameters = MyParameters.relay_close_parameters & 0x00FF;
			MyParameters.relay_close_parameters += (value << 8);
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sPumpModeMotorCycles)
	{
		if(value >= 2 && value <= 255)
		{
			
			DNPData.sPumpModeMotorCycles.Value = value;
			//DNPData.PumpMotorCycleLimit = value;
			MyParameters.relay_close_parameters = MyParameters.relay_close_parameters & 0xFF00;
			MyParameters.relay_close_parameters += value;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sPumpModeLockoutTime)
	{
		if(value >= 10 && value <= 120)
		{	
			DNPData.sPumpModeLockoutTime.Value = value;
			//DNPData.PumpProtectTime = value;
			MyParameters.protect_time = value;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sPhasingMode)
	{
		if(value >= 0 && value <= 2)
		{	
			DNPData.sPhasingMode.Value = value;
			
			MyParameters.relay_type &= 0xFF00;
			MyParameters.relay_type |= value;
			
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	else if(point == &DNPData.sTripModeTripStyle)
	{
		if(value >= 0 && value <= 2)
		{	
			DNPData.sTripModeTripStyle.Value = value;
			RAM_parameters.dummy_PC_param_bytes[0] = value;
			returnValue = DNPDEFS_CTLSTAT_SUCCESS;
		}
		else
		{
			returnValue = DNPDEFS_CTLSTAT_OUT_OF_RANGE;
		}
	}
	
	SaveParameters();
#endif
	return returnValue;	
}

void DNP_command(unsigned int *dataPtr)
{
	unsigned char tempChar = (unsigned char)*dataPtr;
	
	
	switch(tempChar)
	{
		case 'A':	//Self Address
			setSelfAddress(dataPtr);
			break;
		case 'a':
			sdnpsesn_closeSession(mySclSession);
			setAll(dataPtr);
			mySclSession = (TMWSESN *)sdnpsesn_openSession(mySclChannel, &mySesnConfig, &myUserContext);
			break;
		case 'D':
			setDestinationAddress(dataPtr);
			break;
		case 'd':
			setDeadBandLimits(dataPtr);
			break;
		case 'e':
			setBinaryEventEnables(dataPtr);
			break;
		case 'E':
			setAnalogEventEnables(dataPtr);
			break;
		case 'F':
			setFragmentSize(dataPtr);
			break;
		case 'L':	//For link layer confirm
			setLinkLayerConfirm(dataPtr);
			break;
		case 'M':
			setMaxEvents(dataPtr);
			break;
		case 'R':
			setUnsolRetries(dataPtr);
			break;
		case 'S':
			setSourceAddress(dataPtr);
			break;
		case 'T':
			setUnsolTimeout(dataPtr);
			break;
		case 't':
			setTerminationResistor(dataPtr);
			break;
		case 'U':	//Unsolicited
			setUnsolicited(dataPtr);
			break;
		default:
			break;
	}
	
	
}

void setBinaryEventEnables(unsigned int *dataPtr)
{
	unsigned int tempI;
	
	tempI = *dataPtr;
	tempI >>= 8;
	
	MyDNPSettings.BinaryEventEnables[0] = tempI & 0xFF;
	
	dataPtr++;
	tempI = *dataPtr;
	MyDNPSettings.BinaryEventEnables[1] = tempI & 0xFF;
	tempI >>= 8;
#if DNPCustomer == DNPCustomerMemphis
	MyDNPSettings.BinaryEventEnables[2] = tempI & 0xFF;

	dataPtr++;

	tempI = *dataPtr;
	MyDNPSettings.BinaryEventEnables[3] = tempI & 0xFF;
	tempI >>= 8;
	MyDNPSettings.BinaryEventEnables[4] = tempI & 0xFF;
	
	dataPtr++;
	tempI = *dataPtr;
	MyDNPSettings.BinaryEventEnables[5] = tempI & 0xFF;
#endif
	
	DNPInitializeBinaryEventsFromMemory();
	SaveDNPSettings();
}

void setAnalogEventEnables(unsigned int *dataPtr)
{
	unsigned int tempI;
	
	tempI = *dataPtr;
	tempI >>= 8;
	
	MyDNPSettings.AnalogEventEnables[0] = tempI & 0xFF;
	
	dataPtr++;
	
	tempI = *dataPtr;
	MyDNPSettings.AnalogEventEnables[1] = tempI & 0xFF;
	tempI >>= 8;
	MyDNPSettings.AnalogEventEnables[2] = tempI & 0xFF;

	dataPtr++;
	
	tempI = *dataPtr;
	MyDNPSettings.AnalogEventEnables[3] = tempI & 0xFF;
	tempI >>= 8;
	MyDNPSettings.AnalogEventEnables[4] = tempI & 0xFF;

	dataPtr++;
	tempI = *dataPtr;
	MyDNPSettings.AnalogEventEnables[5] = tempI & 0xFF;
	tempI >>= 8;
	MyDNPSettings.AnalogEventEnables[6] = tempI & 0xFF;

	dataPtr++;
	tempI = *dataPtr;
	MyDNPSettings.AnalogEventEnables[7] = tempI & 0xFF;
	tempI >>= 8;
	MyDNPSettings.AnalogEventEnables[8] = tempI & 0xFF;

	dataPtr++;
	DNPInitializeAnanlogEventsFromMemory();
	SaveDNPSettings();
}

void setDeadBandLimits(unsigned int *dataPtr)
{
#if DNPCustomer == DNPCustomerMemphis
	dataPtr++;	
	
	MyDNPSettings.DeadBandVoltage = *dataPtr;
	dataPtr++;
	
	MyDNPSettings.DeadBandTHD = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandCurrent = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandTemperature = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandOdometer = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandRMSDifferentialVoltage = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandRealDifferentialVoltage = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandCurrentAngle = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandPhasekW = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandPhasekVAR = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandPhasekVA = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandTotalkW = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandTotalkVAkVAR = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandAnalog1 = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandAnalog2 = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandAnalog3 = *dataPtr;
	dataPtr++;

	MyDNPSettings.DeadBandAnalog4 = *dataPtr;
	dataPtr++;
#else //TEST to do
	unsigned int iChar = 0;
	MyDNPSettings.DBSettings[0] = (char)(*dataPtr >> 8);	//This first one done because first byte share word with OPCODE
	dataPtr++;
	for(iChar = 1; iChar < 38; dataPtr++)
	{
		MyDNPSettings.DBSettings[iChar++] = (char)*dataPtr;
		MyDNPSettings.DBSettings[iChar++] = (char)(*dataPtr >> 8);
	}
#endif;	
	SaveDNPSettings();
	initializeDNPVariables();
}

void setAll(unsigned int *dataPtr)
{
	int temp;
	
	dataPtr++;
	
	MyDNPSettings.ControlWord.word = *dataPtr;
	
	dataPtr++;
	
	MyDNPSettings.UnsolTimeout = *dataPtr;
	MyDNPSettings.UnsolTimeout <<= 16;
	dataPtr++;
	MyDNPSettings.UnsolTimeout += *dataPtr;
	
	dataPtr++;
	MyDNPSettings.FragmentSize = *dataPtr;
	
	dataPtr++;
	MyDNPSettings.Destination = *dataPtr;
	
	dataPtr++;
	MyDNPSettings.Source = *dataPtr;
#if DNPCustomer == DNPCustomerMemphis
	DNPData.DeviceAddress.Value = *dataPtr;
#endif
	
	dataPtr++;
	MyDNPSettings.UnsolMaxRetries = *dataPtr;
	
	dataPtr++;
	MyDNPSettings.MaxEvents = (char)*dataPtr;
	
	SaveDNPSettings();
	
	initializeSesnConfig();
	initializeDNPConfig();
}

void setTerminationResistor(unsigned int *dataPtr)
{
	dataPtr++;
	

	if(*dataPtr == 0)
	{
		//disable
		GPIOC_Data_reg.bit.DB5 = 1;
		MyDNPSettings.ControlWord.TerminationResistor = 0;
		
	}
	else
	{	
		//enable
		GPIOC_Data_reg.bit.DB5 = 0;
		MyDNPSettings.ControlWord.TerminationResistor = 1;
	}

	SaveDNPSettings();
}

void setUnsolRetries(unsigned int *dataPtr)
{
	dataPtr++;
	if(*dataPtr == 0)
	{
		//Make infinite
		mySesnConfig.unsolOfflineRetryDelay = mySesnConfig.unsolRetryDelay;//infinite retries
	}
	else
	{	
		mySesnConfig.unsolMaxRetries = *dataPtr;
	}
	MyDNPSettings.UnsolMaxRetries = *dataPtr;
	
	SaveDNPSettings();
}

void setMaxEvents(unsigned int *dataPtr)
{
	dataPtr++;
	MyDNPSettings.MaxEvents = *dataPtr;
	mySesnConfig.unsolClass1MaxEvents = *dataPtr;
	mySesnConfig.unsolClass2MaxEvents = *dataPtr;
	mySesnConfig.unsolClass3MaxEvents = *dataPtr;
	
	SaveDNPSettings();
}

//master address
void setDestinationAddress(unsigned int *dataPtr)
{
	dataPtr++;
	mySesnConfig.destination = MyDNPSettings.Destination = *dataPtr;

	if(mySesnConfig.destination == 0)
		mySesnConfig.destination = MyDNPSettings.Destination = 3;
	
	SaveDNPSettings();
}

//relay address
void setSourceAddress(unsigned int *dataPtr)
{
	
	dataPtr++;
	mySesnConfig.source = MyDNPSettings.Source = *dataPtr;
	if(mySesnConfig.source == 0)
		mySesnConfig.source = MyDNPSettings.Source = 4;
	
	SaveDNPSettings();
}
	

void setFragmentSize(unsigned int *dataPtr)
{
	dataPtr++;
	MyDNPSettings.FragmentSize = *dataPtr;
	
	dnpchnl_closeChannel(mySclChannel);
	
	//myDNPConfig.rxFragmentSize = MyDNPSettings.FragmentSize;
	myDNPConfig.txFragmentSize = MyDNPSettings.FragmentSize;
	
	mySclChannel = dnpchnl_openChannel(myApplContext, &myDNPConfig, &myTprtConfig,
	&myLinkConfig, &myPhysConfig, &myIOCnfg, &myTargConfig);
	
	SaveDNPSettings();
}

void setUnsolTimeout(unsigned int *dataPtr)
{
	
	dataPtr++;
	MyDNPSettings.UnsolTimeout = *dataPtr;
	mySesnConfig.applConfirmTimeout = *dataPtr;
	MyDNPSettings.UnsolTimeout <<= 16;
	mySesnConfig.applConfirmTimeout <<= 16;
	
	
	dataPtr++;
	MyDNPSettings.UnsolTimeout += *dataPtr;
	mySesnConfig.applConfirmTimeout += *dataPtr;
	
	SaveDNPSettings();
	
}

void setLinkLayerConfirm(unsigned int *dataPtr)
{
	unsigned char tempChar = (unsigned char)(*dataPtr >> 8);
	
	switch(tempChar)
	{
		case 'A':		//Always
			MyDNPSettings.ControlWord.LinkLayerConfirm = 2;
			myLinkConfig.confirmMode = TMWDEFS_LINKCNFM_ALWAYS;
			break;
		case 'S':		//Sometimes
			MyDNPSettings.ControlWord.LinkLayerConfirm = 1;
			myLinkConfig.confirmMode = TMWDEFS_LINKCNFM_SOMETIMES;
			break;
		case 'N':		//Never
			MyDNPSettings.ControlWord.LinkLayerConfirm = 0;
			myLinkConfig.confirmMode = TMWDEFS_LINKCNFM_NEVER;
		default:
			break;
	}
	SaveDNPSettings();
}

void setSelfAddress(unsigned int *dataPtr)
{
	unsigned char tempChar = (unsigned char)(*dataPtr >> 8);
	
	switch(tempChar)
	{
		case 'D':	//Disable
			MyDNPSettings.ControlWord.SelfAddress = 0;
			mySesnConfig.enableSelfAddress = TMWDEFS_FALSE;
			break;
		case 'E':	//Enable
			MyDNPSettings.ControlWord.SelfAddress = 1;
			mySesnConfig.enableSelfAddress = TMWDEFS_TRUE;
			break;
	}
	SaveDNPSettings();
}

void setUnsolicited(unsigned int *dataPtr)
{
	unsigned char tempChar = (unsigned char)(*dataPtr >> 8);

	switch(tempChar)
	{
		case 'D':	//Disable
			MyDNPSettings.ControlWord.UnsolicitedAllowed = 0;
			mySesnConfig.unsolAllowed = TMWDEFS_FALSE;
			break;
		case 'E':	//Enable
			MyDNPSettings.ControlWord.UnsolicitedAllowed = 1;
			mySesnConfig.unsolAllowed = TMWDEFS_TRUE;
			break;
	}
	SaveDNPSettings();
}

void SaveDNPSettings()
{
	//49 words to pass

	store_DNP_parameters((unsigned int *)&(MyDNPSettings.Word[0]));
}


/*Description: 
	32 bit multiplication of two FIXED16_16 variables
  Inputs:
  	2 FIXED16_16 variables
  Outputs:
  	1 FIXED16_16 variable
*/
union FIXED16_16 MULT16_16(union FIXED16_16 a, union FIXED16_16 b)
{
	union FIXED16_16 returnVal;
	int sign = 0;
	unsigned int aL, aH, bL, bH;
	unsigned long lowR, midR1, midR2, highR;
	
	if(a.Full16 < 0)
	{
		sign = 1;
		a.Full16 = -a.Full16;
	}
	if(b.Full16 < 0)
	{
		sign ^= 1;
		b.Full16 = -b.Full16;
	}
	
	aL = a.Full16 & 0xFFFF;
	bL = b.Full16 & 0xFFFF;
	aH = a.Full16 >> 16;
	bH = b.Full16 >> 16;
	
	lowR	= (long)aL * (long)bL;
	midR1	= (long)aL * (long)bH;
	midR2	= (long)aH * (long)bL;
	highR	= (long)aH * (long)bH;
	
	returnVal.Full16 = (lowR >> 16) + (midR1) + (midR2) + (highR << 16);
	
	if(sign == 1)
		returnVal.Full16 = -returnVal.Full16;
	
	return returnVal;
};

//Converts a DNP Map point number to the proper UNCONVERTED address of the data.  Will get converted during the read

int * DNPEventPoint(unsigned short point)
{
	#if DNPCustomer == DNPCustomerMemphis
	int offset;			//All 9 signals are sampled at the same time and put in consecutive memory locations, 
	int cycleNumber, totalPoints;	//which cycle we are working with
	union FIXED16_16 temp;
	union recorderevent *workingEvent = selectedEvent(DNPData.sEventNumber.Value);

	
	
	//Calculate Offset
	if(point < 2420)
	{
		offset = 0;  //VnA
		point -= 500;
	}
	else if(point < 4340)
	{
		offset = 1;
		point -= 2420;
	}
	else if(point < 6260)
	{
		offset = 2;
		point -= 4340;
	}
	else if(point < 8180)
	{
		offset = 3;
		point -= 6260;
	}
	else if(point < 10100)
	{
		offset = 4;
		point -= 8180;
	}
	else if(point < 12020)
	{
		offset = 5;
		point -= 10100;
	}
	else if(point < 13940)
	{
		offset = 6;
		point -= 12020;
	}
	else if(point < 15860)
	{	
		offset = 7;
		point -= 13940;
	}
	else
	{
		offset = 8;
		point -= 15860;
	}
	
	//starting point
	//1152 number of stored samples of EVERY signal in 1 cycle (128 * 9)
	//startingPoint = (8 - DNPData.NumberOfCyclesEvent) * 1152;
	

	temp.Full16 = ((long)point) << 16;
	temp = MULT16_16(temp, scalingFactor);
	
	point = (int)temp.Integer16;
	point *= 9;
	point += eventStartingPoint + offset;
	
	return (int *)&(workingEvent->data_word[point]);
	
	#endif
}

//Takes the address of the point in the event_recorder and converts it to the right data.
//Must determine what type of data it is looking at
int DNPEventData(int * point)
{
	//data_word is 9 consecutive samples VnABC, VtABC, IABC
	//Voltage (as done by GUI- will need modify)
	//	>>= 3 for DAC
	//	mult by const
	//	>>= 10 for conversion to 
	//Current (as done by GUI- will need modify)
	//	>>= 3 for DAC
	//	mult by const
	//	>>= 12 for conversion to
	
	//will need to be converted
	int returnVal;
	
	switch(signalSwitch(point))
	{
		case 0:
		default:
			returnVal = convertNetworkVoltageReading(*point, &RAM_parameters.Cal_data[0]);
			break;
		case 1:
			returnVal = convertNetworkVoltageReading(*point, &RAM_parameters.Cal_data[2]);
			break;
		case 2:
			returnVal = convertNetworkVoltageReading(*point, &RAM_parameters.Cal_data[4]);
			break;
		case 3:
			returnVal = convertDifferentialVoltageReading(point, &RAM_parameters.Cal_data[0], &RAM_parameters.Cal_data[6]);
			break;
		case 4:
			returnVal = convertDifferentialVoltageReading(point, &RAM_parameters.Cal_data[2], &RAM_parameters.Cal_data[8]);
			break;
		case 5:
			returnVal = convertDifferentialVoltageReading(point, &RAM_parameters.Cal_data[4], &RAM_parameters.Cal_data[10]);		//Skip 3 sets of 2 here for Difference Voltage
			break;
		case 6:
			returnVal = convertCurrentReading(*point, &RAM_parameters.Cal_data[18]);
			break;
		case 7:
			returnVal = convertCurrentReading(*point, &RAM_parameters.Cal_data[20]);
			break;
		case 8:
			returnVal = convertCurrentReading(*point, &RAM_parameters.Cal_data[22]);
			break;
	}
	return returnVal;
}

void DNPSetNumberOfEvents(void)
{
#if DNPCustomer == DNPCustomerMemphis
	union recorderevent *eventPtr;  
	int i = 0, numOfEvents = 0;
	
	for(i = 0; i < 8; i++)
	{
		eventPtr = &recorder_event[i];
		
		if(eventPtr->time != 0x00AE00EE && eventPtr->time != 0xFFFFFFFF) //Valid event, not sure why 00AE00EE 
		{
			numOfEvents++;
		}
	}
	
	DNPData.NumberOfEvents.Value = numOfEvents;
#endif
}

void DNPSetEventDateTime(long eventNumber)
{
	union recorderevent * workingEvent = selectedEvent(eventNumber);
	
	DNPConvertRelayTimeToMemphisDNPData(workingEvent->time, 0);
}

//Returns requested event, 1 - 8
//event 0 is not necessarily at event_recorder[0] . . . last_event_ptr points to most recent event (which would be 0)
//so must count around buffer to find correct one
union recorderevent * selectedEvent(int eventNumber)
{
#if DNPCustomer == DNPCustomerMemphis
	int i = 0;
	union recorderevent * eventPtr = last_event_ptr;
	
	//eventNumber = 8 - eventNumber;
	
	if(eventNumber > 8 || eventNumber > DNPData.NumberOfEvents.Value)
		return NULL;
	
	for(i = 0; i < eventNumber - 1; ++i) 			//iterate backwards because they are in the buffer backwards
	{
		eventPtr--;
		if(eventPtr < &recorder_event[0])		//if we moved past the first event in the buffer
			eventPtr = &recorder_event[7];
	}
	
	return eventPtr;
#endif
}

///External Point Handlers

void DNPCheckExternalDigitalFlags(TMWDTIME *time)
{
	TMWTYPES_UCHAR flags;

	if(DNPInit.InitComplete)
		flags = DNPDEFS_DBAS_FLAG_ON_LINE;
	else
		flags = DNPDEFS_DBAS_FLAG_RESTART | DNPDEFS_DBAS_FLAG_ON_LINE;
	
	checkBinaryEvent(&DNPData.DigitalIn1, DNPData.DigitalIn1.Value, current.flag.B, flags, time);
	checkBinaryEvent(&DNPData.DigitalIn2, DNPData.DigitalIn2.Value, current.flag.A, flags, time);
	checkBinaryEvent(&DNPData.DigitalIn3, DNPData.DigitalIn3.Value, current.flag.C, flags, time);
	checkBinaryEvent(&DNPData.DigitalIn4, DNPData.DigitalIn4.Value, current.flag.D, flags, time);
	
	DNPData.DigitalIn1.Value = current.flag.B; //Protector Status
	DNPData.BFlag.Value = DNPData.DigitalIn1.Value;
	DNPData.DigitalIn2.Value = current.flag.A; //Pressure
	DNPData.DigitalIn3.Value = current.flag.C; //Water in Protector
	DNPData.DigitalIn4.Value = current.flag.D; //Water in Vault/Door Laram / Vault Pump On
	
	DNPInit.FirstExternalPointRead = 1;
}

void DNPCheckExternalAnalogs(TMWDTIME *time)
{
	TMWTYPES_UCHAR flags;
	
	if(DNPInit.InitComplete)
		flags = DNPDEFS_DBAS_FLAG_ON_LINE;
	else
		flags = DNPDEFS_DBAS_FLAG_RESTART | DNPDEFS_DBAS_FLAG_ON_LINE;
	
#ifdef DG6000_1
	//TEST check to see if this switch actually switched it in
	DNPData.Aux1.Value = DNPConvertExternalAnalog(DG6000_analog1);
	checkVariableAnalogEvent(&DNPData.Aux1, flags, time, EventTriggerRange.Analog1);
	DNPData.Aux2.Value = DNPConvertExternalAnalog(DG6000_analog2);
	checkVariableAnalogEvent(&DNPData.Aux2, flags, time, EventTriggerRange.Analog2);
	DNPData.Aux3.Value = DNPConvertExternalAnalog(DG6000_analog3);
	checkVariableAnalogEvent(&DNPData.Aux3, flags, time, EventTriggerRange.Analog3);
	DNPData.Aux4.Value = DNPConvertExternalAnalog(DG6000_analog4);
	checkVariableAnalogEvent(&DNPData.Aux4, flags, time, EventTriggerRange.Analog4);
#endif
}

long DNPConvertExternalAnalog(int workingCurrent)
{
	// 5 volts measured 0x0120
	// 0 volts measured 0x3E80
	//-5 volts measured 0x7C60
	
	//TEST remove all double double conversion = 0.03147d; //conversion rate x 100
	
	if(workingCurrent > 0x3E80)
		workingCurrent = 0x3E80 - workingCurrent;
	else
		workingCurrent = -(workingCurrent - 0x3E80);
	
	//TEST remove all double conversion *= (double)workingCurrent;
	
	//TEST remove all double return (long)conversion;
	
	return workingCurrent;
	
}



//Convert Raw ADC Voltage reads to Voltage for DNP
//inputs
//int data - raw data
//unsigned int * calPtr - pointer to the first word of the calibration for this signal
//output
//returnVal - Voltage converted and formatted for DNP

int convertNetworkVoltageReading(int data, unsigned int * calPtr)
{

	union FIXED16_16 calibrationConstant, dataVal, ten;
	int returnVal;
	
	calibrationConstant.Full16 = (long)*calPtr;
	calibrationConstant.Full16 <<= 16;
	calPtr++;
	calibrationConstant.Full16 += (long)*calPtr;
	calibrationConstant.Full16 <<= 4;			//This shift is because the value is actually in 20_12 and not 16_16
	
	data >>= 3;					//shift due to nature of the DAC
	
	if(data >= 0x0800)
		data = -data;
	
	
	dataVal.Full16 = data;
	
	dataVal = MULT16_16(dataVal, calibrationConstant);
	
	dataVal.Full16 <<= 6;		//Not sure why, but this is how much it should be shifted now
	
	//Scale by 10 for DNP
	ten.Full16 = 0;
	ten.Integer16 = 10;		
	dataVal = MULT16_16(dataVal, ten);
	
	returnVal = (int)dataVal.Integer16;
	
	return returnVal;
}

int convertDifferentialVoltageReading(int * data, unsigned int * calPtrN, unsigned int * calPtrT)
{

	union FIXED16_16 calibrationConstant, dataVal, hundred, tempNetwork, tempTransformer;
	int returnVal;
	int tempData;
	
	calibrationConstant.Full16 = (long)*calPtrN;
	calibrationConstant.Full16 <<= 16;
	calPtrN++;
	calibrationConstant.Full16 += (long)*calPtrN;
	calibrationConstant.Full16 <<= 4;			//This shift is because the value is actually in 20_12 and not 16_16
	
	tempData = (*(data - 3)) >> 3;					//shift due to nature of the DAC - and get Network
	
	if(tempData >= 0x0800)
		tempData = -tempData;
	
	tempNetwork.Full16 = tempData;
	
	tempNetwork = MULT16_16(tempNetwork, calibrationConstant);
	tempNetwork.Full16 <<= 6;
	
	calibrationConstant.Full16 = (long)*calPtrT;
	calibrationConstant.Full16 <<= 16;
	calPtrT++;
	calibrationConstant.Full16 += (long)*calPtrT;
	calibrationConstant.Full16 <<= 4;			//This shift is because the value is actually in 20_12 and not 16_16
	
	
	tempData = (*data) >> 3;					//shift due to nature of the DAC - and get Network
	
	if(tempData >= 0x0800)
		tempData = -tempData;

	tempTransformer.Full16 = tempData;
	
	tempTransformer = MULT16_16(tempTransformer, calibrationConstant);
	tempTransformer.Full16 <<= 6;		//Not sure why, but this is how much it should be shifted now
	
	dataVal.Full16 = tempTransformer.Full16 - tempNetwork.Full16;
	
	//Scale by 100 for DNP
	hundred.Full16 = 0;
	hundred.Integer16 = 100;		
	dataVal = MULT16_16(dataVal, hundred);
	
	returnVal = (int)dataVal.Integer16;
	
	return returnVal;
}


int convertCurrentReading(int data, unsigned int * calPtr)
{

	union FIXED16_16 calibrationConstant, dataVal, sixtyfourhundred, CTRatio;
	int returnVal;
	static unsigned int highCount = 0, lowCount = 0;
		
	calibrationConstant.Full16 = (long)*calPtr;
	calibrationConstant.Full16 <<= 16;
	calPtr++;
	calibrationConstant.Full16 += (long)*calPtr;

	data >>= 3;					//shift due to nature of the DAC

	dataVal.Full16 = data;
	
	
	if(dataVal.Integer16 < 0)
		dataVal.Integer16 = dataVal.Integer16;
	dataVal = MULT16_16(dataVal, calibrationConstant);

	CTRatio.Full16 = 0;
#if DNPCustomer == DNPCustomerMemphis
	//CTRatio.Integer16 = DNPData.CTRatio.Value / 5;
#elif DNPCustomer == DNPCustomerDigitalGrid
	CTRatio.Integer16 = DNPData.sCTRatio.Value / 5;
#endif
	
	//dataVal = MULT16_16(dataVal, CTRatio);
	
	sixtyfourhundred.Full16 = 0;
	sixtyfourhundred.Integer16 = 1000;//TEST 6400;		
	
	
	if(dataVal.Integer16 <= 5 && dataVal.Integer16 >= -5)
	{
		//Scale by 6400 for DNP
		
		dataVal = MULT16_16(dataVal, sixtyfourhundred);
		
		dataVal.Full16 >>= 8;
		
		returnVal = (int)dataVal.Full16;
		lowCount++;
	}
	else
	{
		dataVal.Full16 >>= 11;				//(max value) 9000 >> 11 puts us under 5
		dataVal = MULT16_16(dataVal, sixtyfourhundred);
		dataVal.Full16 <<= 3;
		
		returnVal = (int)dataVal.Full16;
		highCount++;
	}
	
	return returnVal;
}

unsigned int signalSwitch(int * point)
{
#if DNPCustomer == DNPCustomerMemphis
	unsigned long difference;
	union recorderevent * workingEvent = selectedEvent(DNPData.sEventNumber.Value);
	
	difference = (unsigned int *)point - &(workingEvent->data_word[0]);
	
	return difference%9;
#endif
}

unsigned int InEventRegion(int * dataPoint)
{
	if
	(
		((unsigned int *)dataPoint >= recorder_event[0].data_word && (unsigned int *)dataPoint < &recorder_event[0].data_word[18432]) ||
		((unsigned int *)dataPoint >= recorder_event[1].data_word && (unsigned int *)dataPoint < &recorder_event[1].data_word[18432]) ||
		((unsigned int *)dataPoint >= recorder_event[2].data_word && (unsigned int *)dataPoint < &recorder_event[2].data_word[18432]) ||
		((unsigned int *)dataPoint >= recorder_event[3].data_word && (unsigned int *)dataPoint < &recorder_event[3].data_word[18432]) ||
		((unsigned int *)dataPoint >= recorder_event[4].data_word && (unsigned int *)dataPoint < &recorder_event[4].data_word[18432]) ||
		((unsigned int *)dataPoint >= recorder_event[5].data_word && (unsigned int *)dataPoint < &recorder_event[5].data_word[18432]) ||
		((unsigned int *)dataPoint >= recorder_event[6].data_word && (unsigned int *)dataPoint < &recorder_event[6].data_word[18432]) ||
		((unsigned int *)dataPoint >= recorder_event[7].data_word && (unsigned int *)dataPoint < &recorder_event[7].data_word[18432])
	)
		return 1;
	else
		return 0;
}

unsigned int DNPVariableAnalogPoint(AnalogPointIn * dataPoint)
{
	if((dataPoint >= &DNPData.AnalogInputs) & (dataPoint <= &DNPData.AnalogInputs + 100))
		return 1;
	else
		return 0;
}

union FIXED16_16 DIV16_16(union FIXED16_16 dividend, union FIXED16_16 divisor)
{

	unsigned long quotient = 0, highQuotient = 0;
	//long remainder;
	long temp;//, temp2;
	union FIXED16_16 returnVal;
	long tempDividend;
	long tempDivisor;
	char sign = 0;
	int i, j;
	
	if(dividend.Full16 < 0)
	{
		tempDividend = -dividend.Full16;
		sign = 1;
	}
	else
	{
		tempDividend = dividend.Full16;
	}
	
	if(divisor.Full16 < 0)
	{
		tempDivisor = -divisor.Full16;
		sign ^= 1;
	}
	else
	{
		tempDivisor = divisor.Full16;
	}
	
	for(i = 0; i < 32; ++i)
	{
		temp = tempDivisor << i;
		if(0x80000000 == (0x80000000 & temp))
		{
			break;	
		}
	}
	
	tempDivisor <<= i - 1;
	
	for(j = -i + 1; j < 32; ++j)
	{	
		temp = tempDividend - tempDivisor;
		if(temp > 0)
		{
			++quotient;
			tempDividend = temp;
		}
		tempDivisor >>= 1;
		
		if(0x80000000 == (0x80000000 & quotient))
		{
			++highQuotient;
		}
		
		quotient <<= 1;
		highQuotient <<= 1;
	}
	
	quotient >>= 16;			//These two shifts are what need to be edited to change
	highQuotient <<= 15;		//for a different fixed point
	
	
	returnVal.Full16 = quotient + highQuotient;
	if(sign == 1)
		returnVal.Full16 = -returnVal.Full16;
	return returnVal;
};

char * ConvertSerialNumberToString(unsigned int number)
{
	int i = 0, j = 0;
	unsigned int temp;
	
	char returnString[5];
	
	for	(i = 10000, j = 0; i >= 0; i /= 10, j++)
	{
		if(temp != 0)
			temp = number / i;
		else
			temp = number;
		
		temp = temp % 10;
		switch(temp)	
		{
			case 0:
				returnString[j] = '0';
				break;
			case 1:
				returnString[j] = '1';
				break;
			case 2:
				returnString[j] = '2';
				break;
			case 3:
				returnString[j] = '3';
				break;
			case 4:
				returnString[j] = '4';
				break;
			case 5:
				returnString[j] = '5';
				break;
			case 6:
				returnString[j] = '6';
				break;
			case 7:
				returnString[j] = '7';
				break;
			case 8:
				returnString[j] = '8';
				break;
			case 9:
				returnString[j] = '9';
				break;
		}
	}
	return returnString;
}

void setScalingFactors(void)
{
#if DNPCustomer == DNPCustomerMemphis
	union FIXED16_16 dividend = {0x800000};
	union FIXED16_16 divisor;
	
	divisor.Full16 = ((long)DNPData.WaveCaptureSampleRate.Value) << 16;
	
	scalingFactor = DIV16_16(dividend, divisor);
	
	eventRange = DNPData.WaveCaptureSampleRate.Value * DNPData.NumberOfCyclesEvent.Value * 2;
	
	eventStartingPoint = (8 - DNPData.NumberOfCyclesEvent.Value) * 1152;
#endif
}

//Timer handler, updates the CurrentTime variable
#pragma interrupt saveall
unsigned int secondTimer = 0;
void TimerB0_IH(void)
{
	DNPTimer++;
	
	secondTimer++;
	if(secondTimer == 1000)
	{
		secondTimer = 0;	
		DNP1SecTimer++;
	}
	
	timerB0_status.bit.TCF = 0;
}

#pragma interrupt saveall
void TimerB1_IH(void)
{
	
};
#endif
