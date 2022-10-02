using Leopard;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace System
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    internal static class EnumExtensions
    {
        /// <summary>
        /// 获取 [EnumMemberAttribute] 的value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetEnumMemberValue<T>(this T value, string defaultValue = Constants.String_Empty) where T : Enum
        {
            string result = typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString())
                ?.GetCustomAttribute<EnumMemberAttribute>(false)
                ?.Value;

            return String.IsNullOrEmpty(result) ? defaultValue : result;
        }
    }
}
