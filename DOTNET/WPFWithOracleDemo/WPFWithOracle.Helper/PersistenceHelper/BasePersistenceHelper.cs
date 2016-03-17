using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Helper.PersistenceHelper
{
  public abstract class BasePersistenceHelper
  {
    private static BasePersistenceHelper _helper;
    public static BasePersistenceHelper Instence
    {
      get
      {
        if (_helper == null)
          _helper = CreateInstence(Global.PersistenceType, Global.DatabaseType);
        return _helper;
      }
    }

    public static BasePersistenceHelper CreateInstence(PersistenceType type, DatabaseType dbType)
    {
      return PersistenceFactory.CreateInstence(type, dbType);
    }

  }
}
