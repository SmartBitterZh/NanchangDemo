using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Model.Discount
{
  public interface IDiscountStrategy
  {
    decimal ApplyExtraDiscountsTo(decimal OriginalSalePrice, decimal discountPercent);
  }
}
