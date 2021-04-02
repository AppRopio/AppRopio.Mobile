using System;
using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.ECommerce.Products.Core.Models;
using MvvmCross;

namespace AppRopio.ECommerce.Products.Core.Services.Implementation
{
    public class ProductConfigService : IProductConfigService
    {
        #region Properties

        private ProductsConfig _config;
        public ProductsConfig Config
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private ProductsConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, ProductsConstants.CONFIG_NAME);
            var json = Mvx.IoCProvider.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ProductsConfig>(json);
        }

        #endregion
    }
}

