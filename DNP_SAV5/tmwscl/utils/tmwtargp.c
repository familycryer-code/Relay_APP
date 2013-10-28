/*****************************************************************************/
/* Triangle MicroWorks, Inc.                        Copyright (c) 1997-2011  */
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


/* file: tmwtargp.cpp
 * description: "private" implementation of target routines defined in 
 *  tmwtarg.h and tmwtargp.h for use with WinIoTarg, linuxio or Rabbit board.
 */
#include "tmwscl/utils/tmwtarg.h"
#include "tmwscl/utils/tmwcnfg.h"
#include "tmwscl/utils/tmwsim.h"
#include "tmwscl/utils/tmwpltmr.h"

#if defined(TMW_WTK_TARGET) 
/* Use Triangle MicroWorks, Inc. Windows Toolkit target implementation
 * This target implementation is available for customers whose target
 * platform is a Microsoft Windows platform. In some cases this
 * target implementation is also useful as a reference when porting
 * to other platforms.
 */
#include "WinIoTarg/include/WinIoTarg.h"
#include <stdlib.h>
#if _MSC_VER > 1400
#include <strsafe.h>
#else
#include <stdarg.h>
#endif
#endif

#if defined(TMW_LINUX_TARGET)
#include <time.h>

#include "LinIoTarg/liniotarg.h"
#endif

#if defined(TMW_WCE_TARGET)
/* Windows CE is not provided or supported by TMW. 
 * However, if you choose to write your own WinCeTarg_xxx functions 
 * you could define TMW_WCE_TARGET to provide a starting point.
 */
#include "WinIoCeTarg/include/WinIoTarg.h"
#endif

#if defined(TMW_RABBIT)
#include "rabbit/RabIoTarg/rabttarg.h"
#endif

#if defined(TMW_PRIVATE) && defined(TMWCNFG_INCLUDE_ASSERTS) && defined(_MSC_VER)
void TMWAssertion(const char *expr, const char *fileName, int lineNum)
{
#if _MSC_VER > 1400
  char buf[1024];
  
  StringCchPrintfA
    (
    buf,
    1024,
    "%s\nFile: %s, Line: %d",
    expr,
    fileName,
    (int) lineNum
    );
#endif
  
  /*
  MessageBox
    (
    NULL,
    (LPCTSTR)buf,
    (LPCTSTR)"TMW Assertion Failed. Breaking!",
    MB_OK | MB_ICONSTOP
    );
  */

  //DebugBreak();
  
  return;
}
#endif /* TMWCNFG_INCLUDE_ASSERTS */


#if defined(__GNUC__) && defined(TMW_WTK_TARGET)
extern TMWTYPES_INT _vsnprintf(TMWTYPES_CHAR *string, size_t maxlen, const TMWTYPES_CHAR *format, va_list args);
#endif

#if defined(TMW_WTK_TARGET) || defined(TMW_RABBIT) || defined(TMW_LINUX_TARGET) || defined(TMW_WCE_TARGET)

#if defined(TMW_WTK_TARGET)
/* function: _failureRequested
 * purpose: check to see if this context has been set to fail
 *          for testing purposes
 * arguments:
 *  pContext - pointer to physical layer context
 * returns
 *  TMWDEFS_TRUE if should fail, else
 *  TMWDEFS_FALSE if not failure
 */
static TMWTYPES_BOOL TMWDEFS_GLOBAL _xmtFailureRequested(
  void *pContext);
#endif

/* Function implementations for WinIOTarg, linuxio, Windows CE, and Rabbit board follow */

/* function: tmwtarg_alloc */
void * TMWDEFS_GLOBAL tmwtarg_alloc(TMWTYPES_UINT numBytes)
{
#if TMWCNFG_USE_DYNAMIC_MEMORY
#if defined(TMW_LINUX_TARGET)
  return( (void *) malloc( numBytes ) );
#else
  return (malloc(numBytes));
#endif
#else
  TMWTARG_UNUSED_PARAM(numBytes);
  return (TMWDEFS_NULL);
#endif
}

/* function: tmwtarg_calloc */
void * TMWDEFS_GLOBAL tmwtarg_calloc(TMWTYPES_UINT num, TMWTYPES_UINT size)
{
#if TMWCNFG_USE_DYNAMIC_MEMORY
  return (calloc(num, size));
#else
  TMWTARG_UNUSED_PARAM(num);
  TMWTARG_UNUSED_PARAM(size);
  return (TMWDEFS_NULL);
#endif
}

/* function: tmwtarg_free */
void TMWDEFS_GLOBAL tmwtarg_free(void *pBuf)
{
#if TMWCNFG_USE_DYNAMIC_MEMORY
#if defined(TMW_LINUX_TARGET)
  free( (char *) pBuf );
#else
  free(pBuf);
#endif
#else
  TMWTARG_UNUSED_PARAM(pBuf);
#endif
  return;
}

