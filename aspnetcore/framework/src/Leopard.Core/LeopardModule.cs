using Leopard.AuthServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
        }
    }
}
