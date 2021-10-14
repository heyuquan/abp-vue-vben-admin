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
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Encryption;
using Volo.Abp.Timing;

namespace Leopard.Utils.Host.Leopard.Utils
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
        /// 项目组合Key {ProjectKey}.{ModuleKey}
        /// </summary>
        protected readonly string ProjectCombinationKey;


        public HostCommonModule(string moduleKey, bool isEnableMultiTenancy) 
            : this("Leopard", moduleKey, isEnableMultiTenancy)
        {
            //todo： moduleKey 从这里获取 configuration["AuthServer:SwaggerClientId"]
        }

        public HostCommonModule(string projectKey, string moduleKey, bool isEnableMultiTenancy) : base()
        {
            ProjectKey = projectKey;
            ModuleKey = moduleKey;
            IsEnableMultiTenancy = isEnableMultiTenancy;
            ProjectCombinationKey = $"{ProjectKey}.{ModuleKey}";
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = IsEnableMultiTenancy;
            });

            // 设置分页默认返回20条数据   
            LimitedResultRequestDto.DefaultMaxResultCount = 20;

            // 中文序列化的编码问题
            Configure<AbpSystemTextJsonSerializerOptions>(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
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

            context.Services.AddLeopardSwaggerGen();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });

            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = ModuleKey;
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            // http调用链
            app.UseCorrelationId();
            // 虚拟文件系统
            app.UseStaticFiles();
            //路由
            app.UseRouting();
            app.UseCors();
            // 认证
            app.UseAuthentication();
            if (IsEnableMultiTenancy)
            {
                app.UseMultiTenancy();
            }
            // 本地化
            app.UseAbpRequestLocalization();
            // 授权
            app.UseAuthorization();
            // swagger
            app.UseSwagger();
            app.UseLeopardSwaggerUI();
            // 审计日志
            app.UseAuditing();
            // Serilog
            app.UseAbpSerilogEnrichers();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();
        }
    }
}
