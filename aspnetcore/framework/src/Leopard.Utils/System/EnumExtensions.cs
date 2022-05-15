using Leopard;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

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
        /// <returns>找不到 value 对应的enum，则返回String.Empty</returns>
        public static string GetDescription(this Enum value, string defaultValue = Constants.String_Empty)
        {
            Type type = value.GetType();
            MemberInfo member = type.GetMember(value.ToString()).FirstOrDefault();
            string result = member?.GetDescription();

            return result.IsNullOrEmpty2() ? defaultValue : result;
        }

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

            return result.IsNullOrEmpty2() ? defaultValue : result;
        }

        /// <summary>
        /// 把字符串转为枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T ToEnum<T>(this string value, T defaultValue)
        {
            if (!value.HasValue())
            {
                return defaultValue;
            }
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
        }

        #region 枚举成员转成dictionary类型
        /// <summary>
        /// 转成dictionary类型
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<int, string> EnumToDictionary(this Type enumType)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            Type typeDescription = typeof(DescriptionAttribute);
            FieldInfo[] fields = enumType.GetFields();
            int sValue = 0;
            string sText = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    sValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null));
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute da = (DescriptionAttribute)arr[0];
                        sText = da.Description;
                    }
                    else
                    {
                        sText = field.Name;
                    }
                    dictionary.Add(sValue, sText);
                }
            }
            return dictionary;
        }
        /// <summary>
        /// 枚举成员转成键值对Json字符串
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string EnumToDictionaryString(this Type enumType)
        {
            List<KeyValuePair<int, string>> dictionaryList = EnumToDictionary(enumType).ToList();
            var sJson = dictionaryList.ToJson();
            return sJson;
        }
        #endregion
    }
}
