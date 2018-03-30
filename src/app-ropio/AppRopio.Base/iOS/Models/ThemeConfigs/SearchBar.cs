using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class SearchBar : ICloneable
    {
        [JsonProperty("backgroundColor")]
        public Color BackgroundColor { get; set; }

        [JsonProperty("textField")]
        public TextField TextField { get; set; }

        [JsonProperty("searchImage")]
        public Image SearchImage { get; set; }

        public object Clone()
        {
            return new SearchBar
            {
                BackgroundColor = (Color)this.BackgroundColor?.Clone(),
                TextField = (TextField)this.TextField?.Clone(),
                SearchImage = (Image)this.SearchImage?.Clone()
            };
        }
    }
}
