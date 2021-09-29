using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle.Filter;
using Leopard.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Mk.DemoB;
using Mk.DemoC;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using SSO.AuthServer;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.AuthServer;

namespace BackendAdminAppGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardConsulModule),
        typeof(LeopardAspNetCoreSwashbuckleModule),
        typeof(AuthServerHttpApiClientModule),
        typeof(DemoBHttpApiClientModule),
        typeof(DemoBApplicationModule),
        typeof(DemoCHttpApiClientModule),
        typeof(DemoCApplicationModule)
    )]
    public class BackendAdminAppGatewayHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            var a = configuration.GetSection("AuthServer");
            AuthServerOptions jiguangConfig = configuration.GetSection("AuthServer").Get<AuthServerOptions>();

            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.ApiName = configuration["AuthServer:SwaggerClientId"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                });

            context.Services.AddLepardSwaggerGen();

            context.Services.AddOcelot(context.Services.GetConfiguration());
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAbpClaimsMap();

            app.UseSwagger();
            app.UseLepardSwaggerUI();

            app.MapWhen(
                ctx => ctx.Request.Path.ToString().StartsWith("/api/abp/") ||
                       ctx.Request.Path.ToString().StartsWith("/Abp/") ||
                       ctx.Request.Path.ToString().StartsWith("/api/permission-management/")||
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
    }
}
