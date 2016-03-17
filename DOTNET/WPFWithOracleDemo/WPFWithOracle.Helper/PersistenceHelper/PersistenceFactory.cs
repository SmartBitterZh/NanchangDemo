using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Helper.PersistenceHelper
{
  public class PersistenceFactory
  {
    public static BaseADOPersistenceHelper CreateADOInstence(DatabaseType dbType = DatabaseType.SqlServer)
    {
      if (dbType == DatabaseType.SqlServer)
        return new SqlHelper();
      else if (dbType == DatabaseType.Oracle)
        return new OracleHelper();
      return null;
    }
    public static BasePersistenceHelper CreateInstence(PersistenceType type = PersistenceType.ADO, DatabaseType dbType = DatabaseType.SqlServer)
    {
      if (type == PersistenceType.ADO)
        return CreateADOInstence(dbType);
      else if (type == PersistenceType.NHibernate)
        return new NHibernateHelper();
      else if (type == PersistenceType.EntityFarmwork)
        return new EntityFarmworkHelper();

      return null;
    }
  }
}
