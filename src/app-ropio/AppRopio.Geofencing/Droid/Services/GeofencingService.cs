using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using AppRopio.Geofencing.Core.Service;
using MvvmCross;

namespace AppRopio.Geofencing.Droid.Services
{
    public class GeofencingService : IGeofencingService
    {
        #region IGeofencingService implementation

        public async Task<bool> Start()
        {
            Application.Context.StartService(new Intent(Application.Context, typeof(GeofencingIntentService)));
            return true;
        }

        public async Task Stop()
        {
            Application.Context.StopService(new Intent(Application.Context, typeof(GeofencingIntentService)));
        }

        #endregion
    }
}
