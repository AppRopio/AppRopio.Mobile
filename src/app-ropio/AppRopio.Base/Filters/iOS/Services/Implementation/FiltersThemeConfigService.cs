using System;
using AppRopio.Base.iOS.Services.ThemeConfig;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.Core;

namespace AppRopio.Base.Filters.iOS.Services.Implementation
{
    public class FiltersThemeConfigService : BaseThemeConfigService<FiltersThemeConfig>, IFiltersThemeConfigService
    {
        protected override string ConfigName
        {
            get
            {
                return FiltersConstants.CONFIG_NAME;
            }
        }
    }
}
