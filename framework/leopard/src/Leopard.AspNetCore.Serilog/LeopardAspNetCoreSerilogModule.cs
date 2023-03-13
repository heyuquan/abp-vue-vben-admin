using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;

namespace Leopard.AspNetCore.Serilog
{
    [DependsOn(
        typeof(AbpAspNetCoreSerilogModule)
    )]
    public class LeopardAspNetCoreSerilogModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<LeopardLogOptions>(configuration.GetSection(LeopardLogOptions.SectionName));

            context.Services.AddOptions<LeopardLogOptions>()
                .Bind(configuration.GetSection(LeopardLogOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}
