/*CHECKED
 *   Function: time.c                  Project: DNP
 *
 *
 *
 *   DESCRIPTION:
 *
 *       This file contains functions for converting between
 *       DNP time format and RTU time format (rtc counter type).
 *
 *       DNP format is to express the time in the number of milliseconds
 *       since the start of 1970.
 *
 *
 *   REVISION HISTORY:
 *
 *   Num   Modified by              Date       Reason
 *   ---   -----------------------  ---------  -------------------------------
 *		1	Charles Barnes			Sept 14, 2010	Fit My Design
 *
 */

/*
 *   Externals and Include Files
 */

#include    "stdio.h"
#include	"tmwdtime.h"
#include	"definitions.h"
#include	"DNP.h"
#include	"DNPTimeConvert.h"
#include 	"oddball.h"



/**********************************************************************
 *  This table is used for calculating the number of seconds from     *
 *  the start of 1970 to the start of the current year.  To get this  *
 *  value,                                                            *
 *                                                                    *
 *     Seconds_to_start_of_year = YearSec[ CurrentYear - 1970 ]       *
 *                                                                    *
 **********************************************************************/

/*
const unsigned long  YearSec[] = {
	0L,       		31536000L,       63072000L,       94694400L,
	126230400L,		157766400L,      189302400L,      220924800L,
	252460800L,		283996800L,      315532800L,      347155200L,
	378691200L,		410227200L,      441763200L,      473385600L,
	504921600L,		536457600L,      567993600L,      599616000L,
	631152000L,		662688000L,      694224000L,      725846400L,
	757382400L,		788918400L,      820454400L,      852076800L,
	883612800L,		915148800L,      946684800L,      978307200L,
	1009843200L,	1041379200L,     1072915200L,     1104537600L,
	1136073600L,	1167609600L,     1199145600L,     1230768000L,
	1262304000L,	1293840000L,     1325376000L,     1356998400L,
	1388534400L,     1420070400L,     1451606400L,     1483228800L,
	1514764800L,     1546300800L,     1577836800L,     1609459200L,
	1640995200L,     1672531200L,     1704067200L,     1735689600L,
	1767225600L,     1798761600L,     1830297600L,     1861920000L,
	1893456000L,     1924992000L,     1956528000L,     1988150400L,
	2019686400L,     2051222400L,     2082758400L,     2114380800L,
	2145916800L,     2177452800L,     2208988800L,     2240611200L,
	2272147200L,     2303683200L,     2335219200L,     2366841600L,
	2398377600L,     2429913600L,     2461449600L,     2493072000L,
	2524608000L,     2556144000L,     2587680000L,     2619302400L,
	2650838400L,     2682374400L,     2713910400L,     2745532800L,
	2777068800L,     2808604800L,     2840140800L,     2871763200L,
	2903299200L,     2934835200L,     2966371200L,     2997993600L,
	3029529600L,     3061065600L,     3092601600L,     3124224000L,
};*/

//Years 2000 - 2127
const unsigned long YearSec[] = { 
	0L,				31622400L,		63158400L,		94694400L,	
	126230400L,		157852800L,		189388800L,		220924800L,	
	252460800L,		284083200L,		315619200L,		347155200L,	
	378691200L,		410313600L,		441849600L,		473385600L,	
	504921600L,		536544000L,		568080000L,		599616000L,	
	631152000L,		662774400L,		694310400L,		725846400L,	
	757382400L,		789004800L,		820540800L,		852076800L,	
	883612800L,		915235200L,		946771200L,		978307200L,	
	1009843200L,	1041465600L,	1073001600L,	1104537600L,	
	1136073600L,	1167696000L,	1199232000L,	1230768000L,	
	1262304000L,	1293926400L,	1325462400L,	1356998400L,	
	1388534400L,	1420156800L,	1451692800L,	1483228800L,	
	1514764800L,	1546387200L,	1577923200L,	1609459200L,	
	1640995200L,	1672617600L,	1704153600L,	1735689600L,	
	1767225600L,	1798848000L,	1830384000L,	1861920000L,	
	1893456000L,	1925078400L,	1956614400L,	1988150400L,	
	2019686400L,	2051308800L,	2082844800L,	2114380800L,	
	2145916800L,	2177539200L,	2209075200L,	2240611200L,	
	2272147200L,	2303769600L,	2335305600L,	2366841600L,	
	2398377600L,	2430000000L,	2461536000L,	2493072000L,	
	2524608000L,	2556230400L,	2587766400L,	2619302400L,	
	2650838400L,	2682460800L,	2713996800L,	2745532800L,	
	2777068800L,	2808691200L,	2840227200L,	2871763200L,	
	2903299200L,	2934921600L,	2966457600L,	2997993600L,	
	3029529600L,	3061152000L,	3092688000L,	3124224000L,	
	3155760000L,	3187296000L,	3218832000L,	3250368000L,	
	3281904000L,	3313526400L,	3345062400L,	3376598400L,	
	3408134400L,	3439756800L,	3471292800L,	3502828800L,	
	3534364800L,	3565987200L,	3597523200L,	3629059200L,	
	3660595200L,	3692217600L,	3723753600L,	3755289600L,	
	3786825600L,	3818448000L,	3849984000L,	3881520000L,	
	3913056000L,	3944678400L,	3976214400L,	4007750400L
};


