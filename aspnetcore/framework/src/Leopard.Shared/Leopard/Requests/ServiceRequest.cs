using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Requests
{
    /// <summary>
    /// Service 请求实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceRequest<T>
    {
        /// <summary>
        /// 请求Id
        /// </summary>
        public string RequestId { get; set; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// 请求头
        /// </summary>
        public ServiceRequestHeader Header { get; set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public T Data { get; set; }

    }

    /// <summary>
    /// 请求头
    /// </summary>
    public class ServiceRequestHeader
    {
        /// <summary>
        /// 请求时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 用于唯一标识客户端  eg:APP=4，H5=5，pc web=6
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// client version，APP版本号 （App会存在多个版本，一般pc web只有一个版本）
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// web api 版本号
        /// </summary>
        public string ServerVersion { get; set; }

        /// <summary>
        /// 系统标识Code
        /// </summary>
        public string SystemCode { get; set; }

        /// <summary>
        /// 身份Token
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// [营销] 来源，eg：百度搜索、**广告Id
        /// </summary>
        public string MarketFrom { get; set; }
        /// <summary>
        /// 语言 eg：en，zh
        /// </summary>
        public string Language { get; set; }

    }
}
