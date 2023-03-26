using Leopard.AuthServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class LeopardSwaggerUIBuilderExtensions
    {
        public static IApplicationBuilder UseLeopardSwaggerUI(
            this IApplicationBuilder app,
            Action<SwaggerUIOptions> setupAction = null)
        {
            app.UseSwagger();

            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var authServerOptions = configuration.GetSection(AuthServerOptions.SectionName).Get<AuthServerOptions>();
            if (authServerOptions != null)
            {
                app.UseAbpSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", authServerOptions.ApiName);

                    options.OAuthClientId(authServerOptions.SwaggerClientId);
                    options.OAuthClientSecret(authServerOptions.SwaggerClientSecret);
                    options.OAuthScopes(authServerOptions.SwaggerClientScopes);

                    setupAction?.Invoke(options);
                });
            }
            return app;
        }
    }
}
