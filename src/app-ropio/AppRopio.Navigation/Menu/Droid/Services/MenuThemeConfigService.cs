using AppRopio.Base.Droid.Services.ThemeConfig;
using AppRopio.Navigation.Menu.Core;
using AppRopio.Navigation.Menu.Droid.Models;

namespace AppRopio.Navigation.Menu.Droid.Services
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
