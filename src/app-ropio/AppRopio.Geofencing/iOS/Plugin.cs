using System;
using AppRopio.Geofencing.Core;
using AppRopio.Geofencing.Core.Service;
using AppRopio.Geofencing.iOS.Services;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin;

namespace AppRopio.Geofencing.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IGeofencingService>(() => new GeofencingService());

            try
            {
                Mvx.IoCProvider.Resolve<IGeofencingService>().Start();
            }
            catch (Exception ex)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Trace($"Geofencing: {ex.BuildAllMessagesAndStackTrace()}");
            }
        }
    }
}
