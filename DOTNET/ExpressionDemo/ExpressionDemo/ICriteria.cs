using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ExpressionDemo
{
    public interface ICriteria
    {
        Type ObjectType { get; }
        Dictionary<string, List<Expression>> ExpressionDictionary { get; }
        ICriteriaProvider Provider { get; }
    }

    public interface ICriteria<T> : ICriteria
    {

        ICriteria<T> Where(Expression<Func<T, bool>> predicate);

        ICriteria<T> OrderBy<K>(Expression<Func<T, K>> predicate);
        ICriteria<T> OrderByDescending<K>(Expression<Func<T, K>> predicate);

        ICriteria<T> ThenBy<K>(Expression<Func<T, K>> predicate);
        ICriteria<T> ThenByDescending<K>(Expression<Func<T, K>> predicate);

        ICriteria<T> Skip(int count);
        ICriteria<T> Take(int count);

        ICriteria<T> First();
        ICriteria<T> First(Expression<Func<T, bool>> predicate);

        ICriteria<T> Distinct<K>(Expression<Func<T, K>> predicate);
        ICriteria<T> All(Expression<Func<T, bool>> predicate);
        ICriteria<T> Any(Expression<Func<T, bool>> predicate);

        ICriteria<T> GroupBy<K>(Expression<Func<T, K>> predicate);

        ICriteria<T> Max<K>(Expression<Func<T, K>> predicate);
        ICriteria<T> Min<K>(Expression<Func<T, K>> predicate);
    }
}
