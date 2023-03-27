using EShop.Common.Shared;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.Gateway;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace AdministrationAppGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule)
        //typeof(LeopardConsulModule),
    )]
    public class AdministrationAppGatewayHostModule : CommonGatewayModule
    {
        public AdministrationAppGatewayHostModule() : base(ModuleIdentity.AdministrationGateway.ServiceType, ModuleIdentity.AdministrationGateway.Name)
        { }        
    }
}
