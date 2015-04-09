using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace LightJson.Serialization
{
	using ErrorCode = JsonSerializationException.ErrorCode;

	public sealed class JsonWriter : IDisposable
	{
		private int indent;
		private bool isNewLine;
		private TextWriter writer;
		private HashSet<JsonValue> RenderingValues;

		public string IndentString { get; set; }
		public string SpacingString { get; set; }
		public string NewLineString { get; set; }

		public JsonWriter() : this(false) { }

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
				throw new CircularReferenceException();
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
					throw new InvalidOperationException("Invalid value type.");
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

		public string Serialize(JsonValue jsonValue)
		{
			Initialize();

			Render(jsonValue);

			return writer.ToString();
		}

		public void Dispose()
		{
			if (this.writer != null)
			{
				this.writer.Dispose();
			}
		}

		public static string EncodeJsonValue(JsonValue value)
		{
			switch (value.Type)
			{
				case JsonValueType.Null:
					return "null";

				case JsonValueType.Boolean:
					return (bool)value ? "true" : "false";

				case JsonValueType.Number:
					return ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);

				case JsonValueType.String:
					return EncodeStringValue((string)value);

				default:
					return value.ToString();
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

			// Surounding text with double quotes.
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
