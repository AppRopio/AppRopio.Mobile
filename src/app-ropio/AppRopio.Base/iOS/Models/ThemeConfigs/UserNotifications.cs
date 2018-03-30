using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class UserNotifications
    {
        [JsonProperty("error")]
        public IconText Error { get; set; }

        [JsonProperty("alert")]
        public IconText Alert { get; set; }

        [JsonProperty("confirm")]
        public TextButton Confirm { get; set; }

        public class IconText
        {
            public Image Icon { get; set; }

            public Label Text { get; set; }

            public Color Background { get; set; }
        }

        public class TextButton
        {
            public Label Text { get; set; }

            public Button Button { get; set; }

            public Color Background { get; set; }
        }
    }
}