using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWithOracle.Model.Products;

namespace WPFWithOracle.Repository.Products
{
  public class ProductRepositoryImpl : BaseRepositoryImpl<Product>, IProductRepository
  {
    public override IList<Product> FindAll()
    {
      List<Product> _list = new List<Product>();
      //var products = from p in new ShopDataContext().Product
      //               select new Product
      //               {
      //                 Id = p.ProductId,
      //                 Name = p.ProductName,
      //                 Price = new Model.Price(p.RRP, p.SellingPrice)
      //               };

      //return products.ToList();

      _list.Add(new Product() { Id = 1, Name = "a", Price = new Price(15, 20) });
      _list.Add(new Product() { Id = 2, Name = "b", Price = new Price(5, 15) });
      _list.Add(new Product() { Id = 3, Name = "c", Price = new Price(10, 15) });
      return _list;
    }

  }
}
