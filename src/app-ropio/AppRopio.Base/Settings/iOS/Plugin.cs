using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Settings.Core.ViewModels.Languages;
using AppRopio.Base.Settings.Core.ViewModels.Regions;
using AppRopio.Base.Settings.Core.ViewModels.Settings;
using AppRopio.Base.Settings.iOS.Services;
using AppRopio.Base.Settings.iOS.Services.Implementation;
using AppRopio.Base.Settings.iOS.Views.Languages;
using AppRopio.Base.Settings.iOS.Views.Regions;
using AppRopio.Base.Settings.iOS.Views.Settings;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Base.Settings.iOS
{
    public class Plugin : IMvxPlugin
	{
		public void Load()
		{
			Mvx.RegisterSingleton<ISettingsThemeConfigService>(() => new SettingsThemeConfigService());

			var viewLookupService = Mvx.Resolve<IViewLookupService>();

			viewLookupService.Register<ISettingsViewModel, SettingsViewController>();
            viewLookupService.Register<IRegionsViewModel, RegionsViewController>();
            viewLookupService.Register<ILanguagesViewModel, LanguagesViewController>();
		}
	}
}