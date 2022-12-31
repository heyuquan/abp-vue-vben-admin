﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EventBus;

namespace Mk.DemoB.Domain.Events.SaleOrders
{
    /// <summary>
    /// 销售订单的sku和数量变化事件
    /// </summary>
    [EventName("Mk.DemoB.SaleOrderSkuQuantity.Changed")]
    public class SaleOrderSkuQuantityChangedEvent
    {
        public Guid SaleOrderId { get; set; }
        public Guid SaleOrderDetailId { get; set; }
        public string ProductSkuCode { get; set; }
        /// <summary>
        /// 变动的数量，减少为负数，增加为正数
        /// </summary>
        public int ChangeQuantity { get; set; }

    }
}
