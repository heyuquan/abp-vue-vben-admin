using Leopard;
using Leopard.Base.Shared;
using Leopard.Consul;
using Leopard.Host;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk.DemoC.EntityFrameworkCore;
using System.IO;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Mk.DemoC
{
    [DependsOn(
        typeof(DemoCApplicationModule),
        typeof(DemoCEntityFrameworkCoreModule),
        typeof(DemoCHttpApiModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(AbpAutofacModule),
        
        typeof(AbpEventBusRabbitMqModule),
        typeof(LeopardConsulModule)
        )]
    public class DemoCHttpApiHostModule : CommonHostModule
    {
        public DemoCHttpApiHostModule() : base(ApplicationServiceType.ApiHost, "MkDemoC", MultiTenancyConsts.IsEnabled)
        { }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            // 自动API控制器
            ConfigureConventionalControllers();
         
            ConfigureLocalization();       

            //context.Services.AddHttpsRedirection(options =>
            //{
            //    // 默认情况下，该 app.UseHttpsRedirection() 发出307临时重定向响应
            //    // 如果没有代码中指定https端口，则该类将从HTTPS_PORT环境变量或IServerAddress功能获取https端口。
            //    // .netcore的证书需要 pfx格式
            //    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            //    options.HttpsPort = 44402;
            //});
        }
   
        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<DemoCDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Mk.DemoC.Domain.Shared", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<DemoCDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Mk.DemoC.Domain", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<DemoCApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Mk.DemoC.Application.Contracts", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<DemoCApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Mk.DemoC.Application", Path.DirectorySeparatorChar)));
                });
            }
        }

        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(DemoCApplicationModule).Assembly);
            });
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
        }
    }
}
