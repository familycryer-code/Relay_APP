/*****************************************************************************/
/* Triangle MicroWorks, Inc.                         Copyright (c) 1997-2011 */
/*****************************************************************************/
/*                                                                           */
/* This file is the property of:                                             */
/*                                                                           */
/*                       Triangle MicroWorks, Inc.                           */
/*                      Raleigh, North Carolina USA                          */
/*                       www.TriangleMicroWorks.com                          */
/*                          (919) 870-6615                                   */
/*                                                                           */
/* This Source Code and the associated Documentation contain proprietary     */
/* information of Triangle MicroWorks, Inc. and may not be copied or         */
/* distributed in any form without the written permission of Triangle        */
/* MicroWorks, Inc.  Copies of the source code may be made only for backup   */
/* purposes.                                                                 */
/*                                                                           */
/* Your License agreement may limit the installation of this source code to  */
/* specific products.  Before installing this source code on a new           */
/* application, check your license agreement to ensure it allows use on the  */
/* product in question.  Contact Triangle MicroWorks for information about   */
/* extending the number of products that may use this source code library or */
/* obtaining the newest revision.                                            */
/*                                                                           */
/*****************************************************************************/

// DNPSlave.cpp : Sample DNP Slave console application.
//
#if defined(TMW_LINUX_TARGET) 
#else
#include "stdafx.h"
#endif

extern "C" {
#include "tmwscl/utils/tmwdb.h"
#include "tmwscl/utils/tmwpltmr.h"
#include "tmwscl/utils/tmwtarg.h"

#include "tmwscl/dnp/dnpchnl.h"
#include "tmwscl/dnp/sdnpsesn.h"
 
#if defined(TMW_WTK_TARGET) 
#include "winiotarg/include/winiotarg.h"
#endif

#if defined(TMW_LINUX_TARGET) 
#include "LinIoTarg/liniotarg.h"
#endif
}

#if !TMWCNFG_MULTIPLE_TIMER_QS

/* forward references */
static void mySleep(int milliseconds);
void myPutDiagString(const TMWDIAG_ANLZ_ID *pAnlzId, const TMWTYPES_CHAR *pString);

#if SDNPDATA_SUPPORT_OBJ120
void              myInitSecureAuthentication(SDNPSESN_CONFIG *pSesnConfig);

