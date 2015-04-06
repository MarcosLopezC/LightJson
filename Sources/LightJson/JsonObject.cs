using System;
using System.Collections.Generic;
using System.Diagnostics;
using LightJson.Serialization;

namespace LightJson
{
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(JsonObjectDebugView))]
	public sealed class JsonObject : IEnumerable<KeyValuePair<string, JsonValue>>, IEnumerable<JsonValue>
	{
		private IDictionary<string, JsonValue> properties;

		public int Count
		{
			get
			{
				return this.properties.Count;
			}
		}

		public JsonValue this[string key]
		{
			get
			{
				return this.properties[key];
			}
			set
			{
				this.properties[key] = value;
			}
		}

		public JsonObject()
		{
			this.properties = new Dictionary<string, JsonValue>();
		}

		public JsonObject Add(string key)
		{
			return Add(key, JsonValue.Null);
		}

		public JsonObject Add(string key, JsonValue value)
		{
			this.properties.Add(key, value);
			return this;
		}

		public JsonObject Remove(string key)
		{
			this.properties.Remove(key);
			return this;
		}

		public JsonObject Clear()
		{
			this.properties.Clear();
			return this;
		}

		public bool Contains(string key)
		{
			return this.properties.ContainsKey(key);
		}

		public string Serialize()
		{
			return Serialize(false);
		}

		public string Serialize(bool pretty)
		{
			var writer = new JsonWriter(pretty);

			return writer.Serialize(this);
		}

		public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator()
		{
			return this.properties.GetEnumerator();
		}

		IEnumerator<JsonValue> IEnumerable<JsonValue>.GetEnumerator()
		{
			return this.properties.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public override string ToString()
		{
			return string.Format("Object[{0}]", this.Count);
		}

		internal class JsonObjectDebugView
		{
			private JsonObject jsonObject;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public KeyValuePair[] Keys
			{
				get
				{
					var keys = new KeyValuePair[jsonObject.Count];

					var i = 0;
					foreach (var property in jsonObject)
					{
						keys[i] = new KeyValuePair(property.Key, property.Value);
						i += 1;
					}

					return keys;
				}
			}

			public JsonObjectDebugView(JsonObject jsonObject)
			{
				this.jsonObject = jsonObject;
			}

			[DebuggerDisplay("{value.DebuggerDisplay(),nq}", Name = "{key}", Type = "JsonValue({Type})")]
			internal class KeyValuePair
			{
				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				private string key;

				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				private JsonValue value;

				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				private JsonValueType Type
				{
					get
					{
						return value.Type;
					}
				}

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public object View
				{
					get
					{
						if (this.value.IsObject)
						{
							return (JsonObject)this.value;
						}
						else if (this.value.IsArray)
						{
							return (JsonArray)this.value;
						}
						else
						{
							return this.value;
						}
					}
				}

				public KeyValuePair(string key, JsonValue value)
				{
					this.key = key;
					this.value = value;
				}
			}
		}
	}
}
