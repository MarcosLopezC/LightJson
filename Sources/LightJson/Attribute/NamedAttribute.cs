using System;

namespace LightJson
{
    [AttributeUsage(AttributeTargets.Field)]
    public class NamedAttribute : Attribute
    {
        public readonly string Name;

        public NamedAttribute(string name)
        {
            Name = name;
        }
    }
}