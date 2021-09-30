using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Leopard.Saas.EntityFrameworkCore
{
    public class SaasHttpApiHostMigrationsDbContext : AbpDbContext<SaasHttpApiHostMigrationsDbContext>
    {
        public SaasHttpApiHostMigrationsDbContext(DbContextOptions<SaasHttpApiHostMigrationsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureSaas();
        }
    }
}
