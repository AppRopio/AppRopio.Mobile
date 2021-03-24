using System;
using AppRopio.Analytics.GoogleAnalytics.Core.Services;
using MvvmCross;
using MvvmCross.Plugin;

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
