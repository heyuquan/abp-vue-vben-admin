using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.AuthServer
{
    // 配置案例
    //"AuthServer": {
    //  "ApiName": "BackendAdminAppGateway",
    //  "Authority": "https://localhost:44115",
    //  "RequireHttpsMetadata": "true",

    //  "SwaggerClientId": "BackendAdminAppGateway",
    //  "SwaggerClientSecret": "1q2w3e*",
    //  "SwaggerClientScopes": []
    //},

    /// <summary>
    /// swagger配置信息
    /// </summary>
    public class AuthServerOptions
    {
        public const string SectionName = "AuthServer";

        /// <summary>
        /// api名称(项目具体名称)
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// 授权中心地址
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// 是否需要HTTP元数据
        /// </summary>
        public string RequireHttpsMetadata { get; set; }
        /// <summary>
        /// SwaggerClientId
        /// </summary>
        public string SwaggerClientId { get; set; }
        /// <summary>
        /// SwaggerClientSecret
        /// </summary>
        public string SwaggerClientSecret { get; set; }
        /// <summary>
        /// SwaggerClientScopes
        /// </summary>
        public string[] SwaggerClientScopes { get; set; } 

    }
}
