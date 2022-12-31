using Leopard.Identity.Web.Navigation;
using Leopard.UI.Navigation;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Leopard.BackendAdmin.Web
{
    [DependsOn(
        typeof(LeopardUiNavigationModule),
        typeof(LeopardBackendAdminApplicationContractsModule)
    )]
    public class LeopardBackendAdminWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new BackendAdminWebMenuContributor());
            });
        }
    }
}
