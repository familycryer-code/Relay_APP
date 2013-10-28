/*****************************************************************************/
/* Triangle MicroWorks, Inc.                         Copyright(c) 1997-2011  */
/*****************************************************************************/
/*                                                                           */
/* This file is the property of:                                             */
/*                                                                           */
/*                       Triangle MicroWorks, Inc.                           */
/*                      Raleigh, North Carolina USA                          */
/*                       www.TriangleMicroWorks.com                          */
/* (919) 870 - 6615                                                          */
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

// WinIoTarg.cpp : Defines the entry point for the DLL application.
//
/* file: WinIoTarg.cpp
* description: Implementation of generic Windows I/O Target interface. This
*  file dispatches the I/O request to one of the supported low level I/O
*  implementations.
*
*  The interface from the source code libraries is through a set of
*  functions that hide the class interface. These functions are defined 
*  in the WinIoTarg.h header file and implemented in this file.
*/

#include "stdafx.h"
//VLD #include "thirdpartycode/Visual Leak Detector/include/vld.h"
#ifdef _DEBUG
#define _CRTDBG_MAP_ALLOC
#include <stdlib.h>
#include <crtdbg.h>
#endif

#include "WinIoTarg/include/WinIoTarg.h"
#include "WinIoTarg/include/WinIoTargDefs.h"

#include "WinIoTarg/WinIoBaseTime.h"
#include "WinIoTarg/WinIoSystemTime.h"
#include "WinIoTarg/WinIoSimulatedTime.h"

#include "WinIoTarg/WinTCPChannel.h"
#include "WinIoTarg/Win232Channel.h"
#if TMW_SUPPORT_MONITOR
#include "WinIoTarg/WinMonChannel.h"
#endif
#if WIN_MBPLUS_SUPPORT
#include "WinIoTarg/WinMBPChannel.h"
#endif
#if WIN_MODEM_SUPPORT
#include "WinIoTarg/WinModemPoolChannel.h"
#include "WinIoTarg/WinModemPoolManager.h"
#include "WinIoTarg/WinModemPool.h"
#include "WinIoTarg/WinModem.h"
#endif
#include "tmwscl/utils/tmwtarg.h"

static TMWTYPES_BOOL m_bUseConnectorThread = false;
static TMWTYPES_BOOL m_bWinSockInitialized = false;
 
// global data
WinIoTargProtoAnaLogFunType WinIoTargProtoAnaLogFun = WINIOTARG_NULL;

// the application wide shared time object
static WinIoBaseTime *g_pWinIoTimeObject = WINIOTARG_NULL;

// has winpcap been initialized
static TMWTYPES_BOOL pcapNotInitialized = true;

TMWTYPES_BOOL isWinSockInitialized(void)
{
  return m_bWinSockInitialized == true ? TRUE : FALSE;
}

/*!
*   global  DllMain
*   <TODO: insert function description here>
* 
*   @param  hModule HANDLE     <TODO: insert parameter description here>
*   @param  ul_reason_for_call TMWTYPES_ULONG     <TODO: insert parameter description here>
*   @param  lpReserved LPVOID     <TODO: insert parameter description here>
* 
*   @return TMWTYPES_BOOL <TODO: insert return value description here>
* 
*   @remarks <TODO: insert remarks here>
*/
TMWTYPES_BOOL APIENTRY DllMain(HANDLE hModule, 
                               TMWTYPES_ULONG  ul_reason_for_call, 
                               LPVOID lpReserved
                               )
{
  hModule=hModule;
  lpReserved=lpReserved;

  switch (ul_reason_for_call)
  {
  case DLL_PROCESS_ATTACH:
    struct WSAData wsaData;

    // Initialize winsock 2.0
    if(WSAStartup(MAKEWORD(2,0), &wsaData ) == 0)
    {
      m_bWinSockInitialized = true;
    }
    // Make sure version 2.0 is supported
    if(LOBYTE(wsaData.wVersion) != 2 || HIBYTE(wsaData.wVersion) != 0)
    {
      WSACleanup();
      m_bWinSockInitialized = false;
    }
    break;
  case DLL_PROCESS_DETACH:
    if (g_pWinIoTimeObject)
    {
      delete g_pWinIoTimeObject;
    }
#if WIN_MODEM_SUPPORT
    WinModemPoolManager_destroy();
#endif
    WSACleanup();
    break;

  case DLL_THREAD_ATTACH:
  case DLL_THREAD_DETACH:
    break;
  }
  return TRUE;
}

