using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class Layer : ICloneable
    {
        [JsonProperty("border")]
        public Border Border { get; set; }

        [JsonProperty("cornerRadius")]
        public float? CornerRadius { get; set; }

        [JsonProperty("shadow")]
        public Shadow Shadow { get; set; }

        [JsonProperty("maskToBounds")]
        public bool? MaskToBounds { get; set; }

        [JsonProperty("background")]
        public Color Background { get; set; }

        public object Clone()
        {
            return new Layer
            {
                Border = (Border)this.Border?.Clone(),
                CornerRadius = this.CornerRadius,
                Shadow = (Shadow)this.Shadow?.Clone(),
                MaskToBounds = this.MaskToBounds,
                Background = (Color)this.Background?.Clone()
            };
        }
    }
}
