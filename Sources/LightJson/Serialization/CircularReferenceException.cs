using System;

namespace LightJson.Serialization
{
	/// <summary>
	/// The exception that is thrown when a circular reference is detected during serialization.
	/// </summary>
	public class CircularReferenceException : Exception
	{
		private const string DefaultMessage = "The object been serialized contains circular references.";

		/// <summary>
		/// Initializes a new instance of the CircularReferenceException class.
		/// </summary>
		public CircularReferenceException() : this(DefaultMessage) { }

		/// <summary>
		/// Initializes a new instance of the CircularReferenceException  class with a given message.
		/// </summary>
		/// <param name="message">The error message.</param>
		public CircularReferenceException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the CircularReferenceException class with a given message
		/// and a reference to the inner exception that caused this exception.
		/// </summary>
		/// <param name="message">The error message.</param>
		/// <param name="inner"></param>
		public CircularReferenceException(string message, Exception inner) : base(message, inner) { }
	}
}
