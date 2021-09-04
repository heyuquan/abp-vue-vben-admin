using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SSO.AuthServer.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class AuthServerDbContextFactory : IDesignTimeDbContextFactory<AuthServerDbContext>
    {
        public AuthServerDbContext CreateDbContext(string[] args)
        {
            AuthServerEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var version = new MySqlServerVersion(new Version(5, 7, 33));
            var builder = new DbContextOptionsBuilder<AuthServerDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"), version);

            return new AuthServerDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SSO.AuthServer.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
