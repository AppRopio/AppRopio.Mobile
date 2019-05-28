using System;
using System.Threading.Tasks;
using AppRopio.Base.API;
using AppRopio.Base.API.Services;
using AppRopio.Base.Core;
using AppRopio.ECommerce.HistoryOrders.API.Services;
using AppRopio.ECommerce.HistoryOrders.API.Services.Implementation;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.HistoryOrders.API
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
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
                Mvx.RegisterType<IHistoryOrdersService>(() => new AppRopio.ECommerce.HistoryOrders.API.Services.Fakes.HistoryOrdersFakeService());
            else
                Mvx.RegisterType<IHistoryOrdersService>(() => new HistoryOrdersService(ConnectionService));
        }
    }
}
