using Leopard;
using Leopard.Buiness.Shared;
using Leopard.Consul;
using Leopard.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SSO.AuthServer.EntityFrameworkCore;
using System.IO;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace SSO.AuthServer
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),

        typeof(LeopardConsulModule),

        typeof(AuthServerHttpApiModule),
        typeof(AuthServerApplicationModule),
        typeof(AuthServerEntityFrameworkCoreModule)
    )]
    public class AuthServerHttpApiHostModule : HostCommonModule
    {
        public AuthServerHttpApiHostModule() : base(ApplicationServiceType.AuthHost, "AuthServer", MultiTenancyConsts.IsEnabled)
        { }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<AuthServerDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}SSO.AuthServer.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AuthServerDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}SSO.AuthServer.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AuthServerApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}SSO.AuthServer.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AuthServerApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}SSO.AuthServer.Application"));
                });
            }

            base.ConfigureServices(context);

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
        }
    }
}
