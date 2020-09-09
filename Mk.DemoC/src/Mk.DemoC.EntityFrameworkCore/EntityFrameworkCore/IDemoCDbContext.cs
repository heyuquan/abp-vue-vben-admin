using Microsoft.EntityFrameworkCore;
using Mk.DemoC.SearchDocumentMgr.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Mk.DemoC.EntityFrameworkCore
{
    [ConnectionStringName(DemoCDbProperties.ConnectionStringName)]
    public interface IDemoCDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<ProductSpuDoc> ProductSpuDocs { get; set; }
    }
}