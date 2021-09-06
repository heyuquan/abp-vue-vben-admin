using Leopard.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mk.DemoC.EntityFrameworkCore;
using MsDemo.Shared;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Timing;
using Volo.Abp.VirtualFileSystem;

namespace Mk.DemoC
{
    [DependsOn(
        typeof(DemoCApplicationModule),
        typeof(DemoCEntityFrameworkCoreModule),
        typeof(DemoCHttpApiModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpEventBusRabbitMqModule),
        typeof(AbpSwashbuckleModule)
        // 注册consul后，负载是正常的。但是不知道为什么 kibana 就会一直报错。  
        // 可能是consul做health检查时，日志格式问题？？但依旧找不到具体原因，所以注释掉consul
        //typeof(LeopardConsulModule)  
        )]
    public class DemoCHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            // 自动API控制器
            ConfigureConventionalControllers();

            ConfigureAuthentication(context, configuration);
            ConfigureLocalization();
            ConfigureCache(configuration);
            ConfigureVirtualFileSystem(context);
            ConfigureRedis(context, configuration, hostingEnvironment);
            ConfigureCors(context, configuration);
            ConfigureSwaggerServices(context, configuration);

            // 设置分页默认返回20条数据   
            LimitedResultRequestDto.DefaultMaxResultCount = 20;

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MsDemoConsts.IsMultiTenancyEnabled;
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
            });

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MsDemoConsts.IsMultiTenancyEnabled;
            });

            Configure<MvcOptions>(mvcOptions =>
            {
                // 全局异常替换
                // https://www.cnblogs.com/twoBcoder/p/12838913.html
                var index = mvcOptions.Filters.ToList().FindIndex(filter => filter is ServiceFilterAttribute attr && attr.ServiceType.Equals(typeof(AbpExceptionFilter)));
                if (index > -1)
                    mvcOptions.Filters.RemoveAt(index);
                mvcOptions.Filters.Add(typeof(LeopardExceptionFilter));
            });

            Configure<AbpClockOptions>(options =>
            {
                options.Kind = DateTimeKind.Utc;
            });

            //context.Services.AddHttpsRedirection(options =>
            //{
            //    // 默认情况下，该 app.UseHttpsRedirection() 发出307临时重定向响应
            //    // 如果没有代码中指定https端口，则该类将从HTTPS_PORT环境变量或IServerAddress功能获取https端口。
            //    // .netcore的证书需要 pfx格式
            //    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            //    options.HttpsPort = 44402;
            //});
        }

        private void ConfigureCache(IConfiguration configuration)
        {
            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "MkDemoC:";
            });
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
                options.ConventionalControllers.Create(typeof(DemoCApplicationModule).Assembly, opt =>
                {
                    // 默认是：/api/app/***
                    //如下修改为：/api/volosoft/book-store/***
                    //opts.RootPath = "volosoft/book-store";
                });
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.ApiName = "MkDemoC";
                });
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"],
                new Dictionary<string, string>
                {
                    {"MkDemoC", "MkDemoC API"}
                },
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MkDemoC API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    //options.SchemaFilter<EnumSchemaFilter>();
                    options.CustomSchemaIds(type => type.FullName);
                    // 为 Swagger JSON and UI设置xml文档注释路径
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Mk.DemoC.Application.xml"), true);
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Mk.DemoC.Application.Contracts.xml"), true);
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

        private void ConfigureRedis(
            ServiceConfigurationContext context,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "MkDemoC-Protection-Keys");
            }
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            //app.UseHttpsRedirection();
            app.UseCorrelationId();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseAbpClaimsMap();
            if (MsDemoConsts.IsMultiTenancyEnabled)
            {
                app.UseMultiTenancy();
            }
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MkDemoC API");

                var configuration = context.GetConfiguration();
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
                options.OAuthScopes("MkDemoC");
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();
        }
    }
}
