using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YYT;

namespace YYTTest
{
    public class Person : BusinessBase<Person>
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
