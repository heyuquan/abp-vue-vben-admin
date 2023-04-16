using Leopard.AspNetCore.Middlewares;
using Leopard.AspNetCore.Mvc;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.Options;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using StackExchange.Redis;
using System;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
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
    public class LeopardHostModule : AbpModule
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

        public LeopardHostModule(
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
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            var applicationOptions = configuration.GetSection(ApplicationOptions.SectionName).Get<ApplicationOptions>();

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = IsEnableMultiTenancy;
            });

            Configure<AbpClockOptions>(options => { options.Kind = DateTimeKind.Utc; });

            #region json
            context.Services.AddControllersWithViews()
                    .AddJsonOptions(options =>
                    {
                        SetJsonSerializerOptions(options.JsonSerializerOptions);
                    });

            Configure<AbpSystemTextJsonSerializerOptions>(options =>
            {
                SetJsonSerializerOptions(options.JsonSerializerOptions);
            });

            void SetJsonSerializerOptions(JsonSerializerOptions options)
            {
                options.Converters.Add(new DatetimeJsonConverter());
                options.Converters.Add(new AbpStringToBooleanConverter());
                // 默认枚举只能接收数值，并且swagger上显示为 0，1，2……没有枚举的字符串定义
                // 设置后，在swagger上就可以将枚举定义的值都显示出来。对于接收值可以是：数值和枚举字符串，swagger默认示例是用字符串描述的
                options.Converters.Add(new JsonStringEnumConverter());
                // 默认的 System.Text.Json 序列化的时候会把所有的非 ASCII 的字符进行转义，这就会导致很多时候我们的一些非 ASCII 的字符就会变成 \\uxxxx 这样的形式，很多场景下并不太友好
                // https://www.cnblogs.com/cdaniu/p/16024229.html
                options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                //数据格式首字母小写
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                //反序列化过程中属性名称是否使用不区分大小写的比较
                options.PropertyNameCaseInsensitive = false;
                //要反序列化的 JSON 有效负载中是否允许（和忽略）对象或数组中 JSON 值的列表末尾多余的逗号
                // https://docs.microsoft.com/zh-cn/dotnet/api/system.text.json.jsonserializeroptions.allowtrailingcommas?view=net-7.0
                options.AllowTrailingCommas = true;   // 兼容 Newtonsoft.Json ，默认情况 Newtonsoft.Json 下会忽略尾随逗号
            }
            #endregion

            // Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);

            #region db
            //Configure<AbpUnitOfWorkDefaultOptions>(options =>
            //{
            //    //Standalone MongoDB servers don't support transactions
            //    options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
            //});
            #endregion

            //Configure<MvcOptions>(mvcOptions =>
            //{
            //    // 全局异常替换
            //    // https://www.cnblogs.com/twoBcoder/p/12838913.html
            //    var index = mvcOptions.Filters.ToList().FindIndex(filter => filter is ServiceFilterAttribute attr && attr.ServiceType.Equals(typeof(AbpExceptionFilter)));
            //    if (index > -1)
            //        mvcOptions.Filters.RemoveAt(index);
            //    mvcOptions.Filters.Add(typeof(LeopardExceptionFilter));
            //});
            // 全局异常统一处理
            context.Services.ConfigureModelBindingExceptionHandling();

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

            if (ApplicationServiceType == ApplicationServiceType.ApiHost)
            {
                if (applicationOptions.Auth?.Authority?.IsNullOrWhiteSpace() ?? false)
                {
                    throw new UserFriendlyException($"缺少 {AuthOptions.SectionName} 配置节点");
                }
                context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    // .addJwtBearer 只是对token的校验。 用于api，本身不获取token
                    // .AddAbpOpenIdConnect 是要从认证服务器上获取token。 用于web应用，用户在web界面上输入账户和密码
                    .AddJwtBearer(options =>
                    {
                        // ValidateIssuer，验证访问令牌中的iss声明是否与API信任的颁发者（权限）匹配（即，您的令牌服务）。验证令牌的颁发者是否符合此API的预期。
                        // ValidateAudience，验证访问令牌内的aud声明是否与访问群体参数匹配。也就是说，接收到的令牌是用于此API的。
                        options.Authority = applicationOptions.Auth.Authority;
                        options.RequireHttpsMetadata = applicationOptions.Auth.RequireHttpsMetadata;
                        options.Audience = applicationOptions.AppName;
                        //options.TokenValidationParameters.ValidateAudience = false;
                    });
            }

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

            #region 注册响应压缩

            context.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;  // 是否对https请求压缩.因为存在安全风险，默认禁用此选项。 

                // NET 6 webapi自定义响应压缩
                // https://blog.csdn.net/superQuestion/article/details/122494493
                // 使用自定义压缩，来更多的决策什么内容需要被压缩
                // 大于1kb（1024字节）的数据才压缩 ，因为压缩小文件的开销可能会产生比未压缩文件更大的压缩文件。

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
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            this.LeopardApplicationInitialization(context);
        }

        /// <summary>
        /// LeopardApplicationInitialization
        /// </summary>
        /// <param name="context"></param>
        /// <param name="betweenAuthApplicationInitialization">在认证之后，授权之前执行</param>
        protected void LeopardApplicationInitialization(
            ApplicationInitializationContext context
            , Action<ApplicationInitializationContext> betweenAuthApplicationInitialization = null)
        {
            // ASP.NET Core 中间件顺序
            // https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0#middleware-order

            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            app.UseRequestLocalization(options =>
            {
                // ABP (.NET 5.0) 设置默认语言为简体中文
                // https://www.cnblogs.com/feng-NET/p/16044457.html
                // 移除header中的中文参数，因为浏览器会默认带上语言参数。  移除后由cookie来决定语言，并设置默认语言为zh-hands
                options.RequestCultureProviders = options.RequestCultureProviders.Where(a => !(a is AcceptLanguageHeaderRequestCultureProvider)).ToList();
                options.SetDefaultCulture("zh-Hans");
            });

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
            // Serilog
            app.UseAbpSerilogEnrichers();
            app.UseAbpSecurityHeaders();

            // 虚拟文件系统
            app.UseStaticFiles();
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
            app.UseAnonymousUser();
            //#if DEBUG
            // swagger
            app.UseLeopardSwaggerUI();
            //访问 "/" and "" (whitespace) 地址，自动跳转到 /swagger 的首页
            app.UseRewriter(new RewriteOptions()
                .AddRedirect("^(|\\|\\s+)$", "/swagger"));
            //#endif

            // 审计日志
            app.UseAuditing();
            app.UseUnitOfWork();

            // 在需要缓存的组件之前。 UseCORS 必须在 UseResponseCaching 之前。
            app.UseResponseCompression();
            app.UseConfiguredEndpoints();
        }
    }
}
