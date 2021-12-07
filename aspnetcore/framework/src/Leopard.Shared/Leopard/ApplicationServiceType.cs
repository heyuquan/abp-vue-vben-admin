using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// 认证服务
        /// eg：AuthServer.HttpApi.Host ， AuthServer.IdentityServer
        /// </summary>
        [Description("认证服务")]
        AuthHost = 4,

        /// <summary>
        /// 认证服务
        /// eg：AuthServer.HttpApi.Host ， AuthServer.IdentityServer
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
