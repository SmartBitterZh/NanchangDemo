using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySocketServer2
{
  [Serializable]
  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal RecommendedRetailPrice { get; set; }
    public decimal SellingPrice { get; set; }
  }
}
