﻿using Leopard.AuthServer;
using Leopard.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Leopard
{
    public class LeopardModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            var authServerOptionsSection = configuration.GetSection(AuthServerOptions.SectionName);
            if (!authServerOptionsSection.Exists())
            {
                throw new Exception($"配置文件中缺少{AuthServerOptions.SectionName}节点的配置");
            }
            Configure<AuthServerOptions>(authServerOptionsSection);

            #region AppSettings 不注册为Options了，直接使用 App 来访问
            //var appSettings = configuration.GetSection(AppSettingsOptions.SectionName);
            //Configure<AppSettingsOptions>(appSettings);
            #endregion

            App.Init(configuration);

            if (App.Settings.EnableMiniProfiler == true)
            {
                // 为什么在 Leopard 程序集中引入 MiniProfiler。
                // 因为，在任何一个程序集中，都可能调用 MiniProfiler 中定义的收集性能的方法
                context.Services.AddMiniProfiler(options =>
                {
                    // profiler的路径 /profiler
                    // 分析报告路径   /profiler/results
                    options.RouteBasePath = "/profiler";
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
