namespace Leopard.Consul
{
    /// <summary>
    /// 服务检查
    /// </summary>
    public class HealthCheckOptions
    {
        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string HealthCheckTemplate { get; set; }
        /// <summary>
        /// 间隔
        /// 默认10秒一次
        /// </summary>
        public int Interval { get; set; } = 10;
    }
}
