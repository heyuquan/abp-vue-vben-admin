using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Leopard.Saas.EntityFrameworkCore
{
    public static class SaasDbContextModelCreatingExtensions
	{
		public static void ConfigureSaas(this ModelBuilder builder, Action<SaasModelBuilderConfigurationOptions> optionsAction = null)
		{
			Check.NotNull<ModelBuilder>(builder, nameof(builder));
			var options = new SaasModelBuilderConfigurationOptions(SaasServiceDbProperties.DbTablePrefix, SaasServiceDbProperties.DbSchema);
			optionsAction?.Invoke(options);

			builder.Entity<Tenant>(b =>
			{
				b.ToTable(options.TablePrefix + "Tenants", options.Schema);

				b.ConfigureByConvention();

				b.Property<string>(x => x.Name).IsRequired().HasMaxLength(TenantConsts.MaxNameLength);

				b.HasMany<TenantConnectionString>(x => x.ConnectionStrings).WithOne().HasForeignKey(x => x.TenantId).IsRequired();

				b.HasIndex(x => x.Name);
			});

			builder.Entity<Edition>(b =>
			{
				b.ToTable(options.TablePrefix + "Editions", options.Schema);

				b.ConfigureByConvention();

				b.Property<string>(x => x.DisplayName).IsRequired().HasMaxLength(EditionConsts.MaxDisplayNameLength);

				b.HasIndex(x => x.DisplayName);
			});

			builder.Entity<TenantConnectionString>(b =>
			{
				b.ToTable(options.TablePrefix + "TenantConnectionStrings", options.Schema);

				b.ConfigureByConvention();

				b.HasKey(x => new { x.TenantId, x.Name });

				b.Property<string>(x => x.Name).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxNameLength);

				b.Property<string>(x => x.Value).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxValueLength);
			});
		}
	}
}
