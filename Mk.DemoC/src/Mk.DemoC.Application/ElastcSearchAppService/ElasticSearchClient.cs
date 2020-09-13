using Microsoft.Extensions.Configuration;
using Mk.DemoC.SearchDocumentMgr.Documents;
using Nest;
using System;
using Volo.Abp.DependencyInjection;

namespace Mk.DemoC.ElastcSearchAppService
{
    public class ElasticSearchClient : ITransientDependency
    {
        public const string MALL_SEARCH_PRODUCT = "mall.search.product";

        private readonly IElasticClient client;
        private readonly IConfiguration _configuration;

        public ElasticSearchClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var node = new Uri(_configuration["ElasticConfiguration:Uri"]);
            var settings = new ConnectionSettings(node);
            client = new ElasticClient(settings);
        }

        public void InitIndex()
        {
            // term
            // 插入 "A001 " 带空格， SpuCode 字段标注 keyword，lowercase   是否可以用"A001"、"a001"搜索到？
            // 不能搜索到。 term是精确匹配，存带空格，查也必须带上
            if (!client.Indices.Exists(MALL_SEARCH_PRODUCT).Exists)
            {
                var index = client.Indices.Create(MALL_SEARCH_PRODUCT, c => c
                                  .Settings(s => s
                                      .Analysis(a => a
                                            .Normalizers(n => n.Custom("my_normalizer", cn => cn.Filters("lowercase")))
                                          )
                                      )
                                  .Map<ProductSpuDocument>(mm => mm
                                      .AutoMap()
                                  )
                             );
                if (!index.IsValid)
                {
                    throw new Exception($"[{MALL_SEARCH_PRODUCT}]索引创建失败");
                }
            }
        }

        public IElasticClient Get()
        {
            return client;
        }
    }
}
