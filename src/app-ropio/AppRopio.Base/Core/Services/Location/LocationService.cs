using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.Services.Permissions;
using AppRopio.Models.Base.Responses;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Logging;

using Xamarin.Essentials;

namespace AppRopio.Base.Core.Services.Location {
	public class LocationService : ILocationService
    {
        public LocationService()
        {
            CurrentOrLastLocation = new Coordinates();
        }

        #region Implementation ILocationService

        public Coordinates CurrentOrLastLocation { get; private set; }

        public Task<Coordinates> GetCurrentLocation()
        {
            var tcs = new TaskCompletionSource<Coordinates>();

            //плагин геолокации работает только в главном потоке
            Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(async () =>
            {
                Coordinates location = default(Coordinates);

                var hasPermission = await Mvx.IoCProvider.Resolve<IPermissionsService>().CheckPermission(new Xamarin.Essentials.Permissions.LocationWhenInUse());
                if (hasPermission)
                {
                    Xamarin.Essentials.Location position = null;
                    try
                    {
                        var request = new GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.High, TimeSpan.FromSeconds(20));

                        position = await Geolocation.GetLastKnownLocationAsync();

                        if (position == null)
                            position = await Geolocation.GetLocationAsync(request);
                    }
                    catch (Exception ex)
                    {
                        Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{nameof(LocationService)}: {ex.BuildAllMessagesAndStackTrace()}");
                    }

                    if (position != null)
                    {
                        var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAccuracy: {4} \nCourse: {5} \nSpeed: {6}",
                            position.Timestamp, position.Latitude, position.Longitude,
                            position.Altitude, position.Accuracy, position.Course, position.Speed);

                        Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{nameof(LocationService)}: {output}");

                        location = (CurrentOrLastLocation = new Coordinates { Latitude = position.Latitude, Longitude = position.Longitude });
                    }
                }

                tcs.TrySetResult(location);
            });

            return tcs.Task;
        }

        #endregion
    }
}
