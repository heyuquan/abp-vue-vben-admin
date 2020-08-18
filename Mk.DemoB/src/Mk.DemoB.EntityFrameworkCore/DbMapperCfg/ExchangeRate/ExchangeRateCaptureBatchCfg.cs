using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mk.DemoB.Consts;
using Mk.DemoB.ExchangeRateMgr.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mk.DemoB.DbMapperCfg
{
    public class ExchangeRateCaptureBatchCfg : IEntityTypeConfiguration<ExchangeRateCaptureBatch>
    {
        public void Configure(EntityTypeBuilder<ExchangeRateCaptureBatch> builder)
        {
            builder.ToTable(DemoBConsts.DbTablePrefix + nameof(ExchangeRateCaptureBatch), DemoBConsts.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(p => p.CaptureBatchNumber).IsRequired()
              .HasMaxLength(ExchangeRateCaptureBatchConsts.MaxCaptureBatchNumberLength)
              .HasColumnName(nameof(ExchangeRateCaptureBatch.CaptureBatchNumber));

            builder.Property(p => p.IsSuccess).IsRequired()
              .HasColumnName(nameof(ExchangeRateCaptureBatch.IsSuccess));

            builder.Property(p => p.Remark).IsRequired()
              .HasMaxLength(ExchangeRateCaptureBatchConsts.MaxRemarkLength)
              .HasColumnName(nameof(ExchangeRateCaptureBatch.Remark));
        }
    }
}
