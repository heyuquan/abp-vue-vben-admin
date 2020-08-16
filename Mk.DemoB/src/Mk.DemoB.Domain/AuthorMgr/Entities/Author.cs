using Mk.DemoB.BookMgr.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mk.DemoB.AuthorMgr.Entities
{
    /// <summary>
    /// 作者
    /// </summary>
    public class Author : FullAuditedAggregateRoot<Guid>
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 作者写的书
        /// </summary>
        public ICollection<Book> Books { get; set; }
    }
}
