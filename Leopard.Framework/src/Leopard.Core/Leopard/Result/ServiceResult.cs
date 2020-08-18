using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp;
using Volo.Abp.Http;

namespace Leopard.Result
{
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
        public long Timestamp { get; } = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;

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
    public class ServiceResult<T> : ServiceResult where T : class
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
