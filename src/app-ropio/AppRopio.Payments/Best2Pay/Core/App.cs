using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Payments.Best2Pay.Core.Services;
using AppRopio.Payments.Best2Pay.Core.Services.Implementation;
using AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay;
using AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay.Services;
using MvvmCross;
using MvvmCross.ViewModels;

namespace AppRopio.Payments.Best2Pay.Core
{
    public class App : MvxApplication
	{
		public override void Initialize()
		{
            new API.App().Initialize();

			new Payments.API.App().Initialize();

			Mvx.IoCProvider.RegisterSingleton<IBest2PayPaymentNavigationVmService>(() => new Best2PayPaymentNavigationVmService());
            Mvx.IoCProvider.RegisterSingleton<IBest2PayVmService>(() => new Best2PayVmService());
            Mvx.IoCProvider.RegisterSingleton<IBest2PayConfigService>(() => new Best2PayConfigService());

			#region VMs registration

			var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();
            vmLookupService.Register<IBest2PayViewModel, Best2PayViewModel>();

			#endregion

			//register start point for current navigation module
			var routerService = Mvx.IoCProvider.Resolve<IRouterService>();
			routerService.Register<IBest2PayViewModel>(new Best2PayRouterSubscriber());
		}
	}
}