using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mk.DemoB.Consts.SaleOrders;
using Mk.DemoB.SaleOrderMgr.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mk.DemoB.DbMapperCfg.SaleOrders
{
    public class SaleOrderCfg : IEntityTypeConfiguration<SaleOrder>
    {
        public void Configure(EntityTypeBuilder<SaleOrder> builder)
        {
            builder.ToTable(DemoBConsts.DbTablePrefix + nameof(SaleOrder), DemoBConsts.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(p => p.OrderNo).IsRequired()
                .HasMaxLength(SaleOrderConsts.MaxOrderNoLength)
                .HasColumnName(nameof(SaleOrder.OrderNo));

            builder.Property(p => p.Currency).IsRequired()
                .HasMaxLength(SaleOrderConsts.MaxCurrencyLength)
                .HasColumnName(nameof(SaleOrder.Currency));

            builder.Property(p => p.TotalAmount).IsRequired()
                .HasColumnName(nameof(SaleOrder.TotalAmount))
                .HasColumnType("decimal(18,6)");

            builder.Property(p => p.OrderStatus).IsRequired()
                .HasColumnName(nameof(SaleOrder.OrderStatus));

            builder.HasMany(x => x.SaleOrderDetails)
                .WithOne().HasForeignKey(x => x.ParentId)
                .IsRequired();
        }
    }
}
