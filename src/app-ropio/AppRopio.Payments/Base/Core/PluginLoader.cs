using System;
using AppRopio.Base.Core;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Payments.Core
{
    public class PluginLoader : IMvxPluginLoader
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        private bool _loaded;

        public void EnsureLoaded()
        {
            if (_loaded)
            {
                return;
            }

            new AppRopio.Payments.API.App().Initialize();

            new App().Initialize();

            var manager = Mvx.Resolve<IMvxPluginManager>();
            manager.EnsurePlatformAdaptionLoaded<PluginLoader>();

            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Payments plugin is loaded");

            _loaded = true;
        }
    }
}