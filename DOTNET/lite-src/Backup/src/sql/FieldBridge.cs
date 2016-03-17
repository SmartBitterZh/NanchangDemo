using System;
using System.Reflection;

namespace lite.sqlserver
{
	public class FieldBridge : IDataBridge
	{
		private FieldInfo field;
		
		public FieldBridge(FieldInfo field)
		{
			this.field = field;
		}
		
		public bool Readable
		{
			get { return true; }
		}
		
		public bool Writeable
		{
			get { return true; }
		}
		
		public Type DataType
		{
			get { return field.FieldType; }
		}
		
		public object Read(object obj)
		{
			return field.GetValue(obj);
		}
		
		public void Write(object obj, object val)
		{
			field.SetValue(obj, val);
		}
	}
}
