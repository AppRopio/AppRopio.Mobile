using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class ContentInsets : ICloneable
    {
        [JsonProperty("left")]
        public int Left { get; set; }

        [JsonProperty("top")]
        public int Top { get; set; }

        [JsonProperty("right")]
        public int Right { get; set; }

        [JsonProperty("bottom")]
        public int Bottom { get; set; }

        public object Clone()
        {
            return new ContentInsets
            {
                Left = this.Left,
                Top = this.Top,
                Right = this.Right,
                Bottom = this.Bottom
            };
        }
    }
}
