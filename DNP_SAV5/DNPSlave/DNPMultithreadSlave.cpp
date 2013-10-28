/*****************************************************************************/
/* Triangle MicroWorks, Inc.                         Copyright (c) 1997-2006 */
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

// DNPMultithreadSlave.cpp : Sample multithreaded DNP Outstation console application.
//

#if defined(TMW_LINUX_TARGET) 
#else
#include "stdafx.h"
#endif

extern "C" {
#include "tmwscl/utils/tmwappl.h"
#include "tmwscl/utils/tmwdb.h"
#include "tmwscl/utils/tmwphys.h"
#include "tmwscl/utils/tmwpltmr.h"
#include "tmwscl/utils/tmwtarg.h"
 
#include "tmwscl/dnp/dnpchnl.h"
#include "tmwscl/dnp/sdnpsesn.h"
#include "tmwscl/dnp/sdnputil.h"
#include "tmwscl/dnp/sdnpo002.h"
#include "tmwscl/dnp/sdnpo022.h"

#if defined(TMW_WTK_TARGET)
#include <stdlib.h>     /* atoi */
#include <process.h>    /* _beginthread, _endthread */
#include <Mmsystem.h>   /* timeSetEvent */
#include <windows.h>
#include "WinIoTarg/include/winiotarg.h"
  
/* Windows thread signature*/
#define TMW_ThreadDecl unsigned int __stdcall
#endif

#if defined(TMW_LINUX_TARGET) 
#include <stdio.h>
#include <signal.h>
#include <sys/time.h>
#include "LinIoTarg/liniotarg.h"
#endif
}

/* If Multiple Timer Queues is configured, use this file */
#if TMWCNFG_MULTIPLE_TIMER_QS

/* Number of channels and therefore channel threads */
#define NUMBER_CHANNELS 3
/* Number of sessions per channel */
#define NUMBER_SESSIONS 1

/* Sample application configuration */
#define ALIVE_CHECK_INTERVAL       20000
#define EVENT_INTERVAL             10000

/* Simple application structure for controlling generation of events */
typedef struct myRequestInfo
{
  TMWTYPES_MILLISECONDS  eventInterval;
  TMWTYPES_MILLISECONDS  lastEvent;    
} MY_REQUEST_INFO;
 
/* Per Thread timer data */
/* Defines a data structure to hold timer info */
typedef struct {
  TMWTYPES_BOOL            isActive;
  TMWTYPES_BOOL            isExpired;
  TMWTYPES_MILLISECONDS    timeout;
  int                      timerEvent;
  TMWTYPES_CALLBACK_FUNC   pCallback;
  void                    *pCallbackParam;
} MY_TIMER_TYPE;

/* Declare an array of timers, one per channel */
static MY_TIMER_TYPE myThreadTimer[NUMBER_CHANNELS] = {0};

static MY_REQUEST_INFO *myInitRequests(void);
static void             myStartSystemTimer();
static void             myStartThread(int i);
static void             mySleep(int milliseconds);
static void             myCheckThreadTimer(MY_TIMER_TYPE *pTimer);
static MY_TIMER_TYPE   *myGetTimer(int channelNumber);
static TMWTYPES_BOOL    myTimeToSendEvent(TMWTYPES_MILLISECONDS *lastTime, TMWTYPES_MILLISECONDS interval); 

/* Diagnostics */
static void             myLogOutput(char *pBuf);
static void             myPutDiagString(const TMWDIAG_ANLZ_ID *pAnlzId, const TMWTYPES_CHAR *pString);

static TMW_ThreadDecl   myChannelThreadDNP(void *pParam);
 
/*
 * Begin the main loop
 */
