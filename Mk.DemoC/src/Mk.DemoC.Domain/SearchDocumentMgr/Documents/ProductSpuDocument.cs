using Mk.DemoC.Domain.Consts.ElastcSearchs;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoC.SearchDocumentMgr.Documents
{
    [ElasticsearchType(IdProperty = "DocId")]
    public class ProductSpuDocument
    {
        /// <summary>
        /// Doc id
        /// </summary>
        [Keyword(Normalizer = "my_normalizer")]
        public string DocId { get; set; }
        /// <summary>
        /// Spu编码
        /// </summary>
        [Keyword(Normalizer = "my_normalizer")]
        public string SpuCode { get; set; }
        /// <summary>
        /// Spu下所有Sku拼接，以空格隔开
        /// </summary>
        [Text]
        public string SumSkuCode { get; set; }
        /// <summary>
        /// Spu产品名
        /// </summary>
        [Text(Analyzer = ElastcSearchAnazer.IK_SMART)]
        public string SpuName { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Keyword(Normalizer = "my_normalizer")]
        public string Brand { get; set; }

        [Text(Analyzer = ElastcSearchAnazer.IK_SMART)]
        public string SpuKeywords { get; set; }
        [Text(Analyzer = ElastcSearchAnazer.IK_SMART)]
        public string SumSkuKeywords { get; set; }

        /// <summary>
        /// 币别
        /// </summary>
        [Keyword(Normalizer = "my_normalizer")]
        public string Currency { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }
    }
}
