using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoB.SerializerAppService
{
    // 程序集：Volo.Abp.Json，接口：IJsonSerializer  abp对接了Newtonsoft
    // 程序集：Volo.Abp.Serialization， 接口：IObjectSerializer  二进制序列化  可以使用IObjectSerializer<T>来做定制化序列化
    public class SerializerAppService : DemoBAppService
    {
        public SerializerAppService()
        {
            
        }


    }
}
