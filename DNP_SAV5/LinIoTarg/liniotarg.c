/*****************************************************************************/
/* Triangle MicroWorks, Inc.                         Copyright (c) 2008-2011 */
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


/* file: liniotarg.c
* description: Implementation of target I/O routines for Linux
*/
#include "liniotarg.h"
#include "liniodefs.h"
#include "liniocnfg.h"
#include "lin232.h"
#include "lintcp.h"

#include "tmwscl/utils/tmwcnfg.h"
#include "tmwscl/utils/tmwpltmr.h"

#include "linuxio.h"

LINIO_CONTEXT LinIoContext = {
  TMWDEFS_FALSE   /* bStarted */
#if LINIOTARG_SUPPORT_232
  ,
  TMWDEFS_NULL,   /* serialSemaphore  */
  TMWDEFS_NULL    /* *pSerialChannels */
#endif
#if LINIOTARG_SUPPORT_TCP
  ,
  TMWDEFS_NULL,   /* tcpSemaphore          */
  TMWDEFS_NULL,   /* *pTcpChannels         */
  TMWDEFS_NULL,   /* *pTcpListeners        */
  TMWDEFS_FALSE,  /* bSocketThreadRunning  */
  TMWDEFS_FALSE,  /* bConnectThreadRunning */
  0,              /* socketThreadHandle    */
  0               /* connectThreadHandle   */
#endif
};

/* function: liniotarg_initConfig */
void TMWDEFS_GLOBAL liniotarg_initConfig(LINIO_CONFIG *pConfig)
{

#if (LINIOTARG_SUPPORT_TCP == TMWDEFS_FALSE) && (LINIOTARG_SUPPORT_232 == TMWDEFS_FALSE)
  pConfig->type = LINIO_TYPE_NONE;
#endif

#if (LINIOTARG_SUPPORT_TCP == TMWDEFS_FALSE) && (LINIOTARG_SUPPORT_232 == TMWDEFS_TRUE)
  pConfig->type = LINIO_TYPE_232;
#endif

#if (LINIOTARG_SUPPORT_TCP == TMWDEFS_TRUE) && (LINIOTARG_SUPPORT_232 == TMWDEFS_FALSE)
  pConfig->type = LINIO_TYPE_TCP;
#endif

#if (LINIOTARG_SUPPORT_TCP == TMWDEFS_TRUE) && (LINIOTARG_SUPPORT_232 == TMWDEFS_TRUE)
  pConfig->type = LINIO_TYPE_232;
#endif

#if LINIOTARG_SUPPORT_232 
  strcpy(pConfig->lin232.chnlName, "/dev/ttyS0");
  strcpy(pConfig->lin232.portName, "/dev/ttyS0");
  pConfig->lin232.baudRate    = LIN232_BAUD_9600;
  pConfig->lin232.numDataBits = LIN232_DATA_BITS_8;
  pConfig->lin232.numStopBits = LIN232_STOP_BITS_1;
  pConfig->lin232.parity      = LIN232_PARITY_NONE;
  pConfig->lin232.portMode    = LIN232_MODE_NONE;
  pConfig->lin232.bModbusRTU  = TMWDEFS_FALSE;
#endif

#if LINIOTARG_SUPPORT_TCP
  strcpy(pConfig->linTCP.chnlName, "");
  pConfig->linTCP.mode = LINTCP_MODE_CLIENT;
  pConfig->linTCP.role = LINTCP_ROLE_MASTER;
  strcpy(pConfig->linTCP.ipAddress, "127.0.0.1");  
  strcpy(pConfig->linTCP.udpBroadcastAddress, "192.168.1.255");
  pConfig->linTCP.ipPort                 = 20000;
  pConfig->linTCP.dualEndPointIpPort     = 20000;
  pConfig->linTCP.destUDPPort            = 20000;
  pConfig->linTCP.initUnsolUDPPort       = 20000;
  pConfig->linTCP.localUDPPort           = TMWTARG_UDP_PORT_NONE;
  pConfig->linTCP.validateUDPAddress     = TMWDEFS_TRUE;
  pConfig->linTCP.disconnectOnNewSyn     = TMWDEFS_FALSE;
  pConfig->linTCP.ipConnectTimeout       = 1000;
#endif
}

