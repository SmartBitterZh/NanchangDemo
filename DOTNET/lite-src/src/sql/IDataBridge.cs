using System;

namespace lite.sqlserver
{
	public interface IDataBridge
	{
		bool Readable { get; }
		bool Writeable { get; }
		object Read(object obj);
		void Write(object obj, object val);
		Type DataType { get; }
	}
}
