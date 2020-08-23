using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mk.DemoB.Consts.SaleOrders;
using Mk.DemoB.SaleOrderMgr.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mk.DemoB.DbMapperCfg.SaleOrders
{
    public class SaleOrderDetailCfg : IEntityTypeConfiguration<SaleOrderDetail>
    {
        public void Configure(EntityTypeBuilder<SaleOrderDetail> builder)
        {
            builder.ToTable(DemoBConsts.DbTablePrefix + nameof(SaleOrderDetail), DemoBConsts.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(p => p.ParentId).IsRequired()
                .HasColumnName(nameof(SaleOrderDetail.ParentId));

            builder.Property(p=>p.LineNo).IsRequired()
                .HasColumnName(nameof(SaleOrderDetail.LineNo));

            builder.Property(p => p.ProductSkuCode).IsRequired()
                .HasMaxLength(SaleOrderDetailConsts.MaxProductSkuCodeLength)
                .HasColumnName(nameof(SaleOrderDetail.ProductSkuCode));

            builder.Property(p => p.Price).IsRequired()
                .HasColumnName(nameof(SaleOrderDetail.Price))
                .HasColumnType("decimal(18,6)");

            builder.Property(p => p.Quantity).IsRequired()
                .HasColumnName(nameof(SaleOrderDetail.Quantity));
        }
    }
}
