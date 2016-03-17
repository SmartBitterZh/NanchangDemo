using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;

namespace YYT.Query
{
    /// <summary>
    /// 所有的查询对象都要从这个接口继承
    /// </summary>
    public interface ICriteria
    {
        #region Property

        Type ObjectType { get; }
        string Name { get; set; }
        bool IsCache { get; set; }
        bool IsTransaction { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int Count { get; set; }
        ICriteriaProvider Provider { get; }

        #endregion

    }

    /// <summary>
    /// 泛型版的条件对象
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface ICriteria<T> : ICriteria
    {
        Dictionary<string, List<Expression>> ExpressionDictionary { get; }

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
