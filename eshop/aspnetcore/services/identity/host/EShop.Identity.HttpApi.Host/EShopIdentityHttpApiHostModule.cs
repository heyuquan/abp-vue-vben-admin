using EShop.Shared;
using EShop.Identity.Microsoft.Extensions.DependencyInjection;
using Leopard;
using Leopard.Host;
using Leopard.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Threading;

namespace EShop.Identity;

[DependsOn(
   
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),

    typeof(EShopIdentityHttpApiModule),
    typeof(EShopIdentityApplicationModule),
    typeof(EShopIdentityEntityFrameworkCoreModule),

    // 在执行 Volo.Abp.Identity.IdentityDataSeeder 的时候会用到Setting
    typeof(AbpSettingManagementEntityFrameworkCoreModule)
)]
public class EShopIdentityHttpApiHostModule : LeopardHostModule
{
    public EShopIdentityHttpApiHostModule() : base(ModuleIdentity.Identity.ServiceType, ModuleIdentity.Identity.Name, MultiTenancyConsts.IsEnabled)
    { }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureSameSiteCookiePolicy(context);

        base.ConfigureServices(context);

    }

    private void ConfigureSameSiteCookiePolicy(ServiceConfigurationContext context)
    {
        context.Services.AddSameSiteCookiePolicy();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        base.OnApplicationInitialization(context);

        SeedData(context);
    }

    private void SeedData(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(async () =>
        {
            using var scope = context.ServiceProvider.CreateScope();
            await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync();
        });
    }
}
