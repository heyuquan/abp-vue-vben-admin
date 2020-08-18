using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mk.DemoB.ExchangeRateAppService
{
    // ID 和 Class h2[@id='test'][@class='test']
    // Html Agility Pack如何快速实现解析Html
    // https://www.cnblogs.com/xuliangxing/p/8004403.html

    /// <summary>
    /// 汇率
    /// </summary>
    public class ExchangeRateAppService : DemoBAppService
    {
        public readonly IHttpClientFactory _clientFactory;

        public ExchangeRateAppService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task CaptureRate()
        {
            HttpClient client = _clientFactory.CreateClient();
            client.BaseAddress = new System.Uri("https://www.exchange-rates.org/");

            HttpResponseMessage response = await client.GetAsync("/converter/CNY/USD/1");
            string html = await response.Content.ReadAsStringAsync();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode node = doc.DocumentNode.SelectSingleNode("//span[@id='ctl00_M_lblToAmount']");

            double rate = double.Parse(node.InnerText);

        }
    }
}
