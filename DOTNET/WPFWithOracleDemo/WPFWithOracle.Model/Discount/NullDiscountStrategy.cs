using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Model.Discount
{
  public sealed class NullDiscountStrategy : IDiscountStrategy
  {
    public decimal ApplyExtraDiscountsTo(decimal OriginalSalePrice, decimal discountPercent)
    {
      return OriginalSalePrice;
    }
  }
}
