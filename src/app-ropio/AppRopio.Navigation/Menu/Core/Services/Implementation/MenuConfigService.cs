using System;
using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.Navigation.Menu.Core.Models;
using MvvmCross;

namespace AppRopio.Navigation.Menu.Core.Services.Implementation
{
    public class MenuConfigService : IMenuConfigService
    {
        #region Properties

        private MenuConfig _config;
        public MenuConfig Config
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private MenuConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, MenuConstants.CONFIG_NAME);
            var json = Mvx.IoCProvider.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MenuConfig>(json);
        }

        #endregion
    }
}
