using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace YYT.Query
{
    public interface ICriteriaProvider
    {       
        object Execute<T>(ICriteria<T> condition);
    }
}
