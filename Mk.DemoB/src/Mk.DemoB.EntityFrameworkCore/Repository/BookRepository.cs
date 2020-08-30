using Leopard.Paging;
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
    public class BookRepository : EfCoreRepository<DemoBDbContext, Book, Guid>, IBookRepository
    {
        public BookRepository(IDbContextProvider<DemoBDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<PageData<Book>> GetPagingListAsync(
            decimal? minPrice, decimal? maxPrice
            , int maxResultCount, int skipCount)
        {
            PageData<Book> result = new PageData<Book>();
            var query = GetQueryable()
                 .WhereIf(minPrice.HasValue, x => x.Price >= minPrice.Value)
                 .WhereIf(maxPrice.HasValue, x => x.Price <= maxPrice.Value);

            result.Items=await query.PageBy(skipCount, maxResultCount).ToListAsync();
            result.TotalCount = await query.LongCountAsync();

            return result;
        }

    }
}
