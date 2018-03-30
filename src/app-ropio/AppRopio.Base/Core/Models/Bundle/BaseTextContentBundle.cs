using System;
using AppRopio.Base.Core.Models.Navigation;
namespace AppRopio.Base.Core.Models.Bundle
{
    public class BaseTextContentBundle : BaseBundle
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public BaseTextContentBundle()
        {

        }

        public BaseTextContentBundle(string title, string text, NavigationType navigationType)
            : base(navigationType, new System.Collections.Generic.Dictionary<string, string>
            {
                { nameof(Title), title },
                { nameof(Text), text }
            })
        {
            Title = title;
            Text = text;
        }
    }
}
