using System;
using MvvmCross.Platform.Plugins;
using AppRopio.ECommerce.HistoryOrders.API;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace AppRopio.ECommerce.HistoryOrders.Core
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

			new API.App().Initialize();

			new App().Initialize();

			var manager = Mvx.Resolve<IMvxPluginManager>();
			manager.EnsurePlatformAdaptionLoaded<PluginLoader>();

			MvxTrace.Trace(MvxTraceLevel.Diagnostic, "HistoryOrders plugin is loaded");

			_loaded = true;
		}
	}
}
