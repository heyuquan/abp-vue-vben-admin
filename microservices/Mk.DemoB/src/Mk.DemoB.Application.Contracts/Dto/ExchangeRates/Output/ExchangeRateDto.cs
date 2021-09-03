using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoB.Dto.ExchangeRates
{
    public class ExchangeRateDto : EntityDto<Guid>
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
        /// 买入价
        /// </summary>
        public decimal BuyPrice { get; set; }
        /// <summary>
        /// 抓取时间
        /// </summary>
        public DateTime CaptureTime { get; set; }
    }
}
