using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Timing;

namespace Mk.DemoB.TimingAppService
{

    //DateTime.Now 返回带有服务器本地日期和时间的 DateTime 对象.DateTime 对象不存储时区信息.
    //因此你无法知道此对象中存储的绝对日期和时间.你只能做一些假设, 例如假设它是在UTC+05时区创建的.
    //当你此值保存到数据库中并稍后读取, 或发送到不同时区的客户端时, 事情就变得特别复杂.

    //解决此问题的一种方法是始终使用 DateTime.UtcNow 并将所有 DateTime 对象假定为UTC时间. 
    //在这种情况下你可以在需要时将其转换为目标客户端的时区.

    /// <summary>
    /// 研究时钟市区
    /// </summary>
    public class TimingAppService: DemoBAppService
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


    }
}
