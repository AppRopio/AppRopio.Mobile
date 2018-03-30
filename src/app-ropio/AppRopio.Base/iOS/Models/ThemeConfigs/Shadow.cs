using System;
using Newtonsoft.Json;
namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class Shadow : ICloneable
    {
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }

        [JsonProperty("blur")]
        public float Blur { get; set; }

        [JsonProperty("color")]
        public Color Color { get; set; }

        [JsonProperty("opacity")]
        public float Opacity { get; set; }

        [JsonProperty("spread")]
        public float Spread { get; set; }

        public object Clone()
        {
            return new Shadow
            {
                X = this.X,
                Y = this.Y,
                Blur = this.Blur,
                Color = (Color)this.Color?.Clone(),
                Opacity = this.Opacity,
                Spread = this.Spread
            };
        }
    }


}
