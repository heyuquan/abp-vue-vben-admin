using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle.Filter;
using Leopard.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace PublicWebSiteGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardConsulModule)
        )]
    public class PublicWebSiteGatewayHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.ApiName = configuration["AuthServer:SwaggerClientId"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                });

            context.Services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"],
                new Dictionary<string, string>
                {
                    {"PublicWebSiteGateway", "PublicWebSiteGateway API"}
                },
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PublicWebSiteGateway API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    //options.SchemaFilter<EnumSchemaFilter>();
                    options.CustomSchemaIds(type => type.FullName);
                    // 为 Swagger JSON and UI设置xml文档注释路径
                    //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Mk.DemoB.Application.xml"), true);
                    //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Mk.DemoB.Application.Contracts.xml"), true);

                    options.OperationFilter<SwaggerTagsFilter>();
                });

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
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PublicWebSite Gateway API");

                var configuration = context.GetConfiguration();
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
                options.OAuthScopes("PublicWebSiteGateway");
            });

            app.MapWhen(
                ctx => ctx.Request.Path.ToString().StartsWith("/api/abp/") ||
                       ctx.Request.Path.ToString().StartsWith("/Abp/") ||
                       ctx.Request.Path.ToString().StartsWith("/api/publicWebSiteGateway/"),
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
