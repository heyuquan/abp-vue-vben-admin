using Mk.DemoB.Dto.SaleOrders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoB.Dto.ExtraSaleOrder
{
    public class CreateSaleOrderRequestEx : CreateSaleOrderRequest
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
    }
}
