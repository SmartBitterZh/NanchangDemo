using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.UnitTest
{
  [TestFixture]
  public class ProductServiceTester
  {
    Service.Products.ProductService _service = new Service.Products.ProductService();

    [Test]
    public void Test_GetAllProductsFo_Is_Executed_Successfullyr()
    {
      Assert.IsNotNull(_service.GetAllProductsFor(new Service.Products.ProductListRequestImpl()), "Successfull");
    }

  }
}
