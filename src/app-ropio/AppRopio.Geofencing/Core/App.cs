using System;
using AppRopio.Base.API;
using AppRopio.Base.API.Services;
using AppRopio.Base.Core;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Geofencing.Core
{
    public class App : MvxApplication
    {
        private IConnectionService _connectionService;
        protected IConnectionService ConnectionService
        {
            get
            {
                return Mvx.CanResolve<IConnectionService>() ?
                          (_connectionService = Mvx.Resolve<IConnectionService>())
                              :
                          (_connectionService = new ConnectionService
                          {
                              ErrorWhenConnectionFailed = AppSettings.ErrorWhenConnectionFailed,
                              ErrorWhenTaskCanceled = AppSettings.ErrorWhenTaskCanceled,
                              //IsConnectionAvailable = () => Task<bool>.Factory.StartNew(() => Mvx.Resolve<IMvxReachability>().IsHostReachable(AppSettings.Host)),
                              RequestTimeoutInSeconds = AppSettings.RequestTimeoutInSeconds,
                              BaseUrl = new Uri(AppSettings.Host)
                          });
            }
        }

        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.RegisterSingleton<API.Services.IAreaService>(new API.Services.Fakes.AreaService(ConnectionService));
            else
                Mvx.RegisterSingleton<API.Services.IAreaService>(new API.Services.Implementation.AreaService(ConnectionService));
        }
    }
}
