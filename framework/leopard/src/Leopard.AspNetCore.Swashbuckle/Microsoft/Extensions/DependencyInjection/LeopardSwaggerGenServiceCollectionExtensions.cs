using Leopard.AspNetCore.Swashbuckle.Options;
using Leopard.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LeopardSwaggerGenServiceCollectionExtensions
    {
        public static IServiceCollection AddLeopardSwaggerGen(
            this IServiceCollection services,
            Action<SwaggerGenOptions> setupAction = null)
        {
            var configuration = services.GetConfiguration();
            var applicationOptions = configuration.GetSection(ApplicationOptions.SectionName).Get<ApplicationOptions>();
            bool isRequiredSetAuth = !applicationOptions.Auth?.Authority?.IsNullOrWhiteSpace() ?? false;
            var swaggerOptions = configuration.GetSection(SwaggerOptions.SectionName).Get<SwaggerOptions>() ?? new SwaggerOptions();

            Action<SwaggerGenOptions> innerSetupAction = options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = applicationOptions.AppName, Version = applicationOptions.AppVersion });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                if (!swaggerOptions.IsHideAbpEndpoints)
                {
                    options.HideAbpEndpoints();
                }

                //options.OperationFilter<EnumDescriptionFilter>();

                // 为 Swagger JSON and UI设置xml文档注释路径
                var filePaths = System.IO.Directory.GetFiles(AppContext.BaseDirectory, "*.xml")
                                                   .Where(x =>
                                                              x.EndsWith(".Application.xml")
                                                           || x.EndsWith(".Application.Contracts.xml")
                                                           || x.EndsWith(".HttpApi.xml")
                                                       );
                foreach (var xmlPath in filePaths)
                {
                    options.IncludeXmlComments(xmlPath);
                }

                setupAction?.Invoke(options);
            };

            if (isRequiredSetAuth)
            {
                services = services.AddAbpSwaggerGenWithOAuth(
                                            applicationOptions.Auth.Authority,
                                            new Dictionary<string, string>(),
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
