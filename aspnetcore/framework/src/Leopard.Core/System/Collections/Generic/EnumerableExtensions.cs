using System.Linq;
using System.Text;
using Volo.Abp;

namespace System.Collections.Generic
{
    /// <summary>
    /// Enumerable集合扩展
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 根据第三方条件是否为真来决定是否 附加 指定查询条件
        /// </summary>
        /// <param name="source"> 要查询的源 </param>
        /// <param name="predicate"> 查询条件 </param>
        /// <param name="condition"> 第三方条件 </param>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <returns> 查询的结果 </returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            Check.NotNull(predicate, "predicate");
            source = source as IList<T> ?? source.ToList();

            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 随机取IEnumerable中的一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="source">IEnumerable</param>
        /// <returns></returns>
        public static T Random<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            Random random = new Random();
            if (source is ICollection)
            {
                ICollection collection = source as ICollection;
                int count = collection.Count;
                if (count == 0)
                {
                    throw new Exception("IEnumerable没有数据");
                }
                int index = random.Next(count);
                return source.ElementAt(index);
            }
            using (IEnumerator<T> iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    throw new Exception("IEnumerable没有数据");
                }
                int count = 1;
                T current = iterator.Current;
                while (iterator.MoveNext())
                {
                    count++;
                    if (random.Next(count) == 0)
                    {
                        current = iterator.Current;
                    }
                }
                return current;
            }
        }

        /// <summary>
        /// 随机取IEnumerable中的一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="source">IEnumerable</param>
        /// <param name="random">随机对象</param>
        /// <returns></returns>
        public static T Random<T>(this IEnumerable<T> source, Random random)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }
            if (source is ICollection)
            {
                ICollection collection = source as ICollection;
                int count = collection.Count;
                if (count == 0)
                {
                    throw new Exception("IEnumerable没有数据");
                }
                int index = random.Next(count);
                return source.ElementAt(index);
            }
            using (IEnumerator<T> iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    throw new Exception("IEnumerable没有数据");
                }
                int count = 1;
                T current = iterator.Current;
                while (iterator.MoveNext())
                {
                    count++;
                    if (random.Next(count) == 0)
                    {
                        current = iterator.Current;
                    }
                }
                return current;
            }
        }

        /// <summary>
        /// IEnumerable集合中加入分隔符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="separator">分隔符默认 ,</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> values, string separator = ",")
        {
            if (values == null || !values.Any())
            {
                return string.Empty;
            }
            if (separator.IsNull())
            {
                separator = string.Empty;
            }
            return string.Join(separator, values);
        }

        /// <summary>
        /// 循环集合的每一项，调用委托生成字符串，返回合并后的字符串。默认分隔符为逗号
        /// </summary>
        /// <param name="collection">待处理的集合</param>
        /// <param name="itemFormatFunc">单个集合项的转换委托</param>
        /// <param name="separator">分隔符，默认为逗号</param>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> collection, Func<T, string> itemFormatFunc, string separator = ",")
        {
            collection = collection as IList<T> ?? collection.ToList();
            Check.NotNull(itemFormatFunc, "itemFormatFunc");
            if (!collection.Any())
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            int i = 0;
            int count = collection.Count();
            foreach (T t in collection)
            {
                if (i == count - 1)
                {
                    sb.Append(itemFormatFunc(t));
                }
                else
                {
                    sb.Append(itemFormatFunc(t) + separator);
                }
                i++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 判断IEnumerable是否有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool HasItems<T>(this IEnumerable<T> values)
        {
            return values.IsNotNull() && values.Any();
        }

        /// <summary>
        /// 打乱一个集合的项顺序
        /// </summary>
        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            return source.OrderBy(m => Guid.NewGuid());
        }

        /// <summary>
        /// 根据指定条件返回集合中不重复的元素
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <typeparam name="TKey">动态筛选条件类型</typeparam>
        /// <param name="source">要操作的源</param>
        /// <param name="keySelector">重复数据筛选条件</param>
        /// <returns>不重复元素的集合</returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            Check.NotNull(keySelector, "keySelector");
            source = source as IList<T> ?? source.ToList();

            return source.GroupBy(keySelector).Select(group => group.First());
        }

    }
}
