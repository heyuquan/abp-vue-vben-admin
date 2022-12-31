using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EventBus.Distributed;
using Mk.DemoB.Domain.Events.SaleOrders;
using Volo.Abp.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Mk.DemoC.UserMgr.EnventHandlers
{
    public class SaleOrderCreatedSendEmailHandler
        : IDistributedEventHandler<SaleOrderCreatedEvent>, ITransientDependency
    {
        private readonly ILogger<SaleOrderCreatedSendEmailHandler> _logger;
        public SaleOrderCreatedSendEmailHandler(
            ILogger<SaleOrderCreatedSendEmailHandler> logger
            )
        {
            _logger = logger;
        }

        public async Task HandleEventAsync(SaleOrderCreatedEvent eventData)
        {
            _logger.LogInformation($"尊敬的用户：{eventData.CustomerName},您的销售订单已经创建成功");

            await Task.CompletedTask;
        }
    }
}
