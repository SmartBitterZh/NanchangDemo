using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DBControl
{
  //[KnownType(typeof(Product))]

  [Serializable]
  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal RecommendedRetailPrice { get; set; }
    public decimal SellingPrice { get; set; }
  }
}
