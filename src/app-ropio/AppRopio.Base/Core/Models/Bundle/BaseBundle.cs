using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Models.Navigation;
using MvvmCross.Core.ViewModels;
using AppRopio.Base.Core.Attributes;

namespace AppRopio.Base.Core.Models.Bundle
{
    public class BaseBundle : MvxBundle
    {
        [DeeplinkProperty("navigationType")]
        public NavigationType NavigationType { get; set; }

        public BaseBundle()
        {

        }

        public BaseBundle(NavigationType navigationType)
            : this(navigationType, new Dictionary<string, string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AppRopio.Base.Core.Models.Bundle.BaseBundle"/> class.
        /// </summary>
        /// <param name="dictionary">Dictionary with NavigationType</param>
        public BaseBundle(Dictionary<string, string> dictionary)
            : this(NavigationType.None, dictionary)
        {
        }

        public BaseBundle(NavigationType navigationType, Dictionary<string, string> dictionary)
            : base(new Dictionary<string, string>(dictionary)
            {
                { nameof(NavigationType), ((int)navigationType).ToString() }
            })
        {

        }
    }
}
