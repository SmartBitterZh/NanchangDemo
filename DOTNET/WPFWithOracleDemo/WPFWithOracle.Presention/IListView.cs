using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWithOracle.Model.Discount;

namespace WPFWithOracle.Presention
{
  public interface IListView<TModel>
  {
    CustomerType CustomerType { get; }
    string ErrorMessage { set; }
    void Display(IList<TModel> Products);
  }
}
