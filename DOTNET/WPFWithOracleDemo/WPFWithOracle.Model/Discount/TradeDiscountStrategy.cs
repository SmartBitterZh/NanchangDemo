using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Model.Discount
{
  public sealed class TradeDiscountStrategy : IDiscountStrategy
  {
    public decimal ApplyExtraDiscountsTo(decimal OriginalSalePrice, decimal discountPercent)
    {
      decimal price = OriginalSalePrice;
      //price = price * 0.6M;  
      price = price * discountPercent;
      return price;
    }
  }
}
