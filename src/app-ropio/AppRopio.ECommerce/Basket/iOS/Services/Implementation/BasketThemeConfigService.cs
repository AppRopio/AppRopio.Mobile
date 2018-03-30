using AppRopio.Base.iOS.Services.ThemeConfig;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.iOS.Models;
namespace AppRopio.ECommerce.Basket.iOS.Services.Implementation
{
    public class BasketThemeConfigService : BaseThemeConfigService<BasketThemeConfig>, IBasketThemeConfigService
    {
        protected override string ConfigName
        {
            get
            {
                return BasketConstants.CONFIG_NAME;
            }
        }
    }
}
