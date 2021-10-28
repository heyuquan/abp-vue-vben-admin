using Leopard.Identity.Web.Navigation;
using Leopard.UI.Navigation;
using System;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Leopard.Identity.Web
{
    [DependsOn(
        typeof(LeopardUiNavigationModule),
        typeof(LeopardIdentityApplicationContractsModule)
        )]
    public class LeopardIdentityWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new LeopardIdentityWebMenuContributor());
            });
        }
    }
}
