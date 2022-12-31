﻿using JetBrains.Annotations;
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
        public string CaptureBatchNumber { get; protected set; }
        /// <summary>
        /// 抓取时间
        /// </summary>
        public DateTime CaptureTime { get; protected set; }
        /// <summary>
        /// 抓取是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public ExchangeRateCaptureBatch(
            Guid id
            , [NotNull]string captureBatchNumber
            , [NotNull]DateTime captureTime
            )
        {
            Id = id;
            CaptureBatchNumber = captureBatchNumber;
            CaptureTime = captureTime;
        }
    }
}
