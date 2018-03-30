using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class ButtonsPalette
    {
        [JsonProperty("base")]
        public Button Base { get; set; }

        [JsonProperty("border")]
        public Button Border { get; set; }

        [JsonProperty("text")]
        public Button Text { get; set; }

        [JsonProperty("icon")]
        public Button Icon { get; set; }

        [JsonProperty("accessory")]
        public Button Accessory { get; set; }
    }
}
