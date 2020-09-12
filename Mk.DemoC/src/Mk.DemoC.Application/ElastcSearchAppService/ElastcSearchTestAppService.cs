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
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mk.DemoC.ElastcSearchAppService
{
    // ElasticSearch 分词器
    // https://www.shangmayuan.com/a/49084a28ed9847aca4c9af36.html
    // Elasticsearch7 分词器(内置分词器和自定义分词器)
    // https://blog.csdn.net/white_while/article/details/98504574
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
    // ElasticSearch match, match_phrase, term区别
    // https://www.cnblogs.com/buxizhizhoum/p/9874703.html

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

        //[HttpPost("doc/reindex")]
        //public async Task<ServiceResult> Reindex()
        //{
        //    IReindexRequest<ProductSpuDocument> request=new ReindexRequest<ProductSpuDocument>()
        //    client.Reindex()
        //}

        /// <summary>
        /// 初始化索引测试数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("doc/init")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ServiceResult))]
        public async Task<ServiceResult<List<ProductSpuDocumentDto>>> InitDocument()
        {
            ServiceResult<List<ProductSpuDocumentDto>> ret = new ServiceResult<List<ProductSpuDocumentDto>>(IdProvider.Get());

            // id相同的Document，会自行变成更新而不是新增
            // 39f792fdc909816aeda1bbb97faa18a5
            // 39f79307c24e296637ea2dd8215172bd
            // 39f7930c455671def2f955af0906a466
            // 39f793154cac656efa8f211c156e46c4
            ProductSpuDocument doc1 = new ProductSpuDocument
            {
                DocId = "39f792fdc909816aeda1bbb97faa18a5",
                SpuCode = "A001",
                SumSkuCode = "A0012 A0011",
                Brand = "诚实通",
                SpuKeywords = "A001",
                SumSkuKeywords = "A0012 A0011",
                SpuName = "一个产品，这个产品很有价值，是不是你想要的产品？",
                Currency = "CNY",
                MinPrice = 13,
                MaxPrice = 13
            };

            IndexResponse rp1 = await client.IndexAsync(doc1, idx => idx.Index(ElasticSearchClient.MALL_SEARCH_PRODUCT));
            if (!rp1.IsValid)
            {
                throw rp1.OriginalException;
            }

            List<ProductSpuDocument> docList = new List<ProductSpuDocument>();
            ProductSpuDocument doc2 = new ProductSpuDocument
            {
                DocId = "39f79307c24e296637ea2dd8215172bd",
                SpuCode = "A001 ",                  // 带一个空格
                SumSkuCode = "A0012 A0011",
                Brand = "诚实通",
                SpuKeywords = "A001",
                SumSkuKeywords = "A0012 A0011",
                SpuName = "一个产品，这个产品很有价值，是不是你想要的产品？",
                Currency = "CNY",
                MinPrice = 12,
                MaxPrice = 12
            };

            ProductSpuDocument doc3 = new ProductSpuDocument
            {
                DocId = "39f7930c455671def2f955af0906a466",
                SpuCode = "A001",
                SumSkuCode = "A0012 A0011",
                Brand = "诚实通",
                SpuKeywords = "A001",
                SumSkuKeywords = "A0012 A0011",
                SpuName = "一个产品，这个产品很有价值，是不是你想要的产品？",
                Currency = "HKD",
                MinPrice = 12,
                MaxPrice = 12
            };

            ProductSpuDocument doc4 = new ProductSpuDocument
            {
                DocId = "39f793154cac656efa8f211c156e46c4",
                SpuCode = "A001",
                SumSkuCode = "A0012 A0011",
                Brand = "天地药材",
                SpuKeywords = "A001",
                SumSkuKeywords = "A0012 A0011",
                SpuName = "一个产品，这个产品很有价值，是不是你想要的产品？",
                Currency = "CNY",
                MinPrice = 12,
                MaxPrice = 12
            };
            docList.Add(doc2);
            docList.Add(doc3);
            docList.Add(doc4);

            BulkResponse rp2 = await client.IndexManyAsync(docList, ElasticSearchClient.MALL_SEARCH_PRODUCT);
            if (!rp2.IsValid)
            {
                throw rp2.OriginalException;
            }
            // 批量插入，需要实时进度跟踪的案例
            // elasticsearch-net\tests\Tests\Document\Multiple\BulkAll\BulkAllDisposeApiTests.cs

            List<ProductSpuDocumentDto> dtos = new List<ProductSpuDocumentDto>();
            dtos.Add(ObjectMapper.Map<ProductSpuDocument, ProductSpuDocumentDto>(doc1));
            dtos.Add(ObjectMapper.Map<ProductSpuDocument, ProductSpuDocumentDto>(doc2));
            dtos.Add(ObjectMapper.Map<ProductSpuDocument, ProductSpuDocumentDto>(doc3));
            dtos.Add(ObjectMapper.Map<ProductSpuDocument, ProductSpuDocumentDto>(doc4));
            ret.SetSuccess(dtos);
            return ret;

        }

        [HttpPost("doc/get")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ServiceResult<ProductSpuDocumentDto>))]
        public async Task<ServiceResult> GetAsync()
        {
            ServiceResult ret = new ServiceResult(IdProvider.Get());

            #region 获取doc的方式
            var rp1 = await client.GetAsync<ProductSpuDocument>("39f792fdc909816aeda1bbb97faa18a5"
                                                , idx => idx.Index(ElasticSearchClient.MALL_SEARCH_PRODUCT));
            if (rp1.OriginalException != null)
            {
                throw rp1.OriginalException;
            }

            // 39f792fdc909816aeda1bbb97faa18a5
            // 39f79307c24e296637ea2dd8215172bd
            // 39f7930c455671def2f955af0906a466
            // 39f793154cac656efa8f211c156e46c4
            var rp12 = await client.GetManyAsync<ProductSpuDocument>(
                                new List<string>() {
                                    "39f792fdc909816aeda1bbb97faa18a5",
                                    "39f79307c24e296637ea2dd8215172bd",
                                    "39f7930c455671def2f955af0906a466",
                                    "39f793154cac656efa8f211c156e46c4",
                                    "39f7-93154-cac6-56e-fa8f2-11c1-56e4-6c4"
                                }
                                , ElasticSearchClient.MALL_SEARCH_PRODUCT);
            List<ProductSpuDocument> list = rp12.Where(x => x.Found).Select(x => x.Source).ToList();

            // keyword
            var rp2 = client.Search<ProductSpuDocument>(m => m
                                    .Index(ElasticSearchClient.MALL_SEARCH_PRODUCT)
                                    .Query(q => q.Term(tm => tm.SpuCode, "A001 "))
                                );
            if (rp2.OriginalException != null)
            {
                throw rp2.OriginalException;
            }

            // text 分词
            var rp3 = client.Search<ProductSpuDocument>(s => s
                .Index(ElasticSearchClient.MALL_SEARCH_PRODUCT)
                .Query(q => q
                     .Match(m => m
                        .Field(f => f.SumSkuCode)
                        .Query("A0011")
                     )
                )
            );
            if (rp3.OriginalException != null)
            {
                throw rp3.OriginalException;
            }
            List<ProductSpuDocument> list3 = rp3.Documents.ToList();
            #endregion

            ret.SetSuccess();
            return ret;
        }

        [HttpPost("doc/search")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ServiceResult<ProductSpuDocumentDto>))]
        public async Task<ServiceResult<List<ProductSpuDocumentDto>>> SearchAsync(EsTestSearchRequest req)
        {
            ServiceResult<List<ProductSpuDocumentDto>> ret = new ServiceResult<List<ProductSpuDocumentDto>>(IdProvider.Get());

            // term
            var mustQuerys = new List<Func<QueryContainerDescriptor<ProductSpuDocument>, QueryContainer>>();
            if (!string.IsNullOrEmpty(req.Currency))
            {
                mustQuerys.Add(t => t.Term(f => f.Currency, req.Currency));
            }
            if (!string.IsNullOrEmpty(req.Brand))
            {
                mustQuerys.Add(t => t.Term(f => f.Brand, req.Brand));
            }

            // filters
            var mustFilters = new List<Func<QueryContainerDescriptor<ProductSpuDocument>, QueryContainer>>();
            mustFilters.Add(t => t.Match(f => f.Field(x => x.SpuCode).Query(req.Keyword)));
            mustFilters.Add(t => t.Match(f => f.Field(x => x.SumSkuCode).Query(req.Keyword)));
            mustFilters.Add(t => t.Match(f => f.Field(x => x.SpuKeywords).Query(req.Keyword)));
            mustFilters.Add(t => t.Match(f => f.Field(x => x.SumSkuKeywords).Query(req.Keyword)));
            mustFilters.Add(t => t.Match(f => f.Field(x => x.SpuName).Query(req.Keyword)));

            var rp = client.Search<ProductSpuDocument>(s => s
                            .Index(ElasticSearchClient.MALL_SEARCH_PRODUCT)
                            .Query(q => q
                                .Bool(b => b
                                    .Must(mustQuerys)
                                    .Filter(f => f
                                        .Bool(fb => fb.Should(mustFilters))
                                    )
                                )
                            )
                        );
            var productSpuDocuments = rp.Documents.ToList();

            ret.SetSuccess(ObjectMapper.Map<List<ProductSpuDocument>, List<ProductSpuDocumentDto>>(productSpuDocuments));
            return ret;
        }

        [HttpPost("doc/update")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ServiceResult))]
        public async Task<ServiceResult> UpdateDocumentAsync()
        {
            string id = "39f792fdc909816aeda1bbb97faa18a5";
            ServiceResult ret = new ServiceResult(IdProvider.Get());
            var getResponse = await client.GetAsync<ProductSpuDocument>(id
                                        , idx => idx.Index(ElasticSearchClient.MALL_SEARCH_PRODUCT));
            var doc = getResponse.Source;

            // 部分更新
            IUpdateRequest<ProductSpuDocument, object> updateRequest1
                = new UpdateRequest<ProductSpuDocument, object>(ElasticSearchClient.MALL_SEARCH_PRODUCT, id)
                {
                    Doc = new
                    {
                        SpuName = doc.SpuName + "--update1"
                    }
                };

            var rp1 = await client.UpdateAsync(updateRequest1);
            if (rp1.OriginalException != null)
            {
                throw rp1.OriginalException;
            }


            // 全部更新
            doc.SpuCode = "A12345678";
            IUpdateRequest<ProductSpuDocument, ProductSpuDocument> updateRequest3
                = new UpdateRequest<ProductSpuDocument, ProductSpuDocument>(ElasticSearchClient.MALL_SEARCH_PRODUCT, id)
                {
                    Doc = doc
                };

            var rp2 = await client.UpdateAsync(updateRequest3);
            if (rp2.OriginalException != null)
            {
                throw rp2.OriginalException;
            }

            // 批量更新
            var rp3 = await client.UpdateByQueryAsync<ProductSpuDocument>(
                                s => s
                                    .Index(ElasticSearchClient.MALL_SEARCH_PRODUCT)
                                    .Query(q => q
                                        .Match(m => m.Field(x => x.SumSkuCode).Query("A0011"))
                                    ).Script(ss => ss.Source("ctx._source.minPrice=8"))
                                );
            if (rp3.OriginalException != null)
            {
                throw rp3.OriginalException;
            }

            ret.SetSuccess();
            return ret;
        }

        [HttpDelete("doc/delete")]
        public async Task DeleteAsync()
        {
            string id = "39f792fdc909816aeda1bbb97faa18a5";

            DocumentPath<ProductSpuDocument> deletePath = new DocumentPath<ProductSpuDocument>(id);
            var rp1 = await client.DeleteAsync(deletePath, d => d.Index(ElasticSearchClient.MALL_SEARCH_PRODUCT));

            if (rp1.OriginalException != null)
            {
                throw rp1.OriginalException;
            }

            var rp2 = await client.DeleteByQueryAsync<ProductSpuDocument>(
                     s => s
                         .Index(ElasticSearchClient.MALL_SEARCH_PRODUCT)
                         .Query(q => q
                              .Match(m => m.Field(x => x.SumSkuCode).Query("A0011"))
                         )
                     );
            if (rp2.OriginalException != null)
            {
                throw rp2.OriginalException;
            }
        }

    }
}
