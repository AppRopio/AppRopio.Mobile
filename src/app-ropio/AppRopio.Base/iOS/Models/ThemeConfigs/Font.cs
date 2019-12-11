using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UIKit;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class Font : ICloneable
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("size")]
        public float Size { get; set; }

        [JsonProperty("ref")]
        public string Reference { get; set; }

        public static explicit operator UIFont(Font v)
        {
            if (v == null)
                return null;

            string name = string.Empty;
            string weight = string.Empty;

            if (!string.IsNullOrEmpty(v.Reference) && v.Reference.StartsWith("@", StringComparison.InvariantCulture))
            {
                var slittedString = v.Reference.Replace("@", "").Split('/');

                var namePart = slittedString[0];
                var valuePart = slittedString[1];

                if (namePart == nameof(Font).ToLower())
                {
                    var property = Theme.FontsPalette
                                        .GetType()
                                        .GetProperties()
                                        .FirstOrDefault(x => x.GetCustomAttribute<JsonPropertyAttribute>().PropertyName == valuePart);
                    name = property.GetValue(Theme.FontsPalette) as string;
                }
                weight = valuePart;
            }
            else
            {
                name = v.Name;
            }

            if (string.IsNullOrEmpty(name))
            {
                UIFont font;
                if (!string.IsNullOrEmpty(weight))
                {
                    var fontWeight = StringToWeight(weight);
                    font = UIFont.SystemFontOfSize(v.Size, fontWeight);

                    if (weight.ToLower() == "italic")
                    {
                        var fontDescriptor = font.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Italic);
                        font = UIFont.FromDescriptor(fontDescriptor, 0.0f); //0.0f - keep the same size
                    }
                }
                else
                {
                    font = UIFont.SystemFontOfSize(v.Size, UIFontWeight.Regular);
                }
                return font;
            }
            else
            {
                return UIFont.FromName(name, v.Size);
            }

        }

        public object Clone()
        {
            return new Font
            {
                Name = this.Name,
                Size = this.Size,
                Reference = this.Reference
            };
        }

        private static UIFontWeight StringToWeight(string weight)
        {
            weight = weight.ToLower();
            switch (weight)
            {
                default:
                case "regular":
                    return UIFontWeight.Regular;
                case "light":
                    return UIFontWeight.Light;
                case "medium":
                    return UIFontWeight.Medium;
                case "semibold":
                    return UIFontWeight.Semibold;
                case "bold":
                    return UIFontWeight.Bold;
            }
        }
    }
}
