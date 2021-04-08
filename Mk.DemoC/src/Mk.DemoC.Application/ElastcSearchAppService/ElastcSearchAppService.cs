using HtmlAgilityPack;
using Leopard.Paging;
using Leopard.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mk.DemoC.SearchDocumentMgr;
using Mk.DemoC.SearchDocumentMgr.Documents;
using Mk.DemoC.SearchDocumentMgr.Entities;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mk.DemoC.ElastcSearchAppService
{
    [Route("api/democ/elastic")]
    public class ElastcSearchAppService : ElastcSearchBaseAppService
    {
        private readonly ILogger<ElastcSearchAppService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IProductSpuDocRepository _productSpuDocRepository;

        public ElastcSearchAppService(
            ILogger<ElastcSearchAppService> logger
            , IHttpClientFactory clientFactory
            , ElasticSearchClient elasticSearchClient
            , IProductSpuDocRepository productSpuDocRepository
            ) : base(elasticSearchClient)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _productSpuDocRepository = productSpuDocRepository;
        }

        //https://www.zyccst.com/yaocai-277.html		狗鞭
        //https://www.zyccst.com/yaocai-786.html		广金钱草
        //https://www.zyccst.com/yaocai-192.html		枸杞子
        //https://www.zyccst.com/yaocai-526.html		石斛
        [HttpPost("product/capture")]
        public async Task<ServiceResult<long>> CaptureProductDocAsync()
        {
            ServiceResult<long> ret = new ServiceResult<long>(CorrelationIdIdProvider.Get());
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
                    for (var i = 1; i <= capturePageCount; i++)
                    {
                        string productListUrl = string.Format(subUrlTemplate, item.Key, i);
                        _logger.LogInformation($"产品抓取[{item.Value}],url:[{productListUrl}]");
                        HttpResponseMessage subResponse = await client.GetAsync(productListUrl).ConfigureAwait(false);
                        string subHtml = await subResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        HtmlDocument subDoc = new HtmlDocument();
                        subDoc.LoadHtml(subHtml);

                        HtmlNodeCollection productNodes = subDoc.DocumentNode.SelectNodes("//div[@class='productUnit clf']");
                        foreach (var product in productNodes)
                        {
                            HtmlNode titleNode = product.SelectSingleNode(".//div[@class='proTitle']").SelectSingleNode(".//a");
                            string productName = titleNode.InnerText;
                            string productUrl = titleNode.Attributes["href"].Value;
                            string productCode = (productUrl.Split("-")).Last().Split(".")[0];
                            decimal price = decimal.Parse(product.SelectSingleNode(".//div[@class='proPrice l']").SelectSingleNode(".//span").InnerText.Split(" ").Last());

                            ProductSpuDoc entity = new ProductSpuDoc(GuidGenerator.Create(), null, productCode, null
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

        [HttpDelete("product/delete/all")]
        public async Task<ServiceResult> DeleteProductDocAsync()
        {
            ServiceResult ret = new ServiceResult(CorrelationIdIdProvider.Get());
            await _productSpuDocRepository.DeleteAllAsync();
            ret.SetSuccess();
            return ret;
        }

        [HttpPost("doc/create/all")]
        public async Task CreateDocumentAll()
        {
            PageData<ProductSpuDoc> docsEntity = await _productSpuDocRepository.GetPagingAsync(
                isGetTotalCount: false,
                isNotracking: true
                );

            List<ProductSpuDocument> docs = ObjectMapper.Map<List<ProductSpuDoc>, List<ProductSpuDocument>>(docsEntity.Items);

            var rp = await client.IndexManyAsync(docs, ElasticSearchClient.MALL_SEARCH_PRODUCT);
            if (rp.OriginalException != null)
            {
                throw rp.OriginalException;
            }
        }
    }
}
