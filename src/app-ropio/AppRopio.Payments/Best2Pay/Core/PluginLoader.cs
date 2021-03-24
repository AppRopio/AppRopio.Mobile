using System;
using MvvmCross;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugin;

namespace AppRopio.Payments.Best2Pay.Core
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

			new AppRopio.Payments.API.App().Initialize();

			new App().Initialize();

			var manager = Mvx.Resolve<IMvxPluginManager>();
			manager.EnsurePlatformAdaptionLoaded<PluginLoader>();

			MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Best2Pay plugin is loaded");

			_loaded = true;
		}
	}
}