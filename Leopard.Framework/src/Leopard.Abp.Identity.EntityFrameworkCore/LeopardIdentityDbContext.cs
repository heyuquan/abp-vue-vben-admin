using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace Leopard.Abp.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    [ConnectionStringName(LeopardDbProperties.DefaultDbConnectionStringName)]
    public class LeopardIdentityDbContext : AbpDbContext<LeopardIdentityDbContext>, IIdentityDbContext
    {
        public DbSet<IdentityUser> Users { get; set; }

        public DbSet<IdentityRole> Roles { get; set; }

        public DbSet<IdentityClaimType> ClaimTypes { get; set; }

        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }

        public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }

        public DbSet<IdentityLinkUser> LinkUsers { get; set; }

        public LeopardIdentityDbContext(DbContextOptions<LeopardIdentityDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureIdentity();

            builder.DbMapperCameNamelToUnder();
        }
    }
}