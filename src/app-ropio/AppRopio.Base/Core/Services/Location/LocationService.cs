using System;
using System.Threading.Tasks;
using AppRopio.Models.Base.Responses;
using MvvmCross;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Platform;
using AppRopio.Base.Core.Services.Permissions;
using Plugin.Permissions.Abstractions;

namespace AppRopio.Base.Core.Services.Location
{
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
            Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(async () =>
            {
                Coordinates location = default(Coordinates);

                var hasPermission = await Mvx.Resolve<IPermissionsService>().CheckPermission(Permission.LocationWhenInUse);
                if (hasPermission)
                {
                    Plugin.Geolocator.Abstractions.Position position = null;
                    try
                    {
                        var locator = Plugin.Geolocator.CrossGeolocator.Current;
                        locator.DesiredAccuracy = 100;

                        position = await locator.GetLastKnownLocationAsync();

                        if (position == null && locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
                            position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
                    }
                    catch (Exception ex)
                    {
                        MvxTrace.TaggedTrace(MvxTraceLevel.Error, nameof(LocationService), () => ex.BuildAllMessagesAndStackTrace());
                    }

                    if (position != null)
                    {
                        var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                            position.Timestamp, position.Latitude, position.Longitude,
                            position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

                        MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, nameof(LocationService), () => output);

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
