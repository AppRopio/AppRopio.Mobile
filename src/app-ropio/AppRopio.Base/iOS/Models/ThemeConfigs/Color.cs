using System;
using Newtonsoft.Json;
using UIKit;
using AppRopio.Base.iOS.UIExtentions;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class Color : ICloneable
    {
        [JsonProperty("R")]
        public int? R { get; set; }

        [JsonProperty("G")]
        public int? G { get; set; }

        [JsonProperty("B")]
        public int? B { get; set; }

        [JsonProperty("alpha")]
        public int? Alpha { get; set; }

        [JsonProperty("HEX")]
        public string Hex { get; set; }

        [JsonProperty("ref")]
        public string Reference { get; set; }

        public static explicit operator UIColor(Color v)
        {
            if (v == null)
                return null;

            if (!string.IsNullOrEmpty(v.Hex))
                return v.Hex.ToUIColor();

            if (!string.IsNullOrEmpty(v.Reference) && v.Reference.StartsWith("@", StringComparison.InvariantCulture))
            {
                var slittedString = v.Reference.Replace("@", "").Split('/');

                var namePart = slittedString[0];
                var valuePart = slittedString[1];

                if (namePart == nameof(Color).ToLower())
                {
                    var property = Theme.ColorPalette
                                        .GetType()
                                        .GetProperties()
                                        .FirstOrDefault(x => x.GetCustomAttribute<JsonPropertyAttribute>().PropertyName == valuePart);

                    var color = ((Color)property.GetValue(Theme.ColorPalette));

                    if (v.Alpha.HasValue)
                        color.Alpha = v.Alpha;

                    return (UIColor)color;
                }
            }

            return v.R.HasValue && v.G.HasValue && v.B.HasValue ?
                    UIColor.FromRGBA(v.R.Value, v.G.Value, v.B.Value, v.Alpha ?? 255)
                    :
                    UIColor.Clear;
        }

        public object Clone()
        {
            return new Color
            {
                R = this.R,
                G = this.G,
                B = this.B,
                Alpha = this.Alpha,
                Hex = this.Hex,
                Reference = this.Reference
            };
        }
    }
}
