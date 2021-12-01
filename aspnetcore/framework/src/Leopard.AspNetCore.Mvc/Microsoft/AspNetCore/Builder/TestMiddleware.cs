using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TestMiddleware> _logger;


        public TestMiddleware(RequestDelegate next, ILogger<TestMiddleware> logger)
        {
            this._next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            StringBuilder sbuilder = new StringBuilder(128);
            sbuilder.Append("TestMiddleware==>>");

            sbuilder.Append("Headers==>>");
            foreach (var item in context.Request.Headers)
            {
                sbuilder.AppendFormat("{0}={1};", item.Key, item.Value);
            }

            _logger.LogInformation(sbuilder.ToString());
            // Do something...
            await _next(context);
        }
    }
}