/**********************************************************************
 *  This table is used for calculating the number of seconds from     *
 *  the start of the current year to the start of the current month.  *
 *  This is valid for a NON leap year.                                *
 *  To get this value,                                                *
 *                                                                    *
 *     Seconds_to_start_of_mnth = MonthSec[ CurrentMonth ]            *
 *                                      where 0 <= CurrentMonth <= 11 *
 *                                                                    *
 **********************************************************************/


const unsigned long MonthSec[] = {

             0L,  2678400L,  5097600L,  7776000L, 10368000L, 13046400L,
      15638400L, 18316800L, 20995200L, 23587200L, 26265600L, 28857600L
};

const unsigned long LeapMonthSec[] = {

             0L,  2678400L,  5184000L,  7862400L, 10454400L, 13132800L,
      15724800L, 18403200L, 21081600L, 23673600L, 26352000L, 28944000L
};

/*********************************************************************
 * This table contains the number of days in each month in a         *
 * NON leap year.                                                    *
 *********************************************************************/

const unsigned char norm_days[] = {
    31,
    28,
    31,
    30,
    31,
    30,
    31,
    31,
    30,
    31,
    30,
    31
};

/*********************************************************************
 * This table contains the number of days in each month in a         *
 * leap year.                                                        *
 *********************************************************************/

const unsigned char leap_days[] = {
    31,
    29,
    31,
    30,
    31,
    30,
    31,
    31,
    30,
    31,
    30,
    31
};




/*
 *   Function entry point and arguments
 */


/************************************************************************
 *      This function converts from RTU time format (rtc_counter type)  *
 *      to DNP format.                                                  *
 *                                                                      *
 *      DNP format is to express the time in the number of milliseconds *
 *      since the start of 1970.                                        *
 *                                                                      *
 *      INPUT ARGUMENTS : rtctime   - RTU time (internal rtc format).   *
 *                        DNPtime   - points to a buffer of 2 unsigned  *
 *                                    longs for storing the result.     *
 *                                                                      *
 *      OUTPUT :  *DNPtime      = top 4 bytes of result                 *
 *                *DNPtime + 1  = lower 4 bytes of result               *
 *                                                                      *
 *                NOTE: for years greater than 2069, the DNPtime        *
 *                      returned by this function defaults to 0.        *
 *                                                                      *
 ************************************************************************/
const unsigned long secondsInDay = 86400;
extern union event_clock;
void DNPConvertRelayTimeToDNP(unsigned long eventSeconds, unsigned long mSecTimer, TMWDTIME *date)
{
	eventSeconds += RelayTimeConversion;
	
	date->dayOfWeek = findDayOfWeek(eventSeconds);
	
	date->year = findYear(eventSeconds);
	eventSeconds -= YearSec[date->year - 2000];
	
	date->month = findMonth(eventSeconds, date->year);
	if(isLeapYear(date->year))
	{
		eventSeconds -= LeapMonthSec[date->month - 1];
	}
	else
	{
		eventSeconds -= MonthSec[date->month - 1];
	}

	date->dayOfMonth = (eventSeconds / secondsInDay) + 1;
	
	eventSeconds -= (secondsInDay * (date->dayOfMonth - 1));
	
	date->hour = eventSeconds / 3600L;
	
	eventSeconds -= (3600L * date->hour);
	
	date->minutes = eventSeconds / 60;
	
	eventSeconds  -= (60L * date->minutes);
	
	date->mSecsAndSecs = (eventSeconds * 1000) + (mSecTimer % 1000);
	
}

void DNPConvertRelayTimeToMemphisDNPData(unsigned long eventSeconds, unsigned long mSecTimer)
{
#if DNPCustomer == DNPCustomerMemphis
	eventSeconds += RelayTimeConversion;
	
	DNPData.DayOfWeek.Value = findDayOfWeek(eventSeconds) + 1;		//add 1 to go from 0-6 to 1-7
	
	DNPData.Year.Value = findYear(eventSeconds);
	eventSeconds -= YearSec[DNPData.Year.Value - 2000];
	
	DNPData.Month.Value = findMonth(eventSeconds, DNPData.Year.Value);
	if(isLeapYear(DNPData.Year.Value))
	{
		eventSeconds -= LeapMonthSec[DNPData.Month.Value - 1];
	}
	else
	{
		eventSeconds -= MonthSec[DNPData.Month.Value - 1];
	}

	DNPData.DayOfMonth.Value = (eventSeconds / secondsInDay) + 1;
	
	eventSeconds -= (secondsInDay * (DNPData.DayOfMonth.Value - 1));
	
	DNPData.Hour.Value = eventSeconds / 3600L;
	
	eventSeconds -= (3600L * DNPData.Hour.Value);
	
	DNPData.Minute.Value = eventSeconds / 60;
	
	eventSeconds  -= (60L * DNPData.Minute.Value);
	
	DNPData.Second.Value = (eventSeconds);// * 1000);// + (mSecTimer % 1000);
	
	DNPData.MilliSecond.Value = 0;
#endif
}


