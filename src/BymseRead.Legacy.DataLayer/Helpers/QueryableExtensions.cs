using System.Linq.Expressions;

namespace BymseRead.Legacy.DataLayer.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> If<T>(this IQueryable<T> source,
            bool condition,
            Func<IQueryable<T>, IQueryable<T>> @if,
            Func<IQueryable<T>, IQueryable<T>>? @else = null)
        {
            Func<IQueryable<T>, IQueryable<T>> doElse = @else ?? (e => e);
            return condition ? @if(source) : doElse(source);
        }

        public static IQueryable<TSrc> OrderBy<TSrc, TKey>(this IQueryable<TSrc> source,
            Expression<Func<TSrc, TKey>> expression,
            bool descending)
        {
            return source.If(descending,
                e => e.OrderByDescending(expression),
                e => e.OrderBy(expression));
        }
    }
}