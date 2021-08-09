using System;
using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using MvvmCross;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        #region Properties

        private static AppThemeConfig _config;
        private static AppThemeConfig ThemeConfig
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private static AppThemeConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(iOSConstants.THEME_CONFIGS_FOLDER, CoreConstants.CONFIG_NAME);
            var settingsService = new Services.Settings.SettingsService();
            var json = settingsService.ReadStringFromFile(path);
            return JsonConvert.DeserializeObject<AppThemeConfig>(json);
        }

        #endregion
    }
}
