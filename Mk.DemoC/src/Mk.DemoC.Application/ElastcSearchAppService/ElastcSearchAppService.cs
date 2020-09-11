﻿using HtmlAgilityPack;
using Leopard.Paging;
using Leopard.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    [Route("api/democ/elastic")]
    public class ElastcSearchAppService : DemoCAppService
    {
        private readonly ILogger<ElastcSearchAppService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IProductSpuDocRepository _productSpuDocRepository;
        private readonly ElasticSearchClient _elasticSearchClient;
        private readonly IElasticClient client;

        public ElastcSearchAppService(
            ILogger<ElastcSearchAppService> logger
            , IHttpClientFactory clientFactory
            , ElasticSearchClient elasticSearchClient
            , IProductSpuDocRepository productSpuDocRepository
            )
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _productSpuDocRepository = productSpuDocRepository;
            _elasticSearchClient = elasticSearchClient;
            client = _elasticSearchClient.Get();
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

        private string indexName = "mall.search.product";
        [HttpPost("document/create")]
        public async Task CreateDocumentIndex()
        {
            PageData<ProductSpuDoc> docs = await _productSpuDocRepository.GetPagingAsync(
                isGetTotalCount: false,
                isNotracking: true
                );

            ProductSpuDoc entity = new ProductSpuDoc(GuidGenerator.Create(), "A001", "A0012 A0011"
                                , "一个产品，这个产品很有价值，是不是你想要的产品？", "诚实通", "关键词", null, "CNY", 12, 12);
            ProductSpuDocument document = ObjectMapper.Map<ProductSpuDoc, ProductSpuDocument>(entity);

            var response = await client.IndexAsync(document, idx => idx.Index(ElasticSearchClient.MALL_SEARCH_PRODUCT));

            var getResponse = await client.GetAsync<ProductSpuDocument>(response.Id, idx => idx.Index(ElasticSearchClient.MALL_SEARCH_PRODUCT));
            var r = getResponse.Source;

            var searchResponse = client.Search<ProductSpuDocument>(m => m
                                    .Index(ElasticSearchClient.MALL_SEARCH_PRODUCT)
                                    .Query(q => q.Term(tm => tm.SpuCode, "A001"))
                                );

            var searchResponse3 = client.Search<ProductSpuDocument>(s => s
                .Index(ElasticSearchClient.MALL_SEARCH_PRODUCT)
                .Query(q => q
                     .Match(m => m
                        .Field(f => f.SumSkuCode)
                        .Query("A0011")
                     )
                )
            );

            var searchResponse2 = client.Search<ProductSpuDocument>(m => m
                                    .Index(indexName)
                                    );

        }
    }
}
