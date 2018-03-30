using System;
using AppRopio.Base.Settings.iOS.Models;
using AppRopio.Base.iOS.Services.ThemeConfig;
using AppRopio.Base.Settings.Core;

namespace AppRopio.Base.Settings.iOS.Services.Implementation
{
    public class SettingsThemeConfigService : BaseThemeConfigService<SettingsThemeConfig>, ISettingsThemeConfigService
    {
		protected override string ConfigName
		{
			get
			{
				return SettingsConstants.CONFIG_NAME;
			}
		}
    }
}