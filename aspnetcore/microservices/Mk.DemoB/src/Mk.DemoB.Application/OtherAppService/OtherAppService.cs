using Microsoft.AspNetCore.Mvc;
using Mk.DemoC.IAppService;
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
    }

    public class Othertest2
    {
       public SwaggerEmumTestType swaggerEmumTestType { get; set; }
    }
}
