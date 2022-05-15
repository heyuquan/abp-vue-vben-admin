using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeExtensions
    {
        public static bool IsBetween(this DateTime time, DateTime start, DateTime end)
        {
            if (time >= start && time <= end)
            {
                return true;
            }

            return false;
        }
    }
}
