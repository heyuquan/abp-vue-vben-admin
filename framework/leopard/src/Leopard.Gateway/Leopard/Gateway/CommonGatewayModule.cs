using Leopard.AspNetCore.Middlewares;
using Leopard.AspNetCore.Mvc;
using Leopard.AspNetCore.Mvc.Filters;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System;
using System.Linq;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace Leopard.Gateway
{

    [DependsOn(
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardAspNetCoreSwashbuckleModule),
        typeof(LeopardAspNetCoreMvcModule)
    )]
    public class CommonGatewayModule : AbpModule
    {
        /// <summary>
        /// 模块名（模块key）eg：Leopard.Saas
        /// </summary>
        protected string ModuleKey { get; private set; }

        protected ApplicationServiceType ApplicationServiceType { get; private set; }

        public CommonGatewayModule(
            ApplicationServiceType serviceType
            , string moduleKey)
        {
            ModuleKey = moduleKey;
            ApplicationServiceType = serviceType;
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();
            var applicationOptions = configuration.GetSection(ApplicationOptions.SectionName).Get<ApplicationOptions>();

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
#endif

            IdentityModelEventSource.ShowPII = applicationOptions.IsIdentityModelShowPII;

            if (applicationOptions.Auth?.Authority?.IsNullOrWhiteSpace() ?? false)
            {
                throw new UserFriendlyException($"缺少 {AuthOptions.SectionName} 配置节点");
            }
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // ValidateIssuer，验证访问令牌中的iss声明是否与API信任的颁发者（权限）匹配（即，您的令牌服务）。验证令牌的颁发者是否符合此API的预期。
                    // ValidateAudience，验证访问令牌内的aud声明是否与访问群体参数匹配。也就是说，接收到的令牌是用于此API的。
                    options.Authority = applicationOptions.Auth.Authority;
                    options.RequireHttpsMetadata = applicationOptions.Auth.RequireHttpsMetadata;
                    options.Audience = applicationOptions.AppName;
                    //options.TokenValidationParameters.ValidateAudience = false;
                });

            context.Services.AddReverseProxy()
               .LoadFromConfig(configuration.GetSection("ReverseProxy"));
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
            app.UseCors();
            // 认证
            //app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseTestMiddleware();
            }

            //app.UseAuthorization();

            app.UseAnonymousUser();
            //#if DEBUG
            // swagger
            app.UseLeopardSwaggerUI();
            //访问 "/" and "" (whitespace) 地址，自动跳转到 /swagger 的首页
            app.UseRewriter(new RewriteOptions()
                .AddRedirect("^(|\\|\\s+)$", "/swagger"));
            //#endif

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapReverseProxy();
            });
        }
    }
}

