using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace LightJson.Serialization
{
	using ErrorCode = JsonSerializationException.ErrorCode;

	public sealed class JsonWriter
	{
		private int indent;
		private bool isNewLine;
		private TextWriter writer;
		private HashSet<JsonValue> renderedValues;

		public string IndentString { get; set; }
		public string SpacingString { get; set; }
		public string NewLineString { get; set; }

		public JsonWriter(bool pretty = false)
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
			this.renderedValues = new HashSet<JsonValue>();
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

		private void AddRenderedValue(JsonValue value)
		{
			if (renderedValues.Contains(value))
			{
				throw new CircularReferenceException();
			}
			else
			{
				renderedValues.Add(value);
			}
		}

		private void Render(JsonValue value)
		{
			switch (value.Type)
			{
				case JsonValueType.Null:
				case JsonValueType.Boolean:
					Write(value.ToString());
					break;

				case JsonValueType.Number:
					if (IsValidNumber((double)value))
					{
						Write(value.ToString());
					}
					else
					{
						throw new JsonSerializationException(ErrorCode.InvalidNumber, value);
					}
					break;

				case JsonValueType.String:
					WriteEncodedString((string)value);
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
			AddRenderedValue(value);

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
				}
			}

			indent -= 1;

			WriteLine();
			Write("]");
		}

		private void Render(JsonObject value)
		{
			AddRenderedValue(value);

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
				}
			}

			indent -= 1;

			WriteLine();
			Write("}");
		}

		public string Serialize(JsonObject jsonObject)
		{
			Initialize();

			Render(jsonObject);

			return writer.ToString();
		}

		public string Serialize(JsonArray jsonArray)
		{
			Initialize();

			Render(jsonArray);

			return writer.ToString();
		}

		private static string EncodeStringValue(string value)
		{
			var builder = new StringBuilder(value);

			builder.Replace("\\", "\\\\"); // \ -> \\
			builder.Replace("\"", "\\\""); // " -> \"
			builder.Replace("/", "\\/");   // / -> \/
			builder.Replace("\b", "\\b");
			builder.Replace("\f", "\\f");
			builder.Replace("\n", "\\n");
			builder.Replace("\r", "\\r");
			builder.Replace("\t", "\\t");

			return string.Format("\"{0}\"", builder.ToString());
		}

		private static bool IsValidNumber(double number)
		{
			return !(double.IsNaN(number) || double.IsInfinity(number));
		}
	}
}
