using Leopard.Paging;
using Mk.DemoB.BookMgr.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoB.BookMgr
{
    public interface IBookRepository : IBasicRepository<Book, Guid>
    {
        Task<PageData<Book>> GetPagingListAsync(
            decimal? minPrice, decimal? maxPrice
            , int maxResultCount, int skipCount);
    }
}
