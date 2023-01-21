using EShop.Administration.Web.Navigation;
using Leopard.UI.Navigation;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace EShop.Administration.Web
{
    [DependsOn(
        typeof(LeopardUiNavigationModule)
    )]
    public class EShopAdministrationWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AdministrationWebMenuContributor());
            });
        }
    }
}
