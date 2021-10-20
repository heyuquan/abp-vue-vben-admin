using Leopard.BackendAdmin.EntityFrameworkCore;
using Leopard.Buiness.Shared;
using Leopard.Consul;
using Leopard.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Leopard.BackendAdmin
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(LeopardConsulModule),
        typeof(BackendAdminApplicationModule),
        typeof(BackendAdminEntityFrameworkCoreModule),
        typeof(BackendAdminHttpApiModule)
    )]
    public class BackendAdminHttpApiHostModule : HostCommonModule
    {
        public BackendAdminHttpApiHostModule() : base(ApplicationServiceType.ApiHost, "BackendAdmin", MultiTenancyConsts.IsEnabled)
        { }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            ConfigureVirtualFileSystem(context);

            base.ConfigureServices(context);
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<BackendAdminDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Leopard.BackendAdmin.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<BackendAdminDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Leopard.BackendAdmin.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<BackendAdminApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Leopard.BackendAdmin.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<BackendAdminApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Leopard.BackendAdmin.Application"));
                });
            }
        }


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
        }
    }
}
