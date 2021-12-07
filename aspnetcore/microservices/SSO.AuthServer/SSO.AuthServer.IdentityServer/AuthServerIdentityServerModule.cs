using Leopard;
using Leopard.Buiness.Shared;
using Leopard.Consul;
using Leopard.Saas;
using Leopard.Saas.EntityFrameworkCore;
using Leopard.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.UI.Navigation.Urls;

namespace SSO.AuthServer
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),

        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),

        // 引入saas，是因为做多租户登录时，需要查询租户数据
        typeof(LeopardSaasApplicationModule),
        typeof(LeopardSaasEntityFrameworkCoreModule),

        typeof(LeopardConsulModule)
        )]
    public class AuthServerIdentityServerModule : HostCommonModule
    {
        public AuthServerIdentityServerModule() : base(ModuleIdentity.AuthIdentityServer.ServiceType, ModuleIdentity.AuthIdentityServer.Name, MultiTenancyConsts.IsEnabled)
        { }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
            });

            //Configure<AbpLocalizationOptions>(options =>
            //{
            //    options.Resources
            //        .Get<AuthServerResource>()
            //        .AddBaseTypes(
            //            typeof(AbpUiResource)
            //        );
            //});

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

            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"].Split(','));

                options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
                options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
            });

            Configure<AbpBackgroundJobOptions>(options =>
            {
                options.IsJobExecutionEnabled = false;
            });

            base.ConfigureServices(context);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            LeopardApplicationInitialization(
                context,
                betweenAuthApplicationInitialization: (ctx) =>
                    {
                        var app = ctx.GetApplicationBuilder();
                        app.UseIdentityServer();
                    });
        }


    }
}
