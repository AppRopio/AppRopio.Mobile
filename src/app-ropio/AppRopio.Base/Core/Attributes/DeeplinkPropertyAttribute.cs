using System;
namespace AppRopio.Base.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DeeplinkPropertyAttribute : Attribute
    {
        public string Name { get; private set; }

        public DeeplinkPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
