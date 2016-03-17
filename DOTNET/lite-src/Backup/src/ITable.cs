using System;

namespace lite
{
	public interface ITable
	{		
		Type Class { get; }
		string Name { get; }
		string Schema { get; }
		IColumn[] Columns { get; }
	}
}
