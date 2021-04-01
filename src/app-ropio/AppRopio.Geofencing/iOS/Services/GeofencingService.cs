using System;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Geofencing.Core.Service;
using AppRopio.Geofencing.Core.Service.Implementation;
using AppRopio.Geofencing.iOS.Providers;
using AppRopio.Geofencing.iOS.Utils;
using MvvmCross;
using MvvmCross.Logging;

namespace AppRopio.Geofencing.iOS.Services
{
    public class GeofencingService : IGeofencingService
    {
		#region Fields

		private const double _updateRadius = 5000;

		#endregion

        #region Protected

        protected virtual async void OnRegionEntered(object sender, string id)
        {
            try
            {
                await AreaService.Instance.ActivateRegionBy(id);
            }
            catch (Exception ex)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Error($"Geofencing: {ex.BuildAllMessagesAndStackTrace()}");
            }
        }

		protected virtual async void OnUpdateRegionLeft(object sender, EventArgs e)
        {
            await SetupRegions();
        }

        protected virtual async Task SetupRegions()
        {
            var location = await GeofencingUtil.GetCurrentLocationAsync();

            try
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Info("Geofencing: Setup regions");

                var response = await AreaService.Instance.LoadAreasByUserLocation(location.Coordinate.Latitude, location.Coordinate.Longitude);

                var geofencingRegions = response.Select(area => GeofencingProvider.CreateRegion(area.Latitude, area.Longitude, area.Radius, area.ID))
                                                .ToList();
				
				var updateRegion = GeofencingProvider.CreateUpdateRegion(location.Coordinate.Latitude, location.Coordinate.Longitude, _updateRadius);

                GeofencingProvider.SetRegions(geofencingRegions, updateRegion);
            }
            catch (Exception ex)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Error($"Geofencing: {ex.BuildAllMessagesAndStackTrace()}");
            }
        }

        #endregion

        #region IGeofencingService implementation

        public Task<bool> Start()
        {
            Mvx.IoCProvider.Resolve<IMvxLog>().Info("Geofencing: Start");

			var tcs = new TaskCompletionSource<bool>();
			bool result = false;

			UIKit.UIApplication.SharedApplication.InvokeOnMainThread(async () =>
			{
				result = await GeofencingUtil.RequestPermissionAsync();

				if (result)
				{
					GeofencingProvider.UpdateRegionLeft += OnUpdateRegionLeft;
					GeofencingProvider.RegionEntered += OnRegionEntered;
					GeofencingProvider.InitGeoFencing();

					await SetupRegions();
				}

				tcs.TrySetResult(result);
			});

			return tcs.Task;
        }

        public async Task Stop()
        {
            Mvx.IoCProvider.Resolve<IMvxLog>().Info("Geofencing: Stop");

            var result = await GeofencingUtil.RequestPermissionAsync();

            if (result)
            {
                GeofencingProvider.UpdateRegionLeft -= OnUpdateRegionLeft;
                GeofencingProvider.RegionEntered -= OnRegionEntered;

                GeofencingProvider.SetRegions(new System.Collections.Generic.List<CoreLocation.CLCircularRegion>());
            }
        }

        #endregion
    }
}
