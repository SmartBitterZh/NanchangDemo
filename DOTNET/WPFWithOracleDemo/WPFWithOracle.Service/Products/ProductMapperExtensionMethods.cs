using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWithOracle.Model.Products;

namespace WPFWithOracle.Service.Products
{
  public static class ProductMapperExtensionMethods
  {
    public static IList<Product> ConvertToProductList(this IList<ProductViewModel> productViewModels)
    {
      IList<Product> product = new List<Product>();

      foreach (ProductViewModel _p in productViewModels)
      {
        product.Add(_p.ConvertToProduct());
      }

      return product;
    }

    public static IList<ProductViewModel> ConvertToProductListViewModel(this IList<Product> products)
    {
      IList<ProductViewModel> productViewModels = new List<ProductViewModel>();

      foreach (Product _p in products)
      {
        productViewModels.Add(_p.ConvertToProductViewModel());
      }

      return productViewModels;
    }

    public static ProductViewModel ConvertToProductViewModel(this Product product)
    {
      ProductViewModel productViewModel = new ProductViewModel();
      productViewModel.ProductId = product.Id;
      productViewModel.Name = product.Name;
      productViewModel.RecommendedRetailPrice = product.Price.RecommendedRetailPrice.ToString();
      productViewModel.SellingPrice = product.Price.SellingPrice.ToString();

      if (product.Price.Discount > 0)
        productViewModel.Discount = product.Price.Discount.ToString();

      if (product.Price.Savings < 1 && product.Price.Savings > 0)
        productViewModel.Savings = product.Price.Savings.ToString("#%");

      return productViewModel;
    }


    public static Product ConvertToProduct(this ProductViewModel productViewModel)
    {
      Product product = new Product();
      product.Id = productViewModel.ProductId;
      product.Name = productViewModel.Name;
      product.Price = new Price(decimal.Parse(productViewModel.RecommendedRetailPrice), decimal.Parse(productViewModel.SellingPrice));
      //product.Price.Discount = decimal.Parse(productViewModel.Discount);
      //product.Price.Savings = decimal.Parse(productViewModel.Savings);

      return product;
    }
  }
}
