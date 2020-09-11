using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoC.ElastcSearchAppService.Documents
{
    public class ProductSpuDocument
    {
        /// <summary>
        /// Spu编码
        /// </summary>
        [Keyword(Normalizer = "lowercase")]
        public string SpuCode { get; set; }
        /// <summary>
        /// Spu下所有Sku拼接，以空格隔开
        /// </summary>
        [Text]
        public string SumSkuCode { get; set; }
        /// <summary>
        /// Spu产品名
        /// </summary>
        [Text]
        public string SpuName { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Text]
        public string Brand { get; set; }

        [Text]
        public string SpuKeywords { get; set; }
        [Text]
        public string SumSkuKeywords { get; set; }

        /// <summary>
        /// 币别
        /// </summary>
        [Keyword(Normalizer = "lowercase")]
        public string Currency { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }
    }
}
