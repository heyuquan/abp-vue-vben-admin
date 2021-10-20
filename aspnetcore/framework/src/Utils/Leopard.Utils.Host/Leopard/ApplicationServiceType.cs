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
        [Description("Api服务")]
        ApiHost = 0,

        [Description("认证服务")]
        AuthHost = 4,

        [Description("网关服务")]
        GateWay = 8,
    }
}
