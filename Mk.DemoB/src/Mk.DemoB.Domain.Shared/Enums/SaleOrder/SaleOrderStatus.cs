using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoB.Enums.SaleOrder
{
    /// <summary>
    /// 销售订单状态
    /// </summary>
    public enum SaleOrderStatus
    {
        /// <summary>
        /// 未支付
        /// </summary>
        UnPay = 1,
        /// <summary>
        /// 已完成
        /// </summary>
        Complete = 2,
        /// <summary>
        /// 订单取消
        /// </summary>
        Cancel = 3
    }
}
