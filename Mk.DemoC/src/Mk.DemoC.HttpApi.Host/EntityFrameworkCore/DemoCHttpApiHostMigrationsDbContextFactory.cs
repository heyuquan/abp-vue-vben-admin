using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Mk.DemoC.EntityFrameworkCore
{
    public class DemoCHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<DemoCHttpApiHostMigrationsDbContext>
    {
        public DemoCHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<DemoCHttpApiHostMigrationsDbContext>()
                .UseMySql(configuration.GetConnectionString("DemoC"));

            return new DemoCHttpApiHostMigrationsDbContext(builder.Options);
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
