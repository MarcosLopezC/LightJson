using System;
using System.Diagnostics;
using LightJson.Serialization;

namespace LightJson
{
	/// <summary>
	/// A wrapper object that contains a valid JSON value.
	/// </summary>
	[DebuggerDisplay("{DebuggerDisplay(),nq}", Type = "JsonValue({Type})")]
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
		/// Gets a value indicating whether this JsonValue is an Object.
		/// </summary>
		public bool IsObject
		{
			get
			{
				return this.Type == JsonValueType.Object;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this JsonValue is an Array.
		/// </summary>
		public bool IsArray
		{
			get
			{
				return this.Type == JsonValueType.Array;
			}
		}

		/// <summary>
		/// Gets this value as a Boolean type.
		/// </summary>
		public bool Boolean
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
		/// Gets this value as a Number type.
		/// </summary>
		public double? Number
		{
			get
			{
				switch (this.Type)
				{
					case JsonValueType.Boolean:
						return (bool)this ? 1 : 0;

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
						return null;
				}
			}
		}

		/// <summary>
		/// Gets this value as a String type.
		/// </summary>
		public string String
		{
			get
			{
				switch (this.Type)
				{
					case JsonValueType.Boolean:
						return (bool)this ? "true" : "false";

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
		/// Gets this value as an Object.
		/// </summary>
		public JsonObject Object
		{
			get
			{
				return this.IsObject ? (JsonObject)this : null;
			}
		}

		/// <summary>
		/// Gets this value as an Array.
		/// </summary>
		public JsonArray Array
		{
			get
			{
				return this.IsArray ? (JsonArray)this : null;
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
				if (this.IsObject)
				{
					return ((JsonObject)this)[key];
				}
				else
				{
					throw new InvalidOperationException("This value does not represent an Object.");
				}
			}
			set
			{
				if (this.IsObject)
				{
					((JsonObject)this)[key] = value;
				}
				else
				{
					throw new InvalidOperationException("This value does not represent an Object.");
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
				if (this.IsArray)
				{
					return ((JsonArray)this)[index];
				}
				else
				{
					throw new InvalidOperationException("This value does not represent an Array.");
				}
			}
			set
			{
				if (this.IsArray)
				{
					((JsonArray)this)[index] = value;
				}
				else
				{
					throw new InvalidOperationException("This value does not represent an Array.");
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
			this.type  = value == null ? JsonValueType.Null : type;
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
		/// Initializes a new instance of the JsonValue struct, representing an Object reference value.
		/// </summary>
		/// <param name="value">The value to be wrapped.</param>
		public JsonValue(JsonObject value) : this(JsonValueType.Object, value) { }

		/// <summary>
		/// Initializes a new instance of the JsonValue struct, representing a Array reference value.
		/// </summary>
		/// <param name="value">The value to be wrapped.</param>
		public JsonValue(JsonArray value) : this(JsonValueType.Array, value) { }

		/// <summary>
		/// Serializes the contents of this value into a JSON formatted string.
		/// </summary>
		/// <returns>Return a string representation of this value.</returns>
		public string Serialize()
		{
			return Serialize(false);
		}

		/// <summary>
		/// Serializes the contents of this value into a JSON formatted string.
		/// </summary>
		/// <param name="pretty">Indicates whether the output should be formatted to be human-readable.</param>
		/// <returns>Return a string representation of this value.</returns>
		public string Serialize(bool pretty)
		{
			using (var writer = new JsonWriter(pretty))
			{
				return writer.Serialize(this);
			}
		}

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
				throw new InvalidCastException("This value is not a Number.");
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
				throw new InvalidCastException("This value is not a String.");
			}
		}

		/// <summary>
		/// Converts the given JsonValue into a JsonObject.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to be converted.</param>
		public static implicit operator JsonObject(JsonValue jsonValue)
		{
			if (jsonValue.IsObject || jsonValue.IsNull)
			{
				return jsonValue.value as JsonObject;
			}
			else
			{
				throw new InvalidCastException("This value is not an Object.");
			}
		}

		/// <summary>
		/// Converts the given JsonValue into a JsonArray.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to be converted.</param>
		public static implicit operator JsonArray(JsonValue jsonValue)
		{
			if (jsonValue.IsArray || jsonValue.IsNull)
			{
				return jsonValue.value as JsonArray;
			}
			else
			{
				throw new InvalidCastException("This value is not an Array.");
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
		/// Converts this JsonValue into a human-readable string.
		/// </summary>
		public override string ToString()
		{
			if (this.Type == JsonValueType.Null)
			{
				return "null";
			}
			else
			{
				return this.value.ToString();
			}
		}

		private string DebuggerDisplay()
		{
			return JsonWriter.EncodeJsonValue(this);
		}

		private class JsonValueDebugView
		{
			private JsonValue jsonValue;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public JsonObject ObjectView
			{
				get
				{
					if (jsonValue.IsObject)
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
					if (jsonValue.IsArray)
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
					if (jsonValue.IsObject)
					{
						return (JsonObject)jsonValue;
					}
					else if (jsonValue.IsArray)
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
