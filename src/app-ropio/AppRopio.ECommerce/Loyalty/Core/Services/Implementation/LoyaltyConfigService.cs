using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.ECommerce.Loyalty.Core.Models;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Loyalty.Core.Services.Implementation
{
    public class LoyaltyConfigService : ILoyaltyConfigService
    {
        #region Properties

        private LoyaltyConfig _config;
        public LoyaltyConfig Config
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private LoyaltyConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, LoyaltyConstants.CONFIG_NAME);
            var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<LoyaltyConfig>(json);
        }

        #endregion
    }
}
