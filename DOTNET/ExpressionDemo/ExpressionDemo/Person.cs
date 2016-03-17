using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionDemo
{
    public class User : BusinessBase
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public List<User> GetUserByAge()
        {
            ICriteria<User> conditionPerson =
               CriteriaFactory.Create<User>().Where(u => u.Age < this.Age).OrderBy<string>(u => u.Name).Skip(8).Take(8);
            return DataPortal.Query(conditionPerson);
        }
    }
}
