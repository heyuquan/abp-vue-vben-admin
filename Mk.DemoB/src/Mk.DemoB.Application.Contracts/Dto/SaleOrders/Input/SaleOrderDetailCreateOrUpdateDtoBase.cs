using Mk.DemoB.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.ObjectExtending;

namespace Mk.DemoB.Dto.SaleOrders
{
    public class SaleOrderDetailCreateOrUpdateDtoBase : ExtensibleObject
    {
        /// <summary>
        /// 产品Sku编号
        /// </summary>
        [Required]
        public string ProductSkuCode { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        [Range(0.1, CommonConsts.MaxBuySkuPrice)]
        public decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Range(1, CommonConsts.MaxBuySkuQuantity)]
        public int Quantity { get; set; }
    }
}
