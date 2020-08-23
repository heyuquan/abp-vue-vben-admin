using Mk.DemoB.Dto.Timing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Timing;

namespace Mk.DemoB.TimingAppService
{
    // .Net Core与跨平台时区
    // https://www.dazhuanlan.com/2019/12/25/5e02464ce313a/
    // 时区的不同
    // #、windows系统通过windows注册表来维护一个时区列表。
    //    https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-time-zones
    // #、linux系统通过 Internet Assigned Numbers Authority (IANA)维护的时区数据库。
    //    https://www.iana.org/time-zones


    //DateTime.Now 返回带有服务器本地日期和时间的 DateTime 对象.DateTime 对象不存储时区信息.
    //因此你无法知道此对象中存储的绝对日期和时间.你只能做一些假设, 例如假设它是在UTC+05时区创建的.
    //当你此值保存到数据库中并稍后读取, 或发送到不同时区的客户端时, 事情就变得特别复杂.

    //解决此问题的一种方法是始终使用 DateTime.UtcNow 并将所有 DateTime 对象假定为UTC时间. 
    //在这种情况下你可以在需要时将其转换为目标客户端的时区.

    /// <summary>
    /// 研究时钟市区
    /// </summary>
    public class TimingAppService : DemoBAppService
    {
        // ITimezoneProvider 是一个服务,可将Windows时区ID值简单转换为Iana时区名称值,反之亦然. 
        // 它还提供了获取这些时区列表与获取具有给定名称的 TimeZoneInfo 的方法.
        private readonly ITimezoneProvider _timezoneProvider;

        public TimingAppService(
            ITimezoneProvider timezoneProvider
            )
        {
            _timezoneProvider = timezoneProvider;
        }

        public virtual async Task GetTimezoneAsync()
        {
            // Africa/Abidjan
            var windowsTimezones = _timezoneProvider.GetWindowsTimezones();
            // Africa/Abidjan
            var ianaTimezones = _timezoneProvider.GetIanaTimezones();

            var info = _timezoneProvider.GetTimeZoneInfo("Africa/Abidjan");
        }

        public virtual async Task AbpClock()
        {
            // UTC即世界标准时间
            // utc +8 =北京本地时间
            // eg:utc   = 2020/8/22 9:12:26
            // 本地时间 = 2020/8/22 17:12:26
            var dateTime = Clock.Now;

            // 始终使用 DateTime.UtcNow 并将所有 DateTime 对象假定为UTC时间. 
            // 在这种情况下你可以在需要时将其转换为目标客户端的时区.

            // 1、Abp在保存数据到数据库，如果字段是DateTime对象(DateTime中有Kind对象指示当前是Local还是Utc)，会调用Clock的 Normalize()方法，将其格式化为 AbpClockOptions 设置的Local或Utc
            // 最佳编程规范：在获取Now时间时，最好使用 Clock.Now 对象。 对于字符串手动转换local和utc时间时使用： Clock.Normalize(dateTime) 方法
            // 2、Abp会自动处理接口输入的时间字符串参数
            // 3、Abp会自动处理返回对象若包含DateTime字段的格式化，可使用 DisableDateTimeNormalizationAttribute 特性禁止某个字段的处理
        }

        /// <summary>
        /// 输入时间字符串时，默认创建的DateTime 其Kind=Utc  （由AbpClockOptions指定）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public virtual async Task<MultiTimeTypeResult> MultiTimeType(DateTime time)
        {
            // eg
            // 传入字符串： 2020-08-22 17:53:15
            // time对象时间为 2020-08-22 17:53:15  Kind=Utc

            // 标识 DisableDateTimeNormalization 特性，Abp才不会对输出对象的DateTime类型字段做处理
            MultiTimeTypeResult result = new MultiTimeTypeResult
            {
                LocalDateTime = time.ToLocalTime(),     // 2020-08-23T01:53:15+08:00
                UtcDateTime = time.ToUniversalTime(),   // 2020-08-22T17:53:15Z
                UnHandleDateTime = time                 // 2020-08-22T17:53:15Z
            };

            return result;
        }

    }
}
