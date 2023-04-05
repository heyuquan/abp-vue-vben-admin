using EShop.Common.Shared;
using Leopard.Consul;
using Leopard.Host;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;

namespace EShop.Identity;

[DependsOn(
    typeof(AbpAutofacModule),

    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),

    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),

    typeof(EShopIdentityEntityFrameworkCoreModule),

    typeof(LeopardConsulModule)
    )]
public class EShopIdentityAuthServerModule : CommonHostModule
{
    public EShopIdentityAuthServerModule() : base(ModuleIdentity.IdentityAuthServer.ServiceType, ModuleIdentity.IdentityAuthServer.Name, MultiTenancyConsts.IsEnabled)
    { }

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("EShop.Identity.AuthServer");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
            builder.AddServer(options =>
            {
                options.SetAccessTokenLifetime(TimeSpan.FromDays(365));
            });
        });
    }

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
                app.UseAbpOpenIddictValidation();
            });
    }
}
