using System;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Base.Core;
using AppRopio.Models.Beacons.Responses;
using MvvmCross;

namespace AppRopio.Beacons.Core.Services.Implementation
{
    public class BeaconsService : IBeaconsService
    {
        #region Properties

        private static API.Services.IBeaconsService _apiService;
        protected API.Services.IBeaconsService ApiService
        {
            get
            {
                return Mvx.CanResolve<API.Services.IBeaconsService>() ? (_apiService = Mvx.Resolve<API.Services.IBeaconsService>()) : (_apiService = new API.Services.Implementation.BeaconsService(ConnectionService));
            }
        }

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

        private static IBeaconsService _instance;
        public static IBeaconsService Instance
        {
            get
            {
                return Mvx.CanResolve<IBeaconsService>() ? (_instance = Mvx.Resolve<IBeaconsService>()) : (_instance = new BeaconsService());
            }
        }

        #endregion

        #region IBeaconsService implementation

        public Task ActivateEddystoneUid(string uid)
        {
            return ApiService.ActivateEddystoneUid(uid);
        }

        public Task ActivateEddystoneUrl(string url)
        {
            return ApiService.ActivateEddystoneUrl(url);
        }

        public Task ActivateiBeacon(string uuid, int major, int minor)
        {
            return ApiService.ActivateiBeacon(uuid, major, minor);
        }

        public Task<System.Collections.Generic.List<BeaconModel>> LoadBeaconsByUserLocation(double latitude, double longitude)
        {
            return ApiService.LoadBeacons(latitude, longitude);
        }

        #endregion
    }
}
