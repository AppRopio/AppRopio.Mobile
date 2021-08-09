using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using AppRopio.Geofencing.Core.Service.Implementation;
using MvvmCross;
using MvvmCross.Logging;

namespace AppRopio.Geofencing.Droid.Services
{
    [Service]
    public class GeofenceTransitionsIntentService : IntentService
    {
        protected const string TAG = "Geofencing";

        private int notify_number = 0;

        public GeofenceTransitionsIntentService()
            : base(TAG)
        {
            Mvx.IoCProvider.Resolve<IMvxLog>().Trace($"{TAG}: GeofenceTransitionsIntentService started");
        }

        protected override void OnHandleIntent(Intent intent)
        {
            if (intent == null)
                return;

            Mvx.IoCProvider.Resolve<IMvxLog>().Trace($"{TAG}: GeofenceTransitionsIntentService handle intent");

            var geofencingEvent = GeofencingEvent.FromIntent(intent);
            if (geofencingEvent.HasError)
            {
                var errorMessage = GeofenceErrorMessages.GetErrorString(this, geofencingEvent.ErrorCode);
                Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{TAG}: {errorMessage}");
                return;
            }

            int geofenceTransition = geofencingEvent.GeofenceTransition;

            if (geofenceTransition != Geofence.NeverExpire)
            {
                IList<IGeofence> triggeringGeofences = geofencingEvent.TriggeringGeofences;

                var geofencesIds = GetGeofenceTransitionDetails(this, geofenceTransition, triggeringGeofences);
                LoadOffersInGeofences(geofencesIds);
            }
            else
            {
                // Log the error.
                Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{TAG}: " + GetString(Resource.String.geofence_transition_invalid_type, new[] { new Java.Lang.Integer(geofenceTransition) }));
            }
        }

        private async void LoadOffersInGeofences(string[] geofencesIds)
        {
            if (geofencesIds == null || !geofencesIds.Any())
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{TAG}: geofencesIds is null or empty");
                return;
            }

            try
            {
                foreach (var id in geofencesIds)
                    await AreaService.Instance.ActivateRegionBy(id);

                Mvx.IoCProvider.Resolve<IMvxLog>().Trace($"{TAG}: Regions activated");
            }
            catch (Exception ex)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{TAG}: {ex.BuildAllMessagesAndStackTrace()}");
            }
        }

        private string[] GetGeofenceTransitionDetails(Context context, int geofenceTransition, IList<IGeofence> triggeringGeofences)
        {
            string geofenceTransitionString = GetTransitionString(geofenceTransition);

            var triggeringGeofencesIdsList = triggeringGeofences.Select(x => x.RequestId).ToArray();

            Mvx.IoCProvider.Resolve<IMvxLog>().Trace($"{TAG}: " + geofenceTransitionString + ": " + string.Join(", ", triggeringGeofencesIdsList));

            return triggeringGeofencesIdsList;
        }

        private string GetTransitionString(int transitionType)
        {
            switch (transitionType)
            {
                case Geofence.GeofenceTransitionEnter:
                    return GetString(Resource.String.geofence_transition_entered);
                case Geofence.GeofenceTransitionExit:
                    return GetString(Resource.String.geofence_transition_exited);
                case Geofence.GeofenceTransitionDwell:
                    return GetString(Resource.String.geofence_transition_dwelled);
                default:
                    return GetString(Resource.String.unknown_geofence_transition);
            }
        }
    }
}