unsigned long DNPConvertDNPTimeToRelay(TMWDTIME *date)
{
	unsigned long returnTime = 0;
	
	returnTime -= RelayTimeConversion;
	
	returnTime += YearSec[date->year - 2000];
	
	if(isLeapYear(date->year))
	{
		returnTime += LeapMonthSec[date->month - 1];
	}
	else
	{
		returnTime += MonthSec[date->month - 1];
	}
	
	returnTime += secondsInDay * (date->dayOfMonth - 1);
	
	returnTime += (unsigned long)date->hour * 3600;
	
	returnTime += (unsigned long)date->minutes * 60;
	
	returnTime += (date->mSecsAndSecs / 1000);
	
	
	
	event_clock.long_time = returnTime;
	return returnTime;
}

unsigned short findYear(unsigned long eventSeconds)
{
	unsigned short temp = findYearFromLUT(YearSec, 128, &eventSeconds);
	return 2000 + temp;
}

unsigned short findYearFromLUT(const unsigned long *LUT, unsigned int LUTSize, const unsigned long *value)
{
	if(LUTSize == 2)
	{	
		if(*value < LUT[1])
		{
			return 0;
		}
		else
		{
			return 1;
		}
	}
	else
	{
		if(*value < LUT[LUTSize >> 1])// || &LUT[LUTSize >> 1] > &YearSec[99])
		{
			return findYearFromLUT(&LUT[0], LUTSize >> 1, value);
		}
		else
		{
			return (LUTSize >> 1) + findYearFromLUT(&LUT[LUTSize >> 1], LUTSize >> 1, value);
		}
	}
}

unsigned short findMonth(unsigned long eventSeconds, const unsigned short year)
{
	unsigned short returnMonth;
	if(isLeapYear(year))
	{
		returnMonth = findLeapYearMonthFromLUT(LeapMonthSec, 16, &eventSeconds);
	}
	else
	{
		returnMonth = findNormalYearMonthFromLUT(MonthSec, 16, &eventSeconds);
	}
	
	/*
	if(year%4 == 0)
	{
		if(year%100 == 0)
		{
			if(year%400 == 0)
			{
				returnMonth = findLeapYearMonthFromLUT(LeapMonthSec, 16, &eventSeconds);
			}
			else
				returnMonth = findNormalYearMonthFromLUT(MonthSec, 16, &eventSeconds);
		}
		else
			returnMonth = findLeapYearMonthFromLUT(LeapMonthSec, 16, &eventSeconds);
	}
	else
		returnMonth = findNormalYearMonthFromLUT(MonthSec, 16, &eventSeconds);
	*/		
	returnMonth += 1;
	
	return returnMonth;
}

unsigned short findDayOfWeek(unsigned long eventSeconds)
{
	///First day of 2010 was a friday or day # 5
	eventSeconds /= secondsInDay;
	eventSeconds += 5;
	eventSeconds %= 7;
	eventSeconds += 1;
	
	return (unsigned short)eventSeconds;
}

unsigned short findNormalYearMonthFromLUT(const unsigned long *LUT, unsigned short LUTSize, const unsigned long *value)
{
	if(LUTSize == 2)
	{	
		if(*value < LUT[1])
		{
			return 0;
		}
		else
		{
			return 1;
		}
	}
	else
	{
		if(*value < LUT[LUTSize >> 1] || &LUT[LUTSize >> 1] > &MonthSec[11])
		{
			return findYearFromLUT(&LUT[0], LUTSize >> 1, value);
		}
		else
		{
			return (LUTSize >> 1) + findYearFromLUT(&LUT[LUTSize >> 1], LUTSize >> 1, value);
		}
	}
}

unsigned short findLeapYearMonthFromLUT(const unsigned long *LUT, unsigned short LUTSize, const unsigned long *value)
{
	if(LUTSize == 2)
	{	
		if(*value < LUT[1])
		{
			return 0;
		}
		else
		{
			return 1;
		}
	}
	else
	{
		if(*value < LUT[LUTSize >> 1] || &LUT[LUTSize >> 1] > &LeapMonthSec[11])
		{
			return findYearFromLUT(&LUT[0], LUTSize >> 1, value);
		}
		else
		{
			return (LUTSize >> 1) + findYearFromLUT(&LUT[LUTSize >> 1], LUTSize >> 1, value);
		}
	}
}