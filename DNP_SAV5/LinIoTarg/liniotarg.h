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

/* file: liniotarg.h
 * description: Implementation of target routines for Linux
 */
#ifndef liniotarg_DEFINED
#define liniotarg_DEFINED

#include "liniodefs.h"

#include "tmwscl/utils/tmwdefs.h"
#include "tmwscl/utils/tmwdiag.h"
#include "tmwscl/utils/tmwcnfg.h"
#include "tmwscl/utils/tmwtarg.h"
#include "tmwscl/utils/tmwpltmr.h"
  

/* function: liniotarg_initConfig */
void TMWDEFS_GLOBAL liniotarg_initConfig(LINIO_CONFIG *pConfig);

/* function: liniotarg_initChannel */
void * TMWDEFS_GLOBAL liniotarg_initChannel(
  const void *pUserConfig,
  TMWTARG_CONFIG *pTmwConfig);

/* function: liniotarg_deleteChannel */
void TMWDEFS_GLOBAL liniotarg_deleteChannel(
  void *pContext);

/* function: liniotarg_getChannelName */
const char * TMWDEFS_GLOBAL liniotarg_getChannelName(
  void *pContext);

/* function: liniotarg_getChannelInfo */
const char * TMWDEFS_GLOBAL liniotarg_getChannelInfo(
  void *pContext);

/* function: liniotarg_openChannel */
TMWTYPES_BOOL TMWDEFS_GLOBAL liniotarg_openChannel( 
  void *pContext,
  TMWTARG_CHANNEL_RECEIVE_CBK_FUNC pChnlRcvCallbackFunc,
  TMWTARG_CHECK_ADDRESS_FUNC pCheckAddrCallbackFunc,
  void *pCallbackParam);

/* function: liniotarg_closeChannel */
void TMWDEFS_GLOBAL liniotarg_closeChannel(
  void *pContext);

/* function: liniotarg_getTransmitReady */
TMWTYPES_MILLISECONDS TMWDEFS_GLOBAL liniotarg_getTransmitReady(
  void *pContext);

/* function: liniotarg_receive */
TMWTYPES_USHORT TMWDEFS_GLOBAL liniotarg_receive(
  void *pContext,
  TMWTYPES_UCHAR *pBuff,
  TMWTYPES_USHORT maxBytes,
  TMWTYPES_MILLISECONDS maxTimeout,
  TMWTYPES_BOOL *pInterCharTimeoutOccurred);

/* function: liniotarg_transmit */
TMWTYPES_BOOL TMWDEFS_GLOBAL liniotarg_transmit(
  void *pContext,
  TMWTYPES_UCHAR *pBuff,
  TMWTYPES_USHORT numBytes);

/* function: liniotarg_transmitUDP */
TMWTYPES_BOOL TMWDEFS_GLOBAL liniotarg_transmitUDP(
  void *pContext,
  TMWTYPES_UCHAR UDPPort,
  TMWTYPES_UCHAR *pBuff,
  TMWTYPES_USHORT numBytes);

void TMWDEFS_GLOBAL liniotarg_diagMsg(  
  TMWTYPES_CHAR *pMsg);

void TMWDEFS_GLOBAL liniotarg_diagErrorMsg(  
  TMWTYPES_CHAR *pMsg);

#endif // liniotarg_DEFINED
