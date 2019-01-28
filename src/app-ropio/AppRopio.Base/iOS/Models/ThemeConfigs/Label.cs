using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class Label : View
    {
        [JsonProperty("textColor")]
        public Color TextColor { get; set; }

        [JsonProperty("highlightedTextColor")]
        public Color HighlightedTextColor { get; set; }

        [JsonProperty("font")]
        public Font Font { get; set; }

        [JsonProperty("textAlignment")]
        public TextAlignment TextAlignment { get; set; }

        [JsonProperty("textTransform")]
        public TextTransform TextTransform { get; set; }

        [JsonProperty("textDecoration")]
        public TextDecoration TextDecoration { get; set; }

        public float? Kerning { get; set; }

        public override object Clone()
        {
            return new Label
            {
                Background = (Color)this.Background?.Clone(),
                Layer = (Layer)this.Layer?.Clone(),
                TextColor = (Color)this.TextColor?.Clone(),
                HighlightedTextColor = (Color)this.HighlightedTextColor?.Clone(),
                Font = (Font)this.Font?.Clone(),
                TextAlignment = this.TextAlignment,
                TextTransform = this.TextTransform,
                TextDecoration = this.TextDecoration,
                Kerning = this.Kerning ?? null
            };
        }
    }
}
