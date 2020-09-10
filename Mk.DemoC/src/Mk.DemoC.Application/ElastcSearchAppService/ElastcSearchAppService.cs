using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Mk.DemoC.SearchDocumentMgr;
using Mk.DemoC.SearchDocumentMgr.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Leopard.Results;
using Nest;
using Mk.DemoC.Dto.ElastcSearchs;
using Mk.DemoC.ElastcSearchAppService.Documents;

namespace Mk.DemoC.ElastcSearchAppService
{
    [Route("api/democ/elastic")]
    public class ElastcSearchAppService : DemoCAppService
    {
        private readonly ILogger<ElastcSearchAppService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IProductSpuDocRepository _productSpuDocRepository;
        private readonly IElasticClient client;

        public ElastcSearchAppService(
            ILogger<ElastcSearchAppService> logger
            , IHttpClientFactory clientFactory
            , IProductSpuDocRepository productSpuDocRepository
            )
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _productSpuDocRepository = productSpuDocRepository;

            var node = new Uri("http://134.175.121.78:9200");
            var settings = new ConnectionSettings(node);
            client = new ElasticClient(settings);
            client.Indices.Create(indexName, c => c
                .Settings(s => s
                    .Analysis(a => a
                        .Normalizers(n => n.Custom("lowercase", cn => cn.Filters("lowercase")))
                        )
                    )
                .Map(m => m.DynamicTemplates(dt => dt.DynamicTemplate("keyword_to_lowercase", t => t
                            .MatchMappingType("string")
                            .Mapping(map => map.Keyword(k => k.Normalizer("lowercase")))
                         )
                       ))
            );
        }

        //https://www.zyccst.com/yaocai-277.html		狗鞭
        //https://www.zyccst.com/yaocai-786.html		广金钱草
        //https://www.zyccst.com/yaocai-192.html		枸杞子
        //https://www.zyccst.com/yaocai-526.html		石斛
        [HttpPost("product/capture")]
        public async Task<ServiceResult<long>> CaptureProductDocAsync()
        {
            ServiceResult<long> ret = new ServiceResult<long>(IdProvider.Get());
            long hadCaptureCount = 0;
            HttpClient client = _clientFactory.CreateClient();

            Dictionary<string, string> urlMap = new Dictionary<string, string>
            {
                { "277","狗鞭"},
                { "786","广金钱草"},
                { "192","枸杞子"},
                { "526","石斛"},
            };
            string urlTemplate = "https://www.zyccst.com/yaocai-{0}.html";
            string subUrlTemplate = "https://www.zyccst.com/yaocai-{0}-{1}.html";
            foreach (var item in urlMap)
            {
                string url = string.Format(urlTemplate, item.Key);
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                HtmlNode node = doc.DocumentNode.SelectSingleNode("//em[@class='n red num']");
                int totalCount = int.Parse(node.InnerText);
                if (totalCount > 0)
                {
                    // 每页12条记录。只获取前五页数据
                    int capturePageCount = ((totalCount / 12) + 1) > 5 ? 5 : ((totalCount / 12) + 1);

                    List<ProductSpuDoc> list = new List<ProductSpuDoc>();
                    for (var i = 1; i <= 5; i++)
                    {
                        string productListUrl = string.Format(subUrlTemplate, item.Key, i);
                        _logger.LogInformation($"产品抓取[{item.Value}],url:[{productListUrl}]");
                        HttpResponseMessage subResponse = await client.GetAsync(productListUrl).ConfigureAwait(false);
                        string subHtml = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        HtmlDocument subDoc = new HtmlDocument();
                        subDoc.LoadHtml(subHtml);

                        HtmlNodeCollection productNodes = doc.DocumentNode.SelectNodes("//div[@class='productUnit clf']");
                        foreach (var product in productNodes)
                        {
                            HtmlNode titleNode = product.SelectSingleNode(".//div[@class='proTitle']").SelectSingleNode(".//a");
                            string productName = titleNode.InnerText;
                            string productUrl = titleNode.Attributes["href"].Value;
                            string productCode = (productUrl.Split("-")).Last().Split(".")[0];
                            decimal price = decimal.Parse(product.SelectSingleNode(".//div[@class='proPrice l']").SelectSingleNode(".//span").InnerText.Split(" ").Last());

                            ProductSpuDoc entity = new ProductSpuDoc(GuidGenerator.Create(), productCode, null
                                , productName, "诚实通", item.Value, null, "CNY", price, price);
                            list.Add(entity);
                            hadCaptureCount++;
                        }

                        list.ForEach(async i => await _productSpuDocRepository.InsertAsync(i));
                        await CurrentUnitOfWork.SaveChangesAsync();
                        list.Clear();
                    }
                }
            }
            ret.SetSuccess(hadCaptureCount);
            return ret;
        }

        [HttpDelete("product/all")]
        public async Task<ServiceResult> DeleteProductDocAsync()
        {
            ServiceResult ret = new ServiceResult(IdProvider.Get());
            await _productSpuDocRepository.DeleteAllAsync();
            ret.SetSuccess();
            return ret;
        }

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
        private string indexName = "mall.search.product";
        [HttpPost("document/create")]
        public async Task CreateDocumentIndex()
        {
            ProductSpuDoc entity = new ProductSpuDoc(GuidGenerator.Create(), "A001", "A0012 A0011"
                                , "一个产品，这个产品很有价值", "诚实通", "关键词", null, "CNY", 12, 12);
            ProductSpuDocument document = ObjectMapper.Map<ProductSpuDoc, ProductSpuDocument>(entity);

            var response = await client.IndexAsync(document, idx => idx.Index(indexName));

            var getResponse = await client.GetAsync<ProductSpuDocument>(response.Id, idx => idx.Index(indexName));
            var r = getResponse.Source;

            var searchResponse = client.Search<ProductSpuDocument>(m => m
                                    .Index(indexName)
                                    .Query(q => q.Term(tm => tm.SpuCode, "A001"))
                                );

            var searchResponse3 = client.Search<ProductSpuDocument>(s => s
                .Index(indexName)
                .Query(q => q
                     .Match(m => m
                        .Field(f => f.SumSkuCode)
                        .Query("a001")
                     )
                )
            );

            var searchResponse2 = client.Search<ProductSpuDocument>(m => m
                                    .Index(indexName)
                                    );

        }
    }
}
