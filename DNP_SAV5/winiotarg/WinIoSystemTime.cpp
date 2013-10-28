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
/*
 * FileName : WinIoSystemTime.cpp
 * Author   : Fred VanEijk
 * Purpose  : Implement the simulated time
 *            
 */

#include "stdafx.h"
#include "WinIoTarg/WinIoInterface.h"
#include "WinIoTarg/WinIoSystemTime.h"
#include "WinIoTarg/include/WinIoTarg.h"

#ifdef _DEBUG
#define _CRTDBG_MAP_ALLOC
#include <stdlib.h>
#include <crtdbg.h>
#endif

/* function: getDateTime */
void WinIoSystemTime::getDateTime(TMWDTIME *pDateTime)
{
  SYSTEMTIME localTime;
  GetLocalTime(&localTime);
  pDateTime->year = localTime.wYear;
  pDateTime->month = (TMWTYPES_UCHAR)localTime.wMonth;
  pDateTime->dayOfMonth = (TMWTYPES_UCHAR)localTime.wDay;
  pDateTime->dayOfWeek = (TMWTYPES_UCHAR)localTime.wDayOfWeek;
  if (localTime.wDayOfWeek == 0)
  {
    pDateTime->dayOfWeek = 7;
  }
  pDateTime->hour = (TMWTYPES_UCHAR)localTime.wHour;
  pDateTime->minutes = (TMWTYPES_UCHAR)localTime.wMinute;
  pDateTime->mSecsAndSecs = localTime.wSecond*1000 + localTime.wMilliseconds;
  pDateTime->invalid = false;
  pDateTime->qualifier = TMWDTIME_UNKNOWN;
  
  TIME_ZONE_INFORMATION timeZoneInfo;
  if(GetTimeZoneInformation(&timeZoneInfo) == TIME_ZONE_ID_DAYLIGHT)
    pDateTime->dstInEffect = TMWDEFS_TRUE;
  else
    pDateTime->dstInEffect = TMWDEFS_FALSE;
}

/* function: getUTCDateTime */
void WinIoSystemTime::getUTCDateTime(TMWDTIME *pDateTime)
{
  SYSTEMTIME localTime;
  GetSystemTime(&localTime);
  pDateTime->year = localTime.wYear;
  pDateTime->month = (TMWTYPES_UCHAR)localTime.wMonth;
  pDateTime->dayOfMonth = (TMWTYPES_UCHAR)localTime.wDay;
  pDateTime->dayOfWeek = (TMWTYPES_UCHAR)localTime.wDayOfWeek;
  if (localTime.wDayOfWeek == 0)
  {
    pDateTime->dayOfWeek = 7;
  }
  pDateTime->hour = (TMWTYPES_UCHAR)localTime.wHour;
  pDateTime->minutes = (TMWTYPES_UCHAR)localTime.wMinute;
  pDateTime->mSecsAndSecs = localTime.wSecond*1000 + localTime.wMilliseconds;
  pDateTime->invalid = false;
  pDateTime->qualifier = TMWDTIME_UNKNOWN;
  pDateTime->dstInEffect = TMWDEFS_FALSE;
}

/* function: setDateTime */
TMWTYPES_BOOL WinIoSystemTime::setDateTime(const TMWDTIME *pNewDateTime)
{
  if (m_timeMode == WINIO_TIME_MODE_SYSTEM)
  {
    SYSTEMTIME localTime;
  
    localTime.wYear = pNewDateTime->year;
    localTime.wMonth = pNewDateTime->month;
    localTime.wDay = pNewDateTime->dayOfMonth;
    localTime.wDayOfWeek = pNewDateTime->dayOfWeek;
    if (pNewDateTime->dayOfWeek == 7)
    {
      localTime.wDayOfWeek = 0;
    }
    localTime.wHour = pNewDateTime->hour;
    localTime.wMinute = pNewDateTime->minutes;
    localTime.wSecond = (pNewDateTime->mSecsAndSecs*60)/60000;
    localTime.wMilliseconds = pNewDateTime->mSecsAndSecs - (localTime.wSecond * 1000);

    BOOL bStatus = SetLocalTime(&localTime);
    if (bStatus == FALSE)
    {
      WinIoInterface::ProtoAnaLog(TMWDIAG_ID_TARGET,  "SYSTIME: set date time failed (error =0x%08x)\r\n", GetLastError());
      WINIO_ASSERT(FALSE);
      return false;
    }
  }
  return true;
}

/* function: setUTCDateTime */
TMWTYPES_BOOL WinIoSystemTime::setUTCDateTime(const TMWDTIME *pNewDateTime)
{
  SYSTEMTIME localTime;

  localTime.wYear = pNewDateTime->year;
  localTime.wMonth = pNewDateTime->month;
  localTime.wDay = pNewDateTime->dayOfMonth;
  localTime.wDayOfWeek = pNewDateTime->dayOfWeek;
  if (pNewDateTime->dayOfWeek == 7)
  {
    localTime.wDayOfWeek = 0;
  }
  localTime.wHour = pNewDateTime->hour;
  localTime.wMinute = pNewDateTime->minutes;
  localTime.wSecond = (pNewDateTime->mSecsAndSecs*60)/60000;
  localTime.wMilliseconds = pNewDateTime->mSecsAndSecs - (localTime.wSecond * 1000);

  BOOL bStatus = SetSystemTime(&localTime);
  if (bStatus == FALSE)
  {
    WinIoInterface::ProtoAnaLog(TMWDIAG_ID_TARGET,  "SYSTIME: set date time failed (error =0x%08x)\r\n", GetLastError());
    WINIO_ASSERT(FALSE);
    return false;
  }

  return true;
}