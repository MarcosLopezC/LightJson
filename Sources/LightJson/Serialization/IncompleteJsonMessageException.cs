using System;

namespace LightJson.Serialization
{
	public class IncompleteJsonMessageException : Exception
	{
		private const string DefaultMessage = "The JSON message ended before a value could be parsed.";

		public IncompleteJsonMessageException() : base(DefaultMessage) { }

		public IncompleteJsonMessageException(string message) : base(message) { }

		public IncompleteJsonMessageException(string message, Exception inner) : base(message, inner) { }
	}
}
