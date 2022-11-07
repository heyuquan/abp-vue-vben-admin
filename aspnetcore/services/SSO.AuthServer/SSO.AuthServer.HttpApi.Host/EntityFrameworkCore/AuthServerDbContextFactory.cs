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
            var configuration = BuildConfiguration();

            string conn = configuration.GetConnectionString("Default");
            var builder = new DbContextOptionsBuilder<AuthServerDbContext>()
                .UseMySql(conn, ServerVersion.AutoDetect(conn));

            return new AuthServerDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SSO.AuthServer.HttpApi.Host/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
