using System;
using System.Diagnostics;

namespace LightJson
{
	[DebuggerDisplay("{ToString(),nq}")]
	[DebuggerTypeProxy(typeof(JsonValueDebugView))]
	public struct JsonValue
	{
		private readonly JsonValueType type;
		private readonly dynamic value;

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
					return this.value[key];
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
					this.value[key] = value;
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
					return this.value[index];
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
					this.value[index] = value;
				}
				else
				{
					throw new InvalidOperationException("This value does not represent an Array.");
				}
			}
		}

		private JsonValue(JsonValueType type, dynamic value)
		{
			this.type  = value == null ? JsonValueType.Null : type;
			this.value = value;
		}

		public static implicit operator JsonValue(bool value)
		{
			return new JsonValue(JsonValueType.Boolean, value);
		}

		public static implicit operator JsonValue(double value)
		{
			return new JsonValue(JsonValueType.Number, value);
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
					return jsonValue.value;

				case JsonValueType.Number:
					return jsonValue.value != 0;

				case JsonValueType.String:
					return jsonValue.value != "";

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
					return jsonValue.value ? 1 : 0;

				case JsonValueType.Number:
					return jsonValue.value;

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
					return jsonValue.value ? "true" : "false";

				case JsonValueType.Number:
					return jsonValue.value.ToString();

				case JsonValueType.String:
					return jsonValue.value;

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
				return jsonValue.value;
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
				return jsonValue.value;
			}
			else
			{
				throw new InvalidCastException("This value cannot be converted into a JsonArray.");
			}
		}

		public override string ToString()
		{
			switch (this.Type)
			{
				case JsonValueType.Null:
					return "null";

				case JsonValueType.Boolean:
					return this.value ? "true" : "false";

				case JsonValueType.Number:
					return this.value.ToString();

				case JsonValueType.String:
					return string.Format("\"{0}\"", this.value);

				case JsonValueType.Object:
					return string.Format("Object[{0}]", this.value.Count);

				case JsonValueType.Array:
					return string.Format("Array[{0}]", this.value.Count);

				default:
					throw new InvalidProgramException("Invalid value type.");
			}
		}

		internal class JsonValueDebugView
		{
			private JsonValue jsonValue;

			public JsonValueType Type
			{
				get
				{
					return jsonValue.Type;
				}
			}

			public dynamic Value
			{
				get
				{
					return jsonValue.value;
				}
			}

			public JsonValueDebugView(JsonValue jsonValue)
			{
				this.jsonValue = jsonValue;
			}
		}
	}
}
