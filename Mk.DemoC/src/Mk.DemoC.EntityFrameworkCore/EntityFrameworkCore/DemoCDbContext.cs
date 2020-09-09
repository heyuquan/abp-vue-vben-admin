using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mk.DemoC.SearchDocumentMgr.Entities;
using System;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Mk.DemoC.EntityFrameworkCore
{
    [ConnectionStringName(DemoCDbProperties.ConnectionStringName)]
    public class DemoCDbContext : LeopardDbContext<DemoCDbContext>//, IDemoCDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public DbSet<ProductSpuDoc> ProductSpuDocs { get; set; }

        public DemoCDbContext(
            DbContextOptions<DemoCDbContext> options
            , IServiceProvider serviceProvider
            )
            : base(options, serviceProvider)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureDemoC();

            builder.DbMapperCameNamelToUnder();
        }
    }
}