using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class TextField : Label
    {
        [JsonProperty("placeholder")]
        public Label Placeholder { get; set; }

        [JsonProperty("tintColor")]
        public Color TintColor { get; set; }

        [JsonProperty("invalidTintColor")]
        public Color InvalidTintColor { get; set; }

        [JsonProperty("separatorColor")]
        public Color SeparatorColor { get; set; }

        public override object Clone()
        {
            return new TextField
            {
                Background = (Color)this.Background?.Clone(),
                Layer = (Layer)this.Layer?.Clone(),
                TextColor = (Color)this.TextColor?.Clone(),
                HighlightedTextColor = (Color)this.HighlightedTextColor?.Clone(),
                Font = (Font)this.Font?.Clone(),
                TextAlignment = this.TextAlignment,
                TextTransform = this.TextTransform,
                Kerning = this.Kerning ?? null,

                Placeholder = (Label)this.Placeholder?.Clone(),
                TintColor = (Color)this.TintColor?.Clone(),
                InvalidTintColor = (Color)this.InvalidTintColor?.Clone(),
                SeparatorColor = (Color)this.SeparatorColor?.Clone(),
            };
        }
    }
}
