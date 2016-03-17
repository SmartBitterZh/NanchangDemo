using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqTest
{
  class Program
  {
    static void Main(string[] args)
    {
      var product = from p in Formula1.GetProducts()
                  where p.Price > 15 && p.Name == "a"
                  select p;

      Console.WriteLine(product);

      Console.ReadKey();
    }
  }
}
