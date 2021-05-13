using System.Threading.Tasks;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Settings.Core.Services;
using AppRopio.Base.Settings.Core.Services.Implementation;
using AppRopio.Base.Settings.Core.ViewModels.Languages;
using AppRopio.Base.Settings.Core.ViewModels.Regions;
using AppRopio.Base.Settings.Core.ViewModels.Services;
using AppRopio.Base.Settings.Core.ViewModels.Settings;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Settings.Core
{
    public class App : MvxApplication
	{
		public override void Initialize()
		{
            new API.App().Initialize();

			Mvx.IoCProvider.RegisterSingleton<ISettingsVmService>(() => new SettingsVmService());
            Mvx.IoCProvider.RegisterSingleton<ISettingsConfigService>(() => new SettingsConfigService());
            Mvx.IoCProvider.RegisterSingleton<IRegionService>(() => new RegionService());

            Mvx.IoCProvider.RegisterType<ISettingsVmNavigationService>(() => new SettingsVmNavigationService());

			#region VMs registration

			var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();

			vmLookupService.Register<ISettingsViewModel, SettingsViewModel>();
			vmLookupService.Register<IRegionsViewModel, RegionsViewModel>();
            vmLookupService.Register<ILanguagesViewModel, LanguagesViewModel>();

			#endregion

			//register start point for current navigation module
			var routerService = Mvx.IoCProvider.Resolve<IRouterService>();
			routerService.Register<ISettingsViewModel>(new SettingsRouterSubscriber());

			Mvx.IoCProvider.CallbackWhenRegistered<IMvxAppStart>(startup =>
			{
				Task.Run(() =>
				{
					while (!startup.IsStarted);
					Mvx.IoCProvider.Resolve<IRegionService>().CheckRegion();
				});
			});
		}
	}
}
