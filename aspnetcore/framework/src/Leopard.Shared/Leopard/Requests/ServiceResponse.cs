using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp;
using Volo.Abp.Http;
using System.Linq;
using System.Collections.ObjectModel;

namespace Leopard.Requests
{
    // 实体结构参考 https://developer.walmart.com/api/us/mp/fulfillment#operation/printCarrierLabel 设计


    // 约定 ： ServiceResponseMessage.Code
    /*
如果接口错误，则 Code 返回9位的错误码，规则分别是：1位接口状态码 + 2位业务系统编码 + 2位子系统编码 + 4位错误码。
第一位表示接口的状态信息，如下面附录中的“接口状态编码规定表”；
第2，3位是业务系统编码信息，如下面附录中的“业务系统编码规定表”；
第4，5位是子系统编码，请各业务子系统根据自身业务系统定义。默认为00，尽量跟业务系统编码保持一致。例如PBS的机票子系统用04表示，酒店子系统用03表示；
第6-9位是错误码，请接口开发者自定义，定义后请填写到Conf的ErrorCode集合中。

2(状态码)02(PBS)01(FH)0001()

接口状态编码规定表:
接口状态码    描述
1            接口正常错误，后端只需将错误信息记录到日志，前端不需做任何处理。 例如：各种日志接口问题等。
2            接口正常錯誤，前端需要對此类errcode做特殊處理。 例如：创建接口出现“套票已满”，需要重新跳到资源页等情况。
4            接口業務錯誤，前端直接提示errmessage信息。 例如：创建订单接口的信息不全引起的错误等。
5            接口系統錯誤，前端不顯示errmessage，展示默认接口請求失敗頁。 例如：“供应商数据异常”或接口请求其他接口报错等一系列的未知错误。

业务系统编码规定表:
业务系统 编码号
TBS      01
PBS      02
HBS      03
ABS      04
Wireless 05
FBS      06
CRM      07
CMS      08
BANNER   09

*/

    /// <summary>
    /// 服务层响应实体.适用于没有返回值的情况
    /// 只有 Messages 包含Error级别的消息，则IsSuccess=false，否则为true
    /// </summary>
    public class ServiceResponse
    {
        public ServiceResponse() : this(Guid.NewGuid().ToString("N"))
        {
        }

        /// <summary>
        /// ServiceResponse
        /// </summary>
        /// <param name="requestId">确保唯一的Id</param>
        public ServiceResponse(string requestId)
        {
            RequestId = requestId;
            _messages = new List<ServiceResponseMessage>();
        }

        private List<ServiceResponseMessage> _messages = null;

        /// <summary>
        /// 请求Id（也可称：服务端处理Id）
        /// 服务端生成的ID，注意：不能由调用端传入（因为无法确保调用方传入的是唯一值）。   
        /// 这个ID返回到调用端，调用端可使用这个ID与接口负责人员进行沟通
        /// 服务端会通过RequestId进行定位这次调用相关的request参数和response参数
        /// </summary>
        public string RequestId { get; protected set; }

        private ServiceResponseStatus statusCode = ServiceResponseStatus.UnKnow;
        /// <summary>
        /// 状态编码
        /// StatusCode 与 Status 一一对应，
        /// </summary>
        public ServiceResponseStatus StatusCode
        {
            get
            {
                if (IsSuccess)
                {
                    return ServiceResponseStatus.OK;
                }
                else
                {
                    // 有设置过特殊错误就返回特殊错误码
                    // 否则返回一般错误Fail
                    if (ServiceResponseStatusHelper.IsSpecialErrorServiceResponseStatus(statusCode))
                    {
                        return statusCode;
                    }

                    return ServiceResponseStatus.Fail;
                }
            }
            protected set
            {
                statusCode = value;
            }
        }

        /// <summary>
        /// 状态字符串（PS：调用方不能用此字符串做比对来写逻辑。必须使用StatusCode来做判断）--因为返回的字符串可能变动，比如大小写，或者英文描述变动
        /// StatusCode 与 Status 一一对应
        /// </summary>
        public string Status
        {
            get
            {
                return StatusCode.GetEnumMemberValue();
            }
        }

        /// <summary>
        /// 是否成功 ， 当 Messages 对象里面包含 Error 类型的消息，则为false
        /// 约定：执行失败，必须给一个error消息
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                var hadError = _messages.Any(x => x.MessageLevel == ServiceResponseMessageLevel.ERROR.GetEnumMemberValue());
                return !hadError;
            }
        }

        /// <summary>
        /// 时间戳(毫秒)
        /// </summary>
        public long Timestamp { get; } = (DateTime.UtcNow.Ticks - 621355968000000000) / 10000;


        /// <summary>
        /// 只读的消息集合
        /// </summary>
        public ReadOnlyCollection<ServiceResponseMessage> Messages
        {
            get
            {
                return _messages.AsReadOnly();
            }
        }

        /// <summary>
        /// 设置返回的特殊错误状态（eg：未登录，未授权，超时等等）
        /// （常规 OK和Fail 状态不用设置，系统通过IsSuccess自行判断）
        /// </summary>
        /// <param name="statusCode"></param>
        public void SetSpecialErrorStatusCode(ServiceResponseStatus statusCode)
        {
            if (statusCode != ServiceResponseStatus.UnKnow)
            {
                StatusCode = statusCode;
            }
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="msg"></param>
        public void AddMessage(ServiceResponseMessage msg)
        {
            _messages.Add(msg);
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="msgList"></param>
        public void AddMessageList(List<ServiceResponseMessage> msgList)
        {
            _messages.AddRange(msgList);
        }
    }
    /// <summary>
    /// 服务层响应实体
    /// 如果要返回失败，则需在 Messages 中加入error错误信息
    /// </summary>
    public class ServiceResponse<T> : ServiceResponse
    {

        public ServiceResponse() : base(Guid.NewGuid().ToString("N"))
        {
        }

        public ServiceResponse(string requestId) : base(requestId)
        {

        }

        private T payload;
        /// <summary>
        /// 数据
        /// 约定：IsSuccess为false的时候，Payload不返回任何值。
        /// </summary>
        public T Payload
        {
            get
            {
                if (IsSuccess)
                {
                    return payload;
                }

                // C# default(T)关键字
                // https://www.cnblogs.com/raincedar/p/13162801.html
                return default(T);
            }
            set
            {
                payload = value;
            }
        }
    }
}
