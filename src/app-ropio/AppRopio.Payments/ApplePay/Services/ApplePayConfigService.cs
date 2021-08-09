using System;
using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.Payments.ApplePay.Models;
using MvvmCross;

namespace AppRopio.Payments.ApplePay.Services
{
    public class ApplePayConfigService: IApplePayConfigService
    {
    #region Properties

        private ApplePayConfig _config;
        public ApplePayConfig Config
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private ApplePayConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, ApplePayConstants.CONFIG_NAME);
            var json = Mvx.IoCProvider.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ApplePayConfig>(json);
        }

        #endregion
    }
}
