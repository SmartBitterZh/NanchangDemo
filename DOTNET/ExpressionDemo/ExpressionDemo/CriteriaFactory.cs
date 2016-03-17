using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ExpressionDemo
{
    public class CriteriaFactory
    {
        public static Criteria<T> Create<T>()
        {
            return new Criteria<T>();
        }
    }
}
