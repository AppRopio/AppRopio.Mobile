using System;
using AppRopio.Base.API;
using AppRopio.Base.API.Services;
using AppRopio.Base.Core;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Beacons.Core
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
                Mvx.RegisterSingleton<API.Services.IBeaconsService>(new API.Services.Fakes.BeaconsService(ConnectionService));
            else
                Mvx.RegisterSingleton<API.Services.IBeaconsService>(new API.Services.Implementation.BeaconsService(ConnectionService));
        }
    }
}