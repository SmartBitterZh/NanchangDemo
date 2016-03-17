using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWithOracle.Model.Discount;
using WPFWithOracle.Model.Products;

namespace WPFWithOracle.Repository.Products
{
  public class ProductService
  {
    private IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    public IList<Product> GetAllProductsFor(CustomerType customerType)
    {
      IDiscountStrategy _discountStrategy = DiscountFactory.GetDiscountStrategyFor(customerType);
      IList<Product> _productList = _productRepository.FindAll();
      if (_productList != null)
        _productList.Apply(_discountStrategy);
      return _productList;
    }

    public bool UpdateProductsFor(CustomerType customerType, IList<Product> productList)
    {
      IDiscountStrategy _discountStrategy = DiscountFactory.GetDiscountStrategyFor(customerType);
      try
      {
        _productRepository.Update(productList);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
