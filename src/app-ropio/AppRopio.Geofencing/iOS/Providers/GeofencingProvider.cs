using System;
using System.Collections.Generic;
using System.Linq;
using AppRopio.Base.Core.Services.Device;
using AppRopio.Geofencing.iOS.Utils;
using CoreLocation;
using MvvmCross;
using MvvmCross.Logging;

namespace AppRopio.Geofencing.iOS.Providers
{
    public class GeofencingProvider
	{
		private static string GEO_REGION_HEADER = "geo_";
		private static string USER_REGION_HEADER = "userGeo_";

		private static CLLocationManager _locationManager;
		private static bool _isStarted;

		//Покинули границы региона - необходимо загрузить точки
		public static event EventHandler UpdateRegionLeft;

		public static event EventHandler<string> RegionEntered;

		static GeofencingProvider()
		{
			_locationManager = GeofencingUtil.LocationManager;
			GEO_REGION_HEADER = $"geo_{Mvx.IoCProvider.Resolve<IDeviceService>().PackageName}.";
			USER_REGION_HEADER = $"userGeo_{Mvx.IoCProvider.Resolve<IDeviceService>().PackageName}";
		}

		public static void InitGeoFencing()
		{
			if (_isStarted)
				return;

			_isStarted = true;

			_locationManager.RegionEntered += LocationManager_RegionEntered;
			_locationManager.RegionLeft += LocationManager_RegionLeft;
		}

		public static CLCircularRegion CreateRegion(double latitude, double longitude, double radius, string id)
		{
			return new CLCircularRegion(new CLLocationCoordinate2D(latitude, longitude), radius, GEO_REGION_HEADER + id);
		}

		public static CLCircularRegion CreateUpdateRegion(double latitude, double longitude, double radius)
		{
			return new CLCircularRegion(new CLLocationCoordinate2D(latitude, longitude), radius, USER_REGION_HEADER) { NotifyOnExit = true };
		}

		public static void SetRegions(List<CLCircularRegion> regions, CLCircularRegion updateRegion = null)
		{
			//убираем предыдущие регионы
			foreach (var region in _locationManager.MonitoredRegions.OfType<CLCircularRegion>())
			{
				_locationManager.StopMonitoring(region);
			}

			if (regions != null && regions.Count > 0)
			{
				foreach (var region in regions)
				{
					_locationManager.StartMonitoring(region);
				}

				//задаем регион при выходи за границы которого обновляются точки
				if (updateRegion != null)
					_locationManager.StartMonitoring(updateRegion);
			}
		}


		private static void LocationManager_RegionEntered(object sender, CLRegionEventArgs e)
		{
			Mvx.IoCProvider.Resolve<IMvxLog>().Trace("Geofencing: Entered region " + e.Region.Identifier);

			if (e.Region.Identifier == USER_REGION_HEADER)
				return;

			if (!e.Region.Identifier.StartsWith(GEO_REGION_HEADER))
				return;

			string id = e.Region.Identifier.Substring(GEO_REGION_HEADER.Length);

			RegionEntered?.Invoke(null, id);
		}

		private static void LocationManager_RegionLeft(object sender, CLRegionEventArgs e)
		{
			Mvx.IoCProvider.Resolve<IMvxLog>().Trace("Geofencing: Left region " + e.Region.Identifier);

			if (e.Region.Identifier == USER_REGION_HEADER)
			{
				UpdateRegionLeft?.Invoke(null, EventArgs.Empty);
				return;
			}

			if (!e.Region.Identifier.StartsWith(GEO_REGION_HEADER))
				return; //TODO On Region Lefted
		}
	}
}
