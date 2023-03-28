using EShop.Common.Shared;
using Leopard.Gateway;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AdministrationGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule)
        //typeof(LeopardConsulModule),
    )]
    public class AdministrationGatewayHostModule : CommonGatewayModule
    {
        public AdministrationGatewayHostModule() : base(ModuleIdentity.AdministrationGateway.ServiceType, ModuleIdentity.AdministrationGateway.Name)
        { }        
    }
}