#if defined(TMW_LINUX_TARGET)
TMWTYPES_INT TMWDEFS_GLOBAL tmwtarg_snprintf(
  TMWTYPES_CHAR *buf,
  TMWTYPES_UINT count,
  const TMWTYPES_CHAR *format,
  ...)
{
  va_list va;
  TMWTYPES_INT len;

  va_start(va, format);
#if TMWCNFG_HAS_VSNPRINTF
  (void)vsnprintf(buf, count, format, va);
#else
  (void)vsprintf(buf, format, va);
#endif
  va_end(va);
  len = strlen(buf); /* some sprintf's don't return the number of bytes written */
  if (len <= 0)
    return (0);
  return (len);
}
#else

/* function: tmwtarg_snprintf */
TMWTYPES_INT TMWDEFS_GLOBAL tmwtarg_snprintf(
  TMWTYPES_CHAR *buf,
  TMWTYPES_UINT count,
  const TMWTYPES_CHAR *format,
  ...)
{
#if _MSC_VER > 1400
  va_list va;
  TMWTYPES_INT len;
#if TMWCNFG_HAS_VSNPRINTF
#else
  TMWTARG_UNUSED_PARAM(count);
#endif

  va_start(va, format);
#if TMWCNFG_HAS_VSNPRINTF
  (void)StringCchVPrintfA(buf, count, format, va);
#else
  (void)vsprintf(buf, format, va);
#endif
  va_end(va);
  len = strlen(buf); /* some sprintf's don't return the number of bytes written */

  ASSERT(len<(TMWTYPES_INT)count); /* if count == len then we may have a buffer overrun */

  if (len <= 0)
    return (0);
  return (len);
  
#elif defined(_MSC_VER)
  va_list args;
  TMWTYPES_INT len;    
  va_start( args, format );
    
  len = vsprintf( buf, format, args );

  if (len <= 0)
    return (0);
  return (len);
#elif defined(__GNUC__)
  __VALIST va;
  TMWTYPES_INT len;
#if TMWCNFG_HAS_VSNPRINTF
#else
  TMWTARG_UNUSED_PARAM(count);
#endif

  va_start(va, format);
#if TMWCNFG_HAS_VSNPRINTF
  (void)_vsnprintf(buf, count, format, va);
#else
  (void)vsprintf(buf, format, va);
#endif
  va_end(va);
  len = strlen(buf); /* some sprintf's don't return the number of bytes written */
  if (len <= 0)
    return (0);
  return (len);
#elif defined(TMW_RABBIT)
  va_list va;
  TMWTYPES_INT len;
  TMWTARG_UNUSED_PARAM(count);

  va_start(va, format);
  (void)vsprintf(buf, format, va);
  va_end(va);
  len = strlen(buf); /* some sprintf's don't return the number of bytes written */
  if (len <= 0)
    return (0);
  return (len);
#else
  /* Put your code here */
  TMWTARG_UNUSED_PARAM(buf);
  TMWTARG_UNUSED_PARAM(count);
  TMWTARG_UNUSED_PARAM(format);
  return (0);
#endif
}
#endif

#if TMWCNFG_SUPPORT_DIAG

#if defined(TMW_WTK_TARGET)
/* Function to convert WinIoTarg logging to match rest of SCL */
void tmwtargp_WinIoTargLogFunc(void * pChannel, TMWDIAG_ID sourceId, const char *_id, const char *format, va_list ap)
{
#if _MSC_VER > 1400
  int length;
  char strText[1024];
  strcpy_s(strText, 1024, _id);
  length = strlen(_id); 
  vsnprintf_s(strText+length, (1024-length), _TRUNCATE, format, ap);    
  TMWDIAG_TARGET(pChannel, sourceId, strText);
#endif
}
#endif

static TMWTARGP_PUT_DIAG_STRING_FUNC pPutDiagStringFunc = NULL;
/* function: tmwtargp_registerPutDiagStringFunc */
void TMWDEFS_GLOBAL tmwtargp_registerPutDiagStringFunc(
  TMWTARGP_PUT_DIAG_STRING_FUNC pFunc)
{
  pPutDiagStringFunc = pFunc;

#if defined(TMW_WTK_TARGET)
  /* Register to receive logging from WinIoTarg. 
   * If application calls WinIoTarg_setProtoAnaLogFun this will be overwritten by their function 
   */
  WinIoTarg_setProtoAnaLogFun(tmwtargp_WinIoTargLogFunc);
#endif
}

/* function: tmwtarg_putDiagString */
void TMWDEFS_GLOBAL tmwtarg_putDiagString(
  const TMWDIAG_ANLZ_ID *pAnlzId,
  const TMWTYPES_CHAR *pString)
{
/* Log a diagnostic string to a console, file, etc. The pAnlzId
 * structure might be used to filter the display if desired.
 */
  if (pPutDiagStringFunc)
  {
    pPutDiagStringFunc(pAnlzId,pString);
    return;
  }

#if defined(TMW_LINUX_TARGET)
  /* Use printf for now */
  printf("%s", pString);
#endif

#if defined(TMW_RABBIT)
  rabttarg_putDiagString(pAnlzId, pString);
#endif /* defined(TMW_RABBIT) */
}
#endif /* TMWCNFG_SUPPORT_DIAG */

