using Leopard;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.Consul;
using Leopard.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace InternalGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardConsulModule),
        typeof(LeopardAspNetCoreSwashbuckleModule)
        )]
    public class InternalGatewayHostModule : HostCommonModule
    {
        public InternalGatewayHostModule() : base(ApplicationServiceType.GateWay, "InternalGateway", false)
        { }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            this.LeopardConfigureServices(context,
                 otherConfigureServices: (ctx) =>
                 {
                     var configuration = ctx.Services.GetConfiguration();

                     ctx.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                         .AddIdentityServerAuthentication(options =>
                         {
                             options.Authority = configuration["AuthServer:Authority"];
                             options.ApiName = configuration["AuthServer:SwaggerClientId"];
                             options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                         });
                 },
                 afterConfigureServices: (ctx) =>
                 {
                     ctx.Services.AddOcelot(ctx.Services.GetConfiguration());
                 }
             );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            this.LeopardApplicationInitialization(context,
                    betweenAuthApplicationInitialization: (ctx) =>
                    {
                        var app = ctx.GetApplicationBuilder();
                        app.UseAbpClaimsMap();
                    },
                    afterApplicationInitialization: (ctx) =>
                    {
                        var app = ctx.GetApplicationBuilder();
                        app.MapWhen(
                            ctx =>
                                ctx.Request.Path.ToString().StartsWith("/api/abp/", StringComparison.OrdinalIgnoreCase) ||
                                ctx.Request.Path.ToString().StartsWith("/abp/", StringComparison.OrdinalIgnoreCase) ||
                                ctx.Request.Path.ToString().EndsWith("/api/health", StringComparison.OrdinalIgnoreCase) ||
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
