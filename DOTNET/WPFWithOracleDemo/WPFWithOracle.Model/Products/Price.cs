using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFWithOracle.Model.Discount;

namespace WPFWithOracle.Model.Products
{
  public class Price
  {
    private IDiscountStrategy _discountStrategy = new NullDiscountStrategy();
    private decimal _recommendedRetailPrice;
    private decimal _sellingPrice;
    public Price(decimal recommendedRetailPrice, decimal SellingPrice)
    {
      _recommendedRetailPrice = recommendedRetailPrice;
      _sellingPrice = SellingPrice;
    }

    public void SetDiscountStrategyTo(IDiscountStrategy DiscountStrategy)
    {
      _discountStrategy = DiscountStrategy;
    }

    public decimal SellingPrice
    {
      get { return _discountStrategy.ApplyExtraDiscountsTo(_sellingPrice, 0.6M); }
    }

    public decimal RecommendedRetailPrice
    {
      get { return _recommendedRetailPrice; }
    }

    public decimal Discount
    {
      get
      {
        if (RecommendedRetailPrice > SellingPrice)
          return (RecommendedRetailPrice - SellingPrice);
        else
          return 0;
      }
    }

    public decimal Savings
    {
      get
      {
        if (RecommendedRetailPrice > SellingPrice)
          return 1 - (SellingPrice / RecommendedRetailPrice);
        else
          return 0;
      }
    }
  }
}
