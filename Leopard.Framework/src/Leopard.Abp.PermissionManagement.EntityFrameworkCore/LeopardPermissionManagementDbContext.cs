using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace Leopard.Abp.PermissionManagement.EntityFrameworkCore
{
    [ConnectionStringName(LeopardDbProperties.DefaultDbConnectionStringName)]
    public class LeopardPermissionManagementDbContext : AbpDbContext<LeopardPermissionManagementDbContext>, IPermissionManagementDbContext
    {
        public DbSet<PermissionGrant> PermissionGrants { get; set; }

        public LeopardPermissionManagementDbContext(DbContextOptions<LeopardPermissionManagementDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePermissionManagement();

            builder.DbMapperCameNamelToUnder();
        }
    }
}
