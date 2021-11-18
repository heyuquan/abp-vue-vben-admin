using Leopard.AspNetCore.Serilog;
using Leopard.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Leopard.Utils
{
    // 备注：为什么不做成 Program 的基类。因为Program中必须要求显示定义 public static int Main(string[] args) 方法
    /// <summary>
    /// 共用 Program
    /// </summary>
    public class CommonProgram
    {
        private string _assemblyName { get; set; }
        protected ApplicationServiceType ApplicationServiceType { get; private set; }

        public CommonProgram(ApplicationServiceType serviceType, string assemblyName)
        {
            ApplicationServiceType = serviceType;
            _assemblyName = assemblyName;
        }

        private static readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public int CommonMain<T>(string[] args) where T : class
        {
            SerilogConfigurationHelper.Configure(env, _assemblyName, true, false);

            try
            {
                Log.Information($"Starting {_assemblyName}.");
                CreateHostBuilder<T>(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"{_assemblyName} terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal IHostBuilder CreateHostBuilder<T>(string[] args) where T : class =>
           Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    if (ApplicationServiceType == ApplicationServiceType.GateWay)
                    {
                        config.AddJsonFile("ocelot.json");
                    }
                    config.AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    string certFilePath = PathHelper.GetRuntimeDirectory(AppContext.BaseDirectory + "Configs/Cert/test.pfx");
                    Console.WriteLine(certFilePath);

                    webBuilder.UseStartup<T>();
                   // .UseKestrel(option =>
                   //{
                       
                   //     //option.ConfigureHttpsDefaults(o =>
                   //     //{
                   //     //    o.ServerCertificate = new X509Certificate2(certFilePath, "a369220123");
                   //     //});
                   // });
                })
                .UseAutofac()
                .UseSerilog();
    }
}
