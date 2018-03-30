using AppRopio.Base.Information.Core;
using AppRopio.Base.Information.iOS.Models;
using AppRopio.Base.iOS.Services.ThemeConfig;

namespace AppRopio.Base.Information.iOS.Services.Implementation
{
    public class InformationThemeConfigService : BaseThemeConfigService<InformationThemeConfig>,
                                                   IInformationThemeConfigService
    {
        protected override string ConfigName
        {
            get
            {
                return InformationConstants.CONFIG_NAME;
            }
        }
    }
}