using System;

namespace LightJson
{
	public enum JsonValueType : byte
	{
		Null = 0,
		Boolean,
		Number,
		String,
		Object,
		Array,
	}
}
