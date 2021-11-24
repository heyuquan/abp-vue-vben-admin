using Leopard.AspNetCore.Mvc;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using Volo.Abp;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace Leopard.Utils
{

    [DependsOn(
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardAspNetCoreSwashbuckleModule),
        typeof(LeopardAspNetCoreMvcModule)
    )]
    public class GatewayCommonModule : HostCommonModule
    {
        public GatewayCommonModule(
            ApplicationServiceType serviceType
            , string moduleKey
            , bool isEnableMultiTenancy) : base(serviceType, moduleKey, isEnableMultiTenancy)
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            this.LeopardConfigureServices(
                context,
                afterConfigureServices: (ctx) =>
                {
                    ctx.Services.AddOcelot(ctx.Services.GetConfiguration());
                }
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            this.LeopardApplicationInitialization(
                    context,
                    afterApplicationInitialization: (ctx) =>
                    {
                        var app = ctx.GetApplicationBuilder();
                        app.MapWhen(
                            ctx => ctx.Request.Path.ToString().EndsWith("/api/health", StringComparison.OrdinalIgnoreCase) ||
                                   ctx.Request.Path.ToString().EndsWith("/swagger/index", StringComparison.OrdinalIgnoreCase) ||
                                   // ocelot configuration api
                                   ctx.Request.Path.ToString().EndsWith("/configuration", StringComparison.OrdinalIgnoreCase) ||
                                   ctx.Request.Path.ToString().TrimEnd('/').Equals(""),
                            app2 =>
                            {
                                app2.UseRouting();
                                app2.UseConfiguredEndpoints();
                            }
                         );

                        app.UseOcelot().Wait();
                    }
                );
        }
    }
}
