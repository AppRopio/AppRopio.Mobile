using System;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Test.iOS.Bootstrap
{
    public class AppRopio_ContactsPluginBootstrap 
        : MvxLoaderPluginBootstrapAction<AppRopio.Base.Contacts.Core.PluginLoader, AppRopio.Base.Contacts.Droid.Plugin>
    {
    }
}
