using Leopard.Buiness.Shared;
using Leopard.Consul;
using Leopard.Utils.Host.Leopard.Utils;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SSO.AuthServer.EntityFrameworkCore;
using SSO.AuthServer.Localization;
using System.IO;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace SSO.AuthServer
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AuthServerEntityFrameworkCoreModule),
        typeof(LeopardConsulModule)
        )]
    public class AuthServerIdentityServerModule : HostCommonModule
    {
        public AuthServerIdentityServerModule() : base("AuthServerIdentityServer", MultiTenancyConsts.IsEnabled)
        { }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    BasicThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-styles.css");
                    }
                );
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AuthServerResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<AuthServerDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}SSO.AuthServer.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AuthServerDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}SSO.AuthServer.Domain"));
                });
            }


            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"].Split(','));

                options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
                options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
            });

            base.ConfigureServices(context);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
        }
    }
}
