using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Mk.DemoB.Dto.SaleOrders
{
    public class SaleOrderUpdateDto : SaleOrderCreateOrUpdateDtoBase, IHasConcurrencyStamp
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string ConcurrencyStamp { get; set; }

        [Required]
        public List<SaleOrderDetailUpdateDto> SaleOrderDetails { get; set; }
    }
}
