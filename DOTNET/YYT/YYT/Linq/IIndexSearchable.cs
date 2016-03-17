using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YYT.Linq
{
  internal interface IIndexSearchable<T>
  {
    IEnumerable<T> SearchByExpression(Expression<Func<T, bool>> expr);
  }
}
