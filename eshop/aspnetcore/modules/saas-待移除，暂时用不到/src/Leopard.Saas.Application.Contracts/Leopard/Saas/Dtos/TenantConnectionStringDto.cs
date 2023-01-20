using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Saas.Dtos
{
    public class TenantConnectionStringDto: TenantConnectionStringCreateOrUpdateDtoBase
    {
        public Guid TenantId { get; set; }
    }
}
