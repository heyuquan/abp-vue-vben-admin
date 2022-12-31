using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoB.Dto.SaleOrders
{
    [Serializable]
    public class SaleOrderDetailDto : ExtensibleCreationAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 父表ID
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo { get; set; }
        /// <summary>
        /// 产品Sku编号
        /// </summary>
        public string ProductSkuCode { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
