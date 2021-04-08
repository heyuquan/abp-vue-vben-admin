using Leopard.Results;
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
    [Route("api/demob/loadbalance")]
    [Route("api/loadbalance")]
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
        public async Task<ServiceResult<string>> Location()
        {
            ServiceResult<string> ret = new ServiceResult<string>(CorrelationIdIdProvider.Get());
            var context = _httpContextAccessor.HttpContext;

            ret.SetSuccess($"负载|demob|{context.Request.Host.Value}|{context.Request.Path}");
            return await Task.FromResult(ret);
        }
    }
}
