using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Mk.DemoC.EntityFrameworkCore
{
    [ConnectionStringName(DemoCDbProperties.ConnectionStringName)]
    public class DemoCDbContext : AbpDbContext<DemoCDbContext>, IDemoCDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public DemoCDbContext(DbContextOptions<DemoCDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureDemoC();
        }
    }
}