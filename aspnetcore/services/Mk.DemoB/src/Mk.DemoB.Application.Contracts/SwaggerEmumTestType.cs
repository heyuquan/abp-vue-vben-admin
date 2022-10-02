using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mk.DemoB
{
    /// <summary>
    /// swagger枚举注释测试类型
    /// </summary>
    public enum SwaggerEmumTestType:int
    {
        [Description("测试类型1")]
        test1 = 1,
        [Description("测试类型2")]
        test2 = 2
    }
}
