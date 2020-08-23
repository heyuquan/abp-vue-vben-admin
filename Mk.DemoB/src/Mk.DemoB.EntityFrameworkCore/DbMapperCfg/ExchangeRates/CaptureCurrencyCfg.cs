using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mk.DemoB.Consts.ExchangeRates;
using Mk.DemoB.ExchangeRateMgr.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mk.DemoB.DbMapperCfg.ExchangeRates
{
    public class CaptureCurrencyCfg : IEntityTypeConfiguration<CaptureCurrency>
    {
        public void Configure(EntityTypeBuilder<CaptureCurrency> builder)
        {
            builder.ToTable(DemoBConsts.DbTablePrefix + nameof(CaptureCurrency), DemoBConsts.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(p => p.CurrencyCodeFrom).IsRequired()
                .HasMaxLength(CaptureCurrencyConsts.MaxCurrencyCodeFromLength)
                .HasColumnName(nameof(CaptureCurrency.CurrencyCodeFrom));
            builder.Property(p => p.CurrencyCodeTo).IsRequired()
              .HasMaxLength(CaptureCurrencyConsts.MaxCurrencyCodeToLength)
              .HasColumnName(nameof(CaptureCurrency.CurrencyCodeTo));

        }
    }
}
