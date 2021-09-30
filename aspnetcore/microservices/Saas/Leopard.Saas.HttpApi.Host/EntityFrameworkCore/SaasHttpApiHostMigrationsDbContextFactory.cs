using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Leopard.Saas.EntityFrameworkCore
{
    public class SaasHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<SaasHttpApiHostMigrationsDbContext>
    {
        public SaasHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<SaasHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Saas"));

            return new SaasHttpApiHostMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
