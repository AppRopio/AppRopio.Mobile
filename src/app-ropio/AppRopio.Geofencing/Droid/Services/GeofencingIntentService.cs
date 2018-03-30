using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;
using Java.Lang;
using MvvmCross.Platform.Platform;
using AppRopio.Geofencing.Core.Service.Implementation;
using AppRopio.Models.Geofencing.Responses;

[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
[assembly: UsesPermission(Name = "android.permission.GET_TASKS")]
[assembly: UsesPermission(Name = "android.permission.RECEIVE_BOOT_COMPLETED")]
[assembly: UsesPermission(Name = "android.permission.ACCESS_FINE_LOCATION")]

namespace AppRopio.Geofencing.Droid.Services
{
    [Service(Process = "com.notissimus.geofences")]
    [IntentFilter(new[] { Intent.ActionBootCompleted, Intent.ActionPowerConnected, Intent.ActionPowerDisconnected, Intent.ActionScreenOn, Intent.ActionScreenOff }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class GeofencingIntentService : Service
        , GoogleApiClient.IConnectionCallbacks
        , GoogleApiClient.IOnConnectionFailedListener
        , ILocationListener
    {
        private const int GEOFENCE_EXPIRATION_IN_HOURS = 12;
        private const long GEOFENCE_EXPIRATION_IN_MILLISECONDS = GEOFENCE_EXPIRATION_IN_HOURS * 60 * 60 * 1000;

        /// <summary>
        /// Средний радиус зон
        /// </summary>
        private const int GEOFENCE_RADIUS_IN_METERS = 300;

        private const float GEOFENCE_LOITERING_IN_SECONDS = (GEOFENCE_RADIUS_IN_METERS / 4) / (HUMAN_SPEED_IN_KmH * 1000f / 3600f);
        private const int GEOFENCE_LOITERING_IN_MILLISECONDS = (int)(GEOFENCE_LOITERING_IN_SECONDS * 1000);

        private const int LOCATION_UPDATES_IN_MINUTES = 5;
        private const long LOCATION_UPDATES_IN_MILLISECONDS = LOCATION_UPDATES_IN_MINUTES * 60 * 1000;

        /// <summary>
        /// Средняя скорость ходьбы человека в км/ч
        /// </summary>
        private const int HUMAN_SPEED_IN_KmH = 5;

        /// <summary>
        /// Расстояние в метрах, которое человек может пройти за <see cref="LOCATION_UPDATES_IN_MINUTES"/> при средней скорости <see cref="HUMAN_SPEED_IN_KmH"/>
        /// </summary>
        private const long LOCATION_UPDATES_IN_METERS = (HUMAN_SPEED_IN_KmH * 1000 / 3600) * (LOCATION_UPDATES_IN_MINUTES * 60);

        private const string TAG = "Geofencing";

        private List<GeofencingModel> _regionsModels;

        private GoogleApiClient _googleApiClient;
        private IList<IGeofence> _geofenceList = new List<IGeofence>();
        private PendingIntent _geofencingPendingIntent;

        public override void OnCreate()
        {
            base.OnCreate();

            BuildGoogleApiClient();
            MvxTrace.TaggedTrace(TAG, "Created");
        }

        public override ComponentName StartService(Intent service)
        {
            Intent restartServiceIntent = new Intent(Application.Context, this.Class);
            restartServiceIntent.SetPackage(Application.Context.PackageName);

            PendingIntent restartServicePendingIntent = PendingIntent.GetService(Application.Context, 3, restartServiceIntent, PendingIntentFlags.OneShot);
            AlarmManager alarmService = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
            alarmService.Set(
                AlarmType.RtcWakeup,
                SystemClock.CurrentThreadTimeMillis() + 1000,
                restartServicePendingIntent);

            MvxTrace.TaggedTrace(TAG, "StartService");

            return base.StartService(service);
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.RedeliverIntent;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            var restartServiceIntent = new Intent(Application.Context, this.Class);
            restartServiceIntent.SetPackage(Application.Context.PackageName);

            var restartServicePendingIntent = PendingIntent.GetService(Application.Context, 3, restartServiceIntent, PendingIntentFlags.OneShot);
            var alarmService = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
            alarmService.Set(
                AlarmType.RtcWakeup,
                SystemClock.CurrentThreadTimeMillis() + 1000,
                restartServicePendingIntent);

            MvxTrace.TaggedTrace(TAG, "Start after removed");
        }

        public override void OnDestroy()
        {
            var restartServiceIntent = new Intent(Application.Context, this.Class);
            restartServiceIntent.SetPackage(Application.Context.PackageName);

            var restartServicePendingIntent = PendingIntent.GetService(Application.Context, 3, restartServiceIntent, PendingIntentFlags.OneShot);
            var alarmService = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
            alarmService.Set(
                AlarmType.RtcWakeup,
                SystemClock.CurrentThreadTimeMillis() + 1000,
                restartServicePendingIntent);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        #region GoogleApiClient.IConnectionCallbacks implementation

        public void OnConnected(Bundle connectionHint)
        {
            LocationServices.FusedLocationApi.RequestLocationUpdates(_googleApiClient, GetLocationRequest(), this);

            MvxTrace.TaggedTrace(TAG, "GoogleApiClient connected");
        }

        public void OnConnectionSuspended(int cause)
        {
            MvxTrace.TaggedTrace(TAG, string.Format("GoogleApiClient OnConnectionSuspended {0}", cause));
        }

        #endregion

        #region GoogleApiClient.IOnConnectionFailedListener implementation

        public void OnConnectionFailed(global::Android.Gms.Common.ConnectionResult result)
        {
            MvxTrace.TaggedTrace(TAG, string.Format("GoogleApiClient OnConnectionFailed {0} {1}", result.ErrorCode, result.ErrorMessage));
        }

        #endregion

        #region ILocationListener implementation

        public void OnLocationChanged(global::Android.Locations.Location location)
        {
            if (_googleApiClient == null || !_googleApiClient.IsConnected)
                return;
            
            UpdateRegions(location);

            MvxTrace.TaggedTrace(TAG, string.Format("OnLocationChanged Lat {0} : Long {1} – Accuracy {2} : Speed {3}", location.Latitude, location.Longitude, location.Accuracy, location.Speed));
        }

        #endregion

        #region Methods

        private void DestroyService()
        {
            if (_googleApiClient != null && _googleApiClient.IsConnected)
            {
                LocationServices.FusedLocationApi.RemoveLocationUpdates(_googleApiClient, this);
                _googleApiClient?.Disconnect();
            }

            _geofenceList = new List<IGeofence>();

            MvxTrace.TaggedTrace(TAG, "Service been destroyed");
        }

        private async void UpdateRegions(global::Android.Locations.Location location)
        {
            if (_googleApiClient == null || !_googleApiClient.IsConnected)
                return;

            try
            {
                _regionsModels = await AreaService.Instance.LoadAreasByUserLocation(location.Latitude, location.Longitude);

                PopulateGeofenceList();

                RemoveGeofencesHandler();
                AddGeofencesHandler();

                MvxTrace.TaggedTrace(TAG, string.Format("Regions loaded {0}", _regionsModels.Count));
            }
            catch (System.Exception ex)
            {
                MvxTrace.TaggedError(TAG, string.Format("Error when update regions {0}", ex.InnerException?.Message));
            }
        }

        private void PopulateGeofenceList()
        {
            foreach (var entry in _regionsModels)
            {
                _geofenceList.Add(
                    new GeofenceBuilder()
                    .SetRequestId(entry.ID)
                    .SetCircularRegion(entry.Latitude, entry.Longitude, entry.Radius)
                    .SetExpirationDuration(GEOFENCE_EXPIRATION_IN_MILLISECONDS) //Geofence.NeverExpire)
                    .SetLoiteringDelay(GEOFENCE_LOITERING_IN_MILLISECONDS)
                    .SetTransitionTypes(Geofence.GeofenceTransitionEnter | Geofence.GeofenceTransitionExit | Geofence.GeofenceTransitionDwell)
                    .Build()
                );
            }

            MvxTrace.TaggedTrace(TAG, string.Format("GeofenceList Populated – Count {0}", _geofenceList.Count));
        }

        private void BuildGoogleApiClient()
        {
            if (_googleApiClient != null)
                return;

            _googleApiClient = new GoogleApiClient.Builder(Application.Context)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(LocationServices.API)
                .Build();

            if (!_googleApiClient.IsConnected)
                _googleApiClient.Connect();
        }

        private async void AddGeofencesHandler()
        {
            if (_googleApiClient == null || !_googleApiClient.IsConnected)
                return;

            try
            {
                var status = await LocationServices.GeofencingApi.AddGeofencesAsync(_googleApiClient, GetGeofencingRequest(), GetGeofencePendingIntent());
                HandleResult(status, "Geofences Added");
            }
            catch (SecurityException securityException)
            {
                LogSecurityException(securityException);
            }
        }

        private async void RemoveGeofencesHandler()
        {
            if (_googleApiClient == null || !_googleApiClient.IsConnected)
                return;

            try
            {
                var status = await LocationServices.GeofencingApi.RemoveGeofencesAsync(_googleApiClient, GetGeofencePendingIntent());
                HandleResult(status, "Geofences Removed");
            }
            catch (SecurityException securityException)
            {
                LogSecurityException(securityException);
            }
        }

        private void HandleResult(Statuses status, string message)
        {
            if (status.IsSuccess)
                MvxTrace.TaggedTrace(TAG, message);
            else
            {
                var errorMessage = GeofenceErrorMessages.GetErrorString(Application.Context, status.StatusCode);
                MvxTrace.TaggedError(TAG, errorMessage);
            }
        }

        private static LocationRequest GetLocationRequest()
        {
            var request = LocationRequest.Create();

            request.SetPriority(LocationRequest.PriorityBalancedPowerAccuracy);
            request.SetInterval((long)LOCATION_UPDATES_IN_MILLISECONDS);
            request.SetFastestInterval((long)LOCATION_UPDATES_IN_MILLISECONDS);
            request.SetSmallestDisplacement(LOCATION_UPDATES_IN_METERS);

            return request;
        }

        private PendingIntent GetGeofencePendingIntent()
        {
            if (_geofencingPendingIntent != null)
                return _geofencingPendingIntent;

            var intent = new Intent(Application.Context, typeof(GeofenceTransitionsIntentService));
            return (_geofencingPendingIntent = PendingIntent.GetService(Application.Context, 0, intent, PendingIntentFlags.UpdateCurrent));
        }

        private GeofencingRequest GetGeofencingRequest()
        {
            var builder = new GeofencingRequest.Builder();
            builder.SetInitialTrigger(GeofencingRequest.InitialTriggerEnter | GeofencingRequest.InitialTriggerDwell);
            builder.AddGeofences(_geofenceList);

            return builder.Build();
        }

        private void LogSecurityException(SecurityException securityException)
        {
            MvxTrace.TaggedError(TAG, "Invalid location permission. " +
                "You need to use ACCESS_FINE_LOCATION with geofences", securityException);
        }

        #endregion

        [BroadcastReceiver(Enabled = true, Exported = true)]
        [IntentFilter(new[] { Intent.ActionBootCompleted, Intent.ActionPowerConnected, Intent.ActionPowerDisconnected, Intent.ActionScreenOn, Intent.ActionScreenOff }, Priority = (int)IntentFilterPriority.HighPriority)]
        public class GeofenceBootUpReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                var restartServiceIntent = new Intent(Application.Context, typeof(GeofencingIntentService));
                restartServiceIntent.SetPackage(Application.Context.PackageName);

                PendingIntent restartServicePendingIntent = PendingIntent.GetService(Application.Context, 3, restartServiceIntent, PendingIntentFlags.OneShot);
                AlarmManager alarmService = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
                alarmService.Set(
                    AlarmType.RtcWakeup,
                    SystemClock.CurrentThreadTimeMillis() + 1000,
                    restartServicePendingIntent);
                
                MvxTrace.TaggedTrace(TAG, "Start after boot completed");
            }
        }
    }
}
