
   /* DNP Time object   */

   #define SECONDS_IN_MINUTE                    60L
   #define SECONDS_IN_HOUR                              (SECONDS_IN_MINUTE * 60L)
   #define SECONDS_IN_DAY                               (SECONDS_IN_HOUR * 24L)

   #define TIME_OF_DAY_SIZE                             6
   #define TICS_PER_SECOND                              1000
   #define TICS_PER_MINUTE                              (TICS_PER_SECOND * 60L)
   #define TICS_PER_HOUR                                (TICS_PER_MINUTE * 60L)
   #define TICS_PER_DAY                                 (TICS_PER_HOUR * 24L)

   typedef struct
   {
      unsigned int  int_time[(TIME_OF_DAY_SIZE - 1)/2 + 1];
   } DNP_TIME;

const extern unsigned long RelayTimeConversion;
unsigned short findYear(unsigned long eventSeconds);
unsigned short findMonth(unsigned long eventSeconds, const unsigned short year);
unsigned short findYearFromLUT(const unsigned long *LUT, unsigned int LUTSize, const unsigned long *value);
unsigned short findDayOfWeek(unsigned long eventSeconds);
unsigned short findNormalYearMonthFromLUT(const unsigned long *LUT, unsigned short LUTSize, const unsigned long *value);
unsigned short findLeapYearMonthFromLUT(const unsigned long *LUT, unsigned short LUTSize, const unsigned long *value);