using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;


namespace ExpressionDemo
{
    public class Criteria<T> : ICriteria<T>
    {
        #region Fields

        CriteriaProvider provider;
        Dictionary<string, List<Expression>> expressionDictionary;

        #endregion

        #region Constructor

        public Criteria()
        {
            provider = new CriteriaProvider();
            expressionDictionary = new Dictionary<string, List<Expression>>();

        }

        #endregion

        #region interface Methods

        public ICriteria<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return AddExpression("Where", predicate);
        }

        public ICriteria<T> OrderBy<K>(System.Linq.Expressions.Expression<Func<T, K>> predicate)
        {
            return AddExpression("OrderBy", predicate);
        }

        public ICriteria<T> OrderByDescending<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("OrderByDescending", predicate);
        }

        public ICriteria<T> ThenBy<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("ThenBy", predicate);
        }

        public ICriteria<T> ThenByDescending<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("ThenByDescending", predicate);
        }

        public ICriteria<T> Skip(int count)
        {
            ConstantExpression constSkip = Expression.Constant(count, typeof(Int32));
            return AddExpression("Skip", constSkip);
        }

        public ICriteria<T> Take(int count)
        {
            ConstantExpression constSkip = Expression.Constant(count, typeof(Int32));
            return AddExpression("Take", constSkip);
        }

        public ICriteria<T> First()
        {
            throw new NotImplementedException();
        }

        public ICriteria<T> First(Expression<Func<T, bool>> predicate)
        {
            return AddExpression("First", predicate);
        }

        public ICriteria<T> Distinct<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("Distinct", predicate);
        }

        public ICriteria<T> All(Expression<Func<T, bool>> predicate)
        {
            return AddExpression("All", predicate);
        }

        public ICriteria<T> Any(Expression<Func<T, bool>> predicate)
        {
            return AddExpression("Any", predicate);
        }

        public ICriteria<T> GroupBy<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("GroupBy", predicate);
        }

        public ICriteria<T> Max<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("Max", predicate);
        }

        public ICriteria<T> Min<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("Min", predicate);
        }

        public Dictionary<string, List<Expression>> ExpressionDictionary
        {
            get { return this.expressionDictionary; }
        }

        public Type ObjectType
        {
            get
            {
                return typeof(T);
            }

        }

        public ICriteriaProvider Provider
        {
            get
            {
                return this.provider;
            }

        }

        #endregion

        #region Assistant Methods

        private ICriteria<T> AddExpression(string key, Expression predicate)
        {
            List<Expression> expressionList = null;
            if (!(expressionDictionary.TryGetValue(key, out expressionList)))
            {
                lock (expressionDictionary)
                {
                    if (!(expressionDictionary.TryGetValue(key, out expressionList)))
                    {
                        expressionList = new List<Expression>() { predicate };
                        this.expressionDictionary.Add(key, expressionList);
                    }
                }
            }
            else
            {
                lock (expressionDictionary)
                {
                    expressionList = this.expressionDictionary[key];
                    expressionList.Add(predicate);
                    this.expressionDictionary[key] = expressionList;
                }
            }

            return this;
        }

        #endregion

    }
}
