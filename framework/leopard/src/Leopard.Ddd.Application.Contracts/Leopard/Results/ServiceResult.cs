using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp;
using Volo.Abp.Http;

namespace Leopard.Results
{
    // to do 
    // 保留 IsSuccess 字段，用于常规判断调用是否成功。成功即Payload里面会返回接口数据。   失败errors里面会包含错误信息
    // 增加 string:Status  eg：ok，fail  前端可以根据一些状态string，做一些定制化的事情
    // eg:Enum: "OK" "CREATED" "ACCEPTED" "NO_CONTENT" "PARTIAL" "MOVED_PERMANENT" "FOUND" "SEE_OTHER" "NOT_MODIFIED" "TEMPORARY_REDIRECT" "BAD_REQUEST" "UNAUTHORIZED" "FORBIDDEN" "NOT_FOUND" "METHOD_NOT_ALLOWED" "NOT_ACCEPTABLE" "REQUEST_TIMEOUT" "CONFLICT" "REQUEST_ENTITY_TOO_LARGE" "UNSUPPORTED_MEDIA_TYPE" "UNPROCESSABLE_ENTITY" "FAIL" "BAD_GATEWAY" "SERVICE_UNAVAILABLE" "GATEWAY_TIMEOUT"
    // 因为 string:Status 可以把状态表达的更清楚。   SetFailed  增加枚举参数。啥类型的错误。eg：normal->fail ，认证错误啥的
    // 参考 https://developer.walmart.com/api/us/mp/fulfillment#operation/printCarrierLabel 设计
    // 额外增加 errors[] 字段，因为可能有些接口调用成功，但是也想返回 info 类型的数据到前端
    // Data 重命名为  Payload （有效负载） 

    /// <summary>
    /// 服务层响应实体
    /// 适用于没有返回值的情况
    /// </summary>
    public class ServiceResult
    {
        public ServiceResult() : this(Guid.NewGuid().ToString("N"))
        {
        }

        public ServiceResult(string requestId)
        {
            RequestId = requestId;
        }

        /// <summary>
        /// 请求Id
        /// </summary>
        public string RequestId { get; protected set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; protected set; } = false;

        /// <summary>
        /// 时间戳(毫秒)
        /// </summary>
        public long Timestamp { get; } = (DateTime.UtcNow.Ticks - 621355968000000000) / 10000;

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <returns></returns>
        public void SetSuccess()
        {
            IsSuccess = true;
        }

        /// <summary>
        /// 响应失败  
        /// </summary>
        /// <returns></returns>
        public void SetFailed()
        {
            IsSuccess = false;
        }
    }
    /// <summary>
    /// 服务层响应实体
    /// </summary>
    public class ServiceResult<T> : ServiceResult
    {
        public ServiceResult() : base(Guid.NewGuid().ToString("N"))
        {
        }

        public ServiceResult(string requestId) : base(requestId)
        {

        }


        public T Data { get; private set; }


        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void SetSuccess(T data)
        {
            IsSuccess = true;
            Data = data;
        }

        /// <summary>
        /// 响应失败  
        /// </summary>
        /// <param name="data">必须是RemoteServiceErrorInfo对象</param>
        /// <returns></returns>
        public void SetFailed(T data)
        {
            IsSuccess = false;
            if (data is RemoteServiceErrorInfo)
            {
                   Data = data;
            }
            else
            {
                // 代码中应始终抛出异常：UserFriendlyException、AbpValidationException、EntityNotFoundException、AbpAuthorizationException、BusinessException

                throw new BusinessException(message: "抛异常的方式有误");
            }
        }

    }
}
