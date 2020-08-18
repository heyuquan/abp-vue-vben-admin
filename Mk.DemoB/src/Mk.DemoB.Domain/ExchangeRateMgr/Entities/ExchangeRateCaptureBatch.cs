using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mk.DemoB.ExchangeRateMgr.Entities
{
    public class ExchangeRateCaptureBatch : CreationAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 本次抓取批号
        /// </summary>
        public string CaptureBatchNumber { get; set; }
        /// <summary>
        /// 抓取是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
