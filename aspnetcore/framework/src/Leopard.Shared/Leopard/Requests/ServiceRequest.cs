using System;

namespace Leopard.Requests
{
    // request 和 response 序列化字段名都约定一个规则   但是Leopard.Shared这个程序集又不打算引用json相关的
    // #、全部小写
    // #、多单词，用下划线号隔开。   集C#这边命名的大写字母开头的都用 "_" 隔开

    // todo 参照 https://www.cnblogs.com/wucy/p/16124449.html
    // Controller里面弄些快捷方法，或者rsp里面定义一些static方法
    // 加一个 ResultWrapperFilter ，当没有返回统一结构时，直接报错

    // 规范原则
    // #、接口返回数据即显示：前端仅做渲染逻辑处理；
    // #、渲染逻辑禁止跨多个接口调用；
    // #、前端关注交互、渲染逻辑，尽量避免业务逻辑处理的出现；
    // #、请求响应传输数据格式：JSON，JSON数据尽量简单轻量，避免多级JSON的出现；

    // Boolean类型，JSON数据传输中一律使用1/0来标示，1为是/True，0为否/False；
    // 日期类型，JSON数据传输中一律使用字符串，具体日期格式因业务而定；

    /// <summary>
    /// Service 请求实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceRequest<T>
    {
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
