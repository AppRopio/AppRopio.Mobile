using System;
using System.IO;
using AppRopio.Base.Core.Services.Settings;
using MvvmCross.Platform;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Services.ThemeConfig
{
    public abstract class BaseThemeConfigService<TConfig> where TConfig : class
    {
        #region Properties

        private TConfig _config;
        public TConfig ThemeConfig
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        protected abstract string ConfigName { get; }

        #endregion

        #region Private

        private TConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(iOSConstants.THEME_CONFIGS_FOLDER, ConfigName);
            var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(path);
            return json.IsNullOrEmtpy() ? default(TConfig) : JsonConvert.DeserializeObject<TConfig>(json, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        #endregion
    }
}
