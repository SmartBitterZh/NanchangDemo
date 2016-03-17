using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFWithOracle.Model.Products;

namespace WPFWithOracle.Model.Orders
{
  public class Order
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int CustomerId { get; set; }
    public IList<Product> Products { get; set; }
  }
}
