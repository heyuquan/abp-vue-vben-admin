using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Leopard.Saas
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(SaasDomainSharedModule)
    )]
    public class SaasDomainModule : AbpModule
    {

    }
}
