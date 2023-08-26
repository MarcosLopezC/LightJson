using System;

namespace LightJson.Serialization
{
	/// <summary>
	/// Represents a position within a plain text resource.
	/// </summary>
#if LIGHTJSON_INTERNAL
    internal
#else
    public
#endif
    struct TextPosition
	{
		/// <summary>
		/// The column position, 0-based.
		/// </summary>
		public long column;

		/// <summary>
		/// The line position, 0-based.
		/// </summary>
		public long line;
	}
}
