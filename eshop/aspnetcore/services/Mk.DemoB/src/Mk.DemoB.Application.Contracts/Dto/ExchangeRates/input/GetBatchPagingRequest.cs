using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoB.Dto.ExchangeRates
{
    public class GetBatchPagingRequest : PagedAndSortedResultRequestDto
    {
        [Required]
        public string CaptureBatchNumber { get; set; }
    }
}