/* These are the default user keys the test harness uses for testing 
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
static TMWTYPES_UCHAR  authoritySymCertKey[] = {
  0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 
  0x09, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06,
  0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 
  0x09, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06
};

#endif

/* Main entry point */
int main(int argc, char* argv[])
{
  TMWAPPL *pApplContext;
  TMWCHNL *pSclChannel;
  TMWSESN *pSclSession;
  DNPCHNL_CONFIG DNPConfig;
  DNPTPRT_CONFIG tprtConfig;
  TMWTARG_CONFIG targConfig;
  TMWPHYS_CONFIG physConfig;
  DNPLINK_CONFIG linkConfig;
  SDNPSESN_CONFIG sesnConfig;
  bool useSerial = false;
  TMWTYPES_CHAR pKey[32];
  
  TMWTARG_UNUSED_PARAM(argc);
  TMWTARG_UNUSED_PARAM(argv);

#if TMW_WTK_TARGET || TMW_LINUX_TARGET
#if TMWCNFG_SUPPORT_DIAG 
  /* Register function to display diagnostic strings to console 
   * This is only necessary if using the WinIOTarg target layer.
   * If implementing a new target layer, tmwtarg_putDiagString()
   * should be modified if diagnostic output is desired.
   */
  tmwtargp_registerPutDiagStringFunc(myPutDiagString);
#endif
  
  /* Register poll timer timer functions.
   * This is only necessary if using the WinIOTarg target layer.
   * If implementing a new target layer tmwtarg_startTimer and
   * tmwtarg_cancelTimer will call poll timer functions by default,
   * or can be modified to call target provided timer functions.
   */
  tmwtargp_registerStartTimerFunc(tmwpltmr_startTimer);
  tmwtargp_registerCancelTimerFunc(tmwpltmr_cancelTimer);
#endif


  /*
   * Initialize SDNP SCL. This includes:
   *  - initialize polled timer
   *  - initialize application context 
   */
  tmwtimer_initialize();
  pApplContext = tmwappl_initApplication();

  /* Initialize channel configuration to defaults */
  tmwtarg_initConfig(&targConfig);
  dnpchnl_initConfig(&DNPConfig, &tprtConfig, &linkConfig, &physConfig);
  linkConfig.networkType = DNPLINK_NETWORK_TCP_UDP;
 
#if defined(TMW_WTK_TARGET) 
  /* Initialize IO Config Structure
   * Call WinIoTarg_initConfig to initialize default values, then overwrite
   * specific values as needed.
   *
   * This example configures the Slave for a TCP/IP session using the
   * Loopback address and default Session values. This configuration
   * is compatible with the example scripts that ship with the
   * Communication Protocol Test Harness.
   */
  WINIO_CONFIG IOCnfg;
  WinIoTarg_initConfig(&IOCnfg);
  if(useSerial)
  {
    IOCnfg.type = WINIO_TYPE_232;

    /* COM port to open */
    strcpy(IOCnfg.win232.portName, "COM2");
    
    /* Name displayed in analyzer window */
    strcpy(IOCnfg.win232.chnlName, "Slave");

    strcpy(IOCnfg.win232.baudRate, "9600");
    IOCnfg.win232.numDataBits = WIN232_DATA_BITS_8;
    IOCnfg.win232.numStopBits = WIN232_STOP_BITS_1;
    IOCnfg.win232.parity = WIN232_PARITY_NONE;
    IOCnfg.win232.portMode = WIN232_MODE_NONE;
  }
  else
  { 
    IOCnfg.type = WINIO_TYPE_TCP;

    /* Name displayed in analyzer window */
    strcpy(IOCnfg.winTCP.chnlName, "Slave");

    /* TCP/IP address of remote device  
     * 127.0.0.1 is the loopback address and will accept connections from this computer
     * Use *.*.*.* to allow connections from any ip address
     */
    strcpy(IOCnfg.winTCP.ipAddress, "127.0.0.1"); 

    /* port to listen on */
    IOCnfg.winTCP.ipPort = 20000;

    /* listen, do not initiate the connection */
    IOCnfg.winTCP.mode = WINTCP_MODE_SERVER;
 
    /* There are certain rules for an outstation in IP Networking */
    IOCnfg.winTCP.role = WINTCP_ROLE_OUTSTATION;
    IOCnfg.winTCP.disconnectOnNewSyn = TMWDEFS_FALSE;
      
    IOCnfg.winTCP.localUDPPort = 20000; /* TMWTARG_UDP_PORT_NONE */
    IOCnfg.winTCP.validateUDPAddress = TMWDEFS_FALSE;

    /* Some other outstation configurations */
    bool dualEndPoint = false;
    bool udpOnly = false;
    if(dualEndPoint)
    { 
      /* This code will configure the channel for Dual End Point mode 
       * ie. Client AND Server
       * It will both listen and try to connect to the remote end when
       * it has data to send. This mode should be used on both ends.
       */
      IOCnfg.winTCP.mode = WINTCP_MODE_DUAL_ENDPOINT;

      /* if dual end point, the ip address of the master must be specified */
      strcpy(IOCnfg.winTCP.ipAddress, "127.0.0.1");

      /* This is the port to send connect request to when the outstation initiates the connection */
      IOCnfg.winTCP.dualEndPointIpPort = 20000;
    }
    else if(udpOnly)
    {
      /* This code will configure the channel for UDP only.
       */
      
      IOCnfg.winTCP.mode = WINTCP_MODE_UDP; 
      linkConfig.networkType = DNPLINK_NETWORK_UDP_ONLY;

      /* if UDP ONLY, the IP address of the master must be specified */
      strcpy(IOCnfg.winTCP.ipAddress, "127.0.0.1");
      
      /* Choose the local UDP port to send and receive on */
      IOCnfg.winTCP.localUDPPort = 20000;

      /* Send responses to the port the master sends from, Normally this would be configured to use 20000. */
      IOCnfg.winTCP.destUDPPort =  TMWTARG_UDP_PORT_SRC; //20000;

      /* Send the initial Null unsolicited response to this port on the master */
      IOCnfg.winTCP.initUnsolUDPPort = 20001;
    }
  }
#endif

#if defined(TMW_LINUX_TARGET)  
  /* Initialize IO Config Structure
   * Call liniotarg_initConfig to initialize default values, then overwrite
   * specific values as needed.
   *
   * This example configures the Outstation (Slave) for a TCP/IP session using the
   * Loopback address and default Session values. This configuration
   * is compatible with the example scripts that ship with the
   * Communication Protocol Test Harness.
   */
  LINIO_CONFIG IOCnfg;
  liniotarg_initConfig(&IOCnfg);
  if(useSerial)
  {
    IOCnfg.type = LINIO_TYPE_232;

    /* name displayed in analyzer window */
    strcpy(IOCnfg.lin232.chnlName, "Slave"); 

    /* port to open */
    strcpy(IOCnfg.lin232.portName, "/dev/ttyS1");

    IOCnfg.lin232.baudRate    = LIN232_BAUD_9600;
    IOCnfg.lin232.numDataBits = LIN232_DATA_BITS_8;
    IOCnfg.lin232.numStopBits = LIN232_STOP_BITS_1;
    IOCnfg.lin232.parity      = LIN232_PARITY_NONE;
    IOCnfg.lin232.portMode    = LIN232_MODE_NONE;
  }
  else
  { 
    IOCnfg.type = LINIO_TYPE_TCP;
    
    /* Name displayed in analyzer window */
    strcpy(IOCnfg.linTCP.chnlName, "Slave");

    /* TCP/IP address of remote device  
     * 127.0.0.1 is the loopback address and will accept connections from this computer
     * Use *.*.*.* to allow connections from any ip address
     */
    strcpy(IOCnfg.linTCP.ipAddress, "*.*.*.*");
    
    /* port to listen on */
    IOCnfg.linTCP.ipPort = 20000;
    
      /* listen, do not initiate the connection */
    IOCnfg.linTCP.mode = LINTCP_MODE_SERVER;

    /* There are certain rules for an outstation in IP Networking */
    IOCnfg.linTCP.role = LINTCP_ROLE_OUTSTATION;
    IOCnfg.linTCP.disconnectOnNewSyn = TMWDEFS_FALSE;

    /* Only one channel can open the local UDP port
     * setting this to TMWTARG_UDP_PORT_NONE will prevent target code 
     * from trying to open UDP port 
     */
    IOCnfg.linTCP.localUDPPort = 20000;
    IOCnfg.linTCP.validateUDPAddress = TMWDEFS_FALSE;

    /* Some other outstation configurations */
    bool dualEndPoint = false;
    bool udpOnly = false;
    if(dualEndPoint)
    { 
      /* This code will configure the channel for Dual End Point mode 
       * ie. Client AND Server
       * It will both listen and try to connect to the remote end when
       * it has data to send. This mode should be used on both ends.
       */
      IOCnfg.linTCP.mode = LINTCP_MODE_DUAL_ENDPOINT;

      /* if dual end point, the ip address of the master must be specified */
      strcpy(IOCnfg.linTCP.ipAddress, "127.0.0.1");

      /* This is the port to send connect request to when the outstation initiates the connection */
      IOCnfg.linTCP.dualEndPointIpPort = 20000;
    }
    else if(udpOnly)
    {
      /* This code will configure the channel for UDP only.
       */
      IOCnfg.linTCP.mode = LINTCP_MODE_UDP; 
      linkConfig.networkType = DNPLINK_NETWORK_UDP_ONLY;

      /* if UDP ONLY, the IP address of the master must be specified */
      strcpy(IOCnfg.linTCP.ipAddress, "127.0.0.1");
      
      /* Choose the local UDP port to send and receive on */
      IOCnfg.linTCP.localUDPPort = 20000;

      /* Send responses to the port the master sends from, Normally this would be configured to use 20000. */
      IOCnfg.linTCP.destUDPPort =  TMWTARG_UDP_PORT_SRC; //20000;

      /* Send the initial Null unsolicited response to this port on the master */
      IOCnfg.linTCP.initUnsolUDPPort = 20000;
    }
  }
#endif
 
  /* Open DNP channel */
  pSclChannel = dnpchnl_openChannel(pApplContext, &DNPConfig, &tprtConfig, 
    &linkConfig, &physConfig, &IOCnfg, &targConfig);

  /* Initialize and open DNP slave session */
  sdnpsesn_initConfig(&sesnConfig);
  
#if SDNPDATA_SUPPORT_OBJ120
  /* Set this to true to enable DNP3 Secure Authentication support */
  bool myUseAuthentication = true; 

  /* If Secure Authentication is to be enabled */
  if(myUseAuthentication)
    myInitSecureAuthentication(&sesnConfig);
#endif

  pSclSession = (TMWSESN *)sdnpsesn_openSession(pSclChannel, &sesnConfig, 
    TMWDEFS_NULL);

  if(pSclSession == TMWDEFS_NULL)
  {
    /* Failed to open session */
    printf("Failed to open Session, exiting program \n");
  
    /* Sleep for 10 seconds before exiting */
    mySleep(10000);
    return (1);
  } 

#if SDNPCNFG_SUPPORT_SA_VERSION5
    /* In SAv5 we no longer use an array of user configuration, instead you can add one user at a time.
     * The Authority can also tell the master to add a user using a globally unique user name
     * and instruct the master to send the update key and role (permissions) for that user over DNP to the outstation.
     */ 
    if(!sesnConfig.authConfig.operateInV2Mode)
    {
      TMWTYPES_USHORT userNumber = 1;
      
      /* If using simulated database in tmwcrypto, add the User Update Key for the default user (1) to it.
       * This key should really should be in YOUR crypto database not the simulated one in tmwcrypto.c. 
       * You would not normally need to call tmwcrypto_setKeyData to add it to your own database.
       */
      
      if(!tmwcrypto_setKeyData(TMWDEFS_NULL, TMWCRYPTO_USER_UPDATE_KEY, (void *)userNumber, defaultUserKey1, 16))
      {
        /* failed to add key */
      } 
      /* Add the user to the sdnp library */
      sdnpsesn_addAuthUser(pSclSession, userNumber); 
      
      
      /* Add second user, not required */
      userNumber = 100; 
      
      /* If using simulated database in tmwcrypto, add the User Update Key for the second user number.
       * This key should really should be in YOUR crypto database not the simulated one in tmwcrypto.c. 
       * You would not normally need to call tmwcrypto_setKeyData to add it to your own database.
       */
      if(!tmwcrypto_setKeyData(TMWDEFS_NULL, TMWCRYPTO_USER_UPDATE_KEY, (void *)userNumber, defaultUserKeyOther, 16))
      {
        /* failed to add key */
      } 
      /* Add the user to the sdnp library */
      sdnpsesn_addAuthUser(pSclSession, userNumber);  
 

      /* Configure other values in YOUR crypto database to allow remote user key and role update from Master.*/

      /* This sample uses the same values that the Test Harness Outstation uses by default */

      /* Outstation name must be configured in both Master and Outstation. 
       * This is already set in sdnpsim database
       * OutstationName = "SDNP Outstation";
       */
      
      /* If using simulated database in tmwcrypto, configure the Authority Certification Symmetric Key to it.
       * This key really should be in YOUR crypto database not the simulated one in tmwcrypto.c.
       * You would not normally need to call tmwcrypto_setKeyData to add it to your own database.
       * This key is used by the Central Authority, not the master, but this sample is acting as the Authority.
       */
      if(!tmwcrypto_setKeyData(TMWDEFS_NULL, TMWCRYPTO_AUTH_CERT_SYM_KEY, TMWDEFS_NULL, (TMWTYPES_UCHAR *)&authoritySymCertKey, 32))
      {
        /* failed to add key */
      } 
 
      /* If using simulated database in tmwcrypto, configure Outstation Private Key when Asymmetric Key Update is supported.
       * This really should be in YOUR crypto database not the simulated one.
       * You would not normally need to call tmwcrypto_setKeyData to add it to your own database.
       */
      sprintf( pKey, "TMWTestOSAsymPrvKey.pem");
      TMWTYPES_USHORT keyLength = strlen(pKey);
      if(!tmwcrypto_setKeyData(TMWDEFS_NULL, TMWCRYPTO_OS_ASYM_PRV_KEY, TMWDEFS_NULL, (TMWTYPES_UCHAR *)pKey, keyLength))
      {
        /* failed to add key */
      }

      /* If using simulated database in tmwcrypto, configure Authority Public Key when Asymmetric Key Update is supported
       * This really should be in YOUR crypto database not the simulated one.
       * You would not normally need to call tmwcrypto_setKeyData to add it to your own database.
       */
      sprintf( pKey, "TMWTestRsa2048PubKey.pem");
      keyLength = strlen(pKey);
      if(!tmwcrypto_setKeyData(TMWDEFS_NULL, TMWCRYPTO_AUTH_ASYM_PUB_KEY, TMWDEFS_NULL, (TMWTYPES_UCHAR *)pKey, keyLength))
      {
        /* failed to add key */
      }  

    }
#endif

  /* This code will open a second channel and outstation session */
  bool openSecondConnection = false;
  if(openSecondConnection)
  {
    TMWCHNL *pSclChannel2;
    TMWSESN *pSclSession2;
#if defined(TMW_WTK_TARGET) 
    strcpy(IOCnfg.winTCP.chnlName, "Other"); 
    IOCnfg.winTCP.ipPort = 20001;
#endif
#if defined(TMW_LINUX_TARGET)  
    strcpy(IOCnfg.linTCP.chnlName, "Other"); 
    IOCnfg.linTCP.ipPort = 20001;
#endif

    /* Open Second DNP channel */
    pSclChannel2 = dnpchnl_openChannel(pApplContext, &DNPConfig, &tprtConfig,
                                     &linkConfig, &physConfig, &IOCnfg, &targConfig);

    pSclSession2 = (TMWSESN *)sdnpsesn_openSession(pSclChannel2, &sesnConfig,
                 TMWDEFS_NULL);
  }


  /* Begin the main loop.
   * This example uses the Source Code Library in Polled mode.
   * In Polled mode, you must periodically:
   *  - check the timer
   *  - check for (and process) any received data
   */
  while(1)
  {
    tmwpltmr_checkTimer(); 

    tmwappl_checkForInput(pApplContext);

    /* Sleep for 50 milliseconds */
    mySleep(50);
  }
 
  return 0;
}
 

