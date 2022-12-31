using EShop.Administration.EntityFrameworkCore;
using Leopard.Base.Shared;
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
        typeof(AdministrationApplicationModule),
        typeof(AdministrationEntityFrameworkCoreModule),
        typeof(AdministrationHttpApiModule)
    )]
    public class AdministrationHttpApiHostModule : CommonHostModule
    {
        public AdministrationHttpApiHostModule() : base(ModuleIdentity.BackendAdmin.ServiceType, ModuleIdentity.BackendAdmin.Name, MultiTenancyConsts.IsEnabled)
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
                    options.FileSets.ReplaceEmbeddedByPhysical<AdministrationDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}EShop.Administration.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AdministrationDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}EShop.Administration.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AdministrationApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}EShop.Administration.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AdministrationApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}EShop.Administration.Application"));
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