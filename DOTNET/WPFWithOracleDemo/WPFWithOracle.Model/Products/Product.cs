using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Model.Products
{
  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public Price Price { get; set; }
  }
}
