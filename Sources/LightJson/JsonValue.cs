using System;
using System.Diagnostics;

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
		/// Converts the given nullable boolean into a JsonValue.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		public static implicit operator JsonValue(bool? value)
		{
			if (value == null)
			{
				return JsonValue.Null;
			}
			else
			{
				return value.Value;
			}
		}

		/// <summary>
		/// Converts the given nullable double into a JsonValue.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		public static implicit operator JsonValue(double? value)
		{
			if (value == null)
			{
				return JsonValue.Null;
			}
			else
			{
				return value.Value;
			}
		}

		/// <summary>
		/// Converts the given string into a JsonValue.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		public static implicit operator JsonValue(string value)
		{
			return new JsonValue(JsonValueType.String, value);
		}

		/// <summary>
		/// Converts the given JsonObject into a JsonValue.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		public static implicit operator JsonValue(JsonObject value)
		{
			return new JsonValue(JsonValueType.Object, value);
		}

		 /// <summary>
		/// Converts the given JsonArray into a JsonValue.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		public static implicit operator JsonValue(JsonArray value)
		{
			return new JsonValue(JsonValueType.Array, value);
		}

		/// <summary>
		/// Converts the given JsonValue into a bool.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to be converted.</param>
		public static explicit operator bool(JsonValue jsonValue)
		{
			switch (jsonValue.Type)
			{
				case JsonValueType.Null:
					return false;

				case JsonValueType.Boolean:
					return (bool)jsonValue.value;

				case JsonValueType.Number:
					return (double)jsonValue != 0;

				case JsonValueType.String:
					return (string)jsonValue != "";

				case JsonValueType.Object:
				case JsonValueType.Array:
					return true;

				default:
					throw new InvalidOperationException("Invalid value type.");
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
			switch (jsonValue.Type)
			{
				case JsonValueType.Null:
					return 0;

				case JsonValueType.Boolean:
					return (bool)jsonValue ? 1 : 0;

				case JsonValueType.Number:
					return (double)jsonValue.value;

				case JsonValueType.String:
				case JsonValueType.Object:
				case JsonValueType.Array:
					throw new InvalidCastException("This value cannot be converted into a double.");

				default:
					throw new InvalidOperationException("Invalid value type.");
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
			switch (jsonValue.Type)
			{
				case JsonValueType.Null:
					return null;

				case JsonValueType.Boolean:
					return (bool)jsonValue ? "true" : "false";

				case JsonValueType.Number:
					return ((double)jsonValue).ToString();

				case JsonValueType.String:
					return (string)jsonValue.value;

				case JsonValueType.Object:
				case JsonValueType.Array:
					throw new InvalidCastException("This value cannot be converted into a string.");

				default:
					throw new InvalidOperationException("Invalid value type.");
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
				throw new InvalidCastException("This value cannot be converted into an JsonObject.");
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
				throw new InvalidCastException("This value cannot be converted into a JsonArray.");
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
		/// <remarks>
		/// This method is intended to produced user-readable string for the debugger.
		/// To a string representation of a JsonValue, use the cast operator instead.
		/// </remarks>
		public override string ToString()
		{
			switch (this.Type)
			{
				case JsonValueType.Null:
					return "null";

				case JsonValueType.Boolean:
					return (bool)this ? "true" : "false";

				case JsonValueType.Number:
					return this.value.ToString();

				case JsonValueType.String:
					return string.Format("\"{0}\"", this.value);

				case JsonValueType.Object:
					return string.Format("Object[{0}]", ((JsonObject)this).Count);

				case JsonValueType.Array:
					return string.Format("Array[{0}]", ((JsonArray)this).Count);

				default:
					throw new InvalidProgramException("Invalid value type.");
			}
		}

		internal class JsonValueDebugView
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
