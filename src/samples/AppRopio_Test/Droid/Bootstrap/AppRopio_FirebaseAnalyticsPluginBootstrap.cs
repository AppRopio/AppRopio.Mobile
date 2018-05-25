using System;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Test.iOS.Bootstrap
{
    public class AppRopio_FirebaseAnalyticsPluginBootstrap
        : MvxLoaderPluginBootstrapAction<AppRopio.Analytics.Firebase.Core.PluginLoader, AppRopio.Analytics.Firebase.Droid.Plugin>
    {
        
    }
}
