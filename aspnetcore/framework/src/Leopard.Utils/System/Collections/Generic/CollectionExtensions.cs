using Leopard.Utils;
using System.Linq;

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
            Checked.NotNull(list, nameof(list));
            Checked.NotNull(selector, nameof(selector));

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

        #region 为集合的ForEach方法添加break功能

        // break和continue是需要编译器支持的C＃语言关键字。 ForEach，对C＃编译器来说，只是一种方法

        // 【continue】在ForEach中 return 即可实现continue
        //var list = new List<int>() { 1, 2, 3, 4 };
        //list.ForEach(i => 
        //    {
        //        if (i == 3)
        //            return;
        //        Console.WriteLine(i);
        //    }
        //);
        // 打印1,2,4。 3 - 跳过。


        // break示例
        // 参考：https://stackoverflow.com/questions/3145563/list-foreach-break
        // Enumerable.Range(0,10)
        //    .ForEach((int i, ref bool doBreak) => {    // (int i, ref bool doBreak) 全部显示指定类型，这样不用单独再定义 doBreak 变量
        //        System.Windows.MessageBox.Show(i.ToString());
        //        if (i > 2) { doBreak = true; }
        //    });

        public delegate void ForEachAction<T>(T value, ref bool doBreak);
        /// <summary>
        /// 为集合的ForEach方法添加break功能
        /// 【continue】在ForEach中 return 即可实现continue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, ForEachAction<T> action)
        {
            var doBreak = false;
            foreach (var cur in enumerable)
            {
                action(cur, ref doBreak);
                if (doBreak)
                {
                    break;
                }
            }
        }

        #endregion

        /// <summary>
        /// 将集合按指定大小拆分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="groupSize">默认1000</param>
        /// <returns></returns>
        public static List<List<T>> Split<T>(this List<T> list, int groupSize = 1000)
        {
            List<List<T>> group = new List<List<T>>();
            for (int i = 0; i < list.Count; i += groupSize)
            {
                group.Add(list.Skip(i).Take(groupSize).ToList());
            }

            return group;
        }
    }
}
