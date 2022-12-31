using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Leopard.AspNetCore.Middlewares
{
    /// <summary>
    /// 根据request请求，切换语言
    /// </summary>
    public class LocalizationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var cultureKey = context.Request.Headers["Accept-Language"];
            if (!string.IsNullOrEmpty(cultureKey))
            {
                if (CheckCultureExist(cultureKey))
                {
                    var culture = new CultureInfo(cultureKey);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }

            await next(context);
        }

        private static bool CheckCultureExist(string cultureName)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => string.Equals(culture.Name, cultureName, StringComparison.CurrentCultureIgnoreCase));
        }
    }

    public static class LocalizationMiddlewareApplicationBuilderExtensions
    {
        /// <summary>
        /// 根据request请求，切换语言
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLocalization(this IApplicationBuilder app)
        {
            app.UseMiddleware<LocalizationMiddleware>();
            return app;
        }
    }
}
