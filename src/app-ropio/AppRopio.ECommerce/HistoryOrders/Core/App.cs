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
                return _connectionService ?? (Mvx.CanResolve<IConnectionService>() ?
                          (_connectionService = Mvx.Resolve<IConnectionService>())
                              :
                          (_connectionService = new ConnectionService
                          {
                              ErrorWhenConnectionFailed = AppSettings.ErrorWhenConnectionFailed,
                              ErrorWhenTaskCanceled = AppSettings.ErrorWhenTaskCanceled,
                              IsConnectionAvailable = () => Task<bool>.Factory.StartNew(() => true),//Mvx.Resolve<IMvxReachability>().IsHostReachable(AppSettings.Host)),
                              RequestTimeoutInSeconds = AppSettings.RequestTimeoutInSeconds,
                              BaseUrl = new Uri(AppSettings.Host)
                          }));
            }
        }

        public override void Initialize()
		{
            if (ApiSettings.DebugServiceEnabled)
                Mvx.RegisterType<IHistoryOrdersService>(() => new API.Services.Fakes.HistoryOrdersFakeService());
            else
                Mvx.RegisterType<IHistoryOrdersService>(() => new API.Services.Implementation.HistoryOrdersService(ConnectionService));

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
