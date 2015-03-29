using System;

namespace LightJson.Serialization
{
	public class InvalidJsonNumberException : Exception
	{
		private const string DefaultMessage = "Invalid JSON number value.";

		public InvalidJsonNumberException() : base(DefaultMessage) { }

		public InvalidJsonNumberException(string message) : base(message) { }

		public InvalidJsonNumberException(string message, Exception inner) : base(message, inner) { }
	}
}
