using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Leopard.Requests
{
    /// <summary>
    /// 服务调用返回消息类型级别
    /// 级别目前只定义：INFO，WARN，ERROR
    /// </summary>
    public enum ServiceResponseMessageLevel : byte
    {
        /// <summary>
        /// 一般反馈信息
        /// </summary>
        [EnumMember(Value = "INFO")]
        INFO = 0,

        /// <summary>
        /// 缺失输入信息，但不影响流程
        /// </summary>
        [EnumMember(Value = "WARN")]
        WARN =20,

        /// <summary>
        /// 错误，影响流程
        /// </summary>
        [EnumMember(Value = "ERROR")]
        ERROR =40,
    }
}
