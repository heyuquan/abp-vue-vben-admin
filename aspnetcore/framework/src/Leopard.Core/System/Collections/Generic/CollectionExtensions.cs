using System.Linq;
using Volo.Abp;

namespace System.Collections.Generic
{
    /// <summary>
    /// 给集合类，添加累加操作
    /// </summary>
    public static class CollectionExtensions
    {
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

        /// <summary>
        /// 加多个项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="initial"></param>
        /// <param name="other"></param>
        public static void AddRange<T>(this ICollection<T> initial, IEnumerable<T> other)
        {
            if (other == null)
                return;

            if (initial is List<T> list)
            {
                list.AddRange(other);
                return;
            }

            foreach (var local in other)
            {
                initial.Add(local);
            }
        }

        /// <summary>
        /// Safe way to remove selected entries from a list.
        /// </summary>
        /// <remarks>To be used for materialized lists only, not IEnumerable or similar.</remarks>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="list">List.</param>
        /// <param name="selector">Selector for the entries to be removed.</param>
        /// <returns>Number of removed entries.</returns>
        public static int Remove<T>(this IList<T> list, Func<T, bool> selector)
        {
            Check.NotNull(list, nameof(list));
            Check.NotNull(selector, nameof(selector));

            var count = 0;
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (selector(list[i]))
                {
                    list.RemoveAt(i);
                    ++count;
                }
            }

            return count;
        }
    }
}
