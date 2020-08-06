using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Leopard.Abp.BackgroundJobs.EntityFrameworkCore
{
    [ConnectionStringName(LeopardDbProperties.DefaultDbConnectionStringName)]
    public class LeopardBackgroundJobsDbContext : AbpDbContext<LeopardBackgroundJobsDbContext>, IBackgroundJobsDbContext
    {
        public DbSet<BackgroundJobRecord> BackgroundJobs { get; set; }

        public LeopardBackgroundJobsDbContext(DbContextOptions<LeopardBackgroundJobsDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBackgroundJobs();

            builder.DbMapperCameNamelToUnder();
        }
    }
}