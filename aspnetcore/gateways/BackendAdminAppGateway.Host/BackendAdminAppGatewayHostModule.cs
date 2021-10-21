﻿using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.Consul;
using Leopard.Utils;
using Leopard;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using SSO.AuthServer;
using System;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BackendAdminAppGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardConsulModule),
        typeof(LeopardAspNetCoreSwashbuckleModule),
        typeof(AuthServerHttpApiClientModule)
    )]
    public class BackendAdminAppGatewayHostModule : HostCommonModule
    {
        public BackendAdminAppGatewayHostModule() : base(ApplicationServiceType.GateWay, "BackendAdminAppGateway", false)
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
                        var app = context.GetApplicationBuilder();
                        app.UseAbpClaimsMap();
                    },
                    afterApplicationInitialization: (ctx) =>
                    {
                        var app = context.GetApplicationBuilder();
                        app.MapWhen(
                            ctx => ctx.Request.Path.ToString().StartsWith("/api/abp/") ||
                                   ctx.Request.Path.ToString().StartsWith("/Abp/") ||
                                   ctx.Request.Path.ToString().StartsWith("/api/permission-management/") ||
                                   ctx.Request.Path.ToString().StartsWith("/api/feature-management/") ||
                                   ctx.Request.Path.ToString().EndsWith("/api/health"),
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
