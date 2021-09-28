using Leopard.AspNetCore.Swashbuckle.Filter;
using Leopard.AuthServer;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LepardSwaggerGenServiceCollectionExtensions
    {
        public static IServiceCollection AddLepardSwaggerGen(
            this IServiceCollection services,
            Action<SwaggerGenOptions> setupAction = null)
        {
            var configuration = services.GetConfiguration();
            var authServerOptions = configuration.GetSection(AuthServerOptions.SectionName).Get<AuthServerOptions>();
            if (authServerOptions == null)
            {
                throw new Exception("配置文件中缺少SwaggerClient节点的配置");
            }

            bool isRequiredSetAuth = !authServerOptions.Authority.IsNullOrWhiteSpace();

            Action<SwaggerGenOptions> innerSetupAction = options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = authServerOptions.ApiName, Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);

                options.DocumentFilter<EnumDescriptionFilter>();

                options.OperationFilter<CollectionAbpApiFilter>();                

                // 为 Swagger JSON and UI设置xml文档注释路径
                // swagger只需要加载 *.Application.Contracts.xml 和 *.HttpApi.xml
                var filePaths = System.IO.Directory.GetFiles(AppContext.BaseDirectory, "*.xml")
                                                   .Where(x => x.EndsWith(".Application.Contracts.xml") || x.EndsWith(".HttpApi.xml"));
                foreach (var xmlPath in filePaths)
                {
                    options.IncludeXmlComments(xmlPath);
                }

                setupAction?.Invoke(options);
            };

            if (isRequiredSetAuth)
            {
                Dictionary<string, string> scopes = new Dictionary<string, string>();
                foreach (var item in authServerOptions.SwaggerClientScopes)
                {
                    scopes.Add(item, $"{item} API");
                }

                services = services.AddAbpSwaggerGenWithOAuth(
                                            authServerOptions.Authority,
                                            scopes,
                                            innerSetupAction
                                        );
            }
            else
            {
                services = services.AddAbpSwaggerGen(innerSetupAction);
            }

            return services;
        }
    }
}
