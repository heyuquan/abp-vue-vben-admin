using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard
{
    /// <summary>
    /// 统一缓存时间  
    /// </summary>
    public static class CacheTimeSpan
    {
        /// <summary>
        /// 默認緩存時長5分鐘
        /// </summary>
        public static readonly TimeSpan DefaultCacheTime = TimeSpan.FromMinutes(5);
        /// <summary>
        /// 默認留位時長35分鐘
        /// </summary>
        public static readonly TimeSpan DefaultHoldSeatTime = TimeSpan.FromMinutes(35);
        /// <summary>
        /// 短段緩存時長(1分鐘)
        /// </summary>
        public static readonly TimeSpan ShortTime = TimeSpan.FromMinutes(1);
        /// <summary>
        /// 中段緩存時長(半小時)
        /// </summary>
        public static readonly TimeSpan MiddleTime = TimeSpan.FromMinutes(30);
        /// <summary>
        /// 長段緩存時長(24小時)
        /// </summary>
        public static readonly TimeSpan LongTime = TimeSpan.FromHours(24);

        /// <summary>
        /// 其他一天内时间选择
        /// </summary>
        public static class DayOther
        {
            /// <summary>
            /// 10分钟
            /// </summary>
            public static readonly TimeSpan TenMinute = TimeSpan.FromMinutes(10);
            /// <summary>
            /// 15分钟
            /// </summary>
            public static readonly TimeSpan FifteenMinute = TimeSpan.FromMinutes(15);
            /// <summary>
            /// 20分钟
            /// </summary>
            public static readonly TimeSpan TwentyMinute = TimeSpan.FromMinutes(20);
            /// <summary>
            /// 30分钟
            /// </summary>
            public static readonly TimeSpan ThirtyMinute = TimeSpan.FromMinutes(30);
            /// <summary>
            /// 40分钟
            /// </summary>
            public static readonly TimeSpan FortyMinute = TimeSpan.FromMinutes(40);
            /// <summary>
            /// 50分钟
            /// </summary>
            public static readonly TimeSpan FiftyMinute = TimeSpan.FromMinutes(50);
            /// <summary>
            /// 半小时
            /// </summary>
            public static readonly TimeSpan HalfHour = TimeSpan.FromMinutes(30);
            /// <summary>
            /// 1小时
            /// </summary>
            public static readonly TimeSpan OneHour = TimeSpan.FromHours(1);
            /// <summary>
            /// 2小时
            /// </summary>
            public static readonly TimeSpan TwoHour = TimeSpan.FromHours(2);
            /// <summary>
            /// 3小时
            /// </summary>
            public static readonly TimeSpan ThreeHour = TimeSpan.FromHours(3);
            /// <summary>
            /// 4小时
            /// </summary>
            public static readonly TimeSpan FourHour = TimeSpan.FromHours(4);
            /// <summary>
            /// 5小时
            /// </summary>
            public static readonly TimeSpan FiveHour = TimeSpan.FromHours(5);
            /// <summary>
            /// 6小时
            /// </summary>
            public static readonly TimeSpan SixHour = TimeSpan.FromHours(6);
            /// <summary>
            /// 7小时
            /// </summary>
            public static readonly TimeSpan SevenHour = TimeSpan.FromHours(7);
            /// <summary>
            /// 8小时
            /// </summary>
            public static readonly TimeSpan EightHour = TimeSpan.FromHours(8);
            /// <summary>
            /// 9小时
            /// </summary>
            public static readonly TimeSpan NightHour = TimeSpan.FromHours(9);
            /// <summary>
            /// 10小时
            /// </summary>
            public static readonly TimeSpan TenHour = TimeSpan.FromHours(10);
            /// <summary>
            /// 11小时
            /// </summary>
            public static readonly TimeSpan EelvenHour = TimeSpan.FromHours(11);
            /// <summary>
            /// 12小时
            /// </summary>
            public static readonly TimeSpan TwelveHour = TimeSpan.FromHours(12);
            /// <summary>
            /// 13小时
            /// </summary>
            public static readonly TimeSpan ThirteenHour = TimeSpan.FromHours(13);
            /// <summary>
            /// 14小时
            /// </summary>
            public static readonly TimeSpan FourteenHour = TimeSpan.FromHours(14);
            /// <summary>
            /// 15小时
            /// </summary>
            public static readonly TimeSpan FifteenHour = TimeSpan.FromHours(15);
            /// <summary>
            ///16小时
            /// </summary>
            public static readonly TimeSpan SixteenHour = TimeSpan.FromHours(16);
            /// <summary>
            ///17小时
            /// </summary>
            public static readonly TimeSpan SeventeenHour = TimeSpan.FromHours(17);
            /// <summary>
            ///18小时
            /// </summary>
            public static readonly TimeSpan EighteenHour = TimeSpan.FromHours(18);
            /// <summary>
            ///19小时
            /// </summary>
            public static readonly TimeSpan NighteenHour = TimeSpan.FromHours(19);
            /// <summary>
            ///20小时
            /// </summary>
            public static readonly TimeSpan TwentyHour = TimeSpan.FromHours(20);
            /// <summary>
            ///21小时
            /// </summary>
            public static readonly TimeSpan TwentyOneHour = TimeSpan.FromHours(21);
            /// <summary>
            ///22小时
            /// </summary>
            public static readonly TimeSpan TwentyTwoHour = TimeSpan.FromHours(22);
            /// <summary>
            ///23小时
            /// </summary>
            public static readonly TimeSpan TwentyThreeHour = TimeSpan.FromHours(23);
            /// <summary>
            ///24小时
            /// </summary>
            public static readonly TimeSpan TwentyFourHour = TimeSpan.FromHours(24);
        }
    }
}
