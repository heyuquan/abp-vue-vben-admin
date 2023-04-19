using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.EntityFrameworkCore.Logger
{
    /// <summary>
    /// EF日志选项参数
    /// </summary>
    public class EFLogOptions
    {
        public const string SectionName = "EFCore:EFLog";
        /// <summary>
        /// 是否启用日志，默认false
        /// </summary>
        public bool IsEnableLog { get; set; } = false;

        /// <summary>
        /// 输出sql执行时间超过**ms的语句.默认200ms
        /// </summary>
        public int ExecuteTimeSpent { get; set; } = 200;
        /// <summary>
        /// 是否显示sql中的参数值，默认false
        /// </summary>
        public bool EnableSensitiveData { get; set; } = false;

    }
}
