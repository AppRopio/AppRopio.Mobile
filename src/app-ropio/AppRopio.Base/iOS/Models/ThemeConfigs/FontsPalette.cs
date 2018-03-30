using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class FontsPalette
    {
        [JsonProperty("regular")]
        public string Regular { get; set; }

        [JsonProperty("light")]
        public string Light { get; set; }

        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("semibold")]
        public string Semibold { get; set; }

        [JsonProperty("bold")]
        public string Bold { get; set; }

        [JsonProperty("italic")]
        public string Italic { get; set; }

        public Font RegularOfSize(float size)
        {
            return new Font
            {
                Name = Regular,
                Size = size
            };
        }

        public Font LightOfSize(float size)
        {
            return new Font
            {
                Name = Light,
                Size = size
            };
        }

        public Font MediumOfSize(float size)
        {
            return new Font
            {
                Name = Medium,
                Size = size
            };
        }

        public Font SemiboldOfSize(float size)
        {
            return new Font
            {
                Name = Semibold,
                Size = size
            };
        }

        public Font BoldOfSize(float size)
        {
            return new Font
            {
                Name = Bold,
                Size = size
            };
        }

        public Font ItalicOfSize(float size)
        {
            return new Font
            {
                Name = Italic,
                Size = size
            };
        }
    }
}