#if TMWCNFG_USE_SIMULATED_DB
static TMWTARGP_EXE_CMD_FUNC pExeCmdFunc = NULL;
/* function: tmwtargp_registerExeCmd */
void TMWDEFS_GLOBAL tmwtargp_registerExeCmd(
  TMWTARGP_EXE_CMD_FUNC pFunc)
{
  pExeCmdFunc = pFunc;
}

/* function: tmwtarg_executeCommand 
 */
void tmwtargp_executeCommand(
    void *pCommandContext,
  TMWTYPES_UCHAR *pCommandBuf,
  TMWTYPES_UCHAR bufLength)
{
 /* Execute a command
  */
  if (pExeCmdFunc)
  {
    pExeCmdFunc(pCommandContext, pCommandBuf, bufLength);
  }
}
#endif /* TMWCNFG_USE_SIMULATED_DB */

/* function: tmwtargp_Sleep */
void TMWDEFS_GLOBAL tmwtargp_Sleep(TMWTYPES_MILLISECONDS time)
{
/* Put the process to sleep for 'time' milliseconds
 */
#if defined(TMW_WTK_TARGET)
  WinIoTarg_Sleep(time);
#endif
#if defined(TMW_LINUX_TARGET) 
  Sleep(time);
#endif
#if defined(TMW_WCE_TARGET)
  WinCeTarg_Sleep(time);
#endif
}

#if TMWCNFG_SUPPORT_THREADS

#if defined(TMW_LINUX_TARGET)
extern int pthread_mutexattr_setkind_np(pthread_mutexattr_t *attr, int kind);

int _LinuxSemCreate(pthread_mutex_t **sem)
{
  pthread_mutexattr_t initializer;
  pthread_mutex_t *newSem;
  newSem = (pthread_mutex_t *) malloc(sizeof(pthread_mutex_t));
  if (newSem == NULL)
    return(0);
  pthread_mutexattr_init(&initializer);
  pthread_mutexattr_setkind_np(&initializer, PTHREAD_MUTEX_RECURSIVE_NP);
  pthread_mutex_init(newSem, &initializer);
  pthread_mutexattr_destroy(&initializer);
  *sem = newSem;
  return(1);
}

int _LinuxSemDelete(pthread_mutex_t **sem)
{   
  free(*sem);
  *sem = NULL;
  return(1);
}
#endif

void tmwtarg__lockInit(TMWDEFS_RESOURCE_LOCK *pLock)
{
#if defined(TMW_WTK_TARGET)
  WinIoTarg_lockInit(pLock);

#elif defined(TMW_LINUX_TARGET)
  TMW_TP_Semaphore sem;
  _LinuxSemCreate(&sem);
  *pLock = (TMWDEFS_RESOURCE_LOCK *) sem;

#else
  *pLock = (CRITICAL_SECTION *)malloc(sizeof(CRITICAL_SECTION));
  InitializeCriticalSection((CRITICAL_SECTION*)*pLock);
#endif
}

 /* This function is only required for 104 redundancy with a multi-threaded 
  * architecture. It will allow the use of a single lock for the redundancy 
  * group as well as the redundant connection channels.
  */
void tmwtarg__lockShare(TMWDEFS_RESOURCE_LOCK *pLock, TMWDEFS_RESOURCE_LOCK *pLock1)
{
  *pLock1 = *pLock;
}

void tmwtarg__lockSection(TMWDEFS_RESOURCE_LOCK *pLock)
{
#if defined(TMW_WTK_TARGET)
  WinIoTarg_lockSection(pLock);

#elif defined(TMW_LINUX_TARGET)
  TMW_TP_TakeSem(*((TMW_TP_Semaphore *) pLock));

#else
  EnterCriticalSection((CRITICAL_SECTION*)*pLock);
#endif
}

void tmwtarg__unlockSection(TMWDEFS_RESOURCE_LOCK *pLock)
{
#if defined(TMW_WTK_TARGET)
  WinIoTarg_unlockSection(pLock);

#elif defined(TMW_LINUX_TARGET)
  TMW_TP_ReleaseSem(*((TMW_TP_Semaphore *) pLock));

#else
  LeaveCriticalSection((CRITICAL_SECTION*)*pLock);
#endif
}

void tmwtarg__lockDelete(TMWDEFS_RESOURCE_LOCK *pLock)
{
#if defined(TMW_WTK_TARGET)
  WinIoTarg_lockDelete(pLock);

#elif defined(TMW_LINUX_TARGET)
  _LinuxSemDelete(((TMW_TP_Semaphore *) pLock));

#else
  DeleteCriticalSection((CRITICAL_SECTION*)*pLock);
  free(*pLock);
#endif
}
#endif /* TMWCNFG_SUPPORT_THREADS */

#if defined(TMW_LINUX_TARGET)

