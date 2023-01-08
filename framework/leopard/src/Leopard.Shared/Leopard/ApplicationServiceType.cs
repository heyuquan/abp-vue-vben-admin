using System.ComponentModel;

namespace Leopard
{
    /// <summary>
    /// 应用服务类型
    /// </summary>
    public enum ApplicationServiceType : int
    {
        /// <summary>
        /// Api服务
        /// </summary>
        [Description("Api服务")]
        ApiHost = 0,

        /// <summary>
        /// 后台工作者  就不需要AuthServer相关配置
        /// </summary>
        [Description("后台工作者")]
        Worker =4,

        /// <summary>
        /// 认证服务  eg:获取token
        /// </summary>
        [Description("认证服务")]
        AuthIdentityServer = 8,

        /// <summary>
        /// 网关服务
        /// </summary>
        [Description("网关服务")]
        GateWay = 16,
    }
}
