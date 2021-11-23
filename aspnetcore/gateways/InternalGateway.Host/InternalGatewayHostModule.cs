using Leopard;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.Consul;
using Leopard.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace InternalGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardConsulModule),
        typeof(LeopardAspNetCoreSwashbuckleModule)
        )]
    public class InternalGatewayHostModule : GatewayCommonModule
    {
        public InternalGatewayHostModule() : base(ApplicationServiceType.GateWay, "InternalGateway", false)
        { }
        
    }
}
