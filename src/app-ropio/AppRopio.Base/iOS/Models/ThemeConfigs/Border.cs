using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class Border : ICloneable
    {
        [JsonProperty("width")]
        public float Width { get; set; }

        [JsonProperty("color")]
        public Color Color { get; set; }

        public object Clone()
        {
            return new Border
            {
                Width = this.Width,
                Color = (Color)this.Color?.Clone()
            };
        }
    }
}
