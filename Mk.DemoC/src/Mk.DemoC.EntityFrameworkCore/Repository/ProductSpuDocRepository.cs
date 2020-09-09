using Microsoft.EntityFrameworkCore;
using Mk.DemoC.EntityFrameworkCore;
using Mk.DemoC.SearchDocumentMgr;
using Mk.DemoC.SearchDocumentMgr.Entities;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

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
    }
}
