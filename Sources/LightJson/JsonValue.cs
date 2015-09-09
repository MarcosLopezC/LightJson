using System;
using System.Diagnostics;
using LightJson.Serialization;

namespace LightJson
{
	/// <summary>
	/// A wrapper object that contains a valid JSON value.
	/// </summary>
	[DebuggerDisplay("{ToString(),nq}", Type = "JsonValue({Type})")]
	[DebuggerTypeProxy(typeof(JsonValueDebugView))]
	public struct JsonValue
	{
		private readonly JsonValueType type;
		private readonly object value;

		/// <summary>
		/// Represents a null JsonValue.
		/// </summary>
		public static readonly JsonValue Null = new JsonValue(JsonValueType.Null, null);

		/// <summary>
		/// Gets the type of this JsonValue.
		/// </summary>
		public JsonValueType Type
		{
			get
			{
				return this.type;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this JsonValue is Null.
		/// </summary>
		public bool IsNull
		{
			get
			{
				return this.Type == JsonValueType.Null;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this JsonValue is a Boolean.
		/// </summary>
		public bool IsBoolean
		{
			get
			{
				return this.Type == JsonValueType.Boolean;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this JsonValue is an Integer.
		/// </summary>
		public bool IsInteger
		{
			get
			{
				if (this.IsNumber)
				{
					var doubleRepresentation = (double)this;
					var intRepresentation = Convert.ToInt32(doubleRepresentation);

					return doubleRepresentation == intRepresentation;
				}

				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this JsonValue is a Number.
		/// </summary>
		public bool IsNumber
		{
			get
			{
				return this.Type == JsonValueType.Number;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this JsonValue is a String.
		/// </summary>
		public bool IsString
		{
			get
			{
				return this.Type == JsonValueType.String;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this JsonValue is a JsonObject.
		/// </summary>
		public bool IsJsonObject
		{
			get
			{
				return this.Type == JsonValueType.Object;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this JsonValue is a JsonArray.
		/// </summary>
		public bool IsJsonArray
		{
			get
			{
				return this.Type == JsonValueType.Array;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this JsonValue represents a DateTime.
		/// </summary>
		public bool IsDateTime
		{
			get
			{
				return this.AsDateTime != null;
			}
		}

		/// <summary>
		/// Gets this value as a Boolean type.
		/// </summary>
		public bool AsBoolean
		{
			get
			{
				switch (this.Type)
				{
					case JsonValueType.Boolean:
						return (bool)this;

					case JsonValueType.Number:
						return (double)this != 0;

					case JsonValueType.String:
						return (string)this != "";

					case JsonValueType.Object:
					case JsonValueType.Array:
						return true;

					default:
						return false;
				}
			}
		}

		/// <summary>
		/// Gets this value as an Integer type.
		/// </summary>
		public int AsInteger
		{
			get
			{
				return (int)this.AsNumber;
			}
		}

		/// <summary>
		/// Gets this value as a Number type.
		/// </summary>
		public double AsNumber
		{
			get
			{
				switch (this.Type)
				{
					case JsonValueType.Boolean:
						return ((bool)this)
							? 1
							: 0;

					case JsonValueType.Number:
						return (double)this;

					case JsonValueType.String:
						double number;
						if (double.TryParse((string)this, out number))
						{
							return number;
						}
						else
						{
							goto default;
						}

					default:
						return 0;
				}
			}
		}

		/// <summary>
		/// Gets this value as a String type.
		/// </summary>
		public string AsString
		{
			get
			{
				switch (this.Type)
				{
					case JsonValueType.Boolean:
						return ((bool)this)
							? "true"
							: "false";

					case JsonValueType.Number:
						return ((double)this).ToString();

					case JsonValueType.String:
						return (string)this;

					default:
						return null;
				}
			}
		}

		/// <summary>
		/// Gets this value as an JsonObject.
		/// </summary>
		public JsonObject AsJsonObject
		{
			get
			{
				return (this.IsJsonObject)
					? (JsonObject)this
					: null;
			}
		}

		/// <summary>
		/// Gets this value as an JsonArray.
		/// </summary>
		public JsonArray AsJsonArray
		{
			get
			{
				return (this.IsJsonArray)
					? (JsonArray)this
					: null;
			}
		}

		/// <summary>
		/// Gets this value as a system.DateTime.
		/// </summary>
		public DateTime? AsDateTime
		{
			get
			{
				DateTime value;

				if (this.IsString && DateTime.TryParse((string)this, out value))
				{
					return value;
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Gets this value as a System.object.
		/// </summary>
		public object AsObject
		{
			get
			{
				return this.value;
			}
		}

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get or set.</param>
		/// <exception cref="System.InvalidOperationException">
		/// Thrown when this JsonValue is not a JsonObject.
		/// </exception>
		public JsonValue this[string key]
		{
			get
			{
				if (this.IsJsonObject)
				{
					return ((JsonObject)this)[key];
				}
				else
				{
					throw new InvalidOperationException("This value does not represent a JsonObject.");
				}
			}
			set
			{
				if (this.IsJsonObject)
				{
					((JsonObject)this)[key] = value;
				}
				else
				{
					throw new InvalidOperationException("This value does not represent a JsonObject.");
				}
			}
		}

		/// <summary>
		/// Gets or sets the value at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the value to get or set.</param>
		/// <exception cref="System.InvalidOperationException">
		/// Thrown when this JsonValue is not a JsonArray
		/// </exception>
		public JsonValue this[int index]
		{
			get
			{
				if (this.IsJsonArray)
				{
					return ((JsonArray)this)[index];
				}
				else
				{
					throw new InvalidOperationException("This value does not represent a JsonArray.");
				}
			}
			set
			{
				if (this.IsJsonArray)
				{
					((JsonArray)this)[index] = value;
				}
				else
				{
					throw new InvalidOperationException("This value does not represent a JsonArray.");
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the JsonValue struct.
		/// </summary>
		/// <param name="type">The type of the JsonValue.</param>
		/// <param name="value">The value of the JsonValue.</param>
		private JsonValue(JsonValueType type, object value)
		{
			this.type  = (value == null)
				? JsonValueType.Null
				: type;

			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the JsonValue struct, representing a Boolean value.
		/// </summary>
		/// <param name="value">The value to be wrapped.</param>
		public JsonValue(bool? value) : this(JsonValueType.Boolean, value) { }

		/// <summary>
		/// Initializes a new instance of the JsonValue struct, representing a Number value.
		/// </summary>
		/// <param name="value">The value to be wrapped.</param>
		public JsonValue(double? value) : this(JsonValueType.Number, value) { }

		/// <summary>
		/// Initializes a new instance of the JsonValue struct, representing a String value.
		/// </summary>
		/// <param name="value">The value to be wrapped.</param>
		public JsonValue(string value) : this(JsonValueType.String, value) { }

		/// <summary>
		/// Initializes a new instance of the JsonValue struct, representing a JsonObject.
		/// </summary>
		/// <param name="value">The value to be wrapped.</param>
		public JsonValue(JsonObject value) : this(JsonValueType.Object, value) { }

		/// <summary>
		/// Initializes a new instance of the JsonValue struct, representing a Array reference value.
		/// </summary>
		/// <param name="value">The value to be wrapped.</param>
		public JsonValue(JsonArray value) : this(JsonValueType.Array, value) { }

		/// <summary>
		/// Converts the given nullable boolean into a JsonValue.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		public static implicit operator JsonValue(bool? value)
		{
			return new JsonValue(value);
		}

		/// <summary>
		/// Converts the given nullable double into a JsonValue.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		public static implicit operator JsonValue(double? value)
		{
			return new JsonValue(value);
		}

		/// <summary>
		/// Converts the given string into a JsonValue.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		public static implicit operator JsonValue(string value)
		{
			return new JsonValue(value);
		}

		/// <summary>
		/// Converts the given JsonObject into a JsonValue.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		public static implicit operator JsonValue(JsonObject value)
		{
			return new JsonValue(value);
		}

		/// <summary>
		/// Converts the given JsonArray into a JsonValue.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		public static implicit operator JsonValue(JsonArray value)
		{
			return new JsonValue(value);
		}

		/// <summary>
		/// Converts the given DateTime? into a JsonValue.
		/// </summary>
		/// <remarks>
		/// The DateTime value will be stored as a string using ISO 8601 format,
		/// since JSON does not define a DateTime type.
		/// </remarks>
		/// <param name="value">The value to be converted.</param>
		public static implicit operator JsonValue(DateTime? value)
		{
			if (value == null)
			{
				return JsonValue.Null;
			}

			return new JsonValue(value.Value.ToString("o"));
		}

		/// <summary>
		/// Converts the given JsonValue into a bool.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to be converted.</param>
		public static explicit operator bool(JsonValue jsonValue)
		{
			if (jsonValue.IsBoolean)
			{
				return (bool)jsonValue.value;
			}
			else
			{
				throw new InvalidCastException("This JsonValue is not a Boolean.");
			}
		}

		/// <summary>
		/// Converts the given JsonValue into a nullable bool.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to be converted.</param>
		public static implicit operator bool?(JsonValue jsonValue)
		{
			if (jsonValue.IsNull)
			{
				return null;
			}
			else
			{
				return (bool)jsonValue;
			}
		}

		/// <summary>
		/// Converts the given JsonValue into a double.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to be converted.</param>
		public static explicit operator double(JsonValue jsonValue)
		{
			if (jsonValue.IsNumber)
			{
				return (double)jsonValue.value;
			}
			else
			{
				throw new InvalidCastException("This JsonValue is not a Number.");
			}
		}

		/// <summary>
		/// Converts the given JsonValue into a nullable double.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to be converted.</param>
		public static implicit operator double?(JsonValue jsonValue)
		{
			if (jsonValue.IsNull)
			{
				return null;
			}
			else
			{
				return (double)jsonValue;
			}
		}

		/// <summary>
		/// Converts the given JsonValue into a string.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to be converted.</param>
		public static implicit operator string(JsonValue jsonValue)
		{
			if (jsonValue.IsString || jsonValue.IsNull)
			{
				return jsonValue.value as string;
			}
			else
			{
				throw new InvalidCastException("This JsonValue is not a String.");
			}
		}

		/// <summary>
		/// Converts the given JsonValue into a JsonObject.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to be converted.</param>
		public static implicit operator JsonObject(JsonValue jsonValue)
		{
			if (jsonValue.IsJsonObject || jsonValue.IsNull)
			{
				return jsonValue.value as JsonObject;
			}
			else
			{
				throw new InvalidCastException("This JsonValue is not a JsonObject.");
			}
		}

		/// <summary>
		/// Converts the given JsonValue into a JsonArray.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to be converted.</param>
		public static implicit operator JsonArray(JsonValue jsonValue)
		{
			if (jsonValue.IsJsonArray || jsonValue.IsNull)
			{
				return jsonValue.value as JsonArray;
			}
			else
			{
				throw new InvalidCastException("This JsonValue is not a JsonArray.");
			}
		}

		/// <summary>
		/// Returns a value indicating whether the two given JsonValues are equal.
		/// </summary>
		/// <param name="a">A JsonValue to compare.</param>
		/// <param name="b">A JsonValue to compare.</param>
		public static bool operator ==(JsonValue a, JsonValue b)
		{
			return a.Type == b.Type && a.value == b.value;
		}

		/// <summary>
		/// Returns a value indicating whether the two given JsonValues are unequal.
		/// </summary>
		/// <param name="a">A JsonValue to compare.</param>
		/// <param name="b">A JsonValue to compare.</param>
		public static bool operator !=(JsonValue a, JsonValue b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Returns a JsonValue by parsing the given string.
		/// </summary>
		/// <param name="text">The JSON-formatted string to be parsed.</param>
		public static JsonValue Parse(string text)
		{
			return JsonReader.Parse(text);
		}

		/// <summary>
		/// Returns a value indicating whether this JsonValue is equal to the given object.
		/// </summary>
		/// <param name="obj">The object to test.</param>
		public override bool Equals(object obj)
		{
			var jsonValue = obj as JsonValue?;

			if (jsonValue == null)
			{
				return this.IsNull;
			}
			else
			{
				return this == jsonValue.Value;
			}
		}

		/// <summary>
		/// Returns a hash code for this JsonValue.
		/// </summary>
		public override int GetHashCode()
		{
			if (this.IsNull)
			{
				return this.Type.GetHashCode();
			}
			else
			{
				return this.Type.GetHashCode() ^ this.value.GetHashCode();
			}
		}

		/// <summary>
		/// Returns a JSON string representing the state of the object.
		/// </summary>
		/// <remarks>
		/// The resulting string is safe to be inserted as is into dynamically
		/// generated JavaScript or JSON code.
		/// </remarks>
		public override string ToString()
		{
			return ToString(false);
		}

		/// <summary>
		/// Returns a JSON string representing the state of the object.
		/// </summary>
		/// <remarks>
		/// The resulting string is safe to be inserted as is into dynamically
		/// generated JavaScript or JSON code.
		/// </remarks>
		/// <param name="pretty">
		/// Indicates whether the resulting string should be formatted for human-readability.
		/// </param>
		public string ToString(bool pretty)
		{
			using (var reader = new JsonWriter(pretty))
			{
				return reader.Serialize(this);
			}
		}

		private class JsonValueDebugView
		{
			private JsonValue jsonValue;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public JsonObject ObjectView
			{
				get
				{
					if (jsonValue.IsJsonObject)
					{
						return (JsonObject)jsonValue;
					}
					else
					{
						return null;
					}
				}
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public JsonArray ArrayView
			{
				get
				{
					if (jsonValue.IsJsonArray)
					{
						return (JsonArray)jsonValue;
					}
					else
					{
						return null;
					}
				}
			}

			public JsonValueType Type
			{
				get
				{
					return jsonValue.Type;
				}
			}

			public object Value
			{
				get
				{
					if (jsonValue.IsJsonObject)
					{
						return (JsonObject)jsonValue;
					}
					else if (jsonValue.IsJsonArray)
					{
						return (JsonArray)jsonValue;
					}
					else
					{
						return jsonValue;
					}
				}
			}

			public JsonValueDebugView(JsonValue jsonValue)
			{
				this.jsonValue = jsonValue;
			}
		}
	}
}
