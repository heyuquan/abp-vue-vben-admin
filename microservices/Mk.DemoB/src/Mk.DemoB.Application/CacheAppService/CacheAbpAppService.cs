using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace Mk.DemoB.Application
{
    public class BookCacheItem
    {
        public string Name { get; set; }

        public float Price { get; set; }
    }

    // redis 缓存
    // 加载 AbpCachingStackExchangeRedisModule 模块

    [Route("api/abp/cache")]
    public class CacheAbpAppService : DemoBAppService
    {

        //ASP.NET Core 定义了 IDistributedCache 接口用于 get/set 缓存值.但是会有以下问题:
        //  1、它适用于 byte 数组 而不是.NET 对象. 因此你需要对缓存的对象进行序列化/反序列化.
        //  2、它为所有的缓存项提供了 单个 key 池, 因此 ;
        //        你需要注意键区分 不同类型的对象.
        //        你需要注意不同租户(参见多租户)的缓存项.


        // IDistributedCache<TCacheItem> 接口默认了键是 string 类型
        // 其他类型的key，可以使用：IDistributedCache<TCacheItem, TCacheKey>
        private readonly IDistributedCache<BookCacheItem> _cache;
        public CacheAbpAppService(IDistributedCache<BookCacheItem> cache)
        {
            _cache = cache;
            _cache = cache;
        }
        
        [HttpGet("get")]
        public async Task<BookCacheItem> GetAsync(string bookId = "book1")
        {
            return await _cache.GetOrAddAsync(
                bookId,
                async () =>
                {
                    return await Task.FromResult(
                        new BookCacheItem
                        {
                            Name = Guid.NewGuid().ToString(),
                            Price = new Random().Next(1, int.MaxValue)
                        }
                        );
                },
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(2)
                });
        }

        [HttpPost("remove")]
        public async Task RemoveAsync(string bookId = "book1")
        {
            await _cache.RemoveAsync(bookId);
        }

        [HttpPost("setmany")]
        public async Task<List<KeyValuePair<string, BookCacheItem>>> SetManyAsync(string bookId1 = "book1", string bookId2 = "book2")
        {
            List<KeyValuePair<string, BookCacheItem>> items = new List<KeyValuePair<string, BookCacheItem>>();
            items.Add(
                new KeyValuePair<string, BookCacheItem>(bookId1, new BookCacheItem
                {
                    Name = Guid.NewGuid().ToString(),
                    Price = new Random().Next(1, int.MaxValue)
                }
            ));
            items.Add(
                new KeyValuePair<string, BookCacheItem>(bookId2, new BookCacheItem
                {
                    Name = Guid.NewGuid().ToString(),
                    Price = new Random().Next(1, int.MaxValue)
                }
            ));
            await _cache.SetManyAsync(items,
                 new DistributedCacheEntryOptions
                 {
                     AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(2)
                 });

            return items;
        }
    }
}
