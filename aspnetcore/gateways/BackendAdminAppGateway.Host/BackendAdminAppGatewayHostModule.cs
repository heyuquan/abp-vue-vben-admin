using Leopard;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.BackendAdmin;
using Leopard.Consul;
using Leopard.Utils;
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
    public class BackendAdminAppGatewayHostModule : GatewayCommonModule
    {
        public BackendAdminAppGatewayHostModule() : base(ApplicationServiceType.GateWay, "BackendAdminAppGateway", false)
        { }        
    }
}
