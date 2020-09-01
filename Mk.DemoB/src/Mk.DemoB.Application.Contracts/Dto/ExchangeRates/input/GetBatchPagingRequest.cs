using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoB.Dto.ExchangeRates
{
    public class GetBatchPagingRequest : IPagedResultRequest
    {
        [Required]
        public string CaptureBatchNumber { get; set; }

        [Required]
        public int SkipCount { get; set; }
        [Required]
        public int MaxResultCount { get; set; }
    }
}
