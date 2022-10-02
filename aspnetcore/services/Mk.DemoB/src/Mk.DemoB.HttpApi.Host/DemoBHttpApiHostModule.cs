using Leopard;
using Leopard.AspNetCore.Mvc.Filters;
using Leopard.Buiness.Shared;
using Leopard.Consul;
using Leopard.Host;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk.DemoB.Localization;
using System.IO;
using System.Linq;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Guids;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace Mk.DemoB
{
    [DependsOn(
        typeof(DemoBHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(DemoBApplicationModule),
        typeof(AbpEventBusRabbitMqModule),
        typeof(AbpSwashbuckleModule),
        typeof(LeopardConsulModule)
        )]
    public class DemoBHttpApiHostModule  : CommonHostModule
    {
        public DemoBHttpApiHostModule() : base(ApplicationServiceType.ApiHost, "MkDemoB", MultiTenancyConsts.IsEnabled)
        { }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            // 自动API控制器
            ConfigureConventionalControllers();

            ConfigureVirtualFileSystem(context);

            // guid的排序规则
            Configure<AbpSequentialGuidGeneratorOptions>(options =>
            {
                options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
            });

            // 使用错误代码
            context.Services.Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("DemoBError", typeof(DemoBResource));
            });

            // 禁用 BackgroundJob
            //Configure<AbpBackgroundJobOptions>(options =>
            //{
            //    options.IsJobExecutionEnabled = false;
            //});

            Configure<MvcOptions>(mvcOptions =>
            {
                // 全局异常替换
                // https://www.cnblogs.com/twoBcoder/p/12838913.html
                var index = mvcOptions.Filters.ToList().FindIndex(filter => filter is ServiceFilterAttribute attr && attr.ServiceType.Equals(typeof(AbpExceptionFilter)));
                if (index > -1)
                    mvcOptions.Filters.RemoveAt(index);
                mvcOptions.Filters.Add(typeof(LeopardExceptionFilter));
            });

            //Configure<AbpDistributedEntityEventOptions>(options =>
            //{
            //    options.AutoEventSelectors.AddAll();
            //});

            //context.Services.AddHttpsRedirection(options =>
            //{
            //    // 默认情况下，该 app.UseHttpsRedirection() 发出307临时重定向响应
            //    // 如果没有代码中指定https端口，则该类将从HTTPS_PORT环境变量或IServerAddress功能获取https端口。
            //    // .netcore的证书需要 pfx格式
            //    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            //    options.HttpsPort = 44305;
            //});

        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<DemoBDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Mk.DemoB.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<DemoBDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Mk.DemoB.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<DemoBApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Mk.DemoB.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<DemoBApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Mk.DemoB.Application"));
                });
            }
        }

        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(DemoBApplicationModule).Assembly);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
        }
    }
}
