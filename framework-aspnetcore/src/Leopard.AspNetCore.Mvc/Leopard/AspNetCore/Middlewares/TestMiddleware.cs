using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.AspNetCore.Middlewares
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TestMiddleware> _logger;


        public TestMiddleware(RequestDelegate next, ILogger<TestMiddleware> logger)
        {
            _next = next;
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


    public static class TestMiddlewareApplicationBuilderExtensions
    {
        /// <summary>
        /// 用于调试，断点查看中间变量值
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseTestMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<TestMiddleware>();
            return app;
        }
    }
}
