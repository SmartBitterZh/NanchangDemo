using System;

namespace lite
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
	                Inherited = false, AllowMultiple = false)]
	public class ColumnAttribute : Attribute
	{
		private string name;
		private string alias;
		
		public ColumnAttribute() : base()
		{}
		
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		
		public string Alias
		{
			get { return alias; }
			set { alias = value; }
		}
	}
}
