using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWithOracle.Model.Discount;
using WPFWithOracle.Model.Products;

namespace WPFWithOracle.Repository.Products
{
  /// <summary>
  /// add discount info to product
  /// </summary>
  public static class ProductListExtensionMethods
  {
    public static void Apply(this IList<Product> products, IDiscountStrategy discountStrategy)
    {
      foreach (Product p in products)
      {
        p.Price.SetDiscountStrategyTo(discountStrategy);
      }
    }
  }
}
