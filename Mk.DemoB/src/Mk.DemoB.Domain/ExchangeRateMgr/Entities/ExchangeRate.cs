using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mk.DemoB.ExchangeRateMgr.Entities
{
    public class ExchangeRate : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 本币币种编码
        /// </summary>
        public string CurrencyCodeFrom { get; protected set; }
        /// <summary>
        /// 兑换币种编码
        /// </summary>
        public string CurrencyCodeTo { get; protected set; }

        /// <summary>
        /// 买入价
        /// </summary>
        public decimal BuyPrice { get; protected set; }

        /// <summary>
        /// 抓取的数据来源Url
        /// </summary>
        public string DataFromUrl { get; set; }

        /// <summary>
        /// 抓取批次
        /// </summary>
        public string CaptureBatchNumber { get; set; }

        public ExchangeRate(
            Guid id
            , [NotNull]string currencyCodeFrom
            , [NotNull]string currencyCodeTo
            , decimal buyPrice)
        {
            Id = id;
            CurrencyCodeFrom = currencyCodeFrom;
            CurrencyCodeTo = currencyCodeTo;
            BuyPrice = buyPrice;
        }
    }
}
