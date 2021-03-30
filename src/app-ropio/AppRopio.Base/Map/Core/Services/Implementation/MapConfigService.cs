using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.Base.Map.Core.Models;
using MvvmCross;

namespace AppRopio.Base.Map.Core.Services.Implementation
{
    public class MapConfigService : IMapConfigService
    {
        #region Properties

        private MapConfig _config;
        public MapConfig Config
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private MapConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, MapConstants.CONFIG_NAME);
            var json = Mvx.IoCProvider.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MapConfig>(json);
        }

        #endregion
    }
}