/**********************************************************************************\
Function :			WinIoTarg_initConfig
Description : [none]	
Return :			void	-	
Parameters :
WINIO_CONFIG *pConfig	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_initConfig(WINIO_CONFIG *pConfig)
{
  //WinIoTarg_initConfig_start_mark
  WinIoTarg_DebugPrintf("WinIoTarg_initConfig called\n");  // an example of TMWDebugPrintf usage

  pConfig->type = WINIO_TYPE_232;

  if (g_pWinIoTimeObject == WINIOTARG_NULL)
  {
    WinIoTarg_setTimeMode(WINIO_TIME_MODE_SIMULATED);
  }

  memset(&pConfig->win232.chnlName[0], 0, WINIOTARG_STR_LEN);
  strcpy(pConfig->win232.baudRate, "9600");
  pConfig->win232.numDataBits = WIN232_DATA_BITS_8;
  pConfig->win232.numStopBits = WIN232_STOP_BITS_1;
  pConfig->win232.parity = WIN232_PARITY_NONE;
  strcpy(pConfig->win232.portName, "COM1");
  pConfig->win232.portMode = WIN232_MODE_NONE;

  // the next 2 are only used in WIN232_MODE_HARDWARE
  pConfig->win232.dtrMode = WIN232_DTR_ENABLE;
  pConfig->win232.rtsMode = WIN232_RTS_DISABLE;

  pConfig->win232.bModbusRTU = false;
  pConfig->win232.disabled = false;

  memset(&pConfig->winTCP.chnlName[0],0,WINIOTARG_STR_LEN);
  pConfig->winTCP.mode = WINTCP_MODE_CLIENT;
  strcpy(pConfig->winTCP.ipAddress, "127.0.0.1");
  strcpy(pConfig->winTCP.localIpAddress, "0.0.0.0");
  strcpy(pConfig->winTCP.udpBroadcastAddress, "192.168.1.255");
  pConfig->winTCP.ipPort = 2404;
  pConfig->winTCP.ipConnectTimeout = 1000;
  pConfig->winTCP.disconnectOnNewSyn = true;

  // To support DNP3 Specification IP Networking 
  // local UDP port should default to 20000 for DNP, we will
  // initialize it to NONE, so IEC and Modbus protocols don't
  // open UDP port.
  pConfig->winTCP.localUDPPort       = WINTCP_UDP_PORT_NONE; 
  pConfig->winTCP.role               = WINTCP_ROLE_MASTER;
  pConfig->winTCP.validateUDPAddress = TMWDEFS_TRUE;
  pConfig->winTCP.dualEndPointIpPort = 20000;
  pConfig->winTCP.destUDPPort        = 20000;
  pConfig->winTCP.initUnsolUDPPort   = 20000;
  
#if TMW_SUPPORT_SSL
  // SSL support 
  pConfig->winTCP.useSSL                   = TMWDEFS_FALSE; 
  pConfig->winTCP.SSLVerifyClient          = TMWDEFS_FALSE; 
  pConfig->winTCP.sslVerifyCallbackParam   = TMWDEFS_NULL;
  pConfig->winTCP.sslVerifyCallback        = TMWDEFS_NULL;

  // Name of credentials file  
  strcpy(pConfig->winTCP.sslCredFile, "ssl credential file");

  // password to decrypt key from credentials file 
  strcpy(pConfig->winTCP.sslPassword, "ssl password");

  strcpy(pConfig->winTCP.sslCaListFile, "root.pem");
#endif

  memset(pConfig->winMBP.chnlName, 0, WINIOTARG_STR_LEN);
  pConfig->winMBP.mode = WINMBP_MODE_CLIENT;
  pConfig->winMBP.leaveMasterPortOpen = true;
  strcpy(pConfig->winMBP.routePath, "1.0.0.0.0");
  pConfig->winMBP.slavePath = 1;
  pConfig->winMBP.cardNum = 0;

  pConfig->winMON.monitorMode = TMWDEFS_FALSE;
  pConfig->winMON.interfaceNum = 0;
  pConfig->winMON.interfaceDescr[0] = 0;

  pConfig->connectDelay = 0;
  pConfig->forceDisconnected  = TMWDEFS_FALSE;
  //WinIoTarg_initConfig_end_mark
}

void WinIoTarg_getIdleCallBack(void *pContext, 
                               TMWCHNL_IDLE_CALLBACK *pCallBackFun, void **pCallBackParam)
{
#if WIN_MODEM_SUPPORT
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  if (pIOContext->type == WINIO_TYPE_MODEM_POOL_CHANNEL)
  {
    *pCallBackFun = WinModemPoolChannel::IdleCallback;
    *pCallBackParam = pIOContext->pWinIoInterface;
  }
  else
#else
  TMWTARG_UNUSED_PARAM(pContext);
#endif
  {
    *pCallBackFun   = WINIOTARG_NULL;
    *pCallBackParam = WINIOTARG_NULL;
  }
}

/**********************************************************************************\
Function :			WinIoTarg_Create
Description : [none]	
Return :			void *	-	
Parameters :
const void *pUserConfig	-	
TMWTARG_CONFIG *pTmwTargConfig	-	
Note : [none]
\**********************************************************************************/
void *WinIoTarg_Create(const void *pUserConfig,TMWTARG_CONFIG *pTmwTargConfig)
{

  if (pUserConfig == NULL)
  {
    return NULL;
  }

  WINIO_CONFIG *pIOConfig = (WINIO_CONFIG *)pUserConfig;
  WINIO_CONTEXT *pContext = new WINIO_CONTEXT();

  pContext->type = pIOConfig->type;
  pContext->connectDelay = 0;

  pContext->allowConnectorThread = false;
  if (
    pIOConfig->type == WINIO_TYPE_232
    || (pIOConfig->type == WINIO_TYPE_TCP && pIOConfig->winTCP.mode == WINTCP_MODE_CLIENT)
    || (pIOConfig->type == WINIO_TYPE_UDP_TCP && pIOConfig->winTCP.mode == WINTCP_MODE_CLIENT)
    || (pIOConfig->type == WINIO_TYPE_MBP && pIOConfig->winMBP.mode == WINMBP_MODE_CLIENT)
    )
  {
    pContext->allowConnectorThread = true;
    pContext->connectDelay = pIOConfig->connectDelay;
  }

  switch (pIOConfig->type)
  {
#if WIN_MODEM_SUPPORT
    case WINIO_TYPE_MODEM:
      pContext->pWinIoInterface = (void*)WinModem::Create(&pIOConfig->winModem, pTmwTargConfig);
      break;

    case WINIO_TYPE_MODEM_POOL:
      pContext->pWinIoInterface = (void*)WinModemPool::Create(&pIOConfig->winModemPool);
      break;

    case WINIO_TYPE_MODEM_POOL_CHANNEL:
      pContext->pWinIoInterface = (void*)WinModemPoolChannel::Create(&pIOConfig->winModemPoolChannel, pTmwTargConfig);
      break;
#endif

    case WINIO_TYPE_232:
      pContext->pWinIoInterface = (void*)Win232Channel::Create(&pIOConfig->win232, pTmwTargConfig);
      break;

    case WINIO_TYPE_TCP:
    case WINIO_TYPE_UDP_TCP:
      if (isWinSockInitialized() != TRUE)
      {
        pContext->pWinIoInterface = NULL;
      }
      else
      {
#if TMW_SUPPORT_MONITOR
        if(pIOConfig->winMON.monitorMode == TMWDEFS_TRUE)
        {
          pContext->allowConnectorThread = false;
          pContext->pWinIoInterface = (void*)WinMonChannel::Create(&pIOConfig->winMON, pTmwTargConfig);
          break;;
        }
#endif

        pContext->pWinIoInterface = (void*)WinTCPChannel::Create(&pIOConfig->winTCP, pTmwTargConfig);
      }
      break;

#if WIN_MBPLUS_SUPPORT
    case WINIO_TYPE_MBP:
      pContext->pWinIoInterface = (void*)WinMBPChannel::Create(&pIOConfig->winMBP, pTmwTargConfig);
      break;
#endif

#if TMW_SUPPORT_MONITOR
    case WINIO_TYPE_MON:
      pContext->pWinIoInterface = (void*)WinMonChannel::Create(&pIOConfig->winMON, pTmwTargConfig);
      break;
#endif
    default:
      return WINIOTARG_NULL;
  }

  if (pContext->pWinIoInterface == WINIOTARG_NULL)
  {
    delete pContext;
    return WINIOTARG_NULL;
  }


  ((WinIoInterface *)pContext->pWinIoInterface)->setForceDisconnected(pIOConfig->forceDisconnected);

  pContext->pTxCallback = WINIOTARG_NULL;
  pContext->pTxCallbackParam = WINIOTARG_NULL;

  pContext->pRxCallback = WINIOTARG_NULL;
  pContext->pRxCallbackParam = WINIOTARG_NULL;

  return pContext;
}

