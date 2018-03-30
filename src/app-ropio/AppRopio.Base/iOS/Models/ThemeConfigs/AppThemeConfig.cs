using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class AppThemeConfig
    {
        [JsonProperty("controlPalette")]
        public ControlPalette ControlPalette { get; set; }

        [JsonProperty("colorPalette")]
        public ColorPalette ColorPalette { get; set; }

        [JsonProperty("fontsPalette")]
        public FontsPalette FontsPalette { get; set; }
    }
}
