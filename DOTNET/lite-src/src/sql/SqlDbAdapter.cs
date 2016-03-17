using System;
using System.Data;

namespace lite.sqlserver
{
	public class SqlDbAdapter : SqlDb
	{
		public SqlDbAdapter(SqlProvider provider, IDbConnection cn)
			: base(provider, cn)
		{}
		
		// connection is managed by caller
		protected override void CloseConnection()
		{}
	}
}
