using Leopard.AspNetCore.Middlewares;
using Leopard.AspNetCore.Mvc;
using Leopard.AspNetCore.Mvc.Filters;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Encryption;
using Volo.Abp.Timing;

namespace Leopard.Host
{

    [DependsOn(
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpDistributedLockingModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardAspNetCoreSwashbuckleModule),
        typeof(LeopardAspNetCoreMvcModule)
    )]
    public class CommonHostModule : AbpModule
    {
        /// <summary>
        /// 模块名（模块key）eg：Leopard.Saas
        /// </summary>
        protected string ModuleKey { get; private set; }
        /// <summary>
        /// 是否启用多租户
        /// </summary>
        protected bool IsEnableMultiTenancy { get; private set; }

        protected ApplicationServiceType ApplicationServiceType { get; private set; }

        public CommonHostModule(
            ApplicationServiceType serviceType
            , string moduleKey
            , bool isEnableMultiTenancy) : base()
        {
            ModuleKey = moduleKey;
            IsEnableMultiTenancy = isEnableMultiTenancy;
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

            Configure<AbpClockOptions>(options => { options.Kind = DateTimeKind.Utc; });

            // 中文序列化的编码问题   ？？ 确认 使用newtonsoft是不是就没必要配置？？ [todo]
            Configure<AbpSystemTextJsonSerializerOptions>(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = IsEnableMultiTenancy;
            });

            // Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);

            #region db
            //Configure<AbpUnitOfWorkDefaultOptions>(options =>
            //{
            //    //Standalone MongoDB servers don't support transactions
            //    options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
            //});
            #endregion

            Configure<MvcOptions>(mvcOptions =>
            {
                // 全局异常替换
                // https://www.cnblogs.com/twoBcoder/p/12838913.html
                var index = mvcOptions.Filters.ToList().FindIndex(filter => filter is ServiceFilterAttribute attr && attr.ServiceType.Equals(typeof(AbpExceptionFilter)));
                if (index > -1)
                    mvcOptions.Filters.RemoveAt(index);
                mvcOptions.Filters.Add(typeof(LeopardExceptionFilter));
            });

            context.Services.AddHealthChecks();

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
                        // https://www.cnblogs.com/May-day/p/13965087.html
                        .AllowCredentials()
                        // 预检请求的性能
                        // https://jishuin.proginn.com/p/763bfbd36f2f
                        .SetPreflightMaxAge(CacheTimeSpan.DayOther.TwentyFourHour);
                });
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });

            #region 请求长度限制
            // ASP.NET中maxRequestLength和maxAllowedContentLength的区别
            // https://blog.csdn.net/qq_23663693/article/details/89920039

            // 使用 POST 向 ASP.NET Core 传递数据时的长度限制与解决方案
            // https://mp.weixin.qq.com/s/xnz1wnZqvvuZJ4K56BfPQg

            // .Net Core 3.1 解决数据大小限制
            // http://t.zoukankan.com/qtiger-p-13886356.html

            // 对于特殊长度限制的方法，使用特性做标注
            // 使用[DisableRequestSizeLimit]或者[RequestSizeLimit]特性在action上做限制

            // 另外：IIS 中 Post 默认长度限制为 4M 。  Nginx中 Post 默认长度限制为2M。
            // https://www.freesion.com/article/55561320633/
            Configure<FormOptions>(options =>
            {
                // 默认情况下，ASP.NET Core 限制了每个 POST 数据值的长度为 4 MB 大小，超过就会抛出 InvalidDataException 异常。
                // 请求的单个字段
                options.ValueLengthLimit = Constants.RequestLimit.MaxValueLength_Byte;

                // 请求的整个正文长度
                options.MultipartBodyLengthLimit = Constants.RequestLimit.MaxBodyLength_Byte;
            });

            Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = Constants.RequestLimit.MaxBodyLength_Byte;
            });

            #endregion

#if !Production  
            context.Services.AddLeopardSwaggerGen();
            IdentityModelEventSource.ShowPII = true;
