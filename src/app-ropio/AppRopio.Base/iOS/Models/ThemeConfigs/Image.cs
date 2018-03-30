using Newtonsoft.Json;
using System;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class Image : ICloneable
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("highlightedPath")]
        public string HighlightedPath { get; set; }

        [JsonProperty("states")]
        public States States { get; set; }

        [JsonProperty("mask")]
        public Color Mask { get; set; }

        public Layer Layer { get; set; }

        public object Clone()
        {
			return new Image
			{
				Path = this.Path,
				HighlightedPath = this.HighlightedPath,
                States = (States)this.States?.Clone(),
                Mask = (Color)this.Mask?.Clone(),
                Layer = (Layer)this.Layer?.Clone()
            };
        }
    }
}