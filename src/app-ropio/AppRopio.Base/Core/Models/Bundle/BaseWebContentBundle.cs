using System;
using AppRopio.Base.Core.Models.Navigation;
namespace AppRopio.Base.Core.Models.Bundle
{
    public class BaseWebContentBundle : BaseBundle
    {
        public string Url { get; set; }

        public string HtmlContent { get; set; }

        public string Title { get; set; }

        public BaseWebContentBundle()
        {
        }

        public BaseWebContentBundle(NavigationType navigationType, string title, string htmlContent = null, string url = null)
            : base(navigationType, new System.Collections.Generic.Dictionary<string, string>
            {
                { nameof(Title), title },
                { nameof(HtmlContent), htmlContent },
                { nameof(Url), url }
            })
        {
            Url = url;
            HtmlContent = HtmlContent;
            Title = title;
        }
    }
}