/**********************************************************************************\
Function :			WinIoTarg_Modify
Description : [none]	
Return :			void *	-	
Parameters :
void *pContext	-	
const void *pUserConfig	-	
Note : [none]
\**********************************************************************************/
TMWTYPES_BOOL WinIoTarg_Modify(void *pContext, const void *pUserConfig)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);
  return pWinIoInterface->modifyWinIoChannel(pUserConfig);
}

/**********************************************************************************\
Function :			WinIoTarg_Destroy
Description : [none]	
Return :			void	-	
Parameters :
void *pContext	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_Destroy(void *pContext)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);
  WinIoTarg_closeChannel(pIOContext);
  if (pIOContext)
  {
    if (pWinIoInterface)
      delete pWinIoInterface;
    delete pIOContext;
  }
}

/**********************************************************************************\
Function :			WinIoTarg_isChannelOpen
Description : [none]	
Return :			TMWTYPES_BOOL	-	
Parameters :
void *pContext	-	
Note : [none]
\**********************************************************************************/
TMWTYPES_BOOL WinIoTarg_isChannelOpen(void *pContext)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);

  return pWinIoInterface->isChannelOpen();
}

/**********************************************************************************\
Function :			WinIoTarg_openChannel
Description : [none]	
Return :			TMWTYPES_BOOL	-	
Parameters :
void *pContext	-	
Note : [none]
\**********************************************************************************/
TMWTYPES_BOOL   WinIoTarg_openChannel(void *pContext,
                                      TMWTARG_CHANNEL_RECEIVE_CBK_FUNC pReceiveCallbackFunc,
                                      TMWTARG_CHECK_ADDRESS_FUNC pCheckAddrCallbackFunc,
                                      void *pCallbackParam)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);

  if(pWinIoInterface->isForceDisconnected())
  {
    return false;
  }

  // See if already made connection 
  if (pWinIoInterface->isChannelOpen() == true)
  { 
    return true;
  }

  if (m_bUseConnectorThread == true)
  {
    // If we are allowed to use a connector thread..
    WinIoConnector *pIoConnector = pWinIoInterface->getIoConnector();
    
    pWinIoInterface->setDataCallbacks(pReceiveCallbackFunc, pCheckAddrCallbackFunc, pCallbackParam); 

    // Start connector thread if it is not running
    if(!pIoConnector->isRunning())
    {
      pIoConnector->StartConnectorThread(pWinIoInterface, pIOContext->connectDelay);
      return false;
    }
 
    return false;
  }
  else
  { 
    // Servers do not use connector thread for example
    return(pWinIoInterface->openWinIoChannel(pReceiveCallbackFunc, pCheckAddrCallbackFunc, pCallbackParam, WINIO_OPEN_MODE_NONE));
  }
}

