using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Helper.PersistenceHelper
{
  public class SqlHelper : BaseADOPersistenceHelper
  {
    public SqlHelper(string connStr = "")
      : base(connStr)
    { }

    protected override System.Data.Common.DbConnection CreateConnection()
    {
      if (Connection != null)
        return Connection;
      return new SqlConnection(ConnectionString);
    }

    protected override System.Data.Common.DbDataAdapter CreateDataAdapter(string sqlStr)
    {
      return new SqlDataAdapter(sqlStr, (Connection as SqlConnection));
    }

    protected override System.Data.Common.DbCommand CreateCommand(string sqlStr = "")
    {
      return new SqlCommand(sqlStr, (Connection as SqlConnection));
    }
  }
}
