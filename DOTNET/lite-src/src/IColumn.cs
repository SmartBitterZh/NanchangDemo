using System;

namespace lite
{
	public interface IColumn
	{
		ITable Table { get; }
		string Name { get; }
		Type DataType { get; }
	}
}
