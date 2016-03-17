using System;
using System.Data;
using System.Collections;

namespace lite.sqlserver
{
	public static class SqlUtils
	{
		private static readonly Hashtable typemap;
		static SqlUtils()
		{
			typemap = new Hashtable(17, 1f);
			typemap.Add(typeof(string), DbType.String);
			typemap.Add(typeof(int), DbType.Int32);
			typemap.Add(typeof(bool), DbType.Boolean);
			typemap.Add(typeof(DateTime), DbType.DateTime);
			typemap.Add(typeof(double), DbType.Double);
			typemap.Add(typeof(long), DbType.Int64);
			typemap.Add(typeof(short), DbType.Int16);
			typemap.Add(typeof(byte), DbType.Byte);
			typemap.Add(typeof(char), DbType.StringFixedLength);
			typemap.Add(typeof(decimal), DbType.Decimal);
			typemap.Add(typeof(float), DbType.Single);
			typemap.Add(typeof(uint), DbType.UInt32);
			typemap.Add(typeof(ulong), DbType.UInt64);
			typemap.Add(typeof(ushort), DbType.UInt16);
			typemap.Add(typeof(sbyte), DbType.SByte);
			typemap.Add(typeof(Guid), DbType.Guid);
			typemap.Add(typeof(byte[]), DbType.Binary);
		}
		
		public static void PrepParam(IDbDataParameter p, object val)
		{
			if (val == null || val == DBNull.Value)
			{
				PrepParam(p, (Type)null);
				p.Value = DBNull.Value;
			}
			else
			{
				PrepParam(p, val.GetType());
				p.Value = val;
			}
		}
		
		public static void PrepParam(IDbDataParameter p, Type type)
		{
			p.DbType = ResolveType(type);
			SetMaxValues(p);
		}
		
		public static void SetMaxValues(IDbDataParameter p)
		{
			p.Size = int.MaxValue;
			p.Precision = 38; //this is max for SQLServer
			//p.Scale = ...; //not required - works without it
		}
		
		public static DbType ResolveType(Type type)
		{
			if (type != null)
			{
				Type t = type;
				if (t.IsGenericType && t.IsValueType)
				{
					Type[] genericTypes = t.GetGenericArguments();
					if (genericTypes.Length > 0)
						t = genericTypes[0];
				}
				if (t != null && typemap.ContainsKey(t))
					return (DbType) typemap[t];
			}
			return DbType.String;
		}
	}
}
