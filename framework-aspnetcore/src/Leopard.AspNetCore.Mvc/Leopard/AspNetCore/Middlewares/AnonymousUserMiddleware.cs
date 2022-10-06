using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Leopard.AspNetCore.Middlewares
{
    public class AnonymousUserMiddleware
    {
        private readonly RequestDelegate _next;

        public AnonymousUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "GET")
            {
                context.Request.Cookies.TryGetValue(Constants.AnonymousUserClaimName, out string anonymousUserId);
                // Generate guid for anonymous user id and set to cookie for 14 days
                if (string.IsNullOrEmpty(anonymousUserId))
                {
                    anonymousUserId = Guid.NewGuid().ToString();
                    context.Response.Cookies.Append(Constants.AnonymousUserClaimName, anonymousUserId,
                        new CookieOptions
                        {
                            SameSite = SameSiteMode.Lax,
                            Expires = DateTimeOffset.UtcNow.AddDays(14)
                        });
                }
            }
            await _next(context);
        }
    }

    public static class AnonymousUserApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAnonymousUser(this IApplicationBuilder app)
        {
            app.UseMiddleware<AnonymousUserMiddleware>();
            return app;
        }
    }
}