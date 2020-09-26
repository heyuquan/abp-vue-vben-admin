namespace Leopard.Consul
{
    /// <summary>
    /// 服务发现Options
    /// </summary>
    public class ServiceDiscoveryOptions
    {
        /// <summary>
        /// 注册的服务名
        /// </summary>
        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }

        public HealthCheckOptions HealthCheck { get; set; }

        /// <summary>
        /// 要注册站点地址
        /// 使用代理模式启动时，必须设置站点的地址信息 地址:端口 （使用IISExpress属于反向代理方式）
        /// 当为空时， 服务发现会从 server.Features 中获取当前站点启动的 地址:端口(does not work in reverse proxy mode)
        /// </summary>
        public string[] Endpoints { get; set; }
    }
}
