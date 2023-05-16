using System;

namespace Leopard.Helper
{

    // snowflake是Twitter开源的分布式ID生成算法，结果是一个long型的ID
    // 原理：https://www.jianshu.com/p/b7a3f0fdd717
    // 所有位数加起来共64位，恰好是一个Long型（转换为字符串长度为18）
    // eg:412992501465481216
    // C#：https://github.com/RobThree/IdGen

    /// <summary>
    /// ID 生成器
    /// </summary>
    public static class IdHelper
    {
        /// <summary>
        /// 获取有序的GUID
        /// </summary>
        /// <remarks>
        /// The comb algorithm is designed to make the use of GUIDs as Primary Keys, Foreign Keys, 
        /// and Indexes nearly as efficient as ints.
        /// </remarks>
        public static Guid NewSequentialGuid()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

        /// <summary>
        /// 生成有序Guid string
        /// </summary>
        /// <param name="hasDash">是否需要分隔符"-"</param>
        /// <returns></returns>
        public static string NewSequentialGuidStr(bool hasDash = true)
        {
            Guid guid = NewSequentialGuid();
            return hasDash ? guid.ToString() : guid.ToString("N");
        }

        /// <summary>
        /// 使用 Guid.NewGuid() 获取一个 Guid string
        /// </summary>
        /// <param name="hasDash">是否需要分隔符"-"</param>
        /// <returns></returns>
        public static string NewGuidStr(bool hasDash = true)
        {
            return hasDash ? Guid.NewGuid().ToString() : Guid.NewGuid().ToString("N");
        }
    }
}
