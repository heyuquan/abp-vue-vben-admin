using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Domain.Consts
{
    /// <summary>
    /// 分页相关默认值设定
    /// </summary>
    public class PagingConsts
    {
        public static class ResultCount
        {
            public const int Normal = 200;

            public const int More = 1000;

            public const int Max = int.MaxValue;
        }
    }
}
