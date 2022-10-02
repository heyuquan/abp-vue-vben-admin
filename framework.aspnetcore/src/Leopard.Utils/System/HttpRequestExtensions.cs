using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class HttpRequestExtensions
    {
        private static readonly List<(string, string)> _sslHeaders = new List<(string, string)>
        {
            ("HTTP_CLUSTER_HTTPS", "on"),
            ("HTTP_X_FORWARDED_PROTO", "https"),
            ("X-Forwarded-Proto", "https"),
            ("x-arr-ssl", null),
            ("X-Forwarded-Protocol", "https"),
            ("X-Forwarded-Ssl", "on"),
            ("X-Url-Scheme", "https")
        };

        /// <summary>
        /// Tries to read a request value first from <see cref="HttpRequest.Form"/> (if method is POST), then from
        /// <see cref="HttpRequest.Query"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key">The key to lookup.</param>
        /// <param name="values">The found values if any</param>
        /// <returns><c>true</c> if a value with passed <paramref name="key"/> was present, <c>false</c> otherwise.</returns>
        public static bool TryGetValue(this HttpRequest request, string key, out StringValues values)
        {
            values = StringValues.Empty;

            if (request.HasFormContentType)
            {
                values = request.Form[key];
            }

            if (values == StringValues.Empty)
            {
                values = request.Query[key];
            }

            return values != StringValues.Empty;
        }

        /// <summary>
        /// Tries to read a request value first from <see cref="HttpRequest.Form"/> (if method is POST), then from
        /// <see cref="HttpRequest.Query"/>, and converts value to <typeparamref name="T"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key">The key to lookup.</param>
        /// <param name="value">The found and converted value</param>
        /// <returns><c>true</c> if a value with passed <paramref name="key"/> was present and could be converted, <c>false</c> otherwise.</returns>
        public static bool TryGetValueAs<T>(this HttpRequest request, string key, out T value)
        {
            value = default;

            if (request.TryGetValue(key, out var values))
            {
                value = values.ToString().CastTo<T>();
                return true;
            }

            return false;
        }
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }

        public static string UserAgent(this HttpRequest request)
        {
            if (request.Headers.TryGetValue(HeaderNames.UserAgent, out var value))
            {
                return value.ToString();
            }

            return null;
        }

        public static string UrlReferrer(this HttpRequest request)
        {
            if (request.Headers.TryGetValue(HeaderNames.Referer, out var value))
            {
                return value.ToString();
            }

            return null;
        }

        /// <summary>
        /// Gets the raw request path (PathBase + Path + QueryString)
        /// </summary>
        /// <returns>The raw URL</returns>
        public static string RawUrl(this HttpRequest request)
        {
            // Try to resolve unencoded raw value from feature.
            var url = request.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;
            if (url.IsNullOrWhiteSpace2())
            {
                // Fallback
                url = request.PathBase + request.Path + request.QueryString;
            }

            return url;
        }

        /// <summary>
        /// Gets a value which indicates whether the HTTP connection uses secure sockets (HTTPS protocol). 
        /// Works with cloud's load balancers.
        /// </summary>
        public static bool IsSecureConnection(this HttpRequest request)
        {
            if (request.IsHttps)
            {
                return true;
            }

            foreach (var tuple in _sslHeaders)
            {
                var serverVar = request.Headers[tuple.Item1];
                if (serverVar != StringValues.Empty)
                {
                    return tuple.Item2 == null || tuple.Item2.Equals(serverVar, StringComparison.OrdinalIgnoreCase);
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether the current request is an AJAX request.
        /// </summary>
        /// <param name="httpRequest"></param>
        public static bool IsAjaxRequest(this HttpRequest httpRequest)
        {
            string xRequestedWith = "X-Requested-With";  // HeaderNames.XRequestedWith;
            return
                string.Equals(httpRequest.Headers[xRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
                string.Equals(httpRequest.Query[xRequestedWith], "XMLHttpRequest", StringComparison.Ordinal);
        }

    }
}
