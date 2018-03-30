using AppRopio.Base.iOS.Services.ThemeConfig;
using AppRopio.Navigation.Menu.Core;
using AppRopio.Navigation.Menu.iOS.Models;

namespace AppRopio.Navigation.Menu.iOS.Services.Implementation
{
    public class MenuThemeConfigService : BaseThemeConfigService<MenuThemeConfig>, IMenuThemeConfigService
    {
        protected override string ConfigName
        {
            get
            {
                return MenuConstants.CONFIG_NAME;
            }
        }
    }
}
