using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SSO.AuthServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AuthServerIdentityServerModule>();
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health");
            });
        }
    }
}