/* function: tmwtarg_getDateTime */
void TMWDEFS_GLOBAL tmwtarg_getDateTime(
  TMWDTIME *pDateTime)
{
  struct tm     *pTime;
  struct timeval tv;

  /* Current time, is always geniune, not substituted */
  pDateTime->genuineTime = TMWDEFS_TRUE;
  pDateTime->invalid = TMWDEFS_FALSE;

  gettimeofday(&tv, NULL);
  pTime = localtime(&tv.tv_sec);

  pDateTime->year = pTime->tm_year + 1900;  /* tm_year is year since 1900 */
  pDateTime->month = pTime->tm_mon + 1;     /* tm_month is 0..11          */

  /* tm_wday is 0-7 Sunday-Saturday, dayOfWeek is 1-7 Monday-Sunday, 0 is not used */
  if(pTime->tm_wday == 0)
    pDateTime->dayOfWeek = 7;
  else
    pDateTime->dayOfWeek = pTime->tm_wday;

  pDateTime->dayOfMonth = pTime->tm_mday;
  pDateTime->hour = pTime->tm_hour;
  pDateTime->minutes = pTime->tm_min;
  pDateTime->dstInEffect = pTime->tm_isdst;
  pDateTime->mSecsAndSecs = (pTime->tm_sec * 1000) + (tv.tv_usec / 1000);
}

/* function: tmwtarg_setDateTime */
TMWTYPES_BOOL TMWDEFS_GLOBAL tmwtarg_setDateTime(
  TMWDTIME *pDateTime)
{
  struct tm timeToSet;
  time_t wrTime;

  timeToSet.tm_year = pDateTime->year - 1900;
  timeToSet.tm_mon = (int)pDateTime->month - 1;
  timeToSet.tm_mday = pDateTime->dayOfMonth;
  timeToSet.tm_hour = pDateTime->hour;
  timeToSet.tm_min = pDateTime->minutes;
  timeToSet.tm_sec = pDateTime->mSecsAndSecs / 1000;

  wrTime = mktime(&timeToSet);
  
  if( stime(&wrTime) )
    perror( "Time set failed.\n" );

  return(TMWDEFS_TRUE);
}
#else

#if defined(TMW_WTK_TARGET)
static TMWTARGP_GET_DATETIME_FUNC pGetDateTimeFunc = NULL;
void TMWDEFS_GLOBAL tmwtargp_registerGetDateTimeFunc(TMWTARGP_GET_DATETIME_FUNC pFunc)
{
  pGetDateTimeFunc = pFunc;
} 

/* function: _getDateTime */
void TMWDEFS_LOCAL _getDateTime(
  TMWDTIME *pDateTime)
{
  /* Return the time for this session */
  if (pGetDateTimeFunc)
  {  
    pGetDateTimeFunc(pDateTime);
    return;
  }
  WinIoTarg_getDateTime(pDateTime); 
}


static TMWTARGP_SET_DATETIME_FUNC pSetDateTimeFunc = NULL;
void TMWDEFS_GLOBAL tmwtargp_registerSetDateTimeFunc(TMWTARGP_SET_DATETIME_FUNC pFunc)
{
  pSetDateTimeFunc = pFunc;
} 

/* function: _setDateTime */
TMWTYPES_BOOL TMWDEFS_LOCAL _setDateTime(
  TMWDTIME *pDateTime)
{
  /* Return the time for this session */
  if (pSetDateTimeFunc)
  {  
    return(pSetDateTimeFunc(pDateTime));
  } 
  return (WinIoTarg_setDateTime(pDateTime));
}
#endif


/* function: tmwtarg_getDateTime */
void TMWDEFS_GLOBAL tmwtarg_getDateTime(
  TMWDTIME *pDateTime)
{
  /* Current time, is always genuine, not substituted */
  pDateTime->genuineTime = TMWDEFS_TRUE; 
  pDateTime->invalid = TMWDEFS_FALSE; 

/* Get the current system date and time and store into a TMWDTIME
 * structure. The TMWDTIME structure is defined in tmwdtime.h.
 */
#if defined(TMW_WTK_TARGET)  
  _getDateTime(pDateTime);
#endif 
#if defined(TMW_RABBIT)
  rabttarg_getDateTime(pDateTime);
#endif /* defined(TMW_RABBIT) */
#if defined(TMW_WCE_TARGET)
  WinCeTarg_getDateTime(pDateTime);
#endif 
}

/* function: tmwtarg_setDateTime */
TMWTYPES_BOOL TMWDEFS_GLOBAL tmwtarg_setDateTime(
  TMWDTIME *pDateTime)
{
/* Set the current system date and time. This routine is only called
 * on slave devices as the result of a clock synchronization request.
 * For devices where it is not desirable to change the system time
 * as the result of a clock synchronization, it may be desirable to
 * implement a system that reads the system time at startup and uses
 * this time as a time base, maintaining the current time by using
 * the millisecond timer, until tmwtarg_setDateTime is called, after
 * which the new time is used as a time base.
 */
#if defined(TMW_WTK_TARGET)
  return (_setDateTime(pDateTime));
#endif 
#if defined(TMW_RABBIT)
  return (rabttarg_setDateTime(pDateTime));
#endif 
#if defined(TMW_WCE_TARGET)
  return (WinCeTarg_setDateTime(pDateTime));
#endif 
}
#endif