int main(int argc, char* argv[])
{
  TMWTYPES_USHORT i;
  char logBuf[256];
  TMWTARG_UNUSED_PARAM(argc);
  TMWTARG_UNUSED_PARAM(argv);

#if TMW_WTK_TARGET || TMW_LINUX_TARGET
#if TMWCNFG_SUPPORT_DIAG 
  /* Register function to display diagnostic strings to console 
   * This is only necessary if using the Windows or Linux target layer.
   * If implementing a new target layer, tmwtarg_putDiagString()
   * should be modified if diagnostic output is desired.
   */
  tmwtargp_registerPutDiagStringFunc(myPutDiagString);
#endif
#endif

  /*
   * Initialize SCL.
   */
  tmwdb_init(10000);

  /* Start the channel threads */
  for(i=0; i<NUMBER_CHANNELS; i++)
  {
   sprintf(logBuf, "starting dnp outstation thread %d\n", i);
    myLogOutput(logBuf);
    
    myStartThread(i);
  }

  /* start single system timer that will drive the individual thread timers */
  myStartSystemTimer();

  /*  Loop forever. This simple example has nothing else to do
   */
  while(1)
  {
    /* sleep for 10000 milliseconds */
    mySleep(10000);
  }

  return(0);
}

/* Per Channel Thread */
TMW_ThreadDecl myChannelThreadDNP(void *pParam)
{
  int  i;
  TMWAPPL *pThreadApplContext;
  MY_REQUEST_INFO *pMyRequests;
  TMWCHNL *pSclChannel;
  TMWSESN *pSclSession[NUMBER_SESSIONS];

  /* Configuration */
#if defined(TMW_WTK_TARGET)
  WINIO_CONFIG    IOCnfg;
#endif
#if defined(TMW_LINUX_TARGET) 
  LINIO_CONFIG    IOCnfg;
#endif
  TMWPHYS_CONFIG  physConfig;
  TMWTARG_CONFIG  targConfig;
  DNPCHNL_CONFIG  DNPConfig;
  DNPLINK_CONFIG  linkConfig;
  DNPTPRT_CONFIG  tprtConfig;
  SDNPSESN_CONFIG sesnConfig;
  char logBuf[256];

  int channelNumber = (int)pParam;
  MY_TIMER_TYPE *pTimer = myGetTimer(channelNumber);

  /* Create separate application context per channel thread 
   * This is not required, all channels can exist on a single application context.
   */
  pThreadApplContext = tmwappl_initApplication();

  pMyRequests = myInitRequests();

  /*
   * Initialize all configuration structures to defaults
   */
  tmwtarg_initConfig(&targConfig);
  targConfig.pMultiThreadTimerHandle = pTimer;

  dnpchnl_initConfig(&DNPConfig, &tprtConfig, &linkConfig, &physConfig); 
  linkConfig.networkType = DNPLINK_NETWORK_TCP_UDP;
  
#if defined(TMW_WTK_TARGET)
  WinIoTarg_initConfig(&IOCnfg);
  IOCnfg.type = WINIO_TYPE_TCP;

  /* name displayed in analyzer window */
  sprintf(IOCnfg.winTCP.chnlName, "DNPslave%d", channelNumber); 

  /* IP address to accept connections from */
  /* *.*.*.* allows any client to connect */
  strcpy(IOCnfg.winTCP.ipAddress, "*.*.*.*");

  /* IP port to listen on */
  IOCnfg.winTCP.ipPort = 20000 + channelNumber;
  IOCnfg.winTCP.mode = WINTCP_MODE_SERVER;

  IOCnfg.winTCP.localUDPPort = (TMWTYPES_USHORT)(20000 + channelNumber);
  IOCnfg.winTCP.disconnectOnNewSyn = TMWDEFS_FALSE;
  IOCnfg.winTCP.role = WINTCP_ROLE_OUTSTATION; 
#endif

#if defined(TMW_LINUX_TARGET) 
  liniotarg_initConfig(&IOCnfg);

  /*
   * use TCP since it makes sample easier to use on a PC
   */
  IOCnfg.type = LINIO_TYPE_TCP;
  
  /* name displayed in analyzer window */ 
  sprintf(IOCnfg.linTCP.chnlName, "101slave%d", channelNumber);

  /* IP address to accept connections from */
  /* *.*.*.* allows any client to connect */
  strcpy(IOCnfg.linTCP.ipAddress, "*.*.*.*"); 

  /* IP port to listen on */
  IOCnfg.linTCP.ipPort = (TMWTYPES_USHORT)(20000 + channelNumber);
  IOCnfg.linTCP.mode = LINTCP_MODE_SERVER;

  IOCnfg.linTCP.localUDPPort = (TMWTYPES_USHORT)(20000 + channelNumber);
  IOCnfg.linTCP.disconnectOnNewSyn = TMWDEFS_FALSE;
  IOCnfg.linTCP.role = LINTCP_ROLE_OUTSTATION; 
#endif

  sdnpsesn_initConfig(&sesnConfig);

  /*
   * Open the Channel, Sessions, and Sectors
   */
  sprintf(logBuf, "opening DNP channel %d\n", channelNumber);
  myLogOutput(logBuf);
  
  pSclChannel = dnpchnl_openChannel(pThreadApplContext, &DNPConfig, &tprtConfig, 
    &linkConfig, &physConfig, &IOCnfg, &targConfig);

  for (i = 0; i < NUMBER_SESSIONS; i++)
  {
    sprintf(logBuf, "opening DNP session %d\n", i);
    myLogOutput(logBuf);
    
    sesnConfig.destination = (unsigned short)(3 + i);
    sesnConfig.source = (unsigned short)(4 + i);
    pSclSession[i] = (TMWSESN *)sdnpsesn_openSession(pSclChannel, &sesnConfig, TMWDEFS_NULL);
  }

  /*
   * Now that everything is set up, start a "main loop"
   * that sends events.
   */
  while (1)
  {
    /* 
     * Check to see if this per channel thread timer has expired 
     */
    myCheckThreadTimer(pTimer);

    /*
     * Process any data returned by the Slave.
     */
    tmwappl_checkForInput(pThreadApplContext);
   
    if(myTimeToSendEvent(&pMyRequests->lastEvent, pMyRequests->eventInterval))
    {
      for (int i = 0; i < NUMBER_SESSIONS; i++)
      {
        TMWDTIME timeStamp;
        sprintf(logBuf, "Sending event for Channel %d, Session %d\n", channelNumber, i);;
        myLogOutput(logBuf);

        sdnputil_getDateTime(pSclSession[i], &timeStamp);
        sdnpo002_addEvent(pSclSession[i], 4, 0x02, &timeStamp);
        sdnpo022_addEvent(pSclSession[i], 4, 100, 0x02, &timeStamp);
      }
    }
  }

  /* Close sessions and channel */
  for (i = 0; i < NUMBER_SESSIONS; i++)
  {
    sprintf(logBuf, "closing DNP session %d\n", i);
    myLogOutput(logBuf);
    sdnpsesn_closeSession(pSclSession[i]);
  }

  sprintf(logBuf, "closing DNP channel %d\n", channelNumber);
  myLogOutput(logBuf);
  dnpchnl_closeChannel(pSclChannel);

  return (0);
}

