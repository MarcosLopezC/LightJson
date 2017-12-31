using System;

namespace LightJson
{
    [AttributeUsage(AttributeTargets.Field)]
    public class JsonNameAttribute : Attribute
    {
        public readonly string Name;

        public JsonNameAttribute(string name)
        {
            Name = name;
        }
    }
}