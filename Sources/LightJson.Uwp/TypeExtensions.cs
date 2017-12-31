using System;
using System.Linq;
using System.Reflection;

namespace LightJson
{
    public static class TypeExtensions
    {
        public static bool IsPrimitiveType(this Type @this)
        {
            return @this.GetTypeInfo().IsPrimitive;
        }

        public static T[] Attributes<T>(this FieldInfo @this) where T : Attribute
        {
            return @this.GetCustomAttributes<T>().ToArray();
        }
    }
}