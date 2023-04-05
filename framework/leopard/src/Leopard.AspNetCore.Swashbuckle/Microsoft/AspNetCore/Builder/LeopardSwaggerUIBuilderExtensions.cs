using Leopard.AspNetCore.Swashbuckle.Options;
using Leopard.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

            var swaggerOptions = app.ApplicationServices.GetRequiredService<IOptions<SwaggerOptions>>().Value;
            var applicationOptions = app.ApplicationServices.GetRequiredService<IOptions<ApplicationOptions>>().Value;

            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", applicationOptions.AppName);

                if (!swaggerOptions.ClientId.IsNullOrWhiteSpace())
                    options.OAuthClientId(swaggerOptions.ClientId);
                if (!swaggerOptions.ClientSecret.IsNullOrWhiteSpace())
                    options.OAuthClientSecret(swaggerOptions.ClientSecret);

                setupAction?.Invoke(options);
            });

            return app;
        }
    }
}
