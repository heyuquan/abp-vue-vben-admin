using Microsoft.AspNetCore.Mvc;
using Mk.DemoC.IAppService;
using StackExchange.Profiling;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Mk.DemoB.Application
{
    /// <summary>
    /// 其他测试
    /// </summary>
    [Route("api/demob/other")]
    public class OtherAppService : DemoBAppService
    {

        public OtherAppService()
        {

        }

        /// <summary>
        /// test1
        /// </summary>
        /// <param name="swaggerEmumTestType"><see cref="SwaggerEmumTestType" /></param>
        /// <returns></returns>
        [HttpPost]
        [Route("test1")]
        public string test1(SwaggerEmumTestType swaggerEmumTestType)
        {
            return "";
        }

        [HttpPost]
        [Route("test2")]
        public string test2(Othertest2 t)
        {
            return "";
        }

        [HttpGet]
        [Route("test3")]
        public SwaggerEmumTestType test3()
        {
            return SwaggerEmumTestType.test1;
        }

        [HttpGet]
        [Route("MiniProfilerTest")]
        public IEnumerable<string> MiniProfilerTest()
        {
            // MiniProfiler.Current.Step方法定义了分析的步骤，这个方法可以接受一个String类型的参数，它会显示在最终的报告中
            // MiniProfiler.Current.CustomTiming方法是更细粒度的对报告内容进行分类，以上代码中定义了2种分类，一种是SQL, 一种是Http
            string url1 = string.Empty;
            string url2 = string.Empty;
            using (MiniProfiler.Current.Step("Get方法"))
            {
                using (MiniProfiler.Current.Step("准备数据"))
                {
                    using (MiniProfiler.Current.CustomTiming("SQL", "SELECT * FROM Config"))
                    {
                        // 模拟一个SQL查询
                        Thread.Sleep(500);

                        url1 = "https://www.baidu.com";
                        url2 = "https://www.sina.com.cn/";
                    }
                }


                using (MiniProfiler.Current.Step("使用从数据库中查询的数据，进行Http请求"))
                {
                    using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url1))
                    {
                        var client = new WebClient();
                        var reply = client.DownloadString(url1);
                    }

                    using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url2))
                    {
                        var client = new WebClient();
                        var reply = client.DownloadString(url2);
                    }
                }
            }
            return new string[] { "value1", "value2" };
        }
    }

    public class Othertest2
    {
       public SwaggerEmumTestType swaggerEmumTestType { get; set; }
    }
}
