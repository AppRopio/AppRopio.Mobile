using MvvmCross;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugin;

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

			new App().Initialize();

			var manager = Mvx.Resolve<IMvxPluginManager>();
			manager.EnsurePlatformAdaptionLoaded<PluginLoader>();

			MvxTrace.Trace(MvxTraceLevel.Diagnostic, "HistoryOrders plugin is loaded");

			_loaded = true;
		}
	}
}
