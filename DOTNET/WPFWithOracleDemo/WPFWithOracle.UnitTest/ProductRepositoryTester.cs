using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.UnitTest
{
  [TestFixture]
  public class ProductRepositoryTester
  {
    Repository.Products.ProductRepositoryImpl _repository = new Repository.Products.ProductRepositoryImpl();
    [Test]
    public void FindAll_Test()
    {
      Assert.IsNull(_repository.FindAll(), "failed");
    }

  }
}
