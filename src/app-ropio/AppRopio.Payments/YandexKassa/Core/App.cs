using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Payments.YandexKassa.Core.Services;
using AppRopio.Payments.YandexKassa.Core.Services.Implementation;
using AppRopio.Payments.YandexKassa.Core.ViewModels.YandexKassa;
using AppRopio.Payments.YandexKassa.Core.ViewModels.YandexKassa.Services;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Payments.YandexKassa.Core
{
    public class App : MvxApplication
	{
		public override void Initialize()
		{
            Mvx.IoCProvider.RegisterSingleton<IYandexKassaVmService>(() => new YandexKassaVmService());
            Mvx.IoCProvider.RegisterSingleton<IYandexKassaConfigService>(() => new YandexKassaConfigService());
            Mvx.IoCProvider.RegisterSingleton<IYandexKassaPaymentNavigationVmService>(() => new YandexKassaPaymentNavigationVmService());

			#region VMs registration

			var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();
			vmLookupService.Register<IYandexKassaViewModel, YandexKassaViewModel>();

			#endregion

			//register start point for current navigation module
			var routerService = Mvx.IoCProvider.Resolve<IRouterService>();
			routerService.Register<IYandexKassaViewModel>(new YandexKassaRouterSubscriber());

			new API.App().Initialize();
			new Payments.API.App().Initialize();
		}
	}
}
