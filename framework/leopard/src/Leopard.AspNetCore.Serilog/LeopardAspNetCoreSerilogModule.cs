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

            context.Services.Configure<LoggerOptions>(configuration.GetSection(LoggerOptions.SectionName));
        }
    }
}
