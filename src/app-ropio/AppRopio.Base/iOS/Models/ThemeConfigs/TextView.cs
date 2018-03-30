using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class TextView : Label
    {
        [JsonProperty("placeholderColor")]
        public Color PlaceholderColor { get; set; }

        [JsonProperty("tintColor")]
        public Color TintColor { get; set; }

        public override object Clone()
        {
            return new TextView
            {
                Background = (Color)this.Background?.Clone(),
                Layer = (Layer)this.Layer?.Clone(),
                TextColor = (Color)this.TextColor?.Clone(),
                HighlightedTextColor = (Color)this.HighlightedTextColor?.Clone(),
                Font = (Font)this.Font?.Clone(),
                TextAlignment = this.TextAlignment,
                TextTransform = this.TextTransform,
                Kerning = this.Kerning ?? null,

                PlaceholderColor = (Color)this.PlaceholderColor?.Clone(),
                TintColor = (Color)this.TintColor?.Clone(),
            };
        }
    }
}