/**********************************************************************************\
Function :			WinIoTarg_closeChannel
Description : [none]	
Return :			void	-	
Parameters :
void *pContext	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_closeChannel(void *pContext)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);
  if (m_bUseConnectorThread == true)
  {
    TMWTYPES_BOOL isRunning;
    isRunning = pWinIoInterface->getIoConnector()->isRunning();

    if (isRunning == true)
    {
      pWinIoInterface->getIoConnector()->StopConnectorThread();
    }
  }

  pWinIoInterface->closeWinIoChannel();
}


/**********************************************************************************\
Function :			WinIoTarg_resetChannel
Description : [none]	
Return :			TMWTYPES_BOOL	-	
Parameters :
void *pContext	-	
Note : [none]
\**********************************************************************************/
TMWTYPES_BOOL WinIoTarg_resetChannel(void *pContext)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);

  return pWinIoInterface->resetWinIoChannel();
}
/**********************************************************************************\
Function :			WinIoTarg_getChannelName
Description : [none]	
Return :			const char *	-	
Parameters :
void *pContext	-	
Note : [none]
\**********************************************************************************/
const char *WinIoTarg_getChannelName(void *pContext)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);

  if(pWinIoInterface == WINIOTARG_NULL)
    return("No interface");

  return pWinIoInterface->getChannelName();
}

/**********************************************************************************\
Function :			WinIoTarg_getChannelStatus
Description : [none]	
Return :			const char *	-	
Parameters :
void *pContext	-	
Note : [none]
\**********************************************************************************/
const char *WinIoTarg_getChannelStatus(void *pContext)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);

  return pWinIoInterface->getChannelStatus();
}

/**********************************************************************************\
Function :			WinIoTarg_getChannelInfo
Description : [none]	
Return :			const char *	-	
Parameters :
void *pContext	-	
Note : [none]
\**********************************************************************************/
const char *WinIoTarg_getChannelInfo(void *pContext)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);

  return pWinIoInterface->getChannelInfo();
}

/**********************************************************************************\
Function :			WinIoTarg_getTransmitReady
Description : [none]	
Return :			TMWTYPES_MILLISECONDS	-	
Parameters :
void *pContext	-	
Note : [none]
\**********************************************************************************/
TMWTYPES_MILLISECONDS WinIoTarg_getTransmitReady(void *pContext)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);

  return pWinIoInterface->getTransmitReady();
}

/**********************************************************************************\
Function :			WinIoTarg_receive
Description : [none]	
Return :			TMWTYPES_USHORT	-	
Parameters :
void *pContext	-	
TMWTYPES_UCHAR *pBuff	-	
TMWTYPES_USHORT maxBytes	-	
TMWTYPES_MILLISECONDS interCharacterTimeout	-	
TMWTYPES_BOOL *timeoutOccured	-	
Note : [none]
\**********************************************************************************/
TMWTYPES_USHORT WinIoTarg_receive(void *pContext, 
                                  TMWTYPES_UCHAR *pBuff, 
                                  TMWTYPES_USHORT maxBytes, 
                                  TMWTYPES_MILLISECONDS maxTimeout, 
                                  TMWTYPES_BOOL  *pTimeoutOccured)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);
  TMWTYPES_USHORT numBytes = 0;

  TMWTYPES_BOOL bTimeoutOccured = *pTimeoutOccured != 0 ? true : false;
  numBytes = pWinIoInterface->receiveOnChannel(pBuff, maxBytes, maxTimeout, &bTimeoutOccured);
  *pTimeoutOccured = bTimeoutOccured  == true ? true : false;

  if ((numBytes > 0) && (pIOContext->pRxCallback != WINIOTARG_NULL))
  {
    TMWTYPES_USHORT newNumBytes = numBytes;

    if (!pIOContext->pRxCallback(pContext, pIOContext->pRxCallbackParam, numBytes, maxBytes, &newNumBytes, pBuff))
      return false;

    numBytes = newNumBytes;
  }

  return numBytes;
}


/**********************************************************************************\
Function :			WinIoTarg_transmit
Description : [none]	
Return :			TMWTYPES_BOOL	-	
Parameters :
void *pContext	-	
TMWTYPES_UCHAR *pBuff	-	
TMWTYPES_USHORT numBytes	-	
Note : [none]
\**********************************************************************************/
TMWTYPES_BOOL WinIoTarg_transmit(void *pContext, TMWTYPES_UCHAR *pBuff, TMWTYPES_USHORT numBytes)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);

  if (pIOContext->pTxCallback != WINIOTARG_NULL)
  {
    TMWTYPES_USHORT newNumBytes = numBytes;

    if (!pIOContext->pTxCallback(pContext, pIOContext->pTxCallbackParam, numBytes, numBytes, &newNumBytes, pBuff))
      return true;

    if(newNumBytes == 0)
      return true;

    numBytes = newNumBytes;
  }

  return pWinIoInterface->transmitOnChannel(pBuff, numBytes);
}

