using Leopard.AspNetCore.Mvc;
using Leopard.AspNetCore.Mvc.Filters;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Encryption;
using Volo.Abp.Timing;

namespace Leopard.Utils
{

    [DependsOn(
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardAspNetCoreSwashbuckleModule),
        typeof(LeopardAspNetCoreMvcModule)
    )]
    public class HostCommonModule : AbpModule
    {
        /// <summary>
        /// 项目名（项目key）
        /// </summary>
        protected string ProjectKey { get; private set; }
        /// <summary>
        /// 模块名（模块key）
        /// </summary>
        protected string ModuleKey { get; private set; }
        /// <summary>
        /// 是否启用多租户
        /// </summary>
        protected bool IsEnableMultiTenancy { get; private set; }
        /// <summary>
        /// 是否需要认证 （eg：IdentityServer本身是不需要认证的）
        /// </summary>
        protected bool IsRequireAuth { get; private set; }

        protected ApplicationServiceType ApplicationServiceType { get; private set; }

        /// <summary>
        /// 项目组合Key {ProjectKey}.{ModuleKey}
        /// </summary>
        protected readonly string ProjectCombinationKey;

        public HostCommonModule(ApplicationServiceType serviceType, string moduleKey, bool isEnableMultiTenancy)
            : this(serviceType, "Leopard", moduleKey, isEnableMultiTenancy)
        {
            //todo： moduleKey 从这里获取 configuration["AuthServer:SwaggerClientId"]
        }

        public HostCommonModule(
            ApplicationServiceType serviceType
            , string projectKey
            , string moduleKey
            , bool isEnableMultiTenancy) : base()
        {
            ProjectKey = projectKey;
            ModuleKey = moduleKey;
            IsEnableMultiTenancy = isEnableMultiTenancy;
            ProjectCombinationKey = $"{ProjectKey}.{ModuleKey}";
            ApplicationServiceType = serviceType;
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            this.LeopardConfigureServices(context);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            this.LeopardApplicationInitialization(context);
        }

        /// <summary>
        /// LeopardConfigureServices
        /// </summary>
        /// <param name="context"></param>
        /// <param name="otherConfigureServices">其他ConfigureServices，执行上没有顺序要求</param>
        /// <param name="afterConfigureServices">在最后执行的ConfigureServices</param>
        protected void LeopardConfigureServices(
            ServiceConfigurationContext context,
            Action<ServiceConfigurationContext> otherConfigureServices = null,
            Action<ServiceConfigurationContext> afterConfigureServices = null
            )
        {
            if (otherConfigureServices != null)
            {
                otherConfigureServices(context);
            }
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            // 中文序列化的编码问题
            Configure<AbpSystemTextJsonSerializerOptions>(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = IsEnableMultiTenancy;
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

            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
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

#if DEBUG
            context.Services.AddLeopardSwaggerGen();
#endif
            if (ApplicationServiceType == ApplicationServiceType.ApiHost)
            {
                context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = configuration["AuthServer:Authority"];
                        options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                        options.Audience = configuration["AuthServer:SwaggerClientId"];
                    });
            }

            if (IsHost())
            {
                Configure<AbpAuditingOptions>(options =>
                {
                    //options.IsEnabledForGetRequests = true;
                    options.ApplicationName = ModuleKey;
                });

                // 设置分页默认返回20条数据   
                LimitedResultRequestDto.DefaultMaxResultCount = 20;

                // 加解密
                Configure<AbpStringEncryptionOptions>(options =>
                {
                    var encryptionConfiguration = configuration.GetSection("Encryption");
                    if (encryptionConfiguration.Exists())
                    {
                        options.DefaultPassPhrase = encryptionConfiguration["PassPhrase"] ?? options.DefaultPassPhrase;
                        options.DefaultSalt = encryptionConfiguration.GetSection("Salt").Exists()
                            ? Encoding.ASCII.GetBytes(encryptionConfiguration["Salt"])
                            : options.DefaultSalt;
                        options.InitVectorBytes = encryptionConfiguration.GetSection("InitVector").Exists()
                            ? Encoding.ASCII.GetBytes(encryptionConfiguration["InitVector"])
                            : options.InitVectorBytes;
                    }
                });

                Configure<AbpLocalizationOptions>(options =>
                {
                    options.Languages.Add(new LanguageInfo("en", "en", "English"));
                    options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                });

                var redisConfiguration = configuration.GetSection("Redis");
                if (redisConfiguration.Exists())
                {
                    Configure<AbpDistributedCacheOptions>(options =>
                    {
                        // 最好统一命名,不然某个缓存变动其他应用服务有例外发生
                        options.KeyPrefix = $"{ProjectCombinationKey}:";
                        // 滑动过期30天
                        options.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromDays(30);
                        // 绝对过期60天
                        options.GlobalCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
                    });

                    if (!hostingEnvironment.IsDevelopment())
                    {
                        var redis = ConnectionMultiplexer.Connect(redisConfiguration["Configuration"]);
                        context.Services
                            .AddDataProtection()
                            .PersistKeysToStackExchangeRedis(redis, $"{ProjectCombinationKey}-Protection-Keys");
                    }
                }

                Configure<AbpClockOptions>(options =>
                {
                    options.Kind = DateTimeKind.Utc;
                });
            }

            if (afterConfigureServices != null)
            {
                afterConfigureServices(context);
            }
        }

        /// <summary>
        /// LeopardApplicationInitialization
        /// </summary>
        /// <param name="context"></param>
        /// <param name="betweenAuthApplicationInitialization">在认证之后，授权之前执行</param>
        /// <param name="afterApplicationInitialization">在最后执行</param>
        protected void LeopardApplicationInitialization(
            ApplicationInitializationContext context
            , Action<ApplicationInitializationContext> betweenAuthApplicationInitialization = null
            , Action<ApplicationInitializationContext> afterApplicationInitialization = null)
        {
            var app = context.GetApplicationBuilder();

            if (IsHost())
            {
                var env = context.GetEnvironment();

                // 本地化
                app.UseAbpRequestLocalization();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseErrorPage();
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
            }

            // http调用链
            app.UseCorrelationId();
            // 虚拟文件系统
            app.UseStaticFiles();
            //路由
            app.UseRouting();
            app.UseCors();

            // 认证
            app.UseAuthentication();

            if (betweenAuthApplicationInitialization != null)
            {
                betweenAuthApplicationInitialization(context);
            }

            if (IsHost())
            {
                if (IsEnableMultiTenancy)
                {
                    app.UseMultiTenancy();
                }
                // 授权
                app.UseAuthorization();
            }
#if DEBUG
            // swagger
            app.UseSwagger();
            app.UseLeopardSwaggerUI();

            if (IsHost())   // 设置这个后，网关项目启用ocelot会失败（不会进行路由跳转）
            {
                // UseEndpoints 在 UseRouting 之后
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute("default",
                              "{controller=Swagger}/{action=Index}");
                });
            }
#endif
            // Serilog
            app.UseAbpSerilogEnrichers();

            if (IsHost())
            {
                // 审计日志
                app.UseAuditing();
                app.UseUnitOfWork();
                app.UseConfiguredEndpoints();
            }

            if (afterApplicationInitialization != null)
            {
                afterApplicationInitialization(context);
            }
        }

        private bool IsHost()
        {
            if (ApplicationServiceType == ApplicationServiceType.ApiHost || ApplicationServiceType == ApplicationServiceType.AuthHost)
            {
                return true;
            }
            return false;
        }
    }
}
