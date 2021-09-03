using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mk.DemoB.CacheAppService
{
    // redis 缓存
    // 加载 AbpCachingStackExchangeRedisModule 模块

    [Route("api/dts/cache")]
    public class CacheDTSAppService : DemoBAppService
    {
        private IDistributedCache _cache;
        public CacheDTSAppService(IDistributedCache cache)
        {
            this._cache = cache;
        }

        [HttpGet("settime")]
        public async Task<string> SetTime()
        {
            var currentTime = Clock.Now.ToString();
            await this._cache.SetStringAsync("CurrentTime", currentTime);
            return $"设置时间为{currentTime}";
        }

        [HttpGet("gettime")]
        public async Task<string> GetTime()
        {
            var currentTime = await this._cache.GetStringAsync("CurrentTime");
            return $"获取时间为{currentTime}";
        }
    }
}
