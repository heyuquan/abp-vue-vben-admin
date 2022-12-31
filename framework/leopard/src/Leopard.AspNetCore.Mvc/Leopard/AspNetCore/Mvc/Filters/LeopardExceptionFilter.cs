﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leopard.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Volo.Abp.Tracing;

namespace Leopard.AspNetCore.Mvc.Filters
{
    public class LeopardExceptionFilter : IAsyncExceptionFilter, ITransientDependency
    {
        public ILogger<LeopardExceptionFilter> Logger { get; set; }

        private readonly IExceptionToErrorInfoConverter _errorInfoConverter;
        private readonly IHttpExceptionStatusCodeFinder _statusCodeFinder;
        private readonly AbpExceptionHandlingOptions _exceptionHandlingOptions;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly IHostEnvironment _env;

        private readonly Func<object, Task> _clearCacheHeadersDelegate;

        public LeopardExceptionFilter(
            ILogger<LeopardExceptionFilter> logger,
            IHostEnvironment env,
            IExceptionToErrorInfoConverter errorInfoConverter,
            IHttpExceptionStatusCodeFinder statusCodeFinder,
            IOptions<AbpExceptionHandlingOptions> exceptionHAndlingOptions,
            IJsonSerializer jsonSerializer,
            ICorrelationIdProvider correlationIdProvider)
        {
            _errorInfoConverter = errorInfoConverter;
            _exceptionHandlingOptions = exceptionHAndlingOptions.Value;
            _env = env;
            _statusCodeFinder = statusCodeFinder;
            _jsonSerializer = jsonSerializer;
            _correlationIdProvider = correlationIdProvider;

            // Volo.Abp.AspNetCore.Mvc.ExceptionHandling.AbpExceptionFilter 中直接使用NullLogger，但也可以打出日志，不清楚什么原因
            //Logger = NullLogger<LeopardExceptionFilter>.Instance;
            Logger = logger;
            _clearCacheHeadersDelegate = ClearCacheHeaders;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!ShouldHandleException(context))
            {
                return;
            }

            await HandleAndWrapException(context);
        }

        protected virtual bool ShouldHandleException(ExceptionContext context)
        {
            //TODO: Create DontWrap attribute to control wrapping..?

            if (context.ActionDescriptor.IsControllerAction() &&
                context.ActionDescriptor.HasObjectResult())
            {
                return true;
            }

            if (context.HttpContext.Request.CanAccept(MimeTypes.Application.Json))
            {
                return true;
            }

            if (context.HttpContext.Request.IsAjax())
            {
                return true;
            }

            return false;
        }

        protected virtual async Task HandleAndWrapException(ExceptionContext context)
        {
            //TODO: Trigger an AbpExceptionHandled event or something like that.

            // 代码中应始终抛出异常：UserFriendlyException、AbpValidationException、EntityNotFoundException、AbpAuthorizationException、BusinessException

            context.HttpContext.Response.Headers.Add(AbpHttpConsts.AbpErrorFormat, "true");
            context.HttpContext.Response.StatusCode = (int)_statusCodeFinder.GetStatusCode(context.HttpContext, context.Exception);
            context.HttpContext.Response.OnStarting(_clearCacheHeadersDelegate, context.HttpContext.Response);

            var remoteServiceErrorInfo = _errorInfoConverter.Convert(context.Exception, options =>
            {
                options.SendExceptionsDetailsToClients = _exceptionHandlingOptions.SendExceptionsDetailsToClients;
                options.SendStackTraceToClients = _exceptionHandlingOptions.SendStackTraceToClients;
            });
            StringBuilder sbuilder = new StringBuilder(128);
            if (string.IsNullOrWhiteSpace(remoteServiceErrorInfo.Details))
            {
                sbuilder.Append($"{context.Exception.GetType()}. {context.Exception.Message}");
            }
            else
            {
                sbuilder.Append(remoteServiceErrorInfo.Details);
            }

            if (remoteServiceErrorInfo.ValidationErrors != null && remoteServiceErrorInfo.ValidationErrors.Any())
            {
                sbuilder.Append($"ValidationErrors:{_jsonSerializer.Serialize(remoteServiceErrorInfo.ValidationErrors, indented: true)}");
            }

            ServiceResponse<RemoteServiceErrorInfo> ret = new ServiceResponse<RemoteServiceErrorInfo>(_correlationIdProvider.Get());
            ret.AddMessage(ServiceResponseMessage.CreateError(remoteServiceErrorInfo.Code, remoteServiceErrorInfo.Message, sbuilder.ToString()));

            context.Result = new ObjectResult(ret);

            var logLevel = context.Exception.GetLogLevel();

            Logger.LogWithLevel(logLevel, _jsonSerializer.Serialize(ret, indented: true), context.Exception);
            //Logger.LogException(context.Exception, logLevel);

            await context.HttpContext
                .RequestServices
                .GetRequiredService<IExceptionNotifier>()
                .NotifyAsync(
                    new ExceptionNotificationContext(context.Exception)
                );

            context.Exception = null; //Handled!
        }

        private Task ClearCacheHeaders(object state)
        {
            var response = (HttpResponse)state;

            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);

            return Task.CompletedTask;
        }
    }
}