using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LightJson
{
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(JsonArrayDebugView))]
	public sealed class JsonArray : IEnumerable<JsonValue>
	{
		private IList<JsonValue> items;

		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		public JsonValue this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				this.items[index] = value;
			}
		}

		public JsonArray()
		{
			this.items = new List<JsonValue>();
		}

		public JsonArray(params JsonValue[] values) : this()
		{
			foreach (var value in values)
			{
				this.items.Add(value);
			}
		}

		public JsonArray Add(JsonValue value)
		{
			this.items.Add(value);
			return this;
		}

		public JsonArray Insert(int index, JsonValue value)
		{
			this.items.Insert(index, value);
			return this;
		}

		public JsonArray Remove(int index)
		{
			this.items.RemoveAt(index);
			return this;
		}

		public JsonArray Clear()
		{
			this.items.Clear();
			return this;
		}

		public IEnumerator<JsonValue> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public override string ToString()
		{
			return string.Format("Array[{0}]", this.Count);
		}

		internal class JsonArrayDebugView
		{
			private JsonArray jsonArray;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public JsonValue[] Items
			{
				get
				{
					var items = new JsonValue[this.jsonArray.Count];

					for (int i = 0; i < this.jsonArray.Count; i += 1)
					{
						items[i] = this.jsonArray[i];
					}

					return items;
				}
			}

			public JsonArrayDebugView(JsonArray jsonArray)
			{
				this.jsonArray = jsonArray;
			}
		}
	}
}
