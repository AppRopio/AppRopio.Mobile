using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using UIKit;

namespace AppRopio.Geofencing.iOS.Utils
{
    public class GeofencingUtil
    {
        private static CLLocationManager _locationManager;

        public static CLLocation LastKnownLocation { get; private set; }

        public static CLLocationManager LocationManager
        {
            get { return _locationManager; }
        }

        static GeofencingUtil()
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
				Debug.WriteLine("CLAuthorizarion status changed: " + e.Status);

				if (e.Status == CLAuthorizationStatus.AuthorizedAlways)
				{
					_locationManager.AuthorizationChanged -= handler;
					tcs.TrySetResult(true);
				}
				else if (e.Status == CLAuthorizationStatus.Denied || e.Status == CLAuthorizationStatus.Restricted)
				{
					_locationManager.AuthorizationChanged -= handler;
					tcs.TrySetResult(false);
				}
			};

			if (CLLocationManager.Status == CLAuthorizationStatus.NotDetermined)
			{
				_locationManager.AuthorizationChanged += handler;
				UIApplication.SharedApplication.InvokeOnMainThread(() =>
				{
					_locationManager.RequestAlwaysAuthorization();
				});
			}
			else
			{
				if (CLLocationManager.Status == CLAuthorizationStatus.AuthorizedAlways)
					//геофенсинг не работает когда "AuthorizedWhenInUse"
					//|| CLLocationManager.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
					tcs.TrySetResult(true);
				else
					tcs.TrySetResult(false);
			}

			return tcs.Task;
        }

        public static Task<CLLocation> GetCurrentLocationAsync()
        {
            var tcs = new TaskCompletionSource<CLLocation>();

			EventHandler<CLLocationsUpdatedEventArgs> handler = null;
			EventHandler<NSErrorEventArgs> failedHandler = null;

			handler = (object sender, CLLocationsUpdatedEventArgs e) =>
			{
				Debug.WriteLine("UpdatedLocations: " + e.Locations.FirstOrDefault());

				_locationManager.LocationsUpdated -= handler;
				_locationManager.Failed -= failedHandler;

				LastKnownLocation = e.Locations.FirstOrDefault() ?? new CLLocation();

				tcs.TrySetResult(LastKnownLocation);
			};

			failedHandler = (object sender, NSErrorEventArgs e) =>
			{
				//если доступ к локации запрещен, попадем сюда
				Debug.WriteLine("Failed to get location: " + e.Error);

				_locationManager.LocationsUpdated -= handler;
				_locationManager.Failed -= failedHandler;

				tcs.TrySetResult(new CLLocation());
			};
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				_locationManager.DistanceFilter = 0;
				_locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
				_locationManager.LocationsUpdated += handler;
				_locationManager.Failed += failedHandler;

				_locationManager.RequestLocation();

			});

			if (_locationManager.Location != null)
				tcs.TrySetResult(_locationManager.Location);

			return tcs.Task;
        }
    }
}
