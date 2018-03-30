using System;
using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;
using AppRopio.Analytics.GoogleAnalytics.Core.Services;

namespace AppRopio.Analytics.GoogleAnalytics.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IGoogleAnalytics>(() => new Services.GoogleAnalytics());
        }
    }
}
