using System;

namespace LightJson
{
    [AttributeUsage(AttributeTargets.Field)]
    public class JsonProperty : Attribute
    {
        public readonly string Name;

        public JsonProperty(string name)
        {
            Name = name;
        }
    }
}