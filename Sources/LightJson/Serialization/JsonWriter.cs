using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace LightJson.Serialization
{
	using ErrorType = JsonSerializationException.ErrorType;

	/// <summary>
	/// Represents a writer that can write string representations of JsonValues.
	/// </summary>
	public sealed class JsonWriter : IDisposable
	{
		private int indent;
		private bool isNewLine;
		private TextWriter writer;
		private HashSet<JsonValue> RenderingValues;

		/// <summary>
		/// Gets or sets the string representing a indent in the output.
		/// </summary>
		public string IndentString { get; set; }

		/// <summary>
		/// Gets or sets the string representing a space in the output.
		/// </summary>
		public string SpacingString { get; set; }

		/// <summary>
		/// Gets or sets the string representing a new line on the output.
		/// </summary>
		public string NewLineString { get; set; }

		/// <summary>
		/// Initializes a new instance of JsonWriter.
		/// </summary>
		public JsonWriter() : this(false) { }

		/// <summary>
		/// Initializes a new instance of JsonWriter.
		/// </summary>
		/// <param name="pretty">
		/// A value indicating whether the output of the writer should be human-readable.
		/// </param>
		public JsonWriter(bool pretty)
		{
			if (pretty)
			{
				this.IndentString = "\t";
				this.SpacingString = " ";
				this.NewLineString = "\n";
			}
		}

		private void Initialize()
		{
			this.indent = 0;
			this.isNewLine = true;
			this.writer = new StringWriter();
			this.RenderingValues = new HashSet<JsonValue>();
		}

		private void Write(string text)
		{
			if (this.isNewLine)
			{
				this.isNewLine = false;
				WriteIndentation();
			}

			writer.Write(text);
		}

		private void WriteEncodedString(string text)
		{
			Write(EncodeStringValue(text));
		}

		private void WriteIndentation()
		{
			for (var i = 0; i < this.indent; i += 1)
			{
				Write(this.IndentString);
			}
		}

		private void WriteSpacing()
		{
			Write(this.SpacingString);
		}

		private void WriteLine()
		{
			Write(this.NewLineString);
			this.isNewLine = true;
		}

		private void WriteLine(string line)
		{
			Write(line);
			WriteLine();
		}

		private void AddRenderingValue(JsonValue value)
		{
			if (RenderingValues.Contains(value))
			{
				throw new JsonSerializationException(ErrorType.CircularReference);
			}
			else
			{
				RenderingValues.Add(value);
			}
		}

		private void RemoveRenderingValue(JsonValue value)
		{
			RenderingValues.Remove(value);
		}

		private void Render(JsonValue value)
		{
			switch (value.Type)
			{
				case JsonValueType.Null:
				case JsonValueType.Boolean:
				case JsonValueType.Number:
				case JsonValueType.String:
					Write(EncodeJsonValue(value));
					break;

				case JsonValueType.Object:
					Render((JsonObject)value);
					break;

				case JsonValueType.Array:
					Render((JsonArray)value);
					break;

				default:
					throw new JsonSerializationException(ErrorType.InvalidValueType);
			}
		}

		private void Render(JsonArray value)
		{
			AddRenderingValue(value);

			WriteLine("[");

			indent += 1;

			using (var enumerator = value.GetEnumerator())
			{
				var hasNext = enumerator.MoveNext();

				while (hasNext)
				{
					Render(enumerator.Current);

					hasNext = enumerator.MoveNext();

					if (hasNext)
					{
						WriteLine(",");
					}
					else
					{
						WriteLine();
					}
				}
			}

			indent -= 1;

			Write("]");

			RemoveRenderingValue(value);
		}

		private void Render(JsonObject value)
		{
			AddRenderingValue(value);

			WriteLine("{");

			indent += 1;

			using(var enumerator = value.GetEnumerator())
			{
				var hasNext = enumerator.MoveNext();

				while (hasNext)
				{
					WriteEncodedString(enumerator.Current.Key);
					Write(":");
					WriteSpacing();
					Render(enumerator.Current.Value);

					hasNext = enumerator.MoveNext();

					if (hasNext)
					{
						WriteLine(",");
					}
					else
					{
						WriteLine();
					}
				}
			}

			indent -= 1;

			Write("}");

			RemoveRenderingValue(value);
		}

		/// <summary>
		/// Returns a string representation of the given JsonValue.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to serialize.</param>
		public string Serialize(JsonValue jsonValue)
		{
			Initialize();

			Render(jsonValue);

			return writer.ToString();
		}

		/// <summary>
		/// Releases all the resources used by this object.
		/// </summary>
		public void Dispose()
		{
			if (this.writer != null)
			{
				this.writer.Dispose();
			}
		}

		/// <summary>
		/// Returns a string representation of the given JsonValue.
		/// </summary>
		/// <param name="value">The value to encode.</param>
		public static string EncodeJsonValue(JsonValue value)
		{
			switch (value.Type)
			{
				case JsonValueType.Null:
					return "null";

				case JsonValueType.Boolean:
					return (bool)value ? "true" : "false";

				case JsonValueType.Number:
					return ((double)value).ToString(CultureInfo.InvariantCulture);

				case JsonValueType.String:
					return EncodeStringValue((string)value);

				case JsonValueType.Object:
					return string.Format("JsonObject[{0}]", value.AsJsonObject.Count);

				case JsonValueType.Array:
					return string.Format("JsonArray[{0}]", value.AsJsonArray.Count);

				default:
					throw new InvalidOperationException("Invalid value type.");
			}
		}

		private static string EncodeStringValue(string value)
		{
			var builder = new StringBuilder(value.Length + 2);

			// Leaving a place holder for the initial quote.
			builder.Append((char)0);

			// Escaping characters.
			builder.Append(value);
			builder.Replace("\\", "\\\\"); // \ -> \\
			builder.Replace("\"", "\\\""); // " -> \"
			builder.Replace("/", "\\/");   // / -> \/
			builder.Replace("\b", "\\b");
			builder.Replace("\f", "\\f");
			builder.Replace("\n", "\\n");
			builder.Replace("\r", "\\r");
			builder.Replace("\t", "\\t");

			// Surrounding text with double quotes.
			builder[0] = '\"';
			builder.Append("\"");

			return builder.ToString();
		}

		private static bool IsValidNumber(double number)
		{
			return !(double.IsNaN(number) || double.IsInfinity(number));
		}
	}
}
