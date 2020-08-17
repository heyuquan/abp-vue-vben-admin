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
            HttpClient client = _clientFactory.CreateClient("rate");


            var body = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("pjname", "美元")
                };

            string html = string.Empty;
            var content = new FormUrlEncodedContent(body);
            HttpResponseMessage response = client.PostAsync("/search/whpj/search_cn.jsp", content).Result;
            html = response.Content.ReadAsStringAsync().Result;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='BOC_main publish']/table/tr");

            for (int i = 0; i < nodes.Count; i++)
            {
                // index=0为标题
                // 货币名称0 现汇买入价1 现钞买入价2 现汇卖出价3 现钞卖出价4 中行折算价5 发布时间6
                if (i >= 1 && i <= 5)   // 抓前五行即可
                {
                    HtmlNode node = nodes[i];

                    HtmlNodeCollection subNodes = node.SelectNodes("./td");
                    string td0 = subNodes[0].InnerText;
                    string td1 = subNodes[1].InnerText;
                    string td2 = subNodes[2].InnerText;
                    string td3 = subNodes[3].InnerText;
                    string td4 = subNodes[4].InnerText;
                    string td5 = subNodes[5].InnerText;
                    string td6 = subNodes[6].InnerText;
                    //string td1= node.
                }
            }


        }
    }
}
