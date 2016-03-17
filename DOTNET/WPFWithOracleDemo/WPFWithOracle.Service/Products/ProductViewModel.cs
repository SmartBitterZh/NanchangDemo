using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Service.Products
{
  public class ProductViewModel
  {
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string RecommendedRetailPrice { get; set; }
    public string SellingPrice { get; set; }
    public string Discount { get; set; }
    public string Savings { get; set; }
  }
}
