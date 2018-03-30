using System;
using System.Linq;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using MvvmCross.Platform.Platform;

namespace AppRopio.Beacons.iOS.Util
{
    public class BeaconsUtil
    {
        private static CLLocationManager _locationManager;

        public static CLLocation LastKnownLocation { get; private set; }

        public static CLLocationManager LocationManager
        {
            get { return _locationManager; }
        }

        static BeaconsUtil()
        {
            _locationManager = new CLLocationManager();
            LastKnownLocation = new CLLocation();
        }

        public static Task<bool> RequestPermissionAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            EventHandler<CLAuthorizationChangedEventArgs> handler = null;
            handler = (object sender, CLAuthorizationChangedEventArgs e) =>
            {
                MvxTrace.Trace("CLAuthorizarion status changed: " + e.Status);

                if (e.Status == CLAuthorizationStatus.AuthorizedAlways)
                {
                    _locationManager.AuthorizationChanged -= handler;
                    tcs.TrySetResult(true);
                }
                else if (e.Status != CLAuthorizationStatus.NotDetermined)
                {
                    _locationManager.AuthorizationChanged -= handler;
                    tcs.TrySetResult(false);
                }
            };

            _locationManager.AuthorizationChanged += handler;

            _locationManager.RequestAlwaysAuthorization();
            return tcs.Task;
        }

        public static Task<CLLocation> GetCurrentLocationAsync()
        {
            var tcs = new TaskCompletionSource<CLLocation>();

            EventHandler<CLLocationsUpdatedEventArgs> handler = null;
            EventHandler<NSErrorEventArgs> failedHandler = null;

            handler = (object sender, CLLocationsUpdatedEventArgs e) =>
            {
                MvxTrace.Trace("UpdatedLocations: " + e.Locations.FirstOrDefault());

                _locationManager.LocationsUpdated -= handler;
                _locationManager.Failed -= failedHandler;

                LastKnownLocation = e.Locations.FirstOrDefault() ?? new CLLocation();

                tcs.TrySetResult(LastKnownLocation);
            };

            failedHandler = (object sender, NSErrorEventArgs e) =>
            {
                MvxTrace.Trace("Failed to get location: " + e.Error);

                _locationManager.LocationsUpdated -= handler;
                _locationManager.Failed -= failedHandler;

                tcs.TrySetResult(new CLLocation());
            };

            _locationManager.DesiredAccuracy = CLLocation.AccuracyHundredMeters;
            _locationManager.LocationsUpdated += handler;
            _locationManager.Failed += failedHandler;
            _locationManager.RequestLocation();

            return tcs.Task;
        }
    }
}
