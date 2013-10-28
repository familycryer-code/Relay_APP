
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