/**********************************************************************************\
Function :			WinIoTarg_transmitUDP
Description : [none]	
Return :			TMWTYPES_BOOL	-	
Parameters :
void *pContext	-	
TMWTYPES_UCHAR UDPPort	-	 A define that indicates the remote UDP port to
transmit to. 
TMWTARG_UDP_SEND       - Send to the remote port to be used for 
requests or responses
TMWTARG_UDP_SEND_UNSOL - Send to the remote port to be used for   
unsolicited responses
TMWTYPES_UCHAR *pBuff	-	
TMWTYPES_USHORT numBytes	-	
Note : [none]
\**********************************************************************************/
TMWTYPES_BOOL WinIoTarg_transmitUDP(void *pContext, 
                                    TMWTYPES_UCHAR UDPPort,
                                    TMWTYPES_UCHAR *pBuff, 
                                    TMWTYPES_USHORT numBytes)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);

  if (pIOContext->pTxCallback != WINIOTARG_NULL)
  {
    TMWTYPES_USHORT newNumBytes = numBytes;

    if (!pIOContext->pTxCallback(pContext, pIOContext->pTxCallbackParam, numBytes, 
      numBytes, &newNumBytes, pBuff))
    {
      return true;
    }
    numBytes = newNumBytes;
  }

  return pWinIoInterface->transmitUDP(UDPPort, pBuff, numBytes);
}

/**********************************************************************************\
Function :			WinIoTarg_setSyncTransmit
Description : [none]	
Return :			void	-	
Parameters :
TMWTYPES_BOOL flag	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_setSyncTransmit(TMWTYPES_BOOL flag)
{  
  TMWTARG_UNUSED_PARAM(flag);
  /* No longer used */ 
}

/**********************************************************************************\
Function :			WinIoTarg_setTxCallback
Description : [none]	
Return :			void	-	
Parameters :
void *pContext	-	
WINIO_CALLBACK pCallback	-	
void *pCallbackParam	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_setTxCallback(void *pContext, WINIO_CALLBACK pCallback,  void *pCallbackParam)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  pIOContext->pTxCallback = pCallback;
  pIOContext->pTxCallbackParam = pCallbackParam;
}

/**********************************************************************************\
Function :			WinIoTarg_setRxCallback
Description : [none]	
Return :			void	-	
Parameters :
void *pContext	-	
WINIO_CALLBACK pCallback	-	
void *pCallbackParam	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_setRxCallback(void *pContext, WINIO_CALLBACK pCallback, void *pCallbackParam)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  pIOContext->pRxCallback = pCallback;
  pIOContext->pRxCallbackParam = pCallbackParam;
}

/**********************************************************************************\
Function :			WinIoTarg_setStatusCallback
Description : [none]	
Return :			void	-	
Parameters :
void *pContext	-	
WINIO_STATUS_CALLBACK pCallback	-	
void *pCallbackParam	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_setStatusCallback(void *pContext, WINIO_STATUS_CALLBACK pCallback, void *pCallbackParam)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);
  if (pWinIoInterface)
  {
    pWinIoInterface->setStatusCB(pCallback, pCallbackParam);
  }
}

#if WIN_MODEM_SUPPORT
/**********************************************************************************\
Function :			WinModem_setStatusCallback
Description : [none]	
Return :			void	-	
Parameters :
void *pContext	-	
WINIO_STATUS_CALLBACK pCallback	-	
void *pCallbackParam	-	
Note : [none]
\**********************************************************************************/
void WinModem_setStatusCallback(void *pContext, WINIO_STATUS_CALLBACK pCallback, void *pCallbackParam)
{
  WinModem *pWinModem = (WinModem *)pContext;
  if (pWinModem)
  {
    pWinModem->setStatusCB(pCallback, pCallbackParam);
  }
}

/**********************************************************************************\
Function :			WinModemPool_setStatusCallback
Description : [none]	
Return :			void	-	
Parameters :
void *pContext	-	
WINIO_STATUS_CALLBACK pCallback	-	
void *pCallbackParam	-	
Note : [none]
\**********************************************************************************/
void WinModemPool_setStatusCallback(void *pContext, WINIO_STATUS_CALLBACK pCallback, void *pCallbackParam)
{
  WinModemPool *pWinModemPool = (WinModemPool *)pContext;
  if (pWinModemPool)
  {
    pWinModemPool->setStatusCB(pCallback, pCallbackParam);
  }
}

/**********************************************************************************\
Function :			WinModemPoolManager_setStatusCallback
Description : [none]	
Return :			void	-	
Parameters :
void *pContext	-	
WINIO_STATUS_CALLBACK pCallback	-	
void *pCallbackParam	-	
Note : [none]
\**********************************************************************************/
void WinModemPoolManager_setStatusCallback(void *pContext, WINIO_STATUS_CALLBACK pCallback, void *pCallbackParam)
{
  WinModemPoolManager *pWinModemPoolManager = (WinModemPoolManager *)pContext;
  if (pWinModemPoolManager)
  {
    pWinModemPoolManager->setStatusCB(pCallback, pCallbackParam);
  }
}

/**********************************************************************************\
Function :			WinModemPoolManager_getManager
Description : [none]	
Return :			void *	-	
Parameters :
void	-	
Note : [none]
\**********************************************************************************/
void *WinModemPoolManager_getManager(void)
{
  return GetModemPoolManager();
}

/**********************************************************************************\
Function :			WinModemPoolManager_destroy
Description : [none]	
Return :			void -	
Parameters : none
Note : [none]
\**********************************************************************************/
void WinModemPoolManager_destroy(void)
{
  if (WinModemPoolManager::g_pModemPoolManager)
  {
    delete WinModemPoolManager::g_pModemPoolManager;
    WinModemPoolManager::g_pModemPoolManager = NULL;
  }
}

#endif

