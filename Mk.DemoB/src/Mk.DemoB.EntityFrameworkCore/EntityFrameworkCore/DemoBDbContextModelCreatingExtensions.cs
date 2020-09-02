using Microsoft.EntityFrameworkCore;
using Mk.DemoB.DbMapperCfg.ExchangeRates;
using Mk.DemoB.DbMapperCfg.SaleOrders;
using Volo.Abp;

namespace Mk.DemoB.EntityFrameworkCore
{
    public static class DemoBDbContextModelCreatingExtensions
    {
        public static void ConfigureDemoB(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(DemoBConsts.DbTablePrefix + "YourEntities", DemoBConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});

            builder.ApplyConfiguration(new CaptureCurrencyCfg());
            builder.ApplyConfiguration(new ExchangeRateCaptureBatchCfg());
            builder.ApplyConfiguration(new ExchangeRateCfg());

            builder.ApplyConfiguration(new SaleOrderCfg());
            builder.ApplyConfiguration(new SaleOrderDetailCfg());
        }

    }
}