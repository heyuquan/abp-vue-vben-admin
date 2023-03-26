using EShop.Common.Shared;
using Leopard;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.Consul;
using Leopard.Gateway;
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
    public class PublicWebGatewayHostModule : CommonGatewayModule
    {
        public PublicWebGatewayHostModule() : base(ModuleIdentity.PublicWebGateway.ServiceType, ModuleIdentity.PublicWebGateway.Name)
        {
        }
    }
}
