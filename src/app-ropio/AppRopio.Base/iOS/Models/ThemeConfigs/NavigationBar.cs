using Newtonsoft.Json;
using System;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class NavigationBar : ICloneable
    {
        [JsonProperty("translucent")]
        public bool Translucent { get; set; }

        [JsonProperty("opaque")]
        public bool Opaque { get; set; }

        [JsonProperty("backgroundColor")]
        public Color BackgroundColor { get; set; }

        [JsonProperty("tintColor")]
        public Color TintColor { get; set; }

        [JsonProperty("title")]
        public Label Title { get; set; }

        [JsonProperty("prefersLargeTitles")]
        public bool PrefersLargeTitles { get; set; }

        [JsonProperty("largeTitle")]
        public Label LargeTitle { get; set; }

        [JsonProperty("backImage")]
        public Image BackImage { get; set; }

        [JsonProperty("useCustomView")]
        public bool UseCustomView { get; set; }

        [JsonProperty("logo")]
        public Image Logo { get; set; }

        public object Clone()
        {
            return new NavigationBar
            {
                Translucent = this.Translucent,
                BackgroundColor = (Color)this.BackgroundColor?.Clone(),
                TintColor = (Color)this.TintColor?.Clone(),
                Title = (Label)this.Title?.Clone(),
                LargeTitle = (Label)this.LargeTitle?.Clone(),
                BackImage = (Image)this.BackImage?.Clone(),
                Logo = (Image)this.Logo?.Clone()
            };
        }
    }
}