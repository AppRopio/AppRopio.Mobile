using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class Size : ICloneable
    {
        [JsonProperty("width")]
        public float? Width { get; set; }

        [JsonProperty("height")]
        public float? Height { get; set; }

        public object Clone()
        {
            return new Size
            {
                Width = this.Width,
                Height = this.Height
            };
        }
    }
}
