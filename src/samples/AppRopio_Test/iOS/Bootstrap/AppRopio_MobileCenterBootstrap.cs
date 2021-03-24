using System;
using MvvmCross.Plugin;

namespace AppRopio_Test.Bootstrap
{
    public class AppRopio_MobileCenterBootstrap 
        : MvxLoaderPluginBootstrapAction<AppRopio.Analytics.MobileCenter.Core.PluginLoader, AppRopio.Analytics.MobileCenter.iOS.Plugin>
    {
    }
}
