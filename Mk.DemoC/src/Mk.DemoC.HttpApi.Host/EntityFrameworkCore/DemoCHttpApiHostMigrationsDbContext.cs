using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Mk.DemoC.EntityFrameworkCore
{
    public class DemoCHttpApiHostMigrationsDbContext : AbpDbContext<DemoCHttpApiHostMigrationsDbContext>
    {
        public DemoCHttpApiHostMigrationsDbContext(DbContextOptions<DemoCHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureDemoC();
        }
    }
}
