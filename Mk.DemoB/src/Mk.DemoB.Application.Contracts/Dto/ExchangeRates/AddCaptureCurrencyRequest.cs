using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mk.DemoB.Dto.ExchangeRates
{
    public class AddCaptureCurrencyRequest
    {
        /// <summary>
        /// 本币币种编码
        /// </summary>
        [Required]
        public string CurrencyCodeFrom { get; set; }
        /// <summary>
        /// 兑换币种编码
        /// </summary>
        [Required]
        public string CurrencyCodeTo { get; set; }
    }
}
