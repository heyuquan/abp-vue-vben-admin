using Leopard;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.Buiness.Shared;
using Leopard.Consul;
using Leopard.Utils;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace PublicWebSiteGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardModule),
        typeof(LeopardConsulModule),
        typeof(LeopardAspNetCoreSwashbuckleModule)
        )]
    public class PublicWebSiteGatewayHostModule : GatewayCommonModule
    {
        public PublicWebSiteGatewayHostModule() : base(ModuleIdentity.PublicWebSiteGateway.ServiceType, ModuleIdentity.PublicWebSiteGateway.Name, false)
        {
        }
    }
}
