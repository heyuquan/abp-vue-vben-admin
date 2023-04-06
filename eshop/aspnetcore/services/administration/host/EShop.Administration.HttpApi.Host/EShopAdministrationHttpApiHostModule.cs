using EShop.Administration.EntityFrameworkCore;
using EShop.Administration.Web;
using EShop.Shared;
using EShop.Identity;
using Leopard.Consul;
using Leopard.Host;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace EShop.Administration
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(LeopardConsulModule),
        typeof(EShopAdministrationWebModule),
        typeof(EShopAdministrationApplicationModule),
        typeof(EShopAdministrationEntityFrameworkCoreModule),
        typeof(EShopAdministrationHttpApiModule)
    )]
    public class EShopAdministrationHttpApiHostModule : CommonHostModule
    {
        public EShopAdministrationHttpApiHostModule() : base(ModuleIdentity.Administration.ServiceType, ModuleIdentity.Administration.Name, MultiTenancyConsts.IsEnabled)
        { }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            ConfigureVirtualFileSystem(context);

            Configure<AbpLocalizationOptions>(options =>
            {
                // vben admin 语言映射
                options
                    .AddLanguagesMapOrUpdate(
                        "vben-admin-ui",
                        new NameValue("zh_CN", "zh-Hans"));

                options
                    .AddLanguageFilesMapOrUpdate(
                        "vben-admin-ui",
                        new NameValue("zh_CN", "zh-Hans"));
            });

            base.ConfigureServices(context);
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<EShopAdministrationDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EShop.Administration.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<EShopAdministrationDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EShop.Administration.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<EShopAdministrationApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EShop.Administration.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<EShopAdministrationApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EShop.Administration.Application"));
                });
            }
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