/*
 * myInitRequests
 * Initialize sample application request structure
 */
MY_REQUEST_INFO * myInitRequests(void)
{
  MY_REQUEST_INFO *p;
  p = new myRequestInfo;
  p->eventInterval = EVENT_INTERVAL; 
  p->lastEvent = tmwtarg_getMSTime();

  return p;
}

/*
 * myTimeToSendEvent
 * See if it is time to send a request
 *
 * Note that this simplified example does not take into account
 * the fact that the lastGI field could roll over.
 */
TMWTYPES_BOOL myTimeToSendEvent(TMWTYPES_MILLISECONDS *pLastTime, TMWTYPES_MILLISECONDS interval) 
{
  TMWTYPES_MILLISECONDS currentTime;
  TMWTYPES_BOOL returnVal;

  currentTime = tmwtarg_getMSTime();
  if (currentTime >= (*pLastTime + interval))
  {
    *pLastTime = currentTime;
    returnVal = TMWDEFS_TRUE;
  }
  else
  {
    returnVal = TMWDEFS_FALSE;
  }
  return (returnVal);
}
 


/* Sample function to start either Windows or Linux thread */
static void myStartThread(int i)
{
#if TMW_WTK_TARGET
  unsigned int threadId;
  void *threadHandle;
  threadHandle = (void*)_beginthreadex(NULL, 0, myChannelThreadDNP, (void *)i, CREATE_SUSPENDED, &threadId);
  SetThreadPriority(threadHandle, THREAD_PRIORITY_NORMAL);
  ResumeThread(threadHandle);
#endif

#if defined(TMW_LINUX_TARGET) 
  TMW_ThreadId threadId;
  TMW_CreateThread(&threadId, myChannelThreadDNP, (TMW_ThreadArg) i, 0);
#endif
}

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

