using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace SSO.AuthServer.Controllers
{
    [Route("api/authServer/health")]
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