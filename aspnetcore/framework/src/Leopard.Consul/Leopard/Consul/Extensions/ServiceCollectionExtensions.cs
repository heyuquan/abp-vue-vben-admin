using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp;

namespace Leopard.Consul.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加服务注册中心
        /// </summary>
        public static IServiceCollection AddServiceDiscovery(this IServiceCollection services)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            Check.NotNull(configuration, nameof(configuration));

            var serviceDisvoverySection = configuration.GetSection("ServiceDiscovery");
            bool isEnable = serviceDisvoverySection.GetValue<bool>("IsEnable");
            if (isEnable)
            {
                services.Configure<ServiceDiscoveryOptions>(serviceDisvoverySection);

                services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
                {
                    var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;

                    if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                    {
                        cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                    }
                }));
            }

            return services;
        }
    }
}
