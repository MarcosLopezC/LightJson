using System;

namespace LightJson
{
    /// <summary>
    /// Marks fields that serialization should ignore.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class JsonIgnoreAttribute : Attribute
    {
        //
    }
}