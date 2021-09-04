using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mk.DemoB.ExchangeRateMgr.Entities;
using Mk.DemoB.SaleOrderMgr.Entities;
using Mk.DemoB.Users;
using System;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Users.EntityFrameworkCore;

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
        public DbSet<AppUser> Users { get; set; }

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

            /* Configure the shared tables (with included modules) here */

            builder.Entity<AppUser>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users"); //Sharing the same table "AbpUsers" with the IdentityUser

                b.ConfigureByConvention();
                b.ConfigureAbpUser();

                /* Configure mappings for your additional properties
                 * Also see the DemoBEfCoreEntityExtensionMappings class
                 */
            });

            /* Configure your own tables/entities inside the ConfigureDemoB method */
            builder.ConfigureDemoB();

            //builder.DbMapperCameNamelToUnder();
        }


    }
}
