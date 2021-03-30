using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Settings.Core;
using AppRopio.Base.Settings.Core.ViewModels.Languages;
using AppRopio.Base.Settings.Core.ViewModels.Regions;
using AppRopio.Base.Settings.Core.ViewModels.Settings;
using AppRopio.Base.Settings.iOS.Services;
using AppRopio.Base.Settings.iOS.Services.Implementation;
using AppRopio.Base.Settings.iOS.Views.Languages;
using AppRopio.Base.Settings.iOS.Views.Regions;
using AppRopio.Base.Settings.iOS.Views.Settings;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Base.Settings.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

			Mvx.IoCProvider.RegisterSingleton<ISettingsThemeConfigService>(() => new SettingsThemeConfigService());

			var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

			viewLookupService.Register<ISettingsViewModel, SettingsViewController>();
            viewLookupService.Register<IRegionsViewModel, RegionsViewController>();
            viewLookupService.Register<ILanguagesViewModel, LanguagesViewController>();
		}
	}
}