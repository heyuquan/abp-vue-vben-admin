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
    }
}
