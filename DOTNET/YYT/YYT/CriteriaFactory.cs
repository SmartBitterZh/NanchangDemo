using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YYT
{
    /// <summary>
    /// create data operation condition
    /// </summary>
    public class CriteriaFactory
    {
        public static Criteria<T> Create<T>()
        {
            return new Criteria<T>();
        }
    }
}
