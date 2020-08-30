using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mk.DemoB.Dto.SaleOrders
{
    public class SaleOrderCreateDto : SaleOrderCreateOrUpdateDtoBase
    {
        [Required]
        public List<SaleOrderDetailCreateDto> SaleOrderDetails { get; set; }
    }
}
