using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWithOracle.Model.Discount;
using WPFWithOracle.Model.Products;
using WPFWithOracle.Service.Products;

namespace WPFWithOracle.Presention.Products
{
  public interface IProductListView : IListView<ProductViewModel>
  {
  }
}
