using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp;
using Volo.Abp.Http;

namespace Leopard.Requests
{
    // to do 
    // 保留 IsSuccess 字段，用于常规判断调用是否成功。成功即Payload里面会返回接口数据。   失败errors里面会包含错误信息
    // 增加 string:Status  eg：ok，fail  前端可以根据一些状态string，做一些定制化的事情
    // eg:Enum: "OK" "CREATED" "ACCEPTED" "NO_CONTENT" "PARTIAL" "MOVED_PERMANENT" "FOUND" "SEE_OTHER" "NOT_MODIFIED" "TEMPORARY_REDIRECT" "BAD_REQUEST" "UNAUTHORIZED" "FORBIDDEN" "NOT_FOUND" "METHOD_NOT_ALLOWED" "NOT_ACCEPTABLE" "REQUEST_TIMEOUT" "CONFLICT" "REQUEST_ENTITY_TOO_LARGE" "UNSUPPORTED_MEDIA_TYPE" "UNPROCESSABLE_ENTITY" "FAIL" "BAD_GATEWAY" "SERVICE_UNAVAILABLE" "GATEWAY_TIMEOUT"
    // 因为 string:Status 可以把状态表达的更清楚。   SetFailed  增加枚举参数。啥类型的错误。eg：normal->fail ，认证错误啥的
    // 参考 https://developer.walmart.com/api/us/mp/fulfillment#operation/printCarrierLabel 设计
    // 额外增加 errors[] 字段，因为可能有些接口调用成功，但是也想返回 info 类型的数据到前端
    // Data 重命名为  Payload （有效负载） 

    // 新增StatusCode
    /*
现在前端请求RestAPI后获得的errorcode，HBS, ABS, FBS, CRM都是没有统一的规范，导致可能例如errocode=200的情况在某系统RestAPI报错是一种不要处理普通错误，而在另一个系统RestAPI报错则是一种需要处理特殊错误的情况存在。为解决这个问题，经部门会议讨论，用下面方法来规范RestAPI中errorcode的定义。
如果接口成功返回，那么errorcode定义为0。
如果接口错误，则errorcode返回9位的错误码，规则分别是：1位接口状态码 + 2位业务系统编码 + 2位子系统编码 + 4位错误码。
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
