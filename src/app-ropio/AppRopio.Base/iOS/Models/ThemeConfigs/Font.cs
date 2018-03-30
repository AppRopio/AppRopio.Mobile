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

                    return UIFont.FromName((string)property.GetValue(Theme.FontsPalette), v.Size);
                }
            }

            return UIFont.FromName(v.Name, v.Size);
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
    }
}
