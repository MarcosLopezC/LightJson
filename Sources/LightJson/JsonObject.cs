using System;
using System.Collections.Generic;

namespace LightJson
{
	public sealed class JsonObject : IEnumerable<KeyValuePair<string, JsonValue>>
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

		public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator()
		{
			return this.properties.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
