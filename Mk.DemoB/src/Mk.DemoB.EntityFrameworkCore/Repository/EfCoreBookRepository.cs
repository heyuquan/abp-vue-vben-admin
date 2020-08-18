using Microsoft.EntityFrameworkCore;
using Mk.DemoB.BookMgr;
using Mk.DemoB.BookMgr.Entities;
using Mk.DemoB.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Mk.DemoB.Repository
{
    public class EfCoreBookRepository : EfCoreRepository<DemoBDbContext, Book, Guid>, IBookRepository
    {
        public EfCoreBookRepository(IDbContextProvider<DemoBDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Book>> GetListAsync(decimal? minPrice, decimal? maxPrice, int maxResultCount, int skipCount)
        {
            var books = await DbSet
                 .WhereIf(minPrice.HasValue, x => x.Price >= minPrice.Value)
                 .WhereIf(maxPrice.HasValue, x => x.Price <= maxPrice.Value)
                 .PageBy(skipCount, maxResultCount)
                 .ToListAsync();

            return books;
        }

        public async Task<long> GetCountAsync(decimal? minPrice, decimal? maxPrice)
        {
            var count = await DbSet
                 .WhereIf(minPrice.HasValue, x => x.Price >= minPrice.Value)
                 .WhereIf(maxPrice.HasValue, x => x.Price <= maxPrice.Value)                 
                 .LongCountAsync();

            return count;
        }
    }
}