/* Check to see if this per channel thread timer has expired 
 * If it has, call the SCL timer callback function from this context
 */
void myCheckThreadTimer(MY_TIMER_TYPE *pTimer)
{
  /* Check to see if the per channel timer has expired and call back from here */
#if defined(TMW_WTK_TARGET)
  ULONG waitStatus = WaitForSingleObject((HANDLE)pTimer->timerEvent, 50); 
  if (waitStatus == WAIT_OBJECT_0)
  {
    /* per thread timer expired, call callback from this threads context */
    pTimer->pCallback(pTimer->pCallbackParam);
  }
#endif
#if defined(TMW_LINUX_TARGET) 
  /* 
   * Check to see if the timer has expired for this thread and call back from here 
   */
  for(int i=0; i<5; i++)
  {
    if(pTimer->isExpired)
    {
      /* per thread timer expired, call callback from this threads context */
      pTimer->isExpired = TMWDEFS_FALSE;
      pTimer->pCallback(pTimer->pCallbackParam);
      break;
    }
    else
      /* sleep for 10 milliseconds and then check again */
      mySleep(10);
  }
#endif
}

/* Sample function that is called when system timer expires. 
 * It will determine if any of the per channel timers has expired.
 * You could instead use a per channel system timer
 */ 
static void myAlarmWakeup()
{
  int i;
  // tmwdiag_message( TMWDEFS_NULL,TMWDEFS_NULL, TMWDEFS_NULL, TMWDIAG_ID_ERROR, "alarm wakeup");
  for(i=0; i<NUMBER_CHANNELS; i++)
  {
    MY_TIMER_TYPE *pTimer = &myThreadTimer[i];
    if(myThreadTimer[i].isActive)
    { 
      TMWTYPES_ULONG remainingTime = myThreadTimer[i].timeout - tmwtarg_getMSTime();
      if(remainingTime > TMWPLTMR_MAX_SOFT_TIMER_DELAY)
      {
        remainingTime = 0UL;
      }
      if(remainingTime == 0UL)
      {
        /* Could call the per channel timer directly from this context */
        /* pTimer->pCallback(pTimer->pCallbackParam);                  */

        /* Instead, set an event or expired flag to let the channel thread call timer callback */
#if defined(TMW_WTK_TARGET) 
        SetEvent((HANDLE)myThreadTimer[i].timerEvent);
#endif
#if defined(TMW_LINUX_TARGET) 
        /* Mark the per channel timer as expired, so that the callback will run in the context of the channel thread */
        pTimer->isExpired = TMWDEFS_TRUE;    
#endif
      }
    }
  }
}

