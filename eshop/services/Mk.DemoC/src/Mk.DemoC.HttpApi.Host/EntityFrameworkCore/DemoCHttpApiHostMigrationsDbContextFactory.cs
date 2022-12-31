using System;
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

            string conn = configuration.GetConnectionString(DemoCDbProperties.ConnectionStringName);
            var builder = new DbContextOptionsBuilder<DemoCHttpApiHostMigrationsDbContext>()
                .UseMySql(conn, ServerVersion.AutoDetect(conn));

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
