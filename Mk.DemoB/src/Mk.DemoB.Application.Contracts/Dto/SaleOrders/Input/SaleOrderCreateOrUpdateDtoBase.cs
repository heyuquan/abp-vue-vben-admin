using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.ObjectExtending;

namespace Mk.DemoB.Dto.SaleOrders
{
    public class SaleOrderCreateOrUpdateDtoBase : ExtensibleObject
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        [Required]
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单时间
        /// </summary>
        [Required]
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        [Required]
        public string Currency { get; set; }

    }
}
