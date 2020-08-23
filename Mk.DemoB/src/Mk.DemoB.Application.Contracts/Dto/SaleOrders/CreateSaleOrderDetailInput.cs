using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mk.DemoB.Dto.SaleOrders
{
    public class CreateSaleOrderDetailInput
    {
        /// <summary>
        /// 产品Sku编号
        /// </summary>
        [Required]
        public string ProductSkuCode { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        [Range(0, 99999999)]
        public decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Range(1, 9999)]
        public int Quantity { get; set; }
    }
}
