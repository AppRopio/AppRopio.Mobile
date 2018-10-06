using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class ColorPalette
    {
        #region Text

        [JsonProperty("textBase")]
        public Color TextBase { get; set; }

        [JsonProperty("textMenu")]
        public Color TextMenu { get; set; }

        [JsonProperty("textAccent")]
        public Color TextAccent { get; set; }

		[JsonProperty("textAccentHighlighted")]
		public Color TextAccentHighlighted { get; set; }

		[JsonProperty("textAccentDisabled")]
		public Color TextAccentDisabled { get; set; }

        [JsonProperty("textGray")]
        public Color TextGray { get; set; }

        [JsonProperty("textNotification")]
        public Color TextNotification { get; set; }

        #endregion

        #region Background

        [JsonProperty("background")]
        public Color Background { get; set; }

        [JsonProperty("searchBackground")]
        public Color SearchBackground { get; set; }

        [JsonProperty("backgroundMenu")]
        public Color BackgroundMenu { get; set; }

        [JsonProperty("backgroundSearch")]
        public Color BackgroundSearch { get; set; }

        [JsonProperty("backgroundNotification")]
        public Color BackgroundNotification { get; set; }

        #endregion

        #region Extra colors

        [JsonProperty("placeholder")]
        public Color Placeholder { get; set; }

        [JsonProperty("separator")]
        public Color Separator { get; set; }

        #endregion

        #region Interaction

        [JsonProperty("accent")]
        public Color Accent { get; set; }

        [JsonProperty("highlightedControl")]
        public Color HighlightedControl { get; set; }

        [JsonProperty("disabledControl")]
        public Color DisabledControl { get; set; }

        [JsonProperty("invalid")]
        public Color Invalid { get; set; }

        #endregion
    }
}
