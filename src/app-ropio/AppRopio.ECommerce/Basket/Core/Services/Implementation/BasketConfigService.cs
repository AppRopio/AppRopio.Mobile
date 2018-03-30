using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.ECommerce.Basket.Core.Models;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Basket.Core.Services.Implementation
{
    public class BasketConfigService : IBasketConfigService
    {
        #region Properties

        private BasketConfig _config;
        public BasketConfig Config
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private BasketConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, BasketConstants.CONFIG_NAME);
            var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<BasketConfig>(json);
        }

        #endregion
    }
}