/* function: tmwtarg_getMSTime */
TMWTYPES_MILLISECONDS TMWDEFS_GLOBAL tmwtarg_getMSTime(void)
{
/* The returned values must use the entire 32 bit dynamic range
 * but do not require millisecond resolution(i.e. it is not
 * important that the value always increment by one).
 */
#if defined(TMW_WTK_TARGET)
  return (WinIoTarg_getMsTime());
#endif /* defined(TMW_WTK_TARGET) */

#if defined(TMW_LINUX_TARGET)
    TMWTYPES_MILLISECONDS temp;
    struct timespec curr_value;

    clock_gettime( CLOCK_MONOTONIC, &curr_value);
    temp = ( curr_value.tv_sec * 1000 ) + ( ( curr_value.tv_nsec / 1000 ) % 1000 ) ;

    return((TMWTYPES_MILLISECONDS)temp);
#endif

#if defined(TMW_RABBIT)
  return (rabttarg_getMSTime());
#endif /* defined(TMW_RABBIT) */

#if defined(TMW_WCE_TARGET)
  return (WinCeTarg_getMsTime());
#endif
}

#if !TMWCNFG_MULTIPLE_TIMER_QS 
static TMWTARGP_START_TIMER_FUNC pStartTimerFunc = NULL;
void TMWDEFS_GLOBAL tmwtargp_registerStartTimerFunc(TMWTARGP_START_TIMER_FUNC pFunc)
{
  pStartTimerFunc = pFunc;
}

static TMWTARGP_CANCEL_TIMER_FUNC pCancelTimerFunc = NULL;
void TMWDEFS_GLOBAL tmwtargp_registerCancelTimerFunc(TMWTARGP_CANCEL_TIMER_FUNC pFunc)
{
  pCancelTimerFunc = pFunc;
}

/* function: tmwtarg_startTimer() */
void TMWDEFS_GLOBAL tmwtarg_startTimer(
  TMWTYPES_MILLISECONDS timeout,
  TMWTYPES_CALLBACK_FUNC pCallbackFunc,
  void *pCallbackParam)
{
/* Only one timer is required since the source code library will
 * maintain a list of pending timers and only require an external
 * timer for the timer that will expire in the shortest time.
 * Also, a polled timer is implemented in tmwpltmr.h/c that can
 * be used for systems that do not support an event timer. The
 * default implementation can use the polled timer. 
 */
  if (pStartTimerFunc)
  {
    pStartTimerFunc(timeout, pCallbackFunc, pCallbackParam);
  }
  else
  {
    tmwpltmr_startTimer(timeout, pCallbackFunc, pCallbackParam);
  }
}

/* function: tmwtarg_cancelTimer */
void TMWDEFS_GLOBAL tmwtarg_cancelTimer(void)
{
/* Cancel the current event timer, no further timer callbacks
 * will be invoked. The default implementation uses the polled
 * timer. 
 */
  if (pCancelTimerFunc)
  {
    pCancelTimerFunc();
  }
  else
  {
    tmwpltmr_cancelTimer();
  }
}
#else /* TMWCNFG_MULTIPLE_TIMER_QS */
 
#ifdef __cplusplus
extern "C" {
#endif

/* These two functions are contained in the code samples that are shipped with the Source Code Libraries */
void * TMWDEFS_GLOBAL tmwtest_startMultiTimer(
  void                   *pHandle,
  TMWTYPES_MILLISECONDS   msTimeout,
  TMWTYPES_CALLBACK_FUNC  pCallback,            
  void                   *pCallbackParam);

void TMWDEFS_GLOBAL tmwtest_cancelMultiTimer(
  void *handle);

#ifdef __cplusplus
}
#endif

/* function: tmwtarg_startMultiTimer */
void * TMWDEFS_GLOBAL tmwtarg_startMultiTimer(
  void                   *pHandle,
  TMWTYPES_MILLISECONDS   timeout, 
  TMWTYPES_CALLBACK_FUNC  pCallbackFunc, 
  void                   *pCallbackParam)
{
  return(tmwtest_startMultiTimer(pHandle, timeout, pCallbackFunc, pCallbackParam));
}

/* function: tmwtarg_cancelMultiTimer */
void TMWDEFS_GLOBAL tmwtarg_cancelMultiTimer(
  void *timerHandle)
{
  tmwtest_cancelMultiTimer(timerHandle);
}
#endif

