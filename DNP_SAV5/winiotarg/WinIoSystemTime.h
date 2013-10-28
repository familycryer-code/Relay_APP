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
 * FileName : WinIoSystemTime.h
 * Author   : Fred VanEijk
 * Purpose  : Implementation of system time (i.e. uses the PC Clock)
 *            
 */

#ifndef WinIoSystemTime_DEFINED
#define WinIoSystemTime_DEFINED

#include "tmwscl/utils/tmwtarg.h"
#include "WinIoTarg/WinIoBaseTime.h"

class WinIoSystemTime : public WinIoBaseTime
{
public:
  WinIoSystemTime(WINIO_TIME_MODE timeMode)
  {
    m_timeMode = timeMode;
  }

  /* function: getDateTime
   * purpose: return the current date and time
   * arguments:
   *  pDateTime - storage to return date and time into
   * returns:
   *  void
   */
  virtual void getDateTime(TMWDTIME *pDateTime);

  /* function: getUTCDateTime
   * purpose: return the current Coordinated Universal Time(UTC) time
   * arguments:
   *  pDateTime - storage to return date and time into
   * returns:
   *  void
   */
  static void getUTCDateTime(TMWDTIME *pDateTime);

  /* Set clock to specified time. Future calls to
   * getDateTime will return a clock based on this time.
   */
  virtual TMWTYPES_BOOL setDateTime(const TMWDTIME *pNewDateTime);

  /* Set clock to specified time. Future calls to
   * getUTCDateTime will return a clock based on this time.
   */
  static TMWTYPES_BOOL setUTCDateTime(const TMWDTIME *pNewDateTime);

};

#endif // WinIoSystemTime_DEFINED