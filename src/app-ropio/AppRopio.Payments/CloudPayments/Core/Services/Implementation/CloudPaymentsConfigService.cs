using System;
using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.Payments.CloudPayments.Core.Models;
using MvvmCross.Platform;

namespace AppRopio.Payments.CloudPayments.Core.Services.Implementation
{
    public class CloudPaymentsConfigService : ICloudPaymentsConfigService
    {
        #region Properties

        private CloudPaymentsConfig _config;
        public CloudPaymentsConfig Config
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private CloudPaymentsConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, CloudPaymentsConstants.CONFIG_NAME);
            var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<CloudPaymentsConfig>(json);
        }

        #endregion
    }
}
