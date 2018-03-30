using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;
using Newtonsoft.Json;

namespace AppRopio.ECommerce.Loyalty.iOS.Models
{
    public class LoyaltyThemeConfig
    {
        [JsonProperty("promo")]
        public Promo Promo { get; private set; }

        public LoyaltyThemeConfig()
        {
            Promo = new Promo();
        }
    }

    public class Promo
    {
        [JsonProperty("codeFieldSize")]
        public Size CodeFieldSize { get; private set; }

        [JsonProperty("codeField")]
        public TextField CodeField { get; private set; }

        public Promo()
        {
            CodeFieldSize = new Size { Width = DeviceInfo.ScreenWidth, Height = 82 };
            CodeField = (TextField)Theme.ControlPalette.TextField.Clone();
        }
    }
}
