using System;
namespace AppRopio.Base.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DeeplinkAttribute : Attribute
    {
        public string Scheme { get; private set; }

        public DeeplinkAttribute(string scheme)
        {
            Scheme = scheme;
        }
    }
}

