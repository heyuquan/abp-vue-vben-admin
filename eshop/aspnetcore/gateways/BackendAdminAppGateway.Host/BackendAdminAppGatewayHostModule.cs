using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.BackendAdmin;
using Leopard.Base.Shared;
using Leopard.Consul;
using Leopard.Gateway;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace BackendAdminAppGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardConsulModule),
        typeof(LeopardAspNetCoreSwashbuckleModule),

        typeof(LeopardBackendAdminHttpApiModule)
    )]
    public class BackendAdminAppGatewayHostModule : CommonGatewayModule
    {
        public BackendAdminAppGatewayHostModule() : base(ModuleIdentity.BackendAdminAppGateway.ServiceType, ModuleIdentity.BackendAdminAppGateway.Name, false)
        { }        
    }
}