/* function: liniotarg_initChannel */
void * TMWDEFS_GLOBAL liniotarg_initChannel(
  const void *pUserConfig,
  TMWTARG_CONFIG *pTmwConfig)
{
  LINIO_CONFIG  *pIOConfig = (LINIO_CONFIG *) pUserConfig;
  LINIO_CHANNEL *pLinIoChannel;

  if (!LinIoContext.bStarted)
  {
#if LINIOTARG_SUPPORT_TCP
    _LinuxSemCreate(&LinIoContext.tcpSemaphore);
    LinIoContext.pTcpChannels = TMWDEFS_NULL;
    LinIoContext.pTcpListeners = TMWDEFS_NULL;
    LinIoContext.bSocketThreadRunning = TMWDEFS_FALSE;
    LinIoContext.bConnectThreadRunning = TMWDEFS_FALSE;
    LinIoContext.socketThreadHandle = 0;
    LinIoContext.connectThreadHandle = 0;
#endif

#if LINIOTARG_SUPPORT_232
    LinIoContext.pSerialChannels = TMWDEFS_NULL;
    _LinuxSemCreate(&LinIoContext.serialSemaphore);
#endif
    LinIoContext.bStarted = TMWDEFS_TRUE;
  }

  pLinIoChannel = (LINIO_CHANNEL *) malloc(sizeof(LINIO_CHANNEL));
  if ( pLinIoChannel == NULL )
    return TMWDEFS_NULL;

  pLinIoChannel->type = pIOConfig->type;
  switch (pIOConfig->type)
  {
#if LINIOTARG_SUPPORT_232
  case LINIO_TYPE_232:
    pLinIoChannel->pChannelInfo = (void *) lin232_initChannel(pIOConfig, pTmwConfig);
    break;
#endif

#if LINIOTARG_SUPPORT_TCP
  case LINIO_TYPE_TCP:
    pLinIoChannel->pChannelInfo = (void *) linTCP_initChannel(pIOConfig, pTmwConfig);
    break;
#endif
  default:
    return TMWDEFS_NULL;
  }

  return(pLinIoChannel);
}

/* function: liniotarg_deleteChannel */
void TMWDEFS_GLOBAL liniotarg_deleteChannel(void *pContext)
{
  LINIO_CHANNEL *pLinIoChannel = (LINIO_CHANNEL *) pContext;

  switch(pLinIoChannel->type)
  {
#if LINIOTARG_SUPPORT_232
  case LINIO_TYPE_232:
    lin232_deleteChannel((SERIAL_IO_CHANNEL *)pLinIoChannel->pChannelInfo);
    break;
#endif

#if LINIOTARG_SUPPORT_TCP
  case LINIO_TYPE_TCP:
    linTCP_deleteChannel((TCP_IO_CHANNEL *)pLinIoChannel->pChannelInfo);
    break;
#endif

  default:
    break;
  }
  free(pLinIoChannel);
  return;
}

/* function: liniotarg_getChannelName */
const char * TMWDEFS_GLOBAL liniotarg_getChannelName(void *pContext)
{
  LINIO_CHANNEL *pLinIoChannel = (LINIO_CHANNEL *) pContext;

#if LINIOTARG_SUPPORT_232
  if(pLinIoChannel->type == LINIO_TYPE_232)
  {
    return (lin232_getChannelName((SERIAL_IO_CHANNEL *)pLinIoChannel->pChannelInfo));
  }
#endif

#if LINIOTARG_SUPPORT_TCP
  if(pLinIoChannel->type == LINIO_TYPE_TCP)
  {
    return (linTCP_getChannelName((TCP_IO_CHANNEL *)pLinIoChannel->pChannelInfo));
  }
#endif

  return("");
}

