using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mk.DemoB.CacheAppService
{
    // Memory 缓存
    // 加载 AbpCachingModule 模块

    [Route("api/memory/cache")]
    public class CacheMemoryAppService : DemoBAppService
    {
        private IMemoryCache _cache;

        public CacheMemoryAppService(IMemoryCache cache)
        {
            _cache = cache;
        }

        [Route("set")]
        [HttpPost]
        public string Set(string key = "userId")
        {
            var id = Guid.NewGuid().ToString();
            _cache.Set(key, id);
            return $"设置的ID：{id}";
        }

        [Route("get")]
        [HttpGet]
        public string get(string key = "userId")
        {
            return _cache.Get<string>(key) ?? "没有设置缓存";
        }

        [Route("remove")]
        [HttpPost]
        public void Remove(string key = "userId")
        {
            _cache.Remove(key);
        }
    }
}
