using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EShop.Administration.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class AdministrationDbContextFactory : IDesignTimeDbContextFactory<AdministrationDbContext>
    {
        public AdministrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            string conn = configuration.GetConnectionString("Default");
            var builder = new DbContextOptionsBuilder<AdministrationDbContext>()
                .UseMySql(conn, ServerVersion.AutoDetect(conn));

            return new AdministrationDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../EShop.Administration.HttpApi.Host/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
