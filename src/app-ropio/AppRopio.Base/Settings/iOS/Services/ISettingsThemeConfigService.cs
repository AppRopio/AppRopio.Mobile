using System;
using AppRopio.Base.Settings.iOS.Models;

namespace AppRopio.Base.Settings.iOS.Services
{
    public interface ISettingsThemeConfigService
    {
		SettingsThemeConfig ThemeConfig { get; }
    }
}