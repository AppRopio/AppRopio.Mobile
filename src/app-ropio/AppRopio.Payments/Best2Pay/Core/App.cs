using System;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Payments.Best2Pay.Core.Services;
using AppRopio.Payments.Best2Pay.Core.Services.Implementation;
using AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay;
using AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay.Services;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Payments.Best2Pay.Core
{
    public class App : MvxApplication
	{
		public override void Initialize()
		{
            Mvx.RegisterSingleton<IBest2PayPaymentNavigationVmService>(() => new Best2PayPaymentNavigationVmService());
            Mvx.RegisterSingleton<IBest2PayVmService>(() => new Best2PayVmService());
            Mvx.RegisterSingleton<IBest2PayConfigService>(() => new Best2PayConfigService());

			#region VMs registration

			var vmLookupService = Mvx.Resolve<IViewModelLookupService>();
            vmLookupService.Register<IBest2PayViewModel, Best2PayViewModel>();

			#endregion

			//register start point for current navigation module
			var routerService = Mvx.Resolve<IRouterService>();
			routerService.Register<IBest2PayViewModel>(new Best2PayRouterSubscriber());
		}
	}
}