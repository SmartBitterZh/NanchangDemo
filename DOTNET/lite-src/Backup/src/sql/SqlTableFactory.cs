using System;
using System.Reflection;

namespace lite.sqlserver
{
	public class SqlTableFactory
	{
		public static readonly SqlTableFactory Instance = new SqlTableFactory();
		
		private LRUCache<SqlTable> cache = new LRUCache<SqlTable>(50);
		
		private SqlTableFactory()
		{}
		
		public SqlTable Build(Type type)
		{
			SqlTable table = cache.Get(type.FullName);
			if (table == null)
			{
				table = BuildTable(type);
				cache.Put(type.FullName, table);
			}
			return table;
		}
		
		private SqlTable BuildTable(Type type)
		{
			SqlTable table = null;
			if (!type.IsDefined(typeof(TableAttribute), false))
			{
				if (!type.IsDefined(typeof(SPResultAttribute), false))
					throw new LightException("no TableAttribute or SPResultAttribute found on " + type.FullName);
				else
					table = new SqlTable(type, null, null);
			}
			else
			{
				TableAttribute tableAttr = (TableAttribute) type.GetCustomAttributes(typeof(TableAttribute), false)[0];
				string name = tableAttr.Name;
				string schema = tableAttr.Schema;
				if (name == null || name.Length == 0)
					name = type.Name;
				table = new SqlTable(type, name, schema);
			}
			FindInherited(type, table);
			ProcessFields(type, table);
			ProcessProperties(type, table);
			ProcessMethods(type, table);
			return table;
		}
		
		private void FindInherited(Type type, SqlTable table)
		{
			if (type.IsDefined(typeof(MapAttribute), false))
			{
				object[] attrs = type.GetCustomAttributes(typeof(MapAttribute), false);
				foreach (MapAttribute attr in attrs)
				{
					IDataBridge data = null;
					BindingFlags flags = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
					MemberInfo member = type.GetProperty(attr.Field, flags);
					if (member != null)
					{
						data = new PropertyBridge((PropertyInfo)member);
					}
					else
					{
						member = type.GetField(attr.Field, flags);
						if (member != null)
							data = new FieldBridge((FieldInfo)member);
						else
							throw new LightException("member " + attr.Field + " not found in class " + type.Name);
					}
					if (attr.Name == null || attr.Name.Length == 0)
						attr.Name = member.Name;
					SqlColumn column = new SqlColumn(table, attr.Name, data);
					if (attr.Alias == null || attr.Alias.Length == 0)
						column.Alias = attr.Field;
					else
						column.Alias = attr.Alias;
					column.IsID = attr.ID;
					column.IsPK = attr.PK;
					table.Add(column);
				}
			}
		}
		
		private void ProcessProperties(Type type, SqlTable table)
		{
			PropertyInfo[] fields = type.GetProperties(
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			foreach (PropertyInfo field in fields)
			{
				if (!field.IsDefined(typeof(ColumnAttribute), false))
					continue;
				ColumnAttribute colAttr = (ColumnAttribute)
					field.GetCustomAttributes(typeof(ColumnAttribute), false)[0];
				PropertyBridge data = new PropertyBridge(field);
				if (colAttr.Name == null || colAttr.Name.Length == 0)
					colAttr.Name = field.Name;
				SqlColumn column = new SqlColumn(table, colAttr.Name, data);
				if (colAttr.Alias == null || colAttr.Alias.Length == 0)
					column.Alias = field.Name;
				else
					column.Alias = colAttr.Alias;
				if (field.IsDefined(typeof(IDAttribute), false))
					column.IsID = true;
				if (field.IsDefined(typeof(PKAttribute), false))
					column.IsPK = true;
				table.Add(column);
			}
		}
		
		private void ProcessFields(Type type, SqlTable table)
		{
			FieldInfo[] fields = type.GetFields(
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			foreach (FieldInfo field in fields)
			{
				if (!field.IsDefined(typeof(ColumnAttribute), false))
					continue;
				ColumnAttribute colAttr = (ColumnAttribute)
					field.GetCustomAttributes(typeof(ColumnAttribute), false)[0];
				FieldBridge data = new FieldBridge(field);
				if (colAttr.Name == null || colAttr.Name.Length == 0)
					colAttr.Name = field.Name;
				SqlColumn column = new SqlColumn(table, colAttr.Name, data);
				if (colAttr.Alias == null || colAttr.Alias.Length == 0)
					column.Alias = field.Name;
				else
					column.Alias = colAttr.Alias;
				if (field.IsDefined(typeof(IDAttribute), false))
					column.IsID = true;
				if (field.IsDefined(typeof(PKAttribute), false))
					column.IsPK = true;
				table.Add(column);
			}
		}
		
		private void ProcessMethods(Type type, SqlTable table)
		{
			MethodInfo[] methods = type.GetMethods(
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MethodInfo method in methods)
			{
				if (method.IsDefined(typeof(TriggerAttribute), false))
				{
					TriggerAttribute attr = (TriggerAttribute)
						method.GetCustomAttributes(typeof(TriggerAttribute), false)[0];
					SqlTrigger trigger = new SqlTrigger(method, attr.Timing);
					table.AddTrigger(trigger);
				}
			}
		}
	}
}
