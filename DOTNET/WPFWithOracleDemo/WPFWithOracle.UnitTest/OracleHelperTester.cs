using NUnit.Framework;
using WPFWithOracle.Helper.PersistenceHelper;

namespace UnitTest
{
  [TestFixture]
  public class OracleHelperTester
  {
    [Test]
    public void OracleClientConnect_Test()
    {
      Assert.IsTrue(OracleHelper.OracleClientConnect(), "Connect successfull");
    }

    [Test]
    public void OraOLEDBConnect_Test()
    {
      Assert.IsTrue(OracleHelper.OraOLEDBConnect(), "Connect successfull");
    }
  }
}
