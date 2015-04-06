using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LightJson
{
	/// <summary>
	/// Represents an ordered collection of JsonValues.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(JsonArrayDebugView))]
	public sealed class JsonArray : IEnumerable<JsonValue>
	{
		private IList<JsonValue> items;

		/// <summary>
		/// Gets the number of values in this collection.
		/// </summary>
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		/// <summary>
		/// Gets or sets the value at the given index.
		/// </summary>
		/// <param name="index">The zero-based index of the value to get or set.</param>
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

		/// <summary>
		/// Initializes a new instance of JsonArray.
		/// </summary>
		public JsonArray()
		{
			this.items = new List<JsonValue>();
		}

		/// <summary>
		/// Initializes a new instance of JsonArray, adding the given values to the collection.
		/// </summary>
		/// <param name="values">The values to be added to this collection.</param>
		public JsonArray(params JsonValue[] values) : this()
		{
			foreach (var value in values)
			{
				this.items.Add(value);
			}
		}

		/// <summary>
		/// Adds the given value to this collection.
		/// </summary>
		/// <param name="value">The value to be added.</param>
		/// <returns>Returns this collection.</returns>
		public JsonArray Add(JsonValue value)
		{
			this.items.Add(value);
			return this;
		}

		/// <summary>
		/// Inserts the given value at the given index in this collection.
		/// </summary>
		/// <param name="index">The index where the given value will be inserted.</param>
		/// <param name="value">The value to be inserted into this collection.</param>
		/// <returns>Returns this collection.</returns>
		public JsonArray Insert(int index, JsonValue value)
		{
			this.items.Insert(index, value);
			return this;
		}

		/// <summary>
		/// Removes the value at the given index.
		/// </summary>
		/// <param name="index">The index of the value to be removed.</param>
		/// <returns>Return this collection.</returns>
		public JsonArray Remove(int index)
		{
			this.items.RemoveAt(index);
			return this;
		}

		/// <summary>
		/// Clears the contents of this collection.
		/// </summary>
		/// <returns>Returns this collection.</returns>
		public JsonArray Clear()
		{
			this.items.Clear();
			return this;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<JsonValue> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Returns a string representation of this JsonArray.
		/// </summary>
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
