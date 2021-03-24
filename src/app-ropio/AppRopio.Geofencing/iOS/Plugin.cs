using System;
using MvvmCross.Plugin;
using AppRopio.Geofencing.Core.Service;
using MvvmCross;
using AppRopio.Geofencing.iOS.Services;
using MvvmCross.Platform.Platform;

namespace AppRopio.Geofencing.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IGeofencingService>(() => new GeofencingService());

            try
            {
                Mvx.Resolve<IGeofencingService>().Start();
            }
            catch (Exception ex)
            {
                MvxTrace.TaggedTrace("Geofencing", ex.BuildAllMessagesAndStackTrace());
            }
        }
    }
}
