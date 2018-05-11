using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Settings.Core.Services;
using AppRopio.Base.Settings.Core.Services.Implementation;
using AppRopio.Base.Settings.Core.ViewModels.Languages;
using AppRopio.Base.Settings.Core.ViewModels.Regions;
using AppRopio.Base.Settings.Core.ViewModels.Services;
using AppRopio.Base.Settings.Core.ViewModels.Settings;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Base.Settings.Core
{
    public class App : MvxApplication
	{
		public override void Initialize()
		{
			Mvx.RegisterSingleton<ISettingsVmService>(() => new SettingsVmService());
            Mvx.RegisterSingleton<ISettingsConfigService>(() => new SettingsConfigService());
            Mvx.RegisterSingleton<IRegionService>(() => new RegionService());
            Mvx.RegisterSingleton<ISettingsVmNavigationService>(new SettingsVmNavigationService());

			#region VMs registration

			var vmLookupService = Mvx.Resolve<IViewModelLookupService>();

			vmLookupService.Register<ISettingsViewModel, SettingsViewModel>();
			vmLookupService.Register<IRegionsViewModel, RegionsViewModel>();
            vmLookupService.Register<ILanguagesViewModel, LanguagesViewModel>();

			#endregion

			//register start point for current navigation module
			var routerService = Mvx.Resolve<IRouterService>();
			routerService.Register<ISettingsViewModel>(new SettingsRouterSubscriber());
		}
	}
}
