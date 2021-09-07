using Leopard.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mk.DemoC.RemoteCallAppService
{
    /// <summary>
    /// ocelot 负载测试   (两个链接，模拟负载)
    /// </summary>
    [Route("api/democ/loadbalance")]
    [Route("api/loadbalance")]
    public class LoadBalanceAppService : DemoCAppService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoadBalanceAppService(
            IHttpContextAccessor httpContextAccessor
            )
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("location")]
        public async Task<ServiceResult<string>> Location()
        {
            ServiceResult<string> ret = new ServiceResult<string>(CorrelationIdIdProvider.Get());
            var context = _httpContextAccessor.HttpContext;

            ret.SetSuccess($"负载|democ|{context.Request.Host.Value}|{context.Request.Path}");
            return await Task.FromResult(ret);
        }
    }
}