/* function: liniotarg_getChannelInfo */
const char * TMWDEFS_GLOBAL liniotarg_getChannelInfo(void *pContext)
{
  LINIO_CHANNEL *pLinIoChannel = (LINIO_CHANNEL *) pContext;

  switch(pLinIoChannel->type)
  {
#if LINIOTARG_SUPPORT_232
  case LINIO_TYPE_232:
    return (lin232_getChannelInfo((SERIAL_IO_CHANNEL *)pLinIoChannel->pChannelInfo));
    break;
#endif

#if LINIOTARG_SUPPORT_TCP
  case LINIO_TYPE_TCP:
    return (linTCP_getChannelInfo((TCP_IO_CHANNEL *)pLinIoChannel->pChannelInfo));
    break;
#endif

  default:
    return("");
    break;
  }
}
/* function: liniotarg_openChannel */
TMWTYPES_BOOL TMWDEFS_GLOBAL liniotarg_openChannel(
  void *pContext,
  TMWTARG_CHANNEL_RECEIVE_CBK_FUNC pChnlRcvCallbackFunc,
  TMWTARG_CHECK_ADDRESS_FUNC pCheckAddrCallbackFunc,
  void *pCallbackParam)
{
  LINIO_CHANNEL *pLinIoChannel = (LINIO_CHANNEL *) pContext;

#if LINIOTARG_SUPPORT_232
  SERIAL_IO_CHANNEL *pLinSerialContext;
#endif

#if LINIOTARG_SUPPORT_TCP
  TCP_IO_CHANNEL *pLinTcpContext;
#endif


#if LINIOTARG_SUPPORT_232
  if(pLinIoChannel->type == LINIO_TYPE_232)
  {
    pLinSerialContext = (SERIAL_IO_CHANNEL *) pLinIoChannel->pChannelInfo;
    pLinSerialContext->pReceiveCallbackFunc = pChnlRcvCallbackFunc;
    pLinSerialContext->pCheckAddrCallbackFunc = pCheckAddrCallbackFunc;
    pLinSerialContext->pCallbackParam = pCallbackParam;
    return (lin232_openChannel(pLinSerialContext));
  }
#endif

#if LINIOTARG_SUPPORT_TCP
  if(pLinIoChannel->type == LINIO_TYPE_TCP)
  {
    pLinTcpContext = (TCP_IO_CHANNEL *) pLinIoChannel->pChannelInfo;
    pLinTcpContext->pReceiveCallbackFunc = pChnlRcvCallbackFunc;
    pLinTcpContext->pCheckAddrCallbackFunc = pCheckAddrCallbackFunc;
    pLinTcpContext->pCallbackParam = pCallbackParam;
    return(linTCP_openChannel(pLinTcpContext));
  }
#endif

  return(TMWDEFS_FALSE);
}

/* function: liniotarg_closeChannel */
void TMWDEFS_GLOBAL liniotarg_closeChannel(void *pContext)
{
  LINIO_CHANNEL *pLinIoChannel = (LINIO_CHANNEL *) pContext;

  switch(pLinIoChannel->type)
  {
#if LINIOTARG_SUPPORT_232
  case LINIO_TYPE_232:
    lin232_closeChannel((SERIAL_IO_CHANNEL *)pLinIoChannel->pChannelInfo);
    return;
    break;
#endif

#if LINIOTARG_SUPPORT_TCP
  case LINIO_TYPE_TCP:
    linTCP_closeChannel((TCP_IO_CHANNEL *)pLinIoChannel->pChannelInfo);
    return;
    break;
#endif

  default:
    return;
    break;
  }
}

