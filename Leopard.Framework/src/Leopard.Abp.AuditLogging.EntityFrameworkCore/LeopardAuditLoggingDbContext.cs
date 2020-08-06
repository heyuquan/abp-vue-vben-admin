using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Leopard.Abp.AuditLogging.EntityFrameworkCore
{
    [ConnectionStringName(LeopardDbProperties.DefaultDbConnectionStringName)]
    public class LeopardAuditLoggingDbContext : AbpDbContext<LeopardAuditLoggingDbContext>, IAuditLoggingDbContext
    {
        public DbSet<AuditLog> AuditLogs { get; set; }

        public LeopardAuditLoggingDbContext(DbContextOptions<LeopardAuditLoggingDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAuditLogging();

            builder.DbMapperCameNamelToUnder();
        }
    }
}
