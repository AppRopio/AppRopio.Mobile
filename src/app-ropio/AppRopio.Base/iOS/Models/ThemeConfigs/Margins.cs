using System;
using Newtonsoft.Json;
using UIKit;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class Margins : ICloneable
    {
        [JsonProperty("left")]
        public int Left { get; set; }

        [JsonProperty("top")]
        public int Top { get; set; }

        [JsonProperty("right")]
        public int Right { get; set; }

        [JsonProperty("bottom")]
        public int Bottom { get; set; }

        public static explicit operator UIEdgeInsets(Margins v)
        {
            if (v == null)
                return UIEdgeInsets.Zero;

            return new UIEdgeInsets(v.Top, v.Left, v.Bottom, v.Right);
        }

        public object Clone()
        {
            return new Margins
            {
                Left = this.Left,
                Top = this.Top,
                Right = this.Right,
                Bottom = this.Bottom
            };
        }
    }
}
