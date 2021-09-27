using Leopard.AuthServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class LepardSwaggerUIBuilderExtensions
    {
        public static IApplicationBuilder UseLepardSwaggerUI(
            this IApplicationBuilder app,
            Action<SwaggerUIOptions> setupAction = null)
        {
            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var authServerOptions = configuration.GetSection(AuthServerOptions.SectionName).Get<AuthServerOptions>();
            if (authServerOptions == null)
            {
                throw new Exception("配置文件中缺少SwaggerClient节点的配置");
            }

            return app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", authServerOptions.ApiName);

                options.OAuthClientId(authServerOptions.SwaggerClientId);
                options.OAuthClientSecret(authServerOptions.SwaggerClientSecret);
                options.OAuthScopes(authServerOptions.SwaggerClientScopes);

                setupAction?.Invoke(options);
            });
        }
    }
}
