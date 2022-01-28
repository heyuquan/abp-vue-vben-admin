using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 给集合类，添加累加操作
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 累加，若字典中不存在key，则将key，value加入到字典中
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
        /// 累加，若字典中不存在key，则将key，value加入到字典中
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
        /// 累加，若字典中不存在key，则将key，value加入到字典中
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
        /// 根据 keyFunc 将 List 转为 字典
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="list"></param>
        /// <param name="keyFunc"></param>
        /// <returns></returns>
        public static Dictionary<TKey, List<TValue>> AccumulateBy<TKey, TValue>(this List<TValue> list, Func<TValue, TKey> keyFunc) where TKey : IComparable
        {
            if (list == null || list.Count == 0)
            {
                return new Dictionary<TKey, List<TValue>>();
            }

            Dictionary<TKey, List<TValue>> dict = new Dictionary<TKey, List<TValue>>(list.Count, null);

            foreach (var item in list)
            {
                dict.Accumulate(keyFunc(item), item);
            }

            return dict;
        }

        /// <summary>
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
        /// 转为 HashSet 对象（HashSet：包含不重复项的无序列表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            HashSet<T> set = new HashSet<T>();

            if (source != null)
            {
                foreach (var item in source)
                {
                    set.Add(item);
                }
            }

            return set;
        }

        /// <summary>
        /// 转为 SortedSet 对象（SortedSet：包含不重复项的有序列表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static SortedSet<T> ToSortedSet<T>(this IEnumerable<T> source)
        {
            SortedSet<T> set = new SortedSet<T>();

            if (source != null)
            {
                foreach (var item in source)
                {
                    set.Add(item);
                }
            }

            return set;
        }

        /// <summary>
        /// 转为 SortedSet 对象（SortedSet：包含不重复项的有序列表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static SortedSet<T> ToSortedSet<T>(this HashSet<T> source)
        {
            SortedSet<T> set = new SortedSet<T>();

            if (source != null)
            {
                foreach (var item in source)
                {
                    set.Add(item);
                }
            }

            return set;
        }

        /// <summary>
        /// Checks whatever given collection object is null or has no item.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// Adds an item to the collection if it's not already in the collection.
        /// </summary>
        /// <param name="source">Collection</param>
        /// <param name="item">Item to check and add</param>
        /// <typeparam name="T">Type of the items in the collection</typeparam>
        /// <returns>Returns True if added, returns False if not.</returns>
        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Contains(item))
            {
                return false;
            }

            source.Add(item);
            return true;
        }
    }
}
