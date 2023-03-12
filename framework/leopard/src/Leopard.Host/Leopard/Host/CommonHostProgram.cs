using Leopard.AspNetCore.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Volo.Abp.Modularity;

namespace Leopard.Host
{
    // 备注：为什么不做成 Program 的基类。因为Program中必须要求显示定义 public static int Main(string[] args) 方法
    /// <summary>
    /// 共用 Program
    /// </summary>
    public class CommonHostProgram<TModule>
        where TModule : AbpModule
    {
        /// <summary>
        /// 模块名（模块key）eg：Leopard.Saas
        /// </summary>
        protected string ModuleKey { get; private set; }
        protected ApplicationServiceType ApplicationServiceType { get; private set; }

        public CommonHostProgram(ApplicationServiceType serviceType, string moduleKey)
        {
            ApplicationServiceType = serviceType;
            ModuleKey = moduleKey;
        }

        private static readonly string env = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            (System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Machine))
            : System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Process);

        // 函数main的返回值来告知操作系统函数的执行是否成功。返回值为0代表程序执行成功，返回值非0则表示程序执行失败。

        public async Task<int> RunAsync(string[] args)
        {
            // 最先配置并创建日志，然后再打印日志
            // 注意：不能放到 UseSerilog() 中进行配置和创建，因为程序启动中有异常，还没走到UseSerilog就报错，日志会丢失
            Log.Logger = SerilogHelper.Create(env, ModuleKey, true, false);

            try
            {
                Log.Information($"[{env}] Starting {ModuleKey}.");
                //var host = CreateHostBuilder<T>(args).Build();
                var builder = WebApplication.CreateBuilder(args);
                builder.WebHost.UseKestrel((context, options) =>
                {
                    // 对于 Kestrel 托管的应用，默认的最大请求正文大小为 30,000,000 个字节，约为 28.6 MB
                    // options.Limits.MaxRequestBodySize=null表示不限制
                    options.Limits.MaxRequestBodySize = Constants.RequestLimit.MaxBodyLength_Byte;
                });
                builder.Host
                    .AddAppSettingsSecretsJson()
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        this.ConfigureAppConfiguration(hostingContext, config);
                    })
                    .UseAutofac()
                    .UseSerilog();

                await builder.AddApplicationAsync<TModule>();
                var app = builder.Build();
                await app.InitializeApplicationAsync();

                Log.Information($"[{env}] Started {ModuleKey}.");

                // 如何实现 asp.net core 安全优雅退出 ?
                // https://mp.weixin.qq.com/s/TwPNPwD-XlmKiuuYYk1MeQ
                var life = app.Services.GetRequiredService<IHostApplicationLifetime>();
                life.ApplicationStopped.Register(() =>
                {
                    Log.Information($"{ModuleKey} is shut down");
                });

                await app.RunAsync();
                return 0;
            }
            catch (Exception ex) when (ex is not OperationCanceledException && ex.GetType().Name != "StopTheHostException")
            {
                // Microsoft.Extensions.Hosting.HostFactoryResolver+HostingListener+StopTheHostException: Exception of
                // https://github.com/dotnet/efcore/issues/28478
                // https://github.com/dotnet/runtime/issues/60600

                Log.Fatal(ex, $"{ModuleKey} terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        protected virtual void ConfigureAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            // 给子类重写用
        }
    }
}