/* function: tmwtarg_initChannel */
void * TMWDEFS_GLOBAL tmwtarg_initChannel(
  const void *pUserConfig,
  TMWTARG_CONFIG *pTmwConfig,
  TMWCHNL *pChannel)
{
/* Initialize, but not necessarily open a new communication
 * channel. The pConfig pointer is passed to the source code
 * library from the user and not manipulated or changed in any
 * way. It is solely for the use of the users implementation
 * and should be used to provide any required channel id or
 * configuration information to the target implementation.
 * The pChannelCallback function should be called if the channel
 * receives any kind of open or close notification from the operating
 * system. This callback lets the source code library know that
 * it must attempt to reopen the channel.
 * This routine returns a pointer to some kind of context that
 * will be passed to all of the remaining channel target functions.
 * The source code library does not change or manipulate this
 * pointer in any way. The pointer can not be NULL since this
 * is interpreted as a failure.
  */
#if defined(TMW_WTK_TARGET)
  void *pCallBackParam;
  TMWCHNL_IDLE_CALLBACK pCallBackFun;
  void *pContext = WinIoTarg_Create(pUserConfig, pTmwConfig);
  if(pContext != TMWDEFS_NULL)
  {
    WinIoTarg_getIdleCallBack(pContext, &pCallBackFun, &pCallBackParam);
    if(pCallBackFun != TMWDEFS_NULL)
      tmwchnl_setIdleCallback(pChannel, pCallBackFun, pCallBackParam);
  }
  return (pContext);
#endif

#if defined(TMW_LINUX_TARGET) 
  TMWTARG_UNUSED_PARAM(pChannel);
  return (liniotarg_initChannel(pUserConfig, pTmwConfig));
#endif

#if defined(TMW_RABBIT)
  TMWTARG_UNUSED_PARAM(pChannel);
  return (rabttarg_initChannel(pUserConfig, pTmwConfig->pChannelCallback, pTmwConfig->pCallbackParam));
#endif

#if defined(TMW_WCE_TARGET)
  void *pContext = WinCeTarg_Create(pUserConfig, pTmwConfig);
  if(pContext != TMWDEFS_NULL)
  {
    tmwchnl_setIdleCallback(pChannel, NULL, NULL);
  }
  return (pContext);
#endif
}

/* function: tmwtarg_deleteChannel */
void TMWDEFS_GLOBAL tmwtarg_deleteChannel(
  void *pContext)
{
/* Delete a channel that was previously initialized, freeing
 * any memory and resources.
 */
#if defined(TMW_WTK_TARGET)
  WinIoTarg_Destroy(pContext);
#endif

#if defined(TMW_LINUX_TARGET) 
  liniotarg_deleteChannel(pContext);
#endif

#if defined(TMW_RABBIT)
  rabttarg_deleteChannel(pContext);
#endif 

#if defined(TMW_WCE_TARGET)
  WinCeTarg_Destroy(pContext);
#endif
}

/* function: tmwtarg_getChannelName */
const TMWTYPES_CHAR * TMWDEFS_GLOBAL tmwtarg_getChannelName(
  void *pContext)
{
/* Return the name for this channel. Only used for diagnostics.
 */
#if defined(TMW_WTK_TARGET)
  return (WinIoTarg_getChannelName(pContext));
#endif /* defined(TMW_WTK_TARGET) */

#if defined(TMW_LINUX_TARGET) 
 return liniotarg_getChannelName(pContext);
#endif

#if defined(TMW_RABBIT)
  return (rabttarg_getChannelName(pContext));
#endif

#if defined(TMW_WCE_TARGET)
  return (WinCeTarg_getChannelName(pContext));
#endif
}

/* function: tmwtarg_getChannelInfo */
const TMWTYPES_CHAR * TMWDEFS_GLOBAL tmwtarg_getChannelInfo(
  void *pContext)
{
/* Return user defined information string for this channel.
 * This string is only used for diagnostics.
 */
#if defined(TMW_WTK_TARGET)
  return (WinIoTarg_getChannelInfo(pContext));
#endif

#if defined(TMW_LINUX_TARGET) 
 return liniotarg_getChannelInfo(pContext);
#endif

#if defined(TMW_RABBIT)
  return (rabttarg_getChannelInfo(pContext));
#endif

#if defined(TMW_WCE_TARGET)
  return (WinCeTarg_getChannelInfo(pContext));
#endif
}

/* function: tmwtarg_openChannel */
TMWTYPES_BOOL TMWDEFS_GLOBAL tmwtarg_openChannel(
  void *pContext,
  TMWTARG_CHANNEL_RECEIVE_CBK_FUNC pChnlRcvCallbackFunc,
  TMWTARG_CHECK_ADDRESS_FUNC pCheckAddrCallbackFunc,
  void *pCallbackParam)
{
/* Attempt to open this channel. Return TMWDEFS_TRUE
 * if the channel is successfully opened, else TMWDEFS_FALSE.
 */
#if defined(TMW_WTK_TARGET)
  WinIoTarg_setUseConnectorThread(pContext, TMWDEFS_TRUE);
  return(WinIoTarg_openChannel(pContext, pChnlRcvCallbackFunc, pCheckAddrCallbackFunc, pCallbackParam));
#endif

#if defined(TMW_LINUX_TARGET) 
  return(liniotarg_openChannel(pContext, pChnlRcvCallbackFunc, pCheckAddrCallbackFunc, pCallbackParam));
#endif

#if defined(TMW_RABBIT)
  return (rabttarg_openChannel(pContext));
#endif

#if defined(TMW_WCE_TARGET)
  return(WinCeTarg_openChannel(pContext, pChnlRcvCallbackFunc, pCheckAddrCallbackFunc, pCallbackParam));
#endif
}

