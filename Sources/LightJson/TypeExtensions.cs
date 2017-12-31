using System;
using System.Linq;
using System.Reflection;

namespace LightJson
{
    public static class TypeExtensions
    {
        public static bool IsPrimitiveType(this Type @this)
        {
            return @this.IsPrimitive;
        }

        public static T[] Attributes<T>(this FieldInfo @this)
        {
            return @this
                .GetCustomAttributes(typeof(T), true)
                .Cast<T>()
                .ToArray();
        }
    }
}