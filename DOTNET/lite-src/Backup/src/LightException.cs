using System;

namespace lite
{
	public class LightException : Exception
	{
		public LightException(string message)
			: base(message)
		{}
		
		public LightException(string message, Exception cause)
			: base(message, cause)
		{}
		
		public LightException(Exception cause)
			: base(cause.Message, cause)
		{}
	}
}