/* function: tmwtarg_closeChannel */
void TMWDEFS_GLOBAL tmwtarg_closeChannel(
  void *pContext)
{
/* Close a previously opened channel
 */
#if defined(TMW_WTK_TARGET)
  WinIoTarg_closeChannel(pContext);
#endif

#if defined(TMW_LINUX_TARGET) 
  return(liniotarg_closeChannel(pContext));
#endif

#if defined(TMW_RABBIT)
  rabttarg_closeChannel(pContext);
#endif

#if defined(TMW_WCE_TARGET)
  WinCeTarg_closeChannel(pContext);
#endif
}

/* function: tmwtargp_resetChannel */
TMWTYPES_BOOL TMWDEFS_GLOBAL tmwtargp_resetChannel(
  void *pContext)
{
  /* Reset a previously opened channel
   */
#if defined(TMW_WTK_TARGET)
  return WinIoTarg_resetChannel(pContext);
#endif

#if defined(TMW_WCE_TARGET)
  return WinCeTarg_resetChannel(pContext);
#endif

#if defined(TMW_LINUX_TARGET) 
  TMWTARG_UNUSED_PARAM(pContext);
  return TMWDEFS_FALSE;
#endif
}

#if TMWCNFG_SUPPORT_DIAG
static TMWTARGP_GET_SESSION_NAME_FUNC pGetSessionNameFunc = NULL;
void TMWDEFS_GLOBAL tmwtargp_registerGetSessionNameFunc(TMWTARGP_GET_SESSION_NAME_FUNC pFunc)
{
  pGetSessionNameFunc = pFunc;
}

static TMWTARGP_GET_SECTOR_NAME_FUNC pGetSectorNameFunc = NULL;
void TMWDEFS_GLOBAL tmwtargp_registerGetSectorNameFunc(TMWTARGP_GET_SECTOR_NAME_FUNC pFunc)
{
  pGetSectorNameFunc = pFunc;
}

/* function: tmwtarg_getSessionName */
const TMWTYPES_CHAR * TMWDEFS_GLOBAL tmwtarg_getSessionName(
  TMWSESN *pSession)
{
  /* Return the name for this session. Only used for diagnostics.
   */
  if (pGetSessionNameFunc)
  {
    return pGetSessionNameFunc(pSession);
  }
  return (tmwchnl_getChannelName(pSession->pChannel));
}

/* function: tmwtarg_getSectorName */
const TMWTYPES_CHAR * TMWDEFS_GLOBAL tmwtarg_getSectorName(
  TMWSCTR *pSector)
{
  /* Return the name for this sector. Only used for diagnostics.
   */
  if (pGetSectorNameFunc)
  {
    return pGetSectorNameFunc(pSector);
  }
  return (tmwchnl_getChannelName(pSector->pSession->pChannel));
}
#endif

/* function: tmwtarg_getTransmitReady */
TMWTYPES_MILLISECONDS TMWDEFS_GLOBAL tmwtarg_getTransmitReady(
  void *pContext)
{
/* Return 0 if the channel is currently ready to transmit or the
 * number of milliseconds to wait before checking to see if the
 * channel is ready again. This feature is typically used to
 * provide a delay before starting to transmit data. An example
 * would be to provide a key time required by some radio systems.
 *
 * If a channel ready callback function was registered by the SCL in
 * tmwtarg_initChannel() TMWTARG_CONFIG *pTmwConfig the target layer
 * may choose to call the callback function to indicate the channel
 * was ready sooner than the return value suggested. If the callback
 * function is not called the SCL will retry in the number of milliseconds
 * indicated.
 *
 */
#if defined(TMW_WTK_TARGET)
  return (WinIoTarg_getTransmitReady(pContext));
#endif 

#if defined(TMW_LINUX_TARGET)
  return (liniotarg_getTransmitReady(pContext));
#endif 

#if defined(TMW_RABBIT)
  return (rabttarg_getTransmitReady(pContext));
#endif

#if defined(TMW_WCE_TARGET)
  return (WinCeTarg_getTransmitReady(pContext));
#endif
}

/* function: tmwtarg_receive */
TMWTYPES_USHORT TMWDEFS_GLOBAL tmwtarg_receive(
  void *pContext,
  TMWTYPES_UCHAR *pBuff,
  TMWTYPES_USHORT maxBytes,
  TMWTYPES_MILLISECONDS maxTimeout,
  TMWTYPES_BOOL *pInterCharTimeoutOccurred,
  TMWTYPES_MILLISECONDS *pFirstByteReceived)
{
  TMWTARG_UNUSED_PARAM(pFirstByteReceived);
/* Attempt to read the specified number of bytes from this
 * channel. This routine should read at most maxBytes bytes
 * from this channel into pBuff. Implementation of the
 * interCharacterTimeout is optional.
 *
 * NOTE: For Modbus RTU this function should not return any bytes
 *  until either the entire frame was received or an inter Character Timeout
 *  occurred.
 */
#if defined(TMW_WTK_TARGET)
  return (WinIoTarg_receive(pContext, pBuff,
    maxBytes, maxTimeout, pInterCharTimeoutOccurred));
#endif

#if defined(TMW_LINUX_TARGET) 
  return (liniotarg_receive(pContext, pBuff,
    maxBytes, maxTimeout, pInterCharTimeoutOccurred));
#endif 

#if defined(TMW_RABBIT)
  return (rabttarg_receive(pContext, pBuff,
    maxBytes, maxTimeout, pInterCharTimeoutOccurred));
#endif 

#if defined(TMW_WCE_TARGET)
  return (WinCeTarg_receive(pContext, pBuff,
    maxBytes, maxTimeout, pInterCharTimeoutOccurred));
#endif
}