#endif

            if (ApplicationServiceType == ApplicationServiceType.ApiHost
                 || ApplicationServiceType == ApplicationServiceType.GateWay
                )
            {
                context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        // ValidateIssuer，验证访问令牌中的iss声明是否与API信任的颁发者（权限）匹配（即，您的令牌服务）。验证令牌的颁发者是否符合此API的预期。
                        // ValidateAudience，验证访问令牌内的aud声明是否与访问群体参数匹配。也就是说，接收到的令牌是用于此API的。
                        options.Authority = configuration["AuthServer:Authority"];
                        options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                        options.Audience = configuration["AuthServer:ApiName"];
                        //options.TokenValidationParameters.ValidateAudience = false;
                    });
            }

            // 全局异常统一处理
            context.Services.ConfigureModelBindingExceptionHandling();

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

            var redisConfiguration = configuration.GetSection("Redis");
            if (redisConfiguration.Exists())
            {
                Configure<AbpDistributedCacheOptions>(options =>
                {
                    options.KeyPrefix = $"{ModuleKey}:";
                    // 滑动过期30天
                    options.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromDays(30);
                    // 绝对过期60天
                    options.GlobalCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
                });

                var connection = ConnectionMultiplexer.Connect(redisConfiguration["Configuration"]);
                var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName(ModuleKey);
                if (!hostingEnvironment.IsDevelopment())
                {
                    dataProtectionBuilder.PersistKeysToStackExchangeRedis(connection, $"{ModuleKey}-Protection-Keys");
                }

                context.Services.AddSingleton<IDistributedLockProvider>(sp =>
                {
                    return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
                });
            }

            Configure<AbpClockOptions>(options =>
            {
                options.Kind = DateTimeKind.Utc;
            });

            #region 注册响应压缩

            context.Services.AddResponseCompression(options =>
             {
                 options.EnableForHttps = true;  // 是否对https请求压缩.因为存在安全风险，默认禁用此选项。 

                 // NET 6 webapi自定义响应压缩
                 // https://blog.csdn.net/superQuestion/article/details/122494493
                 // 使用自定义压缩，来更多的决策什么内容需要被压缩
                 // 不要压缩小于 150 - 1000 字节的文件，具体取决于文件的内容和压缩效率。 压缩小文件的开销可能会产生比未压缩文件更大的压缩文件。

                 // 当客户端可以处理压缩内容时，客户端必须通过随请求发送 Accept-Encoding 标头来通知服务器其功能。 
                 // 当服务器发送压缩的内容时，它必须在 Content-Encoding 标头中包含有关如何对压缩响应进行编码的信息。
                 // Gzip 有更好的 user-agent 兼容性，而 Brotli 有更好的性能。
                 options.Providers.Add<BrotliCompressionProvider>();    // brotli
                 options.Providers.Add<GzipCompressionProvider>();      // gzip

                 // 压缩的默认 MIME 类型集：
                 // application / javascript
                 // application / json
                 // application / xml
                 // text / css
                 // text / html
                 // text / json
                 // text / plain
                 // text / xml
                 // 使用Concat添加额外的压缩类型
                 options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                  {
                        "image/svg+xml"
                 });
             });

            Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
            Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            #endregion

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
            // ASP.NET Core 中间件顺序
            // https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0#middleware-order

            var app = context.GetApplicationBuilder();

            // 本地化
            app.UseAbpRequestLocalization(options =>
            {
                // 设置ABP默认使用中文
                // https://www.cnblogs.com/waku/p/11433242.html
                options.RequestCultureProviders.RemoveAll(provider => provider is AcceptLanguageHeaderRequestCultureProvider);
            });

            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                // https://github.com/SpringLeee/ViewConfig
                app.UseViewConfig(x => x.Map().RenderJson());

                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseErrorPage();
                // useErrorPage() 是 volo.abp 定制的页面，需要注册abp 的主题，暂时没去看这些东西，先用aspnetcore 默认的页面
                app.UseDeveloperExceptionPage();

                // app.UseHsts();  服务端目前启用证书有问题，去掉https相关
            }

            //app.UseHttpsRedirection();

            // http调用链
            app.UseCorrelationId();
            // 虚拟文件系统
            app.UseStaticFiles();
            app.UseAbpSecurityHeaders();
            //路由
            app.UseRouting();
            app.UseCors();
            // 设置了UseEndpoints openiddict 和identityserver会报错  （ids4再abpv4.0版本不会报错）
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHealthChecks("/api/health");
            //});

            // 认证
            app.UseAuthentication(); 

            if (env.IsDevelopment())
            {
                app.UseTestMiddleware();
            }

            if (ApplicationServiceType == ApplicationServiceType.ApiHost
                 || ApplicationServiceType == ApplicationServiceType.GateWay
                )
            {
                app.UseAbpClaimsMap();
            }

            if (IsEnableMultiTenancy)
            {
                app.UseMultiTenancy();   // 必须在 UseIdentityServer 之前，在做ids4之前从 UseMultiTenancy 获取到tenant信息
            }
            if (betweenAuthApplicationInitialization != null)
            {
                betweenAuthApplicationInitialization(context);
            }

            app.UseAuthorization();

            //#if DEBUG
            // swagger
            app.UseSwagger();
            app.UseLeopardSwaggerUI();
            //#endif

            // Serilog
            app.UseAbpSerilogEnrichers();

            // 审计日志
            app.UseAuditing();
            app.UseUnitOfWork();

            // 在需要缓存的组件之前。 UseCORS 必须在 UseResponseCaching 之前。
            app.UseResponseCompression();
            app.UseConfiguredEndpoints();
            app.UseConfiguredEndpoints();

            if (afterApplicationInitialization != null)
            {
                afterApplicationInitialization(context);
            }

            // 设置了UseEndpoints openiddict 和identityserver会报错  （ids4再abpv4.0版本不会报错）
            //// UseEndpoints 在 UseRouting 之后
            //// 放到 afterApplicationInitialization 之后，是因为gateway会在 after 中做一些转发处理。否则网关的ocelot会无法工作
            // 设置默认swagger index页面
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute("default",
            //              "{controller=Swagger}/{action=Index}");
            //});
        }
    }
}
