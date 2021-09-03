using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Mk.DemoB.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class DemoBMigrationsDbContextFactory : IDesignTimeDbContextFactory<DemoBMigrationsDbContext>
    {
        public DemoBMigrationsDbContext CreateDbContext(string[] args)
        {
            DemoBEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var version = new MySqlServerVersion(new Version(5, 7, 33));
            var builder = new DbContextOptionsBuilder<DemoBMigrationsDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"), version);

            return new DemoBMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Mk.DemoB.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
