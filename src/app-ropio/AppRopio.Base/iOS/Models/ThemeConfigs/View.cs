using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class View : ICloneable
    {
        [JsonProperty("background")]
        public Color Background { get; set; }

        [JsonProperty("layer")]
        public Layer Layer { get; set; }

        public virtual object Clone()
        {
            return new View
            {
                Background = (Color)this.Background?.Clone(),
                Layer = (Layer)this.Layer?.Clone()
            };
        }
    }
}
