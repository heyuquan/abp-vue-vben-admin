using Volo.Abp.Modularity;

namespace Leopard.AspNetCore.Mvc
{
    [DependsOn(typeof(LeopardModule))]
    public class LeopardAspNetCoreMvcModule : AbpModule
    {
    }
}
