using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Base.Core;
using AppRopio.Models.Geofencing.Responses;
using MvvmCross;

namespace AppRopio.Geofencing.Core.Service.Implementation
{
    public class AreaService : IAreaService
    {
        #region Properties

        private static API.Services.IAreaService _apiService;
        protected API.Services.IAreaService ApiService
        {
            get
            {
                return Mvx.IoCProvider.CanResolve<API.Services.IAreaService>() ? (_apiService = Mvx.IoCProvider.Resolve<API.Services.IAreaService>()) : (_apiService = new API.Services.Implementation.AreaService(ConnectionService));
            }
        }

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

        private static IAreaService _instance;
        public static IAreaService Instance
        {
            get
            {
                return Mvx.IoCProvider.CanResolve<IAreaService>() ? (_instance = Mvx.IoCProvider.Resolve<IAreaService>()) : (_instance = new AreaService());
            }
        }

        #endregion

        #region IAreaService implementation

        public Task ActivateRegionBy(string id)
        {
            return ApiService.RegionActivated(id);
        }

        public Task<List<GeofencingModel>> LoadAreasByUserLocation(double latitude, double longitude)
        {
            return ApiService.LoadNearAreas(latitude, longitude);
        }

        #endregion
    }
}
