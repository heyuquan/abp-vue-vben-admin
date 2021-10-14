using Localization.Resources.AbpUi;
using SSO.AuthServer.Localization;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace SSO.AuthServer
{
    [DependsOn(
        typeof(AuthServerApplicationContractsModule),
        typeof(AbpAccountHttpApiModule),
        typeof(AbpIdentityHttpApiModule)
        )]
    public class AuthServerHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AuthServerResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
