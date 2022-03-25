using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{

    // todo  https://www.cnblogs.com/zhuzhongxing/p/14147087.html
    // "yyyy_MM_dd_HH_mm_ss"
    // "yyyy-MM-dd HH:mm:ss.fff"

    //TimeSpan
    //时间 1 是 2010-1-2 8:43:35；
    //时间 2 是 2010-1-12 8:43:34。
    //用时间 2 减时间 1，得到一个 TimeSpan 实例。
    //那么时间 2 比时间 1 多 9 天 23 小时 59 分 59 秒。
    //那么，Days 就是 9，Hours 就是 23，Minutes 就是 59，Seconds 就是 59。
    //再来看 Ticks，Tick 是一个计时周期，表示一百纳秒，即一千万分之一秒，那么 Ticks 在这里表示总共相差多少个时间周期，
    //即：9 * 24 * 3600 * 10000000 + 23 * 3600 * 10000000 +59 * 60 * 10000000 + 59 * 10000000 = 8639990000000。3600 是一小时的秒数。
    //时长（分钟）：ts.Days * 24 + ts.Hours * 60 + ts.Minutes;

    /// <summary>
    /// 时间类
    /// </summary>
    public static class TimeHelper
    {
        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int SecondToMinute(int Second)
        {
            decimal mm = (decimal)((decimal)Second / (decimal)60);
            return Convert.ToInt32(Math.Ceiling(mm));
        }

        #region DateTime 与 秒、毫秒 的转换
        private const long TICKS_PER_SEC = 10000000L;

        private const long TICKS_PER_MILLISEC = 10000L;

        private const long UNIX_EPOCH_TICKS = 621355968000000000L;

        /// <summary>
        /// 将给定时间转为秒
        /// </summary>
        /// <param name="datetime">the date-time object to convert, not null</param>
        /// <returns>the parsed time-stamp</returns>
        public static long ToEpochSecond(DateTime datetime)
        {
            // epochSecond = (ticks - 621355968000000000L) / 10000000L;
            return (datetime.ToUniversalTime().Ticks - UNIX_EPOCH_TICKS) / TICKS_PER_SEC;
        }

        /// <summary>
        /// 将给定时间转为毫秒
        /// </summary>
        /// <param name="datetime">the date-time object to convert, not null</param>
        /// <returns>the parsed time-stamp</returns>
        public static long ToEpochMilli(DateTime datetime)
        {
            // epochMilli = (ticks - 621355968000000000L) / 10000L;
            return (datetime.ToUniversalTime().Ticks - UNIX_EPOCH_TICKS) / TICKS_PER_MILLISEC;
        }

        /// <summary>
        /// 将秒转为DateTime
        /// </summary>
        /// <param name="epochSecond">the epoch seconds to convert</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception> 
        /// <returns>the parsed date-time</returns>
        public static DateTime FromEpochSecond(long epochSecond)
        {
            // ticks = 621355968000000000L + epochSecond * 10000000L;
            var ticks = UNIX_EPOCH_TICKS + epochSecond * TICKS_PER_SEC;
            return new DateTime(ticks, DateTimeKind.Utc).ToLocalTime();
        }

        /// <summary>
        /// 将毫秒转为DateTime
        /// </summary>
        /// <param name="epochMilli">the epoch milliseconds to convert</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception> 
        /// <returns>the parsed date-time</returns>
        public static DateTime FromEpochMilli(long epochMilli)
        {
            // ticks = 621355968000000000L + epochMilli * 10000L;
            var ticks = UNIX_EPOCH_TICKS + epochMilli * TICKS_PER_MILLISEC;
            return new DateTime(ticks, DateTimeKind.Utc).ToLocalTime();
        }
        #endregion

        #region DateTime 与 Unix时间戳格式 转换

        private static readonly TimeZoneInfo gmt8 = TimeZoneInfo.CreateCustomTimeZone("GMT+8", TimeSpan.FromHours(8), "China Standard Time", "(UTC+8)China Standard Time");

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式(默认精度为毫秒)
        /// (从1970年1月1日到现在)
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <param name="digit">时间精度</param>
        /// <returns>Unix时间戳格式</returns> 
        public static long ToUnixTime(DateTime datetime, DateTimeStampDigit digit = DateTimeStampDigit.Millisecond)
        {
            DateTime dateTimeUtc = datetime;
            if (datetime.Kind != DateTimeKind.Utc)
            {
                dateTimeUtc = datetime.ToUniversalTime();
            }

            if (dateTimeUtc.ToUniversalTime() <= DateTime.UnixEpoch)
            {
                return 0;
            }

            long timeStamp = 0;

            switch (digit)
            {
                case DateTimeStampDigit.Second:
                    timeStamp = (long)((dateTimeUtc - DateTime.UnixEpoch).TotalSeconds);
                    break;
                case DateTimeStampDigit.Millisecond:
                    timeStamp = (long)((dateTimeUtc - DateTime.UnixEpoch).TotalMilliseconds);
                    break;
            }

            return timeStamp;
        }

        /// <summary>
        /// Unix时间戳转为本地时间        
        /// </summary>
        /// <param name="unixTimestamp">Unix时间戳格式</param>
        /// <param name="digit">Unix时间戳精度</param>
        /// <returns>本地时间</returns>       
        public static DateTime ToDateTime(long unixTimestamp, DateTimeStampDigit digit = DateTimeStampDigit.Millisecond)
        {
            DateTime time ;

            switch (digit)
            {
                case DateTimeStampDigit.Second:
                    time = DateTime.UnixEpoch.AddSeconds(unixTimestamp);
                    break;
                case DateTimeStampDigit.Millisecond:
                    time = DateTime.UnixEpoch.AddMilliseconds(unixTimestamp);
                    break;
                default:
                    time = DateTime.UnixEpoch.AddMilliseconds(unixTimestamp);
                    break;
            }

            return TimeZoneInfo.ConvertTimeFromUtc(time, gmt8);
        }

        /// <summary>
        /// Converts a DateTime to UTC (with special handling for MinValue and MaxValue).
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>The DateTime in UTC.</returns>
        public static DateTime ToUniversalTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            }
            else if (dateTime == DateTime.MaxValue)
            {
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            }
            else
            {
                return dateTime.ToUniversalTime();
            }
        }

        #endregion

        #region 某年某月最后一天、当天最早最晚时间、当月第一天、当月最后一天
        /// <summary>
        /// 返回某年某月最后一天 
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate(int year, int month)
        {
            DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth(year, month));
            int Day = lastDay.Day;
            return Day;
        }

        /// <summary>
        /// 获取一天的最早时间，类似于"2020-10-29 00:00:00"
        /// </summary>
        public static DateTime DayBegin(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, time.Kind);
        }

        /// <summary>
        /// 获取一天的最晚时间，类似于"2020-10-29 23:59:59"
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime DayEnd(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, 23, 59, 59, time.Kind);
        }

        /// <summary>
        /// 获取当前月的第一天
        /// </summary>
        /// <param name="dateTime">当前时间</param>
        /// <returns></returns>
        public static DateTime MonthFirstDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// 获取当前月的最后一天时间
        /// </summary>
        /// <param name="dateTime">当前时间</param>
        /// <returns></returns>
        public static DateTime MonthLastDay(DateTime dateTime)
        {
            return MonthFirstDay(dateTime).AddMonths(1).AddDays(-1);
        }
        #endregion

        #region 返回时间差        

        /// <summary>
        /// 获取两个时间的模糊时间差
        /// eg：1个月前，1周前，*小时前
        /// </summary>
        /// <param name="dateStart">日期一。</param>
        /// <param name="dateEnd">日期二。</param>
        /// <returns></returns>
        public static string DateDiffString_Fuzzy(DateTime dateStart, DateTime dateEnd)
        {
            TimeSpan span = DateDiffTimeSpan(dateStart, dateEnd);
            if (span.TotalDays > 60)
            {
                return dateStart.ToShortDateString();
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }

            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒前";
            }
        }

        /// <summary>
        /// 获得两个日期的精确时间差
        /// </summary>
        /// <param name="dateStart">日期一。</param>
        /// <param name="dateEnd">日期二。</param>
        /// <returns>日期间隔TimeSpan。</returns>
        public static string DateDiffTimeSpan_Exact(DateTime dateStart, DateTime dateEnd)
        {
            TimeSpan ts = DateDiffTimeSpan(dateStart, dateEnd);
            return $"{ts.Days.ToString()}天{ts.Hours.ToString()}小时{ ts.Minutes.ToString()}分钟{ts.Seconds.ToString()}秒";
        }



        /// <summary>
        /// 获得两个日期的间隔
        /// </summary>
        /// <param name="dateStart">日期一。</param>
        /// <param name="dateEnd">日期二。</param>
        /// <returns>日期间隔TimeSpan。</returns>
        public static TimeSpan DateDiffTimeSpan(DateTime dateStart, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateStart.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts;
        }

        #endregion

        #region 得到随机日期
        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="dateStart">起始日期</param>
        /// <param name="dateEnd">结束日期</param>
        /// <returns>间隔日期之间的 随机日期</returns>
        public static DateTime GetRandomTime(DateTime dateStart, DateTime dateEnd)
        {
            Random random = new Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            TimeSpan ts = DateDiffTimeSpan(dateStart, dateEnd);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = dateEnd;
                maxTime = dateStart;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = dateStart;
                maxTime = dateEnd;
            }
            else
            {
                return dateStart;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }
        #endregion

    }

    /// <summary>
    /// 时间精度
    /// </summary>
    public enum DateTimeStampDigit
    {
        NONE = 0,

        /// <summary>
        /// 精确到 秒 。返回Stamp长度为：10 
        /// </summary>
        Second = 16,

        /// <summary>
        /// 精确到 毫秒 。返回Stamp长度为：13
        /// </summary>
        Millisecond = 32,
    }
}

