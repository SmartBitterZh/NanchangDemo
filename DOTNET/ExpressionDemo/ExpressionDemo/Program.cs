using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ExpressionDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            User p = new User();
            p.Age = 3;
            CriteriaProvider provider = new CriteriaProvider();
            //ICriteria<Person> condition =
            //    new Criteria<Person>().Where(u => u.Name.Contains("a") && u.Age > p.Age || u.Age < 7).OrderBy<string>(u => u.Name).Distinct<string>(u => u.Name).Skip(2);
            //  provider.Execute(condition);
            ICriteria<User> conditionPerson =
                CriteriaFactory.Create<User>().Where(u => u.Age < 7).OrderBy<string>(u => u.Name).Skip(2).Take(10);
            provider.Execute(conditionPerson);
        }
    }
}
