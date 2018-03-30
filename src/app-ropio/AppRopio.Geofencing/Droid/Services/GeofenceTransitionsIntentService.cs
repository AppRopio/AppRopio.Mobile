using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.Support.V4.App;
using MvvmCross.Platform.Platform;
using Android.Support.V4.Content;
using AppRopio.Geofencing.Core.Service.Implementation;

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
            MvxTrace.TaggedTrace(TAG, "GeofenceTransitionsIntentService started");
        }

        protected override void OnHandleIntent(Intent intent)
        {
            if (intent == null)
                return;

            MvxTrace.TaggedTrace(TAG, "GeofenceTransitionsIntentService handle intent");

            var geofencingEvent = GeofencingEvent.FromIntent(intent);
            if (geofencingEvent.HasError)
            {
                var errorMessage = GeofenceErrorMessages.GetErrorString(this, geofencingEvent.ErrorCode);
                MvxTrace.TaggedError(TAG, errorMessage);
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
                MvxTrace.TaggedError(TAG, GetString(Resource.String.geofence_transition_invalid_type, new[] { new Java.Lang.Integer(geofenceTransition) }));
            }
        }

        private async void LoadOffersInGeofences(string[] geofencesIds)
        {
            if (geofencesIds == null || !geofencesIds.Any())
            {
                MvxTrace.TaggedError(TAG, string.Format("geofencesIds is null or empty"));
                return;
            }

            try
            {
                foreach (var id in geofencesIds)
                    await AreaService.Instance.ActivateRegionBy(id);

                MvxTrace.TaggedTrace(TAG, string.Format("Regions activated"));
            }
            catch (Exception ex)
            {
                MvxTrace.TaggedError(TAG, ex.BuildAllMessagesAndStackTrace());
            }
        }

        private string[] GetGeofenceTransitionDetails(Context context, int geofenceTransition, IList<IGeofence> triggeringGeofences)
        {
            string geofenceTransitionString = GetTransitionString(geofenceTransition);

            var triggeringGeofencesIdsList = triggeringGeofences.Select(x => x.RequestId).ToArray();

            MvxTrace.TaggedTrace(TAG, geofenceTransitionString + ": " + string.Join(", ", triggeringGeofencesIdsList));

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
