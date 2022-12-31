using System;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Leopard.UI.Navigation
{
    [DependsOn(
        typeof(AbpUiNavigationModule)
    )]
    public class LeopardUiNavigationModule : AbpModule
    {
    }
}
