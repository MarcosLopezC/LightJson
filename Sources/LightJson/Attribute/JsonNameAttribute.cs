using System;

namespace LightJson
{
    /// <summary>
    /// Manually sets the name of the JSON field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class JsonNameAttribute : Attribute
    {
        /// <summary>
        /// Name of the JSON field.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the JSON field.</param>
        public JsonNameAttribute(string name)
        {
            Name = name;
        }
    }
}