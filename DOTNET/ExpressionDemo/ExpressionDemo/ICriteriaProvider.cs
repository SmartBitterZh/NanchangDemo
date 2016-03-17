using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ExpressionDemo
{
    public interface ICriteriaProvider
    {       
        object Execute<T>(ICriteria<T> condition);
    }
}
