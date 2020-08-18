using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mk.DemoB.ExchangeRateMgr.Entities
{
    public class CaptureCurrency : CreationAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 本币币种编码
        /// </summary>
        public string CurrencyCodeFrom { get; set; }
        /// <summary>
        /// 兑换币种编码
        /// </summary>
        public string CurrencyCodeTo { get; set; }
    }
}
