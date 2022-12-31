using Leopard.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mk.DemoB.LoadBalanceAppService
{
    /// <summary>
    /// ocelot 负载测试
    /// </summary>
    [Route("api/demob/loadbalance-one")]
    [Route("api/demob/loadbalance-two")]
    public class LoadBalanceAppService : DemoBAppService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoadBalanceAppService(
            IHttpContextAccessor httpContextAccessor
            )
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("location")]
        public async Task<ServiceResponse<string>> Location()
        {
            ServiceResponse<string> ret = new ServiceResponse<string>(CorrelationIdIdProvider.Get());
            var context = _httpContextAccessor.HttpContext;

            ret.Payload = $"负载|demob|{context.Request.Host.Value}|{context.Request.Path}";
            return await Task.FromResult(ret);
        }
    }
}
