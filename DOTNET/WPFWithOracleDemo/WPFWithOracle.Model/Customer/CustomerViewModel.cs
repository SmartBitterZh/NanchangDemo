using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Model.Customer
{
  public class CustomerViewModel
  {
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
  }
}
