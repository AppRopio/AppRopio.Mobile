using System;
using System.IO;
using AppRopio.Analytics.AppsFlyer.Core.Models.Config;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using MvvmCross.Platform;

namespace AppRopio.Analytics.AppsFlyer.Core.Services.Implementation
{
    public class AFConfigService : IAFConfigService
    {
        #region Fields

        protected virtual string SettingsName { get { return "AppsFlyer.json"; } }

        #endregion

        #region Properties

        private AFConfig _config;
        public AFConfig Config
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private AFConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, SettingsName);
            var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AFConfig>(json);
        }

        #endregion
    }
}
