using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Leopard.Saas.EntityFrameworkCore
{
    public class SaasHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<SaasHttpApiHostMigrationsDbContext>
    {
        public SaasHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var version = new MySqlServerVersion(new Version(5, 7, 33));
            var builder = new DbContextOptionsBuilder<SaasHttpApiHostMigrationsDbContext>()
                .UseMySql(configuration.GetConnectionString(SaasServiceDbProperties.ConnectionStringName), version);

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
