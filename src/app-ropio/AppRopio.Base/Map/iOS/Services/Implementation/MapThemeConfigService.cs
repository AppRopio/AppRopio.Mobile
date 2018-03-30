using System;
using AppRopio.Base.Map.iOS.Models;
using AppRopio.Base.iOS.Services.ThemeConfig;
using AppRopio.Base.Map.Core;
namespace AppRopio.Base.Map.iOS.Services.Implementation
{
    public class MapThemeConfigService : BaseThemeConfigService<MapThemeConfig>, IMapThemeConfigService
    {
        protected override string ConfigName
        {
            get
            {
                return MapConstants.CONFIG_NAME;
            }
        }
    }
}
