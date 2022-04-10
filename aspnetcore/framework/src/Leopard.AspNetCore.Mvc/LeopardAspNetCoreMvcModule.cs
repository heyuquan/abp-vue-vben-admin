using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Leopard.AspNetCore.Mvc
{
    public class LeopardAspNetCoreMvcModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            if (App.Settings.EnableMiniProfiler == true)
            {
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
                app.UseMiniProfiler();
            }
        }
    }
}
