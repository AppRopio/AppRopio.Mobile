using System;
using AppRopio.Analytics.GoogleAnalytics.Core.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Analytics.GoogleAnalytics.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IGoogleAnalytics>(() => new Services.GoogleAnalytics());
        }
    }
}