/**********************************************************************************\
Function :			WinIoTarg_setEnabled
Description : [none]	
Return :			void	-	
Parameters :
void *pContext	-	
TMWTYPES_BOOL bStatus	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_setEnabled(    
                          void *pContext,
                          TMWTYPES_BOOL bStatus)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  WinIoInterface *pWinIoInterface = (WinIoInterface*)(pIOContext->pWinIoInterface);
  if (pWinIoInterface)
  {
    pWinIoInterface->setChanEnabled(bStatus != 0 ? true : false);
  }
}

/**********************************************************************************\
Function :			WinIoTarg_setUseConnectorThread
Description : [none]	
Return :			void	-	
Parameters :
void *pContext	-	
TMWTYPES_BOOL flag	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_setUseConnectorThread(void *pContext, TMWTYPES_BOOL flag)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  if ((pIOContext->allowConnectorThread != 0 ? true : false) == true)
  {
    m_bUseConnectorThread = flag != 0 ? true : false;
  }
  else
  { // connector thread is not used for servers
    m_bUseConnectorThread = false;
  }
}

/**********************************************************************************\
Function :			WinIoTarg_startThread
Description :  Start event handle thread
Return :			void *	-	
Parameters :
WINIO_THREAD_FUN threadFun	-	
void *pParam	-	
unsigned int *threadID	-	
int nPriority	-	
Note : [none]
\**********************************************************************************/
void *WinIoTarg_startThread(WINIO_THREAD_FUN threadFun, void *pParam, unsigned int *threadID, int nPriority)
{
  // Start thread waiting for comm events
  // void *threadHandle = (void*)_beginthread(threadFun, 0, pParam);
  void *threadHandle = (void*)_beginthreadex(NULL, 0, threadFun, pParam, CREATE_SUSPENDED, threadID);
  if (threadHandle == 0)
  {
    return WINIOTARG_NULL;
  }
  SetThreadPriority(threadHandle, nPriority);
  ResumeThread(threadHandle);

  return threadHandle;
}

/**********************************************************************************\
Function :			WinIoTarg_endThread
Description : [none]	
Return :			void	-	
Parameters :
void *threadHandle	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_endThread(void *threadHandle)
{
  /* threadHandle is not a pointer, it is the handle itself */
  TerminateThread((HANDLE)threadHandle, 1);
}

/**********************************************************************************\
Function :			WinIoTarg_setProtoAnaLogFun
Description : [none]	
Return :			void	-	
Parameters :
WinIoTargProtoAnaLogFunType pFun	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_setProtoAnaLogFun(WinIoTargProtoAnaLogFunType pFun)
{
  WinIoTargProtoAnaLogFun = pFun;
}

/**********************************************************************************\
Function :			WinIoTarg_getMsTime
Description : [none]	
Return :			MWTYPES_MILLISECONDS	-	
Parameters :
void	-	
Note : [none]
\**********************************************************************************/
TMWTYPES_MILLISECONDS WinIoTarg_getMsTime(void)
{
  if (g_pWinIoTimeObject == NULL)
  {
    WinIoTarg_setTimeMode(WINIO_TIME_MODE_SIMULATED);
  }
  return(g_pWinIoTimeObject->getMsTime());
}

/**********************************************************************************\
Function :			WinIoTarg_Sleep
Description : [none]	
Return :			void	-	
Parameters :
TMWTYPES_MILLISECONDS time	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_Sleep(TMWTYPES_MILLISECONDS time)
{
  if (g_pWinIoTimeObject == NULL)
  {
    WinIoTarg_setTimeMode(WINIO_TIME_MODE_SIMULATED);
  }
  g_pWinIoTimeObject->sleep(time);
}

/**********************************************************************************\
Function :			WinIoTarg_getDateTime
Description : [none]	
Return :			void	-	
Parameters :
TMWDTIME *pDateTime	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_getDateTime(TMWDTIME *pDateTime)
{
  if (g_pWinIoTimeObject == NULL)
  {
    WinIoTarg_setTimeMode(WINIO_TIME_MODE_SIMULATED);
  }
  g_pWinIoTimeObject->getDateTime(pDateTime);
}

/**********************************************************************************\
Function :			WinIoTarg_setDateTime
Description : [none]	
Return :			TMWTYPES_BOOL	-	
Parameters :
const TMWDTIME *pNewDateTime	-	
Note : [none]
\**********************************************************************************/
TMWTYPES_BOOL WinIoTarg_setDateTime(const TMWDTIME *pNewDateTime)
{
  if (g_pWinIoTimeObject == NULL)
  {
    WinIoTarg_setTimeMode(WINIO_TIME_MODE_SIMULATED);
  }
  return(g_pWinIoTimeObject->setDateTime(pNewDateTime));
}

/**********************************************************************************\
Function :			WinIoTarg_getUTCDateTime
Description : [none]	
Return :			void	-	
Parameters :
TMWDTIME *pDateTime	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_getUTCDateTime(TMWDTIME *pDateTime)
{  
  WinIoSystemTime::getUTCDateTime(pDateTime);
}

/**********************************************************************************\
Function :			WinIoTarg_setUTCDateTime
Description : [none]	
Return :			TMWTYPES_BOOL	-	
Parameters :
const TMWDTIME *pNewDateTime	-	
Note : [none]
\**********************************************************************************/
TMWTYPES_BOOL WinIoTarg_setUTCDateTime(const TMWDTIME *pNewDateTime)
{
  return(WinIoSystemTime::setUTCDateTime(pNewDateTime));
}

