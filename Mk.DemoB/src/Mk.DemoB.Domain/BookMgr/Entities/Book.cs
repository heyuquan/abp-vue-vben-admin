using JetBrains.Annotations;
using Mk.DemoB.AuthorMgr.Entities;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mk.DemoB.BookMgr.Entities
{
    /// <summary>
    /// 书籍
    /// </summary>
    public class Book : FullAuditedAggregateRoot<Guid>
    {

        public string Name { get; set; }


        public decimal Price { get; set; }

        public Guid AuthorId { get; set; }
        /// <summary>
        /// 书的作者
        /// </summary>
        public Author Author { get; set; }

        public Book(Guid id, [NotNull] string name, decimal price)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
            Price = price;

            ExtraProperties = new Dictionary<string, object>();
        }
    }
}
