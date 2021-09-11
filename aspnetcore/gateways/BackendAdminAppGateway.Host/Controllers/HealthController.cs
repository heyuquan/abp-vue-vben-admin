using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace BackendAdminAppGateway.Controllers
{
    [Route("api/backendAdminAppGateway/health")]
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