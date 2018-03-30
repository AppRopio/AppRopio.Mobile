using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.ECommerce.HistoryOrders.Core.Services;
using AppRopio.ECommerce.HistoryOrders.Core.Services.Implementation;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.HistoryOrders.Core
{
    public class App : MvxApplication
	{
		public override void Initialize()
		{
			Mvx.RegisterSingleton<IHistoryOrdersConfigService>(() => new HistoryOrdersConfigService());

			Mvx.RegisterSingleton<IHistoryOrdersVmService>(() => new HistoryOrdersVmService());

            Mvx.RegisterSingleton<IHistoryOrderDetailsVmService>(() => new HistoryOrderDetailsVmService());

            Mvx.RegisterSingleton<IHistoryOrdersNavigationVmService>(() => new HistoryOrdersNavigationVmService());

			#region VMs registration

			var vmLookupService = Mvx.Resolve<IViewModelLookupService>();

			vmLookupService.Register<IHistoryOrdersViewModel, HistoryOrdersViewModel>();
            vmLookupService.Register<IHistoryOrderDetailsViewModel, HistoryOrderDetailsViewModel>();
            vmLookupService.Register<IHistoryOrderProductsViewModel, HistoryOrderProductsViewModel>();

			#endregion

			//register start point for current navigation module
			var routerService = Mvx.Resolve<IRouterService>();
			routerService.Register<IHistoryOrdersViewModel>(new HistoryOrdersRouterSubscriber());
		}
	}
}
