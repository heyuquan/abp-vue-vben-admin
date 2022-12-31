using Leopard;
using Leopard.Base.Shared;
using Leopard.Consul;
using Leopard.EntityFrameworkCore;
using Leopard.Host;
using Microsoft.Extensions.DependencyInjection;
using EShop.AuthServer.Localization;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Threading;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace EShop.AuthServer
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),

        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),

        typeof(AbpAccountApplicationModule),
        typeof(AbpAccountHttpApiClientModule),

        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityHttpApiClientModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),

        typeof(LeopardModule),
        typeof(LeopardConsulModule),
        typeof(LeopardEntityFrameworkCoreModule)
    )]
    public class EShopAuthServerHttpApiHostModule : CommonHostModule
    {
        public EShopAuthServerHttpApiHostModule() : base(ModuleIdentity.Auth.ServiceType, ModuleIdentity.Auth.Name, MultiTenancyConsts.IsEnabled)
        { }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            ConfigureSameSiteCookiePolicy(context);

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<EShopAuthServerHttpApiHostModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AuthServerResource>()
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Resource");

                options.DefaultResourceType = typeof(AuthServerResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace(ModuleNames.AuthServer, typeof(AuthServerResource));
            });

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
}
