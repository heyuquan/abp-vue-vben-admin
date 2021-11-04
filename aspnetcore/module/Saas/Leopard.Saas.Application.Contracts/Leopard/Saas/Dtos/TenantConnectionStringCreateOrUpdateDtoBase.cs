using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leopard.Saas.Dtos
{
    public class TenantConnectionStringCreateOrUpdateDtoBase
    {
        [StringLength(64)]
        [Required]
        public string Name { get; set; }
        [StringLength(256)]
        [Required]
        public string Value { get; set; }
    }

    public class TenantConnectionStringCreateDto : TenantConnectionStringCreateOrUpdateDtoBase
    {

    }

    public class TenantConnectionStringUpdateDto : TenantConnectionStringCreateOrUpdateDtoBase
    {
    }
}
