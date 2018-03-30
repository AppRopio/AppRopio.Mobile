using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.ECommerce.Products.iOS.Models;
using Newtonsoft.Json;

namespace AppRopio.ECommerce.Marked.iOS.Models
{
    public class MarkedThemeConfig : ProductsThemeConfig
    {
        [JsonProperty("basketButton")]
        public Button BasketButton { get; private set; }

        public MarkedThemeConfig()
        {
            BasketButton = (Button)Theme.ControlPalette.Button.Base.Clone();
        }
    }
}