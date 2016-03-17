using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWithOracle.Model.Discount;
using WPFWithOracle.MySocketServer.SocketHelper;

namespace WPFWithOracle.Service.Products
{
  public class ProductListRequestImpl : BaseRequest
  {
    public CustomerType CustomerType { get; set; }

  }
}
