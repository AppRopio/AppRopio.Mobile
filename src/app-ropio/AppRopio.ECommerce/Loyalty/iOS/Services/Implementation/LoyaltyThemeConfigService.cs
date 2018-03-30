using AppRopio.Base.iOS.Services.ThemeConfig;
using AppRopio.ECommerce.Loyalty.iOS.Models;
using AppRopio.ECommerce.Loyalty.Core;

namespace AppRopio.ECommerce.Loyalty.iOS.Services.Implementation
{
    public class LoyaltyThemeConfigService : BaseThemeConfigService<LoyaltyThemeConfig>, ILoyaltyThemeConfigService
    {
        protected override string ConfigName
        {
            get { return LoyaltyConstants.CONFIG_NAME; }
        }
    }
}
