using System;
using MvvmCross.Plugin;
using MvvmCross;
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
