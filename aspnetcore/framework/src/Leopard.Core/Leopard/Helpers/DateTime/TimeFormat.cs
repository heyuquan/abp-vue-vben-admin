using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    // 参考：https://www.cnblogs.com/zhuzhongxing/p/14147087.html

    /// <summary>
    /// 时间格式化器 。 只做时间格式化
    /// </summary>
    public class TimeFormat
    {
        private readonly string _pattern;

        private TimeFormat() { }

        private TimeFormat(string pattern)
        {
            _pattern = pattern;
        }

        /// <summary> 日期格式 <c>[yyyy-MM-dd]</c> </summary>
        public static readonly TimeFormat DATE = new TimeFormat("yyyy-MM-dd");

        /// <summary> 日期格式 <c>[yyyy-MM-dd]</c> </summary>
        public static readonly TimeFormat DATE_CHINESE = new TimeFormat("yyyy年MM月dd日");

        /// <summary> 日期格式 <c>[yyyyMMdd]</c> </summary>
        public static readonly TimeFormat DATE_COMPACT = new TimeFormat("yyyyMMdd");

        /// <summary> 日期格式 <c>[yyyy_MM_dd]</c> </summary>
        public static readonly TimeFormat DATE_UNDERLINE = new TimeFormat("yyyy_MM_dd");

        /// <summary> 时间格式 <c>[HH:mm:ss]</c> </summary>
        public static readonly TimeFormat TIME = new TimeFormat("HH:mm:ss");

        /// <summary> 时间格式 <c>[HH:mm:ss]</c> </summary>
        public static readonly TimeFormat TIME_CHINESE = new TimeFormat("HH时mm分ss秒");

        /// <summary> 时间格式 <c>[HHmmss]</c> </summary>
        public static readonly TimeFormat TIME_COMPACT = new TimeFormat("HHmmss");

        /// <summary> 时间格式 <c>[HH_mm_ss]</c> </summary>
        public static readonly TimeFormat TIME_UNDERLINE = new TimeFormat("HH_mm_ss");

        /// <summary> 时间格式 <c>[HH:mm:ss.fff]</c> </summary>
        public static readonly TimeFormat TIME_MILLI = new TimeFormat("HH:mm:ss.fff");

        /// <summary> 时间格式 <c>[HHmmssfff]</c> </summary>
        public static readonly TimeFormat TIME_MILLI_COMPACT = new TimeFormat("HHmmssfff");

        /// <summary> 时间格式 <c>[HH_mm_ss_fff]</c> </summary>
        public static readonly TimeFormat TIME_MILLI_UNDERLINE = new TimeFormat("HH_mm_ss_fff");

        /// <summary> 日期时间格式 <c>[yyyy-MM-dd HH:mm:ss]</c> </summary>
        public static readonly TimeFormat DATE_TIME = new TimeFormat("yyyy-MM-dd HH:mm:ss");

        /// <summary> 日期时间格式 <c>[yyyy-MM-dd HH:mm:ss]</c> </summary>
        public static readonly TimeFormat DATE_TIME_CHINESE = new TimeFormat("yyyy年MM月dd日 HH时mm分ss秒");

        /// <summary> 日期时间格式 <c>[yyyyMMddHHmmss]</c> </summary>
        public static readonly TimeFormat DATE_TIME_COMPACT = new TimeFormat("yyyyMMddHHmmss");

        /// <summary> 日期时间格式 <c>[yyyy_MM_dd_HH_mm_ss]</c> </summary>
        public static readonly TimeFormat DATE_TIME_UNDERLINE = new TimeFormat("yyyy_MM_dd_HH_mm_ss");

        /// <summary> 日期时间格式 <c>[yyyy-MM-dd HH:mm:ss.fff]</c> </summary>
        public static readonly TimeFormat DATE_TIME_MILLI = new TimeFormat("yyyy-MM-dd HH:mm:ss.fff");

        /// <summary> 日期时间格式 <c>[yyyyMMddHHmmssfff]</c> </summary>
        public static readonly TimeFormat DATE_TIME_MILLI_COMPACT = new TimeFormat("yyyyMMddHHmmssfff");

        /// <summary> 日期时间格式 <c>[yyyy_MM_dd_HH_mm_ss_fff]</c> </summary>
        public static readonly TimeFormat DATE_TIME_MILLI_UNDERLINE = new TimeFormat("yyyy_MM_dd_HH_mm_ss_fff");


        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>返回格式化后的当前时间字符串</returns>
        public string Now()
        {
            return Format(DateTime.Now);
        }

        /// <summary>
        /// DateTime格式化为自定格式的时间字符串
        /// </summary>
        /// <param name="datetime">the date-time object to format, not null</param>
        /// <returns>返回格式化后的当前时间字符串</returns>
        public string Format(DateTime datetime)
        {
            return datetime.ToString(_pattern, DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// 将对应格式的时间字符串，转为DateTime类型
        /// （PS：若只给定时间部分，没有给日期部分，则日期默认取当前日期）
        /// </summary>
        /// <param name="datetimeStr">the text to parse, not null</param>
        /// <returns>转换成功，返回Datetime；转换失败，返回null</returns>
        public DateTime? Parse(string datetimeStr)
        {
            if (string.IsNullOrWhiteSpace(datetimeStr)) return null;

            try
            {
                return DateTime.ParseExact(datetimeStr, _pattern, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }

}
