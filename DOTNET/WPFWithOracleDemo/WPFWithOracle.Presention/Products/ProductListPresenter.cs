using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWithOracle.Model.Products;
using WPFWithOracle.Service.Products;

namespace WPFWithOracle.Presention.Products
{
  public class ProductListPresenter
  {
    private IProductListView _productListView;
    private ProductService _productService;

    public ProductListPresenter(IProductListView ProductListView, ProductService ProductService)
    {
      _productService = ProductService;
      _productListView = ProductListView;
    }

    public void Display()
    {
      ProductListRequestImpl productListRequest = new ProductListRequestImpl();
      productListRequest.CustomerType = _productListView.CustomerType;

      ProductListResponseImpl productResponse = _productService.GetAllProductsFor(productListRequest);

      if (productResponse != null && productResponse.Success)
      {
        _productListView.Display((IList<ProductViewModel>)productResponse.Context);
      }
      else
      {
        _productListView.ErrorMessage = productResponse.Message;
      }

    }

    public void Update(IList<ProductViewModel> productList)
    {
      ProductListRequestImpl productListRequest = new ProductListRequestImpl();
      productListRequest.CustomerType = _productListView.CustomerType;

      ProductListResponseImpl productResponse = _productService.UpdateProductsFor(productListRequest, productList);

      if (productResponse != null && productResponse.Success)
      {
        _productListView.Display((IList<ProductViewModel>)productList);
      }
      else
      {
        _productListView.ErrorMessage = productResponse.Message;
      }
    }
  }
}
