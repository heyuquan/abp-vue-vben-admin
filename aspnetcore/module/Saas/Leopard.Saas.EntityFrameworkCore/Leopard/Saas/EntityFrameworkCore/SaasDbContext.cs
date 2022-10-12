using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Leopard.Saas.EntityFrameworkCore
{
    [ConnectionStringName(SaasServiceDbProperties.ConnectionStringName)]
    public class SaasDbContext : LeopardDbContext<SaasDbContext>, ISaasDbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        public SaasDbContext(DbContextOptions<SaasDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureSaas(null);
        }
    }
}