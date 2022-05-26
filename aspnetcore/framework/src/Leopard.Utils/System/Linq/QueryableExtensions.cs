using JetBrains.Annotations;
using Leopard.Utils;
using System.Linq.Expressions;

// 因为结合Abp框架使用，而这个类又是从Volo.Abp.Core/System/Linq下拷贝过来，重名多，所以命名空间改为 System.Linq.Ex
namespace System.Linq.Ex
{
    /// <summary>
    /// IQueryable集合扩展方法
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// </summary>
        public static IQueryable<T> PageBy<T>([NotNull] this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            Checked.NotNull(query, nameof(query));

            return query.Skip(skipCount).Take(maxResultCount);
        }

        /// <summary>
        /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// </summary>
        public static TQueryable PageBy<T, TQueryable>([NotNull] this TQueryable query, int skipCount, int maxResultCount)
            where TQueryable : IQueryable<T>
        {
            Checked.NotNull(query, nameof(query));

            return (TQueryable)query.Skip(skipCount).Take(maxResultCount);
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            Checked.NotNull(query, nameof(query));

            return condition
                ? query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static TQueryable WhereIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, Expression<Func<T, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            Checked.NotNull(query, nameof(query));

            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            Checked.NotNull(query, nameof(query));

            return condition
                ? query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static TQueryable WhereIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, Expression<Func<T, int, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            Checked.NotNull(query, nameof(query));

            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }
    }
}