/**********************************************************************************\
Function :			WinIoTarg_convertLocalTime
Description : [none]	
Return :			void	-	
Parameters :
TMWDTIME *pDateTime	-	
time_t timeIn	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_convertLocalTime(TMWDTIME *pDateTime, time_t timeIn)
{
  if (g_pWinIoTimeObject == NULL)
  {
    WinIoTarg_setTimeMode(WINIO_TIME_MODE_SIMULATED);
  }
  g_pWinIoTimeObject->convertLocalTime(pDateTime, timeIn);
}

/**********************************************************************************\
Function :			WinIoTarg_setTimeMode
Description : [none]	
Return :			void	-	
Parameters :
WINIO_TIME_MODE timeMode	-	
Note : [none]
\**********************************************************************************/
void WinIoTarg_setTimeMode(WINIO_TIME_MODE timeMode)
{
  if (g_pWinIoTimeObject)
  {
    delete g_pWinIoTimeObject;
  }
  switch(timeMode)
  {

  case WINIO_TIME_MODE_SYSTEM_NO_SETTIME:
    g_pWinIoTimeObject = new WinIoSystemTime(timeMode);
    break;
  case WINIO_TIME_MODE_SYSTEM:
    g_pWinIoTimeObject = new WinIoSystemTime(timeMode);
    break;
  case WINIO_TIME_MODE_SIMULATED:
    g_pWinIoTimeObject = new WinIoSimulatedTime(timeMode);
    break;
  default:
    g_pWinIoTimeObject = new WinIoSystemTime(WINIO_TIME_MODE_SIMULATED);
    break;
  }
}

#if TMW_SUPPORT_MONITOR

TMWTYPES_BOOL WinIoTarg_initTCPMonitor(void)
{
  if(pcapNotInitialized)
  {
    HINSTANCE hinst = LoadLibrary("wpcap.dll");
    if (hinst == NULL)
      return(TMWDEFS_FALSE);

    pcapNotInitialized = false;
  }
  return(TMWDEFS_TRUE);
}

TMWTYPES_BOOL WinIoTarg_CheckHostName(
                                      char  *pHost)
{
  /* If this is not a x.x.x.x ip address, see if the hostname can be found */
  if(strchr(pHost, '.') == 0)
  {
    HOSTENT *h = gethostbyname(pHost);
    if(h == NULL)
    {
      return(false);
    }
  }
  return(true);
}

void WinIoTarg_GetTCPInterfaces(char *pInterface[], int *pCount)
{ 
  WinMonChannel::getAllInterfaces(pInterface, pCount);
}


void WinIoTarg_inputData(
                         void *pContext, 
                         TMWTYPES_UCHAR *pBuf, 
                         TMWTYPES_USHORT length)
{
  WINIO_CONTEXT *pIOContext = (WINIO_CONTEXT *)pContext;
  if(pIOContext->type == WINIO_TYPE_MON)
  {
    WinMonChannel *pInterface = (WinMonChannel*)(pIOContext->pWinIoInterface);
    pInterface->inputData(pBuf, length);
  }
}
#endif

TMWTYPES_LONG WinIoTarg_GetPrivateProfileInt(
  const char *lpAppName,  // section name
  const char *lpKeyName,  // key name
  int nDefault,           // return value if key name not found
  const char *lpFileName  // initialization file name
  )
{
  return GetPrivateProfileInt(lpAppName,lpKeyName,nDefault,lpFileName);
}

TMWTYPES_BOOL WinIoTarg_SetPrivateProfileInt(
  const char *lpAppName,  // section name
  const char *lpKeyName,  // key name
  int value,              // value to set
  const char *lpFileName  // initialization file name
  )
{
  char str[256];
  sprintf(str,"%d",value);
  BOOL bRet = WritePrivateProfileString(lpAppName,lpKeyName,str,lpFileName);
  TMWTYPES_ULONG error = 0;
  if (bRet == 0)
  {
    error = GetLastError();
    return(TMWDEFS_FALSE);
  }
  return TMWDEFS_TRUE;
}

TMWTYPES_ULONG WinIoTarg_GetPrivateProfileString(
  const char *lpAppName,        // section name
  const char *lpKeyName,        // key name
  const char *lpDefault,        // default string
  char *lpReturnedString,       // destination buffer
  TMWTYPES_ULONG nSize,         // size of destination buffer
  const char *lpFileName        // initialization file name
  )
{
  return GetPrivateProfileString(lpAppName,lpKeyName,lpDefault,lpReturnedString,nSize,lpFileName);
}

TMWTYPES_BOOL WinIoTarg_SetPrivateProfileString(
  const char *lpAppName,        // section name
  const char *lpKeyName,        // key name
  const char *value,            // default string
  const char *lpFileName        // initialization file name
  )
{
  if(WritePrivateProfileString(lpAppName,lpKeyName,value,lpFileName))
  {
    return(TMWDEFS_TRUE);
  }
  return(TMWDEFS_FALSE);
}
// fetch node name and ip address of this computer
TMWTYPES_BOOL WinIoTarg_GetHostNameAndIPaddr(char *name, char *ip_addr)
{
  char host_name[256];
  PHOSTENT hostinfo;

  if (isWinSockInitialized() == TRUE)
  {
    if (gethostname(host_name, sizeof(host_name)) == 0)
    {
      if ((hostinfo = gethostbyname(host_name)) != NULL)
      {
        strcpy(ip_addr,inet_ntoa(* (struct in_addr *)*hostinfo->h_addr_list));
        strcpy(name,host_name);
        return TRUE;
      }
    }
  } 
  return FALSE;
}

