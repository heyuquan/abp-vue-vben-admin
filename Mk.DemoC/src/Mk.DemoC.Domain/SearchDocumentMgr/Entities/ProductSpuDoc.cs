using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mk.DemoC.SearchDocumentMgr.Entities
{
    // 搜索如果以sku为准，可能存在一个spu下的多个sku都被展现出来。所以应当以spu为基准
    // 搜索以spu为基准，要处理的问题：
    //      #、如果有价格范围，那么展现为其下sku（访问url时带上特定sku的code参数）和图片。没有价格范围则展现主图
    /// <summary>
    /// 产品Spu的索引文档
    /// </summary>
    public class ProductSpuDoc : CreationAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Spu编码
        /// </summary>
        public string SpuCode { get; set; }
        /// <summary>
        /// Spu下所有Sku拼接，以空格隔开
        /// </summary>
        public string SumSkuCode { get; set; }
        /// <summary>
        /// Spu产品名
        /// </summary>
        public string SpuName { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }

        public string SpuKeywords { get; set; }

        public string SumSkuKeywords { get; set; }

        /// <summary>
        /// 币别
        /// </summary>
        public string Currency { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }
    }
}
