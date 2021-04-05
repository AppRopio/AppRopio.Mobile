using System;
using System.Threading.Tasks;
using AppRopio.Base.API;
using AppRopio.Base.API.Services;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.ECommerce.HistoryOrders.API.Services;
using AppRopio.ECommerce.HistoryOrders.Core.Services;
using AppRopio.ECommerce.HistoryOrders.Core.Services.Implementation;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders.Services;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.ECommerce.HistoryOrders.Core
{
    public class App : MvxApplication
	{
		private IConnectionService _connectionService;
        protected IConnectionService ConnectionService
        {
            get
            {
                return _connectionService ?? (Mvx.IoCProvider.CanResolve<IConnectionService>() ?
                          (_connectionService = Mvx.IoCProvider.Resolve<IConnectionService>())
                              :
                          (_connectionService = new ConnectionService
                          {
                              ErrorWhenConnectionFailed = AppSettings.ErrorWhenConnectionFailed,
                              ErrorWhenTaskCanceled = AppSettings.ErrorWhenTaskCanceled,
                              IsConnectionAvailable = () => Task<bool>.Factory.StartNew(() => true),//Mvx.IoCProvider.Resolve<IMvxReachability>().IsHostReachable(AppSettings.Host)),
                              RequestTimeoutInSeconds = AppSettings.RequestTimeoutInSeconds,
                              BaseUrl = new Uri(AppSettings.Host)
                          }));
            }
        }

        public override void Initialize()
		{
            if (ApiSettings.DebugServiceEnabled)
                Mvx.IoCProvider.RegisterType<IHistoryOrdersService>(() => new API.Services.Fakes.HistoryOrdersFakeService());
            else
                Mvx.IoCProvider.RegisterType<IHistoryOrdersService>(() => new API.Services.Implementation.HistoryOrdersService(ConnectionService));

            Mvx.IoCProvider.RegisterSingleton<IHistoryOrdersConfigService>(() => new HistoryOrdersConfigService());

			Mvx.IoCProvider.RegisterSingleton<IHistoryOrdersVmService>(() => new HistoryOrdersVmService());

            Mvx.IoCProvider.RegisterSingleton<IHistoryOrderDetailsVmService>(() => new HistoryOrderDetailsVmService());

            Mvx.IoCProvider.RegisterSingleton<IHistoryOrdersNavigationVmService>(() => new HistoryOrdersNavigationVmService());

			#region VMs registration

			var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();

			vmLookupService.Register<IHistoryOrdersViewModel, HistoryOrdersViewModel>();
            vmLookupService.Register<IHistoryOrderDetailsViewModel, HistoryOrderDetailsViewModel>();
            vmLookupService.Register<IHistoryOrderProductsViewModel, HistoryOrderProductsViewModel>();

			#endregion

			//register start point for current navigation module
			var routerService = Mvx.IoCProvider.Resolve<IRouterService>();
			routerService.Register<IHistoryOrdersViewModel>(new HistoryOrdersRouterSubscriber());
        }
	}
}
