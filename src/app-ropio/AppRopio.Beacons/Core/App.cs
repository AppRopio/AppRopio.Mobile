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
                return Mvx.IoCProvider.CanResolve<IConnectionService>() ?
                          (_connectionService = Mvx.IoCProvider.Resolve<IConnectionService>())
                              :
                          (_connectionService = new ConnectionService
                          {
                              ErrorWhenConnectionFailed = AppSettings.ErrorWhenConnectionFailed,
                              ErrorWhenTaskCanceled = AppSettings.ErrorWhenTaskCanceled,
                              //IsConnectionAvailable = () => Task<bool>.Factory.StartNew(() => Mvx.IoCProvider.Resolve<IMvxReachability>().IsHostReachable(AppSettings.Host)),
                              RequestTimeoutInSeconds = AppSettings.RequestTimeoutInSeconds,
                              BaseUrl = new Uri(AppSettings.Host)
                          });
            }
        }

        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.IoCProvider.RegisterSingleton<API.Services.IBeaconsService>(new API.Services.Fakes.BeaconsService(ConnectionService));
            else
                Mvx.IoCProvider.RegisterSingleton<API.Services.IBeaconsService>(new API.Services.Implementation.BeaconsService(ConnectionService));
        }
    }
}