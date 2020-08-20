using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoB.Dto.ExchangeRates
{
    public class GetRateRequest: IPagedResultRequest
    {
        /// <summary>
        /// 本币币种编码
        /// </summary>
        public string CurrencyCodeFrom { get; set; }
        /// <summary>
        /// 兑换币种编码
        /// </summary>
        public string CurrencyCodeTo { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        [Required]
        public int SkipCount { get; set; }
        [Required]
        public int MaxResultCount { get; set; }
    }
}
