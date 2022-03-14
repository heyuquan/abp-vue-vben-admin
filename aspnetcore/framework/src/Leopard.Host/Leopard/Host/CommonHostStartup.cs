using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Leopard.Host
{
    public class CommonHostStartup<T> where T : AbpModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<T>();
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
