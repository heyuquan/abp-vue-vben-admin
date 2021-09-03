using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mk.DemoC.SearchDocumentMgr.Documents;
using Nest;
using System;
using Volo.Abp.DependencyInjection;

namespace Mk.DemoC.ElastcSearchAppService
{
    public class ElasticSearchClient : ITransientDependency
    {
        // Es索引名称必须小写
        public const string MALL_SEARCH_PRODUCT = "mall.search.product";

        private readonly IElasticClient client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ElasticSearchClient> _logger;

        public ElasticSearchClient(
            IConfiguration configuration
            , ILogger<ElasticSearchClient> logger
            )
        {
            _configuration = configuration;
            _logger = logger;

            var node = new Uri(_configuration["ElasticConfiguration:Uri"]);
            var settings = new ConnectionSettings(node)
                .PrettyJson()
                .EnableHttpCompression()
                .RequestTimeout(TimeSpan.FromMinutes(2))
                .DefaultMappingFor<ProductSpuDocument>(m => m.IndexName(ElasticSearchClient.MALL_SEARCH_PRODUCT));
            client = new ElasticClient(settings);
        }

        public void InitIndex()
        {
            // term
            // 插入 "A001 " 带空格， SpuCode 字段标注 keyword，lowercase   是否可以用"A001"、"a001"搜索到？
            // 不能搜索到。 term是精确匹配，存带空格，查也必须带上
            if (!client.Indices.Exists(MALL_SEARCH_PRODUCT).Exists)
            {
                // 配置分词器
                // https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/writing-analyzers.html

                // setting 建议使用elasticsearch 提供的url设置，因为代码里面设置有些api没有，如拼音插件里面的api就没有
                // 查询设置
                // GET /mall.search.product/_settings
                // 创建索引
                // 使用 Index_create.txt 创建索引

                //var index = client.Indices.Create(MALL_SEARCH_PRODUCT, c => c
                //                  .Settings(s => s
                //                      .Analysis(a => a
                //                            .Normalizers(n => n.Custom("my_normalizer", cn => cn.Filters("lowercase")))
                //                            .Analyzers(a => a.Custom("ik_smart_pinyin", ca => ca.Tokenizer("ik_smart").Filters("g_pinyin", "word_delimiter")))
                //                            .Analyzers(a => a.Custom("ik_max_smart_pinyin", ca => ca.Tokenizer("ik_max_smart").Filters("g_pinyin", "word_delimiter")))
                //                            .TokenFilters(tf => tf.)
                //                          )
                //                      )
                //                  .Map<ProductSpuDocument>(mm => mm
                //                      .AutoMap()
                //                  )
                //             );
                _logger.LogWarning($"[{MALL_SEARCH_PRODUCT}]索引不存在，请手动创建索引");                
            }
            else
            {
                client.Map<ProductSpuDocument>(m => m.AutoMap());
               
            }
        }

        public IElasticClient Get()
        {
            return client;
        }
    }
}
