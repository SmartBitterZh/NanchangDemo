using System;

namespace lite.sqlserver
{
	public interface ISqlTableFactory
	{
		SqlTable Build(Type type);
	}
}
