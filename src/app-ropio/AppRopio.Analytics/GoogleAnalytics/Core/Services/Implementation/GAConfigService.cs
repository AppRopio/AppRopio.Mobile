using System;
using System.IO;
using AppRopio.Analytics.GoogleAnalytics.Core.Models.Config;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using MvvmCross;

namespace AppRopio.Analytics.GoogleAnalytics.Core.Services.Implementation
{
    public class GAConfigService : IGAConfigService
    {
        #region Fields

        protected virtual string SettingsName { get { return "GoogleAnalytics.json"; } }

        #endregion

        #region Properties

        private GAConfig _config;
        public GAConfig Config
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private GAConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, SettingsName);
            var json = Mvx.IoCProvider.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<GAConfig>(json);
        }

        #endregion
    }
}
