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
        /// <summary>
        /// 是否启用日志
        /// </summary>
        public bool IsEnableLog { get; set; }

        /// <summary>
        /// 输出sql执行时间超过**ms的语句
        /// </summary>
        public int ExecuteTimeSpent { get; set; }
        /// <summary>
        /// 是否显示sql中的参数值
        /// </summary>
        public bool EnableSensitiveData { get; set; }

    }
}
