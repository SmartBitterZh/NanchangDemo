using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Helper.PersistenceHelper
{
  public class OracleHelper : BaseADOPersistenceHelper
  {
    public static bool OracleClientConnect()
    {
      return false;
    }
    public static bool OraOLEDBConnect()
    {
      return false;
    }

    public OracleHelper(string connStr = "")
      : base(connStr)
    { }

    protected override System.Data.Common.DbConnection CreateConnection()
    {
      if (Connection != null)
        return Connection;
      return Connection = new OleDbConnection(ConnectionString);
    }

    protected override System.Data.Common.DbDataAdapter CreateDataAdapter(string sqlStr)
    {
      return new OleDbDataAdapter(sqlStr, (Connection as OleDbConnection));
    }

    protected override System.Data.Common.DbCommand CreateCommand(string sqlStr = "")
    {
      return new OleDbCommand(sqlStr, (Connection as OleDbConnection));
    }

  }
}
