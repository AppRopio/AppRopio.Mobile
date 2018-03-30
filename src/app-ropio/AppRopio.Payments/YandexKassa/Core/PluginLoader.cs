using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Payments.YandexKassa.Core
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

			new App().Initialize();
            new AppRopio.Payments.YandexKassa.API.App().Initialize();
			new AppRopio.Payments.API.App().Initialize();

			var manager = Mvx.Resolve<IMvxPluginManager>();
			manager.EnsurePlatformAdaptionLoaded<PluginLoader>();

			MvxTrace.Trace(MvxTraceLevel.Diagnostic, "YandexKassa plugin is loaded");

			_loaded = true;
		}
	}
}