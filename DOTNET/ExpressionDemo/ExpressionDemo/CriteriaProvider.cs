using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionDemo
{
    public class CriteriaProvider : ICriteriaProvider
    {
        public object Execute<T>(ICriteria<T> condition)
        {
            CriteriaTranslator translator = new CriteriaTranslator(typeof(T));
            return translator.Translate(condition.ExpressionDictionary);
        }
    }
}
