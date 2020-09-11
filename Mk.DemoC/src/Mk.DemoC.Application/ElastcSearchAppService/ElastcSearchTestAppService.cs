using HtmlAgilityPack;
using Leopard.Paging;
using Leopard.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mk.DemoC.Dto.ElastcSearchs;
using Mk.DemoC.ElastcSearchAppService.Documents;
using Mk.DemoC.SearchDocumentMgr;
using Mk.DemoC.SearchDocumentMgr.Entities;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mk.DemoC.ElastcSearchAppService
{
    // ElasticSearch 分词器
    // https://www.shangmayuan.com/a/49084a28ed9847aca4c9af36.html
    // Elasticsearch .Net Client NEST 多条件查询示例
    // https://www.cnblogs.com/huhangfei/p/5985280.html
    // Elasticsearch .net client NEST 5.x 使用总结
    // https://www.cnblogs.com/huhangfei/p/7524886.html
    // Elasticsearch搜索查询语法
    // https://www.cnblogs.com/haixiang/p/12095578.html
    // ElasticSearch keyword type
    // https://www.jianshu.com/p/8520a1884ac1
    // ElasticSearch索引查询term,match,match_phase,query_string之间的区别
    // https://blog.csdn.net/feinifi/article/details/100512058
    // ElasticSearch查看分词结果   (eg:在使用term时，一直匹配不到数据，使用此命令一看分词结果，发现英文是小写的)
    // https://blog.csdn.net/u014078154/article/details/79760766?utm_source=blogxgwz8  
    // elasticsearch 动态模板
    // https://www.cnblogs.com/zhb-php/p/7510233.html
    // [github]

    // =====normalizer=====
    // ElasticSearch Normalizer 的使用方法？
    // https://blog.csdn.net/yinni11/article/details/104431632
    // elasticsearch大小写无法使用term查询的问题
    // https://www.jianshu.com/p/a86074177585
    // Adding normalizer for all keyword fields NEST
    // https://stackoverflow.com/questions/61062996/adding-normalizer-for-all-keyword-fields-nest
    // Elasticsearch.Net and NEST: the.NET clients[7.x] 系列文档
    // Writing analyzers
    // https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/writing-analyzers.html

    // 在elasticsearch6.0之前是有个type的概念的，index类似于数据库的库，type类似于表，但是在6.0之后就不再使用type这个字段了

    [Route("api/democ/elastic-test")]
    public class ElastcSearchTestAppService : DemoCAppService
    {
        private readonly ILogger<ElastcSearchAppService> _logger;
        private readonly IProductSpuDocRepository _productSpuDocRepository;
        private readonly ElasticSearchClient _elasticSearchClient;
        private readonly IElasticClient client;

        public ElastcSearchTestAppService(
            ILogger<ElastcSearchAppService> logger
            , ElasticSearchClient elasticSearchClient
            , IProductSpuDocRepository productSpuDocRepository
            )
        {
            _logger = logger;
            _productSpuDocRepository = productSpuDocRepository;
            _elasticSearchClient = elasticSearchClient;
            client = _elasticSearchClient.Get();
        }

        [HttpPost("doc/create")]
        public async Task<ServiceResult<ProductSpuDocumentDto>> CreateDocumentAsync()
        {
            ServiceResult<ProductSpuDocumentDto> ret = new ServiceResult<ProductSpuDocumentDto>(IdProvider.Get());
            ProductSpuDoc entity = new ProductSpuDoc(GuidGenerator.Create(), "A001", "A0012 A0011"
                              , "一个产品，这个产品很有价值，是不是你想要的产品？", "诚实通", "关键词", null, "CNY", 12, 12);
            await _productSpuDocRepository.InsertAsync(entity);


            ProductSpuDocument document = ObjectMapper.Map<ProductSpuDoc, ProductSpuDocument>(entity);
            var response = await client.IndexAsync(document, idx => idx.Index(ElasticSearchClient.MALL_SEARCH_PRODUCT));

            //var getResponse = await client.GetAsync<ProductSpuDocument>(response.Id, idx => idx.Index(ElasticSearchClient.MALL_SEARCH_PRODUCT));
            //var r = getResponse.Source;

            entity.DocId = response.Id;
            await _productSpuDocRepository.UpdateAsync(entity);

            var retDto = ObjectMapper.Map<ProductSpuDoc, ProductSpuDocumentDto>(entity);
            ret.SetSuccess(retDto);
            return ret;
        }

        [HttpPost("doc/update/{id}")]
        public async Task<ServiceResult> UpdateDocumentAsync(string id)
        {
            ServiceResult ret = new ServiceResult(IdProvider.Get());
            var getResponse = await client.GetAsync<ProductSpuDocument>(id, idx => idx.Index(ElasticSearchClient.MALL_SEARCH_PRODUCT));
            var doc = getResponse.Source;

            // 部分更新
            IUpdateRequest<ProductSpuDocument, object> updateRequest1 = new UpdateRequest<ProductSpuDocument, object>(ElasticSearchClient.MALL_SEARCH_PRODUCT, id)
            {
                Doc = new
                {
                    SpuName = doc.SpuName + "--update1"
                }
            };

            var updateRp1 = await client.UpdateAsync(updateRequest1);
            doc = updateRp1.Get.Source;



            // 全部更新
            IUpdateRequest<ProductSpuDocument, ProductSpuDocument> updateRequest2 = new UpdateRequest<ProductSpuDocument, ProductSpuDocument>(ElasticSearchClient.MALL_SEARCH_PRODUCT, id)
            {
                Doc = new ProductSpuDocument()
                {
                    SpuCode = "A123456"
                }
            };

            var updateRp2 = await client.UpdateAsync(updateRequest2);
            doc = updateRp2.Get.Source;

            ret.SetSuccess();
            return ret;
        }

        [HttpPost("doc/update/{id}")]
        public async Task<ServiceResult> UpdateSpuCodeAsync(string id, string spuCode)
        {
            ServiceResult ret = new ServiceResult(IdProvider.Get());

            await client.UpdateByQueryAsync<ProductSpuDocument>(
                s => s
                    .Index(ElasticSearchClient.MALL_SEARCH_PRODUCT)
                    .Query(q => q
                        .Term("id", id)
                    )
                );

            ret.SetSuccess();
            return ret;

            // https://stackoom.com/question/3Y881/%E5%A6%82%E4%BD%95%E5%9C%A8ElasticSearch-NEST-%E4%B8%AD%E6%8F%92%E5%85%A5%E6%88%96%E6%9B%B4%E6%96%B0%E6%96%87%E6%A1%A3
        }

        [HttpDelete("doc/delete/{id}")]
        public async Task DeleteAsync(string id)
        {
            DeleteRequest delRequest = new DeleteRequest(ElasticSearchClient.MALL_SEARCH_PRODUCT, id);

            await client.DeleteAsync(delRequest);
        }

        [HttpDelete("doc/delete/{spuCode}")]
        public async Task DeleteBySpuCodeAsync(string spuCode)
        {
            await client.DeleteByQueryAsync<ProductSpuDocument>(
                 s => s
                     .Index(ElasticSearchClient.MALL_SEARCH_PRODUCT)
                     .Query(q => q
                         .Term(f => f.SpuCode, spuCode)
                     )
                 );
        }
    }
}
