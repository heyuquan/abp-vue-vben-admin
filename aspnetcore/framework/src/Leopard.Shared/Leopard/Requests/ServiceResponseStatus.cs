using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;

namespace Leopard.Requests
{
    /// <summary>
    /// 服务调用返回状态
    /// 调用方只需用 == 20 ，来判断调用成功；如需判断具体错误状态，再使用其他枚举值
    /// </summary>
    public enum ServiceResponseStatus : int
    {
        [EnumMember(Value = "UnKnow")]
        UnKnow = 0,
        /// <summary>
        /// 调用成功
        /// </summary>
        [EnumMember(Value = "OK")]
        OK = 20,
        /// <summary>
        /// 调用失败、常规失败
        /// </summary>
        [EnumMember(Value = "Fail")]
        Fail = 40,
        /// <summary>
        /// 未登录
        /// </summary>
        [EnumMember(Value = "UnLogin")]
        UnLogin = 80,
        /// <summary>
        /// 未授权
        /// </summary>
        [EnumMember(Value = "UnAuthorized")]
        UnAuthorized = 120,
        /// <summary>
        /// 禁止调用 （eg：后台策略，限流，熔断等）
        /// </summary>
        [EnumMember(Value = "Forbidden")]
        Forbidden = 150,
        /// <summary>
        /// 未找到调用方法
        /// </summary>
        [EnumMember(Value = "NotFound")]
        NotFound = 180,
        /// <summary>
        /// 请求超时
        /// </summary>
        [EnumMember(Value = "RequestTimeout")]
        RequestTimeout = 210,
    }

    public class ServiceResponseStatusHelper
    {
        private static ServiceResponseStatus[] specialErrorServiceResponseStatus = new ServiceResponseStatus[]
            {
                ServiceResponseStatus.UnLogin,
                ServiceResponseStatus.UnAuthorized,
                ServiceResponseStatus.Forbidden,
                ServiceResponseStatus.NotFound,
                ServiceResponseStatus.RequestTimeout,
            };
        /// <summary>
        /// 判断状态是否为特殊错误状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool IsSpecialErrorServiceResponseStatus(ServiceResponseStatus status)
        {
            return specialErrorServiceResponseStatus.Contains(status);
        }
    }
}