#if TMW_WTK_TARGET
/* Get pointer to a simple "timer" */
MY_TIMER_TYPE *myGetTimer(int channelNumber)
{  
  myThreadTimer[channelNumber].timerEvent = (int)CreateEvent(NULL,FALSE,FALSE,NULL);
  return(&myThreadTimer[channelNumber]);
}
void CALLBACK  myTimerCallback(UINT a, UINT b, DWORD c, DWORD d, DWORD e) 
{
  myAlarmWakeup();
}
#endif

#if defined(TMW_LINUX_TARGET) 
/* Get pointer to a simple "timer" */
MY_TIMER_TYPE *myGetTimer(int channelNumber)
{  
  return(&myThreadTimer[channelNumber]);
}
static void myTimerCallback(int j)
{
  myAlarmWakeup();
}
#endif

/* This will start a single system timer for Windows or Linux */
static void myStartSystemTimer()
{
#if TMW_WTK_TARGET
  /* This is a multimedia timer, it will fire every 50 milliseconds */
  timeSetEvent(50, 50, myTimerCallback, 0, TIME_PERIODIC); 

  /* This fires every 500 milliseconds at its fastest 
   * it also requires GetMessage/DispatchMessage 
   * SetTimer(NULL, 2, 50, myTimerCallback);
   */
#endif

#if defined(TMW_LINUX_TARGET) 
  struct itimerval tout_val;
  tout_val.it_interval.tv_sec = 0;
  tout_val.it_interval.tv_usec = 50000;
  tout_val.it_value.tv_sec = 0;
  tout_val.it_value.tv_usec = 50000; /* 50 milliseconds */
  setitimer(ITIMER_REAL, &tout_val, 0);

  /* set the Alarm signal capture */
  signal(SIGALRM, myTimerCallback);
#endif
}

/* These per thread timer functions are called from tmwtargp.c if TMWCNFG_MULTIPLE_TIMER_QS is defined.
 * Do not use these functions, you should add support for starting and cancelling timers in tmwtarg.c
 * (or in tmwtargp.c if using winiotarg or liniotarg)
 */
extern "C"
{
/* function: tmwtest_startMultiTimer */
void * TMWDEFS_GLOBAL tmwtest_startMultiTimer(
  void                   *pHandle,
  TMWTYPES_MILLISECONDS   msTimeout,
  TMWTYPES_CALLBACK_FUNC  pCallback, 
  void                   *pCallbackParam)
{
  MY_TIMER_TYPE *pTimer = (MY_TIMER_TYPE *)pHandle;

  pTimer->pCallback      = pCallback;
  pTimer->pCallbackParam = pCallbackParam;
  pTimer->timeout        = tmwtarg_getMSTime() + msTimeout;
  pTimer->isActive       = TMWDEFS_TRUE; 

  return(pTimer);
}

  /* function: tmwtest_cancelMultiTimer */
void TMWDEFS_GLOBAL tmwtest_cancelMultiTimer(
  void *pHandle)
{
  MY_TIMER_TYPE *pTimer = (MY_TIMER_TYPE *)pHandle;
  pTimer->isActive = TMWDEFS_FALSE;
}
}

/* Simple output function for log messages */
void myLogOutput(char *pBuf)
{
  /* to stdout */
  printf("%s",pBuf);
}

#if TMWCNFG_SUPPORT_DIAG
/* Simple diagnostic output function, registered with the Source Code Library */
void myPutDiagString(const TMWDIAG_ANLZ_ID *pAnlzId,const TMWTYPES_CHAR *pString)
{
  TMWDIAG_ID id = pAnlzId->sourceId;

  if((TMWDIAG_ID_ERROR & id) 
    ||(TMWDIAG_ID_APPL & id)
    ||(TMWDIAG_ID_USER & id))
  {
    myLogOutput((char *)pString);  
    return;
  }

  /* Comment this out to turn off verbose diagnostics */
  /* For now print everything */
  /* myLogOutput((char *)pString); */
}
#endif
#endif