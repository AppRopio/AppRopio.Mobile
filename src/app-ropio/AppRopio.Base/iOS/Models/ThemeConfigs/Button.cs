using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class Button : View
    {
        [JsonProperty("font")]
        public Font Font { get; set; }

        [JsonProperty("states")]
        public States States { get; set; }

        [JsonProperty("textColor")]
        public Color TextColor { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("imageInsets")]
        public Margins ImageInsets { get; set; }

        [JsonProperty("titleInsets")]
        public Margins TitleInsets { get; set; }

        [JsonProperty("uppercaseTitle")]
        public bool UppercaseTitle { get; set; }

        public override object Clone()
        {
            return new Button
            {
                Background = (Color)this.Background?.Clone(),
                Layer = (Layer)this.Layer?.Clone(),
                Font = (Font)this.Font?.Clone(),
                States = (States)this.States?.Clone(),
                TextColor = (Color)this.TextColor?.Clone(),
                Image = (Image)this.Image?.Clone(),
                ImageInsets = (Margins)this.ImageInsets?.Clone(),
                TitleInsets = (Margins)this.TitleInsets?.Clone(),
                UppercaseTitle = this.UppercaseTitle
            };
        }
    }
}
