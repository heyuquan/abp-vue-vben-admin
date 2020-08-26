using Mk.DemoB.Enums.SaleOrder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mk.DemoB.Dto.SaleOrders
{
    public class CreateSaleOrderRequest
    {
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime? OrderTime { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        [Required]
        public string Currency { get; set; }
        [Required]
        public List<CreateSaleOrderDetailInput> SaleOrderDetails { get; set; }
    }
}
