using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Leopard.AspNetCore.Mvc
{
    [DependsOn(typeof(LeopardModule))]
    public class LeopardAspNetCoreMvcModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            if (App.Settings.EnableMiniProfiler == true)
            {
                // 为什么是在 Leopard 程序集中引入 MiniProfiler。
                // 因为，在任何一个程序集中，都可能调用 MiniProfiler 中定义的收集性能的方法

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
            if (App.Settings.EnableMiniProfiler == true)
            {
                // 一定要把它放在UseMvc()方法之前。 
                app.UseMiniProfiler();
            }
        }
    }
}
