using System;
using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.Base.Filters.Core.Models;
using MvvmCross;

namespace AppRopio.Base.Filters.Core.Services.Implementation
{
    public class FiltersConfigService : IFiltersConfigService
    {
        #region Properties

        private FiltersConfig _config;
        public FiltersConfig Config
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private FiltersConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, FiltersConstants.CONFIG_NAME);
            var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<FiltersConfig>(json);
        }

        #endregion
    }
}
