using System;
using System.Globalization;
using System.IO;
using System.Linq;
using AppRopio.Base.Core.Models.App;
using AppRopio.Base.Core.Services.Settings;
using MvvmCross.Platform;
using Newtonsoft.Json;
using Plugin.Settings;

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
            get { return CrossSettings.Current.GetValueOrDefault(nameof(PushToken), string.Empty); }
            set { CrossSettings.Current.AddOrUpdateValue(nameof(PushToken), value); }
        }

        public static string RegionID
        {
            get { return CrossSettings.Current.GetValueOrDefault(nameof(RegionID), null); }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(nameof(RegionID), value);
                if (Mvx.CanResolve<AppRopio.Base.API.Services.IConnectionService>())
                {
                    var connectionService = Mvx.Resolve<AppRopio.Base.API.Services.IConnectionService>();
                    connectionService.Headers["Region"] = value;
                }
            }
        }

        public static bool? IsGeolocationEnabled
        {
            get
            {
                if (CrossSettings.Current.Contains(nameof(IsGeolocationEnabled)))
                    return CrossSettings.Current.GetValueOrDefault(nameof(IsGeolocationEnabled), false);
                else
                    return null;
            }
            set
            {
                if (value != null)
                    CrossSettings.Current.AddOrUpdateValue(nameof(IsGeolocationEnabled), value.Value);
            }
        }

        public static bool? IsNotificationsEnabled
        {
            get
            {
                if (CrossSettings.Current.Contains(nameof(IsNotificationsEnabled)))
                    return CrossSettings.Current.GetValueOrDefault(nameof(IsNotificationsEnabled), false);
                else
                    return null;
            }
            set
            {
                if (value != null)
                    CrossSettings.Current.AddOrUpdateValue(nameof(IsNotificationsEnabled), value.Value);
            }
        }

        public static CultureInfo SettingsCulture
        {
            get
            {
                var cultureName = CrossSettings.Current.GetValueOrDefault(nameof(SettingsCulture), string.Empty);
                if (!string.IsNullOrEmpty(cultureName))
                {
                    var cultureInfo = new CultureInfo(cultureName);
                    if (!_config.Locales.IsNullOrEmpty())
                    {
                        var locale = _config.Locales.FirstOrDefault(l => l.Name == cultureName);
                        if (locale != null && !string.IsNullOrEmpty(locale.CurrencySymbol))
                        {
                            cultureInfo.NumberFormat.CurrencySymbol = locale.CurrencySymbol;
                        }
                    }
                    return cultureInfo;
                }
                return CultureInfo.CurrentUICulture;
            }
            set { CrossSettings.Current.AddOrUpdateValue(nameof(SettingsCulture), value.Name); }
        }

        static AppSettings()
        {
            LoadFromAppConfig();
        }

        private static void LoadFromAppConfig()
        {
            var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(Path.Combine(CoreConstants.CONFIGS_FOLDER, CoreConstants.CONFIG_NAME));
            _config = JsonConvert.DeserializeObject<AppConfig>(json);

            if (!_config.Locales.IsNullOrEmpty())
            {
                var locale = _config.Locales.FirstOrDefault();
                if (locale != null)
                {
                    AppSettings.SettingsCulture = new CultureInfo(locale.Name);
                }
            }
        }
    }
}