using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mk.DemoB.ExchangeRateMgr.Entities;
using Mk.DemoB.SaleOrderMgr.Entities;

using System;
using Volo.Abp.Data;

namespace Mk.DemoB.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See DemoBMigrationsDbContext for migrations.
     */
    [ConnectionStringName("Default")]
    public class DemoBDbContext : LeopardDbContext<DemoBDbContext>
    {
        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside DemoBDbContextModelCreatingExtensions.ConfigureDemoB
         */

        public DbSet<CaptureCurrency> CaptureCurrencys { get; set; }
        public DbSet<ExchangeRateCaptureBatch> ExchangeRateCaptureBatchs { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        public DbSet<SaleOrder> SaleOrders { get; set; }
        public DbSet<SaleOrderDetail> SaleOrderDetails { get; set; }

        public DemoBDbContext(
            DbContextOptions<DemoBDbContext> options
            , IServiceProvider serviceProvider
            )
            : base(options, serviceProvider)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure your own tables/entities inside the ConfigureDemoB method */
            builder.ConfigureDemoB();

            //builder.DbMapperCameNamelToUnder();
        }


    }
}
