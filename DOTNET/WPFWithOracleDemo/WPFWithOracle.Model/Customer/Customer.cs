using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFWithOracle.Model.Orders;

namespace WPFWithOracle.Model.Customer
{
  public class Customer
  {
    public int Id { get; set; }

    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }

    public void PostOrder()
    {

    }
  }
}
