using Leopard.MiniProfiler.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Leopard.AspNetCore.Mvc
{
    [DependsOn(typeof(LeopardModule))]
    public class LeopardMiniProfilerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            var miniProfilerOptions = configuration.Get<MiniProfilerOptions>() ?? new MiniProfilerOptions();
            Configure<MiniProfilerOptions>(options => options = miniProfilerOptions);

            if (miniProfilerOptions.IsEnabled)
            {
                // ASP.NET CORE MVC用时分析工具MiniProfiler
                // https://www.cnblogs.com/Zhengxue/p/13206769.html

                context.Services.AddMiniProfiler(options =>
                {
                    // 默认的路径是 /mini-profiler-resources
                    // 分析报告路径 /mini-profiler-resources/results
                    options.RouteBasePath = "/mini-profiler";
                });
            }
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            var miniProfilerOptions = context.ServiceProvider.GetRequiredService<IOptions<MiniProfilerOptions>>().Value;
            if (!miniProfilerOptions.IsEnabled)
            {
                // 一定要把它放在UseMvc()方法之前。 
                app.UseMiniProfiler();
            }
        }
    }
}
