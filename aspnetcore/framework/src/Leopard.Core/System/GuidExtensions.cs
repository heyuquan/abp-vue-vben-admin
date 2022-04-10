using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class GuidExtensions
    {

        /// <summary>  
        /// 根据GUID获取16位的唯一字符串  
        /// </summary>  
        /// <returns></returns>  
        public static string To16String(this Guid guid)
        {
            long i = 1;
            foreach (var b in guid.ToByteArray())
            {
                i *= b + 1;
            }

            return $"{i - DateTime.Now.Ticks:x}";
        }

        /// <summary>  
        /// 根据GUID获取19位的唯一数字序列  
        /// </summary>  
        /// <returns></returns>  
        public static long ToLongID(this Guid guid)
        {
            var buffer = guid.ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
