using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Result
{
    /// <summary>
    /// 服务层响应实体
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
        public string RequestId { get; private set; }
        /// <summary>
        /// 响应码
        /// </summary>
        public ServiceResultCode ResultCode { get; private set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrorCode { get; private set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 时间戳(毫秒)
        /// </summary>
        public long Timestamp { get; } = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public void SetSuccess(string message = "")
        {
            Message = message;
            ResultCode = ServiceResultCode.Succeed;
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public void SetFailed(string errorCode, string message = "")
        {
            ErrorCode = errorCode;
            Message = message;
            ResultCode = ServiceResultCode.Failed;
        }

    }
}
