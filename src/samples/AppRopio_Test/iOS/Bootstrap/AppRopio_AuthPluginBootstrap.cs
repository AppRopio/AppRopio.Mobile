using System;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Test.iOS.Bootstrap
{
    public class AppRopio_AuthPluginBootstrap
        : MvxLoaderPluginBootstrapAction<AppRopio.Base.Auth.Core.PluginLoader, AppRopio.Base.Auth.iOS.Plugin>
    {
    }
}
