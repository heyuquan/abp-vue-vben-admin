using Dasync.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Leopard.Utils;

namespace System.Collections.Generic
{
    /// <summary>
    /// Enumerable集合扩展
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Nested classes

        private static class DefaultReadOnlyCollection<T>
        {
            private static ReadOnlyCollection<T> defaultCollection;

            internal static ReadOnlyCollection<T> Empty
            {
                get
                {
                    if (defaultCollection == null)
                    {
                        defaultCollection = new ReadOnlyCollection<T>(new T[0]);
                    }
                    return defaultCollection;
                }
            }
        }

        #endregion

        /// <summary>
        /// 根据第三方条件是否为真来决定是否 附加 指定查询条件
        /// </summary>
        /// <param name="source"> 要查询的源 </param>
        /// <param name="condition"> 第三方条件 </param>
        /// <param name="predicate"> 查询条件 </param>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <returns> 查询的结果 </returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            Checked.NotNull(predicate, "predicate");
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
            return source.Random(random);
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
            Checked.NotNull(itemFormatFunc, "itemFormatFunc");
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
        /// Checks whatever given collection object is null or has no item.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source.IsNull() || !source.Any();
        }

        /// <summary>
        /// 判断IEnumerable是否有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool HasItems<T>(this IEnumerable<T> values)
        {
            return !values.IsNullOrEmpty();
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
            Checked.NotNull(keySelector, "keySelector");
            source = source as IList<T> ?? source.ToList();

            return source.GroupBy(keySelector).Select(group => group.First());
        }

        /// <summary>
        /// Performs an action on each item while iterating through a list. 
        /// This is a handy shortcut for <c>foreach(item in list) { ... }</c>
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="source">The list, which holds the objects.</param>
        /// <param name="action">The action delegate which is called on each item while iterating.</param>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source is List<T> list)
            {
                list.ForEach(action);
                return;
            }

            foreach (T t in source)
            {
                action(t);
            }
        }

        /// <summary>
        /// Performs an action on each item while iterating through a list. 
        /// This is a handy shortcut for <c>foreach(item in list) { ... }</c>
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="source">The list, which holds the objects.</param>
        /// <param name="action">The action delegate which is called on each item while iterating.</param>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Each<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int i = 0;
            foreach (T t in source)
            {
                action(t, i++);
            }
        }

        public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> source)
        {
            if (source == null || !source.Any())
                return DefaultReadOnlyCollection<T>.Empty;

            if (source is ReadOnlyCollection<T> readOnly)
            {
                return readOnly;
            }
            else if (source is List<T> list)
            {
                return list.AsReadOnly();
            }

            return new ReadOnlyCollection<T>(source.ToList());
        }

        #region Async

        /// <summary>
        /// Performs an action on each item while iterating through a list. 
        /// This is a handy shortcut for <c>foreach(item in list) { await ... }</c>
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="source">The list, which holds the objects.</param>
        /// <param name="action">The action delegate which is called on each item while iterating.</param>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task EachAsync<T>(this IEnumerable<T> source, Func<T, Task> action)
        {
            foreach (T t in source)
            {
                await action(t).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Performs an action on each item while iterating through a list. 
        /// This is a handy shortcut for <c>foreach(item in list) { ... }</c>
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="source">The list, which holds the objects.</param>
        /// <param name="action">The action delegate which is called on each item while iterating.</param>
        public static async Task EachAsync<T>(this IEnumerable<T> source, Func<T, int, Task> action)
        {
            int i = 0;
            foreach (T t in source)
            {
                await action(t, i++);
            }
        }

        /// <summary>
        /// Filters a sequence of values based on an async predicate.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence to filter.</param>
        /// <param name="predicate">An async task function to test each element for a condition.</param>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> that contains elements from the input sequence that satisfy the condition.</returns>
        public static async IAsyncEnumerable<T> WhereAsync<T>(this IEnumerable<T> source, Func<T, Task<bool>> predicate)
        {
            await foreach (var item in source.ToAsyncEnumerable())
            {
                if (await predicate(item))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Projects each element of a sequence into a new form in parallel.
        /// </summary>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element.</param>
        public static async Task<IEnumerable<TResult>> SelectAsyncParallel<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            return await Task.WhenAll(source.Select(async x => await selector(x)));
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element.</param>
        public static async IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            await foreach (var item in source.ToAsyncEnumerable())
            {
                yield return await selector(item);
            }
        }

        /// <summary>
        /// Awaits all tasks in a sequence to complete.
        /// </summary>
        public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> source)
        {
            return await Task.WhenAll(source);
        }

        /// <summary>
        /// Determines whether any element of a sequence satisfies a condition. 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The source sequence whose elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public static async Task<bool> AnyAsync<T>(this IEnumerable<T> source, Func<T, Task<bool>> predicate)
        {
            foreach (T t in source)
            {
                if (await predicate(t))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
