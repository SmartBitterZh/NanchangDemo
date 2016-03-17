using System;
using System.Data;

namespace lite.sqlserver
{
	public class SqlColumn : IColumn
	{
		private ITable table;
		private IDataBridge data;
		private string name;
		private string alias;
		private bool pk = false;
		private bool id = false;
		private int ordinal = -1;
		
		public SqlColumn(ITable table, string name, IDataBridge data)
		{
			this.table = table;
			this.name = name.ToLower();
			this.data = data;
		}
		
		public int Ordinal
		{
			get { return ordinal; }
			set { ordinal = value; }
		}
		
		public ITable Table
		{
			get { return table; }
		}
		
		public Type DataType
		{
			get { return data.DataType; }
		}
		
		public string Name
		{
			get { return name; }
		}
		
		public string Alias
		{
			get
			{
				if (alias == null || alias.Length == 0)
					return name;
				return alias;
			}
			set { alias = (value==null) ? null : value.ToLower(); }
		}
		
		public bool IsPK
		{
			get { return pk; }
			set { pk = value; }
		}
		
		public bool IsID
		{
			get { return id; }
			set { id = value; }
		}
		
		public bool IsReadable
		{
			get { return data.Readable; }
		}
		
		public bool IsWriteable
		{
			get { return data.Writeable; }
		}
		
		public void SetParameterValue(IDbDataParameter p, object obj)
		{
			object val = data.Read(obj);
			if (val == null)
				p.Value = DBNull.Value;
			else
				p.Value = val;
		}
		
		public void SetValue(object obj, object val)
		{
			if (DBNull.Value.Equals(val))
				data.Write(obj, null);
			else
				data.Write(obj, val);
		}
	}
}
