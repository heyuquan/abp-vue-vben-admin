using System;
using System.Linq;
using System.Reflection;

namespace System
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举类型的描述 [DescriptionAttribute]
        /// </summary>
        /// <returns></returns>
        public static string GetDescription(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            MemberInfo member = type.GetMember(enumeration.ToString()).FirstOrDefault();
            return member != null ? member.GetDescription() : enumeration.ToString();
        }
    }
}