/* function: liniotarg_getTransmitReady */
TMWTYPES_MILLISECONDS TMWDEFS_GLOBAL liniotarg_getTransmitReady(void *pContext)
{
  LINIO_CHANNEL *pLinIoChannel = (LINIO_CHANNEL *) pContext;

#if LINIOTARG_SUPPORT_232
  if(pLinIoChannel->type == LINIO_TYPE_232)
  {
    return (lin232_getTransmitReady((SERIAL_IO_CHANNEL *)pLinIoChannel->pChannelInfo));
  }
#endif

#if LINIOTARG_SUPPORT_TCP
  if(pLinIoChannel->type == LINIO_TYPE_TCP)
  {
    return (linTCP_getTransmitReady((TCP_IO_CHANNEL *)pLinIoChannel->pChannelInfo));
  }
#endif

  return(500);
}

/* function: liniotarg_receive */
TMWTYPES_USHORT TMWDEFS_GLOBAL liniotarg_receive(
  void *pContext,
  TMWTYPES_UCHAR *pBuff,
  TMWTYPES_USHORT maxBytes,
  TMWTYPES_MILLISECONDS maxTimeout,
  TMWTYPES_BOOL *pInterCharTimeoutOccurred)
{
  LINIO_CHANNEL *pLinIoChannel = (LINIO_CHANNEL *) pContext;

#if LINIOTARG_SUPPORT_232
  if(pLinIoChannel->type == LINIO_TYPE_232)
  {
    return (lin232_receive((SERIAL_IO_CHANNEL *)pLinIoChannel->pChannelInfo,pBuff,maxBytes,maxTimeout,pInterCharTimeoutOccurred));
  }
#endif

#if LINIOTARG_SUPPORT_TCP
  if(pLinIoChannel->type == LINIO_TYPE_TCP)
  {
    return (linTCP_receive((TCP_IO_CHANNEL *)pLinIoChannel->pChannelInfo,pBuff,maxBytes,maxTimeout,pInterCharTimeoutOccurred));
  }
#endif

  return(0);
}

/* function: liniotarg_transmit */
TMWTYPES_BOOL TMWDEFS_GLOBAL liniotarg_transmit(
  void *pContext,
  TMWTYPES_UCHAR *pBuff,
  TMWTYPES_USHORT numBytes)
{
  LINIO_CHANNEL *pLinIoChannel = (LINIO_CHANNEL *) pContext;

#if LINIOTARG_SUPPORT_232
  if(pLinIoChannel->type == LINIO_TYPE_232)
  {
    return (lin232_transmit((SERIAL_IO_CHANNEL *)pLinIoChannel->pChannelInfo,pBuff,numBytes));
  }
#endif

#if LINIOTARG_SUPPORT_TCP
  if(pLinIoChannel->type == LINIO_TYPE_TCP)
  {
    return (linTCP_transmit((TCP_IO_CHANNEL *)pLinIoChannel->pChannelInfo,pBuff,numBytes));
  }
#endif

  return(TMWDEFS_FALSE);
}

TMWTYPES_BOOL TMWDEFS_GLOBAL liniotarg_transmitUDP(
  void *pContext,
  TMWTYPES_UCHAR UDPPort,
  TMWTYPES_UCHAR *pBuff,
  TMWTYPES_USHORT numBytes)
{
  LINIO_CHANNEL *pLinIoChannel = (LINIO_CHANNEL *) pContext;

#if LINIOTARG_SUPPORT_TCP
  if(pLinIoChannel->type == LINIO_TYPE_TCP)
  {
    return (linTCP_transmitUDP((TCP_IO_CHANNEL *)pLinIoChannel->pChannelInfo,UDPPort,pBuff,numBytes));
  }
#endif

  return(TMWDEFS_FALSE);
}  

#if defined(TMW_LINUX_TARGET)
int _fileno( FILE *stream )
{
  return( fileno( stream ) );
}
size_t _read( int fd, void *buf, size_t count )
{
  return( read( fd, buf, count ) );
}
#endif
