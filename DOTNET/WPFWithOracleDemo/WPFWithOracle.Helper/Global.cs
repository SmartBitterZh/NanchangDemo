using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFWithOracle.Helper.PersistenceHelper;

namespace WPFWithOracle.Helper
{
  public sealed class Global
  {
    private static PersistenceType m_type = PersistenceType.ADO;
    public static PersistenceType PersistenceType { get { return m_type; } set { m_type = value; } }
    private static DatabaseType m_dbType = DatabaseType.SqlServer;
    public static DatabaseType DatabaseType { get { return m_dbType; } set { m_dbType = value; } }
  }
}
