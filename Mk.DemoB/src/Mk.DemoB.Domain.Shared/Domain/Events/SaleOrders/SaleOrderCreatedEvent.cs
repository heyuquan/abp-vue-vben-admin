using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoB.Domain.Events.SaleOrders
{
    // 业务：创建成功销售订单，则发布分布式事件通知用户系统发送邮件做通知
    public class SaleOrderCreatedEvent
    {
        public string CustomerName { get; set; }
    }
}
