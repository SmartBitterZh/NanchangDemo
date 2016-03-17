using System;

namespace lite
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
	                Inherited = false, AllowMultiple = false)]
	public class PKAttribute : Attribute
	{
		public PKAttribute() : base()
		{}
	}
}
