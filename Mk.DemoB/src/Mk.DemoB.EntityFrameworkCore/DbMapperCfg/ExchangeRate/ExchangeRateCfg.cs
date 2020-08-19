using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mk.DemoB.Consts;
using Mk.DemoB.ExchangeRateMgr.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mk.DemoB.DbMapperCfg
{
    public class ExchangeRateCfg : IEntityTypeConfiguration<ExchangeRate>
    {
        public void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            builder.ToTable(DemoBConsts.DbTablePrefix + nameof(ExchangeRate), DemoBConsts.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(p => p.CurrencyCodeFrom).IsRequired()
                .HasMaxLength(ExchangeRateConsts.MaxCurrencyCodeFromLength)
                .HasColumnName(nameof(ExchangeRate.CurrencyCodeFrom));
            builder.Property(p => p.CurrencyCodeTo).IsRequired()
              .HasMaxLength(ExchangeRateConsts.MaxCurrencyCodeToLength)
              .HasColumnName(nameof(ExchangeRate.CurrencyCodeTo));

            builder.Property(p => p.BuyPrice).IsRequired()
              .HasColumnName(nameof(ExchangeRate.BuyPrice));

            builder.Property(p => p.DataFromUrl).IsRequired()
              .HasMaxLength(ExchangeRateConsts.MaxDataFromUrlLength)
              .HasColumnName(nameof(ExchangeRate.DataFromUrl));

            builder.Property(p => p.CaptureBatchNumber).IsRequired()
              .HasMaxLength(ExchangeRateConsts.MaxCaptureBatchNumberLength)
              .HasColumnName(nameof(ExchangeRate.CaptureBatchNumber));

            builder.Property(p => p.CaptureTime).IsRequired()
              .HasColumnName(nameof(ExchangeRate.CaptureTime));
        }
    }
}
