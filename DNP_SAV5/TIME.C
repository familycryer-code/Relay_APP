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
 *
 *
 */

/*
 *   Externals and Include Files
 */

#include    "stdio.h"
#include    "b008.h"
#include    "lib_d20.h"
#include    "l012.h"
#include    "lib_netc.h"





/**********************************************************************
 *  This table is used for calculating the number of seconds from     *
 *  the start of 1970 to the start of the current year.  To get this  *
 *  value,                                                            *
 *                                                                    *
 *     Seconds_to_start_of_year = YearSec[ CurrentYear - 1970 ]       *
 *                                                                    *
 **********************************************************************/


static unsigned long  YearSec[] = {

               0L,       31536000L,       63072000L,       94694400L,
       126230400L,      157766400L,      189302400L,      220924800L,
       252460800L,      283996800L,      315532800L,      347155200L,
       378691200L,      410227200L,      441763200L,      473385600L,
       504921600L,      536457600L,      567993600L,      599616000L,
       631152000L,      662688000L,      694224000L,      725846400L,
       757382400L,      788918400L,      820454400L,      852076800L,
       883612800L,      915148800L,      946684800L,      978307200L,
      1009843200L,     1041379200L,     1072915200L,     1104537600L,
      1136073600L,     1167609600L,     1199145600L,     1230768000L,
      1262304000L,     1293840000L,     1325376000L,     1356998400L,
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
      3029529600L,     3061065600L,     3092601600L,     3124224000L

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


static unsigned long MonthSec[] = {

             0L,  2678400L,  5097600L,  7776000L, 10368000L, 13046400L,
      15638400L, 18316800L, 20995200L, 23587200L, 26265600L, 28857600L
};


/*********************************************************************
 * This table contains the number of days in each month in a         *
 * NON leap year.                                                    *
 *********************************************************************/

static unsigned char norm_days[] = {
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

static unsigned char leap_days[] = {
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

void     DNP_convert_rtc_to_dnp_time( unsigned long  rtctime, unsigned long *DNPtime )
{

    long            date;
    long            time;
    int             ticks;

    int             year;
    unsigned char   month;
    unsigned long   SecondsThisMonth;

    unsigned long   high3;
    unsigned long   low3;

    char            topbit;

    register unsigned long   SecondsSince1970;
    register unsigned long   high1;
    register unsigned long   low1;
    register unsigned long   high2;
    register unsigned long   low2;

	

    L012_convert_counter(rtctime, &date, &time, &ticks );

    year  = (int)(date >> 16);
    month = (unsigned char)((date >> 8) - 1);

    if( (year < 1970) || (year > 2069) )
    {
        /*  this routine cannot process the time  */

        *DNPtime++ = 0;
        *DNPtime   = 0;
        return;
    }

    SecondsSince1970 = YearSec[ year - 1970 ];

    SecondsSince1970 += MonthSec[ month ];


    if( ((year & 3) == 0) && (month > 1) )
    {
        /* this is a leap year, somewhere between March and December */

        SecondsSince1970 += SECONDS_IN_DAY;
    }

    SecondsThisMonth = ( ((date & 0xFF) - 1)   * SECONDS_IN_DAY    ) +
                       ( ((time >> 16) & 0xFF) * SECONDS_IN_HOUR   ) +
                       ( ((time >>  8) & 0xFF) * SECONDS_IN_MINUTE ) +
                       (   time & 0xFF                                 );

    SecondsSince1970 += SecondsThisMonth;


    /******************************************************************
     * The stuff below multiplies SecondsSince1970 by 1000 to yield   *
     * milliseconds.                                                  *
     *                                                                *
     *   SecondsSince1970 * 1000 = SecondsSince1970 * (1024 - 16 - 8) *
     *                                                                *
     ******************************************************************/

    high1 = SecondsSince1970 >> 22;    /* SecondsSince1970 * 1024     */
    low1  = SecondsSince1970 << 10;

    high2 = SecondsSince1970 >> 28;    /* SecondsSince1970 * 16       */
    low2  = SecondsSince1970 <<  4;

    high3 = SecondsSince1970 >> 29;    /* SecondsSince1970 * 8        */
    low3  = SecondsSince1970 <<  3;



    if( low1 < low2 )                  /* modify high1, low1 to contain */
    {                                  /* SecondsSince1970 * (1024-16)  */
        high1 -= 1;                    /* = SecondsSince1970 * 1008     */
    }
    low1  = low1 - low2;
    high1 = high1 - high2;


    if( low1 < low3 )                  /* modify high1, low1 to contain */
    {                                  /* SecondsSince1970 * (1008-8)   */
        high1 -= 1;                    /* = SecondsSince1970 * 1000     */
    }
    low1  = low1 - low3;
    high1 = high1 - high3;



    /******************************************************
     * Add millisecond count to the value in high1, low1. *
     ******************************************************/

    topbit = (char) (low1 >> 31);

    low1 += ticks;

    if( topbit && ((low1 >> 31) == 0) )
    {
        /* low has rolled over  */

        high1 += 1;
    }

    *DNPtime++ = high1;
    *DNPtime   = low1;


}



/************************************************************************
 *      This function converts from DNP time format to RTU time format  *
 *      (rtc_counter type).                                             *
 *                                                                      *
 *      DNP format is to express the time in the number of milliseconds *
 *      since the start of 1970.                                        *
 *                                                                      *
 *      INPUT ARGUMENTS : rtctime  - pointer to buffer to which the     *
 *                                   RTU time is stored.                *
 *                        DNPtime  - points to a buffer containing the  *
 *                                   6 byte DNP time.                   *
 *                                   DNPtime[0] : top 4 bytes of time   *
 *                                   DNPtime[1] : lower 4 bytes of time *
 *                                                                      *
 *      OUTPUT :  *rtctime = the RTU system time at the real time       *
 *                           given by DNPtime, as passed to this        *
 *                           function.                                  *
 *                                                                      *
 ************************************************************************/

void     DNP_convert_dnp_to_rtc_time( unsigned long *rtctime, unsigned long *DNPtime )
{
    unsigned long  CurrentDNPtime[ 2 ];   /* current time in DNP format     */
    unsigned long  Currentrtctime;        /* current time in rtc format     */
    unsigned long  delta[2];              /* current DNP time minus DNP time*/
                                          /*         passed to this function*/


    Currentrtctime = rtc_counter();

    DNP_convert_rtc_to_dnp_time( Currentrtctime, CurrentDNPtime );

    /*********************************************************************
     * Calculate "CurrentDNPtime - DNPtime".  This is stored in "delta", *
     * which is effectively an 8 byte signed value.                      *
     *********************************************************************/

    delta[1] = CurrentDNPtime[1] - DNPtime[1];
    *rtctime = Currentrtctime - delta[1];

}

/****************************************************
 *  The following functions are called by function  *
 *  "DNP_convert_dnp_to_time_of_day".               *
 ****************************************************/


void DNP_sub_time(DNP_TIME *time1, DNP_TIME *time2)
{
        unsigned long v1;

        v1 = time1 -> int_time[2];
        v1 -= time2 -> int_time[2];

        time1 -> int_time[2] = (unsigned int) v1;
        v1 >>= 16;
        if(v1 & 0x8000L) v1 |= 0xffff0000L;

        v1 += time1 -> int_time[1];
        v1 -= time2 -> int_time[1];

        time1 -> int_time[1] = (unsigned int) v1;
        v1 >>= 16;
        if(v1 & 0x8000L) v1 |= 0xffff0000L;

        v1 += time1 -> int_time[0];
        v1 -= time2 -> int_time[0];

        time1 -> int_time[0] = (unsigned int) v1;
}

void DNP_add_time(DNP_TIME *time1, DNP_TIME *time2)
{
        unsigned long v1;

        v1 = time1 -> int_time[2];
        v1 += time2 -> int_time[2];

        time1 -> int_time[2] = (unsigned int) v1;
        v1 >>= 16;

        v1 += time1 -> int_time[1];
        v1 += time2 -> int_time[1];

        time1 -> int_time[1] = (unsigned int) v1;
        v1 >>= 16;

        v1 += time1 -> int_time[0];
        v1 += time2 -> int_time[0];

        time1 -> int_time[0] = (unsigned int) v1;
}


void DNP_time_shl(DNP_TIME *time)
{
    unsigned    carry, carry2;

    carry = time -> int_time[2] & 0x8000;
    if(carry) carry = 1;
    time -> int_time[2] <<= 1;

    carry2 = time -> int_time[1] & 0x8000;
    if(carry2) carry2 = 1;
    time -> int_time[1] <<= 1;
    time -> int_time[1] |= carry;

    time -> int_time[0] <<= 1;
    time -> int_time[0] |= carry2;

}

void DNP_time_shr(DNP_TIME *time)
{
    unsigned    carry, carry2;

    carry = time -> int_time[0] & 1;
    if(carry) carry = 0x8000;
    time -> int_time[0] >>= 1;

    carry2 = time -> int_time[1] & 1;
    if(carry2) carry2 = 0x8000;
    time -> int_time[1] >>= 1;
    time -> int_time[1] |= carry;

    time -> int_time[2] >>= 1;
    time -> int_time[2] |= carry2;

}

int DNP_time_cmp(DNP_TIME *time1, DNP_TIME *time2)
{
    int         i;

    for(i = 0; i < (sizeof(DNP_TIME) / sizeof(unsigned)); i++) {
        if(time1 -> int_time[i] > time2 -> int_time[i]) return 1;
        if(time1 -> int_time[i] < time2 -> int_time[i]) return -1;
    }
    return 0;
}

int DNP_time_div( DNP_TIME *num, DNP_TIME *result,
                  DNP_TIME *divisor)
{
    unsigned            count, i;
    unsigned int        check;

    for(check = 0, i = 0; i < sizeof(DNP_TIME)/sizeof(unsigned); i++)
        check |= divisor -> int_time[i];
    if(check == 0) return -1;

    for(check = 0, i = 0; i < sizeof(DNP_TIME)/sizeof(unsigned); i++)
        check |= num -> int_time[i];
    if(check == 0) {
        for(i = 0; i < sizeof(DNP_TIME)/sizeof(unsigned); i++)
            result -> int_time[i] = 0;
        return 0;
    }

    /* align divisor */

    count = 1;
    while((divisor -> int_time[0] & 0x8000) == 0) {
        DNP_time_shl(divisor);
        count++;
    }

    result -> int_time[0] = result -> int_time[1] =
           result -> int_time[2] = 0;

    while(count--) {
        DNP_time_shl(result);
    if(DNP_time_cmp(num, divisor) >= 0 ) {
        DNP_sub_time(num, divisor);
            result -> int_time[2] |= 1;
        }
    DNP_time_shr(divisor);
    }
    return 0;
}







/************************************************************************
 *   Functiom DNP_convert_dnp_to_time_of_day                            *
 *                                                                      *
 *   This function converts from DNP time format to date and time of    *
 *   day.                                                               *
 *                                                                      *
 *                                                                      *
 *   INPUT:   dnp_time - ptr to the six byte DNP time                   *
 *                       dnp_time->int_time[0] is most significant      *
 *                                             2 bytes.                 *
 *                       dnp_time->int_time[2] is least significant     *
 *                                             2 bytes.                 *
 *                                                                      *
 *   OUTPUT:  date     - the date in pSOS time format (See description  *
 *                       of "set_t" in the pSOS User's Manual).         *
 *                     - the time in pSOS time format.                  *
 *                     - ticks between seconds.                         *
 *                                                                      *
 ************************************************************************/


void DNP_convert_dnp_to_time_of_day( DNP_TIME *dnp_time, unsigned long *date, unsigned long *time,
                                     unsigned *tics)

{
    DNP_TIME          t, result, divisor;
    unsigned          years4, leftover4, leap_year, i;
    unsigned          years, month, days, hours, min, sec;
    unsigned long     remainder;

    t.int_time[0] = dnp_time -> int_time[0];
    t.int_time[1] = dnp_time -> int_time[1];
    t.int_time[2] = dnp_time -> int_time[2];

    /* convert to days and remainder */

    divisor.int_time[2] = (unsigned int) (TICS_PER_DAY & 0xffff);
    divisor.int_time[1] = (unsigned int) (TICS_PER_DAY >> 16);
    divisor.int_time[0] = 0;
    DNP_time_div(&t, &result, &divisor);

    /* set days and remainder */

    days = result.int_time[2];

    remainder  = t.int_time[2];
    remainder += ((unsigned long) t.int_time[1]) << 16;

    /* 4 year units and leftover */

    years4 = days / 1461;
    leftover4 = days % 1461;

    /* single year units */

    years = years4 * 4;

    /* adjust for leap years */

    if(leftover4 <= 365)
    {
        leap_year =0;
    }
    else
    {
        leftover4 -= 365;
        years += 1;

        if(leftover4 <= 365)
        {
            leap_year = 0;
        }
        else
        {
            leftover4 -= 365;
            years += 1;

            if(leftover4 <= 366)
            {
                leap_year = 1;
            }
            else
            {
                leftover4 -= 366;
                years += 1;
                leap_year = 0;
            }
        }
    }

    /* calculate month */

    if(leap_year == 0)
    {
        for(i = 0; i < 12; i++)
        {
            if(leftover4 < norm_days[i]) break;
            leftover4 -= norm_days[i];
        }
    }
    else
    {
        for(i = 0; i < 12; i++)
        {
            if(leftover4 < leap_days[i]) break;
            leftover4 -= leap_days[i];
        }
    }
    if(i == 12)
    {
        i = 0;
        years += 1;
    }

    /* set date */

    month = i;
    days = leftover4;
    years += 1970;

    /* calculate time and set */

    hours     = (unsigned int) (remainder / TICS_PER_HOUR);
    remainder = remainder % TICS_PER_HOUR;
    min       = (unsigned int) (remainder / TICS_PER_MINUTE);
    remainder = remainder % TICS_PER_MINUTE;
    sec       = (unsigned int) (remainder / TICS_PER_SECOND);
    remainder = remainder % TICS_PER_SECOND;

    /* tics per second */


    /* return results in pSOS format */

    month += 1;
    days += 1;
    *date = years;
    *date <<= 16;
    *date |= ( month << 8 );
    *date |= days;

    *time = remainder;
    *time <<= 24;
    *time |= ( ((unsigned long) hours) << 16);
    *time |= (min << 8);
    *time |= sec;

    *tics = (unsigned int) remainder;
}

/*
 * End of Function
 */
