using System;

namespace LightJson.Serialization
{
	public sealed class JsonParseException : Exception
	{
		public long Line { get; private set; }
		public long Column { get; private set; }

		public JsonParseException() { }
		public JsonParseException(string message) : base(message) { }
		public JsonParseException(string message, Exception inner) : base(message, inner) { }

		public JsonParseException(string message, long line, long column)
			: base(message)
		{
			this.Line = line;
			this.Column = column;
		}
	}
}
