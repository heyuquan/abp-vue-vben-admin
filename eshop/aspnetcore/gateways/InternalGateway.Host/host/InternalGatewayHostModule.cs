using Leopard;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using EShop.Shared;
using Leopard.Consul;
using Leopard.Gateway;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace InternalGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardModule),
        typeof(LeopardConsulModule),
        typeof(LeopardAspNetCoreSwashbuckleModule)
        )]
    public class InternalGatewayHostModule : LeopardGatewayModule
    {
        public InternalGatewayHostModule() : base(ModuleIdentity.InternalGateway.ServiceType, ModuleIdentity.InternalGateway.Name)
        { }
        
    }
}
