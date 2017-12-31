using System;
using System.Linq;
using System.Reflection;

namespace LightJson
{
    /// <summary>
    /// Extensions for UWP compat.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// True iff type is a primitive.
        /// </summary>
        /// <param name="this">Type.</param>
        /// <returns></returns>
        public static bool IsPrimitiveType(this Type @this)
        {
            return @this.GetTypeInfo().IsPrimitive;
        }

        /// <summary>
        /// Retrives attributes of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of attribute.</typeparam>
        /// <param name="this">The field.</param>
        /// <returns></returns>
        public static T[] Attributes<T>(this FieldInfo @this) where T : Attribute
        {
            return @this.GetCustomAttributes<T>().ToArray();
        }
    }
}