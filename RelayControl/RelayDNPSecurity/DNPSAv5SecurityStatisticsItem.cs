using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RelayDNPSecurity
{
    public class DNPSAv5SecurityStatisticItem
    {
        public DNPSAv5SecurityStatisticItem(string name, decimal defaultValue)
        {
            this.DefaultValue = defaultValue;
            this.StatisticsName = name;
        }

        public decimal DefaultValue;
        public string StatisticsName;
    }
}
