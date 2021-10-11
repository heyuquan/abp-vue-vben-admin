using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Leopard.Utils
{
    public class HostCommonStartup<T> where T : AbpModule
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
