using System;

namespace LightJson.Serialization
{
	/// <summary>
	/// The exception that is thrown when a JSON value cannot be serialized.
	/// </summary>
	public sealed class JsonSerializationException : Exception
	{
		/// <summary>
		/// Gets the error code associated with this exception.
		/// </summary>
		public ErrorCode Code { get; private set; }

		/// <summary>
		/// Gets the JsonValue that caused the exception to be thrown.
		/// </summary>
		public JsonValue Value { get; private set; }

		/// <summary>
		/// Initializes a new instance of JsonSerializationException.
		/// </summary>
		public JsonSerializationException() : base()
		{
			this.Code = ErrorCode.Unknown;
			this.Value = JsonValue.Null;
		}

		private JsonSerializationException(string message) : base(message) { }
		private JsonSerializationException(string message, Exception inner) : base(message, inner) { }

		/// <summary>
		/// Initializes a new instance of JsonSerializationException with the given error code and JSON value.
		/// </summary>
		/// <param name="code">The error code that describes the cause of the exception.</param>
		/// <param name="value">The JsonValue that cause the exception to be thrown.</param>
		public JsonSerializationException(ErrorCode code, JsonValue value)
			: base(GetMessage(code))
		{
			this.Code = code;
			this.Value = value;
		}

		public static string GetMessage(ErrorCode code)
		{
			switch (code)
			{
				case ErrorCode.InvalidNumber:
					return "The object been serialized contains an invalid number value (NAN, infinity).";

				case ErrorCode.InvalidValueType:
					return "The object been serialized contains (or is itself) an invalid JSON type.";

				case ErrorCode.CircularReference:
					return "The object been serialized contains circular references.";

				default:
					return "An error occurred during serialization.";
			}
		}

		/// <summary>
		/// Enumerates the errors that can occur during serialization.
		/// </summary>
		public enum ErrorCode : int
		{
			/// <summary>
			/// Indicates that the cause of the error is unknown.
			/// </summary>
			Unknown = 0,

			/// <summary>
			/// Indicates that the writer encountered an invalid number value (NAN, infinity) during serialization.
			/// </summary>
			InvalidNumber,

			/// <summary>
			/// Indicates that the object been serialized contains an invalid JSON value type.
			/// That is, a value type that is not null, boolean, number, string, object, or array.
			/// </summary>
			InvalidValueType,

			/// <summary>
			/// Indicates that the object been serialized contains a circular reference.
			/// </summary>
			CircularReference,
		}
	}
}
