using Leopard.AuthServer;
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
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
        }
    }
}