/* function: tmwtarg_transmit */
TMWTYPES_BOOL TMWDEFS_GLOBAL tmwtarg_transmit(
  void *pContext,
  TMWTYPES_UCHAR *pBuff,
  TMWTYPES_USHORT numBytes)
{
 /* Attempt to transmit numBytes bytes on this channel.
  */
#if defined(TMW_WTK_TARGET)
  /* Simulate a channel failure for test purposes */
  if (_xmtFailureRequested(pContext))
    return TMWDEFS_TRUE;

  return (WinIoTarg_transmit(pContext, pBuff, numBytes));
#endif

#if defined(TMW_LINUX_TARGET) 
  return (liniotarg_transmit(pContext, pBuff, numBytes));
#endif 

#if defined(TMW_RABBIT)
  return (rabttarg_transmit(pContext, pBuff, numBytes));
#endif 

#if defined(TMW_WCE_TARGET)
  return (WinCeTarg_transmit(pContext, pBuff, numBytes));
#endif
}

/* function: tmwtarg_transmitUDP */
TMWTYPES_BOOL TMWDEFS_GLOBAL tmwtarg_transmitUDP(
  void *pContext,
  TMWTYPES_UCHAR UDPPort,
  TMWTYPES_UCHAR *pBuff,
  TMWTYPES_USHORT numBytes)
{
/* Attempt to transmit numBytes bytes on this channel using
 * UDP datagram. 
 * NOTE: This only needs to be implemented for DNP to support
 * the DNP3 Specification IP Networking. It is not required
 * for IEC or modbus and will not be called by those protocols.
 * If DNP3 UDP is not required, this function can simply return TMWDEFS_FALSE
 */
#if defined(TMW_WTK_TARGET)
  /* Simulate a channel failure for test purposes */
  if (_xmtFailureRequested(pContext))
    return TMWDEFS_TRUE;

  return (WinIoTarg_transmitUDP(pContext, UDPPort, pBuff, numBytes));
#endif 

#if defined(TMW_LINUX_TARGET)
  return (liniotarg_transmitUDP(pContext, UDPPort, pBuff, numBytes));
#endif 

#if defined(TMW_RABBIT)
  return (TMWDEFS_FALSE);
#endif

#if defined(TMW_WCE_TARGET)
  return (WinCeTarg_transmitUDP(pContext, UDPPort, pBuff, numBytes));
#endif
}

#define MAX_FAILURE_CONTEXTS 16
static TMWTYPES_INT failureContextCount;
static void *pFailureContext[MAX_FAILURE_CONTEXTS];
static TMWTYPES_USHORT failureCount[MAX_FAILURE_CONTEXTS];

#if defined(TMW_WTK_TARGET)
/* function: _failureRequested */
static TMWTYPES_BOOL TMWDEFS_GLOBAL _xmtFailureRequested(
  void *pContext)
{
  TMWTYPES_INT i;

  /* If it is in the list return whether to fail or not */
  /* Count indicates how many to transmits to fail */
  for (i = 0; i < failureContextCount; i++)
  {
    if (pFailureContext[i] == pContext)
    {
      if(failureCount[i] > 0)
      {
        failureCount[i]--;
        return TMWDEFS_TRUE;
      }

      /* remove this context since it is zero */
      tmwtargp_requestFailure(pContext, 0); 
    }
  }
  return TMWDEFS_FALSE;
}
#endif

/* function: tmwtargp_requestFailure  */
TMWTYPES_BOOL TMWDEFS_GLOBAL tmwtargp_requestFailure(
  void *pContext,
  TMWTYPES_USHORT failureCnt)
{
  TMWTYPES_INT i;

  if (failureCnt == 0)
  {
    /* If context is in list, set the failure status */
    for (i = 0; i < failureContextCount; i++)
    {
      if (pFailureContext[i] == pContext)
      {
        /* remove from list and move all contexts down */
        failureContextCount--;
        for (; i < failureContextCount; i++)
        {
          pFailureContext[i] = pFailureContext[i + 1];
        }
        pFailureContext[i] = TMWDEFS_NULL;
        return TMWDEFS_TRUE;
      }
    }
    return TMWDEFS_FALSE;
  }
  else
  {
    /* See if context is already in the list */
    for (i = 0; i < failureContextCount; i++)
    {
      if (pFailureContext[i] == pContext)
      {
        failureCount[i] = failureCnt;
        return TMWDEFS_TRUE;
      }
    }
    if (i < MAX_FAILURE_CONTEXTS)
    {
      pFailureContext[i] = pContext;
      failureCount[i] = failureCnt;
      failureContextCount++;
      return TMWDEFS_TRUE;
    }

    return TMWDEFS_FALSE;
  }
}
#endif

