using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Validation;

namespace Mk.DemoB.Application
{
    public class ErrorAppService : DemoBAppService
    {
        private IExceptionNotifier _exceptionNotifier;
        public ErrorAppService()
        {
            // 默认通知。 不知道那里注入进去的，默认会输出到logger日志
            //LazyGetRequiredService<IExceptionNotifier>(ref _exceptionNotifier);
            // Volo.Abp.AspNetCore.Mvc.ExceptionHandling.AbpExceptionFilter 过滤器处理异常
        }

        /// <summary>
        /// 抛出 Exception
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task ThrowDefaultError()
        {
            throw new Exception("this is default error");
        }

        /// <summary>
        /// 抛出 BusinessException
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task ThrowBusinessError()
        {
            Exception ex = new Exception("this is BusinessException error");

            BusinessException bizException = new BusinessException(
                "DemoBError:1001"       // 从资源文件中取Message信息
                , details: "business exception details"
                , innerException: ex
                , logLevel: LogLevel.Warning
                );

            throw bizException;
        }

        /// <summary>
        /// 抛出 UserFriendlyException
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task ThrowUserFriendlyError()
        {
            Exception ex = new Exception("this is UserFriendlyException error");

            UserFriendlyException userException = new UserFriendlyException(
                "user exception message"
                , ""
                , "user exception details"
                , ex
                , LogLevel.Warning
                );

            throw userException;
        }

        /// <summary>
        /// 抛出 UserFriendlyException
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task ThrowValidationError()
        {
            Exception ex = new Exception("this is AbpValidationException error");

            AbpValidationException validException = new AbpValidationException(
                "valid exception message"
                , ex
               );

            ValidationResult vr1 = new ValidationResult(
                "Username should be minimum length of 3."
                , new List<string> { "beek", "jet", "sky" });
            ValidationResult vr2 = new ValidationResult(
               "field is required."
               , new List<string> { "username", "password" });

            validException.ValidationErrors.Add(vr1);
            validException.ValidationErrors.Add(vr2);

            throw validException;
        }


    }
}
