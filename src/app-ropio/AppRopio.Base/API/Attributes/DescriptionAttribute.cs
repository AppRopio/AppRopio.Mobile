using System;
namespace AppRopio.Base.API.Attributes
{
    public class DescriptionAttribute : Attribute
    {
        public string Text { get; private set; }
        
        public DescriptionAttribute(string text)
        {
            Text = text;
        }
    }
}
