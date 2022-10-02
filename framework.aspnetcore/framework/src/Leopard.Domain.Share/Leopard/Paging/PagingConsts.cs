using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Paging
{
    /// <summary>
    /// 分页相关默认值设定
    /// </summary>
    public class PagingConsts
    {
        /// <summary>
        /// 返回结果数量通用约定
        /// </summary>
        public static class ResultCount
        {
            /// <summary>
            /// 常规
            /// </summary>
            public const int Normal= 200;

            public const int Count500 = 500;

            public const int Count1000 = 1000;

            public const int Count2000 = 2000;

            /// <summary>
            /// 最大 int.MaxValue
            /// </summary>
            public const int Max = int.MaxValue;
        }
    }
}
