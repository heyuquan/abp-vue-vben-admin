using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Paging
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public class PageData<T>
    {
        public long TotalCount { get; set; }

        public List<T> Items { get; set; }
    }
}
