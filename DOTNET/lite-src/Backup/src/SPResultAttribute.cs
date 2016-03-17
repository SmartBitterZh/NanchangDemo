using System;

namespace lite
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct,
	                AllowMultiple = false, Inherited = false)]
	public class SPResultAttribute : Attribute
	{
		public SPResultAttribute() : base()
		{}
	}
}
