using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// System.Convert 扩展类
    /// System.Convert 提供的转换方法：
    ///     会先判断输入参数是否为null，为null返回目的类型的默认值；
    ///     不为null会调用目的类型的tryParse方法，转失败了也返回目的类型的默认值
    /// </summary>
    public static class ConvertExtensions
    {
        // eg：TryTo***WithDefault   转失败了，可以给默认值
    }
}
