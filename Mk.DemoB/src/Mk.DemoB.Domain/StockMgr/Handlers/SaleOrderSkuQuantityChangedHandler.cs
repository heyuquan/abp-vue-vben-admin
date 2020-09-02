using Microsoft.Extensions.Logging;
using Mk.DemoB.SaleOrderMgr.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Mk.DemoB.StockMgr.Handlers
{

    // 业务逻辑：在saleOrder中的sku 数量发生变化时，通知库存更新库存
    // 模拟库存模块和销售订单在同一个进程中。所以发的事 本地事件消息

    public class SaleOrderSkuQuantityChangedHandler
        : ILocalEventHandler<SaleOrderSkuQuantityChangedEvent>, ITransientDependency
    {
        private const string EVENT_TYPE = "本地事件";
        private readonly Logger<SaleOrderSkuQuantityChangedHandler> _logger;
        public SaleOrderSkuQuantityChangedHandler(
            Logger<SaleOrderSkuQuantityChangedHandler> logger
            )
        {
            _logger = logger;
        }

        public virtual async Task HandleEventAsync(SaleOrderSkuQuantityChangedEvent eventData)
        {
            _logger.LogInformation($"{EVENT_TYPE}：销售订单{eventData.SaleOrderId}子记录{eventData.SaleOrderDetailId},Sku[{eventData.ProductSkuCode}]数量{(eventData.ChangeQuantity > 0 ? "增加" : "减少")}{Math.Abs(eventData.ChangeQuantity)}");
        }
    }
}
