using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace InternalGateway.Controllers
{
    [Route("api/internalGateway/health")]
    public class HealthController : AbpController
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("OK");
        }
    }
}