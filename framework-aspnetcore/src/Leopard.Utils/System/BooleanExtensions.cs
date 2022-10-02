using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// 布尔值<see cref="Boolean"/>类型的扩展辅助操作类
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        /// 把布尔值转换为 小写 字符串
        /// </summary>
        public static string ToLower(this bool value)
        {
            return value.ToString().ToLower();
        }

        /// <summary>
        /// 把布尔值转换为 大写 字符串
        /// </summary>
        public static string ToUpper(this bool value)
        {
            return value.ToString().ToUpper();
        }
    }
}
