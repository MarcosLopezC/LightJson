using System;
using System.Diagnostics;

namespace LightJson
{
	[DebuggerDisplay("{ToString(),nq}", Type = "JsonValue({Type})")]
	[DebuggerTypeProxy(typeof(JsonValueDebugView))]
	public struct JsonValue
	{
		private readonly JsonValueType type;
		private readonly object value;

		public static readonly JsonValue Null = new JsonValue(JsonValueType.Null, null);

		public JsonValueType Type
		{
			get
			{
				return this.type;
			}
		}

		public bool IsNull
		{
			get
			{
				return this.Type == JsonValueType.Null;
			}
		}

		public bool IsBoolean
		{
			get
			{
				return this.Type == JsonValueType.Boolean;
			}
		}

		public bool IsNumber
		{
			get
			{
				return this.Type == JsonValueType.Number;
			}
		}

		public bool IsString
		{
			get
			{
				return this.Type == JsonValueType.String;
			}
		}

		public bool IsObject
		{
			get
			{
				return this.Type == JsonValueType.Object;
			}
		}

		public bool IsArray
		{
			get
			{
				return this.Type == JsonValueType.Array;
			}
		}

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

		private JsonValue(JsonValueType type, object value)
		{
			this.type  = value == null ? JsonValueType.Null : type;
			this.value = value;
		}

		public static implicit operator JsonValue(bool value)
		{
			return new JsonValue(JsonValueType.Boolean, value);
		}

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

		public static implicit operator JsonValue(double value)
		{
			return new JsonValue(JsonValueType.Number, value);
		}

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

		public static implicit operator JsonValue(string value)
		{
			return new JsonValue(JsonValueType.String, value);
		}

		public static implicit operator JsonValue(JsonObject value)
		{
			return new JsonValue(JsonValueType.Object, value);
		}

		public static implicit operator JsonValue(JsonArray value)
		{
			return new JsonValue(JsonValueType.Array, value);
		}

		public static explicit operator bool(JsonValue jsonValue)
		{
			switch (jsonValue.Type)
			{
				case JsonValueType.Null:
					return false;

				case JsonValueType.Boolean:
					return (bool)jsonValue;

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

		public static explicit operator bool?(JsonValue jsonValue)
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

		public static explicit operator double(JsonValue jsonValue)
		{
			switch (jsonValue.Type)
			{
				case JsonValueType.Null:
					return 0;

				case JsonValueType.Boolean:
					return (bool)jsonValue ? 1 : 0;

				case JsonValueType.Number:
					return (double)jsonValue;

				case JsonValueType.String:
				case JsonValueType.Object:
				case JsonValueType.Array:
					throw new InvalidCastException("This value cannot be converted into a double.");

				default:
					throw new InvalidOperationException("Invalid value type.");
			}
		}

		public static explicit operator double?(JsonValue jsonValue)
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

		public static explicit operator string(JsonValue jsonValue)
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
					return (string)jsonValue;

				case JsonValueType.Object:
				case JsonValueType.Array:
					throw new InvalidCastException("This value cannot be converted into a string.");

				default:
					throw new InvalidOperationException("Invalid value type.");
			}
		}

		public static explicit operator JsonObject(JsonValue jsonValue)
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

		public static explicit operator JsonArray(JsonValue jsonValue)
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

		public static bool operator ==(JsonValue a, JsonValue b)
		{
			return a.Type == b.type && a.value == b.value;
		}

		public static bool operator !=(JsonValue a, JsonValue b)
		{
			return !(a == b);
		}

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
