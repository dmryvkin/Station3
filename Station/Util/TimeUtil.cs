using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Station.Util
{
    public static class TimeUtil
    {
        public static double SecondsInterval(DateTime start, DateTime end)
        {

            TimeSpan span = end.Subtract(start).Duration();

            return span.TotalSeconds;

        }

        public static double MinutesInterval(DateTime start, DateTime end)
        {

            TimeSpan span = end.Subtract(start).Duration();

            return span.TotalMinutes;

        }


    }
}
