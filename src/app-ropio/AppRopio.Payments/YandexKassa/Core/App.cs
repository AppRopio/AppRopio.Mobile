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
            Mvx.RegisterSingleton<IYandexKassaVmService>(() => new YandexKassaVmService());
            Mvx.RegisterSingleton<IYandexKassaConfigService>(() => new YandexKassaConfigService());
            Mvx.RegisterSingleton<IYandexKassaPaymentNavigationVmService>(() => new YandexKassaPaymentNavigationVmService());

			#region VMs registration

			var vmLookupService = Mvx.Resolve<IViewModelLookupService>();
			vmLookupService.Register<IYandexKassaViewModel, YandexKassaViewModel>();

			#endregion

			//register start point for current navigation module
			var routerService = Mvx.Resolve<IRouterService>();
			routerService.Register<IYandexKassaViewModel>(new YandexKassaRouterSubscriber());
		}
	}
}
