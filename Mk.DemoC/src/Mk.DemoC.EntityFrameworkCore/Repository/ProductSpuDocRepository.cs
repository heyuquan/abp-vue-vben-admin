using Leopard.Paging;
using Microsoft.EntityFrameworkCore;
using Mk.DemoC.EntityFrameworkCore;
using Mk.DemoC.SearchDocumentMgr;
using Mk.DemoC.SearchDocumentMgr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Mk.DemoC.Repository
{
    public class ProductSpuDocRepository : EfCoreRepository<DemoCDbContext, ProductSpuDoc, Guid>, IProductSpuDocRepository
    {
        public ProductSpuDocRepository(IDbContextProvider<DemoCDbContext> dbContextProvider)
                : base(dbContextProvider)
        {

        }

        public async Task DeleteAllAsync()
        {
            string sql = "delete from product_spu_doc";
            await DbContext.Database.ExecuteSqlRawAsync(sql);
        }

        /// <summary>
        /// 获取 产品Spu 索引元数据
        /// </summary>
        /// <param name="sorting">Eg：Name asc,Id desc</param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="isGetTotalCount"></param>
        /// <param name="isNoTracking"></param>
        /// <returns></returns>
        public async Task<PageData<ProductSpuDoc>> GetPagingAsync(
            string sorting = null
            , int skipCount = 0
            , int maxResultCount = int.MaxValue
            , bool isGetTotalCount = true
            , bool isNoTracking = false)
        {
            PageData<ProductSpuDoc> result = new PageData<ProductSpuDoc>();
            var query = GetQueryable();
            if (isNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (isGetTotalCount)
            {
                result.TotalCount = await query.LongCountAsync();
            }

            if (!string.IsNullOrWhiteSpace(sorting))
            {
                query = query.OrderBy(sorting);
            }
            else
            {
                query.OrderByDescending(x => x.Id);
            }

            result.Items = await query.PageBy(skipCount, maxResultCount).ToListAsync();

            return result;
        }
    }
}
