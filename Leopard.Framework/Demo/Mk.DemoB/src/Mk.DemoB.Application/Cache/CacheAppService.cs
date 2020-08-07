using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace Mk.DemoB.Cache
{
    public class BookCacheItem
    {
        public string Name { get; set; }

        public float Price { get; set; }
    }

    public class CacheAppService : DemoBAppService
    {
        //ASP.NET Core 定义了 IDistributedCache 接口用于 get/set 缓存值.但是会有以下问题:
        //  1、它适用于 byte 数组 而不是.NET 对象. 因此你需要对缓存的对象进行序列化/反序列化.
        //  2、它为所有的缓存项提供了 单个 key 池, 因此 ;
        //        你需要注意键区分 不同类型的对象.
        //        你需要注意不同租户(参见多租户)的缓存项.


        // IDistributedCache<TCacheItem> 接口默认了键是 string 类型
        // 其他类型的key，可以使用：IDistributedCache<TCacheItem, TCacheKey>
        private readonly IDistributedCache<BookCacheItem> _cache;
        public CacheAppService(IDistributedCache<BookCacheItem> cache)
        {
            _cache = cache;
        }

        public async Task<BookCacheItem> GetAsync(string bookId)
        {
            return await _cache.GetOrAddAsync(
                bookId,
                async () =>
                {
                    return await Task.FromResult(
                        new BookCacheItem
                        {
                            Name = Guid.NewGuid().ToString(),
                            Price = new Random().Next(int.MinValue, int.MaxValue)
                        }
                        );
                },
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(2)
                });
        }
    }
}
