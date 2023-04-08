using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ModelBindingCollectionExtensions
    {
        public static void ConfigureModelBindingExceptionHandling(this IServiceCollection services)
        {
            // 参考：NetCore实现全局模型绑定异常信息统一处理
            // https://www.cnblogs.com/ancold/p/15738246.html
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState?
                        .Where(e => e.Value.Errors.Count > 0)
                        ?.Select(e => new 
                        {
                            ErrorCode = -9998,
                            ErrorMessage = $"模型绑定异常：{e.Value.Errors.First().ErrorMessage}",
                            Status = false
                        })?.FirstOrDefault();

                    return new BadRequestObjectResult(errors);
                    //此时Http状态码返回的依旧是400，如果想返回正常的http状态码200，请使用下面一行代码
                    //return new  ObjectResult(errors);
                };
            });
        }
    }
}
