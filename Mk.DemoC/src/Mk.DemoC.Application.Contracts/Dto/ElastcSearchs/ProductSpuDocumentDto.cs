using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoC.Dto.ElastcSearchs
{
    public class ProductSpuDocumentDto : ExtensibleCreationAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Doc id
        /// </summary>
        public string DocId { get; set; }
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
