using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk.DemoC.EntityFrameworkCore;
using Mk.DemoC.MultiTenancy;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.VirtualFileSystem;
using Leopard.Abp.AuditLogging.EntityFrameworkCore;
using Leopard.Abp.PermissionManagement.EntityFrameworkCore;
using Leopard.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.EventBus.RabbitMq;
using Microsoft.AspNetCore.Mvc;
using Leopard.AspNetCore.Mvc.Filters;
using Volo.Abp.Timing;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Leopard.Consul;
using Serilog;

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
        typeof(LeopardAuditLoggingEntityFrameworkCoreModule),
        typeof(LeopardPermissionManagementEntityFrameworkCoreModule),
        typeof(LeopardSettingManagementEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpEventBusRabbitMqModule)
        //typeof(LeopardConsulModule)
        )]
    public class DemoCHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(DemoCApplicationModule).Assembly, opt => {
                    // 默认是：/api/app/***
                    //如下修改为：/api/volosoft/book-store/***
                    //opts.RootPath = "volosoft/book-store";
                });
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
            });

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });

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

            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "DemoC API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);

                    // 为 Swagger JSON and UI设置xml文档注释路径
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Mk.DemoC.Application.xml"), true);
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Mk.DemoC.Application.Contracts.xml"), true);
                });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            });

            //Updates AbpClaimTypes to be compatible with identity server claims.
            AbpClaimTypes.UserId = JwtClaimTypes.Subject;
            AbpClaimTypes.UserName = JwtClaimTypes.Name;
            AbpClaimTypes.Role = JwtClaimTypes.Role;
            AbpClaimTypes.Email = JwtClaimTypes.Email;

            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "DemoC";
                });

            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "DemoC:";
            });

            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                //context.Services
                //    .AddDataProtection()
                //    .PersistKeysToStackExchangeRedis(redis, "DemoC-Protection-Keys");
            }

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
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseErrorPage();
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseCorrelationId();
         
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);        
            app.UseAuthentication();
            app.UseAbpClaimsMap();
            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }
            app.UseAbpRequestLocalization();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");
            });
            //app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