#if SDNPDATA_SUPPORT_OBJ120
void myInitSecureAuthentication(SDNPSESN_CONFIG *pSesnConfig)
{
  /* set this to TMWDEFS_TRUE to use SAv2 implementation */
  TMWTYPES_BOOL myUseSAv2 = TMWDEFS_FALSE;

#if !SDNPCNFG_SUPPORT_SA_VERSION5
  /* If SAv5 is not supported, use SAv2 */
  myUseSAv2 = TMWDEFS_TRUE;
#endif

  printf( "Using SAv%c\n", myUseSAv2?'2':'5');
  
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

#if SDNPCNFG_SUPPORT_SA_VERSION2 
  /* Use Secure Authentication Version 2 */
  if(myUseSAv2)
  {
    pSesnConfig->authConfig.operateInV2Mode = TMWDEFS_TRUE;
    pSesnConfig->authConfig.maxErrorCount = 2;

    /* Configure user numbers */
    /* Spec says user number 1 provides a user number for the device or "any" user */
    pSesnConfig->authConfig.authUsers[0].userNumber = DNPAUTH_DEFAULT_USERNUMBER;
    pSesnConfig->authConfig.authUsers[1].userNumber = 100;

  }
#endif
  /* Example configuration. Some of these may be the default values,
   * but are shown here as an example of what can be set.
   */
  pSesnConfig->authConfig.extraDiags = TMWDEFS_TRUE;
  pSesnConfig->authConfig.aggressiveModeSupport = TMWDEFS_TRUE;
  pSesnConfig->authConfig.maxKeyChangeCount = 1000; 
  // 120 seconds is very short for demonstration purposes only.
  pSesnConfig->authConfig.keyChangeInterval = 120000;  
  pSesnConfig->authConfig.assocId = 0;
}
#endif

/* Sample function to sleep for x milliseconds */
static void  mySleep(int milliseconds)
{
#if defined(TMW_WTK_TARGET) 
    WinIoTarg_Sleep(milliseconds);
#endif
#if defined(TMW_LINUX_TARGET) 
    Sleep(milliseconds*100);
#endif
}

/* The following functions are called by the SCL */

#if TMWCNFG_SUPPORT_DIAG
/* Simple diagnostic output function, registered with the Source Code Library */
void myPutDiagString(const TMWDIAG_ANLZ_ID *pAnlzId,const TMWTYPES_CHAR *pString)
{
  TMWDIAG_ID id = pAnlzId->sourceId;

  if((TMWDIAG_ID_ERROR & id) 
    ||(TMWDIAG_ID_APPL & id)
    ||(TMWDIAG_ID_USER & id)
    ||(TMWDIAG_ID_SECURITY_DATA & id)
    ||(TMWDIAG_ID_SECURITY_HDRS & id)
    )
  {
    printf( "%s", (char *)pString);  
    return;
  }

  /* Comment this out to turn off verbose diagnostics */
  /* For now print everything */
  /* printf((char *)pString); */
}
#endif
#endif
