using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// 字典扩展类
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 对key进行累加，若字典中不存在key，则将key，value加入到字典中
        /// </summary>
        public static void Accumulate<TKey>(this Dictionary<TKey, int> dict, TKey key, int value) where TKey : IComparable
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        /// <summary>
        /// 对key进行累加，若字典中不存在key，则将key，value加入到字典中
        /// </summary>
        public static void Accumulate<TKey>(this Dictionary<TKey, long> dict, TKey key, long value) where TKey : IComparable
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        /// <summary>
        /// 对key进行累加，若字典中不存在key，则将key，value加入到字典中
        /// </summary>
        public static void Accumulate<TKey>(this Dictionary<TKey, decimal> dict, TKey key, decimal value) where TKey : IComparable
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        /// <summary>
        /// 对key进行累加
        /// 如果词典中含有key，则将对象加到 HashSet 中，
        /// 如果不含key，创建 HashSet，加入key， HashSet 到词典中.
        /// </summary>
        public static void Accumulate<TKey, TValue>(this Dictionary<TKey, HashSet<TValue>> dict, TKey key, TValue obj) where TKey : IComparable
        {
            HashSet<TValue> list;
            if (dict.TryGetValue(key, out list))
            {
                list.Add(obj);
            }
            else
            {
                dict.Add(key, new HashSet<TValue>() { obj });
            }
        }

        /// <summary>
        /// 对key进行累加
        /// 如果词典中含有key，则将对象加到 HashSet 中，
        /// 如果不含key，创建 HashSet，加入key， HashSet 到词典中.
        /// </summary>
        public static void Accumulate<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict, TKey key, TValue obj) where TKey : IComparable
        {
            List<TValue> list;
            if (dict.TryGetValue(key, out list))
            {
                list.Add(obj);
            }
            else
            {
                dict.Add(key, new List<TValue>() { obj });
            }
        }

        /// <summary>
        /// 对key进行累加
        /// 往集合里添加键值对，如果已经存在，则覆盖.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void TryAddWithReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value) where TKey : IComparable
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        /// <summary>
        /// 将对象转成字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object input)
        {
            if (input == null) return default;

            if (input is IDictionary<string, object> dictionary)
                return dictionary;

            if (input is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
            {
                return jsonElement.ToObject() as IDictionary<string, object>;
            }

            var properties = input.GetType().GetProperties();
            var fields = input.GetType().GetFields();
            var members = properties.Cast<MemberInfo>().Concat(fields.Cast<MemberInfo>());

            return members.ToDictionary(m => m.Name, m => GetValue(input, m));
        }


        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private static object GetValue(object obj, MemberInfo member)
        {
            if (member is PropertyInfo info)
                return info.GetValue(obj, null);

            if (member is FieldInfo info1)
                return info1.GetValue(obj);

            throw new ArgumentException("Passed member is neither a PropertyInfo nor a FieldInfo.");
        }

        /// <summary>
        /// 合并两个字典。相同键的使用srcDic的值覆盖destDic的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destDic">目的字典</param>
        /// <param name="srcDic">来源字典</param>
        /// <returns></returns>
        public static Dictionary<string, T> AddOrUpdate<T>(this Dictionary<string, T> destDic, IDictionary<string, T> srcDic)
        {
            foreach (var key in srcDic.Keys)
            {
                if (destDic.ContainsKey(key))
                    destDic[key] = srcDic[key];
                else
                    destDic.Add(key, srcDic[key]);
            }

            return destDic;
        }
    }
}
