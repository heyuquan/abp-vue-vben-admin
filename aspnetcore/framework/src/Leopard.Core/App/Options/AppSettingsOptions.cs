using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Options
{
    /// <summary>
    /// 应用全局配置
    /// </summary>
    public sealed class AppSettingsOptions
    {
        public const string SectionName = "AppSettings";
        /// <summary>
        /// 启用 MiniProfiler 组件
        /// </summary>
        public bool? EnableMiniProfiler { get; set; }

        /// <summary>
        /// 是否启用规范化文档
        /// </summary>
        public bool? EnableSpecificationDocument { get; set; }
    }
}
