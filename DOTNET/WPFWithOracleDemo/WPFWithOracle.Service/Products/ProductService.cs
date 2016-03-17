using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWithOracle.Model.Products;
using WPFWithOracle.MySocketServer;
using WPFWithOracle.Repository.Products;

namespace WPFWithOracle.Service.Products
{
  public class ProductService
  {
    public ProductListResponseImpl GetAllProductsFor(ProductListRequestImpl request)
    {
      ProductListResponseImpl _response = new ProductListResponseImpl();

      Repository.Products.ProductService _server = new Repository.Products.ProductService(new ProductRepositoryImpl());
      try
      {
        IList<Product> _productList = _server.GetAllProductsFor(request.CustomerType);
        IList<ProductViewModel> _list = _productList.ConvertToProductListViewModel();
        _response.Success = true;
        _response.Message = string.Empty;
        _response.Context = _list;
      }
      catch (Exception e)
      {
        _response.Success = false;
        _response.Message = e.Message;
        _response.Context = null;
      }
      return _response;
    }


    public ProductListResponseImpl UpdateProductsFor(ProductListRequestImpl request, IList<ProductViewModel> productList)
    {
      ProductListResponseImpl _response = new ProductListResponseImpl();

      Repository.Products.ProductService _server = new Repository.Products.ProductService(new ProductRepositoryImpl());
      try
      {
        bool _result = _server.UpdateProductsFor(request.CustomerType, productList.ConvertToProductList());
        _response.Success = true;
        _response.Message = string.Empty;
        _response.Context = _result;
      }
      catch (Exception e)
      {
        _response.Success = false;
        _response.Message = e.Message;
        _response.Context = null;
      }
      return _response;
    }
  }
}
