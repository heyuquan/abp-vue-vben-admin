using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoB.Dto
{
    public class BookDto : ExtensibleFullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }


        public decimal Price { get; set; }
    }
}
