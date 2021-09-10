using Leopard.AspNetCore.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace SSO.AuthServer
{
    public class Program
    {
        private static readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public static int Main(string[] args)
        {
            var assemblyName = typeof(Program).Assembly.GetName().Name;

            SerilogConfigurationHelper.Configure(env, assemblyName, true, false);

            try
            {
                Log.Information($"Starting {assemblyName}.");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"{assemblyName} terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(build =>
                {
                    build.AddJsonFile("appsettings.secrets.json", optional: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac()
                .UseSerilog();
    }
}
