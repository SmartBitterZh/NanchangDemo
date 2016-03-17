using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFWithOracle.Model.Customer;

namespace WPFWithOracle.UnitTest
{

  [TestFixture]
  public class CustomerTester
  {
    Customer _customer = new Customer();

    [Test]
    public void Test_OrderProcecss_Is_Executed_Successfully()
    {
      _customer.PostOrder();
    }
  }
}
