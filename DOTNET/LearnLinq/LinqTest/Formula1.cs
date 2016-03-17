using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqTest
{
  public class Formula1
  {
    public static IEnumerable<Product> GetProducts()
    {
      List<Product> _list = new List<Product>();
      _list.Add(new Product() { Id = 1, Name = "a", Price = 20 });
      _list.Add(new Product() { Id = 2, Name = "b", Price = 14 });
      _list.Add(new Product() { Id = 3, Name = "c", Price = 9 });
      return _list;
    }
    public static void GetContructorChampions()
    {

    }
  }
}