/* get the IP address from the node name */
TMWTYPES_BOOL WinIoTarg_GetIPaddrForHost(const char *nodeName, char *ipAddrStr)
{
  struct hostent *host;
  struct in_addr *ip_addr;	/* To retrieve the IP Address */
  host = gethostbyname(nodeName);
  if (host != NULL)
  {
    ip_addr = (struct in_addr *) host->h_addr_list[0];

    /* Eg. 211.40.35.76 split up like this. */
    int a = ip_addr->S_un.S_un_b.s_b1;  /* 211 */
    int b = ip_addr->S_un.S_un_b.s_b2;  /* 40 */
    int c = ip_addr->S_un.S_un_b.s_b3;  /* 35 */
    int d = ip_addr->S_un.S_un_b.s_b4;  /* 76 */

    sprintf(ipAddrStr, "%d.%d.%d.%d", a, b, c, d);
    return TRUE;
  }
  return FALSE;
}

void * TMWDEFS_GLOBAL WinIoTarg_CreateSessionTimeObject(const TMWDTIME *pDateTime)
{
  WinIoBaseTime *pTimeObject = new WinIoSimulatedTime(WINIO_TIME_MODE_SIMULATED);
  pTimeObject->setDateTime(pDateTime);
  return pTimeObject;
} 

void TMWDEFS_GLOBAL WinIoTarg_SetSessionDateTime(void *pHandle, TMWDTIME *pDateTime)
{
  WinIoBaseTime *pTimeObject = (WinIoBaseTime*)pHandle;
  pTimeObject->setDateTime(pDateTime);
}

void TMWDEFS_GLOBAL WinIoTarg_GetSessionDateTime(void *pHandle, TMWDTIME *pDateTime)
{
  WinIoBaseTime *pTimeObject = (WinIoBaseTime*)pHandle;
  pTimeObject->getDateTime(pDateTime);
}

void TMWDEFS_GLOBAL WinIoTarg_DeleteSessionTimeObject(void *pHandle)
{
  WinIoBaseTime *pTimeObject = (WinIoBaseTime*)pHandle;
  delete pTimeObject;
}

void WinIoTarg_lockInit(TMWDEFS_RESOURCE_LOCK *pLock)
{
  *pLock = (CRITICAL_SECTION *)malloc(sizeof(CRITICAL_SECTION));
  InitializeCriticalSection((CRITICAL_SECTION*)*pLock);
}
void WinIoTarg_lockSection(TMWDEFS_RESOURCE_LOCK *pLock)
{
  EnterCriticalSection((CRITICAL_SECTION*)*pLock);
}

void WinIoTarg_unlockSection(TMWDEFS_RESOURCE_LOCK *pLock)
{
  LeaveCriticalSection((CRITICAL_SECTION*)*pLock);
}

void WinIoTarg_lockDelete(TMWDEFS_RESOURCE_LOCK *pLock)
{
  DeleteCriticalSection((CRITICAL_SECTION*)*pLock);
  free(*pLock);
}

void WinIoTarg_formatErrorMsg(char *pBuffer, TMWTYPES_ULONG error)
{
  ::FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, NULL, error,
      0, (LPTSTR) pBuffer, 128, NULL);
}

/*****************************************************************************/
int WinIoTarg_DebugPrintf(char *format,...)
{
  struct tm *newtime;
  time_t aclock;
  char   file_name[256];
  static HANDLE hStdout = 0;
  static FILE* fp = 0;
  char   Buffer[2048];
  va_list argpointer;
  int Result;
  DWORD BytesWritten;

  if (WinIoTarg_GetPrivateProfileInt("debug", "log", 0, "TMWDebug.ini") == 0)
    return 0;

  va_start(argpointer, format);
  Result = _vstprintf(Buffer, format, argpointer);

  if (WinIoTarg_GetPrivateProfileInt("debug", "file", 0, "TMWDebug.ini") == 0)
  {
    if (fp != 0)
    {
      fclose(fp);
      fp =0;
    }
    if (hStdout == 0)
    {
      AllocConsole();
      hStdout = GetStdHandle(STD_OUTPUT_HANDLE);
    }
    WriteFile(hStdout, Buffer, Result, &BytesWritten, NULL);    
  }
  else
  {
    if (hStdout != 0)
    {
      FreeConsole();
      hStdout = 0;
    }

    if (fp == 0)
    {
      WinIoTarg_GetPrivateProfileString("debug", "file_name", "", file_name, 255, "TMWDebug.ini");
      fp = fopen(file_name, "w");
      if (fp == 0)
      {
        ::MessageBox
          (
          NULL,
          "Could not open log file.\n Please define 'file_name' in TMWDebug.ini",
          "TMW Debug Log Message",
          MB_OK | MB_ICONSTOP
          );
        exit(1);
      }
    }

    time(&aclock);                 /* Get time in seconds */
    newtime = localtime(&aclock);  /* Convert time to struct tm form  */
    fprintf(fp, "%s: %s", asctime(newtime), Buffer);
    fflush(fp);
  }
  va_end(argpointer);

  return (Result);
}
