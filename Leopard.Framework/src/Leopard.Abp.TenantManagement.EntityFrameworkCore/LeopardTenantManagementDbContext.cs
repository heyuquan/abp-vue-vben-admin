using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Leopard.Abp.TenantManagement.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(LeopardDbProperties.DefaultDbConnectionStringName)]
    public class LeopardTenantManagementDbContext : AbpDbContext<LeopardTenantManagementDbContext>, ITenantManagementDbContext
    {
        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        public LeopardTenantManagementDbContext(DbContextOptions<LeopardTenantManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureTenantManagement();

            builder.DbMapperCameNamelToUnder();
        }
    }
}
