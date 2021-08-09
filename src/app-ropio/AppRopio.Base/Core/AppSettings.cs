using System;
using System.Globalization;
using System.IO;
using System.Linq;
using AppRopio.Base.Core.Models.App;
using AppRopio.Base.Core.Services.Settings;
using MvvmCross;
using Newtonsoft.Json;

using Xamarin.Essentials;

namespace AppRopio.Base.Core
{
    public static class AppSettings
    {
        private static AppConfig _config;

        public static int RequestTimeoutInSeconds { get { return _config.RequestTimeoutInSeconds; } }

        public static string ApiKey { get { return _config.ApiKey; } }

        public static string AppID { get { return _config.AppID; } }

        public static string CompanyID { get { return _config.CompanyID; } }

        public static string ErrorWhenConnectionFailed { get { return _config.ErrorWhenConnectionFailed; } }

        public static string ErrorWhenTaskCanceled { get { return _config.ErrorWhenRequestCancelled; } }

        public static string Host { get { return _config.Host; } }

        public static string AppLabel { get { return _config.AppLabel; } }

        public static string DefaultRegionID => _config.DefaultRegionID;

        public static bool PreciseCurrency => _config.PreciseCurrency;

        public static string CurrencyFormat => _config.PreciseCurrency ? "C2" : "C0";

        public static string PushToken
        {
            get { return Preferences.Get(nameof(PushToken), string.Empty); }
            set { Preferences.Set(nameof(PushToken), value); }
        }

        public static string RegionID
        {
            get { return Preferences.Get(nameof(RegionID), null); }
            set 
            {
                Preferences.Set(nameof(RegionID), value);
                if (Mvx.IoCProvider.CanResolve<AppRopio.Base.API.Services.IConnectionService>())
                {
                    var connectionService = Mvx.IoCProvider.Resolve<AppRopio.Base.API.Services.IConnectionService>();
                    connectionService.Headers["Region"] = value;
                }
            }
        }

		public static bool? IsGeolocationEnabled
        {
            get 
            {
                if (Preferences.ContainsKey(nameof(IsGeolocationEnabled)))
                    return Preferences.Get(nameof(IsGeolocationEnabled), false);
                else
                    return null;
            }
            set 
            {
                if (value != null)
                    Preferences.Set(nameof(IsGeolocationEnabled), value.Value);
            }
        }

        public static bool? IsNotificationsEnabled
        {
			get
			{
				if (Preferences.ContainsKey(nameof(IsNotificationsEnabled)))
					return Preferences.Get(nameof(IsNotificationsEnabled), false);
				else
					return null;
			}
			set 
            {
                if (value != null)
                    Preferences.Set(nameof(IsNotificationsEnabled), value.Value);
            }
        }

        public static CultureInfo SettingsCulture
        {
            get
            {
                var cultureName = Preferences.Get(nameof(SettingsCulture), string.Empty);
                if (!string.IsNullOrEmpty(cultureName))
                {
                    var cultureInfo = new CultureInfo(cultureName);
                    if (!_config.Localizations.IsNullOrEmpty())
                    {
                        if (_config.Localizations.TryGetValue(cultureName, out Localization locale) && !string.IsNullOrEmpty(locale.CurrencySymbol))
                        {
                            cultureInfo.NumberFormat.CurrencySymbol = locale.CurrencySymbol;
                        }
                    }
                    return cultureInfo;
                }
                return CultureInfo.CurrentUICulture;
            }
            set { Preferences.Set(nameof(SettingsCulture), value.Name); }
        }

        static AppSettings()
        {
            LoadFromAppConfig();
        }

        private static void LoadFromAppConfig()
        {
            var json = Mvx.IoCProvider.Resolve<ISettingsService>().ReadStringFromFile(Path.Combine(CoreConstants.CONFIGS_FOLDER, CoreConstants.CONFIG_NAME));
            _config = JsonConvert.DeserializeObject<AppConfig>(json);

            if (AppSettings.SettingsCulture == CultureInfo.CurrentUICulture)
            {
                if (!_config.Localizations.IsNullOrEmpty())
                {
                    var locale = _config.Localizations.FirstOrDefault();
                    if (!string.IsNullOrEmpty(locale.Key))
                    {
                        AppSettings.SettingsCulture = new CultureInfo(locale.Key);
                    }
                }
            }
        }
    }
}