using Leopard;
using Leopard.Utils;
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
        public static string GetDescription(this Enum @enum, string defaultValue = Constants.String_Empty)
        {
            Type type = @enum.GetType();
            MemberInfo member = type.GetMember(@enum.ToString()).FirstOrDefault();
            string result = member?.GetDescription();

            return result.IsNullOrEmpty2() ? defaultValue : result;
        }

        /// <summary>
        /// 获取 [EnumMemberAttribute] 的value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enum"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetEnumMemberValue<T>(this T @enum, string defaultValue = Constants.String_Empty) where T : Enum
        {
            string? result = typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == @enum.ToString())
                ?.GetCustomAttribute<EnumMemberAttribute>(false)
                ?.Value;

            return result.IsNullOrEmpty2() ? defaultValue : result;
        }

        #region 枚举 特性使用 小技巧

        // 使用 案例
        // public enum MeliSite
        // {
        //     [MeliSiteId("MLA")]
        //     [MeliCountryCode("AR")]
        //     [MeliDomain("mercadolibre.com.ar")]
        //     Argentina,
        // }
        // public class MeliCountryCodeAttribute : BaseValueAttribute
        // MeliSite.Argentina.GetValue<MeliCountryCodeAttribute>()

        // 可以为枚举定义扩展方法
        // public static class MeliSiteExtensions
        // {
        //     public static string ToSiteId(this MeliSite @enum)
        //     {
        //         return @enum.GetValue<MeliSiteIdAttribute>();
        //     }
        // }

        /// <summary>
        /// Gets the attribute from enum constant.
        /// </summary>
        /// <typeparam name="T">The type of attribute to obtain.</typeparam>
        /// <param name="enum">The enumeration value.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetAttributesFromEnumConstant<T>(this Enum @enum) where T : Attribute
        {
            var type = @enum.GetType();

            var memberInfo = type.GetMember(@enum.ToString());

            if (memberInfo.Length == 0) yield break;

            var attrs = memberInfo[0].GetCustomAttributes(typeof(T), true);

            foreach (var attr in attrs)
            {
                yield return (T)attr;
            }
        }

        /// <summary>
        /// Gets the value of the first relevant attribute on the given enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enum">The enum.</param>
        /// <returns></returns>
        public static string GetValue<T>(this Enum @enum) where T : BaseValueAttribute
        {
            return GetAttributesFromEnumConstant<T>(@enum)
                             .First()
                             .Value;
        }
        #endregion

        /// <summary>
        /// 把字符串转为枚举值
        /// </summary>
        [DebuggerStepThrough]
        public static T ToEnum<T>(this string @enum, T defaultValue, bool isIgnoreCase = true)
            where T : Enum
        {
            if (!@enum.HasValue())
            {
                return defaultValue;
            }
            try
            {
                string temp = isIgnoreCase ? @enum.ToLower().ToUpperHead() : @enum;
                return (T)Enum.Parse(typeof(T), @enum, true);
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取枚举定义值的字符串列表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetList(this Type enumType)
        {
            return enumType.GetFields().Where(f => f.FieldType.IsEnum).Select(o => o.Name);
        }

        #region 枚举成员转成dictionary类型        

        /// <summary>
        /// 转成dictionary类型  [字段字符串,EnumMemberAttribute的Value]
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string, string> EnumMemberMap(this Type enumType)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Type typeEnumMember = typeof(EnumMemberAttribute);
            FieldInfo[] fields = enumType.GetFields();
            string sText = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    object[] arr = field.GetCustomAttributes(typeEnumMember, true);
                    if (arr.Length > 0)
                    {
                        EnumMemberAttribute da = (EnumMemberAttribute)arr[0];
                        sText = da.Value ?? field.Name;
                    }
                    else
                    {
                        sText = field.Name;
                    }
                    dictionary.Add(field.Name, sText);
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
            List<KeyValuePair<string, string>> dictionaryList = EnumMemberMap(enumType).ToList();
            var sJson = dictionaryList.ToJson();
            return sJson;
        }
        #endregion
    }
}
