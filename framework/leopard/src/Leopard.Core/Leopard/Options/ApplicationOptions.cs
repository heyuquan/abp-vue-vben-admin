using System.ComponentModel.DataAnnotations;

namespace Leopard.Options
{
    /// <summary>
    /// 应用配置信息
    /// </summary>
    public class ApplicationOptions
    {
        public const string SectionName = "Application";

        /// <summary>
        /// 应用名
        /// </summary>
        [Required]
        public string AppName { get; set; }

        public string AppVersion { get; set; } = "v1";

        public AuthOptions Auth { get; set; }

        public bool IsIdentityModelShowPII { get; set; }
    }

    /// <summary>
    /// 认证选项
    /// </summary>
    public class AuthOptions
    {
        public const string SectionName = "Application:Auth";

        /// <summary>
        /// 授权中心地址
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// 是否需要HTTP元数据
        /// </summary>
        public bool RequireHttpsMetadata { get; set; } 
    }
}
