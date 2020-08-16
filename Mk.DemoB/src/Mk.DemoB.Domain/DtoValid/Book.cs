using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mk.DemoB.DtoValid
{
    public class Book: FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }


        public decimal Price { get; set; }

        public Book(Guid id, [NotNull] string name, decimal price)
        {
            Check.NotNull(name, nameof(name));

            Id=id;
            Name = name;
            Price = price;

            ExtraProperties = new Dictionary<string, object>();
        }
    }
}
